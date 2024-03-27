using System;
using System.Collections.Generic;

// Token: 0x02000230 RID: 560
public class GetThingDefinitionAreaBundle_Response : ResponseBase
{
	// Token: 0x0400131E RID: 4894
	public bool ok;

	// Token: 0x0400131F RID: 4895
	public string reasonFailed;

	// Token: 0x04001320 RID: 4896
	public List<ThingIdAndDefinition> thingDefinitions;
}
