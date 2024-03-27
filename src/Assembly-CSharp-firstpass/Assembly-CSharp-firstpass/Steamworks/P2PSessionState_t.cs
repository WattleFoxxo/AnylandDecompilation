using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B5 RID: 693
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionState_t
	{
		// Token: 0x04000C57 RID: 3159
		public byte m_bConnectionActive;

		// Token: 0x04000C58 RID: 3160
		public byte m_bConnecting;

		// Token: 0x04000C59 RID: 3161
		public byte m_eP2PSessionError;

		// Token: 0x04000C5A RID: 3162
		public byte m_bUsingRelay;

		// Token: 0x04000C5B RID: 3163
		public int m_nBytesQueuedForSend;

		// Token: 0x04000C5C RID: 3164
		public int m_nPacketsQueuedForSend;

		// Token: 0x04000C5D RID: 3165
		public uint m_nRemoteIP;

		// Token: 0x04000C5E RID: 3166
		public ushort m_nRemotePort;
	}
}
