using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class ThingTagsDialog : Dialog
{
	// Token: 0x06000C88 RID: 3208 RVA: 0x00072DDC File Offset: 0x000711DC
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		Managers.thingManager.AddOutlineHighlightMaterial(this.thing, false);
		if (this.tagsByUs != null && this.tagsByOthers != null)
		{
			this.AddMainContent();
		}
		else
		{
			this.LoadThingTags();
		}
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00072E38 File Offset: 0x00071238
	private void AddMainContent()
	{
		base.AddHeadline("Tags", -370, -460, TextColor.Default, TextAlignment.Left, false);
		base.AddBackButton();
		base.AddCloseButton();
		GameObject gameObject = base.AddModelButton("HelpButton", "toggleHelp", null, 100, -640, false);
		gameObject.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
		this.UpdateTagsDisplay();
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x00072EA9 File Offset: 0x000712A9
	private void LoadThingTags()
	{
		Managers.thingManager.GetThingTags(this.thing.thingId, delegate(List<ThingTagInfo> thingTagInfos)
		{
			if (this == null)
			{
				return;
			}
			if (thingTagInfos != null)
			{
				string userId = Managers.personManager.ourPerson.userId;
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				foreach (ThingTagInfo thingTagInfo in thingTagInfos)
				{
					bool flag = thingTagInfo.userIds.Contains(userId);
					if (flag)
					{
						list.Add(thingTagInfo.tag);
					}
					if (thingTagInfo.userIds.Count >= 2 || !flag)
					{
						list2.Add(thingTagInfo.tag);
					}
				}
				this.tagsByUs = list.ToArray();
				this.tagsByOthers = list2.ToArray();
			}
			if (!this.didAutoOpenKeyboard)
			{
				this.didAutoOpenKeyboard = true;
				this.EditThingTags();
			}
			else
			{
				this.AddMainContent();
			}
		});
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x00072ECC File Offset: 0x000712CC
	private void UpdateTagsDisplay()
	{
		base.AddModelButton("EditTextButton", "editThingTags", null, -400, -300, false);
		base.AddLabel("By you: " + this.GetTagsDisplayString(this.tagsByUs), -350, -310, 0.9f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddLabel("By you all time: " + Managers.personManager.ourPerson.thingTagCount, -350, -70, 0.9f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddLabel("By others: " + this.GetTagsDisplayString(this.tagsByOthers), -350, 30, 0.9f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		if (this.weAreCreator)
		{
			base.AddSeparator(0, 280, false);
			base.AddCheckbox("changeUnlisted", null, "Unlisted", 0, 380, this.isUnlisted, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
			base.AddCheckboxHelpButton("toggleUnlisted_help", 380);
		}
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x00072FF4 File Offset: 0x000713F4
	private string[] GetSampleTagsToTest()
	{
		return new string[]
		{
			"furniture", "chair", "foo bar", "helloworld", "test", "lightsaber", "another", "word", "too", "much",
			"to display", "rocket", "science", "furniture", "chair", "foo bar", "helloworld", "test", "lightsaber", "another",
			"word", "too", "much", "to display", "rocket", "science", "furniture", "chair", "foo bar", "helloworld",
			"test", "lightsaber", "another", "word", "too", "much", "to display", "rocket", "science"
		};
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00073160 File Offset: 0x00071560
	private string GetTagsDisplayString(string[] tags)
	{
		string text = "-";
		if (tags != null && tags.Length >= 1)
		{
			for (int i = 0; i < tags.Length; i++)
			{
				tags[i] = tags[i].Trim().ToLower();
			}
			text = string.Join(", ", tags);
			text = Misc.WrapWithNewlines(text, 28, 4);
		}
		return text;
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x000731C0 File Offset: 0x000715C0
	private string GetTagsDisplayStringForInput(string[] tags)
	{
		string text = string.Empty;
		if (tags != null && tags.Length >= 1)
		{
			text = string.Join(", ", tags);
		}
		return text;
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x000731F0 File Offset: 0x000715F0
	private List<string> GetAddedTags(string[] oldTags, string[] newTags)
	{
		List<string> list = new List<string>();
		foreach (string text in newTags)
		{
			if (text != string.Empty && Array.IndexOf<string>(oldTags, text) == -1 && !list.Contains(text))
			{
				string text2 = Misc.Truncate(text, 50, false);
				text2 = SearchWords.NormalizeWord(text2);
				list.Add(text2);
			}
		}
		return list;
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x00073264 File Offset: 0x00071664
	private List<string> GetRemovedTags(string[] oldTags, string[] newTags)
	{
		List<string> list = new List<string>();
		foreach (string text in oldTags)
		{
			if (Array.IndexOf<string>(newTags, text) == -1)
			{
				list.Add(text);
			}
		}
		return list;
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x000732A8 File Offset: 0x000716A8
	private string[] NormalizeTags(string[] tags)
	{
		for (int i = 0; i < tags.Length; i++)
		{
			tags[i] = SearchWords.NormalizeWord(tags[i]);
		}
		return tags;
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x000732D8 File Offset: 0x000716D8
	private void ReOpen(Thing thisThing, bool thisWeAreCreator, string[] oldTagsByUs = null, string[] oldTagsByOthers = null)
	{
		if (thisThing != null)
		{
			GameObject gameObject = base.SwitchTo(DialogType.ThingTags, string.Empty);
			ThingTagsDialog component = gameObject.GetComponent<ThingTagsDialog>();
			component.thing = thisThing;
			component.weAreCreator = thisWeAreCreator;
			component.didAutoOpenKeyboard = this.didAutoOpenKeyboard;
			if (oldTagsByUs != null && oldTagsByOthers != null)
			{
				component.tagsByUs = oldTagsByUs;
				component.tagsByOthers = oldTagsByOthers;
			}
		}
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x0007333C File Offset: 0x0007173C
	private void EditThingTags()
	{
		this.keepHighlightOnDestroy = true;
		string thingId = this.thing.thingId;
		Managers.dialogManager.GetInput(delegate(string text)
		{
			if (text != null)
			{
				string[] array;
				if (text != string.Empty)
				{
					array = Misc.Split(text, ",", StringSplitOptions.RemoveEmptyEntries);
				}
				else
				{
					array = new string[0];
				}
				array = this.NormalizeTags(array);
				ThingTagsDialog.recentlyEnteredInput = this.AddToRecentlyEnteredInput(ThingTagsDialog.recentlyEnteredInput, string.Join(", ", array));
				List<string> addedTags = this.GetAddedTags(this.tagsByUs, array);
				List<string> removedTags = this.GetRemovedTags(this.tagsByUs, array);
				bool flag = addedTags.Count + removedTags.Count > 0;
				if (flag)
				{
					Managers.thingManager.SetThingTags(thingId, addedTags, removedTags, delegate(bool wasOk)
					{
						if (wasOk)
						{
							this.ReOpen(this.thing, this.weAreCreator, null, null);
						}
						else
						{
							Managers.soundManager.Play("no", null, 1f, false, false);
						}
					});
				}
				else
				{
					this.ReOpen(this.thing, this.weAreCreator, this.tagsByUs, this.tagsByOthers);
				}
			}
			else
			{
				this.ReOpen(this.thing, this.weAreCreator, this.tagsByUs, this.tagsByOthers);
			}
		}, "editThingTags", this.GetTagsDisplayStringForInput(this.tagsByUs), 1000, "comma-separated tags", true, false, false, false, 1f, false, false, null, false);
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x000733AC File Offset: 0x000717AC
	private void OnDestroy()
	{
		if (this.thing != null && !this.keepHighlightOnDestroy)
		{
			Managers.thingManager.RemoveOutlineHighlightMaterial(this.thing, false);
		}
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x000733DC File Offset: 0x000717DC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "editThingTags"))
			{
				if (!(contextName == "changeUnlisted"))
				{
					if (!(contextName == "toggleHelp"))
					{
						if (!(contextName == "toggleUnlisted_help"))
						{
							if (!(contextName == "back"))
							{
								if (contextName == "close")
								{
									base.CloseDialog();
								}
							}
							else
							{
								GameObject gameObject = base.SwitchTo(DialogType.Thing, string.Empty);
								ThingDialog component = gameObject.GetComponent<ThingDialog>();
								component.thing = this.thing;
							}
						}
						else
						{
							string text = "By default, every thing that was placed in a public area at some point (and is still placed somewhere) will appear in universe-wide searches from the inventory. Unlisted things however will never appear. Only you as creator can toggle this.";
							base.ToggleHelpLabel(text, -700, 1f, 50, 0.7f);
						}
					}
					else
					{
						string text2 = "Tag things with anything you want, e.g. \"chair\", \"furniture\", \"patterns\", \"funny\". Along with the thing's name they help show it in inventory speech searches. You can also use tags others already used to strengthen the tag.";
						base.ToggleHelpLabel(text2, -700, 1f, 50, 0.7f);
					}
				}
				else
				{
					Managers.thingManager.SetThingUnlisted(this.thing.thingId, state, delegate(bool wasOk)
					{
						if (!wasOk)
						{
							Managers.soundManager.Play("no", null, 1f, false, false);
						}
					});
				}
			}
			else
			{
				this.EditThingTags();
			}
		}
	}

	// Token: 0x0400096E RID: 2414
	public Thing thing;

	// Token: 0x0400096F RID: 2415
	public bool weAreCreator;

	// Token: 0x04000970 RID: 2416
	public bool isUnlisted;

	// Token: 0x04000971 RID: 2417
	public string[] tagsByUs;

	// Token: 0x04000972 RID: 2418
	public string[] tagsByOthers;

	// Token: 0x04000973 RID: 2419
	public bool didAutoOpenKeyboard;

	// Token: 0x04000974 RID: 2420
	public static List<string> recentlyEnteredInput = new List<string>();

	// Token: 0x04000975 RID: 2421
	private bool keepHighlightOnDestroy;
}
