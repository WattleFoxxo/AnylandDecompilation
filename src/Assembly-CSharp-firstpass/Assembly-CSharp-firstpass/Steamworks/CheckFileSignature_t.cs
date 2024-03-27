using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200025D RID: 605
	[CallbackIdentity(705)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CheckFileSignature_t
	{
		// Token: 0x040007CE RID: 1998
		public const int k_iCallback = 705;

		// Token: 0x040007CF RID: 1999
		public ECheckFileSignature m_eCheckFileSignature;
	}
}
