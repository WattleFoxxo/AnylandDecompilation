using System;
using System.Collections.Generic;

namespace Steamworks
{
	// Token: 0x020001AB RID: 427
	public static class SteamRemoteStorage
	{
		// Token: 0x060007F4 RID: 2036 RVA: 0x000098E8 File Offset: 0x00007AE8
		public static bool FileWrite(string pchFile, byte[] pvData, int cubData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				flag = NativeMethods.ISteamRemoteStorage_FileWrite(utf8StringHandle, pvData, cubData);
			}
			return flag;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00009930 File Offset: 0x00007B30
		public static int FileRead(string pchFile, byte[] pvData, int cubDataToRead)
		{
			InteropHelp.TestIfAvailableClient();
			int num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				num = NativeMethods.ISteamRemoteStorage_FileRead(utf8StringHandle, pvData, cubDataToRead);
			}
			return num;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00009978 File Offset: 0x00007B78
		public static SteamAPICall_t FileWriteAsync(string pchFile, byte[] pvData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileWriteAsync(utf8StringHandle, pvData, cubData);
			}
			return steamAPICall_t;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000099C4 File Offset: 0x00007BC4
		public static SteamAPICall_t FileReadAsync(string pchFile, uint nOffset, uint cubToRead)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileReadAsync(utf8StringHandle, nOffset, cubToRead);
			}
			return steamAPICall_t;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00009A10 File Offset: 0x00007C10
		public static bool FileReadAsyncComplete(SteamAPICall_t hReadCall, byte[] pvBuffer, uint cubToRead)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileReadAsyncComplete(hReadCall, pvBuffer, cubToRead);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00009A20 File Offset: 0x00007C20
		public static bool FileForget(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				flag = NativeMethods.ISteamRemoteStorage_FileForget(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00009A64 File Offset: 0x00007C64
		public static bool FileDelete(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				flag = NativeMethods.ISteamRemoteStorage_FileDelete(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00009AA8 File Offset: 0x00007CA8
		public static SteamAPICall_t FileShare(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileShare(utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00009AF0 File Offset: 0x00007CF0
		public static bool SetSyncPlatforms(string pchFile, ERemoteStoragePlatform eRemoteStoragePlatform)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				flag = NativeMethods.ISteamRemoteStorage_SetSyncPlatforms(utf8StringHandle, eRemoteStoragePlatform);
			}
			return flag;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00009B34 File Offset: 0x00007D34
		public static UGCFileWriteStreamHandle_t FileWriteStreamOpen(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			UGCFileWriteStreamHandle_t ugcfileWriteStreamHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				ugcfileWriteStreamHandle_t = (UGCFileWriteStreamHandle_t)NativeMethods.ISteamRemoteStorage_FileWriteStreamOpen(utf8StringHandle);
			}
			return ugcfileWriteStreamHandle_t;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00009B7C File Offset: 0x00007D7C
		public static bool FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t writeHandle, byte[] pvData, int cubData)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamWriteChunk(writeHandle, pvData, cubData);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00009B8B File Offset: 0x00007D8B
		public static bool FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamClose(writeHandle);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00009B98 File Offset: 0x00007D98
		public static bool FileWriteStreamCancel(UGCFileWriteStreamHandle_t writeHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamCancel(writeHandle);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00009BA8 File Offset: 0x00007DA8
		public static bool FileExists(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				flag = NativeMethods.ISteamRemoteStorage_FileExists(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00009BEC File Offset: 0x00007DEC
		public static bool FilePersisted(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				flag = NativeMethods.ISteamRemoteStorage_FilePersisted(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00009C30 File Offset: 0x00007E30
		public static int GetFileSize(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			int num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				num = NativeMethods.ISteamRemoteStorage_GetFileSize(utf8StringHandle);
			}
			return num;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00009C74 File Offset: 0x00007E74
		public static long GetFileTimestamp(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			long num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				num = NativeMethods.ISteamRemoteStorage_GetFileTimestamp(utf8StringHandle);
			}
			return num;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00009CB8 File Offset: 0x00007EB8
		public static ERemoteStoragePlatform GetSyncPlatforms(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			ERemoteStoragePlatform eremoteStoragePlatform;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				eremoteStoragePlatform = NativeMethods.ISteamRemoteStorage_GetSyncPlatforms(utf8StringHandle);
			}
			return eremoteStoragePlatform;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00009CFC File Offset: 0x00007EFC
		public static int GetFileCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetFileCount();
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00009D08 File Offset: 0x00007F08
		public static string GetFileNameAndSize(int iFile, out int pnFileSizeInBytes)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamRemoteStorage_GetFileNameAndSize(iFile, out pnFileSizeInBytes));
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00009D1B File Offset: 0x00007F1B
		public static bool GetQuota(out ulong pnTotalBytes, out ulong puAvailableBytes)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetQuota(out pnTotalBytes, out puAvailableBytes);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00009D29 File Offset: 0x00007F29
		public static bool IsCloudEnabledForAccount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForAccount();
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00009D35 File Offset: 0x00007F35
		public static bool IsCloudEnabledForApp()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForApp();
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00009D41 File Offset: 0x00007F41
		public static void SetCloudEnabledForApp(bool bEnabled)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamRemoteStorage_SetCloudEnabledForApp(bEnabled);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00009D4E File Offset: 0x00007F4E
		public static SteamAPICall_t UGCDownload(UGCHandle_t hContent, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UGCDownload(hContent, unPriority);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00009D61 File Offset: 0x00007F61
		public static bool GetUGCDownloadProgress(UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetUGCDownloadProgress(hContent, out pnBytesDownloaded, out pnBytesExpected);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00009D70 File Offset: 0x00007F70
		public static bool GetUGCDetails(UGCHandle_t hContent, out AppId_t pnAppID, out string ppchName, out int pnFileSizeInBytes, out CSteamID pSteamIDOwner)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr;
			bool flag = NativeMethods.ISteamRemoteStorage_GetUGCDetails(hContent, out pnAppID, out intPtr, out pnFileSizeInBytes, out pSteamIDOwner);
			ppchName = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			return flag;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00009DA4 File Offset: 0x00007FA4
		public static int UGCRead(UGCHandle_t hContent, byte[] pvData, int cubDataToRead, uint cOffset, EUGCReadAction eAction)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UGCRead(hContent, pvData, cubDataToRead, cOffset, eAction);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00009DB6 File Offset: 0x00007FB6
		public static int GetCachedUGCCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetCachedUGCCount();
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00009DC2 File Offset: 0x00007FC2
		public static UGCHandle_t GetCachedUGCHandle(int iCachedContent)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCHandle_t)NativeMethods.ISteamRemoteStorage_GetCachedUGCHandle(iCachedContent);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00009DD4 File Offset: 0x00007FD4
		public static SteamAPICall_t PublishWorkshopFile(string pchFile, string pchPreviewFile, AppId_t nConsumerAppId, string pchTitle, string pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IList<string> pTags, EWorkshopFileType eWorkshopFileType)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchTitle))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchDescription))
						{
							steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_PublishWorkshopFile(utf8StringHandle, utf8StringHandle2, nConsumerAppId, utf8StringHandle3, utf8StringHandle4, eVisibility, new InteropHelp.SteamParamStringArray(pTags), eWorkshopFileType);
						}
					}
				}
			}
			return steamAPICall_t;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00009E94 File Offset: 0x00008094
		public static PublishedFileUpdateHandle_t CreatePublishedFileUpdateRequest(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (PublishedFileUpdateHandle_t)NativeMethods.ISteamRemoteStorage_CreatePublishedFileUpdateRequest(unPublishedFileId);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00009EA8 File Offset: 0x000080A8
		public static bool UpdatePublishedFileFile(PublishedFileUpdateHandle_t updateHandle, string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				flag = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileFile(updateHandle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00009EEC File Offset: 0x000080EC
		public static bool UpdatePublishedFilePreviewFile(PublishedFileUpdateHandle_t updateHandle, string pchPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPreviewFile))
			{
				flag = NativeMethods.ISteamRemoteStorage_UpdatePublishedFilePreviewFile(updateHandle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00009F30 File Offset: 0x00008130
		public static bool UpdatePublishedFileTitle(PublishedFileUpdateHandle_t updateHandle, string pchTitle)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				flag = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTitle(updateHandle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00009F74 File Offset: 0x00008174
		public static bool UpdatePublishedFileDescription(PublishedFileUpdateHandle_t updateHandle, string pchDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				flag = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileDescription(updateHandle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00009FB8 File Offset: 0x000081B8
		public static bool UpdatePublishedFileVisibility(PublishedFileUpdateHandle_t updateHandle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileVisibility(updateHandle, eVisibility);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00009FC6 File Offset: 0x000081C6
		public static bool UpdatePublishedFileTags(PublishedFileUpdateHandle_t updateHandle, IList<string> pTags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTags(updateHandle, new InteropHelp.SteamParamStringArray(pTags));
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00009FDE File Offset: 0x000081DE
		public static SteamAPICall_t CommitPublishedFileUpdate(PublishedFileUpdateHandle_t updateHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_CommitPublishedFileUpdate(updateHandle);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00009FF0 File Offset: 0x000081F0
		public static SteamAPICall_t GetPublishedFileDetails(PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetPublishedFileDetails(unPublishedFileId, unMaxSecondsOld);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0000A003 File Offset: 0x00008203
		public static SteamAPICall_t DeletePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_DeletePublishedFile(unPublishedFileId);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0000A015 File Offset: 0x00008215
		public static SteamAPICall_t EnumerateUserPublishedFiles(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserPublishedFiles(unStartIndex);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0000A027 File Offset: 0x00008227
		public static SteamAPICall_t SubscribePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_SubscribePublishedFile(unPublishedFileId);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0000A039 File Offset: 0x00008239
		public static SteamAPICall_t EnumerateUserSubscribedFiles(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserSubscribedFiles(unStartIndex);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0000A04B File Offset: 0x0000824B
		public static SteamAPICall_t UnsubscribePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UnsubscribePublishedFile(unPublishedFileId);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0000A060 File Offset: 0x00008260
		public static bool UpdatePublishedFileSetChangeDescription(PublishedFileUpdateHandle_t updateHandle, string pchChangeDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeDescription))
			{
				flag = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(updateHandle, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0000A0A4 File Offset: 0x000082A4
		public static SteamAPICall_t GetPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetPublishedItemVoteDetails(unPublishedFileId);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0000A0B6 File Offset: 0x000082B6
		public static SteamAPICall_t UpdateUserPublishedItemVote(PublishedFileId_t unPublishedFileId, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UpdateUserPublishedItemVote(unPublishedFileId, bVoteUp);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0000A0C9 File Offset: 0x000082C9
		public static SteamAPICall_t GetUserPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetUserPublishedItemVoteDetails(unPublishedFileId);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0000A0DB File Offset: 0x000082DB
		public static SteamAPICall_t EnumerateUserSharedWorkshopFiles(CSteamID steamId, uint unStartIndex, IList<string> pRequiredTags, IList<string> pExcludedTags)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(steamId, unStartIndex, new InteropHelp.SteamParamStringArray(pRequiredTags), new InteropHelp.SteamParamStringArray(pExcludedTags));
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0000A104 File Offset: 0x00008304
		public static SteamAPICall_t PublishVideo(EWorkshopVideoProvider eVideoProvider, string pchVideoAccount, string pchVideoIdentifier, string pchPreviewFile, AppId_t nConsumerAppId, string pchTitle, string pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IList<string> pTags)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVideoAccount))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchVideoIdentifier))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchTitle))
						{
							using (InteropHelp.UTF8StringHandle utf8StringHandle5 = new InteropHelp.UTF8StringHandle(pchDescription))
							{
								steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_PublishVideo(eVideoProvider, utf8StringHandle, utf8StringHandle2, utf8StringHandle3, nConsumerAppId, utf8StringHandle4, utf8StringHandle5, eVisibility, new InteropHelp.SteamParamStringArray(pTags));
							}
						}
					}
				}
			}
			return steamAPICall_t;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0000A1EC File Offset: 0x000083EC
		public static SteamAPICall_t SetUserPublishedFileAction(PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_SetUserPublishedFileAction(unPublishedFileId, eAction);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0000A1FF File Offset: 0x000083FF
		public static SteamAPICall_t EnumeratePublishedFilesByUserAction(EWorkshopFileAction eAction, uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(eAction, unStartIndex);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0000A212 File Offset: 0x00008412
		public static SteamAPICall_t EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, IList<string> pTags, IList<string> pUserTags)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(eEnumerationType, unStartIndex, unCount, unDays, new InteropHelp.SteamParamStringArray(pTags), new InteropHelp.SteamParamStringArray(pUserTags));
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0000A240 File Offset: 0x00008440
		public static SteamAPICall_t UGCDownloadToLocation(UGCHandle_t hContent, string pchLocation, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLocation))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UGCDownloadToLocation(hContent, utf8StringHandle, unPriority);
			}
			return steamAPICall_t;
		}
	}
}
