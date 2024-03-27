using System;

namespace Steamworks
{
	// Token: 0x02000262 RID: 610
	public static class Constants
	{
		// Token: 0x040007DA RID: 2010
		public const string STEAMAPPLIST_INTERFACE_VERSION = "STEAMAPPLIST_INTERFACE_VERSION001";

		// Token: 0x040007DB RID: 2011
		public const string STEAMAPPS_INTERFACE_VERSION = "STEAMAPPS_INTERFACE_VERSION008";

		// Token: 0x040007DC RID: 2012
		public const string STEAMAPPTICKET_INTERFACE_VERSION = "STEAMAPPTICKET_INTERFACE_VERSION001";

		// Token: 0x040007DD RID: 2013
		public const string STEAMCLIENT_INTERFACE_VERSION = "SteamClient017";

		// Token: 0x040007DE RID: 2014
		public const string STEAMCONTROLLER_INTERFACE_VERSION = "SteamController005";

		// Token: 0x040007DF RID: 2015
		public const string STEAMFRIENDS_INTERFACE_VERSION = "SteamFriends015";

		// Token: 0x040007E0 RID: 2016
		public const string STEAMGAMECOORDINATOR_INTERFACE_VERSION = "SteamGameCoordinator001";

		// Token: 0x040007E1 RID: 2017
		public const string STEAMGAMESERVER_INTERFACE_VERSION = "SteamGameServer012";

		// Token: 0x040007E2 RID: 2018
		public const string STEAMGAMESERVERSTATS_INTERFACE_VERSION = "SteamGameServerStats001";

		// Token: 0x040007E3 RID: 2019
		public const string STEAMHTMLSURFACE_INTERFACE_VERSION = "STEAMHTMLSURFACE_INTERFACE_VERSION_003";

		// Token: 0x040007E4 RID: 2020
		public const string STEAMHTTP_INTERFACE_VERSION = "STEAMHTTP_INTERFACE_VERSION002";

		// Token: 0x040007E5 RID: 2021
		public const string STEAMINVENTORY_INTERFACE_VERSION = "STEAMINVENTORY_INTERFACE_V001";

		// Token: 0x040007E6 RID: 2022
		public const string STEAMMATCHMAKING_INTERFACE_VERSION = "SteamMatchMaking009";

		// Token: 0x040007E7 RID: 2023
		public const string STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION = "SteamMatchMakingServers002";

		// Token: 0x040007E8 RID: 2024
		public const string STEAMMUSIC_INTERFACE_VERSION = "STEAMMUSIC_INTERFACE_VERSION001";

		// Token: 0x040007E9 RID: 2025
		public const string STEAMMUSICREMOTE_INTERFACE_VERSION = "STEAMMUSICREMOTE_INTERFACE_VERSION001";

		// Token: 0x040007EA RID: 2026
		public const string STEAMNETWORKING_INTERFACE_VERSION = "SteamNetworking005";

		// Token: 0x040007EB RID: 2027
		public const string STEAMREMOTESTORAGE_INTERFACE_VERSION = "STEAMREMOTESTORAGE_INTERFACE_VERSION014";

		// Token: 0x040007EC RID: 2028
		public const string STEAMSCREENSHOTS_INTERFACE_VERSION = "STEAMSCREENSHOTS_INTERFACE_VERSION003";

		// Token: 0x040007ED RID: 2029
		public const string STEAMUGC_INTERFACE_VERSION = "STEAMUGC_INTERFACE_VERSION009";

		// Token: 0x040007EE RID: 2030
		public const string STEAMUNIFIEDMESSAGES_INTERFACE_VERSION = "STEAMUNIFIEDMESSAGES_INTERFACE_VERSION001";

		// Token: 0x040007EF RID: 2031
		public const string STEAMUSER_INTERFACE_VERSION = "SteamUser019";

		// Token: 0x040007F0 RID: 2032
		public const string STEAMUSERSTATS_INTERFACE_VERSION = "STEAMUSERSTATS_INTERFACE_VERSION011";

		// Token: 0x040007F1 RID: 2033
		public const string STEAMUTILS_INTERFACE_VERSION = "SteamUtils008";

		// Token: 0x040007F2 RID: 2034
		public const string STEAMVIDEO_INTERFACE_VERSION = "STEAMVIDEO_INTERFACE_V001";

		// Token: 0x040007F3 RID: 2035
		public const int k_cubAppProofOfPurchaseKeyMax = 240;

		// Token: 0x040007F4 RID: 2036
		public const int k_iSteamUserCallbacks = 100;

