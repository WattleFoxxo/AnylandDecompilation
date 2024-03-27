using System;
using UnityEngine;
using Valve.VR;

// Token: 0x0200029B RID: 667
public class SteamVR_Fade : MonoBehaviour
{
	// Token: 0x06001933 RID: 6451 RVA: 0x000E4CE8 File Offset: 0x000E30E8
	public static void Start(Color newColor, float duration, bool fadeOverlay = false)
	{
		SteamVR_Utils.Event.Send("fade", new object[] { newColor, duration, fadeOverlay });
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x000E4D18 File Offset: 0x000E3118
	public static void View(Color newColor, float duration)
	{
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			compositor.FadeToColor(duration, newColor.r, newColor.g, newColor.b, newColor.a, false);
		}
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x000E4D58 File Offset: 0x000E3158
	public void OnStartFade(params object[] args)
	{
		Color color = (Color)args[0];
		float num = (float)args[1];
		this.fadeOverlay = args.Length > 2 && (bool)args[2];
		if (num > 0f)
		{
			this.targetColor = color;
			this.deltaColor = (this.targetColor - this.currentColor) / num;
		}
		else
		{
			this.currentColor = color;
		}
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x000E4DCC File Offset: 0x000E31CC
	private void OnEnable()
	{
		if (SteamVR_Fade.fadeMaterial == null)
		{
			SteamVR_Fade.fadeMaterial = new Material(Shader.Find("Custom/SteamVR_Fade"));
			SteamVR_Fade.fadeMaterialColorID = Shader.PropertyToID("fadeColor");
		}
		SteamVR_Utils.Event.Listen("fade", new SteamVR_Utils.Event.Handler(this.OnStartFade));
		SteamVR_Utils.Event.Send("fade_ready", new object[0]);
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x000E4E32 File Offset: 0x000E3232
	private void OnDisable()
	{
		SteamVR_Utils.Event.Remove("fade", new SteamVR_Utils.Event.Handler(this.OnStartFade));
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x000E4E4C File Offset: 0x000E324C
	private void OnPostRender()
	{
		if (this.currentColor != this.targetColor)
		{
			if (Mathf.Abs(this.currentColor.a - this.targetColor.a) < Mathf.Abs(this.deltaColor.a) * Time.deltaTime)
			{
				this.currentColor = this.targetColor;
				this.deltaColor = new Color(0f, 0f, 0f, 0f);
				global::UnityEngine.Object.Destroy(this);
			}
			else
			{
				this.currentColor += this.deltaColor * Time.deltaTime;
			}
			if (this.fadeOverlay)
			{
				SteamVR_Overlay instance = SteamVR_Overlay.instance;
				if (instance != null)
				{
					instance.alpha = 1f - this.currentColor.a;
				}
			}
		}
		if (this.currentColor.a > 0f && SteamVR_Fade.fadeMaterial)
		{
			Color color = this.currentColor;
			color.a = this.EaseInOut(color.a);
			SteamVR_Fade.fadeMaterial.SetColor(SteamVR_Fade.fadeMaterialColorID, color);
			SteamVR_Fade.fadeMaterial.SetPass(0);
			GL.Begin(7);
			GL.Vertex3(-1f, -1f, 0f);
			GL.Vertex3(1f, -1f, 0f);
			GL.Vertex3(1f, 1f, 0f);
			GL.Vertex3(-1f, 1f, 0f);
			GL.End();
		}
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x000E4FE8 File Offset: 0x000E33E8
	private float EaseInOut(float v)
	{
		float num = Mathf.Pow(v, 2f);
		return num / (2f * (num - v) + 1f);
	}

	// Token: 0x0400175E RID: 5982
	private Color currentColor = new Color(0f, 0f, 0f, 1f);

	// Token: 0x0400175F RID: 5983
	private Color targetColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04001760 RID: 5984
	private Color deltaColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04001761 RID: 5985
	private bool fadeOverlay;

	// Token: 0x04001762 RID: 5986
	private static Material fadeMaterial;

	// Token: 0x04001763 RID: 5987
	private static int fadeMaterialColorID = -1;
}
