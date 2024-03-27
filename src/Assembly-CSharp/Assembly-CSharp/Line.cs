using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class Line
{
	// Token: 0x06000D83 RID: 3459 RVA: 0x000787DC File Offset: 0x00076BDC
	public Line(Vector3 start, Vector3 end)
	{
		this.start = new Vector3(start.x, start.y, start.z);
		this.end = new Vector3(end.x, end.y, end.z);
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00078845 File Offset: 0x00076C45
	public float length
	{
		get
		{
			return Vector3.Distance(this.start, this.end);
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00078858 File Offset: 0x00076C58
	public Vector3 direction
	{
		get
		{
			return this.end - this.start;
		}
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0007886C File Offset: 0x00076C6C
	public void Draw()
	{
		GL.Vertex3(this.start.x, this.start.y, this.start.z);
		GL.Vertex3(this.end.x, this.end.y, this.end.z);
	}

	// Token: 0x04000EFD RID: 3837
	public Vector3 start = Vector3.zero;

	// Token: 0x04000EFE RID: 3838
	public Vector3 end = Vector3.zero;
}
