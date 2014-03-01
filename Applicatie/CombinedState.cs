using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automaat
{
	class CombinedState<T> : SortedSet<T>, IComparable
	{
		public CombinedState()
		{

		}

		public CombinedState(SortedSet<T> set)
			: base(set)
		{
		}

		//Ugly hack :-(
		public T ToT()
		{
			return castToT(ToString());
		}

		public static T castToT(string str)
		{
			return (T)(object)str;
		}

		public override string ToString()
		{
			String s = String.Empty;

			foreach (var item in this)
			{
				s += ", " +  item.ToString();
			}

			if (s.Length > 2)
			{
				s = s.Substring(2);
			}

			return "[" + s + "]";
		}

		public int CompareTo(object obj)
		{
			if (obj is CombinedState<T>)
			{
				var other = (obj as CombinedState<T>);
				return other.ToString().CompareTo(this.ToString());
			}
			return -1;
		}

		public bool Contains(CombinedState<T> other)
		{
			other.UnionWith(this);
			return (other.Count == this.Count);
		}
	}
}
