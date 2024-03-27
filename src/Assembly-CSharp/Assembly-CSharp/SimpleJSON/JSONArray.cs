using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020001CA RID: 458
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0007F7F3 File Offset: 0x0007DBF3
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Array;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0007F7F6 File Offset: 0x0007DBF6
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000186 RID: 390
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (value == null)
				{
					value = new JSONNull();
				}
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
				}
				else
				{
					this.m_List[aIndex] = value;
				}
			}
		}

		// Token: 0x17000187 RID: 391
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				if (value == null)
				{
					value = new JSONNull();
				}
				this.m_List.Add(value);
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x0007F8A7 File Offset: 0x0007DCA7
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0007F8B4 File Offset: 0x0007DCB4
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = new JSONNull();
			}
			this.m_List.Add(aItem);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0007F8D8 File Offset: 0x0007DCD8
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode jsonnode = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return jsonnode;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0007F919 File Offset: 0x0007DD19
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0007F92C File Offset: 0x0007DD2C
		public override IEnumerable<JSONNode> Children
		{
			get
			{
				foreach (JSONNode N in this.m_List)
				{
					yield return N;
				}
				yield break;
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0007F950 File Offset: 0x0007DD50
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode N in this.m_List)
			{
				yield return N;
			}
			yield break;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0007F96C File Offset: 0x0007DD6C
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0007F9C0 File Offset: 0x0007DDC0
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('[');
			int count = this.m_List.Count;
			if (this.inline)
			{
				aMode = JSONTextMode.Compact;
			}
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
				{
					aSB.Append(',');
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.AppendLine();
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.Append(' ', aIndent + aIndentInc);
				}
				this.m_List[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine().Append(' ', aIndent);
			}
			aSB.Append(']');
		}

		// Token: 0x04000F67 RID: 3943
		private List<JSONNode> m_List = new List<JSONNode>();

		// Token: 0x04000F68 RID: 3944
		public bool inline;
	}
}
