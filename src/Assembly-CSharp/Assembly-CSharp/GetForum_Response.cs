using System;
using System.Collections.Generic;

// Token: 0x0200021F RID: 543
public class GetForum_Response : ResponseBase
{
	// Token: 0x040012FB RID: 4859
	public bool ok;

	// Token: 0x040012FC RID: 4860
	public string reasonFailed;

	// Token: 0x040012FD RID: 4861
	public ForumData forum;

	// Token: 0x040012FE RID: 4862
	public List<ForumThreadData> stickies;

	// Token: 0x040012FF RID: 4863
	public List<ForumThreadData> threads;
}
