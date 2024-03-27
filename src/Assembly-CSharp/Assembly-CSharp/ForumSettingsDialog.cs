using System;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class ForumSettingsDialog : Dialog
{
	// Token: 0x060009B7 RID: 2487 RVA: 0x000400AD File Offset: 0x0003E4AD
	public void Start()
	{
		base.Init(base.gameObject, false, true, false);
		base.AddBackButton();
		Managers.forumManager.GetForumInfo(Managers.forumManager.currentForumId, delegate(string reasonFailed, ForumData forum)
		{
			if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
			{
				Managers.forumManager.currentForumData = forum;
				this.rights = new ForumRights(forum);
				base.AddDialogThingFundamentIfNeeded();
				base.AddHeadline(forum.name, -370, -460, TextColor.Default, TextAlignment.Left, false);
				string text;
				if (string.IsNullOrEmpty(forum.description))
				{
					text = ((!this.rights.isModerator) ? string.Empty : "description");
				}
				else
				{
					text = Misc.WrapWithNewlines(forum.description, 60, 3);
				}
				if (this.rights.isModerator)
				{
					base.AddModelButton("EditTextButton", "editForumDescription", null, -880, -800, false);
				}
				base.AddLabel(text, -840, -810, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				base.AddLabel("Editor: " + Misc.Truncate(forum.creatorName, 40, true), 420, -940, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				int num = 0;
				base.AddCheckbox("toggleFavorite", null, "★ favorite", 0, -500 + num++ * 115, forum.user_hasFavorited, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
				if (this.rights.isModerator)
				{
					base.AddSeparator(0, -500 + num++ * 115, false);
					num++;
					float num2 = 0.75f;
					base.AddCheckbox("rights", "0", "Editors can moderate", 0, -500 + num++ * 115, forum.protectionLevel == 0, num2, "Checkbox", TextColor.Default, null, ExtraIcon.None);
					base.AddCheckbox("rights", "1", "Only ed. add topics (+ all above)", 0, -500 + num++ * 115, forum.protectionLevel == 1, num2, "Checkbox", TextColor.Default, null, ExtraIcon.None);
					base.AddCheckbox("rights", "2", "Only ed. add comments (+ all above)", 0, -500 + num++ * 115, forum.protectionLevel == 2, num2, "Checkbox", TextColor.Default, null, ExtraIcon.None);
					base.AddCheckbox("rights", "3", "Only ed. can read (+ all above)", 0, -500 + num++ * 115, forum.protectionLevel == 3, num2, "Checkbox", TextColor.Default, null, ExtraIcon.None);
					if (Managers.areaManager.weAreEditorOfCurrentArea)
					{
						int num3 = -500 + num * 115 + 230;
						base.AddSeparator(0, num3, false);
						base.AddButton("createDialogThing", null, "create board dialog", "ButtonBig", 0, num3 + 150, "createThing", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
						base.AddLabel("or context-laser a board dialog\ncreation now to add it", -380, num3 + 280, 1f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
					}
				}
			}
		});
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x000400E4 File Offset: 0x0003E4E4
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "rights"))
			{
				if (!(contextName == "editForumDescription"))
				{
					if (!(contextName == "toggleFavorite"))
					{
						if (!(contextName == "createDialogThing"))
						{
							if (contextName == "back")
							{
								base.SwitchTo(DialogType.Forum, string.Empty);
							}
						}
						else
						{
							CreationHelper.showDialogShapesTab = true;
							CreationHelper.shapesTab = 10;
							Our.SetMode(EditModes.Thing, false);
							base.SwitchTo(DialogType.Create, string.Empty);
						}
					}
					else
					{
						Managers.forumManager.ToggleFavoriteForum(Managers.forumManager.currentForumId, delegate(string reasonFailed, bool isFavorited)
						{
							if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
							{
								string text2 = ((!isFavorited) ? "pickUp" : "success");
								Managers.soundManager.Play(text2, this.transform, 0.2f, false, false);
							}
						});
					}
				}
				else
				{
					Managers.dialogManager.GetInput(delegate(string text)
					{
						if (text == null)
						{
							base.SwitchTo(DialogType.ForumSettings, string.Empty);
						}
						else
						{
							Managers.forumManager.EditForumInfo(Managers.forumManager.currentForumId, text, delegate(string reasonFailed)
							{
								if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
								{
									base.SwitchTo(DialogType.ForumSettings, string.Empty);
								}
							});
						}
					}, contextName, Managers.forumManager.currentForumData.description, 500, "forum description", true, false, false, false, 1f, false, false, null, false);
				}
			}
			else
			{
				int num = int.Parse(contextId);
				Managers.forumManager.SetForumProtectionLevel(Managers.forumManager.currentForumId, num, delegate(string reasonFailed)
				{
					if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
					{
						Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
						base.SwitchTo(DialogType.ForumSettings, string.Empty);
					}
				});
			}
		}
	}

	// Token: 0x0400074A RID: 1866
	private ForumRights rights;
}
