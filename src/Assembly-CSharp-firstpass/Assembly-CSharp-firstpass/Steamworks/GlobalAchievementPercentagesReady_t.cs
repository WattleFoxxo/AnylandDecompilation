using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000256 RID: 598
	[CallbackIdentity(1110)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalAchievementPercentagesReady_t
	{
		// Token: 0x040007BD RID: 1981
		public const int k_iCallback = 1110;

		// Token: 0x040007BE RID: 1982
		public ulong m_nGameID;

		// Token: 0x040007BF RID: 1983
		public EResult m_eResult;
	}
}
