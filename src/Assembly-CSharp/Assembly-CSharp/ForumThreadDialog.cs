using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class ForumThreadDialog : Dialog
{
	// Token: 0x060009C4 RID: 2500 RVA: 0x00040B44 File Offset: 0x0003EF44
	public void Start()
	{
		base.Init(base.gameObject, false, true, false);
		base.AddDialogThingFundamentIfNeeded();
		base.AddBackButton();
		base.AddCloseButton();
		this.AddBacksideCopyThreadIdButton();
		Managers.forumManager.GetForumThread(Managers.forumManager.currentForumThreadId, delegate(string reasonFailed, ForumData forum, ForumThreadData thread)
		{
			if (this != null && !Managers.forumManager.HandleFailIfNeeded(reasonFailed))
			{
				this.rights = new ForumRights(Managers.forumManager.currentForumData);
				string text = thread.title;
				if (!string.IsNullOrEmpty(thread.titleClarification))
				{
					text = text + " [" + thread.titleClarification + "]";
				}
				text = Misc.WrapWithNewlines(text, 40, 2);
				base.AddHeadline(text, -370, -460, TextColor.Default, TextAlignment.Left, false);
				base.AddSeparator(0, 800, false);
				if (this.rights.isModerator)
				{
					string text2 = "threadSettings";
					string text3 = null;
					string text4 = null;
					string text5 = "ButtonSmall";
					int num = -870;
					int num2 = 880;
					string text6 = "attributes";
					TextColor customDefaultTextColor = this.customDefaultTextColor;
					string dialogColor = Managers.forumManager.currentForumData.dialogColor;
					GameObject gameObject = base.AddButton(text2, text3, text4, text5, num, num2, text6, false, 1f, customDefaultTextColor, 1f, 0f, 0f, dialogColor, false, false, TextAlignment.Left, false, false);
					Transform transform = gameObject.transform.Find("Cube");
					transform.localScale = new Vector3(1f, 1f, 0.55f);
				}
				if (thread.comments.Count >= 1)
				{
					this.threadStartingComment = thread.comments[0];
					this.allComments = thread.comments;
					if (thread.comments.Count >= 2)
					{
						this.comments = thread.comments.GetRange(1, thread.comments.Count - 1);
						this.maxPages = Mathf.CeilToInt((float)this.comments.Count / 6f);
						this.page = this.maxPages - 1;
					}
					base.StartCoroutine(this.UpdateCommentsDisplay());
					if (!thread.isLocked && (!this.rights.onlyModsCanAddComments || this.rights.isModerator))
					{
						if (this.isQuestThreadWithLockedCommenting)
						{
							base.AddLabel("Completing the quest will list you above", -330, 860, 0.95f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
						}
						else
						{
							base.AddModelButton("EditTextButton", "createForumThreadComment", null, -390, 870, false);
							base.AddLabel("Say something", -330, 860, 0.95f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
							bool flag = thread.comments.Count >= 235;
							if (flag)
							{
								base.AddLabel("limits", 700, 860, 0.95f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
								GameObject gameObject2 = base.AddModelButton("HelpButton", "nearCommentLimitInfo", null, 700, 680, false);
								gameObject2.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
							}
						}
					}
				}
			}
		});
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00040B98 File Offset: 0x0003EF98
	private void AddBacksideCopyThreadIdButton()
	{
		string text = "copyThreadId";
		string text2 = null;
		string text3 = "Copy thread link";
		string text4 = "ButtonBig";
		TextColor textColor = this.customDefaultTextColor;
		string text5 = Managers.forumManager.currentForumData.dialogColor;
		base.AddButton(text, text2, text3, text4, 0, 0, null, false, 1f, textColor, 1f, 0f, 0f, text5, false, false, TextAlignment.Left, true, false);
		string text6 = "Can be pasted in comments as [thread: link] or used with the \"show thread link\" command";
		text5 = text6;
		int num = 0;
		int num2 = 110;
		textColor = this.customDefaultTextColor;
		base.AddLabel(text5, num, num2, 1f, true, textColor, false, TextAlignment.Center, 32, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00040C38 File Offset: 0x0003F038
	private new void Update()
	{
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.C))
		{
			this.CopyImageUrlToClipboard();
		}
		base.Update();
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00040C5C File Offset: 0x0003F05C
	private void CopyImageUrlToClipboard()
	{
		if (this.currentEnlagedImageUrl != null)
		{
			GUIUtility.systemCopyBuffer = this.currentEnlagedImageUrl;
			Managers.soundManager.Play("pickUp", this.transform, 0.4f, false, false);
		}
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00040C90 File Offset: 0x0003F090
	private IEnumerator UpdateCommentsDisplay()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		this.commentButtonsByDate = new Dictionary<string, List<GameObject>>();
		this.likeLabelsByDate = new Dictionary<string, TextMesh>();
		this.ShowThreadComment(this.threadStartingComment, -760, true);
		if (this.comments != null)
		{
			int num = this.page * 6;
			int num2 = Mathf.Min(num + 6 - 1, this.comments.Count - 1);
			if (this.page == this.maxPages - 1 && num2 - num < 6)
			{
				num = num2 - 5;
				if (num < 0)
				{
					num = 0;
				}
			}
			this.UpdateThreadPageUpDownButtons(num);
			int num3 = 0;
			for (int i = num; i <= num2; i++)
			{
				ForumCommentData forumCommentData = this.comments[i];
				int num4 = -460 + num3++ * 200;
				this.ShowThreadComment(forumCommentData, num4, false);
			}
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00040CAC File Offset: 0x0003F0AC
	private void ShowThreadComment(ForumCommentData comment, int y, bool isThreadStarterComment = false)
	{
		TextLink textLink = new TextLink();
		string text = "userInfo";
		string text2 = comment.userId;
		string text3 = Misc.Truncate(comment.userName, 20, true);
		string text4 = "ButtonCompactNoIcon";
		int num = -720;
		TextColor customDefaultTextColor = this.customDefaultTextColor;
		base.AddButton(text, text2, text3, text4, num, y, null, false, 1.2f, customDefaultTextColor, 1f, 0f, 0f, Managers.forumManager.currentForumData.dialogColor, false, false, TextAlignment.Left, false, false);
		bool flag = comment.userId == Managers.personManager.ourPerson.userId;
		int num2 = -930;
		if ((this.rights.isModerator || flag) && (!isThreadStarterComment || flag))
		{
			base.AddModelButton("Settings", "showCommentHandlingButtons", comment.date, -905, y + 70, false);
			this.AddCommentHandlingButtons(comment, y, isThreadStarterComment, flag);
			num2 += 65;
		}
		base.AddLabel(this.GetEditedDateDisplay(comment.date, comment.lastEditedDate), num2, y + 50, 0.75f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		string text5 = ((!isThreadStarterComment) ? "addLike" : "addLikeToThreadStarter");
		base.AddModelButton("Like", text5, comment.date, -530, y + 65, false);
		this.likeLabelsByDate.Add(comment.date, base.AddLabel(this.GetLikedByText(comment, isThreadStarterComment), -500, y + 90, 0.5f, false, TextColor.Default, false, TextAlignment.Right, -1, 0.85f, false, TextAnchor.MiddleLeft));
		bool flag2 = !string.IsNullOrEmpty(comment.thingId);
		string text6 = comment.text;
		text6 = text6.ReplaceCaseInsensitive("[version]", Universe.GetClientVersionDisplay());
		bool flag3 = textLink.TryParseText(text6);
		if (textLink.type == TextLink.Type.quest)
		{
			if (isThreadStarterComment)
			{
				this.isQuestThreadWithLockedCommenting = true;
			}
			else
			{
				flag3 = false;
				textLink = new TextLink();
			}
		}
		if (textLink.IsImageType())
		{
			text6 = textLink.RemoveLink(text6);
		}
		text6 = Misc.WrapWithNewlines(text6, 60, -1);
		float num3 = 1.1f;
		float? num4 = null;
		if ((flag2 || flag3) && text6.Length >= 40)
		{
			num3 = 0.8f;
		}
		else if (text6.Length >= 165)
		{
			num3 = 0.8f;
			num4 = new float?(0.85f);
		}
		else if (text6.Length >= 50)
		{
			num3 = 0.9f;
		}
		TextMesh textMesh = base.AddLabel(text6, -440, y - 20, num3, false, TextColor.Black, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		if (num4 != null)
		{
			textMesh.lineSpacing = num4.Value;
		}
		int num5 = 3;
		for (int i = 0; i < num5; i++)
		{
			this.AddCommentBackQuad(textMesh.transform.parent);
		}
		if (flag2)
		{
			Vector3 vector = new Vector3(0.2f, 0.025f, textMesh.transform.parent.localPosition.z - 0.016f);
			Managers.thingManager.InstantiateThingOnDialogViaCache(ThingRequestContext.ForumThreadDialog, comment.thingId, this.wrapper.transform, vector, 0.04f, Our.mode == EditModes.Area, false, 0f, 0f, 0f, false, false);
		}
		else if (flag3)
		{
			if (textLink.type == TextLink.Type.steamimage || textLink.type == TextLink.Type.imgur)
			{
				string fullUrl = textLink.GetFullUrl();
				text4 = "toggleImageLargeSmall";
				text3 = fullUrl;
				text2 = null;
				text = "ButtonWithWebImage";
				int num6 = 830;
				num = y + 55;
				string dialogColor = Managers.forumManager.currentForumData.dialogColor;
				GameObject gameObject = base.AddButton(text4, text3, text2, text, num6, num, null, false, 1f, TextColor.Default, 1f, 0f, 0f, dialogColor, false, false, TextAlignment.Left, false, false);
				GameObject gameObject2 = gameObject.transform.Find("ImageQuad").gameObject;
				base.StartCoroutine(this.StartImageLoad(gameObject2, fullUrl));
			}
			else
			{
				base.AddTextLinkButton(textLink, 800, y + 50);
			}
		}
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x000410E8 File Offset: 0x0003F4E8
	private IEnumerator StartImageLoad(GameObject imageQuad, string url)
	{
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			Texture2D texture = www.texture;
			float num = 0.03525f;
			float num2 = num / (float)texture.width;
			float num3 = num / (float)texture.height;
			float num4 = Math.Min(num2, num3);
			float num5 = (float)texture.width * num4;
			float num6 = (float)texture.height * num4;
			imageQuad.transform.localScale = new Vector3(num5, num6, this.transform.localScale.z);
			Renderer component = imageQuad.GetComponent<Renderer>();
			component.material.mainTexture = texture;
		}
		else
		{
			Log.Debug("ForumThreadDialog image load error: " + www.error);
		}
		yield break;
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00041114 File Offset: 0x0003F514
	private void AddCommentHandlingButtons(ForumCommentData comment, int y, bool isThreadStarterComment, bool isOurComment)
	{
		if (!this.commentButtonsByDate.ContainsKey(comment.date))
		{
			List<GameObject> list = new List<GameObject>();
			y += 50;
			if (isOurComment)
			{
				list.Add(base.AddButton("editComment", comment.date, "Edit...", "ButtonCompactNoIcon", -175, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			}
			string text = "deleteComment";
			string text2 = "Delete";
			if (isThreadStarterComment)
			{
				text = "deleteThread";
				text2 = "Delete Whole Thread";
			}
			list.Add(base.AddButton(text, comment.date, text2, "ButtonCompactNoIcon", 325, y, null, false, 1f, TextColor.Red, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.commentButtonsByDate.Add(comment.date, list);
			foreach (GameObject gameObject in list)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00041250 File Offset: 0x0003F650
	private void AddCommentBackQuad(Transform commentTransform)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		Renderer component = gameObject.GetComponent<Renderer>();
		Material material = component.material;
		material.SetInt("_SrcBlend", 1);
		material.SetInt("_DstBlend", 10);
		material.SetInt("_ZWrite", 0);
		material.DisableKeyword("_ALPHATEST_ON");
		material.DisableKeyword("_ALPHABLEND_ON");
		material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
		material.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		gameObject.transform.parent = this.transform;
		gameObject.transform.localScale = new Vector3(0.335f, 0.04f, 2f);
		gameObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
		gameObject.transform.localPosition = new Vector3(0.055f, 0.0125f, (commentTransform.localPosition.z * 0.5f - 0.009f) * 2f);
		gameObject.transform.parent = this.wrapper.transform;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0004137C File Offset: 0x0003F77C
	private void UpdateThreadPageUpDownButtons(int previousAmount)
	{
		if (this.maxPages >= 2)
		{
			Vector3 vector = new Vector3(0.8f, 1f, 0.8f);
			if (this.page > 0)
			{
				GameObject gameObject = base.AddModelButton("ButtonBack", "PreviousPage", null, -400, -560, false);
				gameObject.transform.localScale = vector;
				base.AddLabel(previousAmount + " previous", -340, -580, 0.75f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				gameObject.transform.Rotate(new Vector3(0f, 90f, 0f));
			}
			if (this.page < this.maxPages - 1)
			{
				GameObject gameObject2 = base.AddModelButton("ButtonBack", "NextPage", null, 850, -560, false);
				gameObject2.transform.localScale = vector;
				gameObject2.transform.Rotate(new Vector3(0f, -90f, 0f));
			}
		}
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0004148C File Offset: 0x0003F88C
	private string GetEditedDateDisplay(string dateTimeString, string dateTimeLastEditedString)
	{
		string text = Misc.GetHowLongAgoText(dateTimeString);
		text = text.Replace("minute", "min");
		text = text.Replace("second", "sec");
		if (!string.IsNullOrEmpty(dateTimeLastEditedString))
		{
			DateTime dateTime = Convert.ToDateTime(dateTimeString);
			DateTime dateTime2 = Convert.ToDateTime(dateTimeLastEditedString);
			TimeSpan timeSpan = new TimeSpan(dateTime.Ticks - dateTime2.Ticks);
			float num = Mathf.Round((float)Math.Abs(timeSpan.TotalSeconds));
			if (num >= 900f)
			{
				text += "*";
			}
		}
		return text;
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00041520 File Offset: 0x0003F920
	private ForumCommentData GetCommentByDate(string date)
	{
		ForumCommentData forumCommentData = null;
		foreach (ForumCommentData forumCommentData2 in this.allComments)
		{
			if (forumCommentData2.date == date)
			{
				forumCommentData = forumCommentData2;
				break;
			}
		}
		return forumCommentData;
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00041590 File Offset: 0x0003F990
	private string GetLikedByText(ForumCommentData comment, bool isThreadStarterComment)
	{
		string text = string.Empty;
		if (comment.likes != null && comment.likes.Count >= 1)
		{
			foreach (ForumCommentLikePersonIdAndName forumCommentLikePersonIdAndName in comment.oldestLikes)
			{
				if (text != string.Empty)
				{
					text += ", ";
				}
				text += Misc.Truncate(Managers.personManager.GetScreenNameWithDiscloser(forumCommentLikePersonIdAndName.id, forumCommentLikePersonIdAndName.n), 15, true);
			}
			if (comment.oldestLikes.Count >= 1 && comment.newestLikes.Count >= 1 && comment.oldestLikes.Count + comment.newestLikes.Count < comment.likes.Count)
			{
				text += ", ..";
			}
			foreach (ForumCommentLikePersonIdAndName forumCommentLikePersonIdAndName2 in comment.newestLikes)
			{
				if (text != string.Empty)
				{
					text += ", ";
				}
				text += Misc.Truncate(Managers.personManager.GetScreenNameWithDiscloser(forumCommentLikePersonIdAndName2.id, forumCommentLikePersonIdAndName2.n), 15, true);
			}
			text = Misc.WrapWithNewlines(text, 36, (!isThreadStarterComment) ? 2 : 6);
			int num = comment.oldestLikes.Count + comment.newestLikes.Count;
			if (comment.likes.Count > num)
			{
				text = text + " +" + (comment.likes.Count - num);
			}
			text = text.ToUpper();
		}
		return text;
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0004178C File Offset: 0x0003FB8C
	private void AddSampleCommentLikesForTesting(ForumCommentData comment)
	{
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00041790 File Offset: 0x0003FB90
	private ForumCommentLikePersonIdAndName GetNewLikeIdName(string name, string id = "")
	{
		return new ForumCommentLikePersonIdAndName
		{
			n = name,
			id = id
		};
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x000417B4 File Offset: 0x0003FBB4
	private void AddLikeToCommentData(ForumCommentData comment)
	{
		ForumCommentLikePersonIdAndName newLikeIdName = this.GetNewLikeIdName(Managers.personManager.ourPerson.screenName, Managers.personManager.ourPerson.userId);
		comment.likes.Add(newLikeIdName.id);
		if (comment.oldestLikes.Count < 5)
		{
			comment.oldestLikes.Add(newLikeIdName);
		}
		else
		{
			comment.newestLikes.Add(newLikeIdName);
			if (comment.newestLikes.Count > 5)
			{
				comment.newestLikes.RemoveAt(0);
			}
		}
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00041844 File Offset: 0x0003FC44
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "threadSettings":
			base.SwitchTo(DialogType.ForumThreadSettings, string.Empty);
			break;
		case "PreviousPage":
		{
			int num = --this.page;
			if (num < 0)
			{
				this.page = this.maxPages - 1;
			}
			base.StartCoroutine(this.UpdateCommentsDisplay());
			break;
		}
		case "NextPage":
		{
			int num = ++this.page;
			if (num > this.maxPages - 1)
			{
				this.page = 0;
			}
			base.StartCoroutine(this.UpdateCommentsDisplay());
			break;
		}
		case "toggleImageLargeSmall":
			if (state)
			{
				if (this.currentEnlargedImageButton)
				{
					this.currentEnlargedImageButton.transform.localScale = Vector3.one;
					base.SetCheckboxState(this.currentEnlargedImageButton, false, true);
				}
				thisButton.transform.localScale = Vector3.Scale(thisButton.transform.localScale, new Vector3(10f, 2f, 10f));
				base.SetButtonHighlight(thisButton, false);
				this.currentEnlargedImageButton = thisButton;
				this.currentEnlagedImageUrl = contextId;
			}
			else
			{
				thisButton.transform.localScale = Vector3.one;
				this.currentEnlargedImageButton = null;
				this.currentEnlagedImageUrl = null;
			}
			break;
		case "userInfo":
			Our.personIdOfInterest = contextId;
			this.hand.lastContextInfoHit = null;
			Our.dialogToGoBackTo = DialogType.ForumThread;
			base.SwitchTo(DialogType.Profile, string.Empty);
			break;
		case "createForumThreadComment":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				string commentThingIdBeingCreated = Managers.forumManager.commentThingIdBeingCreated;
				if (text == null || (text == string.Empty && commentThingIdBeingCreated == null))
				{
					this.SwitchTo(DialogType.ForumThread, string.Empty);
				}
				else
				{
					Managers.forumManager.commentThingIdBeingCreated = null;
					Managers.forumManager.AddForumComment(Managers.forumManager.currentForumThreadId, text, commentThingIdBeingCreated, delegate(string reasonFailed)
					{
						if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
						{
							Managers.forumManager.SetLatestCommentDateSeenToNow();
							this.SwitchTo(DialogType.ForumThread, string.Empty);
						}
					});
				}
			}, contextName, string.Empty, 180, string.Empty, true, true, false, false, 1f, false, false, null, false);
			break;
		case "showCommentHandlingButtons":
		{
			List<GameObject> list;
			if (this.commentButtonsByDate.TryGetValue(contextId, out list))
			{
				foreach (GameObject gameObject in list)
				{
					gameObject.SetActive(!gameObject.activeSelf);
				}
			}
			break;
		}
		case "addLike":
		case "addLikeToThreadStarter":
		{
			bool isThreadStarterComment = contextName == "addLikeToThreadStarter";
			ForumCommentData comment2 = this.GetCommentByDate(contextId);
			if (comment2 != null)
			{
				if (comment2.likes.IndexOf(Managers.personManager.ourPerson.userId) < 0)
				{
					this.AddLikeToCommentData(comment2);
					Managers.forumManager.LikeForumComment(Managers.forumManager.currentForumThreadId, comment2.userId, comment2.date, delegate
					{
						TextMesh textMesh;
						if (this.likeLabelsByDate.TryGetValue(contextId, out textMesh))
						{
							textMesh.text = this.GetLikedByText(comment2, isThreadStarterComment);
							Managers.soundManager.Play("success", this.transform, 0.1f, false, false);
						}
					});
				}
				else
				{
					Managers.soundManager.Play("no", this.transform, 0.1f, false, false);
				}
			}
			break;
		}
		case "editComment":
		case "deleteComment":
		{
			ForumCommentData comment = this.GetCommentByDate(contextId);
			if (comment != null)
			{
				if (contextName == "editComment")
				{
					Managers.dialogManager.GetInput(delegate(string text)
					{
						if (text == null)
						{
							this.SwitchTo(DialogType.ForumThread, string.Empty);
						}
						else
						{
							Managers.forumManager.EditForumComment(Managers.forumManager.currentForumThreadId, comment.date, text, delegate(string reasonFailed)
							{
								if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
								{
									this.SwitchTo(DialogType.ForumThread, string.Empty);
								}
							});
						}
					}, "editForumThreadComment", comment.text, 180, string.Empty, true, true, false, false, 1f, false, false, null, false);
				}
				else if (contextName == "deleteComment")
				{
					Managers.forumManager.RemoveForumComment(Managers.forumManager.currentForumThreadId, comment.userId, comment.date, delegate(string reasonFailed)
					{
						if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
						{
							Managers.soundManager.Play("delete", this.transform, 0.1f, false, false);
							this.SwitchTo(DialogType.ForumThread, string.Empty);
						}
					});
				}
			}
			break;
		}
		case "deleteThread":
			Managers.forumManager.RemoveForumThread(Managers.forumManager.currentForumThreadId, delegate(string reasonFailed)
			{
				if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
				{
					Managers.soundManager.Play("delete", this.transform, 0.1f, false, false);
					this.SwitchTo(DialogType.Forum, string.Empty);
				}
			});
			break;
		case "nearCommentLimitInfo":
		{
			string text2 = ((!this.rights.isModerator) ? "An editor can lock this thread" : "As editor, you can lock this thread");
			Managers.dialogManager.ShowInfo(string.Concat(new object[] { "please note when a topic reaches ", 250, " comments, every new comment added will push out the oldest one (except the topic-starting comment). ", text2, " to prevent new comments from being added. sorry for this limit & thanks!" }), true, true, 0, this.hand.currentDialogType, 1f, false, TextColor.Default, TextAlignment.Left);
			break;
		}
		case "copyThreadId":
			GUIUtility.systemCopyBuffer = Managers.forumManager.currentForumThreadId;
			Managers.soundManager.Play("pickUp", this.transform, 1f, false, false);
			break;
		case "textLink":
			base.ExecuteTextLink(this.hand, contextId);
			break;
		case "back":
			Managers.forumManager.currentForumThreadId = null;
			if (ForumThreadSearchResultDialog.searchQuery != null)
			{
				base.SwitchTo(DialogType.ForumThreadSearchResult, string.Empty);
			}
			else
			{
				base.SwitchTo(DialogType.Forum, string.Empty);
			}
			break;
		case "close":
			Managers.forumManager.currentForumThreadId = null;
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x04000750 RID: 1872
	private GameObject fundament;

	// Token: 0x04000751 RID: 1873
	private int page;

	// Token: 0x04000752 RID: 1874
	private const int commentsPerPage = 6;

	// Token: 0x04000753 RID: 1875
	private int maxPages = -1;

	// Token: 0x04000754 RID: 1876
	private ForumCommentData threadStartingComment;

	// Token: 0x04000755 RID: 1877
	private List<ForumCommentData> comments;

	// Token: 0x04000756 RID: 1878
	private List<ForumCommentData> allComments;

	// Token: 0x04000757 RID: 1879
	private Dictionary<string, List<GameObject>> commentButtonsByDate;

	// Token: 0x04000758 RID: 1880
	private Dictionary<string, TextMesh> likeLabelsByDate;

	// Token: 0x04000759 RID: 1881
	private ForumRights rights;

	// Token: 0x0400075A RID: 1882
	private bool isQuestThreadWithLockedCommenting;

	// Token: 0x0400075B RID: 1883
	private GameObject currentEnlargedImageButton;

	// Token: 0x0400075C RID: 1884
	private string currentEnlagedImageUrl;
}
