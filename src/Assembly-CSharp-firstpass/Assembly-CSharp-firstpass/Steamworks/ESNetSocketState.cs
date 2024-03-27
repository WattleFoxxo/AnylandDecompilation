using System;

namespace Steamworks
{
	// Token: 0x0200027B RID: 635
	public enum ESNetSocketState
	{
		// Token: 0x040009F8 RID: 2552
		k_ESNetSocketStateInvalid,
		// Token: 0x040009F9 RID: 2553
		k_ESNetSocketStateConnected,
		// Token: 0x040009FA RID: 2554
		k_ESNetSocketStateInitiated = 10,
		// Token: 0x040009FB RID: 2555
		k_ESNetSocketStateLocalCandidatesFound,
		// Token: 0x040009FC RID: 2556
		k_ESNetSocketStateReceivedRemoteCandidates,
		// Token: 0x040009FD RID: 2557
		k_ESNetSocketStateChallengeHandshake = 15,
		// Token: 0x040009FE RID: 2558
		k_ESNetSocketStateDisconnecting = 21,
		// Token: 0x040009FF RID: 2559
		k_ESNetSocketStateLocalDisconnect,
		// Token: 0x04000A00 RID: 2560
		k_ESNetSocketStateTimeoutDuringConnect,
		// Token: 0x04000A01 RID: 2561
		k_ESNetSocketStateRemoteEndDisconnected,
		// Token: 0x04000A02 RID: 2562
		k_ESNetSocketStateConnectionBroken
	}
}
