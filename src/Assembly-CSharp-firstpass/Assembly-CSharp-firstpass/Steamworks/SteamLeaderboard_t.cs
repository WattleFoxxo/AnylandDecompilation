using System;

namespace Steamworks
{
	// Token: 0x02000316 RID: 790
	[Serializable]
	public struct SteamLeaderboard_t : IEquatable<SteamLeaderboard_t>, IComparable<SteamLeaderboard_t>
	{
		// Token: 0x06000EC3 RID: 3779 RVA: 0x0000EF00 File Offset: 0x0000D100
		public SteamLeaderboard_t(ulong value)
		{
			this.m_SteamLeaderboard = value;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0000EF09 File Offset: 0x0000D109
		public override string ToString()
		{
			return this.m_SteamLeaderboard.ToString();
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0000EF1C File Offset: 0x0000D11C
		public override bool Equals(object other)
		{
			return other is SteamLeaderboard_t && this == (SteamLeaderboard_t)other;
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0000EF3D File Offset: 0x0000D13D
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboard.GetHashCode();
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0000EF50 File Offset: 0x0000D150
		public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return x.m_SteamLeaderboard == y.m_SteamLeaderboard;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0000EF62 File Offset: 0x0000D162
		public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0000EF6E File Offset: 0x0000D16E
		public static explicit operator SteamLeaderboard_t(ulong value)
		{
			return new SteamLeaderboard_t(value);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0000EF76 File Offset: 0x0000D176
		public static explicit operator ulong(SteamLeaderboard_t that)
		{
			return that.m_SteamLeaderboard;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0000EF7F File Offset: 0x0000D17F
		public bool Equals(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard == other.m_SteamLeaderboard;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0000EF90 File Offset: 0x0000D190
		public int CompareTo(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
		}

		// Token: 0x04000D26 RID: 3366
		public ulong m_SteamLeaderboard;
	}
}
