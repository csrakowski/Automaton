using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automaat
{
	public class Transition<T> : IComparable<Transition<T>> where T : IComparable<T>
	{
		public static readonly char EPSILON = 'ε';
	
		//private T fromState;
		//private char symbol;
		//private T toState;

		public T FromState { get; set; }
		public char Symbol { get; set; }
		public T ToState { get; set; }


	   // this constructor can be used to define loops:
		public Transition(T fromOrTo, char s) : this(fromOrTo, s, fromOrTo) { }
		public Transition(T from, T to) : this(from, EPSILON, to) {	}
		public Transition(T from, char s, T to)
		{
			FromState = from;
			Symbol = s;
			ToState = to;
		}

		// overriding equals
		public override bool Equals ( Object other )
		{
			if (other == null)
			{
				return false;
			}
			else if (other is Transition<T>)
			{
				var otherCast = other as Transition<T>;
				return (FromState.Equals(otherCast.FromState) &&
						ToState.Equals(otherCast.ToState) &&
						Symbol.Equals(otherCast.Symbol));
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{			
			return base.GetHashCode();
		}
	
		//@SuppressWarnings("unchecked")
		public int CompareTo(Transition<T> t)
		{
			int fromCmp = FromState.CompareTo(t.FromState);
			int symbolCmp = Symbol.CompareTo(t.Symbol);
			int toCmp = ToState.CompareTo(t.ToState);
		
			return (fromCmp != 0 ? fromCmp : (symbolCmp != 0 ? symbolCmp : toCmp));
		}
	
		public override String ToString()
		{
		   return String.Format("({0}, {1}) --> {2}", FromState, Symbol, ToState);
		}

		public string ToGraphvizString()
		{
			return String.Format("\"{0}\" -> \"{1}\" [ label = \"{2}\" ];\n", FromState, ToState, Symbol);
		}
	}
}
