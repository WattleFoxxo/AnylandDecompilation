using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class ServerTime : MonoBehaviour
{
	// Token: 0x060006B6 RID: 1718 RVA: 0x0001F314 File Offset: 0x0001D714
	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 100), 0f, 200f, 30f));
		GUILayout.Label(string.Format("Time Offset: {0}", PhotonNetwork.ServerTimestamp - Environment.TickCount), new GUILayoutOption[0]);
		if (GUILayout.Button("fetch", new GUILayoutOption[0]))
		{
			PhotonNetwork.FetchServerTimestamp();
		}
		GUILayout.EndArea();
	}
}
