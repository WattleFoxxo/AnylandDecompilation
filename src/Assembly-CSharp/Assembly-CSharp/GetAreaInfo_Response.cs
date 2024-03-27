using System;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class GetAreaInfo_Response : ServerResponse
{
	// Token: 0x060015D0 RID: 5584 RVA: 0x000BFF15 File Offset: 0x000BE315
	public GetAreaInfo_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.areaInfo = JsonUtility.FromJson<AreaInfo>(www.text);
		}
	}

	// Token: 0x040012F7 RID: 4855
	public AreaInfo areaInfo;
}
