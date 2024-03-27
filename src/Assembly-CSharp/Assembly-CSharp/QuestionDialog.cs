using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class QuestionDialog : Dialog
{
	// Token: 0x06000B29 RID: 2857 RVA: 0x0005BB60 File Offset: 0x00059F60
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		string text = string.Empty;
		string text2 = "yes";
		string text3 = "no";
		QuestionDialog.Mode mode = QuestionDialog.mode;
		if (mode == QuestionDialog.Mode.RetrievePlacedSubThings)
		{
			text = "Place sub-things of creation here?";
		}
		text = Misc.WrapWithNewlines(text, 36, -1);
		base.AddLabel(text, -435, -200, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton(text2, null, "yes", "Button", -235, 280, "checkmark", false, 1f, TextColor.Green, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddButton(text3, null, "no", "Button", 235, 280, "cross", false, 1f, TextColor.Red, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0005BC6C File Offset: 0x0005A06C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		QuestionDialog.Mode mode = QuestionDialog.mode;
		if (mode == QuestionDialog.Mode.RetrievePlacedSubThings)
		{
			if (contextName == "yes")
			{
				Managers.thingManager.PlacePlacedSubThingsAsTheyWereOriginallyPositioned(CreationHelper.thingBeingEdited);
			}
			base.SwitchTo(DialogType.Create, string.Empty);
		}
	}

	// Token: 0x04000879 RID: 2169
	public static QuestionDialog.Mode mode;

	// Token: 0x0400087A RID: 2170
	private bool didStart;

	// Token: 0x02000129 RID: 297
	public enum Mode
	{
		// Token: 0x0400087C RID: 2172
		None,
		// Token: 0x0400087D RID: 2173
		RetrievePlacedSubThings
	}
}
