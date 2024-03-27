using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class GetCreatedThingIds_Response : ServerResponse
{
	// Token: 0x060015D2 RID: 5586 RVA: 0x000BFF60 File Offset: 0x000BE360
	public GetCreatedThingIds_Response(WWW www)
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

	// Token: 0x040012F9 RID: 4857
	public List<string> ThingIds;
}
