using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200024C RID: 588
	[CallbackIdentity(165)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StoreAuthURLResponse_t
	{
		// Token: 0x04000796 RID: 1942
		public const int k_iCallback = 165;

		// Token: 0x04000797 RID: 1943
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string m_szURL;
	}
}
