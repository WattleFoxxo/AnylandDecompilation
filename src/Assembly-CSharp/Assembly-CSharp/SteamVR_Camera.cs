using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000290 RID: 656
[RequireComponent(typeof(Camera))]
public class SteamVR_Camera : MonoBehaviour
{
	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x060018DF RID: 6367 RVA: 0x000E2A9C File Offset: 0x000E0E9C
	public Transform head
	{
		get
		{
			return this._head;
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x060018E0 RID: 6368 RVA: 0x000E2AA4 File Offset: 0x000E0EA4
	public Transform offset
	{
		get
		{
			return this._head;
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x060018E1 RID: 6369 RVA: 0x000E2AAC File Offset: 0x000E0EAC
	public Transform origin
	{
		get
		{
			return this._head.parent;
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060018E2 RID: 6370 RVA: 0x000E2AB9 File Offset: 0x000E0EB9
	public Transform ears
	{
		get
		{
			return this._ears;
		}
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x000E2AC1 File Offset: 0x000E0EC1
	public Ray GetRay()
	{
		return new Ray(this._head.position, this._head.forward);
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060018E4 RID: 6372 RVA: 0x000E2ADE File Offset: 0x000E0EDE
	// (set) Token: 0x060018E5 RID: 6373 RVA: 0x000E2AE5 File Offset: 0x000E0EE5
	public static float sceneResolutionScale
	{
		get
		{
			return XRSettings.eyeTextureResolutionScale;
		}
		set
		{
			XRSettings.eyeTextureResolutionScale = value;
		}
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x000E2AED File Offset: 0x000E0EED
	private void OnDisable()
	{
		SteamVR_Render.Remove(this);
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x000E2AF8 File Offset: 0x000E0EF8
	private void OnEnable()
	{
		Transform transform = base.transform;
		if (this.head != transform)
		{
			this.Expand();
			transform.parent = this.origin;
			while (this.head.childCount > 0)
			{
				this.head.GetChild(0).parent = transform;
			}
			this.head.parent = transform;
			this.head.localPosition = Vector3.zero;
			this.head.localRotation = Quaternion.identity;
			this.head.localScale = Vector3.one;
			this.head.gameObject.SetActive(false);
			this._head = transform;
		}
		if (this.ears == null)
		{
			SteamVR_Ears componentInChildren = base.transform.GetComponentInChildren<SteamVR_Ears>();
			if (componentInChildren != null)
			{
				this._ears = componentInChildren.transform;
			}
		}
		if (this.ears != null)
		{
			this.ears.GetComponent<SteamVR_Ears>().vrcam = this;
		}
		SteamVR_Render.Add(this);
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x000E2C08 File Offset: 0x000E1008
	private void Awake()
	{
		this.ForceLast();
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x000E2C10 File Offset: 0x000E1010
	public void ForceLast()
	{
		if (SteamVR_Camera.values != null)
		{
			IDictionaryEnumerator enumerator = SteamVR_Camera.values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					FieldInfo fieldInfo = dictionaryEntry.Key as FieldInfo;
					fieldInfo.SetValue(this, dictionaryEntry.Value);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
			SteamVR_Camera.values = null;
		}
		else
		{
			Component[] array = base.GetComponents<Component>();
			for (int i = 0; i < array.Length; i++)
			{
				SteamVR_Camera steamVR_Camera = array[i] as SteamVR_Camera;
				if (steamVR_Camera != null && steamVR_Camera != this)
				{
					global::UnityEngine.Object.DestroyImmediate(steamVR_Camera);
				}
			}
			array = base.GetComponents<Component>();
			if (this != array[array.Length - 1])
			{
				SteamVR_Camera.values = new Hashtable();
				FieldInfo[] fields = base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo2 in fields)
				{
					if (fieldInfo2.IsPublic || fieldInfo2.IsDefined(typeof(SerializeField), true))
					{
						SteamVR_Camera.values[fieldInfo2] = fieldInfo2.GetValue(this);
					}
				}
				GameObject gameObject = base.gameObject;
				global::UnityEngine.Object.DestroyImmediate(this);
				gameObject.AddComponent<SteamVR_Camera>().ForceLast();
			}
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060018EA RID: 6378 RVA: 0x000E2D98 File Offset: 0x000E1198
	public string baseName
	{
		get
		{
			return (!base.name.EndsWith(string.Empty)) ? base.name : base.name.Substring(0, base.name.Length - string.Empty.Length);
		}
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x000E2DE8 File Offset: 0x000E11E8
	public void Expand()
	{
		Transform transform = base.transform.parent;
		if (transform == null)
		{
			transform = new GameObject(base.name + " (origin)").transform;
			transform.localPosition = base.transform.localPosition;
			transform.localRotation = base.transform.localRotation;
			transform.localScale = base.transform.localScale;
		}
		if (this.head == null)
		{
			this._head = new GameObject(base.name + " (head)", new Type[] { typeof(SteamVR_TrackedObject) }).transform;
			this.head.parent = transform;
			this.head.position = base.transform.position;
			this.head.rotation = base.transform.rotation;
			this.head.localScale = Vector3.one;
			this.head.tag = base.tag;
			Camera component = this.head.GetComponent<Camera>();
			component.clearFlags = CameraClearFlags.Nothing;
			component.cullingMask = 0;
			component.eventMask = 0;
			component.orthographic = true;
			component.orthographicSize = 1f;
			component.nearClipPlane = 0f;
			component.farClipPlane = 1f;
			component.useOcclusionCulling = false;
		}
		if (base.transform.parent != this.head)
		{
			base.transform.parent = this.head;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			base.transform.localScale = Vector3.one;
			while (base.transform.childCount > 0)
			{
				base.transform.GetChild(0).parent = this.head;
			}
			GUILayer component2 = base.GetComponent<GUILayer>();
			if (component2 != null)
			{
				global::UnityEngine.Object.DestroyImmediate(component2);
				this.head.gameObject.AddComponent<GUILayer>();
			}
			AudioListener component3 = base.GetComponent<AudioListener>();
			if (component3 != null)
			{
				global::UnityEngine.Object.DestroyImmediate(component3);
				this._ears = new GameObject(base.name + " (ears)", new Type[] { typeof(SteamVR_Ears) }).transform;
				this.ears.parent = this._head;
				this.ears.localPosition = Vector3.zero;
				this.ears.localRotation = Quaternion.identity;
				this.ears.localScale = Vector3.one;
			}
		}
		if (!base.name.EndsWith(string.Empty))
		{
			base.name += string.Empty;
		}
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x000E30B4 File Offset: 0x000E14B4
	public void Collapse()
	{
		base.transform.parent = null;
		while (this.head.childCount > 0)
		{
			this.head.GetChild(0).parent = base.transform;
		}
		GUILayer component = this.head.GetComponent<GUILayer>();
		if (component != null)
		{
			global::UnityEngine.Object.DestroyImmediate(component);
			base.gameObject.AddComponent<GUILayer>();
		}
		if (this.ears != null)
		{
			while (this.ears.childCount > 0)
			{
				this.ears.GetChild(0).parent = base.transform;
			}
			global::UnityEngine.Object.DestroyImmediate(this.ears.gameObject);
			this._ears = null;
			base.gameObject.AddComponent(typeof(AudioListener));
		}
		if (this.origin != null)
		{
			if (this.origin.name.EndsWith(" (origin)"))
			{
				Transform origin = this.origin;
				while (origin.childCount > 0)
				{
					origin.GetChild(0).parent = origin.parent;
				}
				global::UnityEngine.Object.DestroyImmediate(origin.gameObject);
			}
			else
			{
				base.transform.parent = this.origin;
			}
		}
		global::UnityEngine.Object.DestroyImmediate(this.head.gameObject);
		this._head = null;
		if (base.name.EndsWith(string.Empty))
		{
			base.name = base.name.Substring(0, base.name.Length - string.Empty.Length);
		}
	}

	// Token: 0x04001716 RID: 5910
	[SerializeField]
	private Transform _head;

	// Token: 0x04001717 RID: 5911
	[SerializeField]
	private Transform _ears;

	// Token: 0x04001718 RID: 5912
	public bool wireframe;

	// Token: 0x04001719 RID: 5913
	private static Hashtable values;

	// Token: 0x0400171A RID: 5914
	private const string eyeSuffix = "";

	// Token: 0x0400171B RID: 5915
	private const string earsSuffix = " (ears)";

	// Token: 0x0400171C RID: 5916
	private const string headSuffix = " (head)";

	// Token: 0x0400171D RID: 5917
	private const string originSuffix = " (origin)";
}
