using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002CC RID: 716
	public class ISteamMatchmakingServerListResponse
	{
		// Token: 0x06000C84 RID: 3204 RVA: 0x0000C7B4 File Offset: 0x0000A9B4
		public ISteamMatchmakingServerListResponse(ISteamMatchmakingServerListResponse.ServerResponded onServerResponded, ISteamMatchmakingServerListResponse.ServerFailedToRespond onServerFailedToRespond, ISteamMatchmakingServerListResponse.RefreshComplete onRefreshComplete)
		{
			if (onServerResponded == null || onServerFailedToRespond == null || onRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_RefreshComplete = onRefreshComplete;
			this.m_VTable = new ISteamMatchmakingServerListResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingServerListResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingServerListResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond),
				m_VTRefreshComplete = new ISteamMatchmakingServerListResponse.InternalRefreshComplete(this.InternalOnRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingServerListResponse.VTable)));
			Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0000C87C File Offset: 0x0000AA7C
		~ISteamMatchmakingServerListResponse()
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

		// Token: 0x06000C86 RID: 3206 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		private void InternalOnServerResponded(IntPtr thisptr, HServerListRequest hRequest, int iServer)
		{
			this.m_ServerResponded(hRequest, iServer);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0000C8EF File Offset: 0x0000AAEF
		private void InternalOnServerFailedToRespond(IntPtr thisptr, HServerListRequest hRequest, int iServer)
		{
			this.m_ServerFailedToRespond(hRequest, iServer);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0000C8FE File Offset: 0x0000AAFE
		private void InternalOnRefreshComplete(IntPtr thisptr, HServerListRequest hRequest, EMatchMakingServerResponse response)
		{
			this.m_RefreshComplete(hRequest, response);
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0000C90D File Offset: 0x0000AB0D
		public static explicit operator IntPtr(ISteamMatchmakingServerListResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000CA5 RID: 3237
		private ISteamMatchmakingServerListResponse.VTable m_VTable;

		// Token: 0x04000CA6 RID: 3238
		private IntPtr m_pVTable;

		// Token: 0x04000CA7 RID: 3239
		private GCHandle m_pGCHandle;

		// Token: 0x04000CA8 RID: 3240
		private ISteamMatchmakingServerListResponse.ServerResponded m_ServerResponded;

		// Token: 0x04000CA9 RID: 3241
		private ISteamMatchmakingServerListResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x04000CAA RID: 3242
		private ISteamMatchmakingServerListResponse.RefreshComplete m_RefreshComplete;

		// Token: 0x020002CD RID: 717
		// (Invoke) Token: 0x06000C8B RID: 3211
		public delegate void ServerResponded(HServerListRequest hRequest, int iServer);

		// Token: 0x020002CE RID: 718
		// (Invoke) Token: 0x06000C8F RID: 3215
		public delegate void ServerFailedToRespond(HServerListRequest hRequest, int iServer);

		// Token: 0x020002CF RID: 719
		// (Invoke) Token: 0x06000C93 RID: 3219
		public delegate void RefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

		// Token: 0x020002D0 RID: 720
		// (Invoke) Token: 0x06000C97 RID: 3223
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(IntPtr thisptr, HServerListRequest hRequest, int iServer);

		// Token: 0x020002D1 RID: 721
		// (Invoke) Token: 0x06000C9B RID: 3227
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(IntPtr thisptr, HServerListRequest hRequest, int iServer);

		// Token: 0x020002D2 RID: 722
		// (Invoke) Token: 0x06000C9F RID: 3231
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalRefreshComplete(IntPtr thisptr, HServerListRequest hRequest, EMatchMakingServerResponse response);

		// Token: 0x020002D3 RID: 723
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000CAB RID: 3243
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000CAC RID: 3244
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;

			// Token: 0x04000CAD RID: 3245
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalRefreshComplete m_VTRefreshComplete;
		}
	}
}
