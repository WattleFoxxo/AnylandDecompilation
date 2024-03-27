using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class CullArea : MonoBehaviour
{
	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x0001BED8 File Offset: 0x0001A2D8
	// (set) Token: 0x06000608 RID: 1544 RVA: 0x0001BEE0 File Offset: 0x0001A2E0
	public int CellCount { get; private set; }

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001BEE9 File Offset: 0x0001A2E9
	// (set) Token: 0x0600060A RID: 1546 RVA: 0x0001BEF1 File Offset: 0x0001A2F1
	public CellTree CellTree { get; private set; }

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001BEFA File Offset: 0x0001A2FA
	// (set) Token: 0x0600060C RID: 1548 RVA: 0x0001BF02 File Offset: 0x0001A302
	public Dictionary<int, GameObject> Map { get; private set; }

	// Token: 0x0600060D RID: 1549 RVA: 0x0001BF0B File Offset: 0x0001A30B
	private void Awake()
	{
		this.idCounter = this.FIRST_GROUP_ID;
		this.CreateCellHierarchy();
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0001BF1F File Offset: 0x0001A31F
	public void OnDrawGizmos()
	{
		this.idCounter = this.FIRST_GROUP_ID;
		if (this.RecreateCellHierarchy)
		{
			this.CreateCellHierarchy();
		}
		this.DrawCells();
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0001BF44 File Offset: 0x0001A344
	private void CreateCellHierarchy()
	{
		if (!this.IsCellCountAllowed())
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"There are too many cells created by your subdivision options. Maximum allowed number of cells is ",
					(int)(250 - this.FIRST_GROUP_ID),
					". Current number of cells is ",
					this.CellCount,
					"."
				}));
				return;
			}
			Application.Quit();
		}
		byte b;
		this.idCounter = (b = this.idCounter) + 1;
		CellTreeNode cellTreeNode = new CellTreeNode(b, CellTreeNode.ENodeType.Root, null);
		if (this.YIsUpAxis)
		{
			this.Center = new Vector2(base.transform.position.x, base.transform.position.y);
			this.Size = new Vector2(base.transform.localScale.x, base.transform.localScale.y);
			cellTreeNode.Center = new Vector3(this.Center.x, this.Center.y, 0f);
			cellTreeNode.Size = new Vector3(this.Size.x, this.Size.y, 0f);
			cellTreeNode.TopLeft = new Vector3(this.Center.x - this.Size.x / 2f, this.Center.y - this.Size.y / 2f, 0f);
			cellTreeNode.BottomRight = new Vector3(this.Center.x + this.Size.x / 2f, this.Center.y + this.Size.y / 2f, 0f);
		}
		else
		{
			this.Center = new Vector2(base.transform.position.x, base.transform.position.z);
			this.Size = new Vector2(base.transform.localScale.x, base.transform.localScale.z);
			cellTreeNode.Center = new Vector3(this.Center.x, 0f, this.Center.y);
			cellTreeNode.Size = new Vector3(this.Size.x, 0f, this.Size.y);
			cellTreeNode.TopLeft = new Vector3(this.Center.x - this.Size.x / 2f, 0f, this.Center.y - this.Size.y / 2f);
			cellTreeNode.BottomRight = new Vector3(this.Center.x + this.Size.x / 2f, 0f, this.Center.y + this.Size.y / 2f);
		}
		this.CreateChildCells(cellTreeNode, 1);
		this.CellTree = new CellTree(cellTreeNode);
		this.RecreateCellHierarchy = false;
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0001C288 File Offset: 0x0001A688
	private void CreateChildCells(CellTreeNode parent, int cellLevelInHierarchy)
	{
		if (cellLevelInHierarchy > this.NumberOfSubdivisions)
		{
			return;
		}
		int num = (int)this.Subdivisions[cellLevelInHierarchy - 1].x;
		int num2 = (int)this.Subdivisions[cellLevelInHierarchy - 1].y;
		float num3 = parent.Center.x - parent.Size.x / 2f;
		float num4 = parent.Size.x / (float)num;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				float num5 = num3 + (float)i * num4 + num4 / 2f;
				byte b;
				this.idCounter = (b = this.idCounter) + 1;
				CellTreeNode cellTreeNode = new CellTreeNode(b, (this.NumberOfSubdivisions != cellLevelInHierarchy) ? CellTreeNode.ENodeType.Node : CellTreeNode.ENodeType.Leaf, parent);
				if (this.YIsUpAxis)
				{
					float num6 = parent.Center.y - parent.Size.y / 2f;
					float num7 = parent.Size.y / (float)num2;
					float num8 = num6 + (float)j * num7 + num7 / 2f;
					cellTreeNode.Center = new Vector3(num5, num8, 0f);
					cellTreeNode.Size = new Vector3(num4, num7, 0f);
					cellTreeNode.TopLeft = new Vector3(num5 - num4 / 2f, num8 - num7 / 2f, 0f);
					cellTreeNode.BottomRight = new Vector3(num5 + num4 / 2f, num8 + num7 / 2f, 0f);
				}
				else
				{
					float num9 = parent.Center.z - parent.Size.z / 2f;
					float num10 = parent.Size.z / (float)num2;
					float num11 = num9 + (float)j * num10 + num10 / 2f;
					cellTreeNode.Center = new Vector3(num5, 0f, num11);
					cellTreeNode.Size = new Vector3(num4, 0f, num10);
					cellTreeNode.TopLeft = new Vector3(num5 - num4 / 2f, 0f, num11 - num10 / 2f);
					cellTreeNode.BottomRight = new Vector3(num5 + num4 / 2f, 0f, num11 + num10 / 2f);
				}
				parent.AddChild(cellTreeNode);
				this.CreateChildCells(cellTreeNode, cellLevelInHierarchy + 1);
			}
		}
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0001C4F8 File Offset: 0x0001A8F8
	private void DrawCells()
	{
		if (this.CellTree != null && this.CellTree.RootNode != null)
		{
			this.CellTree.RootNode.Draw();
		}
		else
		{
			this.RecreateCellHierarchy = true;
		}
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0001C534 File Offset: 0x0001A934
	private bool IsCellCountAllowed()
	{
		int num = 1;
		int num2 = 1;
		foreach (Vector2 vector in this.Subdivisions)
		{
			num *= (int)vector.x;
			num2 *= (int)vector.y;
		}
		this.CellCount = num * num2;
		return this.CellCount <= (int)(250 - this.FIRST_GROUP_ID);
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0001C5A8 File Offset: 0x0001A9A8
	public List<byte> GetActiveCells(Vector3 position)
	{
		List<byte> list = new List<byte>(0);
		this.CellTree.RootNode.GetActiveCells(list, this.YIsUpAxis, position);
		return list;
	}

	// Token: 0x04000472 RID: 1138
	private const int MAX_NUMBER_OF_ALLOWED_CELLS = 250;

	// Token: 0x04000473 RID: 1139
	public const int MAX_NUMBER_OF_SUBDIVISIONS = 3;

	// Token: 0x04000474 RID: 1140
	public readonly byte FIRST_GROUP_ID = 1;

	// Token: 0x04000475 RID: 1141
	public readonly int[] SUBDIVISION_FIRST_LEVEL_ORDER = new int[] { 0, 1, 1, 1 };

	// Token: 0x04000476 RID: 1142
	public readonly int[] SUBDIVISION_SECOND_LEVEL_ORDER = new int[] { 0, 2, 1, 2, 0, 2, 1, 2 };

	// Token: 0x04000477 RID: 1143
	public readonly int[] SUBDIVISION_THIRD_LEVEL_ORDER = new int[]
	{
		0, 3, 2, 3, 1, 3, 2, 3, 1, 3,
		2, 3
	};

	// Token: 0x04000478 RID: 1144
	public Vector2 Center;

	// Token: 0x04000479 RID: 1145
	public Vector2 Size = new Vector2(25f, 25f);

	// Token: 0x0400047A RID: 1146
	public Vector2[] Subdivisions = new Vector2[3];

	// Token: 0x0400047B RID: 1147
	public int NumberOfSubdivisions;

	// Token: 0x0400047F RID: 1151
	public bool YIsUpAxis = true;

	// Token: 0x04000480 RID: 1152
	public bool RecreateCellHierarchy;

	// Token: 0x04000481 RID: 1153
	private byte idCounter;
}
