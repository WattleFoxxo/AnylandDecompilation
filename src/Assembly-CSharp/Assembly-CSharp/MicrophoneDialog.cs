using System;
using System.Collections;
using DaikonForge.VoIP;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000122 RID: 290
public class MicrophoneDialog : Dialog
{
	// Token: 0x06000ADF RID: 2783 RVA: 0x000560EC File Offset: 0x000544EC
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddMirror();
		Universe.hearEchoOfMyVoice = true;
		this.microphone = Managers.personManager.ourPerson.Head.GetComponent<MicrophoneInputDevice>();
		base.AddHeadline("Microphone", -370, -460, TextColor.Default, TextAlignment.Left, false);
		this.doManyDevicesTest = false;
		base.AddMicrophoneIndicators(170);
		base.AddSeparator(0, 320, false);
		this.AddMuteButton();
		this.AddMicrophoneButtons();
		base.AddButton("copyInfo", null, "Copy info", "ButtonCompactNoIconShortCentered", 0, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, true, false);
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x000561C0 File Offset: 0x000545C0
	private void AddMuteButton()
	{
		global::UnityEngine.Object.Destroy(this.muteButton);
		this.muteButton = base.AddCheckbox("muted", null, "Mute me", 0, 400, !Managers.settingManager.GetState(Setting.Microphone), 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Muted);
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x00056210 File Offset: 0x00054610
	private void AddMicrophoneButtons()
	{
		base.StartCoroutine(this.DoAddMicrophoneButtons());
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x00056220 File Offset: 0x00054620
	private IEnumerator DoAddMicrophoneButtons()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int y = -280;
		string[] devices = Microphone.devices;
		if (this.doManyDevicesTest)
		{
			string[] array = new string[]
			{
				"Hello", "there", "this", "is", "just", "a", "small", "test", "of", "many",
				"buttons including those with a very long device name text"
			};
			devices = array;
		}
		this.microphonesFound = devices.Length;
		if (this.microphonesFound >= 1)
		{
			int num = 4;
			int num2 = 0;
			int num3 = Mathf.Min(num2 + num, this.microphonesFound) - 1;
			this.maxPages = 1;
			if (this.microphonesFound > num)
			{
				base.AddDefaultPagingButtons(80, 90, "Page", false, 0, 0.85f, false);
				num2 = this.page * 3;
				num3 = Mathf.Min(num2 + 3, this.microphonesFound) - 1;
				this.maxPages = Mathf.CeilToInt((float)this.microphonesFound / 3f);
			}
			int num4 = 0;
			for (int i = num2; i <= num3; i++)
			{
				string text = devices[i];
				string text2 = Misc.Truncate(text, 38, true);
				bool flag = Microphone.IsRecording(text);
				base.AddCheckbox("setToThisMicrophone", text, text2, 0, y + num4 * 115, flag, 0.7f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
				num4++;
			}
		}
		else
		{
			base.AddLabel("No microphone found", 0, -120, 0.9f, false, TextColor.Red, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0005623C File Offset: 0x0005463C
	private new void Update()
	{
		base.UpdateMicrophoneIndicator(this.microphone);
		if (!this.doManyDevicesTest)
		{
			this.UpdateButtonsIfMicrophonesChange();
		}
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.C))
		{
			this.CopyInfoToClipboard();
		}
		base.Update();
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00056288 File Offset: 0x00054688
	private void UpdateButtonsIfMicrophonesChange()
	{
		if (this.microphonesFound != -1 && this.microphonesFound != Microphone.devices.Length)
		{
			this.microphonesFound = -1;
			this.page = 0;
			base.CancelInvoke("AddMicrophoneButtons");
			base.Invoke("AddMicrophoneButtons", 0.75f);
		}
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x000562DC File Offset: 0x000546DC
	private void CleanUpIfNeeded()
	{
		if (!this.didCleanUp)
		{
			this.didCleanUp = true;
			base.RemoveMirror();
			Our.dialogToGoBackTo = DialogType.None;
			Universe.hearEchoOfMyVoice = false;
			Our.SetPreviousMode();
		}
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x00056307 File Offset: 0x00054707
	private void OnDestroy()
	{
		this.CleanUpIfNeeded();
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x00056310 File Offset: 0x00054710
	private void CopyInfoToClipboard()
	{
		string text = string.Empty;
		string text2 = new string('-', 50);
		text = text + text2 + Environment.NewLine;
		if (Microphone.devices.Length >= 1)
		{
			foreach (string text3 in Microphone.devices)
			{
				text = text + "- " + text3;
				if (Microphone.IsRecording(text3))
				{
					text += " [Active]";
				}
				text += Environment.NewLine;
			}
		}
		else
		{
			text = text + "No microphones found" + Environment.NewLine;
		}
		text += Environment.NewLine;
		text = text + "Mute me: " + Misc.BoolToYesNo(!Managers.settingManager.GetState(Setting.Microphone)) + Environment.NewLine;
		text = text + "VR present: " + Misc.BoolToYesNo(XRDevice.isPresent) + Environment.NewLine;
		text = text + "VR device: " + ((XRDevice.model == null) ? "-" : XRDevice.model) + Environment.NewLine;
		text = text + "Anyland version: " + Universe.GetClientVersionDisplay() + Environment.NewLine;
		text = text + "OS: " + SystemInfo.operatingSystem + Environment.NewLine;
		text += text2;
		GUIUtility.systemCopyBuffer = text;
		Managers.soundManager.Play("pickUp", this.transform, 0.4f, false, false);
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0005647B File Offset: 0x0005487B
	public override void RecreateInterfaceAfterSettingsChangeIfNeeded()
	{
		this.AddMuteButton();
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x00056484 File Offset: 0x00054884
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "setToThisMicrophone":
			this.microphone.ChangeMicrophoneDevice(contextId);
			this.AddMicrophoneButtons();
			break;
		case "muted":
			if (Managers.personManager.ourPerson.isSoftBanned)
			{
				Managers.soundManager.Play("no", this.transform, 1f, false, false);
				Managers.dialogManager.SwitchToNewDialog(DialogType.Softban, null, string.Empty);
			}
			else
			{
				Managers.settingManager.SetState(Setting.Microphone, !state, false);
				base.SwitchTo(DialogType.Microphone, string.Empty);
			}
			break;
		case "previousPage":
		{
			int num = --this.page;
			if (num < 0)
			{
				this.page = this.maxPages - 1;
			}
			this.AddMicrophoneButtons();
			break;
		}
		case "nextPage":
		{
			int num = ++this.page;
			if (num > this.maxPages - 1)
			{
				this.page = 0;
			}
			this.AddMicrophoneButtons();
			break;
		}
		case "copyInfo":
			this.CopyInfoToClipboard();
			break;
		case "back":
			Universe.hearEchoOfMyVoice = false;
			base.SwitchTo(DialogType.OwnProfile, string.Empty);
			break;
		case "close":
			this.CleanUpIfNeeded();
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x04000841 RID: 2113
	private MicrophoneInputDevice microphone;

	// Token: 0x04000842 RID: 2114
	private bool didCleanUp;

	// Token: 0x04000843 RID: 2115
	private int microphonesFound = -1;

	// Token: 0x04000844 RID: 2116
	private const int maxPerPage = 3;

	// Token: 0x04000845 RID: 2117
	private int page;

	// Token: 0x04000846 RID: 2118
	private int maxPages = -1;

	// Token: 0x04000847 RID: 2119
	private bool doManyDevicesTest;

	// Token: 0x04000848 RID: 2120
	private GameObject muteButton;
}
