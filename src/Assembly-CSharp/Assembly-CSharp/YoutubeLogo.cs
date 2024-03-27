using System;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class YoutubeLogo : MonoBehaviour
{
	// Token: 0x06001A76 RID: 6774 RVA: 0x000EE03E File Offset: 0x000EC43E
	private void OnMouseDown()
	{
		Application.OpenURL(this.youtubeurl);
	}

	// Token: 0x04001867 RID: 6247
	public string youtubeurl;
}
