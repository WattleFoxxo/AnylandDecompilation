using System;
using UnityEngine.SceneManagement;

// Token: 0x02000085 RID: 133
public class SceneManagerHelper
{
	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000456 RID: 1110 RVA: 0x00014868 File Offset: 0x00012C68
	public static string ActiveSceneName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000457 RID: 1111 RVA: 0x00014884 File Offset: 0x00012C84
	public static int ActiveSceneBuildIndex
	{
		get
		{
			return SceneManager.GetActiveScene().buildIndex;
		}
	}
}
