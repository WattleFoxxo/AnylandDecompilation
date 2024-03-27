using System;
using UnityEngine;

// Token: 0x02000164 RID: 356
[Serializable]
public class LocationData
{
	// Token: 0x06000CE8 RID: 3304 RVA: 0x000751A4 File Offset: 0x000735A4
	public LocationData()
	{
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x000751AC File Offset: 0x000735AC
	public LocationData(Transform t)
	{
		this.P = t.position;
		this.R = t.rotation.eulerAngles;
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x000751DF File Offset: 0x000735DF
	public LocationData(Vector3 position, Vector3 rotation)
	{
		this.P = position;
		this.R = rotation;
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000CEB RID: 3307 RVA: 0x000751F5 File Offset: 0x000735F5
	public Vector3 position
	{
		get
		{
			return this.P;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000CEC RID: 3308 RVA: 0x000751FD File Offset: 0x000735FD
	public Vector3 rotation
	{
		get
		{
			return this.R;
		}
	}

	// Token: 0x04000A32 RID: 2610
	public Vector3 P;

	// Token: 0x04000A33 RID: 2611
	public Vector3 R;
}
