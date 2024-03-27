using System;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class GiftDropScopeCube : MonoBehaviour
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00077A57 File Offset: 0x00075E57
	// (set) Token: 0x06000D60 RID: 3424 RVA: 0x00077A5F File Offset: 0x00075E5F
	public bool isInside { get; private set; }

	// Token: 0x06000D61 RID: 3425 RVA: 0x00077A68 File Offset: 0x00075E68
	private void OnTriggerEnter(Collider other)
	{
		if (this.IsRelevantCollider(other))
		{
			this.isInside = true;
			this.ScaleHeldThingIfNeeded(other.gameObject);
		}
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x00077A89 File Offset: 0x00075E89
	private void OnTriggerStay(Collider other)
	{
		if (this.IsRelevantCollider(other))
		{
			this.isInside = true;
		}
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x00077A9E File Offset: 0x00075E9E
	private void OnTriggerExit(Collider other)
	{
		if (this.IsRelevantCollider(other))
		{
			this.isInside = false;
			this.ScaleHeldThingIfNeeded(other.gameObject);
		}
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x00077ABF File Offset: 0x00075EBF
	private bool IsRelevantCollider(Collider other)
	{
		return other.name == "HandDot";
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x00077AD4 File Offset: 0x00075ED4
	private void ScaleHeldThingIfNeeded(GameObject handDotObject)
	{
		HandDot component = handDotObject.GetComponent<HandDot>();
		GameObject gameObject = ((!(component != null)) ? null : component.currentlyHeldObject);
		if (gameObject != null)
		{
			Thing component2 = gameObject.GetComponent<Thing>();
			if (component2 != null)
			{
				if (this.isInside)
				{
					gameObject.transform.localScale = Managers.thingManager.GetAppropriateDownScaleForThing(gameObject, 0.17f, true);
				}
				else
				{
					gameObject.transform.localScale = Vector3.one;
				}
			}
		}
	}
}
