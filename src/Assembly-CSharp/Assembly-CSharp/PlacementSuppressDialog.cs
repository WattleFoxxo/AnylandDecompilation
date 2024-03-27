using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class PlacementSuppressDialog : Dialog
{
	// Token: 0x06000BC0 RID: 3008 RVA: 0x0006355C File Offset: 0x0006195C
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		base.AddHeadline("Enabled", -420, -460, TextColor.Default, TextAlignment.Left, false);
		this.AddPropertyButtons();
		string text = "Disabling placement features can remove unwanted effects & optimize area speed, even when you can't edit the creation";
		base.AddLabel(text, 0, 320, 0.775f, false, TextColor.Gray, false, TextAlignment.Center, 40, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x000635CC File Offset: 0x000619CC
	private void AddPropertyButtons()
	{
		int num = 0;
		string text = string.Empty;
		text = PlacementAttribute.SuppressScriptsAndStates.ToString();
		base.AddCheckbox("toggleProperty", text, "Scripts & States", 0, -280 + num * 115, !this.thing.suppressScriptsAndStates, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("suppressScriptsAndStates_help", -280 + num * 115);
		num++;
		text = PlacementAttribute.SuppressCollisions.ToString();
		base.AddCheckbox("toggleProperty", text, "Collisions", 0, -280 + num * 115, !this.thing.suppressCollisions, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("suppressCollisions_help", -280 + num * 115);
		num++;
		text = PlacementAttribute.SuppressLights.ToString();
		base.AddCheckbox("toggleProperty", text, "Lights", 0, -280 + num * 115, !this.thing.suppressLights, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("suppressLights_help", -280 + num * 115);
		num++;
		text = PlacementAttribute.SuppressParticles.ToString();
		base.AddCheckbox("toggleProperty", text, "Particles", 0, -280 + num * 115, !this.thing.suppressParticles, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("suppressParticles_help", -280 + num * 115);
		num++;
		text = PlacementAttribute.SuppressShowAtDistance.ToString();
		base.AddCheckbox("toggleProperty", text, "Show if far away", 0, -280 + num * 115, !this.thing.suppressShowAtDistance, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.AddCheckboxHelpButton("suppressShowAtDistance_help", -280 + num * 115);
		num++;
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x000637CE File Offset: 0x00061BCE
	private new void Update()
	{
		if (this.thing == null)
		{
			base.CloseDialog();
		}
		base.Update();
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x000637F0 File Offset: 0x00061BF0
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "toggleProperty":
		{
			bool flag = !state;
			PlacementAttribute placementAttribute = (PlacementAttribute)Enum.Parse(typeof(PlacementAttribute), contextId);
			this.thing.SetPlacementAttribute(placementAttribute, flag);
			this.thing = Managers.thingManager.ReCreatePlacementAfterPlacementAttributeChange(this.thing);
			if (flag)
			{
				Managers.areaManager.SetPlacementAttribute(this.thing.placementId, placementAttribute, delegate(bool ok)
				{
					if (!ok)
					{
						Managers.soundManager.Play("no", null, 1f, false, false);
					}
				});
			}
			else
			{
				Managers.areaManager.ClearPlacementAttribute(this.thing.placementId, placementAttribute, delegate(bool ok)
				{
					if (!ok)
					{
						Managers.soundManager.Play("no", null, 1f, false, false);
					}
				});
			}
			Managers.personManager.DoSetPlacementAttribute(this.thing.placementId, placementAttribute, flag);
			break;
		}
		case "suppressScriptsAndStates_help":
			base.ToggleHelpLabel("When off, removes all scripts of this placement and reduces to a single cell only. This can help with optimizing the thing merging.", -700, 1f, 50, 0.7f);
			break;
		case "suppressCollisions_help":
			base.ToggleHelpLabel("When off, makes this placement uncollidable, which can help with optimizing area speed, especially if the thing has many parts.", -700, 1f, 50, 0.7f);
			break;
		case "suppressLights_help":
			base.ToggleHelpLabel("When off, removes all lights from this placement.", -700, 1f, 50, 0.7f);
			break;
		case "suppressParticles_help":
			base.ToggleHelpLabel("When off, removes all particles from this placement.", -700, 1f, 50, 0.7f);
			break;
		case "suppressHoldable_help":
			base.ToggleHelpLabel("When off, turns holdables into non-holdables.", -700, 1f, 50, 0.7f);
			break;
		case "suppressShowAtDistance_help":
			base.ToggleHelpLabel("When off, suppresses special showing of the placement even from far away (which happens if it's e.g. very big, includes Placed Sub-Things, or uses \"Show if far away too\").", -700, 1f, 50, 0.7f);
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x040008ED RID: 2285
	public Thing thing;
}
