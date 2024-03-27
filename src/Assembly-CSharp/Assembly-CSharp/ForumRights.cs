using System;

// Token: 0x020001B1 RID: 433
public class ForumRights
{
	// Token: 0x06000D54 RID: 3412 RVA: 0x00077884 File Offset: 0x00075C84
	public ForumRights(ForumData forumData)
	{
		this.isModerator = forumData.user_isModerator;
		int protectionLevel = forumData.protectionLevel;
		if (protectionLevel >= 1)
		{
			this.onlyModsCanAddThreads = true;
		}
		if (protectionLevel >= 2)
		{
			this.onlyModsCanAddComments = true;
		}
		if (protectionLevel >= 3)
		{
			this.onlyModsCanRead = true;
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000D55 RID: 3413 RVA: 0x000778D4 File Offset: 0x00075CD4
	// (set) Token: 0x06000D56 RID: 3414 RVA: 0x000778DC File Offset: 0x00075CDC
	public bool onlyModsCanAddThreads { get; private set; }

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000D57 RID: 3415 RVA: 0x000778E5 File Offset: 0x00075CE5
	// (set) Token: 0x06000D58 RID: 3416 RVA: 0x000778ED File Offset: 0x00075CED
	public bool onlyModsCanAddComments { get; private set; }

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000D59 RID: 3417 RVA: 0x000778F6 File Offset: 0x00075CF6
	// (set) Token: 0x06000D5A RID: 3418 RVA: 0x000778FE File Offset: 0x00075CFE
	public bool onlyModsCanRead { get; private set; }

	// Token: 0x04000EEC RID: 3820
	public bool isModerator;
}
