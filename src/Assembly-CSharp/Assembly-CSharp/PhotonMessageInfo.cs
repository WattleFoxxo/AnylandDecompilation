using System;

// Token: 0x02000082 RID: 130
public struct PhotonMessageInfo
{
	// Token: 0x0600043D RID: 1085 RVA: 0x0001429C File Offset: 0x0001269C
	public PhotonMessageInfo(PhotonPlayer player, int timestamp, PhotonView view)
	{
		this.sender = player;
		this.timeInt = timestamp;
		this.photonView = view;
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x0600043E RID: 1086 RVA: 0x000142B4 File Offset: 0x000126B4
	public double timestamp
	{
		get
		{
			uint num = (uint)this.timeInt;
			double num2 = num;
			return num2 / 1000.0;
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x000142D7 File Offset: 0x000126D7
	public override string ToString()
	{
		return string.Format("[PhotonMessageInfo: Sender='{1}' Senttime={0}]", this.timestamp, this.sender);
	}

	// Token: 0x04000358 RID: 856
	private readonly int timeInt;

	// Token: 0x04000359 RID: 857
	public readonly PhotonPlayer sender;

	// Token: 0x0400035A RID: 858
	public readonly PhotonView photonView;
}
