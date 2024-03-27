using System;
using UnityEngine;

// Token: 0x02000229 RID: 553
public class GetPlacementInfo_Response : ServerResponse
{
	// Token: 0x060015DE RID: 5598 RVA: 0x000C00BB File Offset: 0x000BE4BB
	public GetPlacementInfo_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.placementInfo = JsonUtility.FromJson<PlacementInfo>(www.text);
		}
	}

	// Token: 0x04001315 RID: 4885
	public PlacementInfo placementInfo;
}
