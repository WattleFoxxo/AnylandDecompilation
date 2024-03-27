using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000205 RID: 517
	[CallbackIdentity(4002)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct VolumeHasChanged_t
	{
		// Token: 0x0400069B RID: 1691
		public const int k_iCallback = 4002;

		// Token: 0x0400069C RID: 1692
		public float m_flNewVolume;
	}
}
