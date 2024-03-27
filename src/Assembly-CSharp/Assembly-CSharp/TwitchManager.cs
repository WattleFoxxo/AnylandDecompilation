using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class TwitchManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000243 RID: 579
	// (get) Token: 0x060013D9 RID: 5081 RVA: 0x000B41AF File Offset: 0x000B25AF
	// (set) Token: 0x060013DA RID: 5082 RVA: 0x000B41B7 File Offset: 0x000B25B7
	public ManagerStatus status { get; private set; }

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x060013DB RID: 5083 RVA: 0x000B41C0 File Offset: 0x000B25C0
	// (set) Token: 0x060013DC RID: 5084 RVA: 0x000B41C8 File Offset: 0x000B25C8
	public string failMessage { get; private set; }

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x060013DD RID: 5085 RVA: 0x000B41D1 File Offset: 0x000B25D1
	// (set) Token: 0x060013DE RID: 5086 RVA: 0x000B41D9 File Offset: 0x000B25D9
	public string nickName { get; private set; }

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x060013DF RID: 5087 RVA: 0x000B41E2 File Offset: 0x000B25E2
	// (set) Token: 0x060013E0 RID: 5088 RVA: 0x000B41EA File Offset: 0x000B25EA
	public string channelName { get; private set; }

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x060013E1 RID: 5089 RVA: 0x000B41F3 File Offset: 0x000B25F3
	// (set) Token: 0x060013E2 RID: 5090 RVA: 0x000B41FB File Offset: 0x000B25FB
	public string oauth { get; private set; }

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060013E3 RID: 5091 RVA: 0x000B4204 File Offset: 0x000B2604
	// (set) Token: 0x060013E4 RID: 5092 RVA: 0x000B420C File Offset: 0x000B260C
	public bool alertNewMessages { get; private set; }

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x060013E5 RID: 5093 RVA: 0x000B4215 File Offset: 0x000B2615
	// (set) Token: 0x060013E6 RID: 5094 RVA: 0x000B421D File Offset: 0x000B261D
	public bool allowGuidingCommands { get; private set; }

	// Token: 0x060013E7 RID: 5095 RVA: 0x000B4228 File Offset: 0x000B2628
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.nickName = PlayerPrefs.GetString("twitch_nickName", null);
		this.channelName = PlayerPrefs.GetString("twitch_channelName", null);
		this.oauth = PlayerPrefs.GetString("twitch_oauth", null);
		this.alertNewMessages = PlayerPrefs.GetInt("twitch_alertNewMessages", 1) == 1;
		this.allowGuidingCommands = PlayerPrefs.GetInt("twitch_allowGuidingCommands", 0) == 1;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x000B429E File Offset: 0x000B269E
	public void SetNickName(string s)
	{
		this.nickName = s;
		PlayerPrefs.SetString("twitch_nickName", this.nickName);
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x000B42B7 File Offset: 0x000B26B7
	public void SetChannelName(string s)
	{
		this.channelName = s;
		PlayerPrefs.SetString("twitch_channelName", this.channelName);
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x000B42D0 File Offset: 0x000B26D0
	public void SetOauth(string s)
	{
		this.oauth = s;
		PlayerPrefs.SetString("twitch_oauth", this.oauth);
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x000B42E9 File Offset: 0x000B26E9
	public void SetAlertNewMessages(bool state)
	{
		this.alertNewMessages = state;
		PlayerPrefs.SetInt("twitch_alertNewMessages", (!state) ? 0 : 1);
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x000B4309 File Offset: 0x000B2709
	public void SetAllowGuidingCommands(bool state)
	{
		this.allowGuidingCommands = state;
		PlayerPrefs.SetInt("twitch_allowGuidingCommands", (!state) ? 0 : 1);
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x000B4329 File Offset: 0x000B2729
	public bool AllRequiredInfoIsSet()
	{
		return !string.IsNullOrEmpty(this.channelName) && !string.IsNullOrEmpty(this.nickName) && !string.IsNullOrEmpty(this.oauth);
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x000B435C File Offset: 0x000B275C
	public bool IsGuidingCommand(string expandedCommand)
	{
		return this.guidingCommands.Contains(expandedCommand);
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x000B436A File Offset: 0x000B276A
	public string GetMetaInfoJsonUrl()
	{
		return "https://api.twitch.tv/kraken/streams/" + this.channelName + "?client_id=5mr9xxb64fvqw6qct6xlh4gbimwjy4";
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x000B4381 File Offset: 0x000B2781
	public bool ChatDialogShows()
	{
		return this.chatDialogLeft.activeSelf || this.chatDialogRight.activeSelf;
	}

	// Token: 0x0400120F RID: 4623
	private const string clientId = "5mr9xxb64fvqw6qct6xlh4gbimwjy4";

	// Token: 0x04001210 RID: 4624
	public GameObject chatDialogLeft;

	// Token: 0x04001211 RID: 4625
	public GameObject chatDialogRight;

	// Token: 0x04001212 RID: 4626
	private string[] guidingCommands = new string[]
	{
		"forward", "forward_double", "forward_triple", "backward", "backward_double", "backward_triple", "left", "left_double", "left_triple", "right",
		"right_double", "right_triple", "to", "help", "turn", "areas", "mirror", "up", "down"
	};

	// Token: 0x04001213 RID: 4627
	public Dictionary<string, string> guidingCommandsHelp = new Dictionary<string, string>
	{
		{ "!forward  or ^,w", "Move a step forward" },
		{ "!backward  or v,s", "Move a step backward" },
		{ "!left  or <,a", "Turn left by 45 degrees" },
		{ "!right  or >,d", "Turn right by 45 degrees" },
		{ "!up", "Moves up if zero gravity/ editor" },
		{ "!down", "Moves down if zero gravity/ editor" },
		{ "!turn", "Turns by 180 degrees" },
		{ "!areas", "Lists some areas" },
		{ "!to somearea", "Sends to area named \"somearea\"" },
		{ "!to random", "Sends to a random area" },
		{ "!to people", "Sends to lively area if possible" },
		{ "!mirror", "Briefly show mirror" },
		{ "!help  or help", "Show commands help" }
	};
}
