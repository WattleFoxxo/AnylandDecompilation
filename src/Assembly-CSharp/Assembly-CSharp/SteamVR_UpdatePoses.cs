using System;
using UnityEngine;
using Valve.VR;

// Token: 0x020002B6 RID: 694
[RequireComponent(typeof(Camera))]
public class SteamVR_UpdatePoses : MonoBehaviour
{
	// Token: 0x060019E2 RID: 6626 RVA: 0x000EB610 File Offset: 0x000E9A10
	private void Awake()
	{
		Camera component = base.GetComponent<Camera>();
		component.stereoTargetEye = StereoTargetEyeMask.None;
		component.clearFlags = CameraClearFlags.Nothing;
		component.useOcclusionCulling = false;
		component.cullingMask = 0;
		component.depth = -9999f;
	}

	// Token: 0x060019E3 RID: 6627 RVA: 0x000EB64C File Offset: 0x000E9A4C
	private void OnPreCull()
	{
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			SteamVR_Render instance = SteamVR_Render.instance;
			compositor.GetLastPoses(instance.poses, instance.gamePoses);
			SteamVR_Utils.Event.Send("new_poses", new object[] { instance.poses });
			SteamVR_Utils.Event.Send("new_poses_applied", new object[0]);
		}
	}
}
