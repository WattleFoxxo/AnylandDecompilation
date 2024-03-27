using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A7 RID: 423
	public static class SteamMatchmakingServers
	{
		// Token: 0x060007A4 RID: 1956 RVA: 0x000092CD File Offset: 0x000074CD
		public static HServerListRequest RequestInternetServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestInternetServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000092F1 File Offset: 0x000074F1
		public static HServerListRequest RequestLANServerList(AppId_t iApp, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestLANServerList(iApp, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00009309 File Offset: 0x00007509
		public static HServerListRequest RequestFriendsServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestFriendsServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0000932D File Offset: 0x0000752D
		public static HServerListRequest RequestFavoritesServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestFavoritesServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00009351 File Offset: 0x00007551
		public static HServerListRequest RequestHistoryServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestHistoryServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00009375 File Offset: 0x00007575
		public static HServerListRequest RequestSpectatorServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestSpectatorServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00009399 File Offset: 0x00007599
		public static void ReleaseRequest(HServerListRequest hServerListRequest)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_ReleaseRequest(hServerListRequest);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x000093A6 File Offset: 0x000075A6
		public static gameserveritem_t GetServerDetails(HServerListRequest hRequest, int iServer)
		{
			InteropHelp.TestIfAvailableClient();
			return (gameserveritem_t)Marshal.PtrToStructure(NativeMethods.ISteamMatchmakingServers_GetServerDetails(hRequest, iServer), typeof(gameserveritem_t));
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x000093C8 File Offset: 0x000075C8
		public static void CancelQuery(HServerListRequest hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_CancelQuery(hRequest);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x000093D5 File Offset: 0x000075D5
		public static void RefreshQuery(HServerListRequest hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_RefreshQuery(hRequest);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x000093E2 File Offset: 0x000075E2
		public static bool IsRefreshing(HServerListRequest hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmakingServers_IsRefreshing(hRequest);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000093EF File Offset: 0x000075EF
		public static int GetServerCount(HServerListRequest hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmakingServers_GetServerCount(hRequest);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000093FC File Offset: 0x000075FC
		public static void RefreshServer(HServerListRequest hRequest, int iServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_RefreshServer(hRequest, iServer);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0000940A File Offset: 0x0000760A
		public static HServerQuery PingServer(uint unIP, ushort usPort, ISteamMatchmakingPingResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerQuery)NativeMethods.ISteamMatchmakingServers_PingServer(unIP, usPort, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00009423 File Offset: 0x00007623
		public static HServerQuery PlayerDetails(uint unIP, ushort usPort, ISteamMatchmakingPlayersResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerQuery)NativeMethods.ISteamMatchmakingServers_PlayerDetails(unIP, usPort, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0000943C File Offset: 0x0000763C
		public static HServerQuery ServerRules(uint unIP, ushort usPort, ISteamMatchmakingRulesResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerQuery)NativeMethods.ISteamMatchmakingServers_ServerRules(unIP, usPort, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00009455 File Offset: 0x00007655
		public static void CancelServerQuery(HServerQuery hServerQuery)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_CancelServerQuery(hServerQuery);
		}
	}
}
