using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x020002F0 RID: 752
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 372)]
	public class gameserveritem_t
	{
		// Token: 0x06000D13 RID: 3347 RVA: 0x0000CF63 File Offset: 0x0000B163
		public string GetGameDir()
		{
			return Encoding.UTF8.GetString(this.m_szGameDir, 0, Array.IndexOf<byte>(this.m_szGameDir, 0));
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0000CF82 File Offset: 0x0000B182
		public void SetGameDir(string dir)
		{
			this.m_szGameDir = Encoding.UTF8.GetBytes(dir + '\0');
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0000CFA0 File Offset: 0x0000B1A0
		public string GetMap()
		{
			return Encoding.UTF8.GetString(this.m_szMap, 0, Array.IndexOf<byte>(this.m_szMap, 0));
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0000CFBF File Offset: 0x0000B1BF
		public void SetMap(string map)
		{
			this.m_szMap = Encoding.UTF8.GetBytes(map + '\0');
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0000CFDD File Offset: 0x0000B1DD
		public string GetGameDescription()
		{
			return Encoding.UTF8.GetString(this.m_szGameDescription, 0, Array.IndexOf<byte>(this.m_szGameDescription, 0));
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0000CFFC File Offset: 0x0000B1FC
		public void SetGameDescription(string desc)
		{
			this.m_szGameDescription = Encoding.UTF8.GetBytes(desc + '\0');
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0000D01A File Offset: 0x0000B21A
		public string GetServerName()
		{
			if (this.m_szServerName[0] == 0)
			{
				return this.m_NetAdr.GetConnectionAddressString();
			}
			return Encoding.UTF8.GetString(this.m_szServerName, 0, Array.IndexOf<byte>(this.m_szServerName, 0));
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0000D052 File Offset: 0x0000B252
		public void SetServerName(string name)
		{
			this.m_szServerName = Encoding.UTF8.GetBytes(name + '\0');
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0000D070 File Offset: 0x0000B270
		public string GetGameTags()
		{
			return Encoding.UTF8.GetString(this.m_szGameTags, 0, Array.IndexOf<byte>(this.m_szGameTags, 0));
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0000D08F File Offset: 0x0000B28F
		public void SetGameTags(string tags)
		{
			this.m_szGameTags = Encoding.UTF8.GetBytes(tags + '\0');
		}

		// Token: 0x04000CD1 RID: 3281
		public servernetadr_t m_NetAdr;

		// Token: 0x04000CD2 RID: 3282
		public int m_nPing;

		// Token: 0x04000CD3 RID: 3283
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHadSuccessfulResponse;

		// Token: 0x04000CD4 RID: 3284
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDoNotRefresh;

		// Token: 0x04000CD5 RID: 3285
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szGameDir;

		// Token: 0x04000CD6 RID: 3286
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szMap;

		// Token: 0x04000CD7 RID: 3287
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szGameDescription;

		// Token: 0x04000CD8 RID: 3288
		public uint m_nAppID;

		// Token: 0x04000CD9 RID: 3289
		public int m_nPlayers;

		// Token: 0x04000CDA RID: 3290
		public int m_nMaxPlayers;

		// Token: 0x04000CDB RID: 3291
		public int m_nBotPlayers;

		// Token: 0x04000CDC RID: 3292
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPassword;

		// Token: 0x04000CDD RID: 3293
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSecure;

		// Token: 0x04000CDE RID: 3294
		public uint m_ulTimeLastPlayed;

		// Token: 0x04000CDF RID: 3295
		public int m_nServerVersion;

		// Token: 0x04000CE0 RID: 3296
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szServerName;

		// Token: 0x04000CE1 RID: 3297
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szGameTags;

		// Token: 0x04000CE2 RID: 3298
		public CSteamID m_steamID;
	}
}
