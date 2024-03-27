using System;
using UnityEngine;

// Token: 0x02000154 RID: 340
[Serializable]
public class EnvironmentChangerData
{
	// Token: 0x06000CC6 RID: 3270 RVA: 0x00074DA8 File Offset: 0x000731A8
	public EnvironmentChangerData()
	{
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x00074DB0 File Offset: 0x000731B0
	public EnvironmentChangerData(string name, Vector3 rotation, float scale, Color color)
	{
		this.Name = name;
		this.Rotation = rotation;
		this.Scale = scale;
		this.Color = color;
	}

	// Token: 0x040009C1 RID: 2497
	public string Name;

	// Token: 0x040009C2 RID: 2498
	public Vector3 Rotation;

	// Token: 0x040009C3 RID: 2499
	public float Scale;

	// Token: 0x040009C4 RID: 2500
	public Color Color;
}
