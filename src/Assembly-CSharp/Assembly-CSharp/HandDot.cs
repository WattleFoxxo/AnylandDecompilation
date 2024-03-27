using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x020000E3 RID: 227
public class HandDot : MonoBehaviour
{
	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000761 RID: 1889 RVA: 0x00024AFC File Offset: 0x00022EFC
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

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000762 RID: 1890 RVA: 0x00024B37 File Offset: 0x00022F37
	// (set) Token: 0x06000763 RID: 1891 RVA: 0x00024B3F File Offset: 0x00022F3F
	public Side side { get; private set; }

	// Token: 0x06000764 RID: 1892 RVA: 0x00024B48 File Offset: 0x00022F48
	private void Start()
	{
		this.trackedObj = base.transform.parent.GetComponent<SteamVR_TrackedObject>();
		this.side = ((!(base.transform.parent.name == "HandCoreLeft")) ? Side.Right : Side.Left);
		this.sideString = this.side.ToString();
		this.sideStringLower = this.sideString.ToLower();
		this.currentlyHeldTag = "CurrentlyHeld" + this.sideString;
		this.currentlyHeldOtherTag = "CurrentlyHeld" + ((this.side != Side.Left) ? "Left" : "Right");
		this.handCoreName = "HandCore" + this.sideString;
		this.armAttachmentPointName = "Arm" + this.sideString + "AttachmentPoint";
		this.otherDotScript = this.otherDot.GetComponent<HandDot>();
		this.isAuthoritativeSide = this.side == Side.Left;
		this.handScript = base.transform.parent.GetComponent<Hand>();
		this.otherHandScript = this.otherHand.GetComponent<Hand>();
		IEnumerator enumerator = base.transform.parent.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.name == "HandLeftAttachmentPoint" || transform.gameObject.name == "HandRightAttachmentPoint")
				{
					this.handForPushBack = transform.gameObject;
					break;
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

	// Token: 0x06000765 RID: 1893 RVA: 0x00024D18 File Offset: 0x00023118
	private bool EverythingIsReady()
	{
		return Managers.broadcastNetworkManager != null && Managers.broadcastNetworkManager.inRoom && Managers.areaManager != null && Managers.dialogManager != null;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00024D58 File Offset: 0x00023158
	private void Update()
	{
		if (!this.EverythingIsReady())
		{
			return;
		}
		this.deletedSomethingThisRound = false;
		if (this.GetPressDown(CrossDevice.button_grab) || this.GetPressDown(CrossDevice.button_grabTip))
		{
			float num = 0.005f;
			base.transform.localScale = new Vector3(num, num, num);
			if (Our.mode == EditModes.Area && this.currentlyHeldObject == null && Managers.areaManager.weAreEditorOfCurrentArea && Managers.dialogManager.CurrentNonStartDialogTypeIsOneOf(this.dialogTypesAllowingFarAwayHold))
			{
				this.EditHoldOrCloneFromFarAway();
			}
			else
			{
				Managers.soundManager.Play("click", base.transform, 0.1f, false, false);
				if (this.currentlyHeldObject == null && (Our.mode == EditModes.Thing || Our.mode == EditModes.Environment || Our.mode == EditModes.None))
				{
					this.positionEmptyClickStarted = base.transform.position;
					this.timeEmptyClickForMoveStarted = Time.time;
				}
				else
				{
					this.positionEmptyClickStarted = Vector3.zero;
					this.timeEmptyClickForMoveStarted = -1f;
				}
				if (this.handScript.isLegPuppeteering)
				{
					if (CrossDevice.type == global::DeviceType.Vive || this.GetPressDown(CrossDevice.button_grabTip))
					{
						Thing legThing = this.handScript.GetLegThing();
						if (legThing != null)
						{
							legThing.TriggerEventAsStateAuthority(StateListener.EventType.OnTriggered, string.Empty);
						}
					}
				}
				else if (this.holdableInHand != null)
				{
					Thing component = this.holdableInHand.GetComponent<Thing>();
					if (component != null)
					{
						if (this.ignoreNextHoldableTrigger)
						{
							this.ignoreNextHoldableTrigger = false;
						}
						else if ((CrossDevice.type == global::DeviceType.Vive || this.GetPressDown(CrossDevice.button_grabTip)) && !this.IsInJoystickScope())
						{
							component.TriggerEventAsStateAuthority(StateListener.EventType.OnTriggered, string.Empty);
						}
					}
				}
				if (!this.handScript.isLegPuppeteering && !this.otherHandScript.isLegPuppeteering && this.otherDotScript.GetPress(CrossDevice.button_grabTip))
				{
					Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "both hands triggered", true);
				}
				string text = ((!this.handScript.isLegPuppeteering) ? "hand" : "leg") + " triggered " + this.sideStringLower;
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, text, true);
			}
		}
		if (this.GetPress(CrossDevice.button_grab) || this.GetPress(CrossDevice.button_grabTip))
		{
			bool flag = this.positionEmptyClickStarted != Vector3.zero;
			bool flag2 = this.otherDotScript.positionEmptyClickStarted != Vector3.zero;
			if ((flag && flag2) || ((flag || flag2) && Managers.movableByEveryoneManager.HasGroup()))
			{
				this.TriggerHapticPulse(Universe.veryLowHapticPulse);
				if (this.isAuthoritativeSide || !flag || !flag2)
				{
					if (Our.mode == EditModes.None)
					{
						Managers.movableByEveryoneManager.HandleInteraction(this, this.otherDotScript);
					}
					else
					{
						Thing thing = ((!(CreationHelper.thingBeingEdited != null)) ? null : CreationHelper.thingBeingEdited.GetComponent<Thing>());
						if (thing != null && thing.scaleAllParts)
						{
							if (!thing.ContainsUnremovableCenter() && !thing.HasIncludedSubThings())
							{
								this.HandleScalingOfWholeThing(CreationHelper.thingBeingEdited.transform);
							}
						}
						else if (Our.lastTransformHandled != null)
						{
							ThingPart thingPart = null;
							if (Our.lastTransformHandled.parent != null)
							{
								thingPart = Our.lastTransformHandled.GetComponent<ThingPart>();
							}
							bool flag3 = Our.mode == EditModes.Environment || (thingPart != null && (thingPart.scalesUniformly || (thingPart.baseType == ThingPartBase.HighPolySphere && thingPart.changedVertices == null) || thing.scaleEachPartUniformly));
							bool flag4 = thingPart != null && thingPart.isText;
							if (thingPart == null || !thingPart.isUnremovableCenter)
							{
								this.HandleScalingOfLastTransformHandled(flag3, flag4);
							}
						}
					}
				}
			}
		}
		if (this.GetPressDown(CrossDevice.button_delete))
		{
			if (this.holdableInHand != null)
			{
				Thing component2 = this.holdableInHand.GetComponent<Thing>();
				if (component2 && component2.remainsHeld)
				{
					Managers.personManager.DoThrowThing(this.holdableInHand);
					this.holdableInHand = null;
					this.currentlyHeldObject = null;
				}
				else if (this.holdableInHand.name == "VertexMover")
				{
					global::UnityEngine.Object.Destroy(this.holdableInHand);
					this.holdableInHand = null;
					this.currentlyHeldObject = null;
					if (Managers.dialogManager.GetCurrentNonStartDialogType() == DialogType.VertexMover)
					{
						this.otherHandScript.SwitchToNewDialog(DialogType.ThingPart, string.Empty);
					}
				}
			}
			else
			{
				this.CheckForThingDeletionFromAfar();
				this.CheckForThingPartStateDeletionStart();
			}
			this.SetMySphereColor(new Color32(byte.MaxValue, 0, 0, byte.MaxValue));
			Managers.soundManager.Play("delete", base.transform, 0.1f, false, false);
			if (this.currentlyHeldObject == null && this.holdableInHand == null)
			{
				Managers.personManager.DoAdditionalSanityClearOfHandObjects(base.transform.parent);
			}
			if (this.stuckObjectThatIsColliding != null)
			{
				Thing component3 = this.stuckObjectThatIsColliding.GetComponent<Thing>();
				if (component3 != null)
				{
					component3.OnDeleteStuck(false);
				}
			}
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "deleting " + this.sideStringLower, true);
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "deleting", true);
		}
		if (CrossDevice.hasSeparateTriggerAndGrab && !CrossDevice.desktopMode && this.GetPressDown(CrossDevice.button_grab) && this.holdableInHand != null)
		{
			Thing component4 = this.holdableInHand.GetComponent<Thing>();
			if (component4 && component4.remainsHeld)
			{
				if (this.didLetGoOfGrabSinceHolding)
				{
					this.LetGoOfHoldableInHand();
				}
				else
				{
					this.didLetGoOfGrabSinceHolding = true;
				}
			}
			if (this.currentlyHeldObject == null && this.holdableInHand == null)
			{
				Managers.personManager.DoAdditionalSanityClearOfHandObjects(base.transform.parent);
			}
		}
		if (this.GetPressUp(CrossDevice.button_delete))
		{
			this.ResetMySphereColor();
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "deleting ends " + this.sideStringLower, true);
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "deleting ends", true);
		}
		if (this.GetPressUp(CrossDevice.button_grab) || this.GetPressUp(CrossDevice.button_grabTip))
		{
			if (this.handScript.isLegPuppeteering)
			{
				if (CrossDevice.type == global::DeviceType.Vive || this.GetPressDown(CrossDevice.button_grabTip))
				{
					Thing legThing2 = this.handScript.GetLegThing();
					if (legThing2 != null)
					{
						legThing2.TriggerEventAsStateAuthority(StateListener.EventType.OnUntriggered, string.Empty);
					}
				}
			}
			else if (this.holdableInHand != null)
			{
				Thing component5 = this.holdableInHand.GetComponent<Thing>();
				if (component5)
				{
					component5.TriggerEventAsStateAuthority(StateListener.EventType.OnUntriggered, string.Empty);
				}
			}
			string text2 = ((!this.handScript.isLegPuppeteering) ? "hand" : "leg") + " trigger_let_go " + this.sideStringLower;
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, text2, true);
			if (Our.mode == EditModes.Environment && Our.lastTransformHandled != null)
			{
				Managers.personManager.DoSetEnvironmentChanger(Our.lastTransformHandled.gameObject);
			}
			if (Our.lastTransformHandled != null && this.holdableInHand == null)
			{
				Our.lastTransformHandledStartScale = Our.lastTransformHandled.localScale;
				Our.lastTransformHandledStartPosition = Our.lastTransformHandled.position;
				Our.checkForOneSideExtrusionThisTurn = true;
				Our.oneSideExtrusionDirectionFound = null;
			}
			if (CreationHelper.thingBeingEdited != null)
			{
				Our.lastThingStartScale = CreationHelper.thingBeingEdited.transform.localScale;
				Our.lastThingStartPosition = CreationHelper.thingBeingEdited.transform.localPosition;
				this.NormalizeThingAndThingPartsScale(CreationHelper.thingBeingEdited.transform);
			}
			this.positionEmptyClickStarted = Vector3.zero;
			this.timeEmptyClickForMoveStarted = -1f;
			if (this.IsDominantSide())
			{
				Managers.movableByEveryoneManager.ResetGroup(this, this.otherDotScript);
			}
			float num2 = 0.01f;
			base.transform.localScale = new Vector3(num2, num2, num2);
			if (this.holdableInHand != null)
			{
				if (this.otherHandScript.currentDialogType == DialogType.Gifts && Our.mode == EditModes.None)
				{
					GiftsDialog giftsDialog = this.GetGiftsDialog();
					if (giftsDialog != null)
					{
						giftsDialog.HandleAddedThingFromInventory(this.holdableInHand);
					}
					Managers.personManager.DoClearFromHand(this.holdableInHand, null, false);
				}
				else if (this.holdableInHand.name == "Brush")
				{
					this.otherHandScript.SwitchToNewDialog(CreationHelper.dialogBeforeBrushWasPicked, string.Empty);
					CreationHelper.dialogBeforeBrushWasPicked = DialogType.None;
				}
				else if (!(this.holdableInHand.name == "VertexMover"))
				{
					if (this.holdableInHand.name == DialogType.Inventory.ToString())
					{
						Managers.inventoryManager.CloseDialog();
						this.holdableInHand = null;
						if (this.otherDotScript.currentlyHeldObject != null && Our.previousMode == EditModes.Body)
						{
							Managers.personManager.DoClearFromHand(this.otherDotScript.currentlyHeldObject, null, true);
						}
						if (this.otherDotScript.currentlyHeldObject != null && Our.previousMode != EditModes.Body)
						{
							Thing component6 = this.otherDotScript.currentlyHeldObject.GetComponent<Thing>();
							if (component6 != null)
							{
								Transform parent = component6.transform.parent;
								component6.transform.parent = null;
								component6.transform.localScale = Vector3.one;
								component6.transform.parent = parent;
							}
							if (component6 != null && !component6.isHoldable && Our.previousMode != EditModes.Area && Our.previousMode != EditModes.Thing && this.handScript.currentDialogType != DialogType.Gifts && this.handScript.currentDialogType != DialogType.Gifts)
							{
								Managers.personManager.DoThrowThing(component6.gameObject);
								this.holdableInHand = null;
								this.currentlyHeldObject = null;
							}
						}
						Our.SetPreviousMode();
					}
				}
				if (this.holdableInHand != null)
				{
					Thing component7 = this.holdableInHand.GetComponent<Thing>();
					if (Our.mode == EditModes.Inventory && this.holdableInHand.name != DialogType.Inventory.ToString())
					{
						if (component7 != null)
						{
							component7.EndPushBackIfNeededAsLetGo();
						}
						this.holdableInHand = null;
					}
					else if (component7 != null)
					{
						if (!component7.remainsHeld)
						{
							component7.EndPushBackIfNeededAsLetGo();
							Managers.personManager.DoThrowThing(this.holdableInHand);
							this.holdableInHand = null;
							this.currentlyHeldObject = null;
						}
					}
					else if (this.holdableInHand.name == DialogType.Inventory.ToString())
					{
						this.holdableInHand = null;
					}
					else if (this.holdableInHand.name != "VertexMover")
					{
						global::UnityEngine.Object.Destroy(this.holdableInHand);
					}
				}
			}
			bool flag5 = false;
			if (this.currentlyHeldObject != null && this.holdableInHand == null)
			{
				this.currentlyHeldObject.transform.parent = Managers.thingManager.placements.transform;
				if (this.currentlyHeldObject.name == DialogType.Keyboard.ToString())
				{
					this.currentlyHeldObject.transform.parent = null;
					this.currentlyHeldObject.tag = "Untagged";
					this.currentlyHeldObject = null;
				}
				else if (this.otherHandScript.currentDialogType == DialogType.Gifts && Our.mode == EditModes.None)
				{
					GiftsDialog giftsDialog2 = this.GetGiftsDialog();
					if (giftsDialog2 != null)
					{
						giftsDialog2.HandleAddedThingFromInventory(this.currentlyHeldObject);
					}
					this.currentlyHeldObject.transform.parent = null;
					this.currentlyHeldObject.tag = "Untagged";
					this.currentlyHeldObject = null;
				}
				else if (Our.lastTransformHandled != null && Our.lastTransformHandled.gameObject.CompareTag("ThingPart") && this.otherHandScript.currentDialogType != DialogType.Keyboard && this.handScript.currentDialogType != DialogType.Keyboard && (Our.mode == EditModes.Thing || (Our.mode == EditModes.Inventory && Managers.inventoryManager.IsInventoryModeButOutsideScopeCube())) && this.currentlyHeldObject.GetComponent<Thing>() != null)
				{
					this.AddCurrentlyHeldObjectAsIncludedSubThing();
				}
				else if (this.currentlyHeldObject.name != "Brush" && this.currentlyHeldObject.name != "VertexMover")
				{
					Our.SetLastTransformHandled(this.currentlyHeldObject.transform);
					if (Our.mode == EditModes.Environment)
					{
						this.currentlyHeldObject.tag = "EnvironmentChanger";
						this.currentlyHeldObject.transform.parent = this.environmentChangers.transform;
						this.currentlyHeldObject = null;
					}
					else if (Our.mode == EditModes.Area || Managers.inventoryManager.IsInventoryModeButOutsideScopeCube())
					{
						if ((Our.mode == EditModes.Area || Our.previousMode == EditModes.Area) && Managers.areaManager.weAreEditorOfCurrentArea)
						{
							KeyboardDialog keyboardDialog = this.GetKeyboardDialog();
							if (keyboardDialog != null)
							{
								flag5 = keyboardDialog.HandleAddedThingFromInventory(this.currentlyHeldObject);
							}
							if (!flag5)
							{
								GiftsDialog giftsDialog3 = this.GetGiftsDialog();
								if (giftsDialog3 != null)
								{
									flag5 = giftsDialog3.HandleAddedThingFromInventory(this.currentlyHeldObject);
								}
							}
							if (flag5)
							{
								Managers.personManager.DoClearFromHand(this.currentlyHeldObject, this.handScript.gameObject, false);
								this.currentlyHeldObject = null;
							}
							else
							{
								Thing thing2 = this.currentlyHeldObject.GetComponent<Thing>();
								SnapHelper.SnapAllNeeded(thing2, this.currentlyHeldObject, base.transform, this.myPickUpPosition, this.objectPickUpPosition, this.myPickUpAngles, this.objectPickUpAngles);
								Managers.personManager.DoPlaceThingFromHand(base.transform.parent.gameObject, this.currentlyHeldObject);
								this.currentlyHeldObject = null;
								this.objectPickUpPosition = Vector3.zero;
								this.myPickUpPosition = Vector3.zero;
							}
						}
						else if (Our.mode == EditModes.Inventory)
						{
							if (Our.previousMode == EditModes.Body)
							{
								Thing thing2 = this.currentlyHeldObject.GetComponent<Thing>();
								this.AttachIfCollidesWithAttachmentPointSphere(this.currentlyHeldObject, thing2, true);
								this.currentlyHeldObject = null;
							}
							else
							{
								KeyboardDialog keyboardDialog = this.GetKeyboardDialog();
								if (keyboardDialog != null)
								{
									flag5 = keyboardDialog.HandleAddedThingFromInventory(this.currentlyHeldObject);
									if (flag5)
									{
										Managers.personManager.DoClearFromHand(this.currentlyHeldObject, this.handScript.gameObject, false);
										this.currentlyHeldObject = null;
									}
								}
								if (!flag5)
								{
									GiftsDialog giftsDialog4 = this.GetGiftsDialog();
									if (giftsDialog4 != null)
									{
										flag5 = giftsDialog4.HandleAddedThingFromInventory(this.currentlyHeldObject);
									}
								}
								if (!flag5)
								{
									Thing component8 = this.currentlyHeldObject.GetComponent<Thing>();
									if (component8 != null)
									{
										component8.EndPushBackIfNeededAsLetGo();
										this.currentlyHeldObject.transform.parent = base.transform.parent;
										Managers.personManager.DoThrowThing(this.currentlyHeldObject);
										this.holdableInHand = null;
										this.currentlyHeldObject = null;
									}
								}
							}
						}
					}
					else if (Our.mode == EditModes.Body)
					{
						Thing thing2 = this.currentlyHeldObject.GetComponent<Thing>();
						this.AttachIfCollidesWithAttachmentPointSphere(this.currentlyHeldObject, thing2, false);
						this.currentlyHeldObject = null;
					}
					else if (Our.mode == EditModes.Inventory)
					{
						if (Managers.inventoryManager.IsShowingNormalInventory())
						{
							Managers.personManager.DoClearFromHand(this.currentlyHeldObject, base.transform.parent.gameObject, true);
							Managers.inventoryManager.SaveThingPlacement(this.currentlyHeldObject);
						}
						else
						{
							Managers.personManager.DoClearFromHand(this.currentlyHeldObject, this.handScript.gameObject, false);
						}
						this.currentlyHeldObject = null;
						this.holdableInHand = null;
					}
					else if (Our.mode == EditModes.Thing)
					{
						if (this.currentlyHeldObject.GetComponent<Thing>() != null)
						{
							KeyboardDialog keyboardDialog = this.GetKeyboardDialog();
							if (keyboardDialog != null)
							{
								bool flag6 = keyboardDialog.HandleAddedThingFromInventory(this.currentlyHeldObject);
								if (flag6)
								{
									Managers.personManager.DoClearFromHand(this.currentlyHeldObject, this.handScript.gameObject, true);
								}
							}
							global::UnityEngine.Object.Destroy(this.currentlyHeldObject);
						}
						else
						{
							Thing thing2 = CreationHelper.thingBeingEdited.GetComponent<Thing>();
							ThingPart component9 = this.currentlyHeldObject.GetComponent<ThingPart>();
							if (thing2.ContainsUnremovableCenter() && component9.isUnremovableCenter)
							{
								Managers.soundManager.Play("no", base.transform, 1f, false, false);
								global::UnityEngine.Object.Destroy(this.currentlyHeldObject);
							}
							else if (CreationHelper.thingBeingEdited.transform.childCount >= 1000)
							{
								Managers.dialogManager.ShowInfo("Sorry, you can only add up to " + 1000 + " parts!", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
								Managers.soundManager.Play("no", base.transform, 1f, false, false);
								global::UnityEngine.Object.Destroy(this.currentlyHeldObject);
							}
							else
							{
								SnapHelper.SnapAllNeeded(thing2, this.currentlyHeldObject, base.transform, this.myPickUpPosition, this.objectPickUpPosition, this.myPickUpAngles, this.objectPickUpAngles);
								this.currentlyHeldObject.name = Misc.RemoveCloneFromName(this.currentlyHeldObject.name);
								this.currentlyHeldObject.tag = "ThingPart";
								if (component9 != null && !this.GetThingHasThingParts(CreationHelper.thingBeingEdited) && component9.states.Count == 1)
								{
									if (thing2.autoAddReflectionPartsSideways)
									{
										SnapHelper.PositionSnappedInFrontOfUs(CreationHelper.thingBeingEdited.transform, this.currentlyHeldObject.transform);
									}
									else if (CreationHelper.thingBeingEdited.transform.position == Vector3.zero || (!thing2.autoAddReflectionPartsVertical && !thing2.autoAddReflectionPartsDepth))
									{
										CreationHelper.thingBeingEdited.transform.position = this.currentlyHeldObject.transform.localPosition;
										CreationHelper.thingBeingEdited.transform.rotation = this.currentlyHeldObject.transform.localRotation;
									}
									thing2.MemorizeOriginalTransform(false);
								}
								Managers.achievementManager.RegisterAchievement(Achievement.DraggedBaseShape);
								this.currentlyHeldObject.transform.parent = CreationHelper.thingBeingEdited.transform;
								SnapHelper.SnapToAngleLockerIfNeeded(component9);
								SnapHelper.SnapToPositionLockerIfNeeded(component9);
								this.ClearPickUpPosition();
								if (component9 != null)
								{
									component9.SetStatePropertiesByTransform(false);
									this.MoveThingPartStatePositionsInRelationToPickupPosition(component9);
									this.UpdateThingPartUndoButtonIfNeeded();
								}
								this.ReCenterThingIfIsUnremovableCenter(component9);
								Our.SetLastTransformHandled(this.currentlyHeldObject.transform);
								this.SetThingPartReflectionBasedOnThingSettings(thing2, component9);
								this.OpenKeyboardIfDroppedObjectIsDefaultText(component9);
								this.ClearMyIncludedSubThings();
							}
						}
						this.currentlyHeldObject = null;
					}
					else if (Our.mode == EditModes.None)
					{
						Thing component10 = this.currentlyHeldObject.GetComponent<Thing>();
						if (component10 != null && component10.movableByEveryone)
						{
							this.currentlyHeldObject.tag = "Thing";
							this.currentlyHeldObject = null;
							Managers.personManager.DoPlaceAsMovableByEveryone(component10, this.side);
						}
					}
				}
				Managers.soundManager.Play("putDown", base.transform, 0.35f, false, false);
			}
		}
		if (this.hasJustPickedUp)
		{
			this.TriggerHapticPulse(Universe.mediumHapticPulse);
		}
		this.HandleFarAwayThingPartMoving();
		this.HandlePushBackReturnToNormal();
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0002627C File Offset: 0x0002467C
	private void HandleFarAwayThingPartMoving()
	{
		if (Our.mode == EditModes.Thing && this.timeEmptyClickForMoveStarted != -1f && this.positionEmptyClickStarted != Vector3.zero && Our.lastTransformHandled != null)
		{
			if (this.otherDotScript.positionEmptyClickStarted != Vector3.zero)
			{
				this.timeEmptyClickForMoveStarted = -1f;
				this.otherDotScript.timeEmptyClickForMoveStarted = -1f;
			}
			else
			{
				float num = Time.time - this.timeEmptyClickForMoveStarted;
				if (num >= 0.75f && this.BothHandsAreEmpty() && this.otherDotScript.positionEmptyClickStarted == Vector3.zero && Our.lastTransformHandled.gameObject.GetComponent<ThingPart>() != null && !this.ThingPartIsLockedForEditing(Our.lastTransformHandled.gameObject))
				{
					this.timeEmptyClickForMoveStarted = -1f;
					GameObject gameObject = Our.lastTransformHandled.gameObject;
					gameObject.transform.parent = base.transform.parent;
					gameObject.tag = this.currentlyHeldTag;
					this.currentlyHeldObject = gameObject;
					this.StorePickUpPosition(gameObject);
					Managers.soundManager.Play("pickUp", base.transform, 0.3f, false, false);
				}
			}
		}
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x000263D4 File Offset: 0x000247D4
	private bool BothHandsAreEmpty()
	{
		return this.holdableInHand == null && this.currentlyHeldObject == null && this.otherDotScript.holdableInHand == null && this.otherDotScript.currentlyHeldObject == null;
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0002642D File Offset: 0x0002482D
	public bool IsDominantSide()
	{
		return Misc.TopographyIdToSide(Our.lastPreferentialHandSide) != this.side;
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x00026444 File Offset: 0x00024844
	private void SetThingPartReflectionBasedOnThingSettings(Thing thing, ThingPart thingPart)
	{
		bool flag = Managers.thingManager.ThingPartBaseSupportsReflectionPart(thingPart.baseType);
		bool flag2 = flag || Managers.thingManager.ThingPartBaseSupportsLimitedReflectionPart(thingPart.baseType);
		if (flag || flag2)
		{
			bool flag3 = false;
			if (thing.autoAddReflectionPartsSideways && !thingPart.hasReflectionPartSideways)
			{
				thingPart.hasReflectionPartSideways = true;
				flag3 = true;
			}
			if (thing.autoAddReflectionPartsVertical && !thingPart.hasReflectionPartVertical)
			{
				thingPart.hasReflectionPartVertical = true;
				flag3 = true;
			}
			if (thing.autoAddReflectionPartsDepth && !thingPart.hasReflectionPartDepth && flag)
			{
				thingPart.hasReflectionPartDepth = true;
				flag3 = true;
			}
			if (flag3)
			{
				thingPart.RemoveMyReflectionPartsIfNeeded();
				thingPart.CreateMyReflectionPartsIfNeeded(null);
			}
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x00026500 File Offset: 0x00024900
	private void AddCurrentlyHeldObjectAsIncludedSubThing()
	{
		CreationHelper.thingPartWhoseStatesAreEdited = Our.lastTransformHandled.gameObject;
		Thing component = CreationHelper.thingBeingEdited.GetComponent<Thing>();
		SnapHelper.SnapAllNeeded(component, this.currentlyHeldObject, base.transform, this.myPickUpPosition, this.objectPickUpPosition, this.myPickUpAngles, this.objectPickUpAngles);
		ThingPart component2 = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
		if (!component2.isLocked)
		{
			component2.ResetAndStopStateAsCurrentlyEditing();
			Thing component3 = this.currentlyHeldObject.GetComponent<Thing>();
			if (component3 != null && component2 != null)
			{
				component2.AddThingAsIncludedSubThing(component3);
			}
			Managers.dialogManager.SwitchToNewDialog(DialogType.IncludedSubThings, null, string.Empty);
		}
		else
		{
			Managers.soundManager.Play("no", base.transform, 0.3f, false, false);
		}
		Managers.personManager.DoClearFromHand(this.currentlyHeldObject, this.handScript.gameObject, false);
		this.currentlyHeldObject = null;
		this.holdableInHand = null;
		this.ClearPickUpPosition();
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x000265FC File Offset: 0x000249FC
	private void ClearMyIncludedSubThings()
	{
		IEnumerator enumerator = base.transform.parent.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.CompareTag("IncludedSubThings"))
				{
					global::UnityEngine.Object.Destroy(transform.gameObject);
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

	// Token: 0x0600076D RID: 1901 RVA: 0x0002667C File Offset: 0x00024A7C
	public void LetGoOfHoldableInHand()
	{
		if (this.holdableInHand != null)
		{
			Thing component = this.holdableInHand.GetComponent<Thing>();
			if (component != null)
			{
				component.EndPushBackIfNeededAsLetGo();
			}
			Managers.personManager.DoThrowThing(this.holdableInHand);
			this.holdableInHand = null;
			this.currentlyHeldObject = null;
			this.didLetGoOfGrabSinceHolding = false;
		}
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x000266E0 File Offset: 0x00024AE0
	private bool IsInJoystickScope()
	{
		bool flag = false;
		if (this.otherHandScript.currentDialogType == DialogType.Joystick)
		{
			JoystickDialog component = this.otherHandScript.currentDialog.GetComponent<JoystickDialog>();
			flag = component != null && component.JoystickHandIsInScope();
		}
		return flag;
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0002672C File Offset: 0x00024B2C
	private void CheckForThingDeletionFromAfar()
	{
		if (this.handScript.currentDialogType == DialogType.Thing || this.otherHandScript.currentDialogType == DialogType.Thing)
		{
			GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
			if (currentNonStartDialog != null)
			{
				ThingDialog component = currentNonStartDialog.GetComponent<ThingDialog>();
				if (component != null)
				{
					component.ShowDeleteButtonIfNeeded();
				}
			}
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x00026788 File Offset: 0x00024B88
	private void CheckForThingPartStateDeletionStart()
	{
		if (this.handScript.currentDialogType == DialogType.ThingPart || this.otherHandScript.currentDialogType == DialogType.ThingPart)
		{
			GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
			if (currentNonStartDialog != null)
			{
				ThingPartDialog component = currentNonStartDialog.GetComponent<ThingPartDialog>();
				if (component != null)
				{
					component.ShowStateDeleteButtonIfNeeded();
				}
			}
		}
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x000267E4 File Offset: 0x00024BE4
	private void UpdateThingPartUndoButtonIfNeeded()
	{
		this.UpdateThingPartUndoButtonOfThisHandIfNeeded(this.handScript);
		this.UpdateThingPartUndoButtonOfThisHandIfNeeded(this.otherHandScript);
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00026800 File Offset: 0x00024C00
	private void UpdateThingPartUndoButtonOfThisHandIfNeeded(Hand thisHand)
	{
		if (thisHand.currentDialogType == DialogType.ThingPart)
		{
			ThingPartDialog component = thisHand.currentDialog.GetComponent<ThingPartDialog>();
			if (component != null)
			{
				component.UpdateUndoButton();
			}
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00026838 File Offset: 0x00024C38
	private bool IsInIncludedSubThingsEditModeForThis(GameObject thisObject)
	{
		bool flag = false;
		if (CreationHelper.thingBeingEdited != null)
		{
			bool flag2 = this.IncludedSubThingsDialogIsOpen();
			bool flag3 = false;
			if (flag2)
			{
				Transform transform = thisObject.transform;
				while (transform != null)
				{
					if (transform.gameObject == CreationHelper.thingBeingEdited)
					{
						flag3 = true;
						break;
					}
					transform = transform.parent;
				}
			}
			flag = flag2 && flag3;
		}
		return flag;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x000268AD File Offset: 0x00024CAD
	private bool IncludedSubThingsDialogIsOpen()
	{
		return this.handScript.currentDialogType == DialogType.IncludedSubThings || this.otherHandScript.currentDialogType == DialogType.IncludedSubThings;
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x000268D4 File Offset: 0x00024CD4
	private void EditHoldOrCloneFromFarAway()
	{
		GameObject gameObject = null;
		DialogType currentNonStartDialogType = Managers.dialogManager.GetCurrentNonStartDialogType();
		if (currentNonStartDialogType != DialogType.Thing)
		{
			if (currentNonStartDialogType == DialogType.Browser)
			{
				GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
				BrowserDialog component = currentNonStartDialog.GetComponent<BrowserDialog>();
				gameObject = component.browser.transform.parent.gameObject;
			}
		}
		else
		{
			GameObject currentNonStartDialog2 = Our.GetCurrentNonStartDialog();
			ThingDialog component2 = currentNonStartDialog2.GetComponent<ThingDialog>();
			component2.ResolveToRootThing();
			if (component2.canMoveFromFarAway)
			{
				gameObject = component2.thingObject;
			}
		}
		if (gameObject != null && !this.ThingIsLockedForEditing(gameObject) && this.IsUnThrownOrEmittedThing(gameObject.transform))
		{
			if (this.otherDotScript.currentlyHeldObject == gameObject)
			{
				this.StorePickUpPosition(gameObject);
				this.currentlyHeldObject = Managers.personManager.DoCloneToHand(base.transform.parent.gameObject, gameObject, false);
				if (this.handScript.currentDialogType != DialogType.Start)
				{
					this.handScript.SwitchToNewDialog(DialogType.Start, string.Empty);
				}
				this.otherHandScript.lastContextInfoHit = this.currentlyHeldObject;
				this.otherHandScript.SwitchToNewDialog(DialogType.Thing, string.Empty);
			}
			else
			{
				this.currentlyHeldObject = gameObject;
				this.StorePickUpPosition(this.currentlyHeldObject);
				Managers.personManager.DoEditHold(base.transform.parent.gameObject, this.currentlyHeldObject);
			}
		}
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00026A44 File Offset: 0x00024E44
	private void OpenKeyboardIfDroppedObjectIsDefaultText(ThingPart thingPart)
	{
		if (thingPart != null && thingPart.isText)
		{
			string text3 = thingPart.GetComponent<TextMesh>().text;
			if (text3 == "ABC")
			{
				CreationHelper.thingPartWhoseStatesAreEdited = this.currentlyHeldObject;
				this.otherHandScript.lastContextInfoHit = this.currentlyHeldObject;
				if (text3 == "ABC")
				{
					text3 = string.Empty;
				}
				DialogManager dialogManager = Managers.dialogManager;
				Action<string> action = delegate(string text)
				{
					if (thingPart == null)
					{
						this.otherHandScript.SwitchToNewDialog(DialogType.Create, string.Empty);
					}
					else
					{
						if (!string.IsNullOrEmpty(text))
						{
							thingPart.SetOriginalText(text);
						}
						this.otherHandScript.SwitchToNewDialog(DialogType.ThingPart, string.Empty);
					}
				};
				Hand hand = this.otherHandScript;
				string text2 = text3;
				dialogManager.GetInput(action, "editCreationText", text2, 10000, string.Empty, true, true, true, true, 1f, false, false, hand, false);
			}
		}
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00026B1C File Offset: 0x00024F1C
	private KeyboardDialog GetKeyboardDialog()
	{
		KeyboardDialog keyboardDialog = null;
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null)
		{
			keyboardDialog = currentNonStartDialog.GetComponent<KeyboardDialog>();
		}
		return keyboardDialog;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00026B48 File Offset: 0x00024F48
	private GiftsDialog GetGiftsDialog()
	{
		GiftsDialog giftsDialog = null;
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null)
		{
			giftsDialog = currentNonStartDialog.GetComponent<GiftsDialog>();
		}
		return giftsDialog;
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00026B74 File Offset: 0x00024F74
	private void SetMySphereColor(Color color)
	{
		Renderer component = base.gameObject.GetComponent<Renderer>();
		Material material = component.material;
		material.SetColor("_EmissionColor", color);
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00026BA0 File Offset: 0x00024FA0
	private void ResetMySphereColor()
	{
		this.SetMySphereColor(Color.white);
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00026BB0 File Offset: 0x00024FB0
	private void HandlePushBackReturnToNormal()
	{
		if (!this.isInPushBackMode)
		{
			float num = 0.5f;
			this.handForPushBack.transform.localPosition = Vector3.Slerp(this.handForPushBack.transform.localPosition, Vector3.zero, num);
			this.handForPushBack.transform.localRotation = Quaternion.Slerp(this.handForPushBack.transform.localRotation, Quaternion.identity, num);
		}
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00026C24 File Offset: 0x00025024
	private void AttachIfCollidesWithAttachmentPointSphere(GameObject thing, Thing thingScript, bool syncIfDestroying = false)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("AttachmentPointSphere");
		GameObject gameObject = null;
		foreach (GameObject gameObject2 in array)
		{
			AttachmentPointSphere component = gameObject2.GetComponent<AttachmentPointSphere>();
			if (component.collidingThing == thing)
			{
				Transform parent = gameObject2.transform.parent;
				if (!Misc.HasChildWithTag(parent, "Attachment"))
				{
					gameObject = parent.gameObject;
					break;
				}
			}
		}
		if (gameObject == null)
		{
			float? num = null;
			foreach (GameObject gameObject3 in array)
			{
				Transform parent2 = gameObject3.transform.parent;
				if (!Misc.HasChildWithTag(parent2, "Attachment"))
				{
					float num2 = Vector3.Distance(thing.transform.position, gameObject3.transform.position);
					if ((num == null || num2 < num) && num2 < 0.25f)
					{
						num = new float?(num2);
						gameObject = parent2.gameObject;
					}
				}
			}
		}
		if (gameObject != null)
		{
			Managers.personManager.DoAttachThing(gameObject, thing, true);
			if ((thingScript.addBodyWhenAttached || thingScript.addBodyWhenAttachedNonClearing) && gameObject.name == "HeadAttachmentPoint")
			{
				base.StartCoroutine(Managers.thingManager.SetOurCurrentBodyAttachmentsByThing(ThingRequestContext.BodyAttachmentViaHand, thingScript.thingId, thingScript.addBodyWhenAttached, null));
			}
		}
		else
		{
			if (syncIfDestroying)
			{
				Managers.personManager.DoClearFromHand(thing, this.handScript.gameObject, false);
			}
			global::UnityEngine.Object.Destroy(thing);
		}
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00026DF4 File Offset: 0x000251F4
	private bool GetThingHasThingParts(GameObject thing)
	{
		bool flag = false;
		IEnumerator enumerator = thing.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.CompareTag("ThingPart"))
				{
					flag = true;
					break;
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
		return flag;
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00026E74 File Offset: 0x00025274
	public void DoSmallEditMovementOnly(GameObject thisObject)
	{
		if (this.objectPickUpPosition != Vector3.zero && this.myPickUpPosition != Vector3.zero)
		{
			Vector3 vector = this.objectPickUpPosition - this.myPickUpPosition;
			thisObject.transform.position = Vector3.Lerp(this.objectPickUpPosition, base.transform.position + vector, 0.25f);
			thisObject.transform.eulerAngles = this.objectPickUpAngles;
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00026EFA File Offset: 0x000252FA
	private void ClearPickUpPosition()
	{
		this.myPickUpPosition = Vector3.zero;
		this.objectPickUpPosition = Vector3.zero;
		this.myPickUpAngles = Vector3.zero;
		this.objectPickUpAngles = Vector3.zero;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00026F28 File Offset: 0x00025328
	private void HandleScalingOfWholeThing(Transform thingTransform)
	{
		if (Our.lastThingStartScale == Vector3.zero)
		{
			Our.lastThingStartScale = Vector3.one;
		}
		Vector3 vector = thingTransform.localScale;
		Vector3 lastThingStartScale = Our.lastThingStartScale;
		Vector3 vector2 = this.positionEmptyClickStarted;
		Vector3 position = base.transform.position;
		Vector3 vector3 = this.otherDotScript.positionEmptyClickStarted;
		Vector3 position2 = this.otherDot.transform.position;
		float num = Vector3.Distance(vector2, vector3);
		float num2 = Vector3.Distance(position, position2);
		float num3 = num2 - num;
		vector = Misc.AddToAllVectorValues(lastThingStartScale, num3);
		Vector3 localScale = thingTransform.localScale;
		Vector3 localPosition = thingTransform.localPosition;
		thingTransform.localScale = vector;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00026FD0 File Offset: 0x000253D0
	private void NormalizeThingAndThingPartsScale(Transform thingTransform)
	{
		Vector3 localScale = thingTransform.localScale;
		if (localScale != Vector3.one)
		{
			List<ThingPart> list = new List<ThingPart>();
			List<int> list2 = new List<int>();
			Component[] componentsInChildren = thingTransform.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				list.Add(thingPart);
				list2.Add(thingPart.currentState);
			}
			for (int j = 0; j < 50; j++)
			{
				thingTransform.localScale = Vector3.one;
				foreach (ThingPart thingPart2 in list)
				{
					if (j < thingPart2.states.Count)
					{
						thingPart2.currentState = j;
						thingPart2.SetTransformPropertiesByState(true, false);
					}
				}
				thingTransform.localScale = localScale;
				foreach (ThingPart thingPart3 in list)
				{
					if (thingPart3.currentState == j)
					{
						thingPart3.transform.parent = null;
					}
				}
				thingTransform.localScale = Vector3.one;
				foreach (ThingPart thingPart4 in list)
				{
					if (thingPart4.currentState == j)
					{
						thingPart4.transform.parent = thingTransform;
						thingPart4.SetStatePropertiesByTransform(true);
						thingPart4.SetTransformPropertiesByState(true, false);
					}
				}
			}
			int num = 0;
			foreach (ThingPart thingPart5 in list)
			{
				thingPart5.currentState = list2[num++];
			}
			thingTransform.localScale = Vector3.one;
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00027210 File Offset: 0x00025610
	private void HandleScalingOfLastTransformHandled(bool scaleUniformly = false, bool isText = false)
	{
		ThingPart component = Our.lastTransformHandled.gameObject.GetComponent<ThingPart>();
		Vector3 vector = Our.lastTransformHandled.localScale;
		Vector3 lastTransformHandledStartScale = Our.lastTransformHandledStartScale;
		Vector3 vector2 = this.positionEmptyClickStarted;
		Vector3 vector3 = base.transform.position;
		Vector3 vector4 = this.otherDotScript.positionEmptyClickStarted;
		Vector3 vector5 = this.otherDot.transform.position;
		Vector3 eulerAngles = Quaternion.Inverse(Our.lastTransformHandled.rotation).eulerAngles;
		Vector3 vector6 = Vector3.Lerp(vector2, vector4, 0.5f);
		vector2 = Misc.RotatePointAroundPivot(vector2, vector6, eulerAngles);
		vector4 = Misc.RotatePointAroundPivot(vector4, vector6, eulerAngles);
		vector6 = Vector3.Lerp(vector3, vector5, 0.5f);
		vector3 = Misc.RotatePointAroundPivot(vector3, vector6, eulerAngles);
		vector5 = Misc.RotatePointAroundPivot(vector5, vector6, eulerAngles);
		float num = Vector3.Distance(vector2, vector4);
		float num2 = Vector3.Distance(vector3, vector5);
		float num3 = num2 - num;
		if (isText)
		{
			num3 *= 0.1f;
		}
		if (scaleUniformly || isText)
		{
			vector = Misc.AddToAllVectorValues(lastTransformHandledStartScale, num3);
		}
		else
		{
			float num4 = Mathf.Abs(vector2.x - vector3.x) + Mathf.Abs(vector4.x - vector5.x);
			float num5 = Mathf.Abs(vector2.y - vector3.y) + Mathf.Abs(vector4.y - vector5.y);
			float num6 = Mathf.Abs(vector2.z - vector3.z) + Mathf.Abs(vector4.z - vector5.z);
			float num7 = Mathf.Abs(vector2.x - vector4.x);
			float num8 = Mathf.Abs(vector2.y - vector4.y);
			float num9 = Mathf.Abs(vector2.z - vector4.z);
			num4 += num7 * 0.25f;
			num5 += num8 * 0.25f;
			num6 += num9 * 0.25f;
			if (num4 >= num5 && num4 >= num6)
			{
				if (lastTransformHandledStartScale.x >= 3f && lastTransformHandledStartScale.x <= 2500f)
				{
					num3 *= vector.x;
				}
				vector.x = lastTransformHandledStartScale.x + num3;
			}
			else if (num5 >= num4 && num5 >= num6)
			{
				if (lastTransformHandledStartScale.y >= 3f && lastTransformHandledStartScale.y <= 2500f)
				{
					num3 *= vector.y;
				}
				vector.y = lastTransformHandledStartScale.y + num3;
			}
			else if (num6 >= num4 && num6 >= num5)
			{
				if (lastTransformHandledStartScale.z >= 3f && lastTransformHandledStartScale.z <= 2500f)
				{
					num3 *= vector.z;
				}
				vector.z = lastTransformHandledStartScale.z + num3;
			}
		}
		float num10 = 0.001f;
		float num11 = 2500f;
		if (isText)
		{
			num10 = 0.001f;
			num11 = 0.05f;
		}
		else if (Our.mode == EditModes.Environment)
		{
			num10 = 0.03f;
			num11 = 0.15f;
			string name = Our.lastTransformHandled.gameObject.name;
			if (name != null)
			{
				if (!(name == "fog"))
				{
					if (name == "clouds")
					{
						num10 = 0.01f;
						num11 = 0.075f;
					}
				}
				else
				{
					num10 = 0.01f;
				}
			}
		}
		vector.x = Mathf.Clamp(vector.x, num10, num11);
		vector.y = Mathf.Clamp(vector.y, num10, num11);
		vector.z = Mathf.Clamp(vector.z, num10, num11);
		Our.lastTransformHandled.localScale = vector;
		if (component != null)
		{
			if (scaleUniformly && component.ImageNeedsProportionalScaling())
			{
				component.SetScaleBasedOnImage();
			}
			component.SetStatePropertiesByTransform(false);
			if (component.materialType == MaterialTypes.PointLight || component.materialType == MaterialTypes.SpotLight)
			{
				component.UpdateMaterial();
			}
		}
		Managers.achievementManager.RegisterAchievement(Achievement.ResizedThingPart);
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0002764C File Offset: 0x00025A4C
	private void HandleOneSideExtrusionScaling()
	{
		if (Our.checkForOneSideExtrusionThisTurn)
		{
			float num = Vector3.Distance(this.positionEmptyClickStarted, base.transform.position);
			float num2 = Vector3.Distance(this.otherDotScript.positionEmptyClickStarted, this.otherDot.transform.position);
			if (num > 0.05f && num2 > 0.05f)
			{
				Vector3? oneSideExtrusionDirectionFound = Our.oneSideExtrusionDirectionFound;
				if (oneSideExtrusionDirectionFound == null)
				{
					Our.checkForOneSideExtrusionThisTurn = false;
					Our.oneSideExtrusionDirectionFound = null;
					Our.lastTransformHandled.position = Our.lastTransformHandledStartPosition;
					return;
				}
			}
			if (num < 0.1f && num2 < 0.1f)
			{
				Vector3? oneSideExtrusionDirectionFound2 = Our.oneSideExtrusionDirectionFound;
				if (oneSideExtrusionDirectionFound2 == null)
				{
					Our.lastTransformHandled.position = Our.lastTransformHandledStartPosition;
					goto IL_2D4;
				}
			}
			Vector3 zero = Vector3.zero;
			zero.x = Our.lastTransformHandled.localScale.x - Our.lastTransformHandledStartScale.x;
			zero.y = Our.lastTransformHandled.localScale.y - Our.lastTransformHandledStartScale.y;
			zero.z = Our.lastTransformHandled.localScale.z - Our.lastTransformHandledStartScale.z;
			Vector3 vector = new Vector3(Mathf.Abs(zero.x), Mathf.Abs(zero.y), Mathf.Abs(zero.z));
			float num3;
			if (vector.x > vector.y && vector.x > vector.z)
			{
				num3 = zero.x;
			}
			else if (vector.y > vector.x && vector.y > vector.z)
			{
				num3 = zero.y;
			}
			else
			{
				num3 = zero.z;
			}
			Vector3 vector2 = Vector3.Lerp(this.positionEmptyClickStarted, this.otherDotScript.positionEmptyClickStarted, 0.5f);
			Vector3 vector3 = Vector3.Lerp(base.transform.position, this.otherDot.transform.position, 0.5f);
			Vector3 vector4 = vector3 - vector2;
			Vector3 vector5 = this.SnapDirectionToTransformRotation(Our.lastTransformHandled, vector4.normalized);
			Vector3? oneSideExtrusionDirectionFound3 = Our.oneSideExtrusionDirectionFound;
			if (oneSideExtrusionDirectionFound3 == null)
			{
				Our.oneSideExtrusionDirectionFound = new Vector3?(vector5);
			}
			else
			{
				Vector3 vector6 = vector5;
				Vector3? oneSideExtrusionDirectionFound4 = Our.oneSideExtrusionDirectionFound;
				if (vector6 != oneSideExtrusionDirectionFound4.Value)
				{
					Our.oneSideExtrusionDirectionFound = null;
					Our.checkForOneSideExtrusionThisTurn = false;
					Our.lastTransformHandled.position = Our.lastTransformHandledStartPosition;
				}
			}
			if (Our.checkForOneSideExtrusionThisTurn)
			{
				Our.lastTransformHandled.position = Our.lastTransformHandledStartPosition + vector5 * Mathf.Abs(num3) * 0.5f;
			}
			IL_2D4:;
		}
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00027958 File Offset: 0x00025D58
	private Vector3 SnapDirectionToTransformRotation(Transform thisTransform, Vector3 direction)
	{
		Vector3 vector = Vector3.zero;
		Vector3[] array = new Vector3[]
		{
			thisTransform.forward,
			-thisTransform.forward,
			thisTransform.right,
			-thisTransform.right,
			thisTransform.up,
			-thisTransform.up
		};
		float num = -1f;
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector2 = array[i];
			float num2 = Vector3.Distance(direction, vector2);
			if (i == 0 || num2 < num)
			{
				num = num2;
				vector = vector2;
			}
		}
		return vector;
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00027A34 File Offset: 0x00025E34
	private void StopPickUpHapticPulse()
	{
		this.hasJustPickedUp = false;
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00027A40 File Offset: 0x00025E40
	private void StartShowTouchPushBack()
	{
		if (this.handForPushBack != null)
		{
			this.handForPushBack.transform.parent = null;
			this.isInPushBackMode = true;
			base.CancelInvoke();
			base.Invoke("EndShowTouchPushBack", 0.08f);
		}
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00027A8C File Offset: 0x00025E8C
	private void EndShowTouchPushBack()
	{
		this.isInPushBackMode = false;
		this.handForPushBack.transform.parent = base.transform.parent;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00027AB0 File Offset: 0x00025EB0
	private void OnTriggerEnter(Collider other)
	{
		GameObject gameObject = other.gameObject;
		Transform transform = other.transform;
		if (!this.IsInIncludedSubThingsEditModeForThis(other.gameObject))
		{
			GameObject includedSubThingTopMasterThingPart = Managers.thingManager.GetIncludedSubThingTopMasterThingPart(transform);
			if (includedSubThingTopMasterThingPart != null)
			{
				gameObject = includedSubThingTopMasterThingPart;
				transform = includedSubThingTopMasterThingPart.transform;
			}
		}
		if (gameObject.CompareTag("ThingPart") && transform.parent != null && transform.parent.gameObject != this.holdableInHand && !this.ColliderIsLockedForEditing(gameObject) && !this.ColliderIsArmAttachmentOfSameSide(transform))
		{
			if (!(transform.parent.tag == "Attachment") || Managers.personManager.GetIsThisObjectOfOurPerson(gameObject, false))
			{
				ThingPart component = other.gameObject.GetComponent<ThingPart>();
				if (component != null)
				{
					bool flag = this.controller != null && this.controller.velocity.magnitude >= 1.15f;
					if (flag)
					{
						component.TriggerEventAsStateAuthority(StateListener.EventType.OnHitting, "hand");
						component.TriggerEventAsStateAuthority(StateListener.EventType.OnHitting, string.Empty);
					}
					component.TriggerEventAsStateAuthority(StateListener.EventType.OnTouches, "hand");
					component.TriggerEventAsStateAuthority(StateListener.EventType.OnTouches, string.Empty);
					Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "hand touches " + this.sideStringLower, true);
					this.HandleOneTapBodyAttachmentIfNeeded(component);
				}
				if (this.ParentThingShouldCollide(component))
				{
					if (!component.invisible)
					{
						this.TriggerHapticPulse(Universe.miniBurstPulse);
					}
					if (!Managers.soundLibraryManager.didPlaySoundThisUpdate && (!component.invisible || Our.mode != EditModes.None))
					{
						if (component.isLiquid)
						{
							Managers.soundManager.Play("water_tap", base.transform, 1f, false, false);
						}
						else
						{
							Managers.soundManager.Play("tap", base.transform, 1f, false, false);
						}
					}
					bool flag2 = component.isVideoButton || component.isSlideshowButton || component.isCameraButton;
					if (Our.mode == EditModes.None && !component.invisible && this.handScript.currentDialogType != DialogType.Gifts && this.otherHandScript.currentDialogType != DialogType.Gifts && (!flag2 || !Managers.personManager.WeAreResized()) && !component.GetMyRootThing().movableByEveryone)
					{
						this.StartShowTouchPushBack();
					}
				}
				if (transform.parent != null && transform.parent.CompareTag("Thing"))
				{
					Thing component2 = transform.parent.GetComponent<Thing>();
					if (component2 != null && component2.isThrownOrEmitted && Managers.personManager.GetIsThisObjectOfOurPerson(transform.parent.gameObject, true))
					{
						this.stuckObjectThatIsColliding = transform.parent.gameObject;
					}
				}
			}
		}
		else if (other.gameObject.name == "Browser" && this.IsMatchingBrowserHand())
		{
			this.browserHand.touchedByHand = true;
		}
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00027E05 File Offset: 0x00026205
	private void OnTriggerStay(Collider other)
	{
		this.HandleOnTriggerStay(other, false);
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00027E10 File Offset: 0x00026210
	private void OnTriggerExit(Collider other)
	{
		GameObject gameObject = other.gameObject;
		Transform transform = other.transform;
		if (!this.IsInIncludedSubThingsEditModeForThis(other.gameObject))
		{
			GameObject includedSubThingTopMasterThingPart = Managers.thingManager.GetIncludedSubThingTopMasterThingPart(transform);
			if (includedSubThingTopMasterThingPart != null)
			{
				gameObject = includedSubThingTopMasterThingPart;
				transform = includedSubThingTopMasterThingPart.transform;
			}
		}
		if (!this.ColliderIsLockedForEditing(gameObject) && gameObject.CompareTag("ThingPart") && transform.parent != null && transform.parent.gameObject != this.holdableInHand && !this.ColliderIsArmAttachmentOfSameSide(transform))
		{
			if (!(transform.parent.tag == "Attachment") || Managers.personManager.GetIsThisObjectOfOurPerson(gameObject, false))
			{
				ThingPart component = gameObject.GetComponent<ThingPart>();
				if (component != null)
				{
					component.TriggerEventAsStateAuthority(StateListener.EventType.OnTouchEnds, string.Empty);
				}
			}
		}
		else if (other.gameObject.name == "Browser" && this.IsMatchingBrowserHand())
		{
			this.browserHand.touchedByHand = false;
		}
		this.objectThatIsCollidingDuringEdit = null;
		this.stuckObjectThatIsColliding = null;
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00027F4B File Offset: 0x0002634B
	private bool IsMatchingBrowserHand()
	{
		return (this.browserHand.hand == XRNode.LeftHand && this.side == Side.Left) || (this.browserHand.hand == XRNode.RightHand && this.side == Side.Right);
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00027F8C File Offset: 0x0002638C
	private void HandleOneTapBodyAttachmentIfNeeded(ThingPart thingPart)
	{
		if (Our.mode == EditModes.Inventory && Our.previousMode == EditModes.Body && this.doHandleOneTapBodyAttachment)
		{
			Thing myRootThing = thingPart.GetMyRootThing();
			if (myRootThing != null && myRootThing.isInInventory && (myRootThing.addBodyWhenAttached || myRootThing.addBodyWhenAttachedNonClearing))
			{
				this.doHandleOneTapBodyAttachment = false;
				base.Invoke("ContinueHandlingOneTapBodyAttachment", 0.25f);
				base.StartCoroutine(Managers.thingManager.SetOurCurrentBodyAttachmentsByThing(ThingRequestContext.BodyAttachmentViaHand, myRootThing.thingId, myRootThing.addBodyWhenAttached, myRootThing));
			}
		}
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00028025 File Offset: 0x00026425
	private void ContinueHandlingOneTapBodyAttachment()
	{
		this.doHandleOneTapBodyAttachment = true;
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00028030 File Offset: 0x00026430
	public void HandleOnTriggerStay(Collider other, bool isSecondaryDot = false)
	{
		GameObject gameObject = other.gameObject;
		Transform transform = other.transform;
		if (!this.IsInIncludedSubThingsEditModeForThis(other.gameObject) && Our.mode != EditModes.None)
		{
			GameObject includedSubThingTopMasterThingPart = Managers.thingManager.GetIncludedSubThingTopMasterThingPart(transform);
			if (includedSubThingTopMasterThingPart != null)
			{
				gameObject = includedSubThingTopMasterThingPart;
				transform = includedSubThingTopMasterThingPart.transform;
			}
		}
		if (this.ColliderIsLockedForEditing(gameObject) || this.holdableInHand != null || this.ColliderIsArmAttachmentOfSameSide(transform))
		{
			return;
		}
		if (gameObject.name == "InventoryBiggerCollider")
		{
			if (Our.mode != EditModes.Inventory && !this.handScript.isLegPuppeteering)
			{
				this.TriggerHapticPulse(Universe.lowHapticPulse);
				if ((this.GetPressDown(CrossDevice.button_grab) || this.GetPressDown(CrossDevice.button_grabTip)) && this.EverythingIsReady() && Managers.inventoryManager.CheckIfMayOpenAndAlertIfNot(this.handScript))
				{
					this.holdableInHand = Managers.inventoryManager.OpenDialog(this.handScript, false);
					Our.SetMode(EditModes.Inventory, false);
					Managers.soundManager.Play("clone", base.transform, 0.3f, false, false);
				}
			}
		}
		else if (gameObject.name == DialogType.Keyboard.ToString())
		{
			if (this.GetPressDown(CrossDevice.button_grab) || this.GetPressDown(CrossDevice.button_grabTip))
			{
				transform.parent = base.transform.parent;
				gameObject.tag = this.currentlyHeldTag;
				this.currentlyHeldObject = gameObject;
			}
			else
			{
				this.TriggerHapticPulse(Universe.lowHapticPulse);
			}
		}
		else if (gameObject.name == "BrushGrip")
		{
			if (this.currentlyHeldObject == null)
			{
				if (this.GetPressDown(CrossDevice.button_grab) || this.GetPressDown(CrossDevice.button_grabTip))
				{
					GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(transform.parent.gameObject, transform.position, transform.rotation);
					gameObject2.name = "Brush";
					gameObject2.transform.parent = base.transform.parent;
					gameObject2.tag = this.currentlyHeldTag;
					this.currentlyHeldObject = gameObject2;
					Vector3 localEulerAngles = gameObject2.transform.localEulerAngles;
					localEulerAngles.x = 122f;
					gameObject2.transform.localEulerAngles = localEulerAngles;
					gameObject2.transform.localEulerAngles = new Vector3(122f, 0f, 0f);
					gameObject2.transform.localPosition = new Vector3(0f, -0.005f, 0.02f);
					gameObject2.SetActive(true);
					this.holdableInHand = gameObject2;
					Managers.soundManager.Play("clone", base.transform, 0.3f, false, false);
					CreationHelper.dialogBeforeBrushWasPicked = this.otherHandScript.currentDialog.GetComponent<Dialog>().dialogType;
					this.otherHandScript.SwitchToNewDialog(DialogType.Material, string.Empty);
					this.TriggerHapticPulse(Universe.miniBurstPulse);
				}
				else
				{
					this.TriggerHapticPulse(Universe.lowHapticPulse);
				}
			}
		}
		else if (gameObject.name == "VertexMoverGrip")
		{
			if (this.currentlyHeldObject == null)
			{
				if (this.GetPressDown(CrossDevice.button_grab) || this.GetPressDown(CrossDevice.button_grabTip))
				{
					GameObject gameObject3 = transform.parent.gameObject;
					gameObject3.name = "VertexMover";
					gameObject3.transform.parent = base.transform.parent;
					gameObject3.tag = this.currentlyHeldTag;
					this.currentlyHeldObject = gameObject3;
					Vector3 localEulerAngles2 = gameObject3.transform.localEulerAngles;
					localEulerAngles2.x = 122f;
					gameObject3.transform.localEulerAngles = localEulerAngles2;
					gameObject3.transform.localEulerAngles = new Vector3(122f, 0f, 0f);
					gameObject3.transform.localPosition = new Vector3(0f, -0.005f, 0.02f);
					this.holdableInHand = gameObject3;
					Managers.soundManager.Play("clone", base.transform, 0.3f, false, false);
					this.TriggerHapticPulse(Universe.miniBurstPulse);
					this.otherHandScript.SwitchToNewDialog(DialogType.VertexMover, string.Empty);
					VertexMover vertexMover = (VertexMover)gameObject3.GetComponentInChildren(typeof(VertexMover), true);
					vertexMover.enabled = true;
				}
				else
				{
					this.TriggerHapticPulse(Universe.lowHapticPulse);
				}
			}
		}
		else if (transform.parent != null && transform.parent.CompareTag("GrabbableDialogThingThumb") && this.otherHandScript != null && this.otherHandScript.currentDialogType == DialogType.ForumThread && this.currentlyHeldObject == null)
		{
			this.TriggerHapticPulse(Universe.lowHapticPulse);
			if (this.GetPressDown(CrossDevice.button_grab) || this.GetPressDown(CrossDevice.button_grabTip))
			{
				this.StorePickUpPosition(transform.parent.gameObject);
				this.currentlyHeldObject = Managers.personManager.DoCloneToHand(base.transform.parent.gameObject, transform.parent.gameObject, true);
				Managers.soundManager.Play("clone", base.transform, 0.3f, false, false);
			}
		}
		else if (gameObject.CompareTag("PressableDialogPart"))
		{
			if (!isSecondaryDot)
			{
				this.HandlePressableDialogPartCollision(other);
			}
		}
		else if ((Our.mode == EditModes.Area && (gameObject.CompareTag("Thing") || (gameObject.CompareTag("ThingPart") && !transform.parent.CompareTag("Attachment") && !transform.parent.CompareTag("DialogThingThumb")) || gameObject.CompareTag(this.currentlyHeldOtherTag))) || (Our.mode == EditModes.Thing && (gameObject.CompareTag("ThingPart") || gameObject.CompareTag(this.currentlyHeldOtherTag) || gameObject.CompareTag("ThingPartBase"))) || (Our.mode == EditModes.Environment && gameObject.CompareTag("EnvironmentChanger")) || (Our.mode == EditModes.None && Managers.movableByEveryoneManager.MyRootThingIsMovableByEveryonePlacement(gameObject) && !Managers.movableByEveryoneManager.IsHoldableSubThing(gameObject)))
		{
			this.HandleEditInteraction(other, gameObject, transform, isSecondaryDot);
		}
		else if (gameObject.CompareTag("ThingPart"))
		{
			if (this.GetPressDown(CrossDevice.button_delete))
			{
				if (this.holdableInHand == null && !isSecondaryDot && transform.parent != null && !this.deletedSomethingThisRound)
				{
					this.deletedSomethingThisRound = true;
					if (Our.mode == EditModes.Body && transform.parent.gameObject.tag == "Attachment")
					{
						if (Managers.personManager.GetIsThisObjectOfOurPerson(gameObject, false))
						{
							Thing component = transform.parent.GetComponent<Thing>();
							if (component != null && !component.alreadyDeleted)
							{
								component.alreadyDeleted = true;
								Effects.SpawnCrumbles(transform.parent.gameObject);
								Managers.personManager.DoRemoveAttachedThing(transform.parent.parent.gameObject);
								Managers.soundManager.Play("delete", base.transform, 0.5f, false, false);
								Managers.achievementManager.RegisterAchievement(Achievement.DeletedSomething);
							}
						}
					}
					else if (Our.mode == EditModes.Inventory && transform.parent.parent != null)
					{
						Thing component2 = transform.parent.GetComponent<Thing>();
						ThingPart component3 = gameObject.GetComponent<ThingPart>();
						if (component2 != null && component3 != null)
						{
							if (component2.isInInventoryOrDialog)
							{
								if (Managers.inventoryManager.IsShowingNormalInventory())
								{
									Managers.inventoryManager.DeleteThingPlacement(transform.parent.gameObject);
									Effects.SpawnCrumbles(transform.parent.gameObject);
									global::UnityEngine.Object.Destroy(transform.parent.gameObject);
									Managers.soundManager.Play("delete", transform.parent.transform, 0.5f, false, false);
									Managers.achievementManager.RegisterAchievement(Achievement.DeletedSomething);
								}
							}
							else if (component2.IsPlacement() && Managers.areaManager.weAreEditorOfCurrentArea && Our.previousMode == EditModes.Area && !component2.isLocked && (!component2.isInvisibleToEditors || Our.seeInvisibleAsEditor) && !component3.invisible)
							{
								Managers.personManager.DoDeletePlacement(component2.gameObject, false);
							}
						}
					}
				}
			}
			else if ((this.GetPressDown(CrossDevice.button_grab) || this.GetPressDown(CrossDevice.button_grabTip)) && this.holdableInHand == null && this.currentlyHeldObject == null)
			{
				bool flag = transform.parent.gameObject.tag == "Attachment";
				if (flag)
				{
					if (Our.mode == EditModes.Body && Managers.personManager.GetIsThisObjectOfOurPerson(gameObject, false))
					{
						Transform transform2 = transform.parent.transform;
						GameObject gameObject4 = global::UnityEngine.Object.Instantiate<GameObject>(transform2.gameObject, transform2.position, transform2.rotation);
						Thing component4 = gameObject4.GetComponent<Thing>();
						component4.SetInvisibleToOurPerson(false);
						component4.UpdateContainsBehaviorScriptValue();
						Managers.thingManager.MakeDeepThingClone(transform2.gameObject, gameObject4, false, false, false);
						Managers.personManager.DoRemoveAttachedThing(transform2.parent.gameObject);
						gameObject4.transform.parent = base.transform.parent;
						gameObject4.tag = this.currentlyHeldTag;
						this.currentlyHeldObject = gameObject4;
						Managers.soundManager.Play("pickUp", base.transform, 0.3f, false, false);
					}
				}
				else
				{
					ThingPart component5 = other.GetComponent<ThingPart>();
					if (component5 != null && component5.isGrabbable)
					{
						component5.TriggerEventAsStateAuthority(StateListener.EventType.OnGrabbed, string.Empty);
					}
					GameObject gameObject5 = transform.parent.gameObject;
					Thing thing = gameObject5.GetComponent<Thing>();
					if (Our.mode == EditModes.Inventory || Our.mode == EditModes.Body || thing.isHoldable)
					{
						if (thing.isHoldable && Our.mode != EditModes.Inventory && Our.mode != EditModes.Body)
						{
							if (thing.isHeldAsHoldable)
							{
								if (thing.isHeldAsHoldableByOurPerson && !thing.isInPushBackMode)
								{
									this.currentlyHeldObject = gameObject5;
									this.holdableInHand = gameObject5;
									this.didLetGoOfGrabSinceHolding = !this.GetPressDown(CrossDevice.button_grab);
									this.ignoreNextHoldableTrigger = true;
									if (base.transform.parent.gameObject != null)
									{
										Managers.personManager.DoHoldFromHand(gameObject5, base.transform.parent.gameObject);
									}
								}
							}
							else if (thing.isThrownOrEmitted)
							{
								GameObject gameObject6 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject5, gameObject5.transform.position, gameObject5.transform.rotation);
								Managers.thingManager.MakeDeepThingClone(gameObject5, gameObject6, false, false, false);
								string thrownId = thing.thrownId;
								global::UnityEngine.Object.Destroy(gameObject5);
								this.currentlyHeldObject = gameObject6;
								this.holdableInHand = gameObject6;
								this.didLetGoOfGrabSinceHolding = !this.GetPressDown(CrossDevice.button_grab);
								this.ignoreNextHoldableTrigger = true;
								gameObject6.transform.parent = base.transform.parent.transform;
								Managers.personManager.DoHoldThrownThing(gameObject6, thrownId);
							}
							else if (this.handScript.HasFreeLineOfSightToPoint(gameObject5.transform.position, gameObject5, false))
							{
								GameObject gameObject7 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject5, transform.parent.position, transform.parent.rotation);
								this.currentlyHeldObject = gameObject7;
								this.holdableInHand = gameObject7;
								this.didLetGoOfGrabSinceHolding = !this.GetPressDown(CrossDevice.button_grab);
								this.ignoreNextHoldableTrigger = true;
								gameObject7.transform.parent = base.transform.parent.transform;
								Managers.personManager.DoHoldThing(gameObject7, gameObject5);
							}
						}
						Thing component6 = gameObject5.GetComponent<Thing>();
						if (Our.mode == EditModes.Inventory)
						{
							if (component6 != null && gameObject5 != CreationHelper.thingBeingEdited)
							{
								if (!(gameObject5.transform.parent != null) || !(gameObject5.transform.parent.name == DialogType.Inventory.ToString()))
								{
									Managers.inventoryManager.CloseTrashIfNeeded();
								}
								bool flag2 = component6.isHoldable || component6.remainsHeld;
								this.ignoreNextHoldableTrigger = true;
								bool flag3 = Managers.inventoryManager.IsInsideScopeCube();
								this.currentlyHeldObject = Managers.personManager.DoAddToHand(base.transform.parent.gameObject, gameObject5, flag3);
								this.holdableInHand = ((!flag2 || Our.previousMode != EditModes.None) ? null : this.currentlyHeldObject);
								if (this.holdableInHand != null)
								{
									this.didLetGoOfGrabSinceHolding = !this.GetPressDown(CrossDevice.button_grab);
								}
							}
						}
						else if (Our.mode == EditModes.Body && gameObject5.tag != "DialogThingThumb")
						{
							GameObject gameObject8 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject5, transform.parent.position, transform.parent.rotation);
							Managers.thingManager.MakeDeepThingClone(gameObject5, gameObject8, false, false, false);
							this.currentlyHeldObject = gameObject8;
							gameObject8.transform.parent = base.transform.parent.transform;
							gameObject8.tag = this.currentlyHeldTag;
							Thing component7 = gameObject8.GetComponent<Thing>();
							if (component7 != null)
							{
								bool flag4 = Managers.areaManager.weAreEditorOfCurrentArea && Our.touchUncollidableAsEditor;
								component7.UpdateAllVisibilityAndCollision(true, flag4);
								if (component7.replacesHandsWhenAttached)
								{
									Managers.personManager.ShowOurSecondaryDots(true);
								}
							}
						}
						Managers.soundManager.Play("clone", base.transform, 0.3f, false, false);
					}
				}
			}
			if (!this.GetPress(CrossDevice.button_grab) && !this.GetPress(CrossDevice.button_grabTip))
			{
				Thing thing = transform.parent.gameObject.GetComponent<Thing>();
				if (!thing.isPassable)
				{
					ThingPart component8 = other.GetComponent<ThingPart>();
					if ((Our.mode != EditModes.None || thing.isHoldable || thing.remainsHeld || (component8 != null && component8.isGrabbable)) && this.ParentThingShouldCollide(component8))
					{
						this.TriggerHapticPulse(Universe.lowHapticPulse);
					}
				}
			}
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00028F52 File Offset: 0x00027352
	public bool ColliderIsLockedForEditing(GameObject thisGameObject)
	{
		return this.ThingPartIsLockedForEditing(thisGameObject) || (thisGameObject.transform.parent != null && this.ThingIsLockedForEditing(thisGameObject.transform.parent.gameObject));
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00028F94 File Offset: 0x00027394
	private bool ThingIsLockedForEditing(GameObject thingObject)
	{
		bool flag = false;
		if (thingObject != null && Our.mode == EditModes.Area)
		{
			Thing component = thingObject.GetComponent<Thing>();
			if (component != null)
			{
				flag = component.isLocked || (component.isInvisibleToEditors && !Our.seeInvisibleAsEditor);
			}
		}
		return flag;
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00028FF4 File Offset: 0x000273F4
	private bool ThingPartIsLockedForEditing(GameObject thingPartObject)
	{
		bool flag = false;
		if (thingPartObject != null && Our.mode == EditModes.Thing)
		{
			ThingPart component = thingPartObject.GetComponent<ThingPart>();
			flag = component != null && component.isLocked;
		}
		return flag;
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00029038 File Offset: 0x00027438
	public bool ColliderIsArmAttachmentOfSameSide(Transform otherTransform)
	{
		return base.transform.parent.name == this.handCoreName && otherTransform.parent != null && otherTransform.parent.parent != null && otherTransform.parent.parent.name == this.armAttachmentPointName;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x000290AC File Offset: 0x000274AC
	private bool ParentThingShouldCollide(ThingPart possibleThingPart)
	{
		bool flag = true;
		if (possibleThingPart != null)
		{
			Thing parentThing = possibleThingPart.GetParentThing();
			Thing myRootThing = possibleThingPart.GetMyRootThing();
			if (parentThing != null && myRootThing != null)
			{
				bool flag2 = myRootThing.transform.parent != null && myRootThing.transform.parent.name == "HeadAttachmentPoint";
				bool flag3 = parentThing.IsAtArmAttachmentPoint() && parentThing.replacesHandsWhenAttached && ((this.side == Side.Left && parentThing.transform.parent.name == "ArmLeftAttachmentPoint") || (this.side == Side.Right && parentThing.transform.parent.name == "ArmRightAttachmentPoint"));
				bool flag4 = parentThing.isHeldAsHoldable && parentThing.IsRotatingTowardsEmptyHand();
				flag = (!flag2 || Our.mode == EditModes.Body) && (!flag3 || Our.mode != EditModes.Body) && !flag4;
			}
		}
		return flag;
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x000291D8 File Offset: 0x000275D8
	public void HandlePressableDialogPartCollision(Collider other)
	{
		Transform parent = other.transform.parent;
		DialogPart component = parent.GetComponent<DialogPart>();
		bool flag = false;
		if (component != null && parent.localPosition.y > -0.0075f + component.verticalOffset)
		{
			if (this.handScript.HasFreeLineOfSightToPoint(other.transform.position, null, true))
			{
				parent.localPosition -= Vector3.up * (0.225f * Time.deltaTime);
				if (parent.localPosition.y <= -0.0075f && component.Press(other))
				{
					if (parent.parent != null && parent.parent.parent != null && parent.parent.parent.gameObject == this.otherHand)
					{
						this.otherHandScript.TriggerHapticPulse(Universe.miniBurstPulse);
					}
					this.TriggerHapticPulse(Universe.miniBurstPulse);
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
		}
		if (!flag)
		{
			this.TriggerHapticPulse(350);
		}
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0002930C File Offset: 0x0002770C
	private ThingPart GetEditRelevantIncludedSubThingMaster(Transform thisTransform)
	{
		ThingPart thingPart = null;
		if (Our.mode == EditModes.Thing)
		{
			GameObject includedSubThingDirectMasterThingPart = Managers.thingManager.GetIncludedSubThingDirectMasterThingPart(thisTransform);
			if (includedSubThingDirectMasterThingPart != null && includedSubThingDirectMasterThingPart.transform.parent != null && includedSubThingDirectMasterThingPart.transform.parent.gameObject == CreationHelper.thingBeingEdited)
			{
				thingPart = includedSubThingDirectMasterThingPart.GetComponent<ThingPart>();
			}
		}
		return thingPart;
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0002937C File Offset: 0x0002777C
	private void HandleEditInteraction(Collider originalOther, GameObject otherGameObject, Transform otherTransform, bool isSecondaryDot)
	{
		bool flag = otherGameObject.CompareTag("ThingPartBase");
		if (this.GetPressDown(CrossDevice.button_grab) || this.GetPressDown(CrossDevice.button_grabTip))
		{
			if (this.currentlyHeldObject == null)
			{
				if (flag)
				{
					if (!isSecondaryDot)
					{
						GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(otherGameObject, otherTransform.position, otherTransform.rotation);
						gameObject.transform.parent = base.transform.parent;
						gameObject.transform.localScale = Managers.thingManager.GetBaseShapeDropUpScale(otherGameObject.GetComponent<ThingPart>().baseType);
						gameObject.tag = this.currentlyHeldTag;
						gameObject.GetComponent<ThingPart>().material = gameObject.GetComponent<Renderer>().material;
						this.currentlyHeldObject = gameObject;
						Managers.soundManager.Play("clone", base.transform, 0.3f, false, false);
					}
				}
				else if (Our.mode == EditModes.None && Managers.movableByEveryoneManager.MyRootThingIsMovableByEveryonePlacement(otherGameObject))
				{
					ThingPart component = otherGameObject.GetComponent<ThingPart>();
					Thing myRootThing = component.GetMyRootThing();
					this.currentlyHeldObject = myRootThing.gameObject;
					myRootThing.transform.tag = this.currentlyHeldTag;
					myRootThing.transform.parent = base.transform.parent;
					Managers.soundManager.Play("pickUp", base.transform, 0.3f, false, false);
					if (this.otherDotScript.currentlyHeldObject == myRootThing.gameObject)
					{
						this.otherDotScript.currentlyHeldObject = null;
					}
					Managers.personManager.DoHoldAsMovableByEveryone(myRootThing, this.side);
				}
				else if (otherGameObject.name != DialogType.Keyboard.ToString())
				{
					Transform transform = null;
					GameObject gameObject2 = null;
					GameObject gameObject3 = null;
					EditModes mode = Our.mode;
					if (mode != EditModes.Area)
					{
						if (mode != EditModes.Thing)
						{
							if (mode == EditModes.Environment)
							{
								transform = otherTransform;
								gameObject2 = otherGameObject;
							}
						}
						else
						{
							transform = otherTransform;
							gameObject2 = otherGameObject;
							gameObject3 = otherGameObject;
						}
					}
					else
					{
						transform = otherTransform.parent;
						gameObject2 = otherTransform.parent.gameObject;
						gameObject3 = otherTransform.parent.gameObject;
					}
					if (transform != null && gameObject2 != null)
					{
						ThingPart editRelevantIncludedSubThingMaster = this.GetEditRelevantIncludedSubThingMaster(transform);
						if (editRelevantIncludedSubThingMaster != null)
						{
							Thing component2 = transform.parent.GetComponent<Thing>();
							component2.transform.parent = base.transform.parent;
							this.currentlyHeldObject = component2.gameObject;
							this.StorePickUpPosition(component2.gameObject);
							editRelevantIncludedSubThingMaster.RemoveThingAsIncludedSubThing(component2, false);
							Managers.soundManager.Play("pickUp", base.transform, 0.3f, false, false);
							this.ResetObjectThatIsCollidingDuringEdit();
						}
						else if ((transform.parent && (transform.parent.name == "HandCoreLeft" || transform.parent.name == "HandCoreRight")) || (this.otherDotScript.objectThatIsCollidingDuringEdit != null && gameObject3 != null && (this.otherDotScript.objectThatIsCollidingDuringEdit == gameObject3 || this.otherDotScript.objectThatIsCollidingDuringEdit.transform.parent.gameObject == gameObject3)) || (CrossDevice.desktopMode && Input.GetKey(KeyCode.LeftAlt)))
						{
							if (this.currentlyHeldObject == null)
							{
								if (Our.mode == EditModes.Thing)
								{
									GameObject gameObject4 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject2, transform.position, transform.rotation);
									ThingPart component3 = gameObject2.GetComponent<ThingPart>();
									ThingPart component4 = gameObject4.GetComponent<ThingPart>();
									CreationHelper.thingPartPickupPosition = gameObject2.transform.localPosition;
									gameObject4.tag = this.currentlyHeldTag;
									gameObject4.transform.parent = base.transform.parent;
									component4.material = gameObject4.GetComponent<Renderer>().material;
									component4.particleSystemType = component3.particleSystemType;
									component4.isAngleLocker = false;
									component4.isPositionLocker = false;
									component4.changedVertices = ((component3.changedVertices == null) ? null : new Dictionary<int, Vector3>(component3.changedVertices));
									component4.smoothingAngle = component3.smoothingAngle;
									component4.convex = component3.convex;
									component4.ClearAssignedReflectionParts();
									MeshFilter component5 = component4.GetComponent<MeshFilter>();
									if (component5 != null)
									{
										Mesh mesh = component5.mesh;
										component4.GetComponent<MeshFilter>().mesh = mesh;
									}
									ThingPartState thingPartState = component3.states[0];
									ThingPartState thingPartState2 = component4.states[0];
									thingPartState2.particleSystemProperty = Managers.thingManager.CloneParticleSystemProperty(thingPartState.particleSystemProperty);
									thingPartState2.particleSystemColor = thingPartState.particleSystemColor;
									thingPartState2.textureProperties = Managers.thingManager.CloneTextureProperties(thingPartState.textureProperties);
									if (thingPartState.textureColors != null)
									{
										for (int i = 0; i < thingPartState.textureColors.Length; i++)
										{
											thingPartState2.textureColors[i] = thingPartState.textureColors[i];
										}
									}
									component4.includedSubThings = component3.includedSubThings;
									this.currentlyHeldObject = gameObject4;
									Thing thing = ((!(transform.parent != null)) ? null : transform.parent.GetComponent<Thing>());
									if (thing != null && thing.smallEditMovements)
									{
										this.ClearPickUpPosition();
									}
									else
									{
										this.StorePickUpPosition(this.currentlyHeldObject);
									}
									Managers.soundManager.Play("clone", base.transform, 0.3f, false, false);
									Managers.achievementManager.RegisterAchievement(Achievement.DuplicatedThingPart);
								}
								else if (Our.mode == EditModes.Area)
								{
									this.StorePickUpPosition(gameObject2);
									this.currentlyHeldObject = Managers.personManager.DoCloneToHand(base.transform.parent.gameObject, gameObject2, false);
								}
							}
						}
						else if (Our.mode != EditModes.Thing || otherTransform.parent.gameObject == CreationHelper.thingBeingEdited)
						{
							if (Our.mode == EditModes.Area)
							{
								if (this.IsUnThrownOrEmittedThing(gameObject2.transform))
								{
									this.currentlyHeldObject = gameObject2;
									this.StorePickUpPosition(gameObject2);
									Managers.personManager.DoEditHold(base.transform.parent.gameObject, gameObject2);
								}
								else
								{
									Our.SetMode(EditModes.None, false);
								}
							}
							else
							{
								transform.parent = base.transform.parent;
								gameObject2.tag = this.currentlyHeldTag;
								this.currentlyHeldObject = gameObject2;
								this.StorePickUpPosition(gameObject2);
								Managers.soundManager.Play("pickUp", base.transform, 0.3f, false, false);
							}
							Our.SetLastTransformHandled(transform);
						}
					}
				}
				this.hasJustPickedUp = true;
				base.Invoke("StopPickUpHapticPulse", 0.1f);
			}
		}
		else if (this.GetPressDown(CrossDevice.button_delete) && !isSecondaryDot && this.holdableInHand == null && this.currentlyHeldObject == null && !this.deletedSomethingThisRound && Our.mode != EditModes.Environment)
		{
			this.deletedSomethingThisRound = true;
			if (Our.mode == EditModes.Area)
			{
				bool flag2 = otherTransform.parent == null;
				if (flag2)
				{
					global::UnityEngine.Object.Destroy(otherGameObject);
				}
				else if (this.IsUnThrownOrEmittedThing(otherTransform.parent))
				{
					ThingPart component6 = otherGameObject.GetComponent<ThingPart>();
					if (component6 != null && (!component6.invisible || Our.seeInvisibleAsEditor || component6.HasVertexTexture()))
					{
						Managers.personManager.DoDeletePlacement(otherTransform.parent.gameObject, false);
					}
				}
			}
			else
			{
				ThingPart editRelevantIncludedSubThingMaster2 = this.GetEditRelevantIncludedSubThingMaster(otherTransform);
				if (editRelevantIncludedSubThingMaster2 != null)
				{
					Thing component7 = otherTransform.parent.GetComponent<Thing>();
					Effects.SpawnCrumbles(component7.gameObject);
					editRelevantIncludedSubThingMaster2.RemoveThingAsIncludedSubThing(component7, true);
					this.ResetObjectThatIsCollidingDuringEdit();
					Managers.soundManager.Play("delete", base.transform, 0.5f, false, false);
				}
				else if (Our.mode != EditModes.Thing || otherTransform.parent.gameObject == CreationHelper.thingBeingEdited)
				{
					GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
					if (!(currentNonStartDialog != null) || currentNonStartDialog.GetComponent<Dialog>().dialogType != DialogType.Keyboard)
					{
						GameObject includedSubThingDirectMasterThingPart = Managers.thingManager.GetIncludedSubThingDirectMasterThingPart(originalOther.transform);
						if (includedSubThingDirectMasterThingPart != null && includedSubThingDirectMasterThingPart.transform.parent.gameObject == CreationHelper.thingBeingEdited)
						{
							ThingPart component8 = includedSubThingDirectMasterThingPart.GetComponent<ThingPart>();
							Effects.SpawnCrumbles(originalOther.transform.parent.gameObject);
							component8.RemoveThingAsIncludedSubThing(originalOther.transform.parent.GetComponent<Thing>(), true);
							this.ResetObjectThatIsCollidingDuringEdit();
							Managers.soundManager.Play("delete", base.transform, 0.5f, false, false);
						}
						else
						{
							ThingPart component9 = otherGameObject.GetComponent<ThingPart>();
							if (component9 == null || !component9.isUnremovableCenter)
							{
								bool flag3 = false;
								if (component9 != null)
								{
									Thing myRootThing2 = component9.GetMyRootThing();
									flag3 = myRootThing2 != null && myRootThing2.movableByEveryone;
								}
								if (!flag3 || Our.mode != EditModes.None)
								{
									Effects.SpawnCrumbles(otherGameObject);
									global::UnityEngine.Object.Destroy(otherGameObject);
									float num = 0.1f;
									base.Invoke("AutoSetLastTransformHandledIfOnlySingleThingPart", num);
									Managers.achievementManager.RegisterAchievement(Achievement.DeletedSomething);
									Managers.soundManager.Play("delete", base.transform, 0.5f, false, false);
								}
							}
							else
							{
								Managers.soundManager.Play("no", base.transform, 1f, false, false);
							}
						}
					}
				}
			}
		}
		if (!isSecondaryDot)
		{
			this.objectThatIsCollidingDuringEdit = null;
		}
		this.didJustBuzz = false;
		if (!this.GetPress(CrossDevice.button_grab) && !this.GetPress(CrossDevice.button_grabTip) && (!flag || !isSecondaryDot) && this.ParentThingShouldCollide(otherGameObject.GetComponent<ThingPart>()))
		{
			if (CrossDevice.type == global::DeviceType.OculusTouch && this.otherDotScript.didJustBuzz)
			{
				bool flag4 = Universe.counterForOculusTouchBuzzIssue % 2 == 0;
				if ((flag4 && this.side == Side.Left) || (!flag4 && this.side == Side.Right))
				{
					this.TriggerHapticPulse(500);
				}
			}
			else
			{
				this.TriggerHapticPulse(Universe.lowHapticPulse);
			}
			this.didJustBuzz = true;
			if (!isSecondaryDot)
			{
				bool flag5 = otherTransform.parent != null && otherTransform.parent.parent != null && otherTransform.parent.parent.CompareTag("PlacedSubThings");
				Thing thing2 = null;
				if (otherTransform.parent != null)
				{
					thing2 = otherTransform.parent.GetComponent<Thing>();
				}
				bool flag6 = thing2 != null && thing2.containsPlacedSubThings;
				bool flag7 = thing2 != null && Our.mode == EditModes.Thing && thing2.gameObject != CreationHelper.thingBeingEdited;
				bool flag8 = thing2 != null && thing2.IsIncludedSubThing();
				if (!flag5 && !flag8 && !flag6 && !flag7 && !this.IncludedSubThingsDialogIsOpen())
				{
					this.objectThatIsCollidingDuringEdit = otherGameObject;
				}
			}
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00029F48 File Offset: 0x00028348
	private void ResetObjectThatIsCollidingDuringEdit()
	{
		base.CancelInvoke("DoResetObjectThatIsCollidingDuringEdit");
		base.Invoke("DoResetObjectThatIsCollidingDuringEdit", 0.1f);
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00029F65 File Offset: 0x00028365
	private void DoResetObjectThatIsCollidingDuringEdit()
	{
		this.objectThatIsCollidingDuringEdit = null;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00029F70 File Offset: 0x00028370
	private bool IsUnThrownOrEmittedThing(Transform thisTransform)
	{
		bool flag = false;
		if (thisTransform != null)
		{
			Thing component = thisTransform.GetComponent<Thing>();
			if (component != null)
			{
				bool flag2 = !string.IsNullOrEmpty(component.thrownId) || component.isThrownOrEmitted;
				flag = !flag2;
			}
		}
		return flag;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00029FC0 File Offset: 0x000283C0
	private void ReCenterThingIfIsUnremovableCenter(ThingPart thingPart)
	{
		if (thingPart.isUnremovableCenter && thingPart.transform.parent != null)
		{
			Thing component = thingPart.transform.parent.GetComponent<Thing>();
			component.ReCenterThingBasedOnPart(thingPart);
		}
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0002A006 File Offset: 0x00028406
	private void MoveThingPartStatePositionsInRelationToPickupPosition(ThingPart thingPart)
	{
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0002A008 File Offset: 0x00028408
	private void AutoSetLastTransformHandledIfOnlySingleThingPart()
	{
		Our.AutoSetLastTransformHandledIfOnlySingleThingPart();
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0002A00F File Offset: 0x0002840F
	public void TriggerHapticPulse(ushort pulseAmount)
	{
		CrossDevice.TriggerHapticPulse(this.handScript, pulseAmount);
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0002A01D File Offset: 0x0002841D
	public bool GetPress(ulong type)
	{
		return CrossDevice.GetPress(this.controller, type, this.side);
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x0002A031 File Offset: 0x00028431
	private bool GetPressUp(ulong type)
	{
		return CrossDevice.GetPressUp(this.controller, type, this.side);
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0002A045 File Offset: 0x00028445
	private bool GetPressDown(ulong type)
	{
		return CrossDevice.GetPressDown(this.controller, type, this.side);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0002A05C File Offset: 0x0002845C
	private void StorePickUpPosition(GameObject thisObject)
	{
		if (thisObject != null)
		{
			this.myPickUpPosition = base.transform.position;
			this.myPickUpAngles = base.transform.eulerAngles;
			this.objectPickUpPosition = thisObject.transform.position;
			this.objectPickUpAngles = thisObject.transform.eulerAngles;
		}
		else
		{
			this.ClearPickUpPosition();
		}
	}

	// Token: 0x04000577 RID: 1399
	public TopographyId topographyId;

	// Token: 0x04000578 RID: 1400
	public GameObject environmentChangers;

	// Token: 0x04000579 RID: 1401
	public GameObject otherHand;

	// Token: 0x0400057A RID: 1402
	public GameObject otherDot;

	// Token: 0x0400057B RID: 1403
	public Vector3 positionEmptyClickStarted = Vector3.zero;

	// Token: 0x0400057C RID: 1404
	public float timeEmptyClickForMoveStarted = -1f;

	// Token: 0x0400057D RID: 1405
	public GameObject holdableInHand;

	// Token: 0x0400057E RID: 1406
	public GameObject currentlyHeldObject;

	// Token: 0x0400057F RID: 1407
	private HandDot otherDotScript;

	// Token: 0x04000580 RID: 1408
	private SteamVR_TrackedObject trackedObj;

	// Token: 0x04000581 RID: 1409
	public VRBrowserHand browserHand;

	// Token: 0x04000582 RID: 1410
	private bool didLetGoOfGrabSinceHolding;

	// Token: 0x04000583 RID: 1411
	private Hand handScript;

	// Token: 0x04000584 RID: 1412
	private Hand otherHandScript;

	// Token: 0x04000585 RID: 1413
	private bool isAuthoritativeSide;

	// Token: 0x04000586 RID: 1414
	public bool ignoreNextHoldableTrigger;

	// Token: 0x04000587 RID: 1415
	private bool hasJustPickedUp;

	// Token: 0x04000588 RID: 1416
	private string currentlyHeldTag = string.Empty;

	// Token: 0x04000589 RID: 1417
	private string currentlyHeldOtherTag = string.Empty;

	// Token: 0x0400058A RID: 1418
	private const string currentlyHeldAnyTag = "CurrentlyHeld";

	// Token: 0x0400058B RID: 1419
	private GameObject objectThatIsCollidingDuringEdit;

	// Token: 0x0400058C RID: 1420
	private GameObject stuckObjectThatIsColliding;

	// Token: 0x0400058D RID: 1421
	private GameObject handForPushBack;

	// Token: 0x0400058E RID: 1422
	private bool isInPushBackMode;

	// Token: 0x0400058F RID: 1423
	public Vector3 myPickUpPosition = Vector3.zero;

	// Token: 0x04000590 RID: 1424
	public Vector3 objectPickUpPosition = Vector3.zero;

	// Token: 0x04000591 RID: 1425
	public Vector3 myPickUpAngles = Vector3.zero;

	// Token: 0x04000592 RID: 1426
	public Vector3 objectPickUpAngles = Vector3.zero;

	// Token: 0x04000593 RID: 1427
	private bool deletedSomethingThisRound;

	// Token: 0x04000595 RID: 1429
	public bool didJustBuzz;

	// Token: 0x04000596 RID: 1430
	private string handCoreName = string.Empty;

	// Token: 0x04000597 RID: 1431
	private string armAttachmentPointName = string.Empty;

	// Token: 0x04000598 RID: 1432
	private bool doHandleOneTapBodyAttachment = true;

	// Token: 0x04000599 RID: 1433
	private DialogType[] dialogTypesAllowingFarAwayHold = new DialogType[] { DialogType.Thing };

	// Token: 0x0400059A RID: 1434
	private string sideString = string.Empty;

	// Token: 0x0400059B RID: 1435
	private string sideStringLower = string.Empty;
}
