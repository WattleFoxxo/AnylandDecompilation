using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200012D RID: 301
public class TwitchChatDialog : Dialog
{
	// Token: 0x06000B58 RID: 2904 RVA: 0x0005DC0C File Offset: 0x0005C00C
	private void OnEnable()
	{
		base.Init(base.gameObject, false, false, false);
		Vector3 vector = new Vector3(0f, 0f, -0.25f);
		this.alternateFundament = base.AddFundament();
		this.label = base.AddLabel(string.Empty, -450, -450, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 0.7f, false, TextAnchor.MiddleLeft);
		this.labelViewerCount = base.AddLabel(string.Empty, 440, -440, 1.25f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.labelMetaInfo = base.AddLabel(string.Empty, 440, -330, 0.85f, true, TextColor.Default, true, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.transform.Translate(vector);
		this.twitchIrc = base.gameObject.AddComponent<TwitchIRC>();
		this.twitchIrc.Connect(Managers.twitchManager.oauth, Managers.twitchManager.nickName, Managers.twitchManager.channelName);
		this.twitchIrc.messageReceivedEvent.AddListener(new UnityAction<string>(this.OnChatMsgReceived));
		this.UpdateMetaInfo();
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x0005DD36 File Offset: 0x0005C136
	private void UpdateMetaInfo()
	{
		base.StartCoroutine(this.DoUpdateMetaInfo());
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x0005DD48 File Offset: 0x0005C148
	private IEnumerator DoUpdateMetaInfo()
	{
		WWW www = new WWW(Managers.twitchManager.GetMetaInfoJsonUrl());
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = Managers.twitchManager.channelName;
			int num = 0;
			int num2 = 0;
			float num3 = 0f;
			string text4 = string.Empty;
			string text5 = www.text.Replace("\":null", "\":\"\"");
			JSONNode jsonnode = JSON.Parse(text5);
			bool flag = false;
			if (jsonnode != null)
			{
				JSONNode jsonnode2 = jsonnode["stream"];
				if (jsonnode2 != null)
				{
					flag = true;
					JSONNode jsonnode3 = jsonnode2["channel"];
					if (jsonnode3 != null)
					{
						if (jsonnode3["status"] != null)
						{
							text = jsonnode3["status"];
						}
						if (jsonnode3["followers"] != null)
						{
							num2 = jsonnode3["followers"].AsInt;
						}
						if (jsonnode3["name"] != null)
						{
							text3 = jsonnode3["name"];
						}
					}
					if (jsonnode2["game"] != null)
					{
						text2 = jsonnode2["game"];
					}
					if (jsonnode2["average_fps"] != null)
					{
						num3 = jsonnode2["average_fps"].AsFloat;
					}
					if (jsonnode2["viewers"] != null)
					{
						num = jsonnode2["viewers"].AsInt;
					}
					if (jsonnode2["preview"] != null && jsonnode2["preview"]["large"] != null)
					{
						text4 = jsonnode2["preview"]["large"];
					}
				}
			}
			this.labelViewerCount.text = num.ToString() + " viewers";
			if (flag)
			{
				this.labelMetaInfo.text = string.Concat(new string[]
				{
					Misc.Truncate(text, 50, true),
					"\nat twitch.tv/",
					text3,
					"/\nCategory: ",
					text2,
					" | Followers: ",
					num2.ToString(),
					"\nFrame rate: ",
					Mathf.RoundToInt(num3).ToString()
				});
			}
			else
			{
				this.labelMetaInfo.text = string.Empty;
			}
			if (!this.didShowViewerCountMessage)
			{
				this.didShowViewerCountMessage = true;
				string text6 = ((!(text != string.Empty)) ? "Channel not currently streaming" : Misc.Truncate(text, 50, true));
				this.messages.Add(string.Concat(new string[]
				{
					"[",
					text6,
					" | ",
					num.ToString(),
					" viewers | Also shown on backside]"
				}));
				this.UpdateDisplay();
			}
			if (text4 != string.Empty)
			{
				base.StartCoroutine(this.ShowStreamImage(text4));
			}
			base.Invoke("UpdateMetaInfo", 20f);
		}
		else
		{
			Log.Debug("Twitch meta data issue: " + www.error);
		}
		yield break;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x0005DD64 File Offset: 0x0005C164
	private IEnumerator ShowStreamImage(string url)
	{
		Texture2D texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		WWW www = new WWW(url);
		yield return www;
		global::UnityEngine.Object.Destroy(this.imageQuad);
		this.imageQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		this.imageQuad.transform.localScale = new Vector3(0.64f, 0.36f, 1f) * 0.35f;
		this.imageQuad.transform.parent = this.transform;
		this.imageQuad.transform.localPosition = Vector3.zero;
		this.imageQuad.transform.localRotation = Quaternion.identity;
		this.imageQuad.transform.Translate(new Vector3(-0.001f, -0.015f, -0.04f));
		this.imageQuad.transform.Rotate(new Vector3(270f, 180f, 0f));
		www.LoadImageIntoTexture(texture);
		this.imageQuad.GetComponent<Renderer>().material.mainTexture = texture;
		yield break;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0005DD88 File Offset: 0x0005C188
	private void OnChatMsgReceived(string message)
	{
		int num = message.IndexOf("PRIVMSG #");
		string text = message.Substring(num + this.twitchIrc.channelName.Length + 11);
		string text2 = message.Substring(1, message.IndexOf('!') - 1);
		string text3 = "<" + text2 + ">  " + text;
		this.messages.Add(text3);
		if (this.messages.Count > 28)
		{
			this.messages.RemoveAt(0);
		}
		this.UpdateDisplay();
		if (Managers.twitchManager.alertNewMessages)
		{
			this.AlertNewMessage();
		}
		this.ParsePotentialCommand(text);
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0005DE30 File Offset: 0x0005C230
	private void ParsePotentialCommand(string message)
	{
		if (this.hand == null)
		{
			return;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "^", "!forward" },
			{ "^^", "!forward_double" },
			{ "^^^", "!forward_triple" },
			{ "v", "!backward" },
			{ "vv", "!backward_double" },
			{ "vvv", "!backward_triple" },
			{ "<", "!left" },
			{ ">", "!right" },
			{ "w", "!forward" },
			{ "ww", "!forward_double" },
			{ "www", "!forward_triple" },
			{ "s", "!backward" },
			{ "ss", "!backward_double" },
			{ "sss", "!backward_triple" },
			{ "a", "!left" },
			{ "aa", "!left_double" },
			{ "aaa", "!left_triple" },
			{ "d", "!right" },
			{ "dd", "!right_double" },
			{ "ddd", "!right_triple" },
			{ "help", "!help" },
			{ "<<", "!left_double" },
			{ "<<<", "!left_triple" },
			{ ">>", "!right_double" },
			{ ">>>", "!right_triple" },
			{ "<<<<", "!turn" },
			{ ">>>>", "!turn" }
		};
		string text;
		if (dictionary.TryGetValue(message, out text))
		{
			message = text;
		}
		bool flag = message.IndexOf("!") == 0;
		if (flag)
		{
			string text2 = message.Substring(1);
			text2 = text2.Trim().ToLower();
			string[] array = Misc.Split(text2, " ", StringSplitOptions.RemoveEmptyEntries);
			string text3 = string.Empty;
			if (array.Length >= 2)
			{
				text2 = array[0];
				text3 = string.Join(" ", array, 1, array.Length - 1);
			}
			if (Managers.twitchManager.allowGuidingCommands && Managers.twitchManager.IsGuidingCommand(text2))
			{
				switch (text2)
				{
				case "forward":
				case "forward_double":
				case "forward_triple":
				{
					float num2 = 1f;
					if (text2 == "forward_double")
					{
						num2 = 2f;
					}
					else if (text2 == "forward_triple")
					{
						num2 = 3f;
					}
					if (this.movementSpeedForward == 0 && this.movementSpeedUpward == 0 && (this.canGoThroughWalls || Managers.personManager.ourPerson.CanWeMoveForward(0.5f * num2)))
					{
						this.movementSpeedForward = 1;
						base.Invoke("ResetMovementSpeed", num2);
					}
					break;
				}
				case "backward":
				case "backward_double":
				case "backward_triple":
				{
					float num3 = 1f;
					if (text2 == "backward_double")
					{
						num3 = 2f;
					}
					else if (text2 == "backward_triple")
					{
						num3 = 3f;
					}
					if (this.movementSpeedForward == 0 && this.movementSpeedUpward == 0 && (this.canGoThroughWalls || Managers.personManager.ourPerson.CanWeMoveBackward(0.5f * num3)))
					{
						this.movementSpeedForward = -1;
						base.Invoke("ResetMovementSpeed", num3);
					}
					break;
				}
				case "left":
					this.hand.HandleTurnAround(-1, 45f);
					break;
				case "left_double":
					this.hand.HandleTurnAround(-1, 45f);
					this.hand.HandleTurnAround(-1, 45f);
					break;
				case "left_triple":
					this.hand.HandleTurnAround(-1, 45f);
					this.hand.HandleTurnAround(-1, 45f);
					this.hand.HandleTurnAround(-1, 45f);
					break;
				case "right":
					this.hand.HandleTurnAround(1, 45f);
					break;
				case "right_double":
					this.hand.HandleTurnAround(1, 45f);
					this.hand.HandleTurnAround(1, 45f);
					break;
				case "right_triple":
					this.hand.HandleTurnAround(1, 45f);
					this.hand.HandleTurnAround(1, 45f);
					this.hand.HandleTurnAround(1, 45f);
					break;
				case "up":
					if (this.movementSpeedForward == 0 && this.movementSpeedUpward == 0 && (this.canAlwaysGoVertical || Managers.areaManager.isZeroGravity || Our.WeCanFly()))
					{
						this.movementSpeedUpward = 1;
						base.Invoke("ResetMovementSpeed", 1f);
					}
					break;
				case "down":
					if (this.movementSpeedForward == 0 && this.movementSpeedUpward == 0 && (this.canAlwaysGoVertical || Managers.areaManager.isZeroGravity || Our.WeCanFly()))
					{
						this.movementSpeedUpward = -1;
						base.Invoke("ResetMovementSpeed", 1f);
					}
					break;
				case "turn":
					this.hand.HandleTurnAround(1, 180f);
					break;
				case "areas":
					this.OutputAreasInfo();
					break;
				case "mirror":
					this.ShowMirror();
					break;
				case "to":
					if (text3 == "people")
					{
						this.TryTransportToLivelyArea();
					}
					else if (text3 == "random")
					{
						Managers.areaManager.TransportToRandomArea();
					}
					else if (text3 != string.Empty && !Managers.areaManager.TryTransportToAreaByNameOrUrlName(text3, string.Empty, false))
					{
						this.OutputInfo("Oops, that's not an area name.");
					}
					break;
				case "help":
					this.OutputHelp();
					break;
				}
			}
			else
			{
				Managers.behaviorScriptManager.TriggerTellAnyEvent(text2, true);
			}
		}
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0005E594 File Offset: 0x0005C994
	private void ShowMirror()
	{
		if (Our.mode == EditModes.Area || Our.mode == EditModes.None)
		{
			base.SwitchTo(DialogType.OwnProfile, string.Empty);
		}
		base.CancelInvoke();
		this.ResetMovementSpeed();
		base.Invoke("HideMirror", 5f);
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0005E5E0 File Offset: 0x0005C9E0
	private void HideMirror()
	{
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		OwnProfileDialog component = currentNonStartDialog.GetComponent<OwnProfileDialog>();
		if (component != null)
		{
			component.Close();
		}
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0005E60C File Offset: 0x0005CA0C
	private void ResetMovementSpeed()
	{
		this.movementSpeedForward = 0;
		this.movementSpeedUpward = 0;
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0005E61C File Offset: 0x0005CA1C
	private void OutputAreasInfo()
	{
		Managers.areaManager.GetAreaLists(delegate(AreaListSet areaListSet)
		{
			if (this == null)
			{
				return;
			}
			List<AreaOverview> suggestedAreasMix = this.GetSuggestedAreasMix(areaListSet);
			string text = string.Empty;
			foreach (AreaOverview areaOverview in suggestedAreasMix)
			{
				text += areaOverview.name;
				if (areaOverview.playerCount >= 1)
				{
					string text2 = text;
					text = string.Concat(new object[] { text2, " (", areaOverview.playerCount, ")" });
				}
				text += " | ";
			}
			text = text + areaListSet.totalPublicAreas + " public areas | ";
			if (areaListSet.totalOnline >= 2)
			{
				text = text + areaListSet.totalOnline + " people now";
			}
			text = text.ToUpper();
			text = Misc.Truncate(text, 500, true);
			this.twitchIrc.SendMsg(text);
		});
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0005E634 File Offset: 0x0005CA34
	private List<AreaOverview> GetSuggestedAreasMix(AreaListSet areaListSet)
	{
		List<AreaOverview> list = new List<AreaOverview>();
		AreaResultType[] array = new AreaResultType[]
		{
			AreaResultType.popular,
			AreaResultType.popularRandom,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.popularNewRandom,
			AreaResultType.popularRandom,
			AreaResultType.popular,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularNew,
			AreaResultType.popular,
			AreaResultType.newest,
			AreaResultType.popular,
			AreaResultType.popular,
			AreaResultType.popularNew,
			AreaResultType.popularNew,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom
		};
		foreach (AreaResultType areaResultType in array)
		{
			AreaOverview areaOverview = null;
			switch (areaResultType)
			{
			case AreaResultType.popular:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.popular, list);
				break;
			case AreaResultType.popularRandom:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.popular_rnd, list);
				break;
			case AreaResultType.newest:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.newest, list);
				break;
			case AreaResultType.popularNew:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.popularNew, list);
				break;
			case AreaResultType.popularNewRandom:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.popularNew_rnd, list);
				break;
			case AreaResultType.revisited:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.revisited, list);
				break;
			case AreaResultType.revisitedRandom:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.revisited_rnd, list);
				break;
			case AreaResultType.lively:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.lively, list);
				break;
			}
			if (areaOverview != null)
			{
				list.Add(areaOverview);
			}
		}
		return list;
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0005E778 File Offset: 0x0005CB78
	private void OutputHelp()
	{
		string text = string.Empty;
		text += "Anyland commands: ".ToUpper();
		foreach (KeyValuePair<string, string> keyValuePair in Managers.twitchManager.guidingCommandsHelp)
		{
			text = text + keyValuePair.Key + " | ";
		}
		string text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			" You're in ",
			Managers.areaManager.currentAreaName.ToUpper(),
			" & named ",
			Managers.personManager.ourPerson.screenName.ToUpper()
		});
		this.twitchIrc.SendMsg(text);
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0005E854 File Offset: 0x0005CC54
	public void OutputInfo(string text)
	{
		this.twitchIrc.SendMsg("[" + text.ToUpper() + "]");
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0005E876 File Offset: 0x0005CC76
	private void TryTransportToLivelyArea()
	{
		Managers.areaManager.GetAreaLists(delegate(AreaListSet areaListSet)
		{
			if (this == null)
			{
				return;
			}
			this.RemoveCurrentAreaFromList(areaListSet.lively);
			if (areaListSet.lively.Count >= 1)
			{
				int num = 0;
				if (areaListSet.lively.Count >= 2)
				{
					num = global::UnityEngine.Random.Range(0, areaListSet.lively.Count - 1);
				}
				string name = areaListSet.lively[num].name;
				Managers.areaManager.TryTransportToAreaByNameOrUrlName(name, string.Empty, false);
			}
		});
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0005E890 File Offset: 0x0005CC90
	private void RemoveCurrentAreaFromList(List<AreaOverview> areas)
	{
		for (int i = areas.Count - 1; i >= 0; i--)
		{
			if (areas[i].name == Managers.areaManager.currentAreaName)
			{
				areas.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0005E8DD File Offset: 0x0005CCDD
	private void AlertNewMessage()
	{
		this.hand.StartShrinkingHapticPulseOverTime();
		Managers.soundManager.Play("newTwitchMessage", this.transform, 0.1f, false, false);
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0005E908 File Offset: 0x0005CD08
	private void UpdateDisplay()
	{
		List<string> list = new List<string>();
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			list.Add(this.messages[i]);
		}
		string text = string.Empty;
		foreach (string text2 in list)
		{
			text = Misc.WrapWithNewlines(text2, 46, 4) + "\n\n" + text;
			int num = text.Split(new char[] { '\n' }).Length - 1;
			if (num >= 28)
			{
				break;
			}
		}
		this.label.text = text;
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0005E9DC File Offset: 0x0005CDDC
	private new void Update()
	{
		if (this.movementSpeedForward == 1)
		{
			Managers.personManager.ourPerson.MoveUsForward(1f);
		}
		else if (this.movementSpeedForward == -1)
		{
			Managers.personManager.ourPerson.MoveUsBackward(1f);
		}
		if (this.movementSpeedUpward == 1)
		{
			Managers.personManager.ourPerson.MoveUsUpward(1f);
		}
		else if (this.movementSpeedUpward == -1)
		{
			Managers.personManager.ourPerson.MoveUsDownward(1f);
		}
		base.ReactToOnClick();
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0005EA79 File Offset: 0x0005CE79
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (contextName == "close")
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x0005EAA8 File Offset: 0x0005CEA8
	private void OnDisable()
	{
		this.didShowViewerCountMessage = false;
		base.CancelInvoke();
		this.messages = new List<string>();
		global::UnityEngine.Object.Destroy(this.imageQuad);
		global::UnityEngine.Object.Destroy(this.labelMetaInfo.gameObject);
		global::UnityEngine.Object.Destroy(this.labelViewerCount.gameObject);
		global::UnityEngine.Object.Destroy(this.twitchIrc);
		global::UnityEngine.Object.Destroy(this.label.gameObject);
		global::UnityEngine.Object.Destroy(this.alternateFundament);
	}

	// Token: 0x0400089B RID: 2203
	private const int maxMessageLines = 28;

	// Token: 0x0400089C RID: 2204
	private const int maxMessages = 28;

	// Token: 0x0400089D RID: 2205
	private const int maxRowLength = 46;

	// Token: 0x0400089E RID: 2206
	private List<string> messages = new List<string>();

	// Token: 0x0400089F RID: 2207
	private TwitchIRC twitchIrc;

	// Token: 0x040008A0 RID: 2208
	private TextMesh label;

	// Token: 0x040008A1 RID: 2209
	private GameObject alternateFundament;

	// Token: 0x040008A2 RID: 2210
	private TextMesh labelMetaInfo;

	// Token: 0x040008A3 RID: 2211
	private TextMesh labelViewerCount;

	// Token: 0x040008A4 RID: 2212
	private const float metaInfoUpdateIntervalSeconds = 20f;

	// Token: 0x040008A5 RID: 2213
	private bool didShowViewerCountMessage;

	// Token: 0x040008A6 RID: 2214
	private GameObject imageQuad;

	// Token: 0x040008A7 RID: 2215
	private int movementSpeedForward;

	// Token: 0x040008A8 RID: 2216
	private int movementSpeedUpward;

	// Token: 0x040008A9 RID: 2217
	private bool canGoThroughWalls = true;

	// Token: 0x040008AA RID: 2218
	private bool canAlwaysGoVertical = true;
}
