using System;

namespace Steamworks
{
	// Token: 0x02000299 RID: 665
	public enum EResult
	{
		// Token: 0x04000ACC RID: 2764
		k_EResultOK = 1,
		// Token: 0x04000ACD RID: 2765
		k_EResultFail,
		// Token: 0x04000ACE RID: 2766
		k_EResultNoConnection,
		// Token: 0x04000ACF RID: 2767
		k_EResultInvalidPassword = 5,
		// Token: 0x04000AD0 RID: 2768
		k_EResultLoggedInElsewhere,
		// Token: 0x04000AD1 RID: 2769
		k_EResultInvalidProtocolVer,
		// Token: 0x04000AD2 RID: 2770
		k_EResultInvalidParam,
		// Token: 0x04000AD3 RID: 2771
		k_EResultFileNotFound,
		// Token: 0x04000AD4 RID: 2772
		k_EResultBusy,
		// Token: 0x04000AD5 RID: 2773
		k_EResultInvalidState,
		// Token: 0x04000AD6 RID: 2774
		k_EResultInvalidName,
		// Token: 0x04000AD7 RID: 2775
		k_EResultInvalidEmail,
		// Token: 0x04000AD8 RID: 2776
		k_EResultDuplicateName,
		// Token: 0x04000AD9 RID: 2777
		k_EResultAccessDenied,
		// Token: 0x04000ADA RID: 2778
		k_EResultTimeout,
		// Token: 0x04000ADB RID: 2779
		k_EResultBanned,
		// Token: 0x04000ADC RID: 2780
		k_EResultAccountNotFound,
		// Token: 0x04000ADD RID: 2781
		k_EResultInvalidSteamID,
		// Token: 0x04000ADE RID: 2782
		k_EResultServiceUnavailable,
		// Token: 0x04000ADF RID: 2783
		k_EResultNotLoggedOn,
		// Token: 0x04000AE0 RID: 2784
		k_EResultPending,
		// Token: 0x04000AE1 RID: 2785
		k_EResultEncryptionFailure,
		// Token: 0x04000AE2 RID: 2786
		k_EResultInsufficientPrivilege,
		// Token: 0x04000AE3 RID: 2787
		k_EResultLimitExceeded,
		// Token: 0x04000AE4 RID: 2788
		k_EResultRevoked,
		// Token: 0x04000AE5 RID: 2789
		k_EResultExpired,
		// Token: 0x04000AE6 RID: 2790
		k_EResultAlreadyRedeemed,
		// Token: 0x04000AE7 RID: 2791
		k_EResultDuplicateRequest,
		// Token: 0x04000AE8 RID: 2792
		k_EResultAlreadyOwned,
		// Token: 0x04000AE9 RID: 2793
		k_EResultIPNotFound,
		// Token: 0x04000AEA RID: 2794
		k_EResultPersistFailed,
		// Token: 0x04000AEB RID: 2795
		k_EResultLockingFailed,
		// Token: 0x04000AEC RID: 2796
		k_EResultLogonSessionReplaced,
		// Token: 0x04000AED RID: 2797
		k_EResultConnectFailed,
		// Token: 0x04000AEE RID: 2798
		k_EResultHandshakeFailed,
		// Token: 0x04000AEF RID: 2799
		k_EResultIOFailure,
		// Token: 0x04000AF0 RID: 2800
		k_EResultRemoteDisconnect,
		// Token: 0x04000AF1 RID: 2801
		k_EResultShoppingCartNotFound,
		// Token: 0x04000AF2 RID: 2802
		k_EResultBlocked,
		// Token: 0x04000AF3 RID: 2803
		k_EResultIgnored,
		// Token: 0x04000AF4 RID: 2804
		k_EResultNoMatch,
		// Token: 0x04000AF5 RID: 2805
		k_EResultAccountDisabled,
		// Token: 0x04000AF6 RID: 2806
		k_EResultServiceReadOnly,
		// Token: 0x04000AF7 RID: 2807
		k_EResultAccountNotFeatured,
		// Token: 0x04000AF8 RID: 2808
		k_EResultAdministratorOK,
		// Token: 0x04000AF9 RID: 2809
		k_EResultContentVersion,
		// Token: 0x04000AFA RID: 2810
		k_EResultTryAnotherCM,
		// Token: 0x04000AFB RID: 2811
		k_EResultPasswordRequiredToKickSession,
		// Token: 0x04000AFC RID: 2812
		k_EResultAlreadyLoggedInElsewhere,
		// Token: 0x04000AFD RID: 2813
		k_EResultSuspended,
		// Token: 0x04000AFE RID: 2814
		k_EResultCancelled,
		// Token: 0x04000AFF RID: 2815
		k_EResultDataCorruption,
		// Token: 0x04000B00 RID: 2816
		k_EResultDiskFull,
		// Token: 0x04000B01 RID: 2817
		k_EResultRemoteCallFailed,
		// Token: 0x04000B02 RID: 2818
		k_EResultPasswordUnset,
		// Token: 0x04000B03 RID: 2819
		k_EResultExternalAccountUnlinked,
		// Token: 0x04000B04 RID: 2820
		k_EResultPSNTicketInvalid,
		// Token: 0x04000B05 RID: 2821
		k_EResultExternalAccountAlreadyLinked,
		// Token: 0x04000B06 RID: 2822
		k_EResultRemoteFileConflict,
		// Token: 0x04000B07 RID: 2823
		k_EResultIllegalPassword,
		// Token: 0x04000B08 RID: 2824
		k_EResultSameAsPreviousValue,
		// Token: 0x04000B09 RID: 2825
		k_EResultAccountLogonDenied,
		// Token: 0x04000B0A RID: 2826
		k_EResultCannotUseOldPassword,
		// Token: 0x04000B0B RID: 2827
		k_EResultInvalidLoginAuthCode,
		// Token: 0x04000B0C RID: 2828
		k_EResultAccountLogonDeniedNoMail,
		// Token: 0x04000B0D RID: 2829
		k_EResultHardwareNotCapableOfIPT,
		// Token: 0x04000B0E RID: 2830
		k_EResultIPTInitError,
		// Token: 0x04000B0F RID: 2831
		k_EResultParentalControlRestricted,
		// Token: 0x04000B10 RID: 2832
		k_EResultFacebookQueryError,
		// Token: 0x04000B11 RID: 2833
		k_EResultExpiredLoginAuthCode,
		// Token: 0x04000B12 RID: 2834
		k_EResultIPLoginRestrictionFailed,
		// Token: 0x04000B13 RID: 2835
		k_EResultAccountLockedDown,
		// Token: 0x04000B14 RID: 2836
		k_EResultAccountLogonDeniedVerifiedEmailRequired,
		// Token: 0x04000B15 RID: 2837
		k_EResultNoMatchingURL,
		// Token: 0x04000B16 RID: 2838
		k_EResultBadResponse,
		// Token: 0x04000B17 RID: 2839
		k_EResultRequirePasswordReEntry,
		// Token: 0x04000B18 RID: 2840
		k_EResultValueOutOfRange,
		// Token: 0x04000B19 RID: 2841
		k_EResultUnexpectedError,
		// Token: 0x04000B1A RID: 2842
		k_EResultDisabled,
		// Token: 0x04000B1B RID: 2843
		k_EResultInvalidCEGSubmission,
		// Token: 0x04000B1C RID: 2844
		k_EResultRestrictedDevice,
		// Token: 0x04000B1D RID: 2845
		k_EResultRegionLocked,
		// Token: 0x04000B1E RID: 2846
		k_EResultRateLimitExceeded,
		// Token: 0x04000B1F RID: 2847
		k_EResultAccountLoginDeniedNeedTwoFactor,
		// Token: 0x04000B20 RID: 2848
		k_EResultItemDeleted,
		// Token: 0x04000B21 RID: 2849
		k_EResultAccountLoginDeniedThrottle,
		// Token: 0x04000B22 RID: 2850
		k_EResultTwoFactorCodeMismatch,
		// Token: 0x04000B23 RID: 2851
		k_EResultTwoFactorActivationCodeMismatch,
		// Token: 0x04000B24 RID: 2852
		k_EResultAccountAssociatedToMultiplePartners,
		// Token: 0x04000B25 RID: 2853
		k_EResultNotModified,
		// Token: 0x04000B26 RID: 2854
		k_EResultNoMobileDevice,
		// Token: 0x04000B27 RID: 2855
		k_EResultTimeNotSynced,
		// Token: 0x04000B28 RID: 2856
		k_EResultSmsCodeFailed,
		// Token: 0x04000B29 RID: 2857
		k_EResultAccountLimitExceeded,
		// Token: 0x04000B2A RID: 2858
		k_EResultAccountActivityLimitExceeded,
		// Token: 0x04000B2B RID: 2859
		k_EResultPhoneActivityLimitExceeded,
		// Token: 0x04000B2C RID: 2860
		k_EResultRefundToWallet,
		// Token: 0x04000B2D RID: 2861
		k_EResultEmailSendFailure,
		// Token: 0x04000B2E RID: 2862
		k_EResultNotSettled,
		// Token: 0x04000B2F RID: 2863
		k_EResultNeedCaptcha,
		// Token: 0x04000B30 RID: 2864
		k_EResultGSLTDenied,
		// Token: 0x04000B31 RID: 2865
		k_EResultGSOwnerDenied,
		// Token: 0x04000B32 RID: 2866
		k_EResultInvalidItemType,
		// Token: 0x04000B33 RID: 2867
		k_EResultIPBanned,
		// Token: 0x04000B34 RID: 2868
		k_EResultGSLTExpired
	}
}
