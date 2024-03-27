using System;

namespace Valve.VR
{
	// Token: 0x0200013C RID: 316
	public enum EVRInitError
	{
		// Token: 0x04000261 RID: 609
		None,
		// Token: 0x04000262 RID: 610
		Unknown,
		// Token: 0x04000263 RID: 611
		Init_InstallationNotFound = 100,
		// Token: 0x04000264 RID: 612
		Init_InstallationCorrupt,
		// Token: 0x04000265 RID: 613
		Init_VRClientDLLNotFound,
		// Token: 0x04000266 RID: 614
		Init_FileNotFound,
		// Token: 0x04000267 RID: 615
		Init_FactoryNotFound,
		// Token: 0x04000268 RID: 616
		Init_InterfaceNotFound,
		// Token: 0x04000269 RID: 617
		Init_InvalidInterface,
		// Token: 0x0400026A RID: 618
		Init_UserConfigDirectoryInvalid,
		// Token: 0x0400026B RID: 619
		Init_HmdNotFound,
		// Token: 0x0400026C RID: 620
		Init_NotInitialized,
		// Token: 0x0400026D RID: 621
		Init_PathRegistryNotFound,
		// Token: 0x0400026E RID: 622
		Init_NoConfigPath,
		// Token: 0x0400026F RID: 623
		Init_NoLogPath,
		// Token: 0x04000270 RID: 624
		Init_PathRegistryNotWritable,
		// Token: 0x04000271 RID: 625
		Init_AppInfoInitFailed,
		// Token: 0x04000272 RID: 626
		Init_Retry,
		// Token: 0x04000273 RID: 627
		Init_InitCanceledByUser,
		// Token: 0x04000274 RID: 628
		Init_AnotherAppLaunching,
		// Token: 0x04000275 RID: 629
		Init_SettingsInitFailed,
		// Token: 0x04000276 RID: 630
		Init_ShuttingDown,
		// Token: 0x04000277 RID: 631
		Init_TooManyObjects,
		// Token: 0x04000278 RID: 632
		Init_NoServerForBackgroundApp,
		// Token: 0x04000279 RID: 633
		Init_NotSupportedWithCompositor,
		// Token: 0x0400027A RID: 634
		Init_NotAvailableToUtilityApps,
		// Token: 0x0400027B RID: 635
		Init_Internal,
		// Token: 0x0400027C RID: 636
		Init_HmdDriverIdIsNone,
		// Token: 0x0400027D RID: 637
		Init_HmdNotFoundPresenceFailed,
		// Token: 0x0400027E RID: 638
		Init_VRMonitorNotFound,
		// Token: 0x0400027F RID: 639
		Init_VRMonitorStartupFailed,
		// Token: 0x04000280 RID: 640
		Init_LowPowerWatchdogNotSupported,
		// Token: 0x04000281 RID: 641
		Init_InvalidApplicationType,
		// Token: 0x04000282 RID: 642
		Init_NotAvailableToWatchdogApps,
		// Token: 0x04000283 RID: 643
		Init_WatchdogDisabledInSettings,
		// Token: 0x04000284 RID: 644
		Driver_Failed = 200,
		// Token: 0x04000285 RID: 645
		Driver_Unknown,
		// Token: 0x04000286 RID: 646
		Driver_HmdUnknown,
		// Token: 0x04000287 RID: 647
		Driver_NotLoaded,
		// Token: 0x04000288 RID: 648
		Driver_RuntimeOutOfDate,
		// Token: 0x04000289 RID: 649
		Driver_HmdInUse,
		// Token: 0x0400028A RID: 650
		Driver_NotCalibrated,
		// Token: 0x0400028B RID: 651
		Driver_CalibrationInvalid,
		// Token: 0x0400028C RID: 652
		Driver_HmdDisplayNotFound,
		// Token: 0x0400028D RID: 653
		Driver_TrackedDeviceInterfaceUnknown,
		// Token: 0x0400028E RID: 654
		Driver_HmdDriverIdOutOfBounds = 211,
		// Token: 0x0400028F RID: 655
		Driver_HmdDisplayMirrored,
		// Token: 0x04000290 RID: 656
		IPC_ServerInitFailed = 300,
		// Token: 0x04000291 RID: 657
		IPC_ConnectFailed,
		// Token: 0x04000292 RID: 658
		IPC_SharedStateInitFailed,
		// Token: 0x04000293 RID: 659
		IPC_CompositorInitFailed,
		// Token: 0x04000294 RID: 660
		IPC_MutexInitFailed,
		// Token: 0x04000295 RID: 661
		IPC_Failed,
		// Token: 0x04000296 RID: 662
		IPC_CompositorConnectFailed,
		// Token: 0x04000297 RID: 663
		IPC_CompositorInvalidConnectResponse,
		// Token: 0x04000298 RID: 664
		IPC_ConnectFailedAfterMultipleAttempts,
		// Token: 0x04000299 RID: 665
		Compositor_Failed = 400,
		// Token: 0x0400029A RID: 666
		Compositor_D3D11HardwareRequired,
		// Token: 0x0400029B RID: 667
		Compositor_FirmwareRequiresUpdate,
		// Token: 0x0400029C RID: 668
		Compositor_OverlayInitFailed,
		// Token: 0x0400029D RID: 669
		Compositor_ScreenshotsInitFailed,
		// Token: 0x0400029E RID: 670
		VendorSpecific_UnableToConnectToOculusRuntime = 1000,
		// Token: 0x0400029F RID: 671
		VendorSpecific_HmdFound_CantOpenDevice = 1101,
		// Token: 0x040002A0 RID: 672
		VendorSpecific_HmdFound_UnableToRequestConfigStart,
		// Token: 0x040002A1 RID: 673
		VendorSpecific_HmdFound_NoStoredConfig,
		// Token: 0x040002A2 RID: 674
		VendorSpecific_HmdFound_ConfigTooBig,
		// Token: 0x040002A3 RID: 675
		VendorSpecific_HmdFound_ConfigTooSmall,
		// Token: 0x040002A4 RID: 676
		VendorSpecific_HmdFound_UnableToInitZLib,
		// Token: 0x040002A5 RID: 677
		VendorSpecific_HmdFound_CantReadFirmwareVersion,
		// Token: 0x040002A6 RID: 678
		VendorSpecific_HmdFound_UnableToSendUserDataStart,
		// Token: 0x040002A7 RID: 679
		VendorSpecific_HmdFound_UnableToGetUserDataStart,
		// Token: 0x040002A8 RID: 680
		VendorSpecific_HmdFound_UnableToGetUserDataNext,
		// Token: 0x040002A9 RID: 681
		VendorSpecific_HmdFound_UserDataAddressRange,
		// Token: 0x040002AA RID: 682
		VendorSpecific_HmdFound_UserDataError,
		// Token: 0x040002AB RID: 683
		VendorSpecific_HmdFound_ConfigFailedSanityCheck,
		// Token: 0x040002AC RID: 684
		Steam_SteamInstallationNotFound = 2000
	}
}
