using System;

namespace Steamworks
{
	// Token: 0x020002FD RID: 765
	[Serializable]
	public struct ControllerHandle_t : IEquatable<ControllerHandle_t>, IComparable<ControllerHandle_t>
	{
		// Token: 0x06000DB6 RID: 3510 RVA: 0x0000DDF8 File Offset: 0x0000BFF8
		public ControllerHandle_t(ulong value)
		{
			this.m_ControllerHandle = value;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0000DE01 File Offset: 0x0000C001
		public override string ToString()
		{
			return this.m_ControllerHandle.ToString();
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0000DE14 File Offset: 0x0000C014
		public override bool Equals(object other)
		{
			return other is ControllerHandle_t && this == (ControllerHandle_t)other;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0000DE35 File Offset: 0x0000C035
		public override int GetHashCode()
		{
			return this.m_ControllerHandle.GetHashCode();
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0000DE48 File Offset: 0x0000C048
		public static bool operator ==(ControllerHandle_t x, ControllerHandle_t y)
		{
			return x.m_ControllerHandle == y.m_ControllerHandle;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0000DE5A File Offset: 0x0000C05A
		public static bool operator !=(ControllerHandle_t x, ControllerHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0000DE66 File Offset: 0x0000C066
		public static explicit operator ControllerHandle_t(ulong value)
		{
			return new ControllerHandle_t(value);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0000DE6E File Offset: 0x0000C06E
		public static explicit operator ulong(ControllerHandle_t that)
		{
			return that.m_ControllerHandle;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0000DE77 File Offset: 0x0000C077
		public bool Equals(ControllerHandle_t other)
		{
			return this.m_ControllerHandle == other.m_ControllerHandle;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0000DE88 File Offset: 0x0000C088
		public int CompareTo(ControllerHandle_t other)
		{
			return this.m_ControllerHandle.CompareTo(other.m_ControllerHandle);
		}

		// Token: 0x04000CF9 RID: 3321
		public ulong m_ControllerHandle;
	}
}
