using System;
using UnityEngine;

// Token: 0x02000236 RID: 566
public class LoadInventory_Response : ServerResponse
{
	// Token: 0x060015EB RID: 5611 RVA: 0x000C0456 File Offset: 0x000BE856
	public LoadInventory_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.inventoryItems = JsonUtility.FromJson<InventoryItemDataCollection>(www.text);
		}
	}

	// Token: 0x04001329 RID: 4905
	public InventoryItemDataCollection inventoryItems;
}
