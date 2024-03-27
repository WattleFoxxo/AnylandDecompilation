using System;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class ShowStatusWhenConnecting : MonoBehaviour
{
	// Token: 0x060006BB RID: 1723 RVA: 0x0001F57C File Offset: 0x0001D97C
	private void OnGUI()
	{
		if (this.Skin != null)
		{
			GUI.skin = this.Skin;
		}
		float num = 400f;
		float num2 = 100f;
		Rect rect = new Rect(((float)Screen.width - num) / 2f, ((float)Screen.height - num2) / 2f, num, num2);
		GUILayout.BeginArea(rect, GUI.skin.box);
		GUILayout.Label("Connecting" + this.GetConnectingDots(), GUI.skin.customStyles[0], new GUILayoutOption[0]);
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed, new GUILayoutOption[0]);
		GUILayout.EndArea();
		if (PhotonNetwork.inRoom)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0001F644 File Offset: 0x0001DA44
	private string GetConnectingDots()
	{
		string text = string.Empty;
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad * 3f % 4f);
		for (int i = 0; i < num; i++)
		{
			text += " .";
		}
		return text;
	}

	// Token: 0x040004EA RID: 1258
	public GUISkin Skin;
}
