using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class PollManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000227 RID: 551
	// (get) Token: 0x0600126C RID: 4716 RVA: 0x0009BC52 File Offset: 0x0009A052
	// (set) Token: 0x0600126D RID: 4717 RVA: 0x0009BC5A File Offset: 0x0009A05A
	public ManagerStatus status { get; private set; }

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x0600126E RID: 4718 RVA: 0x0009BC63 File Offset: 0x0009A063
	// (set) Token: 0x0600126F RID: 4719 RVA: 0x0009BC6B File Offset: 0x0009A06B
	public string failMessage { get; private set; }

	// Token: 0x06001270 RID: 4720 RVA: 0x0009BC74 File Offset: 0x0009A074
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x0009BC84 File Offset: 0x0009A084
	public void StartPolling()
	{
		base.InvokeRepeating("_PollServer", 15f, 15f);
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x0009BC9B File Offset: 0x0009A09B
	public void StopPolling()
	{
		base.CancelInvoke("_PollServer");
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x0009BCA8 File Offset: 0x0009A0A8
	public void ForcePoll()
	{
		this._PollServer();
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x0009BCB0 File Offset: 0x0009A0B0
	public void _PollServer()
	{
		if (!(Managers.areaManager == null) && !string.IsNullOrEmpty(Managers.areaManager.currentAreaId))
		{
			base.StartCoroutine(Managers.serverManager.PollServer(Managers.areaManager.currentAreaId, delegate(PollServerResponse response)
			{
				if (response.error != null)
				{
					Log.Error(response.error);
					if (Misc.getResponseCode(response.www) == 401)
					{
						Log.Warning("Athentication lost - trying to reauth");
						Managers.serverManager.TryReauthenticate();
					}
					else if (!Misc.wwwObjectHasStatusHeader(response.www))
					{
						this.internetConnectionFailCounter++;
						if (this.internetConnectionFailCounter >= 2)
						{
							Managers.errorManager.ShowCriticalHaltError("Oops, it seems there's no Internet connection.", true, false, true, false, false);
						}
					}
				}
				else
				{
					this.internetConnectionFailCounter = 0;
					this.HandleForceUpdateCheck(response.versionMajorServerAndClient);
					Universe.versionMinorServerOnly = response.versionMinorServerOnly;
					if (!string.IsNullOrEmpty(response.pingFromPersonId))
					{
						Log.Info("Ping message received", false);
						this.HandleReceivedPingMessage(new global::Ping(response.pingFromPersonId, response.pingFromPersonName, response.pingFromAreaName));
					}
					if (!string.IsNullOrEmpty(response.generalMessage))
					{
						Log.Info("General message received", false);
						this.HandleReceivedGeneralMessage(response.generalMessage, response.generalMessage_data1, response.generalMessage_data2);
					}
					if (response.unseenGiftsExist)
					{
						this.HandleUnseenGiftsExist();
					}
				}
			}));
		}
		else
		{
			Log.Info("Not Polling server this time (currentAreaId not set)", false);
		}
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x0009BD18 File Offset: 0x0009A118
	private void HandleForceUpdateCheck(string versionMajorServerAndClient)
	{
		if (!this.startedForceUpdate && Misc.ForceUpdateIsNeeded(versionMajorServerAndClient))
		{
			this.startedForceUpdate = true;
			Managers.dialogManager.ShowInfo("There's a fresh update with improvements! And sorry, this update is required and will need to restart Anyland in a few seconds...", true, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
			base.Invoke("ForceUpdate", 30f);
		}
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x0009BD6F File Offset: 0x0009A16F
	private void ForceUpdate()
	{
		Misc.ForceUpdate();
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x0009BD78 File Offset: 0x0009A178
	private void HandleReceivedPingMessage(global::Ping ping)
	{
		if (Managers.personManager.ourPerson != null && !Managers.personManager.ourPerson.isSoftBanned && Universe.features.pingAlerts && !Our.stopPingsAndAlerts && Managers.settingManager != null)
		{
			if (Managers.settingManager.GetState(Setting.OnlyFriendsCanPingUs))
			{
				Managers.personManager.GetPersonInfo(ping.originPersonId, delegate(PersonInfo personInfo)
				{
					if (personInfo.isFriend)
					{
						this.ShowPing(ping);
					}
				});
			}
			else
			{
				this.ShowPing(ping);
			}
		}
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x0009BE30 File Offset: 0x0009A230
	private void ShowPing(global::Ping ping)
	{
		if (this.receivedPings.Count >= 10000)
		{
			this.receivedPings = new List<global::Ping>();
		}
		for (int i = 0; i < this.receivedPings.Count; i++)
		{
			if (this.receivedPings[i].originPersonId == ping.originPersonId)
			{
				this.receivedPings.RemoveAt(i);
				break;
			}
		}
		this.receivedPings.Add(ping);
		Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "ping received from " + ping.originPersonName.ToLower(), true);
		Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "ping received", true);
		Managers.dialogManager.ShowPing(ping);
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x0009BF08 File Offset: 0x0009A308
	private void HandleReceivedGeneralMessage(string messageType, string data1, string data2)
	{
		Log.Info("Got general message : " + messageType, false);
		if (messageType == "1")
		{
			Managers.personManager.ourPerson.isSoftBanned = true;
			Managers.areaManager.ForceLoadHomeArea();
		}
		else if (messageType == "2")
		{
			Managers.personManager.ourPerson.isHardBanned = true;
			Misc.LogOff();
			Managers.serverManager.StartAuthentication();
		}
		else if (messageType == "3")
		{
			this.ShowNewPersonBornAlertDialogIfAppropriate(data1, data2);
		}
		else
		{
			Log.Error("Unknown gmt : " + messageType);
		}
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x0009BFB8 File Offset: 0x0009A3B8
	private void HandleUnseenGiftsExist()
	{
		bool flag = this.showAlertForNewGifts && Time.realtimeSinceStartup >= 10f && (Our.mode == EditModes.None || Our.mode == EditModes.Area);
		if (flag)
		{
			GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
			DialogType dialogType = ((!(currentNonStartDialog != null)) ? DialogType.None : currentNonStartDialog.GetComponent<Dialog>().dialogType);
			if (dialogType != DialogType.Keyboard && dialogType != DialogType.ApproveBody && dialogType != DialogType.Gifts)
			{
				AlertDialog alertDialog = Managers.dialogManager.ShowInfo("You've got a Nift!", true, false, 2, DialogType.OwnProfile, 1f, false, TextColor.Gold, TextAlignment.Center);
				if (alertDialog != null)
				{
					this.showAlertForNewGifts = false;
					Managers.soundManager.Play("purchaseSuccess", alertDialog.transform.parent, 0.125f, true, false);
					GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/GiftPurchaseSuccess", typeof(GameObject))) as GameObject;
					gameObject.transform.parent = alertDialog.transform;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Misc.GetUniformVector3(0.1f);
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.Translate(new Vector3(0f, 0.015f, 0f));
					gameObject.transform.Rotate(new Vector3(90f, 0f, 0f));
				}
			}
		}
		Managers.achievementManager.RegisterAchievement(Achievement.ReceivedGift);
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x0009C140 File Offset: 0x0009A540
	private void ShowNewPersonBornAlertDialogIfAppropriate(string newUserName, string newUserHomeAreaName)
	{
		if (newUserName != Managers.personManager.ourPerson.screenName && newUserHomeAreaName != Managers.areaManager.currentAreaName && Managers.personManager.ourPerson.ageInSecs >= 900)
		{
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "someone was born", true);
			if (Universe.features.newbornAlerts && !Our.stopPingsAndAlerts)
			{
				GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
				DialogType dialogType = ((!(currentNonStartDialog != null)) ? DialogType.None : currentNonStartDialog.GetComponent<Dialog>().dialogType);
				if (dialogType != DialogType.Keyboard && dialogType != DialogType.ApproveBody)
				{
					Hand ahandOfOurs = Misc.GetAHandOfOurs();
					if (ahandOfOurs != null)
					{
						Universe.hearEchoOfMyVoice = false;
						ahandOfOurs.SwitchToNewDialog(DialogType.Alert, string.Empty);
						AlertDialog component = ahandOfOurs.currentDialog.GetComponent<AlertDialog>();
						component.SetNewPersonBorn(newUserName, newUserHomeAreaName);
					}
				}
				else
				{
					Managers.soundManager.Play("newPersonBorn", currentNonStartDialog.transform, 0.2f, false, false);
				}
			}
		}
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x0009C258 File Offset: 0x0009A658
	public void RemovePingsReceivedFromArea(string areaName)
	{
		for (int i = this.receivedPings.Count - 1; i >= 0; i--)
		{
			if (this.receivedPings[i].originAreaName == areaName)
			{
				this.receivedPings.RemoveAt(i);
			}
		}
	}

	// Token: 0x0400116C RID: 4460
	private const float POLL_PERIOD_SECS = 15f;

	// Token: 0x0400116D RID: 4461
	private const int HTTP_CODE_UNAUTHORIZED = 401;

	// Token: 0x0400116E RID: 4462
	private int internetConnectionFailCounter;

	// Token: 0x0400116F RID: 4463
	private const int MAX_CONSECUTIVE_CONNECTION_FAILS = 2;

	// Token: 0x04001170 RID: 4464
	private bool startedForceUpdate;

	// Token: 0x04001171 RID: 4465
	public List<global::Ping> receivedPings = new List<global::Ping>();

	// Token: 0x04001172 RID: 4466
	public bool showAlertForNewGifts = true;
}
