using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200002F RID: 47
	public struct IVRExtendedDisplay
	{
		// Token: 0x0400002D RID: 45
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRExtendedDisplay._GetWindowBounds GetWindowBounds;

		// Token: 0x0400002E RID: 46
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRExtendedDisplay._GetEyeOutputViewport GetEyeOutputViewport;

		// Token: 0x0400002F RID: 47
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRExtendedDisplay._GetDXGIOutputInfo GetDXGIOutputInfo;

		// Token: 0x02000030 RID: 48
		// (Invoke) Token: 0x060000B2 RID: 178
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetWindowBounds(ref int pnX, ref int pnY, ref uint pnWidth, ref uint pnHeight);

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x060000B6 RID: 182
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetEyeOutputViewport(EVREye eEye, ref uint pnX, ref uint pnY, ref uint pnWidth, ref uint pnHeight);

		// Token: 0x02000032 RID: 50
		// (Invoke) Token: 0x060000BA RID: 186
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetDXGIOutputInfo(ref int pnAdapterIndex, ref int pnAdapterOutputIndex);
	}
}
