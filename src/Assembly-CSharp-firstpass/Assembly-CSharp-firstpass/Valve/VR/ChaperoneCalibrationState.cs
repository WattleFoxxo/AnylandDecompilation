using System;

namespace Valve.VR
{
	// Token: 0x02000144 RID: 324
	public enum ChaperoneCalibrationState
	{
		// Token: 0x040002F8 RID: 760
		OK = 1,
		// Token: 0x040002F9 RID: 761
		Warning = 100,
		// Token: 0x040002FA RID: 762
		Warning_BaseStationMayHaveMoved,
		// Token: 0x040002FB RID: 763
		Warning_BaseStationRemoved,
		// Token: 0x040002FC RID: 764
		Warning_SeatedBoundsInvalid,
		// Token: 0x040002FD RID: 765
		Error = 200,
		// Token: 0x040002FE RID: 766
		Error_BaseStationUninitalized,
		// Token: 0x040002FF RID: 767
		Error_BaseStationConflict,
		// Token: 0x04000300 RID: 768
		Error_PlayAreaInvalid,
		// Token: 0x04000301 RID: 769
		Error_CollisionBoundsInvalid
	}
}
