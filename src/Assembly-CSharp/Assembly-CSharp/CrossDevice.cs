using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x020000F2 RID: 242
public static class CrossDevice
{
	// Token: 0x1700013E RID: 318
	// (get) Token: 0x0600081B RID: 2075 RVA: 0x0002DD1F File Offset: 0x0002C11F
	// (set) Token: 0x0600081C RID: 2076 RVA: 0x0002DD26 File Offset: 0x0002C126
	public static global::DeviceType type { get; private set; }

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x0600081D RID: 2077 RVA: 0x0002DD2E File Offset: 0x0002C12E
	// (set) Token: 0x0600081E RID: 2078 RVA: 0x0002DD35 File Offset: 0x0002C135
	public static ulong button_grab { get; private set; }

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x0600081F RID: 2079 RVA: 0x0002DD3D File Offset: 0x0002C13D
	// (set) Token: 0x06000820 RID: 2080 RVA: 0x0002DD44 File Offset: 0x0002C144
	public static ulong button_grabTip { get; private set; }

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000821 RID: 2081 RVA: 0x0002DD4C File Offset: 0x0002C14C
	// (set) Token: 0x06000822 RID: 2082 RVA: 0x0002DD53 File Offset: 0x0002C153
	public static ulong button_teleport { get; private set; }

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000823 RID: 2083 RVA: 0x0002DD5B File Offset: 0x0002C15B
	// (set) Token: 0x06000824 RID: 2084 RVA: 0x0002DD62 File Offset: 0x0002C162
	public static ulong button_context { get; private set; }

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000825 RID: 2085 RVA: 0x0002DD6A File Offset: 0x0002C16A
	// (set) Token: 0x06000826 RID: 2086 RVA: 0x0002DD71 File Offset: 0x0002C171
	public static ulong button_delete { get; private set; }

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x0002DD79 File Offset: 0x0002C179
	// (set) Token: 0x06000828 RID: 2088 RVA: 0x0002DD80 File Offset: 0x0002C180
	public static ulong button_legPuppeteering { get; private set; }

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000829 RID: 2089 RVA: 0x0002DD88 File Offset: 0x0002C188
	// (set) Token: 0x0600082A RID: 2090 RVA: 0x0002DD8F File Offset: 0x0002C18F
	public static bool joystickWasFound { get; private set; }

