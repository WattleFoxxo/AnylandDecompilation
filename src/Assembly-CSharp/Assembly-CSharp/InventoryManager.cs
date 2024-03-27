using System;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class InventoryManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001EF RID: 495
	// (get) Token: 0x0600112B RID: 4395 RVA: 0x00094141 File Offset: 0x00092541
	// (set) Token: 0x0600112C RID: 4396 RVA: 0x00094149 File Offset: 0x00092549
	public ManagerStatus status { get; private set; }

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x0600112D RID: 4397 RVA: 0x00094152 File Offset: 0x00092552
	// (set) Token: 0x0600112E RID: 4398 RVA: 0x0009415A File Offset: 0x0009255A
	public string failMessage { get; private set; }

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x0600112F RID: 4399 RVA: 0x00094163 File Offset: 0x00092563
	public string[] InventorySearchWords
	{
		get
		{
			return SearchWords.GetAllWords();
		}
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x0009416A File Offset: 0x0009256A
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x0009417C File Offset: 0x0009257C
	public GameObject OpenDialog(Hand hand, bool defaultViewNeedsCloseButton = false)
	{
		if (this.dialog == null)
		{
			this.CreateDialog();
		}
		this.dialog.transform.parent = hand.transform;
		this.dialog.defaultViewNeedsCloseButton = defaultViewNeedsCloseButton;
		return this.dialog.Open(hand);
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x000941CE File Offset: 0x000925CE
	public void CloseDialog()
	{
		if (this.dialog != null)
		{
			this.dialog.Close();
		}
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x000941EC File Offset: 0x000925EC
	public bool CheckIfMayOpenAndAlertIfNot(Hand hand)
	{
		bool flag = !Managers.areaManager.onlyEditorsCanUseInventory || Managers.areaManager.weAreEditorOfCurrentArea;
		if (!flag)
		{
			Managers.soundManager.Play("no", hand.transform, 0.2f, false, false);
			Managers.dialogManager.ShowInfo("This area currently disallows using the inventory.", false, true, 1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
		}
		return flag;
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x00094256 File Offset: 0x00092656
	public bool IsInsideScopeCube()
	{
		return this.scopeCube != null && Our.mode == EditModes.Inventory && this.scopeCube.isInside;
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x00094282 File Offset: 0x00092682
	public bool IsInventoryModeButOutsideScopeCube()
	{
		return this.scopeCube != null && Our.mode == EditModes.Inventory && !this.scopeCube.isInside;
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x000942B1 File Offset: 0x000926B1
	public void SaveThingPlacement(GameObject thing)
	{
		if (this.dialog != null)
		{
			this.dialog.SaveThingPlacement(thing);
		}
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x000942D0 File Offset: 0x000926D0
	public bool IsShowingNormalInventory()
	{
		return !this.IsShowingTrash() && !this.IsShowingSearch();
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x000942E9 File Offset: 0x000926E9
	public bool IsShowingTrash()
	{
		return this.dialog != null && this.dialog.isShowingTrash;
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x0009430A File Offset: 0x0009270A
	private bool IsShowingSearch()
	{
		return this.dialog != null && this.dialog.isShowingSearch;
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x0009432B File Offset: 0x0009272B
	public void DeleteThingPlacement(GameObject thing)
	{
		if (this.dialog != null)
		{
			this.dialog.DeleteThingPlacement(thing);
		}
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x0009434A File Offset: 0x0009274A
	public void CloseTrashIfNeeded()
	{
		if (this.IsShowingTrash())
		{
			this.dialog.ToggleTrashShows();
		}
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00094362 File Offset: 0x00092762
	public void GoToPage(int pageNumber)
	{
		if (this.dialog != null)
		{
			this.dialog.GoToPage(pageNumber);
		}
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x00094384 File Offset: 0x00092784
	private void CreateDialog()
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("Dialogs/Inventory", typeof(GameObject))) as GameObject;
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		this.dialog = gameObject.GetComponent<InventoryDialog>();
		this.scopeCube = gameObject.transform.Find("InventoryScopeCube").GetComponent<InventoryScopeCube>();
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x000943E8 File Offset: 0x000927E8
	public void ActivateInventorySpeechListener()
	{
		SpeechInput.InitListener(this.InventorySearchWords, new SpeechInput.SpeechRecognizedHandler(this.dialog.OnSpeechRecognized));
	}

	// Token: 0x04001107 RID: 4359
	private InventoryDialog dialog;

	// Token: 0x04001108 RID: 4360
	private InventoryScopeCube scopeCube;
}
