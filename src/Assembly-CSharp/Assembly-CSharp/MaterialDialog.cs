using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR;

// Token: 0x0200011E RID: 286
public class MaterialDialog : Dialog
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00051374 File Offset: 0x0004F774
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

	// Token: 0x06000A8B RID: 2699 RVA: 0x000513B0 File Offset: 0x0004F7B0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		if (!MaterialDialog.didLoadColorExpanderShowsSetting)
		{
			MaterialDialog.didLoadColorExpanderShowsSetting = true;
			MaterialDialog.colorExpanderShows = PlayerPrefs.GetInt("colorExpanderShows", 0) == 1;
		}
		this.trackedObj = this.transform.parent.GetComponent<SteamVR_TrackedObject>();
		Transform transform = this.transform.parent.Find("HandDot");
		this.handDot = transform.GetComponent<HandDot>();
		this.side = ((!(this.transform.parent.name == "HandCoreLeft")) ? Side.Right : Side.Left);
		this.parentTransform = base.GetTransform();
		if (Our.mode == EditModes.Thing)
		{
			this.AddTabs();
			this.AddTypePagingButtons();
			this.AddTypeButtons();
			if (Our.lastTransformHandled != null)
			{
				CreationHelper.thingPartWhoseStatesAreEdited = Our.lastTransformHandled.gameObject;
			}
		}
		else
		{
			CreationHelper.currentMaterialTab = MaterialTab.material;
		}
		this.AddColorsToPick();
		this.AddHuesToPick();
		base.AddLabel("Ctrl+C: Copy red-green-blue values\nCtrl+V: Paste rgb like \"255,0,0\" or \"#FF0000\"", 430, 320, 0.8f, true, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.InitTextureProperties();
		this.InitParticleSystemProperties();
		this.CloneTexturePropertiesFromLastHandled();
		this.CloneParticleSystemPropertiesFromLastHandled();
		this.AddPropertySideInterface();
		this.UpdatePropertyInterface();
		this.UpdateTypeButtons(true);
		this.UpdateTabNumberLabels();
		this.SetBrushTipToLastColor();
		this.UpdateColorExpanderButton();
		this.UpdateColorExpander();
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00051524 File Offset: 0x0004F924
	private void AddTabs()
	{
		int num = 0;
		IEnumerator enumerator = Enum.GetValues(typeof(MaterialTab)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				MaterialTab materialTab = (MaterialTab)obj;
				bool flag = materialTab == MaterialTab.texture1 || materialTab == MaterialTab.texture2;
				string text = materialTab.ToString();
				string text2 = ((!flag) ? text : "texture");
				int num2 = -412 + num * 278;
				int num3 = 300;
				GameObject gameObject = base.AddModelButton("MaterialTabs/" + text2, "tab", text, num2, num3, false);
				this.tabs.Add(materialTab, gameObject);
				int num4 = num2 + ((materialTab != MaterialTab.particleSystem) ? 48 : (-95));
				int num5 = num3 - 25;
				TextMesh textMesh = base.AddLabel(string.Empty, num4, num5, 1f, false, TextColor.White, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				textMesh.gameObject.SetActive(false);
				this.tabNumberLabels.Add(materialTab, textMesh);
				num++;
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
		base.SetCheckboxState(this.tabs[CreationHelper.currentMaterialTab], true, true);
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0005167C File Offset: 0x0004FA7C
	private void UpdateColorExpanderButton()
	{
		global::UnityEngine.Object.Destroy(this.colorExpanderButton);
		bool flag = this.backsideWrapper != null;
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		this.colorExpanderButton = base.AddModelButton("ButtonBack", "toggleColorExpander", null, -400, -400, true);
		float num = ((!MaterialDialog.colorExpanderShows) ? 90f : (-90f));
		this.colorExpanderButton.transform.Rotate(new Vector3(0f, num, 0f));
		if (!flag)
		{
			base.RotateBacksideWrapper();
		}
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x00051730 File Offset: 0x0004FB30
	private void UpdateColorExpander()
	{
		if (MaterialDialog.colorExpanderShows)
		{
			if (this.expanderFundament == null)
			{
				this.expanderFundament = base.Add("HelpLabelSide", 0, 0, false).gameObject;
			}
			this.AddExpanderColorsToPick();
		}
		else
		{
			if (this.expanderFundament != null)
			{
				global::UnityEngine.Object.Destroy(this.expanderFundament);
				this.expanderFundament = null;
			}
			global::UnityEngine.Object.Destroy(this.wrapper);
		}
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x000517AC File Offset: 0x0004FBAC
	private void AddPropertySideInterface()
	{
		this.verticalSide = base.Add("VerticalSide", 0, 0, false).gameObject;
		this.propertyDot = this.verticalSide.transform.Find("QuadDot").gameObject;
		if (this.side == Side.Right)
		{
			Misc.SetLocalPositionX(this.propertyDot, Math.Abs(this.propertyDot.transform.localPosition.x));
		}
		this.propertyDot.SetActive(false);
		bool flag = Managers.achievementManager.DidAchieve(Achievement.SlidMaterialProperty);
		if (flag)
		{
			this.propertyDot.SetActive(true);
		}
		this.verticalSide.SetActive(false);
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00051860 File Offset: 0x0004FC60
	private void UpdateSlider(string path)
	{
		Transform transform = this.verticalSide.transform.Find("QuadFrom");
		Managers.thingManager.LoadMaterial(transform.gameObject, "DialogIconMaterials/" + path + "/low");
		Transform transform2 = this.verticalSide.transform.Find("QuadTo");
		Managers.thingManager.LoadMaterial(transform2.gameObject, "DialogIconMaterials/" + path + "/high");
		if (this.side == Side.Right)
		{
			Transform transform3 = this.verticalSide.transform.Find("QuadCenter");
			Misc.SetLocalPositionX(transform, Math.Abs(transform.localPosition.x));
			Misc.SetLocalPositionX(transform2, Math.Abs(transform2.localPosition.x));
			Misc.SetLocalPositionX(transform3, Math.Abs(transform3.localPosition.x));
			Renderer component = transform3.GetComponent<Renderer>();
			component.material.SetTextureScale("_MainTex", new Vector2(-1f, 1f));
		}
		this.UpdatePropertyDotPosition();
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x00051978 File Offset: 0x0004FD78
	public void UpdatePropertyInterface()
	{
		bool flag = CreationHelper.textureTypes[0] != TextureType.None && CreationHelper.currentMaterialTab == MaterialTab.texture1;
		bool flag2 = CreationHelper.textureTypes[1] != TextureType.None && CreationHelper.currentMaterialTab == MaterialTab.texture2;
		bool flag3 = CreationHelper.particleSystemType != ParticleSystemType.None && CreationHelper.currentMaterialTab == MaterialTab.particleSystem;
		if (flag || flag2)
		{
			int textureIndex = CreationHelper.GetTextureIndex();
			if ((CreationHelper.textureTypes[textureIndex] == TextureType.Vertex_Scatter || CreationHelper.textureTypes[textureIndex] == TextureType.Vertex_Expand) && CreationHelper.currentTextureProperty[textureIndex] == TextureProperty.ScaleY)
			{
				CreationHelper.currentTextureProperty[textureIndex] = TextureProperty.Strength;
			}
			this.AddPropertyButtonsIfNeeded();
			this.verticalSide.SetActive(true);
			bool flag4 = Managers.thingManager.IsTextureTypeWithOnlyAlphaSetting(CreationHelper.textureTypes[textureIndex]);
			int num = ((!flag4) ? (-620) : (-480));
			if (this.side == Side.Left)
			{
				num = Math.Abs(num);
			}
			Misc.SetLocalPositionX(this.verticalSide, (float)(num * -1) * this.scaleFactor);
			this.UpdateSlider("MaterialDialogProperty_texture/" + this.GetPropertyGroup(CreationHelper.currentTextureProperty[textureIndex]));
			foreach (KeyValuePair<TextureProperty, GameObject> keyValuePair in this.texturePropertyButtons)
			{
				bool flag5 = keyValuePair.Key == CreationHelper.currentTextureProperty[textureIndex];
				base.SetCheckboxState(keyValuePair.Value, flag5, true);
			}
		}
		else if (flag3)
		{
			this.AddPropertyButtonsIfNeeded();
			this.verticalSide.SetActive(true);
			bool flag6 = Managers.thingManager.IsParticleSystemTypeWithOnlyAlphaSetting(CreationHelper.particleSystemType);
			int num2 = ((!flag6) ? (-620) : (-480));
			if (this.side == Side.Left)
			{
				num2 = Math.Abs(num2);
			}
			Misc.SetLocalPositionX(this.verticalSide, (float)(num2 * -1) * this.scaleFactor);
			this.UpdateSlider("MaterialDialogProperty_particleSystem/" + CreationHelper.currentParticleSystemProperty.ToString());
			foreach (KeyValuePair<ParticleSystemProperty, GameObject> keyValuePair2 in this.particleSystemPropertyButtons)
			{
				bool flag7 = keyValuePair2.Key == CreationHelper.currentParticleSystemProperty;
				base.SetCheckboxState(keyValuePair2.Value, flag7, true);
			}
		}
		else
		{
			this.verticalSide.SetActive(false);
			this.DeleteTexturePropertyButtons();
			this.DeleteParticleSystemPropertyButtons();
			this.currentAddedPropertyButtonsListSignature = string.Empty;
		}
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x00051C34 File Offset: 0x00050034
	private void AddPropertyButtonsIfNeeded()
	{
		int num = -590 * ((this.side != Side.Left) ? (-1) : 1);
		int num2 = 0;
		if (CreationHelper.currentMaterialTab == MaterialTab.texture1 || CreationHelper.currentMaterialTab == MaterialTab.texture2)
		{
			int textureIndex = CreationHelper.GetTextureIndex();
			TextureType textureType = CreationHelper.textureTypes[textureIndex];
			List<TextureProperty> texturePropertiesList = Managers.thingManager.GetTexturePropertiesList(textureType);
			string texturePropertyListSignature = this.GetTexturePropertyListSignature(texturePropertiesList);
			if (texturePropertyListSignature != this.currentAddedPropertyButtonsListSignature)
			{
				this.currentAddedPropertyButtonsListSignature = texturePropertyListSignature;
				this.DeleteTexturePropertyButtons();
				this.DeleteParticleSystemPropertyButtons();
				if (!Managers.thingManager.IsTextureTypeWithOnlyAlphaSetting(textureType))
				{
					int num3 = (int)(800f / (float)(texturePropertiesList.Count - 1));
					foreach (TextureProperty textureProperty in texturePropertiesList)
					{
						int num4 = -400 + num2 * num3;
						int num5 = (int)textureProperty;
						string text = "MaterialDialogProperty_texture/" + this.GetPropertyGroup(textureProperty) + "/button";
						GameObject gameObject = base.AddButton("textureProperty", num5.ToString(), null, "ButtonSmall", num, num4, text, false, 1f, TextColor.Default, 0.725f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
						this.StyleAsPropertyButton(gameObject);
						this.texturePropertyButtons.Add(textureProperty, gameObject);
						num2++;
					}
				}
			}
		}
		else if (CreationHelper.currentMaterialTab == MaterialTab.particleSystem)
		{
			ParticleSystemType particleSystemType = CreationHelper.particleSystemType;
			List<ParticleSystemProperty> particleSystemPropertiesList = Managers.thingManager.GetParticleSystemPropertiesList(particleSystemType);
			string particleSystemPropertyListSignature = this.GetParticleSystemPropertyListSignature(particleSystemPropertiesList);
			if (particleSystemPropertyListSignature != this.currentAddedPropertyButtonsListSignature)
			{
				this.currentAddedPropertyButtonsListSignature = particleSystemPropertyListSignature;
				this.DeleteTexturePropertyButtons();
				this.DeleteParticleSystemPropertyButtons();
				if (!Managers.thingManager.IsParticleSystemTypeWithOnlyAlphaSetting(particleSystemType))
				{
					int num3 = 157;
					IEnumerator enumerator2 = Enum.GetValues(typeof(ParticleSystemProperty)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj = enumerator2.Current;
							ParticleSystemProperty particleSystemProperty = (ParticleSystemProperty)obj;
							int num6 = -400 + num2 * num3;
							int num7 = (int)particleSystemProperty;
							string text2 = "MaterialDialogProperty_particleSystem/" + particleSystemProperty.ToString() + "/button";
							GameObject gameObject2 = base.AddButton("particleSystemProperty", num7.ToString(), null, "ButtonSmall", num, num6, text2, false, 1f, TextColor.Default, 0.725f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
							this.StyleAsPropertyButton(gameObject2);
							this.particleSystemPropertyButtons.Add(particleSystemProperty, gameObject2);
							num2++;
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
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00051F0C File Offset: 0x0005030C
	private string GetTexturePropertyListSignature(List<TextureProperty> properties)
	{
		string text = "texture_";
		int textureIndex = CreationHelper.GetTextureIndex();
		if (!Managers.thingManager.IsTextureTypeWithOnlyAlphaSetting(CreationHelper.textureTypes[textureIndex]))
		{
			foreach (TextureProperty textureProperty in properties)
			{
				text = text + textureProperty.ToString() + "_";
			}
		}
		return text;
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x00051F98 File Offset: 0x00050398
	private string GetParticleSystemPropertyListSignature(List<ParticleSystemProperty> properties)
	{
		string text = "particleSystem_";
		if (!Managers.thingManager.IsParticleSystemTypeWithOnlyAlphaSetting(CreationHelper.particleSystemType))
		{
			foreach (ParticleSystemProperty particleSystemProperty in properties)
			{
				text = text + particleSystemProperty.ToString() + "_";
			}
		}
		return text;
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0005201C File Offset: 0x0005041C
	private void DeleteTexturePropertyButtons()
	{
		foreach (KeyValuePair<TextureProperty, GameObject> keyValuePair in this.texturePropertyButtons)
		{
			global::UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		this.texturePropertyButtons = new Dictionary<TextureProperty, GameObject>();
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00052088 File Offset: 0x00050488
	private void DeleteParticleSystemPropertyButtons()
	{
		foreach (KeyValuePair<ParticleSystemProperty, GameObject> keyValuePair in this.particleSystemPropertyButtons)
		{
			global::UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		this.particleSystemPropertyButtons = new Dictionary<ParticleSystemProperty, GameObject>();
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x000520F4 File Offset: 0x000504F4
	private string GetPropertyGroup(TextureProperty property)
	{
		string text = string.Empty;
		switch (property)
		{
		case TextureProperty.Param1:
		case TextureProperty.Param2:
		case TextureProperty.Param3:
			text = "Param";
			break;
		default:
			text = property.ToString();
			break;
		}
		return text;
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00052140 File Offset: 0x00050540
	private void StyleAsPropertyButton(GameObject thisButton)
	{
		DialogPart component = thisButton.GetComponent<DialogPart>();
		component.SetVerticalOffset(-0.005f);
		component.autoStopHighlight = false;
		base.MinifyButton(thisButton, 0.725f, 1f, 0.725f, false);
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00052180 File Offset: 0x00050580
	public void AddColorsToPick()
	{
		Color color = CreationHelper.currentBaseColor[CreationHelper.currentMaterialTab];
		if (this.colorToPickShapes == null)
		{
			this.colorToPickShapes = new GameObject[100];
			this.colorToPickRenderer = new Renderer[100];
		}
		HSBColor hsbcolor = new HSBColor(CreationHelper.lastHueColor);
		float h = hsbcolor.h;
		bool flag = false;
		int num = 0;
		for (float num2 = 0f; num2 < 10f; num2 += 1f)
		{
			for (float num3 = 0f; num3 < 10f; num3 += 1f)
			{
				bool flag2 = this.colorToPickShapes[num] != null;
				if (!flag2)
				{
					this.colorToPickShapes[num] = GameObject.CreatePrimitive(PrimitiveType.Cube);
				}
				GameObject gameObject = this.colorToPickShapes[num];
				if (!flag2)
				{
					gameObject.tag = "ColorToPick";
					gameObject.transform.localScale = new Vector3(0.01825f, 0.01825f, 0.017251f);
					gameObject.transform.parent = this.parentTransform;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.AddComponent<BoxCollider>();
				}
				float num4 = ((num3 <= 0f) ? 0f : (num3 / 9f));
				float num5 = ((num2 <= 0f) ? 0f : (num2 / 9f));
				HSBColor hsbcolor2 = new HSBColor(h, num5, num4);
				Color color2 = HSBColor.ToColor(hsbcolor2);
				Vector3 vector = new Vector3(num2 * 0.01825f - 0.0975f, 0.0125f, num3 * 0.017251f - 0.0504f);
				if (!flag && color == color2)
				{
					flag = true;
					CreationHelper.lastColorPickTransform = gameObject.transform;
					CreationHelper.lastColorPickTransformOriginalPosition = vector;
					vector.y = 0.01f;
				}
				gameObject.transform.localPosition = vector;
				if (!flag2)
				{
					this.colorToPickRenderer[num] = gameObject.GetComponent<Renderer>();
				}
				this.colorToPickRenderer[num].material.color = color2;
				this.colorToPickRenderer[num].shadowCastingMode = ShadowCastingMode.Off;
				num++;
			}
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x000523B0 File Offset: 0x000507B0
	private void AddExpanderColorsToPick()
	{
		global::UnityEngine.Random.InitState(MaterialDialog.expanderColorsRandomizerSeed);
		Color color = CreationHelper.currentColor[CreationHelper.currentMaterialTab];
		Color color2 = CreationHelper.currentBaseColor[CreationHelper.currentMaterialTab];
		HSBColor hsbcolor = new HSBColor(color2);
		int num = 0;
		global::UnityEngine.Object.Destroy(this.wrapper);
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		bool flag = false;
		for (float num2 = 0f; num2 < 20f; num2 += 1f)
		{
			for (float num3 = 0f; num3 < 5f; num3 += 1f)
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				gameObject.tag = "ExpanderColorToPick";
				gameObject.transform.localScale = new Vector3(0.009125f, 0.01825f, 0.0086255f);
				gameObject.transform.parent = this.wrapper.transform;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.AddComponent<BoxCollider>();
				HSBColor hsbcolor2 = new HSBColor(hsbcolor.h, hsbcolor.s, hsbcolor.b);
				hsbcolor2.h += global::UnityEngine.Random.Range(-0.185f, 0.185f);
				hsbcolor2.s += global::UnityEngine.Random.Range(-0.185f, 0.185f);
				hsbcolor2.b += global::UnityEngine.Random.Range(-0.185f, 0.185f);
				if (hsbcolor2.h < 0f)
				{
					hsbcolor2.h = 1f - hsbcolor2.h;
				}
				else if (hsbcolor2.h > 1f)
				{
					hsbcolor2.h -= 1f;
				}
				hsbcolor2.ClampToValid();
				Color color3 = HSBColor.ToColor(hsbcolor2);
				Vector3 vector = new Vector3(num2 * 0.009125f - 0.0975f - 0.0047f, 0.0125f, num3 * 0.0086255f - 0.0504f + 0.184f);
				if (!flag && color == color3)
				{
					flag = true;
					CreationHelper.lastExpanderColorPickTransform = gameObject.transform;
					CreationHelper.lastExpanderColorPickTransformOriginalPosition = vector;
					vector.y = 0.01f;
				}
				gameObject.transform.localPosition = vector;
				Renderer component = gameObject.GetComponent<Renderer>();
				component.material.color = color3;
				component.shadowCastingMode = ShadowCastingMode.Off;
				num++;
			}
		}
		base.SetUiWrapper(base.gameObject);
		Misc.RandomizeRandomizer();
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00052640 File Offset: 0x00050A40
	private void AddHuesToPick()
	{
		int num = 35;
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		dictionary.Add(12, true);
		dictionary.Add(34, true);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (!dictionary.ContainsKey(i))
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				gameObject.tag = "HueToPick";
				float num3 = 0.025f;
				float num4 = 0.013f;
				float num5 = 0.00525f;
				gameObject.transform.localScale = new Vector3(num3, num4, num5);
				gameObject.transform.parent = this.parentTransform;
				gameObject.transform.localPosition = new Vector3(0.0975f, 0.015f, (float)num2 * num5 - 0.057f);
				gameObject.transform.localRotation = Quaternion.identity;
				float num6 = ((i <= 0) ? 0f : ((float)i / (float)num));
				float num7 = 1f;
				float num8 = 1f;
				HSBColor hsbcolor = new HSBColor(num6, num8, num7);
				gameObject.GetComponent<Renderer>().material.color = HSBColor.ToColor(hsbcolor);
				gameObject.AddComponent<BoxCollider>();
				num2++;
			}
		}
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00052768 File Offset: 0x00050B68
	private void AddTypeButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.typeButtons)
		{
			global::UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		this.typeButtons = new Dictionary<string, GameObject>();
		this.typeButtonsPage = 0;
		List<string> list = new List<string>();
		string text = CreationHelper.currentMaterialTab.ToString();
		switch (CreationHelper.currentMaterialTab)
		{
		case MaterialTab.material:
		{
			List<MaterialTypes> customMaterialOrder = this.GetCustomMaterialOrder();
			foreach (MaterialTypes materialTypes in customMaterialOrder)
			{
				list.Add(materialTypes.ToString());
			}
			break;
		}
		case MaterialTab.texture1:
		case MaterialTab.texture2:
			foreach (TextureType textureType in this.textureTypesOrder)
			{
				list.Add(textureType.ToString());
			}
			text = "texture";
			break;
		case MaterialTab.particleSystem:
		{
			IEnumerator enumerator4 = Enum.GetValues(typeof(ParticleSystemType)).GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					object obj = enumerator4.Current;
					ParticleSystemType particleSystemType = (ParticleSystemType)obj;
					if (particleSystemType != ParticleSystemType.None)
					{
						list.Add(particleSystemType.ToString());
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator4 as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
			break;
		}
		}
		bool flag = text == "texture";
		string currentTypeString = this.GetCurrentTypeString();
		int num = 0;
		int num2 = 0;
		foreach (string text2 in list)
		{
			int num3 = -300 + num2 * 120;
			string text3 = "MaterialDialog_" + text + "/";
			string text4;
			if (this.sharedIcons.TryGetValue(text2, out text4))
			{
				text3 += text4;
			}
			else
			{
				text3 += text2;
			}
			string text5 = null;
			GameObject gameObject = base.AddButton(CreationHelper.currentMaterialTab.ToString(), text2, text5, "ButtonSmall", num3, 410, text3, false, 0.2f, TextColor.White, 0.9f, -0.04f, -0.003f, string.Empty, true, false, TextAlignment.Left, false, false);
			gameObject.name = "buttonOfPage" + num;
			base.MinifyButton(gameObject, 0.9f, 1f, 0.9f, false);
			this.typeButtons.Add(text2, gameObject);
			if (text2 == currentTypeString)
			{
				this.typeButtonsPage = num;
			}
			if (++num2 >= 6)
			{
				num++;
				num2 = 0;
			}
		}
		this.typeButtonsMaxPages = Mathf.CeilToInt((float)list.Count / 6f);
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00052B10 File Offset: 0x00050F10
	private string GetCurrentTypeString()
	{
		string text = string.Empty;
		switch (CreationHelper.currentMaterialTab)
		{
		case MaterialTab.material:
			text = CreationHelper.materialType.ToString();
			break;
		case MaterialTab.texture1:
		case MaterialTab.texture2:
			text = CreationHelper.textureTypes[CreationHelper.GetTextureIndex()].ToString();
			break;
		case MaterialTab.particleSystem:
			text = CreationHelper.particleSystemType.ToString();
			break;
		}
		return text;
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00052B94 File Offset: 0x00050F94
	private void AddTypePagingButtons()
	{
		GameObject gameObject = base.AddModelButton("ButtonBack", "previousTypeButtons", null, -412, 409, false);
		GameObject gameObject2 = base.AddModelButton("ButtonForward", "nextTypeButtons", null, 416, 409, false);
		gameObject.transform.localScale = Misc.GetUniformVector3(0.875f);
		gameObject2.transform.localScale = Misc.GetUniformVector3(0.875f);
		this.UseBoxCollider(gameObject);
		this.UseBoxCollider(gameObject2);
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00052C14 File Offset: 0x00051014
	private List<MaterialTypes> GetCustomMaterialOrder()
	{
		List<MaterialTypes> list = new List<MaterialTypes>
		{
			MaterialTypes.Metallic,
			MaterialTypes.Glow,
			MaterialTypes.PointLight,
			MaterialTypes.SpotLight,
			MaterialTypes.Transparent,
			MaterialTypes.Plastic,
			MaterialTypes.Unshiny,
			MaterialTypes.VeryMetallic,
			MaterialTypes.DarkMetallic,
			MaterialTypes.BrightMetallic,
			MaterialTypes.TransparentGlossy,
			MaterialTypes.TransparentGlossyMetallic,
			MaterialTypes.VeryTransparent,
			MaterialTypes.VeryTransparentGlossy,
			MaterialTypes.SlightlyTransparent,
			MaterialTypes.Unshaded,
			MaterialTypes.Inversion,
			MaterialTypes.Brightness,
			MaterialTypes.TransparentTexture,
			MaterialTypes.TransparentGlowTexture
		};
		if (CreationHelper.thingBeingEdited != null && !this.includeOldVersionsParticleMaterials)
		{
			Component[] componentsInChildren = CreationHelper.thingBeingEdited.GetComponentsInChildren(typeof(ThingPart), true);
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (thingPart.materialType == MaterialTypes.Particles || thingPart.materialType == MaterialTypes.ParticlesBig)
				{
					this.includeOldVersionsParticleMaterials = true;
					break;
				}
			}
		}
		if (this.includeOldVersionsParticleMaterials)
		{
			list.Add(MaterialTypes.Particles);
			list.Add(MaterialTypes.ParticlesBig);
		}
		return list;
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00052D5C File Offset: 0x0005115C
	private void UseBoxCollider(GameObject buttonWrapper)
	{
		Transform transform = buttonWrapper.transform.Find("Arrow");
		GameObject gameObject = transform.gameObject;
		global::UnityEngine.Object.Destroy(gameObject.GetComponent<MeshCollider>());
		gameObject.AddComponent<BoxCollider>();
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00052D94 File Offset: 0x00051194
	public void UpdateTypeButtons(bool autoSetPage = false)
	{
		string currentTypeString = this.GetCurrentTypeString();
		if (autoSetPage)
		{
			int num = -1;
			int num2 = 0;
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.typeButtons)
			{
				bool flag = keyValuePair.Key == currentTypeString;
				if (flag)
				{
					num = num2;
					break;
				}
				num2++;
			}
			if (num >= 0)
			{
				this.typeButtonsPage = (int)Mathf.Floor((float)num / 6f);
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.typeButtons)
		{
			GameObject value = keyValuePair2.Value;
			bool flag2 = value.name == "buttonOfPage" + this.typeButtonsPage;
			value.SetActive(flag2);
			bool flag3 = keyValuePair2.Key == currentTypeString;
			base.SetCheckboxState(value, flag3, true);
		}
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00052ECC File Offset: 0x000512CC
	private new void Update()
	{
		this.HandlePropertySliding();
		base.ReactToOnClick();
		if (this.wrapper != null)
		{
			base.ReactToOnClickInWrapper(this.wrapper);
		}
		if (this.backsideWrapper != null)
		{
			base.ReactToOnClickInWrapper(this.backsideWrapper);
		}
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00052F20 File Offset: 0x00051320
	private void HandlePropertySliding()
	{
		if (this.controller == null || !this.controller.connected || !this.verticalSide.activeSelf)
		{
			return;
		}
		if (this.controller.GetPressDown(CrossDevice.button_grab) || this.controller.GetPressDown(CrossDevice.button_grabTip))
		{
			if (this.handDot.currentlyHeldObject == null)
			{
				Managers.soundManager.Play("click", this.transform, 0.1f, false, false);
				this.positionEmptyClickStarted = this.transform.position;
				MaterialTab currentMaterialTab = CreationHelper.currentMaterialTab;
				if (currentMaterialTab != MaterialTab.particleSystem)
				{
					if (currentMaterialTab == MaterialTab.texture1 || currentMaterialTab == MaterialTab.texture2)
					{
						int textureIndex = CreationHelper.GetTextureIndex();
						this.propertyValueWhenEmptyClickStarted = this.textureProperties[textureIndex][CreationHelper.currentTextureProperty[textureIndex]];
					}
				}
				else
				{
					this.propertyValueWhenEmptyClickStarted = this.particleSystemProperty[CreationHelper.currentParticleSystemProperty];
				}
				if (!this.propertyDot.activeSelf)
				{
					this.propertyDot.SetActive(true);
				}
				Managers.thingManager.LoadMaterial(this.propertyDot, "DialogIconMaterials/propertyDotActive");
			}
		}
		else if (this.controller.GetPress(CrossDevice.button_grab) || this.controller.GetPress(CrossDevice.button_grabTip))
		{
			if (this.positionEmptyClickStarted != Vector3.zero)
			{
				float num = this.transform.position.y - this.positionEmptyClickStarted.y;
				int textureIndex2 = CreationHelper.GetTextureIndex();
				MaterialTab currentMaterialTab2 = CreationHelper.currentMaterialTab;
				float num2;
				if (currentMaterialTab2 != MaterialTab.texture1 && currentMaterialTab2 != MaterialTab.texture2)
				{
					if (currentMaterialTab2 == MaterialTab.particleSystem)
					{
						num2 = this.particleSystemProperty[CreationHelper.currentParticleSystemProperty];
					}
				}
				else
				{
					num2 = this.textureProperties[textureIndex2][CreationHelper.currentTextureProperty[textureIndex2]];
				}
				num2 = Mathf.Clamp(this.propertyValueWhenEmptyClickStarted + num * 2f, 0f, 1f);
				float currentAlphaCap = this.GetCurrentAlphaCap();
				bool flag;
				if (currentAlphaCap != -1f)
				{
					float num3 = currentAlphaCap * 0.5f;
					float num4 = Mathf.Clamp(num2, num3, 0.5f + num3);
					flag = num4 != num2;
					num2 = num4;
				}
				else
				{
					flag = num2 <= 0f || num2 >= 1f;
				}
				this.controller.TriggerHapticPulse((!flag) ? Universe.lowHapticPulse : Universe.mediumHapticPulse, EVRButtonId.k_EButton_Axis0);
				MaterialTab currentMaterialTab3 = CreationHelper.currentMaterialTab;
				if (currentMaterialTab3 != MaterialTab.texture1 && currentMaterialTab3 != MaterialTab.texture2)
				{
					if (currentMaterialTab3 == MaterialTab.particleSystem)
					{
						this.particleSystemProperty[CreationHelper.currentParticleSystemProperty] = num2;
						if (CreationHelper.currentParticleSystemProperty == ParticleSystemProperty.Shape)
						{
							this.SetOtherParticleSystemStatesToSameValue();
						}
						this.UpdateConnectedThingPartParticleSystems();
					}
				}
				else
				{
					this.textureProperties[textureIndex2][CreationHelper.currentTextureProperty[textureIndex2]] = num2;
					if (Our.lastTransformHandled != null)
					{
						ThingPart component = Our.lastTransformHandled.GetComponent<ThingPart>();
						if (component != null && component.textureScalesUniformly)
						{
							TextureProperty textureProperty = CreationHelper.currentTextureProperty[textureIndex2];
							if (textureProperty != TextureProperty.ScaleX)
							{
								if (textureProperty == TextureProperty.ScaleY)
								{
									this.textureProperties[textureIndex2][TextureProperty.ScaleX] = num2;
								}
							}
							else
							{
								this.textureProperties[textureIndex2][TextureProperty.ScaleY] = num2;
							}
						}
					}
					this.UpdateConnectedThingPartTextures();
				}
				this.UpdatePropertyDotPosition();
			}
		}
		else if (this.controller.GetPressUp(CrossDevice.button_grab) || this.controller.GetPressUp(CrossDevice.button_grabTip))
		{
			if (this.positionEmptyClickStarted != Vector3.zero)
			{
				Managers.achievementManager.RegisterAchievement(Achievement.SlidMaterialProperty);
				this.positionEmptyClickStarted = Vector3.zero;
				this.SnapTextureRotationValueIfNeeded();
			}
			Managers.thingManager.LoadMaterial(this.propertyDot, "DialogIconMaterials/propertyDotInactive");
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00053338 File Offset: 0x00051738
	private void SnapTextureRotationValueIfNeeded()
	{
		if (Our.lastTransformHandled != null)
		{
			ThingPart component = Our.lastTransformHandled.GetComponent<ThingPart>();
			if (component != null && component.doSnapTextureAngles)
			{
				int textureIndex = CreationHelper.GetTextureIndex();
				if (textureIndex >= 0 && CreationHelper.currentTextureProperty[textureIndex] == TextureProperty.Rotation)
				{
					float num = this.textureProperties[textureIndex][TextureProperty.Rotation];
					if (num != 0f)
					{
						num *= 360f;
						num = Mathf.Round(num / 22.5f) * 22.5f;
						num /= 360f;
						this.textureProperties[textureIndex][TextureProperty.Rotation] = num;
						this.UpdateConnectedThingPartTextures();
						this.UpdatePropertyDotPosition();
					}
				}
			}
		}
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x000533EC File Offset: 0x000517EC
	private float GetCurrentAlphaCap()
	{
		float num = -1f;
		int textureIndex = CreationHelper.GetTextureIndex();
		float num2;
		if (textureIndex >= 0 && CreationHelper.currentTextureProperty[textureIndex] == TextureProperty.Strength && Managers.thingManager.textureAlphaCaps.TryGetValue(CreationHelper.textureTypes[textureIndex], out num2))
		{
			num = num2;
		}
		return num;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0005343C File Offset: 0x0005183C
	private void SetOtherParticleSystemStatesToSameValue()
	{
		if (Our.lastTransformHandled != null)
		{
			ThingPart component = Our.lastTransformHandled.GetComponent<ThingPart>();
			if (component != null && component.particleSystemType == CreationHelper.particleSystemType && component.states.Count >= 2)
			{
				ParticleSystemProperty currentParticleSystemProperty = CreationHelper.currentParticleSystemProperty;
				ThingPartState thingPartState = component.states[component.currentState];
				for (int i = 0; i < component.states.Count; i++)
				{
					if (i != component.currentState)
					{
						component.states[i].particleSystemProperty[currentParticleSystemProperty] = thingPartState.particleSystemProperty[currentParticleSystemProperty];
					}
				}
			}
		}
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x000534F4 File Offset: 0x000518F4
	private void UpdatePropertyDotPosition()
	{
		float num = 0f;
		MaterialTab currentMaterialTab = CreationHelper.currentMaterialTab;
		if (currentMaterialTab != MaterialTab.texture1 && currentMaterialTab != MaterialTab.texture2)
		{
			if (currentMaterialTab == MaterialTab.particleSystem)
			{
				num = this.particleSystemProperty[CreationHelper.currentParticleSystemProperty];
			}
		}
		else
		{
			int textureIndex = CreationHelper.GetTextureIndex();
			num = this.textureProperties[textureIndex][CreationHelper.currentTextureProperty[textureIndex]];
			float currentAlphaCap = this.GetCurrentAlphaCap();
			if (currentAlphaCap != -1f)
			{
				num *= 1f / currentAlphaCap;
				num -= 0.5f;
			}
		}
		Vector3 localPosition = this.propertyDot.transform.localPosition;
		localPosition.z = -0.07f + 0.07f * num * 2f;
		this.propertyDot.transform.localPosition = localPosition;
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x000535C0 File Offset: 0x000519C0
	private void UpdateConnectedThingPartParticleSystems()
	{
		if (Our.lastTransformHandled != null)
		{
			ThingPart component = Our.lastTransformHandled.GetComponent<ThingPart>();
			if (component != null && component.particleSystemType == CreationHelper.particleSystemType)
			{
				IEnumerator enumerator = Enum.GetValues(typeof(ParticleSystemProperty)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						ParticleSystemProperty particleSystemProperty = (ParticleSystemProperty)obj;
						ThingPartState thingPartState = component.states[component.currentState];
						if (thingPartState.particleSystemProperty != null && thingPartState.particleSystemProperty.ContainsKey(particleSystemProperty))
						{
							thingPartState.particleSystemProperty[particleSystemProperty] = this.particleSystemProperty[particleSystemProperty];
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
				component.ApplyParticleSystemColor();
				component.ApplyParticleSystemProperties();
			}
		}
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x000536B4 File Offset: 0x00051AB4
	private void UpdateConnectedThingPartTextures()
	{
		if (Our.lastTransformHandled != null)
		{
			ThingPart component = Our.lastTransformHandled.GetComponent<ThingPart>();
			int textureIndex = CreationHelper.GetTextureIndex();
			if (component != null && component.textureTypes[textureIndex] == CreationHelper.textureTypes[textureIndex])
			{
				ThingPartState thingPartState = component.states[component.currentState];
				IEnumerator enumerator = Enum.GetValues(typeof(TextureProperty)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						TextureProperty textureProperty = (TextureProperty)obj;
						thingPartState.textureProperties[textureIndex][textureProperty] = this.textureProperties[textureIndex][textureProperty];
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
				component.ApplyTextureColor(textureIndex);
				component.ApplyTextureProperties(textureIndex);
			}
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x000537A0 File Offset: 0x00051BA0
	private void UpdateTabNumberLabels()
	{
		foreach (KeyValuePair<MaterialTab, TextMesh> keyValuePair in this.tabNumberLabels)
		{
			MaterialTab key = keyValuePair.Key;
			TextMesh value = keyValuePair.Value;
			bool flag = key == CreationHelper.currentMaterialTab && this.typeButtonsPage >= 1;
			if (flag)
			{
				string text = (this.typeButtonsPage + 1).ToString();
				bool flag2 = key == MaterialTab.particleSystem;
				if (flag2 && text.Length <= 1)
				{
					text = "  " + text;
				}
				value.text = text;
			}
			value.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00053880 File Offset: 0x00051C80
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "tab":
		{
			MaterialTab materialTab = (MaterialTab)Enum.Parse(typeof(MaterialTab), contextId);
			if (materialTab != CreationHelper.currentMaterialTab)
			{
				base.SetCheckboxState(this.tabs[CreationHelper.currentMaterialTab], false, true);
				CreationHelper.currentMaterialTab = materialTab;
				this.AddTypeButtons();
				this.UpdateTypeButtons(true);
				this.UpdateTabNumberLabels();
				this.UpdatePropertyInterface();
				this.SetBrushTipToLastColor();
				Managers.achievementManager.RegisterAchievement(Achievement.SwitchedMaterialTab);
			}
			else
			{
				base.SetCheckboxState(this.tabs[CreationHelper.currentMaterialTab], true, true);
				this.typeButtonsPage = 0;
				this.UpdateTypeButtons(false);
				this.UpdateTabNumberLabels();
			}
			break;
		}
		case "material":
		{
			MaterialTypes materialTypes = (MaterialTypes)Enum.Parse(typeof(MaterialTypes), contextId);
			CreationHelper.materialType = ((!state) ? MaterialTypes.None : materialTypes);
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.typeButtons)
			{
				bool flag = keyValuePair.Key == contextId && state;
				base.SetCheckboxState(keyValuePair.Value, flag, true);
			}
			break;
		}
		case "texture1":
		case "texture2":
		{
			TextureType textureType = (TextureType)Enum.Parse(typeof(TextureType), contextId);
			CreationHelper.textureTypes[CreationHelper.GetTextureIndex()] = ((!state) ? TextureType.None : textureType);
			this.UpdateTypeButtons(false);
			this.InitTextureProperties();
			this.UpdatePropertyInterface();
			break;
		}
		case "particleSystem":
		{
			ParticleSystemType particleSystemType = (ParticleSystemType)Enum.Parse(typeof(ParticleSystemType), contextId);
			CreationHelper.particleSystemType = ((!state) ? ParticleSystemType.None : particleSystemType);
			this.UpdateTypeButtons(false);
			this.InitParticleSystemProperties();
			this.UpdatePropertyInterface();
			break;
		}
		case "particleSystemProperty":
		{
			ParticleSystemProperty particleSystemProperty = (ParticleSystemProperty)int.Parse(contextId);
			if (particleSystemProperty != CreationHelper.currentParticleSystemProperty)
			{
				CreationHelper.currentParticleSystemProperty = particleSystemProperty;
				this.UpdatePropertyInterface();
			}
			else
			{
				GameObject gameObject = this.particleSystemPropertyButtons[CreationHelper.currentParticleSystemProperty];
				base.SetCheckboxState(gameObject, true, true);
			}
			break;
		}
		case "textureProperty":
		{
			int textureIndex = CreationHelper.GetTextureIndex();
			TextureProperty textureProperty = (TextureProperty)int.Parse(contextId);
			if (textureProperty != CreationHelper.currentTextureProperty[textureIndex])
			{
				CreationHelper.currentTextureProperty[textureIndex] = textureProperty;
				this.UpdatePropertyInterface();
			}
			else
			{
				GameObject gameObject2 = this.texturePropertyButtons[CreationHelper.currentTextureProperty[textureIndex]];
				base.SetCheckboxState(gameObject2, true, true);
			}
			break;
		}
		case "previousTypeButtons":
		{
			int num = --this.typeButtonsPage;
			if (num < 0)
			{
				this.typeButtonsPage = this.typeButtonsMaxPages - 1;
			}
			this.UpdateTypeButtons(false);
			this.UpdateTabNumberLabels();
			break;
		}
		case "nextTypeButtons":
		{
			int num = ++this.typeButtonsPage;
			if (num >= this.typeButtonsMaxPages)
			{
				this.typeButtonsPage = 0;
				if (CreationHelper.currentMaterialTab == MaterialTab.material && !this.includeOldVersionsParticleMaterials)
				{
					num = ++this.fullyCircledThroughMaterialTabCount;
					if (num == 5)
					{
						this.includeOldVersionsParticleMaterials = true;
						this.AddTypeButtons();
					}
				}
			}
			this.UpdateTypeButtons(false);
			this.UpdateTabNumberLabels();
			break;
		}
		case "toggleColorExpander":
			MaterialDialog.colorExpanderShows = !MaterialDialog.colorExpanderShows;
			this.UpdateColorExpanderButton();
			this.UpdateColorExpander();
			PlayerPrefs.SetInt("colorExpanderShows", (!MaterialDialog.colorExpanderShows) ? 0 : 1);
			break;
		}
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x00053CAC File Offset: 0x000520AC
	private void SetBrushTipToLastColor()
	{
		string text = "OurPersonRig/HandCore" + ((this.side != Side.Left) ? "Left" : "Right") + "/Brush/Tip";
		GameObject gameObject = GameObject.Find(text);
		Renderer component = gameObject.GetComponent<Renderer>();
		component.material.color = CreationHelper.currentColor[CreationHelper.currentMaterialTab];
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x00053D0C File Offset: 0x0005210C
	private void InitTextureProperties()
	{
		for (int i = 0; i < this.textureProperties.Length; i++)
		{
			this.textureProperties[i] = new Dictionary<TextureProperty, float>();
			Managers.thingManager.SetTexturePropertiesToDefault(this.textureProperties[i], CreationHelper.textureTypes[i]);
			bool flag = Managers.thingManager.IsTextureTypeWithOnlyAlphaSetting(CreationHelper.textureTypes[i]);
			if (flag)
			{
				CreationHelper.currentTextureProperty[i] = TextureProperty.Strength;
			}
		}
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x00053D78 File Offset: 0x00052178
	private void InitParticleSystemProperties()
	{
		this.particleSystemProperty = new Dictionary<ParticleSystemProperty, float>();
		Managers.thingManager.SetParticleSystemPropertiesToDefault(this.particleSystemProperty, CreationHelper.particleSystemType);
		bool flag = Managers.thingManager.IsParticleSystemTypeWithOnlyAlphaSetting(CreationHelper.particleSystemType);
		if (flag)
		{
			CreationHelper.currentParticleSystemProperty = ParticleSystemProperty.Alpha;
		}
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00053DC4 File Offset: 0x000521C4
	private void CloneTexturePropertiesFromLastHandled()
	{
		ThingPart thingPart = ((!(Our.lastTransformHandled != null)) ? null : Our.lastTransformHandled.GetComponent<ThingPart>());
		if (thingPart != null)
		{
			for (int i = 0; i < thingPart.textureTypes.Length; i++)
			{
				if (thingPart.textureTypes[i] != TextureType.None && thingPart.textureTypes[i] == CreationHelper.textureTypes[i])
				{
					ThingPartState thingPartState = thingPart.states[thingPart.currentState];
					MaterialTab materialTab = ((i != 0) ? MaterialTab.texture2 : MaterialTab.texture1);
					CreationHelper.currentColor[materialTab] = thingPartState.textureColors[i];
					this.textureProperties[i] = Managers.thingManager.CloneTextureProperty(thingPartState.textureProperties[i]);
				}
			}
		}
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00053E90 File Offset: 0x00052290
	private void CloneParticleSystemPropertiesFromLastHandled()
	{
		ThingPart thingPart = ((!(Our.lastTransformHandled != null)) ? null : Our.lastTransformHandled.GetComponent<ThingPart>());
		if (thingPart != null && thingPart.particleSystemType != ParticleSystemType.None)
		{
			CreationHelper.particleSystemType = thingPart.particleSystemType;
			ThingPartState thingPartState = thingPart.states[thingPart.currentState];
			CreationHelper.currentColor[MaterialTab.particleSystem] = thingPartState.particleSystemColor;
			this.particleSystemProperty = Managers.thingManager.CloneParticleSystemProperty(thingPartState.particleSystemProperty);
		}
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x00053F1C File Offset: 0x0005231C
	public void ColorWasPicked(Color previousColor)
	{
		bool flag = CreationHelper.currentBaseColor[CreationHelper.currentMaterialTab] != previousColor;
		if (flag)
		{
			this.colorExpanderMayBeReshuffled = true;
			base.CancelInvoke("SetColorExpanderMayBeReshuffled");
		}
		if (this.colorExpanderMayBeReshuffled)
		{
			MaterialDialog.expanderColorsRandomizerSeed = global::UnityEngine.Random.Range(1, 100000);
			this.UpdateColorExpander();
			base.CancelInvoke("SetColorExpanderMayBeReshuffled");
			this.colorExpanderMayBeReshuffled = false;
			base.Invoke("SetColorExpanderMayBeReshuffled", 0.2f);
		}
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00053F9A File Offset: 0x0005239A
	private void SetColorExpanderMayBeReshuffled()
	{
		this.colorExpanderMayBeReshuffled = true;
	}

	// Token: 0x040007F3 RID: 2035
	private Transform parentTransform;

	// Token: 0x040007F4 RID: 2036
	private GameObject colorsToPickWrapper;

	// Token: 0x040007F5 RID: 2037
	private GameObject[] colorToPickShapes;

	// Token: 0x040007F6 RID: 2038
	private Renderer[] colorToPickRenderer;

	// Token: 0x040007F7 RID: 2039
	private const int typeButtonsPerPage = 6;

	// Token: 0x040007F8 RID: 2040
	private int typeButtonsPage;

	// Token: 0x040007F9 RID: 2041
	private int typeButtonsMaxPages = -1;

	// Token: 0x040007FA RID: 2042
	private const string buttonOfPagePrefix = "buttonOfPage";

	// Token: 0x040007FB RID: 2043
	private GameObject verticalSide;

	// Token: 0x040007FC RID: 2044
	private HandDot handDot;

	// Token: 0x040007FD RID: 2045
	private GameObject colorExpanderButton;

	// Token: 0x040007FE RID: 2046
	private static bool colorExpanderShows;

	// Token: 0x040007FF RID: 2047
	private static bool didLoadColorExpanderShowsSetting;

	// Token: 0x04000800 RID: 2048
	private GameObject expanderFundament;

	// Token: 0x04000801 RID: 2049
	private static int expanderColorsRandomizerSeed = 1;

	// Token: 0x04000802 RID: 2050
	private const string colorExpanderShows_key = "colorExpanderShows";

	// Token: 0x04000803 RID: 2051
	private bool colorExpanderMayBeReshuffled = true;

	// Token: 0x04000804 RID: 2052
	private bool includeOldVersionsParticleMaterials;

	// Token: 0x04000805 RID: 2053
	private Dictionary<ParticleSystemProperty, GameObject> particleSystemPropertyButtons = new Dictionary<ParticleSystemProperty, GameObject>();

	// Token: 0x04000806 RID: 2054
	public Dictionary<ParticleSystemProperty, float> particleSystemProperty;

	// Token: 0x04000807 RID: 2055
	private Dictionary<TextureProperty, GameObject> texturePropertyButtons = new Dictionary<TextureProperty, GameObject>();

	// Token: 0x04000808 RID: 2056
	public Dictionary<TextureProperty, float>[] textureProperties = new Dictionary<TextureProperty, float>[2];

	// Token: 0x04000809 RID: 2057
	private Vector3 positionEmptyClickStarted = Vector3.zero;

	// Token: 0x0400080A RID: 2058
	private const float colorShapeWidth = 0.01825f;

	// Token: 0x0400080B RID: 2059
	private const float colorShapeHeight = 0.017251f;

	// Token: 0x0400080C RID: 2060
	private SteamVR_TrackedObject trackedObj;

	// Token: 0x0400080D RID: 2061
	private GameObject propertyDot;

	// Token: 0x0400080E RID: 2062
	private float propertyValueWhenEmptyClickStarted = -1f;

	// Token: 0x0400080F RID: 2063
	private TextMesh typeLabel;

	// Token: 0x04000810 RID: 2064
	private const float typeButtonSize = 0.9f;

	// Token: 0x04000811 RID: 2065
	private const float propertyButtonSize = 0.725f;

	// Token: 0x04000812 RID: 2066
	private Side side;

	// Token: 0x04000813 RID: 2067
	private Dictionary<MaterialTab, GameObject> tabs = new Dictionary<MaterialTab, GameObject>();

	// Token: 0x04000814 RID: 2068
	private Dictionary<string, GameObject> typeButtons = new Dictionary<string, GameObject>();

	// Token: 0x04000815 RID: 2069
	private Dictionary<MaterialTab, TextMesh> tabNumberLabels = new Dictionary<MaterialTab, TextMesh>();

	// Token: 0x04000816 RID: 2070
	private string currentAddedPropertyButtonsListSignature = string.Empty;

	// Token: 0x04000817 RID: 2071
	private int fullyCircledThroughMaterialTabCount;

	// Token: 0x04000818 RID: 2072
	private List<TextureType> textureTypesOrder = new List<TextureType>
	{
		TextureType.Misc_CrackedIce2,
		TextureType.Geometry_Checkerboard,
		TextureType.Geometry_CheckerboardBumpMap,
		TextureType.Pool,
		TextureType.VoronoiPolys,
		TextureType.SideGlow,
		TextureType.Geometry_RadialGradient,
		TextureType.Misc_SoftNoise,
		TextureType.Misc_SoftNoiseBumpMap,
		TextureType.Geometry_Wave,
		TextureType.Misc_Stars,
		TextureType.Misc_StarsBumpMap,
		TextureType.Misc_CottonBalls,
		TextureType.WavyLines,
		TextureType.VoronoiDots,
		TextureType.PerlinNoise1,
		TextureType.QuasiCrystal,
		TextureType.PlasmaNoise,
		TextureType.FractalNoise,
		TextureType.LightSquares,
		TextureType.SweptNoise,
		TextureType.Abstract,
		TextureType.LayeredNoise,
		TextureType.SquareRegress,
		TextureType.Swirly,
		TextureType.WoodGrain,
		TextureType.Gradient,
		TextureType.Geometry_Gradient,
		TextureType.Geometry_DoubleGradient,
		TextureType.Geometry_MultiGradient,
		TextureType.Ground_Spotty,
		TextureType.Ground_SpottyBumpMap,
		TextureType.Ground_LineBumpMap,
		TextureType.Ground_SplitBumpMap,
		TextureType.Ground_Wet,
		TextureType.Ground_Rocky,
		TextureType.Ground_RockyBumpmap,
		TextureType.Ground_BrokenBumpMap,
		TextureType.Ground_Pebbles,
		TextureType.Ground_PebblesBumpMap,
		TextureType.Ground_1BumpMap,
		TextureType.Misc_IceSoft,
		TextureType.Misc_CrackedIce,
		TextureType.Misc_StraightIce,
		TextureType.Misc_CrackedGround,
		TextureType.Misc_LinesPattern,
		TextureType.Misc_Shades,
		TextureType.Misc_StoneGround,
		TextureType.Misc_Lava,
		TextureType.Misc_LavaBumpMap,
		TextureType.Misc_Cork,
		TextureType.Misc_Wool,
		TextureType.Misc_Salad,
		TextureType.Misc_CrossLines,
		TextureType.Misc_Holes,
		TextureType.Misc_1,
		TextureType.Misc_1BumpMap,
		TextureType.Misc_2,
		TextureType.Misc_2BumpMap,
		TextureType.Misc_3,
		TextureType.Misc_3BumpMap,
		TextureType.Misc_4,
		TextureType.Misc_4BumpMap,
		TextureType.Misc_5,
		TextureType.Misc_5BumpMap,
		TextureType.Misc_Glass,
		TextureType.Geometry_Circle_3,
		TextureType.Geometry_Circle_2,
		TextureType.Geometry_Circle_2BumpMap,
		TextureType.Geometry_Circle_1,
		TextureType.Geometry_Half,
		TextureType.Geometry_TiltedHalf,
		TextureType.Geometry_Pyramid,
		TextureType.Geometry_Dots,
		TextureType.Geometry_Border_1,
		TextureType.Geometry_Border_2,
		TextureType.Geometry_Border_2BumpMap,
		TextureType.Geometry_Border_3,
		TextureType.Geometry_Border_4,
		TextureType.Geometry_Rectangles,
		TextureType.Geometry_RectanglesBumpMap,
		TextureType.Geometry_MoreRectangles,
		TextureType.Geometry_RoundBorder,
		TextureType.Geometry_RoundBorderBumpMap,
		TextureType.Geometry_RoundBorder2,
		TextureType.Geometry_Lines,
		TextureType.Geometry_LinesBumpMap,
		TextureType.Geometry_MoreLines,
		TextureType.Geometry_Lines2,
		TextureType.Geometry_Lines2Blurred,
		TextureType.Geometry_Line_1,
		TextureType.Geometry_Line_2,
		TextureType.Geometry_Line_2BumpMap,
		TextureType.Geometry_Line_3,
		TextureType.Geometry_Octagon_1,
		TextureType.Geometry_Octagon_2,
		TextureType.Geometry_Octagon_3,
		TextureType.Geometry_Hexagon2,
		TextureType.Geometry_Hexagon2BumpMap,
		TextureType.Metal_1,
		TextureType.Metal_2,
		TextureType.Metal_Wet,
		TextureType.Marble_1,
		TextureType.Marble_2,
		TextureType.Marble_3,
		TextureType.Tree_1BumpMap,
		TextureType.Tree_2,
		TextureType.Tree_3,
		TextureType.Tree_4,
		TextureType.Grass_4BumpMap,
		TextureType.Grass_3,
		TextureType.Grass_3BumpMap,
		TextureType.Grass_2BumpMap,
		TextureType.Grass_1,
		TextureType.Grass_1BumpMap,
		TextureType.Grass_5,
		TextureType.Grass_5BumpMap,
		TextureType.Grass_6BumpMap,
		TextureType.Wall_1,
		TextureType.Wall_1BumpMap,
		TextureType.Wall_2,
		TextureType.Wall_2BumpMap,
		TextureType.Wall_3BumpMap,
		TextureType.Wall_Rocky,
		TextureType.Wall_RockyBumpMap,
		TextureType.Wall_Freckles,
		TextureType.Wall_FrecklesBumpMap,
		TextureType.Wall_Freckles2,
		TextureType.Wall_Freckles2BumpMap,
		TextureType.Wall_Scratches,
		TextureType.Wall_ScratchesBumpMap,
		TextureType.Wall_Mossy,
		TextureType.Wall_MossyBumpMap,
		TextureType.Wall_Wavy,
		TextureType.Wall_WavyBumpMap,
		TextureType.Wall_Lines,
		TextureType.Wall_LinesBumpMap,
		TextureType.Wall_Lines2,
		TextureType.Wall_Lines2BumpMap,
		TextureType.Wall_Lines3,
		TextureType.Wall_Lines3BumpMap,
		TextureType.Wall_ScratchyLines,
		TextureType.Wall_ScratchyLinesBumpMap,
		TextureType.Cloth_1,
		TextureType.Cloth_2,
		TextureType.Cloth_2BumpMap,
		TextureType.Cloth_3,
		TextureType.Cloth_4,
		TextureType.Cloth_4BumpMap,
		TextureType.Misc_SoftNoiseBumpMapVariant,
		TextureType.Filled,
		TextureType.Bio,
		TextureType.Wireframe,
		TextureType.Outline
	};

	// Token: 0x04000819 RID: 2073
	private Dictionary<string, string> sharedIcons = new Dictionary<string, string>
	{
		{ "Ground_Spotty", "Ground" },
		{ "Ground_SpottyBumpMap", "GroundBumpMap" },
		{ "Ground_LineBumpMap", "GroundBumpMap" },
		{ "Ground_SplitBumpMap", "GroundBumpMap" },
		{ "Ground_Wet", "Ground" },
		{ "Ground_Rocky", "Ground" },
		{ "Ground_RockyBumpmap", "GroundBumpMap" },
		{ "Ground_Broken", "Ground" },
		{ "Ground_BrokenBumpMap", "GroundBumpMap" },
		{ "Ground_1BumpMap", "GroundBumpMap" },
		{ "Ground_Pebbles", "Stones" },
		{ "Ground_PebblesBumpMap", "StonesBumpMap" },
		{ "PerlinNoise1", "Noise" },
		{ "Misc_IceSoft", "Noise" },
		{ "Misc_CrackedGround", "CrackedGround" },
		{ "Misc_LinesPattern", "RoughLines" },
		{ "Misc_StoneGround", "Stones" },
		{ "Misc_Lava", "Lava" },
		{ "Misc_LavaBumpMap", "LavaBumpMap" },
		{ "Misc_StraightIce", "Cracked" },
		{ "Misc_CrackedIce", "Cracked" },
		{ "Misc_CrackedIce2", "Cracked" },
		{ "Misc_Cork", "TightStones" },
		{ "Misc_Shades", "RoughLines" },
		{ "Misc_Wool", "Hair" },
		{ "Misc_Salad", "Mixed" },
		{ "Misc_2", "Marble" },
		{ "Misc_2BumpMap", "MarbleBumpMap" },
		{ "Misc_SoftNoise", "SoftNoise" },
		{ "Misc_SoftNoiseBumpMap", "SoftNoiseBumpMap" },
		{ "Misc_SoftNoiseBumpMapVariant", "SoftNoiseBumpMap" },
		{ "Misc_Stars", "Stars" },
		{ "Misc_StarsBumpMap", "StarsBumpMap" },
		{ "Misc_5", "Marble" },
		{ "Misc_5BumpMap", "MarbleBumpMap" },
		{ "Geometry_Checkerboard", "Checkerboard" },
		{ "Geometry_CheckerboardBumpMap", "CheckerboardBumpMap" },
		{ "Marble_1", "Marble" },
		{ "Marble_2", "Marble" },
		{ "Marble_3", "Marble" },
		{ "Tree_1BumpMap", "TreeBumpMap" },
		{ "Tree_2", "Tree" },
		{ "Tree_3", "Tree" },
		{ "Tree_4", "Tree" },
		{ "Grass_4BumpMap", "GrassBumpMap" },
		{ "Grass_3", "Grass" },
		{ "Grass_3BumpMap", "GrassBumpMap" },
		{ "Grass_2BumpMap", "GrassBumpMap" },
		{ "Grass_1", "Grass" },
		{ "Grass_1BumpMap", "GrassBumpMap" },
		{ "Grass_5", "Grass" },
		{ "Grass_5BumpMap", "GrassBumpMap" },
		{ "Grass_6BumpMap", "GrassBumpMap" },
		{ "Wall_1", "Wall" },
		{ "Wall_1BumpMap", "WallBumpMap" },
		{ "Wall_2", "Wall" },
		{ "Wall_2BumpMap", "WallBumpMap" },
		{ "Wall_3BumpMap", "WallBumpMap" },
		{ "Wall_Rocky", "Wall" },
		{ "Wall_RockyBumpMap", "WallBumpMap" },
		{ "Wall_Freckles", "Wall" },
		{ "Wall_FrecklesBumpMap", "WallBumpMap" },
		{ "Wall_Freckles2", "Wall" },
		{ "Wall_Freckles2BumpMap", "WallBumpMap" },
		{ "Wall_Mossy", "Wall" },
		{ "Wall_MossyBumpMap", "WallBumpMap" },
		{ "Wall_Lines", "Wall_Lines" },
		{ "Wall_LinesBumpMap", "Wall_LinesBumpMap" },
		{ "Wall_Lines2", "Wall_Lines" },
		{ "Wall_Lines2BumpMap", "Wall_LinesBumpMap" },
		{ "Wall_Lines3", "Wall_Lines3" },
		{ "Wall_Lines3BumpMap", "Wall_Lines3BumpMap" },
		{ "Wall_ScratchyLines", "Wall" },
		{ "Wall_ScratchyLinesBumpMap", "WallBumpMap" },
		{ "Cloth_1", "Cloth" },
		{ "Cloth_2", "Cloth" },
		{ "Cloth_2BumpMap", "ClothBumpMap" },
		{ "Cloth_3", "Cloth" },
		{ "Cloth_4", "Cloth" },
		{ "Cloth_4BumpMap", "ClothBumpMap" },
		{ "Geometry_MoreLines", "Geometry_Lines" },
		{ "Geometry_MoreRectangles", "Geometry_Rectangles" },
		{ "Geometry_Gradient", "Gradient" },
		{ "Wireframe", "Wireframe" },
		{ "Outline", "Outline" }
	};
}
