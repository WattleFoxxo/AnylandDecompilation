using System;
using System.Collections.Generic;

// Token: 0x02000163 RID: 355
[Serializable]
public class LoadAreaData
{
	// Token: 0x04000A1B RID: 2587
	public string _reasonDenied;

	// Token: 0x04000A1C RID: 2588
	public TransportDenialReason reasonDenied;

	// Token: 0x04000A1D RID: 2589
	public string areaId;

	// Token: 0x04000A1E RID: 2590
	public string name;

	// Token: 0x04000A1F RID: 2591
	public string areaName;

	// Token: 0x04000A20 RID: 2592
	public string areaKey;

	// Token: 0x04000A21 RID: 2593
	public string areaCreatorId;

	// Token: 0x04000A22 RID: 2594
	public string parentAreaId;

	// Token: 0x04000A23 RID: 2595
	public bool isPrivate;

	// Token: 0x04000A24 RID: 2596
	public bool isZeroGravity;

	// Token: 0x04000A25 RID: 2597
	public bool hasFloatingDust;

	// Token: 0x04000A26 RID: 2598
	public bool isCopyable;

	// Token: 0x04000A27 RID: 2599
	public bool onlyOwnerSetsLocks;

	// Token: 0x04000A28 RID: 2600
	public bool isExcluded;

	// Token: 0x04000A29 RID: 2601
	public bool requestorIsEditor;

	// Token: 0x04000A2A RID: 2602
	public bool requestorIsListEditor;

	// Token: 0x04000A2B RID: 2603
	public bool requestorIsOwner;

	// Token: 0x04000A2C RID: 2604
	public List<PlacementData> placements;

	// Token: 0x04000A2D RID: 2605
	public string _environmentType;

	// Token: 0x04000A2E RID: 2606
	public EnvironmentType environmentType;

	// Token: 0x04000A2F RID: 2607
	public string environmentChangersJSON;

	// Token: 0x04000A30 RID: 2608
	public EnvironmentChangerDataCollection environmentChangers;

	// Token: 0x04000A31 RID: 2609
	public string settingsJSON;
}
