using System;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class ErrorDisplay2d : MonoBehaviour
{
	// Token: 0x06000D07 RID: 3335 RVA: 0x000758C0 File Offset: 0x00073CC0
	private void OnGUI()
	{
		this.style = new GUIStyle(GUI.skin.label);
		this.style.padding = new RectOffset(20, 20, 20, 20);
		this.style.fontSize = 22;
		this.style.fontStyle = FontStyle.Bold;
		int num = 2;
		int num2 = 2;
		this.AddFontShadow(num, num2, new Color32(69, 3, 118, 28), 2);
		GUI.contentColor = new Color32(235, 205, byte.MaxValue, byte.MaxValue);
		Rect rect = new Rect((float)num, (float)num2, (float)Screen.width, (float)Screen.height);
		GUI.Label(rect, this.messageHandler.message, this.style);
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x00075984 File Offset: 0x00073D84
	private void AddFontShadow(int centerX, int centerY, Color color, int shadowSize = 2)
	{
		GUI.contentColor = color;
		for (int i = -shadowSize; i <= shadowSize; i++)
		{
			for (int j = -shadowSize; j <= shadowSize; j++)
			{
				Rect rect = new Rect((float)(centerX + i), (float)(centerY + j), (float)Screen.width, (float)Screen.height);
				GUI.Label(rect, this.messageHandler.message, this.style);
			}
		}
	}

	// Token: 0x04000EB9 RID: 3769
	public ErrorMessageHandler messageHandler;

	// Token: 0x04000EBA RID: 3770
	private GUIStyle style;
}
