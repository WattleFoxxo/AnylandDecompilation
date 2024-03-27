using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

// Token: 0x020002A1 RID: 673
public class SteamVR_Overlay : MonoBehaviour
{
	// Token: 0x170002BB RID: 699
	// (get) Token: 0x0600195E RID: 6494 RVA: 0x000E764E File Offset: 0x000E5A4E
	// (set) Token: 0x0600195F RID: 6495 RVA: 0x000E7655 File Offset: 0x000E5A55
	public static SteamVR_Overlay instance { get; private set; }

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001960 RID: 6496 RVA: 0x000E765D File Offset: 0x000E5A5D
	public static string key
	{
		get
		{
			return "unity:" + Application.companyName + "." + Application.productName;
		}
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x000E7678 File Offset: 0x000E5A78
	private void OnEnable()
	{
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay != null)
		{
			EVROverlayError evroverlayError = overlay.CreateOverlay(SteamVR_Overlay.key, base.gameObject.name, ref this.handle);
			if (evroverlayError != EVROverlayError.None)
			{
				Debug.Log(overlay.GetOverlayErrorNameFromEnum(evroverlayError));
				base.enabled = false;
				return;
			}
		}
		SteamVR_Overlay.instance = this;
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x000E76D0 File Offset: 0x000E5AD0
	private void OnDisable()
	{
		if (this.handle != 0UL)
		{
			CVROverlay overlay = OpenVR.Overlay;
			if (overlay != null)
			{
				overlay.DestroyOverlay(this.handle);
			}
			this.handle = 0UL;
		}
		SteamVR_Overlay.instance = null;
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x000E7714 File Offset: 0x000E5B14
	public void UpdateOverlay()
	{
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay == null)
		{
			return;
		}
		if (this.texture != null)
		{
			EVROverlayError evroverlayError = overlay.ShowOverlay(this.handle);
			if ((evroverlayError == EVROverlayError.InvalidHandle || evroverlayError == EVROverlayError.UnknownOverlay) && overlay.FindOverlay(SteamVR_Overlay.key, ref this.handle) != EVROverlayError.None)
			{
				return;
			}
			Texture_t texture_t = default(Texture_t);
			texture_t.handle = this.texture.GetNativeTexturePtr();
			texture_t.eType = SteamVR.instance.graphicsAPI;
			texture_t.eColorSpace = EColorSpace.Auto;
			overlay.SetOverlayTexture(this.handle, ref texture_t);
			overlay.SetOverlayAlpha(this.handle, this.alpha);
			overlay.SetOverlayWidthInMeters(this.handle, this.scale);
			overlay.SetOverlayAutoCurveDistanceRangeInMeters(this.handle, this.curvedRange.x, this.curvedRange.y);
			VRTextureBounds_t vrtextureBounds_t = default(VRTextureBounds_t);
			vrtextureBounds_t.uMin = this.uvOffset.x * this.uvOffset.z;
			vrtextureBounds_t.vMin = (1f + this.uvOffset.y) * this.uvOffset.w;
			vrtextureBounds_t.uMax = (1f + this.uvOffset.x) * this.uvOffset.z;
			vrtextureBounds_t.vMax = this.uvOffset.y * this.uvOffset.w;
			overlay.SetOverlayTextureBounds(this.handle, ref vrtextureBounds_t);
			HmdVector2_t hmdVector2_t = default(HmdVector2_t);
			hmdVector2_t.v0 = this.mouseScale.x;
			hmdVector2_t.v1 = this.mouseScale.y;
			overlay.SetOverlayMouseScale(this.handle, ref hmdVector2_t);
			SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
			if (steamVR_Camera != null && steamVR_Camera.origin != null)
			{
				SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(steamVR_Camera.origin, base.transform);
				rigidTransform.pos.x = rigidTransform.pos.x / steamVR_Camera.origin.localScale.x;
				rigidTransform.pos.y = rigidTransform.pos.y / steamVR_Camera.origin.localScale.y;
				rigidTransform.pos.z = rigidTransform.pos.z / steamVR_Camera.origin.localScale.z;
				rigidTransform.pos.z = rigidTransform.pos.z + this.distance;
				HmdMatrix34_t hmdMatrix34_t = rigidTransform.ToHmdMatrix34();
				overlay.SetOverlayTransformAbsolute(this.handle, SteamVR_Render.instance.trackingSpace, ref hmdMatrix34_t);
			}
			overlay.SetOverlayInputMethod(this.handle, this.inputMethod);
			if (this.curved || this.antialias)
			{
				this.highquality = true;
			}
			if (this.highquality)
			{
				overlay.SetHighQualityOverlay(this.handle);
				overlay.SetOverlayFlag(this.handle, VROverlayFlags.Curved, this.curved);
				overlay.SetOverlayFlag(this.handle, VROverlayFlags.RGSS4X, this.antialias);
			}
			else if (overlay.GetHighQualityOverlay() == this.handle)
			{
				overlay.SetHighQualityOverlay(0UL);
			}
		}
		else
		{
			overlay.HideOverlay(this.handle);
		}
	}

	// Token: 0x06001964 RID: 6500 RVA: 0x000E7A64 File Offset: 0x000E5E64
	public bool PollNextEvent(ref VREvent_t pEvent)
	{
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay == null)
		{
			return false;
		}
		uint num = (uint)Marshal.SizeOf(typeof(VREvent_t));
		return overlay.PollNextOverlayEvent(this.handle, ref pEvent, num);
	}

