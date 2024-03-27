using System;

// Token: 0x0200006E RID: 110
public enum EventCaching : byte
{
	// Token: 0x040002AB RID: 683
	DoNotCache,
	// Token: 0x040002AC RID: 684
	[Obsolete]
	MergeCache,
	// Token: 0x040002AD RID: 685
	[Obsolete]
	ReplaceCache,
	// Token: 0x040002AE RID: 686
	[Obsolete]
	RemoveCache,
	// Token: 0x040002AF RID: 687
	AddToRoomCache,
	// Token: 0x040002B0 RID: 688
	AddToRoomCacheGlobal,
	// Token: 0x040002B1 RID: 689
	RemoveFromRoomCache,
	// Token: 0x040002B2 RID: 690
	RemoveFromRoomCacheForActorsLeft,
	// Token: 0x040002B3 RID: 691
	SliceIncreaseIndex = 10,
	// Token: 0x040002B4 RID: 692
	SliceSetIndex,
	// Token: 0x040002B5 RID: 693
	SlicePurgeIndex,
	// Token: 0x040002B6 RID: 694
	SlicePurgeUpToIndex
}
