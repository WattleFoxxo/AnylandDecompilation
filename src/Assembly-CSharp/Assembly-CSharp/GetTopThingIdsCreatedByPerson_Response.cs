using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000234 RID: 564
public class GetTopThingIdsCreatedByPerson_Response : ServerResponse
{
	// Token: 0x060015E9 RID: 5609 RVA: 0x000C025C File Offset: 0x000BE65C
	public GetTopThingIdsCreatedByPerson_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.Ids = new List<string>();
				for (int i = 0; i < jsonnode["ids"].Count; i++)
				{
					this.Ids.Add(jsonnode["ids"][i].Value);
				}
			}
		}
	}

	// Token: 0x04001324 RID: 4900
	public List<string> Ids;
}
