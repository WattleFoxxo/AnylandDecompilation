using System;
using System.Collections.Generic;

// Token: 0x0200014C RID: 332
[Serializable]
public class AreaInfo
{
	// Token: 0x0400098B RID: 2443
	public string name;

	// Token: 0x0400098C RID: 2444
	public string description;

	// Token: 0x0400098D RID: 2445
	public string creationDate;

	// Token: 0x0400098E RID: 2446
	public string parentAreaId;

	// Token: 0x0400098F RID: 2447
	public int totalVisitors;

	// Token: 0x04000990 RID: 2448
	public bool isPrivate;

	// Token: 0x04000991 RID: 2449
	public bool isZeroGravity;

	// Token: 0x04000992 RID: 2450
	public bool hasFloatingDust;

	// Token: 0x04000993 RID: 2451
	public bool isCopyable;

	// Token: 0x04000994 RID: 2452
	public bool onlyOwnerSetsLocks;

	// Token: 0x04000995 RID: 2453
	public bool isExcluded;

	// Token: 0x04000996 RID: 2454
	public int renameCount;

	// Token: 0x04000997 RID: 2455
	public int copiedCount;

	// Token: 0x04000998 RID: 2456
	public List<EditorInfo> editors;

	// Token: 0x04000999 RID: 2457
	public List<EditorInfo> listEditors;

	// Token: 0x0400099A RID: 2458
	public List<AreaIdNameAndCreatorId> copiedFromAreas;

	// Token: 0x0400099B RID: 2459
	public bool isFavorited;
}
