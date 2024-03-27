using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000246 RID: 582
public class StartAuthenticatedSession_Response : ServerResponse
{
	// Token: 0x060015FC RID: 5628 RVA: 0x000C0724 File Offset: 0x000BEB24
	public StartAuthenticatedSession_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.startInfo = new StartInfo();
			JSONNode jsonnode = JSON.Parse(www.text);
			if (jsonnode != null)
			{
				this.isHardBanned = jsonnode["isHardBanned"].AsBool;
				this.steamErrorCode = jsonnode["steamErrorCode"].Value;
				this.startInfo.versionMajorServerAndClient = jsonnode["vMaj"].Value;
				this.startInfo.versionMinorServerOnly = jsonnode["vMinSrv"].Value;
				this.startInfo.personId = jsonnode["personId"].Value;
				this.startInfo.screenName = jsonnode["screenName"].Value;
				this.startInfo.statusText = jsonnode["statusText"].Value;
				this.startInfo.isFindable = jsonnode["isFindable"].AsBool;
				this.startInfo.age = jsonnode["age"].AsInt;
				this.startInfo.ageSecs = jsonnode["ageSecs"].AsInt;
				this.startInfo.homeAreaId = jsonnode["homeAreaId"].Value;
				this.startInfo.isSoftBanned = jsonnode["isSoftBanned"].AsBool;
				this.startInfo.showFlagWarning = jsonnode["showFlagWarning"].AsBool;
				this.startInfo.flagTags = new string[] { jsonnode["flagTags"].Value };
				this.startInfo.areaCount = jsonnode["areaCount"].AsInt;
				this.startInfo.thingTagCount = jsonnode["thingTagCount"].AsInt;
				this.startInfo.allThingsClonable = jsonnode["allThingsClonable"].AsBool;
				this.startInfo.hasEditTools = jsonnode["hasEditTools"].AsBool;
				this.startInfo.hasEditToolsPermanently = jsonnode["hasEditToolsPermanently"].AsBool;
				this.startInfo.editToolsExpiryDate = jsonnode["editToolsExpiryDate"].Value;
				this.startInfo.isInEditToolsTrial = jsonnode["isInEditToolsTrial"].AsBool;
				this.startInfo.wasEditToolsTrialEverActivated = jsonnode["wasEditToolsTrialEverActivated"].AsBool;
				this.startInfo.timesEditToolsPurchased = jsonnode["timesEditToolsPurchased"].AsInt;
				this.startInfo.customSearchWords = jsonnode["customSearchWords"].Value;
				string value = jsonnode["attachments"].Value;
				this.startInfo.attachmentsData = new AttachmentDataSet(value);
				JSONNode jsonnode2 = jsonnode["achievements"];
				for (int i = 0; i < jsonnode2.Count; i++)
				{
					Achievement asInt = (Achievement)jsonnode2[i].AsInt;
					this.startInfo.achievements.Add(asInt);
				}
				if (jsonnode["handColor"] != null)
				{
					this.startInfo.handColor = new Color?(new Color(jsonnode["handColor"]["r"].AsFloat, jsonnode["handColor"]["g"].AsFloat, jsonnode["handColor"]["b"].AsFloat));
				}
			}
			else
			{
				Log.Error("No parsed startinfo data");
			}
		}
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x000C0ADC File Offset: 0x000BEEDC
	private Dictionary<AttachmentPointId, AttachmentData> buildAttachmentsDataFromJSON(string attachmentsDataJSON)
	{
		Dictionary<AttachmentPointId, AttachmentData> dictionary = new Dictionary<AttachmentPointId, AttachmentData>();
		if (!string.IsNullOrEmpty(attachmentsDataJSON))
		{
			JSONNode jsonnode = JSON.Parse(attachmentsDataJSON);
			foreach (string text in jsonnode.Keys)
			{
				int num = int.Parse(text);
				AttachmentPointId attachmentPointId = (AttachmentPointId)num;
				JSONNode jsonnode2 = jsonnode[text];
				string value = jsonnode2["Tid"].Value;
				Vector3 vector = new Vector3(jsonnode2["P"]["x"].AsFloat, jsonnode2["P"]["y"].AsFloat, jsonnode2["P"]["z"].AsFloat);
				Vector3 vector2 = new Vector3(jsonnode2["R"]["x"].AsFloat, jsonnode2["R"]["y"].AsFloat, jsonnode2["R"]["z"].AsFloat);
				dictionary.Add(attachmentPointId, new AttachmentData(value, vector, vector2));
			}
		}
		return dictionary;
	}

	// Token: 0x0400134D RID: 4941
	public StartInfo startInfo;

	// Token: 0x0400134E RID: 4942
	public bool isHardBanned;

	// Token: 0x0400134F RID: 4943
	public string steamErrorCode;
}
