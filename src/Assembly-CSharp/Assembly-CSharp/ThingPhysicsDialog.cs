using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class ThingPhysicsDialog : Dialog
{
	// Token: 0x06000C7F RID: 3199 RVA: 0x00072540 File Offset: 0x00070940
	public void Start()
	{
		this.thing = CreationHelper.thingBeingEdited.GetComponent<Thing>();
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		base.AddBackButton();
		base.AddHeadline("Physics", -370, -460, TextColor.Default, TextAlignment.Left, false);
		this.AddMassDragAngularDragButtons();
		this.AddLockButtons();
		base.AddLabel("For emitted, thrown or movable things", 0, 410, 0.65f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddBacksideEditButtons();
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x000725D0 File Offset: 0x000709D0
	private void AddMassDragAngularDragButtons()
	{
		int num = -260;
		string text = string.Empty;
		float? mass = this.thing.mass;
		text = ((mass == null) ? (1f.ToString() + " (default)") : this.thing.mass.ToString());
		base.AddButton("mass", null, "Mass: " + text, "ButtonCompactNoIcon", 0, num, null, false, 0.9f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddHelpButton("mass_help", 280, num, true);
		num += 100;
		float? drag = this.thing.drag;
		text = ((drag == null) ? (0f.ToString() + " (default)") : this.thing.drag.ToString());
		base.AddButton("drag", null, "Drag: " + text, "ButtonCompactNoIcon", 0, num, null, false, 0.9f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddHelpButton("drag_help", 280, num, true);
		num += 100;
		float? angularDrag = this.thing.angularDrag;
		text = ((angularDrag == null) ? (0.05f.ToString() + " (default)") : this.thing.angularDrag.ToString());
		base.AddButton("angularDrag", null, "Rotation Drag: " + text, "ButtonCompactNoIcon", 0, num, null, false, 0.9f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddHelpButton("angularDrag_help", 280, num, true);
		num += 100;
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x000727E8 File Offset: 0x00070BE8
	private void AddLockButtons()
	{
		int num = 120;
		base.AddLabel("Lock Position:", -420, num + -20, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddDimensionIndicator("lockPhysicsPositionX", null, string.Empty, 0, num, DimensionIndicatorType.PositionX, true, this.thing.lockPhysicsPosition.x);
		base.AddDimensionIndicator("lockPhysicsPositionY", null, string.Empty, 0, num, DimensionIndicatorType.PositionY, true, this.thing.lockPhysicsPosition.y);
		base.AddDimensionIndicator("lockPhysicsPositionZ", null, string.Empty, 0, num, DimensionIndicatorType.PositionZ, true, this.thing.lockPhysicsPosition.z);
		num += 120;
		base.AddLabel("Lock Rotation:", -420, num + -20, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddDimensionIndicator("lockPhysicsRotationX", null, string.Empty, 0, num, DimensionIndicatorType.RotationX, true, this.thing.lockPhysicsRotation.x);
		base.AddDimensionIndicator("lockPhysicsRotationY", null, string.Empty, 0, num, DimensionIndicatorType.RotationY, true, this.thing.lockPhysicsRotation.y);
		base.AddDimensionIndicator("lockPhysicsRotationZ", null, string.Empty, 0, num, DimensionIndicatorType.RotationZ, true, this.thing.lockPhysicsRotation.z);
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x00072928 File Offset: 0x00070D28
	private string NullableFloatToPromptString(float? value)
	{
		string text = string.Empty;
		if (value != null)
		{
			text = value.ToString();
		}
		return text;
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00072958 File Offset: 0x00070D58
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "mass":
		{
			string text = "Mass, e.g. 20. Doesn't affect fall speed. Default " + 1f.ToString();
			Managers.dialogManager.GetFloatInput(this.NullableFloatToPromptString(this.thing.mass), text, delegate(float? mass)
			{
				this.thing.mass = mass;
				if (this.thing.mass == 1f)
				{
					this.thing.mass = null;
				}
				base.SwitchTo(DialogType.ThingPhysics, string.Empty);
			}, false, false);
			break;
		}
		case "drag":
		{
			string text2 = "Drag, e.g. solid metal = 0.001, feather = 10. Default " + 0f.ToString();
			Managers.dialogManager.GetFloatInput(this.NullableFloatToPromptString(this.thing.drag), text2, delegate(float? drag)
			{
				this.thing.drag = drag;
				if (this.thing.drag == 0f)
				{
					this.thing.drag = null;
				}
				base.SwitchTo(DialogType.ThingPhysics, string.Empty);
			}, false, false);
			break;
		}
		case "angularDrag":
		{
			string text3 = "Rotation Drag. Default " + 0.05f.ToString();
			Managers.dialogManager.GetFloatInput(this.NullableFloatToPromptString(this.thing.angularDrag), text3, delegate(float? angularDrag)
			{
				this.thing.angularDrag = angularDrag;
				if (this.thing.angularDrag == 0.05f)
				{
					this.thing.angularDrag = null;
				}
				base.SwitchTo(DialogType.ThingPhysics, string.Empty);
			}, false, false);
			break;
		}
		case "lockPhysicsPositionX":
			this.thing.lockPhysicsPosition.x = state;
			break;
		case "lockPhysicsPositionY":
			this.thing.lockPhysicsPosition.y = state;
			break;
		case "lockPhysicsPositionZ":
			this.thing.lockPhysicsPosition.z = state;
			break;
		case "lockPhysicsRotationX":
			this.thing.lockPhysicsRotation.x = state;
			break;
		case "lockPhysicsRotationY":
			this.thing.lockPhysicsRotation.y = state;
			break;
		case "lockPhysicsRotationZ":
			this.thing.lockPhysicsRotation.z = state;
			break;
		case "mass_help":
			base.ToggleHelpLabel("The Thing's mass in kilograms. When two things collide, their relative masses defines how they react. Note greater mass does not make things fall faster, but you can use Drag to adjust that.", -700, 1f, 50, 0.7f);
			break;
		case "drag_help":
			base.ToggleHelpLabel("The Thing's air resistance when thrown or emitted. Higher values slow it down. Low drag values makes a thing seem heavy, high ones seem light.", -700, 1f, 50, 0.7f);
			break;
		case "angularDrag_help":
			base.ToggleHelpLabel("The Thing's rotation resistance when thrown or emitted. Higher values slow down the rotation.", -700, 1f, 50, 0.7f);
			break;
		case "back":
			base.SwitchTo((Our.dialogToGoBackTo == DialogType.None) ? DialogType.ThingAttributes : Our.dialogToGoBackTo, string.Empty);
			Our.dialogToGoBackTo = DialogType.None;
			break;
		case "close":
			CreationHelper.thingPartWhoseStatesAreEdited = null;
			Our.dialogToGoBackTo = DialogType.None;
			base.SwitchTo(DialogType.Create, string.Empty);
			break;
		}
	}

	// Token: 0x0400096C RID: 2412
	private Thing thing;
}
