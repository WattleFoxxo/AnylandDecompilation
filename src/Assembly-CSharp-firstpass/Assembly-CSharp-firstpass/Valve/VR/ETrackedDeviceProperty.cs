using System;

namespace Valve.VR
{
	// Token: 0x0200012D RID: 301
	public enum ETrackedDeviceProperty
	{
		// Token: 0x04000137 RID: 311
		Prop_TrackingSystemName_String = 1000,
		// Token: 0x04000138 RID: 312
		Prop_ModelNumber_String,
		// Token: 0x04000139 RID: 313
		Prop_SerialNumber_String,
		// Token: 0x0400013A RID: 314
		Prop_RenderModelName_String,
		// Token: 0x0400013B RID: 315
		Prop_WillDriftInYaw_Bool,
		// Token: 0x0400013C RID: 316
		Prop_ManufacturerName_String,
		// Token: 0x0400013D RID: 317
		Prop_TrackingFirmwareVersion_String,
		// Token: 0x0400013E RID: 318
		Prop_HardwareRevision_String,
		// Token: 0x0400013F RID: 319
		Prop_AllWirelessDongleDescriptions_String,
		// Token: 0x04000140 RID: 320
		Prop_ConnectedWirelessDongle_String,
		// Token: 0x04000141 RID: 321
		Prop_DeviceIsWireless_Bool,
		// Token: 0x04000142 RID: 322
		Prop_DeviceIsCharging_Bool,
		// Token: 0x04000143 RID: 323
		Prop_DeviceBatteryPercentage_Float,
		// Token: 0x04000144 RID: 324
		Prop_StatusDisplayTransform_Matrix34,
		// Token: 0x04000145 RID: 325
		Prop_Firmware_UpdateAvailable_Bool,
		// Token: 0x04000146 RID: 326
		Prop_Firmware_ManualUpdate_Bool,
		// Token: 0x04000147 RID: 327
		Prop_Firmware_ManualUpdateURL_String,
		// Token: 0x04000148 RID: 328
		Prop_HardwareRevision_Uint64,
		// Token: 0x04000149 RID: 329
		Prop_FirmwareVersion_Uint64,
		// Token: 0x0400014A RID: 330
		Prop_FPGAVersion_Uint64,
		// Token: 0x0400014B RID: 331
		Prop_VRCVersion_Uint64,
		// Token: 0x0400014C RID: 332
		Prop_RadioVersion_Uint64,
		// Token: 0x0400014D RID: 333
		Prop_DongleVersion_Uint64,
		// Token: 0x0400014E RID: 334
		Prop_BlockServerShutdown_Bool,
		// Token: 0x0400014F RID: 335
		Prop_CanUnifyCoordinateSystemWithHmd_Bool,
		// Token: 0x04000150 RID: 336
		Prop_ContainsProximitySensor_Bool,
		// Token: 0x04000151 RID: 337
		Prop_DeviceProvidesBatteryStatus_Bool,
		// Token: 0x04000152 RID: 338
		Prop_DeviceCanPowerOff_Bool,
		// Token: 0x04000153 RID: 339
		Prop_Firmware_ProgrammingTarget_String,
		// Token: 0x04000154 RID: 340
		Prop_DeviceClass_Int32,
		// Token: 0x04000155 RID: 341
		Prop_HasCamera_Bool,
		// Token: 0x04000156 RID: 342
		Prop_DriverVersion_String,
		// Token: 0x04000157 RID: 343
		Prop_Firmware_ForceUpdateRequired_Bool,
		// Token: 0x04000158 RID: 344
		Prop_ReportsTimeSinceVSync_Bool = 2000,
		// Token: 0x04000159 RID: 345
		Prop_SecondsFromVsyncToPhotons_Float,
		// Token: 0x0400015A RID: 346
		Prop_DisplayFrequency_Float,
		// Token: 0x0400015B RID: 347
		Prop_UserIpdMeters_Float,
		// Token: 0x0400015C RID: 348
		Prop_CurrentUniverseId_Uint64,
		// Token: 0x0400015D RID: 349
		Prop_PreviousUniverseId_Uint64,
		// Token: 0x0400015E RID: 350
		Prop_DisplayFirmwareVersion_Uint64,
		// Token: 0x0400015F RID: 351
		Prop_IsOnDesktop_Bool,
		// Token: 0x04000160 RID: 352
		Prop_DisplayMCType_Int32,
		// Token: 0x04000161 RID: 353
		Prop_DisplayMCOffset_Float,
		// Token: 0x04000162 RID: 354
		Prop_DisplayMCScale_Float,
		// Token: 0x04000163 RID: 355
		Prop_EdidVendorID_Int32,
		// Token: 0x04000164 RID: 356
		Prop_DisplayMCImageLeft_String,
		// Token: 0x04000165 RID: 357
		Prop_DisplayMCImageRight_String,
		// Token: 0x04000166 RID: 358
		Prop_DisplayGCBlackClamp_Float,
		// Token: 0x04000167 RID: 359
		Prop_EdidProductID_Int32,
		// Token: 0x04000168 RID: 360
		Prop_CameraToHeadTransform_Matrix34,
		// Token: 0x04000169 RID: 361
		Prop_DisplayGCType_Int32,
		// Token: 0x0400016A RID: 362
		Prop_DisplayGCOffset_Float,
		// Token: 0x0400016B RID: 363
		Prop_DisplayGCScale_Float,
		// Token: 0x0400016C RID: 364
		Prop_DisplayGCPrescale_Float,
		// Token: 0x0400016D RID: 365
		Prop_DisplayGCImage_String,
		// Token: 0x0400016E RID: 366
		Prop_LensCenterLeftU_Float,
		// Token: 0x0400016F RID: 367
		Prop_LensCenterLeftV_Float,
		// Token: 0x04000170 RID: 368
		Prop_LensCenterRightU_Float,
		// Token: 0x04000171 RID: 369
		Prop_LensCenterRightV_Float,
		// Token: 0x04000172 RID: 370
		Prop_UserHeadToEyeDepthMeters_Float,
		// Token: 0x04000173 RID: 371
		Prop_CameraFirmwareVersion_Uint64,
		// Token: 0x04000174 RID: 372
		Prop_CameraFirmwareDescription_String,
		// Token: 0x04000175 RID: 373
		Prop_DisplayFPGAVersion_Uint64,
		// Token: 0x04000176 RID: 374
		Prop_DisplayBootloaderVersion_Uint64,
		// Token: 0x04000177 RID: 375
		Prop_DisplayHardwareVersion_Uint64,
		// Token: 0x04000178 RID: 376
		Prop_AudioFirmwareVersion_Uint64,
		// Token: 0x04000179 RID: 377
		Prop_CameraCompatibilityMode_Int32,
		// Token: 0x0400017A RID: 378
		Prop_ScreenshotHorizontalFieldOfViewDegrees_Float,
		// Token: 0x0400017B RID: 379
		Prop_ScreenshotVerticalFieldOfViewDegrees_Float,
		// Token: 0x0400017C RID: 380
		Prop_DisplaySuppressed_Bool,
		// Token: 0x0400017D RID: 381
		Prop_AttachedDeviceId_String = 3000,
		// Token: 0x0400017E RID: 382
		Prop_SupportedButtons_Uint64,
		// Token: 0x0400017F RID: 383
		Prop_Axis0Type_Int32,
		// Token: 0x04000180 RID: 384
		Prop_Axis1Type_Int32,
		// Token: 0x04000181 RID: 385
		Prop_Axis2Type_Int32,
		// Token: 0x04000182 RID: 386
		Prop_Axis3Type_Int32,
		// Token: 0x04000183 RID: 387
		Prop_Axis4Type_Int32,
		// Token: 0x04000184 RID: 388
		Prop_ControllerRoleHint_Int32,
		// Token: 0x04000185 RID: 389
		Prop_FieldOfViewLeftDegrees_Float = 4000,
		// Token: 0x04000186 RID: 390
		Prop_FieldOfViewRightDegrees_Float,
		// Token: 0x04000187 RID: 391
		Prop_FieldOfViewTopDegrees_Float,
		// Token: 0x04000188 RID: 392
		Prop_FieldOfViewBottomDegrees_Float,
		// Token: 0x04000189 RID: 393
		Prop_TrackingRangeMinimumMeters_Float,
		// Token: 0x0400018A RID: 394
		Prop_TrackingRangeMaximumMeters_Float,
		// Token: 0x0400018B RID: 395
		Prop_ModeLabel_String,
		// Token: 0x0400018C RID: 396
		Prop_VendorSpecific_Reserved_Start = 10000,
		// Token: 0x0400018D RID: 397
		Prop_VendorSpecific_Reserved_End = 10999
	}
}
