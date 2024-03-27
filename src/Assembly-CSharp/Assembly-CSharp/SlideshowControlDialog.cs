using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class SlideshowControlDialog : Dialog
{
	// Token: 0x06000B8A RID: 2954 RVA: 0x00060A08 File Offset: 0x0005EE08
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		if (this.thingPart == null)
		{
			this.thingPart = this.GetClosestThingPartWithSlideshow();
		}
		if (this.thingPart != null)
		{
			this.slideshow = this.GetOrAddSlideshow();
			bool flag = !string.IsNullOrEmpty(this.slideshow.searchText);
			base.AddModelButton("Find", "search", null, -410, -410, false);
			string text = ((!flag) ? "Find slideshow images..." : this.slideshow.searchText);
			text = Misc.Truncate(text, 30, true);
			base.AddLabel(text, -320, -460, 1.1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			if (flag)
			{
				this.UpdateDisplay();
			}
		}
		else
		{
			string text2 = "No slideshow screen found here.";
			if (Managers.areaManager.weAreEditorOfCurrentArea)
			{
				text2 += " Please create a thing with a part that uses the \"...\" setting \"Slideshow screen\".";
			}
			Managers.dialogManager.ShowInfo(text2, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
		}
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x00060B30 File Offset: 0x0005EF30
	private ThingPart GetClosestThingPartWithSlideshow()
	{
		ThingPart thingPart = null;
		float? num = null;
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart2 in componentsInChildren)
		{
			if (thingPart2.offersSlideshowScreen)
			{
				float num2 = Vector3.Distance(thingPart2.transform.position, this.transform.position);
				if (num == null || num2 < num)
				{
					num = new float?(num2);
					thingPart = thingPart2;
				}
			}
		}
		return thingPart;
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00060BF0 File Offset: 0x0005EFF0
	private void UpdateDisplay()
	{
		Misc.DestroyMultiple(new GameObject[] { this.playPauseButton, this.stopButton });
		if (this.slideshow != null && this.slideshow.HasUrls())
		{
			this.playPauseIconOnPlay = !this.slideshow.running;
			this.playPauseButton = base.AddButton("playPause", null, string.Empty, "ButtonSmall", 0, 0, (!this.playPauseIconOnPlay) ? "pause" : "playSquare", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			this.stopButton = base.AddButton("stop", null, "stop", "ButtonCompactSmallIcon", 0, 420, "stop", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x00060CF0 File Offset: 0x0005F0F0
	private new void Update()
	{
		if (this.thingPart == null || this.slideshow == null)
		{
			base.CloseDialog();
		}
		else
		{
			bool flag = this.slideshow.HasUrls();
			if ((flag && (this.stopButton == null || this.playPauseButton == null)) || (!flag && (this.stopButton != null || this.playPauseButton != null)) || this.playPauseIconOnPlay == this.slideshow.running)
			{
				this.UpdateDisplay();
			}
		}
		base.ReactToOnClick();
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00060DA8 File Offset: 0x0005F1A8
	private ThingPartSlideshow GetOrAddSlideshow()
	{
		ThingPartSlideshow thingPartSlideshow = this.thingPart.GetComponent<ThingPartSlideshow>();
		if (thingPartSlideshow == null)
		{
			thingPartSlideshow = this.thingPart.gameObject.AddComponent<ThingPartSlideshow>();
		}
		return thingPartSlideshow;
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00060DE0 File Offset: 0x0005F1E0
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		Our.SetPreferentialHandSide(this.hand);
		if (contextName != null)
		{
			if (!(contextName == "search"))
			{
				if (!(contextName == "playPause"))
				{
					if (!(contextName == "stop"))
					{
						if (contextName == "close")
						{
							base.CloseDialog();
						}
					}
					else
					{
						Managers.personManager.DoSlideshowControl_Stop(this.thingPart);
						base.CloseDialog();
					}
				}
				else if (this.playPauseIconOnPlay)
				{
					Managers.personManager.DoSlideshowControl_Play(this.thingPart);
				}
				else
				{
					Managers.personManager.DoSlideshowControl_Pause(this.thingPart);
				}
			}
			else
			{
				Managers.dialogManager.GetInput(delegate(string text)
				{
					if (!string.IsNullOrEmpty(text) && this.thingPart != null)
					{
						ThingPartSlideshow orAddSlideshow = this.GetOrAddSlideshow();
						orAddSlideshow.LoadSearchResultsAndTriggerPlay(text);
					}
					GameObject gameObject = base.SwitchTo(DialogType.SlideshowControl, string.Empty);
					SlideshowControlDialog component = gameObject.GetComponent<SlideshowControlDialog>();
					component.thingPart = this.thingPart;
				}, contextName, string.Empty, 200, string.Empty, false, false, false, false, 1f, false, false, null, false);
			}
		}
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x00060ED8 File Offset: 0x0005F2D8
	private void OnDestroy()
	{
		ThingPartSlideshow component = this.thingPart.GetComponent<ThingPartSlideshow>();
		if (component != null && !component.HasUrls())
		{
			global::UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x040008C0 RID: 2240
	public ThingPart thingPart;

	// Token: 0x040008C1 RID: 2241
	private ThingPartSlideshow slideshow;

	// Token: 0x040008C2 RID: 2242
	private GameObject playPauseButton;

	// Token: 0x040008C3 RID: 2243
	private GameObject stopButton;

	// Token: 0x040008C4 RID: 2244
	private bool playPauseIconOnPlay;
}
