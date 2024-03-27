using System;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x02000039 RID: 57
	internal class Lexer
	{
		// Token: 0x0600023E RID: 574 RVA: 0x00009095 File Offset: 0x00007495
		static Lexer()
		{
			Lexer.PopulateFsmTables();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000909C File Offset: 0x0000749C
		public Lexer(TextReader reader)
		{
			this.allow_comments = true;
			this.allow_single_quoted_strings = true;
			this.input_buffer = 0;
			this.string_buffer = new StringBuilder(128);
			this.state = 1;
			this.end_of_input = false;
			this.reader = reader;
			this.fsm_context = new FsmContext();
			this.fsm_context.L = this;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00009100 File Offset: 0x00007500
		// (set) Token: 0x06000241 RID: 577 RVA: 0x00009108 File Offset: 0x00007508
		public bool AllowComments
		{
			get
			{
				return this.allow_comments;
			}
			set
			{
				this.allow_comments = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00009111 File Offset: 0x00007511
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00009119 File Offset: 0x00007519
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.allow_single_quoted_strings;
			}
			set
			{
				this.allow_single_quoted_strings = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00009122 File Offset: 0x00007522
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000912A File Offset: 0x0000752A
		public int Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00009132 File Offset: 0x00007532
		public string StringValue
		{
			get
			{
				return this.string_value;
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000913C File Offset: 0x0000753C
		private static int HexValue(int digit)
		{
			switch (digit)
			{
			case 65:
				break;
			case 66:
				return 11;
			case 67:
				return 12;
			case 68:
				return 13;
			case 69:
				return 14;
			case 70:
				return 15;
			default:
				switch (digit)
				{
				case 97:
					break;
				case 98:
					return 11;
				case 99:
					return 12;
				case 100:
					return 13;
				case 101:
					return 14;
				case 102:
					return 15;
				default:
					return digit - 48;
				}
				break;
			}
			return 10;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000091A8 File Offset: 0x000075A8
		private static void PopulateFsmTables()
		{
			Lexer.StateHandler[] array = new Lexer.StateHandler[28];
			array[0] = new Lexer.StateHandler(Lexer.State1);
			array[1] = new Lexer.StateHandler(Lexer.State2);
			array[2] = new Lexer.StateHandler(Lexer.State3);
			array[3] = new Lexer.StateHandler(Lexer.State4);
			array[4] = new Lexer.StateHandler(Lexer.State5);
			array[5] = new Lexer.StateHandler(Lexer.State6);
			array[6] = new Lexer.StateHandler(Lexer.State7);
			array[7] = new Lexer.StateHandler(Lexer.State8);
			array[8] = new Lexer.StateHandler(Lexer.State9);
			array[9] = new Lexer.StateHandler(Lexer.State10);
			array[10] = new Lexer.StateHandler(Lexer.State11);
			array[11] = new Lexer.StateHandler(Lexer.State12);
			array[12] = new Lexer.StateHandler(Lexer.State13);
			array[13] = new Lexer.StateHandler(Lexer.State14);
			array[14] = new Lexer.StateHandler(Lexer.State15);
			array[15] = new Lexer.StateHandler(Lexer.State16);
			array[16] = new Lexer.StateHandler(Lexer.State17);
			array[17] = new Lexer.StateHandler(Lexer.State18);
			array[18] = new Lexer.StateHandler(Lexer.State19);
			array[19] = new Lexer.StateHandler(Lexer.State20);
			array[20] = new Lexer.StateHandler(Lexer.State21);
			array[21] = new Lexer.StateHandler(Lexer.State22);
			array[22] = new Lexer.StateHandler(Lexer.State23);
			array[23] = new Lexer.StateHandler(Lexer.State24);
			array[24] = new Lexer.StateHandler(Lexer.State25);
			array[25] = new Lexer.StateHandler(Lexer.State26);
			array[26] = new Lexer.StateHandler(Lexer.State27);
			array[27] = new Lexer.StateHandler(Lexer.State28);
			Lexer.fsm_handler_table = array;
			Lexer.fsm_return_table = new int[]
			{
				65542, 0, 65537, 65537, 0, 65537, 0, 65537, 0, 0,
				65538, 0, 0, 0, 65539, 0, 0, 65540, 65541, 65542,
				0, 0, 65541, 65542, 0, 0, 0, 0
			};
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000956C File Offset: 0x0000796C
		private static char ProcessEscChar(int esc_char)
		{
			switch (esc_char)
			{
			case 114:
				return '\r';
			default:
				if (esc_char == 34 || esc_char == 39 || esc_char == 47 || esc_char == 92)
				{
					return Convert.ToChar(esc_char);
				}
				if (esc_char == 98)
				{
					return '\b';
				}
				if (esc_char == 102)
				{
					return '\f';
				}
				if (esc_char != 110)
				{
					return '?';
				}
				return '\n';
			case 116:
				return '\t';
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000095E4 File Offset: 0x000079E4
		private static bool State1(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char != 32 && (ctx.L.input_char < 9 || ctx.L.input_char > 13))
				{
					if (ctx.L.input_char >= 49 && ctx.L.input_char <= 57)
					{
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 3;
						return true;
					}
					int num = ctx.L.input_char;
					switch (num)
					{
					case 44:
						break;
					case 45:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 2;
						return true;
					default:
						switch (num)
						{
						case 91:
						case 93:
							break;
						default:
							switch (num)
							{
							case 123:
							case 125:
								break;
							default:
								if (num == 34)
								{
									ctx.NextState = 19;
									ctx.Return = true;
									return true;
								}
								if (num != 39)
								{
									if (num != 58)
									{
										if (num == 102)
										{
											ctx.NextState = 12;
											return true;
										}
										if (num == 110)
										{
											ctx.NextState = 16;
											return true;
										}
										if (num != 116)
										{
											return false;
										}
										ctx.NextState = 9;
										return true;
									}
								}
								else
								{
									if (!ctx.L.allow_single_quoted_strings)
									{
										return false;
									}
									ctx.L.input_char = 34;
									ctx.NextState = 23;
									ctx.Return = true;
									return true;
								}
								break;
							}
							break;
						}
						break;
					case 47:
						if (!ctx.L.allow_comments)
						{
							return false;
						}
						ctx.NextState = 25;
						return true;
					case 48:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 4;
						return true;
					}
					ctx.NextState = 1;
					ctx.Return = true;
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000097F0 File Offset: 0x00007BF0
		private static bool State2(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 49 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 3;
				return true;
			}
			int num = ctx.L.input_char;
			if (num != 48)
			{
				return false;
			}
			ctx.L.string_buffer.Append((char)ctx.L.input_char);
			ctx.NextState = 4;
			return true;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00009894 File Offset: 0x00007C94
		private static bool State3(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					switch (num)
					{
					case 44:
						break;
					default:
						if (num != 69)
						{
							if (num == 93)
							{
								break;
							}
							if (num != 101)
							{
								if (num != 125)
								{
									return false;
								}
								break;
							}
						}
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 7;
						return true;
					case 46:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 5;
						return true;
					}
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000099F4 File Offset: 0x00007DF4
		private static bool State4(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
			{
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			int num = ctx.L.input_char;
			switch (num)
			{
			case 44:
				break;
			default:
				if (num != 69)
				{
					if (num == 93)
					{
						break;
					}
					if (num != 101)
					{
						if (num != 125)
						{
							return false;
						}
						break;
					}
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 7;
				return true;
			case 46:
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 5;
				return true;
			}
			ctx.L.UngetChar();
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00009B04 File Offset: 0x00007F04
		private static bool State5(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 6;
				return true;
			}
			return false;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00009B68 File Offset: 0x00007F68
		private static bool State6(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num != 44)
					{
						if (num != 69)
						{
							if (num == 93)
							{
								goto IL_CA;
							}
							if (num != 101)
							{
								if (num != 125)
								{
									return false;
								}
								goto IL_CA;
							}
						}
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 7;
						return true;
					}
					IL_CA:
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009C94 File Offset: 0x00008094
		private static bool State7(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 8;
				return true;
			}
			int num = ctx.L.input_char;
			if (num != 43 && num != 45)
			{
				return false;
			}
			ctx.L.string_buffer.Append((char)ctx.L.input_char);
			ctx.NextState = 8;
			return true;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009D40 File Offset: 0x00008140
		private static bool State8(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num != 44 && num != 93 && num != 125)
					{
						return false;
					}
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009E38 File Offset: 0x00008238
		private static bool State9(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 114)
			{
				return false;
			}
			ctx.NextState = 10;
			return true;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009E78 File Offset: 0x00008278
		private static bool State10(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 117)
			{
				return false;
			}
			ctx.NextState = 11;
			return true;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009EB8 File Offset: 0x000082B8
		private static bool State11(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 101)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009EFC File Offset: 0x000082FC
		private static bool State12(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 97)
			{
				return false;
			}
			ctx.NextState = 13;
			return true;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00009F3C File Offset: 0x0000833C
		private static bool State13(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.NextState = 14;
			return true;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00009F7C File Offset: 0x0000837C
		private static bool State14(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 115)
			{
				return false;
			}
			ctx.NextState = 15;
			return true;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00009FBC File Offset: 0x000083BC
		private static bool State15(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 101)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000A000 File Offset: 0x00008400
		private static bool State16(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 117)
			{
				return false;
			}
			ctx.NextState = 17;
			return true;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000A040 File Offset: 0x00008440
		private static bool State17(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.NextState = 18;
			return true;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000A080 File Offset: 0x00008480
		private static bool State18(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000A0C4 File Offset: 0x000084C4
		private static bool State19(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				int num = ctx.L.input_char;
				if (num == 34)
				{
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 20;
					return true;
				}
				if (num == 92)
				{
					ctx.StateStack = 19;
					ctx.NextState = 21;
					return true;
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
			}
			return true;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000A158 File Offset: 0x00008558
		private static bool State20(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 34)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000A19C File Offset: 0x0000859C
		private static bool State21(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			switch (num)
			{
			case 114:
			case 116:
				break;
			default:
				if (num != 34 && num != 39 && num != 47 && num != 92 && num != 98 && num != 102 && num != 110)
				{
					return false;
				}
				break;
			case 117:
				ctx.NextState = 22;
				return true;
			}
			ctx.L.string_buffer.Append(Lexer.ProcessEscChar(ctx.L.input_char));
			ctx.NextState = ctx.StateStack;
			return true;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000A254 File Offset: 0x00008654
		private static bool State22(FsmContext ctx)
		{
			int num = 0;
			int num2 = 4096;
			ctx.L.unichar = 0;
			while (ctx.L.GetChar())
			{
				if ((ctx.L.input_char < 48 || ctx.L.input_char > 57) && (ctx.L.input_char < 65 || ctx.L.input_char > 70) && (ctx.L.input_char < 97 || ctx.L.input_char > 102))
				{
					return false;
				}
				ctx.L.unichar += Lexer.HexValue(ctx.L.input_char) * num2;
				num++;
				num2 /= 16;
				if (num == 4)
				{
					ctx.L.string_buffer.Append(Convert.ToChar(ctx.L.unichar));
					ctx.NextState = ctx.StateStack;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000A364 File Offset: 0x00008764
		private static bool State23(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				int num = ctx.L.input_char;
				if (num == 39)
				{
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 24;
					return true;
				}
				if (num == 92)
				{
					ctx.StateStack = 23;
					ctx.NextState = 21;
					return true;
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
			}
			return true;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000A3F8 File Offset: 0x000087F8
		private static bool State24(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 39)
			{
				return false;
			}
			ctx.L.input_char = 34;
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000A448 File Offset: 0x00008848
		private static bool State25(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num == 42)
			{
				ctx.NextState = 27;
				return true;
			}
			if (num != 47)
			{
				return false;
			}
			ctx.NextState = 26;
			return true;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000A497 File Offset: 0x00008897
		private static bool State26(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char == 10)
				{
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000A4CA File Offset: 0x000088CA
		private static bool State27(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char == 42)
				{
					ctx.NextState = 28;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000A500 File Offset: 0x00008900
		private static bool State28(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char != 42)
				{
					if (ctx.L.input_char == 47)
					{
						ctx.NextState = 1;
						return true;
					}
					ctx.NextState = 27;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000A560 File Offset: 0x00008960
		private bool GetChar()
		{
			if ((this.input_char = this.NextChar()) != -1)
			{
				return true;
			}
			this.end_of_input = true;
			return false;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000A58C File Offset: 0x0000898C
		private int NextChar()
		{
			if (this.input_buffer != 0)
			{
				int num = this.input_buffer;
				this.input_buffer = 0;
				return num;
			}
			return this.reader.Read();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000A5C0 File Offset: 0x000089C0
		public bool NextToken()
		{
			this.fsm_context.Return = false;
			for (;;)
			{
				Lexer.StateHandler stateHandler = Lexer.fsm_handler_table[this.state - 1];
				if (!stateHandler(this.fsm_context))
				{
					break;
				}
				if (this.end_of_input)
				{
					return false;
				}
				if (this.fsm_context.Return)
				{
					goto Block_3;
				}
				this.state = this.fsm_context.NextState;
			}
			throw new JsonException(this.input_char);
			Block_3:
			this.string_value = this.string_buffer.ToString();
			this.string_buffer.Remove(0, this.string_buffer.Length);
			this.token = Lexer.fsm_return_table[this.state - 1];
			if (this.token == 65542)
			{
				this.token = this.input_char;
			}
			this.state = this.fsm_context.NextState;
			return true;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000A6A3 File Offset: 0x00008AA3
		private void UngetChar()
		{
			this.input_buffer = this.input_char;
		}

		// Token: 0x040000F5 RID: 245
		private static int[] fsm_return_table;

		// Token: 0x040000F6 RID: 246
		private static Lexer.StateHandler[] fsm_handler_table;

		// Token: 0x040000F7 RID: 247
		private bool allow_comments;

		// Token: 0x040000F8 RID: 248
		private bool allow_single_quoted_strings;

		// Token: 0x040000F9 RID: 249
		private bool end_of_input;

		// Token: 0x040000FA RID: 250
		private FsmContext fsm_context;

		// Token: 0x040000FB RID: 251
		private int input_buffer;

		// Token: 0x040000FC RID: 252
		private int input_char;

		// Token: 0x040000FD RID: 253
		private TextReader reader;

		// Token: 0x040000FE RID: 254
		private int state;

		// Token: 0x040000FF RID: 255
		private StringBuilder string_buffer;

		// Token: 0x04000100 RID: 256
		private string string_value;

		// Token: 0x04000101 RID: 257
		private int token;

		// Token: 0x04000102 RID: 258
		private int unichar;

		// Token: 0x0200003A RID: 58
		// (Invoke) Token: 0x0600026B RID: 619
		private delegate bool StateHandler(FsmContext ctx);
	}
}
