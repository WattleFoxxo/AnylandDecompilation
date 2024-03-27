using System;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class SupportLogger : MonoBehaviour
{
	// Token: 0x060006C2 RID: 1730 RVA: 0x0001F834 File Offset: 0x0001DC34
	public void Start()
	{
		GameObject gameObject = GameObject.Find("PunSupportLogger");
		if (gameObject == null)
		{
			gameObject = new GameObject("PunSupportLogger");
			global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
			SupportLogging supportLogging = gameObject.AddComponent<SupportLogging>();
			supportLogging.LogTrafficStats = this.LogTrafficStats;
		}
	}

	// Token: 0x040004EE RID: 1262
	public bool LogTrafficStats = true;
}
