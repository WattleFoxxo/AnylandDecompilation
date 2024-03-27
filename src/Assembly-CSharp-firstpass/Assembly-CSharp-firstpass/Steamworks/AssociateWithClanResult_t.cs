using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D7 RID: 471
	[CallbackIdentity(210)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AssociateWithClanResult_t
	{
		// Token: 0x040005DD RID: 1501
		public const int k_iCallback = 210;

		// Token: 0x040005DE RID: 1502
		public EResult m_eResult;
	}
}
