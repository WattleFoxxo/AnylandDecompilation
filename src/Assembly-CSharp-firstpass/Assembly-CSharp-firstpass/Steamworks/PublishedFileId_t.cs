using System;

namespace Steamworks
{
	// Token: 0x02000309 RID: 777
	[Serializable]
	public struct PublishedFileId_t : IEquatable<PublishedFileId_t>, IComparable<PublishedFileId_t>
	{
		// Token: 0x06000E35 RID: 3637 RVA: 0x0000E607 File Offset: 0x0000C807
		public PublishedFileId_t(ulong value)
		{
			this.m_PublishedFileId = value;
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0000E610 File Offset: 0x0000C810
		public override string ToString()
		{
			return this.m_PublishedFileId.ToString();
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0000E623 File Offset: 0x0000C823
		public override bool Equals(object other)
		{
			return other is PublishedFileId_t && this == (PublishedFileId_t)other;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0000E644 File Offset: 0x0000C844
		public override int GetHashCode()
		{
			return this.m_PublishedFileId.GetHashCode();
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0000E657 File Offset: 0x0000C857
		public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y)
		{
			return x.m_PublishedFileId == y.m_PublishedFileId;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0000E669 File Offset: 0x0000C869
		public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0000E675 File Offset: 0x0000C875
		public static explicit operator PublishedFileId_t(ulong value)
		{
			return new PublishedFileId_t(value);
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0000E67D File Offset: 0x0000C87D
		public static explicit operator ulong(PublishedFileId_t that)
		{
			return that.m_PublishedFileId;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0000E686 File Offset: 0x0000C886
		public bool Equals(PublishedFileId_t other)
		{
			return this.m_PublishedFileId == other.m_PublishedFileId;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0000E697 File Offset: 0x0000C897
		public int CompareTo(PublishedFileId_t other)
		{
			return this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);
		}

		// Token: 0x04000D0D RID: 3341
		public static readonly PublishedFileId_t Invalid = new PublishedFileId_t(0UL);

		// Token: 0x04000D0E RID: 3342
		public ulong m_PublishedFileId;
	}
}
