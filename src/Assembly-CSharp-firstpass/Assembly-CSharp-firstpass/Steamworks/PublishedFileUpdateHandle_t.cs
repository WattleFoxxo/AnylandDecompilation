using System;

namespace Steamworks
{
	// Token: 0x0200030A RID: 778
	[Serializable]
	public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t>
	{
		// Token: 0x06000E40 RID: 3648 RVA: 0x0000E6B9 File Offset: 0x0000C8B9
		public PublishedFileUpdateHandle_t(ulong value)
		{
			this.m_PublishedFileUpdateHandle = value;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0000E6C2 File Offset: 0x0000C8C2
		public override string ToString()
		{
			return this.m_PublishedFileUpdateHandle.ToString();
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0000E6D5 File Offset: 0x0000C8D5
		public override bool Equals(object other)
		{
			return other is PublishedFileUpdateHandle_t && this == (PublishedFileUpdateHandle_t)other;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0000E6F6 File Offset: 0x0000C8F6
		public override int GetHashCode()
		{
			return this.m_PublishedFileUpdateHandle.GetHashCode();
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0000E709 File Offset: 0x0000C909
		public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return x.m_PublishedFileUpdateHandle == y.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0000E71B File Offset: 0x0000C91B
		public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0000E727 File Offset: 0x0000C927
		public static explicit operator PublishedFileUpdateHandle_t(ulong value)
		{
			return new PublishedFileUpdateHandle_t(value);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0000E72F File Offset: 0x0000C92F
		public static explicit operator ulong(PublishedFileUpdateHandle_t that)
		{
			return that.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0000E738 File Offset: 0x0000C938
		public bool Equals(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle == other.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0000E749 File Offset: 0x0000C949
		public int CompareTo(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
		}

		// Token: 0x04000D0F RID: 3343
		public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000D10 RID: 3344
		public ulong m_PublishedFileUpdateHandle;
	}
}
