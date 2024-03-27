using System;

namespace Steamworks
{
	// Token: 0x02000303 RID: 771
	[Serializable]
	public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
	{
		// Token: 0x06000DF7 RID: 3575 RVA: 0x0000E211 File Offset: 0x0000C411
		public SteamItemDef_t(int value)
		{
			this.m_SteamItemDef = value;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0000E21A File Offset: 0x0000C41A
		public override string ToString()
		{
			return this.m_SteamItemDef.ToString();
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0000E22D File Offset: 0x0000C42D
		public override bool Equals(object other)
		{
			return other is SteamItemDef_t && this == (SteamItemDef_t)other;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0000E24E File Offset: 0x0000C44E
		public override int GetHashCode()
		{
			return this.m_SteamItemDef.GetHashCode();
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0000E261 File Offset: 0x0000C461
		public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y)
		{
			return x.m_SteamItemDef == y.m_SteamItemDef;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0000E273 File Offset: 0x0000C473
		public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0000E27F File Offset: 0x0000C47F
		public static explicit operator SteamItemDef_t(int value)
		{
			return new SteamItemDef_t(value);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0000E287 File Offset: 0x0000C487
		public static explicit operator int(SteamItemDef_t that)
		{
			return that.m_SteamItemDef;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0000E290 File Offset: 0x0000C490
		public bool Equals(SteamItemDef_t other)
		{
			return this.m_SteamItemDef == other.m_SteamItemDef;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0000E2A1 File Offset: 0x0000C4A1
		public int CompareTo(SteamItemDef_t other)
		{
			return this.m_SteamItemDef.CompareTo(other.m_SteamItemDef);
		}

		// Token: 0x04000D04 RID: 3332
		public int m_SteamItemDef;
	}
}
