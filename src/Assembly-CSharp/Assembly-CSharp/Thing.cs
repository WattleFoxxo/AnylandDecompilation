using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000274 RID: 628
public class Thing : MonoBehaviour
{
	// Token: 0x17000285 RID: 645
	// (get) Token: 0x060016C4 RID: 5828 RVA: 0x000CC9C0 File Offset: 0x000CADC0
	// (set) Token: 0x060016C5 RID: 5829 RVA: 0x000CC9C8 File Offset: 0x000CADC8
	public bool isInPushBackMode { get; private set; }

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x060016C6 RID: 5830 RVA: 0x000CC9D1 File Offset: 0x000CADD1
	// (set) Token: 0x060016C7 RID: 5831 RVA: 0x000CC9D9 File Offset: 0x000CADD9
	public string thingId
	{
		get
		{
			return this._thingId;
		}
		set
		{
			this.DisallowEmptyString(value, "thingId");
			this._thingId = value;
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x060016C8 RID: 5832 RVA: 0x000CC9EE File Offset: 0x000CADEE
	// (set) Token: 0x060016C9 RID: 5833 RVA: 0x000CC9F6 File Offset: 0x000CADF6
	public string placementId
	{
		get
		{
			return this._placementId;
		}
		set
		{
			this.DisallowEmptyString(value, "placementId");
			this._placementId = value;
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x060016CA RID: 5834 RVA: 0x000CCA0B File Offset: 0x000CAE0B
	// (set) Token: 0x060016CB RID: 5835 RVA: 0x000CCA13 File Offset: 0x000CAE13
	public Vector3 originalPlacementPosition { get; private set; }

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x060016CC RID: 5836 RVA: 0x000CCA1C File Offset: 0x000CAE1C
	// (set) Token: 0x060016CD RID: 5837 RVA: 0x000CCA24 File Offset: 0x000CAE24
	public Vector3 originalPlacementRotation { get; private set; }

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x060016CE RID: 5838 RVA: 0x000CCA2D File Offset: 0x000CAE2D
	// (set) Token: 0x060016CF RID: 5839 RVA: 0x000CCA35 File Offset: 0x000CAE35
	public bool stickyIsStuckToSomething { get; private set; }

	// Token: 0x060016D0 RID: 5840 RVA: 0x000CCA40 File Offset: 0x000CAE40
	private void Update()
	{
		if (this.isHeldAsHoldable)
		{
			this.HandleHeldAsHoldable();
		}
		if (this.handleRotateTowards)
		{
			this.HandleRotateTowards();
		}
		if (this.doShowDirection)
		{
			this.ShowCenterAndDirectionLine();
		}
		Vector3? vector = this.localTarget;
		if (vector != null)
		{
			this.FollowLocalTarget();
		}
		if (this.isThrownOrEmitted)
		{
			this.HandleThrownOrEmitted();
		}
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x000CCAAC File Offset: 0x000CAEAC
	private void HandleHeldAsHoldable()
	{
		this.HandlePotentialKinematicRigidbodyCreation();
		if (this.isHeldAsHoldableByOurPerson && this.handHoldingMe != null)
		{
			this.velocityMagnitude = ((this.handHoldingMe.controller == null) ? 0f : this.handHoldingMe.controller.velocity.magnitude);
			this.angularVelocityMagnitude = ((this.handHoldingMe.controller == null) ? 0f : this.handHoldingMe.controller.angularVelocity.magnitude);
			this.HandlePushBackReturnToNormal();
			if (this.haltEventsUntilTime != -1f && this.haltEventsUntilTime <= Time.time)
			{
				this.haltEventsUntilTime = -1f;
			}
			if (this.haltEventsUntilTime == -1f)
			{
				this.CheckForTurnedAroundEvent();
				this.CheckForHighSpeedEvent();
				this.CheckForRaisedAndLoweredEvent();
				this.HandleInfrequentHoldableEventChecks();
			}
		}
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x000CCBA4 File Offset: 0x000CAFA4
	private void HandleRotateTowards()
	{
		if (this.doRotateTowardsClosestPerson || this.doRotateTowardsSecondClosestPerson)
		{
			this.RotateTowardsPerson();
		}
		if (this.doRotateTowardsTop)
		{
			this.RotateTowardsTop();
		}
		if (this.doRotateTowardsClosestEmptyHand || (this.doRotateTowardsClosestEmptyHandWhileHeld && this.isHeldAsHoldable))
		{
			this.RotateTowardsClosestEmptyHand();
		}
		if (this.rotateTowardsClosestThingOfName != null)
		{
			this.HandleInfrequentEventsCheck();
			this.RotateTowardsClosestThingOfName();
		}
		if (this.doRotateTowardsMainCamera)
		{
			this.RotateTowardsMainCamera();
		}
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x000CCC30 File Offset: 0x000CB030
	private void RotateTowardsMainCamera()
	{
		if (this.mainCameraForRotatingTowards == null)
		{
			this.mainCameraForRotatingTowards = Managers.treeManager.GetTransform("/OurPersonRig/HeadCore");
		}
		base.transform.LookAt(this.mainCameraForRotatingTowards);
		base.transform.Rotate(Vector3.up * 180f);
		base.transform.Rotate(Vector3.forward * 180f);
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x000CCCA8 File Offset: 0x000CB0A8
	private void HandleThrownOrEmitted()
	{
		this.HandlePropelForward();
		this.HandleRotateForward();
		if (this.doesFloat)
		{
			this.ApplyWeakGravity();
		}
		if (this.destroyMeInTime <= Time.time)
		{
			Misc.Destroy(base.gameObject);
		}
		else if (this.stricterPhysicsSyncing && Our.IsMasterClient(false) && Time.time >= this.lastStricterPhysicsSyncingInformSent + 0.5f)
		{
			this.lastStricterPhysicsSyncingInformSent = Time.time;
			if (Managers.personManager != null)
			{
				Managers.personManager.DoInformOfThingPhysics(this);
			}
		}
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x000CCD44 File Offset: 0x000CB144
	private void HandlePropelForward()
	{
		if (this.isUsingRigidbody && this.rigidbody != null)
		{
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (thingPart.propelForwardPercent != 0f)
				{
					this.rigidbody.AddForceAtPosition(thingPart.propelForwardPercent * 0.015f * thingPart.transform.forward, thingPart.transform.position, ForceMode.VelocityChange);
				}
			}
		}
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000CCDDC File Offset: 0x000CB1DC
	private void HandleRotateForward()
	{
		if (this.isUsingRigidbody && this.rigidbody != null)
		{
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (thingPart.rotateForwardPercent != 0f)
				{
					this.rigidbody.maxAngularVelocity = 100f;
					this.rigidbody.angularDrag = 50f;
					this.rigidbody.AddTorque(thingPart.rotateForwardPercent * 2f * thingPart.transform.right, ForceMode.VelocityChange);
				}
			}
		}
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x000CCE88 File Offset: 0x000CB288
	private void FollowLocalTarget()
	{
		Vector3? vector = this.localTarget;
		Vector3 value = vector.Value;
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, value, 0.2f);
		if (Vector3.Distance(base.transform.localPosition, value) <= 0.01f)
		{
			Managers.soundManager.Play("bump", base.transform, 0.4f, false, false);
			if (Misc.Chance(25f))
			{
				Hand myRootHand = this.GetMyRootHand();
				if (myRootHand != null)
				{
					myRootHand.TriggerHapticPulse(Universe.highHapticPulse);
				}
			}
			this.localTarget = null;
		}
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x000CCF38 File Offset: 0x000CB338
	private Hand GetMyRootHand()
	{
		Hand hand = null;
		Transform transform = base.transform;
		while (hand == null)
		{
			if (transform.parent != null)
			{
				transform = transform.parent;
				if (transform == null)
				{
					break;
				}
				hand = transform.GetComponent<Hand>();
			}
		}
		return hand;
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x000CCF90 File Offset: 0x000CB390
	public void ApplyVelocitySetterIfNeeded(Vector3 vector)
	{
		if (this.isUsingRigidbody && this.rigidbody != null)
		{
			this.rigidbody.velocity = vector;
		}
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x000CCFBA File Offset: 0x000CB3BA
	public void ApplyVelocityMultiplierIfNeeded(Vector3 vector)
	{
		if (this.isUsingRigidbody && this.rigidbody != null)
		{
			this.rigidbody.velocity = Vector3.Scale(this.rigidbody.velocity, vector);
		}
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x000CCFF4 File Offset: 0x000CB3F4
	public void ApplyForceAdderIfNeeded(Vector3 vector)
	{
		if (this.isUsingRigidbody && this.rigidbody != null)
		{
			this.rigidbody.AddForce(vector, ForceMode.VelocityChange);
		}
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x000CD020 File Offset: 0x000CB420
	private void ApplyWeakGravity()
	{
		if (!Managers.areaManager.isZeroGravity && this.rigidbody != null && this.rigidbody.useGravity)
		{
			this.rigidbody.AddForce(new Vector3(0f, -0.5f, 0f));
		}
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x000CD07C File Offset: 0x000CB47C
	private void ShowCenterAndDirectionLine()
	{
		if (Our.mode == EditModes.Thing)
		{
			if (this.directionLine == null && base.gameObject == CreationHelper.thingBeingEdited)
			{
				this.directionLine = base.gameObject.AddComponent<LineRenderer>();
				if (this.directionLine != null)
				{
					this.directionLine.SetWidth(0.003f, 0.003f);
					this.directionLine.material = new Material(Shader.Find("Custom/SeeThroughLine"));
				}
				this.directionUpLine = new GameObject
				{
					name = "UpLineWrapper",
					transform = 
					{
						parent = base.transform,
						localScale = Vector3.one,
						localPosition = Vector3.zero,
						localEulerAngles = Vector3.zero
					}
				}.AddComponent<LineRenderer>();
				if (this.directionUpLine != null)
				{
					this.directionUpLine.SetWidth(0.003f, 0.003f);
					this.directionUpLine.material = new Material(Shader.Find("Custom/SeeThroughLine"));
				}
			}
			if (this.directionLine != null)
			{
				this.directionLine.SetPosition(0, base.transform.position);
				this.directionLine.SetPosition(1, base.transform.position + base.transform.forward * 100f);
			}
			if (this.directionUpLine != null)
			{
				this.directionUpLine.SetPosition(0, base.transform.position);
				this.directionUpLine.SetPosition(1, base.transform.position + base.transform.up * 0.05f);
			}
		}
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x000CD25C File Offset: 0x000CB65C
	public void RemoveDirectionLine()
	{
		if (this.directionLine != null)
		{
			global::UnityEngine.Object.Destroy(this.directionLine);
			bool flag = this.directionUpLine == null;
			if (flag)
			{
				Transform transform = base.transform.Find("UpLineWrapper");
				if (transform != null)
				{
					Misc.Destroy(transform.gameObject);
				}
			}
			else
			{
				Misc.Destroy(this.directionUpLine.gameObject);
				global::UnityEngine.Object.Destroy(this.directionUpLine);
			}
		}
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x000CD2E0 File Offset: 0x000CB6E0
	private void RotateTowardsPerson()
	{
		int second = DateTime.UtcNow.Second;
		if (second != this.lastUtcSecondsInRotateTowards)
		{
			this.lastUtcSecondsInRotateTowards = second;
			if (this.doRotateTowardsClosestPerson)
			{
				this.personHeadToRotateTowards = Managers.personManager.GetPersonHeadClosestToPosition(base.transform.position, null);
			}
			else
			{
				this.personHeadToRotateTowards = Managers.personManager.GetPersonHeadSecondClosestToPosition(base.transform.position);
			}
		}
		this.RotateTowards(this.personHeadToRotateTowards, 0.05f);
	}

	// Token: 0x060016E0 RID: 5856 RVA: 0x000CD368 File Offset: 0x000CB768
	private void RotateTowardsClosestEmptyHand()
	{
		GameObject[] array = Managers.personManager.GetEveryonesHands();
		array = array.OrderBy((GameObject x) => Vector3.Distance(base.transform.position, x.transform.position)).ToArray<GameObject>();
		GameObject gameObject = null;
		foreach (GameObject gameObject2 in array)
		{
			string text = ((!(gameObject2.name == "HandCoreLeft")) ? "Right" : "Left");
			GameObject childWithTag = Misc.GetChildWithTag(gameObject2.transform, "CurrentlyHeld" + text);
			if (childWithTag == null)
			{
				gameObject = gameObject2;
				break;
			}
		}
		this.RotateTowards(gameObject, 1f);
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x000CD418 File Offset: 0x000CB818
	private void RotateTowardsClosestThingOfName()
	{
		if (this.cachedClosestThingOfName == null || this.cachedClosestThingOfName.name != this.cachedClosestThingName)
		{
			this.CacheClosestThingOfName();
		}
		if (this.cachedClosestThingOfName != null)
		{
			this.RotateTowards(this.cachedClosestThingOfName, 1f);
		}
		else if (this.originalPosition != Vector3.zero)
		{
			base.transform.localPosition = this.originalPosition;
			base.transform.localRotation = this.originalRotation;
		}
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x000CD4B8 File Offset: 0x000CB8B8
	private void CacheClosestThingOfName()
	{
		if (Managers.treeManager != null && Managers.thingManager != null)
		{
			List<GameObject> list = new List<GameObject>();
			list.Add(Managers.treeManager.GetObject("/OurPersonRig"));
			list.Add(Managers.treeManager.GetObject("/People"));
			list.Add(Managers.treeManager.GetObject("/Universe/ThrownOrEmittedThings"));
			list.Add(Managers.thingManager.placements);
			this.cachedClosestThingOfName = Managers.thingManager.GetClosestThingOfNameIn(base.gameObject, list, this.rotateTowardsClosestThingOfName);
			this.cachedClosestThingName = ((!(this.cachedClosestThingOfName != null)) ? null : this.cachedClosestThingOfName.name);
		}
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x000CD580 File Offset: 0x000CB980
	private void RotateTowards(GameObject thisObject, float fractionOfJourney = 0.05f)
	{
		if (thisObject != null)
		{
			if (this.rotateTowardsSettings != null && (this.rotateTowardsSettings.locked != null || this.rotateTowardsSettings.lockedLocal != null))
			{
				Vector3 eulerAngles = base.transform.eulerAngles;
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				this.DoRotateTowards(thisObject, fractionOfJourney);
				if (this.rotateTowardsSettings.locked != null)
				{
					Vector3 eulerAngles2 = base.transform.eulerAngles;
					if (this.rotateTowardsSettings.locked.x)
					{
						eulerAngles2.x = eulerAngles.x;
					}
					if (this.rotateTowardsSettings.locked.y)
					{
						eulerAngles2.y = eulerAngles.y;
					}
					if (this.rotateTowardsSettings.locked.z)
					{
						eulerAngles2.z = eulerAngles.z;
					}
					base.transform.eulerAngles = eulerAngles2;
				}
				if (this.rotateTowardsSettings.lockedLocal != null)
				{
					Vector3 localEulerAngles2 = base.transform.localEulerAngles;
					if (this.rotateTowardsSettings.lockedLocal.x)
					{
						localEulerAngles2.x = localEulerAngles.x;
					}
					if (this.rotateTowardsSettings.lockedLocal.y)
					{
						localEulerAngles2.y = localEulerAngles.y;
					}
					if (this.rotateTowardsSettings.lockedLocal.z)
					{
						localEulerAngles2.z = localEulerAngles.z;
					}
					base.transform.localEulerAngles = localEulerAngles2;
				}
			}
			else
			{
				this.DoRotateTowards(thisObject, fractionOfJourney);
			}
		}
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x000CD714 File Offset: 0x000CBB14
	private void DoRotateTowards(GameObject thisObject, float fractionOfJourney)
	{
		Vector3 eulerAngles = Quaternion.LookRotation(base.transform.position - thisObject.transform.position).eulerAngles;
		eulerAngles = new Vector3(eulerAngles.x + 180f, eulerAngles.y, eulerAngles.z);
		Quaternion quaternion = Quaternion.Euler(eulerAngles);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, fractionOfJourney);
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x000CD790 File Offset: 0x000CBB90
	private void RotateTowardsTop()
	{
		Vector3 eulerAngles = Quaternion.LookRotation(base.transform.position - Vector3.up * 100000f).eulerAngles;
		eulerAngles = new Vector3(eulerAngles.x + 180f, eulerAngles.y, eulerAngles.z);
		Quaternion quaternion = Quaternion.Euler(eulerAngles);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, 0.05f);
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x000CD814 File Offset: 0x000CBC14
	private Quaternion GetRandomQuaternion(float maxOffset)
	{
		Vector3 randomVector = Misc.GetRandomVector3(maxOffset);
		return Quaternion.Euler(randomVector.x, randomVector.y, randomVector.z);
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x000CD844 File Offset: 0x000CBC44
	public void ThrowMe(Vector3 velocity, Vector3 angularVelocity, string thisThrownId = null)
	{
		this.UpdateSpeechAmplification(false);
		this.isHeldAsHoldableByOurPerson = false;
		this.editMode = EditModes.None;
		this.placementId = null;
		this.RemoveThingPartRigidbodiesAndColliderTriggers();
		base.transform.parent = Managers.treeManager.GetTransform("/Universe/ThrownOrEmittedThings");
		base.gameObject.tag = "Thing";
		this.NormalizeName();
		ThingManager.SetLayerForThingAndParts(this, "IgnorePassableObjects");
		if (this.rigidbody == null)
		{
			this.rigidbody = base.gameObject.AddComponent<Rigidbody>();
		}
		if (this.rigidbody != null)
		{
			this.rigidbody.isKinematic = false;
			this.rigidbody.useGravity = true;
			this.rigidbody.velocity = velocity;
			this.rigidbody.angularVelocity = angularVelocity;
			if (this.doesFloat)
			{
				float num = 0.1f;
				this.rigidbody.angularVelocity = new Vector3(angularVelocity.x * num, angularVelocity.y * num, angularVelocity.z * num);
				this.rigidbody.useGravity = false;
			}
		}
		this.handHoldingMe = null;
		this.isHeldAsHoldable = false;
		this.isThrownOrEmitted = true;
		if (!this.persistWhenThrownOrEmitted)
		{
			this.destroyMeInTime = Time.time + this.GetDestructionDelayWhenReleased();
		}
		this.thrownId = ((thisThrownId == null) ? Misc.GetRandomId() : thisThrownId);
		this.ApplyPropertiesToRigidbodyAndCreateCollider();
		if (this.rigidbody != null)
		{
			this.RegisterMeWithAttractors();
		}
		Managers.soundManager.Play("whoosh", base.transform, 0.5f, false, false);
	}

	// Token: 0x060016E8 RID: 5864 RVA: 0x000CD9DB File Offset: 0x000CBDDB
	private float GetDestructionDelayWhenReleased()
	{
		return (!this.stricterPhysicsSyncing) ? 30f : 120f;
	}

	// Token: 0x060016E9 RID: 5865 RVA: 0x000CD9F8 File Offset: 0x000CBDF8
	private void ApplyPropertiesToRigidbodyAndCreateCollider()
	{
		MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
		meshCollider.convex = true;
		if (this.isBouncy && this.isSlidy && this.version >= 5)
		{
			this.ApplyPhysicsMaterial(meshCollider, Managers.thingManager.bouncySlidyMaterial);
		}
		else if (this.isBouncy)
		{
			this.ApplyPhysicsMaterial(meshCollider, Managers.thingManager.bouncyMaterial);
		}
		else if (this.isSlidy)
		{
			this.ApplyPhysicsMaterial(meshCollider, Managers.thingManager.slidyMaterial);
		}
		if (this.rigidbody != null)
		{
			float? num = this.drag;
			if (num != null)
			{
				float? num2 = this.drag;
				if (num2.Value != 0f)
				{
					Rigidbody rigidbody = this.rigidbody;
					float? num3 = this.drag;
					rigidbody.drag = num3.Value;
				}
			}
			float? num4 = this.angularDrag;
			if (num4 != null)
			{
				float? num5 = this.angularDrag;
				if (num5.Value != 0.05f)
				{
					Rigidbody rigidbody2 = this.rigidbody;
					float? num6 = this.angularDrag;
					rigidbody2.angularDrag = num6.Value;
				}
			}
			if (this.lockPhysicsPosition.x)
			{
				this.rigidbody.constraints |= RigidbodyConstraints.FreezePositionX;
			}
			if (this.lockPhysicsPosition.y)
			{
				this.rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
			}
			if (this.lockPhysicsPosition.z)
			{
				this.rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
			}
			if (this.lockPhysicsRotation.x)
			{
				this.rigidbody.constraints |= RigidbodyConstraints.FreezeRotationX;
			}
			if (this.lockPhysicsRotation.y)
			{
				this.rigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;
			}
			if (this.lockPhysicsRotation.z)
			{
				this.rigidbody.constraints |= RigidbodyConstraints.FreezeRotationZ;
			}
		}
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x000CDC00 File Offset: 0x000CC000
	private void ApplyPhysicsMaterial(MeshCollider meshCollider, PhysicMaterial thisMaterial)
	{
		meshCollider.material = thisMaterial;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshCollider>();
		foreach (MeshCollider meshCollider2 in componentsInChildren)
		{
			meshCollider2.material = thisMaterial;
		}
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x000CDC48 File Offset: 0x000CC048
	public void EmitMeFromOrigin(Transform origin, Vector3 originPosition, Vector3 originEulerAngles, Vector3 originForward, float velocityPercent, bool isGravityFree, bool omitSound = false)
	{
		if (origin != null)
		{
			base.transform.localPosition = originPosition + originForward * (origin.localScale.z * 0.5f);
		}
		else
		{
			base.transform.localPosition = originPosition;
		}
		base.transform.localEulerAngles = originEulerAngles;
		this.rigidbody = base.gameObject.AddComponent<Rigidbody>();
		this.rigidbody.isKinematic = false;
		this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		this.isThrownOrEmitted = true;
		if (!this.persistWhenThrownOrEmitted)
		{
			this.destroyMeInTime = Time.time + this.GetDestructionDelayWhenReleased();
		}
		this.ApplyPropertiesToRigidbodyAndCreateCollider();
		if (origin != null)
		{
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
			Collider component = origin.GetComponent<Collider>();
			if (component != null)
			{
				foreach (ThingPart thingPart in componentsInChildren)
				{
					Collider component2 = thingPart.GetComponent<Collider>();
					if (component2 != null)
					{
						Physics.IgnoreCollision(component, component2);
						if (this.isPassable)
						{
							component2.isTrigger = true;
						}
					}
				}
			}
		}
		if (velocityPercent > 0f)
		{
			float num = velocityPercent / 100f * 100f;
			this.rigidbody.velocity = originForward * num;
		}
		if (velocityPercent == 100f)
		{
			isGravityFree = true;
		}
		this.rigidbody.useGravity = !isGravityFree;
		this.isUsingRigidbody = true;
		if (this.isUsingRigidbody && isGravityFree && !this.doesFloat)
		{
			this.isUsingRigidbody = false;
		}
		ThingManager.SetLayerForThingAndParts(this, "IgnorePassableObjects");
		this.RegisterMeWithAttractors();
		if (!omitSound)
		{
			Managers.soundManager.Play("whoosh", base.transform, 0.5f, false, false);
		}
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x000CDE30 File Offset: 0x000CC230
	private void RegisterMeWithAttractors()
	{
		Component[] allAttractors = Managers.thingManager.GetAllAttractors();
		foreach (AttractThings attractThings in allAttractors)
		{
			attractThings.WatchThingRigidbody(this.rigidbody);
		}
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x000CDE74 File Offset: 0x000CC274
	private void RemoveThingPartRigidbodiesAndColliderTriggers()
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			Rigidbody component = thingPart.GetComponent<Rigidbody>();
			if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
			Collider component2 = thingPart.GetComponent<Collider>();
			if (component2 != null)
			{
				component2.isTrigger = false;
			}
		}
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x000CDEE8 File Offset: 0x000CC2E8
	private void OnCollisionEnter(Collision collision)
	{
		string tag = collision.gameObject.tag;
		if (tag == "PressableDialogPart")
		{
			return;
		}
		Thing thing = null;
		if (tag != null)
		{
			if (!(tag == "ThingPart"))
			{
				if (tag == "Thing")
				{
					thing = collision.gameObject.GetComponent<Thing>();
				}
			}
			else if (collision.transform.parent != null)
			{
				thing = collision.transform.parent.GetComponent<Thing>();
			}
		}
		if (!(thing != null) || ((thing.emittedByThingId == null || !(thing.emittedByThingId == this._thingId)) && (this.emittedByThingId == null || !(thing.thingId == this.emittedByThingId))))
		{
			if (this.rigidbody != null)
			{
				this.rigidbody.useGravity = true;
				if (this.doesFloat)
				{
					this.rigidbody.useGravity = false;
				}
			}
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				string text = string.Empty;
				if (thingPart.currentState < thingPart.states.Count && string.IsNullOrEmpty(thingPart.states[thingPart.currentState].name))
				{
					text += thingPart.states[thingPart.currentState].name;
				}
				bool flag = dictionary.ContainsKey(text);
				thingPart.HandleTriggerEnter(collision.collider, true, flag);
				dictionary[text] = true;
			}
			if (this.doesShatter)
			{
				this.DeleteMe(false);
			}
			else if (this.isSticky)
			{
				if (this.rigidbody != null)
				{
					global::UnityEngine.Object.Destroy(this.rigidbody);
				}
				base.transform.parent = collision.other.transform.parent.transform;
				this.stickyIsStuckToSomething = true;
				this.isHoldable = false;
				this.remainsHeld = false;
				this.floatsOnLiquid = false;
			}
			else if (this.isBouncy)
			{
				float magnitude = collision.relativeVelocity.magnitude;
				if (magnitude >= 0.1f)
				{
					float num = Mathf.Clamp(magnitude * 0.15f, 0.0001f, 1f);
					num *= num;
					Managers.soundManager.Play((!this.omitAutoSounds) ? "bounce" : "bump", base.transform, num, false, false);
				}
			}
			else
			{
				float magnitude2 = collision.relativeVelocity.magnitude;
				if (magnitude2 >= 0.1f)
				{
					float num2 = Mathf.Clamp(magnitude2 * 0.15f, 0.0001f, 1f);
					num2 *= num2;
					Managers.soundManager.Play("bump", base.transform, num2, false, false);
				}
			}
		}
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x000CE21C File Offset: 0x000CC61C
	public string GetCurrentlyHeldTag()
	{
		GameObject gameObject = base.transform.parent.gameObject;
		return "CurrentlyHeld" + ((!(gameObject.name == "HandCoreLeft")) ? "Right" : "Left");
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x000CE268 File Offset: 0x000CC668
	private void HandleInfrequentEventsCheck()
	{
		if (this.timeOfLastInfrequentEventsCheck == -1f || this.timeOfLastInfrequentEventsCheck + this.infrequentEventsCheckDelay <= Time.time)
		{
			this.timeOfLastInfrequentEventsCheck = Time.time;
			if (this.rotateTowardsClosestThingOfName != null)
			{
				this.CacheClosestThingOfName();
			}
		}
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x000CE2B8 File Offset: 0x000CC6B8
	private void HandleInfrequentHoldableEventChecks()
	{
		if (this.timeOfLastInfrequentHoldableEventsCheck == -1f || this.timeOfLastInfrequentHoldableEventsCheck + this.infrequentHoldableEventsCheckDelay <= Time.time)
		{
			this.timeOfLastInfrequentHoldableEventsCheck = Time.time;
			this.CheckForShakenEvent();
		}
	}

	// Token: 0x060016F2 RID: 5874 RVA: 0x000CE2F4 File Offset: 0x000CC6F4
	private void CheckForTurnedAroundEvent()
	{
		float z = base.transform.eulerAngles.z;
		if (z >= 170f && z <= 190f)
		{
			this.TriggerEventAsStateAuthority(StateListener.EventType.OnTurnedAround, string.Empty);
			this.BrieflyHaltFurtherEvents();
		}
	}

	// Token: 0x060016F3 RID: 5875 RVA: 0x000CE33F File Offset: 0x000CC73F
	private void BrieflyHaltFurtherEvents()
	{
		this.haltEventsUntilTime = Time.time + 0.5f;
	}

	// Token: 0x060016F4 RID: 5876 RVA: 0x000CE352 File Offset: 0x000CC752
	private void CheckForShakenEvent()
	{
		if (this.velocityMagnitude >= 1f || this.angularVelocityMagnitude >= 1f)
		{
			this.TriggerEventAsStateAuthority(StateListener.EventType.OnShaken, string.Empty);
		}
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x000CE384 File Offset: 0x000CC784
	private void CheckForRaisedAndLoweredEvent()
	{
		if (this.handHoldingMe.controller != null)
		{
			Vector3 velocity = this.handHoldingMe.controller.velocity;
			float num = 0.5f;
			if (velocity.y <= -num)
			{
				this.TriggerEventAsStateAuthority(StateListener.EventType.OnLowered, string.Empty);
				this.BrieflyHaltFurtherEvents();
			}
			else if (velocity.y >= num)
			{
				this.TriggerEventAsStateAuthority(StateListener.EventType.OnRaised, string.Empty);
				this.BrieflyHaltFurtherEvents();
			}
		}
	}

	// Token: 0x060016F6 RID: 5878 RVA: 0x000CE400 File Offset: 0x000CC800
	private void CheckForHighSpeedEvent()
	{
		if (this.velocityMagnitude >= 1f)
		{
			this.TriggerEventAsStateAuthority(StateListener.EventType.OnHighSpeed, string.Empty);
			this.BrieflyHaltFurtherEvents();
		}
	}

	// Token: 0x060016F7 RID: 5879 RVA: 0x000CE428 File Offset: 0x000CC828
	public void StartShowTouchPushBack()
	{
		if (this.isHeldAsHoldableByOurPerson && this.parentBeforePushBack == null && !this.doRotateTowardsClosestEmptyHand)
		{
			bool flag = false;
			Hand hand = ((!(base.transform.parent != null)) ? null : base.transform.parent.GetComponent<Hand>());
			if (hand != null && hand.otherHandScript != null && hand.otherHandScript.currentDialogType == DialogType.Gifts)
			{
				flag = true;
			}
			if (!flag)
			{
				this.parentBeforePushBack = base.transform.parent;
				base.transform.parent = null;
				this.isInPushBackMode = true;
				base.CancelInvoke();
				base.Invoke("EndShowTouchPushBack", 0.08f);
			}
		}
	}

	// Token: 0x060016F8 RID: 5880 RVA: 0x000CE4FC File Offset: 0x000CC8FC
	public void EndPushBackIfNeededAsLetGo()
	{
		if (this.isInPushBackMode)
		{
			base.CancelInvoke();
			this.EndShowTouchPushBack();
		}
	}

	// Token: 0x060016F9 RID: 5881 RVA: 0x000CE515 File Offset: 0x000CC915
	private void EndShowTouchPushBack()
	{
		if (this.isHeldAsHoldableByOurPerson)
		{
			this.isInPushBackMode = false;
			base.transform.parent = this.parentBeforePushBack;
			this.parentBeforePushBack = null;
		}
	}

	// Token: 0x060016FA RID: 5882 RVA: 0x000CE544 File Offset: 0x000CC944
	private void HandlePushBackReturnToNormal()
	{
		if (!this.isInPushBackMode)
		{
			float num = 0.5f;
			base.transform.localPosition = Vector3.Slerp(base.transform.localPosition, this.originalPosition, num);
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.originalRotation, num);
		}
	}

	// Token: 0x060016FB RID: 5883 RVA: 0x000CE5A8 File Offset: 0x000CC9A8
	private void HandlePotentialKinematicRigidbodyCreation()
	{
		if (this.editMode == EditModes.None && !this.isUsingRigidbody)
		{
			this.isUsingRigidbody = true;
			this.UpdateHandHoldingMe();
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.CompareTag("ThingPart"))
					{
						Collider component = transform.GetComponent<Collider>();
						if (!(component == null) && component.enabled)
						{
							Rigidbody rigidbody = transform.gameObject.AddComponent<Rigidbody>();
							rigidbody.useGravity = false;
							rigidbody.isKinematic = true;
							MeshCollider component2 = transform.GetComponent<MeshCollider>();
							if (component2 != null)
							{
								component2.isTrigger = true;
								component2.convex = true;
							}
							else
							{
								BoxCollider component3 = transform.GetComponent<BoxCollider>();
								if (component3 != null)
								{
									component3.isTrigger = true;
								}
								else
								{
									SphereCollider component4 = transform.GetComponent<SphereCollider>();
									if (component4 != null)
									{
										component4.isTrigger = true;
									}
								}
							}
						}
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
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x000CE6E4 File Offset: 0x000CCAE4
	public bool TriggerEventAsStateAuthority(StateListener.EventType eventType, string data = "")
	{
		return this.TriggerEvent(eventType, data, true, null);
	}

	// Token: 0x060016FD RID: 5885 RVA: 0x000CE700 File Offset: 0x000CCB00
	public bool TriggerEvent(StateListener.EventType eventType, string data = "", bool weAreStateAuthority = false, Person relevantPerson = null)
	{
		if (base.name == Universe.objectNameIfAlreadyDestroyed)
		{
			return false;
		}
		bool flag = false;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			int currentStateTarget = thingPart.currentStateTarget;
			bool flag2 = thingPart.TriggerEvent(eventType, data, weAreStateAuthority, relevantPerson);
			if (flag2)
			{
				flag = true;
			}
		}
		if (flag && this.handHoldingMe != null && !this.IsTellEvent(eventType))
		{
			this.handHoldingMe.TriggerHapticPulse(Universe.miniBurstPulse);
		}
		return flag;
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x000CE7A8 File Offset: 0x000CCBA8
	private bool IsTellEvent(StateListener.EventType eventType)
	{
		return eventType == StateListener.EventType.OnTold || eventType == StateListener.EventType.OnToldByNearby || eventType == StateListener.EventType.OnToldByAny || eventType == StateListener.EventType.OnToldByBody;
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x000CE7C8 File Offset: 0x000CCBC8
	public void SetInvisibleToOurPerson(bool isInvisible)
	{
		string text = ((!isInvisible) ? "Default" : "InvisibleToOurPerson");
		ThingManager.SetLayerForThingAndParts(this, text);
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x000CE7F4 File Offset: 0x000CCBF4
	public void ResetStates()
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			thingPart.ResetStates();
		}
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x000CE834 File Offset: 0x000CCC34
	public void UpdateMaterials()
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(ThingPart));
		foreach (ThingPart thingPart in componentsInChildren)
		{
			thingPart.UpdateMaterial();
		}
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x000CE87C File Offset: 0x000CCC7C
	public void OnHold(bool isOur = false)
	{
		this.crossClientSubThingId = null;
		this.NormalizeName();
		if (isOur)
		{
			this.isHeldAsHoldableByOurPerson = true;
			this.rigidbody = base.gameObject.GetComponent<Rigidbody>();
			if (this.rigidbody != null)
			{
				global::UnityEngine.Object.Destroy(this.rigidbody);
			}
			this.isThrownOrEmitted = false;
			this.isUsingRigidbody = false;
			this.destroyMeInTime = -1f;
		}
		base.gameObject.tag = this.GetCurrentlyHeldTag();
		this.isHeldAsHoldable = this.editMode == EditModes.None;
		if (this.isHeldAsHoldable)
		{
			this.placementId = null;
			this.SetTextMeshesActive(true);
			this.UpdateHandHoldingMe();
		}
		this.MemorizeOriginalTransform(false);
		this.UpdateSpeechAmplification(true);
		this.UnassignMyPlacedSubThings();
		this.SetLightShadows(true, null);
		if (this.ContainsOnHears())
		{
			Managers.areaManager.ActivateAreaSpeechListener(false);
		}
		this.TriggerEvent(StateListener.EventType.OnTaken, string.Empty, false, null);
	}

	// Token: 0x06001703 RID: 5891 RVA: 0x000CE973 File Offset: 0x000CCD73
	public void SetEditMode(EditModes editMode, EditModes? previousEditMode = null)
	{
		this.editMode = editMode;
		this.previousEditMode = previousEditMode;
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x000CE984 File Offset: 0x000CCD84
	public int SetLightShadows(bool showShadow = false, int? limit = null)
	{
		int num = 0;
		if (!showShadow || !Managers.optimizationManager.doOptimizeSpeed)
		{
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(Light), true);
			foreach (Light light in componentsInChildren)
			{
				light.shadows = ((!showShadow) ? LightShadows.None : LightShadows.Soft);
				if (showShadow)
				{
					ThingPart component = light.GetComponent<ThingPart>();
					if (component != null && component.lightOmitsShadow)
					{
						light.shadows = LightShadows.None;
					}
				}
				if (light.shadows != LightShadows.None)
				{
					num++;
					if (num >= limit)
					{
						showShadow = false;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x000CEA58 File Offset: 0x000CCE58
	public void SetTextMeshesActive(bool isActive)
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(Transform), true);
		foreach (Transform transform in componentsInChildren)
		{
			if (transform.CompareTag("ThingPart"))
			{
				ThingPart component = transform.GetComponent<ThingPart>();
				if (component != null && component.isText)
				{
					component.gameObject.SetActive(isActive);
				}
			}
		}
	}

	// Token: 0x06001706 RID: 5894 RVA: 0x000CEADC File Offset: 0x000CCEDC
	public void SetImagePartsActive(bool isActive)
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (Component component in componentsInChildren)
		{
			if (component.CompareTag("ThingPart"))
			{
				ThingPart component2 = component.GetComponent<ThingPart>();
				if (component2 != null && component2.imageUrl != string.Empty)
				{
					component2.gameObject.SetActive(isActive);
				}
			}
		}
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x000CEB64 File Offset: 0x000CCF64
	public void OnCloneToHand(bool isOur = false)
	{
		this.crossClientSubThingId = null;
		base.gameObject.tag = this.GetCurrentlyHeldTag();
		if (isOur)
		{
			this.isHeldAsHoldable = this.editMode == EditModes.None;
			if (this.isHeldAsHoldable)
			{
				this.isHeldAsHoldableByOurPerson = true;
				this.UpdateHandHoldingMe();
			}
		}
		else
		{
			this.isHeldAsHoldable = true;
			this.isHeldAsHoldableByOurPerson = false;
		}
		this.placementId = null;
		this.editMode = EditModes.Area;
		Managers.soundManager.Play("clone", base.transform, 0.5f, false, false);
		this.MemorizeOriginalTransform(false);
		this.UpdateSpeechAmplification(true);
		this.UnassignMyPlacedSubThings();
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x000CEC08 File Offset: 0x000CD008
	public void OnAddToHand(bool isOur = false)
	{
		this.crossClientSubThingId = null;
		base.gameObject.tag = this.GetCurrentlyHeldTag();
		if (isOur)
		{
			this.isHeldAsHoldable = Our.mode == EditModes.None || (Our.mode == EditModes.Inventory && Our.previousMode == EditModes.None);
			if (this.isHeldAsHoldable)
			{
				this.isHeldAsHoldableByOurPerson = true;
				this.UpdateHandHoldingMe();
			}
		}
		else
		{
			bool flag;
			if (this.editMode != EditModes.None)
			{
				if (this.editMode == EditModes.Inventory)
				{
					EditModes? editModes = this.previousEditMode;
					flag = editModes.Value == EditModes.None;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			this.isHeldAsHoldable = flag;
			this.isHeldAsHoldableByOurPerson = false;
		}
		this.placementId = null;
		this.editMode = EditModes.None;
		Managers.soundManager.Play("pickUp", base.transform, 0.5f, false, false);
		this.MemorizeOriginalTransform(false);
		this.UpdateSpeechAmplification(true);
		this.UnassignMyPlacedSubThings();
		this.SetLightShadows(true, null);
		this.AutoUpdateAllVisibilityAndCollision();
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x000CED0C File Offset: 0x000CD10C
	private void UpdateSpeechAmplification(bool amplify)
	{
		if (Managers.personManager != null && this.amplifySpeech && Managers.areaManager != null && Managers.areaManager.rights.amplifiedSpeech == true)
		{
			Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(base.gameObject);
			if (personThisObjectIsOf != null && personThisObjectIsOf.isOurPerson)
			{
				Managers.personManager.DoAmplifySpeech(amplify, false);
			}
		}
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x000CEDA4 File Offset: 0x000CD1A4
	public void OnEditHold(bool isOur = false)
	{
		this.crossClientSubThingId = null;
		base.gameObject.tag = this.GetCurrentlyHeldTag();
		this.isHeldAsHoldable = this.editMode == EditModes.None;
		if (this.isHeldAsHoldable)
		{
			this.isHeldAsHoldableByOurPerson = isOur;
		}
		this.editMode = EditModes.Area;
		Managers.soundManager.Play("pickUp", base.transform, 0.3f, false, false);
		this.UnassignMyPlacedSubThings();
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x000CEE14 File Offset: 0x000CD214
	public void OnAttached()
	{
		this.crossClientSubThingId = null;
		base.gameObject.tag = "Attachment";
		this.isHeldAsHoldable = false;
		this.AdjustLightsOptimization(true);
		this.MemorizeOriginalTransform(false);
		this.NormalizeName();
		base.transform.localScale = Vector3.one;
		Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(base.gameObject);
		if (this.IsAtArmAttachmentPoint())
		{
			this.SetDefaultHandModelVisibility(!this.replacesHandsWhenAttached);
			if (!personThisObjectIsOf.isOurPerson && Managers.areaManager != null)
			{
				bool flag = this.replacesHandsWhenAttached && (Managers.areaManager.rights.invisibility == true || personThisObjectIsOf.IsWearingSomethingVisible());
				this.SetRingVisibility(!flag);
			}
		}
		if (this.ContainsOnHears())
		{
			Managers.areaManager.ActivateAreaSpeechListener(false);
		}
		this.UpdateAllVisibilityAndCollision(false, false);
		if (personThisObjectIsOf.isOurPerson)
		{
			Managers.settingManager.TriggerAllSettingChangesForAttachment(base.transform.parent.gameObject);
		}
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x000CEF37 File Offset: 0x000CD337
	public void OnAttachedInMeMode()
	{
		this.placementId = null;
		base.transform.localScale = Vector3.one;
		base.gameObject.tag = "Attachment";
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x000CEF60 File Offset: 0x000CD360
	private void SetRingVisibility(bool isVisible)
	{
		if (base.transform.parent != null && base.transform.parent.parent != null)
		{
			GameObject gameObject = Misc.FindObject(base.transform.parent.parent.gameObject, "Ring");
			if (gameObject != null)
			{
				Renderer component = gameObject.GetComponent<Renderer>();
				if (component != null)
				{
					component.enabled = isVisible;
				}
			}
		}
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x000CEFE4 File Offset: 0x000CD3E4
	private void NormalizeName()
	{
		base.gameObject.name = Misc.RemoveCloneFromName(base.gameObject.name);
	}

	// Token: 0x0600170F RID: 5903 RVA: 0x000CF004 File Offset: 0x000CD404
	public void OnDeleteAttachment()
	{
		this.UpdateSpeechAmplification(false);
		if (this.IsAtArmAttachmentPoint())
		{
			this.SetDefaultHandModelVisibility(true);
		}
		base.gameObject.name = Universe.objectNameIfAlreadyDestroyed;
		if (this.ContainsOnHears())
		{
			Managers.areaManager.ActivateAreaSpeechListener(false);
		}
	}

	// Token: 0x06001710 RID: 5904 RVA: 0x000CF050 File Offset: 0x000CD450
	public bool IsAtArmAttachmentPoint()
	{
		return base.transform.parent != null && base.transform.parent.name.IndexOf("Arm") == 0 && base.transform.parent.CompareTag("AttachmentPoint");
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x000CF0AC File Offset: 0x000CD4AC
	public void SetCollider(bool isActive)
	{
		Collider component = base.GetComponent<Collider>();
		if (component != null)
		{
			component.enabled = isActive;
		}
		Component[] componentsInChildren = base.transform.GetComponentsInChildren(typeof(Collider), true);
		foreach (Collider collider in componentsInChildren)
		{
			collider.enabled = isActive;
		}
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x000CF118 File Offset: 0x000CD518
	private void SetDefaultHandModelVisibility(bool isVisible)
	{
		if (base.transform.parent != null && base.transform.parent.parent != null)
		{
			string name = base.transform.parent.name;
			string text = null;
			if (name == "ArmLeftAttachmentPoint")
			{
				text = "Left";
			}
			else if (name == "ArmRightAttachmentPoint")
			{
				text = "Right";
			}
			if (text != null)
			{
				string text2 = "HandModel" + text + "ThingPart";
				Component[] componentsInChildren = base.transform.parent.parent.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
				foreach (ThingPart thingPart in componentsInChildren)
				{
					if (thingPart.gameObject.name == text2)
					{
						Renderer component = thingPart.GetComponent<Renderer>();
						if (component != null)
						{
							component.enabled = isVisible;
							break;
						}
					}
				}
			}
			else
			{
				Log.Debug("SetDefaultHandModelVisibility cannot determine side, ignored");
			}
		}
	}

	// Token: 0x06001713 RID: 5907 RVA: 0x000CF248 File Offset: 0x000CD648
	public void AdjustLightsOptimization(bool doOptimize)
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (doOptimize)
			{
				if (thingPart.materialType == MaterialTypes.PointLight || thingPart.materialType == MaterialTypes.SpotLight)
				{
					thingPart.materialTypeBeforeOptimization = thingPart.materialType;
					thingPart.materialType = MaterialTypes.Glow;
					thingPart.ResetStates();
					if (thingPart.states.Count <= 1)
					{
						thingPart.UpdateMaterial();
					}
				}
			}
			else if (thingPart.materialTypeBeforeOptimization == MaterialTypes.PointLight || thingPart.materialTypeBeforeOptimization == MaterialTypes.SpotLight)
			{
				thingPart.materialType = thingPart.materialTypeBeforeOptimization;
				thingPart.materialTypeBeforeOptimization = MaterialTypes.None;
				thingPart.ResetStates();
				if (thingPart.states.Count <= 1)
				{
					thingPart.UpdateMaterial();
				}
			}
		}
	}

	// Token: 0x06001714 RID: 5908 RVA: 0x000CF31C File Offset: 0x000CD71C
	private void TurnLightsIntoGlowsToOptimize()
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			MeshRenderer component = thingPart.gameObject.GetComponent<MeshRenderer>();
			if (component != null)
			{
				component.shadowCastingMode = ShadowCastingMode.Off;
				component.receiveShadows = false;
			}
		}
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x000CF380 File Offset: 0x000CD780
	public void OnPlacedFromHand(bool isOur = false, bool isPlacementUpdate = false)
	{
		base.gameObject.tag = "Thing";
		base.gameObject.name = this.givenName;
		this.isHeldAsHoldable = false;
		if (!isOur)
		{
			Managers.soundManager.Play("putDown", base.transform, 0.35f, false, false);
		}
		this.DisableIsInInventoryOrDialog();
		if (!isPlacementUpdate)
		{
			this.ResetStates();
		}
		this.MemorizeOriginalTransform(true);
		if (!isPlacementUpdate)
		{
			this.HandleAssignMyPlacedSubThingsAndMeAsPlacedSubThing();
		}
		if (Managers.optimizationManager.maxLightsToThrowShadows == 0)
		{
			this.SetLightShadows(false, null);
		}
		else if (isOur)
		{
			this.SetLightShadows(true, null);
		}
		this.AutoUpdateAllVisibilityAndCollision();
		if (this.ContainsOnHears())
		{
			Managers.areaManager.ActivateAreaSpeechListener(false);
		}
	}

	// Token: 0x06001716 RID: 5910 RVA: 0x000CF454 File Offset: 0x000CD854
	public void HandleAssignMyPlacedSubThingsAndMeAsPlacedSubThing()
	{
		if (this.IsPlacement())
		{
			if (this.containsPlacedSubThings)
			{
				this.AssignMyPlacedSubThings();
			}
			else
			{
				this.AssignMeAsPlacedSubThing();
			}
		}
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x000CF480 File Offset: 0x000CD880
	private void AssignMyPlacedSubThings()
	{
		ThingPart[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			thingPart.AssignMyPlacedSubThings(string.Empty);
		}
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x000CF4C0 File Offset: 0x000CD8C0
	private void AssignMeAsPlacedSubThing()
	{
		Thing[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren<Thing>();
		foreach (Thing thing in componentsInChildren)
		{
			if (thing.containsPlacedSubThings && thing.transform.parent == Managers.thingManager.placements.transform && thing.CompareTag("Thing"))
			{
				ThingPart[] componentsInChildren2 = thing.GetComponentsInChildren<ThingPart>();
				foreach (ThingPart thingPart in componentsInChildren2)
				{
					thingPart.AssignMyPlacedSubThings(this._placementId);
				}
			}
		}
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x000CF570 File Offset: 0x000CD970
	public bool IsPlacement()
	{
		bool flag = false;
		if (Managers.thingManager != null && Managers.thingManager.placements != null)
		{
			Thing myRootThing = this.GetMyRootThing();
			if (myRootThing != null)
			{
				flag = !string.IsNullOrEmpty(myRootThing.placementId);
			}
		}
		return flag;
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x000CF5C8 File Offset: 0x000CD9C8
	public Thing GetMyRootThing()
	{
		Thing thing = this;
		if (!this.stickyIsStuckToSomething)
		{
			Transform transform = base.transform.parent;
			while (transform != null)
			{
				Thing component = transform.GetComponent<Thing>();
				if (component != null)
				{
					thing = component;
					if (thing.stickyIsStuckToSomething)
					{
						break;
					}
				}
				transform = transform.parent;
			}
		}
		return thing;
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x000CF62C File Offset: 0x000CDA2C
	public void OnPlaced(bool isOur = false)
	{
		this.OnPlacedFromHand(isOur, false);
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x000CF638 File Offset: 0x000CDA38
	private void DisableIsInInventoryOrDialog()
	{
		if (this.isInInventoryOrDialog)
		{
			this.isInInventoryOrDialog = false;
			this.isInInventory = false;
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.CompareTag("ThingPart"))
					{
						ThingPart component = transform.GetComponent<ThingPart>();
						if (component != null)
						{
							component.isInInventoryOrDialog = false;
							component.isInInventory = false;
							component.UpdateMaterial();
						}
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
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x000CF6E8 File Offset: 0x000CDAE8
	public void OnPlacedJustCreated(bool isOur = false)
	{
		this.RemoveDirectionLine();
		Managers.thingManager.UpdateShowThingPartDirectionArrows(this, false);
		base.gameObject.tag = "Thing";
		this.isHeldAsHoldable = false;
		this.NormalizeName();
		this.givenName = base.name;
		this.ResetStates();
		Managers.soundManager.Play("putDown", base.transform, 0.35f, false, false);
		Managers.soundManager.Play("success", base.transform, 0.25f, false, false);
		this.MemorizeOriginalTransform(true);
		this.HandleAssignMyPlacedSubThingsAndMeAsPlacedSubThing();
		if (isOur)
		{
			this.SetLightShadows(true, null);
		}
		if (this.ContainsOnHears())
		{
			Managers.areaManager.ActivateAreaSpeechListener(false);
		}
		this.UpdateMaterials();
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (!string.IsNullOrEmpty(thingPart.videoIdToPlayAtAreaStart))
			{
				base.Invoke("HandleAutoPlayingVideos", 0.25f);
				break;
			}
		}
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x000CF80E File Offset: 0x000CDC0E
	private void HandleAutoPlayingVideos()
	{
		Managers.areaManager.HandleAutoPlayingVideos(this);
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x000CF81C File Offset: 0x000CDC1C
	public void OnAddJustCreatedTemporary(bool isOur = false)
	{
		this.RemoveDirectionLine();
		Managers.thingManager.UpdateShowThingPartDirectionArrows(this, false);
		base.gameObject.tag = "Thing";
		this.isHeldAsHoldable = false;
		this.NormalizeName();
		this.givenName = base.name;
		this.ResetStates();
		Managers.soundManager.Play("putDown", base.transform, 0.35f, false, false);
		Managers.soundManager.Play("success", base.transform, 0.15f, false, false);
		this.MemorizeOriginalTransform(false);
		base.transform.parent = Managers.treeManager.GetTransform("/Universe/ThrownOrEmittedThings");
		if (isOur)
		{
			this.SetLightShadows(true, null);
		}
		this.UpdateMaterials();
		this.EmitMeFromOrigin(base.transform, base.transform.localPosition, base.transform.localEulerAngles, Vector3.zero, 0f, false, true);
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x000CF910 File Offset: 0x000CDD10
	public void OnPlacedRecreatedPlacedSubThing(bool isOur = false)
	{
		base.gameObject.tag = "Thing";
		this.isHeldAsHoldable = false;
		this.givenName = base.name;
		this.ResetStates();
		Managers.soundManager.Play("putDown", base.transform, 0.35f, false, false);
		this.MemorizeOriginalTransform(true);
		this.HandleAssignMyPlacedSubThingsAndMeAsPlacedSubThing();
		if (isOur)
		{
			this.SetLightShadows(true, null);
		}
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x000CF988 File Offset: 0x000CDD88
	public void MemorizeOriginalTransform(bool isOriginalPlacement = false)
	{
		this.originalPosition = base.transform.localPosition;
		this.originalRotation = base.transform.localRotation;
		if (isOriginalPlacement)
		{
			this.originalPlacementPosition = base.transform.position;
			this.originalPlacementRotation = base.transform.eulerAngles;
		}
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x000CF9E0 File Offset: 0x000CDDE0
	public void OnHoldFromHand(bool isOur = false)
	{
		this.crossClientSubThingId = null;
		base.gameObject.tag = this.GetCurrentlyHeldTag();
		this.isHeldAsHoldable = true;
		this.isHeldAsHoldableByOurPerson = isOur;
		this.UpdateHandHoldingMe();
		this.editMode = EditModes.None;
		this.thrownId = null;
		this.isThrownOrEmitted = false;
		if (!isOur)
		{
			Managers.soundManager.Play("pickUp", base.transform, 0.3f, false, false);
		}
		this.MemorizeOriginalTransform(false);
		this.UpdateSpeechAmplification(true);
		this.SetLightShadows(true, null);
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x000CFA70 File Offset: 0x000CDE70
	public void OnHoldFromThrownThing(bool isOur = false)
	{
		this.crossClientSubThingId = null;
		if (isOur)
		{
			this.rigidbody = base.gameObject.GetComponent<Rigidbody>();
			if (this.rigidbody != null)
			{
				global::UnityEngine.Object.Destroy(this.rigidbody);
			}
			this.isThrownOrEmitted = false;
			this.isUsingRigidbody = false;
			this.destroyMeInTime = -1f;
			this.TriggerEvent(StateListener.EventType.OnTaken, string.Empty, false, null);
		}
		this.thrownId = null;
		base.gameObject.tag = this.GetCurrentlyHeldTag();
		if (isOur)
		{
			this.isHeldAsHoldable = this.editMode == EditModes.None;
			if (this.isHeldAsHoldable)
			{
				this.isHeldAsHoldableByOurPerson = true;
				this.UpdateHandHoldingMe();
			}
		}
		else
		{
			this.isHeldAsHoldable = true;
			this.isHeldAsHoldableByOurPerson = false;
		}
		Managers.soundManager.Play("pickUp", base.transform, 0.3f, false, false);
		this.MemorizeOriginalTransform(false);
		this.UpdateSpeechAmplification(true);
		this.SetLightShadows(true, null);
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x000CFB74 File Offset: 0x000CDF74
	public void DeleteMe(bool avoidDeletionEffects = false)
	{
		bool flag = this.ContainsOnHears();
		this.UpdateSpeechAmplification(false);
		this.UnassignMyPlacedSubThings();
		this.alreadyDeleted = true;
		if (!avoidDeletionEffects && !Managers.areaManager.isTransportInProgress)
		{
			this.TriggerEvent(StateListener.EventType.OnDestroyed, string.Empty, false, null);
			if (!Managers.soundLibraryManager.didPlaySoundThisUpdate && !this.omitAutoSounds)
			{
				if (this.doesShatter)
				{
					Sound sound = new Sound();
					sound.name = "glass shatters";
					Managers.soundLibraryManager.Play(base.transform.position, sound, false, false, false, -1f);
				}
				else
				{
					Managers.soundManager.Play("delete", base.transform, 0.5f, false, false);
				}
			}
			Effects.SpawnCrumbles(base.gameObject);
		}
		base.gameObject.name = Universe.objectNameIfAlreadyDestroyed;
		base.gameObject.tag = "Untagged";
		Managers.areaManager.AssignPlacedSubThings(this._thingId);
		global::UnityEngine.Object.Destroy(base.gameObject);
		if (this.ContainsOnHears())
		{
			Managers.areaManager.ActivateAreaSpeechListener(false);
		}
	}

	// Token: 0x06001725 RID: 5925 RVA: 0x000CFC94 File Offset: 0x000CE094
	private bool ContainsOnHears()
	{
		if (Managers.areaManager != null)
		{
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
			foreach (ThingPart thingPart in componentsInChildren)
			{
				foreach (ThingPartState thingPartState in thingPart.states)
				{
					foreach (StateListener stateListener in thingPartState.listeners)
					{
						if (stateListener.eventType == StateListener.EventType.OnHears && !string.IsNullOrEmpty(stateListener.whenData))
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x000CFDA4 File Offset: 0x000CE1A4
	private string GetUserIdOfPersonThisIsOf()
	{
		string text = string.Empty;
		if (Managers.personManager != null)
		{
			Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(base.gameObject);
			if (personThisObjectIsOf != null)
			{
				text = personThisObjectIsOf.userId;
			}
		}
		return text;
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x000CFDEC File Offset: 0x000CE1EC
	public string GetSpecifierId(ref ThingSpecifierType specifier)
	{
		string text = null;
		Thing myRootThing = this.GetMyRootThing();
		if (myRootThing.movableByEveryone && myRootThing.transform.parent != null && myRootThing.transform.parent != Managers.thingManager.placements.transform)
		{
			text = string.Concat(new string[]
			{
				this.GetUserIdOfPersonThisIsOf(),
				"~",
				Managers.personManager.GetTopographyIdOfHand(myRootThing.transform.parent.gameObject).ToString(),
				"~",
				myRootThing.placementId
			});
			specifier = ThingSpecifierType.HandMovableByEveryone;
		}
		else if ((myRootThing.isHeldAsHoldable && myRootThing.isHeldAsHoldableByOurPerson) || (myRootThing.transform.parent != null && myRootThing.transform.parent.name.StartsWith("HandCore") && !myRootThing.gameObject.CompareTag("Attachment")))
		{
			if (myRootThing.transform.parent != null && myRootThing.transform.parent.gameObject != null)
			{
				text = this.GetUserIdOfPersonThisIsOf() + "~" + Managers.personManager.GetTopographyIdOfHand(myRootThing.transform.parent.gameObject).ToString();
				specifier = ThingSpecifierType.Hand;
			}
		}
		else if (myRootThing.isThrownOrEmitted)
		{
			text = myRootThing.thrownId;
			specifier = ThingSpecifierType.Thrown;
		}
		else if (myRootThing.gameObject.CompareTag("Attachment"))
		{
			if (myRootThing.transform.parent != null && myRootThing.transform.parent.gameObject != null)
			{
				AttachmentPoint component = myRootThing.transform.parent.gameObject.GetComponent<AttachmentPoint>();
				if (component != null)
				{
					text = this.GetUserIdOfPersonThisIsOf() + "~" + component.id.ToString();
					specifier = ThingSpecifierType.Attachment;
				}
			}
		}
		else if (!string.IsNullOrEmpty(this.placementId))
		{
			text = this.placementId;
			specifier = ThingSpecifierType.Placement;
		}
		else if (!string.IsNullOrEmpty(myRootThing.placementId))
		{
			text = myRootThing.placementId;
			specifier = ThingSpecifierType.Placement;
		}
		if (!string.IsNullOrEmpty(this.crossClientSubThingId))
		{
			text = text + ";" + this.crossClientSubThingId;
		}
		return text;
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x000D0085 File Offset: 0x000CE485
	public void StartRotateTowardsClosestPerson()
	{
		this.handleRotateTowards = true;
		this.MemorizeOriginalTransform(false);
		this.doRotateTowardsClosestPerson = true;
		this.doRotateTowardsSecondClosestPerson = false;
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x000D00A3 File Offset: 0x000CE4A3
	public void StartRotateTowardsSecondClosestPerson()
	{
		this.handleRotateTowards = true;
		this.MemorizeOriginalTransform(false);
		this.doRotateTowardsClosestPerson = false;
		this.doRotateTowardsSecondClosestPerson = true;
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x000D00C1 File Offset: 0x000CE4C1
	public void StartRotateTowardsTop()
	{
		this.handleRotateTowards = true;
		this.MemorizeOriginalTransform(false);
		this.doRotateTowardsTop = true;
	}

	// Token: 0x0600172B RID: 5931 RVA: 0x000D00D8 File Offset: 0x000CE4D8
	public void StartRotateTowardsClosestEmptyHand()
	{
		this.handleRotateTowards = true;
		this.MemorizeOriginalTransform(false);
		this.doRotateTowardsClosestEmptyHand = true;
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x000D00EF File Offset: 0x000CE4EF
	public void StartRotateTowardsClosestThingOfName(string thingName, RotateThingSettings settings = null)
	{
		this.handleRotateTowards = true;
		this.rotateTowardsSettings = settings;
		this.MemorizeOriginalTransform(false);
		this.rotateTowardsClosestThingOfName = thingName;
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x000D010D File Offset: 0x000CE50D
	public void StartRotateTowardsMainCamera()
	{
		this.handleRotateTowards = true;
		this.MemorizeOriginalTransform(false);
		this.doRotateTowardsMainCamera = true;
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x000D0124 File Offset: 0x000CE524
	public void StopRotateTowardsPerson()
	{
		this.doRotateTowardsClosestPerson = false;
		this.doRotateTowardsSecondClosestPerson = false;
		this.RestoreOriginalPositionRotation();
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x000D013A File Offset: 0x000CE53A
	public void StopRotateTowardsTop()
	{
		this.doRotateTowardsTop = false;
		this.RestoreOriginalPositionRotation();
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x000D0149 File Offset: 0x000CE549
	public void StopRotateTowardsClosestEmptyHand()
	{
		this.doRotateTowardsClosestEmptyHand = false;
		this.doRotateTowardsClosestEmptyHandWhileHeld = false;
		this.RestoreOriginalPositionRotation();
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x000D015F File Offset: 0x000CE55F
	public void StopRotateThingTowardsClosestThingOfName()
	{
		this.rotateTowardsClosestThingOfName = null;
		this.rotateTowardsSettings = null;
		this.cachedClosestThingOfName = null;
		this.cachedClosestThingName = null;
		this.RestoreOriginalPositionRotation();
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x000D0183 File Offset: 0x000CE583
	public void StopRotateTowardsMainCamera()
	{
		this.doRotateTowardsMainCamera = false;
		this.RestoreOriginalPositionRotation();
	}

	// Token: 0x06001733 RID: 5939 RVA: 0x000D0192 File Offset: 0x000CE592
	private void RestoreOriginalPositionRotation()
	{
		if (this.originalPosition != Vector3.zero)
		{
			base.transform.localPosition = this.originalPosition;
			base.transform.localRotation = this.originalRotation;
		}
	}

	// Token: 0x06001734 RID: 5940 RVA: 0x000D01CC File Offset: 0x000CE5CC
	public void UpdateIsBigIndicators()
	{
		Bounds bounds = new Bounds(base.transform.position, Vector3.zero);
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
		{
			bounds.Encapsulate(renderer.bounds);
		}
		Vector3 size = bounds.size;
		this.isBig = size.x >= 7.5f || size.y >= 7.5f || size.z >= 7.5f;
		this.isVeryBig = size.x >= 15f || size.y >= 15f || size.z >= 15f;
		this.biggestSize = Misc.GetLargestValueOfVector(size);
	}

	// Token: 0x06001735 RID: 5941 RVA: 0x000D02AC File Offset: 0x000CE6AC
	public void UpdateContainsBehaviorScriptValue()
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			foreach (ThingPartState thingPartState in thingPart.states)
			{
				if (thingPartState.listeners.Count >= 1)
				{
					this.containsBehaviorScript = true;
					break;
				}
			}
		}
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x000D034C File Offset: 0x000CE74C
	public bool GetHasRecentUndoState()
	{
		bool flag = false;
		if (this.timeOfLastMemorizationForUndo != -1f)
		{
			float num = Time.time - this.timeOfLastMemorizationForUndo;
			flag = num <= 300f;
		}
		return flag;
	}

	// Token: 0x06001737 RID: 5943 RVA: 0x000D0388 File Offset: 0x000CE788
	public void UndoPositionAndRotationAsAuthority()
	{
		Vector3 vector = this.lastPositionForUndo;
		Vector3 vector2 = this.lastRotationForUndo;
		this.MemorizePositionAndRotationForUndo();
		Managers.personManager.DoMovePlacement(base.gameObject, vector, vector2);
	}

	// Token: 0x06001738 RID: 5944 RVA: 0x000D03BC File Offset: 0x000CE7BC
	public void OnMovePlacement(bool isOur = false)
	{
		if (!isOur)
		{
			base.gameObject.tag = "Thing";
			this.isHeldAsHoldable = false;
			if (!isOur)
			{
				Managers.soundManager.Play("putDown", base.transform, 0.35f, false, false);
			}
			this.DisableIsInInventoryOrDialog();
		}
		Managers.soundManager.Play("putDown", base.transform, 0.35f, false, false);
		this.ResetStates();
		this.MemorizeOriginalTransform(true);
		this.HandleAssignMyPlacedSubThingsAndMeAsPlacedSubThing();
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x000D043D File Offset: 0x000CE83D
	public void MemorizePositionAndRotationForUndo()
	{
		this.lastPositionForUndo = base.transform.localPosition;
		this.lastRotationForUndo = base.transform.localEulerAngles;
		this.timeOfLastMemorizationForUndo = Time.time;
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x000D046C File Offset: 0x000CE86C
	public void ReCenterThingBasedOnPart(ThingPart newCenterThingPart)
	{
		Vector3 position = base.transform.position;
		Vector3 eulerAngles = base.transform.eulerAngles;
		Vector3 position2 = newCenterThingPart.transform.position;
		Vector3 eulerAngles2 = newCenterThingPart.transform.eulerAngles;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart in componentsInChildren)
		{
			foreach (ThingPartState thingPartState in thingPart.states)
			{
				thingPart.transform.localPosition = thingPartState.position;
				thingPart.transform.localEulerAngles = thingPartState.rotation;
				thingPart.transform.parent = null;
				base.transform.position = position2;
				base.transform.eulerAngles = eulerAngles2;
				thingPart.transform.parent = base.transform;
				thingPartState.position = thingPart.transform.localPosition;
				thingPartState.rotation = thingPart.transform.localEulerAngles;
				base.transform.position = position;
				base.transform.eulerAngles = eulerAngles;
			}
			thingPart.ResetStates();
		}
		base.transform.position = position2;
		base.transform.eulerAngles = eulerAngles2;
		this.MemorizeOriginalTransform(true);
		if (this.doShowDirection)
		{
			this.ShowCenterAndDirectionLine();
		}
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x000D0608 File Offset: 0x000CEA08
	public bool ContainsUnremovableCenter()
	{
		bool flag = false;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.isUnremovableCenter)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x000D065A File Offset: 0x000CEA5A
	private void UpdateHandHoldingMe()
	{
		if (base.transform.parent != null)
		{
			this.handHoldingMe = base.transform.parent.gameObject.GetComponent<Hand>();
		}
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x000D0690 File Offset: 0x000CEA90
	public void UnassignMyPlacedSubThings()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.CompareTag("ThingPart"))
				{
					ThingPart component = transform.GetComponent<ThingPart>();
					if (component != null)
					{
						component.UnassignMyPlacedSubThings();
					}
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

	// Token: 0x0600173E RID: 5950 RVA: 0x000D0718 File Offset: 0x000CEB18
	public void RestoreOriginalPlacement(bool ignoreScale = false)
	{
		base.transform.parent = Managers.thingManager.placements.transform;
		base.transform.position = this.originalPlacementPosition;
		base.transform.eulerAngles = this.originalPlacementRotation;
		if (!ignoreScale)
		{
			base.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x000D0777 File Offset: 0x000CEB77
	public void RestoreOriginalPosition()
	{
		base.transform.position = this.originalPlacementPosition;
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x000D078A File Offset: 0x000CEB8A
	public void RestoreOriginalRotation()
	{
		base.transform.eulerAngles = this.originalPlacementRotation;
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x000D079D File Offset: 0x000CEB9D
	public bool IsAtOriginalPlacement()
	{
		return this.originalPlacementPosition == base.transform.position && this.originalPlacementRotation == base.transform.eulerAngles;
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x000D07D4 File Offset: 0x000CEBD4
	public void SetParticleSystemsStopPlay(bool doPlay)
	{
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(ParticleSystem), true);
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			if (doPlay)
			{
				particleSystem.Play();
			}
			else
			{
				particleSystem.Stop();
			}
		}
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x000D082E File Offset: 0x000CEC2E
	private void DisallowEmptyString(string value, string contextName)
	{
		if (value == string.Empty)
		{
			throw new Exception("Attempt to set " + contextName + " to empty string");
		}
	}

	// Token: 0x06001744 RID: 5956 RVA: 0x000D0856 File Offset: 0x000CEC56
	public bool IsRotatingTowardsEmptyHand()
	{
		return this.doRotateTowardsClosestEmptyHand || this.doRotateTowardsClosestEmptyHandWhileHeld;
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x000D086C File Offset: 0x000CEC6C
	public void OnDeleteStuck(bool isOur = false)
	{
		Managers.soundManager.Play("delete", base.transform, 0.3f, false, false);
		Effects.SpawnCrumbles(base.gameObject);
		Misc.Destroy(base.gameObject);
	}

	// Token: 0x06001746 RID: 5958 RVA: 0x000D08A0 File Offset: 0x000CECA0
	public bool IsControllable()
	{
		bool flag = false;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.HasControllableSettings())
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06001747 RID: 5959 RVA: 0x000D08F4 File Offset: 0x000CECF4
	public bool IsControllableWithThrust()
	{
		bool flag = false;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.joystickToControllablePart != null)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06001748 RID: 5960 RVA: 0x000D0946 File Offset: 0x000CED46
	public void SetTemporarilyInactiveAsControllableIsSpawned()
	{
		this.EndPushBackIfNeededAsLetGo();
		base.gameObject.SetActive(false);
		this.isTemporarilyInactiveAsControllableIsSpawned = true;
		base.Invoke("SetGameObjectActive", 2f);
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x000D0971 File Offset: 0x000CED71
	private void SetGameObjectActive()
	{
		base.gameObject.SetActive(true);
		this.isTemporarilyInactiveAsControllableIsSpawned = false;
	}

	// Token: 0x0600174A RID: 5962 RVA: 0x000D0986 File Offset: 0x000CED86
	public void MakeControllable()
	{
	}

	// Token: 0x0600174B RID: 5963 RVA: 0x000D0988 File Offset: 0x000CED88
	public bool IsPlacedSubThing()
	{
		return base.transform.parent != null && base.transform.parent.CompareTag("PlacedSubThings");
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x000D09B8 File Offset: 0x000CEDB8
	public void LetGoIfWeAreHoldableInHand()
	{
		if (this.isHeldAsHoldable && this.handHoldingMe != null)
		{
			this.handHoldingMe.handDot.GetComponent<HandDot>().LetGoOfHoldableInHand();
		}
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x000D09EB File Offset: 0x000CEDEB
	public bool IsIncludedSubThing()
	{
		return base.transform.parent != null && base.transform.parent.CompareTag("IncludedSubThings");
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x000D0A1C File Offset: 0x000CEE1C
	public bool HasIncludedSubThings()
	{
		bool flag = false;
		Component[] componentsInChildren = base.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.HasIncludedSubThings())
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x000D0A6C File Offset: 0x000CEE6C
	public ThingPart GetThingPartByGuid(string guid)
	{
		ThingPart thingPart = null;
		Component[] componentsInChildren = base.transform.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart2 in componentsInChildren)
		{
			if (thingPart2.name != Universe.objectNameIfAlreadyDestroyed && thingPart2.guid == guid)
			{
				thingPart = thingPart2;
				break;
			}
		}
		return thingPart;
	}

	// Token: 0x06001750 RID: 5968 RVA: 0x000D0AE4 File Offset: 0x000CEEE4
	public float GetBehaviorScriptVariable(string variableName)
	{
		float num = 0f;
		float num2;
		if (this.behaviorScriptVariables.TryGetValue(variableName, out num2))
		{
			num = num2;
		}
		return num;
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x000D0B0D File Offset: 0x000CEF0D
	public void SetBehaviorScriptVariable(string variableName, float value)
	{
		this.behaviorScriptVariables[variableName] = value;
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x000D0B1C File Offset: 0x000CEF1C
	public void AutoUpdateAllVisibilityAndCollision()
	{
		bool flag = Managers.areaManager != null && Managers.areaManager.weAreEditorOfCurrentArea;
		bool flag2 = flag && Our.seeInvisibleAsEditor;
		bool flag3 = flag && Our.touchUncollidableAsEditor;
		this.UpdateAllVisibilityAndCollision(flag2, flag3);
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x000D0B70 File Offset: 0x000CEF70
	public void UpdateAllVisibilityAndCollision(bool forceVisible = false, bool forceCollidable = false)
	{
		Thing myRootThing = this.GetMyRootThing();
		if (!(myRootThing != null) || !(myRootThing.gameObject == CreationHelper.thingBeingEdited))
		{
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (thingPart.transform.parent == base.transform)
				{
					Renderer component = thingPart.GetComponent<Renderer>();
					if (component != null)
					{
						component.enabled = forceVisible || (!this.invisible && !thingPart.invisible);
					}
					Collider component2 = thingPart.GetComponent<Collider>();
					if (component2 != null)
					{
						component2.enabled = forceCollidable || ((!this.uncollidable || thingPart.isDedicatedCollider) && !thingPart.uncollidable && !this.suppressCollisions);
					}
				}
			}
		}
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x000D0C88 File Offset: 0x000CF088
	public void ApplyTurnCommandToProperties(string command)
	{
		if (command != null)
		{
			if (!(command == "on"))
			{
				if (!(command == "off"))
				{
					if (!(command == "visible"))
					{
						if (!(command == "invisible"))
						{
							if (!(command == "collidable"))
							{
								if (command == "uncollidable")
								{
									this.uncollidable = true;
								}
							}
							else
							{
								this.uncollidable = false;
							}
						}
						else
						{
							this.invisible = true;
						}
					}
					else
					{
						this.invisible = false;
					}
				}
				else
				{
					this.invisible = true;
					this.uncollidable = true;
				}
			}
			else
			{
				this.invisible = false;
				this.uncollidable = false;
			}
		}
		this.persistentAttributeChangingCommandWasApplied = true;
	}

	// Token: 0x06001755 RID: 5973 RVA: 0x000D0D60 File Offset: 0x000CF160
	public void SetPlacementAttribute(PlacementAttribute attribute, bool state)
	{
		switch (attribute)
		{
		case PlacementAttribute.Locked:
			this.isLocked = state;
			break;
		case PlacementAttribute.InvisibleToEditors:
			this.isInvisibleToEditors = state;
			break;
		case PlacementAttribute.SuppressScriptsAndStates:
			this.suppressScriptsAndStates = state;
			break;
		case PlacementAttribute.SuppressCollisions:
			this.suppressCollisions = state;
			break;
		case PlacementAttribute.SuppressLights:
			this.suppressLights = state;
			break;
		case PlacementAttribute.SuppressParticles:
			this.suppressParticles = state;
			break;
		case PlacementAttribute.SuppressHoldable:
			this.suppressHoldable = state;
			break;
		}
	}

	// Token: 0x04001525 RID: 5413
	public string givenName;

	// Token: 0x04001526 RID: 5414
	public string thrownId;

	// Token: 0x04001527 RID: 5415
	public Dictionary<string, string> includedNameIds = new Dictionary<string, string>();

	// Token: 0x04001528 RID: 5416
	public bool isLocked;

	// Token: 0x04001529 RID: 5417
	public bool isInvisibleToEditors;

	// Token: 0x0400152A RID: 5418
	public int version = 9;

	// Token: 0x0400152B RID: 5419
	public string description;

	// Token: 0x0400152C RID: 5420
	public bool isHighlighted;

	// Token: 0x0400152D RID: 5421
	public ThingPart subThingMasterPart;

	// Token: 0x0400152E RID: 5422
	public string includedSubThingNameOverride;

	// Token: 0x0400152F RID: 5423
	public Dictionary<string, float> behaviorScriptVariables = new Dictionary<string, float>();

	// Token: 0x04001530 RID: 5424
	public bool isClonable;

	// Token: 0x04001531 RID: 5425
	public bool isHoldable;

	// Token: 0x04001532 RID: 5426
	public bool remainsHeld;

	// Token: 0x04001533 RID: 5427
	public bool isClimbable;

	// Token: 0x04001534 RID: 5428
	public bool isPassable;

	// Token: 0x04001535 RID: 5429
	public bool isUnwalkable;

	// Token: 0x04001536 RID: 5430
	public bool doSnapAngles;

	// Token: 0x04001537 RID: 5431
	public bool doSoftSnapAngles;

	// Token: 0x04001538 RID: 5432
	public bool doLockAngles;

	// Token: 0x04001539 RID: 5433
	public bool isBouncy;

	// Token: 0x0400153A RID: 5434
	public bool doShowDirection;

	// Token: 0x0400153B RID: 5435
	public bool keepPreciseCollider;

	// Token: 0x0400153C RID: 5436
	public bool doesFloat;

	// Token: 0x0400153D RID: 5437
	public bool doesShatter;

	// Token: 0x0400153E RID: 5438
	public bool isSticky;

	// Token: 0x0400153F RID: 5439
	public bool isSlidy;

	// Token: 0x04001540 RID: 5440
	public bool doSnapPosition;

	// Token: 0x04001541 RID: 5441
	public bool doLockPosition;

	// Token: 0x04001542 RID: 5442
	public bool amplifySpeech;

	// Token: 0x04001543 RID: 5443
	public bool benefitsFromShowingAtDistance;

	// Token: 0x04001544 RID: 5444
	public bool scaleAllParts;

	// Token: 0x04001545 RID: 5445
	public bool doAlwaysMergeParts;

	// Token: 0x04001546 RID: 5446
	public bool addBodyWhenAttached;

	// Token: 0x04001547 RID: 5447
	public bool addBodyWhenAttachedNonClearing;

	// Token: 0x04001548 RID: 5448
	public bool hasSurroundSound;

	// Token: 0x04001549 RID: 5449
	public bool canGetEventsWhenStateChanging;

	// Token: 0x0400154A RID: 5450
	public bool replacesHandsWhenAttached;

	// Token: 0x0400154B RID: 5451
	public bool mergeParticleSystems;

	// Token: 0x0400154C RID: 5452
	public bool isSittable;

	// Token: 0x0400154D RID: 5453
	public bool smallEditMovements;

	// Token: 0x0400154E RID: 5454
	public bool scaleEachPartUniformly;

	// Token: 0x0400154F RID: 5455
	public bool snapAllPartsToGrid;

	// Token: 0x04001550 RID: 5456
	public bool invisibleToUsWhenAttached;

	// Token: 0x04001551 RID: 5457
	public bool replaceInstancesInArea;

	// Token: 0x04001552 RID: 5458
	public bool avoidCastShadow;

	// Token: 0x04001553 RID: 5459
	public bool avoidReceiveShadow;

	// Token: 0x04001554 RID: 5460
	public bool omitAutoSounds;

	// Token: 0x04001555 RID: 5461
	public bool omitAutoHapticFeedback;

	// Token: 0x04001556 RID: 5462
	public bool keepSizeInInventory;

	// Token: 0x04001557 RID: 5463
	public bool activeEvenInInventory;

	// Token: 0x04001558 RID: 5464
	public bool stricterPhysicsSyncing;

	// Token: 0x04001559 RID: 5465
	public bool removeOriginalWhenGrabbed;

	// Token: 0x0400155A RID: 5466
	public bool persistWhenThrownOrEmitted;

	// Token: 0x0400155B RID: 5467
	public bool movableByEveryone;

	// Token: 0x0400155C RID: 5468
	public bool isNeverClonable;

	// Token: 0x0400155D RID: 5469
	public bool floatsOnLiquid;

	// Token: 0x0400155E RID: 5470
	public bool invisibleToDesktopCamera;

	// Token: 0x0400155F RID: 5471
	public bool personalExperience;

	// Token: 0x04001560 RID: 5472
	public bool invisible;

	// Token: 0x04001561 RID: 5473
	public bool uncollidable;

	// Token: 0x04001562 RID: 5474
	public bool persistentAttributeChangingCommandWasApplied;

	// Token: 0x04001563 RID: 5475
	public bool suppressScriptsAndStates;

	// Token: 0x04001564 RID: 5476
	public bool suppressCollisions;

	// Token: 0x04001565 RID: 5477
	public bool suppressLights;

	// Token: 0x04001566 RID: 5478
	public bool suppressParticles;

	// Token: 0x04001567 RID: 5479
	public bool suppressHoldable;

	// Token: 0x04001568 RID: 5480
	public bool suppressShowAtDistance;

	// Token: 0x04001569 RID: 5481
	public bool hideEffectShapes_deprecated;

	// Token: 0x0400156A RID: 5482
	public bool autoAddReflectionPartsSideways;

	// Token: 0x0400156B RID: 5483
	public bool autoAddReflectionPartsVertical;

	// Token: 0x0400156C RID: 5484
	public bool autoAddReflectionPartsDepth;

	// Token: 0x0400156D RID: 5485
	public bool temporarilyBenefitsFromShowingAtDistance;

	// Token: 0x0400156E RID: 5486
	public bool isThrownOrEmitted;

	// Token: 0x04001570 RID: 5488
	public bool isTemporarilyInactiveAsControllableIsSpawned;

	// Token: 0x04001571 RID: 5489
	public GameObject inWorldSourceObject;

	// Token: 0x04001572 RID: 5490
	public bool isBig;

	// Token: 0x04001573 RID: 5491
	public bool isVeryBig;

	// Token: 0x04001574 RID: 5492
	public bool containsBehaviorScript;

	// Token: 0x04001575 RID: 5493
	public bool containsLight;

	// Token: 0x04001576 RID: 5494
	public bool containsParticleSystem;

	// Token: 0x04001577 RID: 5495
	public bool containsBaseLayerParticleSystem;

	// Token: 0x04001578 RID: 5496
	public bool containsPlacedSubThings;

	// Token: 0x04001579 RID: 5497
	public bool containsOnAnyListener;

	// Token: 0x0400157A RID: 5498
	public bool containsBehaviorScriptVariables;

	// Token: 0x0400157B RID: 5499
	public float? distanceToShow;

	// Token: 0x0400157C RID: 5500
	public bool containsInvisibleOrUncollidable;

	// Token: 0x0400157D RID: 5501
	public bool isInInventoryOrDialog;

	// Token: 0x0400157E RID: 5502
	public bool isInInventory;

	// Token: 0x0400157F RID: 5503
	public bool isGiftInDialog;

	// Token: 0x04001580 RID: 5504
	public string emittedByThingId;

	// Token: 0x04001581 RID: 5505
	public bool isHeldAsHoldable;

	// Token: 0x04001582 RID: 5506
	public bool isHeldAsHoldableByOurPerson;

	// Token: 0x04001583 RID: 5507
	public bool requiresWiderReach;

	// Token: 0x04001584 RID: 5508
	public float? mass;

	// Token: 0x04001585 RID: 5509
	public float? drag;

	// Token: 0x04001586 RID: 5510
	public float? angularDrag;

	// Token: 0x04001587 RID: 5511
	public BoolVector3 lockPhysicsPosition = new BoolVector3(false, false, false);

	// Token: 0x04001588 RID: 5512
	public BoolVector3 lockPhysicsRotation = new BoolVector3(false, false, false);

	// Token: 0x04001589 RID: 5513
	public Vector3? localTarget;

	// Token: 0x0400158A RID: 5514
	public string crossClientSubThingId;

	// Token: 0x0400158B RID: 5515
	private float lastStricterPhysicsSyncingInformSent = -1f;

	// Token: 0x0400158C RID: 5516
	private RotateThingSettings rotateTowardsSettings;

	// Token: 0x0400158D RID: 5517
	[SerializeField]
	private string _thingId;

	// Token: 0x0400158E RID: 5518
	[SerializeField]
	private string _placementId;

	// Token: 0x0400158F RID: 5519
	public bool alreadyDeleted;

	// Token: 0x04001590 RID: 5520
	public int thingPartCount;

	// Token: 0x04001591 RID: 5521
	public int allPartsImageCount;

	// Token: 0x04001592 RID: 5522
	public bool triggeredOnSomeoneNewInVicinity;

	// Token: 0x04001593 RID: 5523
	private bool handleRotateTowards;

	// Token: 0x04001594 RID: 5524
	private bool doRotateTowardsClosestPerson;

	// Token: 0x04001595 RID: 5525
	private bool doRotateTowardsSecondClosestPerson;

	// Token: 0x04001596 RID: 5526
	private bool doRotateTowardsTop;

	// Token: 0x04001597 RID: 5527
	private bool doRotateTowardsClosestEmptyHand;

	// Token: 0x04001598 RID: 5528
	private bool doRotateTowardsClosestEmptyHandWhileHeld;

	// Token: 0x04001599 RID: 5529
	private bool doRotateTowardsMainCamera;

	// Token: 0x0400159A RID: 5530
	private Transform mainCameraForRotatingTowards;

	// Token: 0x0400159B RID: 5531
	private string rotateTowardsClosestThingOfName;

	// Token: 0x0400159C RID: 5532
	private GameObject cachedClosestThingOfName;

	// Token: 0x0400159D RID: 5533
	private string cachedClosestThingName;

	// Token: 0x0400159E RID: 5534
	private GameObject personHeadToRotateTowards;

	// Token: 0x0400159F RID: 5535
	public LineRenderer directionLine;

	// Token: 0x040015A0 RID: 5536
	private LineRenderer directionUpLine;

	// Token: 0x040015A1 RID: 5537
	public float biggestSize;

	// Token: 0x040015A2 RID: 5538
	private Hand handHoldingMe;

	// Token: 0x040015A3 RID: 5539
	private bool isUsingRigidbody;

	// Token: 0x040015A4 RID: 5540
	private Transform parentHand;

	// Token: 0x040015A5 RID: 5541
	public Rigidbody rigidbody;

	// Token: 0x040015A6 RID: 5542
	private int lastUtcSecondsInRotateTowards = -1;

	// Token: 0x040015A7 RID: 5543
	private Vector3 originalPosition = Vector3.zero;

	// Token: 0x040015A8 RID: 5544
	private Quaternion originalRotation = Quaternion.identity;

	// Token: 0x040015AB RID: 5547
	private float velocityMagnitude;

	// Token: 0x040015AC RID: 5548
	private float angularVelocityMagnitude;

	// Token: 0x040015AD RID: 5549
	private Transform parentBeforePushBack;

	// Token: 0x040015AE RID: 5550
	private float infrequentHoldableEventsCheckDelay = 0.5f;

	// Token: 0x040015AF RID: 5551
	private float timeOfLastInfrequentHoldableEventsCheck = -1f;

	// Token: 0x040015B0 RID: 5552
	private float haltEventsUntilTime = -1f;

	// Token: 0x040015B1 RID: 5553
	public float destroyMeInTime = -1f;

	// Token: 0x040015B2 RID: 5554
	private float infrequentEventsCheckDelay = 0.25f;

	// Token: 0x040015B3 RID: 5555
	private float timeOfLastInfrequentEventsCheck = -1f;

	// Token: 0x040015B4 RID: 5556
	public Vector3 lastPositionForUndo = Vector3.zero;

	// Token: 0x040015B5 RID: 5557
	public Vector3 lastRotationForUndo = Vector3.zero;

	// Token: 0x040015B6 RID: 5558
	public float timeOfLastMemorizationForUndo = -1f;

	// Token: 0x040015B7 RID: 5559
	private EditModes editMode;

	// Token: 0x040015B8 RID: 5560
	private EditModes? previousEditMode;
}
