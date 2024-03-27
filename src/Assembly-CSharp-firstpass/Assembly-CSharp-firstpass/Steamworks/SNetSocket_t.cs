using System;

namespace Steamworks
{
	// Token: 0x02000308 RID: 776
	[Serializable]
	public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
	{
		// Token: 0x06000E2B RID: 3627 RVA: 0x0000E563 File Offset: 0x0000C763
		public SNetSocket_t(uint value)
		{
			this.m_SNetSocket = value;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0000E56C File Offset: 0x0000C76C
		public override string ToString()
		{
			return this.m_SNetSocket.ToString();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0000E57F File Offset: 0x0000C77F
		public override bool Equals(object other)
		{
			return other is SNetSocket_t && this == (SNetSocket_t)other;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public override int GetHashCode()
		{
			return this.m_SNetSocket.GetHashCode();
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0000E5B3 File Offset: 0x0000C7B3
		public static bool operator ==(SNetSocket_t x, SNetSocket_t y)
		{
			return x.m_SNetSocket == y.m_SNetSocket;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0000E5C5 File Offset: 0x0000C7C5
		public static bool operator !=(SNetSocket_t x, SNetSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0000E5D1 File Offset: 0x0000C7D1
		public static explicit operator SNetSocket_t(uint value)
		{
			return new SNetSocket_t(value);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0000E5D9 File Offset: 0x0000C7D9
		public static explicit operator uint(SNetSocket_t that)
		{
			return that.m_SNetSocket;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0000E5E2 File Offset: 0x0000C7E2
		public bool Equals(SNetSocket_t other)
		{
			return this.m_SNetSocket == other.m_SNetSocket;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0000E5F3 File Offset: 0x0000C7F3
		public int CompareTo(SNetSocket_t other)
		{
			return this.m_SNetSocket.CompareTo(other.m_SNetSocket);
		}

		// Token: 0x04000D0C RID: 3340
		public uint m_SNetSocket;
	}
}
