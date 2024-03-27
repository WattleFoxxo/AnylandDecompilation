using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class ControllableDialog : Dialog
{
	// Token: 0x06000981 RID: 2433 RVA: 0x0003D300 File Offset: 0x0003B700
	public void Start()
	{
		this.thingPart = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
		if (this.thingPart.joystickToControllablePart == null)
		{
			this.thingPart.joystickToControllablePart = new JoystickToControllablePart();
		}
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		base.AddBackButton();
		base.AddGenericHelpButton();
		base.AddHeadline("Controls", -110, -460, TextColor.Default, TextAlignment.Left, false);
		base.AddBackground("Controllable", false, false);
		base.AddSeparator(0, -360, false);
		float num = 0.65f;
		int num2 = -310;
		int num3 = -255;
		base.AddLabel("Body", num2, -335, num, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddCheckbox("isControllableCollider", null, "Is collision body", 180, num3, false, 1f, "CheckboxCompact", TextColor.Default, null, ExtraIcon.None);
		num3 += 80;
		this.slidinessButton = base.AddButton("controllableBodySlidiness", null, "Slidiness: " + this.FloatToPercentForButton(this.thingPart.controllableBodySlidiness), "ButtonCompactNoIcon", 180, num3, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		num3 += 80;
		this.bouncinessButton = base.AddButton("controllableBodyBounciness", null, "Bounciness: " + this.FloatToPercentForButton(this.thingPart.controllableBodyBounciness), "ButtonCompactNoIcon", 180, num3, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		num3 += 80;
		base.AddSeparator(0, -10, false);
		base.AddLabel("Stick Control", num2, 15, num, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.StartCoroutine(this.AddThrustAndMoreButtons());
		base.AddSeparator(0, 370, false);
		base.AddCheckbox("invisible", null, "Invisible", -240, 430, this.thingPart.invisible, 1f, "CheckboxCompact", TextColor.Default, null, ExtraIcon.None);
		base.AddButton("thingPhysics", null, "Thing Physics...", "ButtonCompactNoIcon", 240, 430, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		if (this.thingPart.joystickToControllablePart != null)
		{
			this.AddDirectionArrows();
		}
		base.AddBacksideEditButtons();
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0003D580 File Offset: 0x0003B980
	private IEnumerator AddThrustAndMoreButtons()
	{
		int y = 80;
		int labelX = -160;
		int labeledButtonX = labelX + 220;
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		if (this.page == 0)
		{
			base.AddLabel("Thrust:", labelX, y + -15, 0.65f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			base.AddDimensionIndicator("thrustX", null, this.FloatToPercentForButton(this.thingPart.joystickToControllablePart.thrust.x), labeledButtonX, y, DimensionIndicatorType.PositionX, false, false);
			base.AddDimensionIndicator("thrustY", null, this.FloatToPercentForButton(this.thingPart.joystickToControllablePart.thrust.y), labeledButtonX, y, DimensionIndicatorType.PositionY, false, false);
			base.AddDimensionIndicator("thrustZ", null, this.FloatToPercentForButton(this.thingPart.joystickToControllablePart.thrust.z), labeledButtonX, y, DimensionIndicatorType.PositionZ, false, false);
			y += 110;
			base.AddLabel("Rotate:", labelX, y + -15, 0.65f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			base.AddDimensionIndicator("rotationY", null, this.FloatToPercentForButton(this.thingPart.joystickToControllablePart.rotation.y), labeledButtonX, y, DimensionIndicatorType.RotationY, false, false);
		}
		else if (this.page == 1)
		{
			base.AddCheckbox("isControllableWheel", null, "Is Wheel", 180, y, this.thingPart.isControllableWheel, 1f, "CheckboxCompact", TextColor.Default, null, ExtraIcon.None);
			y += 80;
		}
		else if (this.page == 2)
		{
			string text = string.Concat(new string[]
			{
				"Script event: ",
				Environment.NewLine,
				"\"When controlled...\"",
				Environment.NewLine,
				Environment.NewLine,
				"Dialog hand trigger:",
				Environment.NewLine,
				"\"When triggered...\""
			});
			base.AddLabel(text, -120, 50, 0.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0003D59C File Offset: 0x0003B99C
	private void AddDirectionArrows()
	{
		string text = "Prefabs/DirectionArrows";
		this.arrows = global::UnityEngine.Object.Instantiate(Resources.Load(text, typeof(GameObject))) as GameObject;
		this.arrows.name = "DirectionArrows";
		this.arrows.transform.parent = this.thingPart.transform.parent;
		this.UpdateArrowsTransform();
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x0003D608 File Offset: 0x0003BA08
	private void UpdateArrowsTransform()
	{
		if (this.arrows != null && this.thingPart != null)
		{
			this.arrows.transform.rotation = this.thingPart.transform.rotation;
			this.arrows.transform.position = this.thingPart.transform.position;
			this.arrows.transform.Rotate(90f, 0f, 0f);
			this.arrows.transform.Rotate(0f, -90f, 0f);
			float num = Misc.GetLargestValueOfVector(this.thingPart.transform.localScale) * 7.5f;
			this.arrows.transform.localScale = Misc.GetUniformVector3(num);
		}
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0003D6E8 File Offset: 0x0003BAE8
	private string FloatToPercentForButton(float f)
	{
		return Mathf.Round(f * 100f).ToString() + "%";
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0003D71C File Offset: 0x0003BB1C
	private string FloatToPercentForPrompt(float f)
	{
		return (f == 0f) ? string.Empty : Mathf.Round(f * 100f).ToString();
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x0003D758 File Offset: 0x0003BB58
	private new void Update()
	{
		if (this.thingPart == null)
		{
			this.Close();
		}
		this.UpdateArrowsTransform();
		base.ReactToOnClick();
		base.ReactToOnClickInWrapper(this.wrapper);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x0003D789 File Offset: 0x0003BB89
	private void Close()
	{
		base.SwitchTo(DialogType.Create, string.Empty);
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0003D799 File Offset: 0x0003BB99
	private void OnDestroy()
	{
		global::UnityEngine.Object.Destroy(this.arrows);
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x0003D7A6 File Offset: 0x0003BBA6
	private float PercentToFloat(object percent, float maxPercent = 1000f)
	{
		return Mathf.Clamp((float)percent / 100f, -maxPercent, maxPercent);
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0003D7BC File Offset: 0x0003BBBC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "isControllableWheel":
			this.thingPart.isControllableWheel = state;
			if (state && !this.arrows)
			{
				this.AddDirectionArrows();
			}
			break;
		case "invisible":
			this.thingPart.invisible = state;
			break;
		case "controllableBodySlidiness":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.controllableBodySlidiness), "slidiness in %", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.controllableBodySlidiness = this.PercentToFloat(percent, 100f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, false, false);
			break;
		case "controllableBodyBounciness":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.controllableBodyBounciness), "bounciness in %", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.controllableBodyBounciness = this.PercentToFloat(percent, 100f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, false, false);
			break;
		case "thrustX":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.thrust.x), "thrust in %", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.thrust.x = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "thrustY":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.thrust.y), "thrust in %", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.thrust.y = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "thrustZ":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.thrust.z), "thrust in %", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.thrust.z = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "constantMininumThrustX":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.constantMininumThrust.x), "thrust in %", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.constantMininumThrust.x = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "constantMininumThrustY":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.constantMininumThrust.y), "thrust in %", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.constantMininumThrust.y = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "constantMininumThrustZ":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.constantMininumThrust.z), "thrust in %", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.constantMininumThrust.z = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "rotationX":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.rotation.x), "rotation in % (100% = 90°)", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.rotation.x = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "rotationY":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.rotation.y), "rotation in % (100% = 90°)", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.rotation.y = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "rotationZ":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.rotation.z), "rotation in % (100% = 90°)", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.rotation.z = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "constantMininumRotationX":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.constantMininumRotation.x), "rotation in % (100% = 90°)", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.constantMininumRotation.x = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "constantMininumRotationY":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.constantMininumRotation.y), "rotation in % (100% = 90°)", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.constantMininumRotation.y = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "constantMininumRotationZ":
			Managers.dialogManager.GetFloatInput(this.FloatToPercentForPrompt(this.thingPart.joystickToControllablePart.constantMininumRotation.z), "rotation in % (100% = 90°)", delegate(float? percent)
			{
				if (percent != null)
				{
					this.thingPart.joystickToControllablePart.constantMininumRotation.z = this.PercentToFloat(percent, 1000f);
				}
				base.SwitchTo(DialogType.Controllable, string.Empty);
			}, true, false);
			break;
		case "thingPhysics":
			Our.dialogToGoBackTo = DialogType.Controllable;
			base.SwitchTo(DialogType.ThingPhysics, string.Empty);
			break;
		case "previousPage":
		{
			int num = --this.page;
			if (num < 0)
			{
				this.page = 2;
			}
			base.StartCoroutine(this.AddThrustAndMoreButtons());
			break;
		}
		case "nextPage":
		{
			int num = ++this.page;
			if (num >= 3)
			{
				this.page = 0;
			}
			base.StartCoroutine(this.AddThrustAndMoreButtons());
			break;
		}
		case "help":
			Managers.browserManager.OpenGuideBrowser("3gPznj8QYTg", null);
			break;
		case "back":
			ThingPartAttributesDialog.immediatelyOpenControllableDialogIfControllable = false;
			base.SwitchTo(DialogType.ThingPartAttributes, string.Empty);
			break;
		case "close":
			this.Close();
			break;
		}
	}

	// Token: 0x0400072B RID: 1835
	private ThingPart thingPart;

	// Token: 0x0400072C RID: 1836
	private GameObject arrows;

	// Token: 0x0400072D RID: 1837
	private const int maxPages = 3;

	// Token: 0x0400072E RID: 1838
	private int page;

	// Token: 0x0400072F RID: 1839
	private const int buttonMarginY = 80;

	// Token: 0x04000730 RID: 1840
	private const int buttonX = 180;

	// Token: 0x04000731 RID: 1841
	private GameObject slidinessButton;

	// Token: 0x04000732 RID: 1842
	private GameObject bouncinessButton;
}
