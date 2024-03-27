using System;
using UnityEngine;
using Valve.VR;

// Token: 0x020002B4 RID: 692
public class SteamVR_TrackedObject : MonoBehaviour
{
	// Token: 0x060019DD RID: 6621 RVA: 0x000EB420 File Offset: 0x000E9820
	private void OnNewPoses(params object[] args)
	{
		if (this.index == SteamVR_TrackedObject.EIndex.None)
		{
			return;
		}
		int num = (int)this.index;
		this.isValid = false;
		TrackedDevicePose_t[] array = (TrackedDevicePose_t[])args[0];
		if (array.Length <= num)
		{
			return;
		}
		if (!array[num].bDeviceIsConnected)
		{
			return;
		}
		if (!array[num].bPoseIsValid)
		{
			return;
		}
		this.isValid = true;
		SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(array[num].mDeviceToAbsoluteTracking);
		if (this.origin != null)
		{
			rigidTransform = new SteamVR_Utils.RigidTransform(this.origin) * rigidTransform;
			rigidTransform.pos.x = rigidTransform.pos.x * this.origin.localScale.x;
			rigidTransform.pos.y = rigidTransform.pos.y * this.origin.localScale.y;
			rigidTransform.pos.z = rigidTransform.pos.z * this.origin.localScale.z;
			base.transform.position = rigidTransform.pos;
			base.transform.rotation = rigidTransform.rot;
		}
		else
		{
			base.transform.localPosition = rigidTransform.pos;
			base.transform.localRotation = rigidTransform.rot;
		}
		CrossDevice.AdjustControllerTransformIfNeeded(base.transform);
	}

	// Token: 0x060019DE RID: 6622 RVA: 0x000EB588 File Offset: 0x000E9988
	private void OnEnable()
	{
		SteamVR_Render instance = SteamVR_Render.instance;
		if (instance == null)
		{
			base.enabled = false;
			return;
		}
		SteamVR_Utils.Event.Listen("new_poses", new SteamVR_Utils.Event.Handler(this.OnNewPoses));
	}

	// Token: 0x060019DF RID: 6623 RVA: 0x000EB5C5 File Offset: 0x000E99C5
	private void OnDisable()
	{
		SteamVR_Utils.Event.Remove("new_poses", new SteamVR_Utils.Event.Handler(this.OnNewPoses));
		this.isValid = false;
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x000EB5E4 File Offset: 0x000E99E4
	public void SetDeviceIndex(int index)
	{
		if (Enum.IsDefined(typeof(SteamVR_TrackedObject.EIndex), index))
		{
			this.index = (SteamVR_TrackedObject.EIndex)index;
		}
	}

	// Token: 0x04001816 RID: 6166
	public SteamVR_TrackedObject.EIndex index;

	// Token: 0x04001817 RID: 6167
	public Transform origin;

	// Token: 0x04001818 RID: 6168
	public bool isValid;

	// Token: 0x020002B5 RID: 693
	public enum EIndex
	{
		// Token: 0x0400181A RID: 6170
		None = -1,
		// Token: 0x0400181B RID: 6171
		Hmd,
		// Token: 0x0400181C RID: 6172
		Device1,
		// Token: 0x0400181D RID: 6173
		Device2,
		// Token: 0x0400181E RID: 6174
		Device3,
		// Token: 0x0400181F RID: 6175
		Device4,
		// Token: 0x04001820 RID: 6176
		Device5,
		// Token: 0x04001821 RID: 6177
		Device6,
		// Token: 0x04001822 RID: 6178
		Device7,
		// Token: 0x04001823 RID: 6179
		Device8,
		// Token: 0x04001824 RID: 6180
		Device9,
		// Token: 0x04001825 RID: 6181
		Device10,
		// Token: 0x04001826 RID: 6182
		Device11,
		// Token: 0x04001827 RID: 6183
		Device12,
		// Token: 0x04001828 RID: 6184
		Device13,
		// Token: 0x04001829 RID: 6185
		Device14,
		// Token: 0x0400182A RID: 6186
		Device15
	}
}
