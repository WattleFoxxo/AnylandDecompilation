using System;

namespace Steamworks
{
	// Token: 0x020001AA RID: 426
	public static class SteamNetworking
	{
		// Token: 0x060007DE RID: 2014 RVA: 0x0000978D File Offset: 0x0000798D
		public static bool SendP2PPacket(CSteamID steamIDRemote, byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_SendP2PPacket(steamIDRemote, pubData, cubData, eP2PSendType, nChannel);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0000979F File Offset: 0x0000799F
		public static bool IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_IsP2PPacketAvailable(out pcubMsgSize, nChannel);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000097AD File Offset: 0x000079AD
		public static bool ReadP2PPacket(byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_ReadP2PPacket(pubDest, cubDest, out pcubMsgSize, out psteamIDRemote, nChannel);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000097BF File Offset: 0x000079BF
		public static bool AcceptP2PSessionWithUser(CSteamID steamIDRemote)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_AcceptP2PSessionWithUser(steamIDRemote);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x000097CC File Offset: 0x000079CC
		public static bool CloseP2PSessionWithUser(CSteamID steamIDRemote)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_CloseP2PSessionWithUser(steamIDRemote);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000097D9 File Offset: 0x000079D9
		public static bool CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_CloseP2PChannelWithUser(steamIDRemote, nChannel);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000097E7 File Offset: 0x000079E7
		public static bool GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetP2PSessionState(steamIDRemote, out pConnectionState);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000097F5 File Offset: 0x000079F5
		public static bool AllowP2PPacketRelay(bool bAllow)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_AllowP2PPacketRelay(bAllow);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00009802 File Offset: 0x00007A02
		public static SNetListenSocket_t CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, bool bAllowUseOfPacketRelay)
		{
			InteropHelp.TestIfAvailableClient();
			return (SNetListenSocket_t)NativeMethods.ISteamNetworking_CreateListenSocket(nVirtualP2PPort, nIP, nPort, bAllowUseOfPacketRelay);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00009817 File Offset: 0x00007A17
		public static SNetSocket_t CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, bool bAllowUseOfPacketRelay)
		{
			InteropHelp.TestIfAvailableClient();
			return (SNetSocket_t)NativeMethods.ISteamNetworking_CreateP2PConnectionSocket(steamIDTarget, nVirtualPort, nTimeoutSec, bAllowUseOfPacketRelay);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0000982C File Offset: 0x00007A2C
		public static SNetSocket_t CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec)
		{
			InteropHelp.TestIfAvailableClient();
			return (SNetSocket_t)NativeMethods.ISteamNetworking_CreateConnectionSocket(nIP, nPort, nTimeoutSec);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00009840 File Offset: 0x00007A40
		public static bool DestroySocket(SNetSocket_t hSocket, bool bNotifyRemoteEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_DestroySocket(hSocket, bNotifyRemoteEnd);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0000984E File Offset: 0x00007A4E
		public static bool DestroyListenSocket(SNetListenSocket_t hSocket, bool bNotifyRemoteEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_DestroyListenSocket(hSocket, bNotifyRemoteEnd);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0000985C File Offset: 0x00007A5C
		public static bool SendDataOnSocket(SNetSocket_t hSocket, byte[] pubData, uint cubData, bool bReliable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_SendDataOnSocket(hSocket, pubData, cubData, bReliable);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0000986C File Offset: 0x00007A6C
		public static bool IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_IsDataAvailableOnSocket(hSocket, out pcubMsgSize);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0000987A File Offset: 0x00007A7A
		public static bool RetrieveDataFromSocket(SNetSocket_t hSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_RetrieveDataFromSocket(hSocket, pubDest, cubDest, out pcubMsgSize);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0000988A File Offset: 0x00007A8A
		public static bool IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_IsDataAvailable(hListenSocket, out pcubMsgSize, out phSocket);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00009899 File Offset: 0x00007A99
		public static bool RetrieveData(SNetListenSocket_t hListenSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_RetrieveData(hListenSocket, pubDest, cubDest, out pcubMsgSize, out phSocket);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000098AB File Offset: 0x00007AAB
		public static bool GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetSocketInfo(hSocket, out pSteamIDRemote, out peSocketStatus, out punIPRemote, out punPortRemote);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000098BD File Offset: 0x00007ABD
		public static bool GetListenSocketInfo(SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetListenSocketInfo(hListenSocket, out pnIP, out pnPort);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x000098CC File Offset: 0x00007ACC
		public static ESNetSocketConnectionType GetSocketConnectionType(SNetSocket_t hSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetSocketConnectionType(hSocket);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000098D9 File Offset: 0x00007AD9
		public static int GetMaxPacketSize(SNetSocket_t hSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetMaxPacketSize(hSocket);
		}
	}
}
