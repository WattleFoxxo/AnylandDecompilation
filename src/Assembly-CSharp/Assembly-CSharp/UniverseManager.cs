using System;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class UniverseManager : MonoBehaviour, IGameManager
{
	// Token: 0x1700024A RID: 586
	// (get) Token: 0x060013F2 RID: 5106 RVA: 0x000B43A9 File Offset: 0x000B27A9
	// (set) Token: 0x060013F3 RID: 5107 RVA: 0x000B43B1 File Offset: 0x000B27B1
	public ManagerStatus status { get; private set; }

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x060013F4 RID: 5108 RVA: 0x000B43BA File Offset: 0x000B27BA
	// (set) Token: 0x060013F5 RID: 5109 RVA: 0x000B43C2 File Offset: 0x000B27C2
	public string failMessage { get; private set; }

	// Token: 0x060013F6 RID: 5110 RVA: 0x000B43CB File Offset: 0x000B27CB
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}
}
