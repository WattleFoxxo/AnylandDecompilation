using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class PlacedSubThingsDialog : Dialog
{
	// Token: 0x06000BE2 RID: 3042 RVA: 0x00064C24 File Offset: 0x00063024
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddGenericHelpButton();
		if (CreationHelper.thingPartWhoseStatesAreEdited == null)
		{
			return;
		}
		this.thingPart = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
		if (this.thingPart == null)
		{
			return;
		}
		this.thingPart.ResetStates();
		string text = "Context-laser a placement to add or remove." + Environment.NewLine + "It will be moved by this part when done.";
		base.AddLabel(text, 0, 60, 0.8f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddSubThingRelatedInterface(this.thingPart);
		this.UpdatePlacedSubThingsInfo();
		base.AddButton("switchToIncludedSubThings", null, "Included Sub-Things...", "ButtonCompactNoIcon", 0, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, true, false);
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x00064D08 File Offset: 0x00063108
	private new void Update()
	{
		if (CreationHelper.thingPartWhoseStatesAreEdited == null)
		{
			this.Close();
		}
		base.ReactToOnClick();
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x00064D28 File Offset: 0x00063128
	public void UpdatePlacedSubThingsInfo()
	{
		if (this.currentInfo != null)
		{
			global::UnityEngine.Object.Destroy(this.currentInfo);
		}
		int count = this.thingPart.placedSubThingIdsWithOriginalInfo.Count;
		TextColor textColor = ((count < 1) ? TextColor.Default : TextColor.Green);
		string text = "Current: " + count;
		if (count >= 100)
		{
			text += " (limit reached)";
		}
		string text2 = text;
		int num = 0;
		int num2 = -110;
		TextColor textColor2 = textColor;
		this.currentInfo = base.AddLabel(text2, num, num2, 1f, false, textColor2, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00064DC8 File Offset: 0x000631C8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "help":
			global::UnityEngine.Object.Destroy(this.helpVideoButton);
			if (base.ToggleCompactHelpLabel("Sub-things can be other placements in this area, which are then moved by this part (and pass \"when any part\" events to the other script)."))
			{
				this.helpVideoButton = base.AddButton("helpVideo", null, "Help Video", "ButtonCompactNoIconShortCentered", 0, -560, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
			break;
		case "helpVideo":
			Managers.browserManager.OpenGuideBrowser("yjXGqa9pkuE", null);
			break;
		case "invisible":
			this.thingPart.invisible = state;
			break;
		case "subThingsFollowDelayed":
			this.thingPart.subThingsFollowDelayed = state;
			break;
		case "back":
			this.hand.lastContextInfoHit = this.thingPart.gameObject;
			base.SwitchTo(DialogType.ThingPart, string.Empty);
			break;
		case "switchToIncludedSubThings":
			base.SwitchTo(DialogType.IncludedSubThings, string.Empty);
			break;
		case "close":
			this.Close();
			break;
		}
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00064F60 File Offset: 0x00063360
	private void Close()
	{
		CreationHelper.thingPartWhoseStatesAreEdited = null;
		base.SwitchTo(DialogType.Create, string.Empty);
	}

	// Token: 0x04000900 RID: 2304
	private ThingPart thingPart;

	// Token: 0x04000901 RID: 2305
	private TextMesh currentInfo;

	// Token: 0x04000902 RID: 2306
	private GameObject helpVideoButton;
}
