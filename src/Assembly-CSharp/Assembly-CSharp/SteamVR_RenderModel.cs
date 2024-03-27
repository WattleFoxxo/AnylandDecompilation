using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Valve.VR;

// Token: 0x020002A6 RID: 678
[ExecuteInEditMode]
public class SteamVR_RenderModel : MonoBehaviour
{
	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06001987 RID: 6535 RVA: 0x000E8E6A File Offset: 0x000E726A
	// (set) Token: 0x06001988 RID: 6536 RVA: 0x000E8E72 File Offset: 0x000E7272
	public string renderModelName { get; private set; }

	// Token: 0x06001989 RID: 6537 RVA: 0x000E8E7B File Offset: 0x000E727B
	private void OnModelSkinSettingsHaveChanged(params object[] args)
	{
		if (!string.IsNullOrEmpty(this.renderModelName))
		{
			this.renderModelName = string.Empty;
			this.UpdateModel();
		}
	}

	// Token: 0x0600198A RID: 6538 RVA: 0x000E8EA0 File Offset: 0x000E72A0
	private void OnHideRenderModels(params object[] args)
	{
		bool flag = (bool)args[0];
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component != null)
		{
			component.enabled = !flag;
		}
		foreach (MeshRenderer meshRenderer in base.transform.GetComponentsInChildren<MeshRenderer>())
		{
			meshRenderer.enabled = !flag;
		}
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x000E8F08 File Offset: 0x000E7308
	private void OnDeviceConnected(params object[] args)
	{
		int num = (int)args[0];
		if (num != (int)this.index)
		{
			return;
		}
		bool flag = (bool)args[1];
		if (flag)
		{
			this.UpdateModel();
		}
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x000E8F40 File Offset: 0x000E7340
	public void UpdateModel()
	{
		CVRSystem system = OpenVR.System;
		if (system == null)
		{
			return;
		}
		ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
		uint stringTrackedDeviceProperty = system.GetStringTrackedDeviceProperty((uint)this.index, ETrackedDeviceProperty.Prop_RenderModelName_String, null, 0U, ref etrackedPropertyError);
		if (stringTrackedDeviceProperty <= 1U)
		{
			Debug.LogError("Failed to get render model name for tracked object " + this.index);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder((int)stringTrackedDeviceProperty);
		system.GetStringTrackedDeviceProperty((uint)this.index, ETrackedDeviceProperty.Prop_RenderModelName_String, stringBuilder, stringTrackedDeviceProperty, ref etrackedPropertyError);
		string text = stringBuilder.ToString();
		if (this.renderModelName != text)
		{
			this.renderModelName = text;
			base.StartCoroutine(this.SetModelAsync(text));
		}
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x000E8FE4 File Offset: 0x000E73E4
	private IEnumerator SetModelAsync(string renderModelName)
	{
		if (string.IsNullOrEmpty(renderModelName))
		{
			yield break;
		}
		using (SteamVR_RenderModel.RenderModelInterfaceHolder holder = new SteamVR_RenderModel.RenderModelInterfaceHolder())
		{
			CVRRenderModels renderModels = holder.instance;
			if (renderModels == null)
			{
				yield break;
			}
			uint count = renderModels.GetComponentCount(renderModelName);
			string[] renderModelNames;
			if (count > 0U)
			{
				renderModelNames = new string[count];
				int num = 0;
				while ((long)num < (long)((ulong)count))
				{
					uint num2 = renderModels.GetComponentName(renderModelName, (uint)num, null, 0U);
					if (num2 != 0U)
					{
						StringBuilder stringBuilder = new StringBuilder((int)num2);
						if (renderModels.GetComponentName(renderModelName, (uint)num, stringBuilder, num2) != 0U)
						{
							num2 = renderModels.GetComponentRenderModelName(renderModelName, stringBuilder.ToString(), null, 0U);
							if (num2 != 0U)
							{
								StringBuilder stringBuilder2 = new StringBuilder((int)num2);
								if (renderModels.GetComponentRenderModelName(renderModelName, stringBuilder.ToString(), stringBuilder2, num2) != 0U)
								{
									string text = stringBuilder2.ToString();
									SteamVR_RenderModel.RenderModel renderModel = SteamVR_RenderModel.models[text] as SteamVR_RenderModel.RenderModel;
									if (renderModel == null || renderModel.mesh == null)
									{
										renderModelNames[num] = text;
									}
								}
							}
						}
					}
					num++;
				}
			}
			else
			{
				SteamVR_RenderModel.RenderModel renderModel2 = SteamVR_RenderModel.models[renderModelName] as SteamVR_RenderModel.RenderModel;
				if (renderModel2 == null || renderModel2.mesh == null)
				{
					renderModelNames = new string[] { renderModelName };
				}
				else
				{
					renderModelNames = new string[0];
				}
			}
			for (;;)
			{
				bool loading = false;
				foreach (string text2 in renderModelNames)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						IntPtr zero = IntPtr.Zero;
						EVRRenderModelError evrrenderModelError = renderModels.LoadRenderModel_Async(text2, ref zero);
						if (evrrenderModelError == EVRRenderModelError.Loading)
						{
							loading = true;
						}
						else if (evrrenderModelError == EVRRenderModelError.None)
						{
							RenderModel_t renderModel_t = (RenderModel_t)Marshal.PtrToStructure(zero, typeof(RenderModel_t));
							Material material = SteamVR_RenderModel.materials[renderModel_t.diffuseTextureId] as Material;
							if (material == null || material.mainTexture == null)
							{
								IntPtr zero2 = IntPtr.Zero;
								evrrenderModelError = renderModels.LoadTexture_Async(renderModel_t.diffuseTextureId, ref zero2);
								if (evrrenderModelError == EVRRenderModelError.Loading)
								{
									loading = true;
								}
							}
						}
					}
				}
				if (!loading)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
		}
		bool success = this.SetModel(renderModelName);
		SteamVR_Utils.Event.Send("render_model_loaded", new object[] { this, success });
		yield break;
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x000E9008 File Offset: 0x000E7408
	private bool SetModel(string renderModelName)
	{
		this.StripMesh(base.gameObject);
		using (SteamVR_RenderModel.RenderModelInterfaceHolder renderModelInterfaceHolder = new SteamVR_RenderModel.RenderModelInterfaceHolder())
		{
			if (this.createComponents)
			{
				if (this.LoadComponents(renderModelInterfaceHolder, renderModelName))
				{
					this.UpdateComponents();
					return true;
				}
				Debug.Log("[" + base.gameObject.name + "] Render model does not support components, falling back to single mesh.");
			}
			if (!string.IsNullOrEmpty(renderModelName))
			{
				SteamVR_RenderModel.RenderModel renderModel = SteamVR_RenderModel.models[renderModelName] as SteamVR_RenderModel.RenderModel;
				if (renderModel == null || renderModel.mesh == null)
				{
					CVRRenderModels instance = renderModelInterfaceHolder.instance;
					if (instance == null)
					{
						return false;
					}
					if (this.verbose)
					{
						Debug.Log("Loading render model " + renderModelName);
					}
					renderModel = this.LoadRenderModel(instance, renderModelName, renderModelName);
					if (renderModel == null)
					{
						return false;
					}
					SteamVR_RenderModel.models[renderModelName] = renderModel;
				}
				base.gameObject.AddComponent<MeshFilter>().mesh = renderModel.mesh;
				base.gameObject.AddComponent<MeshRenderer>().sharedMaterial = renderModel.material;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x000E9154 File Offset: 0x000E7554
	private SteamVR_RenderModel.RenderModel LoadRenderModel(CVRRenderModels renderModels, string renderModelName, string baseName)
	{
		IntPtr zero = IntPtr.Zero;
		EVRRenderModelError evrrenderModelError;
		for (;;)
		{
			evrrenderModelError = renderModels.LoadRenderModel_Async(renderModelName, ref zero);
			if (evrrenderModelError != EVRRenderModelError.Loading)
			{
				break;
			}
			Thread.Sleep(1);
		}
		if (evrrenderModelError != EVRRenderModelError.None)
		{
			Debug.LogError(string.Format("Failed to load render model {0} - {1}", renderModelName, evrrenderModelError.ToString()));
			return null;
		}
		RenderModel_t renderModel_t = (RenderModel_t)Marshal.PtrToStructure(zero, typeof(RenderModel_t));
		Vector3[] array = new Vector3[renderModel_t.unVertexCount];
		Vector3[] array2 = new Vector3[renderModel_t.unVertexCount];
		Vector2[] array3 = new Vector2[renderModel_t.unVertexCount];
		Type typeFromHandle = typeof(RenderModel_Vertex_t);
		int num = 0;
		while ((long)num < (long)((ulong)renderModel_t.unVertexCount))
		{
			IntPtr intPtr = new IntPtr(renderModel_t.rVertexData.ToInt64() + (long)(num * Marshal.SizeOf(typeFromHandle)));
			RenderModel_Vertex_t renderModel_Vertex_t = (RenderModel_Vertex_t)Marshal.PtrToStructure(intPtr, typeFromHandle);
			array[num] = new Vector3(renderModel_Vertex_t.vPosition.v0, renderModel_Vertex_t.vPosition.v1, -renderModel_Vertex_t.vPosition.v2);
			array2[num] = new Vector3(renderModel_Vertex_t.vNormal.v0, renderModel_Vertex_t.vNormal.v1, -renderModel_Vertex_t.vNormal.v2);
			array3[num] = new Vector2(renderModel_Vertex_t.rfTextureCoord0, renderModel_Vertex_t.rfTextureCoord1);
			num++;
		}
		int num2 = (int)(renderModel_t.unTriangleCount * 3U);
		short[] array4 = new short[num2];
		Marshal.Copy(renderModel_t.rIndexData, array4, 0, array4.Length);
		int[] array5 = new int[num2];
		int num3 = 0;
		while ((long)num3 < (long)((ulong)renderModel_t.unTriangleCount))
		{
			array5[num3 * 3] = (int)array4[num3 * 3 + 2];
			array5[num3 * 3 + 1] = (int)array4[num3 * 3 + 1];
			array5[num3 * 3 + 2] = (int)array4[num3 * 3];
			num3++;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.uv = array3;
		mesh.triangles = array5;
		Material material = SteamVR_RenderModel.materials[renderModel_t.diffuseTextureId] as Material;
		if (material == null || material.mainTexture == null)
		{
			IntPtr zero2 = IntPtr.Zero;
			for (;;)
			{
				evrrenderModelError = renderModels.LoadTexture_Async(renderModel_t.diffuseTextureId, ref zero2);
				if (evrrenderModelError != EVRRenderModelError.Loading)
				{
					break;
				}
				Thread.Sleep(1);
			}
			if (evrrenderModelError == EVRRenderModelError.None)
			{
				RenderModel_TextureMap_t renderModel_TextureMap_t = (RenderModel_TextureMap_t)Marshal.PtrToStructure(zero2, typeof(RenderModel_TextureMap_t));
				Texture2D texture2D = new Texture2D((int)renderModel_TextureMap_t.unWidth, (int)renderModel_TextureMap_t.unHeight, TextureFormat.ARGB32, false);
				if (SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL"))
				{
					byte[] array6 = new byte[(int)(renderModel_TextureMap_t.unWidth * renderModel_TextureMap_t.unHeight * '\u0004')];
					Marshal.Copy(renderModel_TextureMap_t.rubTextureMapData, array6, 0, array6.Length);
					Color32[] array7 = new Color32[(int)(renderModel_TextureMap_t.unWidth * renderModel_TextureMap_t.unHeight)];
					int num4 = 0;
					for (int i = 0; i < (int)renderModel_TextureMap_t.unHeight; i++)
					{
						for (int j = 0; j < (int)renderModel_TextureMap_t.unWidth; j++)
						{
							byte b = array6[num4++];
							byte b2 = array6[num4++];
							byte b3 = array6[num4++];
							byte b4 = array6[num4++];
							array7[i * (int)renderModel_TextureMap_t.unWidth + j] = new Color32(b, b2, b3, b4);
						}
					}
					texture2D.SetPixels32(array7);
					texture2D.Apply();
				}
				else
				{
					texture2D.Apply();
					for (;;)
					{
						evrrenderModelError = renderModels.LoadIntoTextureD3D11_Async(renderModel_t.diffuseTextureId, texture2D.GetNativeTexturePtr());
						if (evrrenderModelError != EVRRenderModelError.Loading)
						{
							break;
						}
						Thread.Sleep(1);
					}
				}
				material = new Material((!(this.shader != null)) ? Shader.Find("Standard") : this.shader);
				material.mainTexture = texture2D;
				SteamVR_RenderModel.materials[renderModel_t.diffuseTextureId] = material;
				renderModels.FreeTexture(zero2);
			}
			else
			{
				Debug.Log("Failed to load render model texture for render model " + renderModelName);
			}
		}
		base.StartCoroutine(this.FreeRenderModel(zero));
		return new SteamVR_RenderModel.RenderModel(mesh, material);
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x000E95D4 File Offset: 0x000E79D4
	private IEnumerator FreeRenderModel(IntPtr pRenderModel)
	{
		yield return new WaitForSeconds(1f);
		using (SteamVR_RenderModel.RenderModelInterfaceHolder renderModelInterfaceHolder = new SteamVR_RenderModel.RenderModelInterfaceHolder())
		{
			CVRRenderModels instance = renderModelInterfaceHolder.instance;
			instance.FreeRenderModel(pRenderModel);
		}
		yield break;
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x000E95F0 File Offset: 0x000E79F0
	public Transform FindComponent(string componentName)
	{
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.name == componentName)
			{
				return child;
			}
		}
		return null;
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x000E9638 File Offset: 0x000E7A38
	private void StripMesh(GameObject go)
	{
		MeshRenderer component = go.GetComponent<MeshRenderer>();
		if (component != null)
		{
			global::UnityEngine.Object.DestroyImmediate(component);
		}
		MeshFilter component2 = go.GetComponent<MeshFilter>();
		if (component2 != null)
		{
			global::UnityEngine.Object.DestroyImmediate(component2);
		}
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x000E9678 File Offset: 0x000E7A78
	private bool LoadComponents(SteamVR_RenderModel.RenderModelInterfaceHolder holder, string renderModelName)
	{
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			child.gameObject.SetActive(false);
			this.StripMesh(child.gameObject);
		}
		if (string.IsNullOrEmpty(renderModelName))
		{
			return true;
		}
		CVRRenderModels instance = holder.instance;
		if (instance == null)
		{
			return false;
		}
		uint componentCount = instance.GetComponentCount(renderModelName);
		if (componentCount == 0U)
		{
			return false;
		}
		int num = 0;
		while ((long)num < (long)((ulong)componentCount))
		{
			uint num2 = instance.GetComponentName(renderModelName, (uint)num, null, 0U);
			if (num2 != 0U)
			{
				StringBuilder stringBuilder = new StringBuilder((int)num2);
				if (instance.GetComponentName(renderModelName, (uint)num, stringBuilder, num2) != 0U)
				{
					transform = this.FindComponent(stringBuilder.ToString());
					if (transform != null)
					{
						transform.gameObject.SetActive(true);
					}
					else
					{
						transform = new GameObject(stringBuilder.ToString()).transform;
						transform.parent = base.transform;
						transform.gameObject.layer = base.gameObject.layer;
						Transform transform2 = new GameObject("attach").transform;
						transform2.parent = transform;
						transform2.localPosition = Vector3.zero;
						transform2.localRotation = Quaternion.identity;
						transform2.localScale = Vector3.one;
						transform2.gameObject.layer = base.gameObject.layer;
					}
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
					transform.localScale = Vector3.one;
					num2 = instance.GetComponentRenderModelName(renderModelName, stringBuilder.ToString(), null, 0U);
					if (num2 != 0U)
					{
						StringBuilder stringBuilder2 = new StringBuilder((int)num2);
						if (instance.GetComponentRenderModelName(renderModelName, stringBuilder.ToString(), stringBuilder2, num2) != 0U)
						{
							SteamVR_RenderModel.RenderModel renderModel = SteamVR_RenderModel.models[stringBuilder2] as SteamVR_RenderModel.RenderModel;
							if (renderModel == null || renderModel.mesh == null)
							{
								if (this.verbose)
								{
									Debug.Log("Loading render model " + stringBuilder2);
								}
								renderModel = this.LoadRenderModel(instance, stringBuilder2.ToString(), renderModelName);
								if (renderModel == null)
								{
									goto IL_265;
								}
								SteamVR_RenderModel.models[stringBuilder2] = renderModel;
							}
							transform.gameObject.AddComponent<MeshFilter>().mesh = renderModel.mesh;
							transform.gameObject.AddComponent<MeshRenderer>().sharedMaterial = renderModel.material;
						}
					}
				}
			}
			IL_265:
			num++;
		}
		return true;
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x000E98FC File Offset: 0x000E7CFC
	private void OnEnable()
	{
		if (!string.IsNullOrEmpty(this.modelOverride))
		{
			Debug.Log("Model override is really only meant to be used in the scene view for lining things up; using it at runtime is discouraged.  Use tracked device index instead to ensure the correct model is displayed for all users.");
			base.enabled = false;
			return;
		}
		CVRSystem system = OpenVR.System;
		if (system != null && system.IsTrackedDeviceConnected((uint)this.index))
		{
			this.UpdateModel();
		}
		SteamVR_Utils.Event.Listen("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
		SteamVR_Utils.Event.Listen("hide_render_models", new SteamVR_Utils.Event.Handler(this.OnHideRenderModels));
		SteamVR_Utils.Event.Listen("ModelSkinSettingsHaveChanged", new SteamVR_Utils.Event.Handler(this.OnModelSkinSettingsHaveChanged));
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x000E9990 File Offset: 0x000E7D90
	private void OnDisable()
	{
		SteamVR_Utils.Event.Remove("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
		SteamVR_Utils.Event.Remove("hide_render_models", new SteamVR_Utils.Event.Handler(this.OnHideRenderModels));
		SteamVR_Utils.Event.Remove("ModelSkinSettingsHaveChanged", new SteamVR_Utils.Event.Handler(this.OnModelSkinSettingsHaveChanged));
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x000E99DF File Offset: 0x000E7DDF
	private void Update()
	{
		if (this.updateDynamically)
		{
			this.UpdateComponents();
		}
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x000E99F4 File Offset: 0x000E7DF4
	public void UpdateComponents()
	{
		Transform transform = base.transform;
		if (transform.childCount == 0)
		{
			return;
		}
		using (SteamVR_RenderModel.RenderModelInterfaceHolder renderModelInterfaceHolder = new SteamVR_RenderModel.RenderModelInterfaceHolder())
		{
			VRControllerState_t vrcontrollerState_t = ((this.index == SteamVR_TrackedObject.EIndex.None) ? default(VRControllerState_t) : SteamVR_Controller.Input((int)this.index).GetState());
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				CVRRenderModels instance = renderModelInterfaceHolder.instance;
				if (instance == null)
				{
					break;
				}
				RenderModel_ComponentState_t renderModel_ComponentState_t = default(RenderModel_ComponentState_t);
				if (instance.GetComponentState(this.renderModelName, child.name, ref vrcontrollerState_t, ref this.controllerModeState, ref renderModel_ComponentState_t))
				{
					SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(renderModel_ComponentState_t.mTrackingToComponentRenderModel);
					child.localPosition = rigidTransform.pos;
					child.localRotation = rigidTransform.rot;
					Transform transform2 = child.Find("attach");
					if (transform2 != null)
					{
						SteamVR_Utils.RigidTransform rigidTransform2 = new SteamVR_Utils.RigidTransform(renderModel_ComponentState_t.mTrackingToComponentLocal);
						transform2.position = transform.TransformPoint(rigidTransform2.pos);
						transform2.rotation = transform.rotation * rigidTransform2.rot;
					}
					bool flag = (renderModel_ComponentState_t.uProperties & 2U) != 0U;
					if (flag != child.gameObject.activeSelf)
					{
						child.gameObject.SetActive(flag);
					}
				}
			}
		}
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x000E9B90 File Offset: 0x000E7F90
	public void SetDeviceIndex(int index)
	{
		this.index = (SteamVR_TrackedObject.EIndex)index;
		this.modelOverride = string.Empty;
		if (base.enabled)
		{
			this.UpdateModel();
		}
	}

	// Token: 0x040017D4 RID: 6100
	public SteamVR_TrackedObject.EIndex index = SteamVR_TrackedObject.EIndex.None;

	// Token: 0x040017D5 RID: 6101
	public string modelOverride;

	// Token: 0x040017D6 RID: 6102
	public Shader shader;

	// Token: 0x040017D7 RID: 6103
	public bool verbose;

	// Token: 0x040017D8 RID: 6104
	public bool createComponents = true;

	// Token: 0x040017D9 RID: 6105
	public bool updateDynamically = true;

	// Token: 0x040017DA RID: 6106
	public RenderModel_ControllerMode_State_t controllerModeState;

	// Token: 0x040017DB RID: 6107
	public const string k_localTransformName = "attach";

	// Token: 0x040017DD RID: 6109
	public static Hashtable models = new Hashtable();

	// Token: 0x040017DE RID: 6110
	public static Hashtable materials = new Hashtable();

	// Token: 0x020002A7 RID: 679
	public class RenderModel
	{
		// Token: 0x0600199A RID: 6554 RVA: 0x000E9BCB File Offset: 0x000E7FCB
		public RenderModel(Mesh mesh, Material material)
		{
			this.mesh = mesh;
			this.material = material;
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600199B RID: 6555 RVA: 0x000E9BE1 File Offset: 0x000E7FE1
		// (set) Token: 0x0600199C RID: 6556 RVA: 0x000E9BE9 File Offset: 0x000E7FE9
		public Mesh mesh { get; private set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600199D RID: 6557 RVA: 0x000E9BF2 File Offset: 0x000E7FF2
		// (set) Token: 0x0600199E RID: 6558 RVA: 0x000E9BFA File Offset: 0x000E7FFA
		public Material material { get; private set; }
	}

	// Token: 0x020002A8 RID: 680
	public sealed class RenderModelInterfaceHolder : IDisposable
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060019A0 RID: 6560 RVA: 0x000E9C0C File Offset: 0x000E800C
		public CVRRenderModels instance
		{
			get
			{
				if (this._instance == null && !this.failedLoadInterface)
				{
					if (!SteamVR.active && !SteamVR.usingNativeSupport)
					{
						EVRInitError evrinitError = EVRInitError.None;
						OpenVR.Init(ref evrinitError, EVRApplicationType.VRApplication_Other);
						this.needsShutdown = true;
					}
					this._instance = OpenVR.RenderModels;
					if (this._instance == null)
					{
						Debug.LogError("Failed to load IVRRenderModels interface version IVRRenderModels_005");
						this.failedLoadInterface = true;
					}
				}
				return this._instance;
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x000E9C82 File Offset: 0x000E8082
		public void Dispose()
		{
			if (this.needsShutdown)
			{
				OpenVR.Shutdown();
			}
		}

		// Token: 0x040017E1 RID: 6113
		private bool needsShutdown;

		// Token: 0x040017E2 RID: 6114
		private bool failedLoadInterface;

		// Token: 0x040017E3 RID: 6115
		private CVRRenderModels _instance;
	}
}
