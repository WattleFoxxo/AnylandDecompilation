using System;

// Token: 0x020001C3 RID: 451
public class Ping
{
	// Token: 0x06000DD2 RID: 3538 RVA: 0x0007C385 File Offset: 0x0007A785
	public Ping(string _originPersonId, string _originPersonName, string _originAreaName)
	{
		this.originPersonId = _originPersonId;
		this.originPersonName = _originPersonName;
		this.originAreaName = _originAreaName;
		this.receivedOn = DateTime.Now;
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x0007C3AD File Offset: 0x0007A7AD
	// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x0007C3B5 File Offset: 0x0007A7B5
	public string originPersonId { get; private set; }

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0007C3BE File Offset: 0x0007A7BE
	// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x0007C3C6 File Offset: 0x0007A7C6
	public string originPersonName { get; private set; }

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x0007C3CF File Offset: 0x0007A7CF
	// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x0007C3D7 File Offset: 0x0007A7D7
	public string originAreaName { get; private set; }

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0007C3E0 File Offset: 0x0007A7E0
	// (set) Token: 0x06000DDA RID: 3546 RVA: 0x0007C3E8 File Offset: 0x0007A7E8
	public DateTime receivedOn { get; private set; }
}
