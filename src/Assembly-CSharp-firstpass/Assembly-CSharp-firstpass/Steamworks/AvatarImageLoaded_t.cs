using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BF RID: 447
	[CallbackIdentity(334)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AvatarImageLoaded_t
	{
		// Token: 0x04000584 RID: 1412
		public const int k_iCallback = 334;

		// Token: 0x04000585 RID: 1413
		public CSteamID m_steamID;

		// Token: 0x04000586 RID: 1414
		public int m_iImage;

		// Token: 0x04000587 RID: 1415
		public int m_iWide;

		// Token: 0x04000588 RID: 1416
		public int m_iTall;
	}
}
