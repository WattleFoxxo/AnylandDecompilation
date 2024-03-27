using System;

namespace Steamworks
{
	// Token: 0x02000307 RID: 775
	[Serializable]
	public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t>
	{
		// Token: 0x06000E21 RID: 3617 RVA: 0x0000E4BF File Offset: 0x0000C6BF
		public SNetListenSocket_t(uint value)
		{
			this.m_SNetListenSocket = value;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0000E4C8 File Offset: 0x0000C6C8
		public override string ToString()
		{
			return this.m_SNetListenSocket.ToString();
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0000E4DB File Offset: 0x0000C6DB
		public override bool Equals(object other)
		{
			return other is SNetListenSocket_t && this == (SNetListenSocket_t)other;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0000E4FC File Offset: 0x0000C6FC
		public override int GetHashCode()
		{
			return this.m_SNetListenSocket.GetHashCode();
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0000E50F File Offset: 0x0000C70F
		public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return x.m_SNetListenSocket == y.m_SNetListenSocket;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0000E521 File Offset: 0x0000C721
		public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0000E52D File Offset: 0x0000C72D
		public static explicit operator SNetListenSocket_t(uint value)
		{
			return new SNetListenSocket_t(value);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0000E535 File Offset: 0x0000C735
		public static explicit operator uint(SNetListenSocket_t that)
		{
			return that.m_SNetListenSocket;
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0000E53E File Offset: 0x0000C73E
		public bool Equals(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket == other.m_SNetListenSocket;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0000E54F File Offset: 0x0000C74F
		public int CompareTo(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
		}

		// Token: 0x04000D0B RID: 3339
		public uint m_SNetListenSocket;
	}
}
