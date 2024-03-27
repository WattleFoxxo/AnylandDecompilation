using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020001E8 RID: 488
public class DesktopManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06001062 RID: 4194 RVA: 0x0008C9A3 File Offset: 0x0008ADA3
	// (set) Token: 0x06001063 RID: 4195 RVA: 0x0008C9AB File Offset: 0x0008ADAB
	public ManagerStatus status { get; private set; }

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06001064 RID: 4196 RVA: 0x0008C9B4 File Offset: 0x0008ADB4
	// (set) Token: 0x06001065 RID: 4197 RVA: 0x0008C9BC File Offset: 0x0008ADBC
	public string failMessage { get; private set; }

	// Token: 0x06001066 RID: 4198 RVA: 0x0008C9C5 File Offset: 0x0008ADC5
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06001067 RID: 4199 RVA: 0x0008C9D5 File Offset: 0x0008ADD5
	// (set) Token: 0x06001068 RID: 4200 RVA: 0x0008C9DD File Offset: 0x0008ADDD
	public bool showDialogBackside { get; private set; }

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x0008C9E6 File Offset: 0x0008ADE6
	// (set) Token: 0x0600106A RID: 4202 RVA: 0x0008C9EE File Offset: 0x0008ADEE
	public MovementSmoothing movementSmoothing { get; private set; }

	// Token: 0x0600106B RID: 4203 RVA: 0x0008C9F8 File Offset: 0x0008ADF8
	private void Start()
	{
		this.SetMovementSmoothing(MovementSmoothing.Medium);
		this.rig = Managers.treeManager.GetObject("/OurPersonRig");
		this.headCore = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		this.handCore[Side.Left] = Managers.treeManager.GetObject("/OurPersonRig/HandCoreLeft");
		this.handCore[Side.Right] = Managers.treeManager.GetObject("/OurPersonRig/HandCoreRight");
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HandCoreLeft/HandDot");
		GameObject object2 = Managers.treeManager.GetObject("/OurPersonRig/HandCoreRight/HandDot");
		this.body.enabled = false;
		this.person = this.rig.GetComponent<Person>();
		this.hand[Side.Left] = this.handCore[Side.Left].GetComponent<Hand>();
		this.hand[Side.Right] = this.handCore[Side.Right].GetComponent<Hand>();
		this.handDot[Side.Left] = @object.GetComponent<HandDot>();
		this.handDot[Side.Right] = object2.GetComponent<HandDot>();
		this.handOffset[Side.Left] = Vector3.zero;
		this.handOffset[Side.Right] = Vector3.zero;
		this.camera = this.headCore.GetComponent<Camera>();
		this.lastFinalizedPosition = this.headCore.transform.position;
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x0008CB50 File Offset: 0x0008AF50
	private void Update()
	{
		if (this.status != ManagerStatus.Started)
		{
			return;
		}
		this.ReactivateHandsInCaseSteamVRDisabledThem();
		if (this.WASDPressed() && !this.showTextInput)
		{
			if (!this.didEnterDesktopModeWithEscape)
			{
				this.showBottomHelp = true;
			}
			this.SetDesktopMode(true);
		}
		if (CrossDevice.desktopMode)
		{
			if (this.hand[CrossDevice.desktopDialogSide].currentDialogType == DialogType.Keyboard)
			{
				this.ForwardKeysToKeyboardDialog();
			}
			else if (this.AreaLoaded())
			{
				this.HandleRidingOnPlacedSubThings();
				Vector3? vector = this.autoMoveTarget;
				if (vector != null)
				{
					this.HandleAutoMovement();
				}
				else if (this.person.ridingBeacon == null)
				{
					this.HandleHeadPosition();
					this.HandleMovement();
				}
				if (!this.showTextInput)
				{
					this.HandleInventory();
				}
				this.HandleHeldObjectRotation();
			}
			if (this.hand[CrossDevice.desktopDialogSide].HasNonStartDialogOpen() || this.showTextInput || Our.mode == EditModes.Inventory)
			{
				DialogType currentDialogType = this.hand[CrossDevice.desktopDialogSide].currentDialogType;
				if (currentDialogType == DialogType.ApproveBody || currentDialogType == DialogType.OwnProfile || this.showDialogBackside)
				{
					this.ResetHeadPitchRoll();
				}
				this.SetCursorUseActive(true);
				this.HandleMouseDialogClicks();
			}
			else if (Input.GetKey(KeyCode.LeftControl))
			{
				this.SetCursorUseActive(false);
				this.UpdateHandPositionBasedOnMouse();
			}
			else if (!(Managers.cameraManager != null) || !Managers.cameraManager.isStreamingToDesktop)
			{
				this.SetCursorUseActive(false);
				this.UpdateCameraRotationBasedOnMouse();
			}
			this.ResetHeadRollIfNeeded();
			this.HandleHandsFurtherBack();
			this.PositionHands();
			if (Our.mode == EditModes.Environment)
			{
				this.UpdateSunBasedOnMouseWheel();
			}
			else
			{
				IEnumerator enumerator = Enum.GetValues(typeof(Side)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Side side = (Side)obj;
						this.UpdateHandForwardStretch(side);
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
			}
			this.HandleFinalizeBasedOnPosition();
			this.HandleFieldOfViewZoom();
			this.wasGrounded = this.body.isGrounded;
		}
		this.HandleShortcuts();
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0008CDAC File Offset: 0x0008B1AC
	private void HandleHeadPosition()
	{
		if (Misc.AltIsPressed() && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
		{
			float num = 1f * Time.deltaTime;
			if (Input.GetKey(KeyCode.UpArrow))
			{
				this.bodyOffsetY -= num;
				Vector3 vector = Vector3.up * Time.deltaTime;
				this.body.Move(vector);
			}
			else
			{
				this.bodyOffsetY += num;
			}
			this.bodyOffsetY = Mathf.Clamp(this.bodyOffsetY, 0f, 0.5f);
			this.UpdateBodyCenter();
		}
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x0008CE5B File Offset: 0x0008B25B
	private void UpdateBodyCenter()
	{
		this.body.center = new Vector3(0f, 0.499f + this.bodyOffsetY, 0f);
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x0008CE84 File Offset: 0x0008B284
	private void HandleInventory()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			if (Our.mode == EditModes.Inventory)
			{
				Managers.inventoryManager.CloseDialog();
				Our.SetPreviousMode();
			}
			else
			{
				Managers.inventoryManager.OpenDialog(Misc.GetAHandOfOurs(), false);
				Our.SetMode(EditModes.Inventory, false);
			}
		}
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x0008CED4 File Offset: 0x0008B2D4
	private void HandleFieldOfViewZoom()
	{
		if (Input.GetKey(KeyCode.LeftAlt))
		{
			float num = -Input.GetAxis("Mouse ScrollWheel");
			if (num != 0f)
			{
				this.fieldOfView += num * 100f;
				this.fieldOfView = Mathf.Clamp(this.fieldOfView, 10f, 60f);
			}
		}
		float num2 = Mathf.Lerp(this.camera.fieldOfView, this.fieldOfView, this.zoomSmoothingFactor);
		if (num2 != this.camera.fieldOfView)
		{
			this.camera.fieldOfView = num2;
		}
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x0008CF70 File Offset: 0x0008B370
	public void SetCursorUseActive(bool active)
	{
		if (active)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x0008CF95 File Offset: 0x0008B395
	private bool AreaLoaded()
	{
		return !Managers.areaManager.isTransportInProgress && Managers.areaManager.didFinishLoadingPlacements;
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x0008CFB4 File Offset: 0x0008B3B4
	private void HandleMovement()
	{
		float smoothDeltaTime = Time.smoothDeltaTime;
		if (CrossDevice.rigPositionIsAuthority)
		{
			this.body.transform.position = this.rig.transform.position;
			CrossDevice.rigPositionIsAuthority = false;
			this.body.transform.Translate(Vector3.up * 1f, Space.World);
		}
		if (this.isOnUnwalkable)
		{
			this.autoMoveTarget = new Vector3?(this.lastWalkablePosition);
			this.isOnUnwalkable = false;
			this.currentVelocity = Vector3.zero;
			return;
		}
		if (this.body.isGrounded)
		{
			this.lastWalkablePosition = this.body.transform.position;
		}
		float num = ((!Input.GetKey(KeyCode.LeftShift)) ? 2f : this.fastMovementSpeed);
		bool flag = Our.canEditFly && Our.mode == EditModes.Area && Managers.areaManager.weAreEditorOfCurrentArea;
		bool flag2 = Managers.areaManager.isZeroGravity || flag;
		Vector3 vector = this.headCore.transform.TransformDirection(Vector3.forward);
		if (!flag2 && !this.isTouchingClimbable)
		{
			vector.y = 0f;
		}
		vector = vector.normalized;
		Vector3 vector2 = new Vector3(vector.z, 0f, -vector.x);
		float num2 = 0f;
		if (Input.GetKey(KeyCode.A))
		{
			num2 = -1f;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			num2 = 1f;
		}
		float num3 = 0f;
		if (Input.GetKey(KeyCode.W))
		{
			num3 = 1f;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			num3 = -1f;
		}
		Vector3 vector3 = num2 * vector2 + num3 * vector;
		vector3 *= num;
		bool flag3 = false;
		if (this.isTouchingClimbable)
		{
			this.verticalVelocity = 0f;
			vector3 += num3 * 0.5f * Vector3.up;
			this.isTouchingClimbable = false;
		}
		else if (flag2)
		{
			this.verticalVelocity = 0f;
		}
		else
		{
			if (this.body.isGrounded)
			{
				if (this.verticalVelocity <= -1f)
				{
					Managers.soundManager.Play("bump", this.body.transform, 0.2f, false, false);
				}
				this.verticalVelocity = 0f;
				if (Input.GetButton("Jump"))
				{
					this.verticalVelocity = this.jumpSpeed;
					flag3 = true;
					Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "jumping", true);
					Managers.soundManager.Play("whoosh", this.body.transform, 0.5f, false, false);
				}
			}
			else
			{
				this.verticalVelocity += -9.81f * smoothDeltaTime;
			}
			this.verticalVelocity = Mathf.Clamp(this.verticalVelocity, -100f, 100f);
			vector3.y += this.verticalVelocity;
		}
		this.currentVelocity.y = vector3.y;
		if (this.slidiness > 1f)
		{
			this.currentVelocity = Vector3.Lerp(this.currentVelocity, vector3, 1f / this.slidiness);
		}
		else
		{
			this.currentVelocity = vector3;
		}
		Vector3 vector4 = this.currentVelocity;
		if (this.UseSmoothMovement() && !flag3)
		{
			vector4 = Vector3.Lerp(this.previousVelocity, vector4, this.movementSmoothingFactor);
		}
		this.body.Move(vector4 * smoothDeltaTime);
		this.previousVelocity = vector4;
		this.rig.transform.position = this.body.transform.position;
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x0008D3A0 File Offset: 0x0008B7A0
	private void HandleAutoMovement()
	{
		Vector3? vector = this.autoMoveTarget;
		Vector3 value = vector.Value;
		if (Vector3.Distance(base.transform.position, value) <= 10f)
		{
			float num = 20f * Time.deltaTime;
			this.body.transform.position = Vector3.MoveTowards(base.transform.position, value, num);
			this.rig.transform.position = this.body.transform.position;
			if (base.transform.position == value)
			{
				this.autoMoveTarget = null;
			}
		}
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x0008D44C File Offset: 0x0008B84C
	private void HandleRidingOnPlacedSubThings()
	{
		if (this.person.ridingBeacon == null)
		{
			if ((this.body.isGrounded && !this.wasGrounded) || this.AnyWASDKeyUp())
			{
				Vector3 zero = Vector3.zero;
				ThingPart ridableThingPartBelowUs = this.GetRidableThingPartBelowUs(ref zero);
				if (ridableThingPartBelowUs != null)
				{
					Managers.personManager.DoAddRidingBeacon(ridableThingPartBelowUs.GetParentThing(), zero);
					CrossDevice.rigPositionIsAuthority = true;
					this.autoMoveTarget = null;
				}
			}
		}
		else if (this.AnyWASDKeyUp() || Input.GetKeyUp(KeyCode.Space))
		{
			Managers.personManager.DoRemoveRidingBeacon();
		}
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0008D504 File Offset: 0x0008B904
	private void UpdateSunBasedOnMouseWheel()
	{
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			Vector3 localEulerAngles = this.sun.transform.localEulerAngles;
			localEulerAngles.x += axis * 80f;
			this.sun.transform.localEulerAngles = localEulerAngles;
			base.CancelInvoke("SaveSunData");
			base.Invoke("SaveSunData", 2f);
		}
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x0008D579 File Offset: 0x0008B979
	private void SaveSunData()
	{
		Managers.personManager.DoSetEnvironmentChanger(this.sun);
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x0008D58C File Offset: 0x0008B98C
	private ThingPart GetRidableThingPartBelowUs(ref Vector3 hitPoint)
	{
		ThingPart thingPart = null;
		Ray ray = new Ray(base.transform.position + Vector3.up * 1f, -Vector3.up);
		float num = 2f;
		foreach (RaycastHit raycastHit in (from h in Physics.RaycastAll(ray, num)
			orderby h.distance
			select h).ToArray<RaycastHit>())
		{
			GameObject gameObject = raycastHit.collider.gameObject;
			bool flag = raycastHit.normal.y >= 0.5f;
			if (gameObject.tag == "ThingPart" && flag)
			{
				ThingPart component = gameObject.GetComponent<ThingPart>();
				if (component != null && component.WeArePartOfPlacedSubThing())
				{
					thingPart = component;
					hitPoint = raycastHit.point;
					break;
				}
			}
		}
		return thingPart;
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x0008D6A8 File Offset: 0x0008BAA8
	private void HandleHeldObjectRotation()
	{
		Side oppositeSide = Misc.GetOppositeSide(CrossDevice.desktopDialogSide);
		if (this.handDot[oppositeSide].currentlyHeldObject != null)
		{
			Vector3 vector = this.handDot[oppositeSide].currentlyHeldObject.transform.position;
			Vector3 zero = Vector3.zero;
			Transform transform = this.headCore.transform;
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				float num = 0.25f * Time.deltaTime;
				if (this.GetKeyOrKeyUp(KeyCode.UpArrow))
				{
					vector += num * transform.up;
				}
				else if (this.GetKeyOrKeyUp(KeyCode.DownArrow))
				{
					vector -= num * transform.up;
				}
				if (this.GetKeyOrKeyUp(KeyCode.LeftArrow))
				{
					vector -= num * transform.right;
				}
				else if (this.GetKeyOrKeyUp(KeyCode.RightArrow))
				{
					vector += num * transform.right;
				}
			}
			else if (!Misc.AltIsPressed())
			{
				float num2 = 100f * Time.deltaTime;
				if (this.GetKeyOrKeyUp(KeyCode.UpArrow))
				{
					zero.x -= num2;
				}
				else if (this.GetKeyOrKeyUp(KeyCode.DownArrow))
				{
					zero.x += num2;
				}
				if (this.GetKeyOrKeyUp(KeyCode.LeftArrow))
				{
					zero.y += num2;
				}
				else if (this.GetKeyOrKeyUp(KeyCode.RightArrow))
				{
					zero.y -= num2;
				}
			}
			if (zero != Vector3.zero || vector != this.handDot[oppositeSide].currentlyHeldObject.transform.position)
			{
				Thing component = this.handDot[oppositeSide].currentlyHeldObject.GetComponent<Thing>();
				if (component != null)
				{
					component.EndPushBackIfNeededAsLetGo();
				}
				this.handDot[oppositeSide].currentlyHeldObject.transform.position = vector;
				this.RotateHandThenRevertHandWhileHeldObjectUnhinged(oppositeSide, zero);
				if (component != null)
				{
					component.MemorizeOriginalTransform(false);
					if (this.AnyArrowKeyUp())
					{
						Managers.personManager.DoChangeHeldThingPositionRotation(component, false);
					}
				}
			}
		}
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x0008D928 File Offset: 0x0008BD28
	private void RotateHandThenRevertHandWhileHeldObjectUnhinged(Side side, Vector3 rotation)
	{
		Vector3 eulerAngles = this.hand[side].transform.eulerAngles;
		Transform parent = this.handDot[side].currentlyHeldObject.transform.parent;
		this.hand[side].transform.Rotate(-rotation);
		this.handDot[side].currentlyHeldObject.transform.parent = null;
		this.hand[side].transform.eulerAngles = eulerAngles;
		this.handDot[side].currentlyHeldObject.transform.parent = parent;
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x0008D9D4 File Offset: 0x0008BDD4
	private void UpdateHandPositionBasedOnMouse()
	{
		Side oppositeSide = Misc.GetOppositeSide(CrossDevice.desktopDialogSide);
		Vector3 vector = this.handOffset[oppositeSide];
		vector.x = Mathf.Clamp(vector.x + Input.GetAxis("Mouse X") * 0.01f, -0.35f, 0.35f);
		vector.y = Mathf.Clamp(vector.y + Input.GetAxis("Mouse Y") * 0.01f, -0.35f, 0.35f);
		if (vector != this.handOffset[oppositeSide])
		{
			this.timeWhenHandOffsetResets = Time.time + 10f;
			this.handOffset[oppositeSide] = vector;
		}
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x0008DA8C File Offset: 0x0008BE8C
	private void UpdateCameraRotationBasedOnMouse()
	{
		this.headRotation.pitch -= Input.GetAxis("Mouse Y") * 2.5f;
		if (Input.GetKey(KeyCode.Q))
		{
			this.headRotation.roll -= Input.GetAxis("Mouse X") * 2.5f;
			this.timeWhenHeadRollResets = new float?(Time.time + 4f);
		}
		else
		{
			this.headRotation.yaw += Input.GetAxis("Mouse X") * 2.5f;
		}
		this.headRotation.pitch = Mathf.Clamp(this.headRotation.pitch, -85f, 85f);
		this.headRotation.roll = Mathf.Clamp(this.headRotation.roll, -85f, 85f);
		if (this.UseSmoothMovement())
		{
			this.headCore.transform.eulerAngles = Misc.AngleLerp(this.headCore.transform.eulerAngles, this.headRotation.GetEulerAngles(), this.rotationSmoothingFactor);
		}
		else
		{
			this.headCore.transform.eulerAngles = this.headRotation.GetEulerAngles();
		}
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x0008DBD4 File Offset: 0x0008BFD4
	private void ResetHeadRollIfNeeded()
	{
		float? num = this.timeWhenHeadRollResets;
		if (num != null)
		{
			float time = Time.time;
			float? num2 = this.timeWhenHeadRollResets;
			if (time >= num2.Value)
			{
				this.headRotation.roll = Mathf.MoveTowards(this.headRotation.roll, 0f, 50f * Time.deltaTime);
				this.headCore.transform.eulerAngles = this.headRotation.GetEulerAngles();
				if (this.headRotation.roll == 0f)
				{
					this.timeWhenHeadRollResets = null;
				}
			}
		}
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x0008DC78 File Offset: 0x0008C078
	private void UpdateHandForwardStretch(Side side)
	{
		if (side != CrossDevice.desktopDialogSide && !Input.GetKey(KeyCode.LeftAlt))
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis != 0f)
			{
				this.timeWhenHandStretchResets[side] = Time.time + 10f;
				Dictionary<Side, float> dictionary;
				(dictionary = this.handForwardStretchTarget)[side] = dictionary[side] + axis * 1f;
			}
		}
		float num = 0.45f;
		float num2 = 40f;
		if (this.headRotation.pitch > num2)
		{
			float num3 = this.headRotation.pitch - num2;
			num3 = Mathf.Clamp(num3 / 30f, 0f, 0.7f);
			num += num3;
		}
		this.handForwardStretchTarget[side] = Mathf.Clamp(this.handForwardStretchTarget[side], -0.25f, num);
		float num4 = 4f;
		this.handForwardStretch[side] = Mathf.MoveTowards(this.handForwardStretch[side], this.handForwardStretchTarget[side], num4 * Time.deltaTime);
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x0008DD94 File Offset: 0x0008C194
	public void ReactivateHandsInCaseSteamVRDisabledThem()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(Side)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Side side = (Side)obj;
				if (this.handCore[side] != null && !this.handCore[side].activeSelf)
				{
					this.handCore[side].SetActive(true);
				}
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
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x0008DE3C File Offset: 0x0008C23C
	private void HandleFinalizeBasedOnPosition()
	{
		if (Vector3.Distance(this.lastFinalizedPosition, this.headCore.transform.position) >= 2.5f)
		{
			this.lastFinalizedPosition = this.headCore.transform.position;
			Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
			Managers.behaviorScriptManager.TriggerEventsRelatedToPosition();
		}
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x0008DEA0 File Offset: 0x0008C2A0
	private void HandleMouseDialogClicks()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
		{
			Ray ray = this.camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] array = (from h in Physics.RaycastAll(ray, 10f)
				orderby h.distance
				select h).ToArray<RaycastHit>();
			if (Input.GetMouseButtonDown(0))
			{
				foreach (RaycastHit raycastHit in array)
				{
					string tag = raycastHit.collider.gameObject.tag;
					if (tag == "PressableDialogPart")
					{
						DialogPart component = raycastHit.collider.transform.parent.GetComponent<DialogPart>();
						component.Press(raycastHit.collider);
						break;
					}
					if (tag == "ThingPartBase")
					{
						this.AddThingPartToCreation(raycastHit.collider.gameObject);
						break;
					}
					if (raycastHit.collider.transform.parent != null && raycastHit.collider.transform.parent.CompareTag("GrabbableDialogThingThumb"))
					{
						Side oppositeSide = Misc.GetOppositeSide(CrossDevice.desktopDialogSide);
						GameObject gameObject = raycastHit.collider.transform.parent.gameObject;
						GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
						Thing component2 = gameObject2.GetComponent<Thing>();
						component2.remainsHeld = true;
						component2.isInInventoryOrDialog = false;
						component2.isInInventory = false;
						component2.isGiftInDialog = false;
						this.handDot[oppositeSide].currentlyHeldObject = gameObject2;
						this.handDot[oppositeSide].holdableInHand = gameObject2;
						gameObject2.transform.parent = this.handDot[oppositeSide].transform.parent;
						gameObject2.transform.localScale = Vector3.one;
						gameObject2.transform.localPosition = Vector3.zero;
						gameObject2.transform.localRotation = Quaternion.identity;
						this.handDot[oppositeSide].ignoreNextHoldableTrigger = true;
						raycastHit.collider.gameObject.GetComponent<Thing>();
						Managers.soundManager.Play("clone", this.handDot[oppositeSide].transform, 0.3f, false, false);
						Managers.personManager.DoHoldThing(gameObject2, gameObject);
						this.CloseDialog();
						break;
					}
					if (tag == "ThingPart" && Our.mode == EditModes.Inventory)
					{
						Thing componentInParent = raycastHit.collider.gameObject.GetComponentInParent<Thing>();
						if (componentInParent != null && componentInParent.isInInventory)
						{
							if (Managers.areaManager.weAreEditorOfCurrentArea && Our.createThingsInDesktopMode)
							{
								this.PlaceThing(componentInParent);
								Managers.inventoryManager.CloseDialog();
								Our.SetMode(EditModes.Area, false);
							}
							break;
						}
					}
				}
			}
			else
			{
				foreach (RaycastHit raycastHit2 in array)
				{
					DialogSliderCollisionCube component3 = raycastHit2.collider.gameObject.GetComponent<DialogSliderCollisionCube>();
					if (component3 != null)
					{
						Side oppositeSide2 = Misc.GetOppositeSide(CrossDevice.desktopDialogSide);
						component3.PushInDirection(this.handDot[oppositeSide2]);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x0008E214 File Offset: 0x0008C614
	private void PlaceThing(Thing thing)
	{
		Transform transform = this.headCore.transform;
		Vector3 vector = transform.position + transform.forward * 1f;
		Managers.soundManager.Play("putDown", vector, 0.5f, false, false);
		base.StartCoroutine(Managers.personManager.DoPlaceThing(thing.thingId, vector, thing.transform.rotation));
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x0008E284 File Offset: 0x0008C684
	private void AddThingPartToCreation(GameObject basePart)
	{
		if (CreationHelper.thingBeingEdited == null)
		{
			return;
		}
		Thing component = CreationHelper.thingBeingEdited.GetComponent<Thing>();
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(basePart);
		ThingPart component2 = gameObject.GetComponent<ThingPart>();
		component2.name = Misc.RemoveCloneFromName(component2.name);
		component2.tag = "ThingPart";
		component2.transform.parent = CreationHelper.thingBeingEdited.transform;
		if (component != null && component2 != null)
		{
			if (!component.ContainsUnremovableCenter() && CreationHelper.thingBeingEdited.transform.childCount < 1000)
			{
				Side oppositeSide = Misc.GetOppositeSide(CrossDevice.desktopDialogSide);
				component2.transform.position = this.handDot[oppositeSide].transform.position;
				component2.transform.rotation = this.handDot[oppositeSide].transform.rotation;
				component2.transform.localScale = Managers.thingManager.GetBaseShapeDropUpScale(component2.baseType);
				component2.UpdateMaterial();
				component2.SetStatePropertiesByTransform(false);
				Our.SetLastTransformHandled(component2.transform);
				SnapHelper.SnapAllNeeded(component, component2.gameObject, component2.transform, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero);
				Managers.soundManager.Play("clone", this.handDot[oppositeSide].transform, 0.3f, false, false);
			}
			else
			{
				global::UnityEngine.Object.Destroy(component2.gameObject);
				Managers.soundManager.Play("no", base.transform, 1f, false, false);
			}
		}
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x0008E424 File Offset: 0x0008C824
	private void CloseDialog()
	{
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null)
		{
			KeyboardDialog component = currentNonStartDialog.GetComponent<KeyboardDialog>();
			if (component != null)
			{
				component.HandleAndCloseDialog();
			}
			else
			{
				Transform parent = currentNonStartDialog.transform.parent;
				if (parent != null)
				{
					Hand component2 = parent.GetComponent<Hand>();
					if (component2 != null)
					{
						component2.SwitchToNewDialog(DialogType.Start, string.Empty);
					}
				}
			}
		}
		this.showDialogBackside = false;
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x0008E4A1 File Offset: 0x0008C8A1
	private bool WASDPressed()
	{
		return Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d");
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x0008E4E0 File Offset: 0x0008C8E0
	private bool SetDesktopMode(bool mode)
	{
		bool flag = true;
		if (mode == CrossDevice.desktopMode)
		{
			flag = true;
		}
		else if (mode && Our.mode == EditModes.Thing)
		{
			flag = false;
		}
		else if (!(Managers.broadcastNetworkManager != null) || !Managers.broadcastNetworkManager.inRoom)
		{
			flag = false;
		}
		else
		{
			CrossDevice.desktopMode = mode;
			if (CrossDevice.desktopMode)
			{
				if (Managers.personManager.WeAreResized())
				{
					Managers.personManager.ResetAndCachePhotonRigScale();
				}
				this.person.ResetPositionAndRotation();
				Managers.achievementManager.RegisterAchievement(Achievement.MovedViaKeyboard);
			}
			this.camera.stereoTargetEye = ((!CrossDevice.desktopMode) ? StereoTargetEyeMask.Both : StereoTargetEyeMask.None);
			this.camera.fieldOfView = ((!CrossDevice.desktopMode) ? 111.7791f : 60f);
			this.fieldOfView = this.camera.fieldOfView;
			if (CrossDevice.desktopMode)
			{
				this.FixSkewedScreenResolutionProportions();
			}
			SteamVR_TrackedObject component = this.handCore[Side.Left].GetComponent<SteamVR_TrackedObject>();
			component.enabled = !CrossDevice.desktopMode;
			SteamVR_TrackedObject component2 = this.handCore[Side.Right].GetComponent<SteamVR_TrackedObject>();
			component2.enabled = !CrossDevice.desktopMode;
			this.SetCursorUseActive(!CrossDevice.desktopMode);
			Managers.filterManager.ApplySettings(false);
			this.body.enabled = CrossDevice.desktopMode;
			CrossDevice.rigPositionIsAuthority = !CrossDevice.desktopMode;
			QualitySettings.vSyncCount = ((!CrossDevice.desktopMode) ? 0 : 1);
			Application.targetFrameRate = ((!CrossDevice.desktopMode) ? (-1) : 60);
			this.bodyOffsetY = 0f;
			this.UpdateBodyCenter();
			if (CrossDevice.desktopMode)
			{
				Our.SetMode(EditModes.None, false);
				this.PositionHeadToDefault();
				this.DisableCollisionsBetweenPersonBodyAndCharacterController();
			}
			else
			{
				this.person.ResetPositionAndRotation();
			}
			Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
			Managers.filterManager.ApplySettings(false);
			bool flag2 = !CrossDevice.desktopMode;
			if (flag2)
			{
				Misc.ReloadScene();
			}
			Managers.optimizationManager.UpdateSettings();
			base.StartCoroutine(Managers.serverManager.RegisterUsageMode(mode, delegate(ResponseBase response)
			{
			}));
		}
		return flag;
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x0008E729 File Offset: 0x0008CB29
	private void FixSkewedScreenResolutionProportions()
	{
		if (!Universe.useWindowedApp)
		{
			Screen.SetResolution(Screen.width, Screen.height, true);
		}
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x0008E748 File Offset: 0x0008CB48
	public void DisableCollisionsBetweenPersonBodyAndCharacterController()
	{
		Component[] componentsInChildren = this.rig.GetComponentsInChildren(typeof(Collider), true);
		foreach (Component component in componentsInChildren)
		{
			Collider component2 = component.GetComponent<Collider>();
			Physics.IgnoreCollision(component2, this.body);
		}
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x0008E79B File Offset: 0x0008CB9B
	private void PositionHeadToDefault()
	{
		this.headCore.transform.rotation = Quaternion.identity;
		this.headCore.transform.position = Vector3.up * 1.75f;
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x0008E7D4 File Offset: 0x0008CBD4
	private void PositionHands()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(Side)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Side side = (Side)obj;
				if (this.handCore[side] != null)
				{
					this.PositionHand(side);
				}
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
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x0008E85C File Offset: 0x0008CC5C
	public void PositionHand(Side side)
	{
		float num = -0.2f;
		if (side == Side.Right)
		{
			num *= -1f;
		}
		this.handCore[side].transform.position = this.headCore.transform.position;
		this.handCore[side].transform.rotation = this.headCore.transform.rotation;
		bool flag = this.hand[side].HasNonStartDialogOpen();
		if (flag)
		{
			this.hand[side].dialogOpenFactor += Time.deltaTime * 10f;
			this.hand[side].dialogBacksideOpenFactor = this.GrowOrShrink(this.hand[side].dialogBacksideOpenFactor, 10f, this.showDialogBackside, 0f, 1f);
		}
		else
		{
			this.hand[side].dialogOpenFactor += Time.deltaTime * -10f;
			this.hand[side].dialogBacksideOpenFactor += Time.deltaTime * -10f;
		}
		this.hand[side].dialogOpenFactor = Mathf.Clamp(this.hand[side].dialogOpenFactor, 0f, 1f);
		this.hand[side].dialogBacksideOpenFactor = Mathf.Clamp(this.hand[side].dialogBacksideOpenFactor, 0f, 1f);
		bool flag2 = false;
		bool flag3 = false;
		DialogType currentDialogType = this.hand[side].currentDialogType;
		if (currentDialogType != DialogType.None)
		{
			flag2 = this.hand[side].currentDialog.GetComponent<Dialog>().isBig;
			flag3 = currentDialogType == DialogType.OwnProfile || currentDialogType == DialogType.BodyMotions || currentDialogType == DialogType.Microphone || currentDialogType == DialogType.ApproveBody;
		}
		float num2 = 1f;
		if (side == CrossDevice.desktopDialogSide)
		{
			bool flag4 = CrossDevice.GetPress(this.hand[side].controller, CrossDevice.button_context, side) || CrossDevice.GetPressUp(this.hand[side].controller, CrossDevice.button_context, side);
			this.handForwardLock = this.GrowOrShrink(this.handForwardLock, 20f, flag4, 0f, 1f);
			num2 -= this.handForwardLock;
		}
		float num3 = 1f - this.hand[side].dialogOpenFactor;
		Vector3 localEulerAngles = this.handCore[side].transform.localEulerAngles;
		localEulerAngles.x -= 30f * num2;
		localEulerAngles.x -= 55f * this.hand[side].dialogOpenFactor * num2;
		localEulerAngles.y += 170f * ((side != Side.Left) ? (-1f) : 1f) * this.hand[side].dialogBacksideOpenFactor * num2;
		localEulerAngles.x -= 20f * this.hand[side].dialogBacksideOpenFactor * num2;
		this.handCore[side].transform.localEulerAngles = localEulerAngles;
		this.handCore[side].transform.position += this.headCore.transform.forward * 0.1f * this.hand[side].dialogOpenFactor + this.headCore.transform.up * 0.2f * this.hand[side].dialogOpenFactor;
		if (flag2)
		{
			this.handCore[side].transform.position += this.headCore.transform.forward * 0.175f * this.hand[side].dialogOpenFactor;
		}
		else if (currentDialogType == DialogType.Keyboard)
		{
			this.handCore[side].transform.position += this.headCore.transform.forward * 0.125f * this.hand[side].dialogOpenFactor;
			this.handCore[side].transform.position += this.headCore.transform.up * -0.05f * this.hand[side].dialogOpenFactor;
		}
		if (flag3 && !this.showDialogBackside)
		{
			this.handCore[side].transform.position += this.headCore.transform.right * 0.085f * ((side != Side.Left) ? 1f : (-1f));
		}
		this.handCore[side].transform.position += this.headCore.transform.right * num + this.headCore.transform.up * -0.1f + this.headCore.transform.forward * 0.3f;
		this.handCore[side].transform.position += this.headCore.transform.up * -0.05f * num3;
		this.handCore[side].transform.position += this.headCore.transform.right * (0.15f * ((side != Side.Left) ? (-1f) : 1f)) * this.hand[side].dialogBacksideOpenFactor;
		this.handCore[side].transform.position += this.headCore.transform.forward * -0.07f * this.hand[side].dialogBacksideOpenFactor;
		if (!flag)
		{
			this.handCore[side].transform.position += this.headCore.transform.forward * this.handsFurtherBackStretch;
		}
		if (Managers.twitchManager != null && Managers.twitchManager.ChatDialogShows())
		{
			this.handCore[side].transform.position += this.headCore.transform.forward * 0.15f;
		}
		this.handCore[side].transform.position += this.headCore.transform.forward * this.handForwardStretch[side];
		this.handCore[side].transform.Translate(this.handOffset[side], Space.Self);
		if (Our.mode != EditModes.Area || !(this.handDot[side].currentlyHeldObject != null))
		{
			if (Time.time >= this.timeWhenHandStretchResets[side])
			{
				this.handForwardStretchTarget[side] = Mathf.MoveTowards(this.handForwardStretchTarget[side], 0f, 0.35f * Time.deltaTime);
			}
			if (Time.time >= this.timeWhenHandOffsetResets)
			{
				this.handOffset[side] = Vector3.MoveTowards(this.handOffset[side], Vector3.zero, 0.35f * Time.deltaTime);
			}
		}
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x0008F0D0 File Offset: 0x0008D4D0
	private void HandleHandsFurtherBack()
	{
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			this.handsFurtherBack = !this.handsFurtherBack;
			this.handForwardStretchTarget[Side.Left] = 0f;
			this.handForwardStretchTarget[Side.Right] = 0f;
			Managers.soundManager.Play("whoosh", this.body.transform, 0.2f, false, false);
		}
		float num = ((!this.handsFurtherBack) ? 0f : (-0.2f));
		this.handsFurtherBackStretch = Mathf.MoveTowards(this.handsFurtherBackStretch, num, 2f * Time.deltaTime);
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x0008F174 File Offset: 0x0008D574
	private void ResetHeadPitchRoll()
	{
		if (this.headRotation.pitch != 0f || this.headRotation.roll != 0f)
		{
			float num = 100f * Time.deltaTime;
			this.headRotation.pitch = Mathf.MoveTowards(this.headRotation.pitch, 0f, num);
			this.headRotation.roll = Mathf.MoveTowards(this.headRotation.roll, 0f, num);
			this.headCore.transform.eulerAngles = this.headRotation.GetEulerAngles();
		}
	}

	// Token: 0x0600108E RID: 4238 RVA: 0x0008F214 File Offset: 0x0008D614
	private float GrowOrShrink(float value, float speed, bool doGrow, float min = 0f, float max = 1f)
	{
		if (!doGrow)
		{
			speed *= -1f;
		}
		value = Mathf.Clamp(value + Time.deltaTime * speed, min, max);
		return value;
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x0008F23C File Offset: 0x0008D63C
	private void InitTextInput()
	{
		this.textInputTexture = new Texture2D(1, 1);
		this.textInputTexture.SetPixel(0, 0, Color.black);
		this.textInputStyle = new GUIStyle();
		this.textInputStyle.font = this.font;
		this.textInputStyle.fontSize = 40;
		this.textInputStyle.alignment = TextAnchor.MiddleCenter;
		this.textInputStyle.normal.textColor = Color.white;
		this.textInputStyle.normal.background = this.textInputTexture;
		this.textInputStyle.wordWrap = true;
		this.textInputStyle.clipping = TextClipping.Clip;
		this.textInputStyle.padding = new RectOffset(10, 10, 0, 0);
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x0008F2F8 File Offset: 0x0008D6F8
	private void OnGUI()
	{
		if (this.showBottomHelp && Universe.features.ringMenu && Managers.broadcastNetworkManager != null && Managers.broadcastNetworkManager.inRoom)
		{
			GUI.DrawTexture(this.GetGuiBottomCenterRect(), this.bottomHelpTexture);
			if (!this.didShowBottomHelp)
			{
				this.didShowBottomHelp = true;
				base.Invoke("AutoHideBottomHelpIfNeeded", 5f);
			}
		}
		if (this.showTextInput)
		{
			if (Event.current.keyCode == KeyCode.Return)
			{
				Managers.personManager.DoAddTypedText(this.textEntered);
				this.showTextInput = false;
				this.textEntered = string.Empty;
				Managers.soundManager.Play("click", this.body.transform, 0.1f, false, false);
			}
			else if (Event.current.keyCode == KeyCode.Escape)
			{
				this.showTextInput = false;
				this.textEntered = string.Empty;
				Managers.soundManager.Play("whoosh", this.body.transform, 0.1f, false, false);
			}
		}
		if (this.showTextInput)
		{
			if (this.textInputStyle == null)
			{
				this.InitTextInput();
			}
			Rect guiCenterRect = this.GetGuiCenterRect((float)(Screen.width / 2), (float)(Screen.height / 6));
			if (!this.textEntered.Contains("://"))
			{
				this.textEntered = this.textEntered.ToUpper();
			}
			GUI.SetNextControlName("TextInput");
			this.textEntered = GUI.TextField(guiCenterRect, this.textEntered, 250, this.textInputStyle);
			GUI.FocusControl("TextInput");
		}
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x0008F4A5 File Offset: 0x0008D8A5
	private Rect GetRectWithOffset(Rect rect, int x, int y)
	{
		return new Rect(rect.x + (float)x, rect.y + (float)y, rect.width, rect.height);
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x0008F4D0 File Offset: 0x0008D8D0
	private Rect GetGuiCenterRect(float width, float height)
	{
		float num = (float)(Screen.width / 2) - width / 2f;
		float num2 = (float)(Screen.height / 2) - height / 2f;
		return new Rect(num, num2, width, height);
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x0008F508 File Offset: 0x0008D908
	private Rect GetGuiBottomCenterRect()
	{
		float num = (float)Screen.height / 6f;
		float num2 = num;
		float num3 = (float)(Screen.width / 2) - num2 / 2f;
		float num4 = (float)Screen.height - num;
		return new Rect(num3, num4, num2, num);
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x0008F548 File Offset: 0x0008D948
	private void ForwardKeysToKeyboardDialog()
	{
		KeyboardDialog component = this.hand[CrossDevice.desktopDialogSide].currentDialog.GetComponent<KeyboardDialog>();
		if (component == null)
		{
			return;
		}
		bool key = Input.GetKey(KeyCode.LeftShift);
		bool key2 = Input.GetKey(KeyCode.LeftControl);
		if (Input.GetKeyDown(KeyCode.Home))
		{
			component.MoveCaretToStart();
		}
		else if (Input.GetKeyDown(KeyCode.End))
		{
			component.MoveCaretToEnd();
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (component.allowNewlines && !key2)
			{
				component.AddNewLine();
			}
			else
			{
				component.HandleDone();
			}
		}
		this.HandleBackspaceAndDeleteKeys(component);
		this.HandleArrowsMovingKeyboardCaret(component, key2);
		if (!key2)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				component.AddCharacter(" ", true);
			}
			else if (Input.GetKeyDown(KeyCode.Comma))
			{
				component.AddCharacter((!key) ? "," : "<", true);
			}
			else if (Input.GetKeyDown(KeyCode.Period))
			{
				component.AddCharacter((!key) ? "." : ">", true);
			}
			else if (Input.GetKeyDown(KeyCode.Quote))
			{
				component.AddCharacter((!key) ? "'" : "\"", true);
			}
			else if (Input.GetKeyDown(KeyCode.Semicolon))
			{
				component.AddCharacter((!key) ? ";" : ":", true);
			}
			else if (Input.GetKeyDown(KeyCode.Slash))
			{
				component.AddCharacter((!key) ? "/" : "?", true);
			}
			else if (Input.GetKeyDown(KeyCode.LeftBracket))
			{
				component.AddCharacter((!key) ? "[" : "{", true);
			}
			else if (Input.GetKeyDown(KeyCode.RightBracket))
			{
				component.AddCharacter((!key) ? "]" : "}", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				component.AddCharacter((!key) ? "1" : "!", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				component.AddCharacter((!key) ? "2" : "@", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				component.AddCharacter((!key) ? "3" : "#", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				component.AddCharacter((!key) ? "4" : "$", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				component.AddCharacter((!key) ? "5" : "%", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				component.AddCharacter((!key) ? "6" : "^", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				component.AddCharacter((!key) ? "7" : "&", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				component.AddCharacter((!key) ? "8" : "*", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				component.AddCharacter((!key) ? "9" : "(", true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				component.AddCharacter((!key) ? "0" : ")", true);
			}
			else if (Input.GetKeyDown(KeyCode.Minus))
			{
				component.AddCharacter((!key) ? "-" : "_", true);
			}
			else if (Input.GetKeyDown(KeyCode.Equals))
			{
				component.AddCharacter((!key) ? "=" : "+", true);
			}
			else if (Input.GetKeyDown(KeyCode.BackQuote))
			{
				if (key)
				{
					component.AddCharacter("~", true);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Backslash))
			{
				if (key)
				{
					component.AddCharacter("|", true);
				}
			}
			else if (Input.GetKeyDown(KeyCode.A))
			{
				component.AddCharacter("a", true);
			}
			else if (Input.GetKeyDown(KeyCode.B))
			{
				component.AddCharacter("b", true);
			}
			else if (Input.GetKeyDown(KeyCode.C))
			{
				component.AddCharacter("c", true);
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				component.AddCharacter("d", true);
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				component.AddCharacter("e", true);
			}
			else if (Input.GetKeyDown(KeyCode.F))
			{
				component.AddCharacter("f", true);
			}
			else if (Input.GetKeyDown(KeyCode.G))
			{
				component.AddCharacter("g", true);
			}
			else if (Input.GetKeyDown(KeyCode.H))
			{
				component.AddCharacter("h", true);
			}
			else if (Input.GetKeyDown(KeyCode.I))
			{
				component.AddCharacter("i", true);
			}
			else if (Input.GetKeyDown(KeyCode.J))
			{
				component.AddCharacter("j", true);
			}
			else if (Input.GetKeyDown(KeyCode.K))
			{
				component.AddCharacter("k", true);
			}
			else if (Input.GetKeyDown(KeyCode.L))
			{
				component.AddCharacter("l", true);
			}
			else if (Input.GetKeyDown(KeyCode.M))
			{
				component.AddCharacter("m", true);
			}
			else if (Input.GetKeyDown(KeyCode.N))
			{
				component.AddCharacter("n", true);
			}
			else if (Input.GetKeyDown(KeyCode.O))
			{
				component.AddCharacter("o", true);
			}
			else if (Input.GetKeyDown(KeyCode.P))
			{
				component.AddCharacter("p", true);
			}
			else if (Input.GetKeyDown(KeyCode.Q))
			{
				component.AddCharacter("q", true);
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				component.AddCharacter("r", true);
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				component.AddCharacter("s", true);
			}
			else if (Input.GetKeyDown(KeyCode.T))
			{
				component.AddCharacter("t", true);
			}
			else if (Input.GetKeyDown(KeyCode.U))
			{
				component.AddCharacter("u", true);
			}
			else if (Input.GetKeyDown(KeyCode.V))
			{
				component.AddCharacter("v", true);
			}
			else if (Input.GetKeyDown(KeyCode.W))
			{
				component.AddCharacter("w", true);
			}
			else if (Input.GetKeyDown(KeyCode.X))
			{
				component.AddCharacter("x", true);
			}
			else if (Input.GetKeyDown(KeyCode.Y))
			{
				component.AddCharacter("y", true);
			}
			else if (Input.GetKeyDown(KeyCode.Z))
			{
				component.AddCharacter("z", true);
			}
		}
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x0008FCAC File Offset: 0x0008E0AC
	private void HandleBackspaceAndDeleteKeys(KeyboardDialog keyboard)
	{
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			keyboard.AddBackspace();
			this.keyDownRepeatTime = Time.time + 0.5f;
		}
		else if (Input.GetKey(KeyCode.Backspace) && Time.time >= this.keyDownRepeatTime)
		{
			keyboard.AddBackspace();
			this.keyDownRepeatTime = Time.time + 0.04f;
		}
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			keyboard.DeleteNextCharacter();
			this.keyDownRepeatTime = Time.time + 0.5f;
		}
		else if (Input.GetKey(KeyCode.Delete) && Time.time >= this.keyDownRepeatTime)
		{
			keyboard.DeleteNextCharacter();
			this.keyDownRepeatTime = Time.time + 0.04f;
		}
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0008FD70 File Offset: 0x0008E170
	private void HandleArrowsMovingKeyboardCaret(KeyboardDialog keyboard, bool ctrl)
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			keyboard.MoveCaretIfPossible((!ctrl) ? TextCaretDirection.LetterBack : TextCaretDirection.WordBack, null, null);
			this.keyDownRepeatTime = Time.time + 0.5f;
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && Time.time >= this.keyDownRepeatTime)
		{
			keyboard.MoveCaretIfPossible((!ctrl) ? TextCaretDirection.LetterBack : TextCaretDirection.WordBack, null, null);
			this.keyDownRepeatTime = Time.time + 0.04f;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			keyboard.MoveCaretIfPossible((!ctrl) ? TextCaretDirection.LetterForward : TextCaretDirection.WordForward, null, null);
			this.keyDownRepeatTime = Time.time + 0.5f;
		}
		else if (Input.GetKey(KeyCode.RightArrow) && Time.time >= this.keyDownRepeatTime)
		{
			keyboard.MoveCaretIfPossible((!ctrl) ? TextCaretDirection.LetterForward : TextCaretDirection.WordForward, null, null);
			this.keyDownRepeatTime = Time.time + 0.04f;
		}
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0008FE7C File Offset: 0x0008E27C
	private void HandleShortcuts()
	{
		bool flag = Misc.CtrlIsPressed();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (this.SetDesktopMode(true))
			{
				this.didEnterDesktopModeWithEscape = true;
				this.showBottomHelp = false;
				this.showDialogBackside = false;
				this.showTextInput = false;
				if (Our.GetCurrentNonStartDialog() != null)
				{
					this.CloseDialog();
				}
				else if (Our.mode == EditModes.Inventory)
				{
					Managers.inventoryManager.CloseDialog();
					Our.SetMode(EditModes.None, false);
				}
				else
				{
					this.timeWhenHeadRollResets = new float?(Time.time);
					this.DropHoldable(CrossDevice.desktopDialogSide);
					if (Universe.features.ringMenu)
					{
						StartDialog component = this.hand[CrossDevice.desktopDialogSide].currentDialog.GetComponent<StartDialog>();
						if (component != null)
						{
							component.OpenAppropriateDialog();
						}
						else
						{
							this.hand[CrossDevice.desktopDialogSide].SwitchToNewDialog(DialogType.Main, string.Empty);
						}
					}
				}
			}
			else
			{
				Managers.soundManager.Play("no", this.body.transform, 0.2f, false, false);
			}
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			this.CloseDialog();
			CrossDevice.desktopDialogSide = Misc.GetOppositeSide(CrossDevice.desktopDialogSide);
			Managers.soundManager.Play("whoosh", this.body.transform, 0.2f, false, false);
		}
		bool flag2 = flag && Input.GetKeyDown(KeyCode.V);
		DialogType currentNonStartDialogType = Managers.dialogManager.GetCurrentNonStartDialogType();
		bool flag3 = currentNonStartDialogType == DialogType.Main || currentNonStartDialogType == DialogType.Create || currentNonStartDialogType == DialogType.Material || currentNonStartDialogType == DialogType.CameraControl || currentNonStartDialogType == DialogType.VideoControl;
		if ((Input.GetKeyDown(KeyCode.T) || (flag2 && !flag3)) && !this.showTextInput && this.hand[CrossDevice.desktopDialogSide].currentDialogType != DialogType.Keyboard)
		{
			this.textEntered = string.Empty;
			this.showTextInput = true;
			Managers.soundManager.Play("whoosh", this.body.transform, 0.1f, false, false);
			if (flag2)
			{
				this.textEntered = GUIUtility.systemCopyBuffer;
			}
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			this.hand[CrossDevice.desktopDialogSide].SwitchToNewDialog(DialogType.BodyMotions, string.Empty);
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			if (this.hand[CrossDevice.desktopDialogSide].currentDialogType == DialogType.Equipment)
			{
				Managers.dialogManager.CloseDialog();
			}
			else
			{
				this.DropHoldable(Misc.GetOppositeSide(CrossDevice.desktopDialogSide));
				this.hand[CrossDevice.desktopDialogSide].SwitchToNewDialog(DialogType.Equipment, string.Empty);
			}
		}
		else if (Input.GetKeyDown(KeyCode.F4) && !Misc.AltIsPressed())
		{
			this.showDialogBackside = !this.showDialogBackside;
			if (this.showDialogBackside && Our.GetCurrentNonStartDialog() == null)
			{
				this.SetDesktopMode(true);
				if (Universe.features.ringMenu)
				{
					this.hand[CrossDevice.desktopDialogSide].SwitchToNewDialog(DialogType.Main, string.Empty);
				}
			}
		}
		if (flag && Input.GetKeyDown(KeyCode.F5))
		{
			Misc.RestartApp();
		}
		if (Input.GetKeyDown(KeyCode.F9))
		{
			this.showBottomHelp = false;
			if (CrossDevice.desktopMode)
			{
				if (XRDevice.isPresent)
				{
					this.SetDesktopMode(false);
				}
				else
				{
					Managers.browserManager.OpenGuideBrowser(null, null);
				}
			}
		}
		if (flag && Input.GetKeyDown(KeyCode.P))
		{
			Misc.OpenWindowsExplorerAtPath(Application.persistentDataPath);
		}
		if (flag && !CrossDevice.desktopMode && Managers.dialogManager != null && !Managers.dialogManager.KeyboardIsOpen())
		{
			this.HandleStaticHandModeIfNeeded();
		}
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x0009026C File Offset: 0x0008E66C
	public void ShowDialogFront()
	{
		this.showDialogBackside = false;
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x00090278 File Offset: 0x0008E678
	private void HandleStaticHandModeIfNeeded()
	{
		if (Input.GetKeyDown(KeyCode.Less) || Input.GetKeyDown(KeyCode.Comma))
		{
			this.hand[Side.Left].ToggleStaticMode();
		}
		else if (Input.GetKeyDown(KeyCode.Greater) || Input.GetKeyDown(KeyCode.Period))
		{
			this.hand[Side.Right].ToggleStaticMode();
		}
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x000902DC File Offset: 0x0008E6DC
	private void DropHoldable(Side side)
	{
		GameObject holdableInHand = this.handDot[side].holdableInHand;
		if (holdableInHand != null)
		{
			this.handDot[side].holdableInHand = null;
			this.handDot[side].currentlyHeldObject = null;
			Managers.personManager.DoThrowThing(holdableInHand);
		}
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x00090336 File Offset: 0x0008E736
	private bool AnyWASDKeyUp()
	{
		return Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D);
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x00090366 File Offset: 0x0008E766
	private bool AnyArrowKeyUp()
	{
		return Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x000903A2 File Offset: 0x0008E7A2
	private bool GetKeyOrKeyUp(KeyCode keyCode)
	{
		return Input.GetKey(keyCode) || Input.GetKeyUp(keyCode);
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x000903B8 File Offset: 0x0008E7B8
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.CompareTag("ThingPart") && hit.transform.parent != null)
		{
			Thing component = hit.transform.parent.GetComponent<Thing>();
			if (component != null && component.IsPlacement())
			{
				bool flag = hit.moveDirection.y <= -0.3f;
				if (flag)
				{
					if (component.isUnwalkable)
					{
						this.isOnUnwalkable = true;
					}
				}
				else if (component.isClimbable)
				{
					this.isTouchingClimbable = true;
				}
			}
		}
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x00090460 File Offset: 0x0008E860
	private void AutoHideBottomHelpIfNeeded()
	{
		if (!CrossDevice.desktopMode)
		{
			this.showBottomHelp = false;
		}
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x00090473 File Offset: 0x0008E873
	private void AutoHideBottomHelp()
	{
		this.showBottomHelp = false;
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x0009047C File Offset: 0x0008E87C
	public void ShowBottomHelpAgain()
	{
		this.didEnterDesktopModeWithEscape = false;
		this.showBottomHelp = true;
		base.CancelInvoke("AutoHideBottomHelp");
		base.CancelInvoke("AutoHideBottomHelpIfNeeded");
		base.Invoke("AutoHideBottomHelp", 4f);
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x000904B2 File Offset: 0x0008E8B2
	public void ResetToDefaultSpeeds()
	{
		this.fastMovementSpeed = 4.5f;
		this.jumpSpeed = 5f;
		this.slidiness = 0f;
		this.currentVelocity = Vector3.zero;
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x000904E0 File Offset: 0x0008E8E0
	public void SetMovementSmoothing(MovementSmoothing movementSmoothing)
	{
		this.movementSmoothing = movementSmoothing;
		MovementSmoothing movementSmoothing2 = this.movementSmoothing;
		if (movementSmoothing2 != MovementSmoothing.None)
		{
			if (movementSmoothing2 != MovementSmoothing.Medium)
			{
				if (movementSmoothing2 == MovementSmoothing.Strong)
				{
					this.movementSmoothingFactor = 0.025f;
					this.zoomSmoothingFactor = 0.025f;
				}
			}
			else
			{
				this.movementSmoothingFactor = 0.125f;
				this.zoomSmoothingFactor = 0.25f;
			}
		}
		else
		{
			this.movementSmoothingFactor = 1f;
			this.zoomSmoothingFactor = 0.75f;
		}
		this.rotationSmoothingFactor = this.movementSmoothingFactor;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x00090574 File Offset: 0x0008E974
	private bool UseSmoothMovement()
	{
		bool flag;
		if (this.movementSmoothing != MovementSmoothing.None && this.person.ridingBeacon == null)
		{
			Vector3? vector = this.autoMoveTarget;
			flag = vector == null;
		}
		else
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x04001052 RID: 4178
	public const float defaultMovementSpeed = 2f;

	// Token: 0x04001053 RID: 4179
	public const float fastMovementSpeedDefault = 4.5f;

	// Token: 0x04001054 RID: 4180
	public float fastMovementSpeed = 4.5f;

	// Token: 0x04001055 RID: 4181
	public const float jumpSpeedDefault = 5f;

	// Token: 0x04001056 RID: 4182
	public float jumpSpeed = 5f;

	// Token: 0x04001057 RID: 4183
	public float slidiness;

	// Token: 0x04001058 RID: 4184
	public Vector3 currentVelocity = Vector3.zero;

	// Token: 0x04001059 RID: 4185
	private const float mouseSpeedHorizontal = 2.5f;

	// Token: 0x0400105A RID: 4186
	private const float mouseSpeedVertical = 2.5f;

	// Token: 0x0400105B RID: 4187
	public Font font;

	// Token: 0x0400105C RID: 4188
	public Texture2D bottomHelpTexture;

	// Token: 0x0400105D RID: 4189
	public GameObject sun;

	// Token: 0x0400105E RID: 4190
	public CharacterController body;

	// Token: 0x0400105F RID: 4191
	private string textEntered = string.Empty;

	// Token: 0x04001060 RID: 4192
	private bool showTextInput;

	// Token: 0x04001061 RID: 4193
	private GUIStyle textInputStyle;

	// Token: 0x04001062 RID: 4194
	private Texture2D textInputTexture;

	// Token: 0x04001063 RID: 4195
	private Person person;

	// Token: 0x04001064 RID: 4196
	private GameObject rig;

	// Token: 0x04001065 RID: 4197
	private GameObject headCore;

	// Token: 0x04001066 RID: 4198
	private Dictionary<Side, GameObject> handCore = new Dictionary<Side, GameObject>();

	// Token: 0x04001067 RID: 4199
	private Dictionary<Side, Hand> hand = new Dictionary<Side, Hand>();

	// Token: 0x04001068 RID: 4200
	private Dictionary<Side, HandDot> handDot = new Dictionary<Side, HandDot>();

	// Token: 0x04001069 RID: 4201
	private bool isTouchingClimbable;

	// Token: 0x0400106A RID: 4202
	private bool isOnUnwalkable;

	// Token: 0x0400106B RID: 4203
	private Vector3 lastWalkablePosition = Vector3.zero;

	// Token: 0x0400106C RID: 4204
	private Camera camera;

	// Token: 0x0400106D RID: 4205
	private bool showBottomHelp = true;

	// Token: 0x0400106E RID: 4206
	private bool didEnterDesktopModeWithEscape;

	// Token: 0x0400106F RID: 4207
	private const float handForwardStretchResetSeconds = 10f;

	// Token: 0x04001070 RID: 4208
	private Dictionary<Side, float> handForwardStretch = new Dictionary<Side, float>
	{
		{
			Side.Left,
			0f
		},
		{
			Side.Right,
			0f
		}
	};

	// Token: 0x04001071 RID: 4209
	private Dictionary<Side, float> handForwardStretchTarget = new Dictionary<Side, float>
	{
		{
			Side.Left,
			0f
		},
		{
			Side.Right,
			0f
		}
	};

	// Token: 0x04001072 RID: 4210
	private Dictionary<Side, float> timeWhenHandStretchResets = new Dictionary<Side, float>
	{
		{
			Side.Left,
			0f
		},
		{
			Side.Right,
			0f
		}
	};

	// Token: 0x04001073 RID: 4211
	private Vector3? autoMoveTarget;

	// Token: 0x04001074 RID: 4212
	private const float handOffsetResetSeconds = 10f;

	// Token: 0x04001075 RID: 4213
	private Dictionary<Side, Vector3> handOffset = new Dictionary<Side, Vector3>();

	// Token: 0x04001076 RID: 4214
	private float timeWhenHandOffsetResets;

	// Token: 0x04001077 RID: 4215
	private PitchRollYaw headRotation = new PitchRollYaw(0f, 0f, 0f);

	// Token: 0x04001078 RID: 4216
	private float? timeWhenHeadRollResets = new float?(0f);

	// Token: 0x04001079 RID: 4217
	private readonly Vector3 headPositionDefault = Vector3.up * 1.75f;

	// Token: 0x0400107A RID: 4218
	private float bodyOffsetY;

	// Token: 0x0400107C RID: 4220
	private bool wasGrounded;

	// Token: 0x0400107D RID: 4221
	private Vector3 lastFinalizedPosition = Vector3.zero;

	// Token: 0x0400107E RID: 4222
	private float handForwardLock;

	// Token: 0x0400107F RID: 4223
	private float verticalVelocity;

	// Token: 0x04001080 RID: 4224
	private float keyDownRepeatTime = -1f;

	// Token: 0x04001081 RID: 4225
	private const float keyDownRepeatDelay = 0.5f;

	// Token: 0x04001082 RID: 4226
	private const float keyDownRepeatFrequency = 0.04f;

	// Token: 0x04001083 RID: 4227
	private bool didShowBottomHelp;

	// Token: 0x04001084 RID: 4228
	private bool handsFurtherBack;

	// Token: 0x04001085 RID: 4229
	private float handsFurtherBackStretch;

	// Token: 0x04001087 RID: 4231
	private float movementSmoothingFactor;

	// Token: 0x04001088 RID: 4232
	private float rotationSmoothingFactor;

	// Token: 0x04001089 RID: 4233
	private float zoomSmoothingFactor;

	// Token: 0x0400108A RID: 4234
	private Vector3 previousVelocity = Vector3.zero;

	// Token: 0x0400108B RID: 4235
	private const float desktopFieldOfViewDefault = 60f;

	// Token: 0x0400108C RID: 4236
	private float fieldOfView = 60f;
}
