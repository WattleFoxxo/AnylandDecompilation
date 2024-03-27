using System;
using UnityEngine;

// Token: 0x020002AB RID: 683
[ExecuteInEditMode]
public class SteamVR_SphericalProjection : MonoBehaviour
{
	// Token: 0x060019AA RID: 6570 RVA: 0x000EA3F4 File Offset: 0x000E87F4
	public void Set(Vector3 N, float phi0, float phi1, float theta0, float theta1, Vector3 uAxis, Vector3 uOrigin, float uScale, Vector3 vAxis, Vector3 vOrigin, float vScale)
	{
		if (SteamVR_SphericalProjection.material == null)
		{
			SteamVR_SphericalProjection.material = new Material(Shader.Find("Custom/SteamVR_SphericalProjection"));
		}
		SteamVR_SphericalProjection.material.SetVector("_N", new Vector4(N.x, N.y, N.z));
		SteamVR_SphericalProjection.material.SetFloat("_Phi0", phi0 * 0.017453292f);
		SteamVR_SphericalProjection.material.SetFloat("_Phi1", phi1 * 0.017453292f);
		SteamVR_SphericalProjection.material.SetFloat("_Theta0", theta0 * 0.017453292f + 1.5707964f);
		SteamVR_SphericalProjection.material.SetFloat("_Theta1", theta1 * 0.017453292f + 1.5707964f);
		SteamVR_SphericalProjection.material.SetVector("_UAxis", uAxis);
		SteamVR_SphericalProjection.material.SetVector("_VAxis", vAxis);
		SteamVR_SphericalProjection.material.SetVector("_UOrigin", uOrigin);
		SteamVR_SphericalProjection.material.SetVector("_VOrigin", vOrigin);
		SteamVR_SphericalProjection.material.SetFloat("_UScale", uScale);
		SteamVR_SphericalProjection.material.SetFloat("_VScale", vScale);
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x000EA52E File Offset: 0x000E892E
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, SteamVR_SphericalProjection.material);
	}

	// Token: 0x040017F2 RID: 6130
	private static Material material;
}
