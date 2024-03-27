using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000238 RID: 568
public class PollServerResponse : ServerResponse
{
	// Token: 0x060015ED RID: 5613 RVA: 0x000C04DC File Offset: 0x000BE8DC
	public PollServerResponse(WWW www)
		: base(www)
	{
		this.www = www;
		if (www.error == null)
		{
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.versionMajorServerAndClient = jsonnode["vMaj"].Value;
				this.versionMinorServerOnly = jsonnode["vMinSrv"].Value;
				this.pingFromPersonId = jsonnode["pingFromUserId"].Value;
				this.pingFromPersonName = jsonnode["pingFromUserName"].Value;
				this.pingFromAreaId = jsonnode["pingAreaId"].Value;
				this.pingFromAreaName = jsonnode["pingAreaName"].Value;
				this.generalMessage = jsonnode["gm"].Value;
				this.generalMessage_data1 = jsonnode["gm_d1"].Value;
				this.generalMessage_data2 = jsonnode["gm_d2"].Value;
				this.unseenGiftsExist = jsonnode["unseenGiftsExist"].AsBool;
			}
			else
			{
				Log.Info("Got null json in ExtendedServerResponse", false);
			}
		}
		else
		{
			Log.Error(www.error);
		}
	}

	// Token: 0x0400132B RID: 4907
	public string versionMajorServerAndClient;

	// Token: 0x0400132C RID: 4908
	public string versionMinorServerOnly;

	// Token: 0x0400132D RID: 4909
	public string pingFromPersonId;

	// Token: 0x0400132E RID: 4910
	public string pingFromPersonName;

	// Token: 0x0400132F RID: 4911
	public string pingFromAreaId;

	// Token: 0x04001330 RID: 4912
	public string pingFromAreaName;

	// Token: 0x04001331 RID: 4913
	public string generalMessage;

	// Token: 0x04001332 RID: 4914
	public string generalMessage_data1;

	// Token: 0x04001333 RID: 4915
	public string generalMessage_data2;

	// Token: 0x04001334 RID: 4916
	public bool unseenGiftsExist;

	// Token: 0x04001335 RID: 4917
	public WWW www;
}
