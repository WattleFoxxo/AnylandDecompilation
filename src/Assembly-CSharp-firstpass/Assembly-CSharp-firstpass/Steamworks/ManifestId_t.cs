using System;

namespace Steamworks
{
	// Token: 0x02000311 RID: 785
	[Serializable]
	public struct ManifestId_t : IEquatable<ManifestId_t>, IComparable<ManifestId_t>
	{
		// Token: 0x06000E8C RID: 3724 RVA: 0x0000EB86 File Offset: 0x0000CD86
		public ManifestId_t(ulong value)
		{
			this.m_ManifestId = value;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0000EB8F File Offset: 0x0000CD8F
		public override string ToString()
		{
			return this.m_ManifestId.ToString();
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0000EBA2 File Offset: 0x0000CDA2
		public override bool Equals(object other)
		{
			return other is ManifestId_t && this == (ManifestId_t)other;
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0000EBC3 File Offset: 0x0000CDC3
		public override int GetHashCode()
		{
			return this.m_ManifestId.GetHashCode();
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0000EBD6 File Offset: 0x0000CDD6
		public static bool operator ==(ManifestId_t x, ManifestId_t y)
		{
			return x.m_ManifestId == y.m_ManifestId;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
		public static bool operator !=(ManifestId_t x, ManifestId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0000EBF4 File Offset: 0x0000CDF4
		public static explicit operator ManifestId_t(ulong value)
		{
			return new ManifestId_t(value);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0000EBFC File Offset: 0x0000CDFC
		public static explicit operator ulong(ManifestId_t that)
		{
			return that.m_ManifestId;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0000EC05 File Offset: 0x0000CE05
		public bool Equals(ManifestId_t other)
		{
			return this.m_ManifestId == other.m_ManifestId;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0000EC16 File Offset: 0x0000CE16
		public int CompareTo(ManifestId_t other)
		{
			return this.m_ManifestId.CompareTo(other.m_ManifestId);
		}

		// Token: 0x04000D1C RID: 3356
		public static readonly ManifestId_t Invalid = new ManifestId_t(0UL);

		// Token: 0x04000D1D RID: 3357
		public ulong m_ManifestId;
	}
}
