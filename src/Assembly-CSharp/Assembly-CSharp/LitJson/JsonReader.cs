using System;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x02000034 RID: 52
	public class JsonReader
	{
		// Token: 0x06000202 RID: 514 RVA: 0x00007D43 File Offset: 0x00006143
		static JsonReader()
		{
			JsonReader.PopulateParseTable();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00007D4A File Offset: 0x0000614A
		public JsonReader(string json_text)
			: this(new StringReader(json_text), true)
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007D59 File Offset: 0x00006159
		public JsonReader(TextReader reader)
			: this(reader, false)
		{
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007D64 File Offset: 0x00006164
		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.skip_non_members = true;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00007DF7 File Offset: 0x000061F7
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00007E04 File Offset: 0x00006204
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00007E12 File Offset: 0x00006212
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00007E1F File Offset: 0x0000621F
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00007E2D File Offset: 0x0000622D
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00007E35 File Offset: 0x00006235
		public bool SkipNonMembers
		{
			get
			{
				return this.skip_non_members;
			}
			set
			{
				this.skip_non_members = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00007E3E File Offset: 0x0000623E
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00007E46 File Offset: 0x00006246
		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00007E4E File Offset: 0x0000624E
		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00007E56 File Offset: 0x00006256
		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00007E60 File Offset: 0x00006260
		private static void PopulateParseTable()
		{
			JsonReader.parse_table = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(ParserToken.Array);
			JsonReader.TableAddCol(ParserToken.Array, 91, new int[] { 91, 65549 });
			JsonReader.TableAddRow(ParserToken.ArrayPrime);
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 34, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 91, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 93, new int[] { 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 123, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65537, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65538, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65539, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65540, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddRow(ParserToken.Object);
			JsonReader.TableAddCol(ParserToken.Object, 123, new int[] { 123, 65545 });
			JsonReader.TableAddRow(ParserToken.ObjectPrime);
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 34, new int[] { 65546, 65547, 125 });
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 125, new int[] { 125 });
			JsonReader.TableAddRow(ParserToken.Pair);
			JsonReader.TableAddCol(ParserToken.Pair, 34, new int[] { 65552, 58, 65550 });
			JsonReader.TableAddRow(ParserToken.PairRest);
			JsonReader.TableAddCol(ParserToken.PairRest, 44, new int[] { 44, 65546, 65547 });
			JsonReader.TableAddCol(ParserToken.PairRest, 125, new int[] { 65554 });
			JsonReader.TableAddRow(ParserToken.String);
			JsonReader.TableAddCol(ParserToken.String, 34, new int[] { 34, 65541, 34 });
			JsonReader.TableAddRow(ParserToken.Text);
			JsonReader.TableAddCol(ParserToken.Text, 91, new int[] { 65548 });
			JsonReader.TableAddCol(ParserToken.Text, 123, new int[] { 65544 });
			JsonReader.TableAddRow(ParserToken.Value);
			JsonReader.TableAddCol(ParserToken.Value, 34, new int[] { 65552 });
			JsonReader.TableAddCol(ParserToken.Value, 91, new int[] { 65548 });
			JsonReader.TableAddCol(ParserToken.Value, 123, new int[] { 65544 });
			JsonReader.TableAddCol(ParserToken.Value, 65537, new int[] { 65537 });
			JsonReader.TableAddCol(ParserToken.Value, 65538, new int[] { 65538 });
			JsonReader.TableAddCol(ParserToken.Value, 65539, new int[] { 65539 });
			JsonReader.TableAddCol(ParserToken.Value, 65540, new int[] { 65540 });
			JsonReader.TableAddRow(ParserToken.ValueRest);
			JsonReader.TableAddCol(ParserToken.ValueRest, 44, new int[] { 44, 65550, 65551 });
			JsonReader.TableAddCol(ParserToken.ValueRest, 93, new int[] { 65554 });
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000081D9 File Offset: 0x000065D9
		private static void TableAddCol(ParserToken row, int col, params int[] symbols)
		{
			JsonReader.parse_table[(int)row].Add(col, symbols);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000081ED File Offset: 0x000065ED
		private static void TableAddRow(ParserToken rule)
		{
			JsonReader.parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00008200 File Offset: 0x00006600
		private void ProcessNumber(string number)
		{
			double num;
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, out num))
			{
				this.token = JsonToken.Double;
				this.token_value = num;
				return;
			}
			int num2;
			if (int.TryParse(number, out num2))
			{
				this.token = JsonToken.Int;
				this.token_value = num2;
				return;
			}
			long num3;
			if (long.TryParse(number, out num3))
			{
				this.token = JsonToken.Long;
				this.token_value = num3;
				return;
			}
			ulong num4;
			if (ulong.TryParse(number, out num4))
			{
				this.token = JsonToken.Long;
				this.token_value = num4;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000082D0 File Offset: 0x000066D0
		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
			}
			else if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
			}
			else if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
			}
			else if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
			}
			else if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
				}
				else
				{
					if (this.token == JsonToken.None)
					{
						this.token = JsonToken.String;
					}
					this.parser_in_string = true;
				}
			}
			else if (this.current_symbol == 65541)
			{
				this.token_value = this.lexer.StringValue;
			}
			else if (this.current_symbol == 65539)
			{
				this.token = JsonToken.Boolean;
				this.token_value = false;
				this.parser_return = true;
			}
			else if (this.current_symbol == 65540)
			{
				this.token = JsonToken.Null;
				this.parser_return = true;
			}
			else if (this.current_symbol == 65537)
			{
				this.ProcessNumber(this.lexer.StringValue);
				this.parser_return = true;
			}
			else if (this.current_symbol == 65546)
			{
				this.token = JsonToken.PropertyName;
			}
			else if (this.current_symbol == 65538)
			{
				this.token = JsonToken.Boolean;
				this.token_value = true;
				this.parser_return = true;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00008498 File Offset: 0x00006898
		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000084E8 File Offset: 0x000068E8
		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				this.reader.Close();
			}
			this.reader = null;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008524 File Offset: 0x00006924
		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] array;
					try
					{
						array = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException ex)
					{
						throw new JsonException((ParserToken)this.current_input, ex);
					}
					if (array[0] != 65554)
					{
						for (int i = array.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(array[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		// Token: 0x040000CB RID: 203
		private static IDictionary<int, IDictionary<int, int[]>> parse_table;

		// Token: 0x040000CC RID: 204
		private Stack<int> automaton_stack;

		// Token: 0x040000CD RID: 205
		private int current_input;

		// Token: 0x040000CE RID: 206
		private int current_symbol;

		// Token: 0x040000CF RID: 207
		private bool end_of_json;

		// Token: 0x040000D0 RID: 208
		private bool end_of_input;

		// Token: 0x040000D1 RID: 209
		private Lexer lexer;

		// Token: 0x040000D2 RID: 210
		private bool parser_in_string;

		// Token: 0x040000D3 RID: 211
		private bool parser_return;

		// Token: 0x040000D4 RID: 212
		private bool read_started;

		// Token: 0x040000D5 RID: 213
		private TextReader reader;

		// Token: 0x040000D6 RID: 214
		private bool reader_is_owned;

		// Token: 0x040000D7 RID: 215
		private bool skip_non_members;

		// Token: 0x040000D8 RID: 216
		private object token_value;

		// Token: 0x040000D9 RID: 217
		private JsonToken token;
	}
}
