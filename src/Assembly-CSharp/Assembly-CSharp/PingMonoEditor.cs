using System;
using System.Net.Sockets;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class PingMonoEditor : PhotonPing
{
	// Token: 0x06000563 RID: 1379 RVA: 0x00018C34 File Offset: 0x00017034
	public override bool StartPing(string ip)
	{
		base.Init();
		try
		{
			if (ip.Contains("."))
			{
				this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			}
			else
			{
				this.sock = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
			}
			this.sock.ReceiveTimeout = 5000;
			this.sock.Connect(ip, 5055);
			this.PingBytes[this.PingBytes.Length - 1] = this.PingId;
			this.sock.Send(this.PingBytes);
			this.PingBytes[this.PingBytes.Length - 1] = this.PingId - 1;
		}
		catch (Exception ex)
		{
			this.sock = null;
			Console.WriteLine(ex);
		}
		return false;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00018D08 File Offset: 0x00017108
	public override bool Done()
	{
		if (this.GotResult || this.sock == null)
		{
			return true;
		}
		if (this.sock.Available <= 0)
		{
			return false;
		}
		int num = this.sock.Receive(this.PingBytes, SocketFlags.None);
		if (this.PingBytes[this.PingBytes.Length - 1] != this.PingId || num != this.PingLength)
		{
			Debug.Log("ReplyMatch is false! ");
		}
		this.Successful = num == this.PingBytes.Length && this.PingBytes[this.PingBytes.Length - 1] == this.PingId;
		this.GotResult = true;
		return true;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00018DC4 File Offset: 0x000171C4
	public override void Dispose()
	{
		try
		{
			this.sock.Close();
		}
		catch
		{
		}
		this.sock = null;
	}

	// Token: 0x040003E1 RID: 993
	private Socket sock;
}
