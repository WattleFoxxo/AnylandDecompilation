using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AF RID: 431
	public static class SteamUser
	{
		// Token: 0x0600087C RID: 2172 RVA: 0x0000AEC4 File Offset: 0x000090C4
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamUser_GetHSteamUser();
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0000AED5 File Offset: 0x000090D5
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BLoggedOn();
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0000AEE1 File Offset: 0x000090E1
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamUser_GetSteamID();
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0000AEF2 File Offset: 0x000090F2
		public static int InitiateGameConnection(byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, bool bSecure)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_InitiateGameConnection(pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0000AF06 File Offset: 0x00009106
		public static void TerminateGameConnection(uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_TerminateGameConnection(unIPServer, usPortServer);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0000AF14 File Offset: 0x00009114
		public static void TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "")
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchExtraInfo))
			{
				NativeMethods.ISteamUser_TrackAppUsageEvent(gameID, eAppUsageEvent, utf8StringHandle);
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0000AF58 File Offset: 0x00009158
		public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubBuffer);
			bool flag = NativeMethods.ISteamUser_GetUserDataFolder(intPtr, cubBuffer);
			pchBuffer = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0000AF94 File Offset: 0x00009194
		public static void StartVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StartVoiceRecording();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public static void StopVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StopVoiceRecording();
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0000AFAC File Offset: 0x000091AC
		public static EVoiceResult GetAvailableVoice(out uint pcbCompressed, out uint pcbUncompressed, uint nUncompressedVoiceDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetAvailableVoice(out pcbCompressed, out pcbUncompressed, nUncompressedVoiceDesiredSampleRate);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0000AFBC File Offset: 0x000091BC
		public static EVoiceResult GetVoice(bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, bool bWantUncompressed, byte[] pUncompressedDestBuffer, uint cbUncompressedDestBufferSize, out uint nUncompressBytesWritten, uint nUncompressedVoiceDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoice(bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, bWantUncompressed, pUncompressedDestBuffer, cbUncompressedDestBufferSize, out nUncompressBytesWritten, nUncompressedVoiceDesiredSampleRate);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0000AFE1 File Offset: 0x000091E1
		public static EVoiceResult DecompressVoice(byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_DecompressVoice(pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000AFF5 File Offset: 0x000091F5
		public static uint GetVoiceOptimalSampleRate()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoiceOptimalSampleRate();
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0000B001 File Offset: 0x00009201
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return (HAuthTicket)NativeMethods.ISteamUser_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0000B015 File Offset: 0x00009215
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0000B024 File Offset: 0x00009224
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_EndAuthSession(steamID);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0000B031 File Offset: 0x00009231
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_CancelAuthTicket(hAuthTicket);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0000B03E File Offset: 0x0000923E
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_UserHasLicenseForApp(steamID, appID);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0000B04C File Offset: 0x0000924C
		public static bool BIsBehindNAT()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsBehindNAT();
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0000B058 File Offset: 0x00009258
		public static void AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_AdvertiseGame(steamIDGameServer, unIPServer, usPortServer);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0000B067 File Offset: 0x00009267
		public static SteamAPICall_t RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_RequestEncryptedAppTicket(pDataToInclude, cbDataToInclude);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0000B07A File Offset: 0x0000927A
		public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetEncryptedAppTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0000B089 File Offset: 0x00009289
		public static int GetGameBadgeLevel(int nSeries, bool bFoil)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetGameBadgeLevel(nSeries, bFoil);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0000B097 File Offset: 0x00009297
		public static int GetPlayerSteamLevel()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetPlayerSteamLevel();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0000B0A4 File Offset: 0x000092A4
		public static SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchRedirectURL))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUser_RequestStoreAuthURL(utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0000B0EC File Offset: 0x000092EC
		public static bool BIsPhoneVerified()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneVerified();
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0000B0F8 File Offset: 0x000092F8
		public static bool BIsTwoFactorEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsTwoFactorEnabled();
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0000B104 File Offset: 0x00009304
		public static bool BIsPhoneIdentifying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneIdentifying();
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0000B110 File Offset: 0x00009310
		public static bool BIsPhoneRequiringVerification()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneRequiringVerification();
		}
	}
}
