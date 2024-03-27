using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x020000D8 RID: 216
	[RequireComponent(typeof(Text))]
	public class TextButtonTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x060006DD RID: 1757 RVA: 0x0001FBD8 File Offset: 0x0001DFD8
		public void Awake()
		{
			this._text = base.GetComponent<Text>();
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001FBE6 File Offset: 0x0001DFE6
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._text.color = this.HoverColor;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001FBF9 File Offset: 0x0001DFF9
		public void OnPointerExit(PointerEventData eventData)
		{
			this._text.color = this.NormalColor;
		}

		// Token: 0x040004F5 RID: 1269
		private Text _text;

		// Token: 0x040004F6 RID: 1270
		public Color NormalColor = Color.white;

		// Token: 0x040004F7 RID: 1271
		public Color HoverColor = Color.black;
	}
}
