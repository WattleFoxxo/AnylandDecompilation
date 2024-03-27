using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Token: 0x020002B0 RID: 688
public class SteamVR_TestController : MonoBehaviour
{
	// Token: 0x060019B9 RID: 6585 RVA: 0x000EA9C4 File Offset: 0x000E8DC4
	private void OnDeviceConnected(params object[] args)
	{
		int num = (int)args[0];
		CVRSystem system = OpenVR.System;
		if (system == null || system.GetTrackedDeviceClass((uint)num) != ETrackedDeviceClass.Controller)
		{
			return;
		}
		bool flag = (bool)args[1];
		if (flag)
		{
			Debug.Log(string.Format("Controller {0} connected.", num));
			this.PrintControllerStatus(num);
			this.controllerIndices.Add(num);
		}
		else
		{
			Debug.Log(string.Format("Controller {0} disconnected.", num));
			this.PrintControllerStatus(num);
			this.controllerIndices.Remove(num);
		}
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x000EAA59 File Offset: 0x000E8E59
	private void OnEnable()
	{
		SteamVR_Utils.Event.Listen("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x000EAA71 File Offset: 0x000E8E71
	private void OnDisable()
	{
		SteamVR_Utils.Event.Remove("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x000EAA8C File Offset: 0x000E8E8C
	private void PrintControllerStatus(int index)
	{
		SteamVR_Controller.Device device = SteamVR_Controller.Input(index);
		Debug.Log("index: " + device.index);
		Debug.Log("connected: " + device.connected);
		Debug.Log("hasTracking: " + device.hasTracking);
		Debug.Log("outOfRange: " + device.outOfRange);
		Debug.Log("calibrating: " + device.calibrating);
		Debug.Log("uninitialized: " + device.uninitialized);
		Debug.Log("pos: " + device.transform.pos);
		Debug.Log("rot: " + device.transform.rot.eulerAngles);
		Debug.Log("velocity: " + device.velocity);
		Debug.Log("angularVelocity: " + device.angularVelocity);
		int deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost, ETrackedDeviceClass.Controller, 0);
		int deviceIndex2 = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost, ETrackedDeviceClass.Controller, 0);
		Debug.Log((deviceIndex != deviceIndex2) ? ((deviceIndex != index) ? "right" : "left") : "first");
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x000EABFC File Offset: 0x000E8FFC
	private void Update()
	{
		foreach (int num in this.controllerIndices)
		{
			SteamVR_Overlay instance = SteamVR_Overlay.instance;
			if (instance && this.point && this.pointer)
			{
				SteamVR_Utils.RigidTransform transform = SteamVR_Controller.Input(num).transform;
				this.pointer.transform.localPosition = transform.pos;
				this.pointer.transform.localRotation = transform.rot;
				SteamVR_Overlay.IntersectionResults intersectionResults = default(SteamVR_Overlay.IntersectionResults);
				bool flag = instance.ComputeIntersection(transform.pos, transform.rot * Vector3.forward, ref intersectionResults);
				if (flag)
				{
					this.point.transform.localPosition = intersectionResults.point;
					this.point.transform.localRotation = Quaternion.LookRotation(intersectionResults.normal);
				}
			}
			else
			{
				foreach (EVRButtonId evrbuttonId in this.buttonIds)
				{
					if (SteamVR_Controller.Input(num).GetPressDown(evrbuttonId))
					{
						Debug.Log(evrbuttonId + " press down");
					}
					if (SteamVR_Controller.Input(num).GetPressUp(evrbuttonId))
					{
						Debug.Log(evrbuttonId + " press up");
						if (evrbuttonId == EVRButtonId.k_EButton_Axis1)
						{
							SteamVR_Controller.Input(num).TriggerHapticPulse(500, EVRButtonId.k_EButton_Axis0);
							this.PrintControllerStatus(num);
						}
					}
					if (SteamVR_Controller.Input(num).GetPress(evrbuttonId))
					{
						Debug.Log(evrbuttonId);
					}
				}
				foreach (EVRButtonId evrbuttonId2 in this.axisIds)
				{
					if (SteamVR_Controller.Input(num).GetTouchDown(evrbuttonId2))
					{
						Debug.Log(evrbuttonId2 + " touch down");
					}
					if (SteamVR_Controller.Input(num).GetTouchUp(evrbuttonId2))
					{
						Debug.Log(evrbuttonId2 + " touch up");
					}
					if (SteamVR_Controller.Input(num).GetTouch(evrbuttonId2))
					{
						Vector2 axis = SteamVR_Controller.Input(num).GetAxis(evrbuttonId2);
						Debug.Log("axis: " + axis);
					}
				}
			}
		}
	}

	// Token: 0x04001803 RID: 6147
	private List<int> controllerIndices = new List<int>();

	// Token: 0x04001804 RID: 6148
	private EVRButtonId[] buttonIds = new EVRButtonId[]
	{
		EVRButtonId.k_EButton_ApplicationMenu,
		EVRButtonId.k_EButton_Grip,
		EVRButtonId.k_EButton_Axis0,
		EVRButtonId.k_EButton_Axis1
	};

	// Token: 0x04001805 RID: 6149
	private EVRButtonId[] axisIds = new EVRButtonId[]
	{
		EVRButtonId.k_EButton_Axis0,
		EVRButtonId.k_EButton_Axis1
	};

	// Token: 0x04001806 RID: 6150
	public Transform point;

	// Token: 0x04001807 RID: 6151
	public Transform pointer;
}
