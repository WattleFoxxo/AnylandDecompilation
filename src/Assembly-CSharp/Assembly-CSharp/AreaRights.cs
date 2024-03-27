using System;

// Token: 0x020000DA RID: 218
public class AreaRights
{
	// Token: 0x060006E7 RID: 1767 RVA: 0x0001FDF4 File Offset: 0x0001E1F4
	public void SetAllToNull()
	{
		this.emittedClimbing = null;
		this.emittedTransporting = null;
		this.movingThroughObstacles = null;
		this.visionInObstacles = null;
		this.invisibility = null;
		this.anyPersonSize = null;
		this.highlighting = null;
		this.amplifiedSpeech = null;
		this.anyDestruction = null;
		this.webBrowsing = null;
		this.untargetedAttractThings = null;
		this.slowBuildCreation = null;
	}

	// Token: 0x040004FF RID: 1279
	public bool? emittedClimbing = new bool?(true);

	// Token: 0x04000500 RID: 1280
	public bool? emittedTransporting = new bool?(false);

	// Token: 0x04000501 RID: 1281
	public bool? movingThroughObstacles = new bool?(false);

	// Token: 0x04000502 RID: 1282
	public bool? visionInObstacles = new bool?(true);

	// Token: 0x04000503 RID: 1283
	public bool? invisibility = new bool?(false);

	// Token: 0x04000504 RID: 1284
	public bool? anyPersonSize = new bool?(false);

	// Token: 0x04000505 RID: 1285
	public bool? highlighting = new bool?(true);

	// Token: 0x04000506 RID: 1286
	public bool? amplifiedSpeech = new bool?(true);

	// Token: 0x04000507 RID: 1287
	public bool? anyDestruction = new bool?(false);

	// Token: 0x04000508 RID: 1288
	public bool? webBrowsing = new bool?(true);

	// Token: 0x04000509 RID: 1289
	public bool? untargetedAttractThings = new bool?(true);

	// Token: 0x0400050A RID: 1290
	public bool? slowBuildCreation = new bool?(true);
}
