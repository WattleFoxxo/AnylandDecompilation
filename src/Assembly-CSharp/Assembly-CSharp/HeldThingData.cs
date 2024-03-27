using System;
using UnityEngine;

// Token: 0x0200015E RID: 350
[Serializable]
public class HeldThingData
{
	// Token: 0x06000CD1 RID: 3281 RVA: 0x00074E1D File Offset: 0x0007321D
	public HeldThingData()
	{
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x00074E25 File Offset: 0x00073225
	public HeldThingData(string thingId, Vector3 position, Quaternion rotation)
	{
		this.Tid = thingId;
		this.P = position;
		this.R = rotation;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00074E44 File Offset: 0x00073244
	public HeldThingData(Thing thing)
	{
		if (thing != null)
		{
			this.Tid = thing.thingId;
			this.P = thing.transform.localPosition;
			this.R = thing.transform.localRotation;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x00074E91 File Offset: 0x00073291
	public string thingId
	{
		get
		{
			return this.Tid;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00074E99 File Offset: 0x00073299
	public Vector3 position
	{
		get
		{
			return this.P;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00074EA1 File Offset: 0x000732A1
	public Quaternion rotation
	{
		get
		{
			return this.R;
		}
	}

	// Token: 0x04000A0D RID: 2573
	public readonly string Tid;

	// Token: 0x04000A0E RID: 2574
	public readonly Vector3 P;

	// Token: 0x04000A0F RID: 2575
	public readonly Quaternion R;
}
