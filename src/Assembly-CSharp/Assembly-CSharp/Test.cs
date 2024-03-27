using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class Test : MonoBehaviour
{
	// Token: 0x0600026F RID: 623 RVA: 0x0000A6B9 File Offset: 0x00008AB9
	public void OnPreRender()
	{
		Shader.EnableKeyword("DISTORT_OFF");
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000A6C5 File Offset: 0x00008AC5
	public void OnPostRender()
	{
		Shader.DisableKeyword("DISTORT_OFF");
	}
}
