using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class IncludeThingDialog : Dialog
{
	// Token: 0x06000BB2 RID: 2994 RVA: 0x00062B70 File Offset: 0x00060F70
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Include", -370, -460, TextColor.Default, TextAlignment.Left, false);
		this.thing = this.thing.GetMyRootThing();
		Managers.thingManager.GetThingInfo(this.thing.thingId, delegate(ThingInfo thingInfo)
		{
			if (this == null || thingInfo == null)
			{
				return;
			}
			this.AddMainInterface(thingInfo);
		});
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00062BEC File Offset: 0x00060FEC
	private void AddMainInterface(ThingInfo info)
	{
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		this.AddIncludeAsSubThingButtonIfAppropriate();
		this.AddMergePartsButtonIfAppropriate(info);
		base.AddSeparator(0, 270, false);
		if (this.thing.isLocked)
		{
			base.AddLabel("This placement is locked", 0, 355, 0.8f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		else
		{
			base.AddCheckbox("removeOriginal", null, "Remove original", 0, 380, Our.removeOriginalWhenIncluding, 1f, "Checkbox", TextColor.Red, null, ExtraIcon.None);
		}
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00062C9C File Offset: 0x0006109C
	private void AddIncludeAsSubThingButtonIfAppropriate()
	{
		if (Our.lastTransformHandled != null)
		{
			ThingPart component = Our.lastTransformHandled.GetComponent<ThingPart>();
			if (component != null && !component.isLocked && component.GetIsOfThingBeingEdited() && !component.GetParentThing().IsIncludedSubThing())
			{
				this.thingPartToAddSubThingTo = component;
				base.AddButton("includeAsSubThing", null, "Include as Sub-Thing", "ButtonBig", 0, -150, "includeAsSubThing", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00062D40 File Offset: 0x00061140
	private void IncludeAsSubThing()
	{
		if (this.thingPartToAddSubThingTo != null)
		{
			this.thingPartToAddSubThingTo.ResetAndStopStateAsCurrentlyEditing();
			this.thingPartToAddSubThingTo.AddThingAsIncludedSubThing(this.thing);
			CreationHelper.thingPartWhoseStatesAreEdited = this.thingPartToAddSubThingTo.gameObject;
			Managers.soundManager.Play("success", this.transform, 0.4f, false, false);
			this.dialogToSwitchToAfter = DialogType.IncludedSubThings;
			this.RemoveOriginalPlacementIfNeeded();
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00062DB4 File Offset: 0x000611B4
	private void AddMergePartsButtonIfAppropriate(ThingInfo info)
	{
		bool flag = info.clonedFromId != null;
		bool flag2 = info.creatorId == Managers.personManager.ourPerson.userId;
		bool flag3 = (this.thing.isClonable || info.allCreatorsThingsClonable) && !this.thing.isNeverClonable;
		if (flag2 || flag3)
		{
			string text = null;
			if (!flag2 || flag)
			{
				text = this.thing.thingId;
			}
			if ((flag2 && !flag) || string.IsNullOrEmpty(CreationHelper.thingThatWasClonedFromIdIfRelevant))
			{
				base.AddButton("mergePartsIntoCreation", text, "Include Parts", "ButtonBig", 0, 60, "mergePartsIntoCreation", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
		}
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00062EA0 File Offset: 0x000612A0
	private IEnumerator MergePartsIntoCreation(string thisThingThatWasClonedFromIdIfRelevant)
	{
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.MergePartsIntoCreation, this.thing.thingId, delegate(GameObject returnThing)
		{
			returnThing.transform.localScale = this.thing.transform.localScale;
			returnThing.transform.localPosition = this.thing.transform.localPosition;
			returnThing.transform.localRotation = this.thing.transform.localRotation;
			Thing component = CreationHelper.thingBeingEdited.GetComponent<Thing>();
			if (CreationHelper.thingBeingEdited.GetComponentsInChildren<ThingPart>().Length == 0)
			{
				CreationHelper.thingBeingEdited.transform.position = returnThing.transform.position;
				CreationHelper.thingBeingEdited.transform.rotation = returnThing.transform.rotation;
				component.MemorizeOriginalTransform(false);
			}
			Component[] componentsInChildren = returnThing.GetComponentsInChildren<ThingPart>();
			Component[] componentsInChildren2 = CreationHelper.thingBeingEdited.GetComponentsInChildren<ThingPart>();
			int num = componentsInChildren.Length + componentsInChildren2.Length;
			if (num > 1000)
			{
				global::UnityEngine.Object.Destroy(returnThing.gameObject);
				Managers.soundManager.Play("no", this.transform, 1f, false, false);
				Managers.dialogManager.ShowInfo(string.Concat(new object[] { "Oops, this can't be merged as it would create ", num, " parts (the maximum is ", 1000, "). Sorry for these limits!" }), false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
			}
			else
			{
				this.CreateNewPartGuidsToAvoidClash(returnThing.GetComponent<Thing>());
				foreach (ThingPart thingPart in componentsInChildren)
				{
					int currentState = thingPart.currentState;
					for (int j = 0; j < thingPart.states.Count; j++)
					{
						thingPart.currentState = j;
						thingPart.transform.parent = returnThing.transform;
						thingPart.SetTransformPropertiesByState(false, false);
						thingPart.transform.parent = CreationHelper.thingBeingEdited.transform;
						thingPart.SetStatePropertiesByTransform(false);
					}
					thingPart.currentState = currentState;
					thingPart.SetTransformPropertiesByState(false, false);
				}
				if (!string.IsNullOrEmpty(thisThingThatWasClonedFromIdIfRelevant) && componentsInChildren.Length >= 2)
				{
					CreationHelper.thingThatWasClonedFromIdIfRelevant = thisThingThatWasClonedFromIdIfRelevant;
				}
				global::UnityEngine.Object.Destroy(returnThing.gameObject);
				Managers.soundManager.Play("success", this.transform, 0.4f, false, false);
				this.dialogToSwitchToAfter = DialogType.Create;
				this.RemoveOriginalPlacementIfNeeded();
			}
		}, true, false, -1, null));
		yield break;
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00062EC4 File Offset: 0x000612C4
	private void CreateNewPartGuidsToAvoidClash(Thing otherThing)
	{
		Component[] componentsInChildren = otherThing.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (!string.IsNullOrEmpty(thingPart.guid))
			{
				string guid = thingPart.guid;
				thingPart.guid = Misc.GetRandomId();
				this.ReplaceThingPartGuidReferences(otherThing, guid, thingPart.guid);
			}
		}
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00062F3C File Offset: 0x0006133C
	private void ReplaceThingPartGuidReferences(Thing thing, string oldGuid, string newGuid)
	{
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren(typeof(ThingPart));
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.autoContinuation != null && thingPart.autoContinuation.fromPartGuid == oldGuid)
			{
				thingPart.autoContinuation.fromPartGuid = newGuid;
			}
		}
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x00062FAC File Offset: 0x000613AC
	private void RemoveOriginalPlacementIfNeeded()
	{
		if (Our.removeOriginalWhenIncluding && !this.thing.isLocked)
		{
			base.CancelInvoke("DoRemoveOriginalPlacementIfNeeded");
			base.Invoke("DoRemoveOriginalPlacementIfNeeded", 0.2f);
		}
		else if (this.dialogToSwitchToAfter != DialogType.None)
		{
			base.SwitchTo(this.dialogToSwitchToAfter, string.Empty);
		}
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x00063010 File Offset: 0x00061410
	private void DoRemoveOriginalPlacementIfNeeded()
	{
		Managers.personManager.DoDeletePlacement(this.thing.gameObject, false);
		Managers.soundManager.Play("delete", this.transform, 0.4f, false, false);
		if (this.dialogToSwitchToAfter != DialogType.None)
		{
			base.SwitchTo(this.dialogToSwitchToAfter, string.Empty);
		}
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x0006306C File Offset: 0x0006146C
	private new void Update()
	{
		if (this.thing == null)
		{
			base.CloseDialog();
		}
		base.ReactToOnClick();
		if (this.wrapper != null)
		{
			base.ReactToOnClickInWrapper(this.wrapper);
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x000630A8 File Offset: 0x000614A8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		Our.SetPreferentialHandSide(this.hand);
		if (contextName != null)
		{
			if (!(contextName == "includeAsSubThing"))
			{
				if (!(contextName == "mergePartsIntoCreation"))
				{
					if (!(contextName == "removeOriginal"))
					{
						if (!(contextName == "back"))
						{
							if (contextName == "close")
							{
								base.CloseDialog();
							}
						}
						else
						{
							Our.thingIdOfInterest = this.thing.thingId;
							base.SwitchTo(DialogType.Thing, string.Empty);
						}
					}
					else
					{
						Our.SetRemoveOriginalWhenIncluding(state);
					}
				}
				else
				{
					global::UnityEngine.Object.Destroy(this.wrapper);
					base.RemoveCloseButton();
					base.StartCoroutine(this.MergePartsIntoCreation(contextId));
				}
			}
			else
			{
				global::UnityEngine.Object.Destroy(this.wrapper);
				base.RemoveCloseButton();
				this.IncludeAsSubThing();
			}
		}
	}

	// Token: 0x040008EA RID: 2282
	public Thing thing;

	// Token: 0x040008EB RID: 2283
	private ThingPart thingPartToAddSubThingTo;

	// Token: 0x040008EC RID: 2284
	private DialogType dialogToSwitchToAfter;
}
