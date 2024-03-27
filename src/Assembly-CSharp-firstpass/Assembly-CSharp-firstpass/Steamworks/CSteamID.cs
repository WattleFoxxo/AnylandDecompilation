using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002F8 RID: 760
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CSteamID : IEquatable<CSteamID>, IComparable<CSteamID>
	{
		// Token: 0x06000D64 RID: 3428 RVA: 0x0000D6C1 File Offset: 0x0000B8C1
		public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.Set(unAccountID, eUniverse, eAccountType);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0000D6D4 File Offset: 0x0000B8D4
		public CSteamID(AccountID_t unAccountID, uint unAccountInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0000D6E9 File Offset: 0x0000B8E9
		public CSteamID(ulong ulSteamID)
		{
			this.m_SteamID = ulSteamID;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0000D6F2 File Offset: 0x0000B8F2
		public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			if (eAccountType == EAccountType.k_EAccountTypeClan || eAccountType == EAccountType.k_EAccountTypeGameServer)
			{
				this.SetAccountInstance(0U);
			}
			else
			{
				this.SetAccountInstance(1U);
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0000D72A File Offset: 0x0000B92A
		public void InstancedSet(AccountID_t unAccountID, uint unInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			this.SetAccountInstance(unInstance);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0000D749 File Offset: 0x0000B949
		public void Clear()
		{
			this.m_SteamID = 0UL;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0000D753 File Offset: 0x0000B953
		public void CreateBlankAnonLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonGameServer);
			this.SetAccountInstance(0U);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0000D776 File Offset: 0x0000B976
		public void CreateBlankAnonUserLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonUser);
			this.SetAccountInstance(0U);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0000D79A File Offset: 0x0000B99A
		public bool BBlankAnonAccount()
		{
			return this.GetAccountID() == new AccountID_t(0U) && this.BAnonAccount() && this.GetUnAccountInstance() == 0U;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0000D7C9 File Offset: 0x0000B9C9
		public bool BGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0000D7E3 File Offset: 0x0000B9E3
		public bool BPersistentGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0000D7EE File Offset: 0x0000B9EE
		public bool BAnonGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0000D7F9 File Offset: 0x0000B9F9
		public bool BContentServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeContentServer;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0000D804 File Offset: 0x0000BA04
		public bool BClanAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeClan;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0000D80F File Offset: 0x0000BA0F
		public bool BChatAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0000D81A File Offset: 0x0000BA1A
		public bool IsLobby()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat && (this.GetUnAccountInstance() & 262144U) != 0U;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0000D83D File Offset: 0x0000BA3D
		public bool BIndividualAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeIndividual || this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0000D858 File Offset: 0x0000BA58
		public bool BAnonAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0000D873 File Offset: 0x0000BA73
		public bool BAnonUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0000D87F File Offset: 0x0000BA7F
		public bool BConsoleUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0000D88B File Offset: 0x0000BA8B
		public void SetAccountID(AccountID_t other)
		{
			this.m_SteamID = (this.m_SteamID & 18446744069414584320UL) | (((ulong)(uint)other & (ulong)(-1)) << 0);
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0000D8B0 File Offset: 0x0000BAB0
		public void SetAccountInstance(uint other)
		{
			this.m_SteamID = (this.m_SteamID & 18442240478377148415UL) | (((ulong)other & 1048575UL) << 32);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0000D8D5 File Offset: 0x0000BAD5
		public void SetEAccountType(EAccountType other)
		{
			this.m_SteamID = (this.m_SteamID & 18379190079298994175UL) | (ulong)((ulong)((long)other & 15L) << 52);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0000D8F7 File Offset: 0x0000BAF7
		public void SetEUniverse(EUniverse other)
		{
			this.m_SteamID = (this.m_SteamID & 72057594037927935UL) | (ulong)((ulong)((long)other & 255L) << 56);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0000D91C File Offset: 0x0000BB1C
		public void ClearIndividualInstance()
		{
			if (this.BIndividualAccount())
			{
				this.SetAccountInstance(0U);
			}
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0000D930 File Offset: 0x0000BB30
		public bool HasNoIndividualInstance()
		{
			return this.BIndividualAccount() && this.GetUnAccountInstance() == 0U;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0000D949 File Offset: 0x0000BB49
		public AccountID_t GetAccountID()
		{
			return new AccountID_t((uint)(this.m_SteamID & (ulong)(-1)));
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0000D95A File Offset: 0x0000BB5A
		public uint GetUnAccountInstance()
		{
			return (uint)((this.m_SteamID >> 32) & 1048575UL);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0000D96D File Offset: 0x0000BB6D
		public EAccountType GetEAccountType()
		{
			return (EAccountType)((this.m_SteamID >> 52) & 15UL);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0000D97D File Offset: 0x0000BB7D
		public EUniverse GetEUniverse()
		{
			return (EUniverse)((this.m_SteamID >> 56) & 255UL);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0000D990 File Offset: 0x0000BB90
		public bool IsValid()
		{
			return this.GetEAccountType() > EAccountType.k_EAccountTypeInvalid && this.GetEAccountType() < EAccountType.k_EAccountTypeMax && this.GetEUniverse() > EUniverse.k_EUniverseInvalid && this.GetEUniverse() < EUniverse.k_EUniverseMax && (this.GetEAccountType() != EAccountType.k_EAccountTypeIndividual || (!(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() <= 4U)) && (this.GetEAccountType() != EAccountType.k_EAccountTypeClan || (!(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() == 0U)) && (this.GetEAccountType() != EAccountType.k_EAccountTypeGameServer || !(this.GetAccountID() == new AccountID_t(0U)));
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0000DA56 File Offset: 0x0000BC56
		public override string ToString()
		{
			return this.m_SteamID.ToString();
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0000DA69 File Offset: 0x0000BC69
		public override bool Equals(object other)
		{
			return other is CSteamID && this == (CSteamID)other;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0000DA8A File Offset: 0x0000BC8A
		public override int GetHashCode()
		{
			return this.m_SteamID.GetHashCode();
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0000DA9D File Offset: 0x0000BC9D
		public static bool operator ==(CSteamID x, CSteamID y)
		{
			return x.m_SteamID == y.m_SteamID;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0000DAAF File Offset: 0x0000BCAF
		public static bool operator !=(CSteamID x, CSteamID y)
		{
			return !(x == y);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0000DABB File Offset: 0x0000BCBB
		public static explicit operator CSteamID(ulong value)
		{
			return new CSteamID(value);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0000DAC3 File Offset: 0x0000BCC3
		public static explicit operator ulong(CSteamID that)
		{
			return that.m_SteamID;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0000DACC File Offset: 0x0000BCCC
		public bool Equals(CSteamID other)
		{
			return this.m_SteamID == other.m_SteamID;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0000DADD File Offset: 0x0000BCDD
		public int CompareTo(CSteamID other)
		{
			return this.m_SteamID.CompareTo(other.m_SteamID);
		}

		// Token: 0x04000CEE RID: 3310
		public static readonly CSteamID Nil = default(CSteamID);

		// Token: 0x04000CEF RID: 3311
		public static readonly CSteamID OutofDateGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000CF0 RID: 3312
		public static readonly CSteamID LanModeGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000CF1 RID: 3313
		public static readonly CSteamID NotInitYetGS = new CSteamID(new AccountID_t(1U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000CF2 RID: 3314
		public static readonly CSteamID NonSteamGS = new CSteamID(new AccountID_t(2U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000CF3 RID: 3315
		public ulong m_SteamID;
	}
}
