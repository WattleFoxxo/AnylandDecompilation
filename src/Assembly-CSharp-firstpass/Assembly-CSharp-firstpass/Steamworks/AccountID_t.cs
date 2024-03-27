using System;

namespace Steamworks
{
	// Token: 0x0200030E RID: 782
	[Serializable]
	public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
	{
		// Token: 0x06000E6C RID: 3692 RVA: 0x0000E980 File Offset: 0x0000CB80
		public AccountID_t(uint value)
		{
			this.m_AccountID = value;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0000E989 File Offset: 0x0000CB89
		public override string ToString()
		{
			return this.m_AccountID.ToString();
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0000E99C File Offset: 0x0000CB9C
		public override bool Equals(object other)
		{
			return other is AccountID_t && this == (AccountID_t)other;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0000E9BD File Offset: 0x0000CBBD
		public override int GetHashCode()
		{
			return this.m_AccountID.GetHashCode();
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0000E9D0 File Offset: 0x0000CBD0
		public static bool operator ==(AccountID_t x, AccountID_t y)
		{
			return x.m_AccountID == y.m_AccountID;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0000E9E2 File Offset: 0x0000CBE2
		public static bool operator !=(AccountID_t x, AccountID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0000E9EE File Offset: 0x0000CBEE
		public static explicit operator AccountID_t(uint value)
		{
			return new AccountID_t(value);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0000E9F6 File Offset: 0x0000CBF6
		public static explicit operator uint(AccountID_t that)
		{
			return that.m_AccountID;
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0000E9FF File Offset: 0x0000CBFF
		public bool Equals(AccountID_t other)
		{
			return this.m_AccountID == other.m_AccountID;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0000EA10 File Offset: 0x0000CC10
		public int CompareTo(AccountID_t other)
		{
			return this.m_AccountID.CompareTo(other.m_AccountID);
		}

		// Token: 0x04000D17 RID: 3351
		public uint m_AccountID;
	}
}
