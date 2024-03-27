using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class ProfileDialog : Dialog
{
	// Token: 0x06000B03 RID: 2819 RVA: 0x0005767C File Offset: 0x00055A7C
	public void Start()
	{
		this.weAreResized = Managers.personManager.WeAreResized();
		if (this.weAreResized)
		{
			base.gameObject.SetActive(false);
		}
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		if (!Universe.features.profileDialog)
		{
			Our.dialogToGoBackTo = DialogType.None;
			base.CloseDialog();
			return;
		}
		float num = 15f;
		if (Managers.optimizationManager.findOptimizations)
		{
			num *= 2f;
		}
		Managers.personManager.AddOrTimeExtendNameTagsForAllOthers(num);
		this.personThisIsOf = null;
		if (this.hand.lastContextInfoHit != null)
		{
			this.personThisIsOf = Managers.personManager.GetPersonThisObjectIsOf(this.hand.lastContextInfoHit);
			if (this.personThisIsOf != null)
			{
				Our.personIdOfInterest = this.personThisIsOf.userId;
			}
		}
		if (Our.personIdOfInterest != null)
		{
			this.personId = Our.personIdOfInterest;
			if (this.personId == Managers.personManager.ourPerson.userId)
			{
				base.SwitchTo(DialogType.OwnProfile, string.Empty);
				return;
			}
			Managers.personManager.GetPersonInfo(Our.personIdOfInterest, delegate(PersonInfo personInfo)
			{
				if (this == null)
				{
					return;
				}
				bool flag = Our.dialogToGoBackTo != DialogType.None;
				personInfo.screenName = Managers.personManager.GetScreenNameWithDiscloser(personInfo.id, personInfo.screenName);
				this.personName = personInfo.screenName;
				base.AddPersonInfo(this.personThisIsOf, personInfo.screenName, personInfo.statusText, personInfo.age, false, flag, personInfo.isOnline);
				this.theyAreFriend = personInfo.isFriend;
				this.theyAreEditorHere = personInfo.isEditorHere;
				this.theyAreOwnerHere = personInfo.isOwnerHere;
				this.theyAreListEditorHere = personInfo.isListEditorHere;
				this.theyAreAreaLocked = personInfo.isAreaLocked;
				if (!this.weAreResized || !this.theyAreFriend)
				{
					this.UpdateFriendButton();
				}
				if (!this.weAreResized)
				{
					this.UpdateEditorHereButton();
					this.UpdateAreaLockButton();
					if (personInfo.isOnline)
					{
						this.pingButton = base.AddButton("ping", null, "ping", "ButtonCompact", 235, 0, "ping", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
					}
					else
					{
						this.AddLastActivityOnIfNeeded(personInfo);
					}
				}
				base.UpdateScale();
				if (personInfo.isBanned)
				{
					base.ShowHelpLabel("This account is currently banned, which may happen due to a very high number of flag reports. Depending on the ban type, they may still roam in their home area, and the ban may fade over time.", 50, 0.7f, TextAlignment.Left, -700, false, false, 1f, TextColor.Red);
				}
			});
			base.AddTopCreationsOfPerson(Our.personIdOfInterest);
			if (!this.weAreResized)
			{
				Managers.personManager.GetPersonFlagStatus(this.personId, delegate(bool _isFlagged)
				{
					if (this == null)
					{
						return;
					}
					this.isFlagged = _isFlagged;
					base.AddFlag("flagPerson", _isFlagged, "flagPerson", false, -410, 420);
				});
			}
			Our.personIdOfInterest = null;
			Our.personNameOfInterest = null;
		}
		if (!this.weAreResized)
		{
			this.giftButton = base.AddGiftsButton();
		}
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0005781C File Offset: 0x00055C1C
	private void AddLastActivityOnIfNeeded(PersonInfo personInfo)
	{
		DateTime dateTime = Convert.ToDateTime(personInfo.lastActivityOn);
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
		float num = Mathf.Round((float)Math.Abs(timeSpan.TotalSeconds));
		float num2 = num / 60f / 60f / 24f / 30.44f;
		if (num2 >= 1f)
		{
			string text = "Last on: " + Misc.GetHowLongAgoText(personInfo.lastActivityOn);
			base.AddLabel(text, 30, -20, 0.7f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x000578C0 File Offset: 0x00055CC0
	private void UpdateFriendButton()
	{
		if (this.friendButton != null)
		{
			global::UnityEngine.Object.Destroy(this.friendButton);
		}
		string text = ((!this.theyAreFriend) ? "Add as friend" : "Friended");
		this.friendButton = base.AddButton("toggleAddFriend", null, text, "ButtonCompact", -235, 0, "addFriend", this.theyAreFriend, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0005794C File Offset: 0x00055D4C
	private void UpdateEditorHereButton()
	{
		if (this.editorHereButton != null)
		{
			global::UnityEngine.Object.Destroy(this.editorHereButton);
		}
		if (this.listEditorHereButton != null)
		{
			global::UnityEngine.Object.Destroy(this.listEditorHereButton);
		}
		if (!this.theyAreAreaLocked)
		{
			bool flag = Managers.areaManager.weAreOwnerOfCurrentArea || Managers.areaManager.weAreListEditorOfCurrentArea;
			if (flag && !this.theyAreOwnerHere)
			{
				string text = ((!this.theyAreEditorHere) ? "Make editor here" : "Editor");
				float num = 0.9f;
				this.editorHereButton = base.AddButton("toggleTheyAreEditorHere", null, text, "ButtonCompact", -235, 110, "editArea", this.theyAreEditorHere, num, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				if (Managers.areaManager.weAreOwnerOfCurrentArea && this.theyAreEditorHere)
				{
					this.listEditorHereButton = base.AddButton("toggleTheyAreListEditorHere", null, "Can change editors (except me)", "ButtonCompactNoIcon", 235, 110, null, this.theyAreListEditorHere, 0.75f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
					Transform transform = this.listEditorHereButton.transform.Find("Text");
					Vector3 localPosition = transform.localPosition;
					localPosition.z -= 0.001f;
					transform.localPosition = localPosition;
				}
			}
			else if (this.theyAreEditorHere || this.theyAreListEditorHere)
			{
				string text2 = ((!this.theyAreListEditorHere && !this.theyAreOwnerHere) ? "✔ Editor here" : "✔ Can add editors");
				base.AddLabel(text2, -350, 90, 0.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x00057B30 File Offset: 0x00055F30
	private void UpdateAreaLockButton()
	{
		if (this.areaLockButton != null)
		{
			global::UnityEngine.Object.Destroy(this.areaLockButton);
		}
		this.DestroyLockTimeButtons();
		if (Managers.areaManager.weAreEditorOfCurrentArea && !this.theyAreEditorHere)
		{
			string text = ((!this.theyAreAreaLocked) ? "lockFromAreaPickTime" : "unlockFromArea");
			string text2 = ((!this.theyAreAreaLocked) ? "lock" : "locked");
			string text3 = ((!this.theyAreAreaLocked) ? "areaLockOff" : "areaLockOn");
			this.areaLockButton = base.AddButton(text, null, text2, "ButtonCompactShort", 335, 420, text3, this.theyAreAreaLocked, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00057C10 File Offset: 0x00056010
	private void DestroyLockTimeButtons()
	{
		for (int i = 0; i < this.lockTimeButtons.Length; i++)
		{
			if (this.lockTimeButtons[i] != null)
			{
				global::UnityEngine.Object.Destroy(this.lockTimeButtons[i]);
			}
		}
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00057C58 File Offset: 0x00056058
	private void UpdateLockTimeButtons()
	{
		if (this.areaLockButton != null)
		{
			global::UnityEngine.Object.Destroy(this.areaLockButton);
		}
		this.DestroyLockTimeButtons();
		if (Managers.areaManager.weAreEditorOfCurrentArea && !this.theyAreEditorHere)
		{
			this.lockTimeButtons[0] = base.AddButton("lockFromArea_15minutes", null, "15 minutes", "ButtonCompactNoIcon", 235, 270, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			this.lockTimeButtons[1] = base.AddButton("lockFromArea_1day", null, "1 day", "ButtonCompactNoIcon", 235, 350, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			this.lockTimeButtons[2] = base.AddButton("lockFromArea_forever", null, "forever (until removed)", "ButtonCompactNoIcon", 235, 430, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x00057D7C File Offset: 0x0005617C
	private void SendPing()
	{
		global::UnityEngine.Object.Destroy(this.pingButton);
		TextMesh checkmarkText = base.AddLabel("✔", 235, -25, 1f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		Managers.soundManager.Play("ping", this.transform, 0.2f, true, false);
		Managers.personManager.PingPerson(this.personId, delegate(bool ok)
		{
			if (!ok)
			{
				Managers.soundManager.Play("no", this.transform, 1f, false, false);
				checkmarkText.text = string.Empty;
			}
		});
		Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "ping sent", true);
		if (this.theyAreFriend)
		{
			Managers.personManager.IncreaseFriendshipStrength(this.personId, delegate
			{
			});
		}
		if (!Managers.achievementManager.DidAchieve(Achievement.PingedSomeone))
		{
			Managers.achievementManager.RegisterAchievement(Achievement.PingedSomeone);
			string text = "Ping sent! It will be received if the other friended you. For some minutes, it also allows them to enter your current area even if it's private.";
			text = Misc.WrapWithNewlines(text, 55, -1);
			base.AddLabel(text, 0, 200, 0.7f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00057EA0 File Offset: 0x000562A0
	private void UnlockFromArea()
	{
		if (!this.isCurrentlySaving)
		{
			this.isCurrentlySaving = true;
			this.DestroyLockTimeButtons();
			string currentAreaId = Managers.areaManager.currentAreaId;
			Managers.areaManager.UnlockPersonFromArea(currentAreaId, this.personId, delegate(bool savedOk)
			{
				if (savedOk)
				{
					this.theyAreAreaLocked = !this.theyAreAreaLocked;
					this.isCurrentlySaving = false;
					Managers.soundManager.Play("putDown", this.transform, 1f, false, false);
					this.UpdateAreaLockButton();
					this.UpdateEditorHereButton();
				}
			});
		}
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00057EED File Offset: 0x000562ED
	public void AddOrUpdatePersonStats()
	{
		if (this.personThisIsOf != null)
		{
			base.AddOrUpdatePersonStats(this.personThisIsOf);
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00057F0C File Offset: 0x0005630C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "flagPerson":
			if (!this.flagIsBeingToggled)
			{
				this.flagIsBeingToggled = true;
				if (this.isFlagged)
				{
					Managers.personManager.TogglePersonFlag(this.personId, null, delegate(bool _isFlagged)
					{
						this.isFlagged = _isFlagged;
						this.UpdateFlag(this.isFlagged, false, string.Empty);
						this.flagIsBeingToggled = false;
					});
				}
				else
				{
					Our.personIdOfInterest = this.personId;
					Managers.dialogManager.GetInput(delegate(string text)
					{
						if (text == string.Empty)
						{
							Managers.soundManager.Play("no", this.transform, 1f, false, false);
						}
						if (!string.IsNullOrEmpty(text))
						{
							Managers.personManager.TogglePersonFlag(Our.personIdOfInterest, text, delegate(bool isFlagged)
							{
								this.SwitchTo(DialogType.Profile, string.Empty);
							});
						}
						else
						{
							this.SwitchTo(DialogType.Profile, string.Empty);
						}
					}, contextName, string.Empty, 200, "reason for reporting", true, true, false, false, 1f, false, false, null, false);
				}
			}
			break;
		case "toggleAddFriend":
			if (!this.isCurrentlySaving)
			{
				this.isCurrentlySaving = true;
				if (this.theyAreFriend)
				{
					Our.personIdOfInterest = this.personId;
					string text2 = "Please note this also resets the sorting of this person in your friends list, should you friend them again later.";
					base.SwitchToConfirmDialog(text2, delegate(bool didConfirm)
					{
						if (didConfirm)
						{
							Managers.personManager.RemoveFriend(Our.personIdOfInterest, delegate
							{
								this.theyAreFriend = false;
								this.UpdateFriendButton();
								this.isCurrentlySaving = false;
								this.SwitchTo(DialogType.Profile, string.Empty);
							});
						}
						else
						{
							this.SwitchTo(DialogType.Profile, string.Empty);
						}
					});
				}
				else
				{
					Managers.personManager.AddFriend(this.personId, delegate
					{
						this.theyAreFriend = true;
						this.UpdateFriendButton();
						this.isCurrentlySaving = false;
						Managers.achievementManager.RegisterAchievement(Achievement.FriendedSomeone);
						if (this.weAreResized)
						{
							Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
							this.CloseDialog();
						}
					});
				}
			}
			break;
		case "toggleTheyAreEditorHere":
			if (!this.isCurrentlySaving)
			{
				this.isCurrentlySaving = true;
				this.DestroyLockTimeButtons();
				bool newTheyAreEditorHereValue = !this.theyAreEditorHere;
				string currentAreaId = Managers.areaManager.currentAreaId;
				Managers.areaManager.SetEditor(currentAreaId, this.personId, newTheyAreEditorHereValue, delegate
				{
					this.theyAreEditorHere = newTheyAreEditorHereValue;
					this.UpdateEditorHereButton();
					this.UpdateAreaLockButton();
					if (newTheyAreEditorHereValue)
					{
						Managers.personManager.IncreaseFriendshipStrength(this.personId, delegate
						{
						});
					}
					this.isCurrentlySaving = false;
				});
			}
			break;
		case "toggleTheyAreListEditorHere":
			if (!this.isCurrentlySaving)
			{
				this.isCurrentlySaving = true;
				this.DestroyLockTimeButtons();
				bool newTheyAreListEditorHereValue = !this.theyAreListEditorHere;
				string currentAreaId2 = Managers.areaManager.currentAreaId;
				Managers.areaManager.SetListEditor(currentAreaId2, this.personId, newTheyAreListEditorHereValue, delegate
				{
					this.theyAreListEditorHere = newTheyAreListEditorHereValue;
					this.UpdateEditorHereButton();
					this.UpdateAreaLockButton();
					if (newTheyAreListEditorHereValue)
					{
						Managers.personManager.IncreaseFriendshipStrength(this.personId, delegate
						{
						});
					}
					this.isCurrentlySaving = false;
				});
			}
			break;
		case "lockFromAreaPickTime":
			global::UnityEngine.Object.Destroy(this.giftButton);
			this.UpdateLockTimeButtons();
			break;
		case "lockFromArea_15minutes":
		case "lockFromArea_1day":
		case "lockFromArea_forever":
			if (!this.isCurrentlySaving)
			{
				Our.personIdOfInterest = this.personId;
				Managers.dialogManager.GetInput(delegate(string text)
				{
					if (text == string.Empty)
					{
						Managers.soundManager.Play("no", this.transform, 1f, false, false);
					}
					int? num2 = null;
					if (contextName != null)
					{
						if (!(contextName == "lockFromArea_15minutes"))
						{
							if (contextName == "lockFromArea_1day")
							{
								num2 = new int?(1440);
							}
						}
						else
						{
							num2 = new int?(15);
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						string currentAreaId3 = Managers.areaManager.currentAreaId;
						Managers.areaManager.LockPersonFromArea(currentAreaId3, Our.personIdOfInterest, text, num2, delegate(bool savedOk)
						{
							if (savedOk)
							{
								Managers.soundManager.Play("putDown", this.hand.transform, 1f, false, false);
							}
						});
						this.CloseDialog();
					}
					else
					{
						this.SwitchTo(DialogType.Profile, string.Empty);
					}
				}, contextName, string.Empty, 200, "area lock reason", false, false, false, false, 1f, false, false, null, false);
			}
			break;
		case "ping":
			this.SendPing();
			break;
		case "unlockFromArea":
			this.UnlockFromArea();
			break;
		case "findAreasByCreatorId":
			FindAreasDialog.findByCreatorId = this.personId;
			FindAreasDialog.findByCreatorId_nameForReference = contextId;
			base.SwitchTo(DialogType.FindAreas, string.Empty);
			break;
		case "gifts":
			Our.personIdOfInterest = this.personId;
			Our.personNameOfInterest = this.personName;
			base.SwitchTo(DialogType.Gifts, string.Empty);
			break;
		case "back":
		{
			DialogType dialogToGoBackTo = Our.dialogToGoBackTo;
			Our.dialogToGoBackTo = DialogType.None;
			GameObject gameObject = base.SwitchTo(dialogToGoBackTo, string.Empty);
			if (this.friendsDialogPageToGoBackTo != -1)
			{
				FriendsDialog component = gameObject.GetComponent<FriendsDialog>();
				component.page = this.friendsDialogPageToGoBackTo;
			}
			break;
		}
		case "close":
			Our.dialogToGoBackTo = DialogType.None;
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x04000853 RID: 2131
	private Person personThisIsOf;

	// Token: 0x04000854 RID: 2132
	private string personId;

	// Token: 0x04000855 RID: 2133
	private bool theyAreFriend;

	// Token: 0x04000856 RID: 2134
	private bool theyAreEditorHere;

	// Token: 0x04000857 RID: 2135
	private bool theyAreOwnerHere;

	// Token: 0x04000858 RID: 2136
	private bool theyAreAreaLocked;

	// Token: 0x04000859 RID: 2137
	private bool theyAreListEditorHere;

	// Token: 0x0400085A RID: 2138
	private GameObject friendButton;

	// Token: 0x0400085B RID: 2139
	private GameObject pingButton;

	// Token: 0x0400085C RID: 2140
	private GameObject editorHereButton;

	// Token: 0x0400085D RID: 2141
	private GameObject listEditorHereButton;

	// Token: 0x0400085E RID: 2142
	private GameObject areaLockButton;

	// Token: 0x0400085F RID: 2143
	private const int buttonX = 235;

	// Token: 0x04000860 RID: 2144
	private bool isCurrentlySaving;

	// Token: 0x04000861 RID: 2145
	private bool isFlagged;

	// Token: 0x04000862 RID: 2146
	private GameObject[] lockTimeButtons = new GameObject[3];

	// Token: 0x04000863 RID: 2147
	private GameObject giftButton;

	// Token: 0x04000864 RID: 2148
	private string personName;

	// Token: 0x04000865 RID: 2149
	private bool weAreResized;

	// Token: 0x04000866 RID: 2150
	public int friendsDialogPageToGoBackTo = -1;
}