		// Token: 0x040007F5 RID: 2037
		public const int k_iSteamGameServerCallbacks = 200;

		// Token: 0x040007F6 RID: 2038
		public const int k_iSteamFriendsCallbacks = 300;

		// Token: 0x040007F7 RID: 2039
		public const int k_iSteamBillingCallbacks = 400;

		// Token: 0x040007F8 RID: 2040
		public const int k_iSteamMatchmakingCallbacks = 500;

		// Token: 0x040007F9 RID: 2041
		public const int k_iSteamContentServerCallbacks = 600;

		// Token: 0x040007FA RID: 2042
		public const int k_iSteamUtilsCallbacks = 700;

		// Token: 0x040007FB RID: 2043
		public const int k_iClientFriendsCallbacks = 800;

		// Token: 0x040007FC RID: 2044
		public const int k_iClientUserCallbacks = 900;

		// Token: 0x040007FD RID: 2045
		public const int k_iSteamAppsCallbacks = 1000;

		// Token: 0x040007FE RID: 2046
		public const int k_iSteamUserStatsCallbacks = 1100;

		// Token: 0x040007FF RID: 2047
		public const int k_iSteamNetworkingCallbacks = 1200;

		// Token: 0x04000800 RID: 2048
		public const int k_iClientRemoteStorageCallbacks = 1300;

		// Token: 0x04000801 RID: 2049
		public const int k_iClientDepotBuilderCallbacks = 1400;

		// Token: 0x04000802 RID: 2050
		public const int k_iSteamGameServerItemsCallbacks = 1500;

		// Token: 0x04000803 RID: 2051
		public const int k_iClientUtilsCallbacks = 1600;

		// Token: 0x04000804 RID: 2052
		public const int k_iSteamGameCoordinatorCallbacks = 1700;

		// Token: 0x04000805 RID: 2053
		public const int k_iSteamGameServerStatsCallbacks = 1800;

		// Token: 0x04000806 RID: 2054
		public const int k_iSteam2AsyncCallbacks = 1900;

		// Token: 0x04000807 RID: 2055
		public const int k_iSteamGameStatsCallbacks = 2000;

		// Token: 0x04000808 RID: 2056
		public const int k_iClientHTTPCallbacks = 2100;

		// Token: 0x04000809 RID: 2057
		public const int k_iClientScreenshotsCallbacks = 2200;

		// Token: 0x0400080A RID: 2058
		public const int k_iSteamScreenshotsCallbacks = 2300;

		// Token: 0x0400080B RID: 2059
		public const int k_iClientAudioCallbacks = 2400;

		// Token: 0x0400080C RID: 2060
		public const int k_iClientUnifiedMessagesCallbacks = 2500;

		// Token: 0x0400080D RID: 2061
		public const int k_iSteamStreamLauncherCallbacks = 2600;

		// Token: 0x0400080E RID: 2062
		public const int k_iClientControllerCallbacks = 2700;

		// Token: 0x0400080F RID: 2063
		public const int k_iSteamControllerCallbacks = 2800;

		// Token: 0x04000810 RID: 2064
		public const int k_iClientParentalSettingsCallbacks = 2900;

		// Token: 0x04000811 RID: 2065
		public const int k_iClientDeviceAuthCallbacks = 3000;

		// Token: 0x04000812 RID: 2066
		public const int k_iClientNetworkDeviceManagerCallbacks = 3100;

		// Token: 0x04000813 RID: 2067
		public const int k_iClientMusicCallbacks = 3200;

		// Token: 0x04000814 RID: 2068
		public const int k_iClientRemoteClientManagerCallbacks = 3300;

		// Token: 0x04000815 RID: 2069
		public const int k_iClientUGCCallbacks = 3400;

		// Token: 0x04000816 RID: 2070
		public const int k_iSteamStreamClientCallbacks = 3500;

		// Token: 0x04000817 RID: 2071
		public const int k_IClientProductBuilderCallbacks = 3600;

		// Token: 0x04000818 RID: 2072
		public const int k_iClientShortcutsCallbacks = 3700;

		// Token: 0x04000819 RID: 2073
		public const int k_iClientRemoteControlManagerCallbacks = 3800;

		// Token: 0x0400081A RID: 2074
		public const int k_iSteamAppListCallbacks = 3900;

		// Token: 0x0400081B RID: 2075
		public const int k_iSteamMusicCallbacks = 4000;

		// Token: 0x0400081C RID: 2076
		public const int k_iSteamMusicRemoteCallbacks = 4100;

