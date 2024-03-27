using System;

// Token: 0x02000065 RID: 101
public class ErrorCode
{
	// Token: 0x0400020F RID: 527
	public const int Ok = 0;

	// Token: 0x04000210 RID: 528
	public const int OperationNotAllowedInCurrentState = -3;

	// Token: 0x04000211 RID: 529
	[Obsolete("Use InvalidOperation.")]
	public const int InvalidOperationCode = -2;

	// Token: 0x04000212 RID: 530
	public const int InvalidOperation = -2;

	// Token: 0x04000213 RID: 531
	public const int InternalServerError = -1;

	// Token: 0x04000214 RID: 532
	public const int InvalidAuthentication = 32767;

	// Token: 0x04000215 RID: 533
	public const int GameIdAlreadyExists = 32766;

	// Token: 0x04000216 RID: 534
	public const int GameFull = 32765;

	// Token: 0x04000217 RID: 535
	public const int GameClosed = 32764;

	// Token: 0x04000218 RID: 536
	[Obsolete("No longer used, cause random matchmaking is no longer a process.")]
	public const int AlreadyMatched = 32763;

	// Token: 0x04000219 RID: 537
	public const int ServerFull = 32762;

	// Token: 0x0400021A RID: 538
	public const int UserBlocked = 32761;

	// Token: 0x0400021B RID: 539
	public const int NoRandomMatchFound = 32760;

	// Token: 0x0400021C RID: 540
	public const int GameDoesNotExist = 32758;

	// Token: 0x0400021D RID: 541
	public const int MaxCcuReached = 32757;

	// Token: 0x0400021E RID: 542
	public const int InvalidRegion = 32756;

	// Token: 0x0400021F RID: 543
	public const int CustomAuthenticationFailed = 32755;

	// Token: 0x04000220 RID: 544
	public const int AuthenticationTicketExpired = 32753;

	// Token: 0x04000221 RID: 545
	public const int PluginReportedError = 32752;

	// Token: 0x04000222 RID: 546
	public const int PluginMismatch = 32751;

	// Token: 0x04000223 RID: 547
	public const int JoinFailedPeerAlreadyJoined = 32750;

	// Token: 0x04000224 RID: 548
	public const int JoinFailedFoundInactiveJoiner = 32749;

	// Token: 0x04000225 RID: 549
	public const int JoinFailedWithRejoinerNotFound = 32748;

	// Token: 0x04000226 RID: 550
	public const int JoinFailedFoundExcludedUserId = 32747;

	// Token: 0x04000227 RID: 551
	public const int JoinFailedFoundActiveJoiner = 32746;

	// Token: 0x04000228 RID: 552
	public const int HttpLimitReached = 32745;

	// Token: 0x04000229 RID: 553
	public const int ExternalHttpCallFailed = 32744;

	// Token: 0x0400022A RID: 554
	public const int SlotError = 32742;

	// Token: 0x0400022B RID: 555
	public const int InvalidEncryptionParameters = 32741;
}
