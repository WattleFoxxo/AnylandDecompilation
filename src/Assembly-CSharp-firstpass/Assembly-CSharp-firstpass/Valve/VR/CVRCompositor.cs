using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200011E RID: 286
	public class CVRCompositor
	{
		// Token: 0x060004A0 RID: 1184 RVA: 0x000030ED File Offset: 0x000012ED
		internal CVRCompositor(IntPtr pInterface)
		{
			this.FnTable = (IVRCompositor)Marshal.PtrToStructure(pInterface, typeof(IVRCompositor));
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00003110 File Offset: 0x00001310
		public void SetTrackingSpace(ETrackingUniverseOrigin eOrigin)
		{
			this.FnTable.SetTrackingSpace(eOrigin);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00003124 File Offset: 0x00001324
		public ETrackingUniverseOrigin GetTrackingSpace()
		{
			return this.FnTable.GetTrackingSpace();
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00003144 File Offset: 0x00001344
		public EVRCompositorError WaitGetPoses(TrackedDevicePose_t[] pRenderPoseArray, TrackedDevicePose_t[] pGamePoseArray)
		{
			return this.FnTable.WaitGetPoses(pRenderPoseArray, (uint)pRenderPoseArray.Length, pGamePoseArray, (uint)pGamePoseArray.Length);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000316C File Offset: 0x0000136C
		public EVRCompositorError GetLastPoses(TrackedDevicePose_t[] pRenderPoseArray, TrackedDevicePose_t[] pGamePoseArray)
		{
			return this.FnTable.GetLastPoses(pRenderPoseArray, (uint)pRenderPoseArray.Length, pGamePoseArray, (uint)pGamePoseArray.Length);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00003194 File Offset: 0x00001394
		public EVRCompositorError GetLastPoseForTrackedDeviceIndex(uint unDeviceIndex, ref TrackedDevicePose_t pOutputPose, ref TrackedDevicePose_t pOutputGamePose)
		{
			return this.FnTable.GetLastPoseForTrackedDeviceIndex(unDeviceIndex, ref pOutputPose, ref pOutputGamePose);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000031B8 File Offset: 0x000013B8
		public EVRCompositorError Submit(EVREye eEye, ref Texture_t pTexture, ref VRTextureBounds_t pBounds, EVRSubmitFlags nSubmitFlags)
		{
			return this.FnTable.Submit(eEye, ref pTexture, ref pBounds, nSubmitFlags);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000031DC File Offset: 0x000013DC
		public void ClearLastSubmittedFrame()
		{
			this.FnTable.ClearLastSubmittedFrame();
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000031EE File Offset: 0x000013EE
		public void PostPresentHandoff()
		{
			this.FnTable.PostPresentHandoff();
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00003200 File Offset: 0x00001400
		public bool GetFrameTiming(ref Compositor_FrameTiming pTiming, uint unFramesAgo)
		{
			return this.FnTable.GetFrameTiming(ref pTiming, unFramesAgo);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00003224 File Offset: 0x00001424
		public float GetFrameTimeRemaining()
		{
			return this.FnTable.GetFrameTimeRemaining();
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00003243 File Offset: 0x00001443
		public void GetCumulativeStats(ref Compositor_CumulativeStats pStats, uint nStatsSizeInBytes)
		{
			this.FnTable.GetCumulativeStats(ref pStats, nStatsSizeInBytes);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00003257 File Offset: 0x00001457
		public void FadeToColor(float fSeconds, float fRed, float fGreen, float fBlue, float fAlpha, bool bBackground)
		{
			this.FnTable.FadeToColor(fSeconds, fRed, fGreen, fBlue, fAlpha, bBackground);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00003272 File Offset: 0x00001472
		public void FadeGrid(float fSeconds, bool bFadeIn)
		{
			this.FnTable.FadeGrid(fSeconds, bFadeIn);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00003288 File Offset: 0x00001488
		public EVRCompositorError SetSkyboxOverride(Texture_t[] pTextures)
		{
			return this.FnTable.SetSkyboxOverride(pTextures, (uint)pTextures.Length);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000032AB File Offset: 0x000014AB
		public void ClearSkyboxOverride()
		{
			this.FnTable.ClearSkyboxOverride();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000032BD File Offset: 0x000014BD
		public void CompositorBringToFront()
		{
			this.FnTable.CompositorBringToFront();
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000032CF File Offset: 0x000014CF
		public void CompositorGoToBack()
		{
			this.FnTable.CompositorGoToBack();
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000032E1 File Offset: 0x000014E1
		public void CompositorQuit()
		{
			this.FnTable.CompositorQuit();
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000032F4 File Offset: 0x000014F4
		public bool IsFullscreen()
		{
			return this.FnTable.IsFullscreen();
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00003314 File Offset: 0x00001514
		public uint GetCurrentSceneFocusProcess()
		{
			return this.FnTable.GetCurrentSceneFocusProcess();
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00003334 File Offset: 0x00001534
		public uint GetLastFrameRenderer()
		{
			return this.FnTable.GetLastFrameRenderer();
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00003354 File Offset: 0x00001554
		public bool CanRenderScene()
		{
			return this.FnTable.CanRenderScene();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00003373 File Offset: 0x00001573
		public void ShowMirrorWindow()
		{
			this.FnTable.ShowMirrorWindow();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00003385 File Offset: 0x00001585
		public void HideMirrorWindow()
		{
			this.FnTable.HideMirrorWindow();
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00003398 File Offset: 0x00001598
		public bool IsMirrorWindowVisible()
		{
			return this.FnTable.IsMirrorWindowVisible();
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000033B7 File Offset: 0x000015B7
		public void CompositorDumpImages()
		{
			this.FnTable.CompositorDumpImages();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000033CC File Offset: 0x000015CC
		public bool ShouldAppRenderWithLowResources()
		{
			return this.FnTable.ShouldAppRenderWithLowResources();
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000033EB File Offset: 0x000015EB
		public void ForceInterleavedReprojectionOn(bool bOverride)
		{
			this.FnTable.ForceInterleavedReprojectionOn(bOverride);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000033FE File Offset: 0x000015FE
		public void ForceReconnectProcess()
		{
			this.FnTable.ForceReconnectProcess();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00003410 File Offset: 0x00001610
		public void SuspendRendering(bool bSuspend)
		{
			this.FnTable.SuspendRendering(bSuspend);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00003424 File Offset: 0x00001624
		public EVRCompositorError GetMirrorTextureD3D11(EVREye eEye, IntPtr pD3D11DeviceOrResource, ref IntPtr ppD3D11ShaderResourceView)
		{
			return this.FnTable.GetMirrorTextureD3D11(eEye, pD3D11DeviceOrResource, ref ppD3D11ShaderResourceView);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00003448 File Offset: 0x00001648
		public EVRCompositorError GetMirrorTextureGL(EVREye eEye, ref uint pglTextureId, IntPtr pglSharedTextureHandle)
		{
			pglTextureId = 0U;
			return this.FnTable.GetMirrorTextureGL(eEye, ref pglTextureId, pglSharedTextureHandle);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00003470 File Offset: 0x00001670
		public bool ReleaseSharedGLTexture(uint glTextureId, IntPtr glSharedTextureHandle)
		{
			return this.FnTable.ReleaseSharedGLTexture(glTextureId, glSharedTextureHandle);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00003491 File Offset: 0x00001691
		public void LockGLSharedTextureForAccess(IntPtr glSharedTextureHandle)
		{
			this.FnTable.LockGLSharedTextureForAccess(glSharedTextureHandle);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000034A4 File Offset: 0x000016A4
		public void UnlockGLSharedTextureForAccess(IntPtr glSharedTextureHandle)
		{
			this.FnTable.UnlockGLSharedTextureForAccess(glSharedTextureHandle);
		}

		// Token: 0x04000110 RID: 272
		private IVRCompositor FnTable;
	}
}
