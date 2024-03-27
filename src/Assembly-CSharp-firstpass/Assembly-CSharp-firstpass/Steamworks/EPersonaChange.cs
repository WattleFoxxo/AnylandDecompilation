using System;

namespace Steamworks
{
	// Token: 0x0200026E RID: 622
	[Flags]
	public enum EPersonaChange
	{
		// Token: 0x04000983 RID: 2435
		k_EPersonaChangeName = 1,
		// Token: 0x04000984 RID: 2436
		k_EPersonaChangeStatus = 2,
		// Token: 0x04000985 RID: 2437
		k_EPersonaChangeComeOnline = 4,
		// Token: 0x04000986 RID: 2438
		k_EPersonaChangeGoneOffline = 8,
		// Token: 0x04000987 RID: 2439
		k_EPersonaChangeGamePlayed = 16,
		// Token: 0x04000988 RID: 2440
		k_EPersonaChangeGameServer = 32,
		// Token: 0x04000989 RID: 2441
		k_EPersonaChangeAvatar = 64,
		// Token: 0x0400098A RID: 2442
		k_EPersonaChangeJoinedSource = 128,
		// Token: 0x0400098B RID: 2443
		k_EPersonaChangeLeftSource = 256,
		// Token: 0x0400098C RID: 2444
		k_EPersonaChangeRelationshipChanged = 512,
		// Token: 0x0400098D RID: 2445
		k_EPersonaChangeNameFirstSet = 1024,
		// Token: 0x0400098E RID: 2446
		k_EPersonaChangeFacebookInfo = 2048,
		// Token: 0x0400098F RID: 2447
		k_EPersonaChangeNickname = 4096,
		// Token: 0x04000990 RID: 2448
		k_EPersonaChangeSteamLevel = 8192
	}
}
