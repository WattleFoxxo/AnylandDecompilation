using System;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class ErrorDisplay3d : MonoBehaviour
{
	// Token: 0x06000D0A RID: 3338 RVA: 0x000759FC File Offset: 0x00073DFC
	private void Start()
	{
		TextMesh component = base.GetComponent<TextMesh>();
		component.text = this.messageHandler.message.ToUpper();
	}

	// Token: 0x04000EBB RID: 3771
	public ErrorMessageHandler messageHandler;
}
