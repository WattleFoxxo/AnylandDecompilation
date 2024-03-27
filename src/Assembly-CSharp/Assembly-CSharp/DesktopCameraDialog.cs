using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class DesktopCameraDialog : Dialog
{
	// Token: 0x06000B48 RID: 2888 RVA: 0x0005CEA4 File Offset: 0x0005B2A4
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Desktop Stream", -370, -460, TextColor.Default, TextAlignment.Left, false);
		if (CrossDevice.desktopMode)
		{
			this.AddDesktopSettings();
		}
		else
		{
			this.InitCameraPositionOrder();
			this.followerCameraObject = Managers.treeManager.GetObject("/Universe/FollowerCamera");
			this.followerCamera = this.followerCameraObject.GetComponent<FollowerCamera>();
			this.followerCamera.Init();
			this.followerCamera.LoadProperties();
			this.followerCamera.UpdateByProperties();
			bool activeSelf = this.followerCameraObject.activeSelf;
			this.UpdateFollowerCameraActive();
			if (activeSelf != this.followerCameraObject.activeSelf)
			{
				this.ShowSettingsLoaded();
			}
			this.cameraLerpFractionIndex = Array.IndexOf<float>(this.cameraLerpFractions, this.followerCamera.lerpFraction);
			base.AddSeparator(0, -330, false);
			this.UpdateCameraPositionLabel();
			this.UpdateCameraLerpFractionLabel();
			base.AddSeparator(0, -85, false);
			this.UpdateCheckboxes();
			base.AddSeparator(0, 300, false);
		}
		this.AddBottomButtons();
		base.ApplyAppropriateLayerForDesktopStream();
		if (!CrossDevice.desktopMode)
		{
			this.UpdateResetAndOptimizeButton();
		}
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x0005CFE8 File Offset: 0x0005B3E8
	private void AddBottomButtons()
	{
		base.AddButton("twitchSettings", null, null, "ButtonSmall", -400, 400, "twitch", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		if (!CrossDevice.desktopMode)
		{
			base.AddButton("quality", null, "Quality", "ButtonSmallNoIcon", 400, 400, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x0005D084 File Offset: 0x0005B484
	private void AddDesktopSettings()
	{
		int num = -50;
		this.checkboxMovementSmoothingMedium = base.AddCheckbox("movementSmoothingMedium", null, "Smooth movement", 0, num, Managers.desktopManager.movementSmoothing == MovementSmoothing.Medium, 1f, "Checkbox", TextColor.Default, "Default", ExtraIcon.None);
		base.AddCheckboxHelpButton("movementSmoothingMedium_help", num);
		num += 115;
		this.checkboxMovementSmoothingStrong = base.AddCheckbox("movementSmoothingStrong", null, "Very Smooth movement", 0, num, Managers.desktopManager.movementSmoothing == MovementSmoothing.Strong, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("movementSmoothingStrong_help", num);
		num += 115;
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0005D124 File Offset: 0x0005B524
	private void ShowSettingsLoaded()
	{
		string text = "✔ Loaded";
		float num = 0.9f;
		if (this.IsRecordingOptimized())
		{
			text += " Recording-Optimized";
			num -= 0.05f;
		}
		this.settingsAppliedLabel = base.AddLabel(text, 0, 330, num, false, TextColor.Green, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
		base.Invoke("RemoveSettingsAppliedLabel", 4f);
		this.UpdateResetAndOptimizeButton();
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0005D1B0 File Offset: 0x0005B5B0
	private bool IsRecordingOptimized()
	{
		return this.followerCamera.lerpFraction == this.cameraLerpFractions[1] && this.followerCamera.cameraPosition == FollowerCameraPosition.InHeadDesktopOptimized && this.followerCamera.stabilizeToHorizon && !this.followerCamera.weAreInvisible && !this.followerCamera.forceHandsVisible && !this.followerCamera.wideScope;
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0005D227 File Offset: 0x0005B627
	private void RemoveSettingsAppliedLabel()
	{
		if (this != null && this.settingsAppliedLabel != null)
		{
			global::UnityEngine.Object.Destroy(this.settingsAppliedLabel.gameObject);
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0005D258 File Offset: 0x0005B658
	private void InitCameraPositionOrder()
	{
		int num = 0;
		IEnumerator enumerator = Enum.GetValues(typeof(FollowerCameraPosition)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				FollowerCameraPosition followerCameraPosition = (FollowerCameraPosition)obj;
				this.cameraPositionOrder[num++] = followerCameraPosition;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x0005D2D0 File Offset: 0x0005B6D0
	private void UpdateCameraPositionLabel()
	{
		if (this.cameraPositionLabel == null)
		{
			this.cameraPositionLabel = base.AddLabel(string.Empty, -300, -287, 0.9f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			base.AddDefaultPagingButtons(400, -265, "CameraMode", false, 0, 0.85f, false);
		}
		this.cameraPositionLabel.text = this.cameraPositionLabels[this.followerCamera.cameraPosition].ToUpper();
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0005D360 File Offset: 0x0005B760
	private void UpdateCameraLerpFractionLabel()
	{
		if (this.cameraLerpFractionLabel == null)
		{
			this.cameraLerpFractionLabel = base.AddLabel(string.Empty, -300, -180, 0.9f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			base.AddDefaultPagingButtons(400, -158, "LerpFraction", false, 0, 0.85f, false);
		}
		this.cameraLerpFractionLabel.text = this.cameraLerpFractionLabels[this.cameraLerpFractionIndex].ToUpper();
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0005D3E4 File Offset: 0x0005B7E4
	private void UpdateCheckboxes()
	{
		for (int i = 0; i < this.checkboxes.Length; i++)
		{
			if (this.checkboxes[i] != null)
			{
				global::UnityEngine.Object.Destroy(this.checkboxes[i]);
			}
		}
		int num = 0;
		int num2 = 0;
		this.checkboxes[num++] = base.AddCheckbox("stabilizeToHorizon", null, "Stabilize to horizon", 0, -10 + num2 * 115, this.followerCamera.stabilizeToHorizon, 0.9f, "Checkbox", TextColor.Default, null, ExtraIcon.StabilizeToHorizon);
		num2++;
		this.checkboxes[num++] = base.AddCheckbox("weAreInvisible", null, "I'm invisible", -225, -10 + num2 * 115, this.followerCamera.weAreInvisible, 0.9f, "CheckboxCompact", TextColor.Default, null, ExtraIcon.WeAreInvisible);
		this.checkboxes[num++] = base.AddCheckbox("forceHandsVisible", null, "... except hands", 225, -10 + num2 * 115, this.followerCamera.forceHandsVisible, 0.9f, "CheckboxCompact", TextColor.Gray, null, ExtraIcon.None);
		num2++;
		this.checkboxes[num++] = base.AddCheckbox("wideScope", null, "Fish eye wide scope", 0, -10 + num2 * 115, this.followerCamera.wideScope, 0.9f, "Checkbox", TextColor.Default, null, ExtraIcon.FishEyeCamera);
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0005D535 File Offset: 0x0005B935
	private void OnDestroy()
	{
		if (this.followerCamera != null)
		{
			this.followerCamera.SaveProperties();
			this.UpdateFollowerCameraActive();
		}
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x0005D559 File Offset: 0x0005B959
	private void UpdateFollowerCameraActive()
	{
		if (this.followerCameraObject != null)
		{
			this.followerCameraObject.SetActive(!this.AllPropertiesAreAtDefault());
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x0005D580 File Offset: 0x0005B980
	private bool AllPropertiesAreAtDefault()
	{
		return this.followerCamera.cameraPosition == FollowerCameraPosition.InHeadVr && this.followerCamera.lerpFraction == 1f && !this.followerCamera.stabilizeToHorizon && !this.followerCamera.weAreInvisible && !this.followerCamera.forceHandsVisible && !this.followerCamera.wideScope;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x0005D5F4 File Offset: 0x0005B9F4
	private void UpdateResetAndOptimizeButton()
	{
		if (this.resetButton == null)
		{
			this.resetButton = base.AddButton("reset", null, "reset", "ButtonCompactNoIcon", 0, 427, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Center, false, false);
		}
		if (this.optimizeForRecordingButton == null)
		{
			this.optimizeForRecordingButton = base.AddButton("optimizeForRecording", null, "Optimize for recording", "ButtonCompactNoIcon", 0, 427, null, false, 1f, TextColor.Green, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Center, false, false);
		}
		bool flag = this.AllPropertiesAreAtDefault();
		this.resetButton.SetActive(!flag);
		this.optimizeForRecordingButton.SetActive(flag);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x0005D6CC File Offset: 0x0005BACC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "previousCameraMode":
		case "nextCameraMode":
		{
			int num2 = (int)this.followerCamera.cameraPosition;
			if (contextName == "previousCameraMode")
			{
				if (--num2 < 0)
				{
					num2 = this.cameraPositionOrder.Length - 1;
				}
			}
			else if (++num2 > this.cameraPositionOrder.Length - 1)
			{
				num2 = 0;
			}
			this.followerCamera.cameraPosition = this.cameraPositionOrder[num2];
			this.UpdateCameraPositionLabel();
			this.followerCamera.UpdateByProperties();
			break;
		}
		case "previousLerpFraction":
		{
			int num = --this.cameraLerpFractionIndex;
			if (num < 0)
			{
				this.cameraLerpFractionIndex = this.cameraLerpFractions.Length - 1;
			}
			this.UpdateCameraLerpFractionLabel();
			this.followerCamera.lerpFraction = this.cameraLerpFractions[this.cameraLerpFractionIndex];
			break;
		}
		case "nextLerpFraction":
		{
			int num = ++this.cameraLerpFractionIndex;
			if (num > this.cameraLerpFractions.Length - 1)
			{
				this.cameraLerpFractionIndex = 0;
			}
			this.UpdateCameraLerpFractionLabel();
			this.followerCamera.lerpFraction = this.cameraLerpFractions[this.cameraLerpFractionIndex];
			break;
		}
		case "twitchSettings":
			Our.dialogToGoBackTo = DialogType.DesktopCamera;
			base.SwitchTo(DialogType.TwitchSettings, string.Empty);
			break;
		case "stabilizeToHorizon":
			this.followerCamera.stabilizeToHorizon = state;
			this.followerCamera.UpdateByProperties();
			break;
		case "weAreInvisible":
			this.followerCamera.weAreInvisible = state;
			this.followerCamera.UpdateByProperties();
			break;
		case "forceHandsVisible":
			this.followerCamera.forceHandsVisible = state;
			this.followerCamera.UpdateByProperties();
			break;
		case "wideScope":
			this.followerCamera.wideScope = state;
			this.followerCamera.UpdateByProperties();
			break;
		case "reset":
			this.followerCamera.ResetProperties();
			this.cameraLerpFractionIndex = Array.IndexOf<float>(this.cameraLerpFractions, this.followerCamera.lerpFraction);
			this.UpdateCameraLerpFractionLabel();
			this.UpdateCameraPositionLabel();
			this.UpdateCheckboxes();
			this.UpdateResetAndOptimizeButton();
			Managers.soundManager.Play("pickUp", this.transform, 0.5f, false, false);
			break;
		case "optimizeForRecording":
			this.followerCamera.lerpFraction = this.cameraLerpFractions[1];
			this.followerCamera.cameraPosition = FollowerCameraPosition.InHeadDesktopOptimized;
			this.followerCamera.stabilizeToHorizon = true;
			this.followerCamera.weAreInvisible = false;
			this.followerCamera.forceHandsVisible = false;
			this.followerCamera.wideScope = false;
			this.followerCamera.UpdateByProperties();
			this.cameraLerpFractionIndex = Array.IndexOf<float>(this.cameraLerpFractions, this.followerCamera.lerpFraction);
			this.UpdateCameraLerpFractionLabel();
			this.UpdateCameraPositionLabel();
			this.UpdateCheckboxes();
			this.UpdateResetAndOptimizeButton();
			Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
			break;
		case "quality":
		{
			GameObject gameObject = base.SwitchTo(DialogType.SettingsMore, string.Empty);
			SettingsMoreDialog component = gameObject.GetComponent<SettingsMoreDialog>();
			component.SwitchToPageContainingQualitySettings();
			component.dialogToGoBackTo = this.dialogType;
			break;
		}
		case "movementSmoothingMedium":
			if (state)
			{
				Managers.desktopManager.SetMovementSmoothing(MovementSmoothing.Medium);
				base.SetCheckboxState(this.checkboxMovementSmoothingStrong, false, false);
			}
			else
			{
				Managers.desktopManager.SetMovementSmoothing(MovementSmoothing.None);
			}
			break;
		case "movementSmoothingStrong":
			if (state)
			{
				Managers.desktopManager.SetMovementSmoothing(MovementSmoothing.Strong);
				base.SetCheckboxState(this.checkboxMovementSmoothingMedium, false, false);
			}
			else
			{
				Managers.desktopManager.SetMovementSmoothing(MovementSmoothing.None);
			}
			break;
		case "movementSmoothingMedium_help":
			base.ToggleHelpLabel("Softens your movements.", -700, 1f, 50, 0.7f);
			break;
		case "movementSmoothingStrong_help":
			base.ToggleHelpLabel("Softens your movements a lot.", -700, 1f, 50, 0.7f);
			break;
		case "back":
			base.SwitchTo(DialogType.OwnProfile, string.Empty);
			break;
		case "close":
			base.CloseDialog();
			break;
		}
		if (!CrossDevice.desktopMode)
		{
			this.UpdateFollowerCameraActive();
			this.UpdateResetAndOptimizeButton();
		}
	}

	// Token: 0x0400088B RID: 2187
	private GameObject followerCameraObject;

	// Token: 0x0400088C RID: 2188
	private FollowerCamera followerCamera;

	// Token: 0x0400088D RID: 2189
	private Dictionary<FollowerCameraPosition, string> cameraPositionLabels = new Dictionary<FollowerCameraPosition, string>
	{
		{
			FollowerCameraPosition.InHeadVr,
			"Own view (default)"
		},
		{
			FollowerCameraPosition.InHeadDesktopOptimized,
			"Own recording-optimized view"
		},
		{
			FollowerCameraPosition.BehindUp,
			"View from behind me"
		},
		{
			FollowerCameraPosition.FurtherBehindUp,
			"View from further behind"
		},
		{
			FollowerCameraPosition.BirdsEye,
			"Bird's eye"
		},
		{
			FollowerCameraPosition.LooksAtMe,
			"Looking at me"
		},
		{
			FollowerCameraPosition.AtLeftHand,
			"Left hand"
		},
		{
			FollowerCameraPosition.AtRightHand,
			"Right hand"
		}
	};

	// Token: 0x0400088E RID: 2190
	private FollowerCameraPosition[] cameraPositionOrder = new FollowerCameraPosition[8];

	// Token: 0x0400088F RID: 2191
	private TextMesh cameraPositionLabel;

	// Token: 0x04000890 RID: 2192
	private GameObject resetButton;

	// Token: 0x04000891 RID: 2193
	private GameObject optimizeForRecordingButton;

	// Token: 0x04000892 RID: 2194
	private GameObject[] checkboxes = new GameObject[4];

	// Token: 0x04000893 RID: 2195
	private float[] cameraLerpFractions = new float[] { 1f, 0.025f, 0.0075f, 0f };

	// Token: 0x04000894 RID: 2196
	private string[] cameraLerpFractionLabels = new string[] { "Follows instantly (default)", "Follows smoothly", "Follows very smoothly", "Doesn't follow" };

	// Token: 0x04000895 RID: 2197
	private TextMesh cameraLerpFractionLabel;

	// Token: 0x04000896 RID: 2198
	private int cameraLerpFractionIndex;

	// Token: 0x04000897 RID: 2199
	private TextMesh settingsAppliedLabel;

	// Token: 0x04000898 RID: 2200
	private GameObject checkboxMovementSmoothingMedium;

	// Token: 0x04000899 RID: 2201
	private GameObject checkboxMovementSmoothingStrong;
}
