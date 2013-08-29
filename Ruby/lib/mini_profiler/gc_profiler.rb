require 'objspace'

class Rack::MiniProfiler::GCProfiler

  ObjectStats = Struct.new(:objects, :bytes)

  def object_space_stats
    stats = {}
    ids = Set.new
    ObjectSpace.each_object { |o|
      begin
        stats[o.class] ||= ObjectStats.new(0,0)
        stats[o.class].objects += 1
        stats[o.class].bytes += ObjectSpace.memsize_of(o)
        # store object ids for string analysis:
        ids << o.object_id if Integer === o.object_id
      rescue NoMethodError
        # BasicObject derived classes (eg. Redis::Future)
        # don't have .class or .object_id (but .__id__ is the same)
      end
    }
    {:stats => stats, :ids => ids}
  end

  def diff_object_stats(before, after)
    diff = {}
    after.each do |klass,v|
      diff[klass] = v.dup
      if before.has_key?(klass)
        diff[klass].objects -= before[klass].objects
        diff[klass].bytes -= before[klass].bytes
      end
    end
    before.each do |klass,v|
      unless after[klass].objects
        diff[klass] = ObjectStats.new(0,0)
        diff[klass].objects = 0 - v.objects
        diff[klass].bytes = 0 - v.bytes
      end
    end

    diff
  end

  def analyze_strings(ids_before,ids_after)
    result = Hash.new(0)
    ids_after.each do |id|
      obj = ObjectSpace._id2ref(id)
      if String === obj && !ids_before.include?(obj.object_id)
        result[obj] += 1
      end
    end
    result
  end

  def profile_gc_time(app,env)
    body = []

    begin
      GC::Profiler.clear
      GC::Profiler.enable
      b = app.call(env)[2]
      b.close if b.respond_to? :close
      body << "GC Profiler ran during this request, if it fired you will see the cost below:\n\n"
      body << GC::Profiler.result
    ensure
      GC.enable
      GC::Profiler.disable
    end

    return [200, {'Content-Type' => 'text/plain'}, body]
  end

  def profile_gc(app,env)
    body = []

    stat_before,stat_after,diff,string_analysis = nil
    begin
      GC.disable
      stat_before = object_space_stats
      b = app.call(env)[2]
      b.close if b.respond_to? :close
      stat_after = object_space_stats

      diff = diff_object_stats(stat_before[:stats],stat_after[:stats])
      string_analysis = analyze_strings(stat_before[:ids], stat_after[:ids])
    ensure
      GC.enable
    end

    line = ('---' * 20) << "\n\n"

    body << "ObjectSpace delta caused by request:\n" << line
    diff.to_a.reject{|k,v| v.objects == 0}.sort{|x,y| y[1].bytes <=> x[1].bytes}.each do |k,v|
      body << output_object_space(k,v)
    end

    body << "\nObjectSpace stats:\n" << line
    stat_after[:stats].to_a.sort{|x,y| y[1].bytes <=> x[1].bytes}.each do |k,v|
      body << output_object_space(k,v)
    end

    body << "\nString stats:\n" << line
    string_analysis.to_a.sort{|x,y| y[1] <=> x[1] }.take(1000).each do |string,count|
      body << "#{count} : #{string}\n"
    end

    return [200, {'Content-Type' => 'text/plain'}, body]
  end

  private

  def output_object_space(klass, stats)
    "#{klass} : #{stats.objects} : #{(stats.bytes/1024).floor}K\n"
  end
end