		// Token: 0x0400081D RID: 2077
		public const int k_iClientVRCallbacks = 4200;

		// Token: 0x0400081E RID: 2078
		public const int k_iClientGameNotificationCallbacks = 4300;

		// Token: 0x0400081F RID: 2079
		public const int k_iSteamGameNotificationCallbacks = 4400;

		// Token: 0x04000820 RID: 2080
		public const int k_iSteamHTMLSurfaceCallbacks = 4500;

		// Token: 0x04000821 RID: 2081
		public const int k_iClientVideoCallbacks = 4600;

		// Token: 0x04000822 RID: 2082
		public const int k_iClientInventoryCallbacks = 4700;

		// Token: 0x04000823 RID: 2083
		public const int k_iClientBluetoothManagerCallbacks = 4800;

		// Token: 0x04000824 RID: 2084
		public const int k_cchMaxFriendsGroupName = 64;

		// Token: 0x04000825 RID: 2085
		public const int k_cFriendsGroupLimit = 100;

		// Token: 0x04000826 RID: 2086
		public const int k_cEnumerateFollowersMax = 50;

		// Token: 0x04000827 RID: 2087
		public const int k_cchPersonaNameMax = 128;

		// Token: 0x04000828 RID: 2088
		public const int k_cwchPersonaNameMax = 32;

		// Token: 0x04000829 RID: 2089
		public const int k_cubChatMetadataMax = 8192;

		// Token: 0x0400082A RID: 2090
		public const int k_cchMaxRichPresenceKeys = 20;

		// Token: 0x0400082B RID: 2091
		public const int k_cchMaxRichPresenceKeyLength = 64;

		// Token: 0x0400082C RID: 2092
		public const int k_cchMaxRichPresenceValueLength = 256;

		// Token: 0x0400082D RID: 2093
		public const int k_unServerFlagNone = 0;

		// Token: 0x0400082E RID: 2094
		public const int k_unServerFlagActive = 1;

		// Token: 0x0400082F RID: 2095
		public const int k_unServerFlagSecure = 2;

		// Token: 0x04000830 RID: 2096
		public const int k_unServerFlagDedicated = 4;

		// Token: 0x04000831 RID: 2097
		public const int k_unServerFlagLinux = 8;

		// Token: 0x04000832 RID: 2098
		public const int k_unServerFlagPassworded = 16;

		// Token: 0x04000833 RID: 2099
		public const int k_unServerFlagPrivate = 32;

		// Token: 0x04000834 RID: 2100
		public const int k_unFavoriteFlagNone = 0;

		// Token: 0x04000835 RID: 2101
		public const int k_unFavoriteFlagFavorite = 1;

		// Token: 0x04000836 RID: 2102
		public const int k_unFavoriteFlagHistory = 2;

		// Token: 0x04000837 RID: 2103
		public const int k_unMaxCloudFileChunkSize = 104857600;

		// Token: 0x04000838 RID: 2104
		public const int k_cchPublishedDocumentTitleMax = 129;

		// Token: 0x04000839 RID: 2105
		public const int k_cchPublishedDocumentDescriptionMax = 8000;

		// Token: 0x0400083A RID: 2106
		public const int k_cchPublishedDocumentChangeDescriptionMax = 8000;

		// Token: 0x0400083B RID: 2107
		public const int k_unEnumeratePublishedFilesMaxResults = 50;

		// Token: 0x0400083C RID: 2108
		public const int k_cchTagListMax = 1025;

		// Token: 0x0400083D RID: 2109
		public const int k_cchFilenameMax = 260;

		// Token: 0x0400083E RID: 2110
		public const int k_cchPublishedFileURLMax = 256;

		// Token: 0x0400083F RID: 2111
		public const int k_nScreenshotMaxTaggedUsers = 32;

		// Token: 0x04000840 RID: 2112
		public const int k_nScreenshotMaxTaggedPublishedFiles = 32;

		// Token: 0x04000841 RID: 2113
		public const int k_cubUFSTagTypeMax = 255;

		// Token: 0x04000842 RID: 2114
		public const int k_cubUFSTagValueMax = 255;

		// Token: 0x04000843 RID: 2115
		public const int k_ScreenshotThumbWidth = 200;

		// Token: 0x04000844 RID: 2116
		public const int kNumUGCResultsPerPage = 50;

		// Token: 0x04000845 RID: 2117
		public const int k_cchDeveloperMetadataMax = 5000;

		// Token: 0x04000846 RID: 2118
		public const int k_cchStatNameMax = 128;

