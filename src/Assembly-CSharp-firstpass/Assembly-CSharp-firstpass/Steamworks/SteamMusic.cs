using System;

namespace Steamworks
{
	// Token: 0x020001A8 RID: 424
	public static class SteamMusic
	{
		// Token: 0x060007B5 RID: 1973 RVA: 0x00009462 File Offset: 0x00007662
		public static bool BIsEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsEnabled();
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0000946E File Offset: 0x0000766E
		public static bool BIsPlaying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsPlaying();
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0000947A File Offset: 0x0000767A
		public static AudioPlayback_Status GetPlaybackStatus()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetPlaybackStatus();
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00009486 File Offset: 0x00007686
		public static void Play()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Play();
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00009492 File Offset: 0x00007692
		public static void Pause()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Pause();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0000949E File Offset: 0x0000769E
		public static void PlayPrevious()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayPrevious();
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x000094AA File Offset: 0x000076AA
		public static void PlayNext()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayNext();
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x000094B6 File Offset: 0x000076B6
		public static void SetVolume(float flVolume)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_SetVolume(flVolume);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000094C3 File Offset: 0x000076C3
		public static float GetVolume()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetVolume();
		}
	}
}
