using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BC RID: 444
	[CallbackIdentity(331)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameOverlayActivated_t
	{
		// Token: 0x0400057C RID: 1404
		public const int k_iCallback = 331;

		// Token: 0x0400057D RID: 1405
		public byte m_bActive;
	}
}
