Gem::Specification.new do |s|
	s.name = "rack-mini-profiler"
	s.version = "0.1.31"
	s.summary = "Profiles loading speed for rack applications."
	s.authors = ["Sam Saffron", "Robin Ward","Aleks Totic"]
	s.description = "Profiling toolkit for Rack applications with Rails integration. Client Side profiling, DB profiling and Server profiling."
	s.email = "sam.saffron@gmail.com"
	s.homepage = "http://miniprofiler.com"
  s.license = "MIT"
	s.files = [
		'rack-mini-profiler.gemspec',
	].concat( Dir.glob('Ruby/lib/**/*').reject {|f| File.directory?(f) || f =~ /~$/ } )
	s.extra_rdoc_files = [
		"Ruby/README.md",
    "Ruby/CHANGELOG"
	]
	s.add_runtime_dependency 'rack', '>= 1.1.3'
  if RUBY_VERSION < "1.9"
    s.add_runtime_dependency 'json', '>= 1.6'
  end

  s.add_development_dependency 'rake'
  s.add_development_dependency 'rack-test'
  s.add_development_dependency 'activerecord', '~> 3.0'

  s.require_paths = ["Ruby/lib"]
end
