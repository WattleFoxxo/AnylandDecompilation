using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BB RID: 443
	[CallbackIdentity(304)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct PersonaStateChange_t
	{
		// Token: 0x04000579 RID: 1401
		public const int k_iCallback = 304;

		// Token: 0x0400057A RID: 1402
		public ulong m_ulSteamID;

		// Token: 0x0400057B RID: 1403
		public EPersonaChange m_nChangeFlags;
	}
}
