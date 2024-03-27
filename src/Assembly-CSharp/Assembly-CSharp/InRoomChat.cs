using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x020000B5 RID: 181
[RequireComponent(typeof(PhotonView))]
public class InRoomChat : global::Photon.MonoBehaviour
{
	// Token: 0x0600062B RID: 1579 RVA: 0x0001CC02 File Offset: 0x0001B002
	public void Start()
	{
		if (this.AlignBottom)
		{
			this.GuiRect.y = (float)Screen.height - this.GuiRect.height;
		}
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0001CC2C File Offset: 0x0001B02C
	public void OnGUI()
	{
		if (!this.IsVisible || !PhotonNetwork.inRoom)
		{
			return;
		}
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
		{
			if (!string.IsNullOrEmpty(this.inputLine))
			{
				base.photonView.RPC("Chat", PhotonTargets.All, new object[] { this.inputLine });
				this.inputLine = string.Empty;
				GUI.FocusControl(string.Empty);
				return;
			}
			GUI.FocusControl("ChatInput");
		}
		GUI.SetNextControlName(string.Empty);
		GUILayout.BeginArea(this.GuiRect);
		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			GUILayout.Label(this.messages[i], new GUILayoutOption[0]);
		}
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUI.SetNextControlName("ChatInput");
		this.inputLine = GUILayout.TextField(this.inputLine, new GUILayoutOption[0]);
		if (GUILayout.Button("Send", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }))
		{
			base.photonView.RPC("Chat", PhotonTargets.All, new object[] { this.inputLine });
			this.inputLine = string.Empty;
			GUI.FocusControl(string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0001CDC8 File Offset: 0x0001B1C8
	[PunRPC]
	public void Chat(string newLine, PhotonMessageInfo mi)
	{
		string text = "anonymous";
		if (mi.sender != null)
		{
			if (!string.IsNullOrEmpty(mi.sender.NickName))
			{
				text = mi.sender.NickName;
			}
			else
			{
				text = "player " + mi.sender.ID;
			}
		}
		this.messages.Add(text + ": " + newLine);
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0001CE42 File Offset: 0x0001B242
	public void AddLine(string newLine)
	{
		this.messages.Add(newLine);
	}

	// Token: 0x0400049B RID: 1179
	public Rect GuiRect = new Rect(0f, 0f, 250f, 300f);

	// Token: 0x0400049C RID: 1180
	public bool IsVisible = true;

	// Token: 0x0400049D RID: 1181
	public bool AlignBottom;

	// Token: 0x0400049E RID: 1182
	public List<string> messages = new List<string>();

	// Token: 0x0400049F RID: 1183
	private string inputLine = string.Empty;

	// Token: 0x040004A0 RID: 1184
	private Vector2 scrollPos = Vector2.zero;

	// Token: 0x040004A1 RID: 1185
	public static readonly string ChatRPC = "Chat";
}
