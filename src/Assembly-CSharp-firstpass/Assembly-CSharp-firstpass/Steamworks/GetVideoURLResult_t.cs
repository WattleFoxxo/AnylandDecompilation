using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000261 RID: 609
	[CallbackIdentity(4611)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetVideoURLResult_t
	{
		// Token: 0x040007D6 RID: 2006
		public const int k_iCallback = 4611;

		// Token: 0x040007D7 RID: 2007
		public EResult m_eResult;

		// Token: 0x040007D8 RID: 2008
		public AppId_t m_unVideoAppID;

		// Token: 0x040007D9 RID: 2009
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;
	}
}