	// Token: 0x0600082B RID: 2091 RVA: 0x0002DD97 File Offset: 0x0002C197
	public static void Init()
	{
		CrossDevice.oculusTouchLegacyMode = PlayerPrefs.GetInt("oculusTouchLegacyMode", 0) == 1;
		CrossDevice.type = CrossDevice.GetTypeByModelName(CrossDevice.GetVrModelString());
		CrossDevice.SetButtonMappingForType();
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x0002DDC0 File Offset: 0x0002C1C0
	public static string GetVrModelString()
	{
		return (XRDevice.model == null) ? string.Empty : XRDevice.model;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0002DDDC File Offset: 0x0002C1DC
	private static string GetModelOverride()
	{
		string text = null;
		string[] joystickNames = Input.GetJoystickNames();
		if (joystickNames != null)
		{
			foreach (string text2 in joystickNames)
			{
				if (!string.IsNullOrEmpty(text2) && (text2.Contains("Knuckles") || text2.Contains("Index")))
				{
					CrossDevice.joystickWasFound = true;
					text = "Valve Index";
					break;
				}
			}
		}
		return text;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0002DE54 File Offset: 0x0002C254
	private static global::DeviceType GetTypeByModelName(string model)
	{
		global::DeviceType deviceType = global::DeviceType.Other;
		string modelOverride = CrossDevice.GetModelOverride();
		if (!string.IsNullOrEmpty(modelOverride))
		{
			model = modelOverride;
		}
		model = model.ToLower();
		if (model.Contains("rift") || model.Contains("quest") || model.Contains("miramar"))
		{
			deviceType = global::DeviceType.OculusTouch;
		}
		else if (model.Contains("index"))
		{
			deviceType = global::DeviceType.Index;
		}
		else if (model.Contains("vive"))
		{
			deviceType = global::DeviceType.Vive;
		}
		else if (model.Contains("mixed") || model.Contains("lenovo explorer") || model.Contains("acer"))
		{
			deviceType = global::DeviceType.WindowsMixedReality;
		}
		return deviceType;
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x0002DF18 File Offset: 0x0002C318
	private static void SetButtonMappingForType()
	{
		ulong num = 128UL;
		switch (CrossDevice.type)
		{
		case global::DeviceType.OculusTouch:
			CrossDevice.button_grab = 4UL;
			CrossDevice.button_grabTip = 8589934592UL;
			if (CrossDevice.oculusTouchLegacyMode)
			{
				CrossDevice.button_teleport = num;
				CrossDevice.button_context = 2UL;
				CrossDevice.button_delete = 4294967296UL;
			}
			else
			{
				CrossDevice.button_teleport = 4294967296UL;
				CrossDevice.button_context = 2UL;
				CrossDevice.button_delete = num;
			}
			CrossDevice.hasStick = true;
			CrossDevice.hasSeparateTriggerAndGrab = true;
			goto IL_11F;
		case global::DeviceType.Index:
			CrossDevice.button_grab = 4UL;
			CrossDevice.button_grabTip = 8589934592UL;
			CrossDevice.button_teleport = 4294967296UL;
			CrossDevice.button_context = 2UL;
			CrossDevice.button_delete = num;
			CrossDevice.hasStick = true;
			CrossDevice.hasSeparateTriggerAndGrab = true;
			goto IL_11F;
		}
		CrossDevice.button_grab = 8589934592UL;
		CrossDevice.button_grabTip = 8589934592UL;
		CrossDevice.button_teleport = 4294967296UL;
		CrossDevice.button_context = 2UL;
		CrossDevice.button_delete = 4UL;
		CrossDevice.hasStick = false;
		CrossDevice.hasSeparateTriggerAndGrab = false;
		IL_11F:
		CrossDevice.button_legPuppeteering = CrossDevice.button_teleport;
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0002E050 File Offset: 0x0002C450
	public static void AdjustControllerTransformIfNeeded(Transform transform)
	{
		if (transform.CompareTag("HandCore"))
		{
			global::DeviceType type = CrossDevice.type;
			if (type != global::DeviceType.OculusTouch)
			{
				if (type != global::DeviceType.Index)
				{
					if (type == global::DeviceType.WindowsMixedReality)
					{
						transform.Rotate(new Vector3(30f, 0f, 0f));
					}
				}
				else
				{
					transform.Rotate(new Vector3(23.5f, 0f, 0f));
					float num = ((!(Managers.personManager != null)) ? 1f : Managers.personManager.GetOurScale());
					transform.Translate(Vector3.forward * (-0.02f * num));
				}
			}
			else
			{
				transform.Rotate(new Vector3(23.5f, 0f, 0f));
				float num2 = ((!(Managers.personManager != null)) ? 1f : Managers.personManager.GetOurScale());
				transform.Translate(Vector3.forward * (-0.02f * num2));
			}
		}
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x0002E164 File Offset: 0x0002C564
	public static bool GetPress(SteamVR_Controller.Device controller, ulong buttonType, Side handSide)
	{
		bool flag = false;
		if (CrossDevice.desktopMode)
		{
			flag = CrossDevice.GetMouseButton(buttonType, handSide);
		}
		else if (controller != null)
		{
			flag = controller.GetPress(buttonType);
		}
		return flag;
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0002E19C File Offset: 0x0002C59C
	public static bool GetPressUp(SteamVR_Controller.Device controller, ulong buttonType, Side handSide)
	{
		bool flag = false;
		if (CrossDevice.desktopMode)
		{
			flag = CrossDevice.GetMouseButtonUp(buttonType, handSide);
		}
		else if (controller != null)
		{
			flag = controller.GetPressUp(buttonType);
		}
		return flag;
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0002E1D4 File Offset: 0x0002C5D4
	public static bool GetPressDown(SteamVR_Controller.Device controller, ulong buttonType, Side handSide)
	{
		bool flag = false;
		if (CrossDevice.desktopMode)
		{
			flag = CrossDevice.GetMouseButtonDown(buttonType, handSide);
		}
		else if (controller != null)
		{
			flag = controller.GetPressDown(buttonType);
		}
		return flag;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0002E20C File Offset: 0x0002C60C
	private static bool GetMouseButton(ulong buttonType, Side handSide)
	{
		bool flag = false;
		int mouseButtonInt = CrossDevice.GetMouseButtonInt(buttonType, handSide);
		if (mouseButtonInt != -1)
		{
			flag = Input.GetMouseButton(mouseButtonInt);
		}
		return flag;
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0002E234 File Offset: 0x0002C634
	private static bool GetMouseButtonUp(ulong buttonType, Side handSide)
	{
		bool flag = false;
		int mouseButtonInt = CrossDevice.GetMouseButtonInt(buttonType, handSide);
		if (mouseButtonInt != -1)
		{
			flag = Input.GetMouseButtonUp(mouseButtonInt);
		}
		return flag;
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0002E25C File Offset: 0x0002C65C
	private static bool GetMouseButtonDown(ulong buttonType, Side handSide)
	{
		bool flag = false;
		int mouseButtonInt = CrossDevice.GetMouseButtonInt(buttonType, handSide);
		if (mouseButtonInt != -1)
		{
			flag = Input.GetMouseButtonDown(mouseButtonInt);
		}
		return flag;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0002E284 File Offset: 0x0002C684
	public static int GetMouseButtonInt(ulong buttonType, Side handSide)
	{
		int num = -1;
		if (buttonType == CrossDevice.button_grab || buttonType == CrossDevice.button_grabTip)
		{
			if (Our.GetCurrentNonStartDialog() == null)
			{
				num = 0;
			}
		}
		else if (buttonType != CrossDevice.button_teleport)
		{
			if (buttonType == CrossDevice.button_delete)
			{
				num = 2;
			}
			else if (buttonType == CrossDevice.button_context && handSide == CrossDevice.desktopDialogSide)
			{
				num = 1;
			}
		}
		return num;
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0002E2FC File Offset: 0x0002C6FC
	public static void TriggerHapticPulse(Hand hand, ushort pulseAmount)
	{
		if (Managers.areaManager != null && Managers.areaManager.startedLaunchFadeIn && hand != null && Universe.features.hapticPulse)
		{
			if (CrossDevice.desktopMode)
			{
				Managers.soundManager.Play("bump", hand.transform, 0.05f, false, false);
			}
			else if (hand.controller != null && hand.controller.connected)
			{
				hand.controller.TriggerHapticPulse(pulseAmount, EVRButtonId.k_EButton_Axis0);
			}
		}
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0002E397 File Offset: 0x0002C797
	public static void SetOculusTouchLegacyMode(bool state)
	{
		CrossDevice.oculusTouchLegacyMode = state;
		PlayerPrefs.SetInt("oculusTouchLegacyMode", (!state) ? 0 : 1);
		CrossDevice.SetButtonMappingForType();
	}

	// Token: 0x04000629 RID: 1577
	public static bool desktopMode;

	// Token: 0x0400062A RID: 1578
	public static bool rigPositionIsAuthority = true;

	// Token: 0x0400062B RID: 1579
	public static Side desktopDialogSide = Side.Left;

	// Token: 0x0400062C RID: 1580
	public static bool oculusTouchLegacyMode;

	// Token: 0x0400062D RID: 1581
	private const string oculusTouchLegacyMode_key = "oculusTouchLegacyMode";

	// Token: 0x0400062E RID: 1582
	public static bool hasStick;

	// Token: 0x0400062F RID: 1583
	public static bool hasSeparateTriggerAndGrab;

	// Token: 0x04000637 RID: 1591
	public static bool useAlternativeControllerAdjust;
}
