using System;
using UnityEngine;

// Token: 0x0200023E RID: 574
public class SearchAreas_Response : ServerResponse
{
	// Token: 0x060015F4 RID: 5620 RVA: 0x000C06B9 File Offset: 0x000BEAB9
	public SearchAreas_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.areas = JsonUtility.FromJson<AreaList>(www.text);
		}
	}

	// Token: 0x0400133F RID: 4927
	public AreaList areas;
}
