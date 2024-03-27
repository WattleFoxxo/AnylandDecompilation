using System;

// Token: 0x020000B0 RID: 176
public class CellTree
{
	// Token: 0x06000614 RID: 1556 RVA: 0x0001C5D5 File Offset: 0x0001A9D5
	public CellTree()
	{
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0001C5DD File Offset: 0x0001A9DD
	public CellTree(CellTreeNode root)
	{
		this.RootNode = root;
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000616 RID: 1558 RVA: 0x0001C5EC File Offset: 0x0001A9EC
	// (set) Token: 0x06000617 RID: 1559 RVA: 0x0001C5F4 File Offset: 0x0001A9F4
	public CellTreeNode RootNode { get; private set; }
}
