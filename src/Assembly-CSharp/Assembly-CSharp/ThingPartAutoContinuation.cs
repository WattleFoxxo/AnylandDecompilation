using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000278 RID: 632
public class ThingPartAutoContinuation
{
	// Token: 0x060017FC RID: 6140 RVA: 0x000DC34C File Offset: 0x000DA74C
	public void ApplyRandomization(Transform transform)
	{
		if (this.positionRandomization != Vector3.zero)
		{
			transform.localPosition += this.GetRandomizationChange(this.positionRandomization, 0.05f);
		}
		if (this.rotationRandomization != Vector3.zero)
		{
			transform.localEulerAngles += this.GetRandomizationChange(this.rotationRandomization, 0.1f);
		}
		if (this.scaleRandomization != Vector3.zero)
		{
			Vector3 vector = transform.localScale;
			vector += this.GetRandomizationChange(this.scaleRandomization, 0.0275f);
			if (vector.x < 0.0075f)
			{
				vector.x = 0.0075f;
			}
			if (vector.y < 0.0075f)
			{
				vector.y = 0.0075f;
			}
			if (vector.z < 0.0075f)
			{
				vector.z = 0.0075f;
			}
			transform.localScale = vector;
		}
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x000DC45C File Offset: 0x000DA85C
	private Vector3 GetRandomizationChange(Vector3 randomization, float factor)
	{
		float growingPower = this.GetGrowingPower(randomization.x * factor);
		float growingPower2 = this.GetGrowingPower(randomization.y * factor);
		float growingPower3 = this.GetGrowingPower(randomization.z * factor);
		randomization = new Vector3(global::UnityEngine.Random.Range(-growingPower, growingPower), global::UnityEngine.Random.Range(-growingPower2, growingPower2), global::UnityEngine.Random.Range(-growingPower3, growingPower3));
		return randomization;
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x000DC4BC File Offset: 0x000DA8BC
	private float GetGrowingPower(float value)
	{
		if (value > 1f)
		{
			float num = (value - 1f) * 1.5f;
			value = Mathf.Pow(value, num);
		}
		return value;
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x000DC4EC File Offset: 0x000DA8EC
	public string GetJson()
	{
		string text = string.Empty;
		if (this.fromPart != null && this.count > 0)
		{
			ThingPart component = this.fromPart.GetComponent<ThingPart>();
			if (component != null && !string.IsNullOrEmpty(component.guid))
			{
				text += "\"ac\":{";
				text = text + "\"id\":" + JsonHelper.GetJson(component.guid);
				text = text + ",\"c\":" + this.count;
				if (this.waves >= 1)
				{
					text = text + ",\"w\":" + this.waves;
				}
				if (this.positionRandomization != Vector3.zero)
				{
					text = text + ",\"rp\":" + JsonHelper.GetJson(this.positionRandomization);
				}
				if (this.rotationRandomization != Vector3.zero)
				{
					text = text + ",\"rr\":" + JsonHelper.GetJson(this.rotationRandomization);
				}
				if (this.scaleRandomization != Vector3.zero)
				{
					text = text + ",\"rs\":" + JsonHelper.GetJson(this.scaleRandomization);
				}
				text += "}";
			}
		}
		return text;
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x000DC634 File Offset: 0x000DAA34
	public void SetByJson(JSONNode node)
	{
		this.fromPartGuid = node["id"];
		this.count = node["c"].AsInt;
		if (node["w"] != null)
		{
			this.waves = node["w"].AsInt;
		}
		if (node["rp"] != null)
		{
			this.positionRandomization = JsonHelper.GetVector3(node["rp"]);
		}
		if (node["rr"] != null)
		{
			this.rotationRandomization = JsonHelper.GetVector3(node["rr"]);
		}
		if (node["rs"] != null)
		{
			this.scaleRandomization = JsonHelper.GetVector3(node["rs"]);
		}
	}

	// Token: 0x0400165B RID: 5723
	public GameObject fromPart;

	// Token: 0x0400165C RID: 5724
	public string fromPartGuid;

	// Token: 0x0400165D RID: 5725
	public Material fromPartMaterial;

	// Token: 0x0400165E RID: 5726
	public int count;

	// Token: 0x0400165F RID: 5727
	public GameObject[] assignedContinuationParts;

	// Token: 0x04001660 RID: 5728
	public Renderer[] assignedContinuationPartsRenderers;

	// Token: 0x04001661 RID: 5729
	public int waves;

	// Token: 0x04001662 RID: 5730
	public Vector3 positionRandomization = Vector3.zero;

	// Token: 0x04001663 RID: 5731
	public Vector3 rotationRandomization = Vector3.zero;

	// Token: 0x04001664 RID: 5732
	public Vector3 scaleRandomization = Vector3.zero;

	// Token: 0x04001665 RID: 5733
	public bool includeColor;
}
