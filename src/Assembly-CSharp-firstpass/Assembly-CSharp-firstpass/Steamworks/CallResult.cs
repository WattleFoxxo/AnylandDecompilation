using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002BE RID: 702
	public sealed class CallResult<T> : IDisposable
	{
		// Token: 0x06000C4F RID: 3151 RVA: 0x0000BEFF File Offset: 0x0000A0FF
		public CallResult(CallResult<T>.APIDispatchDelegate func = null)
		{
			this.m_Func = func;
			this.BuildCCallbackBase();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000C50 RID: 3152 RVA: 0x0000BF40 File Offset: 0x0000A140
		// (remove) Token: 0x06000C51 RID: 3153 RVA: 0x0000BF78 File Offset: 0x0000A178
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event CallResult<T>.APIDispatchDelegate m_Func;

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x0000BFAE File Offset: 0x0000A1AE
		public SteamAPICall_t Handle
		{
			get
			{
				return this.m_hAPICall;
			}
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0000BFB6 File Offset: 0x0000A1B6
		public static CallResult<T> Create(CallResult<T>.APIDispatchDelegate func = null)
		{
			return new CallResult<T>(func);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0000BFC0 File Offset: 0x0000A1C0
		~CallResult()
		{
			this.Dispose();
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0000BFF0 File Offset: 0x0000A1F0
		public void Dispose()
		{
			if (this.m_bDisposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			this.Cancel();
			if (this.m_pVTable != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pCCallbackBase.IsAllocated)
			{
				this.m_pCCallbackBase.Free();
			}
			this.m_bDisposed = true;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0000C058 File Offset: 0x0000A258
		public void Set(SteamAPICall_t hAPICall, CallResult<T>.APIDispatchDelegate func = null)
		{
			if (func != null)
			{
				this.m_Func = func;
			}
			if (this.m_Func == null)
			{
				throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
			}
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
			}
			this.m_hAPICall = hAPICall;
			if (hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_RegisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)hAPICall);
			}
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0000C0E5 File Offset: 0x0000A2E5
		public bool IsActive()
		{
			return this.m_hAPICall != SteamAPICall_t.Invalid;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0000C0F7 File Offset: 0x0000A2F7
		public void Cancel()
		{
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
				this.m_hAPICall = SteamAPICall_t.Invalid;
			}
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0000C134 File Offset: 0x0000A334
		public void SetGameserverFlag()
		{
			CCallbackBase ccallbackBase = this.m_CCallbackBase;
			ccallbackBase.m_nCallbackFlags |= 2;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0000C14C File Offset: 0x0000A34C
		private void OnRunCallback(IntPtr thisptr, IntPtr pvParam)
		{
			this.m_hAPICall = SteamAPICall_t.Invalid;
			try
			{
				this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))), false);
			}
			catch (Exception ex)
			{
				CallbackDispatcher.ExceptionHandler(ex);
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0000C1A8 File Offset: 0x0000A3A8
		private void OnRunCallResult(IntPtr thisptr, IntPtr pvParam, bool bFailed, ulong hSteamAPICall_)
		{
			SteamAPICall_t steamAPICall_t = (SteamAPICall_t)hSteamAPICall_;
			if (steamAPICall_t == this.m_hAPICall)
			{
				this.m_hAPICall = SteamAPICall_t.Invalid;
				try
				{
					this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))), bFailed);
				}
				catch (Exception ex)
				{
					CallbackDispatcher.ExceptionHandler(ex);
				}
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0000C21C File Offset: 0x0000A41C
		private int OnGetCallbackSizeBytes(IntPtr thisptr)
		{
			return this.m_size;
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0000C224 File Offset: 0x0000A424
		private void BuildCCallbackBase()
		{
			this.VTable = new CCallbackBaseVTable
			{
				m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
				m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
				m_GetCallbackSizeBytes = new CCallbackBaseVTable.GetCallbackSizeBytesDel(this.OnGetCallbackSizeBytes)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CCallbackBaseVTable)));
			Marshal.StructureToPtr(this.VTable, this.m_pVTable, false);
			this.m_CCallbackBase = new CCallbackBase
			{
				m_vfptr = this.m_pVTable,
				m_nCallbackFlags = 0,
				m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof(T))
			};
			this.m_pCCallbackBase = GCHandle.Alloc(this.m_CCallbackBase, GCHandleType.Pinned);
		}

		// Token: 0x04000C8E RID: 3214
		private CCallbackBaseVTable VTable;

		// Token: 0x04000C8F RID: 3215
		private IntPtr m_pVTable = IntPtr.Zero;

		// Token: 0x04000C90 RID: 3216
		private CCallbackBase m_CCallbackBase;

		// Token: 0x04000C91 RID: 3217
		private GCHandle m_pCCallbackBase;

		// Token: 0x04000C93 RID: 3219
		private SteamAPICall_t m_hAPICall = SteamAPICall_t.Invalid;

		// Token: 0x04000C94 RID: 3220
		private readonly int m_size = Marshal.SizeOf(typeof(T));

		// Token: 0x04000C95 RID: 3221
		private bool m_bDisposed;

		// Token: 0x020002BF RID: 703
		// (Invoke) Token: 0x06000C5F RID: 3167
		public delegate void APIDispatchDelegate(T param, bool bIOFailure);
	}
}
