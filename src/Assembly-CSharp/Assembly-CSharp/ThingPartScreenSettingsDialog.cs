using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class ThingPartScreenSettingsDialog : Dialog
{
	// Token: 0x06000C7A RID: 3194 RVA: 0x00072100 File Offset: 0x00070500
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Screen", -370, -460, TextColor.Default, TextAlignment.Left, false);
		this.thingPart = this.hand.lastContextInfoHit.GetComponent<ThingPart>();
		this.AddButtons();
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x00072164 File Offset: 0x00070564
	private void AddButtons()
	{
		int num = -250;
		int num2 = 0;
		this.videoScreenButton = base.AddCheckbox("offersScreen", null, "Is screen", 0, num + num2 * 115, this.thingPart.offersScreen, 1f, "Checkbox", TextColor.Default, "for video & web", ExtraIcon.None);
		base.AddCheckboxHelpButton("offersScreen_help", num + num2++ * 115);
		num2++;
		base.AddLabel("Video", 0, num + num2 * 115 - 15, 0.95f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		num2++;
		base.AddCheckbox("videoScreenHasSurroundSound", null, "Surround sound", 0, num + num2 * 115, this.thingPart.videoScreenHasSurroundSound, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("videoScreenHasSurroundSound_help", num + num2++ * 115);
		base.AddCheckbox("videoScreenLoops", null, "Loops", 0, num + num2 * 115, this.thingPart.videoScreenLoops, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("videoScreenLoops_help", num + num2++ * 115);
		base.AddCheckbox("videoScreenFlipsX", null, "Mirror left right", 0, num + num2 * 115, this.thingPart.videoScreenFlipsX, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("videoScreenFlipsX_help", num + num2++ * 115);
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x000722CF File Offset: 0x000706CF
	private void SetIsVideoScreen()
	{
		if (!this.thingPart.offersScreen)
		{
			this.thingPart.offersScreen = true;
			base.SetCheckboxState(this.videoScreenButton, true, false);
		}
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x000722FC File Offset: 0x000706FC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "offersScreen":
			this.thingPart.offersScreen = state;
			if (!state)
			{
				this.thingPart.videoIdToPlayAtAreaStart = null;
			}
			if (state && this.thingPart.offersSlideshowScreen)
			{
				this.thingPart.offersSlideshowScreen = false;
			}
			break;
		case "videoScreenHasSurroundSound":
			this.thingPart.videoScreenHasSurroundSound = state;
			if (state)
			{
				this.SetIsVideoScreen();
			}
			break;
		case "videoScreenLoops":
			this.thingPart.videoScreenLoops = state;
			if (state)
			{
				this.SetIsVideoScreen();
			}
			break;
		case "videoScreenFlipsX":
			this.thingPart.videoScreenFlipsX = state;
			if (state)
			{
				this.SetIsVideoScreen();
			}
			break;
		case "offersScreen_help":
			base.ToggleHelpLabel("Can be context-lasered to play videos & show web pages. Also receives nearby \"show video [url]\" and \"show web [url]\" commands.", -700, 1f, 50, 0.7f);
			break;
		case "videoScreenHasSurroundSound_help":
			base.ToggleHelpLabel("Videos played on this will be at full volume at any distance", -700, 1f, 50, 0.7f);
			break;
		case "videoScreenLoops_help":
			base.ToggleHelpLabel("Videos played (and auto-played) on this loop endlessly", -700, 1f, 50, 0.7f);
			break;
		case "videoScreenFlipsX_help":
			base.ToggleHelpLabel("Flips the camera stream or video horizontally, useful for e.g. camera streams to make mirrors", -700, 1f, 50, 0.7f);
			break;
		case "back":
			base.SwitchTo(DialogType.ThingPartAttributes, string.Empty);
			break;
		case "close":
			CreationHelper.thingPartWhoseStatesAreEdited = null;
			base.SwitchTo(DialogType.Create, string.Empty);
			break;
		}
	}

	// Token: 0x04000969 RID: 2409
	private ThingPart thingPart;

	// Token: 0x0400096A RID: 2410
	private GameObject videoScreenButton;
}
