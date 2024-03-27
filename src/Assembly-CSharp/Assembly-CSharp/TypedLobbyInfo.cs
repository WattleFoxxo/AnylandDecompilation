using System;

// Token: 0x02000074 RID: 116
public class TypedLobbyInfo : TypedLobby
{
	// Token: 0x06000368 RID: 872 RVA: 0x0000DBF0 File Offset: 0x0000BFF0
	public override string ToString()
	{
		return string.Format("TypedLobbyInfo '{0}'[{1}] rooms: {2} players: {3}", new object[] { this.Name, this.Type, this.RoomCount, this.PlayerCount });
	}

	// Token: 0x040002D7 RID: 727
	public int PlayerCount;

	// Token: 0x040002D8 RID: 728
	public int RoomCount;
}
