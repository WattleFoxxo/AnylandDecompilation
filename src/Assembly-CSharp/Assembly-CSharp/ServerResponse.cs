using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000242 RID: 578
public class ServerResponse
{
	// Token: 0x060015F8 RID: 5624 RVA: 0x000BFCF4 File Offset: 0x000BE0F4
	public ServerResponse(WWW www)
	{
		this.rawBody = www.text;
		try
		{
			this.headers = www.responseHeaders;
		}
		catch (Exception ex)
		{
			Log.Info("Unity www component threw exception when reading headers (caught). Probably response had empty body" + ex, false);
		}
		this.error = www.error;
	}

	// Token: 0x04001345 RID: 4933
	public string rawBody;

	// Token: 0x04001346 RID: 4934
	public Dictionary<string, string> headers;

	// Token: 0x04001347 RID: 4935
	public string error;
}
