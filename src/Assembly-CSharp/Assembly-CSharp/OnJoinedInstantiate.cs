using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class OnJoinedInstantiate : MonoBehaviour
{
	// Token: 0x06000659 RID: 1625 RVA: 0x0001DD50 File Offset: 0x0001C150
	public void OnJoinedRoom()
	{
		if (this.PrefabsToInstantiate != null)
		{
			foreach (GameObject gameObject in this.PrefabsToInstantiate)
			{
				Debug.Log("Instantiating: " + gameObject.name);
				Vector3 vector = Vector3.up;
				if (this.SpawnPosition != null)
				{
					vector = this.SpawnPosition.position;
				}
				Vector3 vector2 = global::UnityEngine.Random.insideUnitSphere;
				vector2.y = 0f;
				vector2 = vector2.normalized;
				Vector3 vector3 = vector + this.PositionOffset * vector2;
				PhotonNetwork.Instantiate(gameObject.name, vector3, Quaternion.identity, 0);
			}
		}
	}

	// Token: 0x040004BD RID: 1213
	public Transform SpawnPosition;

	// Token: 0x040004BE RID: 1214
	public float PositionOffset = 2f;

	// Token: 0x040004BF RID: 1215
	public GameObject[] PrefabsToInstantiate;
}
