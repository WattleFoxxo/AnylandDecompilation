using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020001CB RID: 459
	public class JSONObject : JSONNode, IEnumerable
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0007FD60 File Offset: 0x0007E160
		public override IEnumerable<string> Keys
		{
			get
			{
				foreach (string key in this.m_Dict.Keys)
				{
					yield return key;
				}
				yield break;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0007FD83 File Offset: 0x0007E183
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Object;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0007FD86 File Offset: 0x0007E186
		public override bool IsObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700018D RID: 397
		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict[aKey];
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (value == null)
				{
					value = new JSONNull();
				}
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = value;
				}
				else
				{
					this.m_Dict.Add(aKey, value);
				}
			}
		}

		// Token: 0x1700018E RID: 398
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt(aIndex).Value;
			}
			set
			{
				if (value == null)
				{
					value = new JSONNull();
				}
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0007FE97 File Offset: 0x0007E297
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0007FEA4 File Offset: 0x0007E2A4
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = new JSONNull();
			}
			if (!string.IsNullOrEmpty(aKey))
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = aItem;
				}
				else
				{
					this.m_Dict.Add(aKey, aItem);
				}
			}
			else
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
			}
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0007FF24 File Offset: 0x0007E324
		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode jsonnode = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return jsonnode;
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0007FF60 File Offset: 0x0007E360
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0007FFB0 File Offset: 0x0007E3B0
		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode jsonnode;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.Where((KeyValuePair<string, JSONNode> k) => k.Value == aNode).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				jsonnode = aNode;
			}
			catch
			{
				jsonnode = null;
			}
			return jsonnode;
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x00080020 File Offset: 0x0007E420
		public override IEnumerable<JSONNode> Children
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> N in this.m_Dict)
				{
					yield return N.Value;
				}
				yield break;
			}
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00080044 File Offset: 0x0007E444
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, JSONNode> N in this.m_Dict)
			{
				yield return N;
			}
			yield break;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00080060 File Offset: 0x0007E460
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(2);
			aWriter.Write(this.m_Dict.Count);
			foreach (string text in this.m_Dict.Keys)
			{
				aWriter.Write(text);
				this.m_Dict[text].Serialize(aWriter);
			}
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x000800EC File Offset: 0x0007E4EC
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('{');
			bool flag = true;
			if (this.inline)
			{
				aMode = JSONTextMode.Compact;
			}
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (!flag)
				{
					aSB.Append(',');
				}
				flag = false;
				if (aMode == JSONTextMode.Indent)
				{
					aSB.AppendLine();
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.Append(' ', aIndent + aIndentInc);
				}
				aSB.Append('"').Append(JSONNode.Escape(keyValuePair.Key)).Append('"');
				if (aMode == JSONTextMode.Compact)
				{
					aSB.Append(':');
				}
				else
				{
					aSB.Append(" : ");
				}
				keyValuePair.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine().Append(' ', aIndent);
			}
			aSB.Append('}');
		}

		// Token: 0x04000F69 RID: 3945
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		// Token: 0x04000F6A RID: 3946
		public bool inline;
	}
}
