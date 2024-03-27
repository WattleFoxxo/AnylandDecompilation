using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001F9 RID: 505
	[CallbackIdentity(502)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListChanged_t
	{
		// Token: 0x0400066B RID: 1643
		public const int k_iCallback = 502;

		// Token: 0x0400066C RID: 1644
		public uint m_nIP;

		// Token: 0x0400066D RID: 1645
		public uint m_nQueryPort;

		// Token: 0x0400066E RID: 1646
		public uint m_nConnPort;

		// Token: 0x0400066F RID: 1647
		public uint m_nAppID;

		// Token: 0x04000670 RID: 1648
		public uint m_nFlags;

		// Token: 0x04000671 RID: 1649
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAdd;

		// Token: 0x04000672 RID: 1650
		public AccountID_t m_unAccountId;
	}
}
