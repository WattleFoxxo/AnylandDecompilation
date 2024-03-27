using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200024B RID: 587
	[CallbackIdentity(164)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameWebCallback_t
	{
		// Token: 0x04000794 RID: 1940
		public const int k_iCallback = 164;

		// Token: 0x04000795 RID: 1941
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szURL;
	}
}
