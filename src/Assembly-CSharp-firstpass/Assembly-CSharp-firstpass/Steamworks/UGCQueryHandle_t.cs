using System;

namespace Steamworks
{
	// Token: 0x02000313 RID: 787
	[Serializable]
	public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t>
	{
		// Token: 0x06000EA2 RID: 3746 RVA: 0x0000ECEA File Offset: 0x0000CEEA
		public UGCQueryHandle_t(ulong value)
		{
			this.m_UGCQueryHandle = value;
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0000ECF3 File Offset: 0x0000CEF3
		public override string ToString()
		{
			return this.m_UGCQueryHandle.ToString();
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0000ED06 File Offset: 0x0000CF06
		public override bool Equals(object other)
		{
			return other is UGCQueryHandle_t && this == (UGCQueryHandle_t)other;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0000ED27 File Offset: 0x0000CF27
		public override int GetHashCode()
		{
			return this.m_UGCQueryHandle.GetHashCode();
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0000ED3A File Offset: 0x0000CF3A
		public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return x.m_UGCQueryHandle == y.m_UGCQueryHandle;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0000ED4C File Offset: 0x0000CF4C
		public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0000ED58 File Offset: 0x0000CF58
		public static explicit operator UGCQueryHandle_t(ulong value)
		{
			return new UGCQueryHandle_t(value);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0000ED60 File Offset: 0x0000CF60
		public static explicit operator ulong(UGCQueryHandle_t that)
		{
			return that.m_UGCQueryHandle;
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0000ED69 File Offset: 0x0000CF69
		public bool Equals(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle == other.m_UGCQueryHandle;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0000ED7A File Offset: 0x0000CF7A
		public int CompareTo(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);
		}

		// Token: 0x04000D20 RID: 3360
		public static readonly UGCQueryHandle_t Invalid = new UGCQueryHandle_t(ulong.MaxValue);

		// Token: 0x04000D21 RID: 3361
		public ulong m_UGCQueryHandle;
	}
}
