using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000142 RID: 322
public class ThingPartAttributesMoreDialog : Dialog
{
	// Token: 0x06000C3B RID: 3131 RVA: 0x0006D27C File Offset: 0x0006B67C
	public void Start()
	{
		base.Init(base.gameObject, false, false, true);
		base.AddFundament();
		base.AddBackButton();
		base.AddSideHeadline("More");
		this.thingPart = this.hand.lastContextInfoHit.GetComponent<ThingPart>();
		this.AddAttributes();
		base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0006D2E8 File Offset: 0x0006B6E8
	private void AddAttributes()
	{
		this.RemoveAttributes();
		int num = -420;
		int num2 = 0;
		if (this.currentPage == 1)
		{
			this.attributeButtons.Add(base.AddCheckbox("isAngleLocker", null, "Snap parts to this angle", 0, num + num2 * 115, this.thingPart.isAngleLocker, 0.85f, "Checkbox", TextColor.Default, null, ExtraIcon.IsAngleLocker));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isAngleLocker_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("isPositionLocker", null, "Snap parts to this position", 0, num + num2 * 115, this.thingPart.isPositionLocker, 0.85f, "Checkbox", TextColor.Default, null, ExtraIcon.IsPositionLocker));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isPositionLocker_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("isFishEyeCamera", null, "Fish eye camera", 0, num + num2 * 115, this.thingPart.isFishEyeCamera, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.FishEyeCamera));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isFishEyeCamera_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("looselyCoupledParticles", null, "Loosely coupled particles", 0, num + num2 * 115, this.thingPart.looselyCoupledParticles, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("looselyCoupledParticles_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("omitAutoSounds", null, "Omit auto-sounds", 0, num + num2 * 115, this.thingPart.omitAutoSounds, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("omitAutoSounds_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("personalExperience", null, "Personal experience", 0, num + num2 * 115, this.thingPart.personalExperience, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("personalExperience_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 2)
		{
			this.attributeButtons.Add(base.AddCheckbox("doSnapTextureAngles", null, "Snap texture angles", 0, num + num2 * 115, this.thingPart.doSnapTextureAngles, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("doSnapTextureAngles_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("textureScalesUniformly", null, "Texture scales uniformly", 0, num + num2 * 115, this.thingPart.textureScalesUniformly, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("textureScalesUniformly_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("avoidCastShadow", null, "Doesn't cast shadow", 0, num + num2 * 115, this.thingPart.avoidCastShadow, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.AvoidCastShadow));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("avoidCastShadow_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("avoidReceiveShadow", null, "Doesn't receive shadow", 0, num + num2 * 115, this.thingPart.avoidReceiveShadow, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.AvoidReceiveShadow));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("avoidReceiveShadow_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("lightOmitsShadow", null, "Light omits shadow", 0, num + num2 * 115, this.thingPart.lightOmitsShadow, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("lightOmitsShadow_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 3)
		{
			this.attributeButtons.Add(base.AddCheckbox("isImagePasteScreen", null, "Shows image pastes", 0, num + num2 * 115, this.thingPart.isImagePasteScreen, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isImagePasteScreen_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("allowBlackImageBackgrounds", null, "Allow black image backs", 0, num + num2 * 115, this.thingPart.allowBlackImageBackgrounds, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("allowBlackImageBackgrounds_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("invisibleToUsWhenAttached", null, "Invisible to me when worn", 0, num + num2 * 115, this.thingPart.invisibleToUsWhenAttached, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("invisibleToUsWhenAttached_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddButton("name", null, "Name...", "ButtonCompact", 0, num + num2 * 115, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("name_help", num + num2 * 115));
			num2++;
		}
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0006D8B4 File Offset: 0x0006BCB4
	private void RemoveAttributes()
	{
		foreach (GameObject gameObject in this.attributeButtons)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.attributeButtons = new List<GameObject>();
		base.RemoveSymmetryButtons();
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x0006D920 File Offset: 0x0006BD20
	private new void Update()
	{
		base.ReactToOnClick();
		if (this.thingPart == null)
		{
			this.Close();
		}
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0006D940 File Offset: 0x0006BD40
	private void RemoveCurrentAngleLockerIfNeeded()
	{
		Component[] componentsInChildren = this.thingPart.transform.parent.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.isAngleLocker)
			{
				thingPart.isAngleLocker = false;
				break;
			}
		}
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x0006D99C File Offset: 0x0006BD9C
	private void RemoveCurrentPositionLockerIfNeeded()
	{
		Component[] componentsInChildren = this.thingPart.transform.parent.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.isPositionLocker)
			{
				thingPart.isPositionLocker = false;
				break;
			}
		}
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x0006D9F5 File Offset: 0x0006BDF5
	private void Close()
	{
		CreationHelper.thingPartWhoseStatesAreEdited = null;
		base.SwitchTo(DialogType.Create, string.Empty);
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x0006DA0C File Offset: 0x0006BE0C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "previousPage":
		{
			base.HideHelpLabel();
			int num = --this.currentPage;
			if (num < 1)
			{
				this.currentPage = this.maxPages;
			}
			this.AddAttributes();
			break;
		}
		case "nextPage":
		{
			base.HideHelpLabel();
			int num = ++this.currentPage;
			if (num > this.maxPages)
			{
				this.currentPage = 1;
			}
			this.AddAttributes();
			break;
		}
		case "isAngleLocker":
			if (state)
			{
				this.RemoveCurrentAngleLockerIfNeeded();
			}
			this.thingPart.isAngleLocker = state;
			break;
		case "isPositionLocker":
			if (state)
			{
				this.RemoveCurrentPositionLockerIfNeeded();
			}
			this.thingPart.isPositionLocker = state;
			break;
		case "isFishEyeCamera":
			this.thingPart.isFishEyeCamera = state;
			if (this.thingPart.isFishEyeCamera)
			{
				this.thingPart.isCamera = true;
			}
			break;
		case "isCameraButton":
			this.thingPart.isCameraButton = state;
			break;
		case "isSlideshowButton":
			this.thingPart.isSlideshowButton = state;
			if (state && this.thingPart.isVideoButton)
			{
				this.thingPart.isVideoButton = false;
			}
			break;
		case "looselyCoupledParticles":
			this.thingPart.looselyCoupledParticles = state;
			this.thingPart.RemoveParticleSystems();
			this.thingPart.UpdateParticleSystem();
			break;
		case "omitAutoSounds":
			this.thingPart.omitAutoSounds = state;
			break;
		case "personalExperience":
			this.thingPart.personalExperience = state;
			break;
		case "doSnapTextureAngles":
			this.thingPart.doSnapTextureAngles = state;
			break;
		case "textureScalesUniformly":
			this.thingPart.textureScalesUniformly = state;
			break;
		case "isImagePasteScreen":
			this.thingPart.isImagePasteScreen = state;
			break;
		case "allowBlackImageBackgrounds":
			this.thingPart.allowBlackImageBackgrounds = state;
			break;
		case "invisibleToUsWhenAttached":
			this.thingPart.invisibleToUsWhenAttached = state;
			break;
		case "avoidCastShadow":
			this.thingPart.avoidCastShadow = state;
			this.thingPart.GetComponent<Renderer>().shadowCastingMode = ((!this.thingPart.avoidCastShadow) ? ShadowCastingMode.On : ShadowCastingMode.Off);
			break;
		case "avoidReceiveShadow":
			this.thingPart.avoidReceiveShadow = state;
			this.thingPart.GetComponent<Renderer>().receiveShadows = !state;
			break;
		case "lightOmitsShadow":
			this.thingPart.lightOmitsShadow = state;
			this.thingPart.GetMyRootThing().SetLightShadows(true, null);
			break;
		case "name":
		{
			DialogManager dialogManager = Managers.dialogManager;
			Action<string> action = delegate(string text)
			{
				if (this.thingPart != null)
				{
					this.thingPart.givenName = ((!string.IsNullOrEmpty(text)) ? text : null);
					for (int i = 0; i < this.thingPart.states.Count; i++)
					{
						this.thingPart.states[0].name = this.thingPart.givenName;
					}
				}
				base.SwitchTo(DialogType.ThingPartAttributesMore, string.Empty);
			};
			string text2 = ((this.thingPart.givenName != null) ? this.thingPart.givenName : string.Empty);
			dialogManager.GetInput(action, "editThingPartName", text2, 100, string.Empty, false, false, false, false, 1f, false, false, null, false);
			break;
		}
		case "name_help":
			base.ToggleHelpLabel("Names this part. Can be changed via the \"call me\" command.", -700, 1f, 50, 0.7f);
			break;
		case "isFishEyeCamera_help":
			base.ToggleHelpLabel("Similar to the Camera option, but with a wider view", -700, 1f, 50, 0.7f);
			break;
		case "isAngleLocker_help":
			base.ToggleHelpLabel("Makes all other parts of this thing snap to this rotation when moved", -700, 1f, 50, 0.7f);
			break;
		case "isPositionLocker_help":
			base.ToggleHelpLabel("Makes all other parts of this thing align to this position when moved", -700, 1f, 50, 0.7f);
			break;
		case "isCameraButton_help":
			base.ToggleHelpLabel("When pressed, activates a part of this creation that's set to be a camera", -700, 1f, 50, 0.7f);
			break;
		case "isSlideshowButton_help":
			base.ToggleHelpLabel("When pressed, will offer an image search to play slideshow images on parts of this creation defined to be a slideshow screen", -700, 1f, 50, 0.7f);
			break;
		case "looselyCoupledParticles_help":
			base.ToggleHelpLabel("If this part emits particles, they will after being emitted not move as strictly connected to the part anymore", -700, 1f, 50, 0.7f);
			break;
		case "omitAutoSounds_help":
			base.ToggleHelpLabel("Stops automatically playing sounds like whooshing when something is emitted, when one is transported and so on", -700, 1f, 50, 0.7f);
			break;
		case "personalExperience_help":
			base.ToggleHelpLabel("Scripted personal events like \"When consumed\" or \"When touched\" are not synced to others around, appearing for that person only. Note this is also available as setting for the whole Thing.", -700, 1f, 50, 0.7f);
			break;
		case "doSnapTextureAngles_help":
			base.ToggleHelpLabel("When applying texture materials to this surface, their rotation will snap to steps of 22.5 degrees", -700, 1f, 50, 0.7f);
			break;
		case "textureScalesUniformly_help":
			base.ToggleHelpLabel("When applying texture materials to this surface, they will scale proportionally when resized", -700, 1f, 50, 0.7f);
			break;
		case "avoidCastShadow_help":
			base.ToggleHelpLabel("Will have this part of your creation not cast any shadow onto other objects", -700, 1f, 50, 0.7f);
			break;
		case "avoidReceiveShadow_help":
			base.ToggleHelpLabel("Will have this part of your creation not receive any shadow from other objects", -700, 1f, 50, 0.7f);
			break;
		case "allowBlackImageBackgrounds_help":
			base.ToggleHelpLabel("If not ticked, fully dark shapes with images will automatically become white when loaded to make the image visible.", -700, 1f, 50, 0.7f);
			break;
		case "isImagePasteScreen_help":
			base.ToggleHelpLabel("When pasting an Imgur url using Ctrl+V, shows it on a body part or held item of oneself", -700, 1f, 50, 0.7f);
			break;
		case "invisibleToUsWhenAttached_help":
			base.ToggleHelpLabel("Makes the part invisible (to yourself only) when you wear it. It still reacts to collisions for you. When putting it on, look into the mirror to see if it's showing for others.", -700, 1f, 50, 0.7f);
			break;
		case "lightOmitsShadow_help":
			base.ToggleHelpLabel("If this part is using the Light emission material, it will not cause shadows. This may help optimize speed.", -700, 1f, 50, 0.7f);
			break;
		case "back":
			base.SwitchTo(DialogType.ThingPartAttributes, string.Empty);
			break;
		}
	}

	// Token: 0x04000943 RID: 2371
	private ThingPart thingPart;

	// Token: 0x04000944 RID: 2372
	public int currentPage = 1;

	// Token: 0x04000945 RID: 2373
	private int maxPages = 3;

	// Token: 0x04000946 RID: 2374
	private List<GameObject> attributeButtons = new List<GameObject>();
}
