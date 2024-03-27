using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class ForumThreadSettingsDialog : Dialog
{
	// Token: 0x060009DF RID: 2527 RVA: 0x00042CB5 File Offset: 0x000410B5
	public void Start()
	{
		base.Init(base.gameObject, false, true, false);
		base.AddBackButton();
		base.AddDialogThingFundamentIfNeeded();
		Managers.forumManager.GetForumThreadInfo(Managers.forumManager.currentForumThreadId, delegate(string reasonFailed, ForumData forum, ForumThreadData thread)
		{
			if (this != null && !Managers.forumManager.HandleFailIfNeeded(reasonFailed))
			{
				this.rights = new ForumRights(Managers.forumManager.currentForumData);
				base.AddHeadline(thread.title, -370, -460, TextColor.Default, TextAlignment.Left, false);
				if (this.rights.isModerator)
				{
					this.ShowSettings(thread);
				}
			}
		});
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00042CF4 File Offset: 0x000410F4
	private void ShowSettings(ForumThreadData data)
	{
		int num = 0;
		this.stickyButton = base.AddCheckbox("toggleSticky", null, "stickify topic", 0, -300 + num++ * 130, data.isSticky, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		this.lockedButton = base.AddCheckbox("toggleLocked", null, "lock topic", 0, -300 + num++ * 130, data.isLocked, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddModelButton("EditTextButton", "clarifyForumThreadTitle", null, 580, -937, false);
		base.AddLabel("clarify title", 630, -947, 0.95f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.deleteButton = base.AddButton("deleteIfAsked", null, "delete topic...", "ButtonCompactNoIcon", -700, 880, null, false, 1f, TextColor.Red, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00042E08 File Offset: 0x00041208
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "toggleSticky"))
			{
				if (!(contextName == "toggleLocked"))
				{
					if (!(contextName == "clarifyForumThreadTitle"))
					{
						if (!(contextName == "deleteIfAsked"))
						{
							if (contextName == "back")
							{
								base.SwitchTo(DialogType.ForumThread, string.Empty);
							}
						}
						else if (this.askedIfReallyDelete)
						{
							Managers.forumManager.RemoveForumThread(Managers.forumManager.currentForumThreadId, delegate(string reasonFailed)
							{
								if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
								{
									Managers.forumManager.currentForumThreadId = null;
									Managers.soundManager.Play("delete", this.transform, 0.1f, false, false);
									base.SwitchTo(DialogType.Forum, string.Empty);
								}
							});
						}
						else
						{
							base.SetText(this.deleteButton, "Delete topic & replies?", 1f, null, false);
							this.askedIfReallyDelete = true;
						}
					}
					else
					{
						Managers.dialogManager.GetInput(delegate(string text)
						{
							if (text == null)
							{
								base.SwitchTo(DialogType.ForumThreadSettings, string.Empty);
							}
							else
							{
								Managers.forumManager.ClarifyForumThreadTitle(Managers.forumManager.currentForumThreadId, text, delegate(string reasonFailed)
								{
									if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
									{
										base.SwitchTo(DialogType.ForumThread, string.Empty);
									}
								});
							}
						}, contextName, string.Empty, 40, "clarification", false, false, false, false, 1f, false, false, null, false);
					}
				}
				else if (!this.currentlyBeingToggled)
				{
					this.currentlyBeingToggled = true;
					Managers.forumManager.ToggleForumThreadLocked(Managers.forumManager.currentForumThreadId, delegate(string reasonFailed, bool isLocked)
					{
						base.SetCheckboxState(this.lockedButton, isLocked, true);
						this.currentlyBeingToggled = false;
					});
				}
			}
			else if (!this.currentlyBeingToggled)
			{
				this.currentlyBeingToggled = true;
				Managers.forumManager.ToggleForumThreadSticky(Managers.forumManager.currentForumThreadId, delegate(string reasonFailed, bool isSticky)
				{
					base.SetCheckboxState(this.stickyButton, isSticky, true);
					this.currentlyBeingToggled = false;
				});
			}
		}
	}

	// Token: 0x04000763 RID: 1891
	private bool currentlyBeingToggled;

	// Token: 0x04000764 RID: 1892
	private GameObject stickyButton;

	// Token: 0x04000765 RID: 1893
	private GameObject lockedButton;

	// Token: 0x04000766 RID: 1894
	private GameObject deleteButton;

	// Token: 0x04000767 RID: 1895
	private bool askedIfReallyDelete;

	// Token: 0x04000768 RID: 1896
	private ForumRights rights;
}
