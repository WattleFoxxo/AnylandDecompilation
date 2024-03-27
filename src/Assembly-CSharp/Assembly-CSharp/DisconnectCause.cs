using System;

// Token: 0x0200007A RID: 122
public enum DisconnectCause
{
	// Token: 0x04000306 RID: 774
	DisconnectByServerUserLimit = 1042,
	// Token: 0x04000307 RID: 775
	ExceptionOnConnect = 1023,
	// Token: 0x04000308 RID: 776
	DisconnectByServerTimeout = 1041,
	// Token: 0x04000309 RID: 777
	DisconnectByServerLogic = 1043,
	// Token: 0x0400030A RID: 778
	Exception = 1026,
	// Token: 0x0400030B RID: 779
	InvalidAuthentication = 32767,
	// Token: 0x0400030C RID: 780
	MaxCcuReached = 32757,
	// Token: 0x0400030D RID: 781
	InvalidRegion = 32756,
	// Token: 0x0400030E RID: 782
	SecurityExceptionOnConnect = 1022,
	// Token: 0x0400030F RID: 783
	DisconnectByClientTimeout = 1040,
	// Token: 0x04000310 RID: 784
	InternalReceiveException = 1039,
	// Token: 0x04000311 RID: 785
	AuthenticationTicketExpired = 32753
}
