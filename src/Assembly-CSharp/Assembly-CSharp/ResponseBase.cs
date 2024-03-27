using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200023C RID: 572
public class ResponseBase
{
	// Token: 0x060015F2 RID: 5618 RVA: 0x000BFC91 File Offset: 0x000BE091
	public void SetFromWWW(WWW www)
	{
		this.rawBody = www.text;
		this.headers = www.responseHeaders;
		this.error = www.error;
		this.httpResponseCode = Misc.getResponseCode(www);
	}

	// Token: 0x0400133A RID: 4922
	public string rawBody;

	// Token: 0x0400133B RID: 4923
	public Dictionary<string, string> headers;

	// Token: 0x0400133C RID: 4924
	public string error;

	// Token: 0x0400133D RID: 4925
	public int httpResponseCode;
}
