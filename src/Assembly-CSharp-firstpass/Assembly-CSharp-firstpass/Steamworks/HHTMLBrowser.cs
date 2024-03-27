using System;

namespace Steamworks
{
	// Token: 0x020002FF RID: 767
	[Serializable]
	public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
	{
		// Token: 0x06000DCB RID: 3531 RVA: 0x0000DF4D File Offset: 0x0000C14D
		public HHTMLBrowser(uint value)
		{
			this.m_HHTMLBrowser = value;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0000DF56 File Offset: 0x0000C156
		public override string ToString()
		{
			return this.m_HHTMLBrowser.ToString();
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0000DF69 File Offset: 0x0000C169
		public override bool Equals(object other)
		{
			return other is HHTMLBrowser && this == (HHTMLBrowser)other;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0000DF8A File Offset: 0x0000C18A
		public override int GetHashCode()
		{
			return this.m_HHTMLBrowser.GetHashCode();
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0000DF9D File Offset: 0x0000C19D
		public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y)
		{
			return x.m_HHTMLBrowser == y.m_HHTMLBrowser;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0000DFAF File Offset: 0x0000C1AF
		public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y)
		{
			return !(x == y);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0000DFBB File Offset: 0x0000C1BB
		public static explicit operator HHTMLBrowser(uint value)
		{
			return new HHTMLBrowser(value);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0000DFC3 File Offset: 0x0000C1C3
		public static explicit operator uint(HHTMLBrowser that)
		{
			return that.m_HHTMLBrowser;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		public bool Equals(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser == other.m_HHTMLBrowser;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0000DFDD File Offset: 0x0000C1DD
		public int CompareTo(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
		}

		// Token: 0x04000CFC RID: 3324
		public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0U);

		// Token: 0x04000CFD RID: 3325
		public uint m_HHTMLBrowser;
	}
}
