using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020001C1 RID: 449
public class OBJLoader
{
	// Token: 0x06000DCB RID: 3531 RVA: 0x0007B50C File Offset: 0x0007990C
	public static Vector3 ParseVectorFromCMPS(string[] cmps)
	{
		float num = float.Parse(cmps[1]);
		float num2 = float.Parse(cmps[2]);
		if (cmps.Length == 4)
		{
			float num3 = float.Parse(cmps[3]);
			return new Vector3(num, num2, num3);
		}
		return new Vector2(num, num2);
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0007B554 File Offset: 0x00079954
	public static Color ParseColorFromCMPS(string[] cmps, float scalar = 1f)
	{
		float num = float.Parse(cmps[1]) * scalar;
		float num2 = float.Parse(cmps[2]) * scalar;
		float num3 = float.Parse(cmps[3]) * scalar;
		return new Color(num, num2, num3);
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x0007B58C File Offset: 0x0007998C
	public static string OBJGetFilePath(string path, string basePath, string fileName)
	{
		foreach (string text in OBJLoader.searchPaths)
		{
			string text2 = text.Replace("%FileName%", fileName);
			if (File.Exists(basePath + text2 + path))
			{
				return basePath + text2 + path;
			}
			if (File.Exists(path))
			{
				return path;
			}
		}
		return null;
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x0007B5F0 File Offset: 0x000799F0
	public static Material[] LoadMTLFile(string fn)
	{
		Material material = null;
		List<Material> list = new List<Material>();
		FileInfo fileInfo = new FileInfo(fn);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fn);
		string text = fileInfo.Directory.FullName + Path.DirectorySeparatorChar;
		foreach (string text2 in File.ReadAllLines(fn))
		{
			string text3 = text2.Trim().Replace("  ", " ");
			string[] array2 = text3.Split(new char[] { ' ' });
			string text4 = text3.Remove(0, text3.IndexOf(' ') + 1);
			if (array2[0] == "newmtl")
			{
				if (material != null)
				{
					list.Add(material);
				}
				material = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
				material.name = text4;
			}
			else if (array2[0] == "Kd")
			{
				material.SetColor("_Color", OBJLoader.ParseColorFromCMPS(array2, 1f));
			}
			else if (array2[0] == "map_Kd")
			{
				string text5 = OBJLoader.OBJGetFilePath(text4, text, fileNameWithoutExtension);
				if (text5 != null)
				{
					material.SetTexture("_MainTex", TextureLoader.LoadTexture(text5, false));
				}
			}
			else if (array2[0] == "map_Bump")
			{
				string text6 = OBJLoader.OBJGetFilePath(text4, text, fileNameWithoutExtension);
				if (text6 != null)
				{
					material.SetTexture("_BumpMap", TextureLoader.LoadTexture(text6, true));
					material.EnableKeyword("_NORMALMAP");
				}
			}
			else if (array2[0] == "Ks")
			{
				material.SetColor("_SpecColor", OBJLoader.ParseColorFromCMPS(array2, 1f));
			}
			else if (array2[0] == "Ka")
			{
				material.SetColor("_EmissionColor", OBJLoader.ParseColorFromCMPS(array2, 0.05f));
				material.EnableKeyword("_EMISSION");
			}
			else if (array2[0] == "d")
			{
				float num = float.Parse(array2[1]);
				if (num < 1f)
				{
					Color color = material.color;
					color.a = num;
					material.SetColor("_Color", color);
					material.SetFloat("_Mode", 3f);
					material.SetInt("_SrcBlend", 5);
					material.SetInt("_DstBlend", 10);
					material.SetInt("_ZWrite", 0);
					material.DisableKeyword("_ALPHATEST_ON");
					material.EnableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 3000;
				}
			}
			else if (array2[0] == "Ns")
			{
				float num2 = float.Parse(array2[1]);
				num2 /= 1000f;
				material.SetFloat("_Glossiness", num2);
			}
		}
		if (material != null)
		{
			list.Add(material);
		}
		return list.ToArray();
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x0007B8F8 File Offset: 0x00079CF8
	public static GameObject LoadOBJFile(string fn)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fn);
		bool flag = false;
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<Vector3> list4 = new List<Vector3>();
		List<Vector3> list5 = new List<Vector3>();
		List<Vector2> list6 = new List<Vector2>();
		List<string> list7 = new List<string>();
		List<string> list8 = new List<string>();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		List<OBJLoader.OBJFace> list9 = new List<OBJLoader.OBJFace>();
		string text = string.Empty;
		string text2 = "default";
		Material[] array = null;
		FileInfo fileInfo = new FileInfo(fn);
		foreach (string text3 in File.ReadAllLines(fn))
		{
			if (text3.Length > 0 && text3[0] != '#')
			{
				string text4 = text3.Trim().Replace("  ", " ");
				string[] array3 = text4.Split(new char[] { ' ' });
				string text5 = text4.Remove(0, text4.IndexOf(' ') + 1);
				if (array3[0] == "mtllib")
				{
					string text6 = OBJLoader.OBJGetFilePath(text5, fileInfo.Directory.FullName + Path.DirectorySeparatorChar, fileNameWithoutExtension);
					if (text6 != null)
					{
						array = OBJLoader.LoadMTLFile(text6);
					}
				}
				else if ((array3[0] == "g" || array3[0] == "o") && !OBJLoader.splitByMaterial)
				{
					text2 = text5;
					if (!list8.Contains(text2))
					{
						list8.Add(text2);
					}
				}
				else if (array3[0] == "usemtl")
				{
					text = text5;
					if (!list7.Contains(text))
					{
						list7.Add(text);
					}
					if (OBJLoader.splitByMaterial && !list8.Contains(text))
					{
						list8.Add(text);
					}
				}
				else if (array3[0] == "v")
				{
					list.Add(OBJLoader.ParseVectorFromCMPS(array3));
				}
				else if (array3[0] == "vn")
				{
					list2.Add(OBJLoader.ParseVectorFromCMPS(array3));
				}
				else if (array3[0] == "vt")
				{
					list3.Add(OBJLoader.ParseVectorFromCMPS(array3));
				}
				else if (array3[0] == "f")
				{
					int[] array4 = new int[array3.Length - 1];
					for (int j = 1; j < array3.Length; j++)
					{
						string text7 = array3[j];
						int num = -1;
						int num2 = -1;
						int num3;
						if (text7.Contains("//"))
						{
							string[] array5 = text7.Split(new char[] { '/' });
							num3 = int.Parse(array5[0]) - 1;
							num = int.Parse(array5[2]) - 1;
						}
						else if (text7.Count((char x) => x == '/') == 2)
						{
							string[] array6 = text7.Split(new char[] { '/' });
							num3 = int.Parse(array6[0]) - 1;
							num2 = int.Parse(array6[1]) - 1;
							num = int.Parse(array6[2]) - 1;
						}
						else if (!text7.Contains("/"))
						{
							num3 = int.Parse(text7) - 1;
						}
						else
						{
							string[] array7 = text7.Split(new char[] { '/' });
							num3 = int.Parse(array7[0]) - 1;
							num2 = int.Parse(array7[1]) - 1;
						}
						string text8 = string.Concat(new object[] { num3, "|", num, "|", num2 });
						if (dictionary.ContainsKey(text8))
						{
							array4[j - 1] = dictionary[text8];
						}
						else
						{
							array4[j - 1] = dictionary.Count;
							dictionary[text8] = dictionary.Count;
							list4.Add(list[num3]);
							if (num < 0 || num > list2.Count - 1)
							{
								list5.Add(Vector3.zero);
							}
							else
							{
								flag = true;
								list5.Add(list2[num]);
							}
							if (num2 < 0 || num2 > list3.Count - 1)
							{
								list6.Add(Vector2.zero);
							}
							else
							{
								list6.Add(list3[num2]);
							}
						}
					}
					if (array4.Length < 5 && array4.Length >= 3)
					{
						list9.Add(new OBJLoader.OBJFace
						{
							materialName = text,
							indexes = new int[]
							{
								array4[0],
								array4[1],
								array4[2]
							},
							meshName = ((!OBJLoader.splitByMaterial) ? text2 : text)
						});
						if (array4.Length > 3)
						{
							list9.Add(new OBJLoader.OBJFace
							{
								materialName = text,
								meshName = ((!OBJLoader.splitByMaterial) ? text2 : text),
								indexes = new int[]
								{
									array4[2],
									array4[3],
									array4[0]
								}
							});
						}
					}
				}
			}
		}
		if (list8.Count == 0)
		{
			list8.Add("default");
		}
		GameObject gameObject = new GameObject(fileNameWithoutExtension);
		using (List<string>.Enumerator enumerator = list8.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				OBJLoader.<LoadOBJFile>c__AnonStorey0 <LoadOBJFile>c__AnonStorey = new OBJLoader.<LoadOBJFile>c__AnonStorey0();
				<LoadOBJFile>c__AnonStorey.obj = enumerator.Current;
				OBJLoader.<LoadOBJFile>c__AnonStorey2 <LoadOBJFile>c__AnonStorey2 = new OBJLoader.<LoadOBJFile>c__AnonStorey2();
				<LoadOBJFile>c__AnonStorey2.<>f__ref$0 = <LoadOBJFile>c__AnonStorey;
				GameObject gameObject2 = new GameObject(<LoadOBJFile>c__AnonStorey.obj);
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localScale = new Vector3(-1f, 1f, 1f);
				Mesh mesh = new Mesh();
				mesh.indexFormat = IndexFormat.UInt32;
				mesh.name = <LoadOBJFile>c__AnonStorey.obj;
				List<Vector3> list10 = new List<Vector3>();
				List<Vector3> list11 = new List<Vector3>();
				List<Vector2> list12 = new List<Vector2>();
				List<int[]> list13 = new List<int[]>();
				Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
				<LoadOBJFile>c__AnonStorey2.meshMaterialNames = new List<string>();
				OBJLoader.OBJFace[] array8 = list9.Where((OBJLoader.OBJFace x) => x.meshName == <LoadOBJFile>c__AnonStorey2.<>f__ref$0.obj).ToArray<OBJLoader.OBJFace>();
				using (List<string>.Enumerator enumerator2 = list7.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string mn = enumerator2.Current;
						OBJLoader.OBJFace[] array9 = array8.Where((OBJLoader.OBJFace x) => x.materialName == mn).ToArray<OBJLoader.OBJFace>();
						if (array9.Length > 0)
						{
							int[] array10 = new int[0];
							foreach (OBJLoader.OBJFace objface in array9)
							{
								int num4 = array10.Length;
								Array.Resize<int>(ref array10, num4 + objface.indexes.Length);
								Array.Copy(objface.indexes, 0, array10, num4, objface.indexes.Length);
							}
							<LoadOBJFile>c__AnonStorey2.meshMaterialNames.Add(mn);
							if (mesh.subMeshCount != <LoadOBJFile>c__AnonStorey2.meshMaterialNames.Count)
							{
								mesh.subMeshCount = <LoadOBJFile>c__AnonStorey2.meshMaterialNames.Count;
							}
							for (int l = 0; l < array10.Length; l++)
							{
								int num5 = array10[l];
								if (dictionary2.ContainsKey(num5))
								{
									array10[l] = dictionary2[num5];
								}
								else
								{
									list10.Add(list4[num5]);
									list11.Add(list5[num5]);
									list12.Add(list6[num5]);
									dictionary2[num5] = list10.Count - 1;
									array10[l] = dictionary2[num5];
								}
							}
							list13.Add(array10);
						}
					}
				}
				mesh.vertices = list10.ToArray();
				mesh.normals = list11.ToArray();
				mesh.uv = list12.ToArray();
				for (int m = 0; m < list13.Count; m++)
				{
					mesh.SetTriangles(list13[m], m);
				}
				if (!flag)
				{
					mesh.RecalculateNormals();
				}
				mesh.RecalculateBounds();
				MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
				MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
				Material[] array12 = new Material[<LoadOBJFile>c__AnonStorey2.meshMaterialNames.Count];
				int i;
				for (i = 0; i < <LoadOBJFile>c__AnonStorey2.meshMaterialNames.Count; i++)
				{
					if (array == null)
					{
						array12[i] = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
					}
					else
					{
						Material material = Array.Find<Material>(array, (Material x) => x.name == <LoadOBJFile>c__AnonStorey2.meshMaterialNames[i]);
						if (material == null)
						{
							array12[i] = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
						}
						else
						{
							array12[i] = material;
						}
					}
					array12[i].name = <LoadOBJFile>c__AnonStorey2.meshMaterialNames[i];
				}
				meshRenderer.materials = array12;
				meshFilter.mesh = mesh;
			}
		}
		return gameObject;
	}

	// Token: 0x04000F46 RID: 3910
	public static bool splitByMaterial = false;

	// Token: 0x04000F47 RID: 3911
	public static string[] searchPaths = new string[]
	{
		string.Empty,
		"%FileName%_Textures" + Path.DirectorySeparatorChar
	};

	// Token: 0x04000F48 RID: 3912
	private const string shaderName = "Legacy Shaders/Transparent/Diffuse";

	// Token: 0x020001C2 RID: 450
	private struct OBJFace
	{
		// Token: 0x04000F4A RID: 3914
		public string materialName;

		// Token: 0x04000F4B RID: 3915
		public string meshName;

		// Token: 0x04000F4C RID: 3916
		public int[] indexes;
	}
}
