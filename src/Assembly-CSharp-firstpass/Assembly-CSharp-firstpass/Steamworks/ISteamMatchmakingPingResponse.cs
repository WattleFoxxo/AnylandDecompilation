using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002D4 RID: 724
	public class ISteamMatchmakingPingResponse
	{
		// Token: 0x06000CA3 RID: 3235 RVA: 0x0000C924 File Offset: 0x0000AB24
		public ISteamMatchmakingPingResponse(ISteamMatchmakingPingResponse.ServerResponded onServerResponded, ISteamMatchmakingPingResponse.ServerFailedToRespond onServerFailedToRespond)
		{
			if (onServerResponded == null || onServerFailedToRespond == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_VTable = new ISteamMatchmakingPingResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingPingResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingPingResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingPingResponse.VTable)));
			Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		~ISteamMatchmakingPingResponse()
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

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0000CA30 File Offset: 0x0000AC30
		private void InternalOnServerResponded(IntPtr thisptr, gameserveritem_t server)
		{
			this.m_ServerResponded(server);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0000CA3E File Offset: 0x0000AC3E
		private void InternalOnServerFailedToRespond(IntPtr thisptr)
		{
			this.m_ServerFailedToRespond();
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0000CA4B File Offset: 0x0000AC4B
		public static explicit operator IntPtr(ISteamMatchmakingPingResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000CAE RID: 3246
		private ISteamMatchmakingPingResponse.VTable m_VTable;

		// Token: 0x04000CAF RID: 3247
		private IntPtr m_pVTable;

		// Token: 0x04000CB0 RID: 3248
		private GCHandle m_pGCHandle;

		// Token: 0x04000CB1 RID: 3249
		private ISteamMatchmakingPingResponse.ServerResponded m_ServerResponded;

		// Token: 0x04000CB2 RID: 3250
		private ISteamMatchmakingPingResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x020002D5 RID: 725
		// (Invoke) Token: 0x06000CA9 RID: 3241
		public delegate void ServerResponded(gameserveritem_t server);

		// Token: 0x020002D6 RID: 726
		// (Invoke) Token: 0x06000CAD RID: 3245
		public delegate void ServerFailedToRespond();

		// Token: 0x020002D7 RID: 727
		// (Invoke) Token: 0x06000CB1 RID: 3249
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(IntPtr thisptr, gameserveritem_t server);

		// Token: 0x020002D8 RID: 728
		// (Invoke) Token: 0x06000CB5 RID: 3253
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(IntPtr thisptr);

		// Token: 0x020002D9 RID: 729
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000CB3 RID: 3251
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000CB4 RID: 3252
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
		}
	}
}
