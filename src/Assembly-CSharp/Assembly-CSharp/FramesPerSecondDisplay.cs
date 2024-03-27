using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class FramesPerSecondDisplay : MonoBehaviour
{
	// Token: 0x06000D5C RID: 3420 RVA: 0x00077910 File Offset: 0x00075D10
	private void Start()
	{
		base.transform.localPosition = new Vector3(0f, 0f, 0.5f);
		base.transform.localRotation = Quaternion.identity;
		base.gameObject.name = Misc.RemoveCloneFromName(base.gameObject.name);
		this.textMesh = base.GetComponent<TextMesh>();
		this.colorOk = new Color32(73, byte.MaxValue, 92, byte.MaxValue);
		this.colorWarning = new Color32(244, 73, 73, byte.MaxValue);
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x000779B0 File Offset: 0x00075DB0
	private void Update()
	{
		this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
		float num = this.deltaTime * 1000f;
		float num2 = 1f / this.deltaTime;
		string text = "{1:0.} FPS" + Environment.NewLine + "{0:0.0} MS";
		this.textMesh.text = string.Format(text, num, num2);
		this.textMesh.color = ((num2 < 88f) ? this.colorWarning : this.colorOk);
	}

	// Token: 0x04000EF0 RID: 3824
	private TextMesh textMesh;

	// Token: 0x04000EF1 RID: 3825
	private float deltaTime;

	// Token: 0x04000EF2 RID: 3826
	private Color colorOk;

	// Token: 0x04000EF3 RID: 3827
	private Color colorWarning;
}
