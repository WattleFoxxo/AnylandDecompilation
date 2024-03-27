using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x0200023D RID: 573
public class SaveThing_Response : ServerResponse
{
	// Token: 0x060015F3 RID: 5619 RVA: 0x000C066C File Offset: 0x000BEA6C
	public SaveThing_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.thingId = jsonnode["id"].Value;
			}
		}
	}

	// Token: 0x0400133E RID: 4926
	public string thingId;
}
