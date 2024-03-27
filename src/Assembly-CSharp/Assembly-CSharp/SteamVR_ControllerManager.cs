using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000297 RID: 663
public class SteamVR_ControllerManager : MonoBehaviour
{
	// Token: 0x0600191A RID: 6426 RVA: 0x000E3A54 File Offset: 0x000E1E54
	private void Awake()
	{
		int num = ((this.objects == null) ? 0 : this.objects.Length);
		GameObject[] array = new GameObject[2 + num];
		this.indices = new uint[2 + num];
		array[0] = this.right;
		this.indices[0] = uint.MaxValue;
		array[1] = this.left;
		this.indices[1] = uint.MaxValue;
		for (int i = 0; i < num; i++)
		{
			array[2 + i] = this.objects[i];
			this.indices[2 + i] = uint.MaxValue;
		}
		this.objects = array;
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x000E3AE8 File Offset: 0x000E1EE8
	private void OnEnable()
	{
		for (int i = 0; i < this.objects.Length; i++)
		{
			GameObject gameObject = this.objects[i];
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		this.OnTrackedDeviceRoleChanged(new object[0]);
		for (int j = 0; j < SteamVR.connected.Length; j++)
		{
			if (SteamVR.connected[j])
			{
				this.OnDeviceConnected(new object[] { j, true });
			}
		}
		SteamVR_Utils.Event.Listen("input_focus", new SteamVR_Utils.Event.Handler(this.OnInputFocus));
		SteamVR_Utils.Event.Listen("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
		SteamVR_Utils.Event.Listen("TrackedDeviceRoleChanged", new SteamVR_Utils.Event.Handler(this.OnTrackedDeviceRoleChanged));
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x000E3BBC File Offset: 0x000E1FBC
	private void OnDisable()
	{
		SteamVR_Utils.Event.Remove("input_focus", new SteamVR_Utils.Event.Handler(this.OnInputFocus));
		SteamVR_Utils.Event.Remove("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
		SteamVR_Utils.Event.Remove("TrackedDeviceRoleChanged", new SteamVR_Utils.Event.Handler(this.OnTrackedDeviceRoleChanged));
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x000E3C0C File Offset: 0x000E200C
	private void OnInputFocus(params object[] args)
	{
		bool flag = (bool)args[0];
		if (flag)
		{
			for (int i = 0; i < this.objects.Length; i++)
			{
				GameObject gameObject = this.objects[i];
				if (gameObject != null)
				{
					string text = ((i >= 2) ? (i - 1).ToString() : SteamVR_ControllerManager.labels[i]);
					this.ShowObject(gameObject.transform, "hidden (" + text + ")");
				}
			}
		}
		else
		{
			for (int j = 0; j < this.objects.Length; j++)
			{
				GameObject gameObject2 = this.objects[j];
				if (gameObject2 != null)
				{
					string text2 = ((j >= 2) ? (j - 1).ToString() : SteamVR_ControllerManager.labels[j]);
					this.HideObject(gameObject2.transform, "hidden (" + text2 + ")");
				}
			}
		}
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x000E3D1C File Offset: 0x000E211C
	private void HideObject(Transform t, string name)
	{
		Transform transform = new GameObject(name).transform;
		transform.parent = t.parent;
		t.parent = transform;
		transform.gameObject.SetActive(false);
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x000E3D54 File Offset: 0x000E2154
	private void ShowObject(Transform t, string name)
	{
		Transform parent = t.parent;
		if (parent.gameObject.name != name)
		{
			return;
		}
		t.parent = parent.parent;
		global::UnityEngine.Object.Destroy(parent.gameObject);
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x000E3D98 File Offset: 0x000E2198
	private void SetTrackedDeviceIndex(int objectIndex, uint trackedDeviceIndex)
	{
		if (trackedDeviceIndex != 4294967295U)
		{
			for (int i = 0; i < this.objects.Length; i++)
			{
				if (i != objectIndex && this.indices[i] == trackedDeviceIndex)
				{
					GameObject gameObject = this.objects[i];
					if (gameObject != null)
					{
						gameObject.SetActive(false);
					}
					this.indices[i] = uint.MaxValue;
				}
			}
		}
		if (trackedDeviceIndex != this.indices[objectIndex])
		{
			this.indices[objectIndex] = trackedDeviceIndex;
			GameObject gameObject2 = this.objects[objectIndex];
			if (gameObject2 != null)
			{
				if (trackedDeviceIndex == 4294967295U)
				{
					gameObject2.SetActive(false);
				}
				else
				{
					gameObject2.SetActive(true);
					gameObject2.BroadcastMessage("SetDeviceIndex", (int)trackedDeviceIndex, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x000E3E57 File Offset: 0x000E2257
	private void OnTrackedDeviceRoleChanged(params object[] args)
	{
		this.Refresh();
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x000E3E60 File Offset: 0x000E2260
	private void OnDeviceConnected(params object[] args)
	{
		uint num = (uint)((int)args[0]);
		bool flag = this.connected[(int)((UIntPtr)num)];
		this.connected[(int)((UIntPtr)num)] = false;
		bool flag2 = (bool)args[1];
		if (flag2)
		{
			CVRSystem system = OpenVR.System;
			if (system != null && system.GetTrackedDeviceClass(num) == ETrackedDeviceClass.Controller)
			{
				this.connected[(int)((UIntPtr)num)] = true;
				flag = !flag;
			}
		}
		if (flag)
		{
			this.Refresh();
		}
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x000E3ED0 File Offset: 0x000E22D0
	public void Refresh()
	{
		int i = 0;
		CVRSystem system = OpenVR.System;
		if (system != null)
		{
			this.leftIndex = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
			this.rightIndex = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);
		}
		if (this.leftIndex == 4294967295U && this.rightIndex == 4294967295U)
		{
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this.connected.Length))
			{
				if (this.connected[(int)((UIntPtr)num)])
				{
					this.SetTrackedDeviceIndex(i++, num);
					break;
				}
				num += 1U;
			}
		}
		else
		{
			this.SetTrackedDeviceIndex(i++, ((ulong)this.rightIndex >= (ulong)((long)this.connected.Length) || !this.connected[(int)((UIntPtr)this.rightIndex)]) ? uint.MaxValue : this.rightIndex);
			this.SetTrackedDeviceIndex(i++, ((ulong)this.leftIndex >= (ulong)((long)this.connected.Length) || !this.connected[(int)((UIntPtr)this.leftIndex)]) ? uint.MaxValue : this.leftIndex);
			if (this.leftIndex != 4294967295U && this.rightIndex != 4294967295U)
			{
				uint num2 = 0U;
				while ((ulong)num2 < (ulong)((long)this.connected.Length))
				{
					if (i >= this.objects.Length)
					{
						break;
					}
					if (this.connected[(int)((UIntPtr)num2)])
					{
						if (num2 != this.leftIndex && num2 != this.rightIndex)
						{
							this.SetTrackedDeviceIndex(i++, num2);
						}
					}
					num2 += 1U;
				}
			}
		}
		while (i < this.objects.Length)
		{
			this.SetTrackedDeviceIndex(i++, uint.MaxValue);
		}
	}

	// Token: 0x04001739 RID: 5945
	public GameObject left;

	// Token: 0x0400173A RID: 5946
	public GameObject right;

	// Token: 0x0400173B RID: 5947
	public GameObject[] objects;

	// Token: 0x0400173C RID: 5948
	private uint[] indices;

	// Token: 0x0400173D RID: 5949
	private bool[] connected = new bool[16];

	// Token: 0x0400173E RID: 5950
	private uint leftIndex = uint.MaxValue;

	// Token: 0x0400173F RID: 5951
	private uint rightIndex = uint.MaxValue;

	// Token: 0x04001740 RID: 5952
	private static string[] labels = new string[] { "left", "right" };
}
