using System;

// Token: 0x020000CD RID: 205
public interface IPunTurnManagerCallbacks
{
	// Token: 0x060006A8 RID: 1704
	void OnTurnBegins(int turn);

	// Token: 0x060006A9 RID: 1705
	void OnTurnCompleted(int turn);

	// Token: 0x060006AA RID: 1706
	void OnPlayerMove(PhotonPlayer player, int turn, object move);

	// Token: 0x060006AB RID: 1707
	void OnPlayerFinished(PhotonPlayer player, int turn, object move);

	// Token: 0x060006AC RID: 1708
	void OnTurnTimeEnds(int turn);
}
