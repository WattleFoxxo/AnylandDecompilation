using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class ApproveBodyDialog : Dialog
{
	// Token: 0x06000ABB RID: 2747 RVA: 0x00054520 File Offset: 0x00052920
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		if (!string.IsNullOrEmpty(this.attachThingIdAsHead) || this.clearBody)
		{
			Our.SetMode(EditModes.Body, false);
			base.AddMirror();
			this.MemorizeCurrentAttachments();
			Managers.soundManager.Play("putDown", this.transform, 1f, true, false);
			base.Invoke("StopHapticPulse", 1f);
			if (this.clearBody)
			{
				this.ClearBody();
				this.AddTextAndButtons("Your body was reset");
			}
			else if (!string.IsNullOrEmpty(this.attachThingIdAsHead))
			{
				base.StartCoroutine(this.AttachHead());
			}
		}
		else
		{
			base.SwitchTo(this.dialogToGoBackTo, string.Empty);
		}
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x000545F4 File Offset: 0x000529F4
	private void MemorizeCurrentAttachments()
	{
		this.attachmentIdData = new Dictionary<string, AttachmentData>();
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig");
		Component[] componentsInChildren = @object.GetComponentsInChildren(typeof(AttachmentPoint), true);
		foreach (Component component in componentsInChildren)
		{
			AttachmentPoint component2 = component.GetComponent<AttachmentPoint>();
			if (component2.name.IndexOf("Hand") != 0)
			{
				GameObject attachedThing = component2.attachedThing;
				if (attachedThing != null)
				{
					this.attachmentIdData.Add(component2.name, component2.GetAttachmentData());
				}
			}
		}
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x000546A4 File Offset: 0x00052AA4
	private void ClearBody()
	{
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig");
		Component[] componentsInChildren = @object.GetComponentsInChildren(typeof(AttachmentPoint), true);
		foreach (Component component in componentsInChildren)
		{
			AttachmentPoint component2 = component.GetComponent<AttachmentPoint>();
			if (component2.attachedThing != null)
			{
				Managers.personManager.DoRemoveAttachedThing(component2.gameObject);
			}
		}
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x00054720 File Offset: 0x00052B20
	private IEnumerator AttachHead()
	{
		GameObject thingObject = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.ApproveBodyDialogHead, this.attachThingIdAsHead, delegate(GameObject returnThingObject)
		{
			thingObject = returnThingObject;
		}, false, false, -1, null));
		if (thingObject != null)
		{
			Thing component = thingObject.GetComponent<Thing>();
			GameObject attachmentPointHead = Managers.personManager.ourPerson.AttachmentPointHead;
			component.transform.position = attachmentPointHead.transform.position;
			component.transform.rotation = attachmentPointHead.transform.rotation;
			Managers.personManager.DoAttachThing(attachmentPointHead, component.gameObject, false);
			string text = "head";
			if (component.addBodyWhenAttached || component.addBodyWhenAttachedNonClearing)
			{
				text = "body";
				base.StartCoroutine(Managers.thingManager.SetOurCurrentBodyAttachmentsByThing(ThingRequestContext.ApproveBodyDialogBodyParts, component.thingId, component.addBodyWhenAttached, null));
			}
			this.AddTextAndButtons("You've been given this " + text);
		}
		yield break;
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0005473C File Offset: 0x00052B3C
	private void AddTextAndButtons(string text)
	{
		this.textLabel = base.AddLabel(text, 0, -100, 1f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.undoButton = base.AddButton("undo", null, "Undo", "Button", -235, 380, "cross", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		this.keepButton = base.AddButton("ok", null, "Ok", "Button", 235, 380, "checkmark", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x00054800 File Offset: 0x00052C00
	private void StopHapticPulse()
	{
		this.hapticPulseOn = false;
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00054809 File Offset: 0x00052C09
	private new void Update()
	{
		if (this.hapticPulseOn && this.hand != null)
		{
			this.hand.TriggerHapticPulse(600);
		}
		base.ReactToOnClick();
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0005483D File Offset: 0x00052C3D
	private void DisableBodyCollisionsIfNeededAndClose()
	{
		base.DisableCollisionsBetweenPersonBodyAndCharacterController();
		this.Close();
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0005484B File Offset: 0x00052C4B
	private void Close()
	{
		base.RemoveMirror();
		if (this.dialogToGoBackTo != DialogType.OwnProfile)
		{
			Our.SetPreviousMode();
		}
		base.SwitchTo(this.dialogToGoBackTo, string.Empty);
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x00054877 File Offset: 0x00052C77
	private void OnDestroy()
	{
		if (this.undoBodyOnDestroy)
		{
			this.UndoBody();
		}
		base.RemoveMirror();
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00054890 File Offset: 0x00052C90
	private void UndoBody()
	{
		if (this != null)
		{
			Misc.DestroyMultiple(new GameObject[] { this.keepButton, this.undoButton });
			this.textLabel.text = "Undoing...".ToUpper();
		}
		Managers.personManager.RestoreOurMemorizedAttachments(this.attachmentIdData);
		if (this != null)
		{
			Managers.soundManager.Play("pickUp", this.transform, 1f, false, false);
			base.Invoke("DisableBodyCollisionsIfNeededAndClose", 1.25f);
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00054924 File Offset: 0x00052D24
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "undo"))
			{
				if (contextName == "ok")
				{
					this.undoBodyOnDestroy = false;
					base.DisableCollisionsBetweenPersonBodyAndCharacterController();
					Managers.soundManager.Play("success", this.transform, 0.15f, false, false);
					this.Close();
				}
			}
			else
			{
				this.undoBodyOnDestroy = false;
				this.UndoBody();
			}
		}
	}

	// Token: 0x04000824 RID: 2084
	public string attachThingIdAsHead;

	// Token: 0x04000825 RID: 2085
	public DialogType dialogToGoBackTo = DialogType.Start;

	// Token: 0x04000826 RID: 2086
	public bool clearBody;

	// Token: 0x04000827 RID: 2087
	private bool hapticPulseOn = true;

	// Token: 0x04000828 RID: 2088
	private TextMesh textLabel;

	// Token: 0x04000829 RID: 2089
	private GameObject keepButton;

	// Token: 0x0400082A RID: 2090
	private GameObject undoButton;

	// Token: 0x0400082B RID: 2091
	private bool undoBodyOnDestroy = true;

	// Token: 0x0400082C RID: 2092
	private Dictionary<string, AttachmentData> attachmentIdData;
}
