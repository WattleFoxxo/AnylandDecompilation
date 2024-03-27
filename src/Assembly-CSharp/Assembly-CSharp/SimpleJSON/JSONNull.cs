using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020001CF RID: 463
	public class JSONNull : JSONNode
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x000809C6 File Offset: 0x0007EDC6
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.NullValue;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x000809C9 File Offset: 0x0007EDC9
		public override bool IsNull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x000809CC File Offset: 0x0007EDCC
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x000809D3 File Offset: 0x0007EDD3
		public override string Value
		{
			get
			{
				return "null";
			}
			set
			{
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x000809D5 File Offset: 0x0007EDD5
		// (set) Token: 0x06000E7C RID: 3708 RVA: 0x000809D8 File Offset: 0x0007EDD8
		public override bool AsBool
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x000809DA File Offset: 0x0007EDDA
		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj) || obj is JSONNull;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x000809F3 File Offset: 0x0007EDF3
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x000809F6 File Offset: 0x0007EDF6
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(5);
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x000809FF File Offset: 0x0007EDFF
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}
	}
}
