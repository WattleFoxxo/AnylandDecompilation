using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class SoftbanDialog : Dialog
{
	// Token: 0x06000B93 RID: 2963 RVA: 0x00060F70 File Offset: 0x0005F370
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		bool isSoftBanned = Managers.personManager.ourPerson.isSoftBanned;
		int num = -250;
		List<string> list = new List<string>();
		list.Add("Being very disrespectful or rude to others");
		list.Add("Being below 13 years of age");
		list.Add("Very offensive creations");
		string[] flagTags = Managers.personManager.ourPerson.flagTags;
		float num2 = 0.6f;
		string text = ((!isSoftBanned) ? "Oops, you got many flag reports, which may lead to a ban." : "Oops, your account is banned due to too many flag reports.");
		string text2 = ((flagTags == null || flagTags.Length < 1) ? "While not necessarily related to anything you did in specific,\nhere are some general issues that can cause flags:" : "Reasons marked in red are specific things people reported,\nthe others are general issues that can cause flags:");
		base.AddLabel(text + "\n" + text2, -435, -360, num2, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		if (flagTags != null)
		{
			int num3 = 0;
			while (num3 < flagTags.Length && num3 < 8)
			{
				if (!string.IsNullOrEmpty(flagTags[num3]))
				{
					base.AddLabel("• " + flagTags[num3], -435, num, num2, false, TextColor.Red, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
					num += 40;
				}
				num3++;
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			base.AddLabel("• " + list[i], -435, num, num2, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			num += 40;
		}
		string text3 = "Please check the etiquette & terms at anyland.com/info\nfor more information as to what might have caused flag\nreports. We hope this information helps you find some\nclues as to what may be going wrong!";
		base.AddLabel(text3, -435, num + 30, num2, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		Managers.achievementManager.RegisterAchievement(Achievement.SawTheirSoftbanDialog);
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00061133 File Offset: 0x0005F533
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (contextName == "close")
			{
				base.CloseDialog();
			}
		}
	}
}
