using System;
using UnityEngine;

// Token: 0x02000161 RID: 353
[Serializable]
public class InventoryItemData
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x0007514F File Offset: 0x0007354F
	public InventoryItemData(string thingId, Vector3 position, Vector3 rotation, float scale)
	{
		this.Tid = thingId;
		this.P = position;
		this.R = rotation;
		this.S = scale;
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x00075174 File Offset: 0x00073574
	public string thingId
	{
		get
		{
			return this.Tid;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0007517C File Offset: 0x0007357C
	public Vector3 position
	{
		get
		{
			return this.P;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00075184 File Offset: 0x00073584
	public Vector3 rotation
	{
		get
		{
			return this.R;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0007518C File Offset: 0x0007358C
	public float scale
	{
		get
		{
			return this.S;
		}
	}

	// Token: 0x04000A16 RID: 2582
	public string Tid;

	// Token: 0x04000A17 RID: 2583
	public Vector3 P;

	// Token: 0x04000A18 RID: 2584
	public Vector3 R;

	// Token: 0x04000A19 RID: 2585
	public float S;
}
