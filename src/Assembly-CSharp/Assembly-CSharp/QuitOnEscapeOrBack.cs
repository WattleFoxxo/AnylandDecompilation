using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class QuitOnEscapeOrBack : MonoBehaviour
{
	// Token: 0x060006B4 RID: 1716 RVA: 0x0001F2F7 File Offset: 0x0001D6F7
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
