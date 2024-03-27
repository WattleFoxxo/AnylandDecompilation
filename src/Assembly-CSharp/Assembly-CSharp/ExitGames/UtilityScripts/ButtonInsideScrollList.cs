using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x020000D7 RID: 215
	public class ButtonInsideScrollList : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x060006D9 RID: 1753 RVA: 0x0001FB53 File Offset: 0x0001DF53
		private void Start()
		{
			this.scrollRect = base.GetComponentInParent<ScrollRect>();
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001FB61 File Offset: 0x0001DF61
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.scrollRect != null)
			{
				this.scrollRect.StopMovement();
				this.scrollRect.enabled = false;
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001FB8B File Offset: 0x0001DF8B
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.scrollRect != null && !this.scrollRect.enabled)
			{
				this.scrollRect.enabled = true;
			}
		}

		// Token: 0x040004F4 RID: 1268
		private ScrollRect scrollRect;
	}
}
