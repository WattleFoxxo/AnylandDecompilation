using System;

namespace Steamworks
{
	// Token: 0x020001AC RID: 428
	public static class SteamScreenshots
	{
		// Token: 0x0600082B RID: 2091 RVA: 0x0000A28C File Offset: 0x0000848C
		public static ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return (ScreenshotHandle)NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0000A2A4 File Offset: 0x000084A4
		public static ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight)
		{
			InteropHelp.TestIfAvailableClient();
			ScreenshotHandle screenshotHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFilename))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchThumbnailFilename))
				{
					screenshotHandle = (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(utf8StringHandle, utf8StringHandle2, nWidth, nHeight);
				}
			}
			return screenshotHandle;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0000A310 File Offset: 0x00008510
		public static void TriggerScreenshot()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_TriggerScreenshot();
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0000A31C File Offset: 0x0000851C
		public static void HookScreenshots(bool bHook)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0000A32C File Offset: 0x0000852C
		public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLocation))
			{
				flag = NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0000A370 File Offset: 0x00008570
		public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0000A37E File Offset: 0x0000857E
		public static bool TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0000A38C File Offset: 0x0000858C
		public static bool IsScreenshotsHooked()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_IsScreenshotsHooked();
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0000A398 File Offset: 0x00008598
		public static ScreenshotHandle AddVRScreenshotToLibrary(EVRScreenshotType eType, string pchFilename, string pchVRFilename)
		{
			InteropHelp.TestIfAvailableClient();
			ScreenshotHandle screenshotHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFilename))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchVRFilename))
				{
					screenshotHandle = (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddVRScreenshotToLibrary(eType, utf8StringHandle, utf8StringHandle2);
				}
			}
			return screenshotHandle;
		}
	}
}
