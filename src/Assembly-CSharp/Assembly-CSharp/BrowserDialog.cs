using System;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x0200012A RID: 298
public class BrowserDialog : Dialog
{
	// Token: 0x06000B2D RID: 2861 RVA: 0x0005BCE0 File Offset: 0x0005A0E0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		if (!(this.browser == null) && Universe.features.webBrowsing)
		{
			bool? webBrowsing = Managers.areaManager.rights.webBrowsing;
			if (webBrowsing.Value)
			{
				base.AddHeadline("Web", -370, -460, TextColor.Default, TextAlignment.Left, false);
				this.infoLabel = base.AddLabel(string.Empty, 0, 180, 0.9f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
				if (!Managers.areaManager.onlyEditorsSetScreenContent || Managers.areaManager.weAreEditorOfCurrentArea)
				{
					this.AddUrlSection();
					this.UpdateFields();
					if (this.browser.allowUrlNavigation)
					{
						base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
						this.AddJoystickButtons();
					}
				}
				else
				{
					string text = "Only editors control web screens here at the moment";
					base.AddLabel(text, 0, -50, 0.9f, false, TextColor.Default, false, TextAlignment.Center, 30, 1f, false, TextAnchor.MiddleLeft);
				}
				return;
			}
		}
		base.CloseDialog();
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0005BE18 File Offset: 0x0005A218
	private void AddUrlSection()
	{
		base.AddSeparator(0, -270, false);
		int num = -440;
		this.titleLabel = base.AddLabel(string.Empty, num, -245, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		if (this.browser.allowUrlNavigation)
		{
			base.AddModelButton("EditTextButton", "editUrl", null, -387, -170, false);
			num += 100;
		}
		this.urlLabel = base.AddLabel(string.Empty, num, -175, 0.8f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		if (this.browser.allowUrlNavigation)
		{
			GameObject gameObject = base.AddModelButton("ButtonClose", "closeBrowser", null, 430, -155, false);
			base.ScaleModelButtonWidthHeight(gameObject, 0.7f);
		}
		this.AddUrlSectionToolButtons(-170);
		base.AddSeparator(0, -60, false);
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0005BF08 File Offset: 0x0005A308
	private void AddUrlSectionToolButtons(int urlY)
	{
		TextColor textColor = TextColor.Gray;
		int num = urlY + 190;
		string text = "copyUrlToClipboard";
		string text2 = null;
		string text3 = "Copy";
		string text4 = "ButtonCompactNoIconShortCentered";
		int num2 = -355;
		int num3 = num;
		TextColor textColor2 = textColor;
		GameObject gameObject = base.AddButton(text, text2, text3, text4, num2, num3, null, false, 1f, textColor2, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.MinifyMoreButton(gameObject);
		text4 = "urlSetBy";
		text3 = null;
		text2 = "Via...";
		text = "ButtonCompactNoIconShortCentered";
		num3 = 355;
		num2 = num;
		textColor2 = textColor;
		GameObject gameObject2 = base.AddButton(text4, text3, text2, text, num3, num2, null, false, 1f, textColor2, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.MinifyMoreButton(gameObject2);
		GameObject gameObject3 = base.AddModelButton("Lock", "toggleLockInteraction", null, -135, num, false);
		DialogPart component = gameObject3.GetComponent<DialogPart>();
		component.autoStopHighlight = false;
		component.state = !this.browser.EnableInput;
		base.ScaleModelButtonWidthHeight(gameObject3, 0.75f);
		base.ApplyEmissionColorToShape(gameObject3, component.state);
		if (this.browser.allowUrlNavigation)
		{
			base.AddModelButton("ReloadButton", "reload", null, 0, num, false);
		}
		this.favoriteButton = base.AddModelButton("Favorite", "favorite", null, (!this.browser.allowUrlNavigation) ? 0 : 135, num, false);
		DialogPart component2 = this.favoriteButton.GetComponent<DialogPart>();
		component2.autoStopHighlight = false;
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0005C0A9 File Offset: 0x0005A4A9
	public void UpdateFields()
	{
		this.UpdateTitle();
		this.UpdateUrl();
		this.UpdateFavoriteButton();
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0005C0C0 File Offset: 0x0005A4C0
	private void UpdateFavoriteButton()
	{
		if (this.favoriteButton != null)
		{
			bool flag = Managers.browserManager.IsFavorite(this.browser.Url);
			DialogPart component = this.favoriteButton.GetComponent<DialogPart>();
			component.state = flag;
			base.ApplyEmissionColorToShape(this.favoriteButton, flag);
		}
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0005C114 File Offset: 0x0005A514
	private void UpdateTitle()
	{
		this.browser.EvalJS("document.title", "scripted command").Then(delegate(JSONNode title)
		{
			this.DoUpdateTitle(title);
		}).Done();
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0005C141 File Offset: 0x0005A541
	private void DoUpdateTitle(string title)
	{
		this.title = title;
		if (title == null)
		{
			title = string.Empty;
		}
		title = title.Trim();
		title = Misc.Truncate(title, 42, true);
		this.titleLabel.text = title.ToUpper();
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0005C17C File Offset: 0x0005A57C
	private void UpdateUrl()
	{
		if (this.urlLabel != null)
		{
			string text = this.browser.Url;
			this.fullUrlDisplayed = text;
			if (text == "about:blank")
			{
				text = "Page...";
			}
			else
			{
				text = Misc.RemoveFromStart(text, "http://www.");
				text = Misc.RemoveFromStart(text, "http://");
				text = Misc.WrapWithNewlines(text, 35, 2);
			}
			this.urlLabel.text = text;
		}
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0005C1F6 File Offset: 0x0005A5F6
	private new void Update()
	{
		if (this.browser == null)
		{
			base.CloseDialog();
		}
		base.Update();
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0005C218 File Offset: 0x0005A618
	private void AddJoystickButtons()
	{
		base.AddButton("joystickWasd", null, "Joystick (WASD + Space)", "ButtonBig", 0, -110, "joystickWide", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, true, false);
		base.AddButton("joystickArrowKeys", null, "Joystick (Arrows + Space)", "ButtonBig", 0, 110, "joystickWide", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, true, false);
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0005C2A4 File Offset: 0x0005A6A4
	private void SwitchToThingDialog()
	{
		GameObject gameObject = base.SwitchTo(DialogType.Thing, string.Empty);
		ThingDialog component = gameObject.GetComponent<ThingDialog>();
		component.thing = this.browser.transform.parent.GetComponent<Thing>();
		component.autoForwardIfContainsBrowser = false;
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x0005C2E8 File Offset: 0x0005A6E8
	private void ShowInfo(string s)
	{
		base.CancelInvoke("RemoveInfo");
		this.infoLabel.text = s.ToUpper();
		base.Invoke("RemoveInfo", 2.5f);
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x0005C316 File Offset: 0x0005A716
	private void RemoveInfo()
	{
		base.CancelInvoke("RemoveInfo");
		this.infoLabel.text = string.Empty;
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x0005C334 File Offset: 0x0005A734
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "previousPage":
			if (this.browser.CanGoBack)
			{
				this.browser.GoBack();
			}
			break;
		case "nextPage":
			if (this.browser.CanGoForward)
			{
				this.browser.GoForward();
			}
			break;
		case "copyUrlToClipboard":
			GUIUtility.systemCopyBuffer = this.fullUrlDisplayed;
			Managers.soundManager.Play("pickUp", this.transform, 1f, false, false);
			break;
		case "editUrl":
			Managers.dialogManager.GetInput(delegate(string urlOrSearch)
			{
				if (this.browser != null)
				{
					ThingPart browserThingPart = Managers.browserManager.GetBrowserThingPart(this.browser);
					if (browserThingPart != null)
					{
						if (Managers.browserManager.UrlIsBlockedInThisCase(browserThingPart, urlOrSearch))
						{
							Managers.browserManager.ShowBlockedUrlMessage();
						}
						else
						{
							GameObject gameObject2 = base.SwitchTo(DialogType.Browser, string.Empty);
							BrowserDialog component2 = gameObject2.GetComponent<BrowserDialog>();
							component2.browser = this.browser;
							if (urlOrSearch != null)
							{
								string url = Managers.browserManager.GetValidUrl(urlOrSearch);
								if (!url.StartsWith("http://anyland.com/gif/") && url.EndsWith(".gif"))
								{
									base.SwitchToConfirmDialog("Use gif enlarger URL?", delegate(bool didConfirm)
									{
										if (didConfirm)
										{
											url = Misc.RemoveFromStart(url, "http://");
											url = Misc.RemoveFromStart(url, "https://");
											url = "http://anyland.com/gif/?url=" + url;
										}
										Managers.personManager.DoSetBrowserUrl(this.browser, url, true, false);
										this.CloseDialog();
									});
								}
								else
								{
									Managers.personManager.DoSetBrowserUrl(this.browser, url, true, false);
								}
							}
						}
					}
				}
				else
				{
					base.CloseDialog();
				}
			}, "editBrowserUrl", (!(this.browser.Url == "about:blank")) ? this.browser.Url : string.Empty, 200, "search or url", true, true, false, true, 1f, false, false, null, false);
			break;
		case "favorite":
			if (state)
			{
				Managers.browserManager.AddFavorite(this.browser.Url, this.title);
			}
			else
			{
				Managers.browserManager.RemoveFavorite(this.browser.Url);
			}
			break;
		case "reload":
			Managers.personManager.DoReloadBrowserPage(this.browser);
			break;
		case "urlSetBy":
		{
			string text = null;
			if (this.browser.IsLoaded)
			{
				text = this.browser.GetUrlSetByPersonId(this.browser.Url);
			}
			if (text != null)
			{
				Our.personIdOfInterest = text;
				base.SwitchTo(DialogType.Profile, string.Empty);
			}
			else
			{
				Managers.soundManager.Play("no", this.transform, 1f, false, false);
			}
			break;
		}
		case "joystickWasd":
		case "joystickArrowKeys":
		{
			GameObject gameObject = base.SwitchTo(DialogType.Joystick, string.Empty);
			JoystickDialog component = gameObject.GetComponent<JoystickDialog>();
			component.browser = this.browser;
			component.useArrowKeysForBrowser = contextName == "joystickArrowKeys";
			break;
		}
		case "toggleLockInteraction":
			this.browser.EnableInput = !state;
			if (state)
			{
				this.ShowInfo("✔ Locked cursor interaction");
			}
			else
			{
				this.RemoveInfo();
			}
			break;
		case "closeBrowser":
			Managers.personManager.DoCloseBrowser(this.browser);
			base.CloseDialog();
			break;
		case "back":
			this.SwitchToThingDialog();
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x0400087E RID: 2174
	public Browser browser;

	// Token: 0x0400087F RID: 2175
	private TextMesh titleLabel;

	// Token: 0x04000880 RID: 2176
	private TextMesh urlLabel;

	// Token: 0x04000881 RID: 2177
	private GameObject favoriteButton;

	// Token: 0x04000882 RID: 2178
	private TextMesh infoLabel;

	// Token: 0x04000883 RID: 2179
	private string title = string.Empty;

	// Token: 0x04000884 RID: 2180
	private string fullUrlDisplayed = string.Empty;
}
