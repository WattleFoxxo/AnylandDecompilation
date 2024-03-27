using System;
using UnityEngine;

// Token: 0x02000151 RID: 337
[Serializable]
public class AttachmentData
{
	// Token: 0x06000CBA RID: 3258 RVA: 0x00074A8E File Offset: 0x00072E8E
	public AttachmentData()
	{
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00074A96 File Offset: 0x00072E96
	public AttachmentData(string thingId, Vector3 position, Vector3 rotation)
	{
		this.Tid = thingId;
		this.P = position;
		this.R = rotation;
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00074AB3 File Offset: 0x00072EB3
	public string thingId
	{
		get
		{
			return this.Tid;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00074ABB File Offset: 0x00072EBB
	public Vector3 position
	{
		get
		{
			return this.P;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00074AC3 File Offset: 0x00072EC3
	public Vector3 rotation
	{
		get
		{
			return this.R;
		}
	}

	// Token: 0x040009BB RID: 2491
	public string Tid;

	// Token: 0x040009BC RID: 2492
	public Vector3 P;

	// Token: 0x040009BD RID: 2493
	public Vector3 R;
}
