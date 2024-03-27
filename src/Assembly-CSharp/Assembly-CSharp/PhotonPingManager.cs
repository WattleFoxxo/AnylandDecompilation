using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class PhotonPingManager
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000567 RID: 1383 RVA: 0x00018E08 File Offset: 0x00017208
	public Region BestRegion
	{
		get
		{
			Region region = null;
			int num = int.MaxValue;
			foreach (Region region2 in PhotonNetwork.networkingPeer.AvailableRegions)
			{
				global::UnityEngine.Debug.Log("BestRegion checks region: " + region2);
				if (region2.Ping != 0 && region2.Ping < num)
				{
					num = region2.Ping;
					region = region2;
				}
			}
			return region;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000568 RID: 1384 RVA: 0x00018E9C File Offset: 0x0001729C
	public bool Done
	{
		get
		{
			return this.PingsRunning == 0;
		}
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00018EA8 File Offset: 0x000172A8
	public IEnumerator PingSocket(Region region)
	{
		region.Ping = PhotonPingManager.Attempts * PhotonPingManager.MaxMilliseconsPerPing;
		this.PingsRunning++;
		PhotonPing ping;
		if (PhotonHandler.PingImplementation == typeof(PingNativeDynamic))
		{
			global::UnityEngine.Debug.Log("Using constructor for new PingNativeDynamic()");
			ping = new PingNativeDynamic();
		}
		else if (PhotonHandler.PingImplementation == typeof(PingNativeStatic))
		{
			global::UnityEngine.Debug.Log("Using constructor for new PingNativeStatic()");
			ping = new PingNativeStatic();
		}
		else if (PhotonHandler.PingImplementation == typeof(PingMono))
		{
			ping = new PingMono();
		}
		else
		{
			ping = (PhotonPing)Activator.CreateInstance(PhotonHandler.PingImplementation);
		}
		float rttSum = 0f;
		int replyCount = 0;
		string regionAddress = region.HostAndPort;
		int indexOfColon = regionAddress.LastIndexOf(':');
		if (indexOfColon > 1)
		{
			regionAddress = regionAddress.Substring(0, indexOfColon);
		}
		int indexOfProtocol = regionAddress.IndexOf("wss://");
		if (indexOfProtocol > -1)
		{
			regionAddress = regionAddress.Substring(indexOfProtocol + "wss://".Length);
		}
		regionAddress = PhotonPingManager.ResolveHost(regionAddress);
		for (int i = 0; i < PhotonPingManager.Attempts; i++)
		{
			bool overtime = false;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			try
			{
				ping.StartPing(regionAddress);
			}
			catch (Exception ex)
			{
				global::UnityEngine.Debug.Log("catched: " + ex);
				this.PingsRunning--;
				break;
			}
			while (!ping.Done())
			{
				if (sw.ElapsedMilliseconds >= (long)PhotonPingManager.MaxMilliseconsPerPing)
				{
					overtime = true;
					break;
				}
				yield return 0;
			}
			int rtt = (int)sw.ElapsedMilliseconds;
			if (!PhotonPingManager.IgnoreInitialAttempt || i != 0)
			{
				if (ping.Successful && !overtime)
				{
					rttSum += (float)rtt;
					replyCount++;
					region.Ping = (int)(rttSum / (float)replyCount);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		ping.Dispose();
		this.PingsRunning--;
		yield return null;
		yield break;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00018ECC File Offset: 0x000172CC
	public static string ResolveHost(string hostName)
	{
		string text = string.Empty;
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
			if (hostAddresses.Length == 1)
			{
				return hostAddresses[0].ToString();
			}
			foreach (IPAddress ipaddress in hostAddresses)
			{
				if (ipaddress != null)
				{
					if (ipaddress.ToString().Contains(":"))
					{
						return ipaddress.ToString();
					}
					if (string.IsNullOrEmpty(text))
					{
						text = hostAddresses.ToString();
					}
				}
			}
		}
		catch (Exception ex)
		{
			global::UnityEngine.Debug.Log("Exception caught! " + ex.Source + " Message: " + ex.Message);
		}
		return text;
	}

	// Token: 0x040003E2 RID: 994
	public bool UseNative;

	// Token: 0x040003E3 RID: 995
	public static int Attempts = 5;

	// Token: 0x040003E4 RID: 996
	public static bool IgnoreInitialAttempt = true;

	// Token: 0x040003E5 RID: 997
	public static int MaxMilliseconsPerPing = 800;

	// Token: 0x040003E6 RID: 998
	private const string wssProtocolString = "wss://";

	// Token: 0x040003E7 RID: 999
	private int PingsRunning;
}
