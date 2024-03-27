using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

// Token: 0x020002B1 RID: 689
public class SteamVR_TrackedCamera
{
	// Token: 0x060019BF RID: 6591 RVA: 0x000EAEA0 File Offset: 0x000E92A0
	public static SteamVR_TrackedCamera.VideoStreamTexture Distorted(int deviceIndex = 0)
	{
		if (SteamVR_TrackedCamera.distorted == null)
		{
			SteamVR_TrackedCamera.distorted = new SteamVR_TrackedCamera.VideoStreamTexture[16];
		}
		if (SteamVR_TrackedCamera.distorted[deviceIndex] == null)
		{
			SteamVR_TrackedCamera.distorted[deviceIndex] = new SteamVR_TrackedCamera.VideoStreamTexture((uint)deviceIndex, false);
		}
		return SteamVR_TrackedCamera.distorted[deviceIndex];
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000EAEDA File Offset: 0x000E92DA
	public static SteamVR_TrackedCamera.VideoStreamTexture Undistorted(int deviceIndex = 0)
	{
		if (SteamVR_TrackedCamera.undistorted == null)
		{
			SteamVR_TrackedCamera.undistorted = new SteamVR_TrackedCamera.VideoStreamTexture[16];
		}
		if (SteamVR_TrackedCamera.undistorted[deviceIndex] == null)
		{
			SteamVR_TrackedCamera.undistorted[deviceIndex] = new SteamVR_TrackedCamera.VideoStreamTexture((uint)deviceIndex, true);
		}
		return SteamVR_TrackedCamera.undistorted[deviceIndex];
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x000EAF14 File Offset: 0x000E9314
	public static SteamVR_TrackedCamera.VideoStreamTexture Source(bool undistorted, int deviceIndex = 0)
	{
		return (!undistorted) ? SteamVR_TrackedCamera.Distorted(deviceIndex) : SteamVR_TrackedCamera.Undistorted(deviceIndex);
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x000EAF2D File Offset: 0x000E932D
	private static SteamVR_TrackedCamera.VideoStream Stream(uint deviceIndex)
	{
		if (SteamVR_TrackedCamera.videostreams == null)
		{
			SteamVR_TrackedCamera.videostreams = new SteamVR_TrackedCamera.VideoStream[16];
		}
		if (SteamVR_TrackedCamera.videostreams[(int)((UIntPtr)deviceIndex)] == null)
		{
			SteamVR_TrackedCamera.videostreams[(int)((UIntPtr)deviceIndex)] = new SteamVR_TrackedCamera.VideoStream(deviceIndex);
		}
		return SteamVR_TrackedCamera.videostreams[(int)((UIntPtr)deviceIndex)];
	}

	// Token: 0x04001808 RID: 6152
	private static SteamVR_TrackedCamera.VideoStreamTexture[] distorted;

	// Token: 0x04001809 RID: 6153
	private static SteamVR_TrackedCamera.VideoStreamTexture[] undistorted;

	// Token: 0x0400180A RID: 6154
	private static SteamVR_TrackedCamera.VideoStream[] videostreams;

	// Token: 0x020002B2 RID: 690
	public class VideoStreamTexture
	{
		// Token: 0x060019C3 RID: 6595 RVA: 0x000EAF69 File Offset: 0x000E9369
		public VideoStreamTexture(uint deviceIndex, bool undistorted)
		{
			this.undistorted = undistorted;
			this.videostream = SteamVR_TrackedCamera.Stream(deviceIndex);
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x000EAF8B File Offset: 0x000E938B
		// (set) Token: 0x060019C5 RID: 6597 RVA: 0x000EAF93 File Offset: 0x000E9393
		public bool undistorted { get; private set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x000EAF9C File Offset: 0x000E939C
		public uint deviceIndex
		{
			get
			{
				return this.videostream.deviceIndex;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x000EAFA9 File Offset: 0x000E93A9
		public bool hasCamera
		{
			get
			{
				return this.videostream.hasCamera;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060019C8 RID: 6600 RVA: 0x000EAFB6 File Offset: 0x000E93B6
		public bool hasTracking
		{
			get
			{
				this.Update();
				return this.header.standingTrackedDevicePose.bPoseIsValid;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x000EAFCE File Offset: 0x000E93CE
		public uint frameId
		{
			get
			{
				this.Update();
				return this.header.nFrameSequence;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x000EAFE1 File Offset: 0x000E93E1
		// (set) Token: 0x060019CB RID: 6603 RVA: 0x000EAFE9 File Offset: 0x000E93E9
		public VRTextureBounds_t frameBounds { get; private set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060019CC RID: 6604 RVA: 0x000EAFF2 File Offset: 0x000E93F2
		public EVRTrackedCameraFrameType frameType
		{
			get
			{
				return (!this.undistorted) ? EVRTrackedCameraFrameType.Distorted : EVRTrackedCameraFrameType.Undistorted;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x000EB006 File Offset: 0x000E9406
		public Texture2D texture
		{
			get
			{
				this.Update();
				return this._texture;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x000EB014 File Offset: 0x000E9414
		public SteamVR_Utils.RigidTransform transform
		{
			get
			{
				this.Update();
				return new SteamVR_Utils.RigidTransform(this.header.standingTrackedDevicePose.mDeviceToAbsoluteTracking);
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x000EB034 File Offset: 0x000E9434
		public Vector3 velocity
		{
			get
			{
				this.Update();
				TrackedDevicePose_t standingTrackedDevicePose = this.header.standingTrackedDevicePose;
				return new Vector3(standingTrackedDevicePose.vVelocity.v0, standingTrackedDevicePose.vVelocity.v1, -standingTrackedDevicePose.vVelocity.v2);
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x000EB080 File Offset: 0x000E9480
		public Vector3 angularVelocity
		{
			get
			{
				this.Update();
				TrackedDevicePose_t standingTrackedDevicePose = this.header.standingTrackedDevicePose;
				return new Vector3(-standingTrackedDevicePose.vAngularVelocity.v0, -standingTrackedDevicePose.vAngularVelocity.v1, standingTrackedDevicePose.vAngularVelocity.v2);
			}
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x000EB0CA File Offset: 0x000E94CA
		public TrackedDevicePose_t GetPose()
		{
			this.Update();
			return this.header.standingTrackedDevicePose;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x000EB0DD File Offset: 0x000E94DD
		public ulong Acquire()
		{
			return this.videostream.Acquire();
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x000EB0EC File Offset: 0x000E94EC
		public ulong Release()
		{
			ulong num = this.videostream.Release();
			if (this.videostream.handle == 0UL)
			{
				global::UnityEngine.Object.Destroy(this._texture);
				this._texture = null;
			}
			return num;
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x000EB12C File Offset: 0x000E952C
		private void Update()
		{
			if (Time.frameCount == this.prevFrameCount)
			{
				return;
			}
			this.prevFrameCount = Time.frameCount;
			if (this.videostream.handle == 0UL)
			{
				return;
			}
			SteamVR instance = SteamVR.instance;
			if (instance == null)
			{
				return;
			}
			CVRTrackedCamera trackedCamera = OpenVR.TrackedCamera;
			if (trackedCamera == null)
			{
				return;
			}
			IntPtr intPtr = IntPtr.Zero;
			Texture2D texture2D = ((!(this._texture != null)) ? new Texture2D(2, 2) : this._texture);
			uint num = (uint)Marshal.SizeOf(this.header.GetType());
			if (instance.graphicsAPI == EGraphicsAPIConvention.API_OpenGL)
			{
				if (this.glTextureId != 0U)
				{
					trackedCamera.ReleaseVideoStreamTextureGL(this.videostream.handle, this.glTextureId);
				}
				if (trackedCamera.GetVideoStreamTextureGL(this.videostream.handle, this.frameType, ref this.glTextureId, ref this.header, num) != EVRTrackedCameraError.None)
				{
					return;
				}
				intPtr = (IntPtr)((long)((ulong)this.glTextureId));
			}
			else if (trackedCamera.GetVideoStreamTextureD3D11(this.videostream.handle, this.frameType, texture2D.GetNativeTexturePtr(), ref intPtr, ref this.header, num) != EVRTrackedCameraError.None)
			{
				return;
			}
			if (this._texture == null)
			{
				this._texture = Texture2D.CreateExternalTexture((int)this.header.nWidth, (int)this.header.nHeight, TextureFormat.RGBA32, false, false, intPtr);
				uint num2 = 0U;
				uint num3 = 0U;
				VRTextureBounds_t vrtextureBounds_t = default(VRTextureBounds_t);
				if (trackedCamera.GetVideoStreamTextureSize(this.deviceIndex, this.frameType, ref vrtextureBounds_t, ref num2, ref num3) == EVRTrackedCameraError.None)
				{
					vrtextureBounds_t.vMin = 1f - vrtextureBounds_t.vMin;
					vrtextureBounds_t.vMax = 1f - vrtextureBounds_t.vMax;
					this.frameBounds = vrtextureBounds_t;
				}
			}
			else
			{
				this._texture.UpdateExternalTexture(intPtr);
			}
		}

		// Token: 0x0400180D RID: 6157
		private Texture2D _texture;

		// Token: 0x0400180E RID: 6158
		private int prevFrameCount = -1;

		// Token: 0x0400180F RID: 6159
		private uint glTextureId;

		// Token: 0x04001810 RID: 6160
		private SteamVR_TrackedCamera.VideoStream videostream;

		// Token: 0x04001811 RID: 6161
		private CameraVideoStreamFrameHeader_t header;
	}

	// Token: 0x020002B3 RID: 691
	private class VideoStream
	{
		// Token: 0x060019D5 RID: 6613 RVA: 0x000EB300 File Offset: 0x000E9700
		public VideoStream(uint deviceIndex)
		{
			this.deviceIndex = deviceIndex;
			CVRTrackedCamera trackedCamera = OpenVR.TrackedCamera;
			if (trackedCamera != null)
			{
				trackedCamera.HasCamera(deviceIndex, ref this._hasCamera);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x000EB334 File Offset: 0x000E9734
		// (set) Token: 0x060019D7 RID: 6615 RVA: 0x000EB33C File Offset: 0x000E973C
		public uint deviceIndex { get; private set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x000EB345 File Offset: 0x000E9745
		public ulong handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x000EB34D File Offset: 0x000E974D
		public bool hasCamera
		{
			get
			{
				return this._hasCamera;
			}
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x000EB358 File Offset: 0x000E9758
		public ulong Acquire()
		{
			if (this._handle == 0UL && this.hasCamera)
			{
				CVRTrackedCamera trackedCamera = OpenVR.TrackedCamera;
				if (trackedCamera != null)
				{
					trackedCamera.AcquireVideoStreamingService(this.deviceIndex, ref this._handle);
				}
			}
			return this.refCount += 1UL;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x000EB3B0 File Offset: 0x000E97B0
		public ulong Release()
		{
			if (this.refCount > 0UL && (this.refCount -= 1UL) == 0UL && this._handle != 0UL)
			{
				CVRTrackedCamera trackedCamera = OpenVR.TrackedCamera;
				if (trackedCamera != null)
				{
					trackedCamera.ReleaseVideoStreamingService(this._handle);
				}
				this._handle = 0UL;
			}
			return this.refCount;
		}

		// Token: 0x04001813 RID: 6163
		private ulong _handle;

		// Token: 0x04001814 RID: 6164
		private bool _hasCamera;

		// Token: 0x04001815 RID: 6165
		private ulong refCount;
	}
}
