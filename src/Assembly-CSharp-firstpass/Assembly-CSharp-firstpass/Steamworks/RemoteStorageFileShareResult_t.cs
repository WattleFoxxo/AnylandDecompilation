using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200021B RID: 539
	[CallbackIdentity(1307)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileShareResult_t
	{
		// Token: 0x040006CC RID: 1740
		public const int k_iCallback = 1307;

		// Token: 0x040006CD RID: 1741
		public EResult m_eResult;

		// Token: 0x040006CE RID: 1742
		public UGCHandle_t m_hFile;

		// Token: 0x040006CF RID: 1743
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_rgchFilename;
	}
}
