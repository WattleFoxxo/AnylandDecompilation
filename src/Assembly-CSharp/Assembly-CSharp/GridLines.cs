using System;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class GridLines : MonoBehaviour
{
	// Token: 0x06000D6A RID: 3434 RVA: 0x00077BF8 File Offset: 0x00075FF8
	private void Start()
	{
		Shader shader = Shader.Find("Hidden/Internal-Colored");
		this.lineMaterial = new Material(shader);
		this.lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		this.lineMaterial.SetInt("_SrcBlend", 5);
		this.lineMaterial.SetInt("_DstBlend", 10);
		this.lineMaterial.SetInt("_Cull", 0);
		this.lineMaterial.SetInt("_ZWrite", 0);
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x00077C70 File Offset: 0x00076070
	private void OnPostRender()
	{
		float num = Our.gridSize * 2f;
		float num2 = Our.gridSize * 0.5f - 0.005f;
		Vector3 vector = SnapHelper.SnapToGrid(base.transform.position, false);
		this.lineMaterial.SetPass(0);
		GL.Begin(1);
		foreach (bool flag in Misc.trueFalse)
		{
			if (!flag)
			{
				vector = new Vector3(vector.x + 0.0015f, vector.y - 0.0015f, vector.z + 0.0015f);
			}
			foreach (bool flag2 in Misc.trueFalse)
			{
				float num3 = ((!flag2) ? 0.75f : 0.1f);
				float num4 = ((!flag) ? 0f : 1f);
				GL.Color(new Color(num4, num4, num4, num3));
				for (float num5 = -num; num5 <= num; num5 += Our.gridSize)
				{
					for (float num6 = -num; num6 <= num; num6 += Our.gridSize)
					{
						for (float num7 = -num; num7 <= num; num7 += Our.gridSize)
						{
							bool flag3 = Mathf.Abs(num5) == num || Mathf.Abs(num6) == num || Mathf.Abs(num7) == num;
							if ((flag3 && flag2) || (!flag3 && !flag2))
							{
								Vector3 vector2 = new Vector3(vector.x + num5, vector.y + num6, vector.z + num7);
								this.DrawLineFromCenter(vector2, num2, 0f, 0f);
								Vector3 vector3 = vector2;
								float num8 = num2;
								this.DrawLineFromCenter(vector3, 0f, num8, 0f);
								vector3 = vector2;
								num8 = num2;
								this.DrawLineFromCenter(vector3, 0f, 0f, num8);
							}
						}
					}
				}
			}
		}
		GL.End();
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x00077E8C File Offset: 0x0007628C
	private void DrawLineFromCenter(Vector3 center, float stretchX = 0f, float stretchY = 0f, float stretchZ = 0f)
	{
		GL.Vertex3(center.x - stretchX, center.y - stretchY, center.z - stretchZ);
		GL.Vertex3(center.x + stretchX, center.y + stretchY, center.z + stretchZ);
	}

	// Token: 0x04000EF8 RID: 3832
	private Material lineMaterial;
}
