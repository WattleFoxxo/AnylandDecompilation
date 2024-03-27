using System;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x020002C0 RID: 704
public class ReactingLights : MonoBehaviour
{
	// Token: 0x06001A63 RID: 6755 RVA: 0x000ED9B1 File Offset: 0x000EBDB1
	private void Start()
	{
		this.videoSource.frameReady += this.NewFrame;
		this.videoSource.sendFrameReadyEvents = true;
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x000ED9D8 File Offset: 0x000EBDD8
	private void NewFrame(VideoPlayer vplayer, long frame)
	{
		if (!this.createTexture)
		{
			this.createTexture = true;
			switch (this.videoSide)
			{
			case ReactingLights.VideoSide.up:
				this.tex = new Texture2D(this.videoSource.texture.width / 2, 20);
				break;
			case ReactingLights.VideoSide.left:
				this.tex = new Texture2D(20, this.videoSource.texture.height / 2);
				break;
			case ReactingLights.VideoSide.right:
				this.tex = new Texture2D(20, this.videoSource.texture.height / 2);
				break;
			case ReactingLights.VideoSide.down:
				this.tex = new Texture2D(this.videoSource.texture.width / 2, 20);
				break;
			case ReactingLights.VideoSide.center:
				this.tex = new Texture2D(this.videoSource.texture.height / 2, this.videoSource.texture.height / 2);
				break;
			}
		}
		RenderTexture.active = (RenderTexture)this.videoSource.texture;
		switch (this.videoSide)
		{
		case ReactingLights.VideoSide.up:
			this.tex.ReadPixels(new Rect((float)(this.videoSource.texture.width / 2), 0f, (float)(this.videoSource.texture.width / 2), 20f), 0, 0);
			break;
		case ReactingLights.VideoSide.left:
			this.tex.ReadPixels(new Rect(0f, 0f, 20f, (float)(this.videoSource.texture.height / 2)), 0, 0);
			break;
		case ReactingLights.VideoSide.right:
			this.tex.ReadPixels(new Rect((float)(this.videoSource.texture.width - 20), 0f, 20f, (float)(this.videoSource.texture.height / 2)), 0, 0);
			break;
		case ReactingLights.VideoSide.down:
			this.tex.ReadPixels(new Rect((float)(this.videoSource.texture.width / 2), (float)(this.videoSource.texture.height - 20), (float)(this.videoSource.texture.width / 2), 20f), 0, 0);
			break;
		case ReactingLights.VideoSide.center:
			this.tex.ReadPixels(new Rect((float)(this.videoSource.texture.width / 2 - this.videoSource.texture.width / 2), (float)(this.videoSource.texture.height / 2 - this.videoSource.texture.height / 2), (float)(this.videoSource.texture.width / 2), (float)(this.videoSource.texture.height / 2)), 0, 0);
			break;
		}
		this.tex.Apply();
		this.averageColor = this.AverageColorFromTexture(this.tex);
		this.averageColor.a = 1f;
		foreach (Light light in this.lights)
		{
			light.color = this.averageColor;
		}
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000EDD28 File Offset: 0x000EC128
	private Color32 AverageColorFromTexture(Texture2D tex)
	{
		Color32[] pixels = tex.GetPixels32();
		int num = pixels.Length;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		for (int i = 0; i < num; i++)
		{
			num2 += (float)pixels[i].r;
			num3 += (float)pixels[i].g;
			num4 += (float)pixels[i].b;
		}
		return new Color32((byte)(num2 / (float)num), (byte)(num3 / (float)num), (byte)(num4 / (float)num), 0);
	}

	// Token: 0x0400184D RID: 6221
	public VideoPlayer videoSource;

	// Token: 0x0400184E RID: 6222
	public Light[] lights;

	// Token: 0x0400184F RID: 6223
	public Color averageColor;

	// Token: 0x04001850 RID: 6224
	private Texture2D tex;

	// Token: 0x04001851 RID: 6225
	public ReactingLights.VideoSide videoSide;

	// Token: 0x04001852 RID: 6226
	private bool createTexture;

	// Token: 0x020002C1 RID: 705
	public enum VideoSide
	{
		// Token: 0x04001854 RID: 6228
		up,
		// Token: 0x04001855 RID: 6229
		left,
		// Token: 0x04001856 RID: 6230
		right,
		// Token: 0x04001857 RID: 6231
		down,
		// Token: 0x04001858 RID: 6232
		center
	}
}
