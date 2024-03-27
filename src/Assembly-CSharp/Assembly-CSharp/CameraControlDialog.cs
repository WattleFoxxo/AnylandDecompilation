using System;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class CameraControlDialog : Dialog
{
	// Token: 0x06000B3E RID: 2878 RVA: 0x0005C820 File Offset: 0x0005AC20
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		if (this.hand.lastContextInfoHit != null)
		{
			this.cameraThingPart = this.GetCameraThingPart(this.hand.lastContextInfoHit);
			this.hand.lastContextInfoHit = null;
		}
		if (this.cameraThingPart == null)
		{
			this.cameraThingPart = Managers.cameraManager.GetNearestCameraThingPart();
		}
		this.AddButtons();
		base.AddCloseButton();
		this.AddCalibratorText(null);
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x0005C8B0 File Offset: 0x0005ACB0
	private void AddCalibratorText(string text = null)
	{
		if (this.calibratorText != null)
		{
			global::UnityEngine.Object.Destroy(this.calibratorText.gameObject);
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "You can Ctrl+V a Liv config path when streaming to desktop to align mixed reality";
		}
		this.calibratorText = base.AddLabel(text, 0, 220, 0.75f, true, TextColor.Gray, false, TextAlignment.Center, 42, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x0005C918 File Offset: 0x0005AD18
	public void SetCameraThingPartByButton(GameObject thingPartButton)
	{
		GameObject gameObject = this.GetCameraThingPart(thingPartButton.transform.parent.gameObject);
		if (gameObject != null)
		{
			this.cameraThingPart = gameObject;
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x0005C950 File Offset: 0x0005AD50
	private GameObject GetCameraThingPart(GameObject thisGameObject)
	{
		GameObject gameObject = null;
		Component[] componentsInChildren = thisGameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.isCamera)
			{
				gameObject = thingPart.gameObject;
				break;
			}
		}
		return gameObject;
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x0005C9A4 File Offset: 0x0005ADA4
	private void AddButtons()
	{
		if (this.cameraThingPart != null)
		{
			if (this.streamToDesktopButton != null)
			{
				global::UnityEngine.Object.Destroy(this.streamToDesktopButton);
			}
			if (this.streamToVideoScreenButton != null)
			{
				global::UnityEngine.Object.Destroy(this.streamToVideoScreenButton);
			}
			if (this.weAreInvisibleButton != null)
			{
				global::UnityEngine.Object.Destroy(this.weAreInvisibleButton);
			}
			if (this.backsideWrapper != null)
			{
				global::UnityEngine.Object.Destroy(this.backsideWrapper);
			}
			bool isStreamingToVideoScreen = Managers.cameraManager.isStreamingToVideoScreen;
			bool isStreamingToDesktop = Managers.cameraManager.isStreamingToDesktop;
			this.streamToDesktopButton = base.AddCheckbox("streamToDesktop", null, "Stream to Desktop", 0, -150, isStreamingToDesktop, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
			this.streamToVideoScreenButton = base.AddCheckbox("streamToVideoScreen", null, "Stream to Video Screen", 0, 50, isStreamingToVideoScreen, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
			this.backsideWrapper = base.GetUiWrapper();
			base.SetUiWrapper(this.backsideWrapper);
			this.weAreInvisibleButton = base.AddCheckbox("weAreInvisible", null, "I'm invisible to camera", 0, 0, CameraManager.weAreInvisibleToDesktopCameraStream, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
			base.RotateBacksideWrapper();
			base.SetUiWrapper(base.gameObject);
		}
		else
		{
			base.AddLabel("Please place a camera nearby", -350, -50, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0005CB19 File Offset: 0x0005AF19
	private new void Update()
	{
		base.Update();
		this.CheckForCameraCalibratorConfigFilePasting();
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x0005CB28 File Offset: 0x0005AF28
	private void CheckForCameraCalibratorConfigFilePasting()
	{
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.V))
		{
			bool flag = Managers.cameraManager.isStreamingToDesktop && Managers.cameraManager.currentCameraComponent != null;
			if (flag)
			{
				GameObject gameObject = Managers.cameraManager.currentCameraComponent.gameObject;
				string systemCopyBuffer = GUIUtility.systemCopyBuffer;
				CameraCalibrator cameraCalibrator = gameObject.GetComponentInChildren<CameraCalibrator>();
				if (string.IsNullOrEmpty(systemCopyBuffer))
				{
					global::UnityEngine.Object.Destroy(cameraCalibrator);
					Managers.soundManager.Play("pickUp", this.transform, 0.3f, false, false);
					this.AddCalibratorText(null);
				}
				else
				{
					if (cameraCalibrator == null)
					{
						cameraCalibrator = gameObject.AddComponent<CameraCalibrator>();
					}
					cameraCalibrator.configPath = systemCopyBuffer;
					Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
					this.AddCalibratorText("Pasted calibrator path " + systemCopyBuffer + " (paste empty to remove again)");
				}
			}
			else
			{
				Managers.soundManager.Play("no", this.transform, 0.1f, false, false);
			}
		}
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x0005CC38 File Offset: 0x0005B038
	private void ApplyWeAreInvisibleToDesktopCamera(bool weAreInvisible)
	{
		if (this.cameraThingPart != null)
		{
			Camera componentInChildren = this.cameraThingPart.GetComponentInChildren<Camera>();
			CameraManager.weAreInvisibleToDesktopCameraStream = weAreInvisible;
			if (weAreInvisible)
			{
				Managers.personManager.SetOurPersonIsVisibleToDesktopCamera(false, true, false);
				if (componentInChildren != null)
				{
					componentInChildren.cullingMask = -18689;
				}
			}
			else
			{
				Managers.personManager.SetOurPersonIsVisibleToDesktopCamera(true, false, false);
				if (componentInChildren != null)
				{
					componentInChildren.cullingMask = -1;
				}
			}
		}
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x0005CCB8 File Offset: 0x0005B0B8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "streamToVideoScreen"))
			{
				if (!(contextName == "streamToDesktop"))
				{
					if (!(contextName == "weAreInvisible"))
					{
						if (contextName == "close")
						{
							base.CloseDialog();
						}
					}
					else
					{
						this.ApplyWeAreInvisibleToDesktopCamera(state);
					}
				}
				else if (CrossDevice.desktopMode && state)
				{
					Managers.cameraManager.StartStreamToDesktop(this.cameraThingPart);
					Managers.desktopManager.ShowBottomHelpAgain();
					Managers.desktopManager.SetCursorUseActive(true);
					base.CloseDialog();
				}
				else
				{
					if (state)
					{
						Managers.cameraManager.StartStreamToDesktop(this.cameraThingPart);
					}
					else
					{
						Managers.cameraManager.StopStreamToDesktop();
					}
					this.AddButtons();
				}
			}
			else
			{
				if (state)
				{
					Managers.cameraManager.StartStreamToVideoScreen(this.cameraThingPart, null);
				}
				else
				{
					Managers.cameraManager.StopStreamToVideoScreen();
				}
				this.AddButtons();
			}
		}
	}

	// Token: 0x04000886 RID: 2182
	private GameObject cameraThingPart;

	// Token: 0x04000887 RID: 2183
	private GameObject streamToDesktopButton;

	// Token: 0x04000888 RID: 2184
	private GameObject streamToVideoScreenButton;

	// Token: 0x04000889 RID: 2185
	private GameObject weAreInvisibleButton;

	// Token: 0x0400088A RID: 2186
	private TextMesh calibratorText;
}
