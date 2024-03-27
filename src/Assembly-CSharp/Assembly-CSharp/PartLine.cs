using System;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class PartLine : MonoBehaviour
{
	// Token: 0x0600167C RID: 5756 RVA: 0x000CA588 File Offset: 0x000C8988
	private void Start()
	{
		this.partRenderer = base.gameObject.GetComponent<Renderer>();
		this.line = base.gameObject.GetComponent<LineRenderer>();
		if (this.line == null)
		{
			this.line = base.gameObject.AddComponent<LineRenderer>();
		}
		this.line.material = new Material(Shader.Find("Custom/SeeThroughLine"));
		this.UpdateAppearance();
	}

	// Token: 0x0600167D RID: 5757 RVA: 0x000CA5F9 File Offset: 0x000C89F9
	private void Update()
	{
		this.UpdateAppearance();
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x000CA604 File Offset: 0x000C8A04
	private void UpdateAppearance()
	{
		Transform parent = base.transform.parent;
		if (parent != null)
		{
			this.line.SetWidth(this.settings.startWidth, this.settings.endWidth);
			this.line.SetPosition(0, parent.position);
			this.line.SetPosition(1, base.transform.position);
			if (this.partRenderer != null && this.partRenderer.material != null)
			{
				this.line.material = this.partRenderer.material;
			}
		}
	}

	// Token: 0x0600167F RID: 5759 RVA: 0x000CA6B0 File Offset: 0x000C8AB0
	private void OnDestroy()
	{
		if (this.line != null)
		{
			global::UnityEngine.Object.Destroy(this.line);
		}
	}

	// Token: 0x04001432 RID: 5170
	public PartLineSettings settings = new PartLineSettings();

	// Token: 0x04001433 RID: 5171
	private LineRenderer line;

	// Token: 0x04001434 RID: 5172
	private Renderer partRenderer;
}
