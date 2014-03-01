using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Automaat
{
	public class Automaton<T> where T : IComparable<T>
	{
		private SortedSet<Transition<T>> transitions;
		private SortedSet<T> states;
		private SortedSet<T> startStates;
		private SortedSet<T> finalStates;
		private SortedSet<char> alphabet;

		public Automaton() : this(new SortedSet<char>())
		{
		}    
		public Automaton(char[] s) : this(new SortedSet<char>(s))
		{
			//this(new TreeSet<char>(Arrays.asList(s)));
		}
		public Automaton(SortedSet<char> symbols)
		{
			//transitions = new TreeSet<Transition<T>>();
			//states = new TreeSet<T>();
			//startStates = new TreeSet<T>();
			//finalStates = new TreeSet<T>();

			transitions = new SortedSet<Transition<T>>();
			states = new SortedSet<T>();
			startStates = new SortedSet<T>();
			finalStates = new SortedSet<T>();

			this.SetAlphabet(symbols);
		}

		public void SetAlphabet(char[] s)
		{
			this.SetAlphabet(new SortedSet<char>(s));
			//this.setAlphabet(new TreeSet<char>(Arrays.asList(s)));
		}
		public void SetAlphabet(SortedSet<char> symbols)
		{
		   this.alphabet = symbols;
		}
		public SortedSet<char> GetAlphabet()
		{
		   return alphabet;
		}

		public void AddTransition(Transition<T> t)
		{
			transitions.Add(t);
			states.Add(t.FromState);
			states.Add(t.ToState);
		}
		public void AddTransition(SortedSet<Transition<T>> transitions)
		{
			foreach (var item in transitions)
			{
				AddTransition(item);
			}
		}		
		
		public void DefineAsStartState(T t)
		{
			// if already in states no problem because a Set will remove duplicates.
			states.Add(t);
			startStates.Add(t);
		}
		public void DefineAsFinalState(T t)
		{
			// if already in states no problem because a Set will remove duplicates.
			states.Add(t);
			finalStates.Add(t);        
		}

		public void PrintTransitions()
		{
			foreach(var t in transitions)
			{
				Console.WriteLine(t);
			}
		}
		
		public bool IsDFA()
		{
			bool isDFA = true;
		
			foreach(T from in states)
			{
				foreach(char symbol in alphabet)
				{
					isDFA = isDFA && GetToStates(from, symbol).Count == 1;
				}
			}        
			return isDFA;
		}
		
		public SortedSet<Transition<T>> GetToStates(T currentState, char symbol)
		{
			var result = new SortedSet<Transition<T>>();
			var fromStates = epsilonClosure(currentState);

			var tempResult = transitions.Where(t => (fromStates.Contains(t.FromState)) && ((Taal.IsSameLetterAs(t.Symbol, symbol))));

//			var tempResult = transitions.Where(t => (t.FromState.CompareTo(currentState) == 0) && ((Taal.IsSameLetterAs(t.Symbol, symbol))));
			foreach (var item in tempResult)
			{
				result.Add(item);
			}
			return result;
		}
		public SortedSet<T> epsilonClosure(T fromState)
		{
			var fromStates = new SortedSet<T>();
			fromStates.Add(fromState);
			var reachable = new SortedSet<T>();
			var newFound = new SortedSet<T>();

			do
			{
				foreach (var item in fromStates)
				{
					reachable.Add(item);
				}

				newFound = new SortedSet<T>();
				foreach (var from in fromStates)
				{
					foreach (var t in transitions)
					{
						if ((t.FromState.CompareTo(from) == 0) && Taal.IsSameLetterAs(t.Symbol, Transition<T>.EPSILON) && !fromStates.Contains(t.ToState))
						{
							newFound.Add(t.ToState);
						}
					}
				}

				foreach (var item in newFound)
				{
					fromStates.Add(item);
				}
				reachable = newFound;
			}
			while (newFound.Count > 0);

			return fromStates;
		}

		public SortedSet<T> GetToStates2(T from, char symbol)
		{
			var fromStates = new SortedSet<T>();
			fromStates.Add(from);
			fromStates = epsilonClosure2(fromStates);

			var toStates = new SortedSet<T>();

			foreach (T fromState in fromStates)
			{
				foreach (var t in transitions)
				{
					if ((t.ToState.CompareTo(fromState) == 0) && Taal.IsSameLetterAs(t.Symbol, symbol))
					{
						toStates.Add(t.ToState);
					}
				}
			}

			return epsilonClosure2(toStates);
		}
		public SortedSet<T> epsilonClosure2(SortedSet<T> fromStates)
		{
			var reachable = new SortedSet<T>();
			var newFound = new SortedSet<T>();

			do {
				foreach (var item in fromStates)
				{
					reachable.Add(item);
				}

				newFound = new SortedSet<T>();
				foreach (T fromState in fromStates)
				{
					foreach (var t in transitions)
					{
						if ((t.FromState.CompareTo(fromState) == 0) && Taal.IsSameLetterAs(t.Symbol, Transition<T>.EPSILON) && !fromStates.Contains(t.ToState))
						{
							newFound.Add(t.ToState);
						}
					}
				}

				foreach (var item in newFound)
				{
					fromStates.Add(item);
				}
				reachable = newFound;
			}
			while (newFound.Count > 0);

			return fromStates;
		}

		public bool AcceptNDFA(String str)
		{
			try
			{
				//Get all transitions where the FromState is the startState and the symbol is the first letter of the input word
				var trans = new SortedSet<Transition<T>>();
				foreach (var item in startStates)
				{
					var temp = GetToStates(item, str[0]);
					foreach (var t in temp)
					{
						trans.Add(t);
					}
				}
				foreach (var item in trans)
				{
					if (AcceptInnerNDFA(item.ToState, str.Substring(1)))
					{
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				//This is expected behaviour for the final recursion step (trans.first() causes this)
				Debug.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				//Console.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				return false;
			}
		}
		private bool AcceptInnerNDFA(T state, String remainder)
		{
			var trans = new SortedSet<Transition<T>>();
			foreach (var item in startStates)
			{
				var temp = GetToStates(item, remainder[0]);
				foreach (var t in temp)
				{
					trans.Add(t);
				}
			}

			//Empty string
			if (String.IsNullOrEmpty(remainder))
			{
				//If this state is a final state, return true, else return false
				return (finalStates.Where(s => s.CompareTo(state) == 0).Count() > 0);
			}
			try
			{
				//Keep recursing
				foreach (var item in trans)
				{
					if (AcceptInnerNDFA(item.ToState, remainder.Substring(1)))
					{
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				//This is expected behaviour for the final recursion step (trans.first() causes this)
				Debug.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				//Console.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				return false;
			}
		}

		public bool Accept(String str)
		{			
			try
			{
				//Get all transitions where the FromState is the startState and the symbol is the first letter of the input word
				if (!IsDFA())
				{
					var temp = ConvertToDFA();
					var trans = GetToStates(temp.startStates.First<T>(), str[0]);
					return AcceptInner(trans.First().ToState, str.Substring(1));
				}
				else
				{
					var trans = GetToStates(startStates.First<T>(), str[0]);
					return AcceptInner(trans.First().ToState, str.Substring(1));
				}				
			}
			catch (Exception ex)
			{
				//This is expected behaviour for the final recursion step (trans.first() causes this)
				Debug.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				//Console.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				return false;
			}
		}
		private bool AcceptInner(T state, String remainder)
		{
			//Empty string
			if (String.IsNullOrEmpty(remainder))
			{
				//If this state is a final state, return true, else return false
				return (finalStates.Where(s => s.CompareTo(state) == 0).Count() > 0);
			}
			try
			{
				//Keep recursing
				var trans = GetToStates(state, remainder[0]);
				return AcceptInner(trans.First().ToState, remainder.Substring(1));
			}
			catch (Exception ex)
			{
				//This is expected behaviour for the final recursion step (trans.first() causes this)
				Debug.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				//Console.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				return false;
			}
		}

		public void SetValues(Automaton<T> automaton)
		{
			this.startStates = automaton.startStates;
			this.finalStates = automaton.finalStates;
			this.alphabet = automaton.alphabet;
			this.states = automaton.states;
			this.transitions = automaton.transitions;
		}

		public Taal GetLanguage(int maxLen)
		{
			//t.Add(Transition<T>.EPSILON);		// Empty collection
			return getLanguageInner("", maxLen);
		}
		private Taal getLanguageInner(String start, int maxLen)
		{
			var result = new Taal();
			if (start.Length == maxLen)
			{
				return result;
			}
			foreach (var item in alphabet)
			{
				String str = start + item;
				if (Accept(str))
				{
					result.Add(str);
				}
				result.Add(getLanguageInner(str, maxLen));
			}
			return result;
		}

		public bool SaveToGraphvizFile(String filePath)
		{
			FileStream file;
			try
			{
				file = File.Open(filePath, FileMode.Create);

				#region Write Graphviz Header data
		
				Byte[] headerBuffer = Encoding.UTF8.GetBytes("digraph finite_state_machine {\n");
				file.Write(headerBuffer, 0, headerBuffer.Length);

				headerBuffer = Encoding.UTF8.GetBytes("rankdir=LR;\n");
				file.Write(headerBuffer, 0, headerBuffer.Length);

				//Defines a max size, leave out to auto scale
				//headerBuffer = Encoding.UTF8.GetBytes("size=\"8,5\"\n");
				//file.Write(headerBuffer, 0, headerBuffer.Length);

				#endregion

				#region Write final states to file

				String endstates = "node [shape = doublecircle];";
				foreach (var item in finalStates)
				{
					endstates += " \"" + item.ToString() + "\"";
				}
				endstates += ";\n";

				Byte[] endStates = Encoding.UTF8.GetBytes(endstates);
				file.Write(endStates, 0, endStates.Length);

				#endregion

				#region Write start states to file

				String startState = "node [shape = house];";
				foreach (var item in startStates)
				{
					startState += " \"" + item.ToString() + "\"";
				}
				startState += ";\n";

				Byte[] startStatesBytes = Encoding.UTF8.GetBytes(startState);
				file.Write(startStatesBytes, 0, startStatesBytes.Length);

				#endregion

				#region Write transitions to file

				Byte[] shape = Encoding.UTF8.GetBytes("node [shape = circle];\n");
				file.Write(shape, 0, shape.Length);

				foreach (Transition<T> t in transitions)
				{
					Byte[] buffer = Encoding.UTF8.GetBytes(t.ToGraphvizString());
					file.Write(buffer, 0, buffer.Length);
				}

				#endregion

				#region Write Graphviz Footer data

				Byte[] footerBuffer = Encoding.UTF8.GetBytes("}\n");
				file.Write(footerBuffer, 0, footerBuffer.Length);

				#endregion

				file.Close();
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				Console.WriteLine(String.Format("\n{0}:\t {1}\n{2}\n", ex.Source, ex.GetType(), ex.Message));
				return false;
			}
		}

		private bool ListContainsElement(SortedSet<T> finalStates, T t)
		{
			foreach (var item in finalStates)
			{
				//if (item.Equals(t))
				if (t.ToString().Contains(item.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		public Automaton<T> Minimize()
		{
			var step1 = Reverse();
			step1.SaveToGraphvizFile("step1.gv");
			var step2 = step1.ConvertToDFA();
			step2.SaveToGraphvizFile("step2.gv");
			var step3 = step2.Reverse();
			step3.SaveToGraphvizFile("step3.gv");
			var step4 = step3.ConvertToDFA();
			step4.SaveToGraphvizFile("step4.gv");

			return step4;
		}
		public Automaton<T> ConvertToDFA()
		{
			Console.WriteLine("Converting from NDFA to DFA...");

			var starters = new CombinedState<T>(epsilonClosure2(startStates));
			var result = new Automaton<T>(alphabet);

			var set = new SortedSet<CombinedState<T>>();
			set.Add(starters);
			var res = ConvertToDFAInner(starters, set);
			result.AddTransition(res);

			result.DefineAsStartState(starters.ToT());

			//SortedSet<CombinedState<T>> endstates = new SortedSet<CombinedState<T>>();
			//endstates.Add(new CombinedState<T>(finalStates));
			var s = new CombinedState<T>(finalStates);
			var endstates = result.transitions.Where(t => ListContainsElement(finalStates, t.ToState)).Select(t => t.ToState).ToList<T>();
			foreach (var item in endstates)
			{
				result.DefineAsFinalState(item);
			}

			Console.WriteLine("Conversion succesful\n");
			return result;
		}
		private SortedSet<Transition<T>> ConvertToDFAInner(CombinedState<T> starters, SortedSet<CombinedState<T>> total)
		{
			var result = new SortedSet<Transition<T>>();
			var combis = new SortedSet<CombinedState<T>>();
			foreach (var state in starters)
			{
				foreach (var c in alphabet)
				{
					var states = new SortedSet<T>(GetToStates(state, c).Select(t => t.ToState).ToList<T>());
					var temp = new CombinedState<T>(epsilonClosure2(states));
					if (temp.Count == 0)
					{
						continue;
					}
					combis.Add(temp);
					var tran = new Transition<T>(starters.ToT(), c, temp.ToT());
					//if (!String.IsNullOrEmpty(tran.FromState.ToString()) && !String.IsNullOrEmpty(tran.ToState.ToString()))
					//{
						result.Add(tran);
					//}
				}
			}

			foreach (var item in combis)
			{
				//if (!(item.ToString() == starters.ToString()))
				if(!total.Contains(item))
				{
					total.Add(item);
					var tempres = ConvertToDFAInner(item, total);
					foreach (var itm in tempres)
					{
						result.Add(itm);
					}
				}
			}
			return result;
		}

		public Automaton<T> Reverse()
		{
			Console.WriteLine("Reversing automaton...");
			var result = new Automaton<T>(alphabet);

			foreach (var item in transitions)
			{
				result.AddTransition(new Transition<T>(item.ToState, item.Symbol, item.FromState));
			}

			result.startStates = finalStates;
			result.finalStates = startStates;

			Console.WriteLine("Reversing complete\n");
			return result;
		}
	}
}
