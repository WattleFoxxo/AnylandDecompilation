using System;
using UnityEngine;

// Token: 0x02000225 RID: 549
public class GetFriendsByStrength_Response : ServerResponse
{
	// Token: 0x060015DA RID: 5594 RVA: 0x000C0044 File Offset: 0x000BE444
	public GetFriendsByStrength_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.friends = JsonUtility.FromJson<SplitFriendsLists>(www.text);
		}
	}

	// Token: 0x0400130F RID: 4879
	public SplitFriendsLists friends;
}
