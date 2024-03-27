using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BC RID: 444
public static class NormalSolver
{
	// Token: 0x06000DB2 RID: 3506 RVA: 0x0007A308 File Offset: 0x00078708
	public static void RecalculateNormals(this Mesh mesh, float angle)
	{
		if (angle == 0f)
		{
			mesh.RecalculateNormals();
			return;
		}
		float num = Mathf.Cos(angle * 0.017453292f);
		Vector3[] vertices = mesh.vertices;
		Vector3[] array = new Vector3[vertices.Length];
		Vector3[][] array2 = new Vector3[mesh.subMeshCount][];
		Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>> dictionary = new Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>>(vertices.Length);
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			int[] triangles = mesh.GetTriangles(i);
			array2[i] = new Vector3[triangles.Length / 3];
			for (int j = 0; j < triangles.Length; j += 3)
			{
				int num2 = triangles[j];
				int num3 = triangles[j + 1];
				int num4 = triangles[j + 2];
				Vector3 vector = vertices[num3] - vertices[num2];
				Vector3 vector2 = vertices[num4] - vertices[num2];
				Vector3 normalized = Vector3.Cross(vector, vector2).normalized;
				int num5 = j / 3;
				array2[i][num5] = normalized;
				Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>> dictionary2 = dictionary;
				NormalSolver.VertexKey vertexKey = new NormalSolver.VertexKey(vertices[num2]);
				List<NormalSolver.VertexEntry> list;
				if (!dictionary2.TryGetValue(vertexKey, out list))
				{
					list = new List<NormalSolver.VertexEntry>(4);
					dictionary.Add(vertexKey, list);
				}
				list.Add(new NormalSolver.VertexEntry(i, num5, num2));
				Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>> dictionary3 = dictionary;
				vertexKey = new NormalSolver.VertexKey(vertices[num3]);
				if (!dictionary3.TryGetValue(vertexKey, out list))
				{
					list = new List<NormalSolver.VertexEntry>();
					dictionary.Add(vertexKey, list);
				}
				list.Add(new NormalSolver.VertexEntry(i, num5, num3));
				Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>> dictionary4 = dictionary;
				vertexKey = new NormalSolver.VertexKey(vertices[num4]);
				if (!dictionary4.TryGetValue(vertexKey, out list))
				{
					list = new List<NormalSolver.VertexEntry>();
					dictionary.Add(vertexKey, list);
				}
				list.Add(new NormalSolver.VertexEntry(i, num5, num4));
			}
		}
		foreach (List<NormalSolver.VertexEntry> list2 in dictionary.Values)
		{
			for (int k = 0; k < list2.Count; k++)
			{
				Vector3 vector3 = default(Vector3);
				NormalSolver.VertexEntry vertexEntry = list2[k];
				for (int l = 0; l < list2.Count; l++)
				{
					NormalSolver.VertexEntry vertexEntry2 = list2[l];
					if (vertexEntry.VertexIndex == vertexEntry2.VertexIndex)
					{
						vector3 += array2[vertexEntry2.MeshIndex][vertexEntry2.TriangleIndex];
					}
					else
					{
						float num6 = Vector3.Dot(array2[vertexEntry.MeshIndex][vertexEntry.TriangleIndex], array2[vertexEntry2.MeshIndex][vertexEntry2.TriangleIndex]);
						if (num6 >= num)
						{
							vector3 += array2[vertexEntry2.MeshIndex][vertexEntry2.TriangleIndex];
						}
					}
				}
				array[vertexEntry.VertexIndex] = vector3.normalized;
			}
		}
		mesh.normals = array;
	}

	// Token: 0x020001BD RID: 445
	private struct VertexKey
	{
		// Token: 0x06000DB3 RID: 3507 RVA: 0x0007A678 File Offset: 0x00078A78
		public VertexKey(Vector3 position)
		{
			this._x = (long)Mathf.Round(position.x * 100000f);
			this._y = (long)Mathf.Round(position.y * 100000f);
			this._z = (long)Mathf.Round(position.z * 100000f);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0007A6D0 File Offset: 0x00078AD0
		public override bool Equals(object obj)
		{
			NormalSolver.VertexKey vertexKey = (NormalSolver.VertexKey)obj;
			return this._x == vertexKey._x && this._y == vertexKey._y && this._z == vertexKey._z;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0007A71C File Offset: 0x00078B1C
		public override int GetHashCode()
		{
			long num = (long)((ulong)(-2128831035));
			num ^= this._x;
			num *= 16777619L;
			num ^= this._y;
			num *= 16777619L;
			num ^= this._z;
			return (num * 16777619L).GetHashCode();
		}

		// Token: 0x04000F2C RID: 3884
		private readonly long _x;

		// Token: 0x04000F2D RID: 3885
		private readonly long _y;

		// Token: 0x04000F2E RID: 3886
		private readonly long _z;

		// Token: 0x04000F2F RID: 3887
		private const int Tolerance = 100000;

		// Token: 0x04000F30 RID: 3888
		private const long FNV32Init = 2166136261L;

		// Token: 0x04000F31 RID: 3889
		private const long FNV32Prime = 16777619L;
	}

	// Token: 0x020001BE RID: 446
	private struct VertexEntry
	{
		// Token: 0x06000DB6 RID: 3510 RVA: 0x0007A773 File Offset: 0x00078B73
		public VertexEntry(int meshIndex, int triIndex, int vertIndex)
		{
			this.MeshIndex = meshIndex;
			this.TriangleIndex = triIndex;
			this.VertexIndex = vertIndex;
		}

		// Token: 0x04000F32 RID: 3890
		public int MeshIndex;

		// Token: 0x04000F33 RID: 3891
		public int TriangleIndex;

		// Token: 0x04000F34 RID: 3892
		public int VertexIndex;
	}
}
