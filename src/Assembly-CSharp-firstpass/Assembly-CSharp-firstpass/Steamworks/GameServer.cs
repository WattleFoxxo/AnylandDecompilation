using System;

namespace Steamworks
{
	// Token: 0x020002EE RID: 750
	public static class GameServer
	{
		// Token: 0x06000D02 RID: 3330 RVA: 0x0000CE10 File Offset: 0x0000B010
		public static bool Init(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString)
		{
			InteropHelp.TestIfPlatformSupported();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				flag = NativeMethods.SteamGameServer_Init(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_Shutdown();
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0000CE68 File Offset: 0x0000B068
		public static void RunCallbacks()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_RunCallbacks();
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0000CE74 File Offset: 0x0000B074
		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_ReleaseCurrentThreadMemory();
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0000CE80 File Offset: 0x0000B080
		public static bool BSecure()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamGameServer_BSecure();
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0000CE8C File Offset: 0x0000B08C
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfPlatformSupported();
			return (CSteamID)NativeMethods.SteamGameServer_GetSteamID();
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0000CE9D File Offset: 0x0000B09D
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamGameServer_GetHSteamPipe();
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0000CEAE File Offset: 0x0000B0AE
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamGameServer_GetHSteamUser();
		}
	}
}
