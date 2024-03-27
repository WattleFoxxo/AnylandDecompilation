using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
	// Token: 0x06000689 RID: 1673 RVA: 0x0001EAD4 File Offset: 0x0001CED4
	private void OnGUI()
	{
		if (InputToEvent.goPointedAt != null)
		{
			PhotonView photonView = InputToEvent.goPointedAt.GetPhotonView();
			if (photonView != null)
			{
				GUI.Label(new Rect(Input.mousePosition.x + 5f, (float)Screen.height - Input.mousePosition.y - 15f, 300f, 30f), string.Format("ViewID {0} {1}{2}", photonView.viewID, (!photonView.isSceneView) ? string.Empty : "scene ", (!photonView.isMine) ? ("owner: " + photonView.ownerId) : "mine"));
			}
		}
	}
}
