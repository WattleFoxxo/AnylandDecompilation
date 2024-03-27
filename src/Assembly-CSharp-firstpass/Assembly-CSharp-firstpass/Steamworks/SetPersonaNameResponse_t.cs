using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001CC RID: 460
	[CallbackIdentity(347)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetPersonaNameResponse_t
	{
		// Token: 0x040005B4 RID: 1460
		public const int k_iCallback = 347;

		// Token: 0x040005B5 RID: 1461
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSuccess;

		// Token: 0x040005B6 RID: 1462
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocalSuccess;

		// Token: 0x040005B7 RID: 1463
		public EResult m_result;
	}
}
