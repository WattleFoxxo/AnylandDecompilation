using System;

namespace Steamworks
{
	// Token: 0x0200030F RID: 783
	[Serializable]
	public struct AppId_t : IEquatable<AppId_t>, IComparable<AppId_t>
	{
		// Token: 0x06000E76 RID: 3702 RVA: 0x0000EA24 File Offset: 0x0000CC24
		public AppId_t(uint value)
		{
			this.m_AppId = value;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0000EA2D File Offset: 0x0000CC2D
		public override string ToString()
		{
			return this.m_AppId.ToString();
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0000EA40 File Offset: 0x0000CC40
		public override bool Equals(object other)
		{
			return other is AppId_t && this == (AppId_t)other;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0000EA61 File Offset: 0x0000CC61
		public override int GetHashCode()
		{
			return this.m_AppId.GetHashCode();
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0000EA74 File Offset: 0x0000CC74
		public static bool operator ==(AppId_t x, AppId_t y)
		{
			return x.m_AppId == y.m_AppId;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0000EA86 File Offset: 0x0000CC86
		public static bool operator !=(AppId_t x, AppId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0000EA92 File Offset: 0x0000CC92
		public static explicit operator AppId_t(uint value)
		{
			return new AppId_t(value);
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0000EA9A File Offset: 0x0000CC9A
		public static explicit operator uint(AppId_t that)
		{
			return that.m_AppId;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0000EAA3 File Offset: 0x0000CCA3
		public bool Equals(AppId_t other)
		{
			return this.m_AppId == other.m_AppId;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0000EAB4 File Offset: 0x0000CCB4
		public int CompareTo(AppId_t other)
		{
			return this.m_AppId.CompareTo(other.m_AppId);
		}

		// Token: 0x04000D18 RID: 3352
		public static readonly AppId_t Invalid = new AppId_t(0U);

		// Token: 0x04000D19 RID: 3353
		public uint m_AppId;
	}
}
