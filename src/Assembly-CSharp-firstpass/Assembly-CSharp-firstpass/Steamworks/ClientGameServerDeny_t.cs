using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000244 RID: 580
	[CallbackIdentity(113)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ClientGameServerDeny_t
	{
		// Token: 0x0400077E RID: 1918
		public const int k_iCallback = 113;

		// Token: 0x0400077F RID: 1919
		public uint m_uAppID;

		// Token: 0x04000780 RID: 1920
		public uint m_unGameServerIP;

		// Token: 0x04000781 RID: 1921
		public ushort m_usGameServerPort;

		// Token: 0x04000782 RID: 1922
		public ushort m_bSecure;

		// Token: 0x04000783 RID: 1923
		public uint m_uReason;
	}
}
