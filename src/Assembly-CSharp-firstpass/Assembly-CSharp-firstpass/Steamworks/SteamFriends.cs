using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200019B RID: 411
	public static class SteamFriends
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x00006310 File Offset: 0x00004510
		public static string GetPersonaName()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPersonaName());
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00006324 File Offset: 0x00004524
		public static SteamAPICall_t SetPersonaName(string pchPersonaName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPersonaName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamFriends_SetPersonaName(utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000636C File Offset: 0x0000456C
		public static EPersonaState GetPersonaState()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetPersonaState();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00006378 File Offset: 0x00004578
		public static int GetFriendCount(EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCount(iFriendFlags);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00006385 File Offset: 0x00004585
		public static CSteamID GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetFriendByIndex(iFriend, iFriendFlags);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00006398 File Offset: 0x00004598
		public static EFriendRelationship GetFriendRelationship(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendRelationship(steamIDFriend);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x000063A5 File Offset: 0x000045A5
		public static EPersonaState GetFriendPersonaState(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendPersonaState(steamIDFriend);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000063B2 File Offset: 0x000045B2
		public static string GetFriendPersonaName(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaName(steamIDFriend));
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000063C4 File Offset: 0x000045C4
		public static bool GetFriendGamePlayed(CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendGamePlayed(steamIDFriend, out pFriendGameInfo);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000063D2 File Offset: 0x000045D2
		public static string GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaNameHistory(steamIDFriend, iPersonaName));
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000063E5 File Offset: 0x000045E5
		public static int GetFriendSteamLevel(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendSteamLevel(steamIDFriend);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x000063F2 File Offset: 0x000045F2
		public static string GetPlayerNickname(CSteamID steamIDPlayer)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPlayerNickname(steamIDPlayer));
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00006404 File Offset: 0x00004604
		public static int GetFriendsGroupCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendsGroupCount();
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00006410 File Offset: 0x00004610
		public static FriendsGroupID_t GetFriendsGroupIDByIndex(int iFG)
		{
			InteropHelp.TestIfAvailableClient();
			return (FriendsGroupID_t)NativeMethods.ISteamFriends_GetFriendsGroupIDByIndex(iFG);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00006422 File Offset: 0x00004622
		public static string GetFriendsGroupName(FriendsGroupID_t friendsGroupID)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendsGroupName(friendsGroupID));
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00006434 File Offset: 0x00004634
		public static int GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendsGroupMembersCount(friendsGroupID);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00006441 File Offset: 0x00004641
		public static void GetFriendsGroupMembersList(FriendsGroupID_t friendsGroupID, CSteamID[] pOutSteamIDMembers, int nMembersCount)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_GetFriendsGroupMembersList(friendsGroupID, pOutSteamIDMembers, nMembersCount);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00006450 File Offset: 0x00004650
		public static bool HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_HasFriend(steamIDFriend, iFriendFlags);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0000645E File Offset: 0x0000465E
		public static int GetClanCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanCount();
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0000646A File Offset: 0x0000466A
		public static CSteamID GetClanByIndex(int iClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanByIndex(iClan);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0000647C File Offset: 0x0000467C
		public static string GetClanName(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanName(steamIDClan));
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0000648E File Offset: 0x0000468E
		public static string GetClanTag(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanTag(steamIDClan));
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000064A0 File Offset: 0x000046A0
		public static bool GetClanActivityCounts(CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanActivityCounts(steamIDClan, out pnOnline, out pnInGame, out pnChatting);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000064B0 File Offset: 0x000046B0
		public static SteamAPICall_t DownloadClanActivityCounts(CSteamID[] psteamIDClans, int cClansToRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_DownloadClanActivityCounts(psteamIDClans, cClansToRequest);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000064C3 File Offset: 0x000046C3
		public static int GetFriendCountFromSource(CSteamID steamIDSource)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCountFromSource(steamIDSource);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000064D0 File Offset: 0x000046D0
		public static CSteamID GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetFriendFromSourceByIndex(steamIDSource, iFriend);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000064E3 File Offset: 0x000046E3
		public static bool IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsUserInSource(steamIDUser, steamIDSource);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000064F1 File Offset: 0x000046F1
		public static void SetInGameVoiceSpeaking(CSteamID steamIDUser, bool bSpeaking)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_SetInGameVoiceSpeaking(steamIDUser, bSpeaking);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00006500 File Offset: 0x00004700
		public static void ActivateGameOverlay(string pchDialog)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDialog))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlay(utf8StringHandle);
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00006544 File Offset: 0x00004744
		public static void ActivateGameOverlayToUser(string pchDialog, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDialog))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlayToUser(utf8StringHandle, steamID);
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00006588 File Offset: 0x00004788
		public static void ActivateGameOverlayToWebPage(string pchURL)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchURL))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlayToWebPage(utf8StringHandle);
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000065CC File Offset: 0x000047CC
		public static void ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ActivateGameOverlayToStore(nAppID, eFlag);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000065DA File Offset: 0x000047DA
		public static void SetPlayedWith(CSteamID steamIDUserPlayedWith)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_SetPlayedWith(steamIDUserPlayedWith);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000065E7 File Offset: 0x000047E7
		public static void ActivateGameOverlayInviteDialog(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ActivateGameOverlayInviteDialog(steamIDLobby);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000065F4 File Offset: 0x000047F4
		public static int GetSmallFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetSmallFriendAvatar(steamIDFriend);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00006601 File Offset: 0x00004801
		public static int GetMediumFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetMediumFriendAvatar(steamIDFriend);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0000660E File Offset: 0x0000480E
		public static int GetLargeFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetLargeFriendAvatar(steamIDFriend);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0000661B File Offset: 0x0000481B
		public static bool RequestUserInformation(CSteamID steamIDUser, bool bRequireNameOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_RequestUserInformation(steamIDUser, bRequireNameOnly);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00006629 File Offset: 0x00004829
		public static SteamAPICall_t RequestClanOfficerList(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_RequestClanOfficerList(steamIDClan);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0000663B File Offset: 0x0000483B
		public static CSteamID GetClanOwner(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanOwner(steamIDClan);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0000664D File Offset: 0x0000484D
		public static int GetClanOfficerCount(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanOfficerCount(steamIDClan);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0000665A File Offset: 0x0000485A
		public static CSteamID GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanOfficerByIndex(steamIDClan, iOfficer);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0000666D File Offset: 0x0000486D
		public static uint GetUserRestrictions()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetUserRestrictions();
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0000667C File Offset: 0x0000487C
		public static bool SetRichPresence(string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					flag = NativeMethods.ISteamFriends_SetRichPresence(utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000066E0 File Offset: 0x000048E0
		public static void ClearRichPresence()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ClearRichPresence();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000066EC File Offset: 0x000048EC
		public static string GetFriendRichPresence(CSteamID steamIDFriend, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string text;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				text = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresence(steamIDFriend, utf8StringHandle));
			}
			return text;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00006738 File Offset: 0x00004938
		public static int GetFriendRichPresenceKeyCount(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendRichPresenceKeyCount(steamIDFriend);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00006745 File Offset: 0x00004945
		public static string GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresenceKeyByIndex(steamIDFriend, iKey));
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00006758 File Offset: 0x00004958
		public static void RequestFriendRichPresence(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_RequestFriendRichPresence(steamIDFriend);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00006768 File Offset: 0x00004968
		public static bool InviteUserToGame(CSteamID steamIDFriend, string pchConnectString)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchConnectString))
			{
				flag = NativeMethods.ISteamFriends_InviteUserToGame(steamIDFriend, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x000067AC File Offset: 0x000049AC
		public static int GetCoplayFriendCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetCoplayFriendCount();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000067B8 File Offset: 0x000049B8
		public static CSteamID GetCoplayFriend(int iCoplayFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetCoplayFriend(iCoplayFriend);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000067CA File Offset: 0x000049CA
		public static int GetFriendCoplayTime(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCoplayTime(steamIDFriend);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x000067D7 File Offset: 0x000049D7
		public static AppId_t GetFriendCoplayGame(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (AppId_t)NativeMethods.ISteamFriends_GetFriendCoplayGame(steamIDFriend);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x000067E9 File Offset: 0x000049E9
		public static SteamAPICall_t JoinClanChatRoom(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_JoinClanChatRoom(steamIDClan);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000067FB File Offset: 0x000049FB
		public static bool LeaveClanChatRoom(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_LeaveClanChatRoom(steamIDClan);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00006808 File Offset: 0x00004A08
		public static int GetClanChatMemberCount(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanChatMemberCount(steamIDClan);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00006815 File Offset: 0x00004A15
		public static CSteamID GetChatMemberByIndex(CSteamID steamIDClan, int iUser)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetChatMemberByIndex(steamIDClan, iUser);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00006828 File Offset: 0x00004A28
		public static bool SendClanChatMessage(CSteamID steamIDClanChat, string pchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchText))
			{
				flag = NativeMethods.ISteamFriends_SendClanChatMessage(steamIDClanChat, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0000686C File Offset: 0x00004A6C
		public static int GetClanChatMessage(CSteamID steamIDClanChat, int iMessage, out string prgchText, int cchTextMax, out EChatEntryType peChatEntryType, out CSteamID psteamidChatter)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchTextMax);
			int num = NativeMethods.ISteamFriends_GetClanChatMessage(steamIDClanChat, iMessage, intPtr, cchTextMax, out peChatEntryType, out psteamidChatter);
			prgchText = ((num == 0) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000068AE File Offset: 0x00004AAE
		public static bool IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsClanChatAdmin(steamIDClanChat, steamIDUser);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000068BC File Offset: 0x00004ABC
		public static bool IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsClanChatWindowOpenInSteam(steamIDClanChat);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000068C9 File Offset: 0x00004AC9
		public static bool OpenClanChatWindowInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_OpenClanChatWindowInSteam(steamIDClanChat);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000068D6 File Offset: 0x00004AD6
		public static bool CloseClanChatWindowInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_CloseClanChatWindowInSteam(steamIDClanChat);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000068E3 File Offset: 0x00004AE3
		public static bool SetListenForFriendsMessages(bool bInterceptEnabled)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_SetListenForFriendsMessages(bInterceptEnabled);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x000068F0 File Offset: 0x00004AF0
		public static bool ReplyToFriendMessage(CSteamID steamIDFriend, string pchMsgToSend)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMsgToSend))
			{
				flag = NativeMethods.ISteamFriends_ReplyToFriendMessage(steamIDFriend, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00006934 File Offset: 0x00004B34
		public static int GetFriendMessage(CSteamID steamIDFriend, int iMessageID, out string pvData, int cubData, out EChatEntryType peChatEntryType)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubData);
			int num = NativeMethods.ISteamFriends_GetFriendMessage(steamIDFriend, iMessageID, intPtr, cubData, out peChatEntryType);
			pvData = ((num == 0) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00006974 File Offset: 0x00004B74
		public static SteamAPICall_t GetFollowerCount(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_GetFollowerCount(steamID);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00006986 File Offset: 0x00004B86
		public static SteamAPICall_t IsFollowing(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_IsFollowing(steamID);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00006998 File Offset: 0x00004B98
		public static SteamAPICall_t EnumerateFollowingList(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_EnumerateFollowingList(unStartIndex);
		}
	}
}
