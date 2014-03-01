using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automaat
{

	public class AutomatonInnerResultHolder : Tuple<SortedSet<Transition<String>>, int, int>
	{
		public AutomatonInnerResultHolder(SortedSet<Transition<String>> set, int i, int j) : base(set,i,j)
		{
			
		}
	}

	public class CompareByLength : IComparer<String>
	{
		public int Compare(String str1, String str2)
		{
			if (str1.Length == str2.Length) {
				return str1.CompareTo(str2);
			} else {
				return str1.Length - str2.Length;
			}
		}
	}

	public class Regex
	{
		Operator opr;
		String terminals;
		CompareByLength comperator = new CompareByLength();

		// De mogelijke operatoren voor een reguliere expressie (+, *, |, .) 
		// Daarnaast ook een operator definitie voor 1 keer repeteren (default)
		public enum Operator { PLUS, STAR, OR, DOT, ONE }

		Regex left;
		Regex right;

		public Regex()
		{
			opr = Operator.ONE;
			terminals = "";
			left = null;
			right = null;
		}
		public Regex(String regex)
		{
			opr = Operator.ONE;
			terminals = regex;
			left = null;
			right = null;
		}

		public Regex Plus()
		{
			var result = new Regex();
			result.opr = Operator.PLUS;
			result.left = this;
			return result;
		}
		public Regex Star()
		{
			var result = new Regex();
			result.opr = Operator.STAR;
			result.left = this;
			return result;
		}
		public Regex Or(Regex regex)
		{
			var result = new Regex();
			result.opr = Operator.OR;
			result.left = this;
			result.right = regex;
			return result;
		}
		public Regex Dot(Regex regex)
		{
			var result = new Regex();
			result.opr = Operator.DOT;
			result.left = this;
			result.right = regex;
			return result;
		}

		public Taal GetLanguage(int maxSteps)
		{
			var emptyLanguage = new Taal(comperator);
			var languageResult = new Taal(comperator);

			Taal languageLeft, languageRight;

			if (maxSteps < 1) return emptyLanguage;

			switch (opr)
			{
				case Operator.ONE:
				{
					languageResult.Add(terminals);
					break;
				}
				case Operator.OR:
				{
					languageLeft = left == null ? emptyLanguage : left.GetLanguage(maxSteps - 1);
					languageRight = right == null ? emptyLanguage : right.GetLanguage(maxSteps - 1);

					//languageResult.addAll (languageLeft);
					foreach (var item in languageLeft)
					{
						languageResult.Add(item);
					}

					//languageResult.addAll (languageRight);
					foreach (var item in languageRight)
					{
						languageResult.Add(item);
					}
					break;
				}
				case Operator.DOT:
				{
					languageLeft = left == null ? emptyLanguage : left.GetLanguage(maxSteps - 1);
					languageRight = right == null ? emptyLanguage : right.GetLanguage(maxSteps - 1);
					foreach (var s1 in languageLeft)
					{
						foreach (var s2 in languageRight)
						{
							languageResult.Add(s1 + s2);
						}
					}
					break;
				}
				// STAR(*) en PLUS(+) kunnen we bijna op dezelfde manier uitwerken:
				case Operator.STAR:
				case Operator.PLUS:
				{
					languageLeft = left == null ? emptyLanguage : left.GetLanguage(maxSteps - 1);

					//languageResult.addAll(languageLeft);
					foreach (var item in languageLeft)
					{
						languageResult.Add(item);
					}
					
					for (int i = 1; i < maxSteps; i++)
					{
						var languageTemp = new HashSet<String>(languageResult);
						foreach (var s1 in languageLeft)
						{
							foreach (var s2 in languageTemp)
							{
								languageResult.Add(s1 + s2);
							}
						}
					}
					if (this.opr == Operator.STAR)
					{
						languageResult.Add("");
					}
					break;
				}
				
				default:
				{
					Console.WriteLine("getLanguage is nog niet gedefinieerd voor de operator: " + this.opr);
					break;
				}
			}

			return languageResult;
		}

		public Automaton<String> GetAutomaton()
		{
			char[] alphabet = { 'a', 'b' };
			var automaton = new Automaton<String>(alphabet);

			var transitions = GetAutomatonInner(this, 0);

			foreach(var item in transitions.Item1)
			{
				automaton.AddTransition(item);
			}

			automaton.DefineAsStartState("q" + transitions.Item2);

			automaton.DefineAsFinalState("q" + transitions.Item3);
			//automaton.DefineAsFinalState("q6");

			return automaton;
		}
		public static AutomatonInnerResultHolder GetAutomatonInner(Regex subRegex, int currentState)
		{
			var transitions = new SortedSet<Transition<String>>();
			var transitionsTerminals = new SortedSet<Transition<String>>();
			var transitionsLeft = new SortedSet<Transition<String>>();
			var transitionsRight = new SortedSet<Transition<String>>();

			int state = currentState;
			int beginState = currentState;
			int end_left = 0;
			int begin_left = 0;
			int end_right = 0;
			int begin_right = 0;

			if (subRegex.left != null)
			{
				var res = GetAutomatonInner(subRegex.left, state);
				transitionsLeft = res.Item1;
				begin_left = res.Item2;
				end_left = res.Item3;
			}

			state += (end_left);

			if (subRegex.right != null)
			{
				var res = GetAutomatonInner(subRegex.right, state + 1);
				transitionsRight = res.Item1;
				begin_right = res.Item2;
				end_right = res.Item3;
			}
			state += end_right;

			var chars = subRegex.terminals.ToCharArray();
			for (int i = 0; i < chars.Length; i++)
			{
				if (chars[i] == Transition<String>.EPSILON)
				{
					//TODO - mogelijkheid inbakken voor epsilons in regex
					transitionsTerminals.Add(new Transition<String>("q" + state, chars[i], "q" + (++state)));
				}
				else
				{
					transitionsTerminals.Add(new Transition<String>("q" + state, chars[i], "q" + (++state)));
				}
			}

			foreach (var item in transitionsLeft)
			{
				transitions.Add(item);
			}
			foreach (var item in transitionsRight)
			{
				transitions.Add(item);
			}
			foreach (var item in transitionsTerminals)
			{
				transitions.Add(item);
			}

			switch (subRegex.opr)
			{
				case Operator.PLUS:
				case Operator.STAR:
					{
						//PLUS - optional repeat eromheen
						var trLeft = new Transition<String>("q" + (state + 1), "q" + begin_left);
						var trRight = new Transition<String>("q" + state, "q" + (state + 2));
						transitions.Add(trLeft);
						transitions.Add(trRight);

						var plus = new Transition<String>("q" + state, "q" + beginState);
						transitions.Add(plus);

						beginState = state + 1;
						state += 2;

						//STAR - repeat eromheen, maar ook een bypas
						if (subRegex.opr == Operator.STAR)
						{
							var star = new Transition<String>("q" + beginState, "q" + state);
							transitions.Add(star);
						}
						break;
					}
				case Operator.OR:
					{
						//Nieuwe begin state maken en deze met epsilons laten springen naar de rechtste
						var trLeft = new Transition<String>("q" + state, "q" + begin_left);		//transitionsLeft.First<Transition<String>>().FromState
						var trRight = new Transition<String>("q" + state, "q" + begin_right);
						transitions.Add(trLeft);
						transitions.Add(trRight);
						beginState = state;

						int endState = beginState + 1;
						
						//transitions.Where(t => transitionsRight.First().Equals(t)).First().FromState = "q"+ state;

						trLeft = new Transition<String>("q" + end_left, "q" + endState);	//transitionsLeft.Last<Transition<String>>().FromState
						trRight = new Transition<String>("q"+ end_right, "q" + endState);
						transitions.Add(trLeft);
						transitions.Add(trRight);
						state = endState;

						break;
					}
				case Operator.DOT:
					{
						//gewoon achter elkaar zetten (append)
						var tran = new Transition<String>("q" + end_left, "q" + begin_right);
						transitions.Add(tran);
						break;
					}
				case Operator.ONE:
					{
						//Unknown operand
						break;
					}
				default:
					{
						//Zou niet mogen kunnen gebeuren, nooit niet!
						break;
					}
			}

			return new AutomatonInnerResultHolder(transitions, beginState, state);
		}
	}
}