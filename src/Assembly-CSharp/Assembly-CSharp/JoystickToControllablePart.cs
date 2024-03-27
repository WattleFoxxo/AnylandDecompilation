using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x020001DE RID: 478
public class JoystickToControllablePart
{
	// Token: 0x06000EF1 RID: 3825 RVA: 0x000835D4 File Offset: 0x000819D4
	public bool IsAllDefault()
	{
		return this.thrust == Vector3.zero && this.rotation == Vector3.zero && this.constantMininumThrust == Vector3.zero && this.constantMininumRotation == Vector3.zero;
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x00083634 File Offset: 0x00081A34
	public void SetFromJson(JSONNode node)
	{
		if (node != null)
		{
			this.thrust = JsonHelper.GetVector3(node["t"]);
			this.rotation = JsonHelper.GetVector3(node["r"]);
			this.constantMininumThrust = JsonHelper.GetVector3(node["mT"]);
			this.constantMininumRotation = JsonHelper.GetVector3(node["mR"]);
		}
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x000836A8 File Offset: 0x00081AA8
	public string GetJson()
	{
		string text = string.Empty;
		text += "\"j\":{";
		text = text + "\"t\":" + JsonHelper.GetJson(this.thrust) + ",";
		text = text + "\"r\":" + JsonHelper.GetJson(this.rotation) + ",";
		text = text + "\"mT\":" + JsonHelper.GetJson(this.constantMininumThrust) + ",";
		text = text + "\"mR\":" + JsonHelper.GetJson(this.constantMininumRotation);
		return text + "}";
	}

	// Token: 0x04000FBC RID: 4028
	public Vector3 thrust = Vector3.zero;

	// Token: 0x04000FBD RID: 4029
	public Vector3 rotation = Vector3.zero;

	// Token: 0x04000FBE RID: 4030
	public Vector3 constantMininumThrust = Vector3.zero;

	// Token: 0x04000FBF RID: 4031
	public Vector3 constantMininumRotation = Vector3.zero;
}
