using System;
using UnityEngine;

// Token: 0x0200025C RID: 604
public class DirectionArrows : MonoBehaviour
{
	// Token: 0x0600165C RID: 5724 RVA: 0x000C7504 File Offset: 0x000C5904
	private void Start()
	{
		this.arrows = global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/DirectionArrows", typeof(GameObject))) as GameObject;
		this.arrows.name = "DirectionArrows for " + base.gameObject.name;
		this.arrows.transform.parent = base.transform.parent;
		ThingPart component = base.GetComponent<ThingPart>();
		if (component != null)
		{
			ThingPartBase baseType = component.baseType;
			if (baseType == ThingPartBase.DirectionIndicator)
			{
				this.relativeSize += 5f;
			}
		}
		this.ScaleAndPositionInRelation();
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x000C75B7 File Offset: 0x000C59B7
	private void Update()
	{
		this.ScaleAndPositionInRelation();
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x000C75C0 File Offset: 0x000C59C0
	private void ScaleAndPositionInRelation()
	{
		if (this.arrows != null)
		{
			this.arrows.transform.rotation = base.transform.rotation;
			this.arrows.transform.position = base.transform.position;
			this.arrows.transform.Rotate(90f, 0f, 0f);
			this.arrows.transform.Rotate(0f, -90f, 0f);
			float num = Misc.GetLargestValueOfVector(base.transform.localScale) * this.relativeSize;
			num = Mathf.Clamp(num, 0.1f, 100f);
			this.arrows.transform.localScale = Misc.GetUniformVector3(num);
		}
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x000C7691 File Offset: 0x000C5A91
	private void OnDestroy()
	{
		global::UnityEngine.Object.Destroy(this.arrows);
	}

	// Token: 0x04001428 RID: 5160
	public GameObject arrows;

	// Token: 0x04001429 RID: 5161
	private float relativeSize = 5f;
}
