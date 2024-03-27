using System;
using System.Collections.Generic;

// Token: 0x0200022A RID: 554
public class GetQuickEquipList_Response : ResponseBase
{
	// Token: 0x04001316 RID: 4886
	public bool ok;

	// Token: 0x04001317 RID: 4887
	public string reasonFailed;

	// Token: 0x04001318 RID: 4888
	public List<string> thingIds;
}
