using System;

namespace Steamworks
{
	// Token: 0x02000312 RID: 786
	[Serializable]
	public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
	{
		// Token: 0x06000E97 RID: 3735 RVA: 0x0000EC38 File Offset: 0x0000CE38
		public SteamAPICall_t(ulong value)
		{
			this.m_SteamAPICall = value;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0000EC41 File Offset: 0x0000CE41
		public override string ToString()
		{
			return this.m_SteamAPICall.ToString();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0000EC54 File Offset: 0x0000CE54
		public override bool Equals(object other)
		{
			return other is SteamAPICall_t && this == (SteamAPICall_t)other;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0000EC75 File Offset: 0x0000CE75
		public override int GetHashCode()
		{
			return this.m_SteamAPICall.GetHashCode();
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0000EC88 File Offset: 0x0000CE88
		public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y)
		{
			return x.m_SteamAPICall == y.m_SteamAPICall;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0000EC9A File Offset: 0x0000CE9A
		public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0000ECA6 File Offset: 0x0000CEA6
		public static explicit operator SteamAPICall_t(ulong value)
		{
			return new SteamAPICall_t(value);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0000ECAE File Offset: 0x0000CEAE
		public static explicit operator ulong(SteamAPICall_t that)
		{
			return that.m_SteamAPICall;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0000ECB7 File Offset: 0x0000CEB7
		public bool Equals(SteamAPICall_t other)
		{
			return this.m_SteamAPICall == other.m_SteamAPICall;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0000ECC8 File Offset: 0x0000CEC8
		public int CompareTo(SteamAPICall_t other)
		{
			return this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);
		}

		// Token: 0x04000D1E RID: 3358
		public static readonly SteamAPICall_t Invalid = new SteamAPICall_t(0UL);

		// Token: 0x04000D1F RID: 3359
		public ulong m_SteamAPICall;
	}
}
