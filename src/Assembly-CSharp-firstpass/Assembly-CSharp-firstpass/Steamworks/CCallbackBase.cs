using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002C0 RID: 704
	[StructLayout(LayoutKind.Sequential)]
	internal class CCallbackBase
	{
		// Token: 0x04000C96 RID: 3222
		public const byte k_ECallbackFlagsRegistered = 1;

		// Token: 0x04000C97 RID: 3223
		public const byte k_ECallbackFlagsGameServer = 2;

		// Token: 0x04000C98 RID: 3224
		public IntPtr m_vfptr;

		// Token: 0x04000C99 RID: 3225
		public byte m_nCallbackFlags;

		// Token: 0x04000C9A RID: 3226
		public int m_iCallback;
	}
}
