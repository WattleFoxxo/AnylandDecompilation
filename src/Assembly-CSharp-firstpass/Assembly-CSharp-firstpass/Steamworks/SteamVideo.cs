using System;

namespace Steamworks
{
	// Token: 0x020001B2 RID: 434
	public static class SteamVideo
	{
		// Token: 0x060008DF RID: 2271 RVA: 0x0000BB73 File Offset: 0x00009D73
		public static void GetVideoURL(AppId_t unVideoAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamVideo_GetVideoURL(unVideoAppID);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0000BB80 File Offset: 0x00009D80
		public static bool IsBroadcasting(out int pnNumViewers)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamVideo_IsBroadcasting(out pnNumViewers);
		}
	}
}
