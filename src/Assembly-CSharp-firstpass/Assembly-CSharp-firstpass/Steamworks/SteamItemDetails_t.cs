using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B4 RID: 692
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamItemDetails_t
	{
		// Token: 0x04000C53 RID: 3155
		public SteamItemInstanceID_t m_itemId;

		// Token: 0x04000C54 RID: 3156
		public SteamItemDef_t m_iDefinition;

		// Token: 0x04000C55 RID: 3157
		public ushort m_unQuantity;

		// Token: 0x04000C56 RID: 3158
		public ushort m_unFlags;
	}
}