		// Token: 0x04000847 RID: 2119
		public const int k_cchLeaderboardNameMax = 128;

		// Token: 0x04000848 RID: 2120
		public const int k_cLeaderboardDetailsMax = 64;

		// Token: 0x04000849 RID: 2121
		public const int k_cbMaxGameServerGameDir = 32;

		// Token: 0x0400084A RID: 2122
		public const int k_cbMaxGameServerMapName = 32;

		// Token: 0x0400084B RID: 2123
		public const int k_cbMaxGameServerGameDescription = 64;

		// Token: 0x0400084C RID: 2124
		public const int k_cbMaxGameServerName = 64;

		// Token: 0x0400084D RID: 2125
		public const int k_cbMaxGameServerTags = 128;

		// Token: 0x0400084E RID: 2126
		public const int k_cbMaxGameServerGameData = 2048;

		// Token: 0x0400084F RID: 2127
		public const int k_unSteamAccountIDMask = -1;

		// Token: 0x04000850 RID: 2128
		public const int k_unSteamAccountInstanceMask = 1048575;

		// Token: 0x04000851 RID: 2129
		public const int k_unSteamUserDesktopInstance = 1;

		// Token: 0x04000852 RID: 2130
		public const int k_unSteamUserConsoleInstance = 2;

		// Token: 0x04000853 RID: 2131
		public const int k_unSteamUserWebInstance = 4;

		// Token: 0x04000854 RID: 2132
		public const int k_cchGameExtraInfoMax = 64;

		// Token: 0x04000855 RID: 2133
		public const int k_nSteamEncryptedAppTicketSymmetricKeyLen = 32;

		// Token: 0x04000856 RID: 2134
		public const int k_cubSaltSize = 8;

		// Token: 0x04000857 RID: 2135
		public const ulong k_GIDNil = 18446744073709551615UL;

		// Token: 0x04000858 RID: 2136
		public const ulong k_TxnIDNil = 18446744073709551615UL;

		// Token: 0x04000859 RID: 2137
		public const ulong k_TxnIDUnknown = 0UL;

		// Token: 0x0400085A RID: 2138
		public const int k_uPackageIdFreeSub = 0;

		// Token: 0x0400085B RID: 2139
		public const int k_uPackageIdInvalid = -1;

		// Token: 0x0400085C RID: 2140
		public const ulong k_ulAssetClassIdInvalid = 0UL;

		// Token: 0x0400085D RID: 2141
		public const int k_uPhysicalItemIdInvalid = 0;

		// Token: 0x0400085E RID: 2142
		public const int k_uCellIDInvalid = -1;

		// Token: 0x0400085F RID: 2143
		public const int k_uPartnerIdInvalid = 0;

		// Token: 0x04000860 RID: 2144
		public const int STEAM_CONTROLLER_MAX_COUNT = 16;

		// Token: 0x04000861 RID: 2145
		public const int STEAM_CONTROLLER_MAX_ANALOG_ACTIONS = 16;

		// Token: 0x04000862 RID: 2146
		public const int STEAM_CONTROLLER_MAX_DIGITAL_ACTIONS = 128;

		// Token: 0x04000863 RID: 2147
		public const int STEAM_CONTROLLER_MAX_ORIGINS = 8;

		// Token: 0x04000864 RID: 2148
		public const ulong STEAM_CONTROLLER_HANDLE_ALL_CONTROLLERS = 18446744073709551615UL;

		// Token: 0x04000865 RID: 2149
		public const float STEAM_CONTROLLER_MIN_ANALOG_ACTION_DATA = -1f;

		// Token: 0x04000866 RID: 2150
		public const float STEAM_CONTROLLER_MAX_ANALOG_ACTION_DATA = 1f;

		// Token: 0x04000867 RID: 2151
		public const ushort MASTERSERVERUPDATERPORT_USEGAMESOCKETSHARE = 65535;

		// Token: 0x04000868 RID: 2152
		public const int INVALID_HTTPREQUEST_HANDLE = 0;

		// Token: 0x04000869 RID: 2153
		public const byte k_nMaxLobbyKeyLength = 255;

		// Token: 0x0400086A RID: 2154
		public const int k_SteamMusicNameMaxLength = 255;

		// Token: 0x0400086B RID: 2155
		public const int k_SteamMusicPNGMaxLength = 65535;

		// Token: 0x0400086C RID: 2156
		public const int QUERY_PORT_NOT_INITIALIZED = 65535;

		// Token: 0x0400086D RID: 2157
		public const int QUERY_PORT_ERROR = 65534;
	}
}
