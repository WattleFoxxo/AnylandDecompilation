using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class InputToEvent : MonoBehaviour
{
	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001C944 File Offset: 0x0001AD44
	// (set) Token: 0x06000623 RID: 1571 RVA: 0x0001C94B File Offset: 0x0001AD4B
	public static GameObject goPointedAt { get; private set; }

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001C953 File Offset: 0x0001AD53
	public Vector2 DragVector
	{
		get
		{
			return (!this.Dragging) ? Vector2.zero : (this.currentPos - this.pressedPosition);
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0001C97B File Offset: 0x0001AD7B
	private void Start()
	{
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0001C98C File Offset: 0x0001AD8C
	private void Update()
	{
		if (this.DetectPointedAtGameObject)
		{
			InputToEvent.goPointedAt = this.RaycastObject(Input.mousePosition);
		}
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			this.currentPos = touch.position;
			if (touch.phase == TouchPhase.Began)
			{
				this.Press(touch.position);
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				this.Release(touch.position);
			}
			return;
		}
		this.currentPos = Input.mousePosition;
		if (Input.GetMouseButtonDown(0))
		{
			this.Press(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.Release(Input.mousePosition);
		}
		if (Input.GetMouseButtonDown(1))
		{
			this.pressedPosition = Input.mousePosition;
			this.lastGo = this.RaycastObject(this.pressedPosition);
			if (this.lastGo != null)
			{
				this.lastGo.SendMessage("OnPressRight", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0001CAA6 File Offset: 0x0001AEA6
	private void Press(Vector2 screenPos)
	{
		this.pressedPosition = screenPos;
		this.Dragging = true;
		this.lastGo = this.RaycastObject(screenPos);
		if (this.lastGo != null)
		{
			this.lastGo.SendMessage("OnPress", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0001CAE8 File Offset: 0x0001AEE8
	private void Release(Vector2 screenPos)
	{
		if (this.lastGo != null)
		{
			GameObject gameObject = this.RaycastObject(screenPos);
			if (gameObject == this.lastGo)
			{
				this.lastGo.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
			this.lastGo.SendMessage("OnRelease", SendMessageOptions.DontRequireReceiver);
			this.lastGo = null;
		}
		this.pressedPosition = Vector2.zero;
		this.Dragging = false;
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0001CB5C File Offset: 0x0001AF5C
	private GameObject RaycastObject(Vector2 screenPos)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.m_Camera.ScreenPointToRay(screenPos), out raycastHit, 200f))
		{
			InputToEvent.inputHitPos = raycastHit.point;
			return raycastHit.collider.gameObject;
		}
		return null;
	}

	// Token: 0x04000493 RID: 1171
	private GameObject lastGo;

	// Token: 0x04000494 RID: 1172
	public static Vector3 inputHitPos;

	// Token: 0x04000495 RID: 1173
	public bool DetectPointedAtGameObject;

	// Token: 0x04000497 RID: 1175
	private Vector2 pressedPosition = Vector2.zero;

	// Token: 0x04000498 RID: 1176
	private Vector2 currentPos = Vector2.zero;

	// Token: 0x04000499 RID: 1177
	public bool Dragging;

	// Token: 0x0400049A RID: 1178
	private Camera m_Camera;
}
