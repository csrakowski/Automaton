using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automaat
{
	public class Taal : SortedSet<String>
	{
		public Taal()
		{
		}
		public Taal(IComparer<String> comperator) : base(comperator)
		{
		}

		public static bool IsSameLetterAs(char c, char s)
		{
			return c.ToString().ToLower().Equals(s.ToString().ToLower());
		}

		public override string ToString()
		{
			var s = String.Empty;
			foreach (var item in this)
			{
				s += ", " + item;
			}
			if (s.Length > 2)
			{
				s = s.Substring(2);
			}
			return s;
			//return base.ToString();
		}

		public void Add(Taal taal)
		{
			foreach (var item in taal)
			{
				this.Add(item);
			}
		}
	}
}
