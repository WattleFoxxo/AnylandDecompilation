using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x020000D9 RID: 217
	[RequireComponent(typeof(Text))]
	public class TextToggleIsOnTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x060006E1 RID: 1761 RVA: 0x0001FC40 File Offset: 0x0001E040
		public void OnEnable()
		{
			this._text = base.GetComponent<Text>();
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001FC6A File Offset: 0x0001E06A
		public void OnDisable()
		{
			this.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001FC88 File Offset: 0x0001E088
		public void OnValueChanged(bool isOn)
		{
			this._text.color = ((!isOn) ? ((!this.isHover) ? this.NormalOffColor : this.NormalOnColor) : ((!this.isHover) ? this.HoverOffColor : this.HoverOnColor));
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001FCE3 File Offset: 0x0001E0E3
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.isHover = true;
			this._text.color = ((!this.toggle.isOn) ? this.HoverOffColor : this.HoverOnColor);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001FD18 File Offset: 0x0001E118
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isHover = false;
			this._text.color = ((!this.toggle.isOn) ? this.NormalOffColor : this.NormalOnColor);
		}

		// Token: 0x040004F8 RID: 1272
		public Toggle toggle;

		// Token: 0x040004F9 RID: 1273
		private Text _text;

		// Token: 0x040004FA RID: 1274
		public Color NormalOnColor = Color.white;

		// Token: 0x040004FB RID: 1275
		public Color NormalOffColor = Color.black;

		// Token: 0x040004FC RID: 1276
		public Color HoverOnColor = Color.black;

		// Token: 0x040004FD RID: 1277
		public Color HoverOffColor = Color.black;

		// Token: 0x040004FE RID: 1278
		private bool isHover;
	}
}
