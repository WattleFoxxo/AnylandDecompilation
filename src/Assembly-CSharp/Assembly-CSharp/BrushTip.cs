using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class BrushTip : MonoBehaviour
{
	// Token: 0x060007E1 RID: 2017 RVA: 0x0002B99C File Offset: 0x00029D9C
	private void Start()
	{
		this.renderer = base.gameObject.GetComponent<Renderer>();
		this.isInHand = base.transform.parent != null && base.transform.parent.parent != null && (base.transform.parent.parent.name == "HandCoreLeft" || base.transform.parent.parent.name == "HandCoreRight");
		if (CreationHelper.lastMaterial != null)
		{
			this.renderer.material = CreationHelper.lastMaterial;
		}
		this.renderer.material.color = CreationHelper.currentColor[CreationHelper.currentMaterialTab];
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0002BA79 File Offset: 0x00029E79
	private void Update()
	{
		this.HandleCopyColorToClipboard();
		this.HandlePasteColorFromClipboard();
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0002BA88 File Offset: 0x00029E88
	private void OnTriggerEnter(Collider other)
	{
		if (this.isInHand)
		{
			string tag = other.gameObject.tag;
			if (tag != null)
			{
				if (!(tag == "ThingPart"))
				{
					if (tag == "EnvironmentChanger")
					{
						this.HandleCollisionWithEnvironmentChanger(other);
					}
				}
				else
				{
					this.HandleCollisionWithThingPart(other);
				}
			}
		}
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0002BAF0 File Offset: 0x00029EF0
	private void HandleCollisionWithThingPart(Collider other)
	{
		if (other.transform.parent.gameObject == CreationHelper.thingBeingEdited)
		{
			ThingPart component = other.GetComponent<ThingPart>();
			if (component != null && !component.isLocked)
			{
				ThingPartState thingPartState = component.states[component.currentState];
				switch (CreationHelper.currentMaterialTab)
				{
				case MaterialTab.material:
					component.SetMaterial(this.renderer.material);
					if (!string.IsNullOrEmpty(component.imageUrl))
					{
						component.StartLoadImage();
					}
					break;
				case MaterialTab.texture1:
				case MaterialTab.texture2:
				{
					MaterialDialog materialDialog = this.GetMaterialDialog();
					bool flag = CreationHelper.currentMaterialTab == MaterialTab.texture1;
					int textureIndex = CreationHelper.GetTextureIndex();
					bool flag2 = component.textureTypes[textureIndex] != CreationHelper.textureTypes[textureIndex];
					component.textureTypes[textureIndex] = CreationHelper.textureTypes[textureIndex];
					thingPartState.textureColors[textureIndex] = CreationHelper.currentColor[(!flag) ? MaterialTab.texture2 : MaterialTab.texture1];
					thingPartState.textureProperties[textureIndex] = Managers.thingManager.CloneTextureProperty(materialDialog.textureProperties[textureIndex]);
					this.CloneCurrentTexturePropertyToOtherStatesIfNeeded(component, flag2, textureIndex);
					component.UpdateTextures(true);
					break;
				}
				case MaterialTab.particleSystem:
				{
					MaterialDialog materialDialog = this.GetMaterialDialog();
					bool flag3 = component.particleSystemType != CreationHelper.particleSystemType;
					component.particleSystemType = CreationHelper.particleSystemType;
					thingPartState.particleSystemColor = CreationHelper.currentColor[MaterialTab.particleSystem];
					thingPartState.particleSystemProperty = Managers.thingManager.CloneParticleSystemProperty(materialDialog.particleSystemProperty);
					this.CloneCurrentParticleSystemPropertyToOtherStatesIfNeeded(component, flag3);
					component.UpdateParticleSystem();
					break;
				}
				}
				Managers.soundManager.Play("paint", base.transform, 0.4f, false, false);
				Our.SetLastTransformHandled(other.transform);
			}
		}
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0002BCC0 File Offset: 0x0002A0C0
	private void HandleCollisionWithEnvironmentChanger(Collider other)
	{
		Renderer component = other.gameObject.GetComponent<Renderer>();
		component.material.color = this.renderer.material.color;
		Managers.soundManager.Play("paint", base.transform, 0.4f, false, false);
		Our.SetLastTransformHandled(other.transform);
		Managers.personManager.DoSetEnvironmentChanger(other.gameObject);
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0002BD2C File Offset: 0x0002A12C
	private void CloneCurrentTexturePropertyToOtherStatesIfNeeded(ThingPart part, bool isTypeChange, int textureIndex)
	{
		for (int i = 0; i < part.states.Count; i++)
		{
			if (i != part.currentState)
			{
				ThingPartState thingPartState = part.states[i];
				if (isTypeChange || thingPartState.textureProperties[textureIndex] == null)
				{
					ThingPartState thingPartState2 = part.states[part.currentState];
					thingPartState.textureColors[textureIndex] = thingPartState2.textureColors[textureIndex];
					thingPartState.textureProperties[textureIndex] = Managers.thingManager.CloneTextureProperty(thingPartState2.textureProperties[textureIndex]);
				}
			}
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0002BDD0 File Offset: 0x0002A1D0
	private void CloneCurrentParticleSystemPropertyToOtherStatesIfNeeded(ThingPart part, bool isTypeChange)
	{
		for (int i = 0; i < part.states.Count; i++)
		{
			if (i != part.currentState)
			{
				ThingPartState thingPartState = part.states[i];
				if (isTypeChange || thingPartState.particleSystemProperty == null)
				{
					ThingPartState thingPartState2 = part.states[part.currentState];
					thingPartState.particleSystemColor = thingPartState2.particleSystemColor;
					thingPartState.particleSystemProperty = Managers.thingManager.CloneParticleSystemProperty(thingPartState2.particleSystemProperty);
				}
			}
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0002BE58 File Offset: 0x0002A258
	private MaterialDialog GetMaterialDialog()
	{
		MaterialDialog materialDialog = null;
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null)
		{
			materialDialog = currentNonStartDialog.GetComponent<MaterialDialog>();
		}
		return materialDialog;
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0002BE84 File Offset: 0x0002A284
	private void HandleCopyColorToClipboard()
	{
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.C) && Managers.dialogManager.GetCurrentNonStartDialogType() != DialogType.ThingPart)
		{
			Color32 color = this.renderer.material.color;
			GUIUtility.systemCopyBuffer = string.Concat(new object[] { color.r, ",", color.g, ",", color.b });
			Managers.soundManager.Play("pickUp", base.transform, 0.5f, false, false);
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0002BF3C File Offset: 0x0002A33C
	private void HandlePasteColorFromClipboard()
	{
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.V))
		{
			string systemCopyBuffer = GUIUtility.systemCopyBuffer;
			Color color = this.renderer.material.color;
			Color color2;
			if (ColorUtility.TryParseHtmlString(systemCopyBuffer, out color2))
			{
				color = color2;
				color.a = 1f;
			}
			else
			{
				string[] array = Misc.Split(systemCopyBuffer, ",", StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 3)
				{
					color = new Color32(this.ParseColorPartValue(array[0]), this.ParseColorPartValue(array[1]), this.ParseColorPartValue(array[2]), byte.MaxValue);
				}
			}
			if (color != this.renderer.material.color)
			{
				this.renderer.material.color = color;
				CreationHelper.currentColor[CreationHelper.currentMaterialTab] = this.renderer.material.color;
			}
			Managers.soundManager.Play("putDown", base.transform, 0.5f, false, false);
		}
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x0002C040 File Offset: 0x0002A440
	private byte ParseColorPartValue(string part)
	{
		byte b = 0;
		byte b2;
		if (byte.TryParse(part, out b2))
		{
			b = b2;
		}
		return b;
	}

	// Token: 0x040005C8 RID: 1480
	private Renderer renderer;

	// Token: 0x040005C9 RID: 1481
	private bool isInHand;
}
