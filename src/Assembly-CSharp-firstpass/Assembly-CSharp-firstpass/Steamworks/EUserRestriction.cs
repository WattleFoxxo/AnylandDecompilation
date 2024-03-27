using System;

namespace Steamworks
{
	// Token: 0x0200026C RID: 620
	public enum EUserRestriction
	{
		// Token: 0x04000976 RID: 2422
		k_nUserRestrictionNone,
		// Token: 0x04000977 RID: 2423
		k_nUserRestrictionUnknown,
		// Token: 0x04000978 RID: 2424
		k_nUserRestrictionAnyChat,
		// Token: 0x04000979 RID: 2425
		k_nUserRestrictionVoiceChat = 4,
		// Token: 0x0400097A RID: 2426
		k_nUserRestrictionGroupChat = 8,
		// Token: 0x0400097B RID: 2427
		k_nUserRestrictionRating = 16,
		// Token: 0x0400097C RID: 2428
		k_nUserRestrictionGameInvites = 32,
		// Token: 0x0400097D RID: 2429
		k_nUserRestrictionTrading = 64
	}
}
