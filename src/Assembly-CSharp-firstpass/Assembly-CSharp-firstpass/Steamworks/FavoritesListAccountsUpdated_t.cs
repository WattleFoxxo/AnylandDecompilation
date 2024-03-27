using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000203 RID: 515
	[CallbackIdentity(516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListAccountsUpdated_t
	{
		// Token: 0x04000698 RID: 1688
		public const int k_iCallback = 516;

		// Token: 0x04000699 RID: 1689
		public EResult m_eResult;
	}
}
