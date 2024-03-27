using System;

namespace Steamworks
{
	// Token: 0x0200029D RID: 669
	public enum EAuthSessionResponse
	{
		// Token: 0x04000B59 RID: 2905
		k_EAuthSessionResponseOK,
		// Token: 0x04000B5A RID: 2906
		k_EAuthSessionResponseUserNotConnectedToSteam,
		// Token: 0x04000B5B RID: 2907
		k_EAuthSessionResponseNoLicenseOrExpired,
		// Token: 0x04000B5C RID: 2908
		k_EAuthSessionResponseVACBanned,
		// Token: 0x04000B5D RID: 2909
		k_EAuthSessionResponseLoggedInElseWhere,
		// Token: 0x04000B5E RID: 2910
		k_EAuthSessionResponseVACCheckTimedOut,
		// Token: 0x04000B5F RID: 2911
		k_EAuthSessionResponseAuthTicketCanceled,
		// Token: 0x04000B60 RID: 2912
		k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed,
		// Token: 0x04000B61 RID: 2913
		k_EAuthSessionResponseAuthTicketInvalid,
		// Token: 0x04000B62 RID: 2914
		k_EAuthSessionResponsePublisherIssuedBan
	}
}
