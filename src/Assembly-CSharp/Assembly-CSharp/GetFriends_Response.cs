using System;
using UnityEngine;

// Token: 0x02000224 RID: 548
public class GetFriends_Response : ServerResponse
{
	// Token: 0x060015D9 RID: 5593 RVA: 0x000C001F File Offset: 0x000BE41F
	public GetFriends_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.friends = JsonUtility.FromJson<FriendListInfoCollection>(www.text);
		}
	}

	// Token: 0x0400130E RID: 4878
	public FriendListInfoCollection friends;
}
