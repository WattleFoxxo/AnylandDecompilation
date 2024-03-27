using System;

namespace Steamworks
{
	// Token: 0x020002F6 RID: 758
	[Serializable]
	public struct CGameID : IEquatable<CGameID>, IComparable<CGameID>
	{
		// Token: 0x06000D4B RID: 3403 RVA: 0x0000D467 File Offset: 0x0000B667
		public CGameID(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0000D470 File Offset: 0x0000B670
		public CGameID(AppId_t nAppID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0000D481 File Offset: 0x0000B681
		public CGameID(AppId_t nAppID, uint nModID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
			this.SetType(CGameID.EGameIDType.k_EGameIDTypeGameMod);
			this.SetModID(nModID);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0000D4A0 File Offset: 0x0000B6A0
		public bool IsSteamApp()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeApp;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0000D4AB File Offset: 0x0000B6AB
		public bool IsMod()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeGameMod;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0000D4B6 File Offset: 0x0000B6B6
		public bool IsShortcut()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeShortcut;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0000D4C1 File Offset: 0x0000B6C1
		public bool IsP2PFile()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeP2P;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0000D4CC File Offset: 0x0000B6CC
		public AppId_t AppID()
		{
			return new AppId_t((uint)(this.m_GameID & 16777215UL));
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0000D4E1 File Offset: 0x0000B6E1
		public CGameID.EGameIDType Type()
		{
			return (CGameID.EGameIDType)((this.m_GameID >> 24) & 255UL);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		public uint ModID()
		{
			return (uint)((this.m_GameID >> 32) & (ulong)(-1));
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0000D504 File Offset: 0x0000B704
		public bool IsValid()
		{
			switch (this.Type())
			{
			case CGameID.EGameIDType.k_EGameIDTypeApp:
				return this.AppID() != AppId_t.Invalid;
			case CGameID.EGameIDType.k_EGameIDTypeGameMod:
				return this.AppID() != AppId_t.Invalid && (this.ModID() & 2147483648U) != 0U;
			case CGameID.EGameIDType.k_EGameIDTypeShortcut:
				return (this.ModID() & 2147483648U) != 0U;
			case CGameID.EGameIDType.k_EGameIDTypeP2P:
				return this.AppID() == AppId_t.Invalid && (this.ModID() & 2147483648U) != 0U;
			default:
				return false;
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0000D5AE File Offset: 0x0000B7AE
		public void Reset()
		{
			this.m_GameID = 0UL;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0000D5B8 File Offset: 0x0000B7B8
		public void Set(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0000D5C1 File Offset: 0x0000B7C1
		private void SetAppID(AppId_t other)
		{
			this.m_GameID = (this.m_GameID & 18446744073692774400UL) | (((ulong)(uint)other & 16777215UL) << 0);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0000D5E7 File Offset: 0x0000B7E7
		private void SetType(CGameID.EGameIDType other)
		{
			this.m_GameID = (this.m_GameID & 18446744069431361535UL) | (ulong)((ulong)((long)other & 255L) << 24);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0000D60C File Offset: 0x0000B80C
		private void SetModID(uint other)
		{
			this.m_GameID = (this.m_GameID & (ulong)(-1)) | (((ulong)other & (ulong)(-1)) << 32);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0000D626 File Offset: 0x0000B826
		public override string ToString()
		{
			return this.m_GameID.ToString();
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0000D639 File Offset: 0x0000B839
		public override bool Equals(object other)
		{
			return other is CGameID && this == (CGameID)other;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0000D65A File Offset: 0x0000B85A
		public override int GetHashCode()
		{
			return this.m_GameID.GetHashCode();
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0000D66D File Offset: 0x0000B86D
		public static bool operator ==(CGameID x, CGameID y)
		{
			return x.m_GameID == y.m_GameID;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0000D67F File Offset: 0x0000B87F
		public static bool operator !=(CGameID x, CGameID y)
		{
			return !(x == y);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0000D68B File Offset: 0x0000B88B
		public static explicit operator CGameID(ulong value)
		{
			return new CGameID(value);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0000D693 File Offset: 0x0000B893
		public static explicit operator ulong(CGameID that)
		{
			return that.m_GameID;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0000D69C File Offset: 0x0000B89C
		public bool Equals(CGameID other)
		{
			return this.m_GameID == other.m_GameID;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0000D6AD File Offset: 0x0000B8AD
		public int CompareTo(CGameID other)
		{
			return this.m_GameID.CompareTo(other.m_GameID);
		}

		// Token: 0x04000CE8 RID: 3304
		public ulong m_GameID;

		// Token: 0x020002F7 RID: 759
		public enum EGameIDType
		{
			// Token: 0x04000CEA RID: 3306
			k_EGameIDTypeApp,
			// Token: 0x04000CEB RID: 3307
			k_EGameIDTypeGameMod,
			// Token: 0x04000CEC RID: 3308
			k_EGameIDTypeShortcut,
			// Token: 0x04000CED RID: 3309
			k_EGameIDTypeP2P
		}
	}
}
