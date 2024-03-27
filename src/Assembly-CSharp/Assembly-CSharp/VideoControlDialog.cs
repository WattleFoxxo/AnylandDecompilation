using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000130 RID: 304
public class VideoControlDialog : Dialog
{
	// Token: 0x06000B79 RID: 2937 RVA: 0x0005FAC0 File Offset: 0x0005DEC0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		if (!Managers.areaManager.onlyEditorsSetScreenContent || Managers.areaManager.weAreEditorOfCurrentArea)
		{
			if (VideoControlDialog.currentPageToPersist != 0)
			{
				this.currentPage = VideoControlDialog.currentPageToPersist;
				VideoControlDialog.currentPageToPersist = 0;
			}
			else
			{
				this.SetCurrentPageByCurrentlyPlayingVideo();
			}
			this.UpdateSearchResultsDisplay();
			this.UpdateVideoInfoDisplay();
		}
		else
		{
			base.AddLabel(Misc.WrapWithNewlines("Currently only editors can change videos & volume", 30, -1), -350, -150, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		this.AddBacksideCopyButton();
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0005FB74 File Offset: 0x0005DF74
	private void AddBacksideCopyButton()
	{
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		base.AddLabel("Current Video", 0, 0, 1f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("copyCurrentVideoUrl", null, "Copy URL", "ButtonCompactNoIcon", 0, 140, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		if (Universe.features.webBrowsing)
		{
			bool? webBrowsing = Managers.areaManager.rights.webBrowsing;
			if (webBrowsing.Value)
			{
				base.AddButton("openCurrentVideoInBrowser", null, "➜ Open in Browser", "ButtonCompactNoIcon", 0, 270, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
		}
		base.AddLabel("Ctrl+V to paste YouTube URL", 0, -240, 0.8f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0005FC94 File Offset: 0x0005E094
	private void UpdateSearchResultsDisplay()
	{
		this.resultCount = Managers.videoManager.searchResultIdTitles.Count;
		this.currentMaxPage = 0;
		if (this.resultCount > 0)
		{
			this.currentMaxPage = Mathf.CeilToInt((float)this.resultCount / 8f) - 1;
		}
		string searchText = Managers.videoManager.searchText;
		bool flag = searchText != null && searchText != string.Empty;
		if (this.findButton == null)
		{
			this.findButton = base.AddModelButton("Find", "findVideos", null, -410, -410, false);
			string text = ((!flag) ? "find videos..." : searchText);
			text = Misc.Truncate(text, 30, true);
			base.AddLabel(text, -320, -460, 1.1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		base.RemoveDefaultPagingButtons();
		foreach (GameObject gameObject in this.resultButtons)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.resultButtons = new List<GameObject>();
		if (this.resultCount >= 1)
		{
			base.AddSeparator(0, 280, false);
			int num = 0;
			int num2 = 0;
			int num3 = this.currentPage * 8;
			int num4 = num3 + 8;
			foreach (KeyValuePair<string, string> keyValuePair in Managers.videoManager.searchResultIdTitles)
			{
				if (num >= num3 && num <= num4)
				{
					string key = keyValuePair.Key;
					string text2 = Misc.Truncate(keyValuePair.Value.ToUpper(), 40, true);
					int num5 = 0;
					int num6 = -315 + num2 * 75;
					bool flag2 = Managers.videoManager.currentVideoId == key;
					string text3 = ((!flag2) ? "play" : "stop");
					string text4 = ((!flag2) ? "playVideo" : "stopVideo");
					GameObject gameObject2 = base.AddButton(text4, key, text2, "ButtonCompactSmallIconLong", num5, num6, text3, false, 0.8f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
					this.resultButtons.Add(gameObject2);
					if (flag2)
					{
						base.TurnButtonGreen(gameObject2);
					}
					if (++num2 >= 8)
					{
						break;
					}
				}
				num++;
			}
			if (this.currentMaxPage > 0)
			{
				base.AddDefaultPagingButtons(400, -60, "Page", true, 0, 0.85f, false);
			}
		}
		else if (flag)
		{
			base.AddLabel("nothing found for this search", -280, -150, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0005FFB0 File Offset: 0x0005E3B0
	private void SetCurrentPageByCurrentlyPlayingVideo()
	{
		if (!string.IsNullOrEmpty(Managers.videoManager.currentVideoId))
		{
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<string, string> keyValuePair in Managers.videoManager.searchResultIdTitles)
			{
				if (Managers.videoManager.currentVideoId == keyValuePair.Key)
				{
					break;
				}
				if (++num >= 8)
				{
					num = 0;
					num2++;
				}
			}
			this.currentPage = num2;
		}
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0006005C File Offset: 0x0005E45C
	private void UpdateVideoInfoDisplay()
	{
		string currentVideoId = Managers.videoManager.currentVideoId;
		string text = Managers.videoManager.currentVideoTitle;
		if (currentVideoId != null && text != null)
		{
			int num = 310;
			text = Misc.Truncate(text, 39, true);
			base.AddLabel(text, -445, num, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			num += 100;
			base.AddButton("stopVideo", null, "stop", "ButtonCompactSmallIcon", -225, num, "stop", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			string currentVideoStartedByPersonId = Managers.videoManager.currentVideoStartedByPersonId;
			string text2 = ((!(currentVideoStartedByPersonId == Managers.personManager.ourPerson.userId)) ? Managers.videoManager.currentVideoStartedByPersonName : "me");
			text2 = Managers.personManager.GetScreenNameWithDiscloser(currentVideoStartedByPersonId, text2);
			string text3 = "played by " + text2;
			text3 = Misc.Truncate(text3, 25, true);
			base.AddButton("userInfo", currentVideoStartedByPersonId, text3, "ButtonCompactNoIcon", 225, num, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		else
		{
			this.lastVolumeShown = -1f;
			if (this.volumeButton != null)
			{
				global::UnityEngine.Object.Destroy(this.volumeButton);
			}
		}
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x000601CC File Offset: 0x0005E5CC
	private void UpdateVolumeButton()
	{
		this.lastVolumeShown = Managers.videoManager.volume;
		if (this.volumeButton != null)
		{
			global::UnityEngine.Object.Destroy(this.volumeButton);
		}
		string volumeButtonIcon = Managers.videoManager.GetVolumeButtonIcon(Managers.videoManager.volume);
		this.volumeButton = base.AddButton("toggleVolume", null, string.Empty, "ButtonCompact", 530, 330, volumeButtonIcon, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.MinifyVolumeButton(this.volumeButton);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00060270 File Offset: 0x0005E670
	private new void Update()
	{
		if (Managers.videoManager.currentVideoId != null && Managers.videoManager.volume != this.lastVolumeShown)
		{
			this.UpdateVolumeButton();
		}
		this.HandleVideoURLPaste();
		base.ReactToOnClick();
		base.ReactToOnClickInWrapper(this.backsideWrapper);
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x000602C0 File Offset: 0x0005E6C0
	private void HandleVideoURLPaste()
	{
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.V))
		{
			string systemCopyBuffer = GUIUtility.systemCopyBuffer;
			if (!string.IsNullOrEmpty(systemCopyBuffer))
			{
				int num = systemCopyBuffer.IndexOf("/watch?v=");
				if (num >= 0)
				{
					string text = systemCopyBuffer.Substring(num + "/watch?v=".Length);
					if (!Managers.videoManager.WePlayVideoAtNearestThingPartScreen(text, "Pasted Video", null, null))
					{
						Managers.soundManager.Play("no", this.transform, 1f, false, false);
					}
					base.SwitchTo(DialogType.VideoControl, string.Empty);
				}
			}
		}
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x0006035D File Offset: 0x0005E75D
	private bool GetCurrentVideoUrl(out string url)
	{
		url = null;
		if (!string.IsNullOrEmpty(Managers.videoManager.currentVideoId))
		{
			url = "https://www.youtube.com/watch?v=" + Managers.videoManager.currentVideoId;
		}
		return url != null;
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x00060394 File Offset: 0x0005E794
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		Our.SetPreferentialHandSide(this.hand);
		switch (contextName)
		{
		case "playVideo":
			if (!Managers.videoManager.WePlayVideoAtNearestThingPartScreen(contextId, Managers.videoManager.searchResultIdTitles[contextId], null, null))
			{
				Managers.soundManager.Play("no", this.transform, 1f, false, false);
			}
			base.SwitchTo(DialogType.VideoControl, string.Empty);
			break;
		case "stopVideo":
			Managers.videoManager.StopVideo(true);
			VideoControlDialog.currentPageToPersist = this.currentPage;
			base.SwitchTo(DialogType.VideoControl, string.Empty);
			break;
		case "previousPage":
		{
			int num = --this.currentPage;
			if (num < 0)
			{
				this.currentPage = this.currentMaxPage;
			}
			this.UpdateSearchResultsDisplay();
			break;
		}
		case "nextPage":
		{
			int num = ++this.currentPage;
			if (num > this.currentMaxPage)
			{
				this.currentPage = 0;
			}
			this.UpdateSearchResultsDisplay();
			break;
		}
		case "findVideos":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (Managers.videoManager.searchText != text)
				{
					Managers.videoManager.searchResultIdTitles = new Dictionary<string, string>();
				}
				Managers.videoManager.searchText = text;
				if (text == null || string.IsNullOrEmpty(Managers.videoManager.searchText))
				{
					base.SwitchTo(DialogType.VideoControl, string.Empty);
					Managers.videoManager.searchText = null;
				}
				else
				{
					Managers.videoManager.LoadSearchResults();
					base.CloseDialog();
				}
			}, contextName, string.Empty, 100, "find youtube videos", false, false, false, false, 1f, false, false, null, false);
			break;
		case "userInfo":
			Our.personIdOfInterest = contextId;
			this.hand.lastContextInfoHit = null;
			Our.dialogToGoBackTo = DialogType.VideoControl;
			base.SwitchTo(DialogType.Profile, string.Empty);
			break;
		case "toggleVolume":
		{
			float toggleVolumeViaButton = Managers.videoManager.GetToggleVolumeViaButton(Managers.videoManager.volume);
			Managers.personManager.DoSetVideoVolume(toggleVolumeViaButton);
			break;
		}
		case "copyCurrentVideoUrl":
		{
			string text3;
			if (this.GetCurrentVideoUrl(out text3))
			{
				GUIUtility.systemCopyBuffer = text3;
				Managers.soundManager.Play("success", this.transform, 0.3f, false, false);
			}
			break;
		}
		case "openCurrentVideoInBrowser":
		{
			string text2;
			if (this.GetCurrentVideoUrl(out text2))
			{
				ThingPart currentScreenThingPart = Managers.videoManager.GetCurrentScreenThingPart();
				if (currentScreenThingPart != null)
				{
					BrowserSettings browserSettings = new BrowserSettings();
					browserSettings.url = text2;
					if (Managers.browserManager.TryAttachBrowser(currentScreenThingPart, browserSettings, false) != null)
					{
						Managers.soundManager.Play("success", this.transform, 0.3f, false, false);
					}
				}
			}
			break;
		}
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x040008AF RID: 2223
	private GameObject volumeButton;

	// Token: 0x040008B0 RID: 2224
	private float lastVolumeShown = -1f;

	// Token: 0x040008B1 RID: 2225
	private const int maxResultsPerPage = 8;

	// Token: 0x040008B2 RID: 2226
	private int currentPage;

	// Token: 0x040008B3 RID: 2227
	private int currentMaxPage;

	// Token: 0x040008B4 RID: 2228
	private int resultCount;

	// Token: 0x040008B5 RID: 2229
	private GameObject findButton;

	// Token: 0x040008B6 RID: 2230
	public static int currentPageToPersist;

	// Token: 0x040008B7 RID: 2231
	private List<GameObject> resultButtons = new List<GameObject>();
}
