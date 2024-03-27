using System;

namespace Steamworks
{
	// Token: 0x020002F3 RID: 755
	[Serializable]
	public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser>
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x0000D3C3 File Offset: 0x0000B5C3
		public HSteamUser(int value)
		{
			this.m_HSteamUser = value;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		public override string ToString()
		{
			return this.m_HSteamUser.ToString();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0000D3DF File Offset: 0x0000B5DF
		public override bool Equals(object other)
		{
			return other is HSteamUser && this == (HSteamUser)other;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0000D400 File Offset: 0x0000B600
		public override int GetHashCode()
		{
			return this.m_HSteamUser.GetHashCode();
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0000D413 File Offset: 0x0000B613
		public static bool operator ==(HSteamUser x, HSteamUser y)
		{
			return x.m_HSteamUser == y.m_HSteamUser;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0000D425 File Offset: 0x0000B625
		public static bool operator !=(HSteamUser x, HSteamUser y)
		{
			return !(x == y);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0000D431 File Offset: 0x0000B631
		public static explicit operator HSteamUser(int value)
		{
			return new HSteamUser(value);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0000D439 File Offset: 0x0000B639
		public static explicit operator int(HSteamUser that)
		{
			return that.m_HSteamUser;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0000D442 File Offset: 0x0000B642
		public bool Equals(HSteamUser other)
		{
			return this.m_HSteamUser == other.m_HSteamUser;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0000D453 File Offset: 0x0000B653
		public int CompareTo(HSteamUser other)
		{
			return this.m_HSteamUser.CompareTo(other.m_HSteamUser);
		}

		// Token: 0x04000CE7 RID: 3303
		public int m_HSteamUser;
	}
}
