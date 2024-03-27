using System;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public class LoadVideoTitleToText : MonoBehaviour
{
	// Token: 0x06001A6D RID: 6765 RVA: 0x000EDF02 File Offset: 0x000EC302
	public void SetVideoTitle()
	{
		this.textMesh.text = this.player.GetVideoTitle();
	}

	// Token: 0x0400185D RID: 6237
	public TextMesh textMesh;

	// Token: 0x0400185E RID: 6238
	public YoutubePlayer player;
}
