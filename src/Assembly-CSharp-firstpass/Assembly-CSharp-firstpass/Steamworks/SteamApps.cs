using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000198 RID: 408
	public static class SteamApps
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x0000575B File Offset: 0x0000395B
		public static bool BIsSubscribed()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribed();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00005767 File Offset: 0x00003967
		public static bool BIsLowViolence()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsLowViolence();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00005773 File Offset: 0x00003973
		public static bool BIsCybercafe()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsCybercafe();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000577F File Offset: 0x0000397F
		public static bool BIsVACBanned()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsVACBanned();
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0000578B File Offset: 0x0000398B
		public static string GetCurrentGameLanguage()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetCurrentGameLanguage());
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000579C File Offset: 0x0000399C
		public static string GetAvailableGameLanguages()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetAvailableGameLanguages());
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000057AD File Offset: 0x000039AD
		public static bool BIsSubscribedApp(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribedApp(appID);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000057BA File Offset: 0x000039BA
		public static bool BIsDlcInstalled(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsDlcInstalled(appID);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000057C7 File Offset: 0x000039C7
		public static uint GetEarliestPurchaseUnixTime(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(nAppID);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000057D4 File Offset: 0x000039D4
		public static bool BIsSubscribedFromFreeWeekend()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend();
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000057E0 File Offset: 0x000039E0
		public static int GetDLCCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetDLCCount();
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000057EC File Offset: 0x000039EC
		public static bool BGetDLCDataByIndex(int iDLC, out AppId_t pAppID, out bool pbAvailable, out string pchName, int cchNameBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameBufferSize);
			bool flag = NativeMethods.ISteamApps_BGetDLCDataByIndex(iDLC, out pAppID, out pbAvailable, intPtr, cchNameBufferSize);
			pchName = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000582D File Offset: 0x00003A2D
		public static void InstallDLC(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_InstallDLC(nAppID);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0000583A File Offset: 0x00003A3A
		public static void UninstallDLC(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_UninstallDLC(nAppID);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00005847 File Offset: 0x00003A47
		public static void RequestAppProofOfPurchaseKey(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(nAppID);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00005854 File Offset: 0x00003A54
		public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameBufferSize);
			bool flag = NativeMethods.ISteamApps_GetCurrentBetaName(intPtr, cchNameBufferSize);
			pchName = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00005890 File Offset: 0x00003A90
		public static bool MarkContentCorrupt(bool bMissingFilesOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_MarkContentCorrupt(bMissingFilesOnly);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0000589D File Offset: 0x00003A9D
		public static uint GetInstalledDepots(AppId_t appID, DepotId_t[] pvecDepots, uint cMaxDepots)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetInstalledDepots(appID, pvecDepots, cMaxDepots);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000058AC File Offset: 0x00003AAC
		public static uint GetAppInstallDir(AppId_t appID, out string pchFolder, uint cchFolderBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderBufferSize);
			uint num = NativeMethods.ISteamApps_GetAppInstallDir(appID, intPtr, cchFolderBufferSize);
			pchFolder = ((num == 0U) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000058E9 File Offset: 0x00003AE9
		public static bool BIsAppInstalled(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsAppInstalled(appID);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000058F6 File Offset: 0x00003AF6
		public static CSteamID GetAppOwner()
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamApps_GetAppOwner();
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00005908 File Offset: 0x00003B08
		public static string GetLaunchQueryParam(string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string text;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				text = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetLaunchQueryParam(utf8StringHandle));
			}
			return text;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00005950 File Offset: 0x00003B50
		public static bool GetDlcDownloadProgress(AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetDlcDownloadProgress(nAppID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0000595F File Offset: 0x00003B5F
		public static int GetAppBuildId()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetAppBuildId();
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000596B File Offset: 0x00003B6B
		public static void RequestAllProofOfPurchaseKeys()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_RequestAllProofOfPurchaseKeys();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00005978 File Offset: 0x00003B78
		public static SteamAPICall_t GetFileDetails(string pszFileName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszFileName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamApps_GetFileDetails(utf8StringHandle);
			}
			return steamAPICall_t;
		}
	}
}
