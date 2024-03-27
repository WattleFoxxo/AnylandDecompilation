using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002C4 RID: 708
public class PauseIcon : MonoBehaviour
{
	// Token: 0x06001A6F RID: 6767 RVA: 0x000EDF24 File Offset: 0x000EC324
	private void FixedUpdate()
	{
		if (this.p.pauseCalled)
		{
			this.playImage.gameObject.SetActive(true);
			this.pauseImage.gameObject.SetActive(false);
		}
		else
		{
			this.pauseImage.gameObject.SetActive(true);
			this.playImage.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400185F RID: 6239
	public YoutubePlayer p;

	// Token: 0x04001860 RID: 6240
	public Image playImage;

	// Token: 0x04001861 RID: 6241
	public Image pauseImage;
}
