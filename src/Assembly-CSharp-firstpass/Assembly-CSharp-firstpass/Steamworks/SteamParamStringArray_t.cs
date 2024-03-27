using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B6 RID: 694
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamParamStringArray_t
	{
		// Token: 0x04000C5F RID: 3167
		public IntPtr m_ppStrings;

		// Token: 0x04000C60 RID: 3168
		public int m_nNumStrings;
	}
}
