using System;
using UnityEngine;

// Token: 0x0200025B RID: 603
public class CreationSnapshot
{
	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06001653 RID: 5715 RVA: 0x000C742B File Offset: 0x000C582B
	// (set) Token: 0x06001654 RID: 5716 RVA: 0x000C7433 File Offset: 0x000C5833
	public string json { get; private set; }

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06001655 RID: 5717 RVA: 0x000C743C File Offset: 0x000C583C
	// (set) Token: 0x06001656 RID: 5718 RVA: 0x000C7444 File Offset: 0x000C5844
	public Vector3 position { get; private set; }

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06001657 RID: 5719 RVA: 0x000C744D File Offset: 0x000C584D
	// (set) Token: 0x06001658 RID: 5720 RVA: 0x000C7455 File Offset: 0x000C5855
	public Vector3 rotation { get; private set; }

	// Token: 0x06001659 RID: 5721 RVA: 0x000C7460 File Offset: 0x000C5860
	public void SetByThingObject(GameObject thingObject)
	{
		int num = 0;
		int num2 = 0;
		this.json = ThingToJsonConverter.GetJson(thingObject, ref num, ref num2);
		this.position = thingObject.transform.position;
		this.rotation = thingObject.transform.eulerAngles;
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x000C74A4 File Offset: 0x000C58A4
	public bool Equals(CreationSnapshot snapshot)
	{
		return snapshot.json == this.json && snapshot.position == this.position && snapshot.rotation == this.rotation;
	}
}
