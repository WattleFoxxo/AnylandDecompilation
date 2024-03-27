using System;

namespace Steamworks
{
	// Token: 0x02000315 RID: 789
	[Serializable]
	public struct ClientUnifiedMessageHandle : IEquatable<ClientUnifiedMessageHandle>, IComparable<ClientUnifiedMessageHandle>
	{
		// Token: 0x06000EB8 RID: 3768 RVA: 0x0000EE4E File Offset: 0x0000D04E
		public ClientUnifiedMessageHandle(ulong value)
		{
			this.m_ClientUnifiedMessageHandle = value;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0000EE57 File Offset: 0x0000D057
		public override string ToString()
		{
			return this.m_ClientUnifiedMessageHandle.ToString();
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0000EE6A File Offset: 0x0000D06A
		public override bool Equals(object other)
		{
			return other is ClientUnifiedMessageHandle && this == (ClientUnifiedMessageHandle)other;
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0000EE8B File Offset: 0x0000D08B
		public override int GetHashCode()
		{
			return this.m_ClientUnifiedMessageHandle.GetHashCode();
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0000EE9E File Offset: 0x0000D09E
		public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
		{
			return x.m_ClientUnifiedMessageHandle == y.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0000EEB0 File Offset: 0x0000D0B0
		public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0000EEBC File Offset: 0x0000D0BC
		public static explicit operator ClientUnifiedMessageHandle(ulong value)
		{
			return new ClientUnifiedMessageHandle(value);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0000EEC4 File Offset: 0x0000D0C4
		public static explicit operator ulong(ClientUnifiedMessageHandle that)
		{
			return that.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0000EECD File Offset: 0x0000D0CD
		public bool Equals(ClientUnifiedMessageHandle other)
		{
			return this.m_ClientUnifiedMessageHandle == other.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0000EEDE File Offset: 0x0000D0DE
		public int CompareTo(ClientUnifiedMessageHandle other)
		{
			return this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);
		}

		// Token: 0x04000D24 RID: 3364
		public static readonly ClientUnifiedMessageHandle Invalid = new ClientUnifiedMessageHandle(0UL);

		// Token: 0x04000D25 RID: 3365
		public ulong m_ClientUnifiedMessageHandle;
	}
}
