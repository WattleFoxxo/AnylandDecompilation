using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x0200023B RID: 571
public class RenameArea_Response : ExtendedServerResponse
{
	// Token: 0x060015F0 RID: 5616 RVA: 0x000C0628 File Offset: 0x000BEA28
	public RenameArea_Response(WWW www)
		: base(www)
	{
		JSONNode jsonnode = JSON.Parse(www.text);
		if (jsonnode != null)
		{
			this.reasonFailed = jsonnode["reasonFailed"].Value;
		}
	}
}
