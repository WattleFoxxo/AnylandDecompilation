using System;
using ExitGames.Client.Photon;

// Token: 0x02000064 RID: 100
internal class EnterRoomParams
{
	// Token: 0x04000207 RID: 519
	public string RoomName;

	// Token: 0x04000208 RID: 520
	public RoomOptions RoomOptions;

	// Token: 0x04000209 RID: 521
	public TypedLobby Lobby;

	// Token: 0x0400020A RID: 522
	public Hashtable PlayerProperties;

	// Token: 0x0400020B RID: 523
	public bool OnGameServer = true;

	// Token: 0x0400020C RID: 524
	public bool CreateIfNotExists;

	// Token: 0x0400020D RID: 525
	public bool RejoinOnly;

	// Token: 0x0400020E RID: 526
	public string[] ExpectedUsers;
}
