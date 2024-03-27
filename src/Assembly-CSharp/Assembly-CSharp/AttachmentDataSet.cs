using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000152 RID: 338
[Serializable]
public class AttachmentDataSet : Dictionary<AttachmentPointId, AttachmentData>
{
	// Token: 0x06000CBF RID: 3263 RVA: 0x00074ACB File Offset: 0x00072ECB
	public AttachmentDataSet()
	{
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x00074AD3 File Offset: 0x00072ED3
	public AttachmentDataSet(string attachmentsDataJSON)
	{
		if (!string.IsNullOrEmpty(attachmentsDataJSON))
		{
			this.fromJSON_SimpleJson(attachmentsDataJSON);
		}
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00074AF0 File Offset: 0x00072EF0
	public void fromJSON_SimpleJson(string attachmentsDataJSON)
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
			base.Add(attachmentPointId, new AttachmentData(value, vector, vector2));
		}
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x00074C40 File Offset: 0x00073040
	public string toJSON()
	{
		bool flag = true;
		string text = "{";
		foreach (KeyValuePair<AttachmentPointId, AttachmentData> keyValuePair in this)
		{
			if (!flag)
			{
				text += ",";
			}
			else
			{
				flag = false;
			}
			text = text + "\"" + ((int)keyValuePair.Key).ToString() + "\"";
			text += ":";
			text += JsonUtility.ToJson(keyValuePair.Value);
		}
		text += "}";
		return text;
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x00074D04 File Offset: 0x00073104
	public void UpdateWithNewData(AttachmentPointId attachmentPointId, AttachmentData attachmentData)
	{
		if (attachmentData == null)
		{
			base.Remove(attachmentPointId);
		}
		else
		{
			base[attachmentPointId] = attachmentData;
		}
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x00074D24 File Offset: 0x00073124
	public void LogContents()
	{
		foreach (KeyValuePair<AttachmentPointId, AttachmentData> keyValuePair in this)
		{
			AttachmentData value = keyValuePair.Value;
			Log.Info(keyValuePair.Key + " : " + value.Tid, false);
		}
	}
}
