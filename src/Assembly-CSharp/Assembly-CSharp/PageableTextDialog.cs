using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class PageableTextDialog : Dialog
{
	// Token: 0x06000AB5 RID: 2741 RVA: 0x00053FBC File Offset: 0x000523BC
	public void Start()
	{
		base.Init(base.gameObject, false, true, false);
		base.AddFundament();
		base.AddCloseButton();
		if (this.headline != null)
		{
			base.AddLabel(Misc.Truncate(this.headline, 70, true), -900, -955, 1.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		this.textMesh = base.AddLabel(string.Empty, -900, -750, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.textMesh.richText = true;
		this.text = this.ReplaceMarkup(this.text);
		this.text = this.text.ToUpper();
		this.textByPage = this.GetTextSplitIntoPages(this.text);
		this.maxPages = this.textByPage.Count;
		if (this.maxPages > 1)
		{
			float num = 1f;
			if (this.maxPages > 10)
			{
				num = 0.85f;
			}
			base.AddDefaultPagingButtons(140, 880, "Page", false, 0, 0.85f, false);
			this.pageLabel = base.AddLabel(string.Empty, 0, 855, num, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		this.UpdateText();
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x0005410C File Offset: 0x0005250C
	private string ReplaceMarkup(string s)
	{
		s = s.Replace("<h2>", Environment.NewLine + "<size=120>");
		for (int i = 3; i <= 6; i++)
		{
			s = s.Replace("<h" + i + ">", Environment.NewLine + "<size=100>");
		}
		for (int j = 1; j <= 6; j++)
		{
			s = s.Replace("</h" + j + ">", "</size>" + Environment.NewLine + Environment.NewLine);
		}
		s = s.TrimStart(new char[] { '\r' });
		s = s.TrimStart(new char[] { '\n' });
		s = s.TrimEnd(new char[] { '\r' });
		s = s.TrimEnd(new char[] { '\n' });
		return s;
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00054204 File Offset: 0x00052604
	private List<string> GetTextSplitIntoPages(string text)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		char c = '\n';
		text = text.Replace(Environment.NewLine, c.ToString());
		string text2 = string.Empty;
		foreach (char c2 in text)
		{
			if (c2 == c)
			{
				list2.Add(text2.Trim());
				text2 = string.Empty;
			}
			else
			{
				text2 += c2;
				if (text2.Length > 70)
				{
					int num = text2.LastIndexOf(' ');
					if (num >= 1)
					{
						list2.Add(text2.Substring(0, num).Trim());
						text2 = text2.Substring(num);
					}
				}
			}
		}
		if (text2 != string.Empty)
		{
			list2.Add(text2.Trim());
		}
		string text4 = string.Empty;
		int num2 = 0;
		foreach (string text5 in list2)
		{
			text4 = text4 + text5 + Environment.NewLine;
			if (++num2 > 26)
			{
				text4 = text4.Replace(c.ToString(), "[newline]");
				text4 = text4.Replace("[newline]", Environment.NewLine);
				list.Add(text4);
				num2 = 0;
				text4 = string.Empty;
			}
		}
		if (text4 != string.Empty)
		{
			text4 = text4.Replace(c.ToString(), "[newline]");
			text4 = text4.Replace("[newline]", Environment.NewLine);
			list.Add(text4);
		}
		return list;
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x000543E4 File Offset: 0x000527E4
	private void UpdateText()
	{
		this.textMesh.text = this.textByPage[this.page];
		if (this.pageLabel != null)
		{
			this.pageLabel.text = this.page + 1 + " / " + this.maxPages;
		}
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x0005444C File Offset: 0x0005284C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "previousPage"))
			{
				if (!(contextName == "nextPage"))
				{
					if (contextName == "close")
					{
						base.CloseDialog();
					}
				}
				else
				{
					if (++this.page > this.maxPages - 1)
					{
						this.page = 0;
					}
					this.UpdateText();
				}
			}
			else
			{
				if (--this.page < 0)
				{
					this.page = this.maxPages - 1;
				}
				this.UpdateText();
			}
		}
	}

	// Token: 0x0400081B RID: 2075
	public string headline;

	// Token: 0x0400081C RID: 2076
	public string text;

	// Token: 0x0400081D RID: 2077
	private TextMesh textMesh;

	// Token: 0x0400081E RID: 2078
	private TextMesh pageLabel;

	// Token: 0x0400081F RID: 2079
	private const int maxLineLength = 70;

	// Token: 0x04000820 RID: 2080
	private const int linesPerPage = 26;

	// Token: 0x04000821 RID: 2081
	private int page;

	// Token: 0x04000822 RID: 2082
	private int maxPages = -1;

	// Token: 0x04000823 RID: 2083
	private List<string> textByPage;
}
