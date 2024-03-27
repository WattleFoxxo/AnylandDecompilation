using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x020001E5 RID: 485
public class BrowserManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06001010 RID: 4112 RVA: 0x00089FFE File Offset: 0x000883FE
	// (set) Token: 0x06001011 RID: 4113 RVA: 0x0008A006 File Offset: 0x00088406
	public ManagerStatus status { get; private set; }

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06001012 RID: 4114 RVA: 0x0008A00F File Offset: 0x0008840F
	// (set) Token: 0x06001013 RID: 4115 RVA: 0x0008A017 File Offset: 0x00088417
	public string failMessage { get; private set; }

	// Token: 0x06001014 RID: 4116 RVA: 0x0008A020 File Offset: 0x00088420
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0008A030 File Offset: 0x00088430
	public void ActivateBrowserHandPointerIfNeeded()
	{
		if (!this.browserHandPointer.gameObject.activeSelf)
		{
			this.browserHandPointer.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0008A058 File Offset: 0x00088458
	public void SetDominantPointerHand(TopographyId preferentialDialogHandSide)
	{
		this.browserHandPointer.hand = ((preferentialDialogHandSide != TopographyId.Left) ? XRNode.LeftHand : XRNode.RightHand);
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0008A074 File Offset: 0x00088474
	public void DestroyAllBrowsers()
	{
		if (Universe.features.webBrowsing && this.guideBrowser == null)
		{
			this.browserHandPointer.gameObject.SetActive(false);
			GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (GameObject gameObject in rootGameObjects)
			{
				Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Browser), true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					GameObject gameObject2 = componentsInChildren[j].gameObject;
					ThingPart component = gameObject2.GetComponent<ThingPart>();
					if (component != null)
					{
						global::UnityEngine.Object.Destroy(gameObject2.GetComponent<CursorRendererWorldSpace>());
						global::UnityEngine.Object.Destroy(gameObject2.GetComponent<PointerUIMesh>());
						global::UnityEngine.Object.Destroy(gameObject2.GetComponent<Browser>());
					}
					else
					{
						global::UnityEngine.Object.Destroy(gameObject2);
					}
				}
			}
			BrowserNative.UnloadNative();
		}
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x0008A162 File Offset: 0x00088562
	public bool GuideIsShowing()
	{
		return this.guideBrowser != null;
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x0008A170 File Offset: 0x00088570
	public Browser TryAttachBrowser(ThingPart thingPart, BrowserSettings settings = null, bool isSyncIgnorableUrl = false)
	{
		Browser browser = null;
		if (Universe.features.webBrowsing)
		{
			bool? webBrowsing = Managers.areaManager.rights.webBrowsing;
			if (webBrowsing.Value)
			{
				if (settings == null)
				{
					settings = new BrowserSettings();
				}
				if (!thingPart.offersScreen)
				{
					thingPart = this.GetNearestThingPartScreen();
				}
				if (thingPart != null)
				{
					browser = this.AttachBrowser(thingPart, settings, isSyncIgnorableUrl);
					if (thingPart.videoScreen != null && Our.IsMasterClient(false))
					{
						Managers.videoManager.StopVideo(true);
					}
				}
			}
		}
		return browser;
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x0008A208 File Offset: 0x00088608
	private Browser AttachBrowser(ThingPart thingPart, BrowserSettings settings, bool isSyncIgnorableUrl)
	{
		Browser browser = thingPart.browser;
		if (browser == null)
		{
			browser = thingPart.transform.parent.GetComponentInChildren<Browser>();
		}
		if (browser == null)
		{
			this.ActivateBrowserHandPointerIfNeeded();
			bool flag = thingPart.baseType != ThingPartBase.Cube;
			PointerUIMesh pointerUIMesh;
			CursorRendererWorldSpace cursorRendererWorldSpace;
			if (flag)
			{
				Transform transform = thingPart.transform;
				browser = transform.gameObject.AddComponent<Browser>();
				pointerUIMesh = transform.gameObject.AddComponent<PointerUIMesh>();
				cursorRendererWorldSpace = transform.gameObject.AddComponent<CursorRendererWorldSpace>();
				browser.SetWidth(1000);
				browser.SetHeight(1000);
				bool flag2 = thingPart.materialType == MaterialTypes.Glow || thingPart.materialType == MaterialTypes.PointLight || thingPart.materialType == MaterialTypes.SpotLight;
				Renderer component = transform.GetComponent<Renderer>();
				if (flag2)
				{
					component.material = this.browserMaterial;
				}
				browser.baseColor = ((!component.material.HasProperty("_Color")) ? Color.white : component.material.color);
			}
			else
			{
				Transform transform = global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Browser") as GameObject, Vector3.zero, Quaternion.identity).transform;
				transform.parent = thingPart.transform.parent;
				transform.localPosition = thingPart.transform.localPosition;
				transform.localRotation = thingPart.transform.localRotation;
				transform.localScale = Vector3.one;
				transform.name = Misc.RemoveCloneFromName(transform.name);
				transform.Translate(new Vector3(0f, thingPart.transform.localScale.y * 0.5f + 0.005f, 0f));
				transform.Rotate(new Vector3(90f, 0f, 0f));
				Vector3 localScale = transform.localScale;
				localScale.x = thingPart.transform.localScale.x;
				localScale.y = thingPart.transform.localScale.z;
				transform.localScale = localScale;
				browser = transform.GetComponent<Browser>();
				if (localScale.x < localScale.y)
				{
					browser.SetWidth((int)Mathf.Round(localScale.x * 1000f / localScale.y));
				}
				else if (localScale.y < localScale.x)
				{
					browser.SetHeight((int)Mathf.Round(localScale.y * 1000f / localScale.x));
				}
				pointerUIMesh = transform.GetComponent<PointerUIMesh>();
				cursorRendererWorldSpace = transform.GetComponent<CursorRendererWorldSpace>();
			}
			browser.cursorRenderer = browser.GetComponent<CursorRendererWorldSpace>();
			browser.cursorRenderer.size = 0.1f;
			browser.ourPersonHead = Managers.treeManager.GetTransform("/OurPersonRig/HeadCore");
			pointerUIMesh.enableMouseInput = false;
			pointerUIMesh.enableTouchInput = false;
			pointerUIMesh.enableFPSInput = false;
			pointerUIMesh.enableVRInput = true;
			pointerUIMesh.maxDistance = 1000f;
			pointerUIMesh.dragMovementThreshold = 40f;
			cursorRendererWorldSpace.zOffset = 0.005f;
			cursorRendererWorldSpace.size = 0.01f;
			thingPart.browser = browser;
			if (settings.zoomPercent != 100f)
			{
				browser.Zoom = this.ZoomPercentToInternalUnit(settings.zoomPercent);
			}
			PointerUIBase pointer = browser.GetComponent<PointerUIBase>();
			pointer.onClick += delegate
			{
				this.lastClickedBrowser = browser;
				this.lastClickedPointer = pointer;
				this.Invoke("HandlePossibleTextFieldFocus", 0.1f);
			};
			browser.onLoad += delegate(JSONNode data)
			{
				this.OnPageLoaded(browser);
			};
		}
		this.RegisterBrowserFunctions(browser);
		string text = Managers.browserManager.GetValidUrl(settings.url);
		if (this.UrlIsBlockedInThisCase(thingPart, text))
		{
			text = "about:blank";
			if (Managers.areaManager.weAreEditorOfCurrentArea)
			{
				this.ShowBlockedUrlMessage();
			}
		}
		if (isSyncIgnorableUrl)
		{
			browser.lastRemotePersonReceivedUrl = text;
		}
		browser.Url = text;
		browser.isSyncIgnorableUrl = isSyncIgnorableUrl;
		browser.allowUrlNavigation = settings.allowUrlNavigation;
		browser.allowCursor = settings.allowCursor;
		browser.syncUrlChangesBetweenPeople = settings.syncUrlChangesBetweenPeople;
		return browser;
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0008A6B0 File Offset: 0x00088AB0
	public void RegisterBrowserFunctions(Browser browser)
	{
		if (browser != null)
		{
			browser.RegisterFunction("AnylandTell", delegate(JSONNode args)
			{
				this.BrowserTell(browser, args[0]);
			});
			browser.RegisterFunction("AnylandTellAny", delegate(JSONNode args)
			{
				this.BrowserTellAny(args[0]);
			});
			browser.RegisterFunction("AnylandSetThing", delegate(JSONNode args)
			{
				this.BrowserSetThing(args[0]);
			});
			browser.RegisterFunction("AnylandRequestThing", delegate(JSONNode args)
			{
				this.BrowserRequestThing(browser);
			});
			browser.RegisterFunction("AnylandEmitThing", delegate(JSONNode args)
			{
				this.BrowserEmitThing(browser, args[0]);
			});
			browser.RegisterFunction("AnylandClosePage", delegate(JSONNode args)
			{
				this.BrowserClosePage(browser);
			});
		}
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0008A78C File Offset: 0x00088B8C
	public IEnumerator RegisterBrowserFunctionsDelayed(Browser browser)
	{
		yield return new WaitForSeconds(1f);
		this.RegisterBrowserFunctions(browser);
		yield break;
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0008A7AE File Offset: 0x00088BAE
	private void BrowserClosePage(Browser browser)
	{
		Managers.personManager.DoCloseBrowser(browser);
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0008A7BC File Offset: 0x00088BBC
	public void ToggleGuideBrowser(string specificUrl = null)
	{
		if (this.guideBrowser == null)
		{
			this.OpenGuideBrowser(null, specificUrl);
		}
		else
		{
			this.CloseGuideBrowser();
		}
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0008A7F0 File Offset: 0x00088BF0
	public void OpenGuideBrowser(string videoId = null, string specificUrl = null)
	{
		if (!Universe.features.helpDialog)
		{
			return;
		}
		if (this.guideBrowser != null)
		{
			global::UnityEngine.Object.Destroy(this.guideBrowser);
			this.guideBrowser = null;
		}
		if (this.guideBrowser == null)
		{
			this.ActivateBrowserHandPointerIfNeeded();
			if (CrossDevice.desktopMode)
			{
				Our.SetMode(EditModes.None, false);
			}
			this.guideBrowser = global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/GuideBrowser") as GameObject);
			this.guideBrowser.name = Misc.RemoveCloneFromName(this.guideBrowser.name);
			Browser componentInChildren = this.guideBrowser.GetComponentInChildren<Browser>();
			componentInChildren.ourPersonHead = Managers.treeManager.GetTransform("/OurPersonRig/HeadCore");
			string text = this.GetValidUrl("http://anyland.com/guide/?v=" + 8);
			if (specificUrl != null)
			{
				text = specificUrl;
			}
			if (!string.IsNullOrEmpty(videoId))
			{
				text = text + "&videoId=" + videoId;
			}
			componentInChildren.Url = text;
			componentInChildren.RegisterFunction("AnylandClosePage", delegate(JSONNode args)
			{
				this.CloseGuideBrowser();
			});
			componentInChildren.RegisterFunction("AnylandOpenBoards", delegate(JSONNode args)
			{
				this.OpenBoards();
			});
		}
		this.UpdateGuideBrowserTransform();
		Managers.achievementManager.RegisterAchievement(Achievement.SawHelpVideo);
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0008A92B File Offset: 0x00088D2B
	public void CloseGuideBrowser()
	{
		global::UnityEngine.Object.Destroy(this.guideBrowser);
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0008A938 File Offset: 0x00088D38
	public bool GuideBrowserShows()
	{
		return this.guideBrowser != null;
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x0008A948 File Offset: 0x00088D48
	private void OpenBoards()
	{
		GameObject gameObject = Managers.dialogManager.SwitchToNewDialog(DialogType.Forums, null, string.Empty);
		if (gameObject != null)
		{
			Dialog component = gameObject.GetComponent<Dialog>();
			component.DoLongerHapticPulse();
		}
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0008A984 File Offset: 0x00088D84
	public void UpdateGuideBrowserTransform()
	{
		if (this.guideBrowser != null)
		{
			Transform transform = this.guideBrowser.transform;
			transform.parent = null;
			GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
			Vector3 vector = transform.position;
			vector = @object.transform.position + @object.transform.forward * 2f;
			vector.y = @object.transform.position.y + 0.1f;
			transform.position = vector;
			transform.rotation = @object.transform.rotation;
			transform.Rotate(Vector3.forward * 90f);
			Vector3 localEulerAngles = transform.localEulerAngles;
			localEulerAngles.x = 0f;
			localEulerAngles.z = 0f;
			transform.localEulerAngles = localEulerAngles;
			transform.Rotate(Vector3.left * 90f);
			transform.localScale *= 1.35f;
			transform.parent = Managers.treeManager.GetTransform("/OurPersonRig");
		}
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0008AAA8 File Offset: 0x00088EA8
	public void ShowBlockedUrlMessage()
	{
		Managers.dialogManager.ShowInfo("Oops, some web pages here require the browser to be attached to you, or placed in a private area.", false, true, 1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x0008AAD4 File Offset: 0x00088ED4
	public bool UrlIsBlockedInThisCase(ThingPart thingPart, string url)
	{
		bool flag = false;
		if (!Managers.areaManager.isPrivate && !string.IsNullOrEmpty(url) && url != "about:blank")
		{
			string[] array = new string[] { "sex", "porn", "adult", "nude", "naked", "nudity", "xxx", "fuq", "fucking", "nsfw" };
			string[] array2 = new string[] { "adultswim.com" };
			if (url.IndexOf("://") == -1)
			{
				url = "http://" + url;
			}
			Uri uri = null;
			try
			{
				uri = new Uri(url);
			}
			catch (UriFormatException ex)
			{
			}
			flag = Misc.ContainsAny(url.ToLower(), array) && (!(uri != null) || !Misc.ContainsAny(uri.Host, array2)) && !Managers.personManager.GetIsThisObjectOfOurPerson(thingPart.gameObject, false);
		}
		return flag;
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x0008AC00 File Offset: 0x00089000
	private void HandlePossibleTextFieldFocus()
	{
		Browser browser = this.lastClickedBrowser;
		PointerUIBase pointer = this.lastClickedPointer;
		this.lastClickedBrowser = null;
		this.lastClickedPointer = null;
		if (browser == null)
		{
			return;
		}
		if (browser.focusState.focusedNodeEditable)
		{
			pointer.ForceKeyboardHasFocus(true);
			browser.SendFrameCommand(BrowserNative.FrameCommand.SelectAll);
			if (!Managers.dialogManager.KeyboardIsOpen())
			{
				browser.EnableInput = false;
				Managers.dialogManager.GetInput(delegate(string text)
				{
					if (browser != null && pointer != null)
					{
						if (text != null)
						{
							browser.TypeText(text);
						}
						browser.EnableInput = true;
						pointer.ForceKeyboardHasFocus(false);
					}
					Managers.dialogManager.CloseDialog();
				}, string.Empty, string.Empty, -1, string.Empty, true, true, true, true, 1f, false, false, null, false);
			}
		}
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0008ACC8 File Offset: 0x000890C8
	private void OnPageLoaded(Browser browser)
	{
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null)
		{
			BrowserDialog component = currentNonStartDialog.GetComponent<BrowserDialog>();
			if (component != null && component.browser == browser)
			{
				component.UpdateFields();
			}
		}
		string url = browser.Url;
		if (!string.IsNullOrEmpty(url) && url != browser.lastRemotePersonReceivedUrl && !browser.isSyncIgnorableUrl)
		{
			Managers.personManager.DoSetBrowserUrl(browser, url, false, true);
		}
		browser.isSyncIgnorableUrl = false;
		if (browser.Zoom != 0f)
		{
			browser.Zoom = browser.Zoom;
		}
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0008AD70 File Offset: 0x00089170
	private float ZoomPercentToInternalUnit(float percent)
	{
		return 4f * Mathf.Log(percent * 0.01f);
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0008AD84 File Offset: 0x00089184
	public string GetValidUrl(string urlOrSearch)
	{
		string text = string.Empty;
		if (string.IsNullOrEmpty(urlOrSearch))
		{
			urlOrSearch = "about:blank";
		}
		urlOrSearch = urlOrSearch.Trim();
		if (urlOrSearch == "about:blank")
		{
			text = "about:blank";
		}
		else if (urlOrSearch.IndexOf(".") >= 0 || urlOrSearch.IndexOf("://") >= 0)
		{
			text = urlOrSearch;
			if (!text.StartsWith("http://") && !text.StartsWith("https://"))
			{
				text = text.Replace("://", string.Empty);
				text = "http://" + text;
			}
		}
		else
		{
			text = "https://www.google.com/search?q=" + Uri.EscapeDataString(urlOrSearch);
		}
		if (text.StartsWith("http://anyland.com/guide/"))
		{
			text += this.GetGuideUrlAdditions();
		}
		return text;
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x0008AE64 File Offset: 0x00089264
	private string GetGuideUrlAdditions()
	{
		string text = string.Empty;
		text = text + "#device=" + CrossDevice.type.ToString();
		string text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			"&version=",
			Universe.GetClientVersionDisplay(),
			" | Server version ",
			Universe.GetServerVersionDisplay()
		});
		text = text + "&filesPath=" + Application.persistentDataPath;
		if (CrossDevice.desktopMode)
		{
			string text3 = "You can create using VR devices Vive, Oculus Touch, and Windows Mixed Reality.";
			if (XRDevice.isPresent)
			{
				text3 += " Press F9 to switch to VR mode.";
			}
			text = text + "&extra=" + text3;
		}
		return text;
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0008AF0D File Offset: 0x0008930D
	public bool IsValidUrlProtocol(string url)
	{
		return !string.IsNullOrEmpty(url) && (url.StartsWith("http://") || url.StartsWith("https://") || url == "about:blank");
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0008AF4C File Offset: 0x0008934C
	private void BrowserTell(Browser browser, string data)
	{
		ThingPart browserThingPart = Managers.browserManager.GetBrowserThingPart(browser);
		if (browserThingPart != null)
		{
			Thing component = browserThingPart.transform.parent.GetComponent<Thing>();
			if (component != null)
			{
				component.TriggerEvent(StateListener.EventType.OnTold, data, false, null);
			}
		}
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x0008AF99 File Offset: 0x00089399
	private void BrowserTellAny(string data)
	{
		Managers.behaviorScriptManager.TriggerTellAnyEvent(data, false);
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x0008AFA8 File Offset: 0x000893A8
	private void BrowserSetThing(string json)
	{
		if (Our.mode == EditModes.Thing)
		{
			IEnumerator enumerator = CreationHelper.thingBeingEdited.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					global::UnityEngine.Object.Destroy(transform.gameObject);
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
			JsonToThingConverter.SetThing(CreationHelper.thingBeingEdited, json, true, false, null, null);
			Managers.soundManager.Play("putDown", null, 1f, false, false);
			Managers.dialogManager.SwitchToNewDialog(DialogType.Create, null, string.Empty);
		}
		else
		{
			Managers.thingManager.StartCreateThingViaJson(json);
		}
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0008B07C File Offset: 0x0008947C
	private void BrowserRequestThing(Browser browser)
	{
		if (browser != null)
		{
			if (CreationHelper.thingBeingEdited != null)
			{
				int num = 0;
				int num2 = 0;
				string text = ThingToJsonConverter.GetJson(CreationHelper.thingBeingEdited, ref num, ref num2);
				if (text == null)
				{
					text = string.Empty;
				}
				browser.CallFunction("AnylandGetThing", new JSONNode[] { text }).Done();
			}
			else
			{
				browser.CallFunction("AnylandGetThing", new JSONNode[] { string.Empty }).Done();
			}
		}
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0008B10C File Offset: 0x0008950C
	private void BrowserEmitThing(Browser browser, string thingId)
	{
		Vector3 localEulerAngles = browser.transform.localEulerAngles;
		Vector3 vector = localEulerAngles;
		localEulerAngles.x *= -1f;
		browser.transform.localEulerAngles = localEulerAngles;
		Managers.thingManager.EmitThingFromOrigin(ThingRequestContext.BrowserEmitThingCommand, browser.transform, thingId, 1f, false, false);
		browser.transform.localEulerAngles = vector;
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0008B16C File Offset: 0x0008956C
	private ThingPart GetNearestThingPartScreen()
	{
		Transform centerTransform = Managers.treeManager.GetObject("/OurPersonRig/HeadCore").transform;
		ThingPart[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(ThingPart)) as ThingPart[];
		array = array.OrderBy((ThingPart x) => Vector3.Distance(centerTransform.position, x.transform.position)).ToArray<ThingPart>();
		foreach (ThingPart thingPart in array)
		{
			if (thingPart.offersScreen)
			{
				Thing parentThing = thingPart.GetParentThing();
				if (parentThing != null && !parentThing.isInInventoryOrDialog && parentThing.name != Universe.objectNameIfAlreadyDestroyed && parentThing.gameObject != CreationHelper.thingBeingEdited)
				{
					return thingPart;
				}
			}
		}
		return null;
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0008B240 File Offset: 0x00089640
	private void InitFavoritesIfNeeded()
	{
		if (this.favorites == null)
		{
			this.favorites = new Dictionary<string, string>();
			string favoritesDataFolderPath = this.GetFavoritesDataFolderPath();
			Directory.CreateDirectory(favoritesDataFolderPath);
			string favoritesDataFilePath = this.GetFavoritesDataFilePath();
			if (File.Exists(favoritesDataFilePath))
			{
				string text = File.ReadAllText(favoritesDataFilePath);
				text = text.Replace("\r\n", "\r");
				string[] array = Misc.Split(text, "\r", StringSplitOptions.RemoveEmptyEntries);
				if (array.Length >= 1)
				{
					bool flag = false;
					foreach (string text2 in array)
					{
						int num = text2.IndexOf(" ");
						if (num >= 1)
						{
							string text3 = text2.Substring(0, num).Trim();
							string text4 = text2.Substring(num).Trim();
							if (text3 != string.Empty && !this.favorites.ContainsKey(text3))
							{
								this.favorites.Add(text3, text4);
							}
							else
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						this.SaveFavorites();
					}
				}
			}
		}
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0008B355 File Offset: 0x00089755
	public void AddFavorite(string url, string title)
	{
		this.InitFavoritesIfNeeded();
		if (this.favorites.ContainsKey(url))
		{
			this.favorites.Remove(url);
		}
		this.favorites.Add(url, title);
		this.SaveFavorites();
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0008B38E File Offset: 0x0008978E
	public void RemoveFavorite(string url)
	{
		this.InitFavoritesIfNeeded();
		if (this.favorites.ContainsKey(url))
		{
			this.favorites.Remove(url);
		}
		this.SaveFavorites();
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0008B3BA File Offset: 0x000897BA
	public bool IsFavorite(string url)
	{
		this.InitFavoritesIfNeeded();
		return this.favorites.ContainsKey(url);
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0008B3CE File Offset: 0x000897CE
	public Dictionary<string, string> GetFavorites()
	{
		this.InitFavoritesIfNeeded();
		return this.favorites;
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0008B3DC File Offset: 0x000897DC
	private void SaveFavorites()
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, string> keyValuePair in this.favorites)
		{
			if (text != string.Empty)
			{
				text += "\r";
			}
			text = text + keyValuePair.Key + " " + keyValuePair.Value;
		}
		File.WriteAllText(this.GetFavoritesDataFilePath(), text);
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0008B47C File Offset: 0x0008987C
	private string GetFavoritesDataFolderPath()
	{
		string userId = Managers.personManager.ourPerson.userId;
		return Application.persistentDataPath + "/data/" + userId;
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0008B4A9 File Offset: 0x000898A9
	private string GetFavoritesDataFilePath()
	{
		return this.GetFavoritesDataFolderPath() + "/web-favorites.dat";
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x0008B4BC File Offset: 0x000898BC
	public ThingPart GetBrowserThingPart(Browser browser)
	{
		ThingPart thingPart = null;
		if (browser != null)
		{
			thingPart = browser.GetComponent<ThingPart>();
			if (thingPart == null)
			{
				Component[] componentsInChildren = browser.transform.parent.GetComponentsInChildren(typeof(ThingPart), true);
				foreach (ThingPart thingPart2 in componentsInChildren)
				{
					if (thingPart2.browser == browser)
					{
						thingPart = thingPart2;
						break;
					}
				}
			}
			if (thingPart == null)
			{
				Log.Debug("GetBrowserThingPart didn't find thingPart");
			}
		}
		return thingPart;
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0008B55C File Offset: 0x0008995C
	public bool CursorIsInBrowser()
	{
		if (!this.useCacheForCursorIsInBrowser)
		{
			this.useCacheForCursorIsInBrowser = true;
			base.Invoke("StopUseCacheForCursorIsInBrowser", 0.5f);
			this.cursorIsInBrowser = false;
			GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (GameObject gameObject in rootGameObjects)
			{
				if (gameObject.name == "CursorForBrowser" && gameObject.activeSelf)
				{
					this.cursorIsInBrowser = true;
					break;
				}
			}
		}
		return this.cursorIsInBrowser;
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x0008B5F1 File Offset: 0x000899F1
	private void StopUseCacheForCursorIsInBrowser()
	{
		this.useCacheForCursorIsInBrowser = false;
	}

	// Token: 0x04001036 RID: 4150
	private const string favoritesLineSeparator = "\r";

	// Token: 0x04001037 RID: 4151
	private Dictionary<string, string> favorites;

	// Token: 0x04001038 RID: 4152
	public VRBrowserHand browserHandPointer;

	// Token: 0x04001039 RID: 4153
	public Material browserMaterial;

	// Token: 0x0400103A RID: 4154
	public const string blankUrl = "about:blank";

	// Token: 0x0400103B RID: 4155
	private Browser lastClickedBrowser;

	// Token: 0x0400103C RID: 4156
	private PointerUIBase lastClickedPointer;

	// Token: 0x0400103D RID: 4157
	private GameObject guideBrowser;

	// Token: 0x0400103E RID: 4158
	private bool cursorIsInBrowser;

	// Token: 0x0400103F RID: 4159
	private bool useCacheForCursorIsInBrowser;
}
