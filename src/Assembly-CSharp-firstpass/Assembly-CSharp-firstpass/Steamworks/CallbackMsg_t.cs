using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B8 RID: 696
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CallbackMsg_t
	{
		// Token: 0x04000C7B RID: 3195
		public int m_hSteamUser;

		// Token: 0x04000C7C RID: 3196
		public int m_iCallback;

		// Token: 0x04000C7D RID: 3197
		public IntPtr m_pubParam;

		// Token: 0x04000C7E RID: 3198
		public int m_cubParam;
	}
}
