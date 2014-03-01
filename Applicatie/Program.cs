using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automaat
{
	class Program
	{
		/// <summary>
		/// Main method
		/// </summary>
		/// <param name="args">String arguments passed by the console, not used at this time</param>
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to the Automaton-9000\n\n");
			//Call the automaton construction method here
			var automaton = begintMetABBEnBevatBAAB();			

			while (true)
			{
				Console.WriteLine("Enter input to test against");
				String input = Console.ReadLine();
				if(input.Equals("exit", StringComparison.OrdinalIgnoreCase))
				{
					break;
				}

				if(input.Equals("help", StringComparison.OrdinalIgnoreCase))
				{
					#region help
					Console.WriteLine("regex");
					Console.WriteLine("\tRun the regex test method\n");

					Console.WriteLine("getautomaton");
					Console.WriteLine("\tCreates an automaton from a Regex\n");

					Console.WriteLine("print");
					Console.WriteLine("\tPrint the transitions of the current automaton to the screen\n");

					Console.WriteLine("savefile <filename (string)>");
					Console.WriteLine("\tSave the current automaton to a Graphviz file\n");

					Console.WriteLine("reverse");
					Console.WriteLine("\tReverse the current automaton\n");

					Console.WriteLine("convert");
					Console.WriteLine("\tConvert the current automaton to a DFA\n");

					Console.WriteLine("minimize");
					Console.WriteLine("\tMinimize the current automaton\n");					

					Console.WriteLine("taal: <length (int)>");
					Console.WriteLine("\tGets all words the current automaton accepts, upto max length of \"length\"\n");

					Console.WriteLine("<word (string)>");
					Console.WriteLine("\tTests if the entered word will be accepted by the current automaton\n");
					#endregion
					continue;
				}

				if (input.Equals("regex", StringComparison.OrdinalIgnoreCase))
				{
					testRegex();
					continue;
				}

				if (input.StartsWith("getautomaton", StringComparison.OrdinalIgnoreCase))
				{
					automaton = GetAutomatonForRegex();
					continue;
				}

				if (input.Equals("print", StringComparison.OrdinalIgnoreCase))
				{
					automaton.PrintTransitions();
					continue;
				}

				if (input.Equals("reverse", StringComparison.OrdinalIgnoreCase))
				{
					automaton = automaton.Reverse();
					continue;
				}

				if (input.Equals("minimize", StringComparison.OrdinalIgnoreCase))
				{
					automaton = automaton.Minimize();
					continue;
				}

				if (input.Equals("convert", StringComparison.OrdinalIgnoreCase))
				{
					automaton = automaton.ConvertToDFA();
					continue;
				}

				if (input.StartsWith("savefile", StringComparison.OrdinalIgnoreCase))
				{
					input = input.Substring("savefile".Length).Trim();
					automaton.SaveToGraphvizFile(input + ".gv");
					continue;
				}

				if (input.StartsWith("taal", StringComparison.OrdinalIgnoreCase))
				{
					var parts = input.Split(':');
					int res;
					if(Int32.TryParse(parts[1], out res))
					{
						Console.WriteLine( automaton.GetLanguage(res));
					}
					continue;
				}

				Console.WriteLine("Input string \"{0}\" was tested, it returned \"{1}\"", input, automaton.Accept(input));
			}
		}

		#region Standard automaton construction methods

		public static Automaton<String> begintMetABBOfEindigtOpBAAB()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q0", 'a', "q1"));
			m.AddTransition(new Transition<String>("q0", 'b', "q4"));

			m.AddTransition(new Transition<String>("q1", 'a', "q4"));
			m.AddTransition(new Transition<String>("q1", 'b', "q2"));

			m.AddTransition(new Transition<String>("q2", 'a', "q3"));
			m.AddTransition(new Transition<String>("q2", 'b', "q5"));

			m.AddTransition(new Transition<String>("q3", 'a', "q1"));
			m.AddTransition(new Transition<String>("q3", 'b', "q2"));

			m.AddTransition(new Transition<String>("q4", 'a'));
			m.AddTransition(new Transition<String>("q4", 'b'));

			m.DefineAsStartState("q0");

			m.DefineAsFinalState("q5");
			m.DefineAsFinalState("q6");

			return m;
		}
		public static Automaton<String> bevatEvenAantalBsOfOnevenAantalAs()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q1", 'a', "q2"));
			m.AddTransition(new Transition<String>("q1", 'b', "q3"));

			m.AddTransition(new Transition<String>("q2", 'a', "q1"));
			m.AddTransition(new Transition<String>("q2", 'b', "q4"));

			m.AddTransition(new Transition<String>("q3", 'a', "q3"));
			m.AddTransition(new Transition<String>("q3", 'b', "q1"));

			m.AddTransition(new Transition<String>("q4", 'a', "q3"));
			m.AddTransition(new Transition<String>("q4", 'b', "q2"));

			m.DefineAsStartState("q1");

			m.DefineAsFinalState("q1");
			m.DefineAsFinalState("q2");
			m.DefineAsFinalState("q4");

			return m;
		}
		public static Automaton<String> bevatEvenAantalBsEnEindigtOpAAB()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q1", 'a', "q2"));
			m.AddTransition(new Transition<String>("q1", 'b', "q5"));

			m.AddTransition(new Transition<String>("q2", 'a', "q3"));
			m.AddTransition(new Transition<String>("q2", 'b', "q5"));

			m.AddTransition(new Transition<String>("q3", 'a', "q3"));
			m.AddTransition(new Transition<String>("q3", 'b', "q8"));

			m.AddTransition(new Transition<String>("q4", 'a', "q2"));
			m.AddTransition(new Transition<String>("q4", 'b', "q5"));

			m.AddTransition(new Transition<String>("q5", 'a', "q6"));
			m.AddTransition(new Transition<String>("q5", 'b', "q1"));

			m.AddTransition(new Transition<String>("q6", 'a', "q7"));
			m.AddTransition(new Transition<String>("q6", 'b', "q1"));

			m.AddTransition(new Transition<String>("q7", 'a', "q7"));
			m.AddTransition(new Transition<String>("q7", 'b', "q4"));

			m.AddTransition(new Transition<String>("q8", 'a', "q6"));
			m.AddTransition(new Transition<String>("q8", 'b', "q1"));

			m.DefineAsStartState("q1");

			m.DefineAsFinalState("q4");

			return m;
		}
		public static Automaton<String> begintMetABBEnBevatBAAB()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q1", 'a', "q6"));
			m.AddTransition(new Transition<String>("q1", 'b', "q22"));

			m.AddTransition(new Transition<String>("q2", 'a', "q8"));
			m.AddTransition(new Transition<String>("q2", 'b', "q22"));

			m.AddTransition(new Transition<String>("q3", 'a', "q9"));
			m.AddTransition(new Transition<String>("q3", 'b', "q22"));

			m.AddTransition(new Transition<String>("q4", 'a', "q6"));
			m.AddTransition(new Transition<String>("q4", 'b', "q25"));

			m.AddTransition(new Transition<String>("q5", 'a', "q10"));
			m.AddTransition(new Transition<String>("q5", 'b', "q25"));

			m.AddTransition(new Transition<String>("q6", 'a', "q21"));
			m.AddTransition(new Transition<String>("q6", 'b', "q12"));

			m.AddTransition(new Transition<String>("q7", 'a', "q23"));
			m.AddTransition(new Transition<String>("q7", 'b', "q12"));

			m.AddTransition(new Transition<String>("q8", 'a', "q24"));
			m.AddTransition(new Transition<String>("q8", 'b', "q12"));

			m.AddTransition(new Transition<String>("q9", 'a', "q21"));
			m.AddTransition(new Transition<String>("q9", 'b', "q15"));

			m.AddTransition(new Transition<String>("q10", 'a', "q25"));
			m.AddTransition(new Transition<String>("q10", 'b', "q15"));

			m.AddTransition(new Transition<String>("q11", 'a', "q21"));
			m.AddTransition(new Transition<String>("q11", 'b', "q17"));

			m.AddTransition(new Transition<String>("q12", 'a', "q23"));
			m.AddTransition(new Transition<String>("q12", 'b', "q17"));

			m.AddTransition(new Transition<String>("q13", 'a', "q24"));
			m.AddTransition(new Transition<String>("q13", 'b', "q17"));

			m.AddTransition(new Transition<String>("q14", 'a', "q21"));
			m.AddTransition(new Transition<String>("q14", 'b', "q20"));

			m.AddTransition(new Transition<String>("q15", 'a', "q25"));
			m.AddTransition(new Transition<String>("q15", 'b', "q20"));

			m.AddTransition(new Transition<String>("q16", 'a', "q16"));
			m.AddTransition(new Transition<String>("q16", 'b', "q17"));

			m.AddTransition(new Transition<String>("q17", 'a', "q18"));
			m.AddTransition(new Transition<String>("q17", 'b', "q17"));

			m.AddTransition(new Transition<String>("q18", 'a', "q19"));
			m.AddTransition(new Transition<String>("q18", 'b', "q17"));

			m.AddTransition(new Transition<String>("q19", 'a', "q16"));
			m.AddTransition(new Transition<String>("q19", 'b', "q20"));

			m.AddTransition(new Transition<String>("q20", 'a', "q20"));
			m.AddTransition(new Transition<String>("q20", 'b', "q20"));

			m.AddTransition(new Transition<String>("q21", 'a', "q21"));
			m.AddTransition(new Transition<String>("q21", 'b', "q22"));

			m.AddTransition(new Transition<String>("q22", 'a', "q23"));
			m.AddTransition(new Transition<String>("q22", 'b', "q22"));

			m.AddTransition(new Transition<String>("q23", 'a', "q24"));
			m.AddTransition(new Transition<String>("q23", 'b', "q22"));

			m.AddTransition(new Transition<String>("q24", 'a', "q21"));
			m.AddTransition(new Transition<String>("q24", 'b', "q25"));

			m.AddTransition(new Transition<String>("q25", 'a', "q25"));
			m.AddTransition(new Transition<String>("q25", 'b', "q25"));

			m.DefineAsStartState("q1");

			m.DefineAsFinalState("q20");

			return m;
		}

		public static Automaton<String> SampleNDFA()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q1", 'a', "q1"));
			m.AddTransition(new Transition<String>("q1", 'a', "q2"));
			m.AddTransition(new Transition<String>("q1", "q2"));

			m.AddTransition(new Transition<String>("q2", 'b', "q3"));
			m.AddTransition(new Transition<String>("q2", 'a', "q1"));


			//m.AddTransition(new Transition<String>("q3", 'a', "q3"));
			//m.AddTransition(new Transition<String>("q3", "q1"));

			//m.AddTransition(new Transition<String>("q4", 'a', "q3"));
			//m.AddTransition(new Transition<String>("q4", 'b', "q2"));
			
			m.DefineAsStartState("q1");

			m.DefineAsFinalState("q3");

			return m;
		}
		public static Automaton<String> SimpleNDFA()
		{
			char[] alphabet = { 'a', 'b', 'c' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q1", 'a', "q2"));
			m.AddTransition(new Transition<String>("q1", 'c', "q4"));
			//m.AddTransition(new Transition<String>("q1", "q1"));

			m.AddTransition(new Transition<String>("q2", 'b', "q3"));
			m.AddTransition(new Transition<String>("q2", "q1"));

			m.AddTransition(new Transition<String>("q3", 'a', "q2"));

			m.AddTransition(new Transition<String>("q4", 'c', "q3"));
			m.AddTransition(new Transition<String>("q4", "q3"));

			m.DefineAsStartState("q1");

			m.DefineAsFinalState("q3");

			return m;
		}

		public static Automaton<String> Opdracht4Checklist()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q0", 'a', "q1"));

			m.AddTransition(new Transition<String>("q1", 'b', "q0"));
			m.AddTransition(new Transition<String>("q1", 'b', "q2"));

			m.AddTransition(new Transition<String>("q2", 'a', "q0"));
			m.AddTransition(new Transition<String>("q2", 'a', "q3"));

			m.AddTransition(new Transition<String>("q3", 'a', "q4"));			

			m.DefineAsStartState("q0");

			m.DefineAsFinalState("q0");
			m.DefineAsFinalState("q4");

			return m;
		}
		public static Automaton<String> Opdracht5Checklist()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("A", 'b', "B"));
			m.AddTransition(new Transition<String>("A", 'a', "C"));
			m.AddTransition(new Transition<String>("A", 'b', "C"));

			m.AddTransition(new Transition<String>("B", 'b', "C"));
			m.AddTransition(new Transition<String>("B", "C"));

			m.AddTransition(new Transition<String>("C", 'a', "E"));
			m.AddTransition(new Transition<String>("C", 'a', "D"));
			m.AddTransition(new Transition<String>("C", 'b', "D"));

			m.AddTransition(new Transition<String>("D", 'a', "B"));
			m.AddTransition(new Transition<String>("D", 'a', "C"));

			m.AddTransition(new Transition<String>("E", "D"));
			m.AddTransition(new Transition<String>("E", 'b', "E"));


			m.DefineAsStartState("A");

			m.DefineAsFinalState("C");
			m.DefineAsFinalState("E");

			return m;
		}
		public static Automaton<String> Opdracht6Checklist()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q0", 'a', "q2"));
			m.AddTransition(new Transition<String>("q0", 'b', "q3"));

			m.AddTransition(new Transition<String>("q1", 'a', "q3"));
			m.AddTransition(new Transition<String>("q1", 'b', "q2"));

			m.AddTransition(new Transition<String>("q2", 'a', "q0"));
			m.AddTransition(new Transition<String>("q2", 'b', "q4"));

			m.AddTransition(new Transition<String>("q3", 'a', "q1"));
			m.AddTransition(new Transition<String>("q3", 'b', "q5"));

			m.AddTransition(new Transition<String>("q4", 'a', "q6"));
			m.AddTransition(new Transition<String>("q4", 'b', "q5"));

			m.AddTransition(new Transition<String>("q5", 'a', "q2"));
			m.AddTransition(new Transition<String>("q5", 'b', "q0"));

			m.AddTransition(new Transition<String>("q6", 'a', "q4"));
			m.AddTransition(new Transition<String>("q6", 'b', "q0"));

			m.DefineAsStartState("q0");

			m.DefineAsFinalState("q1");
			m.DefineAsFinalState("q3");
			m.DefineAsFinalState("q4");
			m.DefineAsFinalState("q6");

			return m;
		}

		public static Automaton<String> getExampleSlide8Lesson2()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("q0", 'a', "q1"));
			m.AddTransition(new Transition<String>("q0", 'b', "q4"));

			m.AddTransition(new Transition<String>("q1", 'a', "q4"));
			m.AddTransition(new Transition<String>("q1", 'b', "q2"));

			m.AddTransition(new Transition<String>("q2", 'a', "q3"));
			m.AddTransition(new Transition<String>("q2", 'b', "q4"));

			m.AddTransition(new Transition<String>("q3", 'a', "q1"));
			m.AddTransition(new Transition<String>("q3", 'b', "q2"));

			// the error state, loops for a and b:
			m.AddTransition(new Transition<String>("q4", 'a'));
			m.AddTransition(new Transition<String>("q4", 'b'));

			// only on start state in a dfa:
			m.DefineAsStartState("q0");

			// two final states:
			m.DefineAsFinalState("q2");
			m.DefineAsFinalState("q3");

			return m;
		}
		public static Automaton<String> getExampleSlide14Lesson2()
		{
			char[] alphabet = { 'a', 'b' };
			Automaton<String> m = new Automaton<String>(alphabet);

			m.AddTransition(new Transition<String>("A", 'a', "C"));
			m.AddTransition(new Transition<String>("A", 'b', "B"));
			m.AddTransition(new Transition<String>("A", 'b', "C"));

			m.AddTransition(new Transition<String>("B", 'b', "C"));
			m.AddTransition(new Transition<String>("B", "C"));

			m.AddTransition(new Transition<String>("C", 'a', "D"));
			m.AddTransition(new Transition<String>("C", 'a', "E"));
			m.AddTransition(new Transition<String>("C", 'b', "D"));

			m.AddTransition(new Transition<String>("D", 'a', "B"));
			m.AddTransition(new Transition<String>("D", 'a', "C"));

			m.AddTransition(new Transition<String>("E", 'a'));
			m.AddTransition(new Transition<String>("E", "D"));

			// only on start state in a dfa:
			m.DefineAsStartState("A");

			// two final states:
			m.DefineAsFinalState("C");
			m.DefineAsFinalState("E");

			return m;
		}

		#endregion

		public static Automaton<String> GetAutomatonForRegex()
		{
			Console.WriteLine("Construction Regex for: a* (aa+ | ba*b | (bbb)+)* (aa | $)");

			// a*
			var expr1 = new Regex("a").Star();

			// aa+
			var expr2 = new Regex("a").Dot(new Regex("a").Plus());
			// ba*b
			var expr3 = new Regex("b").Dot(new Regex("a").Star()).Dot(new Regex("b"));
			// (bbb)+
			var expr4 = new Regex("bbb").Plus();
			// (aa+ | ba*b | (bbb)+)*
			var expr5 = expr2.Or(expr3).Or(expr4).Star();

			// aa
			var expr6 = new Regex("aa");
			// $
			var expr7 = new Regex(Transition<String>.EPSILON.ToString());
			// (aa | $)
			var expr8 = expr6.Or(expr7);

			// result
			var result = expr1.Dot(expr5).Dot(expr8);

			Console.WriteLine("Done\n");

			return result.GetAutomaton();
		}

		/// <summary>
		/// Simple regexp test method
		/// </summary>
		public static void testRegex()
		{
			var a = new Regex("a");
			var b = new Regex("b");
			
			// expr1: "baa"
			var expr1 = new Regex("baa");
			// expr2: "bb"
			var expr2 = new Regex("bb");
			// expr3: "baa | bb"
			var expr3 = expr1.Or(expr2);
			// all: "(a|b)*"
			var all = (a.Or(b)).Star();
			// expr4: "(baa | bb)+"
			var expr4 = expr3.Plus();
			 // expr5: "(baa | bb)+ (a|b)*"
			var expr5 = expr4.Dot(all);

			Console.WriteLine("taal van (baa):\n{0}\n", expr1.GetLanguage(5));
			Console.WriteLine("taal van (bb):\n{0}\n", expr2.GetLanguage(5));
			Console.WriteLine("taal van (baa | bb):\n{0}\n", expr3.GetLanguage(5));
			//Console.WriteLine("taal van (a|b)*:\n{0}\n", all.getLanguage(5));
			Console.WriteLine("taal van (baa | bb)+:\n{0}\n", expr4.GetLanguage(5));
			//Console.WriteLine("taal van (baa | bb)+ (a|b)*:\n{0}\n", expr5.getLanguage(6));
			Console.WriteLine("\n");
		}
	}
}
