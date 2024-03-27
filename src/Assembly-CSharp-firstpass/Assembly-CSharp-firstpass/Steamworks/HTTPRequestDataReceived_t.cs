using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001F4 RID: 500
	[CallbackIdentity(2103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestDataReceived_t
	{
		// Token: 0x0400065B RID: 1627
		public const int k_iCallback = 2103;

		// Token: 0x0400065C RID: 1628
		public HTTPRequestHandle m_hRequest;

		// Token: 0x0400065D RID: 1629
		public ulong m_ulContextValue;

		// Token: 0x0400065E RID: 1630
		public uint m_cOffset;

		// Token: 0x0400065F RID: 1631
		public uint m_cBytesReceived;
	}
}
