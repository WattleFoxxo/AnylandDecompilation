using System;

namespace Steamworks
{
	// Token: 0x0200026B RID: 619
	[Flags]
	public enum EFriendFlags
	{
		// Token: 0x04000969 RID: 2409
		k_EFriendFlagNone = 0,
		// Token: 0x0400096A RID: 2410
		k_EFriendFlagBlocked = 1,
		// Token: 0x0400096B RID: 2411
		k_EFriendFlagFriendshipRequested = 2,
		// Token: 0x0400096C RID: 2412
		k_EFriendFlagImmediate = 4,
		// Token: 0x0400096D RID: 2413
		k_EFriendFlagClanMember = 8,
		// Token: 0x0400096E RID: 2414
		k_EFriendFlagOnGameServer = 16,
		// Token: 0x0400096F RID: 2415
		k_EFriendFlagRequestingFriendship = 128,
		// Token: 0x04000970 RID: 2416
		k_EFriendFlagRequestingInfo = 256,
		// Token: 0x04000971 RID: 2417
		k_EFriendFlagIgnored = 512,
		// Token: 0x04000972 RID: 2418
		k_EFriendFlagIgnoredFriend = 1024,
		// Token: 0x04000973 RID: 2419
		k_EFriendFlagChatMember = 4096,
		// Token: 0x04000974 RID: 2420
		k_EFriendFlagAll = 65535
	}
}
