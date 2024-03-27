using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B7 RID: 439
	[CallbackIdentity(1008)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RegisterActivationCodeResponse_t
	{
		// Token: 0x0400056B RID: 1387
		public const int k_iCallback = 1008;

		// Token: 0x0400056C RID: 1388
		public ERegisterActivationCodeResult m_eResult;

		// Token: 0x0400056D RID: 1389
		public uint m_unPackageRegistered;
	}
}
