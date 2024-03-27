using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class IncludedSubThingDialog : Dialog
{
	// Token: 0x06000BCE RID: 3022 RVA: 0x00063EB8 File Offset: 0x000622B8
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Sub-Thing", -370, -460, TextColor.Default, TextAlignment.Left, false);
		if (CreationHelper.thingPartWhoseStatesAreEdited == null || this.hand.lastContextInfoHit == null || this.hand.lastContextInfoHit.transform.parent == null)
		{
			return;
		}
		ThingPart component = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
		foreach (IncludedSubThing includedSubThing in component.includedSubThings)
		{
			if (includedSubThing.assignedObject == this.hand.lastContextInfoHit)
			{
				this.subThing = includedSubThing;
				break;
			}
		}
		if (this.subThing != null)
		{
			this.thing = this.subThing.assignedObject.GetComponent<Thing>();
			this.AddInfo();
			this.ourHead = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
			this.ourHead.AddComponent<IncludedSubThingsLines>();
			if (Managers.areaManager.weAreEditorOfCurrentArea)
			{
				base.AddButton("removeAndPlace", null, "Remove & place", "ButtonBig", 0, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, true, false);
			}
		}
		else
		{
			this.Close();
		}
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x00064060 File Offset: 0x00062460
	private void AddInfo()
	{
		string text = ((this.subThing.nameOverride == null) ? this.subThing.assignedObject.name : this.subThing.nameOverride);
		int num = -300;
		base.AddModelButton("EditTextButton", "editNameOverride", text, -400, num, false);
		base.AddLabel(text, -355, num - 8, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		string text2 = "In your main thing, you can use \"When told [e.g. pressed] by " + text + " ...\" for any tells from the sub-thing";
		if (!this.thing.isHoldable)
		{
			string[] tellsOfSubThing = this.GetTellsOfSubThing();
			if (tellsOfSubThing.Length >= 1)
			{
				text2 = string.Concat(new string[]
				{
					"In your main thing, you can use \"When told ",
					Misc.Truncate(string.Join("/ ", tellsOfSubThing), 70, true),
					" by ",
					text,
					" ...\""
				});
			}
		}
		text2 = Misc.WrapWithNewlines(text2, 40, -1);
		base.AddLabel(text2, -355, num + 60, 0.7f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		num = 150;
		int num2 = 0;
		base.AddCheckbox("isHoldable", null, "Holdable", 0, num + num2++ * 115, this.thing.isHoldable, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckbox("invisible", null, "Invisible", 0, num + num2++ * 115, this.thing.invisible, 1f, "Checkbox", TextColor.Default, "when done", ExtraIcon.None);
		base.AddCheckbox("uncollidable", null, "Uncollidable", 0, num + num2++ * 115, this.thing.uncollidable, 1f, "Checkbox", TextColor.Default, "when done", ExtraIcon.None);
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00064230 File Offset: 0x00062630
	private string[] GetTellsOfSubThing()
	{
		List<string> list = new List<string>();
		Component[] componentsInChildren = this.thing.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			foreach (ThingPartState thingPartState in thingPart.states)
			{
				foreach (StateListener stateListener in thingPartState.listeners)
				{
					if (stateListener.tells != null)
					{
						foreach (KeyValuePair<TellType, string> keyValuePair in stateListener.tells)
						{
							if (keyValuePair.Key == TellType.Self)
							{
								list.Add(keyValuePair.Value);
							}
						}
					}
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0006437C File Offset: 0x0006277C
	private new void Update()
	{
		if (this.subThing == null || this.subThing.assignedObject == null)
		{
			this.Close();
		}
		base.ReactToOnClick();
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x000643AC File Offset: 0x000627AC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "editNameOverride":
		{
			DialogManager dialogManager = Managers.dialogManager;
			Action<string> action = delegate(string text)
			{
				if (this.subThing != null)
				{
					if (text != null)
					{
						this.subThing.nameOverride = text;
						this.thing.includedSubThingNameOverride = this.subThing.nameOverride;
					}
					base.SwitchTo(DialogType.IncludedSubThing, string.Empty);
				}
				else
				{
					base.CloseDialog();
				}
			};
			dialogManager.GetInput(action, string.Empty, contextId, 120, string.Empty, false, false, false, false, 1f, false, false, null, false);
			break;
		}
		case "isHoldable":
			this.thing.isHoldable = state;
			this.subThing.invert_isHoldable = this.thing.isHoldable != this.subThing.original_isHoldable;
			break;
		case "invisible":
			this.thing.invisible = state;
			this.subThing.invert_invisible = this.thing.invisible != this.subThing.original_invisible;
			break;
		case "uncollidable":
			this.thing.uncollidable = state;
			this.subThing.invert_uncollidable = this.thing.uncollidable != this.subThing.original_uncollidable;
			break;
		case "removeAndPlace":
			global::UnityEngine.Object.Destroy(thisButton);
			base.StartCoroutine(Managers.personManager.DoPlaceThing(this.thing.thingId, this.thing.transform.position, this.thing.transform.rotation));
			base.Invoke("RemoveAsIncludedSubThing", 0.1f);
			break;
		case "back":
			base.SwitchTo(DialogType.IncludedSubThings, string.Empty);
			break;
		case "switchToIncludedSubThings":
			base.SwitchTo(DialogType.IncludedSubThings, string.Empty);
			break;
		case "switchToPlacedSubThings":
			base.SwitchTo(DialogType.PlacedSubThings, string.Empty);
			break;
		case "close":
			this.Close();
			break;
		}
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00064600 File Offset: 0x00062A00
	private void RemoveAsIncludedSubThing()
	{
		Effects.SpawnCrumbles(this.thing.gameObject);
		Managers.soundManager.Play("delete", this.thing.transform, 0.4f, false, false);
		ThingPart component = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
		component.RemoveThingAsIncludedSubThing(this.thing, true);
		base.SwitchTo(DialogType.IncludedSubThings, string.Empty);
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00064664 File Offset: 0x00062A64
	private void OnDestroy()
	{
		if (this.ourHead != null)
		{
			IncludedSubThingsLines component = this.ourHead.GetComponent<IncludedSubThingsLines>();
			if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x000646A0 File Offset: 0x00062AA0
	private void Close()
	{
		CreationHelper.thingPartWhoseStatesAreEdited = null;
		base.SwitchTo(DialogType.Create, string.Empty);
	}

	// Token: 0x040008F6 RID: 2294
	public IncludedSubThing subThing;

	// Token: 0x040008F7 RID: 2295
	private GameObject ourHead;

	// Token: 0x040008F8 RID: 2296
	private Thing thing;
}
