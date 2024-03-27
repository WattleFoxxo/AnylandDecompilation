using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020001CC RID: 460
	public class JSONString : JSONNode
	{
		// Token: 0x06000E54 RID: 3668 RVA: 0x00080693 File Offset: 0x0007EA93
		public JSONString(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x000806A2 File Offset: 0x0007EAA2
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.String;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x000806A5 File Offset: 0x0007EAA5
		public override bool IsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x000806A8 File Offset: 0x0007EAA8
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x000806B0 File Offset: 0x0007EAB0
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x000806B9 File Offset: 0x0007EAB9
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x000806CE File Offset: 0x0007EACE
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x000806F0 File Offset: 0x0007EAF0
		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				return true;
			}
			string text = obj as string;
			if (text != null)
			{
				return this.m_Data == text;
			}
			JSONString jsonstring = obj as JSONString;
			return jsonstring != null && this.m_Data == jsonstring.m_Data;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0008074B File Offset: 0x0007EB4B
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000F6B RID: 3947
		private string m_Data;
	}
}
