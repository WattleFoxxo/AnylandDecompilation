using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000033 RID: 51
	public struct IVRTrackedCamera
	{
		// Token: 0x04000030 RID: 48
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetCameraErrorNameFromEnum GetCameraErrorNameFromEnum;

		// Token: 0x04000031 RID: 49
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._HasCamera HasCamera;

		// Token: 0x04000032 RID: 50
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetCameraFrameSize GetCameraFrameSize;

		// Token: 0x04000033 RID: 51
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetCameraIntrinisics GetCameraIntrinisics;

		// Token: 0x04000034 RID: 52
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetCameraProjection GetCameraProjection;

		// Token: 0x04000035 RID: 53
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._AcquireVideoStreamingService AcquireVideoStreamingService;

		// Token: 0x04000036 RID: 54
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._ReleaseVideoStreamingService ReleaseVideoStreamingService;

		// Token: 0x04000037 RID: 55
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetVideoStreamFrameBuffer GetVideoStreamFrameBuffer;

		// Token: 0x04000038 RID: 56
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetVideoStreamTextureSize GetVideoStreamTextureSize;

		// Token: 0x04000039 RID: 57
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetVideoStreamTextureD3D11 GetVideoStreamTextureD3D11;

		// Token: 0x0400003A RID: 58
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetVideoStreamTextureGL GetVideoStreamTextureGL;

		// Token: 0x0400003B RID: 59
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._ReleaseVideoStreamTextureGL ReleaseVideoStreamTextureGL;

		// Token: 0x02000034 RID: 52
		// (Invoke) Token: 0x060000BE RID: 190
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetCameraErrorNameFromEnum(EVRTrackedCameraError eCameraError);

		// Token: 0x02000035 RID: 53
		// (Invoke) Token: 0x060000C2 RID: 194
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _HasCamera(uint nDeviceIndex, ref bool pHasCamera);

		// Token: 0x02000036 RID: 54
		// (Invoke) Token: 0x060000C6 RID: 198
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetCameraFrameSize(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref uint pnWidth, ref uint pnHeight, ref uint pnFrameBufferSize);

		// Token: 0x02000037 RID: 55
		// (Invoke) Token: 0x060000CA RID: 202
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetCameraIntrinisics(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref HmdVector2_t pFocalLength, ref HmdVector2_t pCenter);

		// Token: 0x02000038 RID: 56
		// (Invoke) Token: 0x060000CE RID: 206
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetCameraProjection(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, float flZNear, float flZFar, ref HmdMatrix44_t pProjection);

		// Token: 0x02000039 RID: 57
		// (Invoke) Token: 0x060000D2 RID: 210
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _AcquireVideoStreamingService(uint nDeviceIndex, ref ulong pHandle);

		// Token: 0x0200003A RID: 58
		// (Invoke) Token: 0x060000D6 RID: 214
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _ReleaseVideoStreamingService(ulong hTrackedCamera);

		// Token: 0x0200003B RID: 59
		// (Invoke) Token: 0x060000DA RID: 218
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetVideoStreamFrameBuffer(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, IntPtr pFrameBuffer, uint nFrameBufferSize, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize);

		// Token: 0x0200003C RID: 60
		// (Invoke) Token: 0x060000DE RID: 222
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetVideoStreamTextureSize(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref VRTextureBounds_t pTextureBounds, ref uint pnWidth, ref uint pnHeight);

		// Token: 0x0200003D RID: 61
		// (Invoke) Token: 0x060000E2 RID: 226
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetVideoStreamTextureD3D11(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, IntPtr pD3D11DeviceOrResource, ref IntPtr ppD3D11ShaderResourceView, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize);

		// Token: 0x0200003E RID: 62
		// (Invoke) Token: 0x060000E6 RID: 230
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetVideoStreamTextureGL(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, ref uint pglTextureId, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize);

		// Token: 0x0200003F RID: 63
		// (Invoke) Token: 0x060000EA RID: 234
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _ReleaseVideoStreamTextureGL(ulong hTrackedCamera, uint glTextureId);
	}
}
