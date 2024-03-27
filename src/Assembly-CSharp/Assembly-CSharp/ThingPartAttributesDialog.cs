using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class ThingPartAttributesDialog : Dialog
{
	// Token: 0x06000C2F RID: 3119 RVA: 0x0006BB50 File Offset: 0x00069F50
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		this.thingPart = this.hand.lastContextInfoHit.GetComponent<ThingPart>();
		if (this.thingPart.isText)
		{
			this.maxPages++;
		}
		this.AddAttributes();
		base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
		this.AddMoreButtons();
		if (ThingPartAttributesDialog.immediatelyOpenControllableDialogIfControllable && this.thingPart.HasControllableSettings())
		{
			base.SwitchTo(DialogType.Controllable, string.Empty);
		}
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x0006BBFC File Offset: 0x00069FFC
	private void AddMoreButtons()
	{
		base.MinifyMoreButton(base.AddButton("screen", null, "Screen", "ButtonCompactNoIconShortCentered", -355, 400, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
		base.MinifyMoreButton(base.AddButton("more", null, "More", "ButtonCompactNoIconShortCentered", 355, 400, null, false, 1f, TextColor.Gray, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0006BC98 File Offset: 0x0006A098
	private void AddAttributes()
	{
		this.RemoveAttributes();
		int num = -420;
		int num2 = 0;
		if (this.currentPage == 1)
		{
			this.attributeButtons.Add(base.AddCheckbox("isLiquid", null, "Liquid", 0, num + num2 * 115, this.thingPart.isLiquid, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Liquid));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isLiquid_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("scalesUniformly", null, "Scales uniformly", 0, num + num2 * 115, this.thingPart.scalesUniformly, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.ScalesUniformly));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("scalesUniformly_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("useUnsoftenedAnimations", null, "Unsoftened animations", 0, num + num2 * 115, this.thingPart.useUnsoftenedAnimations, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("useUnsoftenedAnimations_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("persistStates", null, "Persist states", 0, num + num2 * 115, this.thingPart.persistStates, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("persistStates_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("invisible", null, "Invisible", 0, num + num2 * 115, this.thingPart.invisible, 1f, "Checkbox", TextColor.Default, "when done", ExtraIcon.Invisible));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("invisible_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("uncollidable", null, "Uncollidable", 0, num + num2 * 115, this.thingPart.uncollidable, 1f, "Checkbox", TextColor.Default, "when done", ExtraIcon.Uncollidable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("uncollidable_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("isDedicatedCollider", null, "Dedicated collider", 0, num + num2 * 115, this.thingPart.isDedicatedCollider, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isDedicatedCollider_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 2)
		{
			this.attributeButtons.Add(base.AddCheckbox("useTextureAsSky", null, "Use as sky", 0, num + num2 * 115, this.thingPart.useTextureAsSky, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.UseTextureAsSky));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("useTextureAsSky_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("stretchSkydomeSeam", null, string.Empty, 0, num + num2 * 115, this.thingPart.stretchSkydomeSeam, 1.1f, "Checkbox", TextColor.Default, "Stretch sky seams", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("stretchSkydomeSeam_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("isCamera", null, "Camera", 0, num + num2 * 115, this.thingPart.isCamera, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Camera));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isCamera_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("offersSlideshowScreen", null, "Slideshow screen", 0, num + num2 * 115, this.thingPart.offersSlideshowScreen, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("offersSlideshowScreen_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("showDirection", null, "Show direction", 0, num + num2 * 115, this.thingPart.showDirectionArrowsWhenEditing, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.ShowDirection));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("showDirection_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 3)
		{
			num += 140;
			this.attributeButtons.Add(base.AddButton("makeCenter", null, "Make center", "ButtonCompact", 0, num + num2 * 115, "makeCenter", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("makeCenter_help", num + num2 * 115));
			num2++;
			num += 50;
			this.attributeButtons.Add(base.AddButton("autoContinuation", null, "Auto-Complete...", "ButtonCompact", 0, num + num2 * 115, "autoContinuation", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("autoContinuation_help", num + num2 * 115));
			num2++;
			num += 60;
			base.StartCoroutine(base.AddSymmetryButtons("hasReflectionPart", -310, num + num2 * 115, this.thingPart.hasReflectionPartSideways, this.thingPart.hasReflectionPartVertical, this.thingPart.hasReflectionPartDepth, 1));
			this.attributeButtons.Add(base.AddLabel("Symmetry", 30, num + num2 * 115 - 20, 0.9f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft).gameObject);
			this.attributeButtons.Add(base.AddCheckboxHelpButton("hasReflectionPart_help", num + num2 * 115));
		}
		else if (this.currentPage == 4 && this.thingPart.isText)
		{
			this.attributeButtons.Add(base.AddCheckbox("textAlignCenter", null, "Center-align text", 0, num + num2 * 115, this.thingPart.textAlignCenter, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("textAlignCenter_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("textAlignRight", null, "Right-align text", 0, num + num2 * 115, this.thingPart.textAlignRight, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("textAlignRight_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddButton("setLineHeight", null, "Text line height: " + this.thingPart.textLineHeight, "ButtonCompactNoIcon", 0, num + num2 * 115, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("setLineHeight_help", num + num2 * 115));
			num2++;
		}
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0006C414 File Offset: 0x0006A814
	private void RemoveAttributes()
	{
		foreach (GameObject gameObject in this.attributeButtons)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.attributeButtons = new List<GameObject>();
		base.RemoveSymmetryButtons();
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x0006C480 File Offset: 0x0006A880
	private new void Update()
	{
		base.ReactToOnClick();
		if (this.thingPart == null)
		{
			this.Close();
		}
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0006C4A0 File Offset: 0x0006A8A0
	private bool HasReflectionParts(Thing thing)
	{
		bool flag = false;
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.hasReflectionPartSideways || thingPart.hasReflectionPartVertical || thingPart.hasReflectionPartDepth)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x0006C508 File Offset: 0x0006A908
	private void ToggleHasReflectionPart(string contextName, bool state)
	{
		if (state && !Managers.thingManager.ThingPartBaseSupportsReflectionPart(this.thingPart.baseType))
		{
			base.ShowHelpLabel("Sorry, this base shape doesn't support symmetry", 50, 0.7f, TextAlignment.Left, -700, false, false, 1f, TextColor.Default);
			Managers.soundManager.Play("no", this.transform, 0.1f, false, false);
			this.AddAttributes();
		}
		else if (state && CreationHelper.thingBeingEdited.transform.childCount == 1)
		{
			base.ShowHelpLabel("This is the current center around which parts are mirrored. Please try set Symmetry for the whole Thing and drag in parts.", 50, 0.7f, TextAlignment.Left, -700, false, false, 1f, TextColor.Default);
			Managers.soundManager.Play("no", this.transform, 0.1f, false, false);
			this.AddAttributes();
		}
		else
		{
			this.thingPart.RemoveMyReflectionPartsIfNeeded();
			if (contextName != null)
			{
				if (!(contextName == "hasReflectionPartSideways"))
				{
					if (!(contextName == "hasReflectionPartVertical"))
					{
						if (contextName == "hasReflectionPartDepth")
						{
							this.thingPart.hasReflectionPartDepth = state;
						}
					}
					else
					{
						this.thingPart.hasReflectionPartVertical = state;
					}
				}
				else
				{
					this.thingPart.hasReflectionPartSideways = state;
				}
			}
			this.thingPart.CreateMyReflectionPartsIfNeeded(null);
		}
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x0006C665 File Offset: 0x0006AA65
	private void Close()
	{
		CreationHelper.thingPartWhoseStatesAreEdited = null;
		base.SwitchTo(DialogType.Create, string.Empty);
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x0006C67C File Offset: 0x0006AA7C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "scalesUniformly":
			this.thingPart.scalesUniformly = state;
			break;
		case "isLiquid":
			this.thingPart.isLiquid = state;
			break;
		case "offersSlideshowScreen":
			this.thingPart.offersSlideshowScreen = state;
			if (state && this.thingPart.offersScreen)
			{
				this.thingPart.offersScreen = false;
				this.thingPart.videoScreenHasSurroundSound = false;
			}
			break;
		case "makeCenter":
			if (this.thingPart.transform.parent != null)
			{
				Thing thing = this.thingPart.transform.parent.GetComponent<Thing>();
				bool reflectionPartsConfirmNeeded = false;
				bool replaceInstancesConfirmNeeded = false;
				if (this.HasReflectionParts(thing) && !ThingPartAttributesDialog.centeringWithReflectionPartsConfirmed)
				{
					reflectionPartsConfirmNeeded = true;
				}
				else if ((thing.replaceInstancesInArea || CreationHelper.replaceInstancesInAreaOneTime) && !ThingPartAttributesDialog.centeringWithReplaceInstancesConfirmed)
				{
					replaceInstancesConfirmNeeded = true;
				}
				else if (thing.ContainsUnremovableCenter())
				{
					Managers.soundManager.Play("no", this.transform, 1f, false, false);
				}
				else
				{
					thing.ReCenterThingBasedOnPart(this.thingPart);
					Managers.soundManager.Play("success", this.transform, 0.125f, false, false);
				}
				if (reflectionPartsConfirmNeeded || replaceInstancesConfirmNeeded)
				{
					string text = string.Empty;
					if (reflectionPartsConfirmNeeded)
					{
						text = "Recentering also shifts the mirror axis, changing symmetric parts.";
					}
					else if (replaceInstancesConfirmNeeded)
					{
						text = "Recentering when \"Replace same in area\" is on may lead to unwanted shifts in other placements.";
					}
					text += " Continue anyway?";
					text = text.Trim();
					base.SwitchToConfirmDialog(text, delegate(bool didConfirm)
					{
						if (didConfirm && thing != null && this.thingPart != null)
						{
							if (reflectionPartsConfirmNeeded)
							{
								ThingPartAttributesDialog.centeringWithReflectionPartsConfirmed = true;
							}
							else if (replaceInstancesConfirmNeeded)
							{
								ThingPartAttributesDialog.centeringWithReplaceInstancesConfirmed = true;
							}
							thing.ReCenterThingBasedOnPart(this.thingPart);
							Managers.soundManager.Play("success", this.transform, 0.125f, false, false);
						}
						GameObject gameObject = this.SwitchTo(DialogType.ThingPartAttributes, string.Empty);
						ThingPartAttributesDialog component = gameObject.GetComponent<ThingPartAttributesDialog>();
						component.currentPage = this.currentPage;
					});
				}
			}
			break;
		case "previousPage":
		{
			base.HideHelpLabel();
			int num = --this.currentPage;
			if (num < 1)
			{
				this.currentPage = this.maxPages;
			}
			this.AddAttributes();
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
			this.AddAttributes();
			break;
		}
		case "isCamera":
			this.thingPart.isCamera = state;
			if (!this.thingPart.isCamera)
			{
				this.thingPart.isFishEyeCamera = false;
			}
			break;
		case "useUnsoftenedAnimations":
			this.thingPart.useUnsoftenedAnimations = state;
			break;
		case "persistStates":
			this.thingPart.persistStates = state;
			break;
		case "invisible":
			this.thingPart.invisible = state;
			break;
		case "uncollidable":
			this.thingPart.uncollidable = state;
			break;
		case "isDedicatedCollider":
			this.thingPart.isDedicatedCollider = state;
			if (this.thingPart.isDedicatedCollider)
			{
				if (this.thingPart.uncollidable)
				{
					this.thingPart.uncollidable = false;
				}
				Thing parentThing = this.thingPart.GetParentThing();
				parentThing.uncollidable = true;
				this.AddAttributes();
			}
			break;
		case "textAlignCenter":
			this.thingPart.textAlignCenter = state;
			if (this.thingPart.textAlignCenter && this.thingPart.textAlignRight)
			{
				this.thingPart.textAlignRight = false;
				this.AddAttributes();
			}
			this.thingPart.UpdateTextAlignmentAndMore(true);
			break;
		case "textAlignRight":
			this.thingPart.textAlignRight = state;
			if (this.thingPart.textAlignRight && this.thingPart.textAlignCenter)
			{
				this.thingPart.textAlignCenter = false;
				this.AddAttributes();
			}
			this.thingPart.UpdateTextAlignmentAndMore(true);
			break;
		case "setLineHeight":
			Managers.dialogManager.GetFloatInput(this.thingPart.textLineHeight.ToString(), "1.0 is default", delegate(float? value)
			{
				if (value != null)
				{
					this.thingPart.textLineHeight = Mathf.Clamp(value.Value, 0.01f, 100f);
					this.thingPart.UpdateTextAlignmentAndMore(true);
					base.SwitchTo(DialogType.ThingPartAttributes, string.Empty);
				}
			}, false, false);
			break;
		case "useTextureAsSky":
			this.thingPart.useTextureAsSky = state;
			if (state)
			{
				this.thingPart.invisible = false;
				this.thingPart.materialType = MaterialTypes.None;
			}
			if (!state && this.thingPart.stretchSkydomeSeam)
			{
				this.thingPart.stretchSkydomeSeam = false;
				this.AddAttributes();
			}
			this.thingPart.UpdateMaterial();
			this.thingPart.UpdateTextures(true);
			Managers.skyManager.SetThingPart(this.thingPart);
			break;
		case "stretchSkydomeSeam":
			this.thingPart.stretchSkydomeSeam = state;
			if (state && !this.thingPart.useTextureAsSky)
			{
				this.thingPart.useTextureAsSky = true;
				this.thingPart.invisible = false;
				this.AddAttributes();
			}
			this.thingPart.UpdateMaterial();
			this.thingPart.UpdateTextures(true);
			Managers.skyManager.SetThingPart(this.thingPart);
			break;
		case "hasReflectionPartSideways":
		case "hasReflectionPartVertical":
		case "hasReflectionPartDepth":
			this.ToggleHasReflectionPart(contextName, state);
			break;
		case "hasReflectionPart_help":
			base.ToggleHelpLabel("Adds a symmetrically mirrored part to this part. You can automatically enable this for newly dropped parts via the Thing's \"Symmetry\" setting.", -700, 1f, 50, 0.7f);
			break;
		case "invisible_help":
			base.ToggleHelpLabel("Makes part disappear when saved but still trigger script events. Use Me-setting \"See invisible\" to always see it.", -700, 1f, 50, 0.7f);
			break;
		case "uncollidable_help":
			base.ToggleHelpLabel("Disables collisions for this part", -700, 1f, 50, 0.7f);
			break;
		case "isDedicatedCollider_help":
			base.ToggleHelpLabel("Sets Thing to be generally Uncollidable for all parts, but ensures this part is collidable. This can optimize & speed up physics.", -700, 1f, 50, 0.7f);
			break;
		case "persistStates_help":
			base.ToggleHelpLabel("Prevents automatic reverting to the first state after 30 seconds, remaining on the set state (until everyone leaves the area)", -700, 1f, 50, 0.7f);
			break;
		case "isLiquid_help":
			base.ToggleHelpLabel("Makes this part of your creation be a liquid, like water which one can swim in. Also, holdables will disappear when thrown into liquids", -700, 1f, 50, 0.7f);
			break;
		case "scalesUniformly_help":
			base.ToggleHelpLabel("Will make this part of your creation resize proportionally to its original size. You can also enable this for all parts of the creation at once by using the \"scale each part uniformly\" option on the backside of this dialog", -700, 1f, 50, 0.7f);
			break;
		case "makeCenter_help":
			base.ToggleHelpLabel("Sets the Thing's position & rotation center to the current position & rotation of this part. You can use Thing setting \"Show center + direction\" to see the current center.", -700, 1f, 50, 0.7f);
			break;
		case "useUnsoftenedAnimations_help":
			base.ToggleHelpLabel("By default when a part transitions from one state to another, it will slightly ease out and in for a smoother movement. When enabling this option, no such smoothing is used", -700, 1f, 50, 0.7f);
			break;
		case "offersSlideshowScreen_help":
			base.ToggleHelpLabel("When later context-lasered, offers a \"Slideshow\" button to play image search results, useful for e.g. reference images while creating. You can also create a Slideshow button using \"More\" ➞ \"Slideshow button\".", -700, 1f, 50, 0.7f);
			break;
		case "isCamera_help":
			base.ToggleHelpLabel("Streams from the object to nearby Video Screens when activated, like when context-lasered. You can also create a Camera button using the \"More\" ➞ \"Camera button\".", -700, 1f, 50, 0.7f);
			break;
		case "textAlignCenter_help":
			base.ToggleHelpLabel("Use this to automatically center-align a text", -700, 1f, 50, 0.7f);
			break;
		case "textAlignRight_help":
			base.ToggleHelpLabel("Use this to automatically right-align a text", -700, 1f, 50, 0.7f);
			break;
		case "setLineHeight_help":
			base.ToggleHelpLabel("Adjusts the spacing between text rows, e.g. 0.5 to bring them closer (default is 1).", -700, 1f, 50, 0.7f);
			break;
		case "useTextureAsSky_help":
			base.ToggleHelpLabel("When placed, shows this part's material and texture layers on the sky (including animations). Use the inner glow base material to get the fullest visible background color. At most 1 placement is active per area.", -700, 1f, 50, 0.7f);
			break;
		case "stretchSkydomeSeam_help":
			base.ToggleHelpLabel("Uses a different type of sky tiling, stretching the pattern at the seams", -700, 1f, 50, 0.7f);
			break;
		case "showDirection_help":
			base.ToggleHelpLabel("Shows the Part's center and forward and upward direction when editing. For instance, this can help when the part has Sub-Things, or uses the \"tell in front\" command.", -700, 1f, 50, 0.7f);
			break;
		case "showDirection":
			this.thingPart.showDirectionArrowsWhenEditing = state;
			Managers.thingManager.UpdateShowThingPartDirectionArrows(this.thingPart.transform.parent.GetComponent<Thing>(), true);
			break;
		case "controllable":
			ThingPartAttributesDialog.immediatelyOpenControllableDialogIfControllable = true;
			base.SwitchTo(DialogType.Controllable, string.Empty);
			break;
		case "screen":
			base.SwitchTo(DialogType.ThingPartScreenSettings, string.Empty);
			break;
		case "autoContinuation":
			base.SwitchTo(DialogType.ThingPartAutoContinuation, string.Empty);
			break;
		case "more":
			base.SwitchTo(DialogType.ThingPartAttributesMore, string.Empty);
			break;
		case "back":
			base.SwitchTo(DialogType.ThingPart, string.Empty);
			break;
		}
	}

	// Token: 0x04000939 RID: 2361
	public static bool immediatelyOpenControllableDialogIfControllable;

	// Token: 0x0400093A RID: 2362
	private ThingPart thingPart;

	// Token: 0x0400093B RID: 2363
	public int currentPage = 1;

	// Token: 0x0400093C RID: 2364
	private int maxPages = 3;

	// Token: 0x0400093D RID: 2365
	private List<GameObject> attributeButtons = new List<GameObject>();

	// Token: 0x0400093E RID: 2366
	private GameObject expanderButton;

	// Token: 0x0400093F RID: 2367
	private TextMesh autoPlayLabel;

	// Token: 0x04000940 RID: 2368
	private static bool centeringWithReflectionPartsConfirmed;

	// Token: 0x04000941 RID: 2369
	private static bool centeringWithReplaceInstancesConfirmed;
}
