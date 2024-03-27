using System;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class IncludedSubThingsDialog : Dialog
{
	// Token: 0x06000BD8 RID: 3032 RVA: 0x0006471C File Offset: 0x00062B1C
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddGenericHelpButton();
		if (CreationHelper.thingPartWhoseStatesAreEdited == null)
		{
			return;
		}
		this.thingPart = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
		if (this.thingPart == null)
		{
			return;
		}
		base.AddSubThingRelatedInterface(this.thingPart);
		this.ourHead = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		this.ourHead.AddComponent<IncludedSubThingsLines>();
		string text = "Context-laser placement or " + Environment.NewLine + "drop from inventory to include";
		base.AddLabel(text, 0, 20, 0.8f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.UpdateIncludedSubThingsInfo();
		base.AddButton("switchToPlacedSubThings", null, "Placed Sub-Things...", "ButtonCompactNoIcon", 0, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, true, false);
		this.thingPart.ForceMySubThingsVisibleCollidable();
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00064824 File Offset: 0x00062C24
	private void UpdateIncludedSubThingsInfo()
	{
		if (this.currentInfo != null)
		{
			global::UnityEngine.Object.Destroy(this.currentInfo.gameObject);
		}
		if (this.subThingAdjustInfo != null)
		{
			global::UnityEngine.Object.Destroy(this.subThingAdjustInfo.gameObject);
		}
		int num = ((this.thingPart.includedSubThings == null) ? 0 : this.thingPart.includedSubThings.Count);
		TextColor textColor = ((num < 1) ? TextColor.Default : TextColor.Green);
		string text = "Current: " + num;
		if (num >= 1000)
		{
			text += " (limit reached)";
		}
		string text2 = text;
		int num2 = 0;
		int num3 = -110;
		TextColor textColor2 = textColor;
		this.currentInfo = base.AddLabel(text2, num2, num3, 1f, false, textColor2, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		if (num >= 1)
		{
			string text3 = "You can also context-laser & adjust " + Environment.NewLine + "included sub-things now";
			this.subThingAdjustInfo = base.AddLabel(text3, 0, 150, 0.6f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00064944 File Offset: 0x00062D44
	private new void Update()
	{
		if (CreationHelper.thingPartWhoseStatesAreEdited == null || Our.lastTransformHandled == null || CreationHelper.thingPartWhoseStatesAreEdited != Our.lastTransformHandled.gameObject)
		{
			this.Close();
		}
		base.ReactToOnClick();
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x00064998 File Offset: 0x00062D98
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "help"))
			{
				if (!(contextName == "invisible"))
				{
					if (!(contextName == "subThingsFollowDelayed"))
					{
						if (!(contextName == "back"))
						{
							if (!(contextName == "switchToPlacedSubThings"))
							{
								if (contextName == "close")
								{
									this.Close();
								}
							}
							else
							{
								base.SwitchTo(DialogType.PlacedSubThings, string.Empty);
							}
						}
						else
						{
							this.hand.lastContextInfoHit = this.thingPart.gameObject;
							base.SwitchTo(DialogType.ThingPart, string.Empty);
						}
					}
					else
					{
						this.thingPart.subThingsFollowDelayed = state;
					}
				}
				else
				{
					this.thingPart.invisible = state;
				}
			}
			else
			{
				Managers.browserManager.OpenGuideBrowser("yjXGqa9pkuE", null);
			}
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x00064A90 File Offset: 0x00062E90
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

	// Token: 0x06000BDD RID: 3037 RVA: 0x00064ACC File Offset: 0x00062ECC
	private void Close()
	{
		CreationHelper.thingPartWhoseStatesAreEdited = null;
		base.SwitchTo(DialogType.Create, string.Empty);
	}

	// Token: 0x040008FA RID: 2298
	private ThingPart thingPart;

	// Token: 0x040008FB RID: 2299
	private GameObject ourHead;

	// Token: 0x040008FC RID: 2300
	private TextMesh currentInfo;

	// Token: 0x040008FD RID: 2301
	private TextMesh subThingAdjustInfo;
}
