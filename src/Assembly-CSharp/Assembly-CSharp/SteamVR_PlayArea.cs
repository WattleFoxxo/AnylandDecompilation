using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR;

// Token: 0x020002A3 RID: 675
[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class SteamVR_PlayArea : MonoBehaviour
{
	// Token: 0x06001967 RID: 6503 RVA: 0x000E7C28 File Offset: 0x000E6028
	public static bool GetBounds(SteamVR_PlayArea.Size size, ref HmdQuad_t pRect)
	{
		if (size == SteamVR_PlayArea.Size.Calibrated)
		{
			bool flag = !SteamVR.active && !SteamVR.usingNativeSupport;
			if (flag)
			{
				EVRInitError evrinitError = EVRInitError.None;
				OpenVR.Init(ref evrinitError, EVRApplicationType.VRApplication_Other);
			}
			CVRChaperone chaperone = OpenVR.Chaperone;
			bool flag2 = chaperone != null && chaperone.GetPlayAreaRect(ref pRect);
			if (!flag2)
			{
				Debug.LogWarning("Failed to get Calibrated Play Area bounds!  Make sure you have tracking first, and that your space is calibrated.");
			}
			if (flag)
			{
				OpenVR.Shutdown();
			}
			return flag2;
		}
		try
		{
			string text = size.ToString().Substring(1);
			string[] array = text.Split(new char[] { 'x' }, 2);
			float num = float.Parse(array[0]) / 200f;
			float num2 = float.Parse(array[1]) / 200f;
			pRect.vCorners0.v0 = num;
			pRect.vCorners0.v1 = 0f;
			pRect.vCorners0.v2 = num2;
			pRect.vCorners1.v0 = num;
			pRect.vCorners1.v1 = 0f;
			pRect.vCorners1.v2 = -num2;
			pRect.vCorners2.v0 = -num;
			pRect.vCorners2.v1 = 0f;
			pRect.vCorners2.v2 = -num2;
			pRect.vCorners3.v0 = -num;
			pRect.vCorners3.v1 = 0f;
			pRect.vCorners3.v2 = num2;
			return true;
		}
		catch
		{
		}
		return false;
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x000E7DBC File Offset: 0x000E61BC
	public void BuildMesh()
	{
		HmdQuad_t hmdQuad_t = default(HmdQuad_t);
		if (!SteamVR_PlayArea.GetBounds(this.size, ref hmdQuad_t))
		{
			return;
		}
		HmdVector3_t[] array = new HmdVector3_t[] { hmdQuad_t.vCorners0, hmdQuad_t.vCorners1, hmdQuad_t.vCorners2, hmdQuad_t.vCorners3 };
		this.vertices = new Vector3[array.Length * 2];
		for (int i = 0; i < array.Length; i++)
		{
			HmdVector3_t hmdVector3_t = array[i];
			this.vertices[i] = new Vector3(hmdVector3_t.v0, 0.01f, hmdVector3_t.v2);
		}
		if (this.borderThickness == 0f)
		{
			base.GetComponent<MeshFilter>().mesh = null;
			return;
		}
		for (int j = 0; j < array.Length; j++)
		{
			int num = (j + 1) % array.Length;
			int num2 = (j + array.Length - 1) % array.Length;
			Vector3 normalized = (this.vertices[num] - this.vertices[j]).normalized;
			Vector3 normalized2 = (this.vertices[num2] - this.vertices[j]).normalized;
			Vector3 vector = this.vertices[j];
			vector += Vector3.Cross(normalized, Vector3.up) * this.borderThickness;
			vector += Vector3.Cross(normalized2, Vector3.down) * this.borderThickness;
			this.vertices[array.Length + j] = vector;
		}
		int[] array2 = new int[]
		{
			0, 4, 1, 1, 4, 5, 1, 5, 2, 2,
			5, 6, 2, 6, 3, 3, 6, 7, 3, 7,
			0, 0, 7, 4
		};
		Vector2[] array3 = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		Color[] array4 = new Color[]
		{
			this.color,
			this.color,
			this.color,
			this.color,
			new Color(this.color.r, this.color.g, this.color.b, 0f),
			new Color(this.color.r, this.color.g, this.color.b, 0f),
			new Color(this.color.r, this.color.g, this.color.b, 0f),
			new Color(this.color.r, this.color.g, this.color.b, 0f)
		};
		Mesh mesh = new Mesh();
		base.GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = this.vertices;
		mesh.uv = array3;
		mesh.colors = array4;
		mesh.triangles = array2;
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		component.material = new Material(Shader.Find("Sprites/Default"));
		component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.receiveShadows = false;
		component.lightProbeUsage = LightProbeUsage.Off;
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x000E824C File Offset: 0x000E664C
	private void OnDrawGizmos()
	{
		if (!this.drawWireframeWhenSelectedOnly)
		{
			this.DrawWireframe();
		}
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x000E825F File Offset: 0x000E665F
	private void OnDrawGizmosSelected()
	{
		if (this.drawWireframeWhenSelectedOnly)
		{
			this.DrawWireframe();
		}
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x000E8274 File Offset: 0x000E6674
	public void DrawWireframe()
	{
		if (this.vertices == null || this.vertices.Length == 0)
		{
			return;
		}
		Vector3 vector = base.transform.TransformVector(Vector3.up * this.wireframeHeight);
		for (int i = 0; i < 4; i++)
		{
			int num = (i + 1) % 4;
			Vector3 vector2 = base.transform.TransformPoint(this.vertices[i]);
			Vector3 vector3 = vector2 + vector;
			Vector3 vector4 = base.transform.TransformPoint(this.vertices[num]);
			Vector3 vector5 = vector4 + vector;
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector2, vector4);
			Gizmos.DrawLine(vector3, vector5);
		}
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x000E8338 File Offset: 0x000E6738
	public void OnEnable()
	{
		if (Application.isPlaying)
		{
			base.GetComponent<MeshRenderer>().enabled = this.drawInGame;
			base.enabled = false;
			if (this.drawInGame && this.size == SteamVR_PlayArea.Size.Calibrated)
			{
				base.StartCoroutine("UpdateBounds");
			}
		}
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x000E838C File Offset: 0x000E678C
	private IEnumerator UpdateBounds()
	{
		base.GetComponent<MeshFilter>().mesh = null;
		CVRChaperone chaperone = OpenVR.Chaperone;
		if (chaperone == null)
		{
			yield break;
		}
		while (chaperone.GetCalibrationState() != ChaperoneCalibrationState.OK)
		{
			yield return null;
		}
		this.BuildMesh();
		yield break;
	}

	// Token: 0x040017B9 RID: 6073
	public float borderThickness = 0.15f;

	// Token: 0x040017BA RID: 6074
	public float wireframeHeight = 2f;

	// Token: 0x040017BB RID: 6075
	public bool drawWireframeWhenSelectedOnly;

	// Token: 0x040017BC RID: 6076
	public bool drawInGame = true;

	// Token: 0x040017BD RID: 6077
	public SteamVR_PlayArea.Size size;

	// Token: 0x040017BE RID: 6078
	public Color color = Color.cyan;

	// Token: 0x040017BF RID: 6079
	[HideInInspector]
	public Vector3[] vertices;

	// Token: 0x020002A4 RID: 676
	public enum Size
	{
		// Token: 0x040017C1 RID: 6081
		Calibrated,
		// Token: 0x040017C2 RID: 6082
		_400x300,
		// Token: 0x040017C3 RID: 6083
		_300x225,
		// Token: 0x040017C4 RID: 6084
		_200x150
	}
}
