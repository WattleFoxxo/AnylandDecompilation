using System;

namespace Steamworks
{
	// Token: 0x0200030D RID: 781
	[Serializable]
	public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle>
	{
		// Token: 0x06000E61 RID: 3681 RVA: 0x0000E8CF File Offset: 0x0000CACF
		public ScreenshotHandle(uint value)
		{
			this.m_ScreenshotHandle = value;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
		public override string ToString()
		{
			return this.m_ScreenshotHandle.ToString();
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0000E8EB File Offset: 0x0000CAEB
		public override bool Equals(object other)
		{
			return other is ScreenshotHandle && this == (ScreenshotHandle)other;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0000E90C File Offset: 0x0000CB0C
		public override int GetHashCode()
		{
			return this.m_ScreenshotHandle.GetHashCode();
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0000E91F File Offset: 0x0000CB1F
		public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y)
		{
			return x.m_ScreenshotHandle == y.m_ScreenshotHandle;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0000E931 File Offset: 0x0000CB31
		public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0000E93D File Offset: 0x0000CB3D
		public static explicit operator ScreenshotHandle(uint value)
		{
			return new ScreenshotHandle(value);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0000E945 File Offset: 0x0000CB45
		public static explicit operator uint(ScreenshotHandle that)
		{
			return that.m_ScreenshotHandle;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0000E94E File Offset: 0x0000CB4E
		public bool Equals(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle == other.m_ScreenshotHandle;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0000E95F File Offset: 0x0000CB5F
		public int CompareTo(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
		}

		// Token: 0x04000D15 RID: 3349
		public static readonly ScreenshotHandle Invalid = new ScreenshotHandle(0U);

		// Token: 0x04000D16 RID: 3350
		public uint m_ScreenshotHandle;
	}
}
