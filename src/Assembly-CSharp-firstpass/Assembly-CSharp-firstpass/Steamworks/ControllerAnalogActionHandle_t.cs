using System;

namespace Steamworks
{
	// Token: 0x020002FB RID: 763
	[Serializable]
	public struct ControllerAnalogActionHandle_t : IEquatable<ControllerAnalogActionHandle_t>, IComparable<ControllerAnalogActionHandle_t>
	{
		// Token: 0x06000DA2 RID: 3490 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
		public ControllerAnalogActionHandle_t(ulong value)
		{
			this.m_ControllerAnalogActionHandle = value;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0000DCB9 File Offset: 0x0000BEB9
		public override string ToString()
		{
			return this.m_ControllerAnalogActionHandle.ToString();
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0000DCCC File Offset: 0x0000BECC
		public override bool Equals(object other)
		{
			return other is ControllerAnalogActionHandle_t && this == (ControllerAnalogActionHandle_t)other;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0000DCED File Offset: 0x0000BEED
		public override int GetHashCode()
		{
			return this.m_ControllerAnalogActionHandle.GetHashCode();
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0000DD00 File Offset: 0x0000BF00
		public static bool operator ==(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y)
		{
			return x.m_ControllerAnalogActionHandle == y.m_ControllerAnalogActionHandle;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0000DD12 File Offset: 0x0000BF12
		public static bool operator !=(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0000DD1E File Offset: 0x0000BF1E
		public static explicit operator ControllerAnalogActionHandle_t(ulong value)
		{
			return new ControllerAnalogActionHandle_t(value);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0000DD26 File Offset: 0x0000BF26
		public static explicit operator ulong(ControllerAnalogActionHandle_t that)
		{
			return that.m_ControllerAnalogActionHandle;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0000DD2F File Offset: 0x0000BF2F
		public bool Equals(ControllerAnalogActionHandle_t other)
		{
			return this.m_ControllerAnalogActionHandle == other.m_ControllerAnalogActionHandle;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0000DD40 File Offset: 0x0000BF40
		public int CompareTo(ControllerAnalogActionHandle_t other)
		{
			return this.m_ControllerAnalogActionHandle.CompareTo(other.m_ControllerAnalogActionHandle);
		}

		// Token: 0x04000CF7 RID: 3319
		public ulong m_ControllerAnalogActionHandle;
	}
}
