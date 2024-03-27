using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace MathParserTK
{
	// Token: 0x020001BA RID: 442
	public class MathParser
	{
		// Token: 0x06000DA2 RID: 3490 RVA: 0x00078E84 File Offset: 0x00077284
		public MathParser()
		{
			try
			{
				this.decimalSeparator = char.Parse(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
			}
			catch (FormatException ex)
			{
				throw new FormatException("Error: can't read char decimal separator from system, check your regional settings.", ex);
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000790B4 File Offset: 0x000774B4
		public MathParser(char decimalSeparator)
		{
			this.decimalSeparator = decimalSeparator;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000792B0 File Offset: 0x000776B0
		public double Parse(string expression, bool isRadians = false)
		{
			this.isRadians = isRadians;
			double num;
			try
			{
				num = this.Calculate(this.ConvertToRPN(this.FormatString(expression)));
			}
			catch (DivideByZeroException ex)
			{
				throw ex;
			}
			catch (FormatException ex2)
			{
				throw ex2;
			}
			catch (InvalidOperationException ex3)
			{
				throw ex3;
			}
			catch (ArgumentOutOfRangeException ex4)
			{
				throw ex4;
			}
			catch (ArgumentException ex5)
			{
				throw ex5;
			}
			catch (Exception ex6)
			{
				throw ex6;
			}
			return num;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00079344 File Offset: 0x00077744
		private string FormatString(string expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentNullException("Expression is null or empty");
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (char c in expression)
			{
				if (c == '(')
				{
					num++;
				}
				else if (c == ')')
				{
					num--;
				}
				if (!char.IsWhiteSpace(c))
				{
					if (char.IsUpper(c))
					{
						stringBuilder.Append(char.ToLower(c));
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
			}
			if (num != 0)
			{
				throw new FormatException("Number of left and right parenthesis is not equal");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000793F8 File Offset: 0x000777F8
		private string ConvertToRPN(string expression)
		{
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			Stack<string> stack = new Stack<string>();
			while (i < expression.Length)
			{
				string text = this.LexicalAnalysisInfixNotation(expression, ref i);
				stringBuilder = this.SyntaxAnalysisInfixNotation(text, stringBuilder, stack);
			}
			while (stack.Count > 0)
			{
				if (stack.Peek()[0] != "$"[0])
				{
					throw new FormatException("Format exception, there is function without parenthesis");
				}
				stringBuilder.Append(stack.Pop());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00079488 File Offset: 0x00077888
		private string LexicalAnalysisInfixNotation(string expression, ref int pos)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(expression[pos]);
			if (this.supportedOperators.ContainsKey(stringBuilder.ToString()))
			{
				bool flag = pos == 0 || expression[pos - 1] == '(';
				pos++;
				string text = stringBuilder.ToString();
				if (text != null)
				{
					if (text == "+")
					{
						return (!flag) ? "$+" : "$un+";
					}
					if (text == "-")
					{
						return (!flag) ? "$-" : "$un-";
					}
				}
				return this.supportedOperators[stringBuilder.ToString()];
			}
			if (char.IsLetter(stringBuilder[0]) || this.supportedFunctions.ContainsKey(stringBuilder.ToString()) || this.supportedConstants.ContainsKey(stringBuilder.ToString()))
			{
				while (++pos < expression.Length && char.IsLetter(expression[pos]))
				{
					stringBuilder.Append(expression[pos]);
				}
				if (this.supportedFunctions.ContainsKey(stringBuilder.ToString()))
				{
					return this.supportedFunctions[stringBuilder.ToString()];
				}
				if (this.supportedConstants.ContainsKey(stringBuilder.ToString()))
				{
					return this.supportedConstants[stringBuilder.ToString()];
				}
				return "#0";
			}
			else
			{
				if (!char.IsDigit(stringBuilder[0]) && stringBuilder[0] != this.decimalSeparator)
				{
					throw new ArgumentException("Unknown token in expression");
				}
				if (char.IsDigit(stringBuilder[0]))
				{
					while (++pos < expression.Length && char.IsDigit(expression[pos]))
					{
						stringBuilder.Append(expression[pos]);
					}
				}
				else
				{
					stringBuilder = new StringBuilder();
				}
				if (pos < expression.Length && expression[pos] == this.decimalSeparator)
				{
					stringBuilder.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					while (++pos < expression.Length && char.IsDigit(expression[pos]))
					{
						stringBuilder.Append(expression[pos]);
					}
				}
				if (pos + 1 < expression.Length && expression[pos] == 'e' && (char.IsDigit(expression[pos + 1]) || (pos + 2 < expression.Length && (expression[pos + 1] == '+' || expression[pos + 1] == '-') && char.IsDigit(expression[pos + 2]))))
				{
					stringBuilder.Append(expression[pos++]);
					if (expression[pos] == '+' || expression[pos] == '-')
					{
						stringBuilder.Append(expression[pos++]);
					}
					while (pos < expression.Length && char.IsDigit(expression[pos]))
					{
						stringBuilder.Append(expression[pos++]);
					}
					return "#" + Convert.ToDouble(stringBuilder.ToString());
				}
				return "#" + stringBuilder.ToString();
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00079838 File Offset: 0x00077C38
		private StringBuilder SyntaxAnalysisInfixNotation(string token, StringBuilder outputString, Stack<string> stack)
		{
			if (token[0] == "#"[0])
			{
				outputString.Append(token);
			}
			else if (token[0] == "@"[0])
			{
				stack.Push(token);
			}
			else if (token == "$(")
			{
				stack.Push(token);
			}
			else if (token == "$)")
			{
				string text;
				while ((text = stack.Pop()) != "$(")
				{
					outputString.Append(text);
				}
				if (stack.Count > 0 && stack.Peek()[0] == "@"[0])
				{
					outputString.Append(stack.Pop());
				}
			}
			else
			{
				while (stack.Count > 0 && this.Priority(token, stack.Peek()))
				{
					outputString.Append(stack.Pop());
				}
				stack.Push(token);
			}
			return outputString;
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0007994E File Offset: 0x00077D4E
		private bool Priority(string token, string p)
		{
			return (!this.IsRightAssociated(token)) ? (this.GetPriority(token) <= this.GetPriority(p)) : (this.GetPriority(token) < this.GetPriority(p));
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00079984 File Offset: 0x00077D84
		private bool IsRightAssociated(string token)
		{
			return token == "$^";
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00079994 File Offset: 0x00077D94
		private int GetPriority(string token)
		{
			switch (token)
			{
			case "$(":
				return 0;
			case "$+":
			case "$-":
				return 2;
			case "$un+":
			case "$un-":
				return 6;
			case "$*":
			case "$/":
				return 4;
			case "$^":
			case "@sqrt":
			case "@random":
			case "@randomfloat":
			case "@smaller":
			case "@larger":
			case "@mod":
				return 8;
			case "@sin":
			case "@cos":
			case "@tan":
			case "@ctg":
			case "@sh":
			case "@ch":
			case "@th":
			case "@log":
			case "@ln":
			case "@exp":
			case "@absolute":
			case "@round":
			case "@ceil":
			case "@floor":
				return 10;
			}
			throw new ArgumentException("Unknown operator " + token);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00079B68 File Offset: 0x00077F68
		private double Calculate(string expression)
		{
			int i = 0;
			Stack<double> stack = new Stack<double>();
			while (i < expression.Length)
			{
				string text = this.LexicalAnalysisRPN(expression, ref i);
				stack = this.SyntaxAnalysisRPN(stack, text);
			}
			if (stack.Count > 1)
			{
				throw new ArgumentException("Excess operand");
			}
			return stack.Pop();
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00079BC0 File Offset: 0x00077FC0
		private string LexicalAnalysisRPN(string expression, ref int pos)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(expression[pos++]);
			while (pos < expression.Length && expression[pos] != "#"[0] && expression[pos] != "$"[0] && expression[pos] != "@"[0])
			{
				stringBuilder.Append(expression[pos++]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00079C60 File Offset: 0x00078060
		private Stack<double> SyntaxAnalysisRPN(Stack<double> stack, string token)
		{
			if (token[0] == "#"[0])
			{
				stack.Push(double.Parse(token.Remove(0, 1)));
			}
			else
			{
				if (this.NumberOfArguments(token) == 1)
				{
					double num = stack.Pop();
					if (token != null)
					{
						if (MathParser.<>f__switch$map2E == null)
						{
							MathParser.<>f__switch$map2E = new Dictionary<string, int>(16)
							{
								{ "$un+", 0 },
								{ "$un-", 1 },
								{ "@sqrt", 2 },
								{ "@sin", 3 },
								{ "@cos", 4 },
								{ "@tan", 5 },
								{ "@ctg", 6 },
								{ "@sh", 7 },
								{ "@ch", 8 },
								{ "@th", 9 },
								{ "@ln", 10 },
								{ "@exp", 11 },
								{ "@absolute", 12 },
								{ "@round", 13 },
								{ "@ceil", 14 },
								{ "@floor", 15 }
							};
						}
						int num2;
						if (MathParser.<>f__switch$map2E.TryGetValue(token, out num2))
						{
							double num3;
							switch (num2)
							{
							case 0:
								num3 = num;
								break;
							case 1:
								num3 = -num;
								break;
							case 2:
								num3 = Math.Sqrt(num);
								break;
							case 3:
								num3 = this.ApplyTrigFunction(new Func<double, double>(Math.Sin), num);
								break;
							case 4:
								num3 = this.ApplyTrigFunction(new Func<double, double>(Math.Cos), num);
								break;
							case 5:
								num3 = this.ApplyTrigFunction(new Func<double, double>(Math.Tan), num);
								break;
							case 6:
								num3 = 1.0 / this.ApplyTrigFunction(new Func<double, double>(Math.Tan), num);
								break;
							case 7:
								num3 = Math.Sinh(num);
								break;
							case 8:
								num3 = Math.Cosh(num);
								break;
							case 9:
								num3 = Math.Tanh(num);
								break;
							case 10:
								num3 = Math.Log(num);
								break;
							case 11:
								num3 = Math.Exp(num);
								break;
							case 12:
								num3 = Math.Abs(num);
								break;
							case 13:
								num3 = Math.Round(num);
								break;
							case 14:
								num3 = (double)Mathf.Ceil((float)num);
								break;
							case 15:
								num3 = (double)Mathf.Floor((float)num);
								break;
							case 16:
								goto IL_2C8;
							default:
								goto IL_2C8;
							}
							stack.Push(num3);
							return stack;
						}
					}
					IL_2C8:
					throw new ArgumentException("Unknown operator");
				}
				double num4 = stack.Pop();
				double num5 = stack.Pop();
				if (token != null)
				{
					if (MathParser.<>f__switch$map2F == null)
					{
						MathParser.<>f__switch$map2F = new Dictionary<string, int>(11)
						{
							{ "$+", 0 },
							{ "$-", 1 },
							{ "$*", 2 },
							{ "$/", 3 },
							{ "$^", 4 },
							{ "@log", 5 },
							{ "@random", 6 },
							{ "@randomfloat", 7 },
							{ "@mod", 8 },
							{ "@smaller", 9 },
							{ "@larger", 10 }
						};
					}
					int num2;
					if (MathParser.<>f__switch$map2F.TryGetValue(token, out num2))
					{
						double num6;
						switch (num2)
						{
						case 0:
							num6 = num5 + num4;
							break;
						case 1:
							num6 = num5 - num4;
							break;
						case 2:
							num6 = num5 * num4;
							break;
						case 3:
							if (num4 == 0.0)
							{
								throw new DivideByZeroException("Second argument is zero");
							}
							num6 = num5 / num4;
							break;
						case 4:
							num6 = Math.Pow(num5, num4);
							break;
						case 5:
							num6 = Math.Log(num4, num5);
							break;
						case 6:
							num6 = (double)((float)global::UnityEngine.Random.Range((int)num5, (int)(num4 + 1.0)));
							break;
						case 7:
							num6 = (double)global::UnityEngine.Random.Range((float)num5, (float)num4);
							break;
						case 8:
							num6 = (double)((float)num5 % (float)num4);
							break;
						case 9:
							num6 = Math.Min(num5, num4);
							break;
						case 10:
							num6 = Math.Max(num5, num4);
							break;
						case 11:
							goto IL_4AB;
						default:
							goto IL_4AB;
						}
						stack.Push(num6);
						return stack;
					}
				}
				IL_4AB:
				throw new ArgumentException("Unknown operator");
			}
			return stack;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0007A12C File Offset: 0x0007852C
		private double ApplyTrigFunction(Func<double, double> func, double arg)
		{
			if (!this.isRadians)
			{
				arg = arg * 3.141592653589793 / 180.0;
			}
			return func(arg);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0007A158 File Offset: 0x00078558
		private int NumberOfArguments(string token)
		{
			switch (token)
			{
			case "$un+":
			case "$un-":
			case "@sqrt":
			case "@tan":
			case "@sh":
			case "@ch":
			case "@th":
			case "@ln":
			case "@ctg":
			case "@sin":
			case "@cos":
			case "@exp":
			case "@absolute":
			case "@round":
			case "@ceil":
			case "@floor":
				return 1;
			case "$+":
			case "$-":
			case "$*":
			case "$/":
			case "$^":
			case "@log":
			case "@random":
			case "@randomfloat":
			case "@mod":
			case "@smaller":
			case "@larger":
				return 2;
			}
			throw new ArgumentException("Unknown operator");
		}

		// Token: 0x04000EFF RID: 3839
		private const string NumberMaker = "#";

		// Token: 0x04000F00 RID: 3840
		private const string OperatorMarker = "$";

		// Token: 0x04000F01 RID: 3841
		private const string FunctionMarker = "@";

		// Token: 0x04000F02 RID: 3842
		private const string Plus = "$+";

		// Token: 0x04000F03 RID: 3843
		private const string UnPlus = "$un+";

		// Token: 0x04000F04 RID: 3844
		private const string Minus = "$-";

		// Token: 0x04000F05 RID: 3845
		private const string UnMinus = "$un-";

		// Token: 0x04000F06 RID: 3846
		private const string Multiply = "$*";

		// Token: 0x04000F07 RID: 3847
		private const string Divide = "$/";

		// Token: 0x04000F08 RID: 3848
		private const string Degree = "$^";

		// Token: 0x04000F09 RID: 3849
		private const string LeftParent = "$(";

		// Token: 0x04000F0A RID: 3850
		private const string RightParent = "$)";

		// Token: 0x04000F0B RID: 3851
		private const string Sqrt = "@sqrt";

		// Token: 0x04000F0C RID: 3852
		private const string Sin = "@sin";

		// Token: 0x04000F0D RID: 3853
		private const string Cos = "@cos";

		// Token: 0x04000F0E RID: 3854
		private const string Tg = "@tan";

		// Token: 0x04000F0F RID: 3855
		private const string Ctg = "@ctg";

		// Token: 0x04000F10 RID: 3856
		private const string Sh = "@sh";

		// Token: 0x04000F11 RID: 3857
		private const string Ch = "@ch";

		// Token: 0x04000F12 RID: 3858
		private const string Th = "@th";

		// Token: 0x04000F13 RID: 3859
		private const string Log = "@log";

		// Token: 0x04000F14 RID: 3860
		private const string Ln = "@ln";

		// Token: 0x04000F15 RID: 3861
		private const string Exp = "@exp";

		// Token: 0x04000F16 RID: 3862
		private const string Abs = "@absolute";

		// Token: 0x04000F17 RID: 3863
		private const string Round = "@round";

		// Token: 0x04000F18 RID: 3864
		private const string Random = "@random";

		// Token: 0x04000F19 RID: 3865
		private const string RandomFloat = "@randomfloat";

		// Token: 0x04000F1A RID: 3866
		private const string Mod = "@mod";

		// Token: 0x04000F1B RID: 3867
		private const string Ceil = "@ceil";

		// Token: 0x04000F1C RID: 3868
		private const string Floor = "@floor";

		// Token: 0x04000F1D RID: 3869
		private const string Smaller = "@smaller";

		// Token: 0x04000F1E RID: 3870
		private const string Larger = "@larger";

		// Token: 0x04000F1F RID: 3871
		private readonly Dictionary<string, string> supportedOperators = new Dictionary<string, string>
		{
			{ "+", "$+" },
			{ "-", "$-" },
			{ "*", "$*" },
			{ "/", "$/" },
			{ "^", "$^" },
			{ "(", "$(" },
			{ ")", "$)" }
		};

		// Token: 0x04000F20 RID: 3872
		private readonly Dictionary<string, string> supportedFunctions = new Dictionary<string, string>
		{
			{ "sqrt", "@sqrt" },
			{ "sin", "@sin" },
			{ "cos", "@cos" },
			{ "tan", "@tan" },
			{ "log", "@log" },
			{ "exp", "@exp" },
			{ "absolute", "@absolute" },
			{ "round", "@round" },
			{ "random", "@random" },
			{ "randomfloat", "@randomfloat" },
			{ "mod", "@mod" },
			{ "ceil", "@ceil" },
			{ "floor", "@floor" },
			{ "smaller", "@smaller" },
			{ "larger", "@larger" }
		};

		// Token: 0x04000F21 RID: 3873
		private readonly Dictionary<string, string> supportedConstants = new Dictionary<string, string>
		{
			{
				"pi",
				"#" + 3.141592653589793.ToString()
			},
			{
				"e",
				"#" + 2.718281828459045.ToString()
			}
		};

		// Token: 0x04000F22 RID: 3874
		private readonly char decimalSeparator;

		// Token: 0x04000F23 RID: 3875
		private bool isRadians;
	}
}
