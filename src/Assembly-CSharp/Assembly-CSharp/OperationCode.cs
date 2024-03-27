using System;

// Token: 0x0200006A RID: 106
public class OperationCode
{
	// Token: 0x04000289 RID: 649
	[Obsolete("Exchanging encrpytion keys is done internally in the lib now. Don't expect this operation-result.")]
	public const byte ExchangeKeysForEncryption = 250;

	// Token: 0x0400028A RID: 650
	[Obsolete]
	public const byte Join = 255;

	// Token: 0x0400028B RID: 651
	public const byte AuthenticateOnce = 231;

	// Token: 0x0400028C RID: 652
	public const byte Authenticate = 230;

	// Token: 0x0400028D RID: 653
	public const byte JoinLobby = 229;

	// Token: 0x0400028E RID: 654
	public const byte LeaveLobby = 228;

	// Token: 0x0400028F RID: 655
	public const byte CreateGame = 227;

	// Token: 0x04000290 RID: 656
	public const byte JoinGame = 226;

	// Token: 0x04000291 RID: 657
	public const byte JoinRandomGame = 225;

	// Token: 0x04000292 RID: 658
	public const byte Leave = 254;

	// Token: 0x04000293 RID: 659
	public const byte RaiseEvent = 253;

	// Token: 0x04000294 RID: 660
	public const byte SetProperties = 252;

	// Token: 0x04000295 RID: 661
	public const byte GetProperties = 251;

	// Token: 0x04000296 RID: 662
	public const byte ChangeGroups = 248;

	// Token: 0x04000297 RID: 663
	public const byte FindFriends = 222;

	// Token: 0x04000298 RID: 664
	public const byte GetLobbyStats = 221;

	// Token: 0x04000299 RID: 665
	public const byte GetRegions = 220;

	// Token: 0x0400029A RID: 666
	public const byte WebRpc = 219;

	// Token: 0x0400029B RID: 667
	public const byte ServerSettings = 218;

	// Token: 0x0400029C RID: 668
	public const byte GetGameList = 217;
}
