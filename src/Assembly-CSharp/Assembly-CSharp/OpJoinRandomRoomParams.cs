using System;
using ExitGames.Client.Photon;

// Token: 0x02000063 RID: 99
internal class OpJoinRandomRoomParams
{
	// Token: 0x04000201 RID: 513
	public Hashtable ExpectedCustomRoomProperties;

	// Token: 0x04000202 RID: 514
	public byte ExpectedMaxPlayers;

	// Token: 0x04000203 RID: 515
	public MatchmakingMode MatchingType;

	// Token: 0x04000204 RID: 516
	public TypedLobby TypedLobby;

	// Token: 0x04000205 RID: 517
	public string SqlLobbyFilter;

	// Token: 0x04000206 RID: 518
	public string[] ExpectedUsers;
}
