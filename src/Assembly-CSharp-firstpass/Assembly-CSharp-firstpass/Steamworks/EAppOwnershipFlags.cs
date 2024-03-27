using System;

namespace Steamworks
{
	// Token: 0x020002A1 RID: 673
	[Flags]
	public enum EAppOwnershipFlags
	{
		// Token: 0x04000B7B RID: 2939
		k_EAppOwnershipFlags_None = 0,
		// Token: 0x04000B7C RID: 2940
		k_EAppOwnershipFlags_OwnsLicense = 1,
		// Token: 0x04000B7D RID: 2941
		k_EAppOwnershipFlags_FreeLicense = 2,
		// Token: 0x04000B7E RID: 2942
		k_EAppOwnershipFlags_RegionRestricted = 4,
		// Token: 0x04000B7F RID: 2943
		k_EAppOwnershipFlags_LowViolence = 8,
		// Token: 0x04000B80 RID: 2944
		k_EAppOwnershipFlags_InvalidPlatform = 16,
		// Token: 0x04000B81 RID: 2945
		k_EAppOwnershipFlags_SharedLicense = 32,
		// Token: 0x04000B82 RID: 2946
		k_EAppOwnershipFlags_FreeWeekend = 64,
		// Token: 0x04000B83 RID: 2947
		k_EAppOwnershipFlags_RetailLicense = 128,
		// Token: 0x04000B84 RID: 2948
		k_EAppOwnershipFlags_LicenseLocked = 256,
		// Token: 0x04000B85 RID: 2949
		k_EAppOwnershipFlags_LicensePending = 512,
		// Token: 0x04000B86 RID: 2950
		k_EAppOwnershipFlags_LicenseExpired = 1024,
		// Token: 0x04000B87 RID: 2951
		k_EAppOwnershipFlags_LicensePermanent = 2048,
		// Token: 0x04000B88 RID: 2952
		k_EAppOwnershipFlags_LicenseRecurring = 4096,
		// Token: 0x04000B89 RID: 2953
		k_EAppOwnershipFlags_LicenseCanceled = 8192,
		// Token: 0x04000B8A RID: 2954
		k_EAppOwnershipFlags_AutoGrant = 16384,
		// Token: 0x04000B8B RID: 2955
		k_EAppOwnershipFlags_PendingGift = 32768,
		// Token: 0x04000B8C RID: 2956
		k_EAppOwnershipFlags_RentalNotActivated = 65536,
		// Token: 0x04000B8D RID: 2957
		k_EAppOwnershipFlags_Rental = 131072
	}
}
