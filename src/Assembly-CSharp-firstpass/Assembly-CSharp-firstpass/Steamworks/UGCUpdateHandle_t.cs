using System;

namespace Steamworks
{
	// Token: 0x02000314 RID: 788
	[Serializable]
	public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
	{
		// Token: 0x06000EAD RID: 3757 RVA: 0x0000ED9C File Offset: 0x0000CF9C
		public UGCUpdateHandle_t(ulong value)
		{
			this.m_UGCUpdateHandle = value;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0000EDA5 File Offset: 0x0000CFA5
		public override string ToString()
		{
			return this.m_UGCUpdateHandle.ToString();
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		public override bool Equals(object other)
		{
			return other is UGCUpdateHandle_t && this == (UGCUpdateHandle_t)other;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0000EDD9 File Offset: 0x0000CFD9
		public override int GetHashCode()
		{
			return this.m_UGCUpdateHandle.GetHashCode();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return x.m_UGCUpdateHandle == y.m_UGCUpdateHandle;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0000EDFE File Offset: 0x0000CFFE
		public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0000EE0A File Offset: 0x0000D00A
		public static explicit operator UGCUpdateHandle_t(ulong value)
		{
			return new UGCUpdateHandle_t(value);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0000EE12 File Offset: 0x0000D012
		public static explicit operator ulong(UGCUpdateHandle_t that)
		{
			return that.m_UGCUpdateHandle;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0000EE1B File Offset: 0x0000D01B
		public bool Equals(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle == other.m_UGCUpdateHandle;
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0000EE2C File Offset: 0x0000D02C
		public int CompareTo(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
		}

		// Token: 0x04000D22 RID: 3362
		public static readonly UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000D23 RID: 3363
		public ulong m_UGCUpdateHandle;
	}
}
