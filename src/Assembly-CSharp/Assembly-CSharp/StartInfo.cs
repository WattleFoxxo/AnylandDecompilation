using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200016B RID: 363
[Serializable]
public class StartInfo
{
	// Token: 0x04000A50 RID: 2640
	public string versionMajorServerAndClient;

	// Token: 0x04000A51 RID: 2641
	public string versionMinorServerOnly;

	// Token: 0x04000A52 RID: 2642
	public string personId;

	// Token: 0x04000A53 RID: 2643
	public string screenName;

	// Token: 0x04000A54 RID: 2644
	public string statusText;

	// Token: 0x04000A55 RID: 2645
	public bool isFindable;

	// Token: 0x04000A56 RID: 2646
	public string homeAreaId;

	// Token: 0x04000A57 RID: 2647
	public int age;

	// Token: 0x04000A58 RID: 2648
	public int ageSecs;

	// Token: 0x04000A59 RID: 2649
	public AttachmentDataSet attachmentsData;

	// Token: 0x04000A5A RID: 2650
	public Color? handColor;

	// Token: 0x04000A5B RID: 2651
	public bool isHardBanned;

	// Token: 0x04000A5C RID: 2652
	public bool isSoftBanned;

	// Token: 0x04000A5D RID: 2653
	public bool showFlagWarning;

	// Token: 0x04000A5E RID: 2654
	public string[] flagTags;

	// Token: 0x04000A5F RID: 2655
	public int areaCount;

	// Token: 0x04000A60 RID: 2656
	public int thingTagCount;

	// Token: 0x04000A61 RID: 2657
	public bool allThingsClonable;

	// Token: 0x04000A62 RID: 2658
	public bool hasEditTools;

	// Token: 0x04000A63 RID: 2659
	public bool hasEditToolsPermanently;

	// Token: 0x04000A64 RID: 2660
	public string editToolsExpiryDate;

	// Token: 0x04000A65 RID: 2661
	public bool isInEditToolsTrial;

	// Token: 0x04000A66 RID: 2662
	public bool wasEditToolsTrialEverActivated;

	// Token: 0x04000A67 RID: 2663
	public int timesEditToolsPurchased;

	// Token: 0x04000A68 RID: 2664
	public string customSearchWords;

	// Token: 0x04000A69 RID: 2665
	public List<Achievement> achievements = new List<Achievement>();
}
