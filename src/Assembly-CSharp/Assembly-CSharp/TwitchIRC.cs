using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001D8 RID: 472
public class TwitchIRC : MonoBehaviour
{
	// Token: 0x06000EC6 RID: 3782 RVA: 0x0008241C File Offset: 0x0008081C
	public void Connect(string _oauth, string _nickName, string _channelName)
	{
		this.oauth = _oauth;
		this.nickName = _nickName;
		this.channelName = _channelName;
		this.stopThreads = false;
		TcpClient tcpClient = new TcpClient();
		tcpClient.Connect(this.server, this.port);
		if (!tcpClient.Connected)
		{
			Managers.dialogManager.ShowInfo("Oops, failed to connect to Twitch!", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
			return;
		}
		NetworkStream networkStream = tcpClient.GetStream();
		StreamReader input = new StreamReader(networkStream);
		StreamWriter output = new StreamWriter(networkStream);
		output.WriteLine("PASS " + this.oauth);
		output.WriteLine("NICK " + this.nickName.ToLower());
		output.Flush();
		this.outProc = new Thread(delegate
		{
			this.IRCOutputProcedure(output);
		});
		this.outProc.Start();
		this.inProc = new Thread(delegate
		{
			this.IRCInputProcedure(input, networkStream);
		});
		this.inProc.Start();
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0008254C File Offset: 0x0008094C
	private void Update()
	{
		object obj = this.receivedMsgs;
		lock (obj)
		{
			if (this.receivedMsgs.Count > 0)
			{
				for (int i = 0; i < this.receivedMsgs.Count; i++)
				{
					this.messageReceivedEvent.Invoke(this.receivedMsgs[i]);
				}
				this.receivedMsgs.Clear();
			}
		}
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x000825D4 File Offset: 0x000809D4
	private void IRCInputProcedure(TextReader input, NetworkStream networkStream)
	{
		while (!this.stopThreads)
		{
			if (networkStream.DataAvailable)
			{
				this.buffer = input.ReadLine();
				if (this.buffer.Contains("PRIVMSG #"))
				{
					object obj = this.receivedMsgs;
					lock (obj)
					{
						this.receivedMsgs.Add(this.buffer);
					}
				}
				if (this.buffer.StartsWith("PING "))
				{
					this.SendCommand(this.buffer.Replace("PING", "PONG"));
				}
				if (this.buffer.Split(new char[] { ' ' })[1] == "001")
				{
					this.SendCommand("JOIN #" + this.channelName);
				}
			}
		}
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x000826CC File Offset: 0x00080ACC
	private void IRCOutputProcedure(TextWriter output)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		while (!this.stopThreads)
		{
			object obj = this.commandQueue;
			lock (obj)
			{
				if (this.commandQueue.Count > 0 && stopwatch.ElapsedMilliseconds > 1750L)
				{
					output.WriteLine(this.commandQueue.Peek());
					output.Flush();
					this.commandQueue.Dequeue();
					stopwatch.Reset();
					stopwatch.Start();
				}
			}
		}
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x00082770 File Offset: 0x00080B70
	public void SendCommand(string cmd)
	{
		object obj = this.commandQueue;
		lock (obj)
		{
			this.commandQueue.Enqueue(cmd);
		}
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x000827B4 File Offset: 0x00080BB4
	public void SendMsg(string msg)
	{
		object obj = this.commandQueue;
		lock (obj)
		{
			this.commandQueue.Enqueue("PRIVMSG #" + this.channelName + " :" + msg);
		}
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x0008280C File Offset: 0x00080C0C
	private void OnDisable()
	{
		this.stopThreads = true;
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x00082815 File Offset: 0x00080C15
	private void OnDestroy()
	{
		this.stopThreads = true;
	}

	// Token: 0x04000F95 RID: 3989
	public string oauth = string.Empty;

	// Token: 0x04000F96 RID: 3990
	public string nickName = string.Empty;

	// Token: 0x04000F97 RID: 3991
	public string channelName = string.Empty;

	// Token: 0x04000F98 RID: 3992
	private string server = "irc.twitch.tv";

	// Token: 0x04000F99 RID: 3993
	private int port = 6667;

	// Token: 0x04000F9A RID: 3994
	public TwitchIRC.MsgEvent messageReceivedEvent = new TwitchIRC.MsgEvent();

	// Token: 0x04000F9B RID: 3995
	private string buffer = string.Empty;

	// Token: 0x04000F9C RID: 3996
	private bool stopThreads;

	// Token: 0x04000F9D RID: 3997
	private Queue<string> commandQueue = new Queue<string>();

	// Token: 0x04000F9E RID: 3998
	private List<string> receivedMsgs = new List<string>();

	// Token: 0x04000F9F RID: 3999
	private Thread inProc;

	// Token: 0x04000FA0 RID: 4000
	private Thread outProc;

	// Token: 0x020001D9 RID: 473
	public class MsgEvent : UnityEvent<string>
	{
	}
}
