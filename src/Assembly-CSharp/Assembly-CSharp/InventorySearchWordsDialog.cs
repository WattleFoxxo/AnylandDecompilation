using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class InventorySearchWordsDialog : Dialog
{
	// Token: 0x06000A3B RID: 2619 RVA: 0x000464E4 File Offset: 0x000448E4
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		base.AddHeadline("More Words", -440, -460, TextColor.Default, TextAlignment.Left, false);
		bool flag = false;
		GameObject gameObject = base.AddModelButton("HelpButton", "toggleHelp", null, 100, -640, false);
		gameObject.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
		bool flag2 = SearchWords.customWords != null && SearchWords.customWords.Count >= 200;
		if (flag2)
		{
			this.issueToAlertTo = string.Concat(new object[]
			{
				200,
				" words limit reached",
				Environment.NewLine,
				"Please contact us if you need more"
			});
			flag = true;
		}
		else
		{
			base.AddButton("addWord", null, "+ add", "ButtonCompactNoIconShort", -325, -300, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		if (this.issueToAlertTo != null)
		{
			this.issueLabel = base.AddLabel(this.issueToAlertTo, 435, -325, 0.8f, false, (!flag) ? TextColor.Default : TextColor.Gray, false, TextAlignment.Right, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		base.AddSeparator(0, -215, false);
		this.words = SearchWords.customWords;
		this.maxPages = Mathf.CeilToInt((float)this.words.Count / 5f);
		base.StartCoroutine(this.AddWordsDisplay());
		if (this.maxPages >= 2)
		{
			base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
		}
		base.AddLabel("You can precede words you add with \"find\", \"search\" and \"get\" to help understanding of short spoken words", 0, 200, 0.85f, true, TextColor.Gray, false, TextAlignment.Center, 40, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x000466D8 File Offset: 0x00044AD8
	private IEnumerator AddWordsDisplay()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int minI = this.page * 5;
		int maxI = Mathf.Min(minI + 5 - 1, this.words.Count - 1);
		int buttonCount = 0;
		int i = 0;
		foreach (string text in this.words)
		{
			if (i >= minI && i <= maxI)
			{
				int num = -138 + buttonCount * 100;
				string text2 = Misc.WrapWithNewlines(text, 30, 2);
				int num2 = num - 25;
				if (text.Length > 30)
				{
					num2 -= 17;
				}
				base.AddLabel(text2, -440, num2, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 0.85f, false, TextAnchor.MiddleLeft);
				base.AddButton("removeWord", text, "- remove", "ButtonCompactNoIconShort", 325, num, null, false, 1f, TextColor.Gray, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				buttonCount++;
			}
			i++;
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x000466F3 File Offset: 0x00044AF3
	private void HideIssueLabel()
	{
		if (this.issueLabel != null)
		{
			global::UnityEngine.Object.Destroy(this.issueLabel.gameObject);
			this.issueLabel = null;
		}
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00046720 File Offset: 0x00044B20
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		this.HideIssueLabel();
		if (contextName != null)
		{
			if (!(contextName == "addWord"))
			{
				if (!(contextName == "removeWord"))
				{
					if (!(contextName == "previousPage"))
					{
						if (!(contextName == "nextPage"))
						{
							if (!(contextName == "toggleHelp"))
							{
								if (contextName == "close")
								{
									base.CloseDialog();
								}
							}
							else
							{
								string text2 = "There are hundreds of default words you can speak to find public things from across the universe. Here you can add more words or phrases you want to search for. You can also context-laser things and hit \"Tag\" to help them being found.";
								base.ToggleHelpLabel(text2, -700, 1f, 50, 0.7f);
							}
						}
						else
						{
							if (++this.page > this.maxPages - 1)
							{
								this.page = 0;
							}
							base.StartCoroutine(this.AddWordsDisplay());
						}
					}
					else
					{
						if (--this.page < 0)
						{
							this.page = this.maxPages - 1;
						}
						base.StartCoroutine(this.AddWordsDisplay());
					}
				}
				else
				{
					SearchWords.RemoveCustomSearchWord(contextId);
					this.words = SearchWords.customWords;
					Managers.inventoryManager.ActivateInventorySpeechListener();
					base.StartCoroutine(this.AddWordsDisplay());
					Managers.soundManager.Play("delete", this.transform, 0.2f, false, false);
					Managers.personManager.SetCustomSearchWords(SearchWords.GetCustomWordsString(), delegate(bool wasOk)
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
				Managers.dialogManager.GetInput(delegate(string text)
				{
					string text3 = null;
					if (!string.IsNullOrEmpty(text))
					{
						text3 = SearchWords.AddCustomSearchWord(text);
						if (text3 == null)
						{
							Managers.inventoryManager.ActivateInventorySpeechListener();
							Managers.personManager.SetCustomSearchWords(SearchWords.GetCustomWordsString(), delegate(bool wasOk)
							{
								if (!wasOk)
								{
									Managers.soundManager.Play("no", null, 1f, false, false);
								}
							});
						}
					}
					GameObject gameObject = base.SwitchTo(DialogType.InventorySearchWords, string.Empty);
					InventorySearchWordsDialog component = gameObject.GetComponent<InventorySearchWordsDialog>();
					component.issueToAlertTo = text3;
				}, string.Empty, string.Empty, 50, string.Empty, false, false, false, false, 1f, false, false, null, false);
			}
		}
	}

	// Token: 0x040007A4 RID: 1956
	private int page;

	// Token: 0x040007A5 RID: 1957
	private int maxPages = -1;

	// Token: 0x040007A6 RID: 1958
	private const int wordsPerPage = 5;

	// Token: 0x040007A7 RID: 1959
	private const int addRemoveX = 325;

	// Token: 0x040007A8 RID: 1960
	private List<string> words;

	// Token: 0x040007A9 RID: 1961
	public string issueToAlertTo;

	// Token: 0x040007AA RID: 1962
	private TextMesh issueLabel;
}
