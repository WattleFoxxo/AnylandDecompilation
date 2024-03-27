using System;

namespace Steamworks
{
	// Token: 0x020002FE RID: 766
	[Serializable]
	public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
	{
		// Token: 0x06000DC0 RID: 3520 RVA: 0x0000DE9C File Offset: 0x0000C09C
		public FriendsGroupID_t(short value)
		{
			this.m_FriendsGroupID = value;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0000DEA5 File Offset: 0x0000C0A5
		public override string ToString()
		{
			return this.m_FriendsGroupID.ToString();
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0000DEB8 File Offset: 0x0000C0B8
		public override bool Equals(object other)
		{
			return other is FriendsGroupID_t && this == (FriendsGroupID_t)other;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0000DED9 File Offset: 0x0000C0D9
		public override int GetHashCode()
		{
			return this.m_FriendsGroupID.GetHashCode();
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0000DEEC File Offset: 0x0000C0EC
		public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return x.m_FriendsGroupID == y.m_FriendsGroupID;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0000DEFE File Offset: 0x0000C0FE
		public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0000DF0A File Offset: 0x0000C10A
		public static explicit operator FriendsGroupID_t(short value)
		{
			return new FriendsGroupID_t(value);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0000DF12 File Offset: 0x0000C112
		public static explicit operator short(FriendsGroupID_t that)
		{
			return that.m_FriendsGroupID;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0000DF1B File Offset: 0x0000C11B
		public bool Equals(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID == other.m_FriendsGroupID;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0000DF2C File Offset: 0x0000C12C
		public int CompareTo(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
		}

		// Token: 0x04000CFA RID: 3322
		public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t(-1);

		// Token: 0x04000CFB RID: 3323
		public short m_FriendsGroupID;
	}
}
