using System;

namespace Steamworks
{
	// Token: 0x0200030C RID: 780
	[Serializable]
	public struct UGCHandle_t : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
	{
		// Token: 0x06000E56 RID: 3670 RVA: 0x0000E81D File Offset: 0x0000CA1D
		public UGCHandle_t(ulong value)
		{
			this.m_UGCHandle = value;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0000E826 File Offset: 0x0000CA26
		public override string ToString()
		{
			return this.m_UGCHandle.ToString();
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0000E839 File Offset: 0x0000CA39
		public override bool Equals(object other)
		{
			return other is UGCHandle_t && this == (UGCHandle_t)other;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0000E85A File Offset: 0x0000CA5A
		public override int GetHashCode()
		{
			return this.m_UGCHandle.GetHashCode();
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0000E86D File Offset: 0x0000CA6D
		public static bool operator ==(UGCHandle_t x, UGCHandle_t y)
		{
			return x.m_UGCHandle == y.m_UGCHandle;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0000E87F File Offset: 0x0000CA7F
		public static bool operator !=(UGCHandle_t x, UGCHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0000E88B File Offset: 0x0000CA8B
		public static explicit operator UGCHandle_t(ulong value)
		{
			return new UGCHandle_t(value);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0000E893 File Offset: 0x0000CA93
		public static explicit operator ulong(UGCHandle_t that)
		{
			return that.m_UGCHandle;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0000E89C File Offset: 0x0000CA9C
		public bool Equals(UGCHandle_t other)
		{
			return this.m_UGCHandle == other.m_UGCHandle;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0000E8AD File Offset: 0x0000CAAD
		public int CompareTo(UGCHandle_t other)
		{
			return this.m_UGCHandle.CompareTo(other.m_UGCHandle);
		}

		// Token: 0x04000D13 RID: 3347
		public static readonly UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);

		// Token: 0x04000D14 RID: 3348
		public ulong m_UGCHandle;
	}
}
