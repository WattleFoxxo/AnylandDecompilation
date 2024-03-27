using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class ThingAttributesMoreDialog : Dialog
{
	// Token: 0x06000BF4 RID: 3060 RVA: 0x00066950 File Offset: 0x00064D50
	public void Start()
	{
		this.thing = CreationHelper.thingBeingEdited.GetComponent<Thing>();
		base.Init(base.gameObject, false, false, true);
		base.AddFundament();
		base.AddBackButton();
		base.AddSideHeadline("More");
		this.AddAttributes();
		base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x000669B8 File Offset: 0x00064DB8
	private void AddAttributes()
	{
		this.RemoveAttributes();
		int num = -420;
		int num2 = 0;
		if (this.currentPage == 1)
		{
			this.attributeButtons.Add(base.AddCheckbox("replaceInstancesInArea", null, "Replace same in area", 0, num + num2 * 115, this.thing.replaceInstancesInArea, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.ReplaceInstances));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("replaceInstancesInArea_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("replaceInstancesInAreaOneTime", null, "... One-Time", 0, num + num2 * 115, CreationHelper.replaceInstancesInAreaOneTime, 1f, "Checkbox", TextColor.Gray, null, ExtraIcon.ReplaceInstancesOneTime));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("replaceInstancesInAreaOneTime_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("invisibleToUsWhenAttached", null, "Invisible to me when worn", 0, num + num2 * 115, this.thing.invisibleToUsWhenAttached, 0.85f, "Checkbox", TextColor.Default, null, ExtraIcon.Invisible));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("invisibleToUsWhenAttached_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("isPassable", null, "Can pass through", 0, num + num2 * 115, this.thing.isPassable, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isPassable_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("keepPreciseCollider", null, "Always precise collision", 0, num + num2 * 115, this.thing.keepPreciseCollider, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("keepPreciseCollider_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("snapAllPartsToGrid", null, "Snap parts to grid", 0, num + num2 * 115, this.thing.snapAllPartsToGrid, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("snapAllPartsToGrid_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 2)
		{
			this.attributeButtons.Add(base.AddCheckbox("addBodyWhenAttachedNonClearing", null, "Add current body", 0, num + num2 * 115, this.thing.addBodyWhenAttachedNonClearing, 0.8f, "Checkbox", TextColor.Default, "non-clearing", ExtraIcon.AddBody));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("addBodyWhenAttachedNonClearing_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("avoidCastShadow", null, "Doesn't cast shadow", 0, num + num2 * 115, this.thing.avoidCastShadow, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.AvoidCastShadow));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("avoidCastShadow_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("avoidReceiveShadow", null, "Doesn't receive shadow", 0, num + num2 * 115, this.thing.avoidReceiveShadow, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.AvoidReceiveShadow));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("avoidReceiveShadow_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("omitAutoSounds", null, "Avoid auto sounds", 0, num + num2 * 115, this.thing.omitAutoSounds, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("omitAutoSounds_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("omitAutoHapticFeedback", null, "Avoid auto haptic feedback", 0, num + num2 * 115, this.thing.omitAutoHapticFeedback, 0.95f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("omitAutoHapticFeedback_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("floatsOnLiquid", null, "Floats on liquid", 0, num + num2 * 115, this.thing.floatsOnLiquid, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("floatsOnLiquid_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 3)
		{
			this.attributeButtons.Add(base.AddCheckbox("keepSizeInInventory", null, "Keep size in inventory", 0, num + num2 * 115, this.thing.keepSizeInInventory, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("keepSizeInInventory_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("activeEvenInInventory", null, "Active in inventory", 0, num + num2 * 115, this.thing.activeEvenInInventory, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("activeEvenInInventory_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("invisibleToDesktopCamera", null, "Invisible to cameras", 0, num + num2 * 115, this.thing.invisibleToDesktopCamera, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("invisibleToDesktopCamera_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("personalExperience", null, "Personal experience", 0, num + num2 * 115, this.thing.personalExperience, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("personalExperience_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("doAlwaysMergeParts", null, "Also merge parts that may cause issues", 0, num + num2 * 115, this.thing.doAlwaysMergeParts, 0.67f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("doAlwaysMergeParts_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("isNeverClonable", null, "Never clonable", 0, num + num2 * 115, this.thing.isNeverClonable, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.NeverClonable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isNeverClonable_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 4)
		{
			this.attributeButtons.Add(base.AddLabel(string.Concat(new string[]
			{
				"Ctrl+V on Create Dialog:",
				Environment.NewLine,
				"Paste reference image urls or",
				Environment.NewLine,
				"local image/ *.obj paths"
			}), 0, 0, 0.85f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleCenter).gameObject);
		}
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x000670FC File Offset: 0x000654FC
	private void RemoveAttributes()
	{
		foreach (GameObject gameObject in this.attributeButtons)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.attributeButtons = new List<GameObject>();
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00067164 File Offset: 0x00065564
	private void AddMoreButtons()
	{
		base.MinifyMoreButton(base.AddButton("physics", null, "Physics", "ButtonCompactNoIconShortCentered", -355, 400, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x000671B8 File Offset: 0x000655B8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		bool flag = false;
		switch (contextName)
		{
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
		case "replaceInstancesInArea":
			this.thing.replaceInstancesInArea = state;
			if (state && CreationHelper.replaceInstancesInAreaOneTime)
			{
				CreationHelper.replaceInstancesInAreaOneTime = false;
				flag = true;
			}
			break;
		case "replaceInstancesInAreaOneTime":
			CreationHelper.replaceInstancesInAreaOneTime = state;
			if (state && this.thing.replaceInstancesInArea)
			{
				this.thing.replaceInstancesInArea = false;
				flag = true;
			}
			break;
		case "keepPreciseCollider":
			this.thing.keepPreciseCollider = state;
			if (this.thing.keepPreciseCollider && this.thing.doAlwaysMergeParts)
			{
				this.thing.doAlwaysMergeParts = false;
				flag = true;
			}
			break;
		case "doAlwaysMergeParts":
			if (state && !CreationHelper.didSeeAlwaysMergePartsConfirm)
			{
				CreationHelper.didSeeAlwaysMergePartsConfirm = true;
				string text = "Note merging is always on by default even without this setting, so you normally don't need it. This setting only adds to the default merging in regards to parts that may cause side effects, like overlapping collisions when you need precise button presses.";
				base.SwitchToConfirmDialog(text, delegate(bool didConfirm)
				{
					if (didConfirm)
					{
						this.thing.doAlwaysMergeParts = true;
						this.thing.keepPreciseCollider = false;
						base.SwitchTo(DialogType.Create, string.Empty);
					}
					else
					{
						base.SwitchTo(DialogType.Create, string.Empty);
					}
				});
			}
			else
			{
				this.thing.doAlwaysMergeParts = state;
			}
			break;
		case "isNeverClonable":
			this.thing.isNeverClonable = state;
			if (this.thing.isNeverClonable)
			{
				this.thing.isClonable = false;
			}
			break;
		case "snapAllPartsToGrid":
			this.thing.snapAllPartsToGrid = state;
			break;
		case "avoidCastShadow":
			this.thing.avoidCastShadow = state;
			break;
		case "avoidReceiveShadow":
			this.thing.avoidReceiveShadow = state;
			break;
		case "omitAutoSounds":
			this.thing.omitAutoSounds = state;
			break;
		case "omitAutoHapticFeedback":
			this.thing.omitAutoHapticFeedback = state;
			break;
		case "keepSizeInInventory":
			this.thing.keepSizeInInventory = state;
			break;
		case "activeEvenInInventory":
			this.thing.activeEvenInInventory = state;
			break;
		case "invisibleToUsWhenAttached":
			this.thing.invisibleToUsWhenAttached = state;
			break;
		case "isPassable":
			this.thing.isPassable = state;
			break;
		case "floatsOnLiquid":
			this.thing.floatsOnLiquid = state;
			break;
		case "invisibleToDesktopCamera":
			this.thing.invisibleToDesktopCamera = state;
			break;
		case "personalExperience":
			this.thing.personalExperience = state;
			break;
		case "addBodyWhenAttachedNonClearing":
			this.thing.addBodyWhenAttachedNonClearing = state;
			if (state && this.thing.addBodyWhenAttached)
			{
				this.thing.addBodyWhenAttached = false;
				flag = true;
			}
			break;
		case "replaceInstancesInArea_help":
			base.ToggleHelpLabel("When Done, changes all other placements of the Thing in this area to the new version", -700, 1f, 50, 0.7f);
			break;
		case "replaceInstancesInAreaOneTime_help":
			base.ToggleHelpLabel("Like \"Replace same in area\", but the setting itself will not remain selected for the saved version", -700, 1f, 50, 0.7f);
			break;
		case "invisibleToUsWhenAttached_help":
			base.ToggleHelpLabel("Makes the thing invisible (to yourself only) when you wear it. It still reacts to collisions for you. When putting it on, look into the mirror to see if it's showing for others. Heads have this behavior by default", -700, 1f, 50, 0.7f);
			break;
		case "isPassable_help":
			base.ToggleHelpLabel("Lets the teleport laser pass through (and allows moving through in desktop mode) while still able to get touch events. Also, thrown or emitted things won't collide with it. Also see attribute \"Uncollidable\".", -700, 1f, 50, 0.7f);
			break;
		case "keepPreciseCollider_help":
			base.ToggleHelpLabel("You can use this if e.g. hollow things, like chests, don't properly collide, though it makes things count against area limits faster", -700, 1f, 50, 0.7f);
			break;
		case "doAlwaysMergeParts_help":
			base.ToggleHelpLabel("Merging is enabled by default, so you normally don't need this setting. If you however find that normal rules didn't merge your creation, try this to always merge same-color parts to help optimization.", -700, 1f, 50, 0.7f);
			break;
		case "snapAllPartsToGrid_help":
			base.ToggleHelpLabel("Snaps all parts to the 0.5 meters grid. You can enable showing grid lines in the Me ➞ \"...\" settings.", -700, 1f, 50, 0.7f);
			break;
		case "addBodyWhenAttachedNonClearing_help":
			base.ToggleHelpLabel("Same as \"Add current body when worn\", but body parts you have empty now will be ignored and not clear existing parts when wearing.", -700, 1f, 50, 0.7f);
			break;
		case "avoidCastShadow_help":
			base.ToggleHelpLabel("Will have no part of this creation cast any shadow onto other objects", -700, 1f, 50, 0.7f);
			break;
		case "avoidReceiveShadow_help":
			base.ToggleHelpLabel("Will have no part of this creation receive any shadow from other objects", -700, 1f, 50, 0.7f);
			break;
		case "omitAutoSounds_help":
			base.ToggleHelpLabel("Avoids automatic sound effects for attributes like Bouncy or Shatters", -700, 1f, 50, 0.7f);
			break;
		case "omitAutoHapticFeedback_help":
			base.ToggleHelpLabel("Avoids automatic controller vibration for situations like triggering a holdable emit", -700, 1f, 50, 0.7f);
			break;
		case "floatsOnLiquid_help":
			base.ToggleHelpLabel("Treats other placed parts set as Liquid like non-Liquids. For instance, a thrown ball now bumps off of water instead of being absorbed by it. Does not affect behavior towards thrown, emitted or held liquids.", -700, 1f, 50, 0.7f);
			break;
		case "keepSizeInInventory_help":
			base.ToggleHelpLabel("Will not down or upscale from its original size when placed in inventory", -700, 1f, 50, 0.7f);
			break;
		case "benefitsFromNotShowingAtDistance_help":
			base.ToggleHelpLabel("The distance optimizers automatically deactivates placement further away from one as one moves. Some things however are exempt and always show, like when they're very big, or include Placed Sub-Things. Use this option to cancel these exemptions for the Thing.", -700, 1f, 50, 0.7f);
			break;
		case "activeEvenInInventory_help":
			base.ToggleHelpLabel("Triggers events like start even when in the inventory", -700, 1f, 50, 0.7f);
			break;
		case "invisibleToDesktopCamera_help":
			base.ToggleHelpLabel("Will not show the Thing on desktop streams of custom cameras", -700, 1f, 50, 0.7f);
			break;
		case "isNeverClonable_help":
			base.ToggleHelpLabel("Makes this item non-clonable even when your Me-settings use \"All my creations are clonable\".", -700, 1f, 50, 0.7f);
			break;
		case "personalExperience_help":
			base.ToggleHelpLabel("Scripted personal events like \"When consumed\" or \"When touched\" are not synced to others around for any parts of this Thing or its Sub-Things, appearing for that person only. Note this is also available as setting for individual Thing Parts.", -700, 1f, 50, 0.7f);
			break;
		case "back":
			base.SwitchTo(DialogType.ThingAttributes, string.Empty);
			break;
		}
		if (flag)
		{
			this.AddAttributes();
		}
	}

	// Token: 0x04000909 RID: 2313
	private Thing thing;

	// Token: 0x0400090A RID: 2314
	private int currentPage = 1;

	// Token: 0x0400090B RID: 2315
	private int maxPages = 4;

	// Token: 0x0400090C RID: 2316
	private List<GameObject> attributeButtons = new List<GameObject>();
}
