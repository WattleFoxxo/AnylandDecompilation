using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020001CE RID: 462
	public class JSONBool : JSONNode
	{
		// Token: 0x06000E6A RID: 3690 RVA: 0x000808E3 File Offset: 0x0007ECE3
		public JSONBool(bool aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000808F2 File Offset: 0x0007ECF2
		public JSONBool(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x00080901 File Offset: 0x0007ED01
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Boolean;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x00080904 File Offset: 0x0007ED04
		public override bool IsBoolean
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00080907 File Offset: 0x0007ED07
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0008091C File Offset: 0x0007ED1C
		public override string Value
		{
			get
			{
				return this.m_Data.ToString();
			}
			set
			{
				bool flag;
				if (bool.TryParse(value, out flag))
				{
					this.m_Data = flag;
				}
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0008093D File Offset: 0x0007ED3D
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x00080945 File Offset: 0x0007ED45
		public override bool AsBool
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

		// Token: 0x06000E72 RID: 3698 RVA: 0x0008094E File Offset: 0x0007ED4E
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(6);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00080963 File Offset: 0x0007ED63
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append((!this.m_Data) ? "false" : "true");
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00080986 File Offset: 0x0007ED86
		public override bool Equals(object obj)
		{
			return obj != null && obj is bool && this.m_Data == (bool)obj;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x000809AB File Offset: 0x0007EDAB
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000F6D RID: 3949
		private bool m_Data;
	}
}
