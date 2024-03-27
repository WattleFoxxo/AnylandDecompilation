using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200011A RID: 282
	public class CVRTrackedCamera
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x000026A8 File Offset: 0x000008A8
		internal CVRTrackedCamera(IntPtr pInterface)
		{
			this.FnTable = (IVRTrackedCamera)Marshal.PtrToStructure(pInterface, typeof(IVRTrackedCamera));
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000026CC File Offset: 0x000008CC
		public string GetCameraErrorNameFromEnum(EVRTrackedCameraError eCameraError)
		{
			IntPtr intPtr = this.FnTable.GetCameraErrorNameFromEnum(eCameraError);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000026F4 File Offset: 0x000008F4
		public EVRTrackedCameraError HasCamera(uint nDeviceIndex, ref bool pHasCamera)
		{
			pHasCamera = false;
			return this.FnTable.HasCamera(nDeviceIndex, ref pHasCamera);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00002718 File Offset: 0x00000918
		public EVRTrackedCameraError GetCameraFrameSize(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref uint pnWidth, ref uint pnHeight, ref uint pnFrameBufferSize)
		{
			pnWidth = 0U;
			pnHeight = 0U;
			pnFrameBufferSize = 0U;
			return this.FnTable.GetCameraFrameSize(nDeviceIndex, eFrameType, ref pnWidth, ref pnHeight, ref pnFrameBufferSize);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000274C File Offset: 0x0000094C
		public EVRTrackedCameraError GetCameraIntrinisics(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref HmdVector2_t pFocalLength, ref HmdVector2_t pCenter)
		{
			return this.FnTable.GetCameraIntrinisics(nDeviceIndex, eFrameType, ref pFocalLength, ref pCenter);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00002770 File Offset: 0x00000970
		public EVRTrackedCameraError GetCameraProjection(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, float flZNear, float flZFar, ref HmdMatrix44_t pProjection)
		{
			return this.FnTable.GetCameraProjection(nDeviceIndex, eFrameType, flZNear, flZFar, ref pProjection);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00002798 File Offset: 0x00000998
		public EVRTrackedCameraError AcquireVideoStreamingService(uint nDeviceIndex, ref ulong pHandle)
		{
			pHandle = 0UL;
			return this.FnTable.AcquireVideoStreamingService(nDeviceIndex, ref pHandle);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000027C0 File Offset: 0x000009C0
		public EVRTrackedCameraError ReleaseVideoStreamingService(ulong hTrackedCamera)
		{
			return this.FnTable.ReleaseVideoStreamingService(hTrackedCamera);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000027E0 File Offset: 0x000009E0
		public EVRTrackedCameraError GetVideoStreamFrameBuffer(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, IntPtr pFrameBuffer, uint nFrameBufferSize, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize)
		{
			return this.FnTable.GetVideoStreamFrameBuffer(hTrackedCamera, eFrameType, pFrameBuffer, nFrameBufferSize, ref pFrameHeader, nFrameHeaderSize);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00002808 File Offset: 0x00000A08
		public EVRTrackedCameraError GetVideoStreamTextureSize(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref VRTextureBounds_t pTextureBounds, ref uint pnWidth, ref uint pnHeight)
		{
			pnWidth = 0U;
			pnHeight = 0U;
			return this.FnTable.GetVideoStreamTextureSize(nDeviceIndex, eFrameType, ref pTextureBounds, ref pnWidth, ref pnHeight);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00002838 File Offset: 0x00000A38
		public EVRTrackedCameraError GetVideoStreamTextureD3D11(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, IntPtr pD3D11DeviceOrResource, ref IntPtr ppD3D11ShaderResourceView, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize)
		{
			return this.FnTable.GetVideoStreamTextureD3D11(hTrackedCamera, eFrameType, pD3D11DeviceOrResource, ref ppD3D11ShaderResourceView, ref pFrameHeader, nFrameHeaderSize);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00002860 File Offset: 0x00000A60
		public EVRTrackedCameraError GetVideoStreamTextureGL(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, ref uint pglTextureId, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize)
		{
			pglTextureId = 0U;
			return this.FnTable.GetVideoStreamTextureGL(hTrackedCamera, eFrameType, ref pglTextureId, ref pFrameHeader, nFrameHeaderSize);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000288C File Offset: 0x00000A8C
		public EVRTrackedCameraError ReleaseVideoStreamTextureGL(ulong hTrackedCamera, uint glTextureId)
		{
			return this.FnTable.ReleaseVideoStreamTextureGL(hTrackedCamera, glTextureId);
		}

		// Token: 0x0400010C RID: 268
		private IVRTrackedCamera FnTable;
	}
}
