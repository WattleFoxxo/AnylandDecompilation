using System;
using UnityEngine;

// Token: 0x02000235 RID: 565
public class LoadArea_Response : ExtendedServerResponse
{
	// Token: 0x060015EA RID: 5610 RVA: 0x000C02E0 File Offset: 0x000BE6E0
	public LoadArea_Response(WWW www)
		: base(www)
	{
		this.data = new LoadAreaData();
		if (www.error == null)
		{
			this.data = JsonUtility.FromJson<LoadAreaData>(www.text);
			if (!string.IsNullOrEmpty(this.data.environmentChangersJSON))
			{
				this.data.environmentChangers = JsonUtility.FromJson<EnvironmentChangerDataCollection>(this.data.environmentChangersJSON);
			}
			if (this.data.environmentChangers != null && this.data.environmentChangers.environmentChangers != null)
			{
				int count = this.data.environmentChangers.environmentChangers.Count;
			}
			string environmentType = this.data._environmentType;
			if (!string.IsNullOrEmpty(environmentType) && Enum.IsDefined(typeof(EnvironmentType), environmentType))
			{
				this.data.environmentType = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), environmentType);
			}
			else
			{
				this.data.environmentType = EnvironmentType.Default;
			}
			string reasonDenied = this.data._reasonDenied;
			if (!string.IsNullOrEmpty(reasonDenied) && Enum.IsDefined(typeof(TransportDenialReason), this.data._reasonDenied))
			{
				this.data.reasonDenied = (TransportDenialReason)Enum.Parse(typeof(TransportDenialReason), this.data._reasonDenied);
			}
			else
			{
				this.data.reasonDenied = TransportDenialReason.None;
			}
		}
	}

	// Token: 0x04001325 RID: 4901
	public LoadAreaData data;

	// Token: 0x04001326 RID: 4902
	private string placementsJSON;

	// Token: 0x04001327 RID: 4903
	private string environmentChangersJSON;

	// Token: 0x04001328 RID: 4904
	private int serveTime;
}
