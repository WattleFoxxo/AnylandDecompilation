using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class PhotonLagSimulationGui : MonoBehaviour
{
	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000475 RID: 1141 RVA: 0x000150A5 File Offset: 0x000134A5
	// (set) Token: 0x06000476 RID: 1142 RVA: 0x000150AD File Offset: 0x000134AD
	public PhotonPeer Peer { get; set; }

	// Token: 0x06000477 RID: 1143 RVA: 0x000150B6 File Offset: 0x000134B6
	public void Start()
	{
		this.Peer = PhotonNetwork.networkingPeer;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x000150C4 File Offset: 0x000134C4
	public void OnGUI()
	{
		if (!this.Visible)
		{
			return;
		}
		if (this.Peer == null)
		{
			this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, new GUI.WindowFunction(this.NetSimHasNoPeerWindow), "Netw. Sim.", new GUILayoutOption[0]);
		}
		else
		{
			this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, new GUI.WindowFunction(this.NetSimWindow), "Netw. Sim.", new GUILayoutOption[0]);
		}
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00015149 File Offset: 0x00013549
	private void NetSimHasNoPeerWindow(int windowId)
	{
		GUILayout.Label("No peer to communicate with. ", new GUILayoutOption[0]);
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0001515C File Offset: 0x0001355C
	private void NetSimWindow(int windowId)
	{
		GUILayout.Label(string.Format("Rtt:{0,4} +/-{1,3}", this.Peer.RoundTripTime, this.Peer.RoundTripTimeVariance), new GUILayoutOption[0]);
		bool isSimulationEnabled = this.Peer.IsSimulationEnabled;
		bool flag = GUILayout.Toggle(isSimulationEnabled, "Simulate", new GUILayoutOption[0]);
		if (flag != isSimulationEnabled)
		{
			this.Peer.IsSimulationEnabled = flag;
		}
		float num = (float)this.Peer.NetworkSimulationSettings.IncomingLag;
		GUILayout.Label("Lag " + num, new GUILayoutOption[0]);
		num = GUILayout.HorizontalSlider(num, 0f, 500f, new GUILayoutOption[0]);
		this.Peer.NetworkSimulationSettings.IncomingLag = (int)num;
		this.Peer.NetworkSimulationSettings.OutgoingLag = (int)num;
		float num2 = (float)this.Peer.NetworkSimulationSettings.IncomingJitter;
		GUILayout.Label("Jit " + num2, new GUILayoutOption[0]);
		num2 = GUILayout.HorizontalSlider(num2, 0f, 100f, new GUILayoutOption[0]);
		this.Peer.NetworkSimulationSettings.IncomingJitter = (int)num2;
		this.Peer.NetworkSimulationSettings.OutgoingJitter = (int)num2;
		float num3 = (float)this.Peer.NetworkSimulationSettings.IncomingLossPercentage;
		GUILayout.Label("Loss " + num3, new GUILayoutOption[0]);
		num3 = GUILayout.HorizontalSlider(num3, 0f, 10f, new GUILayoutOption[0]);
		this.Peer.NetworkSimulationSettings.IncomingLossPercentage = (int)num3;
		this.Peer.NetworkSimulationSettings.OutgoingLossPercentage = (int)num3;
		if (GUI.changed)
		{
			this.WindowRect.height = 100f;
		}
		GUI.DragWindow();
	}

	// Token: 0x0400037B RID: 891
	public Rect WindowRect = new Rect(0f, 100f, 120f, 100f);

	// Token: 0x0400037C RID: 892
	public int WindowId = 101;

	// Token: 0x0400037D RID: 893
	public bool Visible = true;
}
