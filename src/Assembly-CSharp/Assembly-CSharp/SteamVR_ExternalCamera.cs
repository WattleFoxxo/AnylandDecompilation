using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR;

// Token: 0x02000299 RID: 665
public class SteamVR_ExternalCamera : MonoBehaviour
{
	// Token: 0x0600192A RID: 6442 RVA: 0x000E41A8 File Offset: 0x000E25A8
	public void ReadConfig()
	{
		try
		{
			HmdMatrix34_t hmdMatrix34_t = default(HmdMatrix34_t);
			bool flag = false;
			object obj = this.config;
			string[] array = File.ReadAllLines(this.configPath);
			foreach (string text in array)
			{
				string[] array3 = text.Split(new char[] { '=' });
				if (array3.Length == 2)
				{
					string text2 = array3[0];
					if (text2 == "m")
					{
						string[] array4 = array3[1].Split(new char[] { ',' });
						if (array4.Length == 12)
						{
							hmdMatrix34_t.m0 = float.Parse(array4[0]);
							hmdMatrix34_t.m1 = float.Parse(array4[1]);
							hmdMatrix34_t.m2 = float.Parse(array4[2]);
							hmdMatrix34_t.m3 = float.Parse(array4[3]);
							hmdMatrix34_t.m4 = float.Parse(array4[4]);
							hmdMatrix34_t.m5 = float.Parse(array4[5]);
							hmdMatrix34_t.m6 = float.Parse(array4[6]);
							hmdMatrix34_t.m7 = float.Parse(array4[7]);
							hmdMatrix34_t.m8 = float.Parse(array4[8]);
							hmdMatrix34_t.m9 = float.Parse(array4[9]);
							hmdMatrix34_t.m10 = float.Parse(array4[10]);
							hmdMatrix34_t.m11 = float.Parse(array4[11]);
							flag = true;
						}
					}
					else if (text2 == "disableStandardAssets")
					{
						FieldInfo field = obj.GetType().GetField(text2);
						if (field != null)
						{
							field.SetValue(obj, bool.Parse(array3[1]));
						}
					}
					else
					{
						FieldInfo field2 = obj.GetType().GetField(text2);
						if (field2 != null)
						{
							field2.SetValue(obj, float.Parse(array3[1]));
						}
					}
				}
			}
			this.config = (SteamVR_ExternalCamera.Config)obj;
			if (flag)
			{
				SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(hmdMatrix34_t);
				this.config.x = rigidTransform.pos.x;
				this.config.y = rigidTransform.pos.y;
				this.config.z = rigidTransform.pos.z;
				Vector3 eulerAngles = rigidTransform.rot.eulerAngles;
				this.config.rx = eulerAngles.x;
				this.config.ry = eulerAngles.y;
				this.config.rz = eulerAngles.z;
			}
		}
		catch
		{
		}
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x000E4454 File Offset: 0x000E2854
	public void AttachToCamera(SteamVR_Camera vrcam)
	{
		if (this.target == vrcam.head)
		{
			return;
		}
		this.target = vrcam.head;
		Transform parent = base.transform.parent;
		Transform parent2 = vrcam.head.parent;
		parent.parent = parent2;
		parent.localPosition = Vector3.zero;
		parent.localRotation = Quaternion.identity;
		parent.localScale = Vector3.one;
		vrcam.enabled = false;
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(vrcam.gameObject);
		vrcam.enabled = true;
		gameObject.name = "camera";
		global::UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<SteamVR_Camera>());
		this.cam = gameObject.GetComponent<Camera>();
		this.cam.fieldOfView = this.config.fov;
		this.cam.useOcclusionCulling = false;
		this.cam.enabled = false;
		this.colorMat = new Material(Shader.Find("Custom/SteamVR_ColorOut"));
		this.alphaMat = new Material(Shader.Find("Custom/SteamVR_AlphaOut"));
		this.clipMaterial = new Material(Shader.Find("Custom/SteamVR_ClearAll"));
		Transform transform = gameObject.transform;
		transform.parent = base.transform;
		transform.localPosition = new Vector3(this.config.x, this.config.y, this.config.z);
		transform.localRotation = Quaternion.Euler(this.config.rx, this.config.ry, this.config.rz);
		transform.localScale = Vector3.one;
		while (transform.childCount > 0)
		{
			global::UnityEngine.Object.DestroyImmediate(transform.GetChild(0).gameObject);
		}
		this.clipQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		this.clipQuad.name = "ClipQuad";
		global::UnityEngine.Object.DestroyImmediate(this.clipQuad.GetComponent<MeshCollider>());
		MeshRenderer component = this.clipQuad.GetComponent<MeshRenderer>();
		component.material = this.clipMaterial;
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.receiveShadows = false;
		component.lightProbeUsage = LightProbeUsage.Off;
		component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		Transform transform2 = this.clipQuad.transform;
		transform2.parent = transform;
		transform2.localScale = new Vector3(1000f, 1000f, 1f);
		transform2.localRotation = Quaternion.identity;
		this.clipQuad.SetActive(false);
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x000E46B4 File Offset: 0x000E2AB4
	public float GetTargetDistance()
	{
		if (this.target == null)
		{
			return this.config.near + 0.01f;
		}
		Transform transform = this.cam.transform;
		Vector3 vector = new Vector3(transform.forward.x, 0f, transform.forward.z);
		Vector3 normalized = vector.normalized;
		Vector3 position = this.target.position;
		Vector3 vector2 = new Vector3(this.target.forward.x, 0f, this.target.forward.z);
		Vector3 vector3 = position + vector2.normalized * this.config.hmdOffset;
		Plane plane = new Plane(normalized, vector3);
		float num = -plane.GetDistanceToPoint(transform.position);
		return Mathf.Clamp(num, this.config.near + 0.01f, this.config.far - 0.01f);
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x000E47C4 File Offset: 0x000E2BC4
	public void RenderNear()
	{
		int num = Screen.width / 2;
		int num2 = Screen.height / 2;
		if (this.cam.targetTexture == null || this.cam.targetTexture.width != num || this.cam.targetTexture.height != num2)
		{
			this.cam.targetTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32);
			this.cam.targetTexture.antiAliasing = ((QualitySettings.antiAliasing != 0) ? QualitySettings.antiAliasing : 1);
		}
		this.cam.nearClipPlane = this.config.near;
		this.cam.farClipPlane = this.config.far;
		CameraClearFlags clearFlags = this.cam.clearFlags;
		Color backgroundColor = this.cam.backgroundColor;
		this.cam.clearFlags = CameraClearFlags.Color;
		this.cam.backgroundColor = Color.clear;
		float num3 = Mathf.Clamp(this.GetTargetDistance() + this.config.nearOffset, this.config.near, this.config.far);
		Transform parent = this.clipQuad.transform.parent;
		this.clipQuad.transform.position = parent.position + parent.forward * num3;
		MonoBehaviour[] array = null;
		bool[] array2 = null;
		if (this.config.disableStandardAssets)
		{
			array = this.cam.gameObject.GetComponents<MonoBehaviour>();
			array2 = new bool[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				MonoBehaviour monoBehaviour = array[i];
				if (monoBehaviour.enabled && monoBehaviour.GetType().ToString().StartsWith("UnityStandardAssets."))
				{
					monoBehaviour.enabled = false;
					array2[i] = true;
				}
			}
		}
		this.clipQuad.SetActive(true);
		this.cam.Render();
		this.clipQuad.SetActive(false);
		if (array != null)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array2[j])
				{
					array[j].enabled = true;
				}
			}
		}
		this.cam.clearFlags = clearFlags;
		this.cam.backgroundColor = backgroundColor;
		Graphics.DrawTexture(new Rect(0f, 0f, (float)num, (float)num2), this.cam.targetTexture, this.colorMat);
		Graphics.DrawTexture(new Rect((float)num, 0f, (float)num, (float)num2), this.cam.targetTexture, this.alphaMat);
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x000E4A70 File Offset: 0x000E2E70
	public void RenderFar()
	{
		this.cam.nearClipPlane = this.config.near;
		this.cam.farClipPlane = this.config.far;
		this.cam.Render();
		int num = Screen.width / 2;
		int num2 = Screen.height / 2;
		Graphics.DrawTexture(new Rect(0f, (float)num2, (float)num, (float)num2), this.cam.targetTexture, this.colorMat);
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000E4AEA File Offset: 0x000E2EEA
	private void OnGUI()
	{
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x000E4AEC File Offset: 0x000E2EEC
	private void OnEnable()
	{
		this.cameras = global::UnityEngine.Object.FindObjectsOfType<Camera>();
		if (this.cameras != null)
		{
			int num = this.cameras.Length;
			this.cameraRects = new Rect[num];
			for (int i = 0; i < num; i++)
			{
				Camera camera = this.cameras[i];
				this.cameraRects[i] = camera.rect;
				if (!(camera == this.cam))
				{
					if (!(camera.targetTexture != null))
					{
						if (!(camera.GetComponent<SteamVR_Camera>() != null))
						{
							camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
						}
					}
				}
			}
		}
		if (this.config.sceneResolutionScale > 0f)
		{
			this.sceneResolutionScale = SteamVR_Camera.sceneResolutionScale;
			SteamVR_Camera.sceneResolutionScale = this.config.sceneResolutionScale;
		}
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x000E4BE8 File Offset: 0x000E2FE8
	private void OnDisable()
	{
		if (this.cameras != null)
		{
			int num = this.cameras.Length;
			for (int i = 0; i < num; i++)
			{
				Camera camera = this.cameras[i];
				if (camera != null)
				{
					camera.rect = this.cameraRects[i];
				}
			}
			this.cameras = null;
			this.cameraRects = null;
		}
		if (this.config.sceneResolutionScale > 0f)
		{
			SteamVR_Camera.sceneResolutionScale = this.sceneResolutionScale;
		}
	}

	// Token: 0x04001744 RID: 5956
	public SteamVR_ExternalCamera.Config config;

	// Token: 0x04001745 RID: 5957
	public string configPath;

	// Token: 0x04001746 RID: 5958
	private Camera cam;

	// Token: 0x04001747 RID: 5959
	private Transform target;

	// Token: 0x04001748 RID: 5960
	private GameObject clipQuad;

	// Token: 0x04001749 RID: 5961
	private Material clipMaterial;

	// Token: 0x0400174A RID: 5962
	private Material colorMat;

	// Token: 0x0400174B RID: 5963
	private Material alphaMat;

	// Token: 0x0400174C RID: 5964
	private Camera[] cameras;

	// Token: 0x0400174D RID: 5965
	private Rect[] cameraRects;

	// Token: 0x0400174E RID: 5966
	private float sceneResolutionScale;

	// Token: 0x0200029A RID: 666
	public struct Config
	{
		// Token: 0x0400174F RID: 5967
		public float x;

		// Token: 0x04001750 RID: 5968
		public float y;

		// Token: 0x04001751 RID: 5969
		public float z;

		// Token: 0x04001752 RID: 5970
		public float rx;

		// Token: 0x04001753 RID: 5971
		public float ry;

		// Token: 0x04001754 RID: 5972
		public float rz;

		// Token: 0x04001755 RID: 5973
		public float fov;

		// Token: 0x04001756 RID: 5974
		public float near;

		// Token: 0x04001757 RID: 5975
		public float far;

		// Token: 0x04001758 RID: 5976
		public float sceneResolutionScale;

		// Token: 0x04001759 RID: 5977
		public float frameSkip;

		// Token: 0x0400175A RID: 5978
		public float nearOffset;

		// Token: 0x0400175B RID: 5979
		public float farOffset;

		// Token: 0x0400175C RID: 5980
		public float hmdOffset;

		// Token: 0x0400175D RID: 5981
		public bool disableStandardAssets;
	}
}
