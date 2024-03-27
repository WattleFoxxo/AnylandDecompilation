using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200007D RID: 125
	public struct IVRCompositor
	{
		// Token: 0x04000076 RID: 118
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._SetTrackingSpace SetTrackingSpace;

		// Token: 0x04000077 RID: 119
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetTrackingSpace GetTrackingSpace;

		// Token: 0x04000078 RID: 120
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._WaitGetPoses WaitGetPoses;

		// Token: 0x04000079 RID: 121
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetLastPoses GetLastPoses;

		// Token: 0x0400007A RID: 122
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetLastPoseForTrackedDeviceIndex GetLastPoseForTrackedDeviceIndex;

		// Token: 0x0400007B RID: 123
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._Submit Submit;

		// Token: 0x0400007C RID: 124
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ClearLastSubmittedFrame ClearLastSubmittedFrame;

		// Token: 0x0400007D RID: 125
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._PostPresentHandoff PostPresentHandoff;

		// Token: 0x0400007E RID: 126
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetFrameTiming GetFrameTiming;

		// Token: 0x0400007F RID: 127
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetFrameTimeRemaining GetFrameTimeRemaining;

		// Token: 0x04000080 RID: 128
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetCumulativeStats GetCumulativeStats;

		// Token: 0x04000081 RID: 129
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._FadeToColor FadeToColor;

		// Token: 0x04000082 RID: 130
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._FadeGrid FadeGrid;

		// Token: 0x04000083 RID: 131
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._SetSkyboxOverride SetSkyboxOverride;

		// Token: 0x04000084 RID: 132
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ClearSkyboxOverride ClearSkyboxOverride;

		// Token: 0x04000085 RID: 133
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CompositorBringToFront CompositorBringToFront;

		// Token: 0x04000086 RID: 134
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CompositorGoToBack CompositorGoToBack;

		// Token: 0x04000087 RID: 135
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CompositorQuit CompositorQuit;

		// Token: 0x04000088 RID: 136
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._IsFullscreen IsFullscreen;

		// Token: 0x04000089 RID: 137
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetCurrentSceneFocusProcess GetCurrentSceneFocusProcess;

		// Token: 0x0400008A RID: 138
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetLastFrameRenderer GetLastFrameRenderer;

		// Token: 0x0400008B RID: 139
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CanRenderScene CanRenderScene;

		// Token: 0x0400008C RID: 140
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ShowMirrorWindow ShowMirrorWindow;

		// Token: 0x0400008D RID: 141
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._HideMirrorWindow HideMirrorWindow;

		// Token: 0x0400008E RID: 142
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._IsMirrorWindowVisible IsMirrorWindowVisible;

		// Token: 0x0400008F RID: 143
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CompositorDumpImages CompositorDumpImages;

		// Token: 0x04000090 RID: 144
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ShouldAppRenderWithLowResources ShouldAppRenderWithLowResources;

		// Token: 0x04000091 RID: 145
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ForceInterleavedReprojectionOn ForceInterleavedReprojectionOn;

		// Token: 0x04000092 RID: 146
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ForceReconnectProcess ForceReconnectProcess;

		// Token: 0x04000093 RID: 147
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._SuspendRendering SuspendRendering;

		// Token: 0x04000094 RID: 148
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetMirrorTextureD3D11 GetMirrorTextureD3D11;

		// Token: 0x04000095 RID: 149
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetMirrorTextureGL GetMirrorTextureGL;

		// Token: 0x04000096 RID: 150
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ReleaseSharedGLTexture ReleaseSharedGLTexture;

		// Token: 0x04000097 RID: 151
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._LockGLSharedTextureForAccess LockGLSharedTextureForAccess;

		// Token: 0x04000098 RID: 152
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._UnlockGLSharedTextureForAccess UnlockGLSharedTextureForAccess;

		// Token: 0x0200007E RID: 126
		// (Invoke) Token: 0x060001D6 RID: 470
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetTrackingSpace(ETrackingUniverseOrigin eOrigin);

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x060001DA RID: 474
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ETrackingUniverseOrigin _GetTrackingSpace();

		// Token: 0x02000080 RID: 128
		// (Invoke) Token: 0x060001DE RID: 478
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _WaitGetPoses([In] [Out] TrackedDevicePose_t[] pRenderPoseArray, uint unRenderPoseArrayCount, [In] [Out] TrackedDevicePose_t[] pGamePoseArray, uint unGamePoseArrayCount);

		// Token: 0x02000081 RID: 129
		// (Invoke) Token: 0x060001E2 RID: 482
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _GetLastPoses([In] [Out] TrackedDevicePose_t[] pRenderPoseArray, uint unRenderPoseArrayCount, [In] [Out] TrackedDevicePose_t[] pGamePoseArray, uint unGamePoseArrayCount);

		// Token: 0x02000082 RID: 130
		// (Invoke) Token: 0x060001E6 RID: 486
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _GetLastPoseForTrackedDeviceIndex(uint unDeviceIndex, ref TrackedDevicePose_t pOutputPose, ref TrackedDevicePose_t pOutputGamePose);

		// Token: 0x02000083 RID: 131
		// (Invoke) Token: 0x060001EA RID: 490
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _Submit(EVREye eEye, ref Texture_t pTexture, ref VRTextureBounds_t pBounds, EVRSubmitFlags nSubmitFlags);

		// Token: 0x02000084 RID: 132
		// (Invoke) Token: 0x060001EE RID: 494
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ClearLastSubmittedFrame();

		// Token: 0x02000085 RID: 133
		// (Invoke) Token: 0x060001F2 RID: 498
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _PostPresentHandoff();

		// Token: 0x02000086 RID: 134
		// (Invoke) Token: 0x060001F6 RID: 502
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetFrameTiming(ref Compositor_FrameTiming pTiming, uint unFramesAgo);

		// Token: 0x02000087 RID: 135
		// (Invoke) Token: 0x060001FA RID: 506
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate float _GetFrameTimeRemaining();

		// Token: 0x02000088 RID: 136
		// (Invoke) Token: 0x060001FE RID: 510
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetCumulativeStats(ref Compositor_CumulativeStats pStats, uint nStatsSizeInBytes);

		// Token: 0x02000089 RID: 137
		// (Invoke) Token: 0x06000202 RID: 514
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FadeToColor(float fSeconds, float fRed, float fGreen, float fBlue, float fAlpha, bool bBackground);

		// Token: 0x0200008A RID: 138
		// (Invoke) Token: 0x06000206 RID: 518
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FadeGrid(float fSeconds, bool bFadeIn);

		// Token: 0x0200008B RID: 139
		// (Invoke) Token: 0x0600020A RID: 522
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _SetSkyboxOverride([In] [Out] Texture_t[] pTextures, uint unTextureCount);

		// Token: 0x0200008C RID: 140
		// (Invoke) Token: 0x0600020E RID: 526
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ClearSkyboxOverride();

		// Token: 0x0200008D RID: 141
		// (Invoke) Token: 0x06000212 RID: 530
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CompositorBringToFront();

		// Token: 0x0200008E RID: 142
		// (Invoke) Token: 0x06000216 RID: 534
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CompositorGoToBack();

		// Token: 0x0200008F RID: 143
		// (Invoke) Token: 0x0600021A RID: 538
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CompositorQuit();

		// Token: 0x02000090 RID: 144
		// (Invoke) Token: 0x0600021E RID: 542
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsFullscreen();

		// Token: 0x02000091 RID: 145
		// (Invoke) Token: 0x06000222 RID: 546
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetCurrentSceneFocusProcess();

		// Token: 0x02000092 RID: 146
		// (Invoke) Token: 0x06000226 RID: 550
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetLastFrameRenderer();

		// Token: 0x02000093 RID: 147
		// (Invoke) Token: 0x0600022A RID: 554
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _CanRenderScene();

		// Token: 0x02000094 RID: 148
		// (Invoke) Token: 0x0600022E RID: 558
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ShowMirrorWindow();

		// Token: 0x02000095 RID: 149
		// (Invoke) Token: 0x06000232 RID: 562
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _HideMirrorWindow();

		// Token: 0x02000096 RID: 150
		// (Invoke) Token: 0x06000236 RID: 566
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsMirrorWindowVisible();

		// Token: 0x02000097 RID: 151
		// (Invoke) Token: 0x0600023A RID: 570
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CompositorDumpImages();

		// Token: 0x02000098 RID: 152
		// (Invoke) Token: 0x0600023E RID: 574
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ShouldAppRenderWithLowResources();

		// Token: 0x02000099 RID: 153
		// (Invoke) Token: 0x06000242 RID: 578
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ForceInterleavedReprojectionOn(bool bOverride);

		// Token: 0x0200009A RID: 154
		// (Invoke) Token: 0x06000246 RID: 582
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ForceReconnectProcess();

		// Token: 0x0200009B RID: 155
		// (Invoke) Token: 0x0600024A RID: 586
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SuspendRendering(bool bSuspend);

		// Token: 0x0200009C RID: 156
		// (Invoke) Token: 0x0600024E RID: 590
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _GetMirrorTextureD3D11(EVREye eEye, IntPtr pD3D11DeviceOrResource, ref IntPtr ppD3D11ShaderResourceView);

		// Token: 0x0200009D RID: 157
		// (Invoke) Token: 0x06000252 RID: 594
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _GetMirrorTextureGL(EVREye eEye, ref uint pglTextureId, IntPtr pglSharedTextureHandle);

		// Token: 0x0200009E RID: 158
		// (Invoke) Token: 0x06000256 RID: 598
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ReleaseSharedGLTexture(uint glTextureId, IntPtr glSharedTextureHandle);

		// Token: 0x0200009F RID: 159
		// (Invoke) Token: 0x0600025A RID: 602
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _LockGLSharedTextureForAccess(IntPtr glSharedTextureHandle);

		// Token: 0x020000A0 RID: 160
		// (Invoke) Token: 0x0600025E RID: 606
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _UnlockGLSharedTextureForAccess(IntPtr glSharedTextureHandle);
	}
}
