using System;

namespace Steamworks
{
	// Token: 0x0200019F RID: 415
	public static class SteamGameServerNetworking
	{
		// Token: 0x060006AC RID: 1708 RVA: 0x00007489 File Offset: 0x00005689
		public static bool SendP2PPacket(CSteamID steamIDRemote, byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_SendP2PPacket(steamIDRemote, pubData, cubData, eP2PSendType, nChannel);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0000749B File Offset: 0x0000569B
		public static bool IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_IsP2PPacketAvailable(out pcubMsgSize, nChannel);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000074A9 File Offset: 0x000056A9
		public static bool ReadP2PPacket(byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_ReadP2PPacket(pubDest, cubDest, out pcubMsgSize, out psteamIDRemote, nChannel);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000074BB File Offset: 0x000056BB
		public static bool AcceptP2PSessionWithUser(CSteamID steamIDRemote)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_AcceptP2PSessionWithUser(steamIDRemote);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000074C8 File Offset: 0x000056C8
		public static bool CloseP2PSessionWithUser(CSteamID steamIDRemote)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_CloseP2PSessionWithUser(steamIDRemote);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000074D5 File Offset: 0x000056D5
		public static bool CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_CloseP2PChannelWithUser(steamIDRemote, nChannel);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000074E3 File Offset: 0x000056E3
		public static bool GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_GetP2PSessionState(steamIDRemote, out pConnectionState);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000074F1 File Offset: 0x000056F1
		public static bool AllowP2PPacketRelay(bool bAllow)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_AllowP2PPacketRelay(bAllow);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000074FE File Offset: 0x000056FE
		public static SNetListenSocket_t CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, bool bAllowUseOfPacketRelay)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SNetListenSocket_t)NativeMethods.ISteamGameServerNetworking_CreateListenSocket(nVirtualP2PPort, nIP, nPort, bAllowUseOfPacketRelay);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00007513 File Offset: 0x00005713
		public static SNetSocket_t CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, bool bAllowUseOfPacketRelay)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SNetSocket_t)NativeMethods.ISteamGameServerNetworking_CreateP2PConnectionSocket(steamIDTarget, nVirtualPort, nTimeoutSec, bAllowUseOfPacketRelay);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00007528 File Offset: 0x00005728
		public static SNetSocket_t CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SNetSocket_t)NativeMethods.ISteamGameServerNetworking_CreateConnectionSocket(nIP, nPort, nTimeoutSec);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0000753C File Offset: 0x0000573C
		public static bool DestroySocket(SNetSocket_t hSocket, bool bNotifyRemoteEnd)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_DestroySocket(hSocket, bNotifyRemoteEnd);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0000754A File Offset: 0x0000574A
		public static bool DestroyListenSocket(SNetListenSocket_t hSocket, bool bNotifyRemoteEnd)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_DestroyListenSocket(hSocket, bNotifyRemoteEnd);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00007558 File Offset: 0x00005758
		public static bool SendDataOnSocket(SNetSocket_t hSocket, byte[] pubData, uint cubData, bool bReliable)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_SendDataOnSocket(hSocket, pubData, cubData, bReliable);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00007568 File Offset: 0x00005768
		public static bool IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_IsDataAvailableOnSocket(hSocket, out pcubMsgSize);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00007576 File Offset: 0x00005776
		public static bool RetrieveDataFromSocket(SNetSocket_t hSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_RetrieveDataFromSocket(hSocket, pubDest, cubDest, out pcubMsgSize);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00007586 File Offset: 0x00005786
		public static bool IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_IsDataAvailable(hListenSocket, out pcubMsgSize, out phSocket);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00007595 File Offset: 0x00005795
		public static bool RetrieveData(SNetListenSocket_t hListenSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_RetrieveData(hListenSocket, pubDest, cubDest, out pcubMsgSize, out phSocket);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000075A7 File Offset: 0x000057A7
		public static bool GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_GetSocketInfo(hSocket, out pSteamIDRemote, out peSocketStatus, out punIPRemote, out punPortRemote);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000075B9 File Offset: 0x000057B9
		public static bool GetListenSocketInfo(SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_GetListenSocketInfo(hListenSocket, out pnIP, out pnPort);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000075C8 File Offset: 0x000057C8
		public static ESNetSocketConnectionType GetSocketConnectionType(SNetSocket_t hSocket)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_GetSocketConnectionType(hSocket);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000075D5 File Offset: 0x000057D5
		public static int GetMaxPacketSize(SNetSocket_t hSocket)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerNetworking_GetMaxPacketSize(hSocket);
		}
	}
}
