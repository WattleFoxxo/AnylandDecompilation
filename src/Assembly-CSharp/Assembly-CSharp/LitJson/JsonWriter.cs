using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x02000037 RID: 55
	public class JsonWriter
	{
		// Token: 0x0600021A RID: 538 RVA: 0x000086D4 File Offset: 0x00006AD4
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000086FE File Offset: 0x00006AFE
		public JsonWriter(StringBuilder sb)
			: this(new StringWriter(sb))
		{
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000870C File Offset: 0x00006B0C
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00008732 File Offset: 0x00006B32
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000873A File Offset: 0x00006B3A
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00008758 File Offset: 0x00006B58
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00008760 File Offset: 0x00006B60
		public string IndentString
		{
			get
			{
				return this.indent_string;
			}
			set
			{
				this.indent_string = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00008769 File Offset: 0x00006B69
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00008771 File Offset: 0x00006B71
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000877A File Offset: 0x00006B7A
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00008782 File Offset: 0x00006B82
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0000878A File Offset: 0x00006B8A
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008794 File Offset: 0x00006B94
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000088F8 File Offset: 0x00006CF8
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.indent_string = " ";
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008968 File Offset: 0x00006D68
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000089B5 File Offset: 0x00006DB5
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000089D8 File Offset: 0x00006DD8
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(this.indent_string);
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00008A34 File Offset: 0x00006E34
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00008A40 File Offset: 0x00006E40
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00008AAC File Offset: 0x00006EAC
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						if (str[i] >= ' ' && str[i] <= '~')
						{
							this.writer.Write(str[i]);
						}
						else
						{
							JsonWriter.IntToHex((int)str[i], this.hex_seq);
							this.writer.Write("\\u");
							this.writer.Write(this.hex_seq);
						}
					}
					else
					{
						this.writer.Write('\\');
						this.writer.Write(str[i]);
					}
					break;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				}
			}
			this.writer.Write('"');
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00008C27 File Offset: 0x00007027
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00008C47 File Offset: 0x00007047
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00008C68 File Offset: 0x00007068
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008CC6 File Offset: 0x000070C6
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put((!boolean) ? "false" : "true");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00008CFC File Offset: 0x000070FC
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00008D28 File Offset: 0x00007128
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00008D8D File Offset: 0x0000718D
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00008DB9 File Offset: 0x000071B9
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00008DE5 File Offset: 0x000071E5
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00008E1D File Offset: 0x0000721D
		[CLSCompliant(false)]
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00008E4C File Offset: 0x0000724C
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00008EC0 File Offset: 0x000072C0
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00008F14 File Offset: 0x00007314
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00008F88 File Offset: 0x00007388
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00008FDC File Offset: 0x000073DC
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				for (int i = this.context.Padding - property_name.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x040000E5 RID: 229
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x040000E6 RID: 230
		private WriterContext context;

		// Token: 0x040000E7 RID: 231
		private Stack<WriterContext> ctx_stack;

		// Token: 0x040000E8 RID: 232
		private bool has_reached_end;

		// Token: 0x040000E9 RID: 233
		private char[] hex_seq;

		// Token: 0x040000EA RID: 234
		private int indentation;

		// Token: 0x040000EB RID: 235
		private int indent_value;

		// Token: 0x040000EC RID: 236
		private string indent_string;

		// Token: 0x040000ED RID: 237
		private StringBuilder inst_string_builder;

		// Token: 0x040000EE RID: 238
		private bool pretty_print;

		// Token: 0x040000EF RID: 239
		private bool validate;

		// Token: 0x040000F0 RID: 240
		private TextWriter writer;
	}
}
