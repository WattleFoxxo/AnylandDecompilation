using System;
using UnityEngine;

// Token: 0x02000167 RID: 359
[Serializable]
public class PlacementData
{
	// Token: 0x06000CEF RID: 3311 RVA: 0x00075218 File Offset: 0x00073618
	public PlacementData(string thingId, Vector3 position, Vector3 rotation, float scale, float distanceToShow)
	{
		this.Id = ObjectId.GenerateNewId().ToString();
		this.Tid = thingId;
		this.P = position;
		this.R = rotation;
		this.S = scale;
		this.D = distanceToShow;
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0007526C File Offset: 0x0007366C
	public PlacementData(string placementId, string thingId, Vector3 position, Vector3 rotation, float scale, float distanceToShow)
	{
		if (string.IsNullOrEmpty(placementId))
		{
			throw new Exception("Attempt to instantiate PlacementData with null or empty placementId");
		}
		if (!Misc.IsValidObjectIdString(placementId))
		{
			throw new Exception("Attempt to instantiate PlacementData with invalid placementId : " + placementId);
		}
		if (string.IsNullOrEmpty(thingId))
		{
			throw new Exception("Attempt to instantiate PlacementData with null or empty thingId");
		}
		if (!Misc.IsValidObjectIdString(thingId))
		{
			throw new Exception("Attempt to instantiate PlacementData with invalid thingId : " + thingId);
		}
		this.Id = placementId;
		this.Tid = thingId;
		this.P = position;
		this.R = rotation;
		this.S = scale;
		this.D = distanceToShow;
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00075310 File Offset: 0x00073710
	public string id
	{
		get
		{
			return this.Id;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00075318 File Offset: 0x00073718
	public string thingId
	{
		get
		{
			return this.Tid;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x00075320 File Offset: 0x00073720
	public Vector3 position
	{
		get
		{
			return this.P;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00075328 File Offset: 0x00073728
	public Vector3 rotation
	{
		get
		{
			return this.R;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00075330 File Offset: 0x00073730
	public float scale
	{
		get
		{
			return this.S;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00075338 File Offset: 0x00073738
	public int[] attributes
	{
		get
		{
			return this.A;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x00075340 File Offset: 0x00073740
	public float distanceToShow
	{
		get
		{
			return this.D;
		}
	}

	// Token: 0x04000A42 RID: 2626
	public string Id;

	// Token: 0x04000A43 RID: 2627
	public string Tid;

	// Token: 0x04000A44 RID: 2628
	public Vector3 P;

	// Token: 0x04000A45 RID: 2629
	public Vector3 R;

	// Token: 0x04000A46 RID: 2630
	public float S;

	// Token: 0x04000A47 RID: 2631
	public int[] A;

	// Token: 0x04000A48 RID: 2632
	public float D;
}
