using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public interface IPunPrefabPool
{
	// Token: 0x0600041B RID: 1051
	GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation);

	// Token: 0x0600041C RID: 1052
	void Destroy(GameObject gameObject);
}
