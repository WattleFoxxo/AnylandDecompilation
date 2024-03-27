using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000197 RID: 407
	public static class SteamAppList
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x000056B3 File Offset: 0x000038B3
		public static uint GetNumInstalledApps()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamAppList_GetNumInstalledApps();
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000056BF File Offset: 0x000038BF
		public static uint GetInstalledApps(AppId_t[] pvecAppID, uint unMaxAppIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamAppList_GetInstalledApps(pvecAppID, unMaxAppIDs);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000056D0 File Offset: 0x000038D0
		public static int GetAppName(AppId_t nAppID, out string pchName, int cchNameMax)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameMax);
			int num = NativeMethods.ISteamAppList_GetAppName(nAppID, intPtr, cchNameMax);
			pchName = ((num == -1) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00005710 File Offset: 0x00003910
		public static int GetAppInstallDir(AppId_t nAppID, out string pchDirectory, int cchNameMax)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameMax);
			int num = NativeMethods.ISteamAppList_GetAppInstallDir(nAppID, intPtr, cchNameMax);
			pchDirectory = ((num == -1) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0000574E File Offset: 0x0000394E
		public static int GetAppBuildId(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamAppList_GetAppBuildId(nAppID);
		}
	}
}
