using System;
using UnityEngine;

// Token: 0x0200022F RID: 559
public class GetThingDefinition_Response : ServerResponse
{
	// Token: 0x060015E4 RID: 5604 RVA: 0x000C01DF File Offset: 0x000BE5DF
	public GetThingDefinition_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.thingDefinitionJSON = www.text;
		}
	}

	// Token: 0x0400131D RID: 4893
	public string thingDefinitionJSON;
}
