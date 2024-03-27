using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002BC RID: 700
	public sealed class Callback<T> : IDisposable
	{
		// Token: 0x06000C3D RID: 3133 RVA: 0x0000BBA5 File Offset: 0x00009DA5
		public Callback(Callback<T>.DispatchDelegate func, bool bGameServer = false)
		{
			this.m_bGameServer = bGameServer;
			this.BuildCCallbackBase();
			this.Register(func);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000C3E RID: 3134 RVA: 0x0000BBE4 File Offset: 0x00009DE4
		// (remove) Token: 0x06000C3F RID: 3135 RVA: 0x0000BC1C File Offset: 0x00009E1C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Callback<T>.DispatchDelegate m_Func;

		// Token: 0x06000C40 RID: 3136 RVA: 0x0000BC52 File Offset: 0x00009E52
		public static Callback<T> Create(Callback<T>.DispatchDelegate func)
		{
			return new Callback<T>(func, false);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0000BC5B File Offset: 0x00009E5B
		public static Callback<T> CreateGameServer(Callback<T>.DispatchDelegate func)
		{
			return new Callback<T>(func, true);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0000BC64 File Offset: 0x00009E64
		~Callback()
		{
			this.Dispose();
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0000BC94 File Offset: 0x00009E94
		public void Dispose()
		{
			if (this.m_bDisposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			this.Unregister();
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

		// Token: 0x06000C44 RID: 3140 RVA: 0x0000BCFC File Offset: 0x00009EFC
		public void Register(Callback<T>.DispatchDelegate func)
		{
			if (func == null)
			{
				throw new Exception("Callback function must not be null.");
			}
			if ((this.m_CCallbackBase.m_nCallbackFlags & 1) == 1)
			{
				this.Unregister();
			}
			if (this.m_bGameServer)
			{
				this.SetGameserverFlag();
			}
			this.m_Func = func;
			NativeMethods.SteamAPI_RegisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject(), CallbackIdentities.GetCallbackIdentity(typeof(T)));
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0000BD6A File Offset: 0x00009F6A
		public void Unregister()
		{
			NativeMethods.SteamAPI_UnregisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject());
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0000BD7C File Offset: 0x00009F7C
		public void SetGameserverFlag()
		{
			CCallbackBase ccallbackBase = this.m_CCallbackBase;
			ccallbackBase.m_nCallbackFlags |= 2;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0000BD94 File Offset: 0x00009F94
		private void OnRunCallback(IntPtr thisptr, IntPtr pvParam)
		{
			try
			{
				this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))));
			}
			catch (Exception ex)
			{
				CallbackDispatcher.ExceptionHandler(ex);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0000BDE4 File Offset: 0x00009FE4
		private void OnRunCallResult(IntPtr thisptr, IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
		{
			try
			{
				this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))));
			}
			catch (Exception ex)
			{
				CallbackDispatcher.ExceptionHandler(ex);
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0000BE34 File Offset: 0x0000A034
		private int OnGetCallbackSizeBytes(IntPtr thisptr)
		{
			return this.m_size;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0000BE3C File Offset: 0x0000A03C
		private void BuildCCallbackBase()
		{
			this.VTable = new CCallbackBaseVTable
			{
				m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
				m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
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

		// Token: 0x04000C86 RID: 3206
		private CCallbackBaseVTable VTable;

		// Token: 0x04000C87 RID: 3207
		private IntPtr m_pVTable = IntPtr.Zero;

		// Token: 0x04000C88 RID: 3208
		private CCallbackBase m_CCallbackBase;

		// Token: 0x04000C89 RID: 3209
		private GCHandle m_pCCallbackBase;

		// Token: 0x04000C8B RID: 3211
		private bool m_bGameServer;

		// Token: 0x04000C8C RID: 3212
		private readonly int m_size = Marshal.SizeOf(typeof(T));

		// Token: 0x04000C8D RID: 3213
		private bool m_bDisposed;

		// Token: 0x020002BD RID: 701
		// (Invoke) Token: 0x06000C4C RID: 3148
		public delegate void DispatchDelegate(T param);
	}
}
