using System;

namespace Steamworks
{
	// Token: 0x02000305 RID: 773
	[Serializable]
	public struct HServerListRequest : IEquatable<HServerListRequest>
	{
		// Token: 0x06000E0C RID: 3596 RVA: 0x0000E367 File Offset: 0x0000C567
		public HServerListRequest(IntPtr value)
		{
			this.m_HServerListRequest = value;
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0000E370 File Offset: 0x0000C570
		public override string ToString()
		{
			return this.m_HServerListRequest.ToString();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0000E383 File Offset: 0x0000C583
		public override bool Equals(object other)
		{
			return other is HServerListRequest && this == (HServerListRequest)other;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		public override int GetHashCode()
		{
			return this.m_HServerListRequest.GetHashCode();
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0000E3B7 File Offset: 0x0000C5B7
		public static bool operator ==(HServerListRequest x, HServerListRequest y)
		{
			return x.m_HServerListRequest == y.m_HServerListRequest;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0000E3CC File Offset: 0x0000C5CC
		public static bool operator !=(HServerListRequest x, HServerListRequest y)
		{
			return !(x == y);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0000E3D8 File Offset: 0x0000C5D8
		public static explicit operator HServerListRequest(IntPtr value)
		{
			return new HServerListRequest(value);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0000E3E0 File Offset: 0x0000C5E0
		public static explicit operator IntPtr(HServerListRequest that)
		{
			return that.m_HServerListRequest;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0000E3E9 File Offset: 0x0000C5E9
		public bool Equals(HServerListRequest other)
		{
			return this.m_HServerListRequest == other.m_HServerListRequest;
		}

		// Token: 0x04000D07 RID: 3335
		public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);

		// Token: 0x04000D08 RID: 3336
		public IntPtr m_HServerListRequest;
	}
}
