using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002DA RID: 730
	public class ISteamMatchmakingPlayersResponse
	{
		// Token: 0x06000CB9 RID: 3257 RVA: 0x0000CA60 File Offset: 0x0000AC60
		public ISteamMatchmakingPlayersResponse(ISteamMatchmakingPlayersResponse.AddPlayerToList onAddPlayerToList, ISteamMatchmakingPlayersResponse.PlayersFailedToRespond onPlayersFailedToRespond, ISteamMatchmakingPlayersResponse.PlayersRefreshComplete onPlayersRefreshComplete)
		{
			if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_AddPlayerToList = onAddPlayerToList;
			this.m_PlayersFailedToRespond = onPlayersFailedToRespond;
			this.m_PlayersRefreshComplete = onPlayersRefreshComplete;
			this.m_VTable = new ISteamMatchmakingPlayersResponse.VTable
			{
				m_VTAddPlayerToList = new ISteamMatchmakingPlayersResponse.InternalAddPlayerToList(this.InternalOnAddPlayerToList),
				m_VTPlayersFailedToRespond = new ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond(this.InternalOnPlayersFailedToRespond),
				m_VTPlayersRefreshComplete = new ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete(this.InternalOnPlayersRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingPlayersResponse.VTable)));
			Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0000CB28 File Offset: 0x0000AD28
		~ISteamMatchmakingPlayersResponse()
		{
			if (this.m_pVTable != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pGCHandle.IsAllocated)
			{
				this.m_pGCHandle.Free();
			}
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0000CB8C File Offset: 0x0000AD8C
		private void InternalOnAddPlayerToList(IntPtr thisptr, IntPtr pchName, int nScore, float flTimePlayed)
		{
			this.m_AddPlayerToList(InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0000CBA2 File Offset: 0x0000ADA2
		private void InternalOnPlayersFailedToRespond(IntPtr thisptr)
		{
			this.m_PlayersFailedToRespond();
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0000CBAF File Offset: 0x0000ADAF
		private void InternalOnPlayersRefreshComplete(IntPtr thisptr)
		{
			this.m_PlayersRefreshComplete();
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0000CBBC File Offset: 0x0000ADBC
		public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000CB5 RID: 3253
		private ISteamMatchmakingPlayersResponse.VTable m_VTable;

		// Token: 0x04000CB6 RID: 3254
		private IntPtr m_pVTable;

		// Token: 0x04000CB7 RID: 3255
		private GCHandle m_pGCHandle;

		// Token: 0x04000CB8 RID: 3256
		private ISteamMatchmakingPlayersResponse.AddPlayerToList m_AddPlayerToList;

		// Token: 0x04000CB9 RID: 3257
		private ISteamMatchmakingPlayersResponse.PlayersFailedToRespond m_PlayersFailedToRespond;

		// Token: 0x04000CBA RID: 3258
		private ISteamMatchmakingPlayersResponse.PlayersRefreshComplete m_PlayersRefreshComplete;

		// Token: 0x020002DB RID: 731
		// (Invoke) Token: 0x06000CC0 RID: 3264
		public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

		// Token: 0x020002DC RID: 732
		// (Invoke) Token: 0x06000CC4 RID: 3268
		public delegate void PlayersFailedToRespond();

		// Token: 0x020002DD RID: 733
		// (Invoke) Token: 0x06000CC8 RID: 3272
		public delegate void PlayersRefreshComplete();

		// Token: 0x020002DE RID: 734
		// (Invoke) Token: 0x06000CCC RID: 3276
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalAddPlayerToList(IntPtr thisptr, IntPtr pchName, int nScore, float flTimePlayed);

		// Token: 0x020002DF RID: 735
		// (Invoke) Token: 0x06000CD0 RID: 3280
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalPlayersFailedToRespond(IntPtr thisptr);

		// Token: 0x020002E0 RID: 736
		// (Invoke) Token: 0x06000CD4 RID: 3284
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalPlayersRefreshComplete(IntPtr thisptr);

		// Token: 0x020002E1 RID: 737
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000CBB RID: 3259
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalAddPlayerToList m_VTAddPlayerToList;

			// Token: 0x04000CBC RID: 3260
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;

			// Token: 0x04000CBD RID: 3261
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
		}
	}
}
