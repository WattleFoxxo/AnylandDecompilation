using System;
using System.Linq;
using UnityEngine;
using Valve.VR;

// Token: 0x020000E2 RID: 226
public class Hand : MonoBehaviour
{
	// Token: 0x17000134 RID: 308
	// (get) Token: 0x0600070F RID: 1807 RVA: 0x00021254 File Offset: 0x0001F654
	public SteamVR_Controller.Device controller
	{
		get
		{
			SteamVR_Controller.Device device = null;
			if (this.trackedObj != null)
			{
				int index = (int)this.trackedObj.index;
				if (index != -1)
				{
					device = SteamVR_Controller.Input(index);
				}
			}
			return device;
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000710 RID: 1808 RVA: 0x0002128F File Offset: 0x0001F68F
	// (set) Token: 0x06000711 RID: 1809 RVA: 0x00021297 File Offset: 0x0001F697
	public Transform eyes { get; private set; }

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000712 RID: 1810 RVA: 0x000212A0 File Offset: 0x0001F6A0
	// (set) Token: 0x06000713 RID: 1811 RVA: 0x000212A8 File Offset: 0x0001F6A8
	public Side side { get; private set; }

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000714 RID: 1812 RVA: 0x000212B1 File Offset: 0x0001F6B1
	// (set) Token: 0x06000715 RID: 1813 RVA: 0x000212B9 File Offset: 0x0001F6B9
	public bool isLegPuppeteering { get; private set; }

	// Token: 0x06000716 RID: 1814 RVA: 0x000212C4 File Offset: 0x0001F6C4
	private void Start()
	{
		this.trackedObj = base.GetComponent<SteamVR_TrackedObject>();
		this.side = ((!(base.gameObject.name == "HandCoreLeft")) ? Side.Right : Side.Left);
		this.sideLowerCase = this.side.ToString().ToLower();
		this.handDotScript = this.handDot.GetComponent<HandDot>();
		this.eyes = global::UnityEngine.Object.FindObjectOfType<SteamVR_Camera>().GetComponent<Transform>();
		this.personRig = base.transform.parent;
		GameObject @object = Managers.treeManager.GetObject("/Universe");
		this.headCore = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		this.slidyMovementDirectionCore = Managers.treeManager.GetTransform("/OurPersonRig/Torso");
		this.targetSphere = global::UnityEngine.Object.Instantiate<GameObject>(this.targetSpherePrefab);
		this.targetSphere.transform.parent = @object.transform;
		this.contextInfoTargetSphere = global::UnityEngine.Object.Instantiate<GameObject>(this.targetSpherePrefab);
		this.contextInfoTargetSphere.transform.parent = @object.transform;
		Renderer component = this.contextInfoTargetSphere.GetComponent<Renderer>();
		component.material.color = new Color(0f, 0.5f, 1f);
		this.SwitchToNewDialog(DialogType.Start, string.Empty);
		this.eyeCollisionSphereScript = this.eyeCollisionSphere.GetComponent<EyeCollisionSphere>();
		this.otherHandScript = this.otherHand.GetComponent<Hand>();
		this.handDotNormalPosition = this.handDot.transform.localPosition;
		this.handDotNormalScale = this.handDot.transform.localScale;
		this.checkForMovedHandsAchievement = true;
		GameObject object2 = Managers.treeManager.GetObject("/Universe/FollowerCamera");
		this.followerCamera = object2.GetComponent<FollowerCamera>();
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x00021487 File Offset: 0x0001F887
	public GameObject SwitchToNewDialog(DialogType dialogType, string tabName = "")
	{
		return Managers.dialogManager.SwitchToNewDialog(dialogType, this, tabName);
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x00021496 File Offset: 0x0001F896
	private bool EverythingIsReady()
	{
		return Managers.broadcastNetworkManager != null && Managers.broadcastNetworkManager.inRoom;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x000214B8 File Offset: 0x0001F8B8
	private void Update()
	{
		if (!this.EverythingIsReady())
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		if (this.controller != null)
		{
			num = this.controller.GetAxis(EVRButtonId.k_EButton_Axis0)[0];
			num2 = this.controller.GetAxis(EVRButtonId.k_EButton_Axis0)[1];
		}
		this.HandleLegPuppeteerMovement(num, num2);
		this.HandleTeleportAndTurnAround(num, num2);
		this.HandleContextInfoLaser();
		this.HandleInfrequentEventChecks();
		this.HandleShrinkingHapticPulseOverTime();
		this.HandleSmoothMovement();
		this.HandleMovedHandsAchievement();
		this.NormalizeLaserVolumes();
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0002154C File Offset: 0x0001F94C
	private void HandleTeleportAndTurnAround(float steerX, float steerY)
	{
		bool flag = false;
		if (!Universe.features.teleportLaser || Universe.demoMode)
		{
			return;
		}
		if (CrossDevice.desktopMode)
		{
			flag = this.HandleTeleportLaser(steerX, steerY);
		}
		else if (CrossDevice.hasStick)
		{
			flag = this.HandleTeleportLaser(steerX, steerY);
			if (steerX <= -0.8f)
			{
				if (!this.thisThumbstickEdgeTurnHandled)
				{
					this.HandleTurnAround(-1, 45f);
					this.thisThumbstickEdgeTurnHandled = true;
				}
			}
			else if (steerX >= 0.8f)
			{
				if (!this.thisThumbstickEdgeTurnHandled)
				{
					this.HandleTurnAround(1, 45f);
					this.thisThumbstickEdgeTurnHandled = true;
				}
			}
			else
			{
				this.thisThumbstickEdgeTurnHandled = false;
			}
		}
		else if (steerX != 0f)
		{
			if (steerX <= -0.6f)
			{
				if (this.GetPressUp(CrossDevice.button_teleport))
				{
					this.HandleTurnAround(-1, 45f);
				}
			}
			else if (steerX >= 0.6f)
			{
				if (this.GetPressUp(CrossDevice.button_teleport))
				{
					this.HandleTurnAround(1, 45f);
				}
			}
			else
			{
				flag = this.HandleTeleportLaser(steerX, steerY);
			}
		}
		if (!flag)
		{
			this.timeLaserPressStarted = -1f;
		}
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0002168C File Offset: 0x0001FA8C
	private void HandleLegPuppeteerMovement(float steerX, float steerY)
	{
		if (Our.lockLegMovement && Our.mode != EditModes.Body)
		{
			return;
		}
		bool flag;
		if (CrossDevice.hasStick)
		{
			flag = steerY <= -0.7f && Mathf.Abs(steerX) <= 0.5f && (!(Managers.browserManager != null) || !Managers.browserManager.CursorIsInBrowser());
		}
		else
		{
			flag = steerY <= -0.75f && Mathf.Abs(steerX) <= 0.6f && this.GetPress(CrossDevice.button_legPuppeteering);
		}
		if (flag && !this.isLegPuppeteering && this.GetLegThing() == null && Our.mode != EditModes.Body)
		{
			flag = false;
		}
		if (flag)
		{
			if (this.isLegPuppeteering)
			{
				Vector3 vector = base.transform.position - this.previousPosition;
				this.leg.position += vector;
				Vector3 position = this.leg.position;
				Transform parent = this.leg.parent;
				Vector3 eulerAngles = base.transform.eulerAngles;
				base.transform.rotation = this.previousRotation;
				this.leg.localPosition = Vector3.zero;
				this.leg.parent = base.transform;
				base.transform.eulerAngles = eulerAngles;
				this.leg.parent = parent;
				this.leg.position = position;
			}
			else
			{
				this.StartLegPuppeteering();
			}
			this.previousPosition = base.transform.position;
			this.previousRotation = base.transform.rotation;
		}
		else if (this.isLegPuppeteering)
		{
			this.EndLegPuppeteering();
		}
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00021856 File Offset: 0x0001FC56
	public Thing GetLegThing()
	{
		return this.leg.GetComponentInChildren<Thing>();
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00021864 File Offset: 0x0001FC64
	private void StartLegPuppeteering()
	{
		this.isLegPuppeteering = true;
		global::UnityEngine.Object.Destroy(this.handObjectsWhilePuppeteering);
		string text = "HandObjectsWhilePuppeteering" + this.side.ToString();
		this.handObjectsWhilePuppeteering = new GameObject(text);
		this.handObjectsWhilePuppeteering.transform.parent = base.transform.parent;
		this.handObjectsWhilePuppeteering.transform.position = base.transform.position;
		this.handObjectsWhilePuppeteering.transform.rotation = base.transform.rotation;
		this.handAttachmentPoint.transform.parent = this.handObjectsWhilePuppeteering.transform;
		this.armAttachmentPoint.transform.parent = this.handObjectsWhilePuppeteering.transform;
		this.handDot.transform.parent = this.handObjectsWhilePuppeteering.transform;
		if (this.currentDialog != null)
		{
			this.currentDialog.transform.parent = this.handObjectsWhilePuppeteering.transform;
		}
		if (this.handDotScript.currentlyHeldObject != null)
		{
			this.handDotScript.currentlyHeldObject.transform.parent = this.handObjectsWhilePuppeteering.transform;
		}
		Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "leg move started " + this.sideLowerCase, true);
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x000219D7 File Offset: 0x0001FDD7
	public void EndAllOngoingLegPuppeteering()
	{
		if (this.isLegPuppeteering)
		{
			this.EndLegPuppeteering();
		}
		if (this.otherHandScript.isLegPuppeteering)
		{
			this.otherHandScript.EndLegPuppeteering();
		}
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00021A08 File Offset: 0x0001FE08
	public void EndLegPuppeteering()
	{
		this.isLegPuppeteering = false;
		this.handObjectsWhilePuppeteering.transform.position = base.transform.position;
		this.handObjectsWhilePuppeteering.transform.rotation = base.transform.rotation;
		this.handAttachmentPoint.transform.parent = base.transform;
		this.armAttachmentPoint.transform.parent = base.transform;
		this.handDot.transform.parent = base.transform;
		if (this.currentDialog != null)
		{
			if (this.currentDialogType == DialogType.Start)
			{
				this.currentDialog.transform.parent = this.skeleton.GetRingBone();
			}
			else
			{
				this.currentDialog.transform.parent = base.transform;
			}
		}
		if (this.handDotScript.currentlyHeldObject != null)
		{
			this.handDotScript.currentlyHeldObject.transform.parent = base.transform;
		}
		global::UnityEngine.Object.Destroy(this.handObjectsWhilePuppeteering);
		this.handObjectsWhilePuppeteering = null;
		this.previousPosition = Vector3.zero;
		this.previousRotation = Quaternion.identity;
		if (Managers.personManager != null)
		{
			Managers.personManager.SaveOurLegAttachmentPointPositions();
		}
		Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "leg move ended " + this.sideLowerCase, true);
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00021B80 File Offset: 0x0001FF80
	private void HandleShrinkingHapticPulseOverTime()
	{
		if (this.shrinkingHapticPulseUntilTime != -1f)
		{
			float num = this.shrinkingHapticPulseUntilTime - Time.time;
			if (num > 0f)
			{
				float num2 = 1f - num / 0.2f;
				ushort num3 = (ushort)Mathf.Round(Mathf.Lerp(3000f, 1f, num2));
				this.TriggerHapticPulse(num3);
			}
			else
			{
				this.shrinkingHapticPulseUntilTime = -1f;
			}
		}
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00021BF1 File Offset: 0x0001FFF1
	public void StartShrinkingHapticPulseOverTime()
	{
		this.shrinkingHapticPulseUntilTime = Time.time + 0.2f;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00021C04 File Offset: 0x00020004
	public void HandleTurnAround(int rotationDirection, float degrees = 45f)
	{
		if (!this.eyeCollisionSphereScript.isColliding)
		{
			Vector3 position = this.eyes.position;
			this.personRig.Rotate(0f, degrees * (float)rotationDirection, 0f);
			this.RemoveLaserLine();
			this.targetSphere.SetActive(false);
			this.hasLastTeleportHitPoint = false;
			this.smoothMovementTeleportTarget = null;
			this.otherHandScript.smoothMovementTeleportTarget = null;
			Vector3 position2 = this.personRig.position;
			position2.x += position.x - this.eyes.position.x;
			position2.z += position.z - this.eyes.position.z;
			this.personRig.position = position2;
			Managers.personManager.CachePhotonRigLocation();
			this.ShuffleParticlesForBetterTeleportEffect();
			Transform transform = Managers.treeManager.GetTransform("/OurPersonRig/StaticEnvironmentParticleSystems");
			transform.eulerAngles = Vector3.zero;
			Managers.personManager.DoUpdateRidingBeaconOffset();
			this.UpdatePossibleCameraCalibrator();
		}
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00021D2A File Offset: 0x0002012A
	public bool CanWeMoveForward(float maxDistance = 0.25f)
	{
		return this.WeHaveGroundToWalkOn(this.slidyMovementDirectionCore) && this.DirectionIsFree(this.slidyMovementDirectionCore, this.slidyMovementDirectionCore.forward, maxDistance);
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00021D58 File Offset: 0x00020158
	public bool CanWeMoveBackward(float maxDistance = 0.25f)
	{
		return this.WeHaveGroundToWalkOn(this.slidyMovementDirectionCore) && this.DirectionIsFree(this.slidyMovementDirectionCore, -this.slidyMovementDirectionCore.forward, maxDistance);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00021D8B File Offset: 0x0002018B
	public void MoveUsForward()
	{
		this.personRig.Translate(this.slidyMovementDirectionCore.forward * (Time.deltaTime * 2f), Space.World);
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00021DB4 File Offset: 0x000201B4
	public void MoveUsBackward()
	{
		this.personRig.Translate(-(this.slidyMovementDirectionCore.forward * (Time.deltaTime * 2f)), Space.World);
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00021DE2 File Offset: 0x000201E2
	public void MoveUsUpward()
	{
		this.personRig.Translate(this.slidyMovementDirectionCore.up * (Time.deltaTime * 2f), Space.World);
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00021E0B File Offset: 0x0002020B
	public void MoveUsDownward()
	{
		this.personRig.Translate(-(this.slidyMovementDirectionCore.up * (Time.deltaTime * 2f)), Space.World);
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00021E3C File Offset: 0x0002023C
	private bool WeHaveGroundToWalkOn(Transform center)
	{
		bool flag = false;
		Ray ray = new Ray(center.position, -center.up);
		float num = 2f;
		foreach (RaycastHit raycastHit in (from h in Physics.RaycastAll(ray, num)
			orderby h.distance
			select h).ToArray<RaycastHit>())
		{
			Transform transform = raycastHit.collider.transform;
			if (transform != null && transform.parent != null)
			{
				Thing component = transform.parent.GetComponent<Thing>();
				ThingPart component2 = transform.GetComponent<ThingPart>();
				if (component != null && component2 != null)
				{
					bool flag2 = raycastHit.normal.y >= 0.5f;
					bool flag3 = !string.IsNullOrEmpty(component.placementId);
					if (!component.isPassable && !component2.isLiquid)
					{
						if (flag3 && flag2 && !component.isUnwalkable)
						{
							flag = true;
							break;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00021F8C File Offset: 0x0002038C
	private bool DirectionIsFree(Transform center, Vector3 direction, float maxDistance)
	{
		bool flag = true;
		Ray ray = new Ray(center.position, direction);
		foreach (RaycastHit raycastHit in (from h in Physics.RaycastAll(ray, maxDistance)
			orderby h.distance
			select h).ToArray<RaycastHit>())
		{
			Transform transform = raycastHit.collider.transform;
			if (transform != null && transform.parent != null)
			{
				Thing component = transform.parent.GetComponent<Thing>();
				ThingPart component2 = transform.GetComponent<ThingPart>();
				if (component != null && component2 != null)
				{
					bool flag2 = !string.IsNullOrEmpty(component.placementId);
					if (!component.isPassable && !component2.isLiquid)
					{
						if (flag2)
						{
							flag = false;
							break;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00022098 File Offset: 0x00020498
	public void TurnToAngle(float degrees = 45f)
	{
		Vector3 position = this.eyes.position;
		this.personRig.eulerAngles = new Vector3(0f, degrees, 0f);
		Vector3 position2 = this.personRig.position;
		position2.x += position.x - this.eyes.position.x;
		position2.z += position.z - this.eyes.position.z;
		this.personRig.position = position2;
		Managers.personManager.CachePhotonRigLocation();
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00022140 File Offset: 0x00020540
	private void HandleInfrequentEventChecks()
	{
		if (this.timeOfLastInfrequentEventsCheck == -1f || this.timeOfLastInfrequentEventsCheck + this.infrequentEventsCheckDelay <= Time.time)
		{
			this.timeOfLastInfrequentEventsCheck = Time.time;
			this.CheckForPointedAtEvent();
			this.ReDetectDeviceIfNeeded();
		}
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00022180 File Offset: 0x00020580
	private void ReDetectDeviceIfNeeded()
	{
		if (this.side == Side.Left && !this.authorityControllerFound && this.controller != null && !CrossDevice.joystickWasFound)
		{
			this.authorityControllerFound = true;
			CrossDevice.Init();
		}
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x000221BC File Offset: 0x000205BC
	private void CheckForPointedAtEvent()
	{
		Ray ray = new Ray(base.transform.position - base.transform.forward * 0.04f, base.transform.forward);
		float num = 10f;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, num))
		{
			GameObject gameObject = raycastHit.collider.gameObject;
			if (gameObject.tag == "ThingPart")
			{
				ThingPart component = gameObject.GetComponent<ThingPart>();
				if (component != null)
				{
					component.TriggerEventAsStateAuthority(StateListener.EventType.OnPointedAt, string.Empty);
				}
			}
		}
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002225A File Offset: 0x0002065A
	private float GetLaserWidth()
	{
		return Mathf.Abs(Mathf.Sin(Time.time * 25f)) * 0.005f + 0.005f;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00022280 File Offset: 0x00020680
	private float GetLaserDistanceMultiplier(float ourScale = -1f, bool isForAirLaser = false)
	{
		float num = 1f;
		if (ourScale == -1f)
		{
			ourScale = Managers.personManager.GetOurScale();
		}
		if (ourScale < 1f || (ourScale > 1f && Managers.areaManager.weAreEditorOfCurrentArea))
		{
			num = ourScale;
			if (isForAirLaser)
			{
				num = Mathf.Clamp(num, 0f, 2f);
			}
		}
		return num;
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x000222EC File Offset: 0x000206EC
	private bool HandleTeleportLaser(float steerX, float steerY)
	{
		bool flag = false;
		if (!this.isLegPuppeteering && !this.otherHandScript.isLegPuppeteering && (this.GetPress(CrossDevice.button_teleport) || this.ThumbStickTeleportLaserIsHeld(steerX, steerY) || (CrossDevice.desktopMode && this.side != CrossDevice.desktopDialogSide && Input.GetKey(KeyCode.E) && Managers.dialogManager != null && !Managers.dialogManager.KeyboardIsOpen())))
		{
			flag = true;
			this.lastTeleportHitWasOfAutoTiltedLaser = false;
			if (!this.HandleTeleportLaserPressed(base.transform.forward, false) && Our.teleportLaserAutoTargetsGround)
			{
				this.lastTeleportHitWasOfAutoTiltedLaser = this.HandleTeleportLaserPressedAutoTilted();
			}
		}
		else
		{
			this.HandleTeleportLaserNotPressed();
		}
		return flag;
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x000223BC File Offset: 0x000207BC
	private bool HandleTeleportLaserPressedAutoTilted()
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		Vector3 localEulerAngles2 = base.transform.localEulerAngles;
		localEulerAngles2.x = 0f;
		base.transform.localEulerAngles = localEulerAngles2;
		Vector3 vector = base.transform.forward;
		Vector3 right = base.transform.right;
		right.y = 0f;
		vector = Quaternion.AngleAxis(45f, right) * vector;
		bool flag = this.HandleTeleportLaserPressed(vector, true);
		base.transform.localEulerAngles = localEulerAngles;
		return flag;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0002244C File Offset: 0x0002084C
	private bool HandleTeleportLaserPressed(Vector3 laserDirection, bool onlySetLineDirectionIfHitPointFound = false)
	{
		bool flag = false;
		if (this.eyeCollisionSphereScript.isColliding)
		{
			return flag;
		}
		if (this.timeLaserPressStarted == -1f)
		{
			this.timeLaserPressStarted = Time.time;
		}
		Vector3 laserOrigin = this.GetLaserOrigin();
		float num = this.GetLaserWidth();
		float ourScale = Managers.personManager.GetOurScale();
		float num2 = 7.5f * this.GetLaserDistanceMultiplier(ourScale, false);
		num *= ourScale;
		this.AddLaserLineIfNeeded();
		this.laserLine.SetWidth(num, num);
		this.laserLine.SetPosition(0, laserOrigin);
		Ray ray = new Ray(laserOrigin, laserDirection);
		RaycastHit[] array = (from h in Physics.RaycastAll(ray, num2)
			orderby h.distance
			select h).ToArray<RaycastHit>();
		this.lastContextInfoHit = null;
		this.associatedContextDialogType = DialogType.None;
		bool flag2 = false;
		Vector3 vector = Vector3.zero;
		this.isTeleportingIntoLiquid = false;
		this.isTeleportingOntoSittable = false;
		foreach (RaycastHit raycastHit in array)
		{
			bool flag3 = raycastHit.normal.y >= 0.5f;
			bool flag4 = raycastHit.normal.y <= -0.5f;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = true;
			bool flag9 = false;
			bool flag10 = (Our.mode == EditModes.Area || Our.mode == EditModes.Thing) && Managers.areaManager.weAreEditorOfCurrentArea;
			bool flag11 = flag10;
			GameObject gameObject = raycastHit.collider.gameObject;
			if (!(gameObject.transform.parent == null))
			{
				ThingPart thingPart = null;
				bool flag12 = false;
				bool flag13 = false;
				bool flag14 = false;
				bool flag15 = false;
				string tag = gameObject.tag;
				if (tag == "ThingPart")
				{
					thingPart = gameObject.GetComponent<ThingPart>();
					flag14 = thingPart != null && thingPart.isLiquid;
					Thing component = gameObject.transform.parent.GetComponent<Thing>();
					if (component != null)
					{
						flag5 = component.isClimbable;
						flag6 = component.isUnwalkable;
						flag7 = component.isPassable;
						flag8 = component.IsPlacement();
						flag9 = component.gameObject == CreationHelper.thingBeingEdited;
						this.isTeleportingOntoSittable = component.isSittable;
						Thing myRootThing = component.GetMyRootThing();
						if (myRootThing.transform.parent != null)
						{
							flag12 = myRootThing.transform.CompareTag("Attachment");
							flag13 = myRootThing.transform.CompareTag("CurrentlyHeldLeft") || myRootThing.transform.CompareTag("CurrentlyHeldRight");
							flag15 = myRootThing.movableByEveryone && myRootThing.transform.parent != Managers.thingManager.placements.transform;
						}
					}
				}
				else if (tag == "SlowBuildCreationCollider")
				{
					flag6 = true;
				}
				if (!flag12)
				{
					flag12 = gameObject.transform.parent != null && gameObject.transform.parent.gameObject.tag == "Attachment";
				}
				if (!flag13)
				{
					flag13 = gameObject.transform.parent != null && (gameObject.transform.parent.gameObject.CompareTag("CurrentlyHeldLeft") || gameObject.transform.parent.gameObject.CompareTag("CurrentlyHeldRight"));
				}
				if ((tag == "ThingPart" || tag == "SlowBuildCreationCollider") && (flag8 || (flag9 && Managers.areaManager.weAreEditorOfCurrentArea) || Managers.areaManager.rights.emittedClimbing == true) && !flag7 && !flag12 && !flag13 && !flag15)
				{
					flag2 = true;
					vector = raycastHit.point;
					bool flag16 = flag3 || flag11 || (flag5 && !flag4) || Managers.areaManager.isZeroGravity || Our.WeCanFly();
					if (flag9 && !Managers.areaManager.weAreEditorOfCurrentArea)
					{
						flag16 = false;
					}
					if (flag16 && (!flag6 || flag10))
					{
						flag = true;
						this.lastTeleportHitPoint = raycastHit.point;
						this.lastTeleportHitObject = gameObject;
						this.laserLine.SetPosition(1, raycastHit.point);
						this.hasLastTeleportHitPoint = true;
						this.laserLine.SetColors(this.laserLineStrongColor, this.laserLineStrongColor);
						this.ActivateTargetSphere();
						this.targetSphere.transform.position = new Vector3(this.lastTeleportHitPoint.x, this.lastTeleportHitPoint.y, this.lastTeleportHitPoint.z);
						if (flag3 && flag14)
						{
							this.lastTeleportHitPoint += Vector3.up * -(this.eyes.localPosition.y * 0.85f);
							this.isTeleportingIntoLiquid = true;
							if (thingPart.states[thingPart.currentState] != null)
							{
								this.teleportingIntoLiquidOfColor = thingPart.states[thingPart.currentState].color;
							}
							else
							{
								this.teleportingIntoLiquidOfColor = Color.white;
							}
						}
						else if (flag4)
						{
							Vector3 vector2 = raycastHit.normal * (this.eyes.localPosition.y * 1.25f);
							this.lastTeleportHitPoint += vector2;
						}
						else if (flag16 && !flag3)
						{
							Vector3 vector3 = raycastHit.normal * 0.5f;
							this.lastTeleportHitPoint += vector3;
							this.lastTeleportHitPoint.y = this.lastTeleportHitPoint.y - this.eyes.localPosition.y;
							if (this.lastTeleportHitPoint.y >= this.personRig.localPosition.y - 2f && this.lastTeleportHitPoint.y < this.personRig.localPosition.y)
							{
								this.lastTeleportHitPoint.y = this.personRig.localPosition.y;
							}
						}
						this.TriggerHapticPulse(600);
						break;
					}
					if (!flag10)
					{
						break;
					}
				}
			}
		}
		this.isFreeFloatingZeroGravityHitPoint = false;
		if (!flag && (Managers.areaManager.isZeroGravity || Our.WeCanFly()))
		{
			flag = (this.hasLastTeleportHitPoint = (flag2 = true));
			this.lastTeleportHitObject = null;
			this.isFreeFloatingZeroGravityHitPoint = true;
			this.ActivateTargetSphere();
			this.lastTeleportHitPoint = laserOrigin + base.transform.forward * this.zeroGravityLaserLength * this.GetLaserDistanceMultiplier(-1f, true);
			this.AdjustZeroGravityLaserLength();
			vector = this.lastTeleportHitPoint;
			this.targetSphere.transform.position = this.lastTeleportHitPoint;
			this.laserLine.SetPosition(1, this.lastTeleportHitPoint);
			this.laserLine.SetColors(this.laserLineStrongColor, this.laserLineStrongColor);
			this.TriggerHapticPulse(200);
		}
		if (!flag)
		{
			if (flag2)
			{
				this.targetSphere.transform.position = new Vector3(vector.x, vector.y, vector.z);
				this.laserLine.SetPosition(1, vector);
			}
			else if (!onlySetLineDirectionIfHitPointFound)
			{
				this.laserLine.SetPosition(1, laserOrigin + base.transform.forward * num2);
			}
			this.hasLastTeleportHitPoint = false;
			this.lastTeleportHitObject = null;
			this.laserLine.SetColors(this.laserLineWeakColor, this.laserLineWeakColor);
			this.targetSphere.SetActive(false);
			this.TriggerHapticPulse(300);
		}
		if (!this.touchpadWasPressed && !this.WeAreInvisibleInRecording())
		{
			Managers.soundManager.Play("teleportLaser", base.transform, this.teleportLaserVolume, false, false);
			this.teleportLaserVolume = this.ShrinkLaserVolume(this.teleportLaserVolume);
		}
		this.touchpadWasPressed = true;
		return flag;
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00022CFC File Offset: 0x000210FC
	private void HandleTeleportLaserNotPressed()
	{
		this.RemoveLaserLine();
		this.targetSphere.SetActive(false);
		if (this.hasLastTeleportHitPoint)
		{
			if (Our.mode == EditModes.Area || (!this.eyeCollisionSphereScript.isColliding && this.HasFreeLineOfSightToPoint(this.GetLaserOrigin(), null, false)) || Managers.personManager.GetOurScale() >= 2f)
			{
				Our.lastTeleportHitPoint = new Vector3?(this.lastTeleportHitPoint);
				this.TeleportTo(this.lastTeleportHitObject, this.lastTeleportHitPoint, this.isTeleportingIntoLiquid, false, this.isTeleportingOntoSittable, false, true);
				Managers.achievementManager.RegisterAchievement(Achievement.TeleportWalked);
			}
			this.hasLastTeleportHitPoint = false;
		}
		this.touchpadWasPressed = false;
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00022DB3 File Offset: 0x000211B3
	private bool ThumbStickTeleportLaserIsHeld(float steerX, float steerY)
	{
		return this.controller != null && CrossDevice.hasStick && steerY >= 0.7f;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00022DD8 File Offset: 0x000211D8
	private void AdjustZeroGravityLaserLength()
	{
		if (this.timeLaserPressStarted != -1f && this.timeLaserPressStarted + 0.5f <= Time.time)
		{
			float num = Time.deltaTime * 1.8f;
			this.zeroGravityLaserLength += num * (float)this.zeroGravityLaserGrowDirection;
			if (this.zeroGravityLaserGrowDirection == -1 && this.zeroGravityLaserLength < 0.35f)
			{
				this.zeroGravityLaserLength = 0.35f;
				this.zeroGravityLaserGrowDirection *= -1;
			}
			else if (this.zeroGravityLaserGrowDirection == 1)
			{
				float num2 = 2.4750001f * this.GetLaserDistanceMultiplier(-1f, true);
				if (this.zeroGravityLaserLength > num2)
				{
					this.zeroGravityLaserLength = num2;
					this.zeroGravityLaserGrowDirection *= -1;
				}
			}
			if (this.otherHandScript != null)
			{
				this.otherHandScript.SetLaserLengthFromOtherHand(this.zeroGravityLaserLength, this.zeroGravityLaserGrowDirection);
			}
		}
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00022ECF File Offset: 0x000212CF
	public void SetLaserLengthFromOtherHand(float _zeroGravityLaserLength, int _zeroGravityLaserGrowDirection)
	{
		this.zeroGravityLaserLength = _zeroGravityLaserLength;
		this.zeroGravityLaserGrowDirection = _zeroGravityLaserGrowDirection;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00022EE0 File Offset: 0x000212E0
	private void HandleSmoothMovement()
	{
		Vector3? vector = this.smoothMovementTeleportTarget;
		if (vector != null)
		{
			Vector3? vector2 = this.smoothMovementTeleportTarget;
			Vector3 value = vector2.Value;
			Transform transform = this.personRig;
			Vector3 position = this.personRig.position;
			Vector3? vector3 = this.smoothMovementTeleportTarget;
			transform.position = Vector3.MoveTowards(position, vector3.Value, this.smoothMovementSpeed * Time.deltaTime);
			if (this.personRig.position == value)
			{
				this.FinalizeTeleportTo(false, false);
			}
		}
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00022F64 File Offset: 0x00021364
	public void TeleportTo(GameObject targetObject, Vector3 target, bool isTeleportingIntoLiquid = false, bool omitSound = false, bool isTeleportingOntoSittable = false, bool suppressSmoothMovement = false, bool isAutomatedSend = true)
	{
		if (this.personRig != null && this.eyes != null)
		{
			Vector3 vector = new Vector3(target.x, target.y, target.z);
			vector.x += this.personRig.position.x - this.eyes.position.x;
			vector.z += this.personRig.position.z - this.eyes.position.z;
			bool flag = false;
			bool flag2 = targetObject != null && targetObject.transform.parent != null && targetObject.transform.parent.gameObject == Managers.personManager.ourPerson.controlledControllable;
			ThingPart thingPart = null;
			if (targetObject != null)
			{
				thingPart = targetObject.GetComponent<ThingPart>();
				flag = thingPart != null && thingPart.WeArePartOfPlacedSubThing();
			}
			if (this.isFreeFloatingZeroGravityHitPoint)
			{
				vector.y += this.personRig.position.y - this.eyes.position.y;
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "flying", true);
				this.isFreeFloatingZeroGravityHitPoint = false;
			}
			if (isTeleportingOntoSittable)
			{
				vector.y = this.personRig.position.y;
			}
			float num = Vector3.Distance(this.personRig.position, vector);
			this.TriggerStackedTeleportBodyTells(num);
			if (Our.useSmoothMovement && !suppressSmoothMovement && Managers.personManager.ourPerson.ridingBeacon == null && !flag)
			{
				this.smoothMovementTeleportTarget = new Vector3?(vector);
				this.smoothMovementSpeed = Mathf.Pow(num, 2.5f);
				if (this.lastTeleportHitWasOfAutoTiltedLaser)
				{
					this.smoothMovementSpeed *= 1.6f;
				}
				this.smoothMovementSpeed = Mathf.Clamp(this.smoothMovementSpeed, 1f, 100f);
				if (this.otherHandScript != null)
				{
					this.otherHandScript.smoothMovementTeleportTarget = null;
				}
			}
			else
			{
				this.personRig.position = vector;
				this.FinalizeTeleportTo(isTeleportingIntoLiquid, omitSound);
			}
			if (flag || flag2)
			{
				Managers.personManager.DoAddRidingBeacon(thingPart.GetParentThing(), target);
			}
			else
			{
				Managers.personManager.DoRemoveRidingBeacon();
			}
			CrossDevice.rigPositionIsAuthority = true;
		}
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00023238 File Offset: 0x00021638
	private void TriggerStackedTeleportBodyTells(float distance)
	{
		Person ourPerson = Managers.personManager.ourPerson;
		if (distance >= 3f)
		{
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(ourPerson, "moving very fast", true);
		}
		if (distance >= 1.5f)
		{
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(ourPerson, "moving fast", true);
		}
		Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(ourPerson, "moving", true);
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002329C File Offset: 0x0002169C
	private void FinalizeTeleportTo(bool isTeleportingIntoLiquid = false, bool omitSound = false)
	{
		this.smoothMovementTeleportTarget = null;
		Managers.personManager.CachePhotonRigLocation();
		if (isTeleportingIntoLiquid)
		{
			Effects.AddSplash(Managers.personManager.ourPerson.Head.transform.position, this.teleportingIntoLiquidOfColor);
		}
		if (!omitSound && !this.WeAreInvisibleInRecording())
		{
			Managers.soundManager.Play("teleport", base.transform, this.teleportVolume, false, false);
			this.teleportVolume = this.ShrinkLaserVolume(this.teleportVolume);
		}
		this.FinalizeTeleport();
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00023334 File Offset: 0x00021734
	public void FinalizeTeleport()
	{
		this.EndAllOngoingLegPuppeteering();
		Managers.areaManager.didTeleportMoveInThisArea = true;
		Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
		Managers.behaviorScriptManager.TriggerEventsRelatedToPosition();
		this.ShuffleParticlesForBetterTeleportEffect();
		this.eyeCollisionSphereScript.BrieflyDeactivateAsJustTeleported();
		this.UpdateAreaDialogBacksideStatsIfNeeded();
		this.UpdatePossibleCameraCalibrator();
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00023388 File Offset: 0x00021788
	private void UpdatePossibleCameraCalibrator()
	{
		Camera currentCameraComponent = Managers.cameraManager.currentCameraComponent;
		if (currentCameraComponent != null)
		{
			CameraCalibrator component = currentCameraComponent.transform.GetComponent<CameraCalibrator>();
			if (component != null)
			{
				component.UpdateCamera();
			}
		}
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x000233CC File Offset: 0x000217CC
	private void UpdateAreaDialogBacksideStatsIfNeeded()
	{
		if (this.otherHandScript != null && this.otherHandScript.currentDialog != null)
		{
			AreaDialog component = this.otherHandScript.currentDialog.GetComponent<AreaDialog>();
			if (component != null)
			{
				component.UpdateAreaThingStatsOnBackside();
			}
		}
		else if (this.currentDialog != null)
		{
			AreaDialog component2 = this.currentDialog.GetComponent<AreaDialog>();
			if (component2 != null)
			{
				component2.UpdateAreaThingStatsOnBackside();
			}
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00023458 File Offset: 0x00021858
	public bool HasFreeLineOfSightToPoint(Vector3 point, GameObject thingToIgnore = null, bool alsoCheckForFundament = false)
	{
		bool flag = true;
		Vector3 position = this.eyeCollisionSphere.transform.position;
		Ray ray = new Ray(position, (point - position).normalized);
		float num = Vector3.Distance(position, point);
		RaycastHit[] array = Physics.RaycastAll(ray, num);
		int num2 = 0;
		while (num2 < array.Length && flag)
		{
			RaycastHit raycastHit = array[num2];
			GameObject gameObject = raycastHit.collider.gameObject;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (gameObject.tag == "ThingPart")
			{
				Thing component = gameObject.transform.parent.gameObject.GetComponent<Thing>();
				if (component != null)
				{
					flag2 = component.isPassable;
					flag3 = component.isHoldable || component.remainsHeld;
					Thing myRootThing = component.GetMyRootThing();
					if (myRootThing.transform.parent != null)
					{
						flag4 = myRootThing.transform.CompareTag("Attachment");
						flag5 = myRootThing.transform.CompareTag("CurrentlyHeldLeft") || myRootThing.transform.CompareTag("CurrentlyHeldRight");
					}
				}
			}
			if (!flag4)
			{
				flag4 = gameObject.transform.parent != null && gameObject.transform.parent.gameObject.tag == "Attachment";
			}
			if (!flag5)
			{
				flag5 = gameObject.transform.parent != null && (gameObject.transform.parent.gameObject.CompareTag("CurrentlyHeldLeft") || gameObject.transform.parent.gameObject.CompareTag("CurrentlyHeldRight"));
			}
			if (gameObject.tag == "ThingPart")
			{
				if (!flag2 && !flag4 && !flag5 && !flag3)
				{
					if (thingToIgnore != null)
					{
						if (gameObject.transform.parent != null && gameObject.transform.parent.gameObject != thingToIgnore)
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
			}
			else if (alsoCheckForFundament && (gameObject.transform.name == "FundamentMesh" || gameObject.transform.name == "Inventory"))
			{
				flag = false;
			}
			num2++;
		}
		return flag;
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00023708 File Offset: 0x00021B08
	private Vector3 GetLaserOrigin()
	{
		return base.transform.position - base.transform.forward * 0.04f * Managers.personManager.GetOurScale();
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00023740 File Offset: 0x00021B40
	private void HandleContextInfoLaser()
	{
		if (this.GetPress(CrossDevice.button_teleport))
		{
			return;
		}
		if (!Universe.features.contextLaser || (Universe.demoMode && Our.mode != EditModes.Thing))
		{
			return;
		}
		if (Our.mode == EditModes.Area || Our.mode == EditModes.None)
		{
			if (this.GetPressDown(CrossDevice.button_context))
			{
				Managers.thingManager.UpdateAllVisibilityAndCollision(true);
			}
			else if (this.GetPressUp(CrossDevice.button_context))
			{
				Managers.thingManager.UpdateAllVisibilityAndCollision(false);
			}
		}
		if (this.GetPress(CrossDevice.button_context))
		{
			Vector3 laserOrigin = this.GetLaserOrigin();
			float num = this.GetLaserWidth() * Managers.personManager.GetOurScale();
			this.AddLaserLineIfNeeded();
			this.laserLine.SetWidth(num, num);
			this.laserLine.SetPosition(0, laserOrigin);
			Ray ray = new Ray(laserOrigin, base.transform.forward);
			RaycastHit[] array = (from h in Physics.RaycastAll(ray, 500f)
				orderby h.distance
				select h).ToArray<RaycastHit>();
			this.lastContextInfoHit = null;
			Vector3 vector = Vector3.zero;
			this.associatedContextDialogType = DialogType.None;
			bool flag = false;
			if (array.Length >= 1)
			{
				foreach (RaycastHit raycastHit in array)
				{
					string tag = raycastHit.collider.gameObject.tag;
					Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(raycastHit.collider.gameObject);
					Thing thing = ((!(raycastHit.collider.transform.parent != null)) ? null : raycastHit.collider.transform.parent.GetComponent<Thing>());
					bool flag2 = personThisObjectIsOf != null && raycastHit.collider.transform.parent != null && raycastHit.collider.transform.parent.parent != null && raycastHit.collider.transform.parent.parent.name == "HeadAttachmentPoint";
					Thing thing2 = ((!(thing != null)) ? null : thing.GetMyRootThing());
					if (!(thing2 != null) || !thing2.replacesHandsWhenAttached || !(thing2.gameObject.tag == "Attachment"))
					{
						if (thing && (thing.isInInventoryOrDialog || thing.isGiftInDialog))
						{
							this.associatedContextDialogType = DialogType.Thing;
							this.lastContextInfoHit = raycastHit.collider.transform.parent.gameObject;
						}
						else if (personThisObjectIsOf != null && (personThisObjectIsOf != Managers.personManager.ourPerson || flag2))
						{
							this.associatedContextDialogType = DialogType.Profile;
							this.lastContextInfoHit = raycastHit.collider.gameObject;
						}
						else if (Our.mode == EditModes.Thing && tag == "ThingPart")
						{
							Transform transform = raycastHit.collider.transform;
							bool flag3 = false;
							GameObject includedSubThingDirectMasterThingPart = Managers.thingManager.GetIncludedSubThingDirectMasterThingPart(transform);
							if (includedSubThingDirectMasterThingPart != null && includedSubThingDirectMasterThingPart == CreationHelper.thingPartWhoseStatesAreEdited)
							{
								DialogType currentNonStartDialogType = Managers.dialogManager.GetCurrentNonStartDialogType();
								if (currentNonStartDialogType == DialogType.IncludedSubThing || currentNonStartDialogType == DialogType.IncludedSubThings)
								{
									flag3 = true;
									this.associatedContextDialogType = DialogType.IncludedSubThing;
									this.lastContextInfoHit = transform.parent.gameObject;
								}
							}
							if (!flag3)
							{
								GameObject includedSubThingTopMasterThingPart = Managers.thingManager.GetIncludedSubThingTopMasterThingPart(transform);
								if (includedSubThingTopMasterThingPart != null)
								{
									transform = includedSubThingTopMasterThingPart.transform;
								}
								bool flag4 = transform.parent != null && transform.parent.gameObject == CreationHelper.thingBeingEdited;
								if (flag4)
								{
									this.associatedContextDialogType = DialogType.ThingPart;
									this.lastContextInfoHit = transform.gameObject;
									Our.SetLastTransformHandled(transform);
								}
								else
								{
									this.associatedContextDialogType = DialogType.Thing;
									this.lastContextInfoHit = transform.parent.gameObject;
								}
							}
						}
						else if (tag == "ThingPart")
						{
							this.associatedContextDialogType = DialogType.Thing;
							this.lastContextInfoHit = raycastHit.collider.gameObject.transform.parent.gameObject;
						}
					}
					if (Universe.demoMode && this.associatedContextDialogType == DialogType.Thing)
					{
						this.associatedContextDialogType = DialogType.None;
						this.lastContextInfoHit = null;
					}
					if (this.lastContextInfoHit != null)
					{
						flag = true;
						vector = raycastHit.point;
						this.laserLine.SetPosition(1, raycastHit.point);
					}
					if (this.lastContextInfoHit != null)
					{
						break;
					}
				}
			}
			if (this.lastContextInfoHit != null)
			{
				Color color = new Color(0f, 0.5f, 1f, 1f);
				this.laserLine.SetColors(color, color);
				this.ActivateContextInfoTargetSphere(this.lastContextInfoHit);
				this.contextInfoTargetSphere.transform.position = new Vector3(vector.x, vector.y, vector.z);
				this.TriggerHapticPulse(600);
				if (this.associatedContextDialogType == DialogType.Thing)
				{
					Thing component = this.lastContextInfoHit.GetComponent<Thing>();
					if (component != null)
					{
						if (this.currentlyHighlightedThing != null && component != this.currentlyHighlightedThing)
						{
							Managers.thingManager.RemoveOutlineHighlightMaterial(this.currentlyHighlightedThing, true);
							this.currentlyHighlightedThing = null;
						}
						if (!this.otherHandScript.currentlyHighlightedThing == component && Our.contextHighlightPlacements)
						{
							this.currentlyHighlightedThing = component;
							Managers.thingManager.AddOutlineHighlightMaterial(component, true);
						}
					}
				}
			}
			else
			{
				this.contextInfoTargetSphere.SetActive(false);
				if (!flag)
				{
					this.laserLine.SetPosition(1, laserOrigin + base.transform.forward * 500f);
				}
				Color color2 = new Color(0f, 0.5f, 1f, 0.25f);
				this.laserLine.SetColors(color2, color2);
				this.TriggerHapticPulse(300);
			}
			if (!this.applicationMenuWasPressed && !this.WeAreInvisibleInRecording())
			{
				Managers.soundManager.Play("teleportLaser", base.transform, this.contextLaserVolume, false, false);
				this.contextLaserVolume = this.ShrinkLaserVolume(this.contextLaserVolume);
			}
			if (!this.applicationMenuWasPressed)
			{
				this.applicationMenuWasPressed = true;
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "context laser started " + this.sideLowerCase, true);
			}
		}
		else if (this.GetPressUp(CrossDevice.button_context))
		{
			this.RemoveLaserLine();
			this.contextInfoTargetSphere.SetActive(false);
			if (this.currentlyHighlightedThing != null)
			{
				Managers.thingManager.RemoveOutlineHighlightMaterial(this.currentlyHighlightedThing, true);
				this.currentlyHighlightedThing = null;
			}
			if (this.associatedContextDialogType == DialogType.Thing && (this.currentDialogType == DialogType.IncludedSubThings || this.otherHandScript.currentDialogType == DialogType.IncludedSubThings))
			{
				Thing component2 = this.lastContextInfoHit.GetComponent<Thing>();
				if (component2 != null)
				{
					GameObject gameObject = this.SwitchToNewDialog(DialogType.IncludeThing, string.Empty);
					IncludeThingDialog component3 = gameObject.GetComponent<IncludeThingDialog>();
					component3.thing = component2;
				}
			}
			else if (this.associatedContextDialogType == DialogType.Thing && (this.currentDialogType == DialogType.PlacedSubThings || this.otherHandScript.currentDialogType == DialogType.PlacedSubThings))
			{
				this.TogglePlacedSubThingsId(this.lastContextInfoHit);
			}
			else if (this.associatedContextDialogType == DialogType.ThingPart && (this.currentDialogType == DialogType.ThingPartAutoContinuation || this.otherHandScript.currentDialogType == DialogType.ThingPartAutoContinuation))
			{
				this.AddOrRemoveAutoContinuationFrom(this.lastContextInfoHit);
			}
			else if (this.associatedContextDialogType == DialogType.ThingPart && (this.GetHeldObjectName() == "VertexMover" || this.otherHandScript.GetHeldObjectName() == "VertexMover") && !this.lastContextInfoHit.GetComponent<ThingPart>().isText)
			{
				CreationHelper.thingPartWhoseStatesAreEdited = this.lastContextInfoHit;
				ThingPart component4 = this.lastContextInfoHit.GetComponent<ThingPart>();
				if (this.currentDialogType == DialogType.VertexMover || this.otherHandScript.currentDialogType == DialogType.VertexMover)
				{
					VertexMoverDialog component5 = Our.GetCurrentNonStartDialog().GetComponent<VertexMoverDialog>();
					component5.SetThingPart(component4);
				}
				else
				{
					this.SwitchToNewDialog(DialogType.VertexMover, string.Empty);
				}
				VertexMover componentInChildren = Managers.personManager.ourPerson.Rig.GetComponentInChildren<VertexMover>();
				componentInChildren.SetThingPart(component4);
			}
			else if (this.associatedContextDialogType == DialogType.Thing && (this.currentDialogType == DialogType.ForumSettings || this.otherHandScript.currentDialogType == DialogType.ForumSettings))
			{
				this.SetForumDialogThing(this.lastContextInfoHit);
			}
			else
			{
				GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
				if (currentNonStartDialog != null && this.associatedContextDialogType == DialogType.Thing)
				{
					ThingDialog component6 = currentNonStartDialog.GetComponent<ThingDialog>();
					if (component6 != null && component6.thing != null && this.lastContextInfoHit == component6.thing.gameObject && component6.SecondsSinceOpened() <= 1f)
					{
						this.associatedContextDialogType = DialogType.None;
						this.applicationMenuWasPressed = false;
						component6.CloneIfWeMay();
						return;
					}
				}
				if (currentNonStartDialog != null && currentNonStartDialog.transform.parent != null)
				{
					OwnProfileDialog component7 = currentNonStartDialog.GetComponent<OwnProfileDialog>();
					if (component7 != null)
					{
						component7.Close();
					}
					Hand component8 = currentNonStartDialog.transform.parent.GetComponent<Hand>();
					component8.SwitchToNewDialog(DialogType.Start, string.Empty);
				}
				if (this.lastContextInfoHit != null)
				{
					bool isThisObjectOfOurPerson = Managers.personManager.GetIsThisObjectOfOurPerson(this.lastContextInfoHit, false);
					if (this.associatedContextDialogType == DialogType.Profile && !isThisObjectOfOurPerson)
					{
						Managers.achievementManager.RegisterAchievement(Achievement.ContextLaseredPerson);
					}
					if (isThisObjectOfOurPerson && Our.mode == EditModes.Thing)
					{
						this.associatedContextDialogType = DialogType.Create;
					}
					if (this.associatedContextDialogType != DialogType.None)
					{
						this.SwitchToNewDialog(this.associatedContextDialogType, string.Empty);
					}
				}
			}
			if (this.applicationMenuWasPressed)
			{
				this.applicationMenuWasPressed = false;
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "context laser ended " + this.sideLowerCase, true);
			}
		}
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0002423C File Offset: 0x0002263C
	private void AddOrRemoveAutoContinuationFrom(GameObject thingPartObject)
	{
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		ThingPartAutoContinuationDialog component = currentNonStartDialog.GetComponent<ThingPartAutoContinuationDialog>();
		ThingPart component2 = thingPartObject.GetComponent<ThingPart>();
		if (component != null && component2 != null)
		{
			component.AddOrRemoveAutoContinuationFrom(component2);
		}
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0002427C File Offset: 0x0002267C
	private void TogglePlacedSubThingsId(GameObject thingObject)
	{
		PlacedSubThingsDialog placedSubThingsDialog = this.currentDialog.GetComponent<PlacedSubThingsDialog>();
		if (placedSubThingsDialog == null)
		{
			placedSubThingsDialog = this.otherHandScript.currentDialog.GetComponent<PlacedSubThingsDialog>();
		}
		if (placedSubThingsDialog != null && CreationHelper.thingPartWhoseStatesAreEdited != null)
		{
			Thing component = thingObject.GetComponent<Thing>();
			ThingPart component2 = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
			if (component != null && component2 != null && !string.IsNullOrEmpty(component.placementId))
			{
				component2.ToggleInPlacedSubThingIds(component.placementId, component.thingId, Vector3.zero, Vector3.zero);
				placedSubThingsDialog.UpdatePlacedSubThingsInfo();
				Managers.soundManager.Play((!placedSubThingsDialog) ? "pickUp" : "success", base.transform, 0.3f, false, false);
			}
		}
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0002435C File Offset: 0x0002275C
	private void SetForumDialogThing(GameObject thingObject)
	{
		Thing component = thingObject.GetComponent<Thing>();
		if (component != null)
		{
			if (component.ContainsUnremovableCenter())
			{
				string unremovableCenterColorString = Managers.thingManager.GetUnremovableCenterColorString(component);
				Managers.forumManager.SetForumDialog(Managers.forumManager.currentForumId, component.thingId, unremovableCenterColorString, delegate(string reasonFailed)
				{
					if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
					{
						Managers.soundManager.Play("success", base.transform, 0.2f, false, false);
						DialogType dialogType = DialogType.ForumSettings;
						if (this.currentDialogType == dialogType)
						{
							this.SwitchToNewDialog(dialogType, string.Empty);
						}
						else
						{
							this.otherHandScript.SwitchToNewDialog(dialogType, string.Empty);
						}
					}
				});
			}
			else
			{
				Managers.soundManager.Play("no", base.transform, 1f, false, false);
			}
		}
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x000243DC File Offset: 0x000227DC
	private bool ObjectIsPartlyVerySmall(GameObject thisObject)
	{
		Bounds bounds = new Bounds(thisObject.transform.position, Vector3.zero);
		foreach (Renderer renderer in thisObject.GetComponentsInChildren<Renderer>())
		{
			bounds.Encapsulate(renderer.bounds);
		}
		Vector3 size = bounds.size;
		return size.x <= 0.1f || size.y <= 0.1f || size.z <= 0.1f;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0002446C File Offset: 0x0002286C
	private void AddLaserLineIfNeeded()
	{
		if (this.laserLine == null)
		{
			this.laserLine = base.gameObject.AddComponent<LineRenderer>();
			this.laserLine.SetWidth(0.001f, 0.001f);
			this.laserLine.material = new Material(Shader.Find("Particles/Additive"));
		}
		this.laserLine.enabled = true;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x000244D6 File Offset: 0x000228D6
	private void RemoveLaserLine()
	{
		if (this.laserLine != null)
		{
			this.laserLine.enabled = false;
		}
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x000244F8 File Offset: 0x000228F8
	public void ShuffleParticlesForBetterTeleportEffect()
	{
		Transform transform = Managers.treeManager.GetTransform("/OurPersonRig/EnvironmentParticleSystems");
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.y = (float)global::UnityEngine.Random.Range(0, 360);
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00024536 File Offset: 0x00022936
	public void SwitchToNewDialog_AreaDialog(bool savedOk)
	{
		this.SwitchToNewDialog(DialogType.Area, string.Empty);
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00024545 File Offset: 0x00022945
	public void SetTargetSphereLayer(int layer)
	{
		this.targetSphere.layer = layer;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00024554 File Offset: 0x00022954
	private void ActivateTargetSphere()
	{
		float num = Managers.personManager.GetOurScale();
		num = Misc.ClampMax(num, 2f);
		this.targetSphere.transform.localScale = Misc.GetUniformVector3(0.1228412f * num);
		this.targetSphere.SetActive(true);
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x000245A0 File Offset: 0x000229A0
	private void ActivateContextInfoTargetSphere(GameObject lastContextInfoHit)
	{
		float num = ((!this.ObjectIsPartlyVerySmall(lastContextInfoHit)) ? 0.025f : 0.0125f);
		this.contextInfoTargetSphere.transform.localScale = Misc.GetUniformVector3(num * Managers.personManager.GetOurScale());
		this.contextInfoTargetSphere.SetActive(true);
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x000245F6 File Offset: 0x000229F6
	public void TriggerHapticPulse(ushort pulseAmount)
	{
		CrossDevice.TriggerHapticPulse(this, pulseAmount);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x000245FF File Offset: 0x000229FF
	public bool HasNonStartDialogOpen()
	{
		return true && this.currentDialogType != DialogType.Start;
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00024617 File Offset: 0x00022A17
	private bool GetPress(ulong type)
	{
		return CrossDevice.GetPress(this.controller, type, this.side);
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0002462B File Offset: 0x00022A2B
	public bool GetPressUp(ulong type)
	{
		return CrossDevice.GetPressUp(this.controller, type, this.side);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0002463F File Offset: 0x00022A3F
	public bool GetPressDown(ulong type)
	{
		return CrossDevice.GetPressDown(this.controller, type, this.side);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00024653 File Offset: 0x00022A53
	public Thing GetHeldThing()
	{
		return (!(this.handDotScript != null) || !(this.handDotScript.holdableInHand != null)) ? null : this.handDotScript.holdableInHand.GetComponent<Thing>();
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00024694 File Offset: 0x00022A94
	public string GetHeldObjectName()
	{
		return (!(this.handDotScript != null) || !(this.handDotScript.holdableInHand != null)) ? string.Empty : this.handDotScript.holdableInHand.name;
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x000246E4 File Offset: 0x00022AE4
	private void NormalizeLaserVolumes()
	{
		this.teleportLaserVolume = Mathf.MoveTowards(this.teleportLaserVolume, 0.375f, 0.1f * Time.deltaTime);
		this.contextLaserVolume = Mathf.MoveTowards(this.contextLaserVolume, 0.375f, 0.1f * Time.deltaTime);
		this.teleportVolume = Mathf.MoveTowards(this.teleportVolume, 0.15f, 0.1f * Time.deltaTime);
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x00024754 File Offset: 0x00022B54
	private float ShrinkLaserVolume(float value)
	{
		return Mathf.Clamp(value * 0.5f, 0.025f, 10f);
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0002476C File Offset: 0x00022B6C
	private void HandleMovedHandsAchievement()
	{
		if (this.checkForMovedHandsAchievement)
		{
			if (!this.didMoveHand && base.transform.localPosition != Vector3.zero)
			{
				Vector3? vector = this.firstRelevantPosition;
				if (vector == null)
				{
					this.firstRelevantPosition = new Vector3?(base.transform.localPosition);
				}
				else
				{
					Vector3? vector2 = this.firstRelevantPosition;
					float num = Vector3.Distance(vector2.Value, base.transform.localPosition);
					if (num >= 0.1f)
					{
						this.didMoveHand = true;
					}
				}
			}
			if (this.didMoveHand && this.otherHandScript.didMoveHand && this.side == Side.Left)
			{
				this.checkForMovedHandsAchievement = false;
				this.otherHandScript.checkForMovedHandsAchievement = false;
				if (CrossDevice.type == global::DeviceType.OculusTouch)
				{
					Managers.achievementManager.RegisterAchievement(Achievement.MovedHandsWithOculusTouch);
				}
				else if (CrossDevice.type == global::DeviceType.Index)
				{
					Managers.achievementManager.RegisterAchievement(Achievement.MovedHandsWithIndex);
				}
				else if (CrossDevice.type == global::DeviceType.WindowsMixedReality)
				{
					Managers.achievementManager.RegisterAchievement(Achievement.MovedHandsWithWindowsMixedReality);
				}
				else
				{
					Managers.achievementManager.RegisterAchievement(Achievement.MovedHandsWithVive);
				}
			}
		}
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x000248A5 File Offset: 0x00022CA5
	private bool WeAreInvisibleInRecording()
	{
		return this.followerCamera != null && this.followerCamera.gameObject.activeSelf && this.followerCamera.weAreInvisible;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x000248DC File Offset: 0x00022CDC
	public void ToggleStaticMode()
	{
		SteamVR_TrackedObject component = base.GetComponent<SteamVR_TrackedObject>();
		if (component != null)
		{
			component.enabled = !component.enabled;
			if (!component.enabled)
			{
				Managers.desktopManager.PositionHand(this.side);
				base.transform.position += Vector3.up * 0.01f;
				base.transform.position += this.headCore.transform.forward * 0.1f;
			}
		}
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0002497B File Offset: 0x00022D7B
	public static void TeleportToPosition(Vector3 position)
	{
		if (Hand.handReference == null)
		{
			Hand.handReference = Misc.GetAHandOfOurs();
		}
		if (Hand.handReference != null)
		{
			Hand.handReference.TeleportTo(null, position, false, false, false, false, true);
		}
	}

	// Token: 0x04000526 RID: 1318
	public GameObject targetSpherePrefab;

	// Token: 0x04000527 RID: 1319
	public GameObject handDot;

	// Token: 0x04000528 RID: 1320
	public GameObject handAttachmentPoint;

	// Token: 0x04000529 RID: 1321
	public GameObject armAttachmentPoint;

	// Token: 0x0400052A RID: 1322
	private HandDot handDotScript;

	// Token: 0x0400052B RID: 1323
	public GameObject otherHand;

	// Token: 0x0400052C RID: 1324
	public GameObject lastContextInfoHit;

	// Token: 0x0400052D RID: 1325
	public GameObject eyeCollisionSphere;

	// Token: 0x0400052E RID: 1326
	public GameObject currentDialog;

	// Token: 0x0400052F RID: 1327
	public DialogType currentDialogType;

	// Token: 0x04000530 RID: 1328
	public DialogType dialogTypeToBeOpened;

	// Token: 0x04000531 RID: 1329
	private const float turnAroundTouchpadXThreshold = 0.6f;

	// Token: 0x04000532 RID: 1330
	private const float thumbStickEdgeThreshold = 0.8f;

	// Token: 0x04000533 RID: 1331
	private const float thumbStickEdgeThresholdForTeleportLaser = 0.7f;

	// Token: 0x04000534 RID: 1332
	public Vector3? smoothMovementTeleportTarget;

	// Token: 0x04000535 RID: 1333
	private float smoothMovementSpeed;

	// Token: 0x04000536 RID: 1334
	private Color laserLineStrongColor = new Color(1f, 1f, 1f, 0.75f);

	// Token: 0x04000537 RID: 1335
	private Color laserLineWeakColor = new Color(1f, 1f, 1f, 0.25f);

	// Token: 0x04000538 RID: 1336
	private const float shrinkingHapticPulseSeconds = 0.2f;

	// Token: 0x04000539 RID: 1337
	private float shrinkingHapticPulseUntilTime = -1f;

	// Token: 0x0400053A RID: 1338
	private EyeCollisionSphere eyeCollisionSphereScript;

	// Token: 0x0400053B RID: 1339
	public bool didMoveHand;

	// Token: 0x0400053C RID: 1340
	private Vector3? firstRelevantPosition;

	// Token: 0x0400053D RID: 1341
	public bool checkForMovedHandsAchievement = true;

	// Token: 0x0400053E RID: 1342
	private GameObject headCore;

	// Token: 0x0400053F RID: 1343
	public Thing currentlyHighlightedThing;

	// Token: 0x04000540 RID: 1344
	private bool authorityControllerFound;

	// Token: 0x04000541 RID: 1345
	private static Hand handReference;

	// Token: 0x04000542 RID: 1346
	private SteamVR_TrackedObject trackedObj;

	// Token: 0x04000543 RID: 1347
	private GameObject targetSphere;

	// Token: 0x04000544 RID: 1348
	private GameObject contextInfoTargetSphere;

	// Token: 0x04000545 RID: 1349
	private LineRenderer laserLine;

	// Token: 0x04000546 RID: 1350
	public Hand otherHandScript;

	// Token: 0x04000547 RID: 1351
	private DialogType associatedContextDialogType;

	// Token: 0x04000548 RID: 1352
	public float dialogOpenFactor;

	// Token: 0x04000549 RID: 1353
	public float dialogBacksideOpenFactor;

	// Token: 0x0400054A RID: 1354
	private bool thisThumbstickEdgeTurnHandled;

	// Token: 0x0400054B RID: 1355
	private const float maxLaserDistance = 7.5f;

	// Token: 0x0400054C RID: 1356
	private const float maxContextLaserDistance = 500f;

	// Token: 0x0400054D RID: 1357
	private const float zeroGravityLaserMinLength = 0.35f;

	// Token: 0x0400054E RID: 1358
	private const float zeroGravityLaserMaxLength = 2.4750001f;

	// Token: 0x0400054F RID: 1359
	private float zeroGravityLaserLength = 2.4750001f;

	// Token: 0x04000550 RID: 1360
	private int zeroGravityLaserGrowDirection = -1;

	// Token: 0x04000551 RID: 1361
	private float timeLaserPressStarted = -1f;

	// Token: 0x04000552 RID: 1362
	private bool isFreeFloatingZeroGravityHitPoint;

	// Token: 0x04000553 RID: 1363
	private Vector3 lastTeleportHitPoint;

	// Token: 0x04000554 RID: 1364
	private bool hasLastTeleportHitPoint;

	// Token: 0x04000555 RID: 1365
	private bool touchpadWasPressed;

	// Token: 0x04000556 RID: 1366
	private bool lastTeleportHitWasOfAutoTiltedLaser;

	// Token: 0x04000557 RID: 1367
	private GameObject lastTeleportHitObject;

	// Token: 0x04000558 RID: 1368
	private bool applicationMenuWasPressed;

	// Token: 0x04000559 RID: 1369
	private Transform personRig;

	// Token: 0x0400055C RID: 1372
	private float infrequentEventsCheckDelay = 0.5f;

	// Token: 0x0400055D RID: 1373
	private float timeOfLastInfrequentEventsCheck = -1f;

	// Token: 0x0400055E RID: 1374
	private bool isTeleportingIntoLiquid;

	// Token: 0x0400055F RID: 1375
	private bool isTeleportingOntoSittable;

	// Token: 0x04000560 RID: 1376
	private Color teleportingIntoLiquidOfColor = Color.white;

	// Token: 0x04000561 RID: 1377
	private Vector3 handDotNormalPosition = Vector3.zero;

	// Token: 0x04000562 RID: 1378
	private Vector3 handDotNormalScale = Vector3.one;

	// Token: 0x04000563 RID: 1379
	private const float teleportLaserVolumeMax = 0.375f;

	// Token: 0x04000564 RID: 1380
	private const float contextLaserVolumeMax = 0.375f;

	// Token: 0x04000565 RID: 1381
	private const float teleportVolumeMax = 0.15f;

	// Token: 0x04000566 RID: 1382
	private float teleportLaserVolume = 0.375f;

	// Token: 0x04000567 RID: 1383
	private float contextLaserVolume = 0.375f;

	// Token: 0x04000568 RID: 1384
	private float teleportVolume = 0.15f;

	// Token: 0x04000569 RID: 1385
	private FollowerCamera followerCamera;

	// Token: 0x0400056A RID: 1386
	private const float slidySpeed = 2f;

	// Token: 0x0400056B RID: 1387
	private Transform slidyMovementDirectionCore;

	// Token: 0x0400056C RID: 1388
	private string sideLowerCase = string.Empty;

	// Token: 0x0400056D RID: 1389
	public Transform leg;

	// Token: 0x0400056F RID: 1391
	private Vector3 previousPosition = Vector3.zero;

	// Token: 0x04000570 RID: 1392
	private Quaternion previousRotation = Quaternion.identity;

	// Token: 0x04000571 RID: 1393
	private GameObject handObjectsWhilePuppeteering;

	// Token: 0x04000572 RID: 1394
	public HandSkeleton skeleton;
}
