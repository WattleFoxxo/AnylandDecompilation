using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B0 RID: 432
	public static class SteamUserStats
	{
		// Token: 0x06000899 RID: 2201 RVA: 0x0000B11C File Offset: 0x0000931C
		public static bool RequestCurrentStats()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_RequestCurrentStats();
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0000B128 File Offset: 0x00009328
		public static bool GetStat(string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetStat(utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0000B16C File Offset: 0x0000936C
		public static bool GetStat(string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetStat_(utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0000B1B0 File Offset: 0x000093B0
		public static bool SetStat(string pchName, int nData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_SetStat(utf8StringHandle, nData);
			}
			return flag;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0000B1F4 File Offset: 0x000093F4
		public static bool SetStat(string pchName, float fData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_SetStat_(utf8StringHandle, fData);
			}
			return flag;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0000B238 File Offset: 0x00009438
		public static bool UpdateAvgRateStat(string pchName, float flCountThisSession, double dSessionLength)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_UpdateAvgRateStat(utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return flag;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0000B280 File Offset: 0x00009480
		public static bool GetAchievement(string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetAchievement(utf8StringHandle, out pbAchieved);
			}
			return flag;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0000B2C4 File Offset: 0x000094C4
		public static bool SetAchievement(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_SetAchievement(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0000B308 File Offset: 0x00009508
		public static bool ClearAchievement(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_ClearAchievement(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0000B34C File Offset: 0x0000954C
		public static bool GetAchievementAndUnlockTime(string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetAchievementAndUnlockTime(utf8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return flag;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0000B394 File Offset: 0x00009594
		public static bool StoreStats()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_StoreStats();
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public static int GetAchievementIcon(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			int num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				num = NativeMethods.ISteamUserStats_GetAchievementIcon(utf8StringHandle);
			}
			return num;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0000B3E4 File Offset: 0x000095E4
		public static string GetAchievementDisplayAttribute(string pchName, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string text;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchKey))
				{
					text = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementDisplayAttribute(utf8StringHandle, utf8StringHandle2));
				}
			}
			return text;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0000B450 File Offset: 0x00009650
		public static bool IndicateAchievementProgress(string pchName, uint nCurProgress, uint nMaxProgress)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_IndicateAchievementProgress(utf8StringHandle, nCurProgress, nMaxProgress);
			}
			return flag;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0000B498 File Offset: 0x00009698
		public static uint GetNumAchievements()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetNumAchievements();
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0000B4A4 File Offset: 0x000096A4
		public static string GetAchievementName(uint iAchievement)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementName(iAchievement));
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000B4B6 File Offset: 0x000096B6
		public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestUserStats(steamIDUser);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetUserStat(steamIDUser, utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0000B510 File Offset: 0x00009710
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetUserStat_(steamIDUser, utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0000B558 File Offset: 0x00009758
		public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetUserAchievement(steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return flag;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0000B5A0 File Offset: 0x000097A0
		public static bool GetUserAchievementAndUnlockTime(CSteamID steamIDUser, string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetUserAchievementAndUnlockTime(steamIDUser, utf8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return flag;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000B5E8 File Offset: 0x000097E8
		public static bool ResetAllStats(bool bAchievementsToo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_ResetAllStats(bAchievementsToo);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000B5F8 File Offset: 0x000097F8
		public static SteamAPICall_t FindOrCreateLeaderboard(string pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUserStats_FindOrCreateLeaderboard(utf8StringHandle, eLeaderboardSortMethod, eLeaderboardDisplayType);
			}
			return steamAPICall_t;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0000B644 File Offset: 0x00009844
		public static SteamAPICall_t FindLeaderboard(string pchLeaderboardName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUserStats_FindLeaderboard(utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0000B68C File Offset: 0x0000988C
		public static string GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetLeaderboardName(hSteamLeaderboard));
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0000B69E File Offset: 0x0000989E
		public static int GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardEntryCount(hSteamLeaderboard);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0000B6AB File Offset: 0x000098AB
		public static ELeaderboardSortMethod GetLeaderboardSortMethod(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardSortMethod(hSteamLeaderboard);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0000B6B8 File Offset: 0x000098B8
		public static ELeaderboardDisplayType GetLeaderboardDisplayType(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardDisplayType(hSteamLeaderboard);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0000B6C5 File Offset: 0x000098C5
		public static SteamAPICall_t DownloadLeaderboardEntries(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_DownloadLeaderboardEntries(hSteamLeaderboard, eLeaderboardDataRequest, nRangeStart, nRangeEnd);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0000B6DA File Offset: 0x000098DA
		public static SteamAPICall_t DownloadLeaderboardEntriesForUsers(SteamLeaderboard_t hSteamLeaderboard, CSteamID[] prgUsers, int cUsers)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_DownloadLeaderboardEntriesForUsers(hSteamLeaderboard, prgUsers, cUsers);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0000B6EE File Offset: 0x000098EE
		public static bool GetDownloadedLeaderboardEntry(SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, int[] pDetails, int cDetailsMax)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetDownloadedLeaderboardEntry(hSteamLeaderboardEntries, index, out pLeaderboardEntry, pDetails, cDetailsMax);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000B700 File Offset: 0x00009900
		public static SteamAPICall_t UploadLeaderboardScore(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, int[] pScoreDetails, int cScoreDetailsCount)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_UploadLeaderboardScore(hSteamLeaderboard, eLeaderboardUploadScoreMethod, nScore, pScoreDetails, cScoreDetailsCount);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0000B717 File Offset: 0x00009917
		public static SteamAPICall_t AttachLeaderboardUGC(SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_AttachLeaderboardUGC(hSteamLeaderboard, hUGC);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0000B72A File Offset: 0x0000992A
		public static SteamAPICall_t GetNumberOfCurrentPlayers()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_GetNumberOfCurrentPlayers();
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0000B73B File Offset: 0x0000993B
		public static SteamAPICall_t RequestGlobalAchievementPercentages()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestGlobalAchievementPercentages();
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0000B74C File Offset: 0x0000994C
		public static int GetMostAchievedAchievementInfo(out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)unNameBufLen);
			int num = NativeMethods.ISteamUserStats_GetMostAchievedAchievementInfo(intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num == -1) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0000B78C File Offset: 0x0000998C
		public static int GetNextMostAchievedAchievementInfo(int iIteratorPrevious, out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)unNameBufLen);
			int num = NativeMethods.ISteamUserStats_GetNextMostAchievedAchievementInfo(iIteratorPrevious, intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num == -1) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0000B7D0 File Offset: 0x000099D0
		public static bool GetAchievementAchievedPercent(string pchName, out float pflPercent)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetAchievementAchievedPercent(utf8StringHandle, out pflPercent);
			}
			return flag;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0000B814 File Offset: 0x00009A14
		public static SteamAPICall_t RequestGlobalStats(int nHistoryDays)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestGlobalStats(nHistoryDays);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0000B828 File Offset: 0x00009A28
		public static bool GetGlobalStat(string pchStatName, out long pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				flag = NativeMethods.ISteamUserStats_GetGlobalStat(utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0000B86C File Offset: 0x00009A6C
		public static bool GetGlobalStat(string pchStatName, out double pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				flag = NativeMethods.ISteamUserStats_GetGlobalStat_(utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0000B8B0 File Offset: 0x00009AB0
		public static int GetGlobalStatHistory(string pchStatName, long[] pData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			int num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				num = NativeMethods.ISteamUserStats_GetGlobalStatHistory(utf8StringHandle, pData, cubData);
			}
			return num;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0000B8F8 File Offset: 0x00009AF8
		public static int GetGlobalStatHistory(string pchStatName, double[] pData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			int num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				num = NativeMethods.ISteamUserStats_GetGlobalStatHistory_(utf8StringHandle, pData, cubData);
			}
			return num;
		}
	}
}
