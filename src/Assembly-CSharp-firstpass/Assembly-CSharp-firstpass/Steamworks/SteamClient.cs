using System;

namespace Steamworks
{
	// Token: 0x02000199 RID: 409
	public static class SteamClient
	{
		// Token: 0x060005CF RID: 1487 RVA: 0x000059C0 File Offset: 0x00003BC0
		public static HSteamPipe CreateSteamPipe()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamPipe)NativeMethods.ISteamClient_CreateSteamPipe();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000059D1 File Offset: 0x00003BD1
		public static bool BReleaseSteamPipe(HSteamPipe hSteamPipe)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_BReleaseSteamPipe(hSteamPipe);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000059DE File Offset: 0x00003BDE
		public static HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamClient_ConnectToGlobalUser(hSteamPipe);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000059F0 File Offset: 0x00003BF0
		public static HSteamUser CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamClient_CreateLocalUser(out phSteamPipe, eAccountType);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00005A03 File Offset: 0x00003C03
		public static void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_ReleaseUser(hSteamPipe, hUser);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00005A14 File Offset: 0x00003C14
		public static IntPtr GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUser(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00005A5C File Offset: 0x00003C5C
		public static IntPtr GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamGameServer(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00005AA4 File Offset: 0x00003CA4
		public static void SetLocalIPBinding(uint unIP, ushort usPort)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_SetLocalIPBinding(unIP, usPort);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00005AB4 File Offset: 0x00003CB4
		public static IntPtr GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamFriends(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00005AFC File Offset: 0x00003CFC
		public static IntPtr GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUtils(hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00005B40 File Offset: 0x00003D40
		public static IntPtr GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamMatchmaking(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00005B88 File Offset: 0x00003D88
		public static IntPtr GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamMatchmakingServers(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00005BD0 File Offset: 0x00003DD0
		public static IntPtr GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamGenericInterface(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00005C18 File Offset: 0x00003E18
		public static IntPtr GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUserStats(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00005C60 File Offset: 0x00003E60
		public static IntPtr GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamGameServerStats(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00005CA8 File Offset: 0x00003EA8
		public static IntPtr GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamApps(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00005CF0 File Offset: 0x00003EF0
		public static IntPtr GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamNetworking(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00005D38 File Offset: 0x00003F38
		public static IntPtr GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamRemoteStorage(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00005D80 File Offset: 0x00003F80
		public static IntPtr GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamScreenshots(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00005DC8 File Offset: 0x00003FC8
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_GetIPCCallCount();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00005DD4 File Offset: 0x00003FD4
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_SetWarningMessageHook(pFunction);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00005DE1 File Offset: 0x00003FE1
		public static bool BShutdownIfAllPipesClosed()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_BShutdownIfAllPipesClosed();
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00005DF0 File Offset: 0x00003FF0
		public static IntPtr GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamHTTP(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00005E38 File Offset: 0x00004038
		public static IntPtr GetISteamUnifiedMessages(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUnifiedMessages(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00005E80 File Offset: 0x00004080
		public static IntPtr GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamController(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00005EC8 File Offset: 0x000040C8
		public static IntPtr GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUGC(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00005F10 File Offset: 0x00004110
		public static IntPtr GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamAppList(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00005F58 File Offset: 0x00004158
		public static IntPtr GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamMusic(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00005FA0 File Offset: 0x000041A0
		public static IntPtr GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamMusicRemote(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00005FE8 File Offset: 0x000041E8
		public static IntPtr GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamHTMLSurface(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00006030 File Offset: 0x00004230
		public static IntPtr GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamInventory(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00006078 File Offset: 0x00004278
		public static IntPtr GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamVideo(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}
	}
}
