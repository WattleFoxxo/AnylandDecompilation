using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class SteamVR_LaserPointer : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x0600186E RID: 6254 RVA: 0x000E0A74 File Offset: 0x000DEE74
	// (remove) Token: 0x0600186F RID: 6255 RVA: 0x000E0AAC File Offset: 0x000DEEAC
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PointerEventHandler PointerIn;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06001870 RID: 6256 RVA: 0x000E0AE4 File Offset: 0x000DEEE4
	// (remove) Token: 0x06001871 RID: 6257 RVA: 0x000E0B1C File Offset: 0x000DEF1C
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PointerEventHandler PointerOut;

	// Token: 0x06001872 RID: 6258 RVA: 0x000E0B54 File Offset: 0x000DEF54
	private void Start()
	{
		this.holder = new GameObject();
		this.holder.transform.parent = base.transform;
		this.holder.transform.localPosition = Vector3.zero;
		this.pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.pointer.transform.parent = this.holder.transform;
		this.pointer.transform.localScale = new Vector3(this.thickness, this.thickness, 100f);
		this.pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
		BoxCollider component = this.pointer.GetComponent<BoxCollider>();
		if (this.addRigidBody)
		{
			if (component)
			{
				component.isTrigger = true;
			}
			Rigidbody rigidbody = this.pointer.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
		}
		else if (component)
		{
			global::UnityEngine.Object.Destroy(component);
		}
		Material material = new Material(Shader.Find("Unlit/Color"));
		material.SetColor("_Color", this.color);
		this.pointer.GetComponent<MeshRenderer>().material = material;
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x000E0C8C File Offset: 0x000DF08C
	public virtual void OnPointerIn(PointerEventArgs e)
	{
		if (this.PointerIn != null)
		{
			this.PointerIn(this, e);
		}
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x000E0CA6 File Offset: 0x000DF0A6
	public virtual void OnPointerOut(PointerEventArgs e)
	{
		if (this.PointerOut != null)
		{
			this.PointerOut(this, e);
		}
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x000E0CC0 File Offset: 0x000DF0C0
	private void Update()
	{
		if (!this.isActive)
		{
			this.isActive = true;
			base.transform.GetChild(0).gameObject.SetActive(true);
		}
		float num = 100f;
		SteamVR_TrackedController component = base.GetComponent<SteamVR_TrackedController>();
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		RaycastHit raycastHit;
		bool flag = Physics.Raycast(ray, out raycastHit);
		if (this.previousContact && this.previousContact != raycastHit.transform)
		{
			PointerEventArgs pointerEventArgs = default(PointerEventArgs);
			if (component != null)
			{
				pointerEventArgs.controllerIndex = component.controllerIndex;
			}
			pointerEventArgs.distance = 0f;
			pointerEventArgs.flags = 0U;
			pointerEventArgs.target = this.previousContact;
			this.OnPointerOut(pointerEventArgs);
			this.previousContact = null;
		}
		if (flag && this.previousContact != raycastHit.transform)
		{
			PointerEventArgs pointerEventArgs2 = default(PointerEventArgs);
			if (component != null)
			{
				pointerEventArgs2.controllerIndex = component.controllerIndex;
			}
			pointerEventArgs2.distance = raycastHit.distance;
			pointerEventArgs2.flags = 0U;
			pointerEventArgs2.target = raycastHit.transform;
			this.OnPointerIn(pointerEventArgs2);
			this.previousContact = raycastHit.transform;
		}
		if (!flag)
		{
			this.previousContact = null;
		}
		if (flag && raycastHit.distance < 100f)
		{
			num = raycastHit.distance;
		}
		if (component != null && component.triggerPressed)
		{
			this.pointer.transform.localScale = new Vector3(this.thickness * 5f, this.thickness * 5f, num);
		}
		else
		{
			this.pointer.transform.localScale = new Vector3(this.thickness, this.thickness, num);
		}
		this.pointer.transform.localPosition = new Vector3(0f, 0f, num / 2f);
	}

	// Token: 0x040016D5 RID: 5845
	public bool active = true;

	// Token: 0x040016D6 RID: 5846
	public Color color;

	// Token: 0x040016D7 RID: 5847
	public float thickness = 0.002f;

	// Token: 0x040016D8 RID: 5848
	public GameObject holder;

	// Token: 0x040016D9 RID: 5849
	public GameObject pointer;

	// Token: 0x040016DA RID: 5850
	private bool isActive;

	// Token: 0x040016DB RID: 5851
	public bool addRigidBody;

	// Token: 0x040016DC RID: 5852
	public Transform reference;

	// Token: 0x040016DF RID: 5855
	private Transform previousContact;
}
