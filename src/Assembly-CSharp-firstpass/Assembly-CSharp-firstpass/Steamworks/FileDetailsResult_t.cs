using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BA RID: 442
	[CallbackIdentity(1023)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FileDetailsResult_t
	{
		// Token: 0x04000574 RID: 1396
		public const int k_iCallback = 1023;

		// Token: 0x04000575 RID: 1397
		public EResult m_eResult;

		// Token: 0x04000576 RID: 1398
		public ulong m_ulFileSize;

		// Token: 0x04000577 RID: 1399
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public byte[] m_FileSHA;

		// Token: 0x04000578 RID: 1400
		public uint m_unFlags;
	}
}
