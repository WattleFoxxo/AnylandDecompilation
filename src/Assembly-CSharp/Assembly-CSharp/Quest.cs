using System;
using SimpleJSON;

// Token: 0x0200020C RID: 524
public class Quest
{
	// Token: 0x060015BC RID: 5564 RVA: 0x000BFAA8 File Offset: 0x000BDEA8
	public string GetJson()
	{
		string text = string.Empty;
		text += "{";
		text = text + "\"areaName\":" + JsonHelper.GetJson(this.areaName) + ",";
		text = text + "\"name\":" + JsonHelper.GetJson(this.name) + ",";
		text = text + "\"forumId\":" + JsonHelper.GetJson(this.forumId) + ",";
		text = text + "\"forumThreadId\":" + JsonHelper.GetJson(this.forumThreadId) + ",";
		text = text + "\"achieved\":" + JsonHelper.GetJson(this.achieved);
		return text + "}";
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x000BFB5C File Offset: 0x000BDF5C
	public void SetFromJson(JSONNode questNode)
	{
		this.areaName = questNode["areaName"];
		this.name = questNode["name"];
		this.forumId = questNode["forumId"];
		this.forumThreadId = questNode["forumThreadId"];
		this.achieved = questNode["forumThreadId"].AsBool;
	}

	// Token: 0x040012CF RID: 4815
	public string areaName = string.Empty;

	// Token: 0x040012D0 RID: 4816
	public string name = string.Empty;

	// Token: 0x040012D1 RID: 4817
	public string forumId = string.Empty;

	// Token: 0x040012D2 RID: 4818
	public string forumThreadId = string.Empty;

	// Token: 0x040012D3 RID: 4819
	public bool achieved;
}
