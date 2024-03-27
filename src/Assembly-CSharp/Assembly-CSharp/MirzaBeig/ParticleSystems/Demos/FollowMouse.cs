using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems.Demos
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class FollowMouse : MonoBehaviour
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000AF55 File Offset: 0x00009355
		private void Awake()
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000AF57 File Offset: 0x00009357
		private void Start()
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000AF5C File Offset: 0x0000935C
		private void Update()
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = this.distanceFromCamera;
			Vector3 vector = Camera.main.ScreenToWorldPoint(mousePosition);
			Vector3 vector2 = Vector3.Lerp(base.transform.position, vector, Time.deltaTime * this.speed);
			base.transform.position = vector2;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000AFB2 File Offset: 0x000093B2
		private void LateUpdate()
		{
		}

		// Token: 0x04000160 RID: 352
		public float speed = 8f;

		// Token: 0x04000161 RID: 353
		public float distanceFromCamera = 5f;
	}
}
