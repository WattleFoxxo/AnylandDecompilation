using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class CellTreeNode
{
	// Token: 0x06000618 RID: 1560 RVA: 0x0001C5FD File Offset: 0x0001A9FD
	public CellTreeNode()
	{
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0001C605 File Offset: 0x0001AA05
	public CellTreeNode(byte id, CellTreeNode.ENodeType nodeType, CellTreeNode parent)
	{
		this.Id = id;
		this.NodeType = nodeType;
		this.Parent = parent;
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0001C622 File Offset: 0x0001AA22
	public void AddChild(CellTreeNode child)
	{
		if (this.Childs == null)
		{
			this.Childs = new List<CellTreeNode>(1);
		}
		this.Childs.Add(child);
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0001C647 File Offset: 0x0001AA47
	public void Draw()
	{
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0001C64C File Offset: 0x0001AA4C
	public void GetActiveCells(List<byte> activeCells, bool yIsUpAxis, Vector3 position)
	{
		if (this.NodeType != CellTreeNode.ENodeType.Leaf)
		{
			foreach (CellTreeNode cellTreeNode in this.Childs)
			{
				cellTreeNode.GetActiveCells(activeCells, yIsUpAxis, position);
			}
		}
		else if (this.IsPointNearCell(yIsUpAxis, position))
		{
			if (this.IsPointInsideCell(yIsUpAxis, position))
			{
				activeCells.Insert(0, this.Id);
				for (CellTreeNode cellTreeNode2 = this.Parent; cellTreeNode2 != null; cellTreeNode2 = cellTreeNode2.Parent)
				{
					activeCells.Insert(0, cellTreeNode2.Id);
				}
			}
			else
			{
				activeCells.Add(this.Id);
			}
		}
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0001C71C File Offset: 0x0001AB1C
	public bool IsPointInsideCell(bool yIsUpAxis, Vector3 point)
	{
		if (point.x < this.TopLeft.x || point.x > this.BottomRight.x)
		{
			return false;
		}
		if (yIsUpAxis)
		{
			if (point.y >= this.TopLeft.y && point.y <= this.BottomRight.y)
			{
				return true;
			}
		}
		else if (point.z >= this.TopLeft.z && point.z <= this.BottomRight.z)
		{
			return true;
		}
		return false;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0001C7C8 File Offset: 0x0001ABC8
	public bool IsPointNearCell(bool yIsUpAxis, Vector3 point)
	{
		if (this.maxDistance == 0f)
		{
			this.maxDistance = (this.Size.x + this.Size.y + this.Size.z) / 2f;
		}
		return (point - this.Center).sqrMagnitude <= this.maxDistance * this.maxDistance;
	}

	// Token: 0x04000483 RID: 1155
	public byte Id;

	// Token: 0x04000484 RID: 1156
	public Vector3 Center;

	// Token: 0x04000485 RID: 1157
	public Vector3 Size;

	// Token: 0x04000486 RID: 1158
	public Vector3 TopLeft;

	// Token: 0x04000487 RID: 1159
	public Vector3 BottomRight;

	// Token: 0x04000488 RID: 1160
	public CellTreeNode.ENodeType NodeType;

	// Token: 0x04000489 RID: 1161
	public CellTreeNode Parent;

	// Token: 0x0400048A RID: 1162
	public List<CellTreeNode> Childs;

	// Token: 0x0400048B RID: 1163
	private float maxDistance;

	// Token: 0x020000B2 RID: 178
	public enum ENodeType
	{
		// Token: 0x0400048D RID: 1165
		Root,
		// Token: 0x0400048E RID: 1166
		Node,
		// Token: 0x0400048F RID: 1167
		Leaf
	}
}
