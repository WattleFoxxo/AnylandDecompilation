using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class RidingBeaconCache
{
	// Token: 0x06000DE0 RID: 3552 RVA: 0x0007C4B0 File Offset: 0x0007A8B0
	public string GetJson()
	{
		return string.Concat(new string[]
		{
			"{t:",
			JsonHelper.GetJson(this.thingAttachedToSpecifierType.ToString()),
			",i:",
			JsonHelper.GetJson(this.thingAttachedToSpecifierId),
			",p:",
			JsonHelper.GetJson(this.beaconPosition),
			",pO:",
			JsonHelper.GetJson(this.beaconPositionOffset),
			",rO:",
			JsonHelper.GetJson(this.beaconRotationOffset),
			",}"
		});
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x0007C54C File Offset: 0x0007A94C
	public void InitFromJson(string json)
	{
		JSONNode jsonnode = JSON.Parse(json);
		this.thingAttachedToSpecifierType = (ThingSpecifierType)Enum.Parse(typeof(ThingSpecifierType), jsonnode["t"]);
		this.thingAttachedToSpecifierId = jsonnode["i"];
		this.beaconPosition = JsonHelper.GetVector3(jsonnode["p"]);
		this.beaconPositionOffset = JsonHelper.GetVector3(jsonnode["pO"]);
		this.beaconRotationOffset = JsonHelper.GetVector3(jsonnode["rO"]);
		Log.Debug("RidingBeaconCache InitFromJson");
	}

	// Token: 0x04000F54 RID: 3924
	public ThingSpecifierType thingAttachedToSpecifierType;

	// Token: 0x04000F55 RID: 3925
	public string thingAttachedToSpecifierId = string.Empty;

	// Token: 0x04000F56 RID: 3926
	public Vector3 beaconPosition = Vector3.zero;

	// Token: 0x04000F57 RID: 3927
	public Vector3 beaconPositionOffset = Vector3.zero;

	// Token: 0x04000F58 RID: 3928
	public Vector3 beaconRotationOffset = Vector3.zero;
}
