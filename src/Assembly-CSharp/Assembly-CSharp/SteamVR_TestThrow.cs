using System;
using UnityEngine;

// Token: 0x0200028A RID: 650
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SteamVR_TestThrow : MonoBehaviour
{
	// Token: 0x0600187B RID: 6267 RVA: 0x000E112F File Offset: 0x000DF52F
	private void Awake()
	{
		this.trackedObj = base.GetComponent<SteamVR_TrackedObject>();
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x000E1140 File Offset: 0x000DF540
	private void FixedUpdate()
	{
		SteamVR_Controller.Device device = SteamVR_Controller.Input((int)this.trackedObj.index);
		if (this.joint == null && device.GetTouchDown(8589934592UL))
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			gameObject.transform.position = this.attachPoint.transform.position;
			this.joint = gameObject.AddComponent<FixedJoint>();
			this.joint.connectedBody = this.attachPoint;
		}
		else if (this.joint != null && device.GetTouchUp(8589934592UL))
		{
			GameObject gameObject2 = this.joint.gameObject;
			Rigidbody component = gameObject2.GetComponent<Rigidbody>();
			global::UnityEngine.Object.DestroyImmediate(this.joint);
			this.joint = null;
			global::UnityEngine.Object.Destroy(gameObject2, 15f);
			Transform transform = ((!this.trackedObj.origin) ? this.trackedObj.transform.parent : this.trackedObj.origin);
			if (transform != null)
			{
				component.velocity = transform.TransformVector(device.velocity);
				component.angularVelocity = transform.TransformVector(device.angularVelocity);
			}
			else
			{
				component.velocity = device.velocity;
				component.angularVelocity = device.angularVelocity;
			}
			component.maxAngularVelocity = component.angularVelocity.magnitude;
		}
	}

	// Token: 0x040016E6 RID: 5862
	public GameObject prefab;

	// Token: 0x040016E7 RID: 5863
	public Rigidbody attachPoint;

	// Token: 0x040016E8 RID: 5864
	private SteamVR_TrackedObject trackedObj;

	// Token: 0x040016E9 RID: 5865
	private FixedJoint joint;
}