	// Token: 0x06001965 RID: 6501 RVA: 0x000E7AA0 File Offset: 0x000E5EA0
	public bool ComputeIntersection(Vector3 source, Vector3 direction, ref SteamVR_Overlay.IntersectionResults results)
	{
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay == null)
		{
			return false;
		}
		VROverlayIntersectionParams_t vroverlayIntersectionParams_t = default(VROverlayIntersectionParams_t);
		vroverlayIntersectionParams_t.eOrigin = SteamVR_Render.instance.trackingSpace;
		vroverlayIntersectionParams_t.vSource.v0 = source.x;
		vroverlayIntersectionParams_t.vSource.v1 = source.y;
		vroverlayIntersectionParams_t.vSource.v2 = -source.z;
		vroverlayIntersectionParams_t.vDirection.v0 = direction.x;
		vroverlayIntersectionParams_t.vDirection.v1 = direction.y;
		vroverlayIntersectionParams_t.vDirection.v2 = -direction.z;
		VROverlayIntersectionResults_t vroverlayIntersectionResults_t = default(VROverlayIntersectionResults_t);
		if (!overlay.ComputeOverlayIntersection(this.handle, ref vroverlayIntersectionParams_t, ref vroverlayIntersectionResults_t))
		{
			return false;
		}
		results.point = new Vector3(vroverlayIntersectionResults_t.vPoint.v0, vroverlayIntersectionResults_t.vPoint.v1, -vroverlayIntersectionResults_t.vPoint.v2);
		results.normal = new Vector3(vroverlayIntersectionResults_t.vNormal.v0, vroverlayIntersectionResults_t.vNormal.v1, -vroverlayIntersectionResults_t.vNormal.v2);
		results.UVs = new Vector2(vroverlayIntersectionResults_t.vUVs.v0, vroverlayIntersectionResults_t.vUVs.v1);
		results.distance = vroverlayIntersectionResults_t.fDistance;
		return true;
	}

	// Token: 0x040017A8 RID: 6056
	public Texture texture;

	// Token: 0x040017A9 RID: 6057
	public bool curved = true;

	// Token: 0x040017AA RID: 6058
	public bool antialias = true;

	// Token: 0x040017AB RID: 6059
	public bool highquality = true;

	// Token: 0x040017AC RID: 6060
	public float scale = 3f;

	// Token: 0x040017AD RID: 6061
	public float distance = 1.25f;

	// Token: 0x040017AE RID: 6062
	public float alpha = 1f;

	// Token: 0x040017AF RID: 6063
	public Vector4 uvOffset = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x040017B0 RID: 6064
	public Vector2 mouseScale = new Vector2(1f, 1f);

	// Token: 0x040017B1 RID: 6065
	public Vector2 curvedRange = new Vector2(1f, 2f);

	// Token: 0x040017B2 RID: 6066
	public VROverlayInputMethod inputMethod;

	// Token: 0x040017B4 RID: 6068
	private ulong handle;

	// Token: 0x020002A2 RID: 674
	public struct IntersectionResults
	{
		// Token: 0x040017B5 RID: 6069
		public Vector3 point;

		// Token: 0x040017B6 RID: 6070
		public Vector3 normal;

		// Token: 0x040017B7 RID: 6071
		public Vector2 UVs;

		// Token: 0x040017B8 RID: 6072
		public float distance;
	}
}
