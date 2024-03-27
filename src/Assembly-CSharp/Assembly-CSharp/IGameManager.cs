using System;

// Token: 0x020001F0 RID: 496
public interface IGameManager
{
	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06001127 RID: 4391
	ManagerStatus status { get; }

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06001128 RID: 4392
	string failMessage { get; }

	// Token: 0x06001129 RID: 4393
	void Startup();
}
