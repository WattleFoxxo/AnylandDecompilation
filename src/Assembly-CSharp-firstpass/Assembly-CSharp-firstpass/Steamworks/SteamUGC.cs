using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AD RID: 429
	public static class SteamUGC
	{
		// Token: 0x06000834 RID: 2100 RVA: 0x0000A404 File Offset: 0x00008604
		public static UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0000A41F File Offset: 0x0000861F
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequest(eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0000A436 File Offset: 0x00008636
		public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUGCDetailsRequest(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0000A449 File Offset: 0x00008649
		public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SendQueryUGCRequest(handle);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0000A45B File Offset: 0x0000865B
		public static bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCResult(handle, index, out pDetails);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0000A46C File Offset: 0x0000866C
		public static bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCPreviewURL(handle, index, intPtr, cchURLSize);
			pchURL = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0000A4AC File Offset: 0x000086AC
		public static bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchMetadatasize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCMetadata(handle, index, intPtr, cchMetadatasize);
			pchMetadata = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0000A4EA File Offset: 0x000086EA
		public static bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCChildren(handle, index, pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0000A4FA File Offset: 0x000086FA
		public static bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCStatistic(handle, index, eStatType, out pStatValue);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0000A50A File Offset: 0x0000870A
		public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCNumAdditionalPreviews(handle, index);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0000A518 File Offset: 0x00008718
		public static bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchOriginalFileNameSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCAdditionalPreview(handle, index, previewIndex, intPtr, cchURLSize, intPtr2, cchOriginalFileNameSize, out pPreviewType);
			pchURLOrVideoID = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			pchOriginalFileName = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr2));
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0000A581 File Offset: 0x00008781
		public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCNumKeyValueTags(handle, index);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0000A590 File Offset: 0x00008790
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchKeySize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, intPtr, cchKeySize, intPtr2, cchValueSize);
			pchKey = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			pchValue = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr2));
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0000A5F7 File Offset: 0x000087F7
		public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(handle);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0000A604 File Offset: 0x00008804
		public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				flag = NativeMethods.ISteamUGC_AddRequiredTag(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0000A648 File Offset: 0x00008848
		public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				flag = NativeMethods.ISteamUGC_AddExcludedTag(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0000A68C File Offset: 0x0000888C
		public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnOnlyIDs(handle, bReturnOnlyIDs);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0000A69A File Offset: 0x0000889A
		public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnKeyValueTags(handle, bReturnKeyValueTags);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0000A6A8 File Offset: 0x000088A8
		public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnLongDescription(handle, bReturnLongDescription);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0000A6B6 File Offset: 0x000088B6
		public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnMetadata(handle, bReturnMetadata);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0000A6C4 File Offset: 0x000088C4
		public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnChildren(handle, bReturnChildren);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0000A6D2 File Offset: 0x000088D2
		public static bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnAdditionalPreviews(handle, bReturnAdditionalPreviews);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0000A6E0 File Offset: 0x000088E0
		public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnTotalOnly(handle, bReturnTotalOnly);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0000A6F0 File Offset: 0x000088F0
		public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				flag = NativeMethods.ISteamUGC_SetLanguage(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0000A734 File Offset: 0x00008934
		public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetAllowCachedResponse(handle, unMaxAgeSeconds);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0000A744 File Offset: 0x00008944
		public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
			{
				flag = NativeMethods.ISteamUGC_SetCloudFileNameFilter(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0000A788 File Offset: 0x00008988
		public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetMatchAnyTag(handle, bMatchAnyTag);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0000A798 File Offset: 0x00008998
		public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pSearchText))
			{
				flag = NativeMethods.ISteamUGC_SetSearchText(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0000A7DC File Offset: 0x000089DC
		public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetRankedByTrendDays(handle, unDays);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0000A7EC File Offset: 0x000089EC
		public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					flag = NativeMethods.ISteamUGC_AddRequiredKeyValueTag(handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0000A854 File Offset: 0x00008A54
		public static SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RequestUGCDetails(nPublishedFileID, unMaxAgeSeconds);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0000A867 File Offset: 0x00008A67
		public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_CreateItem(nConsumerAppId, eFileType);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0000A87A File Offset: 0x00008A7A
		public static UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCUpdateHandle_t)NativeMethods.ISteamUGC_StartItemUpdate(nConsumerAppId, nPublishedFileID);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0000A890 File Offset: 0x00008A90
		public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				flag = NativeMethods.ISteamUGC_SetItemTitle(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0000A8D4 File Offset: 0x00008AD4
		public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				flag = NativeMethods.ISteamUGC_SetItemDescription(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0000A918 File Offset: 0x00008B18
		public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				flag = NativeMethods.ISteamUGC_SetItemUpdateLanguage(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0000A95C File Offset: 0x00008B5C
		public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMetaData))
			{
				flag = NativeMethods.ISteamUGC_SetItemMetadata(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0000A9A0 File Offset: 0x00008BA0
		public static bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetItemVisibility(handle, eVisibility);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0000A9AE File Offset: 0x00008BAE
		public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetItemTags(updateHandle, new InteropHelp.SteamParamStringArray(pTags));
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszContentFolder))
			{
				flag = NativeMethods.ISteamUGC_SetItemContent(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0000AA0C File Offset: 0x00008C0C
		public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_SetItemPreview(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0000AA50 File Offset: 0x00008C50
		public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				flag = NativeMethods.ISteamUGC_RemoveItemKeyValueTags(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0000AA94 File Offset: 0x00008C94
		public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					flag = NativeMethods.ISteamUGC_AddItemKeyValueTag(handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0000AAFC File Offset: 0x00008CFC
		public static bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_AddItemPreviewFile(handle, utf8StringHandle, type);
			}
			return flag;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0000AB44 File Offset: 0x00008D44
		public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				flag = NativeMethods.ISteamUGC_AddItemPreviewVideo(handle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0000AB88 File Offset: 0x00008D88
		public static bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				flag = NativeMethods.ISteamUGC_UpdateItemPreviewFile(handle, index, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		public static bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				flag = NativeMethods.ISteamUGC_UpdateItemPreviewVideo(handle, index, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0000AC18 File Offset: 0x00008E18
		public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_RemoveItemPreview(handle, index);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0000AC28 File Offset: 0x00008E28
		public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeNote))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUGC_SubmitItemUpdate(handle, utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0000AC74 File Offset: 0x00008E74
		public static EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0000AC83 File Offset: 0x00008E83
		public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SetUserItemVote(nPublishedFileID, bVoteUp);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0000AC96 File Offset: 0x00008E96
		public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetUserItemVote(nPublishedFileID);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		public static SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddItemToFavorites(nAppId, nPublishedFileID);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0000ACBB File Offset: 0x00008EBB
		public static SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveItemFromFavorites(nAppId, nPublishedFileID);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000ACCE File Offset: 0x00008ECE
		public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SubscribeItem(nPublishedFileID);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0000ACE0 File Offset: 0x00008EE0
		public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_UnsubscribeItem(nPublishedFileID);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0000ACF2 File Offset: 0x00008EF2
		public static uint GetNumSubscribedItems()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetNumSubscribedItems();
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0000ACFE File Offset: 0x00008EFE
		public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetSubscribedItems(pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0000AD0C File Offset: 0x00008F0C
		public static uint GetItemState(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemState(nPublishedFileID);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0000AD1C File Offset: 0x00008F1C
		public static bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderSize);
			bool flag = NativeMethods.ISteamUGC_GetItemInstallInfo(nPublishedFileID, out punSizeOnDisk, intPtr, cchFolderSize, out punTimeStamp);
			pchFolder = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0000AD5C File Offset: 0x00008F5C
		public static bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemDownloadInfo(nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0000AD6B File Offset: 0x00008F6B
		public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_DownloadItem(nPublishedFileID, bHighPriority);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0000AD7C File Offset: 0x00008F7C
		public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszFolder))
			{
				flag = NativeMethods.ISteamUGC_BInitWorkshopForGameServer(unWorkshopDepotID, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0000ADC0 File Offset: 0x00008FC0
		public static void SuspendDownloads(bool bSuspend)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUGC_SuspendDownloads(bSuspend);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0000ADCD File Offset: 0x00008FCD
		public static SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StartPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		public static SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0000ADF3 File Offset: 0x00008FF3
		public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTrackingForAllItems();
		}
	}
}
