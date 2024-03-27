using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B9 RID: 441
	[CallbackIdentity(1021)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AppProofOfPurchaseKeyResponse_t
	{
		// Token: 0x0400056F RID: 1391
		public const int k_iCallback = 1021;

		// Token: 0x04000570 RID: 1392
		public EResult m_eResult;

		// Token: 0x04000571 RID: 1393
		public uint m_nAppID;

		// Token: 0x04000572 RID: 1394
		public uint m_cchKeyLength;

		// Token: 0x04000573 RID: 1395
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 240)]
		public string m_rgchKey;
	}
}
