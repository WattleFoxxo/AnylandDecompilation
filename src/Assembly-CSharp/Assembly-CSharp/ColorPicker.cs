using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class ColorPicker : MonoBehaviour
{
	// Token: 0x060007ED RID: 2029 RVA: 0x0002C070 File Offset: 0x0002A470
	private void Start()
	{
		this.renderer = base.gameObject.GetComponent<Renderer>();
		this.isInHand = base.transform.parent != null && base.transform.parent.parent != null && (base.transform.parent.parent.name == "HandCoreLeft" || base.transform.parent.parent.name == "HandCoreRight");
		this.isPipette = base.name.IndexOf("Pipette") >= 0;
		if (this.isInHand)
		{
			this.handScript = base.transform.parent.parent.GetComponent<Hand>();
			GameObject gameObject = Misc.FindObject(base.transform.parent.parent.gameObject, "HandDot");
			this.handDotScript = gameObject.GetComponent<HandDot>();
		}
		else if (!this.isPipette)
		{
			if (CreationHelper.lastMaterial != null)
			{
				this.renderer.material = CreationHelper.lastMaterial;
			}
			this.renderer.material.color = CreationHelper.currentColor[CreationHelper.currentMaterialTab];
		}
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x0002C1CC File Offset: 0x0002A5CC
	private void OnTriggerEnter(Collider other)
	{
		if (!this.isInHand)
		{
			return;
		}
		string tag = other.gameObject.tag;
		if (tag != null)
		{
			if (!(tag == "ThingPart"))
			{
				if (!(tag == "ColorToPick"))
				{
					if (!(tag == "ExpanderColorToPick"))
					{
						if (tag == "HueToPick")
						{
							this.HandlePickHueFromMaterialDialog(other);
						}
					}
					else
					{
						this.HandlePickExpanderColorFromMaterialDialog(other);
					}
				}
				else
				{
					this.HandlePickColorFromMaterialDialog(other);
				}
			}
			else if (this.isPipette)
			{
				this.PickColorFromThingPart(other);
			}
		}
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0002C278 File Offset: 0x0002A678
	private void HandlePickHueFromMaterialDialog(Collider other)
	{
		if (this.hueChangeIsAllowed)
		{
			Color color = other.gameObject.GetComponent<Renderer>().material.color;
			if (!Misc.ColorsAreSame(CreationHelper.lastHueColor, color))
			{
				this.LockHueChange();
				CreationHelper.lastHueColor = color;
				CreationHelper.currentColor[CreationHelper.currentMaterialTab] = CreationHelper.lastHueColor;
				CreationHelper.currentBaseColor[CreationHelper.currentMaterialTab] = CreationHelper.lastHueColor;
				this.renderer.material.color = CreationHelper.lastHueColor;
				if (this.lastHuePickTransform != null)
				{
					this.lastHuePickTransform.localPosition = this.lastHuePickTransformOriginalPosition;
				}
				this.UpdateTipColor();
				Vector3 localPosition = other.transform.localPosition;
				this.lastHuePickTransformOriginalPosition = localPosition;
				localPosition.y = 0.01f;
				other.transform.localPosition = localPosition;
				this.lastHuePickTransform = other.transform;
				GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
				if (currentNonStartDialog != null)
				{
					MaterialDialog component = currentNonStartDialog.GetComponent<MaterialDialog>();
					if (component != null)
					{
						component.AddColorsToPick();
						this.PlayPickColorSound();
						if (Our.mode == EditModes.Body)
						{
							Managers.personManager.DoSetHandsColor(this.renderer.material.color);
						}
					}
				}
			}
		}
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x0002C3B4 File Offset: 0x0002A7B4
	private void HandlePickColorFromMaterialDialog(Collider other)
	{
		Color color = this.renderer.material.color;
		this.renderer.material.color = other.gameObject.GetComponent<Renderer>().material.color;
		Color color2 = this.renderer.material.color;
		CreationHelper.currentColor[CreationHelper.currentMaterialTab] = color2;
		CreationHelper.currentBaseColor[CreationHelper.currentMaterialTab] = color2;
		if (CreationHelper.lastColorPickTransform != null)
		{
			CreationHelper.lastColorPickTransform.localPosition = CreationHelper.lastColorPickTransformOriginalPosition;
		}
		this.PlayPickColorSound();
		Vector3 localPosition = other.transform.localPosition;
		CreationHelper.lastColorPickTransformOriginalPosition = localPosition;
		localPosition.y = 0.01f;
		other.transform.localPosition = localPosition;
		CreationHelper.lastColorPickTransform = other.transform;
		MaterialDialog component = Our.GetCurrentNonStartDialog().GetComponent<MaterialDialog>();
		component.ColorWasPicked(color);
		this.UpdateTipColor();
		if (Our.mode == EditModes.Body)
		{
			Managers.personManager.DoSetHandsColor(this.renderer.material.color);
		}
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x0002C4C0 File Offset: 0x0002A8C0
	private void HandlePickExpanderColorFromMaterialDialog(Collider other)
	{
		Color color = other.gameObject.GetComponent<Renderer>().material.color;
		CreationHelper.currentColor[CreationHelper.currentMaterialTab] = color;
		this.renderer.material.color = color;
		if (CreationHelper.lastExpanderColorPickTransform != null)
		{
			CreationHelper.lastExpanderColorPickTransform.localPosition = CreationHelper.lastExpanderColorPickTransformOriginalPosition;
		}
		this.SetTipColor(this.renderer.material.color);
		this.PlayPickColorSound();
		if (this.lastExpanderColorPickTransform != null)
		{
			this.lastExpanderColorPickTransform.localPosition = this.lastExpanderColorPickTransformOriginalPosition;
		}
		Vector3 localPosition = other.transform.localPosition;
		this.lastExpanderColorPickTransformOriginalPosition = localPosition;
		localPosition.y = 0.01f;
		other.transform.localPosition = localPosition;
		this.lastExpanderColorPickTransform = other.transform;
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0002C598 File Offset: 0x0002A998
	private void PlayPickColorSound()
	{
		Managers.soundManager.Play("pickColor", base.transform, 0.75f, false, false);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x0002C5B8 File Offset: 0x0002A9B8
	private void PickColorFromThingPart(Collider other)
	{
		ThingPart component = other.gameObject.GetComponent<ThingPart>();
		if (component == null)
		{
			return;
		}
		bool flag = other.transform.parent != null && other.transform.parent.tag == "Attachment";
		Renderer component2 = other.gameObject.GetComponent<Renderer>();
		if (!component.invisible && component2 != null && component2.material != null && (!flag || Our.mode != EditModes.Thing) && (!component.isLocked || !component.GetIsOfThingBeingEdited()))
		{
			Color color = Color.white;
			ThingPartState thingPartState = component.states[component.currentState];
			switch (CreationHelper.currentMaterialTab)
			{
			case MaterialTab.material:
				color = component2.material.color;
				break;
			case MaterialTab.texture1:
			case MaterialTab.texture2:
			{
				int textureIndex = CreationHelper.GetTextureIndex();
				MaterialDialog materialDialog = this.GetMaterialDialog();
				if (component.textureTypes.Length > textureIndex && component.textureTypes[textureIndex] != TextureType.None && materialDialog != null && thingPartState.textureProperties.Length > textureIndex && thingPartState.textureProperties[textureIndex] != null)
				{
					color = thingPartState.textureColors[textureIndex];
					CreationHelper.textureTypes[textureIndex] = component.textureTypes[textureIndex];
					materialDialog.textureProperties[textureIndex] = Managers.thingManager.CloneTextureProperty(thingPartState.textureProperties[textureIndex]);
					bool flag2 = Managers.thingManager.IsTextureTypeWithOnlyAlphaSetting(component.textureTypes[textureIndex]);
					CreationHelper.currentTextureProperty[textureIndex] = ((!flag2) ? TextureProperty.ScaleY : TextureProperty.Strength);
					materialDialog.UpdateTypeButtons(true);
					materialDialog.UpdatePropertyInterface();
				}
				break;
			}
			case MaterialTab.particleSystem:
			{
				MaterialDialog materialDialog = this.GetMaterialDialog();
				if (component.particleSystemType != ParticleSystemType.None && materialDialog != null)
				{
					color = thingPartState.particleSystemColor;
					CreationHelper.particleSystemType = component.particleSystemType;
					materialDialog.particleSystemProperty = Managers.thingManager.CloneParticleSystemProperty(thingPartState.particleSystemProperty);
					bool flag3 = Managers.thingManager.IsParticleSystemTypeWithOnlyAlphaSetting(component.particleSystemType);
					CreationHelper.currentParticleSystemProperty = ((!flag3) ? ParticleSystemProperty.Amount : ParticleSystemProperty.Alpha);
					materialDialog.UpdateTypeButtons(true);
					materialDialog.UpdatePropertyInterface();
				}
				break;
			}
			}
			CreationHelper.currentColor[CreationHelper.currentMaterialTab] = color;
			CreationHelper.currentBaseColor[CreationHelper.currentMaterialTab] = color;
			this.UpdateTipColor();
			if (Our.mode == EditModes.Body)
			{
				Managers.personManager.DoSetHandsColor(CreationHelper.currentColor[CreationHelper.currentMaterialTab]);
			}
			this.PlayPickColorSound();
		}
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0002C86C File Offset: 0x0002AC6C
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

	// Token: 0x060007F5 RID: 2037 RVA: 0x0002C895 File Offset: 0x0002AC95
	private void UpdateTipColor()
	{
		this.SetTipColor(CreationHelper.currentColor[CreationHelper.currentMaterialTab]);
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0002C8AC File Offset: 0x0002ACAC
	private void SetTipColor(Color color)
	{
		Transform transform = base.transform.parent.Find("Tip");
		Renderer component = transform.gameObject.GetComponent<Renderer>();
		component.material.color = color;
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x0002C8E8 File Offset: 0x0002ACE8
	private void OnTriggerStay(Collider other)
	{
		if (!this.isInHand)
		{
			return;
		}
		string tag = other.gameObject.tag;
		if (tag != null)
		{
			if (tag == "ThingPart")
			{
				if (other.transform.parent != null && other.transform.parent.tag != "Attachment")
				{
					this.handScript.TriggerHapticPulse(Universe.lowHapticPulse);
				}
				return;
			}
			if (tag == "ColorToPick" || tag == "HueToPick" || tag == "EnvironmentChanger")
			{
				this.handScript.TriggerHapticPulse(Universe.lowHapticPulse);
				return;
			}
		}
		this.handDotScript.HandlePressableDialogPartCollision(other);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x0002C9C8 File Offset: 0x0002ADC8
	private void LockHueChange()
	{
		this.hueChangeIsAllowed = false;
		base.Invoke("UnlockHueChange", 0.1f);
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0002C9E1 File Offset: 0x0002ADE1
	private void UnlockHueChange()
	{
		this.hueChangeIsAllowed = true;
	}

	// Token: 0x040005CA RID: 1482
	private bool isInHand;

	// Token: 0x040005CB RID: 1483
	private Hand handScript;

	// Token: 0x040005CC RID: 1484
	private HandDot handDotScript;

	// Token: 0x040005CD RID: 1485
	private Renderer renderer;

	// Token: 0x040005CE RID: 1486
	private bool isPipette;

	// Token: 0x040005CF RID: 1487
	private Transform lastHuePickTransform;

	// Token: 0x040005D0 RID: 1488
	private Vector3 lastHuePickTransformOriginalPosition;

	// Token: 0x040005D1 RID: 1489
	private Transform lastTexturePickTransform;

	// Token: 0x040005D2 RID: 1490
	private Vector3 lastTexturePickTransformOriginalPosition;

	// Token: 0x040005D3 RID: 1491
	private bool hueChangeIsAllowed = true;

	// Token: 0x040005D4 RID: 1492
	private Transform lastExpanderColorPickTransform;

	// Token: 0x040005D5 RID: 1493
	private Vector3 lastExpanderColorPickTransformOriginalPosition;
}
