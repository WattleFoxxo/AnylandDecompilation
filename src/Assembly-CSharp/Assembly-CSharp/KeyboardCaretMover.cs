using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class KeyboardCaretMover : MonoBehaviour
{
	// Token: 0x06000A56 RID: 2646 RVA: 0x0004C548 File Offset: 0x0004A948
	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "HandDot")
		{
			Hand component = other.transform.parent.GetComponent<Hand>();
			base.transform.parent.GetComponent<KeyboardDialog>().MoveCaretIfPossible(this.direction, other.transform, component);
		}
	}

	// Token: 0x040007BE RID: 1982
	public TextCaretDirection direction;
}
