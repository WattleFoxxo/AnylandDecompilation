using System;

namespace Steamworks
{
	// Token: 0x02000317 RID: 791
	[Serializable]
	public struct SteamLeaderboardEntries_t : IEquatable<SteamLeaderboardEntries_t>, IComparable<SteamLeaderboardEntries_t>
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		public SteamLeaderboardEntries_t(ulong value)
		{
			this.m_SteamLeaderboardEntries = value;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0000EFAD File Offset: 0x0000D1AD
		public override string ToString()
		{
			return this.m_SteamLeaderboardEntries.ToString();
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0000EFC0 File Offset: 0x0000D1C0
		public override bool Equals(object other)
		{
			return other is SteamLeaderboardEntries_t && this == (SteamLeaderboardEntries_t)other;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0000EFE1 File Offset: 0x0000D1E1
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboardEntries.GetHashCode();
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0000EFF4 File Offset: 0x0000D1F4
		public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return x.m_SteamLeaderboardEntries == y.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0000F006 File Offset: 0x0000D206
		public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0000F012 File Offset: 0x0000D212
		public static explicit operator SteamLeaderboardEntries_t(ulong value)
		{
			return new SteamLeaderboardEntries_t(value);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0000F01A File Offset: 0x0000D21A
		public static explicit operator ulong(SteamLeaderboardEntries_t that)
		{
			return that.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0000F023 File Offset: 0x0000D223
		public bool Equals(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries == other.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0000F034 File Offset: 0x0000D234
		public int CompareTo(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
		}

		// Token: 0x04000D27 RID: 3367
		public ulong m_SteamLeaderboardEntries;
	}
}
