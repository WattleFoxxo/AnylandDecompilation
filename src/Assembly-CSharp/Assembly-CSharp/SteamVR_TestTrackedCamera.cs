using System;
using UnityEngine;
using Valve.VR;

// Token: 0x0200028B RID: 651
public class SteamVR_TestTrackedCamera : MonoBehaviour
{
	// Token: 0x0600187E RID: 6270 RVA: 0x000E12D4 File Offset: 0x000DF6D4
	private void OnEnable()
	{
		SteamVR_TrackedCamera.VideoStreamTexture videoStreamTexture = SteamVR_TrackedCamera.Source(this.undistorted, 0);
		videoStreamTexture.Acquire();
		if (!videoStreamTexture.hasCamera)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x000E1308 File Offset: 0x000DF708
	private void OnDisable()
	{
		this.material.mainTexture = null;
		SteamVR_TrackedCamera.VideoStreamTexture videoStreamTexture = SteamVR_TrackedCamera.Source(this.undistorted, 0);
		videoStreamTexture.Release();
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x000E1338 File Offset: 0x000DF738
	private void Update()
	{
		SteamVR_TrackedCamera.VideoStreamTexture videoStreamTexture = SteamVR_TrackedCamera.Source(this.undistorted, 0);
		Texture2D texture = videoStreamTexture.texture;
		if (texture == null)
		{
			return;
		}
		this.material.mainTexture = texture;
		float num = (float)texture.width / (float)texture.height;
		if (this.cropped)
		{
			VRTextureBounds_t frameBounds = videoStreamTexture.frameBounds;
			this.material.mainTextureOffset = new Vector2(frameBounds.uMin, frameBounds.vMin);
			float num2 = frameBounds.uMax - frameBounds.uMin;
			float num3 = frameBounds.vMax - frameBounds.vMin;
			this.material.mainTextureScale = new Vector2(num2, num3);
			num *= Mathf.Abs(num2 / num3);
		}
		else
		{
			this.material.mainTextureOffset = Vector2.zero;
			this.material.mainTextureScale = new Vector2(1f, -1f);
		}
		this.target.localScale = new Vector3(1f, 1f / num, 1f);
		if (videoStreamTexture.hasTracking)
		{
			SteamVR_Utils.RigidTransform transform = videoStreamTexture.transform;
			this.target.localPosition = transform.pos;
			this.target.localRotation = transform.rot;
		}
	}

	// Token: 0x040016EA RID: 5866
	public Material material;

	// Token: 0x040016EB RID: 5867
	public Transform target;

	// Token: 0x040016EC RID: 5868
	public bool undistorted = true;

	// Token: 0x040016ED RID: 5869
	public bool cropped = true;
}
