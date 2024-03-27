using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class CreateArea_Response : ServerResponse
{
	// Token: 0x060015CA RID: 5578 RVA: 0x000BFD58 File Offset: 0x000BE158
	public CreateArea_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.areaId = jsonnode["id"].Value;
				this.invalidName = jsonnode["invalidName"].AsBool;
				this.badName = jsonnode["badName"].AsBool;
				this.duplicateName = jsonnode["duplicateName"].AsBool;
				this.cantCreate = jsonnode["cantCreate"].AsBool;
				this.newAreaCount = jsonnode["newAreaCount"].AsInt;
			}
		}
	}

	// Token: 0x040012E6 RID: 4838
	public string areaId;

	// Token: 0x040012E7 RID: 4839
	public bool invalidName;

	// Token: 0x040012E8 RID: 4840
	public bool badName;

	// Token: 0x040012E9 RID: 4841
	public bool duplicateName;

	// Token: 0x040012EA RID: 4842
	public bool cantCreate;

	// Token: 0x040012EB RID: 4843
	public int newAreaCount;
}
