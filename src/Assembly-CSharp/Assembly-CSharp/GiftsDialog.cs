using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class GiftsDialog : Dialog
{
	// Token: 0x06000AC8 RID: 2760 RVA: 0x00054B68 File Offset: 0x00052F68
	public void Start()
	{
		this.allowGiftingOnselfForTesting = false;
		base.Init(base.gameObject, false, false, false);
		this.fundament = base.AddFundament();
		base.AddBackButton();
		InventoryScopeCube.resetPositionWhenExitingScope = false;
		this.personId = Our.personIdOfInterest;
		this.isOwn = this.personId == Managers.personManager.ourPerson.userId;
		if (!this.isOwn || this.allowGiftingOnselfForTesting)
		{
			this.isWaitingForThingInclusion = true;
		}
		base.StartCoroutine(this.AddHeadline());
		if (this.isWaitingForThingInclusion)
		{
			this.AddDropScopeCube();
		}
		this.expanderArrow = base.AddModelButton("ButtonBack", "toggleExpander", null, 430, -430, false);
		this.expanderArrow.transform.Rotate(new Vector3(0f, 90f, 0f));
		Managers.extrasManager.GetReceivedGifts(this.personId, delegate(List<GiftInfo> gifts)
		{
			this.SortGiftsIntoPublicPrivate(gifts);
			this.UpdateGifts();
		});
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x00054C6C File Offset: 0x0005306C
	private IEnumerator AddHeadline()
	{
		if (this.headline != null)
		{
			global::UnityEngine.Object.Destroy(this.headline);
			yield return false;
		}
		string name = ((!this.isOwn) ? Our.personNameOfInterest : Managers.personManager.ourPerson.screenName);
		name = Misc.Truncate(name, 24, true);
		char lastChar = char.ToLower(name[name.Length - 1]);
		string text = string.Concat(new string[]
		{
			name,
			(lastChar != 's') ? "'s" : "'",
			"\n",
			(!this.showPrivate) ? string.Empty : "Private ",
			"Nifts"
		});
		Transform thisTransform = base.AddHeadline(text, 0, -470, TextColor.Gold, TextAlignment.Center, false);
		this.headline = thisTransform.gameObject;
		base.ApplyTextSizeFactor(thisTransform, 0.9f, 0f, 0f, false);
		TextMesh thisTextMesh = thisTransform.GetComponent<TextMesh>();
		thisTextMesh.lineSpacing = 0.8f;
		yield break;
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00054C88 File Offset: 0x00053088
	private void AddDropScopeCube()
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("DialogParts/Prefabs/GiftDropScopeCube", typeof(GameObject))) as GameObject;
		gameObject.transform.parent = this.fundament.transform;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = Vector3.zero;
		this.dropScopeCube = gameObject.GetComponent<GiftDropScopeCube>();
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00054CF8 File Offset: 0x000530F8
	private void SortGiftsIntoPublicPrivate(List<GiftInfo> gifts)
	{
		this.publicGifts = new List<GiftInfo>();
		this.privateGifts = new List<GiftInfo>();
		foreach (GiftInfo giftInfo in gifts)
		{
			if (giftInfo.isPrivate)
			{
				this.privateGifts.Add(giftInfo);
			}
			else
			{
				this.publicGifts.Add(giftInfo);
			}
		}
		if (this.privateGifts.Count >= 1 && !this.isOwn)
		{
			Log.Debug("We see private gifts of others, so removing those.");
			this.privateGifts = new List<GiftInfo>();
		}
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00054DB8 File Offset: 0x000531B8
	private void UpdateGifts()
	{
		List<GiftInfo> list = ((!this.showPrivate) ? this.publicGifts : this.privateGifts);
		if (list.Count >= 1)
		{
			base.StartCoroutine(this.ShowGift(list[this.page], list.Count, false));
		}
		else
		{
			base.StartCoroutine(this.ShowNoGiftsYet());
		}
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00054E20 File Offset: 0x00053220
	private IEnumerator ShowNoGiftsYet()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		base.AddLabel("None yet", 0, 0, 0.8f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00054E3C File Offset: 0x0005323C
	private int GetMaxPages()
	{
		List<GiftInfo> list = ((!this.showPrivate) ? this.publicGifts : this.privateGifts);
		return list.Count;
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x00054E6C File Offset: 0x0005326C
	private bool HasUnseenGifts()
	{
		bool flag = false;
		foreach (GiftInfo giftInfo in this.publicGifts)
		{
			if (!giftInfo.wasSeenByReceiver)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			foreach (GiftInfo giftInfo2 in this.privateGifts)
			{
				if (!giftInfo2.wasSeenByReceiver)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00054F34 File Offset: 0x00053334
	private IEnumerator ShowGift(GiftInfo gift, int giftsCount = 0, bool isForPreview = false)
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			global::UnityEngine.Object.Destroy(this.backsideWrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		Vector3 position = new Vector3(gift.positionX, gift.positionY, gift.positionZ);
		ThingManager thingManager = Managers.thingManager;
		ThingRequestContext thingRequestContext = ThingRequestContext.GiftsDialog;
		string thingId = gift.thingId;
		Transform transform = this.transform;
		Vector3 vector = position;
		float num = 0.17f;
		float rotationX = gift.rotationX;
		float rotationY = gift.rotationY;
		thingManager.InstantiateThingOnDialogViaCache(thingRequestContext, thingId, transform, vector, num, false, false, rotationX, rotationY, gift.rotationZ, true, this.isOwn && !gift.wasSeenByReceiver && !isForPreview);
		string senderName = Misc.Truncate(gift.senderName, 22, true);
		base.AddLabel("From\n" + senderName, -450, 370, 0.75f, false, TextColor.Gold, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		string dateSent = ((!isForPreview) ? DateTime.Parse(gift.dateSent) : DateTime.Now).ToString("yyyy-MM-dd");
		base.AddLabel("On\n" + dateSent, 450, 370, 0.75f, false, TextColor.Gold, false, TextAlignment.Right, -1, 1f, false, TextAnchor.MiddleLeft);
		if (!isForPreview)
		{
			if (this.isOwn && !gift.wasSeenByReceiver)
			{
				gift.wasSeenByReceiver = true;
				Managers.extrasManager.MarkGiftSeen(gift.id, delegate(bool ok, string reasonFailed)
				{
					if (ok)
					{
						if (!this.HasUnseenGifts())
						{
							Managers.pollManager.showAlertForNewGifts = true;
						}
					}
					else if (reasonFailed != null)
					{
						Log.Debug("MarkGiftSeen issue: " + reasonFailed);
					}
				});
			}
			if (giftsCount >= 2)
			{
				base.AddDefaultPagingButtons(60, 407, "Page", false, 0, 0.85f, false);
			}
		}
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		if (isForPreview)
		{
			base.AddCheckbox("isPreparedGiftPrivate", null, "Private", 0, -400, gift.isPrivate, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
			base.AddLabel("Note others can't grab a Nift from the dialog", 0, 400, 0.7f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		else if (this.isOwn)
		{
			if (gift.isPrivate)
			{
			}
		}
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00054F64 File Offset: 0x00053364
	private new void Update()
	{
		base.Update();
		base.ReactToOnClickInWrapper(this.expanderWrapper);
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00054F78 File Offset: 0x00053378
	private void ShowExpander()
	{
		this.RotateExpanderArrow();
		string text = "Nifts can be gifts, achievements, messages or anything else you can think of! They cost $5 and support Anyland" + ((!this.isOwn) ? ", so thank you" : string.Empty) + "!";
		TextMesh textMesh = base.ShowHelpLabel(text, 50, 0.7f, TextAlignment.Center, -710, false, false, 1f, TextColor.Default);
		textMesh.lineSpacing = 0.85f;
		this.expanderWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.expanderWrapper);
		if (this.isOwn)
		{
			string text2 = "showPublic";
			string text3 = null;
			string text4 = "Public Nifts";
			string text5 = "ButtonCompactNoIconShortCentered";
			int num = -190;
			int num2 = -560;
			bool flag = !this.showPrivate;
			base.AddButton(text2, text3, text4, text5, num, num2, null, flag, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			text5 = "showPrivate";
			text4 = null;
			text3 = "Private Nifts";
			text2 = "ButtonCompactNoIconShortCentered";
			num2 = 190;
			num = -560;
			flag = this.showPrivate;
			base.AddButton(text5, text4, text3, text2, num2, num, null, flag, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		else
		{
			base.AddButton("prepareCreatingGift", null, "+ Make Nift...", "ButtonCompactNoIconShortCentered", 0, -560, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00055110 File Offset: 0x00053510
	private void RotateExpanderArrow()
	{
		this.expanderArrow.transform.Rotate(new Vector3(0f, 180f, 0f));
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00055136 File Offset: 0x00053536
	private void HideExpander()
	{
		this.RotateExpanderArrow();
		base.HideHelpLabel();
		global::UnityEngine.Object.Destroy(this.expanderWrapper);
		this.expanderWrapper = null;
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00055158 File Offset: 0x00053558
	private void SetShowPrivate(bool _showPrivate)
	{
		this.HideExpander();
		if (this.showPrivate == _showPrivate)
		{
			return;
		}
		this.showPrivate = _showPrivate;
		this.page = 0;
		Transform transform = this.fundament.transform.Find("FundamentMesh");
		Renderer component = transform.GetComponent<Renderer>();
		component.material.color = ((!this.showPrivate) ? new Color32(204, 234, byte.MaxValue, byte.MaxValue) : new Color32(128, 155, 174, byte.MaxValue));
		base.StartCoroutine(this.AddHeadline());
		this.UpdateGifts();
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x00055208 File Offset: 0x00053608
	private IEnumerator PrepareCreatingGift(bool tellAboutOnlyOwnCreations = false)
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			global::UnityEngine.Object.Destroy(this.backsideWrapper);
			yield return false;
		}
		this.isWaitingForThingInclusion = true;
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		string text = "Drop a creation of yours\nfrom the inventory here";
		if (tellAboutOnlyOwnCreations)
		{
			text = "Please drop something \nyou created";
		}
		this.dropText = base.AddLabel(text, 0, -25, 0.8f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0005522C File Offset: 0x0005362C
	public bool HandleAddedThingFromInventory(GameObject thingObject)
	{
		if (this.isWaitingForThingInclusion && thingObject != null && this.dropScopeCube != null && this.dropScopeCube.isInside)
		{
			Thing component = thingObject.GetComponent<Thing>();
			if (component != null)
			{
				this.page = 0;
				this.addedGift = new GiftInfo();
				this.addedGift.senderName = Managers.personManager.ourPerson.screenName;
				this.addedGift.senderId = Managers.personManager.ourPerson.userId;
				Transform parent = thingObject.transform.parent;
				thingObject.transform.parent = this.wrapper.transform;
				this.addedGift.thingId = component.thingId;
				this.addedGift.positionX = thingObject.transform.localPosition.x;
				this.addedGift.positionY = thingObject.transform.localPosition.y;
				this.addedGift.positionZ = thingObject.transform.localPosition.z;
				this.addedGift.rotationX = thingObject.transform.localEulerAngles.x;
				this.addedGift.rotationY = thingObject.transform.localEulerAngles.y;
				this.addedGift.rotationZ = thingObject.transform.localEulerAngles.z;
				thingObject.transform.parent = parent;
				base.StartCoroutine(this.ShowGift(this.addedGift, 0, true));
				this.RemoveDroppedThingIfWeArentCreator();
			}
		}
		return this.isWaitingForThingInclusion;
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x000553E2 File Offset: 0x000537E2
	private void RemoveDroppedThingIfWeArentCreator()
	{
		Managers.thingManager.GetThingInfo(this.addedGift.thingId, delegate(ThingInfo thingInfo)
		{
			if (this == null)
			{
				return;
			}
			bool flag = thingInfo.creatorId == Managers.personManager.ourPerson.userId;
			if (flag)
			{
				if (this.sendButton == null)
				{
					this.sendButton = base.AddButton("saveAndSendNift", null, "✔  Send!", "ButtonCompactNoIconShortCentered", 0, 408, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
			}
			else
			{
				global::UnityEngine.Object.Destroy(this.sendButton);
				Managers.soundManager.Play("no", this.transform, 0.2f, false, false);
				base.StartCoroutine(this.PrepareCreatingGift(true));
			}
		});
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00055405 File Offset: 0x00053805
	private void OnDestroy()
	{
		InventoryScopeCube.resetPositionWhenExitingScope = true;
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00055410 File Offset: 0x00053810
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "toggleExpander":
			if (this.expanderWrapper == null)
			{
				this.ShowExpander();
			}
			else
			{
				this.HideExpander();
			}
			break;
		case "prepareCreatingGift":
			if (this.expanderWrapper != null)
			{
				this.HideExpander();
			}
			global::UnityEngine.Object.Destroy(this.sendButton);
			base.StartCoroutine(this.PrepareCreatingGift(false));
			break;
		case "showPublic":
			this.SetShowPrivate(false);
			break;
		case "showPrivate":
			this.SetShowPrivate(true);
			break;
		case "previousPage":
		{
			int num = --this.page;
			if (num < 0)
			{
				this.page = this.GetMaxPages() - 1;
			}
			this.UpdateGifts();
			break;
		}
		case "nextPage":
		{
			int num = ++this.page;
			if (num >= this.GetMaxPages())
			{
				this.page = 0;
			}
			this.UpdateGifts();
			break;
		}
		case "saveAndSendNift":
		{
			global::UnityEngine.Object.Destroy(this.sendButton);
			this.sendButton = null;
			Vector3 vector = new Vector3(this.addedGift.positionX, this.addedGift.positionY, this.addedGift.positionZ);
			Vector3 vector2 = new Vector3(this.addedGift.rotationX, this.addedGift.rotationY, this.addedGift.rotationZ);
			Managers.extrasManager.SubmitGiftAndStartPurchase(this.personId, this.addedGift.thingId, vector, vector2, this.addedGift.isPrivate, delegate(ResponseError responseError)
			{
				if (responseError == null)
				{
				}
			});
			break;
		}
		case "confirmMakePrivate":
		case "confirmMakePublic":
		{
			string text = string.Empty;
			if (contextName == "confirmMakePrivate")
			{
				text = "Make this Nift private so no one else can see it?";
			}
			else
			{
				text = "Make this Nift public so everyone can see it?";
			}
			base.SwitchToConfirmDialog(text, delegate(bool didConfirm)
			{
				if (didConfirm)
				{
					Managers.extrasManager.ToggleGiftPrivacy(contextId, delegate(bool isNowPrivate)
					{
						this.SwitchTo(DialogType.Gifts, string.Empty);
					});
				}
				else
				{
					this.SwitchTo(DialogType.Gifts, string.Empty);
				}
			});
			break;
		}
		case "isPreparedGiftPrivate":
			if (this.addedGift != null)
			{
				this.addedGift.isPrivate = state;
			}
			break;
		case "back":
			base.SwitchTo((!this.isOwn) ? DialogType.Profile : DialogType.OwnProfile, string.Empty);
			break;
		}
	}

	// Token: 0x0400082D RID: 2093
	private bool allowGiftingOnselfForTesting;

	// Token: 0x0400082E RID: 2094
	public const float giftMaxScale = 0.17f;

	// Token: 0x0400082F RID: 2095
	private int page;

	// Token: 0x04000830 RID: 2096
	private bool isOwn;

	// Token: 0x04000831 RID: 2097
	private string personId;

	// Token: 0x04000832 RID: 2098
	private List<GiftInfo> publicGifts;

	// Token: 0x04000833 RID: 2099
	private List<GiftInfo> privateGifts;

	// Token: 0x04000834 RID: 2100
	private GameObject expanderArrow;

	// Token: 0x04000835 RID: 2101
	private bool showPrivate;

	// Token: 0x04000836 RID: 2102
	private bool isWaitingForThingInclusion;

	// Token: 0x04000837 RID: 2103
	private GameObject expanderWrapper;

	// Token: 0x04000838 RID: 2104
	private new GameObject backsideWrapper;

	// Token: 0x04000839 RID: 2105
	private GameObject sendButton;

	// Token: 0x0400083A RID: 2106
	private GiftInfo addedGift;

	// Token: 0x0400083B RID: 2107
	private GameObject fundament;

	// Token: 0x0400083C RID: 2108
	private GiftDropScopeCube dropScopeCube;

	// Token: 0x0400083D RID: 2109
	private TextMesh dropText;

	// Token: 0x0400083E RID: 2110
	private GameObject headline;
}
