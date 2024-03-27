using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

// Token: 0x020002AC RID: 684
public class SteamVR_Stats : MonoBehaviour
{
	// Token: 0x060019AD RID: 6573 RVA: 0x000EA55C File Offset: 0x000E895C
	private void Awake()
	{
		if (this.text == null)
		{
			this.text = base.GetComponent<GUIText>();
			this.text.enabled = false;
		}
		if (this.fadeDuration > 0f)
		{
			SteamVR_Fade.Start(this.fadeColor, 0f, false);
			SteamVR_Fade.Start(Color.clear, this.fadeDuration, false);
		}
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x000EA5C4 File Offset: 0x000E89C4
	private void Update()
	{
		if (this.text != null)
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				this.text.enabled = !this.text.enabled;
			}
			if (this.text.enabled)
			{
				CVRCompositor compositor = OpenVR.Compositor;
				if (compositor != null)
				{
					Compositor_FrameTiming compositor_FrameTiming = default(Compositor_FrameTiming);
					compositor_FrameTiming.m_nSize = (uint)Marshal.SizeOf(typeof(Compositor_FrameTiming));
					compositor.GetFrameTiming(ref compositor_FrameTiming, 0U);
					double flSystemTimeInSeconds = compositor_FrameTiming.m_flSystemTimeInSeconds;
					if (flSystemTimeInSeconds > this.lastUpdate)
					{
						double num = ((this.lastUpdate <= 0.0) ? 0.0 : (1.0 / (flSystemTimeInSeconds - this.lastUpdate)));
						this.lastUpdate = flSystemTimeInSeconds;
						this.text.text = string.Format("framerate: {0:N0}\ndropped frames: {1}", num, (int)compositor_FrameTiming.m_nNumDroppedFrames);
					}
					else
					{
						this.lastUpdate = flSystemTimeInSeconds;
					}
				}
			}
		}
	}

	// Token: 0x040017F3 RID: 6131
	public GUIText text;

	// Token: 0x040017F4 RID: 6132
	public Color fadeColor = Color.black;

	// Token: 0x040017F5 RID: 6133
	public float fadeDuration = 1f;

	// Token: 0x040017F6 RID: 6134
	private double lastUpdate;
}
