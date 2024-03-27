using System;

// Token: 0x02000150 RID: 336
[Serializable]
public class AreaVideoInfo
{
	// Token: 0x06000CB9 RID: 3257 RVA: 0x00074A51 File Offset: 0x00072E51
	public AreaVideoInfo(string _urlId, string _videoScreenThingPlacementId, string _title, string _startedByPersonId, string _startedByPersonName, float _volume, double _photonTime)
	{
		this.urlId = _urlId;
		this.videoScreenThingPlacementId = _videoScreenThingPlacementId;
		this.title = _title;
		this.startedByPersonId = _startedByPersonId;
		this.startedByPersonName = _startedByPersonName;
		this.volume = _volume;
		this.photonTime = _photonTime;
	}

	// Token: 0x040009B4 RID: 2484
	public string urlId;

	// Token: 0x040009B5 RID: 2485
	public string videoScreenThingPlacementId;

	// Token: 0x040009B6 RID: 2486
	public string title;

	// Token: 0x040009B7 RID: 2487
	public string startedByPersonId;

	// Token: 0x040009B8 RID: 2488
	public string startedByPersonName;

	// Token: 0x040009B9 RID: 2489
	public float volume;

	// Token: 0x040009BA RID: 2490
	public double photonTime;
}
