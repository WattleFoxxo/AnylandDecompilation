using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B7 RID: 695
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCDetails_t
	{
		// Token: 0x04000C61 RID: 3169
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000C62 RID: 3170
		public EResult m_eResult;

		// Token: 0x04000C63 RID: 3171
		public EWorkshopFileType m_eFileType;

		// Token: 0x04000C64 RID: 3172
		public AppId_t m_nCreatorAppID;

		// Token: 0x04000C65 RID: 3173
		public AppId_t m_nConsumerAppID;

		// Token: 0x04000C66 RID: 3174
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		public string m_rgchTitle;

		// Token: 0x04000C67 RID: 3175
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8000)]
		public string m_rgchDescription;

		// Token: 0x04000C68 RID: 3176
		public ulong m_ulSteamIDOwner;

		// Token: 0x04000C69 RID: 3177
		public uint m_rtimeCreated;

		// Token: 0x04000C6A RID: 3178
		public uint m_rtimeUpdated;

		// Token: 0x04000C6B RID: 3179
		public uint m_rtimeAddedToUserList;

		// Token: 0x04000C6C RID: 3180
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x04000C6D RID: 3181
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000C6E RID: 3182
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;

		// Token: 0x04000C6F RID: 3183
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x04000C70 RID: 3184
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
		public string m_rgchTags;

		// Token: 0x04000C71 RID: 3185
		public UGCHandle_t m_hFile;

		// Token: 0x04000C72 RID: 3186
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x04000C73 RID: 3187
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x04000C74 RID: 3188
		public int m_nFileSize;

		// Token: 0x04000C75 RID: 3189
		public int m_nPreviewFileSize;

		// Token: 0x04000C76 RID: 3190
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;

		// Token: 0x04000C77 RID: 3191
		public uint m_unVotesUp;

		// Token: 0x04000C78 RID: 3192
		public uint m_unVotesDown;

		// Token: 0x04000C79 RID: 3193
		public float m_flScore;

		// Token: 0x04000C7A RID: 3194
		public uint m_unNumChildren;
	}
}
