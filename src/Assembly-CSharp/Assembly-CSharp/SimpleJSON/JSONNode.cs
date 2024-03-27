using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020001C9 RID: 457
	public abstract class JSONNode
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0007E9F8 File Offset: 0x0007CDF8
		public virtual IEnumerable<string> Keys
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000171 RID: 369
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000172 RID: 370
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0007EA1E File Offset: 0x0007CE1E
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x0007EA25 File Offset: 0x0007CE25
		public virtual string Value
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0007EA27 File Offset: 0x0007CE27
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x0007EA2A File Offset: 0x0007CE2A
		public virtual bool IsNumber
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0007EA2D File Offset: 0x0007CE2D
		public virtual bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x0007EA30 File Offset: 0x0007CE30
		public virtual bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0007EA33 File Offset: 0x0007CE33
		public virtual bool IsNull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x0007EA36 File Offset: 0x0007CE36
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0007EA39 File Offset: 0x0007CE39
		public virtual bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0007EA3C File Offset: 0x0007CE3C
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0007EA3E File Offset: 0x0007CE3E
		public virtual void Add(JSONNode aItem)
		{
			this.Add(string.Empty, aItem);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0007EA4C File Offset: 0x0007CE4C
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0007EA4F File Offset: 0x0007CE4F
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0007EA52 File Offset: 0x0007CE52
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0007EA58 File Offset: 0x0007CE58
		public virtual IEnumerable<JSONNode> Children
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x0007EA74 File Offset: 0x0007CE74
		public IEnumerable<JSONNode> DeepChildren
		{
			get
			{
				foreach (JSONNode C in this.Children)
				{
					foreach (JSONNode D in C.DeepChildren)
					{
						yield return D;
					}
				}
				yield break;
			}
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0007EA98 File Offset: 0x0007CE98
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, 0, JSONTextMode.Compact);
			return stringBuilder.ToString();
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0007EABC File Offset: 0x0007CEBC
		public virtual string ToString(int aIndent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, aIndent, JSONTextMode.Indent);
			return stringBuilder.ToString();
		}

		// Token: 0x06000E08 RID: 3592
		internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000E09 RID: 3593
		public abstract JSONNodeType Tag { get; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0007EAE0 File Offset: 0x0007CEE0
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x0007EB14 File Offset: 0x0007CF14
		public virtual double AsDouble
		{
			get
			{
				double num = 0.0;
				if (double.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0007EB29 File Offset: 0x0007CF29
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0007EB32 File Offset: 0x0007CF32
		public virtual int AsInt
		{
			get
			{
				return (int)this.AsDouble;
			}
			set
			{
				this.AsDouble = (double)value;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0007EB3C File Offset: 0x0007CF3C
		// (set) Token: 0x06000E0F RID: 3599 RVA: 0x0007EB45 File Offset: 0x0007CF45
		public virtual float AsFloat
		{
			get
			{
				return (float)this.AsDouble;
			}
			set
			{
				this.AsDouble = (double)value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0007EB50 File Offset: 0x0007CF50
		// (set) Token: 0x06000E11 RID: 3601 RVA: 0x0007EB81 File Offset: 0x0007CF81
		public virtual bool AsBool
		{
			get
			{
				bool flag = false;
				if (bool.TryParse(this.Value, out flag))
				{
					return flag;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = ((!value) ? "false" : "true");
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0007EB9E File Offset: 0x0007CF9E
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0007EBA6 File Offset: 0x0007CFA6
		public virtual JSONObject AsObject
		{
			get
			{
				return this as JSONObject;
			}
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0007EBAE File Offset: 0x0007CFAE
		public static implicit operator JSONNode(string s)
		{
			return new JSONString(s);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0007EBB6 File Offset: 0x0007CFB6
		public static implicit operator string(JSONNode d)
		{
			return (!(d == null)) ? d.Value : null;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0007EBD0 File Offset: 0x0007CFD0
		public static implicit operator JSONNode(double n)
		{
			return new JSONNumber(n);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0007EBD8 File Offset: 0x0007CFD8
		public static implicit operator double(JSONNode d)
		{
			return (!(d == null)) ? d.AsDouble : 0.0;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0007EBFA File Offset: 0x0007CFFA
		public static implicit operator JSONNode(float n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0007EC03 File Offset: 0x0007D003
		public static implicit operator float(JSONNode d)
		{
			return (!(d == null)) ? d.AsFloat : 0f;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0007EC21 File Offset: 0x0007D021
		public static implicit operator JSONNode(int n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0007EC2A File Offset: 0x0007D02A
		public static implicit operator int(JSONNode d)
		{
			return (!(d == null)) ? d.AsInt : 0;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0007EC44 File Offset: 0x0007D044
		public static implicit operator JSONNode(bool b)
		{
			return new JSONBool(b);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0007EC4C File Offset: 0x0007D04C
		public static implicit operator bool(JSONNode d)
		{
			return !(d == null) && d.AsBool;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0007EC68 File Offset: 0x0007D068
		public static bool operator ==(JSONNode a, object b)
		{
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}
			bool flag = a is JSONNull || object.ReferenceEquals(a, null) || a is JSONLazyCreator;
			bool flag2 = b is JSONNull || object.ReferenceEquals(b, null) || b is JSONLazyCreator;
			return (flag && flag2) || a.Equals(b);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0007ECE0 File Offset: 0x0007D0E0
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0007ECEC File Offset: 0x0007D0EC
		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0007ECF5 File Offset: 0x0007D0F5
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0007ED00 File Offset: 0x0007D100
		internal static string Escape(string aText)
		{
			JSONNode.m_EscapeBuilder.Length = 0;
			if (JSONNode.m_EscapeBuilder.Capacity < aText.Length + aText.Length / 10)
			{
				JSONNode.m_EscapeBuilder.Capacity = aText.Length + aText.Length / 10;
			}
			foreach (char c in aText)
			{
				switch (c)
				{
				case '\b':
					JSONNode.m_EscapeBuilder.Append("\\b");
					break;
				case '\t':
					JSONNode.m_EscapeBuilder.Append("\\t");
					break;
				case '\n':
					JSONNode.m_EscapeBuilder.Append("\\n");
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							JSONNode.m_EscapeBuilder.Append(c);
						}
						else
						{
							JSONNode.m_EscapeBuilder.Append("\\\\");
						}
					}
					else
					{
						JSONNode.m_EscapeBuilder.Append("\\\"");
					}
					break;
				case '\f':
					JSONNode.m_EscapeBuilder.Append("\\f");
					break;
				case '\r':
					JSONNode.m_EscapeBuilder.Append("\\r");
					break;
				}
			}
			string text = JSONNode.m_EscapeBuilder.ToString();
			JSONNode.m_EscapeBuilder.Length = 0;
			return text;
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0007EE64 File Offset: 0x0007D264
		private static void ParseElement(JSONNode ctx, string token, string tokenName, bool quoted)
		{
			if (quoted)
			{
				ctx.Add(tokenName, token);
				return;
			}
			string text = token.ToLower();
			double num;
			if (text == "false" || text == "true")
			{
				ctx.Add(tokenName, text == "true");
			}
			else if (text == "null")
			{
				ctx.Add(tokenName, null);
			}
			else if (double.TryParse(token, out num))
			{
				ctx.Add(tokenName, num);
			}
			else
			{
				ctx.Add(tokenName, token);
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0007EF14 File Offset: 0x0007D314
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			string text = string.Empty;
			bool flag = false;
			bool flag2 = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				switch (c)
				{
				case '\t':
					goto IL_275;
				case '\n':
				case '\r':
					break;
				default:
					switch (c)
					{
					case '[':
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
							goto IL_371;
						}
						stack.Push(new JSONArray());
						if (jsonnode != null)
						{
							jsonnode.Add(text, stack.Peek());
						}
						text = string.Empty;
						stringBuilder.Length = 0;
						jsonnode = stack.Peek();
						goto IL_371;
					case '\\':
						i++;
						if (flag)
						{
							char c2 = aJSON[i];
							switch (c2)
							{
							case 'r':
								stringBuilder.Append('\r');
								break;
							default:
								if (c2 != 'b')
								{
									if (c2 != 'f')
									{
										if (c2 != 'n')
										{
											stringBuilder.Append(c2);
										}
										else
										{
											stringBuilder.Append('\n');
										}
									}
									else
									{
										stringBuilder.Append('\f');
									}
								}
								else
								{
									stringBuilder.Append('\b');
								}
								break;
							case 't':
								stringBuilder.Append('\t');
								break;
							case 'u':
							{
								string text2 = aJSON.Substring(i + 1, 4);
								stringBuilder.Append((char)int.Parse(text2, NumberStyles.AllowHexSpecifier));
								i += 4;
								break;
							}
							}
						}
						goto IL_371;
					case ']':
						break;
					default:
						switch (c)
						{
						case ' ':
							goto IL_275;
						default:
							switch (c)
							{
							case '{':
								if (flag)
								{
									stringBuilder.Append(aJSON[i]);
									goto IL_371;
								}
								stack.Push(new JSONObject());
								if (jsonnode != null)
								{
									jsonnode.Add(text, stack.Peek());
								}
								text = string.Empty;
								stringBuilder.Length = 0;
								jsonnode = stack.Peek();
								goto IL_371;
							default:
								if (c != ',')
								{
									if (c != ':')
									{
										stringBuilder.Append(aJSON[i]);
										goto IL_371;
									}
									if (flag)
									{
										stringBuilder.Append(aJSON[i]);
										goto IL_371;
									}
									text = stringBuilder.ToString();
									stringBuilder.Length = 0;
									flag2 = false;
									goto IL_371;
								}
								else
								{
									if (flag)
									{
										stringBuilder.Append(aJSON[i]);
										goto IL_371;
									}
									if (stringBuilder.Length > 0 || flag2)
									{
										JSONNode.ParseElement(jsonnode, stringBuilder.ToString(), text, flag2);
									}
									text = string.Empty;
									stringBuilder.Length = 0;
									flag2 = false;
									goto IL_371;
								}
								break;
							case '}':
								break;
							}
							break;
						case '"':
							flag ^= true;
							flag2 = flag2 || flag;
							goto IL_371;
						}
						break;
					}
					if (flag)
					{
						stringBuilder.Append(aJSON[i]);
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (stringBuilder.Length > 0 || flag2)
						{
							JSONNode.ParseElement(jsonnode, stringBuilder.ToString(), text, flag2);
							flag2 = false;
						}
						text = string.Empty;
						stringBuilder.Length = 0;
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
					break;
				}
				IL_371:
				i++;
				continue;
				IL_275:
				if (flag)
				{
					stringBuilder.Append(aJSON[i]);
				}
				goto IL_371;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0007F2B5 File Offset: 0x0007D6B5
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0007F2B8 File Offset: 0x0007D6B8
		public void SaveToStream(Stream aData)
		{
			BinaryWriter binaryWriter = new BinaryWriter(aData);
			this.Serialize(binaryWriter);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0007F2D3 File Offset: 0x0007D6D3
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0007F2DF File Offset: 0x0007D6DF
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0007F2EB File Offset: 0x0007D6EB
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0007F2F7 File Offset: 0x0007D6F7
		public void SaveToFile(string aFileName)
		{
			throw new Exception("Can't use File IO stuff in the webplayer");
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0007F304 File Offset: 0x0007D704
		public string SaveToBase64()
		{
			string text;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				text = Convert.ToBase64String(memoryStream.ToArray());
			}
			return text;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0007F358 File Offset: 0x0007D758
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONNodeType jsonnodeType = (JSONNodeType)aReader.ReadByte();
			switch (jsonnodeType)
			{
			case JSONNodeType.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONNodeType.Object:
			{
				int num2 = aReader.ReadInt32();
				JSONObject jsonobject = new JSONObject();
				for (int j = 0; j < num2; j++)
				{
					string text = aReader.ReadString();
					JSONNode jsonnode = JSONNode.Deserialize(aReader);
					jsonobject.Add(text, jsonnode);
				}
				return jsonobject;
			}
			case JSONNodeType.String:
				return new JSONString(aReader.ReadString());
			case JSONNodeType.Number:
				return new JSONNumber(aReader.ReadDouble());
			case JSONNodeType.NullValue:
				return new JSONNull();
			case JSONNodeType.Boolean:
				return new JSONBool(aReader.ReadBoolean());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonnodeType);
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0007F441 File Offset: 0x0007D841
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0007F44D File Offset: 0x0007D84D
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0007F459 File Offset: 0x0007D859
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0007F468 File Offset: 0x0007D868
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode jsonnode;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				jsonnode = JSONNode.Deserialize(binaryReader);
			}
			return jsonnode;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0007F4A8 File Offset: 0x0007D8A8
		public static JSONNode LoadFromFile(string aFileName)
		{
			throw new Exception("Can't use File IO stuff in the webplayer");
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0007F4B4 File Offset: 0x0007D8B4
		public static JSONNode LoadFromBase64(string aBase64)
		{
			byte[] array = Convert.FromBase64String(aBase64);
			return JSONNode.LoadFromStream(new MemoryStream(array)
			{
				Position = 0L
			});
		}

		// Token: 0x04000F66 RID: 3942
		internal static StringBuilder m_EscapeBuilder = new StringBuilder();
	}
}
