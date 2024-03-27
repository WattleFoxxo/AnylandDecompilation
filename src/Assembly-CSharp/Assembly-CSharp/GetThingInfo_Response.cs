using System;
using UnityEngine;

// Token: 0x02000232 RID: 562
public class GetThingInfo_Response : ServerResponse
{
	// Token: 0x060015E7 RID: 5607 RVA: 0x000C020F File Offset: 0x000BE60F
	public GetThingInfo_Response(WWW www)
		: base(www)
	{
		if (www.error == null && www.text != "null")
		{
			this.thingInfo = JsonUtility.FromJson<ThingInfo>(www.text);
		}
	}

	// Token: 0x04001322 RID: 4898
	public ThingInfo thingInfo;
}
