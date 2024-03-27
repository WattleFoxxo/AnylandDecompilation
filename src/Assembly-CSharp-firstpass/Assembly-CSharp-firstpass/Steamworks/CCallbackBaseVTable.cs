using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002C1 RID: 705
	[StructLayout(LayoutKind.Sequential)]
	internal class CCallbackBaseVTable
	{
		// Token: 0x04000C9B RID: 3227
		private const CallingConvention cc = CallingConvention.Cdecl;

		// Token: 0x04000C9C RID: 3228
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.RunCRDel m_RunCallResult;

		// Token: 0x04000C9D RID: 3229
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.RunCBDel m_RunCallback;

		// Token: 0x04000C9E RID: 3230
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

		// Token: 0x020002C2 RID: 706
		// (Invoke) Token: 0x06000C65 RID: 3173
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void RunCBDel(IntPtr thisptr, IntPtr pvParam);

		// Token: 0x020002C3 RID: 707
		// (Invoke) Token: 0x06000C69 RID: 3177
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void RunCRDel(IntPtr thisptr, IntPtr pvParam, [MarshalAs(UnmanagedType.I1)] bool bIOFailure, ulong hSteamAPICall);

		// Token: 0x020002C4 RID: 708
		// (Invoke) Token: 0x06000C6D RID: 3181
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int GetCallbackSizeBytesDel(IntPtr thisptr);
	}
}
