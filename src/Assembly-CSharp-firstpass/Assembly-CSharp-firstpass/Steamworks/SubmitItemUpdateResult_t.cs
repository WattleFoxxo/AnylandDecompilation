using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000238 RID: 568
	[CallbackIdentity(3404)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SubmitItemUpdateResult_t
	{
		// Token: 0x04000757 RID: 1879
		public const int k_iCallback = 3404;

		// Token: 0x04000758 RID: 1880
		public EResult m_eResult;

		// Token: 0x04000759 RID: 1881
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
