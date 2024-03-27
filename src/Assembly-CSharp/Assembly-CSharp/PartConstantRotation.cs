using System;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class PartConstantRotation : MonoBehaviour
{
	// Token: 0x0600167A RID: 5754 RVA: 0x000CA558 File Offset: 0x000C8958
	private void Update()
	{
		base.transform.Rotate(this.speed * Time.deltaTime);
	}

	// Token: 0x04001431 RID: 5169
	public Vector3 speed = Vector3.zero;
}
