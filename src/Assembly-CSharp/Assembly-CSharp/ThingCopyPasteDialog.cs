using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class ThingCopyPasteDialog : Dialog
{
	// Token: 0x06000BFB RID: 3067 RVA: 0x00067A04 File Offset: 0x00065E04
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		base.AddHeadline("Copy & Paste", -440, -460, TextColor.Default, TextAlignment.Left, false);
		this.AddButtons();
		if (this.thing.isLocked)
		{
			this.AddLockButton();
		}
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x00067A62 File Offset: 0x00065E62
	private void AddButtons()
	{
		base.StartCoroutine(this.DoAddButtons());
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x00067A74 File Offset: 0x00065E74
	private IEnumerator DoAddButtons()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int y = -110;
		TextColor defaultColor = TextColor.Default;
		TextColor ghostedColor = TextColor.Gray;
		TransformClipboard clipboard = CreationHelper.transformClipboard;
		base.AddLabel("Position", -435, y + -21, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("clipboard_copyPosition", null, "Copy ➜", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		string text = "clipboard_pastePosition";
		string text2 = null;
		string text3 = "➜ Paste";
		string text4 = "ButtonCompactNoIconShortCentered";
		int num = 220;
		int num2 = y;
		Vector3? position = clipboard.position;
		TextColor textColor = ((position == null || this.thing.isLocked) ? ghostedColor : defaultColor);
		base.AddButton(text, text2, text3, text4, num, num2, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		y += 100;
		base.AddLabel("Rotation", -435, y + -21, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("clipboard_copyRotation", null, "Copy ➜", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		text4 = "clipboard_pasteRotation";
		text3 = null;
		text2 = "➜ Paste";
		text = "ButtonCompactNoIconShortCentered";
		num2 = 220;
		num = y;
		Vector3? rotation = clipboard.rotation;
		textColor = ((rotation == null || this.thing.isLocked) ? ghostedColor : defaultColor);
		base.AddButton(text4, text3, text2, text, num2, num, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		y += 100;
		base.AddLabel("Size", -435, y + -21, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("clipboard_copyScale", null, "Copy ➜", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		text = "clipboard_pasteScale";
		text2 = null;
		text3 = "➜ Paste";
		text4 = "ButtonCompactNoIconShortCentered";
		num = 220;
		num2 = y;
		Vector3? scale = clipboard.scale;
		textColor = ((scale == null || this.thing.isLocked) ? ghostedColor : defaultColor);
		base.AddButton(text, text2, text3, text4, num, num2, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		y += 220;
		base.AddLabel("Thing Id", -435, y + -21, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("clipboard_copyThingId", null, "Copy", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x00067A90 File Offset: 0x00065E90
	private void AddLockButton()
	{
		if (Managers.areaManager.WeCanChangeLocks())
		{
			GameObject gameObject = base.AddModelButton("Lock", "toggleLock", null, 225, -433, false);
			DialogPart component = gameObject.GetComponent<DialogPart>();
			component.autoStopHighlight = false;
			component.state = this.thing.isLocked;
			base.ApplyEmissionColorToShape(gameObject, this.thing.isLocked);
		}
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x00067AFC File Offset: 0x00065EFC
	private void ToggleLock(bool state)
	{
		this.thing.isLocked = state;
		this.AddButtons();
		if (this.thing.isLocked)
		{
			Managers.areaManager.SetPlacementAttribute(this.thing.placementId, PlacementAttribute.Locked, delegate(bool ok)
			{
				if (this != null && ok)
				{
					Managers.soundManager.Play("success", this.transform, 0.1f, false, false);
				}
			});
		}
		else
		{
			Managers.areaManager.ClearPlacementAttribute(this.thing.placementId, PlacementAttribute.Locked, delegate(bool ok)
			{
				if (this != null && ok)
				{
					Managers.soundManager.Play("pickUp", this.transform, 0.2f, false, false);
				}
			});
		}
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x00067B74 File Offset: 0x00065F74
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (this.thing == null)
		{
			base.CloseDialog();
		}
		switch (contextName)
		{
		case "clipboard_copyPosition":
			CreationHelper.transformClipboard.position = new Vector3?(this.thing.transform.position);
			Managers.soundManager.Play("pickUp", this.transform, 0.35f, true, false);
			this.AddButtons();
			break;
		case "clipboard_copyRotation":
			CreationHelper.transformClipboard.rotation = new Vector3?(this.thing.transform.eulerAngles);
			Managers.soundManager.Play("pickUp", this.transform, 0.35f, true, false);
			this.AddButtons();
			break;
		case "clipboard_copyScale":
			CreationHelper.transformClipboard.scale = new Vector3?(this.thing.transform.localScale);
			Managers.soundManager.Play("pickUp", this.transform, 0.35f, true, false);
			this.AddButtons();
			break;
		case "clipboard_copyThingId":
			GUIUtility.systemCopyBuffer = this.thing.thingId;
			Managers.soundManager.Play("pickUp", this.transform, 0.35f, true, false);
			break;
		case "clipboard_pastePosition":
			if (!this.thing.isLocked)
			{
				Vector3? position = CreationHelper.transformClipboard.position;
				if (position != null)
				{
					Transform transform = this.thing.transform;
					Vector3? position2 = CreationHelper.transformClipboard.position;
					transform.position = position2.Value;
					Managers.areaManager.UpdateThingPlacement(this.thing.gameObject);
					Managers.soundManager.Play("putDown", this.transform, 0.35f, true, false);
					break;
				}
			}
			Managers.soundManager.Play("no", this.transform, 0.35f, true, false);
			break;
		case "clipboard_pasteRotation":
			if (!this.thing.isLocked)
			{
				Vector3? rotation = CreationHelper.transformClipboard.rotation;
				if (rotation != null)
				{
					Transform transform2 = this.thing.transform;
					Vector3? rotation2 = CreationHelper.transformClipboard.rotation;
					transform2.eulerAngles = rotation2.Value;
					Managers.areaManager.UpdateThingPlacement(this.thing.gameObject);
					Managers.soundManager.Play("putDown", this.transform, 0.35f, true, false);
					break;
				}
			}
			Managers.soundManager.Play("no", this.transform, 0.35f, true, false);
			break;
		case "clipboard_pasteScale":
			if (!this.thing.isLocked)
			{
				Vector3? scale = CreationHelper.transformClipboard.scale;
				if (scale != null)
				{
					Transform transform3 = this.thing.transform;
					Vector3? scale2 = CreationHelper.transformClipboard.scale;
					transform3.localScale = scale2.Value;
					Managers.areaManager.UpdateThingPlacement(this.thing.gameObject);
					Managers.soundManager.Play("putDown", this.transform, 0.35f, true, false);
					break;
				}
			}
			Managers.soundManager.Play("no", this.transform, 0.35f, true, false);
			break;
		case "toggleLock":
			this.ToggleLock(state);
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x0400090E RID: 2318
	public Thing thing;

	// Token: 0x0400090F RID: 2319
	private const float volume = 0.35f;
}
