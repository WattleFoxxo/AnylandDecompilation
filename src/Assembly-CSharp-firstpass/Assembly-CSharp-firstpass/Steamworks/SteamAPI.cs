using System;

namespace Steamworks
{
	// Token: 0x020002ED RID: 749
	public static class SteamAPI
	{
		// Token: 0x06000CF8 RID: 3320 RVA: 0x0000CD8B File Offset: 0x0000AF8B
		public static bool InitSafe()
		{
			return SteamAPI.Init();
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0000CD92 File Offset: 0x0000AF92
		public static bool Init()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_Init();
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0000CD9E File Offset: 0x0000AF9E
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_Shutdown();
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0000CDAA File Offset: 0x0000AFAA
		public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0000CDB7 File Offset: 0x0000AFB7
		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_ReleaseCurrentThreadMemory();
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0000CDC3 File Offset: 0x0000AFC3
		public static void RunCallbacks()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_RunCallbacks();
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0000CDCF File Offset: 0x0000AFCF
		public static bool IsSteamRunning()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_IsSteamRunning();
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0000CDDB File Offset: 0x0000AFDB
		public static HSteamUser GetHSteamUserCurrent()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.Steam_GetHSteamUserCurrent();
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0000CDEC File Offset: 0x0000AFEC
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamAPI_GetHSteamPipe();
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0000CDFD File Offset: 0x0000AFFD
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamAPI_GetHSteamUser();
		}
	}
}
