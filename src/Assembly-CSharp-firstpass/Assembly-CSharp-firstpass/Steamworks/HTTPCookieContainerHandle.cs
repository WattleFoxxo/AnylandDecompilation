using System;

namespace Steamworks
{
	// Token: 0x02000300 RID: 768
	[Serializable]
	public struct HTTPCookieContainerHandle : IEquatable<HTTPCookieContainerHandle>, IComparable<HTTPCookieContainerHandle>
	{
		// Token: 0x06000DD6 RID: 3542 RVA: 0x0000DFFE File Offset: 0x0000C1FE
		public HTTPCookieContainerHandle(uint value)
		{
			this.m_HTTPCookieContainerHandle = value;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0000E007 File Offset: 0x0000C207
		public override string ToString()
		{
			return this.m_HTTPCookieContainerHandle.ToString();
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0000E01A File Offset: 0x0000C21A
		public override bool Equals(object other)
		{
			return other is HTTPCookieContainerHandle && this == (HTTPCookieContainerHandle)other;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0000E03B File Offset: 0x0000C23B
		public override int GetHashCode()
		{
			return this.m_HTTPCookieContainerHandle.GetHashCode();
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0000E04E File Offset: 0x0000C24E
		public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return x.m_HTTPCookieContainerHandle == y.m_HTTPCookieContainerHandle;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0000E060 File Offset: 0x0000C260
		public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0000E06C File Offset: 0x0000C26C
		public static explicit operator HTTPCookieContainerHandle(uint value)
		{
			return new HTTPCookieContainerHandle(value);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0000E074 File Offset: 0x0000C274
		public static explicit operator uint(HTTPCookieContainerHandle that)
		{
			return that.m_HTTPCookieContainerHandle;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0000E07D File Offset: 0x0000C27D
		public bool Equals(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle == other.m_HTTPCookieContainerHandle;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0000E08E File Offset: 0x0000C28E
		public int CompareTo(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
		}

		// Token: 0x04000CFE RID: 3326
		public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);

		// Token: 0x04000CFF RID: 3327
		public uint m_HTTPCookieContainerHandle;
	}
}
