using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class DialogPart : MonoBehaviour
{
	// Token: 0x060008B6 RID: 2230 RVA: 0x00032CD4 File Offset: 0x000310D4
	private void Start()
	{
		if (this.isCheckbox)
		{
			this.checkboxActiveMaterial = (Material)Resources.Load("DialogIconMaterials/checkboxActive", typeof(Material));
			this.checkboxInactiveMaterial = (Material)Resources.Load("DialogIconMaterials/checkboxInactive", typeof(Material));
		}
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00032D2C File Offset: 0x0003112C
	public void SetVerticalOffset(float offset)
	{
		this.verticalOffset = offset;
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, offset, base.transform.localPosition.z);
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00032D78 File Offset: 0x00031178
	public bool Press(Collider collider)
	{
		bool flag = false;
		if (!this.stateJustChanged)
		{
			flag = true;
			Transform parent = base.transform.parent;
			this.timeClicked = Time.time;
			this.state = !this.state;
			this.stateJustChanged = true;
			Managers.soundManager.Play("click", base.transform, 0.1f, false, false);
			if (this.doHighlight && Managers.dialogManager != null)
			{
				Managers.dialogManager.ApplyEmissionColor(collider.gameObject, this.state);
			}
			if (this.isCheckbox && !this.isIconCheckbox)
			{
				Transform transform = base.transform.Find("IconQuad");
				if (transform != null)
				{
					Renderer component = transform.GetComponent<Renderer>();
					component.sharedMaterial = ((!this.state) ? this.checkboxInactiveMaterial : this.checkboxActiveMaterial);
				}
			}
		}
		return flag;
	}

	// Token: 0x0400066B RID: 1643
	public string contextName;

	// Token: 0x0400066C RID: 1644
	public string contextId;

	// Token: 0x0400066D RID: 1645
	public bool state;

	// Token: 0x0400066E RID: 1646
	public bool stateJustChanged;

	// Token: 0x0400066F RID: 1647
	public bool isCheckbox;

	// Token: 0x04000670 RID: 1648
	public bool isIconCheckbox;

	// Token: 0x04000671 RID: 1649
	public bool isOnBackside;

	// Token: 0x04000672 RID: 1650
	public bool doHighlight = true;

	// Token: 0x04000673 RID: 1651
	public bool autoStopHighlight = true;

	// Token: 0x04000674 RID: 1652
	public float verticalOffset;

	// Token: 0x04000675 RID: 1653
	public float timeClicked = -1f;

	// Token: 0x04000676 RID: 1654
	private Material checkboxActiveMaterial;

	// Token: 0x04000677 RID: 1655
	private Material checkboxInactiveMaterial;
}
