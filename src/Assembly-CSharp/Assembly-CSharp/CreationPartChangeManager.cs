using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class CreationPartChangeManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06001059 RID: 4185 RVA: 0x0008BE51 File Offset: 0x0008A251
	// (set) Token: 0x0600105A RID: 4186 RVA: 0x0008BE59 File Offset: 0x0008A259
	public ManagerStatus status { get; private set; }

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x0600105B RID: 4187 RVA: 0x0008BE62 File Offset: 0x0008A262
	// (set) Token: 0x0600105C RID: 4188 RVA: 0x0008BE6A File Offset: 0x0008A26A
	public string failMessage { get; private set; }

	// Token: 0x0600105D RID: 4189 RVA: 0x0008BE73 File Offset: 0x0008A273
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0008BE84 File Offset: 0x0008A284
	public void Apply(string mode, float[] changeValues, bool isForAllParts, bool isLocal, bool isRandom, Transform partWhichApplies, bool omitAutoSounds = false)
	{
		List<Transform> partTransforms = this.GetPartTransforms(isForAllParts);
		foreach (Transform transform in partTransforms)
		{
			if (!(transform == partWhichApplies))
			{
				ThingPart component = transform.GetComponent<ThingPart>();
				bool flag = mode == "color" || mode == "hue" || mode == "saturation" || mode == "lightness";
				bool flag2 = mode.StartsWith("material_");
				if (!component.isLocked && (!component.isUnremovableCenter || flag))
				{
					float[] array = (float[])changeValues.Clone();
					if (isRandom)
					{
						for (int i = 0; i < changeValues.Length; i++)
						{
							array[i] = global::UnityEngine.Random.Range(-changeValues[i], changeValues[i]);
						}
					}
					float num = ((array.Length < 1) ? 0f : array[0]);
					Vector3 zero = Vector3.zero;
					if (array.Length == 3)
					{
						zero = new Vector3(array[0], array[1], array[2]);
					}
					Color color = component.states[component.currentState].color;
					if (mode == "scale")
					{
						isLocal = true;
					}
					Space space = ((!isLocal) ? Space.World : Space.Self);
					switch (mode)
					{
					case "move":
						transform.Translate(zero, space);
						break;
					case "scale":
						transform.localScale += zero;
						break;
					case "rotate":
						transform.Rotate(zero, space);
						break;
					case "color":
						color = new Color(Mathf.Clamp(color.r + array[0] / 255f, 0f, 1f), Mathf.Clamp(color.g + array[1] / 255f, 0f, 1f), Mathf.Clamp(color.b + array[2] / 255f, 0f, 1f), 1f);
						component.SetColor(color);
						break;
					case "hue":
					case "saturation":
					case "lightness":
					{
						float num3;
						float num4;
						float num5;
						Color.RGBToHSV(color, out num3, out num4, out num5);
						if (mode != null)
						{
							if (!(mode == "hue"))
							{
								if (!(mode == "saturation"))
								{
									if (mode == "lightness")
									{
										num5 += num;
									}
								}
								else
								{
									num4 += num;
								}
							}
							else
							{
								num3 += num;
							}
						}
						color = Color.HSVToRGB(Mathf.Clamp(num3, 0f, 1f), Mathf.Clamp(num4, 0f, 1f), Mathf.Clamp(num5, 0f, 1f));
						color.r = Mathf.Clamp(color.r, 0f, 1f);
						color.g = Mathf.Clamp(color.g, 0f, 1f);
						color.b = Mathf.Clamp(color.b, 0f, 1f);
						component.SetColor(color);
						break;
					}
					case "duplicate":
						if (partTransforms.Count < 1000)
						{
							GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(transform.gameObject, transform.position, transform.rotation);
							Transform transform2 = gameObject.transform;
							transform2.parent = transform.parent;
							transform2.Translate(zero, space);
							ThingPart component2 = transform2.GetComponent<ThingPart>();
							component2.SetStatePropertiesByTransform(false);
							Our.lastTransformHandled = transform2;
						}
						break;
					case "material_default":
						component.SetMaterialIgnoringColor(MaterialTypes.None);
						break;
					case "material_metallic":
						component.SetMaterialIgnoringColor(MaterialTypes.Metallic);
						break;
					case "material_very_metallic":
						component.SetMaterialIgnoringColor(MaterialTypes.VeryMetallic);
						break;
					case "material_dark_metallic":
						component.SetMaterialIgnoringColor(MaterialTypes.DarkMetallic);
						break;
					case "material_bright_metallic":
						component.SetMaterialIgnoringColor(MaterialTypes.BrightMetallic);
						break;
					case "material_glow":
						component.SetMaterialIgnoringColor(MaterialTypes.Glow);
						break;
					case "material_transparent":
						component.SetMaterialIgnoringColor(MaterialTypes.Transparent);
						break;
					case "material_unshiny":
						component.SetMaterialIgnoringColor(MaterialTypes.Unshiny);
						break;
					case "material_plastic":
						component.SetMaterialIgnoringColor(MaterialTypes.Plastic);
						break;
					case "material_transparent_glossy":
						component.SetMaterialIgnoringColor(MaterialTypes.TransparentGlossy);
						break;
					case "material_transparent_glossy_metallic":
						component.SetMaterialIgnoringColor(MaterialTypes.TransparentGlossyMetallic);
						break;
					case "material_very_transparent":
						component.SetMaterialIgnoringColor(MaterialTypes.VeryTransparent);
						break;
					case "material_very_transparent_glossy":
						component.SetMaterialIgnoringColor(MaterialTypes.VeryTransparentGlossy);
						break;
					case "material_slightly_transparent":
						component.SetMaterialIgnoringColor(MaterialTypes.SlightlyTransparent);
						break;
					case "material_transparent_texture":
						component.SetMaterialIgnoringColor(MaterialTypes.TransparentTexture);
						break;
					case "material_transparent_glow_texture":
						component.SetMaterialIgnoringColor(MaterialTypes.TransparentGlowTexture);
						break;
					case "undo":
						component.Undo();
						break;
					case "become":
					case "become_stopped":
					{
						int num6 = (int)Mathf.Round(num) - 1;
						if (num6 >= 0 && num6 < component.states.Count)
						{
							component.ResetStates();
							component.currentState = num6;
							if (mode == "become_stopped")
							{
								component.states[component.currentState].didTriggerStartEvent = true;
							}
							component.SetTransformPropertiesByState(false, false);
						}
						break;
					}
					case "insert_state":
						if (component.states.Count < 50)
						{
							component.ResetAtCurrentStateDuringEditing();
							int num7 = component.currentState + 1;
							int currentState = component.currentState;
							component.states.Insert(num7, new ThingPartState());
							component.currentState = num7;
							ThingPartState thingPartState = component.states[num7];
							thingPartState.particleSystemProperty = Managers.thingManager.CloneParticleSystemProperty(component.states[currentState].particleSystemProperty);
							thingPartState.particleSystemColor = component.states[currentState].particleSystemColor;
							thingPartState.textureProperties = Managers.thingManager.CloneTextureProperties(component.states[currentState].textureProperties);
							thingPartState.textureColors[0] = component.states[currentState].textureColors[0];
							thingPartState.textureColors[1] = component.states[currentState].textureColors[1];
							component.SetStatePropertiesByTransform(false);
							component.currentState = num7;
							component.SetTransformPropertiesByState(false, true);
							component.ResetAtCurrentStateDuringEditing();
							this.UpdateThingPartDialogIfNeeded();
						}
						break;
					case "remove_state":
						component.DeleteCurrentStateDuringEditing();
						this.UpdateThingPartDialogIfNeeded();
						break;
					}
					if (flag2 && !string.IsNullOrEmpty(component.imageUrl))
					{
						component.StartLoadImage();
					}
					component.SetStatePropertiesByTransform(false);
				}
			}
		}
		if (partTransforms.Count >= 1 && !omitAutoSounds)
		{
			Managers.soundManager.Play("whoosh", partTransforms[0], 0.2f, false, false);
		}
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x0008C758 File Offset: 0x0008AB58
	private void UpdateThingPartDialogIfNeeded()
	{
		if (Managers.dialogManager != null && Managers.dialogManager.GetCurrentNonStartDialogType() == DialogType.ThingPart)
		{
			Managers.dialogManager.SwitchToNewDialog(DialogType.ThingPart, null, string.Empty);
		}
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0008C790 File Offset: 0x0008AB90
	private List<Transform> GetPartTransforms(bool isForAllParts)
	{
		List<Transform> list = new List<Transform>();
		if (isForAllParts)
		{
			IEnumerator enumerator = CreationHelper.thingBeingEdited.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.gameObject.CompareTag("ThingPart"))
					{
						list.Add(transform);
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
		}
		else if (CreationHelper.thingBeingEdited != null && Our.lastTransformHandled != null)
		{
			list.Add(Our.lastTransformHandled);
		}
		return list;
	}
}
