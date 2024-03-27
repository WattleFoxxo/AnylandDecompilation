using System;
using UnityEngine;
using Valve.VR;

// Token: 0x020002A9 RID: 681
public class SteamVR_Skybox : MonoBehaviour
{
	// Token: 0x060019A3 RID: 6563 RVA: 0x000EA1D0 File Offset: 0x000E85D0
	public void SetTextureByIndex(int i, Texture t)
	{
		switch (i)
		{
		case 0:
			this.front = t;
			break;
		case 1:
			this.back = t;
			break;
		case 2:
			this.left = t;
			break;
		case 3:
			this.right = t;
			break;
		case 4:
			this.top = t;
			break;
		case 5:
			this.bottom = t;
			break;
		}
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x000EA248 File Offset: 0x000E8648
	public Texture GetTextureByIndex(int i)
	{
		switch (i)
		{
		case 0:
			return this.front;
		case 1:
			return this.back;
		case 2:
			return this.left;
		case 3:
			return this.right;
		case 4:
			return this.top;
		case 5:
			return this.bottom;
		default:
			return null;
		}
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000EA2A4 File Offset: 0x000E86A4
	public static void SetOverride(Texture front = null, Texture back = null, Texture left = null, Texture right = null, Texture top = null, Texture bottom = null)
	{
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			Texture[] array = new Texture[] { front, back, left, right, top, bottom };
			Texture_t[] array2 = new Texture_t[6];
			for (int i = 0; i < 6; i++)
			{
				array2[i].handle = ((!(array[i] != null)) ? IntPtr.Zero : array[i].GetNativeTexturePtr());
				array2[i].eType = SteamVR.instance.graphicsAPI;
				array2[i].eColorSpace = EColorSpace.Auto;
			}
			EVRCompositorError evrcompositorError = compositor.SetSkyboxOverride(array2);
			if (evrcompositorError != EVRCompositorError.None)
			{
				Debug.LogError("Failed to set skybox override with error: " + evrcompositorError);
				if (evrcompositorError == EVRCompositorError.TextureIsOnWrongDevice)
				{
					Debug.Log("Set your graphics driver to use the same video card as the headset is plugged into for Unity.");
				}
				else if (evrcompositorError == EVRCompositorError.TextureUsesUnsupportedFormat)
				{
					Debug.Log("Ensure skybox textures are not compressed and have no mipmaps.");
				}
			}
		}
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000EA398 File Offset: 0x000E8798
	public static void ClearOverride()
	{
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			compositor.ClearSkyboxOverride();
		}
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x000EA3B7 File Offset: 0x000E87B7
	private void OnEnable()
	{
		SteamVR_Skybox.SetOverride(this.front, this.back, this.left, this.right, this.top, this.bottom);
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x000EA3E2 File Offset: 0x000E87E2
	private void OnDisable()
	{
		SteamVR_Skybox.ClearOverride();
	}

	// Token: 0x040017E4 RID: 6116
	public Texture front;

	// Token: 0x040017E5 RID: 6117
	public Texture back;

	// Token: 0x040017E6 RID: 6118
	public Texture left;

	// Token: 0x040017E7 RID: 6119
	public Texture right;

	// Token: 0x040017E8 RID: 6120
	public Texture top;

	// Token: 0x040017E9 RID: 6121
	public Texture bottom;

	// Token: 0x040017EA RID: 6122
	public SteamVR_Skybox.CellSize StereoCellSize = SteamVR_Skybox.CellSize.x32;

	// Token: 0x040017EB RID: 6123
	public float StereoIpdMm = 64f;

	// Token: 0x020002AA RID: 682
	public enum CellSize
	{
		// Token: 0x040017ED RID: 6125
		x1024,
		// Token: 0x040017EE RID: 6126
		x64,
		// Token: 0x040017EF RID: 6127
		x32,
		// Token: 0x040017F0 RID: 6128
		x16,
		// Token: 0x040017F1 RID: 6129
		x8
	}
}
