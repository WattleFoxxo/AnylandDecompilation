using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A1 RID: 417
	public static class SteamGameServerUGC
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x0000783E File Offset: 0x00005A3E
		public static UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamGameServerUGC_CreateQueryUserUGCRequest(unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00007859 File Offset: 0x00005A59
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamGameServerUGC_CreateQueryAllUGCRequest(eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00007870 File Offset: 0x00005A70
		public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamGameServerUGC_CreateQueryUGCDetailsRequest(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00007883 File Offset: 0x00005A83
		public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_SendQueryUGCRequest(handle);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00007895 File Offset: 0x00005A95
		public static bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetQueryUGCResult(handle, index, out pDetails);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x000078A4 File Offset: 0x00005AA4
		public static bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			bool flag = NativeMethods.ISteamGameServerUGC_GetQueryUGCPreviewURL(handle, index, intPtr, cchURLSize);
			pchURL = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000078E4 File Offset: 0x00005AE4
		public static bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchMetadatasize);
			bool flag = NativeMethods.ISteamGameServerUGC_GetQueryUGCMetadata(handle, index, intPtr, cchMetadatasize);
			pchMetadata = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00007922 File Offset: 0x00005B22
		public static bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetQueryUGCChildren(handle, index, pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00007932 File Offset: 0x00005B32
		public static bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetQueryUGCStatistic(handle, index, eStatType, out pStatValue);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00007942 File Offset: 0x00005B42
		public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetQueryUGCNumAdditionalPreviews(handle, index);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00007950 File Offset: 0x00005B50
		public static bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchOriginalFileNameSize);
			bool flag = NativeMethods.ISteamGameServerUGC_GetQueryUGCAdditionalPreview(handle, index, previewIndex, intPtr, cchURLSize, intPtr2, cchOriginalFileNameSize, out pPreviewType);
			pchURLOrVideoID = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			pchOriginalFileName = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr2));
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000079B9 File Offset: 0x00005BB9
		public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetQueryUGCNumKeyValueTags(handle, index);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000079C8 File Offset: 0x00005BC8
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchKeySize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamGameServerUGC_GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, intPtr, cchKeySize, intPtr2, cchValueSize);
			pchKey = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			pchValue = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr2));
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00007A2F File Offset: 0x00005C2F
		public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_ReleaseQueryUGCRequest(handle);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00007A3C File Offset: 0x00005C3C
		public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				flag = NativeMethods.ISteamGameServerUGC_AddRequiredTag(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00007A80 File Offset: 0x00005C80
		public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				flag = NativeMethods.ISteamGameServerUGC_AddExcludedTag(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00007AC4 File Offset: 0x00005CC4
		public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetReturnOnlyIDs(handle, bReturnOnlyIDs);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00007AD2 File Offset: 0x00005CD2
		public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetReturnKeyValueTags(handle, bReturnKeyValueTags);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00007AE0 File Offset: 0x00005CE0
		public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetReturnLongDescription(handle, bReturnLongDescription);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00007AEE File Offset: 0x00005CEE
		public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetReturnMetadata(handle, bReturnMetadata);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00007AFC File Offset: 0x00005CFC
		public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetReturnChildren(handle, bReturnChildren);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00007B0A File Offset: 0x00005D0A
		public static bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetReturnAdditionalPreviews(handle, bReturnAdditionalPreviews);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00007B18 File Offset: 0x00005D18
		public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetReturnTotalOnly(handle, bReturnTotalOnly);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00007B28 File Offset: 0x00005D28
		public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetLanguage(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00007B6C File Offset: 0x00005D6C
		public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetAllowCachedResponse(handle, unMaxAgeSeconds);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00007B7C File Offset: 0x00005D7C
		public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetCloudFileNameFilter(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00007BC0 File Offset: 0x00005DC0
		public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetMatchAnyTag(handle, bMatchAnyTag);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00007BD0 File Offset: 0x00005DD0
		public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pSearchText))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetSearchText(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00007C14 File Offset: 0x00005E14
		public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetRankedByTrendDays(handle, unDays);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00007C24 File Offset: 0x00005E24
		public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					flag = NativeMethods.ISteamGameServerUGC_AddRequiredKeyValueTag(handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00007C8C File Offset: 0x00005E8C
		public static SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_RequestUGCDetails(nPublishedFileID, unMaxAgeSeconds);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00007C9F File Offset: 0x00005E9F
		public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_CreateItem(nConsumerAppId, eFileType);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00007CB2 File Offset: 0x00005EB2
		public static UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCUpdateHandle_t)NativeMethods.ISteamGameServerUGC_StartItemUpdate(nConsumerAppId, nPublishedFileID);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00007CC8 File Offset: 0x00005EC8
		public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetItemTitle(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00007D0C File Offset: 0x00005F0C
		public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetItemDescription(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00007D50 File Offset: 0x00005F50
		public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetItemUpdateLanguage(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00007D94 File Offset: 0x00005F94
		public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMetaData))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetItemMetadata(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00007DD8 File Offset: 0x00005FD8
		public static bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetItemVisibility(handle, eVisibility);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00007DE6 File Offset: 0x00005FE6
		public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_SetItemTags(updateHandle, new InteropHelp.SteamParamStringArray(pTags));
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00007E00 File Offset: 0x00006000
		public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszContentFolder))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetItemContent(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00007E44 File Offset: 0x00006044
		public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamGameServerUGC_SetItemPreview(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00007E88 File Offset: 0x00006088
		public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				flag = NativeMethods.ISteamGameServerUGC_RemoveItemKeyValueTags(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00007ECC File Offset: 0x000060CC
		public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					flag = NativeMethods.ISteamGameServerUGC_AddItemKeyValueTag(handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00007F34 File Offset: 0x00006134
		public static bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamGameServerUGC_AddItemPreviewFile(handle, utf8StringHandle, type);
			}
			return flag;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00007F7C File Offset: 0x0000617C
		public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				flag = NativeMethods.ISteamGameServerUGC_AddItemPreviewVideo(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00007FC0 File Offset: 0x000061C0
		public static bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamGameServerUGC_UpdateItemPreviewFile(handle, index, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00008008 File Offset: 0x00006208
		public static bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				flag = NativeMethods.ISteamGameServerUGC_UpdateItemPreviewVideo(handle, index, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00008050 File Offset: 0x00006250
		public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_RemoveItemPreview(handle, index);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00008060 File Offset: 0x00006260
		public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
		{
			InteropHelp.TestIfAvailableGameServer();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeNote))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_SubmitItemUpdate(handle, utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000080AC File Offset: 0x000062AC
		public static EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000080BB File Offset: 0x000062BB
		public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_SetUserItemVote(nPublishedFileID, bVoteUp);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x000080CE File Offset: 0x000062CE
		public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_GetUserItemVote(nPublishedFileID);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000080E0 File Offset: 0x000062E0
		public static SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_AddItemToFavorites(nAppId, nPublishedFileID);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x000080F3 File Offset: 0x000062F3
		public static SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_RemoveItemFromFavorites(nAppId, nPublishedFileID);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00008106 File Offset: 0x00006306
		public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_SubscribeItem(nPublishedFileID);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00008118 File Offset: 0x00006318
		public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_UnsubscribeItem(nPublishedFileID);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0000812A File Offset: 0x0000632A
		public static uint GetNumSubscribedItems()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetNumSubscribedItems();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00008136 File Offset: 0x00006336
		public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetSubscribedItems(pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00008144 File Offset: 0x00006344
		public static uint GetItemState(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetItemState(nPublishedFileID);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00008154 File Offset: 0x00006354
		public static bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderSize);
			bool flag = NativeMethods.ISteamGameServerUGC_GetItemInstallInfo(nPublishedFileID, out punSizeOnDisk, intPtr, cchFolderSize, out punTimeStamp);
			pchFolder = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00008194 File Offset: 0x00006394
		public static bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_GetItemDownloadInfo(nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x000081A3 File Offset: 0x000063A3
		public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUGC_DownloadItem(nPublishedFileID, bHighPriority);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000081B4 File Offset: 0x000063B4
		public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszFolder))
			{
				flag = NativeMethods.ISteamGameServerUGC_BInitWorkshopForGameServer(unWorkshopDepotID, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x000081F8 File Offset: 0x000063F8
		public static void SuspendDownloads(bool bSuspend)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUGC_SuspendDownloads(bSuspend);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00008205 File Offset: 0x00006405
		public static SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_StartPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00008218 File Offset: 0x00006418
		public static SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_StopPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0000822B File Offset: 0x0000642B
		public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerUGC_StopPlaytimeTrackingForAllItems();
		}
	}
}
