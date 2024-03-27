using System;
using System.Collections.Generic;

// Token: 0x02000156 RID: 342
[Serializable]
public class ForumCommentData
{
	// Token: 0x040009C6 RID: 2502
	public string date;

	// Token: 0x040009C7 RID: 2503
	public string userId;

	// Token: 0x040009C8 RID: 2504
	public string userName;

	// Token: 0x040009C9 RID: 2505
	public string text;

	// Token: 0x040009CA RID: 2506
	public string thingId;

	// Token: 0x040009CB RID: 2507
	public string lastEditedDate;

	// Token: 0x040009CC RID: 2508
	public List<string> likes;

	// Token: 0x040009CD RID: 2509
	public List<ForumCommentLikePersonIdAndName> oldestLikes;

	// Token: 0x040009CE RID: 2510
	public List<ForumCommentLikePersonIdAndName> newestLikes;

	// Token: 0x040009CF RID: 2511
	public int totalLikes;
}
