using System;

// Token: 0x02000068 RID: 104
public class EventCode
{
	// Token: 0x04000238 RID: 568
	public const byte GameList = 230;

	// Token: 0x04000239 RID: 569
	public const byte GameListUpdate = 229;

	// Token: 0x0400023A RID: 570
	public const byte QueueState = 228;

	// Token: 0x0400023B RID: 571
	public const byte Match = 227;

	// Token: 0x0400023C RID: 572
	public const byte AppStats = 226;

	// Token: 0x0400023D RID: 573
	public const byte LobbyStats = 224;

	// Token: 0x0400023E RID: 574
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureNodeInfo = 210;

	// Token: 0x0400023F RID: 575
	public const byte Join = 255;

	// Token: 0x04000240 RID: 576
	public const byte Leave = 254;

	// Token: 0x04000241 RID: 577
	public const byte PropertiesChanged = 253;

	// Token: 0x04000242 RID: 578
	[Obsolete("Use PropertiesChanged now.")]
	public const byte SetProperties = 253;

	// Token: 0x04000243 RID: 579
	public const byte ErrorInfo = 251;

	// Token: 0x04000244 RID: 580
	public const byte CacheSliceChanged = 250;

	// Token: 0x04000245 RID: 581
	public const byte AuthEvent = 223;
}
