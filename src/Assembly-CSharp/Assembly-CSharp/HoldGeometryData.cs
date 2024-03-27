using System;
using UnityEngine;

// Token: 0x02000160 RID: 352
[Serializable]
public class HoldGeometryData
{
	// Token: 0x06000CDD RID: 3293 RVA: 0x00075109 File Offset: 0x00073509
	public HoldGeometryData(Vector3 position, Quaternion rotation)
	{
		this.P = position;
		this.R = rotation;
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0007511F File Offset: 0x0007351F
	public HoldGeometryData(HeldThingData h)
	{
		this.P = h.P;
		this.R = h.R;
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0007513F File Offset: 0x0007353F
	public Vector3 position
	{
		get
		{
			return this.P;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00075147 File Offset: 0x00073547
	public Quaternion rotation
	{
		get
		{
			return this.R;
		}
	}

	// Token: 0x04000A14 RID: 2580
	public Vector3 P;

	// Token: 0x04000A15 RID: 2581
	public Quaternion R;
}
