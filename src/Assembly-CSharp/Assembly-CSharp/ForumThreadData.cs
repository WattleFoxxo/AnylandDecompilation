using System;
using System.Collections.Generic;

// Token: 0x02000159 RID: 345
[Serializable]
public class ForumThreadData
{
	// Token: 0x040009E3 RID: 2531
	public string id;

	// Token: 0x040009E4 RID: 2532
	public string forumId;

	// Token: 0x040009E5 RID: 2533
	public string title;

	// Token: 0x040009E6 RID: 2534
	public string titleClarification;

	// Token: 0x040009E7 RID: 2535
	public string creatorId;

	// Token: 0x040009E8 RID: 2536
	public string creatorName;

	// Token: 0x040009E9 RID: 2537
	public string creationDate;

	// Token: 0x040009EA RID: 2538
	public bool isSticky;

	// Token: 0x040009EB RID: 2539
	public bool isLocked;

	// Token: 0x040009EC RID: 2540
	public string latestCommentDate;

	// Token: 0x040009ED RID: 2541
	public string latestCommentUserId;

	// Token: 0x040009EE RID: 2542
	public string latestCommentUserName;

	// Token: 0x040009EF RID: 2543
	public string latestCommentContent;

	// Token: 0x040009F0 RID: 2544
	public int commentCount;

	// Token: 0x040009F1 RID: 2545
	public List<ForumCommentData> comments;
}
