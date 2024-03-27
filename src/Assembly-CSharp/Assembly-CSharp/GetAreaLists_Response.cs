using System;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class GetAreaLists_Response : ServerResponse
{
	// Token: 0x060015D1 RID: 5585 RVA: 0x000BFF3A File Offset: 0x000BE33A
	public GetAreaLists_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.areas = JsonUtility.FromJson<AreaListSet>(www.text);
		}
	}

	// Token: 0x040012F8 RID: 4856
	public AreaListSet areas;
}
