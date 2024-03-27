using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class InventoryDialog : Dialog
{
	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000A0B RID: 2571 RVA: 0x00044AB5 File Offset: 0x00042EB5
	// (set) Token: 0x06000A0C RID: 2572 RVA: 0x00044ABD File Offset: 0x00042EBD
	public bool isShowingTrash { get; private set; }

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x00044AC6 File Offset: 0x00042EC6
	// (set) Token: 0x06000A0E RID: 2574 RVA: 0x00044ACE File Offset: 0x00042ECE
	public bool isShowingSearch { get; private set; }

	// Token: 0x06000A0F RID: 2575 RVA: 0x00044AD8 File Offset: 0x00042ED8
	public GameObject Open(Hand hand)
	{
		base.Init(base.gameObject, false, false, false);
		this.isWaitingForServerResponse = false;
		base.gameObject.SetActive(true);
		base.gameObject.tag = "CurrentlyHeld";
		this.SetCurrentDialogEnabledIfNeeded(false);
		hand = this.transform.parent.GetComponent<Hand>();
		Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "inventory opened " + hand.side.ToString().ToLower(), true);
		this.SetArmAttachmentActive(false);
		if (!this.wasOpenedBefore)
		{
			this.wasOpenedBefore = true;
			this.AddPagingUI();
			this.AddTrashButton();
			this.LoadThings();
		}
		if (InventoryDialog.searchToTriggerOnNextOpening != null)
		{
			this.LoadUniverseSearchResults(InventoryDialog.searchToTriggerOnNextOpening);
			InventoryDialog.searchToTriggerOnNextOpening = null;
		}
		else if (!this.isShowingSearch)
		{
			this.AddSwitchToSearch();
		}
		if (this.isShowingSearch)
		{
			Managers.inventoryManager.ActivateInventorySpeechListener();
		}
		this.HandleAdjustPositionForDesktop();
		return base.gameObject;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00044BE8 File Offset: 0x00042FE8
	private void SetInfo(string text)
	{
		if (this.infoLabel == null)
		{
			this.infoLabel = base.AddLabel(string.Empty, 0, -1000, 1.5f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		this.infoLabel.text = text.ToUpper();
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00044C3F File Offset: 0x0004303F
	private void ClearInfo()
	{
		if (this.infoLabel != null)
		{
			this.infoLabel.text = string.Empty;
		}
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x00044C64 File Offset: 0x00043064
	private void AddSwitchToSearch()
	{
		int num = 7000;
		int num2 = -7000;
		global::UnityEngine.Object.Destroy(this.switchToSearch);
		global::UnityEngine.Object.Destroy(this.closeButton);
		if (this.defaultViewNeedsCloseButton)
		{
			this.closeButton = base.AddModelButton("ButtonClose", "close", null, num, -7000, false);
			num -= 900;
			num2 += 150;
		}
		this.switchToSearch = base.AddModelButton("Find", "switchToSearch", null, num - 100, num2, false);
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00044CEC File Offset: 0x000430EC
	private void SwitchToSearch()
	{
		this.isShowingSearch = true;
		this.ClearThings();
		this.RemovePagingUI();
		Misc.DestroyMultiple(new GameObject[] { this.trashButton, this.switchToSearch, this.closeButton });
		this.editSearchButton = base.AddModelButton("EditTextButton", "universalSearchByText", null, -6850, -7200, false);
		base.ScaleModelButtonWidthHeight(this.editSearchButton, 1.25f);
		this.closeButton = base.AddModelButton("ButtonClose", "close", null, 7000, -7000, false);
		this.searchHelpButton = base.AddModelButton("HelpButtonUnpositioned", "searchHelp", null, 6150, -7000, false);
		base.ScaleModelButtonWidthHeight(this.searchHelpButton, 0.65f);
		this.searchWordsButton = base.AddModelButton("Plus", "editSearchWords", null, -7000, 7000, false);
		Managers.inventoryManager.ActivateInventorySpeechListener();
		this.SetInfo("Speak to find");
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x00044DF4 File Offset: 0x000431F4
	private void SwitchToNormalInventory()
	{
		this.isShowingSearch = false;
		this.isWaitingForServerResponse = false;
		SpeechInput.DisposeListener();
		Misc.DestroyMultiple(new GameObject[] { this.editSearchButton, this.closeButton, this.searchWordsButton, this.searchHelpButton });
		this.AddSwitchToSearch();
		this.AddPagingUI();
		this.AddTrashButton();
		this.ClearInfo();
		this.LoadThings();
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x00044E64 File Offset: 0x00043264
	public void OnSpeechRecognized(SpeechRecognizedEventArgs args)
	{
		if (this.isShowingSearch)
		{
			base.TriggerHapticPulseOnParentHand();
			string text = args.Text;
			if (text == "last" || text == "last page")
			{
				if (this.isShowingSearch)
				{
					this.SearchResultsBack();
				}
				else
				{
					this.PageBack();
				}
			}
			else if (text == "next" || text == "next page")
			{
				if (this.isShowingSearch)
				{
					this.SearchResultsForward();
				}
				else
				{
					this.PageBack();
				}
			}
			else
			{
				if (!this.isShowingSearch)
				{
					this.SwitchToSearch();
				}
				this.LoadUniverseSearchResults(SearchWords.SpeechToText(text));
			}
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00044F28 File Offset: 0x00043328
	private void AddPagingUI()
	{
		if (!this.isShowingTrash)
		{
			this.backButton = base.AddModelButton("ButtonBack", (!this.isShowingSearch) ? "back" : "searchResultsBack", null, -1000, 7000, false);
			this.forwardButton = base.AddModelButton("ButtonForward", (!this.isShowingSearch) ? "forward" : "searchResultsForward", null, 1000, 7000, false);
			if (!this.isShowingSearch)
			{
				this.toInventoryStartButton = base.AddModelButton("ButtonToInventoryStart", "toInventoryStart", null, -2550, 7000, false);
				if (this.undoButton != null)
				{
					global::UnityEngine.Object.Destroy(this.undoButton);
				}
			}
		}
		this.UpdatePageLabelAndToInventoryStartButton();
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00045000 File Offset: 0x00043400
	private void LoadThings()
	{
		this.ClearThings();
		List<InventoryItemData> list;
		if (this.cachedResponsesByPage.TryGetValue(this.currentPage, out list))
		{
			this.DisplayThings(list);
		}
		else
		{
			this.isWaitingForServerResponse = true;
			base.StartCoroutine(Managers.serverManager.LoadInventory(this.currentPage, delegate(LoadInventory_Response response)
			{
				this.isWaitingForServerResponse = false;
				if (response.error == null)
				{
					this.cachedResponsesByPage.Add(this.currentPage, response.inventoryItems.inventoryItems);
					this.DisplayThings(response.inventoryItems.inventoryItems);
				}
				else
				{
					Log.Error(response.error);
				}
			}));
		}
		Managers.achievementManager.RegisterAchievement(Achievement.OpenedInventory);
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00045070 File Offset: 0x00043470
	private void DisplayThings(List<InventoryItemData> items)
	{
		if (base.gameObject.activeSelf && items.Count > 0)
		{
			this.ClearInfo();
			foreach (InventoryItemData inventoryItemData in items)
			{
				Managers.thingManager.InstantiateInventoryItemViaCache(ThingRequestContext.InventoryDialogLoadThingsAlternative, inventoryItemData, this.transform, false, false);
			}
		}
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x000450F8 File Offset: 0x000434F8
	public void LoadUniverseSearchResults(string term)
	{
		if (this.isShowingSearch && this.isWaitingForServerResponse)
		{
			return;
		}
		this.isWaitingForServerResponse = true;
		Managers.soundManager.Play("pickUp", this.transform, 0.2f, false, false);
		this.ClearThings();
		this.RemovePagingUI();
		this.currentPage = 0;
		this.SetInfo(term + Environment.NewLine + "...");
		Managers.thingManager.SearchThings(term, delegate(List<string> thingIds)
		{
			this.isWaitingForServerResponse = false;
			if (!this.isShowingSearch)
			{
				return;
			}
			this.ClearInfo();
			if (!this.gameObject.activeSelf)
			{
				return;
			}
			Managers.achievementManager.RegisterAchievement(Achievement.DidVoiceSearch);
			Managers.soundManager.Play("longWhoosh", this.transform, 0.3f, false, false);
			if (thingIds.Count >= 1)
			{
				if (thingIds.Count > this.searchResultsPerPage)
				{
					this.AddPagingUI();
				}
				this.searchResultThingIds = this.RandomizeExceptTopN(thingIds.ToArray(), 10);
				this.searchResultMaxPages = Mathf.CeilToInt((float)this.searchResultThingIds.Length / (float)this.searchResultsPerPage);
				this.DisplaySearchResults(true);
			}
			else
			{
				this.SetInfo("Nothing found for" + Environment.NewLine + term);
			}
		});
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x000451A0 File Offset: 0x000435A0
	private void DisplaySearchResults(bool useFallingEffect = false)
	{
		int num = this.currentSearchResultsPage * this.searchResultsPerPage;
		int num2 = Mathf.Min(num + this.searchResultsPerPage - 1, this.searchResultThingIds.Length - 1);
		int num3 = 0;
		int num4 = 0;
		for (int i = num; i <= num2; i++)
		{
			Vector3 zero = Vector3.zero;
			zero.x = (float)num3 * 0.65f - 1.3f;
			zero.y = 0.175f;
			zero.z = (float)(4 - num4) * 0.65f - 1.3f;
			InventoryItemData inventoryItemData = new InventoryItemData(this.searchResultThingIds[i], zero, Vector3.zero, 0.15f);
			Managers.thingManager.InstantiateInventoryItemViaCache(ThingRequestContext.LoadUniverseSearchResults, inventoryItemData, this.transform, true, useFallingEffect);
			if (++num3 > 4)
			{
				num3 = 0;
				num4++;
			}
		}
		this.HandleAdjustPositionForDesktop();
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00045278 File Offset: 0x00043678
	private string[] RandomizeExceptTopN(string[] items, int topN)
	{
		string[] array3;
		if (items.Length > topN)
		{
			string[] array = items.Take(topN).ToArray<string>();
			string[] array2 = items.Skip(topN).ToArray<string>();
			Misc.ShuffleArray<string>(array2);
			array3 = array.Concat(array2).ToArray<string>();
		}
		else
		{
			array3 = items;
		}
		return array3;
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x000452C8 File Offset: 0x000436C8
	private void ClearThings()
	{
		this.ClearInfo();
		IEnumerator enumerator = this.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.tag == "Thing")
				{
					global::UnityEngine.Object.Destroy(transform.gameObject);
				}
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
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0004534C File Offset: 0x0004374C
	private void SetArmAttachmentActive(bool isActive)
	{
		AttachmentPointId attachmentPointId = ((!(this.transform.parent != null) || !(this.transform.parent.name == "HandCoreLeft")) ? AttachmentPointId.ArmRight : AttachmentPointId.ArmLeft);
		Managers.personManager.ourPerson.SetAttachmentActive(attachmentPointId, isActive);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x000453A8 File Offset: 0x000437A8
	public void Close()
	{
		SpeechInput.DisposeListener();
		Managers.areaManager.ActivateAreaSpeechListener(true);
		this.SetArmAttachmentActive(true);
		if (this.transform.parent != null)
		{
			Hand component = this.transform.parent.GetComponent<Hand>();
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "inventory closed " + component.side.ToString().ToLower(), true);
		}
		this.SetCurrentDialogEnabledIfNeeded(true);
		this.transform.parent = null;
		this.transform.position = Vector3.zero;
		this.transform.rotation = Quaternion.identity;
		base.gameObject.tag = "Untagged";
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0004547C File Offset: 0x0004387C
	private void SetCurrentDialogEnabledIfNeeded(bool isEnabled)
	{
		if (this.hand != null && this.hand.currentDialog != null && this.hand.currentDialogType != DialogType.Keyboard)
		{
			this.hand.currentDialog.SetActive(isEnabled);
		}
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x000454D3 File Offset: 0x000438D3
	private void SwitchToNewPage(int newPage)
	{
		this.currentPage = newPage;
		this.LoadThings();
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x000454E4 File Offset: 0x000438E4
	public bool DoesThingAlreadyExist(string thingId)
	{
		bool flag = false;
		if (this.transform != null)
		{
			Component[] componentsInChildren = this.transform.GetComponentsInChildren(typeof(Thing), true);
			foreach (Thing thing in componentsInChildren)
			{
				if (thing.thingId == thingId)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00045558 File Offset: 0x00043958
	public void SaveThingPlacement(GameObject originalThing)
	{
		if (this.isShowingTrash)
		{
			global::UnityEngine.Object.Destroy(originalThing);
			Managers.soundManager.Play("no", this.transform, 1f, false, false);
			return;
		}
		this.cachedResponsesByPage.Remove(this.currentPage);
		if (originalThing.GetComponent<Thing>() != null)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(originalThing, originalThing.transform.position, originalThing.transform.rotation);
			gameObject.transform.parent = originalThing.transform.parent;
			gameObject.transform.localScale = originalThing.transform.localScale;
			Managers.thingManager.MakeDeepThingClone(originalThing, gameObject, true, false, false);
			Misc.Destroy(originalThing);
			Thing thing = gameObject.GetComponent<Thing>();
			thing.isInInventory = true;
			thing.isInInventoryOrDialog = true;
			thing.isLocked = false;
			thing.isInvisibleToEditors = false;
			GameObject thingByIdOnCurrentPage = this.GetThingByIdOnCurrentPage(thing.thingId);
			base.TriggerHapticPulseOnParentHand();
			if (thingByIdOnCurrentPage == null && this.GetThingCountOnCurrentPage() >= 100)
			{
				Managers.soundManager.Play("no", this.transform, 1f, false, false);
				global::UnityEngine.Object.Destroy(gameObject);
			}
			else
			{
				gameObject.tag = "Thing";
				gameObject.transform.parent = null;
				gameObject.transform.localScale = ((!thing.keepSizeInInventory) ? Managers.thingManager.GetAppropriateDownScaleForThing(gameObject, 0.1f, false) : Vector3.one);
				gameObject.transform.parent = this.transform;
				thing = gameObject.GetComponent<Thing>();
				thing.placementId = null;
				thing.isHeldAsHoldable = false;
				thing.isHeldAsHoldableByOurPerson = false;
				this.SetToAppropriatePosition(gameObject);
				InventoryItemData inventoryItemData = new InventoryItemData(gameObject.GetComponent<Thing>().thingId, gameObject.transform.localPosition, gameObject.transform.localEulerAngles, gameObject.transform.localScale.x);
				if (thingByIdOnCurrentPage != null)
				{
					base.StartCoroutine(Managers.serverManager.UpdateThingInInventory(this.currentPage, inventoryItemData, delegate(ExtendedServerResponse response)
					{
						if (response.ok)
						{
							Log.Info("Thing saved to inventory", false);
						}
						else if (string.IsNullOrEmpty(response.error))
						{
							Log.Error(response.error);
						}
						else
						{
							Log.Error(response.softError);
						}
					}));
					thingByIdOnCurrentPage.transform.localPosition = gameObject.transform.localPosition;
					thingByIdOnCurrentPage.transform.localRotation = gameObject.transform.localRotation;
					global::UnityEngine.Object.Destroy(gameObject);
				}
				else
				{
					base.StartCoroutine(Managers.serverManager.SaveThingToInventory(this.currentPage, inventoryItemData, delegate(ExtendedServerResponse response)
					{
						if (response.ok)
						{
							Log.Info("Thing saved to inventory", false);
						}
						else if (string.IsNullOrEmpty(response.error))
						{
							Log.Error(response.error);
						}
						else
						{
							Log.Error(response.softError);
						}
					}));
				}
				Managers.achievementManager.RegisterAchievement(Achievement.PlacedInInventory);
			}
		}
		else
		{
			global::UnityEngine.Object.Destroy(originalThing);
		}
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00045808 File Offset: 0x00043C08
	private int GetThingCountOnCurrentPage()
	{
		int num = 0;
		IEnumerator enumerator = this.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.CompareTag("Thing"))
				{
					num++;
				}
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
		return num;
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x00045880 File Offset: 0x00043C80
	private GameObject GetThingByIdOnCurrentPage(string thingId)
	{
		GameObject gameObject = null;
		Component[] componentsInChildren = this.transform.GetComponentsInChildren<Thing>();
		foreach (Thing thing in componentsInChildren)
		{
			if (thing.thingId == thingId)
			{
				gameObject = thing.gameObject;
				break;
			}
		}
		return gameObject;
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x000458E0 File Offset: 0x00043CE0
	private void SetToAppropriatePosition(GameObject thing)
	{
		Vector3 localPosition = thing.transform.localPosition;
		localPosition.x = Mathf.Clamp(localPosition.x, -1.7f, 1.7f);
		localPosition.y = 0.1f;
		localPosition.z = Mathf.Clamp(localPosition.z, -1.7f, 1.7f);
		thing.transform.localPosition = localPosition;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0004594C File Offset: 0x00043D4C
	public void DeleteThingPlacement(GameObject thing)
	{
		if (this.isShowingTrash || this.isShowingSearch)
		{
			Managers.soundManager.Play("no", this.transform, 1f, false, false);
			return;
		}
		this.cachedResponsesByPage.Remove(this.currentPage);
		Thing component = thing.GetComponent<Thing>();
		if (!component.alreadyDeleted)
		{
			component.alreadyDeleted = true;
			string thingId = component.thingId;
			base.StartCoroutine(Managers.serverManager.DeleteThingFromInventory(this.currentPage, thingId, delegate(ServerResponse response)
			{
				if (response.error != null)
				{
					Log.Error(response.error);
				}
			}));
		}
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x000459F4 File Offset: 0x00043DF4
	private void UpdatePageLabelAndToInventoryStartButton()
	{
		if (this.pageLabel != null)
		{
			global::UnityEngine.Object.Destroy(this.pageLabel.gameObject);
			this.pageLabel = null;
		}
		if (!this.isShowingTrash && !this.isShowingSearch)
		{
			string text = ((this.currentPage != 0) ? (this.currentPage + 1).ToString() : string.Empty);
			this.pageLabel = base.AddLabel(string.Empty, 0, 6830, 1f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			this.pageLabel.transform.localScale = Misc.GetUniformVector3(0.0011760001f);
			this.pageLabel.text = text;
			this.toInventoryStartButton.SetActive(this.currentPage != 0);
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00045AD0 File Offset: 0x00043ED0
	public void ToggleTrashShows()
	{
		if (!this.isWaitingForServerResponse)
		{
			this.isShowingTrash = !this.isShowingTrash;
			if (this.isShowingTrash)
			{
				this.ClearThings();
				this.RemovePagingUI();
				global::UnityEngine.Object.Destroy(this.switchToSearch);
				this.undoButton = base.AddButton("undoPlacementDeletion", null, "undo last deletion", "ButtonCompactNoIcon", 5750, 7100, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				this.isWaitingForServerResponse = true;
				base.StartCoroutine(Managers.serverManager.GetRecentlyDeletedThingIds(delegate(GetRecentlyDeletedThingIds_Response response)
				{
					List<string> thingIds = response.ThingIds;
					this.isWaitingForServerResponse = false;
					if (response.error == null)
					{
						Vector3 vector = new Vector3(-1.6f, 0f, 1.7f);
						foreach (string text in thingIds)
						{
							Vector3 vector2 = new Vector3(vector.x, vector.y, vector.z);
							InventoryItemData inventoryItemData = new InventoryItemData(text, vector2, Vector3.zero, 1f);
							Managers.thingManager.InstantiateInventoryItemViaCache(ThingRequestContext.InventoryDialogTrash, inventoryItemData, this.transform, false, false);
							vector.x += 0.55f;
							if (vector.x > 1.6f)
							{
								vector.x = -1.6f;
								vector.z -= 0.55f;
								if (vector.z < -1.4f)
								{
									break;
								}
							}
						}
					}
					else
					{
						Log.Error(response.error);
					}
				}));
			}
			else
			{
				this.AddPagingUI();
				this.AddSwitchToSearch();
				this.AddTrashButton();
				this.LoadThings();
			}
		}
		else
		{
			this.AddTrashButton();
		}
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00045BAC File Offset: 0x00043FAC
	public void GoToPage(int pageNumber)
	{
		if (pageNumber >= 0 && pageNumber < 100 && pageNumber != this.currentPage)
		{
			this.currentPage = pageNumber;
			this.UpdatePageLabelAndToInventoryStartButton();
			this.LoadThings();
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00045BDC File Offset: 0x00043FDC
	private void SearchResultsBack()
	{
		if (this.searchResultMaxPages > 0)
		{
			if (--this.currentSearchResultsPage < 0)
			{
				this.currentSearchResultsPage = this.searchResultMaxPages - 1;
			}
			this.ClearThings();
			this.DisplaySearchResults(false);
		}
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00045C28 File Offset: 0x00044028
	private void SearchResultsForward()
	{
		if (this.searchResultMaxPages > 0)
		{
			if (++this.currentSearchResultsPage > this.searchResultMaxPages - 1)
			{
				this.currentSearchResultsPage = 0;
			}
			this.ClearThings();
			this.DisplaySearchResults(false);
		}
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00045C73 File Offset: 0x00044073
	private void PageBack()
	{
		if (!this.isWaitingForServerResponse)
		{
			this.currentPage--;
			if (this.currentPage < 0)
			{
				this.currentPage = 99;
			}
			this.UpdatePageLabelAndToInventoryStartButton();
			this.LoadThings();
		}
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x00045CAE File Offset: 0x000440AE
	private void PageForward()
	{
		if (!this.isWaitingForServerResponse)
		{
			this.currentPage++;
			if (this.currentPage >= 100)
			{
				this.currentPage = 0;
			}
			this.UpdatePageLabelAndToInventoryStartButton();
			this.LoadThings();
		}
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00045CEC File Offset: 0x000440EC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "back":
			this.PageBack();
			break;
		case "forward":
			this.PageForward();
			break;
		case "searchResultsBack":
			this.SearchResultsBack();
			break;
		case "searchResultsForward":
			this.SearchResultsForward();
			break;
		case "toInventoryStart":
			if (!this.isWaitingForServerResponse && this.currentPage != 0)
			{
				this.currentPage = 0;
				this.UpdatePageLabelAndToInventoryStartButton();
				this.LoadThings();
			}
			break;
		case "undoPlacementDeletion":
			Managers.areaManager.UndoLastDeletion();
			break;
		case "trash":
			this.ToggleTrashShows();
			break;
		case "switchToSearch":
			this.SwitchToSearch();
			break;
		case "universalSearchByText":
		{
			Hand dialogHand = this.transform.parent.GetComponent<Hand>();
			this.ForceClose();
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text = text.Trim().ToLower();
					InventoryDialog.searchToTriggerOnNextOpening = text;
					this.CloseDialog();
					HandDot component = dialogHand.handDot.GetComponent<HandDot>();
					component.holdableInHand = Managers.inventoryManager.OpenDialog(dialogHand, true);
					Our.SetMode(EditModes.Inventory, false);
				}
				else
				{
					this.CloseDialog();
				}
			}, string.Empty, string.Empty, 100, string.Empty, true, false, false, false, 1f, false, false, dialogHand, false);
			break;
		}
		case "editSearchWords":
			this.ForceClose();
			base.SwitchTo(DialogType.InventorySearchWords, string.Empty);
			break;
		case "searchHelp":
			this.ForceClose();
			Managers.dialogManager.ShowInfo("Inventory search finds things across the universe by their name or tags, but only if they were placed in a public area, are still placed somewhere, and are not set to Unlisted in the Tag dialog. Items which are only collected or only ever placed in private areas do not appear.", true, true, 0, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
			break;
		case "close":
			if (this.isShowingSearch)
			{
				this.SwitchToNormalInventory();
			}
			else
			{
				this.ForceClose();
			}
			break;
		}
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00045F40 File Offset: 0x00044340
	private void HandleAdjustPositionForDesktop()
	{
		if (CrossDevice.desktopMode)
		{
			Transform transform = Managers.treeManager.GetTransform("/OurPersonRig/HeadCore");
			Transform parent = this.transform.parent;
			this.transform.parent = transform;
			this.transform.localPosition = new Vector3(-0.15f, 0.025f, 0.55f);
			this.transform.localEulerAngles = new Vector3(-80f, 0f, 0f);
			this.transform.parent = parent;
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x00045FC9 File Offset: 0x000443C9
	private void ForceClose()
	{
		if (Our.mode == EditModes.Inventory)
		{
			Our.SetPreviousMode();
		}
		this.hand.handDot.GetComponent<HandDot>().holdableInHand = null;
		this.Close();
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x00045FF8 File Offset: 0x000443F8
	private void AddTrashButton()
	{
		if (this.trashButton != null)
		{
			global::UnityEngine.Object.Destroy(this.trashButton);
		}
		this.trashButton = base.AddModelButton("Trash", "trash", null, -7000, 7200, false);
		if (this.isShowingTrash)
		{
			Transform transform = this.trashButton.transform.Find("Bin");
			base.ApplyEmissionColor(transform, this.isShowingTrash);
		}
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x00046074 File Offset: 0x00044474
	private void RemovePagingUI()
	{
		if (this.backButton != null)
		{
			global::UnityEngine.Object.Destroy(this.backButton);
		}
		if (this.forwardButton != null)
		{
			global::UnityEngine.Object.Destroy(this.forwardButton);
		}
		if (this.toInventoryStartButton != null)
		{
			global::UnityEngine.Object.Destroy(this.toInventoryStartButton);
		}
		this.UpdatePageLabelAndToInventoryStartButton();
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x000460DB File Offset: 0x000444DB
	private void OnDestroy()
	{
		SpeechInput.DisposeListener();
	}

	// Token: 0x04000784 RID: 1924
	public bool defaultViewNeedsCloseButton;

	// Token: 0x04000785 RID: 1925
	private bool wasOpenedBefore;

	// Token: 0x04000786 RID: 1926
	private const int firstPage = 0;

	// Token: 0x04000787 RID: 1927
	private int currentPage;

	// Token: 0x04000788 RID: 1928
	private const int maxThingsPerPage = 100;

	// Token: 0x04000789 RID: 1929
	public const int maxPages = 100;

	// Token: 0x0400078A RID: 1930
	private Dictionary<int, List<InventoryItemData>> cachedResponsesByPage = new Dictionary<int, List<InventoryItemData>>();

	// Token: 0x0400078B RID: 1931
	public bool isWaitingForServerResponse;

	// Token: 0x0400078C RID: 1932
	private TextMesh pageLabel;

	// Token: 0x0400078D RID: 1933
	private GameObject backButton;

	// Token: 0x0400078E RID: 1934
	private GameObject forwardButton;

	// Token: 0x0400078F RID: 1935
	private GameObject toInventoryStartButton;

	// Token: 0x04000790 RID: 1936
	private GameObject trashButton;

	// Token: 0x04000791 RID: 1937
	private GameObject undoButton;

	// Token: 0x04000792 RID: 1938
	private GameObject closeButton;

	// Token: 0x04000793 RID: 1939
	private GameObject searchWordsButton;

	// Token: 0x04000794 RID: 1940
	private GameObject searchHelpButton;

	// Token: 0x04000795 RID: 1941
	private GameObject switchToSearch;

	// Token: 0x04000796 RID: 1942
	private GameObject editSearchButton;

	// Token: 0x04000797 RID: 1943
	private const int edge = 7000;

	// Token: 0x04000798 RID: 1944
	public static string searchToTriggerOnNextOpening;

	// Token: 0x04000799 RID: 1945
	private const int searchResultMaxX = 4;

	// Token: 0x0400079A RID: 1946
	private const int searchResultMaxY = 4;

	// Token: 0x0400079B RID: 1947
	private int searchResultsPerPage = 25;

	// Token: 0x0400079C RID: 1948
	private int searchResultMaxPages = -1;

	// Token: 0x0400079D RID: 1949
	private TextMesh infoLabel;

	// Token: 0x0400079E RID: 1950
	private int currentSearchResultsPage;

	// Token: 0x0400079F RID: 1951
	private string[] searchResultThingIds;
}
