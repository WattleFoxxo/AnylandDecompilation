using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200025E RID: 606
	[CallbackIdentity(714)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GamepadTextInputDismissed_t
	{
		// Token: 0x040007D0 RID: 2000
		public const int k_iCallback = 714;

		// Token: 0x040007D1 RID: 2001
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSubmitted;

		// Token: 0x040007D2 RID: 2002
		public uint m_unSubmittedText;
	}
}
