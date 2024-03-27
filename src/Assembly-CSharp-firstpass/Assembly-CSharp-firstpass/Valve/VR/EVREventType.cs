using System;

namespace Valve.VR
{
	// Token: 0x02000131 RID: 305
	public enum EVREventType
	{
		// Token: 0x040001A8 RID: 424
		VREvent_None,
		// Token: 0x040001A9 RID: 425
		VREvent_TrackedDeviceActivated = 100,
		// Token: 0x040001AA RID: 426
		VREvent_TrackedDeviceDeactivated,
		// Token: 0x040001AB RID: 427
		VREvent_TrackedDeviceUpdated,
		// Token: 0x040001AC RID: 428
		VREvent_TrackedDeviceUserInteractionStarted,
		// Token: 0x040001AD RID: 429
		VREvent_TrackedDeviceUserInteractionEnded,
		// Token: 0x040001AE RID: 430
		VREvent_IpdChanged,
		// Token: 0x040001AF RID: 431
		VREvent_EnterStandbyMode,
		// Token: 0x040001B0 RID: 432
		VREvent_LeaveStandbyMode,
		// Token: 0x040001B1 RID: 433
		VREvent_TrackedDeviceRoleChanged,
		// Token: 0x040001B2 RID: 434
		VREvent_WatchdogWakeUpRequested,
		// Token: 0x040001B3 RID: 435
		VREvent_ButtonPress = 200,
		// Token: 0x040001B4 RID: 436
		VREvent_ButtonUnpress,
		// Token: 0x040001B5 RID: 437
		VREvent_ButtonTouch,
		// Token: 0x040001B6 RID: 438
		VREvent_ButtonUntouch,
		// Token: 0x040001B7 RID: 439
		VREvent_MouseMove = 300,
		// Token: 0x040001B8 RID: 440
		VREvent_MouseButtonDown,
		// Token: 0x040001B9 RID: 441
		VREvent_MouseButtonUp,
		// Token: 0x040001BA RID: 442
		VREvent_FocusEnter,
		// Token: 0x040001BB RID: 443
		VREvent_FocusLeave,
		// Token: 0x040001BC RID: 444
		VREvent_Scroll,
		// Token: 0x040001BD RID: 445
		VREvent_TouchPadMove,
		// Token: 0x040001BE RID: 446
		VREvent_OverlayFocusChanged,
		// Token: 0x040001BF RID: 447
		VREvent_InputFocusCaptured = 400,
		// Token: 0x040001C0 RID: 448
		VREvent_InputFocusReleased,
		// Token: 0x040001C1 RID: 449
		VREvent_SceneFocusLost,
		// Token: 0x040001C2 RID: 450
		VREvent_SceneFocusGained,
		// Token: 0x040001C3 RID: 451
		VREvent_SceneApplicationChanged,
		// Token: 0x040001C4 RID: 452
		VREvent_SceneFocusChanged,
		// Token: 0x040001C5 RID: 453
		VREvent_InputFocusChanged,
		// Token: 0x040001C6 RID: 454
		VREvent_SceneApplicationSecondaryRenderingStarted,
		// Token: 0x040001C7 RID: 455
		VREvent_HideRenderModels = 410,
		// Token: 0x040001C8 RID: 456
		VREvent_ShowRenderModels,
		// Token: 0x040001C9 RID: 457
		VREvent_OverlayShown = 500,
		// Token: 0x040001CA RID: 458
		VREvent_OverlayHidden,
		// Token: 0x040001CB RID: 459
		VREvent_DashboardActivated,
		// Token: 0x040001CC RID: 460
		VREvent_DashboardDeactivated,
		// Token: 0x040001CD RID: 461
		VREvent_DashboardThumbSelected,
		// Token: 0x040001CE RID: 462
		VREvent_DashboardRequested,
		// Token: 0x040001CF RID: 463
		VREvent_ResetDashboard,
		// Token: 0x040001D0 RID: 464
		VREvent_RenderToast,
		// Token: 0x040001D1 RID: 465
		VREvent_ImageLoaded,
		// Token: 0x040001D2 RID: 466
		VREvent_ShowKeyboard,
		// Token: 0x040001D3 RID: 467
		VREvent_HideKeyboard,
		// Token: 0x040001D4 RID: 468
		VREvent_OverlayGamepadFocusGained,
		// Token: 0x040001D5 RID: 469
		VREvent_OverlayGamepadFocusLost,
		// Token: 0x040001D6 RID: 470
		VREvent_OverlaySharedTextureChanged,
		// Token: 0x040001D7 RID: 471
		VREvent_DashboardGuideButtonDown,
		// Token: 0x040001D8 RID: 472
		VREvent_DashboardGuideButtonUp,
		// Token: 0x040001D9 RID: 473
		VREvent_ScreenshotTriggered,
		// Token: 0x040001DA RID: 474
		VREvent_ImageFailed,
		// Token: 0x040001DB RID: 475
		VREvent_RequestScreenshot = 520,
		// Token: 0x040001DC RID: 476
		VREvent_ScreenshotTaken,
		// Token: 0x040001DD RID: 477
		VREvent_ScreenshotFailed,
		// Token: 0x040001DE RID: 478
		VREvent_SubmitScreenshotToDashboard,
		// Token: 0x040001DF RID: 479
		VREvent_ScreenshotProgressToDashboard,
		// Token: 0x040001E0 RID: 480
		VREvent_Notification_Shown = 600,
		// Token: 0x040001E1 RID: 481
		VREvent_Notification_Hidden,
		// Token: 0x040001E2 RID: 482
		VREvent_Notification_BeginInteraction,
		// Token: 0x040001E3 RID: 483
		VREvent_Notification_Destroyed,
		// Token: 0x040001E4 RID: 484
		VREvent_Quit = 700,
		// Token: 0x040001E5 RID: 485
		VREvent_ProcessQuit,
		// Token: 0x040001E6 RID: 486
		VREvent_QuitAborted_UserPrompt,
		// Token: 0x040001E7 RID: 487
		VREvent_QuitAcknowledged,
		// Token: 0x040001E8 RID: 488
		VREvent_DriverRequestedQuit,
		// Token: 0x040001E9 RID: 489
		VREvent_ChaperoneDataHasChanged = 800,
		// Token: 0x040001EA RID: 490
		VREvent_ChaperoneUniverseHasChanged,
		// Token: 0x040001EB RID: 491
		VREvent_ChaperoneTempDataHasChanged,
		// Token: 0x040001EC RID: 492
		VREvent_ChaperoneSettingsHaveChanged,
		// Token: 0x040001ED RID: 493
		VREvent_SeatedZeroPoseReset,
		// Token: 0x040001EE RID: 494
		VREvent_AudioSettingsHaveChanged = 820,
		// Token: 0x040001EF RID: 495
		VREvent_BackgroundSettingHasChanged = 850,
		// Token: 0x040001F0 RID: 496
		VREvent_CameraSettingsHaveChanged,
		// Token: 0x040001F1 RID: 497
		VREvent_ReprojectionSettingHasChanged,
		// Token: 0x040001F2 RID: 498
		VREvent_ModelSkinSettingsHaveChanged,
		// Token: 0x040001F3 RID: 499
		VREvent_EnvironmentSettingsHaveChanged,
		// Token: 0x040001F4 RID: 500
		VREvent_PowerSettingsHaveChanged,
		// Token: 0x040001F5 RID: 501
		VREvent_StatusUpdate = 900,
		// Token: 0x040001F6 RID: 502
		VREvent_MCImageUpdated = 1000,
		// Token: 0x040001F7 RID: 503
		VREvent_FirmwareUpdateStarted = 1100,
		// Token: 0x040001F8 RID: 504
		VREvent_FirmwareUpdateFinished,
		// Token: 0x040001F9 RID: 505
		VREvent_KeyboardClosed = 1200,
		// Token: 0x040001FA RID: 506
		VREvent_KeyboardCharInput,
		// Token: 0x040001FB RID: 507
		VREvent_KeyboardDone,
		// Token: 0x040001FC RID: 508
		VREvent_ApplicationTransitionStarted = 1300,
		// Token: 0x040001FD RID: 509
		VREvent_ApplicationTransitionAborted,
		// Token: 0x040001FE RID: 510
		VREvent_ApplicationTransitionNewAppStarted,
		// Token: 0x040001FF RID: 511
		VREvent_ApplicationListUpdated,
		// Token: 0x04000200 RID: 512
		VREvent_ApplicationMimeTypeLoad,
		// Token: 0x04000201 RID: 513
		VREvent_Compositor_MirrorWindowShown = 1400,
		// Token: 0x04000202 RID: 514
		VREvent_Compositor_MirrorWindowHidden,
		// Token: 0x04000203 RID: 515
		VREvent_Compositor_ChaperoneBoundsShown = 1410,
		// Token: 0x04000204 RID: 516
		VREvent_Compositor_ChaperoneBoundsHidden,
		// Token: 0x04000205 RID: 517
		VREvent_TrackedCamera_StartVideoStream = 1500,
		// Token: 0x04000206 RID: 518
		VREvent_TrackedCamera_StopVideoStream,
		// Token: 0x04000207 RID: 519
		VREvent_TrackedCamera_PauseVideoStream,
		// Token: 0x04000208 RID: 520
		VREvent_TrackedCamera_ResumeVideoStream,
		// Token: 0x04000209 RID: 521
		VREvent_PerformanceTest_EnableCapture = 1600,
		// Token: 0x0400020A RID: 522
		VREvent_PerformanceTest_DisableCapture,
		// Token: 0x0400020B RID: 523
		VREvent_PerformanceTest_FidelityLevel,
		// Token: 0x0400020C RID: 524
		VREvent_VendorSpecific_Reserved_Start = 10000,
		// Token: 0x0400020D RID: 525
		VREvent_VendorSpecific_Reserved_End = 19999
	}
}
