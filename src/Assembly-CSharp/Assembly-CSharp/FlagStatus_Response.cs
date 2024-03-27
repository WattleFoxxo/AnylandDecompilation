using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x0200021A RID: 538
public class FlagStatus_Response : ServerResponse
{
	// Token: 0x060015CF RID: 5583 RVA: 0x000BFEB8 File Offset: 0x000BE2B8
	public FlagStatus_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.isFlagged = jsonnode["isFlagged"].AsBool;
			}
			else
			{
				Log.Info("Got null json in FlagStatus_Response", false);
			}
		}
	}

	// Token: 0x040012F6 RID: 4854
	public bool isFlagged;
}
