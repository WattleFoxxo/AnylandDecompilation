using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000219 RID: 537
public class ExtendedServerResponse : ServerResponse
{
	// Token: 0x060015CE RID: 5582 RVA: 0x000BFE2C File Offset: 0x000BE22C
	public ExtendedServerResponse(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.ok = jsonnode["ok"].AsBool;
				this.softError = jsonnode["err"].Value;
				this.reasonFailed = jsonnode["reasonFailed"].Value;
			}
			else
			{
				Log.Info("Got null json in ExtendedServerResponse", false);
			}
		}
	}

	// Token: 0x040012F3 RID: 4851
	public bool ok;

	// Token: 0x040012F4 RID: 4852
	public string softError;

	// Token: 0x040012F5 RID: 4853
	public string reasonFailed;
}
