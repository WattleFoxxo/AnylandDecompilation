using System;

namespace Steamworks
{
	// Token: 0x020002F2 RID: 754
	[Serializable]
	public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x0000D31F File Offset: 0x0000B51F
		public HSteamPipe(int value)
		{
			this.m_HSteamPipe = value;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0000D328 File Offset: 0x0000B528
		public override string ToString()
		{
			return this.m_HSteamPipe.ToString();
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0000D33B File Offset: 0x0000B53B
		public override bool Equals(object other)
		{
			return other is HSteamPipe && this == (HSteamPipe)other;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0000D35C File Offset: 0x0000B55C
		public override int GetHashCode()
		{
			return this.m_HSteamPipe.GetHashCode();
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0000D36F File Offset: 0x0000B56F
		public static bool operator ==(HSteamPipe x, HSteamPipe y)
		{
			return x.m_HSteamPipe == y.m_HSteamPipe;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0000D381 File Offset: 0x0000B581
		public static bool operator !=(HSteamPipe x, HSteamPipe y)
		{
			return !(x == y);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0000D38D File Offset: 0x0000B58D
		public static explicit operator HSteamPipe(int value)
		{
			return new HSteamPipe(value);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0000D395 File Offset: 0x0000B595
		public static explicit operator int(HSteamPipe that)
		{
			return that.m_HSteamPipe;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0000D39E File Offset: 0x0000B59E
		public bool Equals(HSteamPipe other)
		{
			return this.m_HSteamPipe == other.m_HSteamPipe;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0000D3AF File Offset: 0x0000B5AF
		public int CompareTo(HSteamPipe other)
		{
			return this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
		}

		// Token: 0x04000CE6 RID: 3302
		public int m_HSteamPipe;
	}
}
