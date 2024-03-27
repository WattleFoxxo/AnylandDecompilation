using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class TwitchGuidingDialog : Dialog
{
	// Token: 0x06000B6F RID: 2927 RVA: 0x0005F2E8 File Offset: 0x0005D6E8
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Twitch Guiding", -370, -460, TextColor.Default, TextAlignment.Left, false);
		this.ShowCommandsHelp();
		base.AddCheckbox("toggleAllowGuidingCommands", null, "allow", 0, -300, Managers.twitchManager.allowGuidingCommands, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0005F368 File Offset: 0x0005D768
	private void ShowCommandsHelp()
	{
		int num = 50;
		int num2 = 0;
		foreach (KeyValuePair<string, string> keyValuePair in Managers.twitchManager.guidingCommandsHelp)
		{
			string key = keyValuePair.Key;
			string text = Misc.WrapWithNewlines(keyValuePair.Value, 34, 2);
			base.AddLabel(key, -430, -200 + num * num2, 0.7f, false, TextColor.Default, true, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			base.AddLabel(text, -140, -200 + num * num2, 0.7f, false, TextColor.Gray, true, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			num2++;
		}
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0005F434 File Offset: 0x0005D834
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "toggleAllowGuidingCommands"))
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
					base.SwitchTo(DialogType.TwitchSettings, string.Empty);
				}
			}
			else
			{
				Managers.twitchManager.SetAllowGuidingCommands(state);
			}
		}
	}

	// Token: 0x040008AC RID: 2220
	private GameObject twitchChatDialog;
}
