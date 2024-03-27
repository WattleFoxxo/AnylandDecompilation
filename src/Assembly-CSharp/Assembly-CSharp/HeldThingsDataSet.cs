using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x0200015F RID: 351
[Serializable]
public class HeldThingsDataSet
{
	// Token: 0x06000CD7 RID: 3287 RVA: 0x00074EA9 File Offset: 0x000732A9
	public HeldThingsDataSet()
	{
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x00074EB1 File Offset: 0x000732B1
	public HeldThingsDataSet(string heldThingsDataJSON)
	{
		if (string.IsNullOrEmpty(heldThingsDataJSON))
		{
			throw new Exception("HeldThingsDataSet json contrstructor passed empty string");
		}
		this.fromJSON_SimpleJson(heldThingsDataJSON);
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00074ED8 File Offset: 0x000732D8
	public HeldThingsDataSet(GameObject leftHeldThing, GameObject rightHeldThing)
	{
		if (leftHeldThing != null)
		{
			this.leftHeldThingData = new HeldThingData(leftHeldThing.GetComponent<Thing>().thingId, leftHeldThing.transform.localPosition, leftHeldThing.transform.localRotation);
		}
		if (rightHeldThing != null)
		{
			this.rightHeldThingData = new HeldThingData(rightHeldThing.GetComponent<Thing>().thingId, rightHeldThing.transform.localPosition, rightHeldThing.transform.localRotation);
		}
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x00074F5C File Offset: 0x0007335C
	public void fromJSON_SimpleJson(string heldThingsDataJSON)
	{
		JSONNode jsonnode = JSON.Parse(heldThingsDataJSON);
		JSONNode jsonnode2 = jsonnode["l"];
		JSONNode jsonnode3 = jsonnode["r"];
		this.leftHeldThingData = this.GetHeldThingDataFromSimpleJsonNode(jsonnode2);
		this.rightHeldThingData = this.GetHeldThingDataFromSimpleJsonNode(jsonnode3);
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00074FA4 File Offset: 0x000733A4
	private HeldThingData GetHeldThingDataFromSimpleJsonNode(JSONNode simpleJsonNode)
	{
		HeldThingData heldThingData = null;
		if (simpleJsonNode != null)
		{
			string value = simpleJsonNode["Tid"].Value;
			Vector3 vector = new Vector3(simpleJsonNode["P"]["x"].AsFloat, simpleJsonNode["P"]["y"].AsFloat, simpleJsonNode["P"]["z"].AsFloat);
			Quaternion quaternion = new Quaternion(simpleJsonNode["R"]["x"].AsFloat, simpleJsonNode["R"]["y"].AsFloat, simpleJsonNode["R"]["z"].AsFloat, simpleJsonNode["R"]["w"].AsFloat);
			heldThingData = new HeldThingData(value, vector, quaternion);
		}
		return heldThingData;
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x000750A0 File Offset: 0x000734A0
	public string toJSON()
	{
		string text = "{";
		if (this.leftHeldThingData != null)
		{
			text = text + "\"l\":" + JsonUtility.ToJson(this.leftHeldThingData) + ",";
		}
		if (this.rightHeldThingData != null)
		{
			text = text + "\"r\":" + JsonUtility.ToJson(this.rightHeldThingData);
		}
		return text + "}";
	}

	// Token: 0x04000A10 RID: 2576
	public HeldThingData leftHeldThingData;

	// Token: 0x04000A11 RID: 2577
	public HeldThingData rightHeldThingData;

	// Token: 0x04000A12 RID: 2578
	private const string LEFT_KEY = "l";

	// Token: 0x04000A13 RID: 2579
	private const string RIGHT_KEY = "r";
}
