using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A2 RID: 418
	public static class SteamGameServerUtils
	{
		// Token: 0x0600070F RID: 1807 RVA: 0x0000823C File Offset: 0x0000643C
		public static uint GetSecondsSinceAppActive()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetSecondsSinceAppActive();
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00008248 File Offset: 0x00006448
		public static uint GetSecondsSinceComputerActive()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetSecondsSinceComputerActive();
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00008254 File Offset: 0x00006454
		public static EUniverse GetConnectedUniverse()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetConnectedUniverse();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00008260 File Offset: 0x00006460
		public static uint GetServerRealTime()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetServerRealTime();
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0000826C File Offset: 0x0000646C
		public static string GetIPCountry()
		{
			InteropHelp.TestIfAvailableGameServer();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetIPCountry());
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0000827D File Offset: 0x0000647D
		public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0000828C File Offset: 0x0000648C
		public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0000829B File Offset: 0x0000649B
		public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetCSERIPPort(out unIP, out usPort);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000082A9 File Offset: 0x000064A9
		public static byte GetCurrentBatteryPower()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetCurrentBatteryPower();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000082B5 File Offset: 0x000064B5
		public static AppId_t GetAppID()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (AppId_t)NativeMethods.ISteamGameServerUtils_GetAppID();
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x000082C6 File Offset: 0x000064C6
		public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetOverlayNotificationPosition(eNotificationPosition);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x000082D3 File Offset: 0x000064D3
		public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000082E1 File Offset: 0x000064E1
		public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetAPICallFailureReason(hSteamAPICall);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x000082EE File Offset: 0x000064EE
		public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00008300 File Offset: 0x00006500
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetIPCCallCount();
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0000830C File Offset: 0x0000650C
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetWarningMessageHook(pFunction);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00008319 File Offset: 0x00006519
		public static bool IsOverlayEnabled()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsOverlayEnabled();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00008325 File Offset: 0x00006525
		public static bool BOverlayNeedsPresent()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_BOverlayNeedsPresent();
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00008334 File Offset: 0x00006534
		public static SteamAPICall_t CheckFileSignature(string szFileName)
		{
			InteropHelp.TestIfAvailableGameServer();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(szFileName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamGameServerUtils_CheckFileSignature(utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0000837C File Offset: 0x0000657C
		public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
				{
					flag = NativeMethods.ISteamGameServerUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, utf8StringHandle, unCharMax, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x000083E4 File Offset: 0x000065E4
		public static uint GetEnteredGamepadTextLength()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextLength();
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000083F0 File Offset: 0x000065F0
		public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchText);
			bool flag = NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextInput(intPtr, cchText);
			pchText = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0000842C File Offset: 0x0000662C
		public static string GetSteamUILanguage()
		{
			InteropHelp.TestIfAvailableGameServer();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetSteamUILanguage());
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0000843D File Offset: 0x0000663D
		public static bool IsSteamRunningInVR()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsSteamRunningInVR();
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00008449 File Offset: 0x00006649
		public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00008457 File Offset: 0x00006657
		public static bool IsSteamInBigPictureMode()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsSteamInBigPictureMode();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00008463 File Offset: 0x00006663
		public static void StartVRDashboard()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_StartVRDashboard();
		}
	}
}
