using System;

namespace Steamworks
{
	// Token: 0x020002F9 RID: 761
	[Serializable]
	public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x0000DB5B File Offset: 0x0000BD5B
		public HAuthTicket(uint value)
		{
			this.m_HAuthTicket = value;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0000DB64 File Offset: 0x0000BD64
		public override string ToString()
		{
			return this.m_HAuthTicket.ToString();
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0000DB77 File Offset: 0x0000BD77
		public override bool Equals(object other)
		{
			return other is HAuthTicket && this == (HAuthTicket)other;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0000DB98 File Offset: 0x0000BD98
		public override int GetHashCode()
		{
			return this.m_HAuthTicket.GetHashCode();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0000DBAB File Offset: 0x0000BDAB
		public static bool operator ==(HAuthTicket x, HAuthTicket y)
		{
			return x.m_HAuthTicket == y.m_HAuthTicket;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0000DBBD File Offset: 0x0000BDBD
		public static bool operator !=(HAuthTicket x, HAuthTicket y)
		{
			return !(x == y);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0000DBC9 File Offset: 0x0000BDC9
		public static explicit operator HAuthTicket(uint value)
		{
			return new HAuthTicket(value);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0000DBD1 File Offset: 0x0000BDD1
		public static explicit operator uint(HAuthTicket that)
		{
			return that.m_HAuthTicket;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0000DBDA File Offset: 0x0000BDDA
		public bool Equals(HAuthTicket other)
		{
			return this.m_HAuthTicket == other.m_HAuthTicket;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0000DBEB File Offset: 0x0000BDEB
		public int CompareTo(HAuthTicket other)
		{
			return this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
		}

		// Token: 0x04000CF4 RID: 3316
		public static readonly HAuthTicket Invalid = new HAuthTicket(0U);

		// Token: 0x04000CF5 RID: 3317
		public uint m_HAuthTicket;
	}
}
