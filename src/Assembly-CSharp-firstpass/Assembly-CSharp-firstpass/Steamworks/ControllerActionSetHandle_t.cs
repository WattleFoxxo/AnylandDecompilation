using System;

namespace Steamworks
{
	// Token: 0x020002FA RID: 762
	[Serializable]
	public struct ControllerActionSetHandle_t : IEquatable<ControllerActionSetHandle_t>, IComparable<ControllerActionSetHandle_t>
	{
		// Token: 0x06000D98 RID: 3480 RVA: 0x0000DC0C File Offset: 0x0000BE0C
		public ControllerActionSetHandle_t(ulong value)
		{
			this.m_ControllerActionSetHandle = value;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0000DC15 File Offset: 0x0000BE15
		public override string ToString()
		{
			return this.m_ControllerActionSetHandle.ToString();
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0000DC28 File Offset: 0x0000BE28
		public override bool Equals(object other)
		{
			return other is ControllerActionSetHandle_t && this == (ControllerActionSetHandle_t)other;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0000DC49 File Offset: 0x0000BE49
		public override int GetHashCode()
		{
			return this.m_ControllerActionSetHandle.GetHashCode();
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0000DC5C File Offset: 0x0000BE5C
		public static bool operator ==(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y)
		{
			return x.m_ControllerActionSetHandle == y.m_ControllerActionSetHandle;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0000DC6E File Offset: 0x0000BE6E
		public static bool operator !=(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0000DC7A File Offset: 0x0000BE7A
		public static explicit operator ControllerActionSetHandle_t(ulong value)
		{
			return new ControllerActionSetHandle_t(value);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0000DC82 File Offset: 0x0000BE82
		public static explicit operator ulong(ControllerActionSetHandle_t that)
		{
			return that.m_ControllerActionSetHandle;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0000DC8B File Offset: 0x0000BE8B
		public bool Equals(ControllerActionSetHandle_t other)
		{
			return this.m_ControllerActionSetHandle == other.m_ControllerActionSetHandle;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		public int CompareTo(ControllerActionSetHandle_t other)
		{
			return this.m_ControllerActionSetHandle.CompareTo(other.m_ControllerActionSetHandle);
		}

		// Token: 0x04000CF6 RID: 3318
		public ulong m_ControllerActionSetHandle;
	}
}
