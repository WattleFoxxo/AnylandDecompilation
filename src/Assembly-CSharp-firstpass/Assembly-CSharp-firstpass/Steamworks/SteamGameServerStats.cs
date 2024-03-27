using System;

namespace Steamworks
{
	// Token: 0x020001A0 RID: 416
	public static class SteamGameServerStats
	{
		// Token: 0x060006C2 RID: 1730 RVA: 0x000075E2 File Offset: 0x000057E2
		public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerStats_RequestUserStats(steamIDUser);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000075F4 File Offset: 0x000057F4
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_GetUserStat(steamIDUser, utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0000763C File Offset: 0x0000583C
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_GetUserStat_(steamIDUser, utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00007684 File Offset: 0x00005884
		public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_GetUserAchievement(steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return flag;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000076CC File Offset: 0x000058CC
		public static bool SetUserStat(CSteamID steamIDUser, string pchName, int nData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_SetUserStat(steamIDUser, utf8StringHandle, nData);
			}
			return flag;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00007714 File Offset: 0x00005914
		public static bool SetUserStat(CSteamID steamIDUser, string pchName, float fData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_SetUserStat_(steamIDUser, utf8StringHandle, fData);
			}
			return flag;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0000775C File Offset: 0x0000595C
		public static bool UpdateUserAvgRateStat(CSteamID steamIDUser, string pchName, float flCountThisSession, double dSessionLength)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_UpdateUserAvgRateStat(steamIDUser, utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return flag;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000077A4 File Offset: 0x000059A4
		public static bool SetUserAchievement(CSteamID steamIDUser, string pchName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_SetUserAchievement(steamIDUser, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000077E8 File Offset: 0x000059E8
		public static bool ClearUserAchievement(CSteamID steamIDUser, string pchName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_ClearUserAchievement(steamIDUser, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0000782C File Offset: 0x00005A2C
		public static SteamAPICall_t StoreUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerStats_StoreUserStats(steamIDUser);
		}
	}
}
