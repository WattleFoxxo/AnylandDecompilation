using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200024A RID: 586
	[CallbackIdentity(163)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetAuthSessionTicketResponse_t
	{
		// Token: 0x04000791 RID: 1937
		public const int k_iCallback = 163;

		// Token: 0x04000792 RID: 1938
		public HAuthTicket m_hAuthTicket;

		// Token: 0x04000793 RID: 1939
		public EResult m_eResult;
	}
}
