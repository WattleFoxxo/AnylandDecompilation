using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class GetRecentlyDeletedThingIds_Response : ServerResponse
{
	// Token: 0x060015E2 RID: 5602 RVA: 0x000C0148 File Offset: 0x000BE548
	public GetRecentlyDeletedThingIds_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.ThingIds = new List<string>();
				for (int i = 0; i < jsonnode["thingIds"].Count; i++)
				{
					this.ThingIds.Add(jsonnode["thingIds"][i].Value);
				}
			}
		}
	}

	// Token: 0x0400131B RID: 4891
	public List<string> ThingIds;
}
