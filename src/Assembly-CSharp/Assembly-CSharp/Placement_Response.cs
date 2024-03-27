using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class Placement_Response : ServerResponse
{
	// Token: 0x060015EC RID: 5612 RVA: 0x000C047C File Offset: 0x000BE87C
	public Placement_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.ok = jsonnode["ok"].AsBool;
			}
			else
			{
				Log.Info("Got null json in Placement_Response", false);
			}
		}
	}

	// Token: 0x0400132A RID: 4906
	public bool ok;
}
