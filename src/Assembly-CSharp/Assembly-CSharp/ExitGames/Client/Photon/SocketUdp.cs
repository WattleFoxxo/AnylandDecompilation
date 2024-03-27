using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace ExitGames.Client.Photon
{
	// Token: 0x0200009B RID: 155
	internal class SocketUdp : IPhotonSocket, IDisposable
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x00019F93 File Offset: 0x00018393
		public SocketUdp(PeerBase npeer)
			: base(npeer)
		{
			if (base.ReportDebugOfLevel(DebugLevel.ALL))
			{
				base.Listener.DebugReturn(DebugLevel.ALL, "CSharpSocket: UDP, Unity3d.");
			}
			this.PollReceive = false;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00019FCC File Offset: 0x000183CC
		public void Dispose()
		{
			base.State = PhotonSocketState.Disconnecting;
			if (this.sock != null)
			{
				try
				{
					if (this.sock.Connected)
					{
						this.sock.Close();
					}
				}
				catch (Exception ex)
				{
					base.EnqueueDebugReturn(DebugLevel.INFO, "Exception in Dispose(): " + ex);
				}
			}
			this.sock = null;
			base.State = PhotonSocketState.Disconnected;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001A044 File Offset: 0x00018444
		public override bool Connect()
		{
			object obj = this.syncer;
			bool flag;
			lock (obj)
			{
				if (!base.Connect())
				{
					flag = false;
				}
				else
				{
					base.State = PhotonSocketState.Connecting;
					new Thread(new ThreadStart(this.DnsAndConnect))
					{
						Name = "photon dns thread",
						IsBackground = true
					}.Start();
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001A0C4 File Offset: 0x000184C4
		public override bool Disconnect()
		{
			if (base.ReportDebugOfLevel(DebugLevel.INFO))
			{
				base.EnqueueDebugReturn(DebugLevel.INFO, "CSharpSocket.Disconnect()");
			}
			base.State = PhotonSocketState.Disconnecting;
			object obj = this.syncer;
			lock (obj)
			{
				if (this.sock != null)
				{
					try
					{
						this.sock.Close();
					}
					catch (Exception ex)
					{
						base.EnqueueDebugReturn(DebugLevel.INFO, "Exception in Disconnect(): " + ex);
					}
					this.sock = null;
				}
			}
			base.State = PhotonSocketState.Disconnected;
			return true;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001A168 File Offset: 0x00018568
		public override PhotonSocketError Send(byte[] data, int length)
		{
			object obj = this.syncer;
			lock (obj)
			{
				if (this.sock == null || !this.sock.Connected)
				{
					return PhotonSocketError.Skipped;
				}
				try
				{
					this.sock.Send(data, 0, length, SocketFlags.None);
				}
				catch (Exception ex)
				{
					if (base.ReportDebugOfLevel(DebugLevel.ERROR))
					{
						base.EnqueueDebugReturn(DebugLevel.ERROR, "Cannot send to: " + base.ServerAddress + ". " + ex.Message);
					}
					return PhotonSocketError.Exception;
				}
			}
			return PhotonSocketError.Success;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001A21C File Offset: 0x0001861C
		public override PhotonSocketError Receive(out byte[] data)
		{
			data = null;
			return PhotonSocketError.NoData;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001A224 File Offset: 0x00018624
		internal void DnsAndConnect()
		{
			IPAddress ipaddress = null;
			try
			{
				ipaddress = IPhotonSocket.GetIpAddress(base.ServerAddress);
				if (ipaddress == null)
				{
					throw new ArgumentException("Invalid IPAddress. Address: " + base.ServerAddress);
				}
				object obj = this.syncer;
				lock (obj)
				{
					if (base.State == PhotonSocketState.Disconnecting || base.State == PhotonSocketState.Disconnected)
					{
						return;
					}
					this.sock = new Socket(ipaddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
					this.sock.Connect(ipaddress, base.ServerPort);
					base.AddressResolvedAsIpv6 = base.IsIpv6SimpleCheck(ipaddress);
					base.State = PhotonSocketState.Connected;
					this.peerBase.OnConnect();
				}
			}
			catch (SecurityException ex)
			{
				if (base.ReportDebugOfLevel(DebugLevel.ERROR))
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat(new string[]
					{
						"Connect() to '",
						base.ServerAddress,
						"' (",
						(ipaddress != null) ? ipaddress.AddressFamily.ToString() : string.Empty,
						") failed: ",
						ex.ToString()
					}));
				}
				base.HandleException(StatusCode.SecurityExceptionOnConnect);
				return;
			}
			catch (Exception ex2)
			{
				if (base.ReportDebugOfLevel(DebugLevel.ERROR))
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat(new string[]
					{
						"Connect() to '",
						base.ServerAddress,
						"' (",
						(ipaddress != null) ? ipaddress.AddressFamily.ToString() : string.Empty,
						") failed: ",
						ex2.ToString()
					}));
				}
				base.HandleException(StatusCode.ExceptionOnConnect);
				return;
			}
			new Thread(new ThreadStart(this.ReceiveLoop))
			{
				Name = "photon receive thread",
				IsBackground = true
			}.Start();
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001A440 File Offset: 0x00018840
		public void ReceiveLoop()
		{
			byte[] array = new byte[base.MTU];
			while (base.State == PhotonSocketState.Connected)
			{
				try
				{
					int num = this.sock.Receive(array);
					base.HandleReceivedDatagram(array, num, true);
				}
				catch (Exception ex)
				{
					if (base.State != PhotonSocketState.Disconnecting && base.State != PhotonSocketState.Disconnected)
					{
						if (base.ReportDebugOfLevel(DebugLevel.ERROR))
						{
							base.EnqueueDebugReturn(DebugLevel.ERROR, string.Concat(new object[] { "Receive issue. State: ", base.State, ". Server: '", base.ServerAddress, "' Exception: ", ex }));
						}
						base.HandleException(StatusCode.ExceptionOnReceive);
					}
				}
			}
			this.Disconnect();
		}

		// Token: 0x04000410 RID: 1040
		private Socket sock;

		// Token: 0x04000411 RID: 1041
		private readonly object syncer = new object();
	}
}
