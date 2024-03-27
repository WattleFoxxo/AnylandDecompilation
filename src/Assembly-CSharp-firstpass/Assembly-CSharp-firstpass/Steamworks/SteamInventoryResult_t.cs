using System;

namespace Steamworks
{
	// Token: 0x02000302 RID: 770
	[Serializable]
	public struct SteamInventoryResult_t : IEquatable<SteamInventoryResult_t>, IComparable<SteamInventoryResult_t>
	{
		// Token: 0x06000DEC RID: 3564 RVA: 0x0000E160 File Offset: 0x0000C360
		public SteamInventoryResult_t(int value)
		{
			this.m_SteamInventoryResult = value;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0000E169 File Offset: 0x0000C369
		public override string ToString()
		{
			return this.m_SteamInventoryResult.ToString();
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0000E17C File Offset: 0x0000C37C
		public override bool Equals(object other)
		{
			return other is SteamInventoryResult_t && this == (SteamInventoryResult_t)other;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0000E19D File Offset: 0x0000C39D
		public override int GetHashCode()
		{
			return this.m_SteamInventoryResult.GetHashCode();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0000E1B0 File Offset: 0x0000C3B0
		public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0000E1C2 File Offset: 0x0000C3C2
		public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0000E1CE File Offset: 0x0000C3CE
		public static explicit operator SteamInventoryResult_t(int value)
		{
			return new SteamInventoryResult_t(value);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0000E1D6 File Offset: 0x0000C3D6
		public static explicit operator int(SteamInventoryResult_t that)
		{
			return that.m_SteamInventoryResult;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0000E1DF File Offset: 0x0000C3DF
		public bool Equals(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0000E1F0 File Offset: 0x0000C3F0
		public int CompareTo(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
		}

		// Token: 0x04000D02 RID: 3330
		public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);

		// Token: 0x04000D03 RID: 3331
		public int m_SteamInventoryResult;
	}
}
