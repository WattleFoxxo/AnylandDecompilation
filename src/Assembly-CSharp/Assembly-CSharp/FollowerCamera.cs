using System;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class FollowerCamera : MonoBehaviour
{
	// Token: 0x06000708 RID: 1800 RVA: 0x00020BBC File Offset: 0x0001EFBC
	public void Init()
	{
		this.camera = base.gameObject.GetComponentInChildren<Camera>();
		this.camera.stereoTargetEye = StereoTargetEyeMask.None;
		this.camera.aspect = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
		this.camera.nearClipPlane = 0.001f;
		this.camera.farClipPlane = 10000f;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00020C30 File Offset: 0x0001F030
	private void Update()
	{
		Vector3 position = this.target.position;
		Vector3 eulerAngles = this.target.eulerAngles;
		if (this.cameraPosition == FollowerCameraPosition.BirdsEye)
		{
			eulerAngles.x = 90f;
			eulerAngles.z = 0f;
			position.y += 7f;
		}
		else if (this.stabilizeToHorizon)
		{
			eulerAngles.z = 0f;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, position, this.lerpFraction);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(eulerAngles), this.lerpFraction);
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00020CF4 File Offset: 0x0001F0F4
	public void UpdateByProperties()
	{
		if (this.cameraPosition == FollowerCameraPosition.AtLeftHand)
		{
			this.target = Managers.treeManager.GetTransform("/OurPersonRig/HandCoreLeft");
		}
		else if (this.cameraPosition == FollowerCameraPosition.AtRightHand)
		{
			this.target = Managers.treeManager.GetTransform("/OurPersonRig/HandCoreRight");
		}
		else if (this.cameraPosition == FollowerCameraPosition.BirdsEye)
		{
			this.target = Managers.treeManager.GetTransform("/OurPersonRig/HeadCore");
		}
		else
		{
			this.target = Managers.treeManager.GetTransform("/OurPersonRig/FollowerCameraTarget");
		}
		this.camera.fieldOfView = ((!this.wideScope) ? 60f : 120f);
		if (this.weAreInvisible || this.cameraPosition == FollowerCameraPosition.InHeadDesktopOptimized)
		{
			this.camera.cullingMask = -2305;
			Managers.personManager.SetOurPersonIsVisibleToDesktopCamera(!this.weAreInvisible, this.cameraPosition == FollowerCameraPosition.InHeadDesktopOptimized, this.forceHandsVisible);
		}
		else
		{
			this.camera.cullingMask = ((this.cameraPosition != FollowerCameraPosition.InHeadVr) ? (-1) : (-257));
			Managers.personManager.SetOurPersonIsVisibleToDesktopCamera(true, false, false);
		}
		switch (this.cameraPosition)
		{
		case FollowerCameraPosition.InHeadVr:
			this.target.localPosition = Vector3.zero;
			this.target.localEulerAngles = Vector3.zero;
			break;
		case FollowerCameraPosition.InHeadDesktopOptimized:
			this.target.localPosition = new Vector3(0f, 0f, -0.1f);
			this.target.localEulerAngles = Vector3.zero;
			break;
		case FollowerCameraPosition.BehindUp:
			this.target.localPosition = new Vector3(0f, 1f, -1f);
			this.target.localEulerAngles = new Vector3(20f, 0f, 0f);
			break;
		case FollowerCameraPosition.FurtherBehindUp:
			this.target.localPosition = new Vector3(0f, 2f, -2f);
			this.target.localEulerAngles = new Vector3(30f, 0f, 0f);
			break;
		case FollowerCameraPosition.BirdsEye:
			this.target.localPosition = new Vector3(0f, 7f, 0f);
			this.target.localEulerAngles = new Vector3(90f, 0f, 0f);
			break;
		case FollowerCameraPosition.LooksAtMe:
			this.target.localPosition = new Vector3(0f, -0.4f, 1.5f);
			this.target.localEulerAngles = new Vector3(0f, 180f, 0f);
			break;
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00020FB8 File Offset: 0x0001F3B8
	public void SaveProperties()
	{
		PlayerPrefs.SetString("desktopStream_cameraPositionString", this.cameraPosition.ToString());
		PlayerPrefs.SetFloat("desktopStream_lerpFraction", this.lerpFraction);
		PlayerPrefs.SetInt("desktopStream_stabilizeToHorizon", (!this.stabilizeToHorizon) ? 0 : 1);
		PlayerPrefs.SetInt("desktopStream_weAreInvisible", (!this.weAreInvisible) ? 0 : 1);
		PlayerPrefs.SetInt("desktopStream_forceHandsVisible", (!this.forceHandsVisible) ? 0 : 1);
		PlayerPrefs.SetInt("desktopStream_wideScope", (!this.wideScope) ? 0 : 1);
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00021060 File Offset: 0x0001F460
	public void ResetProperties()
	{
		this.lerpFraction = 1f;
		this.cameraPosition = FollowerCameraPosition.InHeadVr;
		this.stabilizeToHorizon = false;
		this.weAreInvisible = false;
		this.forceHandsVisible = false;
		this.wideScope = false;
		this.UpdateByProperties();
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00021098 File Offset: 0x0001F498
	public void LoadProperties()
	{
		try
		{
			string @string = PlayerPrefs.GetString("desktopStream_cameraPositionString", "InHeadVr");
			this.cameraPosition = (FollowerCameraPosition)Enum.Parse(typeof(FollowerCameraPosition), @string);
		}
		catch (Exception ex)
		{
			this.cameraPosition = FollowerCameraPosition.InHeadVr;
		}
		this.lerpFraction = PlayerPrefs.GetFloat("desktopStream_lerpFraction", 1f);
		this.stabilizeToHorizon = PlayerPrefs.GetInt("desktopStream_stabilizeToHorizon", 0) == 1;
		this.weAreInvisible = PlayerPrefs.GetInt("desktopStream_weAreInvisible", 0) == 1;
		this.forceHandsVisible = PlayerPrefs.GetInt("desktopStream_forceHandsVisible", 0) == 1;
		this.wideScope = PlayerPrefs.GetInt("desktopStream_wideScope", 0) == 1;
	}

	// Token: 0x0400051D RID: 1309
	private Transform target;

	// Token: 0x0400051E RID: 1310
	private Camera camera;

	// Token: 0x0400051F RID: 1311
	public FollowerCameraPosition cameraPosition;

	// Token: 0x04000520 RID: 1312
	public float lerpFraction = 1f;

	// Token: 0x04000521 RID: 1313
	public bool stabilizeToHorizon;

	// Token: 0x04000522 RID: 1314
	public bool weAreInvisible;

	// Token: 0x04000523 RID: 1315
	public bool forceHandsVisible;

	// Token: 0x04000524 RID: 1316
	public bool wideScope;

	// Token: 0x04000525 RID: 1317
	private const string prefPrefix = "desktopStream_";
}
