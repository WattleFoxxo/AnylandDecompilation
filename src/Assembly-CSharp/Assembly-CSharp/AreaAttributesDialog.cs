using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class AreaAttributesDialog : Dialog
{
	// Token: 0x060008E2 RID: 2274 RVA: 0x000340B4 File Offset: 0x000324B4
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		if (Managers.areaManager.weAreEditorOfCurrentArea)
		{
			this.maxPages = 2;
			base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
		}
		this.AddAttributes();
		this.AddMoreButtons();
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003411C File Offset: 0x0003251C
	private void AddMoreButtons()
	{
		base.MinifyMoreButton(base.AddButton("filters", null, "Filters", "ButtonCompactNoIconShortCentered", -355, 400, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
		base.MinifyMoreButton(base.AddButton("rights", null, "Rights", "ButtonCompactNoIconShortCentered", 355, 400, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x000341B8 File Offset: 0x000325B8
	private void AddAttributes()
	{
		this.RemoveAttributes();
		int num = -420;
		int num2 = 0;
		if (this.page == 0)
		{
			this.attributeButtons.Add(base.AddCheckbox("isPrivate", null, "Private", 0, num + num2 * 115, Managers.areaManager.isPrivate, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Private));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isPrivate_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("isZeroGravity", null, "Zero gravity", 0, num + num2 * 115, Managers.areaManager.isZeroGravity, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.ZeroGravity));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isZeroGravity_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("hasFloatingDust", null, "Floating dust", 0, num + num2 * 115, Managers.areaManager.hasFloatingDust, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.FloatingDust));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("hasFloatingDust_help", num + num2 * 115));
			num2++;
			if (Managers.areaManager.weAreOwnerOfCurrentArea)
			{
				this.attributeButtons.Add(base.AddCheckbox("isCopyable", null, "Copyable", 0, num + num2 * 115, Managers.areaManager.isCopyable, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Copyable));
				this.attributeButtons.Add(base.AddCheckboxHelpButton("isCopyable_help", num + num2 * 115));
				num2++;
				this.attributeButtons.Add(base.AddCheckbox("onlyOwnerSetsLocks", null, "Only owner locks things", 0, num + num2 * 115, Managers.areaManager.onlyOwnerSetsLocks, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
				this.attributeButtons.Add(base.AddCheckboxHelpButton("onlyOwnerSetsLocks_help", num + num2 * 115));
				num2++;
			}
			if (Managers.areaManager.weAreOwnerOfCurrentArea && !Managers.areaManager.currentAreaIsHomeArea)
			{
				this.attributeButtons.Add(base.AddCheckbox("setHomeArea", null, "Make my home", 0, num + num2 * 115, Managers.areaManager.currentAreaIsHomeArea, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
				num2++;
			}
		}
		else if (this.page == 1 && Managers.areaManager.weAreEditorOfCurrentArea)
		{
			this.attributeButtons.Add(base.AddCheckbox("isExcluded", null, "Unlisted", 0, num + num2 * 115, Managers.areaManager.isExcluded, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Invisible));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isExcluded_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("onlyEditorsSetScreenContent", null, "Only editors change screens", 0, num + num2 * 115, Managers.areaManager.onlyEditorsSetScreenContent, 0.65f, "Checkbox", TextColor.Default, "temporarily", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("onlyEditorsSetScreenContent_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("onlyEditorsCanUseInventory", null, "Only editors use inventory", 0, num + num2 * 115, Managers.areaManager.onlyEditorsCanUseInventory, 0.65f, "Checkbox", TextColor.Default, "temporarily", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("onlyEditorsCanUseInventory_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("sunOmitsShadow", null, "Sun omits shadow", 0, num + num2 * 115, Managers.areaManager.sunOmitsShadow, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("sunOmitsShadow_help", num + num2 * 115));
			num2++;
		}
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x000345AC File Offset: 0x000329AC
	private void RemoveAttributes()
	{
		foreach (GameObject gameObject in this.attributeButtons)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.attributeButtons = new List<GameObject>();
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00034614 File Offset: 0x00032A14
	private void RefreshTextures()
	{
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart in componentsInChildren)
		{
			thingPart.UpdateTextures(true);
		}
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00034664 File Offset: 0x00032A64
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "previousPage":
		{
			int num = --this.page;
			if (num < 0)
			{
				this.page = this.maxPages - 1;
			}
			this.AddAttributes();
			break;
		}
		case "nextPage":
		{
			int num = ++this.page;
			if (num > this.maxPages - 1)
			{
				this.page = 0;
			}
			this.AddAttributes();
			break;
		}
		case "isPrivate":
			Managers.personManager.DoSetAreaPrivate(state);
			break;
		case "isZeroGravity":
			Managers.broadcastNetworkManager.UpdatePhotonCustomRoomProperty("gravity", string.Empty);
			Managers.personManager.DoSetAreaZeroGravity(state);
			break;
		case "hasFloatingDust":
			Managers.personManager.DoSetAreaFloatingDust(state);
			break;
		case "isCopyable":
			Managers.personManager.DoSetAreaCopyable(state);
			break;
		case "onlyOwnerSetsLocks":
			Managers.personManager.DoSetAreaOnlyOwnerSetsLocks(state);
			break;
		case "isExcluded":
			Managers.personManager.DoSetAreaIsExcluded(state);
			break;
		case "setHomeArea":
			if (!this.waitingForCallback)
			{
				this.waitingForCallback = true;
				Managers.areaManager.SetHomeArea(delegate(bool ok)
				{
					string text = ((!ok) ? "no" : "success");
					Managers.soundManager.Play(text, this.transform, 0.2f, false, false);
					this.AddAttributes();
					this.waitingForCallback = false;
				});
			}
			break;
		case "onlyEditorsSetScreenContent":
			Managers.areaManager.onlyEditorsSetScreenContent = !Managers.areaManager.onlyEditorsSetScreenContent;
			break;
		case "onlyEditorsCanUseInventory":
			Managers.areaManager.onlyEditorsCanUseInventory = !Managers.areaManager.onlyEditorsCanUseInventory;
			break;
		case "sunOmitsShadow":
			Managers.areaManager.sunOmitsShadow = state;
			if (!this.waitingForCallback)
			{
				this.waitingForCallback = true;
				Managers.areaManager.SaveSettings(delegate(bool ok)
				{
					Managers.areaManager.sunOmitsShadow = ((!ok) ? (!state) : state);
					Managers.areaManager.ApplySunOmitsShadow();
					string text2 = ((!ok) ? "no" : "success");
					Managers.soundManager.Play(text2, this.transform, 0.2f, false, false);
					this.waitingForCallback = false;
				});
			}
			break;
		case "isPrivate_help":
			base.ToggleHelpLabel("Only the area's editors or (temporarily) people you ping can join private areas. They will also not show in the areas dialog default suggestions, the friends list, or non-editor search results.", -700, 1f, 50, 0.7f);
			break;
		case "isZeroGravity_help":
			base.ToggleHelpLabel("Thrown things float, and people can teleport up walls or into air", -700, 1f, 50, 0.7f);
			break;
		case "onlyEditorsSetScreenContent_help":
			base.ToggleHelpLabel("Until everyone left, only editors can access video search & controls, or change the content of web screens", -700, 1f, 50, 0.7f);
			break;
		case "onlyEditorsCanUseInventory_help":
			base.ToggleHelpLabel("Until everyone left, only editors can take items out of their backpack inventory. Everyone can still use holdables that were placed in the area.", -700, 1f, 50, 0.7f);
			break;
		case "sunOmitsShadow_help":
			base.ToggleHelpLabel("The area light will not make objects cast a shadow if set. This can help optimize area display speed.", -700, 1f, 50, 0.7f);
			break;
		case "isCopyable_help":
			base.ToggleHelpLabel("Allow others to copy & paste all area placements into their own area. Credit will be given in the area dialog", -700, 1f, 50, 0.7f);
			break;
		case "onlyOwnerSetsLocks_help":
			base.ToggleHelpLabel("Will only allow you as original creator of the area to add or remove placement locks, which disallow moving or deleting a thing", -700, 1f, 50, 0.7f);
			break;
		case "isExcluded_help":
			base.ToggleHelpLabel("Will not list the area in the areas dialog default suggestions, search results, friends list and so on (while still allowing non-editor visits via e.g. teleports). Note Unlisting isn't needed if the area is already set to Private.", -700, 1f, 50, 0.7f);
			break;
		case "hasFloatingDust_help":
			base.ToggleHelpLabel("Adds floating particles swirling around in the area", -700, 1f, 50, 0.7f);
			break;
		case "filters":
			base.SwitchTo(DialogType.AreaFilters, string.Empty);
			break;
		case "rights":
			base.SwitchTo(DialogType.AreaRights, string.Empty);
			break;
		case "back":
			base.SwitchTo(DialogType.Area, string.Empty);
			break;
		}
	}

	// Token: 0x040006A6 RID: 1702
	private int page;

	// Token: 0x040006A7 RID: 1703
	private int maxPages = 1;

	// Token: 0x040006A8 RID: 1704
	private List<GameObject> attributeButtons = new List<GameObject>();

	// Token: 0x040006A9 RID: 1705
	private bool waitingForCallback;
}
