using System;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x020002D6 RID: 726
public class YoutubeSimplified : MonoBehaviour
{
	// Token: 0x06001AFE RID: 6910 RVA: 0x000F485F File Offset: 0x000F2C5F
	private void Awake()
	{
		this.videoPlayer = base.GetComponentInChildren<VideoPlayer>();
		this.player = base.GetComponentInChildren<YoutubePlayer>();
		this.player.videoPlayer = this.videoPlayer;
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x000F488A File Offset: 0x000F2C8A
	private void Start()
	{
		this.Play();
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x000F4894 File Offset: 0x000F2C94
	public void Play()
	{
		if (this.fullscreen)
		{
			this.videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
		}
		this.player.autoPlayOnStart = this.autoPlay;
		this.player.videoQuality = YoutubePlayer.YoutubeVideoQuality.STANDARD;
		if (this.autoPlay)
		{
			this.player.Play(this.url);
		}
	}

	// Token: 0x04001902 RID: 6402
	private YoutubePlayer player;

	// Token: 0x04001903 RID: 6403
	public string url;

	// Token: 0x04001904 RID: 6404
	public bool autoPlay = true;

	// Token: 0x04001905 RID: 6405
	public bool fullscreen = true;

	// Token: 0x04001906 RID: 6406
	private VideoPlayer videoPlayer;
}
