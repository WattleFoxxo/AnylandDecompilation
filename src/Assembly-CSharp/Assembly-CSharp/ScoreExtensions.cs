using System;
using ExitGames.Client.Photon;

// Token: 0x020000C8 RID: 200
public static class ScoreExtensions
{
	// Token: 0x0600068B RID: 1675 RVA: 0x0001EBAC File Offset: 0x0001CFAC
	public static void SetScore(this PhotonPlayer player, int newScore)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["score"] = newScore;
		player.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0001EBDC File Offset: 0x0001CFDC
	public static void AddScore(this PhotonPlayer player, int scoreToAddToCurrent)
	{
		int num = player.GetScore();
		num += scoreToAddToCurrent;
		Hashtable hashtable = new Hashtable();
		hashtable["score"] = num;
		player.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0001EC14 File Offset: 0x0001D014
	public static int GetScore(this PhotonPlayer player)
	{
		object obj;
		if (player.CustomProperties.TryGetValue("score", out obj))
		{
			return (int)obj;
		}
		return 0;
	}
}
