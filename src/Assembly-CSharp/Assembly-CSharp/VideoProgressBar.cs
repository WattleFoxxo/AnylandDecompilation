using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002C5 RID: 709
public class VideoProgressBar : MonoBehaviour, IDragHandler, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06001A71 RID: 6769 RVA: 0x000EDF92 File Offset: 0x000EC392
	public void OnDrag(PointerEventData eventData)
	{
		this.player.TrySkip(eventData);
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x000EDFA0 File Offset: 0x000EC3A0
	public void OnPointerDown(PointerEventData eventData)
	{
		this.player.TrySkip(eventData);
	}

	// Token: 0x04001862 RID: 6242
	public YoutubePlayer player;
}
