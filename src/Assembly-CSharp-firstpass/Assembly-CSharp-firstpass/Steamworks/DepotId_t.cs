using System;

namespace Steamworks
{
	// Token: 0x02000310 RID: 784
	[Serializable]
	public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t>
	{
		// Token: 0x06000E81 RID: 3713 RVA: 0x0000EAD5 File Offset: 0x0000CCD5
		public DepotId_t(uint value)
		{
			this.m_DepotId = value;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0000EADE File Offset: 0x0000CCDE
		public override string ToString()
		{
			return this.m_DepotId.ToString();
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0000EAF1 File Offset: 0x0000CCF1
		public override bool Equals(object other)
		{
			return other is DepotId_t && this == (DepotId_t)other;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0000EB12 File Offset: 0x0000CD12
		public override int GetHashCode()
		{
			return this.m_DepotId.GetHashCode();
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0000EB25 File Offset: 0x0000CD25
		public static bool operator ==(DepotId_t x, DepotId_t y)
		{
			return x.m_DepotId == y.m_DepotId;
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0000EB37 File Offset: 0x0000CD37
		public static bool operator !=(DepotId_t x, DepotId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0000EB43 File Offset: 0x0000CD43
		public static explicit operator DepotId_t(uint value)
		{
			return new DepotId_t(value);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0000EB4B File Offset: 0x0000CD4B
		public static explicit operator uint(DepotId_t that)
		{
			return that.m_DepotId;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0000EB54 File Offset: 0x0000CD54
		public bool Equals(DepotId_t other)
		{
			return this.m_DepotId == other.m_DepotId;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0000EB65 File Offset: 0x0000CD65
		public int CompareTo(DepotId_t other)
		{
			return this.m_DepotId.CompareTo(other.m_DepotId);
		}

		// Token: 0x04000D1A RID: 3354
		public static readonly DepotId_t Invalid = new DepotId_t(0U);

		// Token: 0x04000D1B RID: 3355
		public uint m_DepotId;
	}
}
