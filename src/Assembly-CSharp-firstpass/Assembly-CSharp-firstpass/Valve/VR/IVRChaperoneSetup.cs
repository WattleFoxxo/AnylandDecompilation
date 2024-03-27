using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000068 RID: 104
	public struct IVRChaperoneSetup
	{
		// Token: 0x04000062 RID: 98
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._CommitWorkingCopy CommitWorkingCopy;

		// Token: 0x04000063 RID: 99
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._RevertWorkingCopy RevertWorkingCopy;

		// Token: 0x04000064 RID: 100
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingPlayAreaSize GetWorkingPlayAreaSize;

		// Token: 0x04000065 RID: 101
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingPlayAreaRect GetWorkingPlayAreaRect;

		// Token: 0x04000066 RID: 102
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingCollisionBoundsInfo GetWorkingCollisionBoundsInfo;

		// Token: 0x04000067 RID: 103
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetLiveCollisionBoundsInfo GetLiveCollisionBoundsInfo;

		// Token: 0x04000068 RID: 104
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingSeatedZeroPoseToRawTrackingPose GetWorkingSeatedZeroPoseToRawTrackingPose;

		// Token: 0x04000069 RID: 105
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingStandingZeroPoseToRawTrackingPose GetWorkingStandingZeroPoseToRawTrackingPose;

		// Token: 0x0400006A RID: 106
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingPlayAreaSize SetWorkingPlayAreaSize;

		// Token: 0x0400006B RID: 107
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingCollisionBoundsInfo SetWorkingCollisionBoundsInfo;

		// Token: 0x0400006C RID: 108
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingSeatedZeroPoseToRawTrackingPose SetWorkingSeatedZeroPoseToRawTrackingPose;

		// Token: 0x0400006D RID: 109
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingStandingZeroPoseToRawTrackingPose SetWorkingStandingZeroPoseToRawTrackingPose;

		// Token: 0x0400006E RID: 110
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._ReloadFromDisk ReloadFromDisk;

		// Token: 0x0400006F RID: 111
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetLiveSeatedZeroPoseToRawTrackingPose GetLiveSeatedZeroPoseToRawTrackingPose;

		// Token: 0x04000070 RID: 112
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingCollisionBoundsTagsInfo SetWorkingCollisionBoundsTagsInfo;

		// Token: 0x04000071 RID: 113
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetLiveCollisionBoundsTagsInfo GetLiveCollisionBoundsTagsInfo;

		// Token: 0x04000072 RID: 114
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingPhysicalBoundsInfo SetWorkingPhysicalBoundsInfo;

		// Token: 0x04000073 RID: 115
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetLivePhysicalBoundsInfo GetLivePhysicalBoundsInfo;

		// Token: 0x04000074 RID: 116
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._ExportLiveToBuffer ExportLiveToBuffer;

		// Token: 0x04000075 RID: 117
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._ImportFromBufferToWorking ImportFromBufferToWorking;

		// Token: 0x02000069 RID: 105
		// (Invoke) Token: 0x06000186 RID: 390
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _CommitWorkingCopy(EChaperoneConfigFile configFile);

		// Token: 0x0200006A RID: 106
		// (Invoke) Token: 0x0600018A RID: 394
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _RevertWorkingCopy();

		// Token: 0x0200006B RID: 107
		// (Invoke) Token: 0x0600018E RID: 398
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingPlayAreaSize(ref float pSizeX, ref float pSizeZ);

		// Token: 0x0200006C RID: 108
		// (Invoke) Token: 0x06000192 RID: 402
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingPlayAreaRect(ref HmdQuad_t rect);

		// Token: 0x0200006D RID: 109
		// (Invoke) Token: 0x06000196 RID: 406
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingCollisionBoundsInfo([In] [Out] HmdQuad_t[] pQuadsBuffer, ref uint punQuadsCount);

		// Token: 0x0200006E RID: 110
		// (Invoke) Token: 0x0600019A RID: 410
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetLiveCollisionBoundsInfo([In] [Out] HmdQuad_t[] pQuadsBuffer, ref uint punQuadsCount);

		// Token: 0x0200006F RID: 111
		// (Invoke) Token: 0x0600019E RID: 414
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatSeatedZeroPoseToRawTrackingPose);

		// Token: 0x02000070 RID: 112
		// (Invoke) Token: 0x060001A2 RID: 418
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingStandingZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatStandingZeroPoseToRawTrackingPose);

		// Token: 0x02000071 RID: 113
		// (Invoke) Token: 0x060001A6 RID: 422
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingPlayAreaSize(float sizeX, float sizeZ);

		// Token: 0x02000072 RID: 114
		// (Invoke) Token: 0x060001AA RID: 426
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingCollisionBoundsInfo([In] [Out] HmdQuad_t[] pQuadsBuffer, uint unQuadsCount);

		// Token: 0x02000073 RID: 115
		// (Invoke) Token: 0x060001AE RID: 430
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pMatSeatedZeroPoseToRawTrackingPose);

		// Token: 0x02000074 RID: 116
		// (Invoke) Token: 0x060001B2 RID: 434
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingStandingZeroPoseToRawTrackingPose(ref HmdMatrix34_t pMatStandingZeroPoseToRawTrackingPose);

		// Token: 0x02000075 RID: 117
		// (Invoke) Token: 0x060001B6 RID: 438
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ReloadFromDisk(EChaperoneConfigFile configFile);

		// Token: 0x02000076 RID: 118
		// (Invoke) Token: 0x060001BA RID: 442
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetLiveSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatSeatedZeroPoseToRawTrackingPose);

		// Token: 0x02000077 RID: 119
		// (Invoke) Token: 0x060001BE RID: 446
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingCollisionBoundsTagsInfo([In] [Out] byte[] pTagsBuffer, uint unTagCount);

		// Token: 0x02000078 RID: 120
		// (Invoke) Token: 0x060001C2 RID: 450
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetLiveCollisionBoundsTagsInfo([In] [Out] byte[] pTagsBuffer, ref uint punTagCount);

		// Token: 0x02000079 RID: 121
		// (Invoke) Token: 0x060001C6 RID: 454
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _SetWorkingPhysicalBoundsInfo([In] [Out] HmdQuad_t[] pQuadsBuffer, uint unQuadsCount);

		// Token: 0x0200007A RID: 122
		// (Invoke) Token: 0x060001CA RID: 458
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetLivePhysicalBoundsInfo([In] [Out] HmdQuad_t[] pQuadsBuffer, ref uint punQuadsCount);

		// Token: 0x0200007B RID: 123
		// (Invoke) Token: 0x060001CE RID: 462
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ExportLiveToBuffer(StringBuilder pBuffer, ref uint pnBufferLength);

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x060001D2 RID: 466
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ImportFromBufferToWorking(string pBuffer, uint nImportFlags);
	}
}
