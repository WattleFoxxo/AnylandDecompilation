using System;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class PitchRollYaw
{
	// Token: 0x06000DDB RID: 3547 RVA: 0x0007C3F1 File Offset: 0x0007A7F1
	public PitchRollYaw(float pitch = 0f, float roll = 0f, float yaw = 0f)
	{
		this.pitch = pitch;
		this.roll = roll;
		this.yaw = yaw;
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x0007C40E File Offset: 0x0007A80E
	public PitchRollYaw(PitchRollYaw pitchRollYaw)
	{
		this.pitch = pitchRollYaw.pitch;
		this.roll = pitchRollYaw.roll;
		this.yaw = pitchRollYaw.yaw;
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0007C43A File Offset: 0x0007A83A
	public void SetEulerAngles(Vector3 vector)
	{
		this.pitch = vector.x;
		this.yaw = vector.y;
		this.roll = vector.z;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x0007C463 File Offset: 0x0007A863
	public Vector3 GetEulerAngles()
	{
		return new Vector3(this.pitch, this.yaw, this.roll);
	}

	// Token: 0x04000F51 RID: 3921
	public float pitch;

	// Token: 0x04000F52 RID: 3922
	public float roll;

	// Token: 0x04000F53 RID: 3923
	public float yaw;
}
