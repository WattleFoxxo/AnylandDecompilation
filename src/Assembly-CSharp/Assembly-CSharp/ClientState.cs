using System;

// Token: 0x02000078 RID: 120
public enum ClientState
{
	// Token: 0x040002EB RID: 747
	Uninitialized,
	// Token: 0x040002EC RID: 748
	PeerCreated,
	// Token: 0x040002ED RID: 749
	Queued,
	// Token: 0x040002EE RID: 750
	Authenticated,
	// Token: 0x040002EF RID: 751
	JoinedLobby,
	// Token: 0x040002F0 RID: 752
	DisconnectingFromMasterserver,
	// Token: 0x040002F1 RID: 753
	ConnectingToGameserver,
	// Token: 0x040002F2 RID: 754
	ConnectedToGameserver,
	// Token: 0x040002F3 RID: 755
	Joining,
	// Token: 0x040002F4 RID: 756
	Joined,
	// Token: 0x040002F5 RID: 757
	Leaving,
	// Token: 0x040002F6 RID: 758
	DisconnectingFromGameserver,
	// Token: 0x040002F7 RID: 759
	ConnectingToMasterserver,
	// Token: 0x040002F8 RID: 760
	QueuedComingFromGameserver,
	// Token: 0x040002F9 RID: 761
	Disconnecting,
	// Token: 0x040002FA RID: 762
	Disconnected,
	// Token: 0x040002FB RID: 763
	ConnectedToMaster,
	// Token: 0x040002FC RID: 764
	ConnectingToNameServer,
	// Token: 0x040002FD RID: 765
	ConnectedToNameServer,
	// Token: 0x040002FE RID: 766
	DisconnectingFromNameServer,
	// Token: 0x040002FF RID: 767
	Authenticating
}
