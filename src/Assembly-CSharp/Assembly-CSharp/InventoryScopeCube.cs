using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class InventoryScopeCube : MonoBehaviour
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060007BF RID: 1983 RVA: 0x0002AFF0 File Offset: 0x000293F0
	// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0002AFF8 File Offset: 0x000293F8
	public bool isInside { get; private set; }

	// Token: 0x060007C1 RID: 1985 RVA: 0x0002B001 File Offset: 0x00029401
	private void OnTriggerEnter(Collider other)
	{
		if (this.IsRelevantCollider(other))
		{
			this.isInside = true;
			this.ScaleHeldThingIfNeeded(other.gameObject);
		}
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0002B022 File Offset: 0x00029422
	private void OnTriggerStay(Collider other)
	{
		if (this.IsRelevantCollider(other))
		{
			this.isInside = true;
		}
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0002B037 File Offset: 0x00029437
	private void OnTriggerExit(Collider other)
	{
		if (this.IsRelevantCollider(other))
		{
			this.isInside = false;
			this.ScaleHeldThingIfNeeded(other.gameObject);
		}
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0002B058 File Offset: 0x00029458
	private bool IsRelevantCollider(Collider other)
	{
		bool flag = false;
		if (other.name == "HandDot")
		{
			flag = true;
			HandDot component = other.GetComponent<HandDot>();
			if (component != null && component.holdableInHand != null && component.holdableInHand.name == DialogType.Inventory.ToString())
			{
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x0002B0CC File Offset: 0x000294CC
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
					gameObject.transform.localScale = ((!component2.keepSizeInInventory) ? Managers.thingManager.GetAppropriateDownScaleForThing(gameObject, 0.1f, false) : Vector3.one);
				}
				else
				{
					gameObject.transform.localScale = Vector3.one;
					if (InventoryScopeCube.resetPositionWhenExitingScope)
					{
						gameObject.transform.localPosition = Vector3.zero;
					}
				}
			}
		}
	}

	// Token: 0x040005B9 RID: 1465
	public static bool resetPositionWhenExitingScope = true;
}
