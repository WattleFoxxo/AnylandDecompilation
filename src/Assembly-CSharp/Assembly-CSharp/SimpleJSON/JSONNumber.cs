using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020001CD RID: 461
	public class JSONNumber : JSONNode
	{
		// Token: 0x06000E5D RID: 3677 RVA: 0x00080758 File Offset: 0x0007EB58
		public JSONNumber(double aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00080767 File Offset: 0x0007EB67
		public JSONNumber(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00080776 File Offset: 0x0007EB76
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Number;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00080779 File Offset: 0x0007EB79
		public override bool IsNumber
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x0008077C File Offset: 0x0007EB7C
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x00080790 File Offset: 0x0007EB90
		public override string Value
		{
			get
			{
				return this.m_Data.ToString();
			}
			set
			{
				double num;
				if (double.TryParse(value, out num))
				{
					this.m_Data = num;
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x000807B1 File Offset: 0x0007EBB1
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x000807B9 File Offset: 0x0007EBB9
		public override double AsDouble
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

		// Token: 0x06000E65 RID: 3685 RVA: 0x000807C2 File Offset: 0x0007EBC2
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(4);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x000807D7 File Offset: 0x0007EBD7
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000807E8 File Offset: 0x0007EBE8
		private static bool IsNumeric(object value)
		{
			return value is int || value is uint || value is float || value is double || value is decimal || value is long || value is ulong || value is short || value is ushort || value is sbyte || value is byte;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00080870 File Offset: 0x0007EC70
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (base.Equals(obj))
			{
				return true;
			}
			JSONNumber jsonnumber = obj as JSONNumber;
			if (jsonnumber != null)
			{
				return this.m_Data == jsonnumber.m_Data;
			}
			return JSONNumber.IsNumeric(obj) && Convert.ToDouble(obj) == this.m_Data;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x000808D0 File Offset: 0x0007ECD0
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000F6C RID: 3948
		private double m_Data;
	}
}
