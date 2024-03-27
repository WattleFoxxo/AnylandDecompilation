using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B1 RID: 433
	public static class SteamUtils
	{
		// Token: 0x060008C4 RID: 2244 RVA: 0x0000B940 File Offset: 0x00009B40
		public static uint GetSecondsSinceAppActive()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetSecondsSinceAppActive();
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0000B94C File Offset: 0x00009B4C
		public static uint GetSecondsSinceComputerActive()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetSecondsSinceComputerActive();
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0000B958 File Offset: 0x00009B58
		public static EUniverse GetConnectedUniverse()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetConnectedUniverse();
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0000B964 File Offset: 0x00009B64
		public static uint GetServerRealTime()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetServerRealTime();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0000B970 File Offset: 0x00009B70
		public static string GetIPCountry()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetIPCountry());
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0000B981 File Offset: 0x00009B81
		public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0000B990 File Offset: 0x00009B90
		public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0000B99F File Offset: 0x00009B9F
		public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetCSERIPPort(out unIP, out usPort);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0000B9AD File Offset: 0x00009BAD
		public static byte GetCurrentBatteryPower()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetCurrentBatteryPower();
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0000B9B9 File Offset: 0x00009BB9
		public static AppId_t GetAppID()
		{
			InteropHelp.TestIfAvailableClient();
			return (AppId_t)NativeMethods.ISteamUtils_GetAppID();
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0000B9CA File Offset: 0x00009BCA
		public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetOverlayNotificationPosition(eNotificationPosition);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0000B9D7 File Offset: 0x00009BD7
		public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0000B9E5 File Offset: 0x00009BE5
		public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetAPICallFailureReason(hSteamAPICall);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0000B9F2 File Offset: 0x00009BF2
		public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0000BA04 File Offset: 0x00009C04
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetIPCCallCount();
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0000BA10 File Offset: 0x00009C10
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetWarningMessageHook(pFunction);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0000BA1D File Offset: 0x00009C1D
		public static bool IsOverlayEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsOverlayEnabled();
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0000BA29 File Offset: 0x00009C29
		public static bool BOverlayNeedsPresent()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_BOverlayNeedsPresent();
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0000BA38 File Offset: 0x00009C38
		public static SteamAPICall_t CheckFileSignature(string szFileName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(szFileName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUtils_CheckFileSignature(utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000BA80 File Offset: 0x00009C80
		public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
				{
					flag = NativeMethods.ISteamUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, utf8StringHandle, unCharMax, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0000BAE8 File Offset: 0x00009CE8
		public static uint GetEnteredGamepadTextLength()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetEnteredGamepadTextLength();
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchText);
			bool flag = NativeMethods.ISteamUtils_GetEnteredGamepadTextInput(intPtr, cchText);
			pchText = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0000BB30 File Offset: 0x00009D30
		public static string GetSteamUILanguage()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetSteamUILanguage());
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0000BB41 File Offset: 0x00009D41
		public static bool IsSteamRunningInVR()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamRunningInVR();
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0000BB4D File Offset: 0x00009D4D
		public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0000BB5B File Offset: 0x00009D5B
		public static bool IsSteamInBigPictureMode()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamInBigPictureMode();
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0000BB67 File Offset: 0x00009D67
		public static void StartVRDashboard()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_StartVRDashboard();
		}
	}
}
