using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002E2 RID: 738
	public class ISteamMatchmakingRulesResponse
	{
		// Token: 0x06000CD8 RID: 3288 RVA: 0x0000CBD4 File Offset: 0x0000ADD4
		public ISteamMatchmakingRulesResponse(ISteamMatchmakingRulesResponse.RulesResponded onRulesResponded, ISteamMatchmakingRulesResponse.RulesFailedToRespond onRulesFailedToRespond, ISteamMatchmakingRulesResponse.RulesRefreshComplete onRulesRefreshComplete)
		{
			if (onRulesResponded == null || onRulesFailedToRespond == null || onRulesRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_RulesResponded = onRulesResponded;
			this.m_RulesFailedToRespond = onRulesFailedToRespond;
			this.m_RulesRefreshComplete = onRulesRefreshComplete;
			this.m_VTable = new ISteamMatchmakingRulesResponse.VTable
			{
				m_VTRulesResponded = new ISteamMatchmakingRulesResponse.InternalRulesResponded(this.InternalOnRulesResponded),
				m_VTRulesFailedToRespond = new ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond(this.InternalOnRulesFailedToRespond),
				m_VTRulesRefreshComplete = new ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete(this.InternalOnRulesRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingRulesResponse.VTable)));
			Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0000CC9C File Offset: 0x0000AE9C
		~ISteamMatchmakingRulesResponse()
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

		// Token: 0x06000CDA RID: 3290 RVA: 0x0000CD00 File Offset: 0x0000AF00
		private void InternalOnRulesResponded(IntPtr thisptr, IntPtr pchRule, IntPtr pchValue)
		{
			this.m_RulesResponded(InteropHelp.PtrToStringUTF8(pchRule), InteropHelp.PtrToStringUTF8(pchValue));
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0000CD19 File Offset: 0x0000AF19
		private void InternalOnRulesFailedToRespond(IntPtr thisptr)
		{
			this.m_RulesFailedToRespond();
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0000CD26 File Offset: 0x0000AF26
		private void InternalOnRulesRefreshComplete(IntPtr thisptr)
		{
			this.m_RulesRefreshComplete();
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0000CD33 File Offset: 0x0000AF33
		public static explicit operator IntPtr(ISteamMatchmakingRulesResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000CBE RID: 3262
		private ISteamMatchmakingRulesResponse.VTable m_VTable;

		// Token: 0x04000CBF RID: 3263
		private IntPtr m_pVTable;

		// Token: 0x04000CC0 RID: 3264
		private GCHandle m_pGCHandle;

		// Token: 0x04000CC1 RID: 3265
		private ISteamMatchmakingRulesResponse.RulesResponded m_RulesResponded;

		// Token: 0x04000CC2 RID: 3266
		private ISteamMatchmakingRulesResponse.RulesFailedToRespond m_RulesFailedToRespond;

		// Token: 0x04000CC3 RID: 3267
		private ISteamMatchmakingRulesResponse.RulesRefreshComplete m_RulesRefreshComplete;

		// Token: 0x020002E3 RID: 739
		// (Invoke) Token: 0x06000CDF RID: 3295
		public delegate void RulesResponded(string pchRule, string pchValue);

		// Token: 0x020002E4 RID: 740
		// (Invoke) Token: 0x06000CE3 RID: 3299
		public delegate void RulesFailedToRespond();

		// Token: 0x020002E5 RID: 741
		// (Invoke) Token: 0x06000CE7 RID: 3303
		public delegate void RulesRefreshComplete();

		// Token: 0x020002E6 RID: 742
		// (Invoke) Token: 0x06000CEB RID: 3307
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalRulesResponded(IntPtr thisptr, IntPtr pchRule, IntPtr pchValue);

		// Token: 0x020002E7 RID: 743
		// (Invoke) Token: 0x06000CEF RID: 3311
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalRulesFailedToRespond(IntPtr thisptr);

		// Token: 0x020002E8 RID: 744
		// (Invoke) Token: 0x06000CF3 RID: 3315
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalRulesRefreshComplete(IntPtr thisptr);

		// Token: 0x020002E9 RID: 745
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000CC4 RID: 3268
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesResponded m_VTRulesResponded;

			// Token: 0x04000CC5 RID: 3269
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond m_VTRulesFailedToRespond;

			// Token: 0x04000CC6 RID: 3270
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete m_VTRulesRefreshComplete;
		}
	}
}
