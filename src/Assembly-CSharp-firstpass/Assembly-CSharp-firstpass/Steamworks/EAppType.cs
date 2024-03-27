using System;

namespace Steamworks
{
	// Token: 0x020002A2 RID: 674
	[Flags]
	public enum EAppType
	{
		// Token: 0x04000B8F RID: 2959
		k_EAppType_Invalid = 0,
		// Token: 0x04000B90 RID: 2960
		k_EAppType_Game = 1,
		// Token: 0x04000B91 RID: 2961
		k_EAppType_Application = 2,
		// Token: 0x04000B92 RID: 2962
		k_EAppType_Tool = 4,
		// Token: 0x04000B93 RID: 2963
		k_EAppType_Demo = 8,
		// Token: 0x04000B94 RID: 2964
		k_EAppType_Media_DEPRECATED = 16,
		// Token: 0x04000B95 RID: 2965
		k_EAppType_DLC = 32,
		// Token: 0x04000B96 RID: 2966
		k_EAppType_Guide = 64,
		// Token: 0x04000B97 RID: 2967
		k_EAppType_Driver = 128,
		// Token: 0x04000B98 RID: 2968
		k_EAppType_Config = 256,
		// Token: 0x04000B99 RID: 2969
		k_EAppType_Hardware = 512,
		// Token: 0x04000B9A RID: 2970
		k_EAppType_Franchise = 1024,
		// Token: 0x04000B9B RID: 2971
		k_EAppType_Video = 2048,
		// Token: 0x04000B9C RID: 2972
		k_EAppType_Plugin = 4096,
		// Token: 0x04000B9D RID: 2973
		k_EAppType_Music = 8192,
		// Token: 0x04000B9E RID: 2974
		k_EAppType_Series = 16384,
		// Token: 0x04000B9F RID: 2975
		k_EAppType_Shortcut = 1073741824,
		// Token: 0x04000BA0 RID: 2976
		k_EAppType_DepotOnly = -2147483647
	}
}
