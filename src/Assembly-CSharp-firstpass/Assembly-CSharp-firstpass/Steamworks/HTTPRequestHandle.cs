using System;

namespace Steamworks
{
	// Token: 0x02000301 RID: 769
	[Serializable]
	public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
	{
		// Token: 0x06000DE1 RID: 3553 RVA: 0x0000E0AF File Offset: 0x0000C2AF
		public HTTPRequestHandle(uint value)
		{
			this.m_HTTPRequestHandle = value;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0000E0B8 File Offset: 0x0000C2B8
		public override string ToString()
		{
			return this.m_HTTPRequestHandle.ToString();
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0000E0CB File Offset: 0x0000C2CB
		public override bool Equals(object other)
		{
			return other is HTTPRequestHandle && this == (HTTPRequestHandle)other;
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0000E0EC File Offset: 0x0000C2EC
		public override int GetHashCode()
		{
			return this.m_HTTPRequestHandle.GetHashCode();
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0000E0FF File Offset: 0x0000C2FF
		public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return x.m_HTTPRequestHandle == y.m_HTTPRequestHandle;
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0000E111 File Offset: 0x0000C311
		public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0000E11D File Offset: 0x0000C31D
		public static explicit operator HTTPRequestHandle(uint value)
		{
			return new HTTPRequestHandle(value);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0000E125 File Offset: 0x0000C325
		public static explicit operator uint(HTTPRequestHandle that)
		{
			return that.m_HTTPRequestHandle;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0000E12E File Offset: 0x0000C32E
		public bool Equals(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle == other.m_HTTPRequestHandle;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0000E13F File Offset: 0x0000C33F
		public int CompareTo(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
		}

		// Token: 0x04000D00 RID: 3328
		public static readonly HTTPRequestHandle Invalid = new HTTPRequestHandle(0U);

		// Token: 0x04000D01 RID: 3329
		public uint m_HTTPRequestHandle;
	}
}
