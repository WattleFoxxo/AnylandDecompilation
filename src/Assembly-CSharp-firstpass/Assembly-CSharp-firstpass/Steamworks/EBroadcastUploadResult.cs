using System;

namespace Steamworks
{
	// Token: 0x020002A9 RID: 681
	public enum EBroadcastUploadResult
	{
		// Token: 0x04000BD4 RID: 3028
		k_EBroadcastUploadResultNone,
		// Token: 0x04000BD5 RID: 3029
		k_EBroadcastUploadResultOK,
		// Token: 0x04000BD6 RID: 3030
		k_EBroadcastUploadResultInitFailed,
		// Token: 0x04000BD7 RID: 3031
		k_EBroadcastUploadResultFrameFailed,
		// Token: 0x04000BD8 RID: 3032
		k_EBroadcastUploadResultTimeout,
		// Token: 0x04000BD9 RID: 3033
		k_EBroadcastUploadResultBandwidthExceeded,
		// Token: 0x04000BDA RID: 3034
		k_EBroadcastUploadResultLowFPS,
		// Token: 0x04000BDB RID: 3035
		k_EBroadcastUploadResultMissingKeyFrames,
		// Token: 0x04000BDC RID: 3036
		k_EBroadcastUploadResultNoConnection,
		// Token: 0x04000BDD RID: 3037
		k_EBroadcastUploadResultRelayFailed,
		// Token: 0x04000BDE RID: 3038
		k_EBroadcastUploadResultSettingsChanged,
		// Token: 0x04000BDF RID: 3039
		k_EBroadcastUploadResultMissingAudio,
		// Token: 0x04000BE0 RID: 3040
		k_EBroadcastUploadResultTooFarBehind,
		// Token: 0x04000BE1 RID: 3041
		k_EBroadcastUploadResultTranscodeBehind
	}
}
