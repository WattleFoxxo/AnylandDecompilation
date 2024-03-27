using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class PhotonStatsGui : MonoBehaviour
{
	// Token: 0x06000535 RID: 1333 RVA: 0x00017B34 File Offset: 0x00015F34
	public void Start()
	{
		if (this.statsRect.x <= 0f)
		{
			this.statsRect.x = (float)Screen.width - this.statsRect.width;
		}
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00017B68 File Offset: 0x00015F68
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
		{
			this.statsWindowOn = !this.statsWindowOn;
			this.statsOn = true;
		}
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00017B9C File Offset: 0x00015F9C
	public void OnGUI()
	{
		if (PhotonNetwork.networkingPeer.TrafficStatsEnabled != this.statsOn)
		{
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = this.statsOn;
		}
		if (!this.statsWindowOn)
		{
			return;
		}
		this.statsRect = GUILayout.Window(this.WindowId, this.statsRect, new GUI.WindowFunction(this.TrafficStatsWindow), "Messages (shift+tab)", new GUILayoutOption[0]);
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00017C08 File Offset: 0x00016008
	public void TrafficStatsWindow(int windowID)
	{
		bool flag = false;
		TrafficStatsGameLevel trafficStatsGameLevel = PhotonNetwork.networkingPeer.TrafficStatsGameLevel;
		long num = PhotonNetwork.networkingPeer.TrafficStatsElapsedMs / 1000L;
		if (num == 0L)
		{
			num = 1L;
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.buttonsOn = GUILayout.Toggle(this.buttonsOn, "buttons", new GUILayoutOption[0]);
		this.healthStatsVisible = GUILayout.Toggle(this.healthStatsVisible, "health", new GUILayoutOption[0]);
		this.trafficStatsOn = GUILayout.Toggle(this.trafficStatsOn, "traffic", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		string text = string.Format("Out {0,4} | In {1,4} | Sum {2,4}", trafficStatsGameLevel.TotalOutgoingMessageCount, trafficStatsGameLevel.TotalIncomingMessageCount, trafficStatsGameLevel.TotalMessageCount);
		string text2 = string.Format("{0}sec average:", num);
		string text3 = string.Format("Out {0,4} | In {1,4} | Sum {2,4}", (long)trafficStatsGameLevel.TotalOutgoingMessageCount / num, (long)trafficStatsGameLevel.TotalIncomingMessageCount / num, (long)trafficStatsGameLevel.TotalMessageCount / num);
		GUILayout.Label(text, new GUILayoutOption[0]);
		GUILayout.Label(text2, new GUILayoutOption[0]);
		GUILayout.Label(text3, new GUILayoutOption[0]);
		if (this.buttonsOn)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.statsOn = GUILayout.Toggle(this.statsOn, "stats on", new GUILayoutOption[0]);
			if (GUILayout.Button("Reset", new GUILayoutOption[0]))
			{
				PhotonNetwork.networkingPeer.TrafficStatsReset();
				PhotonNetwork.networkingPeer.TrafficStatsEnabled = true;
			}
			flag = GUILayout.Button("To Log", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		string text4 = string.Empty;
		string text5 = string.Empty;
		if (this.trafficStatsOn)
		{
			GUILayout.Box("Traffic Stats", new GUILayoutOption[0]);
			text4 = "Incoming: \n" + PhotonNetwork.networkingPeer.TrafficStatsIncoming.ToString();
			text5 = "Outgoing: \n" + PhotonNetwork.networkingPeer.TrafficStatsOutgoing.ToString();
			GUILayout.Label(text4, new GUILayoutOption[0]);
			GUILayout.Label(text5, new GUILayoutOption[0]);
		}
		string text6 = string.Empty;
		if (this.healthStatsVisible)
		{
			GUILayout.Box("Health Stats", new GUILayoutOption[0]);
			text6 = string.Format("ping: {6}[+/-{7}]ms resent:{8} \n\nmax ms between\nsend: {0,4} \ndispatch: {1,4} \n\nlongest dispatch for: \nev({3}):{2,3}ms \nop({5}):{4,3}ms", new object[]
			{
				trafficStatsGameLevel.LongestDeltaBetweenSending,
				trafficStatsGameLevel.LongestDeltaBetweenDispatching,
				trafficStatsGameLevel.LongestEventCallback,
				trafficStatsGameLevel.LongestEventCallbackCode,
				trafficStatsGameLevel.LongestOpResponseCallback,
				trafficStatsGameLevel.LongestOpResponseCallbackOpCode,
				PhotonNetwork.networkingPeer.RoundTripTime,
				PhotonNetwork.networkingPeer.RoundTripTimeVariance,
				PhotonNetwork.networkingPeer.ResentReliableCommands
			});
			GUILayout.Label(text6, new GUILayoutOption[0]);
		}
		if (flag)
		{
			string text7 = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", new object[] { text, text2, text3, text4, text5, text6 });
			Debug.Log(text7);
		}
		if (GUI.changed)
		{
			this.statsRect.height = 100f;
		}
		GUI.DragWindow();
	}

	// Token: 0x040003A8 RID: 936
	public bool statsWindowOn = true;

	// Token: 0x040003A9 RID: 937
	public bool statsOn = true;

	// Token: 0x040003AA RID: 938
	public bool healthStatsVisible;

	// Token: 0x040003AB RID: 939
	public bool trafficStatsOn;

	// Token: 0x040003AC RID: 940
	public bool buttonsOn;

	// Token: 0x040003AD RID: 941
	public Rect statsRect = new Rect(0f, 100f, 200f, 50f);

	// Token: 0x040003AE RID: 942
	public int WindowId = 100;
}
