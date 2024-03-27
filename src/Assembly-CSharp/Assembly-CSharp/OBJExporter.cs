using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class OBJExporter
{
	// Token: 0x06000DC5 RID: 3525 RVA: 0x0007ABAC File Offset: 0x00078FAC
	public void Export(string exportPath, GameObject[] gameObjects)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		FileInfo fileInfo = new FileInfo(exportPath);
		this.lastExportFolder = fileInfo.Directory.FullName;
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(exportPath);
		List<MeshFilter> list = new List<MeshFilter>();
		foreach (GameObject gameObject in gameObjects)
		{
			try
			{
				MeshFilter component = gameObject.GetComponent<MeshFilter>();
				if (component != null)
				{
					list.Add(component);
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex);
			}
		}
		MeshFilter[] array = list.ToArray();
		if (Application.isPlaying)
		{
			foreach (MeshFilter meshFilter in array)
			{
				MeshRenderer component2 = meshFilter.gameObject.GetComponent<MeshRenderer>();
				if (component2 != null && component2.isPartOfStaticBatch)
				{
					return;
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder.AppendLine("# Export of " + Application.loadedLevelName);
		stringBuilder.AppendLine("# from Aaro4130 OBJ Exporter " + this.versionString);
		if (this.generateMaterials)
		{
			stringBuilder.AppendLine("mtllib " + fileNameWithoutExtension + ".mtl");
		}
		int num = 0;
		for (int k = 0; k < array.Length; k++)
		{
			string name = array[k].gameObject.name;
			MeshFilter meshFilter2 = array[k];
			MeshRenderer component3 = array[k].gameObject.GetComponent<MeshRenderer>();
			if (this.splitObjects)
			{
				string text = name;
				if (this.objNameAddIdNum)
				{
					text = text + "_" + k;
				}
				stringBuilder.AppendLine("g " + text);
			}
			if (component3 != null && this.generateMaterials)
			{
				foreach (Material material in component3.sharedMaterials)
				{
					if (!dictionary.ContainsKey(material.name))
					{
						dictionary[material.name] = true;
						stringBuilder2.Append(this.MaterialToString(material));
						stringBuilder2.AppendLine();
					}
				}
			}
			Mesh sharedMesh = meshFilter2.sharedMesh;
			int num2 = (int)Mathf.Clamp(meshFilter2.gameObject.transform.lossyScale.x * meshFilter2.gameObject.transform.lossyScale.z, -1f, 1f);
			foreach (Vector3 vector in sharedMesh.vertices)
			{
				Vector3 vector2 = vector;
				if (this.applyScale)
				{
					vector2 = this.MultiplyVec3s(vector2, meshFilter2.gameObject.transform.lossyScale);
				}
				if (this.applyRotation)
				{
					vector2 = this.RotateAroundPoint(vector2, Vector3.zero, meshFilter2.gameObject.transform.rotation);
				}
				if (this.applyPosition)
				{
					vector2 += meshFilter2.gameObject.transform.position;
				}
				vector2.x *= -1f;
				stringBuilder.AppendLine(string.Concat(new object[] { "v ", vector2.x, " ", vector2.y, " ", vector2.z }));
			}
			foreach (Vector3 vector3 in sharedMesh.normals)
			{
				Vector3 vector4 = vector3;
				if (this.applyScale)
				{
					vector4 = this.MultiplyVec3s(vector4, meshFilter2.gameObject.transform.lossyScale.normalized);
				}
				if (this.applyRotation)
				{
					vector4 = this.RotateAroundPoint(vector4, Vector3.zero, meshFilter2.gameObject.transform.rotation);
				}
				vector4.x *= -1f;
				stringBuilder.AppendLine(string.Concat(new object[] { "vn ", vector4.x, " ", vector4.y, " ", vector4.z }));
			}
			foreach (Vector2 vector5 in sharedMesh.uv)
			{
				stringBuilder.AppendLine(string.Concat(new object[] { "vt ", vector5.x, " ", vector5.y }));
			}
			for (int num4 = 0; num4 < sharedMesh.subMeshCount; num4++)
			{
				if (component3 != null && num4 < component3.sharedMaterials.Length)
				{
					string name2 = component3.sharedMaterials[num4].name;
					stringBuilder.AppendLine("usemtl " + name2);
				}
				else
				{
					stringBuilder.AppendLine(string.Concat(new object[] { "usemtl ", name, "_sm", num4 }));
				}
				int[] triangles = sharedMesh.GetTriangles(num4);
				for (int num5 = 0; num5 < triangles.Length; num5 += 3)
				{
					int num6 = triangles[num5] + 1 + num;
					int num7 = triangles[num5 + 1] + 1 + num;
					int num8 = triangles[num5 + 2] + 1 + num;
					if (num2 < 0)
					{
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"f ",
							this.ConstructOBJString(num6),
							" ",
							this.ConstructOBJString(num7),
							" ",
							this.ConstructOBJString(num8)
						}));
					}
					else
					{
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"f ",
							this.ConstructOBJString(num8),
							" ",
							this.ConstructOBJString(num7),
							" ",
							this.ConstructOBJString(num6)
						}));
					}
				}
			}
			num += sharedMesh.vertices.Length;
		}
		File.WriteAllText(exportPath, stringBuilder.ToString());
		if (this.generateMaterials)
		{
			File.WriteAllText(fileInfo.Directory.FullName + "\\" + fileNameWithoutExtension + ".mtl", stringBuilder2.ToString());
		}
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x0007B2AC File Offset: 0x000796AC
	private string ConstructOBJString(int index)
	{
		string text = index.ToString();
		return string.Concat(new string[] { text, "/", text, "/", text });
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x0007B2EE File Offset: 0x000796EE
	private Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
	{
		return angle * (point - pivot) + pivot;
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x0007B303 File Offset: 0x00079703
	private Vector3 MultiplyVec3s(Vector3 v1, Vector3 v2)
	{
		return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0007B338 File Offset: 0x00079738
	private string MaterialToString(Material m)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("newmtl " + m.name);
		if (m.HasProperty("_Color"))
		{
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Kd ",
				m.color.r.ToString(),
				" ",
				m.color.g.ToString(),
				" ",
				m.color.b.ToString()
			}));
			if (m.color.a < 1f)
			{
				stringBuilder.AppendLine("Tr " + (1f - m.color.a).ToString());
				stringBuilder.AppendLine("d " + m.color.a.ToString());
			}
		}
		if (m.HasProperty("_SpecColor"))
		{
			Color color = m.GetColor("_SpecColor");
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Ks ",
				color.r.ToString(),
				" ",
				color.g.ToString(),
				" ",
				color.b.ToString()
			}));
		}
		stringBuilder.AppendLine("illum 2");
		return stringBuilder.ToString();
	}

	// Token: 0x04000F3D RID: 3901
	public bool applyPosition;

	// Token: 0x04000F3E RID: 3902
	public bool applyRotation = true;

	// Token: 0x04000F3F RID: 3903
	public bool applyScale = true;

	// Token: 0x04000F40 RID: 3904
	public bool generateMaterials = true;

	// Token: 0x04000F41 RID: 3905
	public bool splitObjects = true;

	// Token: 0x04000F42 RID: 3906
	public bool autoMarkTexReadable;

	// Token: 0x04000F43 RID: 3907
	public bool objNameAddIdNum;

	// Token: 0x04000F44 RID: 3908
	private string versionString = "v2.0";

	// Token: 0x04000F45 RID: 3909
	private string lastExportFolder;
}
