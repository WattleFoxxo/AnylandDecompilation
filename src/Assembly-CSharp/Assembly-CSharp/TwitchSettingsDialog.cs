using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
public class TwitchSettingsDialog : Dialog
{
	// Token: 0x06000B73 RID: 2931 RVA: 0x0005F4B4 File Offset: 0x0005D8B4
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		Side side = ((!(this.transform.parent.name == "HandCoreLeft")) ? Side.Right : Side.Left);
		string text = ((side != Side.Left) ? "Right" : "Left");
		this.twitchChatDialog = Managers.treeManager.GetObject("/OurPersonRig/HandCore" + text + "/" + DialogType.TwitchChat.ToString());
		base.AddHeadline("Twitch Chat", -370, -460, TextColor.Default, TextAlignment.Left, false);
		base.AddLabel("See what people say when you stream.\n\"!words\" are passed on with \"tell any\"", -370, -390, 0.8f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		int num = -250;
		int num2 = 90;
		base.AddModelButton("EditTextButton", "editTwitchNickName", null, -400, num, false);
		string text2 = "your nickname: " + (string.IsNullOrEmpty(Managers.twitchManager.nickName) ? "..." : Managers.twitchManager.nickName);
		text2 = Misc.Truncate(text2, 38, true);
		base.AddLabel(text2, -358, num + -7, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		num += num2;
		base.AddModelButton("EditTextButton", "editTwitchChannelName", null, -400, num, false);
		string text3 = "channel name: " + (string.IsNullOrEmpty(Managers.twitchManager.channelName) ? "..." : Managers.twitchManager.channelName);
		text3 = Misc.Truncate(text3, 38, true);
		base.AddLabel(text3, -358, num + -7, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		num += num2;
		base.AddModelButton("EditTextButton", "editTwitchOauth", null, -400, num, false);
		string text4 = "authentication string: " + (string.IsNullOrEmpty(Managers.twitchManager.oauth) ? "..." : "**********");
		text4 = Misc.Truncate(text4, 38, true);
		base.AddLabel(text4, -358, num + -7, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddLabel("GET IT AT  twitchapps.com/tmi/", -358, num + 40, 0.8f, false, TextColor.Gray, true, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		num += num2;
		if (Managers.twitchManager.AllRequiredInfoIsSet())
		{
			num2 = 130;
			num = 160;
			base.AddCheckbox("showTwitchChat", null, "Show chat", 0, num, this.twitchChatDialog.activeSelf, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
			num += num2;
			base.AddCheckbox("alertTwitchMessages", null, "Alert when message arrives", 0, num, Managers.twitchManager.alertNewMessages, 0.8f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
			base.AddButton("showTwitchGuidingDialog", null, "Guiding commands...", "ButtonCompactNoIcon", 0, 410, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0005F7F8 File Offset: 0x0005DBF8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "showTwitchChat":
			this.twitchChatDialog.SetActive(state);
			break;
		case "alertTwitchMessages":
			Managers.twitchManager.SetAlertNewMessages(state);
			break;
		case "editTwitchNickName":
			this.twitchChatDialog.SetActive(false);
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (text != null)
				{
					Managers.twitchManager.SetNickName(text);
				}
				base.SwitchTo(DialogType.TwitchSettings, string.Empty);
			}, contextName, Managers.twitchManager.nickName, 500, "you can also ctrl+v", true, true, false, false, 1f, false, false, null, false);
			break;
		case "editTwitchChannelName":
			this.twitchChatDialog.SetActive(false);
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (text != null)
				{
					Managers.twitchManager.SetChannelName(text);
				}
				base.SwitchTo(DialogType.TwitchSettings, string.Empty);
			}, contextName, Managers.twitchManager.channelName, 500, "you can also ctrl+v", true, true, false, false, 1f, false, false, null, false);
			break;
		case "editTwitchOauth":
			this.twitchChatDialog.SetActive(false);
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (text != null)
				{
					if (text.IndexOf("oauth:") == 0)
					{
						Managers.twitchManager.SetOauth(text);
						base.SwitchTo(DialogType.TwitchSettings, string.Empty);
					}
					else
					{
						Managers.dialogManager.ShowInfo("Oops, the authentication string must start with \"oauth:\"", false, true, -1, DialogType.TwitchSettings, 1f, false, TextColor.Default, TextAlignment.Left);
					}
				}
				else
				{
					base.SwitchTo(DialogType.TwitchSettings, string.Empty);
				}
			}, contextName, Managers.twitchManager.oauth, 500, "you can also ctrl+v", true, true, false, false, 1f, false, false, null, false);
			break;
		case "showTwitchGuidingDialog":
			base.SwitchTo(DialogType.TwitchGuiding, string.Empty);
			break;
		case "back":
			base.SwitchTo(DialogType.DesktopCamera, string.Empty);
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x040008AD RID: 2221
	private GameObject twitchChatDialog;
}
