using System;

namespace Steamworks
{
	// Token: 0x02000304 RID: 772
	[Serializable]
	public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t>
	{
		// Token: 0x06000E01 RID: 3585 RVA: 0x0000E2B5 File Offset: 0x0000C4B5
		public SteamItemInstanceID_t(ulong value)
		{
			this.m_SteamItemInstanceID = value;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0000E2BE File Offset: 0x0000C4BE
		public override string ToString()
		{
			return this.m_SteamItemInstanceID.ToString();
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0000E2D1 File Offset: 0x0000C4D1
		public override bool Equals(object other)
		{
			return other is SteamItemInstanceID_t && this == (SteamItemInstanceID_t)other;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0000E2F2 File Offset: 0x0000C4F2
		public override int GetHashCode()
		{
			return this.m_SteamItemInstanceID.GetHashCode();
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0000E305 File Offset: 0x0000C505
		public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return x.m_SteamItemInstanceID == y.m_SteamItemInstanceID;
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0000E317 File Offset: 0x0000C517
		public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0000E323 File Offset: 0x0000C523
		public static explicit operator SteamItemInstanceID_t(ulong value)
		{
			return new SteamItemInstanceID_t(value);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0000E32B File Offset: 0x0000C52B
		public static explicit operator ulong(SteamItemInstanceID_t that)
		{
			return that.m_SteamItemInstanceID;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0000E334 File Offset: 0x0000C534
		public bool Equals(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID == other.m_SteamItemInstanceID;
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0000E345 File Offset: 0x0000C545
		public int CompareTo(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);
		}

		// Token: 0x04000D05 RID: 3333
		public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);

		// Token: 0x04000D06 RID: 3334
		public ulong m_SteamItemInstanceID;
	}
}
