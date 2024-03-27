using System;

namespace Steamworks
{
	// Token: 0x02000306 RID: 774
	[Serializable]
	public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery>
	{
		// Token: 0x06000E16 RID: 3606 RVA: 0x0000E40E File Offset: 0x0000C60E
		public HServerQuery(int value)
		{
			this.m_HServerQuery = value;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0000E417 File Offset: 0x0000C617
		public override string ToString()
		{
			return this.m_HServerQuery.ToString();
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0000E42A File Offset: 0x0000C62A
		public override bool Equals(object other)
		{
			return other is HServerQuery && this == (HServerQuery)other;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0000E44B File Offset: 0x0000C64B
		public override int GetHashCode()
		{
			return this.m_HServerQuery.GetHashCode();
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0000E45E File Offset: 0x0000C65E
		public static bool operator ==(HServerQuery x, HServerQuery y)
		{
			return x.m_HServerQuery == y.m_HServerQuery;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0000E470 File Offset: 0x0000C670
		public static bool operator !=(HServerQuery x, HServerQuery y)
		{
			return !(x == y);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0000E47C File Offset: 0x0000C67C
		public static explicit operator HServerQuery(int value)
		{
			return new HServerQuery(value);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0000E484 File Offset: 0x0000C684
		public static explicit operator int(HServerQuery that)
		{
			return that.m_HServerQuery;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0000E48D File Offset: 0x0000C68D
		public bool Equals(HServerQuery other)
		{
			return this.m_HServerQuery == other.m_HServerQuery;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0000E49E File Offset: 0x0000C69E
		public int CompareTo(HServerQuery other)
		{
			return this.m_HServerQuery.CompareTo(other.m_HServerQuery);
		}

		// Token: 0x04000D09 RID: 3337
		public static readonly HServerQuery Invalid = new HServerQuery(-1);

		// Token: 0x04000D0A RID: 3338
		public int m_HServerQuery;
	}
}
