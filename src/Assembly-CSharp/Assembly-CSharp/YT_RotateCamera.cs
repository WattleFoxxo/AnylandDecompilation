using System;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public class YT_RotateCamera : MonoBehaviour
{
	// Token: 0x06001A74 RID: 6772 RVA: 0x000EDFCC File Offset: 0x000EC3CC
	private void Update()
	{
		this.yaw += this.speedH * Input.GetAxis("Mouse X");
		this.pitch -= this.speedV * Input.GetAxis("Mouse Y");
		base.transform.eulerAngles = new Vector3(this.pitch, this.yaw, 0f);
	}

	// Token: 0x04001863 RID: 6243
	public float speedH = 2f;

	// Token: 0x04001864 RID: 6244
	public float speedV = 2f;

	// Token: 0x04001865 RID: 6245
	private float yaw;

	// Token: 0x04001866 RID: 6246
	private float pitch;
}
