using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000293 RID: 659
public class SteamVR_Controller
{
	// Token: 0x060018F2 RID: 6386 RVA: 0x000E3298 File Offset: 0x000E1698
	public static SteamVR_Controller.Device Input(int deviceIndex)
	{
		if (SteamVR_Controller.devices == null)
		{
			SteamVR_Controller.devices = new SteamVR_Controller.Device[16];
			uint num = 0U;
			while ((ulong)num < (ulong)((long)SteamVR_Controller.devices.Length))
			{
				SteamVR_Controller.devices[(int)((UIntPtr)num)] = new SteamVR_Controller.Device(num);
				num += 1U;
			}
		}
		return SteamVR_Controller.devices[deviceIndex];
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x000E32EC File Offset: 0x000E16EC
	public static void Update()
	{
		int num = 0;
		while ((long)num < 16L)
		{
			SteamVR_Controller.Input(num).Update();
			num++;
		}
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x000E331C File Offset: 0x000E171C
	public static int GetDeviceIndex(SteamVR_Controller.DeviceRelation relation, ETrackedDeviceClass deviceClass = ETrackedDeviceClass.Controller, int relativeTo = 0)
	{
		int num = -1;
		SteamVR_Utils.RigidTransform rigidTransform = ((relativeTo >= 16) ? SteamVR_Utils.RigidTransform.identity : SteamVR_Controller.Input(relativeTo).transform.GetInverse());
		CVRSystem system = OpenVR.System;
		if (system == null)
		{
			return num;
		}
		float num2 = float.MinValue;
		int num3 = 0;
		while ((long)num3 < 16L)
		{
			if (num3 != relativeTo && system.GetTrackedDeviceClass((uint)num3) == deviceClass)
			{
				SteamVR_Controller.Device device = SteamVR_Controller.Input(num3);
				if (device.connected)
				{
					if (relation == SteamVR_Controller.DeviceRelation.First)
					{
						return num3;
					}
					Vector3 vector = rigidTransform * device.transform.pos;
					float num4;
					if (relation == SteamVR_Controller.DeviceRelation.FarthestRight)
					{
						num4 = vector.x;
					}
					else if (relation == SteamVR_Controller.DeviceRelation.FarthestLeft)
					{
						num4 = -vector.x;
					}
					else
					{
						Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
						Vector3 normalized = vector2.normalized;
						float num5 = Vector3.Dot(normalized, Vector3.forward);
						Vector3 vector3 = Vector3.Cross(normalized, Vector3.forward);
						if (relation == SteamVR_Controller.DeviceRelation.Leftmost)
						{
							num4 = ((vector3.y <= 0f) ? num5 : (2f - num5));
						}
						else
						{
							num4 = ((vector3.y >= 0f) ? num5 : (2f - num5));
						}
					}
					if (num4 > num2)
					{
						num = num3;
						num2 = num4;
					}
				}
			}
			num3++;
		}
		return num;
	}

	// Token: 0x0400171E RID: 5918
	private static SteamVR_Controller.Device[] devices;

	// Token: 0x02000294 RID: 660
	public class ButtonMask
	{
		// Token: 0x0400171F RID: 5919
		public const ulong System = 1UL;

		// Token: 0x04001720 RID: 5920
		public const ulong ApplicationMenu = 2UL;

		// Token: 0x04001721 RID: 5921
		public const ulong Grip = 4UL;

		// Token: 0x04001722 RID: 5922
		public const ulong Axis0 = 4294967296UL;

		// Token: 0x04001723 RID: 5923
		public const ulong Axis1 = 8589934592UL;

		// Token: 0x04001724 RID: 5924
		public const ulong Axis2 = 17179869184UL;

		// Token: 0x04001725 RID: 5925
		public const ulong Axis3 = 34359738368UL;

		// Token: 0x04001726 RID: 5926
		public const ulong Axis4 = 68719476736UL;

		// Token: 0x04001727 RID: 5927
		public const ulong Touchpad = 4294967296UL;

		// Token: 0x04001728 RID: 5928
		public const ulong Trigger = 8589934592UL;
	}

	// Token: 0x02000295 RID: 661
	public class Device
	{
		// Token: 0x060018F6 RID: 6390 RVA: 0x000E34A8 File Offset: 0x000E18A8
		public Device(uint i)
		{
			this.index = i;
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x000E34C9 File Offset: 0x000E18C9
		// (set) Token: 0x060018F8 RID: 6392 RVA: 0x000E34D1 File Offset: 0x000E18D1
		public uint index { get; private set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x000E34DA File Offset: 0x000E18DA
		// (set) Token: 0x060018FA RID: 6394 RVA: 0x000E34E2 File Offset: 0x000E18E2
		public bool valid { get; private set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x000E34EB File Offset: 0x000E18EB
		public bool connected
		{
			get
			{
				this.Update();
				return this.pose.bDeviceIsConnected;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x000E34FE File Offset: 0x000E18FE
		public bool hasTracking
		{
			get
			{
				this.Update();
				return this.pose.bPoseIsValid;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x000E3511 File Offset: 0x000E1911
		public bool outOfRange
		{
			get
			{
				this.Update();
				return this.pose.eTrackingResult == ETrackingResult.Running_OutOfRange || this.pose.eTrackingResult == ETrackingResult.Calibrating_OutOfRange;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x000E3540 File Offset: 0x000E1940
		public bool calibrating
		{
			get
			{
				this.Update();
				return this.pose.eTrackingResult == ETrackingResult.Calibrating_InProgress || this.pose.eTrackingResult == ETrackingResult.Calibrating_OutOfRange;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x000E356C File Offset: 0x000E196C
		public bool uninitialized
		{
			get
			{
				this.Update();
				return this.pose.eTrackingResult == ETrackingResult.Uninitialized;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x000E3582 File Offset: 0x000E1982
		public SteamVR_Utils.RigidTransform transform
		{
			get
			{
				this.Update();
				return new SteamVR_Utils.RigidTransform(this.pose.mDeviceToAbsoluteTracking);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x000E359A File Offset: 0x000E199A
		public Vector3 velocity
		{
			get
			{
				this.Update();
				return new Vector3(this.pose.vVelocity.v0, this.pose.vVelocity.v1, -this.pose.vVelocity.v2);
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06001902 RID: 6402 RVA: 0x000E35D8 File Offset: 0x000E19D8
		public Vector3 angularVelocity
		{
			get
			{
				this.Update();
				return new Vector3(-this.pose.vAngularVelocity.v0, -this.pose.vAngularVelocity.v1, this.pose.vAngularVelocity.v2);
			}
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x000E3617 File Offset: 0x000E1A17
		public VRControllerState_t GetState()
		{
			this.Update();
			return this.state;
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x000E3625 File Offset: 0x000E1A25
		public VRControllerState_t GetPrevState()
		{
			this.Update();
			return this.prevState;
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x000E3633 File Offset: 0x000E1A33
		public TrackedDevicePose_t GetPose()
		{
			this.Update();
			return this.pose;
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x000E3644 File Offset: 0x000E1A44
		public void Update()
		{
			if (Time.frameCount != this.prevFrameCount)
			{
				this.prevFrameCount = Time.frameCount;
				this.prevState = this.state;
				CVRSystem system = OpenVR.System;
				if (system != null)
				{
					this.valid = system.GetControllerStateWithPose(SteamVR_Render.instance.trackingSpace, this.index, ref this.state, ref this.pose);
					this.UpdateHairTrigger();
				}
			}
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x000E36B2 File Offset: 0x000E1AB2
		public bool GetPress(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonPressed & buttonMask) != 0UL;
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x000E36CE File Offset: 0x000E1ACE
		public bool GetPressDown(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonPressed & buttonMask) != 0UL && (this.prevState.ulButtonPressed & buttonMask) == 0UL;
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x000E36FE File Offset: 0x000E1AFE
		public bool GetPressUp(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonPressed & buttonMask) == 0UL && (this.prevState.ulButtonPressed & buttonMask) != 0UL;
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x000E3731 File Offset: 0x000E1B31
		public bool GetPress(EVRButtonId buttonId)
		{
			return this.GetPress(1UL << (int)buttonId);
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x000E3740 File Offset: 0x000E1B40
		public bool GetPressDown(EVRButtonId buttonId)
		{
			return this.GetPressDown(1UL << (int)buttonId);
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x000E374F File Offset: 0x000E1B4F
		public bool GetPressUp(EVRButtonId buttonId)
		{
			return this.GetPressUp(1UL << (int)buttonId);
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x000E375E File Offset: 0x000E1B5E
		public bool GetTouch(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonTouched & buttonMask) != 0UL;
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x000E377A File Offset: 0x000E1B7A
		public bool GetTouchDown(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonTouched & buttonMask) != 0UL && (this.prevState.ulButtonTouched & buttonMask) == 0UL;
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x000E37AA File Offset: 0x000E1BAA
		public bool GetTouchUp(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonTouched & buttonMask) == 0UL && (this.prevState.ulButtonTouched & buttonMask) != 0UL;
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x000E37DD File Offset: 0x000E1BDD
		public bool GetTouch(EVRButtonId buttonId)
		{
			return this.GetTouch(1UL << (int)buttonId);
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x000E37EC File Offset: 0x000E1BEC
		public bool GetTouchDown(EVRButtonId buttonId)
		{
			return this.GetTouchDown(1UL << (int)buttonId);
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x000E37FB File Offset: 0x000E1BFB
		public bool GetTouchUp(EVRButtonId buttonId)
		{
			return this.GetTouchUp(1UL << (int)buttonId);
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x000E380C File Offset: 0x000E1C0C
		public Vector2 GetAxis(EVRButtonId buttonId = EVRButtonId.k_EButton_Axis0)
		{
			this.Update();
			switch (buttonId)
			{
			case EVRButtonId.k_EButton_Axis0:
				return new Vector2(this.state.rAxis0.x, this.state.rAxis0.y);
			case EVRButtonId.k_EButton_Axis1:
				return new Vector2(this.state.rAxis1.x, this.state.rAxis1.y);
			case EVRButtonId.k_EButton_Axis2:
				return new Vector2(this.state.rAxis2.x, this.state.rAxis2.y);
			case EVRButtonId.k_EButton_Axis3:
				return new Vector2(this.state.rAxis3.x, this.state.rAxis3.y);
			case EVRButtonId.k_EButton_Axis4:
				return new Vector2(this.state.rAxis4.x, this.state.rAxis4.y);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x000E3908 File Offset: 0x000E1D08
		public void TriggerHapticPulse(ushort durationMicroSec = 500, EVRButtonId buttonId = EVRButtonId.k_EButton_Axis0)
		{
			CVRSystem system = OpenVR.System;
			if (system != null)
			{
				uint num = (uint)(buttonId - EVRButtonId.k_EButton_Axis0);
				system.TriggerHapticPulse(this.index, num, (char)durationMicroSec);
			}
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x000E3934 File Offset: 0x000E1D34
		private void UpdateHairTrigger()
		{
			this.hairTriggerPrevState = this.hairTriggerState;
			float x = this.state.rAxis1.x;
			if (this.hairTriggerState)
			{
				if (x < this.hairTriggerLimit - this.hairTriggerDelta || x <= 0f)
				{
					this.hairTriggerState = false;
				}
			}
			else if (x > this.hairTriggerLimit + this.hairTriggerDelta || x >= 1f)
			{
				this.hairTriggerState = true;
			}
			this.hairTriggerLimit = ((!this.hairTriggerState) ? Mathf.Min(this.hairTriggerLimit, x) : Mathf.Max(this.hairTriggerLimit, x));
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x000E39E6 File Offset: 0x000E1DE6
		public bool GetHairTrigger()
		{
			this.Update();
			return this.hairTriggerState;
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x000E39F4 File Offset: 0x000E1DF4
		public bool GetHairTriggerDown()
		{
			this.Update();
			return this.hairTriggerState && !this.hairTriggerPrevState;
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x000E3A13 File Offset: 0x000E1E13
		public bool GetHairTriggerUp()
		{
			this.Update();
			return !this.hairTriggerState && this.hairTriggerPrevState;
		}

		// Token: 0x0400172B RID: 5931
		private VRControllerState_t state;

		// Token: 0x0400172C RID: 5932
		private VRControllerState_t prevState;

		// Token: 0x0400172D RID: 5933
		private TrackedDevicePose_t pose;

		// Token: 0x0400172E RID: 5934
		private int prevFrameCount = -1;

		// Token: 0x0400172F RID: 5935
		public float hairTriggerDelta = 0.1f;

		// Token: 0x04001730 RID: 5936
		private float hairTriggerLimit;

		// Token: 0x04001731 RID: 5937
		private bool hairTriggerState;

		// Token: 0x04001732 RID: 5938
		private bool hairTriggerPrevState;
	}

	// Token: 0x02000296 RID: 662
	public enum DeviceRelation
	{
		// Token: 0x04001734 RID: 5940
		First,
		// Token: 0x04001735 RID: 5941
		Leftmost,
		// Token: 0x04001736 RID: 5942
		Rightmost,
		// Token: 0x04001737 RID: 5943
		FarthestLeft,
		// Token: 0x04001738 RID: 5944
		FarthestRight
	}
}
