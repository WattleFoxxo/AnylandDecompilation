using System;

namespace Steamworks
{
	// Token: 0x020001A9 RID: 425
	public static class SteamMusicRemote
	{
		// Token: 0x060007BE RID: 1982 RVA: 0x000094D0 File Offset: 0x000076D0
		public static bool RegisterSteamMusicRemote(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamMusicRemote_RegisterSteamMusicRemote(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00009514 File Offset: 0x00007714
		public static bool DeregisterSteamMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_DeregisterSteamMusicRemote();
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00009520 File Offset: 0x00007720
		public static bool BIsCurrentMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BIsCurrentMusicRemote();
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0000952C File Offset: 0x0000772C
		public static bool BActivationSuccess(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BActivationSuccess(bValue);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0000953C File Offset: 0x0000773C
		public static bool SetDisplayName(string pchDisplayName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDisplayName))
			{
				flag = NativeMethods.ISteamMusicRemote_SetDisplayName(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00009580 File Offset: 0x00007780
		public static bool SetPNGIcon_64x64(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetPNGIcon_64x64(pvBuffer, cbBufferLength);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0000958E File Offset: 0x0000778E
		public static bool EnablePlayPrevious(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayPrevious(bValue);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0000959B File Offset: 0x0000779B
		public static bool EnablePlayNext(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayNext(bValue);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000095A8 File Offset: 0x000077A8
		public static bool EnableShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableShuffled(bValue);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x000095B5 File Offset: 0x000077B5
		public static bool EnableLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableLooped(bValue);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000095C2 File Offset: 0x000077C2
		public static bool EnableQueue(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableQueue(bValue);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000095CF File Offset: 0x000077CF
		public static bool EnablePlaylists(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlaylists(bValue);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000095DC File Offset: 0x000077DC
		public static bool UpdatePlaybackStatus(AudioPlayback_Status nStatus)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdatePlaybackStatus(nStatus);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000095E9 File Offset: 0x000077E9
		public static bool UpdateShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateShuffled(bValue);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000095F6 File Offset: 0x000077F6
		public static bool UpdateLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateLooped(bValue);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00009603 File Offset: 0x00007803
		public static bool UpdateVolume(float flValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateVolume(flValue);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00009610 File Offset: 0x00007810
		public static bool CurrentEntryWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryWillChange();
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0000961C File Offset: 0x0000781C
		public static bool CurrentEntryIsAvailable(bool bAvailable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryIsAvailable(bAvailable);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0000962C File Offset: 0x0000782C
		public static bool UpdateCurrentEntryText(string pchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchText))
			{
				flag = NativeMethods.ISteamMusicRemote_UpdateCurrentEntryText(utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00009670 File Offset: 0x00007870
		public static bool UpdateCurrentEntryElapsedSeconds(int nValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(nValue);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0000967D File Offset: 0x0000787D
		public static bool UpdateCurrentEntryCoverArt(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryCoverArt(pvBuffer, cbBufferLength);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000968B File Offset: 0x0000788B
		public static bool CurrentEntryDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryDidChange();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00009697 File Offset: 0x00007897
		public static bool QueueWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueWillChange();
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000096A3 File Offset: 0x000078A3
		public static bool ResetQueueEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetQueueEntries();
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000096B0 File Offset: 0x000078B0
		public static bool SetQueueEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				flag = NativeMethods.ISteamMusicRemote_SetQueueEntry(nID, nPosition, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x000096F8 File Offset: 0x000078F8
		public static bool SetCurrentQueueEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentQueueEntry(nID);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00009705 File Offset: 0x00007905
		public static bool QueueDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueDidChange();
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00009711 File Offset: 0x00007911
		public static bool PlaylistWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistWillChange();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0000971D File Offset: 0x0000791D
		public static bool ResetPlaylistEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetPlaylistEntries();
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0000972C File Offset: 0x0000792C
		public static bool SetPlaylistEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				flag = NativeMethods.ISteamMusicRemote_SetPlaylistEntry(nID, nPosition, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00009774 File Offset: 0x00007974
		public static bool SetCurrentPlaylistEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentPlaylistEntry(nID);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00009781 File Offset: 0x00007981
		public static bool PlaylistDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistDidChange();
		}
	}
}
