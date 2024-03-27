using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class ThingAttributesDialog : Dialog
{
	// Token: 0x06000BE8 RID: 3048 RVA: 0x00064F98 File Offset: 0x00063398
	public void Start()
	{
		this.thing = CreationHelper.thingBeingEdited.GetComponent<Thing>();
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		this.AddMoreButtons();
		this.AddAttributes();
		base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
		this.AddBacksideButtons();
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x00065000 File Offset: 0x00063400
	private void AddAttributes()
	{
		this.RemoveAttributes();
		int num = 0;
		if (this.currentPage == 1)
		{
			this.attributeButtons.Add(base.AddCheckbox("isHoldable", null, "Holdable", 0, -420 + num * 115, this.thing.isHoldable, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Holdable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isHoldable_help", -420 + num * 115));
			num++;
			string text = "Remains held until " + ((!CrossDevice.hasSeparateTriggerAndGrab) ? "grip-pressed" : "delete pressed");
			float num2 = ((!CrossDevice.hasSeparateTriggerAndGrab) ? 1.09f : 1f);
			List<GameObject> list = this.attributeButtons;
			string text2 = "remainsHeld";
			string text3 = null;
			string empty = string.Empty;
			int num3 = 0;
			int num4 = -420 + num * 115;
			bool remainsHeld = this.thing.remainsHeld;
			float num5 = num2;
			string text4 = text;
			list.Add(base.AddCheckbox(text2, text3, empty, num3, num4, remainsHeld, num5, "Checkbox", TextColor.Default, text4, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("remainsHeld_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("isClonable", null, "Clonable", 0, -420 + num * 115, this.thing.isClonable, 1f, "Checkbox", TextColor.Default, "by others", ExtraIcon.Clonable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isClonable_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("isClimbable", null, "Climbable", 0, -420 + num * 115, this.thing.isClimbable, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Climbable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isClimbable_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("doShowDirection", null, "Show center + direction", 0, -420 + num * 115, this.thing.doShowDirection, 0.9f, "Checkbox", TextColor.Default, null, ExtraIcon.ShowDirection));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("doShowDirection_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("invisible", null, "Invisible", 0, -420 + num * 115, this.thing.invisible, 1f, "Checkbox", TextColor.Default, "when done", ExtraIcon.Invisible));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("invisible_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("uncollidable", null, "Uncollidable", 0, -420 + num * 115, this.thing.uncollidable, 1f, "Checkbox", TextColor.Default, "when done", ExtraIcon.Uncollidable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("uncollidable_help", -420 + num * 115));
			num++;
		}
		else if (this.currentPage == 2)
		{
			this.attributeButtons.Add(base.AddCheckbox("isSittable", null, "Sittable", 0, -420 + num * 115, this.thing.isSittable, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Sittable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isSittable_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("isUnwalkable", null, "Unwalkable", 0, -420 + num * 115, this.thing.isUnwalkable, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Unwalkable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isUnwalkable_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("isBouncy", null, "Bouncy", 0, -420 + num * 115, this.thing.isBouncy, 1f, "Checkbox", TextColor.Default, "when thrown", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isBouncy_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("doesFloat", null, "Floats", 0, -420 + num * 115, this.thing.doesFloat, 1f, "Checkbox", TextColor.Default, "when thrown", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("doesFloat_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("isSlidy", null, "Slidy", 0, -420 + num * 115, this.thing.isSlidy, 1f, "Checkbox", TextColor.Default, "when thrown", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isSlidy_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("doesShatter", null, "Shatters", 0, -420 + num * 115, this.thing.doesShatter, 1f, "Checkbox", TextColor.Default, "when thrown", ExtraIcon.Shatters));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("doesShatter_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("isSticky", null, "Sticky", 0, -420 + num * 115, this.thing.isSticky, 1f, "Checkbox", TextColor.Default, "when thrown", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isSticky_help", -420 + num * 115));
			num++;
		}
		else if (this.currentPage == 3)
		{
			this.attributeButtons.Add(base.AddCheckbox("benefitsFromShowingAtDistance", null, "Show if far away too", 0, -420 + num * 115, this.thing.benefitsFromShowingAtDistance, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.ShowAtDistance));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("benefitsFromShowingAtDistance_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("amplifySpeech", null, "Amplify speech", 0, -420 + num * 115, this.thing.amplifySpeech, 1f, "Checkbox", TextColor.Default, "when held", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("amplifySpeech_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("hasSurroundSound", null, "Surround sound", 0, -420 + num * 115, this.thing.hasSurroundSound, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("hasSurroundSound_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("replacesHandsWhenAttached", null, "Replaces hand", 0, -420 + num * 115, this.thing.replacesHandsWhenAttached, 1f, "Checkbox", TextColor.Default, "when worn", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("replacesHandsWhenAttached_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("addBodyWhenAttached", null, "Add current body", 0, -420 + num * 115, this.thing.addBodyWhenAttached, 0.85f, "Checkbox", TextColor.Default, "when worn", ExtraIcon.AddBody));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("addBodyWhenAttached_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("stricterPhysicsSyncing", null, "Stricter physics syncing", 0, -420 + num * 115, this.thing.stricterPhysicsSyncing, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("stricterPhysicsSyncing_help", -420 + num * 115));
			num++;
			this.attributeButtons.Add(base.AddCheckbox("movableByEveryone", null, "Movable", 0, -420 + num * 115, this.thing.movableByEveryone, 1f, "Checkbox", TextColor.Default, "by everyone", ExtraIcon.Movable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("movableByEveryone_help", -420 + num * 115));
			num++;
		}
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x000658F4 File Offset: 0x00063CF4
	private void RemoveAttributes()
	{
		foreach (GameObject gameObject in this.attributeButtons)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.attributeButtons = new List<GameObject>();
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x0006595C File Offset: 0x00063D5C
	private void AddBacksideButtons()
	{
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		base.AddButton("editCreationDescription", null, "Description...", "ButtonCompactNoIcon", 0, -200, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		if (!string.IsNullOrEmpty(this.thing.description))
		{
			string text = Misc.WrapWithNewlines(this.thing.description, 50, 6);
			base.AddLabel(text, 0, -120, 0.65f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		base.AddButton("splitOffParts", null, "Split off completions...", "ButtonCompactNoIcon", 0, 200, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00065A54 File Offset: 0x00063E54
	private void AddMoreButtons()
	{
		base.MinifyMoreButton(base.AddButton("physics", null, "Physics", "ButtonCompactNoIconShortCentered", -355, 400, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
		base.MinifyMoreButton(base.AddButton("more", null, "More", "ButtonCompactNoIconShortCentered", 355, 400, null, false, 1f, TextColor.Gray, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x00065AEF File Offset: 0x00063EEF
	private void MakeHoldableIfNeeded(bool newState)
	{
		if (newState && !this.thing.isHoldable && !this.thing.remainsHeld)
		{
			this.thing.isHoldable = true;
			this.AddAttributes();
		}
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00065B2C File Offset: 0x00063F2C
	private void SplitOffParts()
	{
		string text = "This turns all mirrored and auto-completed parts into standalone parts. Continue?";
		base.SwitchToConfirmDialog(text, delegate(bool didConfirm)
		{
			if (didConfirm)
			{
				this.DoSplitOffParts();
				Managers.soundManager.Play("success", null, 0.2f, false, false);
			}
			base.SwitchTo(DialogType.ThingAttributes, string.Empty);
		});
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00065B54 File Offset: 0x00063F54
	private void DoSplitOffParts()
	{
		if (this.thing != null)
		{
			this.thing.ResetStates();
			this.thing.autoAddReflectionPartsSideways = false;
			this.thing.autoAddReflectionPartsVertical = false;
			this.thing.autoAddReflectionPartsDepth = false;
			IEnumerator enumerator = this.thing.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					ThingPart component = transform.GetComponent<ThingPart>();
					if (component != null)
					{
						component.SplitAutoContinuedAndReflectedPartsMaterial();
					}
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
			IEnumerator enumerator2 = this.thing.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					Transform transform2 = (Transform)obj2;
					ThingPart component2 = transform2.GetComponent<ThingPart>();
					if (component2 != null)
					{
						component2.enabled = true;
						component2.SplitAutoContinuedAndReflectedPartsIntoStandalones();
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = enumerator2 as IDisposable) != null)
				{
					disposable2.Dispose();
				}
			}
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00065C90 File Offset: 0x00064090
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		bool flag = false;
		switch (contextName)
		{
		case "isClonable":
			this.thing.isClonable = state;
			if (this.thing.isClonable && this.thing.isNeverClonable)
			{
				this.thing.isNeverClonable = false;
				string text2 = "Ok, set to Clonable. \"Is never clonable\" was unset now.";
				Managers.dialogManager.ShowInfo(text2, true, true, 1, DialogType.ThingAttributes, 1f, false, TextColor.Default, TextAlignment.Left);
			}
			break;
		case "isClimbable":
			this.thing.isClimbable = state;
			break;
		case "isUnwalkable":
			this.thing.isUnwalkable = state;
			break;
		case "uncollidable":
			this.thing.uncollidable = state;
			break;
		case "doShowDirection":
			this.thing.doShowDirection = state;
			if (!this.thing.doShowDirection)
			{
				this.thing.RemoveDirectionLine();
			}
			break;
		case "isBouncy":
			this.thing.isBouncy = state;
			this.MakeHoldableIfNeeded(state);
			break;
		case "doesFloat":
			this.thing.doesFloat = state;
			this.MakeHoldableIfNeeded(state);
			break;
		case "doesShatter":
			this.thing.doesShatter = state;
			this.MakeHoldableIfNeeded(state);
			break;
		case "isSticky":
			this.thing.isSticky = state;
			this.MakeHoldableIfNeeded(state);
			break;
		case "isSlidy":
			this.thing.isSlidy = state;
			this.MakeHoldableIfNeeded(state);
			break;
		case "amplifySpeech":
			this.thing.amplifySpeech = state;
			this.MakeHoldableIfNeeded(state);
			break;
		case "hasSurroundSound":
			this.thing.hasSurroundSound = state;
			break;
		case "canGetEventsWhenStateChanging":
			this.thing.canGetEventsWhenStateChanging = state;
			break;
		case "replacesHandsWhenAttached":
			this.thing.replacesHandsWhenAttached = state;
			break;
		case "mergeParticleSystems":
			this.thing.mergeParticleSystems = state;
			break;
		case "isSittable":
			this.thing.isSittable = state;
			break;
		case "benefitsFromShowingAtDistance":
			this.thing.benefitsFromShowingAtDistance = state;
			break;
		case "isHoldable":
			this.thing.isHoldable = state;
			if (this.thing.isHoldable && this.thing.movableByEveryone)
			{
				this.thing.movableByEveryone = false;
				flag = true;
			}
			else if (!this.thing.isHoldable && this.thing.remainsHeld)
			{
				this.thing.remainsHeld = false;
				flag = true;
			}
			break;
		case "remainsHeld":
			this.thing.remainsHeld = state;
			if (!this.thing.isHoldable)
			{
				this.thing.isHoldable = true;
				this.thing.movableByEveryone = false;
				flag = true;
			}
			break;
		case "stricterPhysicsSyncing":
			this.thing.stricterPhysicsSyncing = state;
			if (!this.thing.stricterPhysicsSyncing && this.thing.persistWhenThrownOrEmitted)
			{
				this.thing.persistWhenThrownOrEmitted = false;
				flag = true;
			}
			break;
		case "movableByEveryone":
			this.thing.movableByEveryone = state;
			if (!this.thing.movableByEveryone && this.thing.isHoldable)
			{
				this.thing.isHoldable = false;
				this.thing.remainsHeld = false;
				flag = true;
			}
			break;
		case "removeOriginalWhenGrabbed":
			this.thing.removeOriginalWhenGrabbed = state;
			if (this.thing.removeOriginalWhenGrabbed && !this.thing.isHoldable)
			{
				this.thing.isHoldable = true;
				flag = true;
			}
			break;
		case "persistWhenThrownOrEmitted":
			this.thing.persistWhenThrownOrEmitted = state;
			if (this.thing.persistWhenThrownOrEmitted && !this.thing.stricterPhysicsSyncing)
			{
				this.thing.stricterPhysicsSyncing = true;
				flag = true;
			}
			break;
		case "invisible":
			this.thing.invisible = !this.thing.invisible;
			break;
		case "addBodyWhenAttached":
			this.thing.addBodyWhenAttached = state;
			if (state && this.thing.addBodyWhenAttachedNonClearing)
			{
				this.thing.addBodyWhenAttachedNonClearing = false;
				flag = true;
			}
			break;
		case "addBodyWhenAttached_help":
			base.ToggleHelpLabel("When someone puts this on as head, your current body will be auto-worn with it. When edit-cloning your currently worn head, the head position will also auto-adjust. Body parts you have empty now will clear existing parts when wearing.", -700, 1f, 50, 0.7f);
			break;
		case "isSittable_help":
			base.ToggleHelpLabel("Provides a lowered position when teleporting onto", -700, 1f, 50, 0.7f);
			break;
		case "benefitsFromShowingAtDistance_help":
			base.ToggleHelpLabel("Very big things or those using e.g. \"tell any\" already show even when far away, but use this when a thing isn't showing properly. Alternatively, context-laser a placement, turn around the dialog, and click \"Show at...\".", -700, 1f, 50, 0.7f);
			break;
		case "hasSurroundSound_help":
			base.ToggleHelpLabel("Sounds played from this thing will be hearable in all of the area even at far distances (also see the  \"surround\" keyword for play and loop commands)", -700, 1f, 50, 0.7f);
			break;
		case "canGetEventsWhenStateChanging_help":
			base.ToggleHelpLabel("by default, \"when ..\" commands only trigger when parts aren't currently changing to another state, but you can toggle that here", -700, 1f, 50, 0.7f);
			break;
		case "replacesHandsWhenAttached_help":
			base.ToggleHelpLabel("This hides the hand when attached to the arm sphere", -700, 1f, 50, 0.7f);
			break;
		case "mergeParticleSystems_help":
			base.ToggleHelpLabel("When done, all parts of the same color & particle emitter are merged.", -700, 1f, 50, 0.7f);
			break;
		case "isClonable_help":
			base.ToggleHelpLabel("Allows others to edit a clone of your creation, while giving credit to you in the context-laser info. You can also allow this for all your creations via Me ➞ \"...\" ➞ \"All my creations are clonable\"", -700, 1f, 50, 0.7f);
			break;
		case "isHoldable_help":
			base.ToggleHelpLabel("Allows people to pick up and hold the thing when not in \"Change Things\" mode", -700, 1f, 50, 0.7f);
			break;
		case "remainsHeld_help":
			base.ToggleHelpLabel("Will make a holdable automatically stay held in one's hands until one actively releases it again. This is a required setting if you want to script the \"when triggered\" action", -700, 1f, 50, 0.7f);
			break;
		case "isClimbable_help":
			base.ToggleHelpLabel("Makes your creation Climbable. In non-\"Change Things\" mode (for everyone who's not an editor in your area), people will only be able to teleport to vertical or ceiling objects if this is set.", -700, 1f, 50, 0.7f);
			break;
		case "doShowDirection_help":
			base.ToggleHelpLabel("Shows a line coming out of the front-facing direction of your creation during editing, useful for e.g. knowing where a script would emit something. Also note there's an \"...\" option for each Thing Part to \"Show direction\".", -700, 1f, 50, 0.7f);
			break;
		case "isBouncy_help":
			base.ToggleHelpLabel("Makes holdable things that are thrown bounce more when colliding with another object", -700, 1f, 50, 0.7f);
			break;
		case "isUnwalkable_help":
			base.ToggleHelpLabel("Disallows the walk teleport laser to be used on this object. For instance, you can use this for areas you don't want people to go to", -700, 1f, 50, 0.7f);
			break;
		case "doesFloat_help":
			base.ToggleHelpLabel("Make this object fly when held and thrown, unaffected by gravity (also see the Area setting \"Zero Gravity\", which enables this for all objects)", -700, 1f, 50, 0.7f);
			break;
		case "doesShatter_help":
			base.ToggleHelpLabel("This will make a thrown holdable break when it collides with another object", -700, 1f, 50, 0.7f);
			break;
		case "isSticky_help":
			base.ToggleHelpLabel("Thrown holdables or emitted things will remain stuck at objects they collide with", -700, 1f, 50, 0.7f);
			break;
		case "amplifySpeech_help":
			base.ToggleHelpLabel("Have one's voice be heard area-wide at any distance when thing is held or eaten. Can be disabled via area placements using \"When starts then disallow amplified speech\".", -700, 1f, 50, 0.7f);
			break;
		case "isSlidy_help":
			base.ToggleHelpLabel("Makes thrown holdables slide when on surfaces", -700, 1f, 50, 0.7f);
			break;
		case "autoAddReflectionParts_help":
			base.ToggleHelpLabel("Creates a symmetrically mirrored part for parts you drop into the creation. You can start your creation with a snap-angle cube that you lock and set to be \"Invisible when done\" to serve as mirror center.", -700, 1f, 50, 0.7f);
			break;
		case "stricterPhysicsSyncing_help":
			base.ToggleHelpLabel("Extra-strictly synchronizes the object for everyone when it's thrown or emitted. Useful for e.g. a dice roll. Also increases the object's lifetime from 30 seconds to 2 minutes. (Use with care, may add syncing flicker & lag.)", -700, 1f, 50, 0.7f);
			break;
		case "movableByEveryone_help":
			base.ToggleHelpLabel("Allows moving this in non-\"Change Things\" mode (e.g. board game pieces). Position resets if all left, on \"reset area\" command, or moving item in \"Change Things\" mode. Click both hands in air to move stacks (shaking shuffles).", -700, 1f, 50, 0.7f);
			break;
		case "removeOriginalWhenGrabbed_help":
			base.ToggleHelpLabel("A holdable normally leaves a duplicate when grabbed, but this will remove the original for the lifetime of the grabbed object", -700, 1f, 50, 0.7f);
			break;
		case "persistWhenThrownOrEmitted_help":
			base.ToggleHelpLabel("By default, thrown or emitted objects self-destroy after some seconds. This option keeps them around permanently until the \"destroy all parts\" command is used (or everyone left the area)", -700, 1f, 50, 0.7f);
			break;
		case "invisible_help":
			base.ToggleHelpLabel("Makes this whole thing disappear when saved. Use Me-setting \"See invisible\" to always see them.", -700, 1f, 50, 0.7f);
			break;
		case "uncollidable_help":
			base.ToggleHelpLabel("Disables collisions for this whole thing, except where \"Dedicated collider\" is used.", -700, 1f, 50, 0.7f);
			break;
		case "editCreationDescription":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (text != null)
				{
					Thing component = CreationHelper.thingBeingEdited.GetComponent<Thing>();
					component.description = text;
				}
				base.SwitchTo(DialogType.ThingAttributes, string.Empty);
			}, contextName, this.thing.description, 225, string.Empty, true, true, false, true, 1f, false, false, null, true);
			break;
		case "previousPage":
		{
			base.HideHelpLabel();
			int num = --this.currentPage;
			if (num < 1)
			{
				this.currentPage = this.maxPages;
			}
			flag = true;
			break;
		}
		case "nextPage":
		{
			base.HideHelpLabel();
			int num = ++this.currentPage;
			if (num > this.maxPages)
			{
				this.currentPage = 1;
			}
			flag = true;
			break;
		}
		case "more":
			base.SwitchTo(DialogType.ThingAttributesMore, string.Empty);
			break;
		case "splitOffParts":
			this.SplitOffParts();
			break;
		case "physics":
			base.SwitchTo(DialogType.ThingPhysics, string.Empty);
			break;
		case "back":
			base.SwitchTo(DialogType.Create, string.Empty);
			break;
		}
		if (flag)
		{
			this.AddAttributes();
		}
	}

	// Token: 0x04000904 RID: 2308
	private Thing thing;

	// Token: 0x04000905 RID: 2309
	private int currentPage = 1;

	// Token: 0x04000906 RID: 2310
	private int maxPages = 3;

	// Token: 0x04000907 RID: 2311
	private List<GameObject> attributeButtons = new List<GameObject>();
}
