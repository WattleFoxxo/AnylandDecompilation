using System;

// Token: 0x0200005E RID: 94
public class FriendInfo
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000319 RID: 793 RVA: 0x0000C941 File Offset: 0x0000AD41
	// (set) Token: 0x0600031A RID: 794 RVA: 0x0000C949 File Offset: 0x0000AD49
	public string Name { get; protected internal set; }

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600031B RID: 795 RVA: 0x0000C952 File Offset: 0x0000AD52
	// (set) Token: 0x0600031C RID: 796 RVA: 0x0000C95A File Offset: 0x0000AD5A
	public bool IsOnline { get; protected internal set; }

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600031D RID: 797 RVA: 0x0000C963 File Offset: 0x0000AD63
	// (set) Token: 0x0600031E RID: 798 RVA: 0x0000C96B File Offset: 0x0000AD6B
	public string Room { get; protected internal set; }

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600031F RID: 799 RVA: 0x0000C974 File Offset: 0x0000AD74
	public bool IsInRoom
	{
		get
		{
			return this.IsOnline && !string.IsNullOrEmpty(this.Room);
		}
	}

	// Token: 0x06000320 RID: 800 RVA: 0x0000C994 File Offset: 0x0000AD94
	public override string ToString()
	{
		return string.Format("{0}\t is: {1}", this.Name, this.IsOnline ? ((!this.IsInRoom) ? "on master" : "playing") : "offline");
	}
}
