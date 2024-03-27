using System;

namespace Steamworks
{
	// Token: 0x0200030B RID: 779
	[Serializable]
	public struct UGCFileWriteStreamHandle_t : IEquatable<UGCFileWriteStreamHandle_t>, IComparable<UGCFileWriteStreamHandle_t>
	{
		// Token: 0x06000E4B RID: 3659 RVA: 0x0000E76B File Offset: 0x0000C96B
		public UGCFileWriteStreamHandle_t(ulong value)
		{
			this.m_UGCFileWriteStreamHandle = value;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0000E774 File Offset: 0x0000C974
		public override string ToString()
		{
			return this.m_UGCFileWriteStreamHandle.ToString();
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0000E787 File Offset: 0x0000C987
		public override bool Equals(object other)
		{
			return other is UGCFileWriteStreamHandle_t && this == (UGCFileWriteStreamHandle_t)other;
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
		public override int GetHashCode()
		{
			return this.m_UGCFileWriteStreamHandle.GetHashCode();
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0000E7BB File Offset: 0x0000C9BB
		public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return x.m_UGCFileWriteStreamHandle == y.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0000E7CD File Offset: 0x0000C9CD
		public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0000E7D9 File Offset: 0x0000C9D9
		public static explicit operator UGCFileWriteStreamHandle_t(ulong value)
		{
			return new UGCFileWriteStreamHandle_t(value);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0000E7E1 File Offset: 0x0000C9E1
		public static explicit operator ulong(UGCFileWriteStreamHandle_t that)
		{
			return that.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0000E7EA File Offset: 0x0000C9EA
		public bool Equals(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle == other.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0000E7FB File Offset: 0x0000C9FB
		public int CompareTo(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
		}

		// Token: 0x04000D11 RID: 3345
		public static readonly UGCFileWriteStreamHandle_t Invalid = new UGCFileWriteStreamHandle_t(ulong.MaxValue);

		// Token: 0x04000D12 RID: 3346
		public ulong m_UGCFileWriteStreamHandle;
	}
}
