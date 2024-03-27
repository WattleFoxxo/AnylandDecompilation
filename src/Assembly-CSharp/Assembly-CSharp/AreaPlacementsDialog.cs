using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000101 RID: 257
public class AreaPlacementsDialog : Dialog
{
	// Token: 0x06000918 RID: 2328 RVA: 0x00037218 File Offset: 0x00035618
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Area Placements", -370, -460, TextColor.Default, TextAlignment.Left, false);
		base.AddGenericHelpButton();
		if (Managers.areaManager.weAreOwnerOfCurrentArea || Managers.areaManager.isCopyable)
		{
			base.AddButton("copy", null, "copy", "ButtonCompact", -235, -120, "copyArea", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		if (Managers.areaManager.weAreOwnerOfCurrentArea)
		{
			base.AddSeparator(0, 250, false);
			this.deleteButton = base.AddButton("delete", null, "delete", "ButtonCompact", 0, 350, "clear", false, 1f, TextColor.Red, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		base.AddButton("findCopyableAreas", null, "find copyable areas...", "ButtonCompactSmallIcon", -235, 130, "search", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		this.UpdatePasteButton();
		this.UpdateCurrentlyCopiedLabel();
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0003737C File Offset: 0x0003577C
	private void UpdatePasteButton()
	{
		if (Managers.areaManager.weAreOwnerOfCurrentArea && !string.IsNullOrEmpty(Managers.areaManager.currentlyCopiedAreaId) && Managers.areaManager.currentlyCopiedAreaId != Managers.areaManager.currentAreaId)
		{
			Managers.areaManager.GetInfo(delegate(AreaInfo areaInfo)
			{
				this.copiedAreaWasAlreadyPastedToHere = this.GetCopiedAreaWasAlreadyPastedToHere(areaInfo.copiedFromAreas);
				if (this.copiedAreaWasAlreadyPastedToHere)
				{
					this.UpdateCurrentlyCopiedLabel();
				}
				else
				{
					this.pasteButton = base.AddButton("paste", null, "paste", "ButtonCompact", 235, -120, "pasteArea", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
			});
		}
		else
		{
			global::UnityEngine.Object.Destroy(this.pasteButton);
		}
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x000373F0 File Offset: 0x000357F0
	private bool GetCopiedAreaWasAlreadyPastedToHere(List<AreaIdNameAndCreatorId> copiedFromAreas)
	{
		bool flag = false;
		foreach (AreaIdNameAndCreatorId areaIdNameAndCreatorId in copiedFromAreas)
		{
			if (areaIdNameAndCreatorId.id == Managers.areaManager.currentlyCopiedAreaId)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00037464 File Offset: 0x00035864
	private void UpdateCurrentlyCopiedLabel()
	{
		if (this.currentlyCopiedLabel == null)
		{
			this.currentlyCopiedLabel = base.AddLabel(string.Empty, -430, -50, 0.7f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		string text = string.Empty;
		if (!string.IsNullOrEmpty(Managers.areaManager.currentlyCopiedAreaId))
		{
			text = "Currently copied: " + Managers.areaManager.currentlyCopiedAreaName;
			text = Misc.WrapWithNewlines(text, 50, -1);
			if (this.copiedAreaWasAlreadyPastedToHere)
			{
				text = text + Environment.NewLine + "(already pasted here)";
			}
		}
		this.currentlyCopiedLabel.text = text.ToUpper();
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00037511 File Offset: 0x00035911
	private bool NeedsConfirmToPreventAccidents()
	{
		return Managers.thingManager.placements.transform.childCount > 1;
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0003752C File Offset: 0x0003592C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "copy":
			Managers.areaManager.CopyCurrentArea();
			Managers.soundManager.Play("pickUp", this.transform, 0.5f, false, false);
			this.UpdateCurrentlyCopiedLabel();
			this.UpdatePasteButton();
			break;
		case "paste":
			global::UnityEngine.Object.Destroy(this.pasteButton);
			if (this.NeedsConfirmToPreventAccidents())
			{
				Managers.dialogManager.GetInput(delegate(string text)
				{
					if (text == "paste")
					{
						Managers.soundManager.Play("putDown", this.transform, 0.5f, false, false);
						Managers.dialogManager.ShowInfo("Ok, currently pasting...", false, true, 0, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
						Managers.areaManager.PasteCurrentlyCopiedArea();
					}
					else
					{
						if (text != null)
						{
							Managers.soundManager.Play("no", this.transform, 0.5f, false, false);
						}
						base.CloseDialog();
					}
				}, "pasteAreaPlacements", string.Empty, 20, "type \"paste\" to confirm pasting all (cannot be undone)", false, false, false, false, 0.85f, false, false, null, false);
			}
			else
			{
				Managers.soundManager.Play("putDown", this.hand.transform, 0.5f, false, false);
				Managers.dialogManager.ShowInfo("Ok, currently pasting...", false, true, 0, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				Managers.areaManager.PasteCurrentlyCopiedArea();
			}
			break;
		case "delete":
			global::UnityEngine.Object.Destroy(this.deleteButton);
			if (this.NeedsConfirmToPreventAccidents())
			{
				Managers.dialogManager.GetInput(delegate(string text)
				{
					if (text == "delete")
					{
						Managers.soundManager.Play("delete", this.transform, 0.5f, false, false);
						Managers.dialogManager.ShowInfo("Ok, currently deleting...", false, true, 0, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
						Managers.areaManager.PermanentlyDeleteAllPlacementsInArea();
					}
					else
					{
						if (text != null)
						{
							Managers.soundManager.Play("no", this.transform, 0.5f, false, false);
						}
						base.CloseDialog();
					}
				}, "deleteAreaPlacements", string.Empty, 20, "type \"delete\" to confirm deleting all (cannot be undone)", false, false, false, false, 0.85f, false, false, null, false);
			}
			else
			{
				Managers.soundManager.Play("delete", this.hand.transform, 0.5f, false, false);
				Managers.dialogManager.ShowInfo("Ok, currently deleting...", false, true, 0, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				Managers.areaManager.PermanentlyDeleteAllPlacementsInArea();
			}
			break;
		case "findCopyableAreas":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				Our.lastAreasSearchText = text;
				if (string.IsNullOrEmpty(Our.lastAreasSearchText))
				{
					base.SwitchTo(DialogType.Areas, string.Empty);
					Our.lastAreasSearchText = null;
				}
				else
				{
					base.SwitchTo(DialogType.FindAreas, string.Empty);
				}
			}, "findAreas", "copyable", 60, "you can use \"copyable\" plus optional keywords", false, false, false, false, 1f, false, false, null, false);
			break;
		case "help":
			Managers.browserManager.OpenGuideBrowser("vpocr8BJXw8", null);
			break;
		case "back":
			base.SwitchTo(DialogType.Area, string.Empty);
			break;
		case "close":
			Our.areaIdOfInterest = null;
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x040006DE RID: 1758
	private TextMesh currentlyCopiedLabel;

	// Token: 0x040006DF RID: 1759
	private GameObject pasteButton;

	// Token: 0x040006E0 RID: 1760
	private GameObject deleteButton;

	// Token: 0x040006E1 RID: 1761
	private bool copiedAreaWasAlreadyPastedToHere;
}
