using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200024D RID: 589
	[CallbackIdentity(1101)]
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	public struct UserStatsReceived_t
	{
		// Token: 0x04000798 RID: 1944
		public const int k_iCallback = 1101;

		// Token: 0x04000799 RID: 1945
		[FieldOffset(0)]
		public ulong m_nGameID;

		// Token: 0x0400079A RID: 1946
		[FieldOffset(8)]
		public EResult m_eResult;

		// Token: 0x0400079B RID: 1947
		[FieldOffset(12)]
		public CSteamID m_steamIDUser;
	}
}
