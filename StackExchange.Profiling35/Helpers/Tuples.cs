using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackExchange.Profiling.Helpers.Tuples
{
    /// <summary>Provides static methods for creating tuple objects. </summary>
    public static class Tuple35
    {
        /// <summary>Creates a new 1-tuple, or singleton.</summary>
        /// <returns>A tuple whose value is (<paramref name="item1" />).</returns>
        /// <param name="item1">The value of the only component of the tuple.</param>
        /// <typeparam name="T1">The type of the only component of the tuple.</typeparam>
        public static Tuple<T1> Create<T1>(T1 item1)
        {
            return new Tuple<T1>(item1);
        }
        /// <summary>Creates a new 2-tuple, or pair.</summary>
        /// <returns>A 2-tuple whose value is (<paramref name="item1" />, <paramref name="item2" />).</returns>
        /// <param name="item1">The value of the first component of the tuple.</param>
        /// <param name="item2">The value of the second component of the tuple.</param>
        /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>(item1, item2);
        }   
        internal static int CombineHashCodes(int h1, int h2)
        {
            return (h1 << 5) + h1 ^ h2;
        }
        internal static int CombineHashCodes(int h1, int h2, int h3)
        {
            return Tuple35.CombineHashCodes(Tuple35.CombineHashCodes(h1, h2), h3);
        }
    }

    [Serializable]
    public class Tuple<T1> : IComparable
    {
        private readonly T1 m_Item1;
        /// <summary>Gets the value of the <see cref="T:System.Tuple`1" /> object's single component. </summary>
        /// <returns>The value of the current <see cref="T:System.Tuple`1" /> object's single component.</returns>
        public T1 Item1
        {
            get
            {
                return this.m_Item1;
            }
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Tuple`1" /> class.</summary>
        /// <param name="item1">The value of the tuple's only component.</param>
        public Tuple(T1 item1)
        {
            this.m_Item1 = item1;
        }
        /// <summary>Returns a value that indicates whether the current <see cref="T:System.Tuple`1" /> object is equal to a specified object.</summary>
        /// <returns>true if the current instance is equal to the specified object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<object>.Default);
        }
        /// <summary>Returns a value that indicates whether the current <see cref="T:System.Tuple`1" /> object is equal to a specified object based on a specified comparison method.</summary>
        /// <returns>true if the current instance is equal to the specified object; otherwise, false.</returns>
        /// <param name="other">The object to compare with this instance.</param>
        /// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
        bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
            {
                return false;
            }
            Tuple<T1> tuple = other as Tuple<T1>;
            return tuple != null && comparer.Equals(this.m_Item1, tuple.m_Item1);
        }
        /// <summary>Compares the current <see cref="T:System.Tuple`1" /> object to a specified object, and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
        /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="obj" /> in the sort order, as shown in the following table.ValueDescriptionA negative integerThis instance precedes <paramref name="obj" />.ZeroThis instance and <paramref name="obj" /> have the same position in the sort order.A positive integerThis instance follows <paramref name="obj" />.</returns>
        /// <param name="obj">An object to compare with the current instance.</param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="obj" /> is not a <see cref="T:System.Tuple`1" /> object.</exception>
        int IComparable.CompareTo(object obj)
        {
            return this.CompareTo(obj, Comparer<object>.Default);
        }
        /// <summary>Compares the current <see cref="T:System.Tuple`1" /> object to a specified object by using a specified comparer, and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
        /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.ValueDescriptionA negative integerThis instance precedes <paramref name="other" />.ZeroThis instance and <paramref name="other" /> have the same position in the sort order.A positive integerThis instance follows <paramref name="other" />.</returns>
        /// <param name="other">An object to compare with the current instance.</param>
        /// <param name="comparer">An object that provides custom rules for comparison.</param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="other" /> is not a <see cref="T:System.Tuple`1" /> object.</exception>
        int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            Tuple<T1> tuple = other as Tuple<T1>;
            if (tuple == null)
            {
                throw new ArgumentException("ArgumentException_TupleIncorrectType", "other");
            }
            return comparer.Compare(this.m_Item1, tuple.m_Item1);
        }
        /// <summary>Returns the hash code for the current <see cref="T:System.Tuple`1" /> object.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }
        /// <summary>Calculates the hash code for the current <see cref="T:System.Tuple`1" /> object by using a specified computation method.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        /// <param name="comparer">An object whose <see cref="M:System.Collections.IEqualityComparer.GetHashCode(System.Object)" />  method calculates the hash code of the current <see cref="T:System.Tuple`1" /> object.</param>
        int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this.m_Item1);
        }

        /// <summary>Returns a string that represents the value of this <see cref="T:System.Tuple`1" /> instance.</summary>
        /// <returns>The string representation of this <see cref="T:System.Tuple`1" /> object.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            return this.ToString(stringBuilder);
        }
        string ToString(StringBuilder sb)
        {
            sb.Append(this.m_Item1);
            sb.Append(")");
            return sb.ToString();
        }
    }

    public class Tuple<T1, T2> : IComparable
    {
        private readonly T1 m_Item1;
        private readonly T2 m_Item2;
        /// <summary>Gets the value of the current <see cref="T:System.Tuple`2" /> object's first component.</summary>
        /// <returns>The value of the current <see cref="T:System.Tuple`2" /> object's first component.</returns>
        public T1 Item1
        {
            get
            {
                return this.m_Item1;
            }
        }
        /// <summary>Gets the value of the current <see cref="T:System.Tuple`2" /> object's second component.</summary>
        /// <returns>The value of the current <see cref="T:System.Tuple`2" /> object's second component.</returns>
        public T2 Item2
        {
            get
            {
                return this.m_Item2;
            }
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Tuple`2" /> class.</summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        public Tuple(T1 item1, T2 item2)
        {
            this.m_Item1 = item1;
            this.m_Item2 = item2;
        }
        /// <summary>Returns a value that indicates whether the current <see cref="T:System.Tuple`2" /> object is equal to a specified object.</summary>
        /// <returns>true if the current instance is equal to the specified object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<object>.Default);
        }
        /// <summary>Returns a value that indicates whether the current <see cref="T:System.Tuple`2" /> object is equal to a specified object based on a specified comparison method.</summary>
        /// <returns>true if the current instance is equal to the specified object; otherwise, false.</returns>
        /// <param name="other">The object to compare with this instance.</param>
        /// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
        bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
            {
                return false;
            }
            Tuple<T1, T2> tuple = other as Tuple<T1, T2>;
            return tuple != null && comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2);
        }
        /// <summary>Compares the current <see cref="T:System.Tuple`2" /> object to a specified object and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
        /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="obj" /> in the sort order, as shown in the following table.ValueDescriptionA negative integerThis instance precedes <paramref name="obj" />.ZeroThis instance and <paramref name="obj" /> have the same position in the sort order.A positive integerThis instance follows <paramref name="obj" />.</returns>
        /// <param name="obj">An object to compare with the current instance.</param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="obj" /> is not a <see cref="T:System.Tuple`2" /> object.</exception>
        int IComparable.CompareTo(object obj)
        {
            return this.CompareTo(obj, Comparer<object>.Default);
        }
        /// <summary>Compares the current <see cref="T:System.Tuple`2" /> object to a specified object by using a specified comparer, and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
        /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.ValueDescriptionA negative integerThis instance precedes <paramref name="other" />.ZeroThis instance and <paramref name="other" /> have the same position in the sort order.A positive integerThis instance follows <paramref name="other" />.</returns>
        /// <param name="other">An object to compare with the current instance.</param>
        /// <param name="comparer">An object that provides custom rules for comparison.</param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="other" /> is not a <see cref="T:System.Tuple`2" /> object.</exception>
        int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            Tuple<T1, T2> tuple = other as Tuple<T1, T2>;
            if (tuple == null)
            {
                throw new ArgumentException("ArgumentException_TupleIncorrectType", "other");
            }
            int num = comparer.Compare(this.m_Item1, tuple.m_Item1);
            if (num != 0)
            {
                return num;
            }
            return comparer.Compare(this.m_Item2, tuple.m_Item2);
        }
        /// <summary>Returns the hash code for the current <see cref="T:System.Tuple`2" /> object.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }
        /// <summary>Calculates the hash code for the current <see cref="T:System.Tuple`2" /> object by using a specified computation method.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        /// <param name="comparer">An object whose <see cref="M:System.Collections.IEqualityComparer.GetHashCode(System.Object)" />  method calculates the hash code of the current <see cref="T:System.Tuple`2" /> object.</param>
        int GetHashCode(IEqualityComparer comparer)
        {
            return Tuple35.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2));
        }
        /// <summary>Returns a string that represents the value of this <see cref="T:System.Tuple`2" /> instance.</summary>
        /// <returns>The string representation of this <see cref="T:System.Tuple`2" /> object.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            return this.ToString(stringBuilder);
        }
        string ToString(StringBuilder sb)
        {
            sb.Append(this.m_Item1);
            sb.Append(", ");
            sb.Append(this.m_Item2);
            sb.Append(")");
            return sb.ToString();
        }
    }
}
