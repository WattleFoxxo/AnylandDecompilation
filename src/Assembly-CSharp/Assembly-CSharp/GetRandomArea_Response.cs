using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x0200022B RID: 555
public class GetRandomArea_Response : ServerResponse
{
	// Token: 0x060015E0 RID: 5600 RVA: 0x000C00E8 File Offset: 0x000BE4E8
	public GetRandomArea_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.areaId = jsonnode["areaId"].Value;
			}
		}
	}

	// Token: 0x04001319 RID: 4889
	public string areaId;
}
