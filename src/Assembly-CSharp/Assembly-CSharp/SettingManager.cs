using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class SettingManager : MonoBehaviour, IGameManager
{
	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06001310 RID: 4880 RVA: 0x000A5B9C File Offset: 0x000A3F9C
	// (set) Token: 0x06001311 RID: 4881 RVA: 0x000A5BA4 File Offset: 0x000A3FA4
	public ManagerStatus status { get; private set; }

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06001312 RID: 4882 RVA: 0x000A5BAD File Offset: 0x000A3FAD
	// (set) Token: 0x06001313 RID: 4883 RVA: 0x000A5BB5 File Offset: 0x000A3FB5
	public string failMessage { get; private set; }

	// Token: 0x06001314 RID: 4884 RVA: 0x000A5BBE File Offset: 0x000A3FBE
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x000A5BD0 File Offset: 0x000A3FD0
	public void SetState(Setting setting, bool state, bool viaBehaviorScript = false)
	{
		bool flag = false;
		Thing thing = null;
		bool flag2 = this.RequiresThingEditing(setting);
		if (flag2)
		{
			if (CreationHelper.thingBeingEdited != null)
			{
				thing = CreationHelper.thingBeingEdited.GetComponent<Thing>();
			}
			if (thing == null)
			{
				return;
			}
		}
		switch (setting)
		{
		case Setting.Microphone:
			Universe.SetTransmitFromMicrophone(state);
			break;
		case Setting.SeeInvisible:
			Our.SetSeeInvisibleAsEditor(state);
			break;
		case Setting.TouchUncollidable:
			Our.SetTouchUncollidableAsEditor(state);
			break;
		case Setting.LowerGraphicsQuality:
			Managers.optimizationManager.SetDoOptimizeSpeed(state, true, false);
			break;
		case Setting.Fly:
			Our.SetCanEditFly(state);
			break;
		case Setting.Findable:
			Managers.personManager.DoSetIsFindable(state);
			break;
		case Setting.OnlyFriendsCanPingUs:
			Our.SetOnlyFriendsCanPingUs(state);
			break;
		case Setting.StopAlerts:
			Our.SetStopPingsAndAlerts(state);
			break;
		case Setting.ShowGrid:
		{
			GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
			GridLines component = @object.GetComponent<GridLines>();
			if (state)
			{
				if (component == null)
				{
					@object.AddComponent<GridLines>();
				}
			}
			else if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
			break;
		}
		case Setting.SnapThingsToGrid:
			Our.snapThingsToGrid = state;
			break;
		case Setting.SnapAngles:
			CreationHelper.customSnapAngles = null;
			thing.doSnapAngles = state;
			if (!thing.doSnapAngles && thing.doSnapPosition)
			{
				thing.doSnapPosition = false;
				flag = true;
			}
			if (thing.doSnapAngles && thing.doSoftSnapAngles)
			{
				thing.doSoftSnapAngles = false;
				flag = true;
			}
			if (thing.doSnapAngles && thing.smallEditMovements)
			{
				thing.smallEditMovements = false;
				flag = true;
			}
			if (thing.doSnapAngles && thing.doLockAngles)
			{
				thing.doLockAngles = false;
				flag = true;
			}
			break;
		case Setting.SoftSnapAngles:
			CreationHelper.customSnapAngles = null;
			thing.doSoftSnapAngles = state;
			if (thing.doSoftSnapAngles && thing.doSnapAngles)
			{
				thing.doSnapAngles = false;
				flag = true;
			}
			if (!thing.doSoftSnapAngles && thing.doSnapPosition)
			{
				thing.doSnapPosition = false;
				flag = true;
			}
			if (thing.doSoftSnapAngles && thing.smallEditMovements)
			{
				thing.smallEditMovements = false;
				flag = true;
			}
			if (thing.doSoftSnapAngles && thing.doLockAngles)
			{
				thing.doLockAngles = false;
				flag = true;
			}
			break;
		case Setting.LockAngles:
			CreationHelper.customSnapAngles = null;
			thing.doLockAngles = state;
			if (thing.doLockAngles && thing.doSnapAngles)
			{
				thing.doSnapAngles = false;
				flag = true;
			}
			if (thing.doLockAngles && thing.doSoftSnapAngles)
			{
				thing.doSoftSnapAngles = false;
				flag = true;
			}
			if (thing.doLockAngles && thing.smallEditMovements)
			{
				thing.smallEditMovements = false;
				flag = true;
			}
			if (thing.doLockAngles && thing.doLockPosition)
			{
				thing.doLockPosition = false;
				flag = true;
			}
			break;
		case Setting.SnapPosition:
			thing.doSnapPosition = state;
			if (thing.doSnapPosition && !thing.doSnapAngles && !thing.doSoftSnapAngles)
			{
				thing.doSnapAngles = true;
				flag = true;
			}
			if (thing.doSnapPosition && thing.smallEditMovements)
			{
				thing.smallEditMovements = false;
				flag = true;
			}
			if (thing.doSnapPosition && thing.doLockAngles)
			{
				thing.doLockAngles = false;
				flag = true;
			}
			break;
		case Setting.LockPosition:
			thing.doLockPosition = state;
			if (thing.doLockPosition && thing.doSnapPosition)
			{
				thing.doSnapPosition = false;
				flag = true;
			}
			if (thing.doLockPosition && thing.doLockAngles)
			{
				thing.doLockAngles = false;
				flag = true;
			}
			if (thing.doLockPosition && thing.smallEditMovements)
			{
				thing.smallEditMovements = false;
				flag = true;
			}
			break;
		case Setting.ScaleAllParts:
			thing.scaleAllParts = state;
			if (thing.scaleAllParts && thing.scaleEachPartUniformly)
			{
				thing.scaleEachPartUniformly = false;
				flag = true;
			}
			break;
		case Setting.ScaleEachPartUniformly:
			thing.scaleEachPartUniformly = state;
			if (thing.scaleEachPartUniformly && thing.scaleAllParts)
			{
				thing.scaleAllParts = false;
				flag = true;
			}
			break;
		case Setting.FinetunePosition:
			thing.smallEditMovements = state;
			if (thing.smallEditMovements && (thing.doSnapAngles || thing.doSoftSnapAngles || thing.doSnapPosition || thing.doLockPosition))
			{
				thing.doSnapAngles = false;
				thing.doSoftSnapAngles = false;
				thing.doSnapPosition = false;
				thing.doLockAngles = false;
				thing.doLockPosition = false;
				flag = true;
			}
			break;
		case Setting.SymmetrySideways:
			thing.autoAddReflectionPartsSideways = state;
			break;
		case Setting.SymmetryVertical:
			thing.autoAddReflectionPartsVertical = state;
			break;
		case Setting.SymmetryDepth:
			thing.autoAddReflectionPartsDepth = state;
			break;
		case Setting.ExtraEffectsInVr:
			if (Managers.personManager.ourPerson != null && Managers.personManager.ourPerson.ageInDays >= 1)
			{
				Managers.optimizationManager.SetExtraEffectsEvenInVR(state, false);
			}
			break;
		case Setting.SnapThingAngles:
			Our.doSnapThingAngles = state;
			if (Our.doSnapThingAngles)
			{
				Our.disableAllThingSnapping = false;
				flag = true;
			}
			else
			{
				Our.doSnapThingPosition = false;
				flag = true;
			}
			break;
		case Setting.SnapThingPosition:
			Our.doSnapThingPosition = state;
			if (Our.doSnapThingPosition)
			{
				Our.disableAllThingSnapping = false;
				Our.doSnapThingAngles = true;
				flag = true;
			}
			break;
		case Setting.IgnoreThingSnapping:
			Our.disableAllThingSnapping = state;
			if (Our.disableAllThingSnapping)
			{
				Our.doSnapThingAngles = false;
				Our.doSnapThingPosition = false;
				flag = true;
			}
			break;
		}
		if (flag)
		{
			this.TriggerAllSettingChangesForAllAttachments();
		}
		else
		{
			this.TriggerSettingChangeForAllAttachments(setting, state);
		}
		if (viaBehaviorScript || flag)
		{
			Dialog currentNonStartDialogAsDialog = Managers.dialogManager.GetCurrentNonStartDialogAsDialog();
			if (currentNonStartDialogAsDialog != null)
			{
				currentNonStartDialogAsDialog.RecreateInterfaceAfterSettingsChangeIfNeeded();
			}
		}
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x000A61BC File Offset: 0x000A45BC
	public bool GetState(Setting setting)
	{
		bool flag = false;
		Thing thing = null;
		if (this.RequiresThingEditing(setting))
		{
			if (CreationHelper.thingBeingEdited != null)
			{
				thing = CreationHelper.thingBeingEdited.GetComponent<Thing>();
			}
			if (thing == null)
			{
				return false;
			}
		}
		switch (setting)
		{
		case Setting.Microphone:
			flag = Universe.transmitFromMicrophone;
			break;
		case Setting.SeeInvisible:
			flag = Our.seeInvisibleAsEditor;
			break;
		case Setting.TouchUncollidable:
			flag = Our.touchUncollidableAsEditor;
			break;
		case Setting.LowerGraphicsQuality:
			flag = Managers.optimizationManager.doOptimizeSpeed;
			break;
		case Setting.Fly:
			flag = Our.canEditFly;
			break;
		case Setting.Findable:
			flag = Managers.personManager.ourPerson.isFindable;
			break;
		case Setting.OnlyFriendsCanPingUs:
			flag = Our.onlyFriendsCanPingUs;
			break;
		case Setting.StopAlerts:
			flag = Our.stopPingsAndAlerts;
			break;
		case Setting.ShowGrid:
		{
			GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
			flag = @object.GetComponent<GridLines>() != null;
			break;
		}
		case Setting.SnapThingsToGrid:
			flag = Our.snapThingsToGrid;
			break;
		case Setting.SnapAngles:
			flag = thing.doSnapAngles;
			break;
		case Setting.SoftSnapAngles:
			flag = thing.doSoftSnapAngles;
			break;
		case Setting.SnapPosition:
			flag = thing.doSnapPosition;
			break;
		case Setting.ScaleAllParts:
			flag = thing.scaleAllParts;
			break;
		case Setting.ScaleEachPartUniformly:
			flag = thing.scaleEachPartUniformly;
			break;
		case Setting.FinetunePosition:
			flag = thing.smallEditMovements;
			break;
		case Setting.SymmetrySideways:
			flag = thing.autoAddReflectionPartsSideways;
			break;
		case Setting.SymmetryVertical:
			flag = thing.autoAddReflectionPartsVertical;
			break;
		case Setting.SymmetryDepth:
			flag = thing.autoAddReflectionPartsDepth;
			break;
		case Setting.ExtraEffectsInVr:
			flag = Managers.optimizationManager.extraEffectsEvenInVR;
			break;
		case Setting.SnapThingAngles:
			flag = Our.doSnapThingAngles;
			break;
		case Setting.SnapThingPosition:
			flag = Our.doSnapThingPosition;
			break;
		case Setting.IgnoreThingSnapping:
			flag = Our.disableAllThingSnapping;
			break;
		}
		return flag;
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x000A63A4 File Offset: 0x000A47A4
	private bool RequiresThingEditing(Setting setting)
	{
		Setting[] array = new Setting[]
		{
			Setting.SnapAngles,
			Setting.SoftSnapAngles,
			Setting.LockAngles,
			Setting.SnapPosition,
			Setting.LockPosition,
			Setting.ScaleAllParts,
			Setting.ScaleEachPartUniformly,
			Setting.FinetunePosition,
			Setting.SymmetrySideways,
			Setting.SymmetryVertical,
			Setting.SymmetryDepth
		};
		return Array.IndexOf<Setting>(array, setting) >= 0;
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x000A63D4 File Offset: 0x000A47D4
	public void TriggerAllSettingChangesForAllAttachments()
	{
		foreach (KeyValuePair<AttachmentPointId, GameObject> keyValuePair in Managers.personManager.ourPerson.AttachmentPointsById)
		{
			GameObject value = keyValuePair.Value;
			if (value != null)
			{
				IEnumerator enumerator2 = Enum.GetValues(typeof(Setting)).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj = enumerator2.Current;
						Setting setting = (Setting)obj;
						this.TriggerSettingChangeForAttachment(value, setting, this.GetState(setting));
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = enumerator2 as IDisposable) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x000A64B4 File Offset: 0x000A48B4
	public void TriggerAllSettingChangesForAttachment(GameObject attachmentObject)
	{
		IEnumerator enumerator = Enum.GetValues(typeof(Setting)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Setting setting = (Setting)obj;
				this.TriggerSettingChangeForAttachment(attachmentObject, setting, this.GetState(setting));
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

	// Token: 0x0600131A RID: 4890 RVA: 0x000A652C File Offset: 0x000A492C
	private void TriggerSettingChangeForAllAttachments(Setting setting, bool state)
	{
		foreach (KeyValuePair<AttachmentPointId, GameObject> keyValuePair in Managers.personManager.ourPerson.AttachmentPointsById)
		{
			GameObject value = keyValuePair.Value;
			if (value != null)
			{
				this.TriggerSettingChangeForAttachment(value, setting, state);
			}
		}
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x000A65A8 File Offset: 0x000A49A8
	public void TriggerSettingChangeForAttachment(GameObject attachmentObject, Setting setting, bool state)
	{
		string text = Misc.CamelCaseToSpaceSeparated(setting.ToString()).ToLower();
		StateListener.EventType eventType = ((!state) ? StateListener.EventType.OnSettingDisabled : StateListener.EventType.OnSettingEnabled);
		Thing[] componentsInChildren = attachmentObject.GetComponentsInChildren<Thing>();
		foreach (Thing thing in componentsInChildren)
		{
			thing.TriggerEvent(eventType, text, false, null);
		}
	}
}
