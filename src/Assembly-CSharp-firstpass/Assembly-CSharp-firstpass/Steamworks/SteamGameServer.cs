using System;

namespace Steamworks
{
	// Token: 0x0200019C RID: 412
	public static class SteamGameServer
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x000069AC File Offset: 0x00004BAC
		public static bool InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, AppId_t nGameAppId, string pchVersionString)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				flag = NativeMethods.ISteamGameServer_InitGameServer(unIP, usGamePort, usQueryPort, unFlags, nGameAppId, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000069F8 File Offset: 0x00004BF8
		public static void SetProduct(string pszProduct)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszProduct))
			{
				NativeMethods.ISteamGameServer_SetProduct(utf8StringHandle);
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00006A3C File Offset: 0x00004C3C
		public static void SetGameDescription(string pszGameDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszGameDescription))
			{
				NativeMethods.ISteamGameServer_SetGameDescription(utf8StringHandle);
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00006A80 File Offset: 0x00004C80
		public static void SetModDir(string pszModDir)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszModDir))
			{
				NativeMethods.ISteamGameServer_SetModDir(utf8StringHandle);
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00006AC4 File Offset: 0x00004CC4
		public static void SetDedicatedServer(bool bDedicated)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetDedicatedServer(bDedicated);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00006AD4 File Offset: 0x00004CD4
		public static void LogOn(string pszToken)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszToken))
			{
				NativeMethods.ISteamGameServer_LogOn(utf8StringHandle);
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00006B18 File Offset: 0x00004D18
		public static void LogOnAnonymous()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_LogOnAnonymous();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00006B24 File Offset: 0x00004D24
		public static void LogOff()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_LogOff();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00006B30 File Offset: 0x00004D30
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BLoggedOn();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00006B3C File Offset: 0x00004D3C
		public static bool BSecure()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BSecure();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00006B48 File Offset: 0x00004D48
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (CSteamID)NativeMethods.ISteamGameServer_GetSteamID();
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00006B59 File Offset: 0x00004D59
		public static bool WasRestartRequested()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_WasRestartRequested();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00006B65 File Offset: 0x00004D65
		public static void SetMaxPlayerCount(int cPlayersMax)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetMaxPlayerCount(cPlayersMax);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00006B72 File Offset: 0x00004D72
		public static void SetBotPlayerCount(int cBotplayers)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetBotPlayerCount(cBotplayers);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00006B80 File Offset: 0x00004D80
		public static void SetServerName(string pszServerName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszServerName))
			{
				NativeMethods.ISteamGameServer_SetServerName(utf8StringHandle);
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00006BC4 File Offset: 0x00004DC4
		public static void SetMapName(string pszMapName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszMapName))
			{
				NativeMethods.ISteamGameServer_SetMapName(utf8StringHandle);
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00006C08 File Offset: 0x00004E08
		public static void SetPasswordProtected(bool bPasswordProtected)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetPasswordProtected(bPasswordProtected);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00006C15 File Offset: 0x00004E15
		public static void SetSpectatorPort(ushort unSpectatorPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetSpectatorPort(unSpectatorPort);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00006C24 File Offset: 0x00004E24
		public static void SetSpectatorServerName(string pszSpectatorServerName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszSpectatorServerName))
			{
				NativeMethods.ISteamGameServer_SetSpectatorServerName(utf8StringHandle);
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00006C68 File Offset: 0x00004E68
		public static void ClearAllKeyValues()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_ClearAllKeyValues();
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00006C74 File Offset: 0x00004E74
		public static void SetKeyValue(string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					NativeMethods.ISteamGameServer_SetKeyValue(utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00006CDC File Offset: 0x00004EDC
		public static void SetGameTags(string pchGameTags)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchGameTags))
			{
				NativeMethods.ISteamGameServer_SetGameTags(utf8StringHandle);
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00006D20 File Offset: 0x00004F20
		public static void SetGameData(string pchGameData)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchGameData))
			{
				NativeMethods.ISteamGameServer_SetGameData(utf8StringHandle);
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00006D64 File Offset: 0x00004F64
		public static void SetRegion(string pszRegion)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszRegion))
			{
				NativeMethods.ISteamGameServer_SetRegion(utf8StringHandle);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00006DA8 File Offset: 0x00004FA8
		public static bool SendUserConnectAndAuthenticate(uint unIPClient, byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_SendUserConnectAndAuthenticate(unIPClient, pvAuthBlob, cubAuthBlobSize, out pSteamIDUser);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public static CSteamID CreateUnauthenticatedUserConnection()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (CSteamID)NativeMethods.ISteamGameServer_CreateUnauthenticatedUserConnection();
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00006DC9 File Offset: 0x00004FC9
		public static void SendUserDisconnect(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SendUserDisconnect(steamIDUser);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00006DD8 File Offset: 0x00004FD8
		public static bool BUpdateUserData(CSteamID steamIDUser, string pchPlayerName, uint uScore)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPlayerName))
			{
				flag = NativeMethods.ISteamGameServer_BUpdateUserData(steamIDUser, utf8StringHandle, uScore);
			}
			return flag;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00006E20 File Offset: 0x00005020
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HAuthTicket)NativeMethods.ISteamGameServer_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00006E34 File Offset: 0x00005034
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00006E43 File Offset: 0x00005043
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_EndAuthSession(steamID);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00006E50 File Offset: 0x00005050
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_CancelAuthTicket(hAuthTicket);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00006E5D File Offset: 0x0000505D
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_UserHasLicenseForApp(steamID, appID);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00006E6B File Offset: 0x0000506B
		public static bool RequestUserGroupStatus(CSteamID steamIDUser, CSteamID steamIDGroup)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_RequestUserGroupStatus(steamIDUser, steamIDGroup);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00006E79 File Offset: 0x00005079
		public static void GetGameplayStats()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_GetGameplayStats();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00006E85 File Offset: 0x00005085
		public static SteamAPICall_t GetServerReputation()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_GetServerReputation();
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00006E96 File Offset: 0x00005096
		public static uint GetPublicIP()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_GetPublicIP();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00006EA2 File Offset: 0x000050A2
		public static bool HandleIncomingPacket(byte[] pData, int cbData, uint srcIP, ushort srcPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_HandleIncomingPacket(pData, cbData, srcIP, srcPort);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00006EB2 File Offset: 0x000050B2
		public static int GetNextOutgoingPacket(byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_GetNextOutgoingPacket(pOut, cbMaxOut, out pNetAdr, out pPort);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00006EC2 File Offset: 0x000050C2
		public static void EnableHeartbeats(bool bActive)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_EnableHeartbeats(bActive);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00006ECF File Offset: 0x000050CF
		public static void SetHeartbeatInterval(int iHeartbeatInterval)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetHeartbeatInterval(iHeartbeatInterval);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00006EDC File Offset: 0x000050DC
		public static void ForceHeartbeat()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_ForceHeartbeat();
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00006EE8 File Offset: 0x000050E8
		public static SteamAPICall_t AssociateWithClan(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_AssociateWithClan(steamIDClan);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00006EFA File Offset: 0x000050FA
		public static SteamAPICall_t ComputeNewPlayerCompatibility(CSteamID steamIDNewPlayer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_ComputeNewPlayerCompatibility(steamIDNewPlayer);
		}
	}
}
