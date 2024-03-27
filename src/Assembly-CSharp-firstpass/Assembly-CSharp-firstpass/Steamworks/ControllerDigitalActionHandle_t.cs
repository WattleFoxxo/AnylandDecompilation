using System;

namespace Steamworks
{
	// Token: 0x020002FC RID: 764
	[Serializable]
	public struct ControllerDigitalActionHandle_t : IEquatable<ControllerDigitalActionHandle_t>, IComparable<ControllerDigitalActionHandle_t>
	{
		// Token: 0x06000DAC RID: 3500 RVA: 0x0000DD54 File Offset: 0x0000BF54
		public ControllerDigitalActionHandle_t(ulong value)
		{
			this.m_ControllerDigitalActionHandle = value;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0000DD5D File Offset: 0x0000BF5D
		public override string ToString()
		{
			return this.m_ControllerDigitalActionHandle.ToString();
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0000DD70 File Offset: 0x0000BF70
		public override bool Equals(object other)
		{
			return other is ControllerDigitalActionHandle_t && this == (ControllerDigitalActionHandle_t)other;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0000DD91 File Offset: 0x0000BF91
		public override int GetHashCode()
		{
			return this.m_ControllerDigitalActionHandle.GetHashCode();
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
		public static bool operator ==(ControllerDigitalActionHandle_t x, ControllerDigitalActionHandle_t y)
		{
			return x.m_ControllerDigitalActionHandle == y.m_ControllerDigitalActionHandle;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0000DDB6 File Offset: 0x0000BFB6
		public static bool operator !=(ControllerDigitalActionHandle_t x, ControllerDigitalActionHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0000DDC2 File Offset: 0x0000BFC2
		public static explicit operator ControllerDigitalActionHandle_t(ulong value)
		{
			return new ControllerDigitalActionHandle_t(value);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0000DDCA File Offset: 0x0000BFCA
		public static explicit operator ulong(ControllerDigitalActionHandle_t that)
		{
			return that.m_ControllerDigitalActionHandle;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0000DDD3 File Offset: 0x0000BFD3
		public bool Equals(ControllerDigitalActionHandle_t other)
		{
			return this.m_ControllerDigitalActionHandle == other.m_ControllerDigitalActionHandle;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0000DDE4 File Offset: 0x0000BFE4
		public int CompareTo(ControllerDigitalActionHandle_t other)
		{
			return this.m_ControllerDigitalActionHandle.CompareTo(other.m_ControllerDigitalActionHandle);
		}

		// Token: 0x04000CF8 RID: 3320
		public ulong m_ControllerDigitalActionHandle;
	}
}
