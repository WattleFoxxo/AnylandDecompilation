using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200005F RID: 95
	public struct IVRChaperone
	{
		// Token: 0x0400005A RID: 90
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._GetCalibrationState GetCalibrationState;

		// Token: 0x0400005B RID: 91
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._GetPlayAreaSize GetPlayAreaSize;

		// Token: 0x0400005C RID: 92
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._GetPlayAreaRect GetPlayAreaRect;

		// Token: 0x0400005D RID: 93
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._ReloadInfo ReloadInfo;

		// Token: 0x0400005E RID: 94
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._SetSceneColor SetSceneColor;

		// Token: 0x0400005F RID: 95
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._GetBoundsColor GetBoundsColor;

		// Token: 0x04000060 RID: 96
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._AreBoundsVisible AreBoundsVisible;

		// Token: 0x04000061 RID: 97
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._ForceBoundsVisible ForceBoundsVisible;

		// Token: 0x02000060 RID: 96
		// (Invoke) Token: 0x06000166 RID: 358
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ChaperoneCalibrationState _GetCalibrationState();

		// Token: 0x02000061 RID: 97
		// (Invoke) Token: 0x0600016A RID: 362
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetPlayAreaSize(ref float pSizeX, ref float pSizeZ);

		// Token: 0x02000062 RID: 98
		// (Invoke) Token: 0x0600016E RID: 366
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetPlayAreaRect(ref HmdQuad_t rect);

		// Token: 0x02000063 RID: 99
		// (Invoke) Token: 0x06000172 RID: 370
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ReloadInfo();

		// Token: 0x02000064 RID: 100
		// (Invoke) Token: 0x06000176 RID: 374
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetSceneColor(HmdColor_t color);

		// Token: 0x02000065 RID: 101
		// (Invoke) Token: 0x0600017A RID: 378
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetBoundsColor(ref HmdColor_t pOutputColorArray, int nNumOutputColors, float flCollisionBoundsFadeDistance, ref HmdColor_t pOutputCameraColor);

		// Token: 0x02000066 RID: 102
		// (Invoke) Token: 0x0600017E RID: 382
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _AreBoundsVisible();

		// Token: 0x02000067 RID: 103
		// (Invoke) Token: 0x06000182 RID: 386
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ForceBoundsVisible(bool bForce);
	}
}
