using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class OnClickInstantiate : MonoBehaviour
{
	// Token: 0x06000656 RID: 1622 RVA: 0x0001DC34 File Offset: 0x0001C034
	private void OnClick()
	{
		if (!PhotonNetwork.inRoom)
		{
			return;
		}
		int instantiateType = this.InstantiateType;
		if (instantiateType != 0)
		{
			if (instantiateType == 1)
			{
				PhotonNetwork.InstantiateSceneObject(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null);
			}
		}
		else
		{
			PhotonNetwork.Instantiate(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
		}
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0001DCDC File Offset: 0x0001C0DC
	private void OnGUI()
	{
		if (this.showGui)
		{
			GUILayout.BeginArea(new Rect((float)(Screen.width - 180), 0f, 180f, 50f));
			this.InstantiateType = GUILayout.Toolbar(this.InstantiateType, this.InstantiateTypeNames, new GUILayoutOption[0]);
			GUILayout.EndArea();
		}
	}

	// Token: 0x040004B9 RID: 1209
	public GameObject Prefab;

	// Token: 0x040004BA RID: 1210
	public int InstantiateType;

	// Token: 0x040004BB RID: 1211
	private string[] InstantiateTypeNames = new string[] { "Mine", "Scene" };

	// Token: 0x040004BC RID: 1212
	public bool showGui;
}
