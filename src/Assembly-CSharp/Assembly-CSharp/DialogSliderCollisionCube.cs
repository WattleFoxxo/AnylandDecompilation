using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class DialogSliderCollisionCube : MonoBehaviour
{
	// Token: 0x060008D0 RID: 2256 RVA: 0x00033946 File Offset: 0x00031D46
	private void Start()
	{
		this.side = ((!(base.gameObject.name == "LeftCollisionCube")) ? Side.Right : Side.Left);
		this.mover = base.transform.parent.GetComponent<DialogSliderMover>();
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x00033988 File Offset: 0x00031D88
	private void OnTriggerStay(Collider other)
	{
		HandDot handDot = other.gameObject.GetComponent<HandDot>();
		if (handDot == null)
		{
			VertexMover component = other.gameObject.GetComponent<VertexMover>();
			if (component != null)
			{
				handDot = component.handDot;
			}
		}
		if (handDot != null)
		{
			this.PushInDirection(handDot);
		}
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x000339DF File Offset: 0x00031DDF
	public void PushInDirection(HandDot handDot)
	{
		this.mover.PushInDirection((this.side != Side.Left) ? (-1f) : 1f, handDot);
	}

	// Token: 0x0400068C RID: 1676
	private Side side = Side.Left;

	// Token: 0x0400068D RID: 1677
	private DialogSliderMover mover;
}
