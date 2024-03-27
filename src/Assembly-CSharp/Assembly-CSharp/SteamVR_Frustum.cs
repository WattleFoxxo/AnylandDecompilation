using System;
using UnityEngine;
using Valve.VR;

// Token: 0x0200029C RID: 668
[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class SteamVR_Frustum : MonoBehaviour
{
	// Token: 0x0600193C RID: 6460 RVA: 0x000E5074 File Offset: 0x000E3474
	public void UpdateModel()
	{
		this.fovLeft = Mathf.Clamp(this.fovLeft, 1f, 89f);
		this.fovRight = Mathf.Clamp(this.fovRight, 1f, 89f);
		this.fovTop = Mathf.Clamp(this.fovTop, 1f, 89f);
		this.fovBottom = Mathf.Clamp(this.fovBottom, 1f, 89f);
		this.farZ = Mathf.Max(this.farZ, this.nearZ + 0.01f);
		this.nearZ = Mathf.Clamp(this.nearZ, 0.01f, this.farZ - 0.01f);
		float num = Mathf.Sin(-this.fovLeft * 0.017453292f);
		float num2 = Mathf.Sin(this.fovRight * 0.017453292f);
		float num3 = Mathf.Sin(this.fovTop * 0.017453292f);
		float num4 = Mathf.Sin(-this.fovBottom * 0.017453292f);
		float num5 = Mathf.Cos(-this.fovLeft * 0.017453292f);
		float num6 = Mathf.Cos(this.fovRight * 0.017453292f);
		float num7 = Mathf.Cos(this.fovTop * 0.017453292f);
		float num8 = Mathf.Cos(-this.fovBottom * 0.017453292f);
		Vector3[] array = new Vector3[]
		{
			new Vector3(num * this.nearZ / num5, num3 * this.nearZ / num7, this.nearZ),
			new Vector3(num2 * this.nearZ / num6, num3 * this.nearZ / num7, this.nearZ),
			new Vector3(num2 * this.nearZ / num6, num4 * this.nearZ / num8, this.nearZ),
			new Vector3(num * this.nearZ / num5, num4 * this.nearZ / num8, this.nearZ),
			new Vector3(num * this.farZ / num5, num3 * this.farZ / num7, this.farZ),
			new Vector3(num2 * this.farZ / num6, num3 * this.farZ / num7, this.farZ),
			new Vector3(num2 * this.farZ / num6, num4 * this.farZ / num8, this.farZ),
			new Vector3(num * this.farZ / num5, num4 * this.farZ / num8, this.farZ)
		};
		int[] array2 = new int[]
		{
			0, 4, 7, 0, 7, 3, 0, 7, 4, 0,
			3, 7, 1, 5, 6, 1, 6, 2, 1, 6,
			5, 1, 2, 6, 0, 4, 5, 0, 5, 1,
			0, 5, 4, 0, 1, 5, 2, 3, 7, 2,
			7, 6, 2, 7, 3, 2, 6, 7
		};
		int num9 = 0;
		Vector3[] array3 = new Vector3[array2.Length];
		Vector3[] array4 = new Vector3[array2.Length];
		for (int i = 0; i < array2.Length / 3; i++)
		{
			Vector3 vector = array[array2[i * 3]];
			Vector3 vector2 = array[array2[i * 3 + 1]];
			Vector3 vector3 = array[array2[i * 3 + 2]];
			Vector3 normalized = Vector3.Cross(vector2 - vector, vector3 - vector).normalized;
			array4[i * 3] = normalized;
			array4[i * 3 + 1] = normalized;
			array4[i * 3 + 2] = normalized;
			array3[i * 3] = vector;
			array3[i * 3 + 1] = vector2;
			array3[i * 3 + 2] = vector3;
			array2[i * 3] = num9++;
			array2[i * 3 + 1] = num9++;
			array2[i * 3 + 2] = num9++;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array3;
		mesh.normals = array4;
		mesh.triangles = array2;
		base.GetComponent<MeshFilter>().mesh = mesh;
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x000E54B4 File Offset: 0x000E38B4
	private void OnDeviceConnected(params object[] args)
	{
		int num = (int)args[0];
		if (num != (int)this.index)
		{
			return;
		}
		base.GetComponent<MeshFilter>().mesh = null;
		bool flag = (bool)args[1];
		if (flag)
		{
			CVRSystem system = OpenVR.System;
			if (system != null && system.GetTrackedDeviceClass((uint)num) == ETrackedDeviceClass.TrackingReference)
			{
				ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
				float num2 = system.GetFloatTrackedDeviceProperty((uint)num, ETrackedDeviceProperty.Prop_FieldOfViewLeftDegrees_Float, ref etrackedPropertyError);
				if (etrackedPropertyError == ETrackedPropertyError.TrackedProp_Success)
				{
					this.fovLeft = num2;
				}
				num2 = system.GetFloatTrackedDeviceProperty((uint)num, ETrackedDeviceProperty.Prop_FieldOfViewRightDegrees_Float, ref etrackedPropertyError);
				if (etrackedPropertyError == ETrackedPropertyError.TrackedProp_Success)
				{
					this.fovRight = num2;
				}
				num2 = system.GetFloatTrackedDeviceProperty((uint)num, ETrackedDeviceProperty.Prop_FieldOfViewTopDegrees_Float, ref etrackedPropertyError);
				if (etrackedPropertyError == ETrackedPropertyError.TrackedProp_Success)
				{
					this.fovTop = num2;
				}
				num2 = system.GetFloatTrackedDeviceProperty((uint)num, ETrackedDeviceProperty.Prop_FieldOfViewBottomDegrees_Float, ref etrackedPropertyError);
				if (etrackedPropertyError == ETrackedPropertyError.TrackedProp_Success)
				{
					this.fovBottom = num2;
				}
				num2 = system.GetFloatTrackedDeviceProperty((uint)num, ETrackedDeviceProperty.Prop_TrackingRangeMinimumMeters_Float, ref etrackedPropertyError);
				if (etrackedPropertyError == ETrackedPropertyError.TrackedProp_Success)
				{
					this.nearZ = num2;
				}
				num2 = system.GetFloatTrackedDeviceProperty((uint)num, ETrackedDeviceProperty.Prop_TrackingRangeMaximumMeters_Float, ref etrackedPropertyError);
				if (etrackedPropertyError == ETrackedPropertyError.TrackedProp_Success)
				{
					this.farZ = num2;
				}
				this.UpdateModel();
			}
		}
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000E55C7 File Offset: 0x000E39C7
	private void OnEnable()
	{
		base.GetComponent<MeshFilter>().mesh = null;
		SteamVR_Utils.Event.Listen("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x000E55EB File Offset: 0x000E39EB
	private void OnDisable()
	{
		SteamVR_Utils.Event.Remove("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
		base.GetComponent<MeshFilter>().mesh = null;
	}

	// Token: 0x04001764 RID: 5988
	public SteamVR_TrackedObject.EIndex index;

	// Token: 0x04001765 RID: 5989
	public float fovLeft = 45f;

	// Token: 0x04001766 RID: 5990
	public float fovRight = 45f;

	// Token: 0x04001767 RID: 5991
	public float fovTop = 45f;

	// Token: 0x04001768 RID: 5992
	public float fovBottom = 45f;

	// Token: 0x04001769 RID: 5993
	public float nearZ = 0.5f;

	// Token: 0x0400176A RID: 5994
	public float farZ = 2.5f;
}
