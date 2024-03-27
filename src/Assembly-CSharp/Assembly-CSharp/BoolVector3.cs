using System;

// Token: 0x020001A9 RID: 425
public class BoolVector3
{
	// Token: 0x06000D30 RID: 3376 RVA: 0x000768D6 File Offset: 0x00074CD6
	public BoolVector3(bool _x = false, bool _y = false, bool _z = false)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x000768F3 File Offset: 0x00074CF3
	public bool IsAllDefault()
	{
		return !this.x && !this.y && !this.z;
	}

	// Token: 0x04000ECE RID: 3790
	public bool x;

	// Token: 0x04000ECF RID: 3791
	public bool y;

	// Token: 0x04000ED0 RID: 3792
	public bool z;
}
