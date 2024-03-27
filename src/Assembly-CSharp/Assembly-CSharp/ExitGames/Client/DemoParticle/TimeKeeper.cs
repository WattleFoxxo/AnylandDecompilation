using System;

namespace ExitGames.Client.DemoParticle
{
	// Token: 0x020000D6 RID: 214
	public class TimeKeeper
	{
		// Token: 0x060006D0 RID: 1744 RVA: 0x0001FAB9 File Offset: 0x0001DEB9
		public TimeKeeper(int interval)
		{
			this.IsEnabled = true;
			this.Interval = interval;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0001FADA File Offset: 0x0001DEDA
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x0001FAE2 File Offset: 0x0001DEE2
		public int Interval { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001FAEB File Offset: 0x0001DEEB
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x0001FAF3 File Offset: 0x0001DEF3
		public bool IsEnabled { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001FAFC File Offset: 0x0001DEFC
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x0001FB2E File Offset: 0x0001DF2E
		public bool ShouldExecute
		{
			get
			{
				return this.IsEnabled && (this.shouldExecute || Environment.TickCount - this.lastExecutionTime > this.Interval);
			}
			set
			{
				this.shouldExecute = value;
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001FB37 File Offset: 0x0001DF37
		public void Reset()
		{
			this.shouldExecute = false;
			this.lastExecutionTime = Environment.TickCount;
		}

		// Token: 0x040004F0 RID: 1264
		private int lastExecutionTime = Environment.TickCount;

		// Token: 0x040004F1 RID: 1265
		private bool shouldExecute;
	}
}
