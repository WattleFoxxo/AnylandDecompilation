using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathParserTK;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000277 RID: 631
public class ThingPart : MonoBehaviour
{
	// Token: 0x06001762 RID: 5986 RVA: 0x000D12D0 File Offset: 0x000CF6D0
	public ThingPart()
	{
		this.states.Add(new ThingPartState());
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x000D141C File Offset: 0x000CF81C
	private void Update()
	{
		if (!this.isLedByOther)
		{
			this.CreateAllNeededObjectsLedByUs();
			this.HandleStartEvent();
			if (this.states.Count >= 2)
			{
				this.UpdateTransformBasedOnTarget();
				this.UpdateLightBasedOnTransform();
				this.UpdateBaseLayerParticlesBasedOnTransform();
				this.SwitchToDefaultStateIfMaxTimeReached();
			}
			this.HandleAllNeededObjectsLedByUs();
			this.UpdateTextPlaceholders();
			this.PlaySoundTracks();
		}
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x000D147B File Offset: 0x000CF87B
	private void CreateAllNeededObjectsLedByUs()
	{
		this.CreateMyIncludedSubThingsIfNeeded();
		this.CreateMyReflectionPartsIfNeeded(null);
		this.CreateMyAutoContinuationPartsIfNeeded(null);
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x000D1491 File Offset: 0x000CF891
	private void HandleAllNeededObjectsLedByUs()
	{
		this.HandleMyReflectionParts();
		this.HandleMyAutoContinuationParts();
		this.MoveMyIncludedSubThings();
		this.MoveMyPlacedSubThings();
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x000D14AB File Offset: 0x000CF8AB
	public void UpdateIsLedByOther()
	{
		this.isLedByOther = this.isReflectionPart || this.isContinuationPart;
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x000D14C8 File Offset: 0x000CF8C8
	private void PlaySoundTracks()
	{
		if (this.soundTracks != null && (!this.isInInventoryOrDialog || this.isGiftInDialog || this.activeEvenInInventory))
		{
			foreach (SoundTrack soundTrack in this.soundTracks)
			{
				soundTrack.ContinuePlayback(base.transform.position);
			}
		}
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x000D155C File Offset: 0x000CF95C
	public void AddConfirmedNonExistingPlacedSubThingId(string placementId, string thingId, Vector3 relativePosition, Vector3 relativeRotation)
	{
		this.placedSubThingIdsWithOriginalInfo.Add(placementId, new ThingIdPositionRotation(thingId, relativePosition, relativeRotation));
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x000D1574 File Offset: 0x000CF974
	public bool ToggleInPlacedSubThingIds(string placementId, string thingId, Vector3 relativePosition, Vector3 relativeRotation)
	{
		bool flag = false;
		if (this.placedSubThingIdsWithOriginalInfo.ContainsKey(placementId))
		{
			this.placedSubThingIdsWithOriginalInfo.Remove(placementId);
		}
		else if (this.placedSubThingIdsWithOriginalInfo.Count < 100)
		{
			this.AddConfirmedNonExistingPlacedSubThingId(placementId, thingId, relativePosition, relativeRotation);
			flag = true;
		}
		else
		{
			Managers.soundManager.Play("no", base.transform, 1f, false, false);
		}
		return flag;
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x000D15E8 File Offset: 0x000CF9E8
	public void AssignMyPlacedSubThings(string onlyThisPlacementId = "")
	{
		if (this.placedSubThingIdsWithOriginalInfo.Count >= 1)
		{
			bool flag = false;
			foreach (KeyValuePair<string, ThingIdPositionRotation> keyValuePair in this.placedSubThingIdsWithOriginalInfo)
			{
				string key = keyValuePair.Key;
				if (onlyThisPlacementId == string.Empty || onlyThisPlacementId == key)
				{
					GameObject placementById = Managers.thingManager.GetPlacementById(key, false);
					if (placementById != null)
					{
						Thing component = placementById.GetComponent<Thing>();
						if (component != null && component.transform.parent && component.CompareTag("Thing") && component.name != Universe.objectNameIfAlreadyDestroyed && (!(component.transform.parent != null) || !component.transform.parent.CompareTag("PlacedSubThings") || component.transform.parent.name.IndexOf(Universe.objectNameIfAlreadyDestroyed) != -1) && !component.containsPlacedSubThings)
						{
							if (!flag)
							{
								this.ResetStates();
							}
							if (this.assignedPlacedSubThings == null)
							{
								this.assignedPlacedSubThings = new GameObject();
								this.assignedPlacedSubThings.transform.name = "PlacedSubThings of " + base.transform.name;
								this.assignedPlacedSubThings.transform.tag = "PlacedSubThings";
								this.assignedPlacedSubThings.transform.parent = base.transform.parent;
								this.assignedPlacedSubThings.transform.localPosition = base.transform.localPosition;
								this.assignedPlacedSubThings.transform.localRotation = base.transform.localRotation;
								this.assignedPlacedSubThings.transform.localScale = Vector3.one;
							}
							component.transform.parent = this.assignedPlacedSubThings.transform;
							component.subThingMasterPart = this;
							component.gameObject.SetActive(true);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x000D184C File Offset: 0x000CFC4C
	public void UnassignMyPlacedSubThings()
	{
		if (this.assignedPlacedSubThings != null)
		{
			Component[] componentsInChildren = this.assignedPlacedSubThings.GetComponentsInChildren(typeof(Thing), true);
			foreach (Component component in componentsInChildren)
			{
				Thing component2 = component.GetComponent<Thing>();
				component2.subThingMasterPart = null;
				component2.RestoreOriginalPlacement(false);
				component2.ResetStates();
			}
			global::UnityEngine.Object.Destroy(this.assignedPlacedSubThings);
			this.assignedPlacedSubThings = null;
		}
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x000D18CC File Offset: 0x000CFCCC
	private void MoveMyPlacedSubThings()
	{
		if (this.assignedPlacedSubThings != null)
		{
			bool flag = true;
			bool flag2 = false;
			if (this.ourPersonRidingBeaconReference != null)
			{
				if (Our.useSmoothRiding || CrossDevice.desktopMode)
				{
					flag2 = true;
				}
				else
				{
					if (this.ridingBeaconLastUpdatedTime == -1f)
					{
						this.ridingBeaconLastUpdatedTime = Time.time;
					}
					flag = false;
					flag2 = false;
					if (Time.time >= this.ridingBeaconLastUpdatedTime + 0.5f)
					{
						this.ridingBeaconLastUpdatedTime = Time.time;
						flag = true;
						flag2 = true;
					}
				}
			}
			if (flag)
			{
				if (this.subThingsFollowDelayed)
				{
					this.assignedPlacedSubThings.transform.localPosition = Vector3.Lerp(this.assignedPlacedSubThings.transform.localPosition, base.transform.localPosition, 0.05f);
					this.assignedPlacedSubThings.transform.localRotation = Quaternion.Lerp(this.assignedPlacedSubThings.transform.localRotation, base.transform.localRotation, 0.05f);
				}
				else
				{
					this.assignedPlacedSubThings.transform.localPosition = base.transform.localPosition;
					this.assignedPlacedSubThings.transform.localRotation = base.transform.localRotation;
				}
			}
			if (flag2)
			{
				Managers.personManager.ourPerson.HandleRidingBeaconIfNeeded();
			}
		}
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000D1A2C File Offset: 0x000CFE2C
	private void HandleStartEvent()
	{
		if (this.currentState < this.states.Count && !this.states[this.currentState].didTriggerStartEvent)
		{
			this.HandleResetParticleSystem();
			if (!this.MyStatesAreBeingEdited())
			{
				this.TriggerEvent(StateListener.EventType.OnStarts, string.Empty, false, null);
				if (this.currentState != 0 && base.transform.parent.gameObject != CreationHelper.thingBeingEdited && !this.PersistStatesNeeded() && !this.persistStatesForWearerOnly)
				{
					this.timeWhenToRevert = Time.time + 30f + 0.5f;
				}
			}
			this.states[this.currentState].didTriggerStartEvent = true;
			if (this.imageUrl != string.Empty && !this.startedAutoAddingImage)
			{
				this.startedAutoAddingImage = true;
				this.StartLoadImage();
			}
		}
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x000D1B28 File Offset: 0x000CFF28
	private void HandleResetParticleSystem()
	{
		ParticleSystem.MinMaxCurve? minMaxCurve = this.particleSystemOriginalAmountBeforeChange;
		if (minMaxCurve != null && this.particleSystemType != ParticleSystemType.None && this.particleSystemComponents != null)
		{
			foreach (ParticleSystem particleSystem in this.particleSystemComponents)
			{
				ParticleSystem.EmissionModule emission = particleSystem.emission;
				ParticleSystem.MinMaxCurve? minMaxCurve2 = this.particleSystemOriginalAmountBeforeChange;
				emission.rate = minMaxCurve2.Value;
				this.particleSystemOriginalAmountBeforeChange = null;
			}
		}
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x000D1BD4 File Offset: 0x000CFFD4
	public void StartLoadImage()
	{
		string text = Path.Combine(Application.persistentDataPath, "cache\\images");
		string text2 = ((!this.imageUrl.EndsWith(".png")) ? ".jpg" : ".png");
		string text3 = ((this.materialType != MaterialTypes.TransparentTexture && this.materialType != MaterialTypes.TransparentGlowTexture) ? string.Empty : "-hiquality-");
		string text4 = string.Concat(new string[]
		{
			text,
			"\\",
			this.imageUrl.Replace("http://", string.Empty).Replace("https://", string.Empty).Replace(".jpg", string.Empty)
				.Replace(".", "_")
				.Replace("/", "_"),
			text3,
			text2
		});
		if (File.Exists(text4))
		{
			this.LoadAndApplyImageFromCache(text4);
		}
		else
		{
			base.StartCoroutine(this.CacheImageFromWeb(text, text4));
		}
	}

	// Token: 0x06001770 RID: 6000 RVA: 0x000D1CDC File Offset: 0x000D00DC
	private void LoadAndApplyImageFromCache(string cacheFilePath)
	{
		this.imageTexture = new Texture2D(4, 4);
		this.imageTexture.LoadImage(File.ReadAllBytes(cacheFilePath));
		bool flag = this.imageTexture.width >= 100 && this.imageTexture.height >= 100;
		if (flag)
		{
			this.ApplyImageTexture();
		}
		else
		{
			this.imageUrl = string.Empty;
			this.imageTexture = null;
			this.imageType = ImageType.Jpeg;
		}
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x000D1D5C File Offset: 0x000D015C
	private IEnumerator CacheImageFromWeb(string cacheFolder, string cacheFilePath)
	{
		int quality = ((!cacheFilePath.Contains("-hiquality-")) ? 75 : 100);
		int num = quality;
		string fullImageUrl = this.GetFullImageUrl(false, num);
		if (string.IsNullOrEmpty(fullImageUrl))
		{
			yield return null;
		}
		WWW www = new WWW(fullImageUrl);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			Directory.CreateDirectory(cacheFolder);
			File.WriteAllBytes(cacheFilePath, www.bytes);
			this.LoadAndApplyImageFromCache(cacheFilePath);
		}
		else if (this.imageType == ImageType.Jpeg)
		{
			this.imageType = ImageType.Png;
			base.StartCoroutine(this.CacheImageFromWeb(cacheFolder, cacheFilePath));
		}
		yield break;
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x000D1D88 File Offset: 0x000D0188
	public string GetFullImageUrl(bool simplifyUrlForReadability = false, int quality = 75)
	{
		string text = null;
		if (this.isImagePasteScreen)
		{
			text = Misc.GetImgurImageUrl(this.imageUrl);
		}
		else if (this.imageUrl.IndexOf("http://cache.coverbrowser.com/image/") == 0)
		{
			string text2 = this.imageUrl.Replace("http://cache.coverbrowser.com/image/", string.Empty);
			if (Validator.ContainsOnly(text2, "abcdefghijklmnopqrstuvwxyz0123456789-_/."))
			{
				text = "http://cache.coverbrowser.com/image/" + text2;
			}
		}
		else if (Validator.ContainsOnly(this.imageUrl, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789/"))
		{
			text = "http://steamuserimages-a.akamaihd.net/ugc/" + this.imageUrl;
			if (!simplifyUrlForReadability && quality != 100 && this.imageType != ImageType.Png)
			{
				text = text + "?output-format=jpeg&output-quality=" + quality.ToString();
			}
			if (this.imageType == ImageType.Png)
			{
				text = text.Replace("http://", "https://");
			}
		}
		return text.Replace("-hiquality-", string.Empty);
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x000D1E88 File Offset: 0x000D0288
	private int GetThingVersion()
	{
		int num = 9;
		Thing parentThing = this.GetParentThing();
		if (parentThing != null)
		{
			num = parentThing.version;
		}
		return num;
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x000D1EB4 File Offset: 0x000D02B4
	private void ApplyImageTexture()
	{
		this.imageWidth = this.imageTexture.width;
		this.imageHeight = this.imageTexture.height;
		if (this.ImageNeedsProportionalScaling())
		{
			this.SetScaleBasedOnImage();
			this.SetStatePropertiesByTransform(false);
		}
		this.SetRendererIfNeeded();
		if (this.GetThingVersion() >= 3)
		{
			MaterialTypes materialTypes = this.materialType;
			switch (materialTypes)
			{
			case MaterialTypes.None:
				if (this.GetIsOfThingBeingEdited())
				{
					this.UpdateMaterial();
				}
				break;
			default:
				if (materialTypes != MaterialTypes.TransparentTexture && materialTypes != MaterialTypes.TransparentGlowTexture)
				{
					this.renderer.material.shader = Managers.thingManager.shader_standard;
					this.UpdateMaterial();
				}
				break;
			case MaterialTypes.Glow:
			case MaterialTypes.PointLight:
			case MaterialTypes.SpotLight:
				this.renderer.material.shader = Managers.thingManager.shader_customGlow;
				this.UpdateMaterial();
				break;
			}
			if ((this.isImagePasteScreen || this.renderer.material.color == Color.black) && !this.allowBlackImageBackgrounds)
			{
				this.renderer.material.color = Color.white;
			}
			this.renderer.material.mainTexture = this.imageTexture;
			this.renderer.material.mainTextureScale = new Vector2(-1f, -1f);
		}
		else
		{
			this.renderer.material.color = Color.white;
			this.renderer.material.mainTexture = this.imageTexture;
			this.renderer.material.mainTextureScale = new Vector2(-1f, -1f);
			if (this.materialType != MaterialTypes.None)
			{
				this.materialType = MaterialTypes.None;
				this.UpdateMaterial();
			}
		}
	}

	// Token: 0x06001775 RID: 6005 RVA: 0x000D2090 File Offset: 0x000D0490
	public bool ImageNeedsProportionalScaling()
	{
		return this.imageUrl != string.Empty && this.GetIsOfThingBeingEdited() && !Our.disableAllThingSnapping && (this.baseType == ThingPartBase.Cube || this.baseType == ThingPartBase.Quad);
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x000D20E8 File Offset: 0x000D04E8
	public void SetScaleBasedOnImage()
	{
		if (this.imageWidth >= 0 && this.imageHeight >= 0)
		{
			float num = Math.Max(base.transform.localScale.x, base.transform.localScale.z);
			float num2 = num / (float)this.imageWidth;
			float num3 = num / (float)this.imageHeight;
			float num4 = Math.Min(num2, num3);
			float num5 = (float)this.imageWidth * num4;
			float num6 = (float)this.imageHeight * num4;
			base.transform.localScale = new Vector3(num5, base.transform.localScale.y, num6);
		}
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x000D2198 File Offset: 0x000D0598
	private string ReplaceTextPlaceholdersForVariableExpression(string s)
	{
		foreach (string text in Managers.behaviorScriptManager.placeholdersReturningStrings)
		{
			s = s.ReplaceCaseInsensitive("[" + text + "]", string.Empty);
		}
		s = this.ReplaceTextPlaceholders(s);
		return s;
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x000D21F0 File Offset: 0x000D05F0
	private string ReplaceTextPlaceholders(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}
		if (s.IndexOf("[people count]", StringComparison.InvariantCultureIgnoreCase) >= 0)
		{
			s = s.ReplaceCaseInsensitive("[people count]", Managers.personManager.GetCurrentAreaPersonCount().ToString());
		}
		if (!string.IsNullOrEmpty(Managers.areaManager.currentAreaName))
		{
			s = s.ReplaceCaseInsensitive("[area name]", Managers.areaManager.currentAreaName.ToUpper());
		}
		DateTime dateTime = DateTime.Now.ToUniversalTime();
		string text = dateTime.Month.ToString();
		string text2 = text.PadLeft(2, '0');
		string text3 = dateTime.Day.ToString();
		string text4 = text3.PadLeft(2, '0');
		string text5 = dateTime.Hour.ToString();
		string text6 = Misc.GetHourIn12HourFormat(dateTime.Hour).ToString();
		string text7 = text5.PadLeft(2, '0');
		string text8 = text6.PadLeft(2, '0');
		s = s.ReplaceCaseInsensitive("[year]", dateTime.Year.ToString());
		s = s.ReplaceCaseInsensitive("[month]", text2);
		s = s.ReplaceCaseInsensitive("[month unpadded]", text);
		s = s.ReplaceCaseInsensitive("[day]", text4);
		s = s.ReplaceCaseInsensitive("[day unpadded]", text3);
		s = s.ReplaceCaseInsensitive("[hour]", text7);
		s = s.ReplaceCaseInsensitive("[hour 12]", text8);
		s = s.ReplaceCaseInsensitive("[hour unpadded]", text5);
		s = s.ReplaceCaseInsensitive("[hour 12 unpadded]", text6);
		s = s.ReplaceCaseInsensitive("[minute]", dateTime.Minute.ToString().PadLeft(2, '0'));
		s = s.ReplaceCaseInsensitive("[second]", dateTime.Second.ToString().PadLeft(2, '0'));
		s = s.ReplaceCaseInsensitive("[millisecond]", dateTime.Millisecond.ToString().PadLeft(3, '0'));
		if (s.IndexOf("[local hour", StringComparison.InvariantCultureIgnoreCase) >= 0)
		{
			bool flag = base.transform.parent != null && base.transform.parent.CompareTag("Attachment");
			string text9 = ((!flag) ? text5 : DateTime.Now.Hour.ToString());
			string text10 = ((!flag) ? text6 : Misc.GetHourIn12HourFormat(DateTime.Now.Hour).ToString());
			string text11 = text9.PadLeft(2, '0');
			string text12 = text10.PadLeft(2, '0');
			s = s.ReplaceCaseInsensitive("[local hour]", text11);
			s = s.ReplaceCaseInsensitive("[local hour 12]", text12);
			s = s.ReplaceCaseInsensitive("[local hour unpadded]", text9);
			s = s.ReplaceCaseInsensitive("[local hour 12 unpadded]", text10);
		}
		if (s.IndexOf("[closest person]", StringComparison.InvariantCultureIgnoreCase) >= 0)
		{
			string text13 = string.Empty;
			GameObject personHeadClosestToPosition = Managers.personManager.GetPersonHeadClosestToPosition(base.transform.position, null);
			if (personHeadClosestToPosition != null)
			{
				Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(personHeadClosestToPosition);
				if (personThisObjectIsOf != null)
				{
					text13 = personThisObjectIsOf.screenName;
				}
			}
			s = s.ReplaceCaseInsensitive("[closest person]", text13.ToUpper());
		}
		if (s.IndexOf("[person]", StringComparison.InvariantCultureIgnoreCase) >= 0)
		{
			string text14 = string.Empty;
			Person personThisObjectIsOf2 = Managers.personManager.GetPersonThisObjectIsOf(base.gameObject);
			if (personThisObjectIsOf2 != null)
			{
				text14 = personThisObjectIsOf2.screenName;
			}
			s = s.ReplaceCaseInsensitive("[person]", text14.ToUpper());
		}
		if (s.IndexOf("[own person]", StringComparison.InvariantCultureIgnoreCase) >= 0)
		{
			Person ourPerson = Managers.personManager.ourPerson;
			if (ourPerson != null)
			{
				s = s.ReplaceCaseInsensitive("[own person]", ourPerson.screenName.ToUpper());
			}
		}
		if (s.IndexOf("[thing name]", StringComparison.InvariantCultureIgnoreCase) >= 0 && base.transform.parent != null)
		{
			s = s.ReplaceCaseInsensitive("[thing name]", base.transform.parent.name.ToUpper());
		}
		if (s.IndexOf("[closest held]", StringComparison.InvariantCultureIgnoreCase) >= 0)
		{
			string text15 = string.Empty;
			GameObject heldThingClosestToPosition = Managers.personManager.GetHeldThingClosestToPosition(base.transform.position);
			if (heldThingClosestToPosition != null)
			{
				text15 = heldThingClosestToPosition.name;
			}
			s = s.ReplaceCaseInsensitive("[closest held]", text15.ToUpper());
		}
		s = s.ReplaceCaseInsensitive("[x]", base.transform.position.x.ToString("F2"));
		s = s.ReplaceCaseInsensitive("[y]", base.transform.position.y.ToString("F2"));
		s = s.ReplaceCaseInsensitive("[z]", base.transform.position.z.ToString("F2"));
		if (s.IndexOf("[people names]", StringComparison.InvariantCultureIgnoreCase) >= 0)
		{
			s = s.ReplaceCaseInsensitive("[people names]", Managers.personManager.GetCurrentAreaPeopleNames());
		}
		if (Time.time >= this.timeWhenToClearTypedText)
		{
			this.typedText = string.Empty;
		}
		s = s.ReplaceCaseInsensitive("[typed]", this.typedText);
		if (Managers.behaviorScriptManager != null)
		{
			s = Managers.behaviorScriptManager.ReplaceTextPlaceholders(s, this.GetParentThing(), this);
		}
		if (s.IndexOf("[proximity]", StringComparison.InvariantCultureIgnoreCase) >= 0)
		{
			float? proximityToNextForwardObject = this.GetProximityToNextForwardObject();
			string text16 = ((proximityToNextForwardObject == null) ? string.Empty : proximityToNextForwardObject.Value.ToString("F3"));
			s = s.ReplaceCaseInsensitive("[proximity]", text16);
		}
		return s;
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x000D2828 File Offset: 0x000D0C28
	public void Undo()
	{
		if (this.HasUndoForThisState())
		{
			Vector3 vector = this.lastPositionForUndo;
			Vector3 vector2 = this.lastRotationForUndo;
			Vector3 vector3 = this.lastScaleForUndo;
			Color color = this.lastColorForUndo;
			this.MemorizeForUndo();
			base.transform.localPosition = vector;
			base.transform.localEulerAngles = vector2;
			base.transform.localScale = vector3;
			this.material.color = color;
			this.SetStatePropertiesByTransform(false);
			if (this.materialType == MaterialTypes.Transparent)
			{
				this.UpdateMaterial();
			}
		}
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x000D28AC File Offset: 0x000D0CAC
	private void MemorizeForUndo()
	{
		if (this.GetIsOfThingBeingEdited())
		{
			if (this.currentState > this.states.Count - 1)
			{
				this.currentState = 0;
			}
			this.lastPositionForUndo = this.states[this.currentState].position;
			this.lastRotationForUndo = this.states[this.currentState].rotation;
			this.lastScaleForUndo = this.states[this.currentState].scale;
			this.lastColorForUndo = this.states[this.currentState].color;
		}
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x000D2953 File Offset: 0x000D0D53
	private void ForgetForUndo()
	{
		if (this.GetIsOfThingBeingEdited())
		{
			this.lastPositionForUndo = Vector3.zero;
			this.lastRotationForUndo = Vector3.zero;
			this.lastScaleForUndo = Vector3.one;
			this.lastColorForUndo = Color.clear;
		}
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x000D298C File Offset: 0x000D0D8C
	public bool HasUndoForThisState()
	{
		return ((this.lastPositionForUndo != Vector3.zero && this.lastRotationForUndo != Vector3.zero) || (this.lastScaleForUndo != Vector3.one && this.lastColorForUndo != Color.clear)) && this.HasThingPartSiblings();
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x000D29F6 File Offset: 0x000D0DF6
	public void UpdateTextCollider()
	{
		global::UnityEngine.Object.Destroy(base.gameObject.GetComponent<BoxCollider>());
		base.gameObject.AddComponent<BoxCollider>();
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x000D2A14 File Offset: 0x000D0E14
	private bool HasThingPartSiblings()
	{
		Component[] componentsInChildren = base.transform.parent.GetComponentsInChildren<ThingPart>();
		return componentsInChildren.Length >= 2;
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x000D2A3B File Offset: 0x000D0E3B
	private bool MyStatesAreBeingEdited()
	{
		return Our.mode == EditModes.Thing && CreationHelper.thingPartWhoseStatesAreEdited == base.gameObject;
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x000D2A5C File Offset: 0x000D0E5C
	public bool GetIsOfThingBeingEdited()
	{
		return CreationHelper.thingBeingEdited != null && base.transform.parent != null && (base.transform.parent.gameObject == CreationHelper.thingBeingEdited || (Our.mode == EditModes.Thing && (base.gameObject.CompareTag("CurrentlyHeldLeft") || base.gameObject.CompareTag("CurrentlyHeldRight"))));
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x000D2AEC File Offset: 0x000D0EEC
	private void SwitchToDefaultStateIfMaxTimeReached()
	{
		if (this.timeWhenToRevert != -1f && this.currentState != 0 && this.currentStateTarget == -1 && this.timeWhenToRevert - Time.time <= 0f)
		{
			this.currentStateTarget = 0;
			this.currentStateTargetCurveVia = -1;
			this.stateTargetSeconds = 0.5f;
			this.stateTargetTime = Time.time + this.stateTargetSeconds;
		}
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x000D2B61 File Offset: 0x000D0F61
	private void OnTriggerEnter(Collider other)
	{
		this.HandleTriggerEnter(other, false, false);
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x000D2B6C File Offset: 0x000D0F6C
	public void HandleTriggerEnter(Collider other, bool ignoreVideoButtonAndSimilar = false, bool ignoreTriggerOther = false)
	{
		if (other.gameObject.CompareTag("ThingPart"))
		{
			ThingPart component = other.gameObject.GetComponent<ThingPart>();
			if (base.transform.parent != null && component != null && other.transform.parent != null && other.transform.parent.gameObject != base.transform.parent.gameObject)
			{
				Thing component2 = base.transform.parent.gameObject.GetComponent<Thing>();
				Thing component3 = other.transform.parent.gameObject.GetComponent<Thing>();
				if (component2 == null || component3 == null)
				{
					return;
				}
				if (this.uncollidable || component.uncollidable || (component2.uncollidable && !this.isDedicatedCollider) || (component3.uncollidable && !component.isDedicatedCollider))
				{
					return;
				}
				Thing myRootThing = component2.GetMyRootThing();
				Thing myRootThing2 = component3.GetMyRootThing();
				if (!(myRootThing == myRootThing2))
				{
					if (component.isLiquid && (!component2.floatsOnLiquid || !component.IsPartOfPlacement()))
					{
						if (component3.thingId != component2.thingId && component2.isThrownOrEmitted && !component2.IsPlacement())
						{
							MeshRenderer component4 = other.GetComponent<MeshRenderer>();
							if (component4 != null)
							{
								Effects.AddSplash(base.transform.position, component4.material.color);
							}
							global::UnityEngine.Object.Destroy(base.transform.parent.gameObject);
						}
					}
					else if (component2 != null && component3 != null && component2.inWorldSourceObject != other.transform.parent.gameObject && component2.thingId != component3.emittedByThingId)
					{
						bool flag = base.transform.parent != null && base.transform.parent.parent != null && (base.transform.parent.parent.name == "HandCoreLeft" || base.transform.parent.parent.name == "HandCoreRight");
						bool flag2 = flag && Managers.personManager.GetPersonThisObjectIsOf(base.transform.gameObject) == Managers.personManager.GetPersonThisObjectIsOf(other.gameObject);
						bool flag3 = false;
						Hand hand = ((!flag) ? null : this.GetHandHoldingMe());
						bool flag4 = component2.isHeldAsHoldableByOurPerson || (flag && component2.movableByEveryone);
						if (flag4 && hand != null && hand.controller != null)
						{
							flag3 = hand.controller.velocity.magnitude >= 1.15f;
						}
						if (!flag2 || !component3.CompareTag("Attachment"))
						{
							string text = base.transform.parent.gameObject.name;
							string text2 = component.transform.parent.gameObject.name;
							if (this.currentState < this.states.Count && !string.IsNullOrEmpty(this.states[this.currentState].name))
							{
								text = text + " " + this.states[this.currentState].name;
							}
							if (component.currentState < component.states.Count && !string.IsNullOrEmpty(component.states[component.currentState].name))
							{
								text2 = text2 + " " + component.states[this.currentState].name;
							}
							if (!component2.IsPlacement() || component3.IsPlacement())
							{
								bool flag5 = (myRootThing.isThrownOrEmitted || myRootThing2.isThrownOrEmitted) && (this.PersistStatesNeeded() || component.PersistStatesNeeded());
								if (flag5)
								{
									if (Our.IsMasterClient(false))
									{
										if (!ignoreTriggerOther)
										{
											component.TriggerHitsAndTouches(text, flag3, true);
										}
										this.TriggerHitsAndTouches(text2, flag3, true);
									}
								}
								else
								{
									if (!ignoreTriggerOther)
									{
										component.TriggerHitsAndTouches(text, flag3, flag4);
									}
									this.TriggerHitsAndTouches(text2, flag3, flag4);
								}
							}
							bool flag6 = component.TriggerEvent(StateListener.EventType.OnGets, base.transform.parent.gameObject.name, flag4, null);
							if (flag4)
							{
								this.TriggerHapticPulse(Universe.mediumHapticPulse);
							}
							if (!Managers.soundLibraryManager.didPlaySoundThisUpdate && component2.IsPlacement())
							{
								Managers.soundManager.Play("tap", base.transform, 1f, false, false);
							}
							if (!flag6 && this.pushBackEnabled)
							{
								component2.StartShowTouchPushBack();
							}
							if (flag6 && component2.isHoldable && !component2.remainsHeld)
							{
								Managers.personManager.DoClearFromHand(base.transform.parent.gameObject, null, false);
							}
						}
					}
				}
			}
		}
		else if ((other.gameObject.name == "HandDot" || other.gameObject.name == "SecondaryHandDot") && !this.GetIsOfThingBeingEdited() && !ignoreVideoButtonAndSimilar && (Our.mode == EditModes.None || Our.mode == EditModes.Area) && (this.isVideoButton || this.isSlideshowButton || this.isCameraButton))
		{
			Hand ahandOfOurs = Misc.GetAHandOfOurs();
			if (ahandOfOurs != null)
			{
				if (this.isVideoButton)
				{
					if (!string.IsNullOrEmpty(this.videoIdToPlayWhenPressed))
					{
						Managers.videoManager.SetVolume(this.videoAutoPlayVolume);
						Managers.videoManager.WePlayVideoAtNearestThingPartScreen(this.videoIdToPlayWhenPressed, "Auto-Button Video", null, null);
					}
					else
					{
						ahandOfOurs.SwitchToNewDialog(DialogType.VideoControl, string.Empty);
					}
				}
				else if (this.isSlideshowButton)
				{
					ahandOfOurs.SwitchToNewDialog(DialogType.SlideshowControl, string.Empty);
				}
				else if (this.isCameraButton)
				{
					ahandOfOurs.SwitchToNewDialog(DialogType.CameraControl, string.Empty);
					CameraControlDialog component5 = ahandOfOurs.currentDialog.GetComponent<CameraControlDialog>();
					if (component5 != null)
					{
						component5.SetCameraThingPartByButton(base.gameObject);
					}
				}
				Thing component6 = base.transform.parent.gameObject.GetComponent<Thing>();
				if (component6 != null && !component6.omitAutoHapticFeedback)
				{
					ahandOfOurs.StartShrinkingHapticPulseOverTime();
				}
				Managers.soundManager.Play("click", base.transform, 0.1f, false, false);
			}
		}
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x000D32A7 File Offset: 0x000D16A7
	public void TriggerHitsAndTouches(string nameOfWhatHits, bool isFast, bool weAreOwner)
	{
		if (isFast)
		{
			this.TriggerEvent(StateListener.EventType.OnHitting, nameOfWhatHits, weAreOwner, null);
			this.TriggerEvent(StateListener.EventType.OnHitting, string.Empty, weAreOwner, null);
		}
		this.TriggerEvent(StateListener.EventType.OnTouches, nameOfWhatHits, weAreOwner, null);
		this.TriggerEvent(StateListener.EventType.OnTouches, string.Empty, weAreOwner, null);
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x000D32E8 File Offset: 0x000D16E8
	public void TriggerHapticPulse(ushort amount)
	{
		Hand handHoldingMe = this.GetHandHoldingMe();
		if (handHoldingMe != null)
		{
			handHoldingMe.TriggerHapticPulse(amount);
		}
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x000D3310 File Offset: 0x000D1710
	private void TriggerShrinkingHapticPulse()
	{
		Hand hand = this.GetHandHoldingMe();
		if (hand == null && base.transform.parent != null && Managers.personManager != null)
		{
			GameObject gameObject = base.transform.parent.gameObject;
			if (gameObject.tag == "Attachment")
			{
				Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(gameObject);
				if (personThisObjectIsOf != null)
				{
					GameObject handByTopographyId = personThisObjectIsOf.GetHandByTopographyId((!(gameObject.transform.parent.name == "ArmLeftAttachmentPoint")) ? TopographyId.Right : TopographyId.Left);
					hand = handByTopographyId.GetComponent<Hand>();
				}
			}
		}
		if (hand != null)
		{
			hand.StartShrinkingHapticPulseOverTime();
		}
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x000D33DC File Offset: 0x000D17DC
	private Hand GetHandHoldingMe()
	{
		Hand hand = null;
		if (base.transform.parent != null && base.transform.parent.parent != null)
		{
			hand = base.transform.parent.parent.GetComponent<Hand>();
		}
		return hand;
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x000D3433 File Offset: 0x000D1833
	public bool TriggerEventAsStateAuthority(StateListener.EventType eventType, string data = "")
	{
		return this.TriggerEvent(eventType, data, true, null);
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000D3440 File Offset: 0x000D1840
	public bool TriggerEvent(StateListener.EventType eventType, string data = "", bool weAreStateAuthority = false, Person relevantPerson = null)
	{
		bool flag = false;
		Thing thing = null;
		bool flag2 = this.currentStateTarget == -1;
		if (flag2 && this.currentState < this.states.Count)
		{
			bool flag3 = !this.isInInventoryOrDialog || this.isGiftInDialog || (this.isInInventory && this.activeEvenInInventory);
			int num = 0;
			int num2 = this.states.Count - 1;
			if (!this.containsForAnyStateListeners)
			{
				num = this.currentState;
				num2 = this.currentState;
			}
			for (int i = num; i <= num2; i++)
			{
				for (int j = 0; j < this.states[i].listeners.Count; j++)
				{
					StateListener stateListener = this.states[i].listeners[j];
					if ((stateListener.isForAnyState || i == this.currentState) && stateListener.eventType == eventType && (flag3 || stateListener.goToInventoryPage != 0))
					{
						bool flag4 = false;
						bool flag6;
						if (stateListener.whenData != null)
						{
							if (eventType == StateListener.EventType.OnVariableChange)
							{
								if (thing == null)
								{
									thing = this.GetMyRootThing();
								}
								bool flag5 = Our.IsMasterClient(false);
								flag6 = flag5 && (string.IsNullOrEmpty(data) || this.ExpressionContainsVariableName(stateListener.whenData, data)) && this.EvaluateVariableComparison(thing, stateListener.whenData, relevantPerson);
								flag4 = flag5;
							}
							else if (this.EventTypeUsesFuzzyDataMatching(eventType))
							{
								flag6 = data.IndexOf(stateListener.whenData) >= 0;
							}
							else
							{
								flag6 = data == stateListener.whenData;
								if (flag6 && (eventType == StateListener.EventType.OnSettingEnabled || eventType == StateListener.EventType.OnSettingDisabled))
								{
									this.EnablePersistStatesForWearerOnly();
								}
							}
						}
						else
						{
							flag6 = data == string.Empty;
						}
						if (flag6 && stateListener.whenIsData != null)
						{
							if (thing == null)
							{
								thing = this.GetMyRootThing();
							}
							flag6 = this.EvaluateVariableComparison(thing, stateListener.whenIsData, null);
						}
						if (flag6)
						{
							this.ExecuteCommands(stateListener, false);
							if ((weAreStateAuthority || flag4) && !this.GetIsOfThingBeingEdited() && this.indexWithinThing != -1 && !this.personalExperience)
							{
								if (thing == null)
								{
									thing = this.GetMyRootThing();
								}
								if (!thing.personalExperience)
								{
									Managers.personManager.DoBehaviorScriptLine(base.transform.parent.gameObject, this.indexWithinThing, j, i, this.currentState);
								}
							}
							flag = true;
						}
					}
				}
			}
		}
		this.TriggerOnAnyEvents(eventType, data, weAreStateAuthority);
		return flag;
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x000D3710 File Offset: 0x000D1B10
	private bool ExpressionContainsVariableName(string expression, string variableName)
	{
		bool flag = false;
		if (!string.IsNullOrEmpty(expression))
		{
			string[] array = Misc.Split(expression, " ", StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text == variableName)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x000D3768 File Offset: 0x000D1B68
	private bool EvaluateVariableComparison(Thing thing, string expression, Person relevantPerson = null)
	{
		bool? flag = null;
		MathParser mathParser = new MathParser();
		expression = expression.ToLower().Trim();
		expression = this.ReplaceTextPlaceholdersForVariableExpression(expression);
		expression = BehaviorScriptManager.ReplaceVariablesWithValues(thing, this, expression, relevantPerson, false);
		string[] array = new string[] { expression };
		bool flag2 = false;
		if (expression.Contains(" or "))
		{
			array = Misc.Split(expression, " or ", StringSplitOptions.RemoveEmptyEntries);
		}
		else if (expression.Contains(" and "))
		{
			array = Misc.Split(expression, " and ", StringSplitOptions.RemoveEmptyEntries);
			flag2 = true;
		}
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (!(text == string.Empty))
			{
				bool flag3 = false;
				bool flag4 = false;
				foreach (string text2 in Managers.behaviorScriptManager.comparators)
				{
					if (text.Contains(text2))
					{
						flag3 = true;
						string[] array2 = Misc.Split(text, text2, StringSplitOptions.RemoveEmptyEntries);
						if (array2.Length == 2)
						{
							string text3 = array2[0].Trim();
							string text4 = array2[1].Trim();
							float num = 0f;
							float num2 = 0f;
							try
							{
								num = (float)mathParser.Parse(text3, false);
								num2 = (float)mathParser.Parse(text4, false);
							}
							catch (Exception ex)
							{
								return false;
							}
							switch (text2)
							{
							case "<=":
							case "=<":
								flag4 = num <= num2;
								break;
							case ">=":
							case "=>":
								flag4 = num >= num2;
								break;
							case "!=":
							case "<>":
							case "><":
								flag4 = num != num2;
								break;
							case "<":
								flag4 = num < num2;
								break;
							case ">":
								flag4 = num > num2;
								break;
							case "==":
							case "=":
								flag4 = num == num2;
								break;
							}
						}
						break;
					}
				}
				if (!flag3)
				{
					bool flag5 = text.StartsWith("not ");
					if (flag5)
					{
						text = text.Substring("not ".Length);
					}
					flag4 = text != "0";
					if (flag5)
					{
						flag4 = !flag4;
					}
				}
				if (flag == null)
				{
					flag = new bool?(flag4);
				}
				else if (flag2)
				{
					flag = flag && flag4;
				}
				else
				{
					bool? flag6 = flag;
					flag = ((!flag4) ? flag6 : new bool?(true));
				}
			}
		}
		if (flag == null)
		{
			flag = new bool?(false);
		}
		return flag.Value;
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x000D3ADC File Offset: 0x000D1EDC
	private void TriggerOnAnyEvents(StateListener.EventType eventType, string data, bool weAreStateAuthority)
	{
		if (base.transform.parent != null && base.transform.parent.parent != null)
		{
			StateListener.EventType eventType2 = StateListener.EventType.None;
			switch (eventType)
			{
			case StateListener.EventType.OnHitting:
				eventType2 = StateListener.EventType.OnAnyPartHitting;
				break;
			default:
				if (eventType != StateListener.EventType.OnConsumed)
				{
					if (eventType != StateListener.EventType.OnBlownAt)
					{
						if (eventType == StateListener.EventType.OnTouches)
						{
							eventType2 = StateListener.EventType.OnAnyPartTouches;
						}
					}
					else
					{
						eventType2 = StateListener.EventType.OnAnyPartBlownAt;
					}
				}
				else
				{
					eventType2 = StateListener.EventType.OnAnyPartConsumed;
				}
				break;
			case StateListener.EventType.OnPointedAt:
				eventType2 = StateListener.EventType.OnAnyPartPointedAt;
				break;
			case StateListener.EventType.OnLookedAt:
				eventType2 = StateListener.EventType.OnAnyPartLookedAt;
				break;
			}
			if (eventType2 != StateListener.EventType.None)
			{
				Thing myRootThing = this.GetMyRootThing();
				if (myRootThing != null && (myRootThing.containsOnAnyListener || this.WeArePartOfPlacedSubThing()))
				{
					Component[] componentsInChildren = myRootThing.transform.GetComponentsInChildren(typeof(ThingPart), true);
					foreach (Component component in componentsInChildren)
					{
						ThingPart component2 = component.GetComponent<ThingPart>();
						component2.TriggerEvent(eventType2, data, weAreStateAuthority, null);
					}
				}
			}
		}
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x000D3C04 File Offset: 0x000D2004
	public bool WeArePartOfPlacedSubThing()
	{
		return base.transform.parent != null && base.transform.parent.parent != null && base.transform.parent.parent.CompareTag("PlacedSubThings");
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x000D3C60 File Offset: 0x000D2060
	private bool EventTypeUsesFuzzyDataMatching(StateListener.EventType eventType)
	{
		bool flag = true;
		switch (eventType)
		{
		case StateListener.EventType.OnTold:
		case StateListener.EventType.OnToldByNearby:
		case StateListener.EventType.OnToldByAny:
		case StateListener.EventType.OnToldByBody:
			break;
		default:
			if (eventType != StateListener.EventType.OnSettingEnabled && eventType != StateListener.EventType.OnSettingDisabled)
			{
				return flag;
			}
			break;
		}
		flag = false;
		return flag;
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x000D3CA4 File Offset: 0x000D20A4
	public void ExecuteCommands(StateListener listener, bool originatesFromOtherClient = false)
	{
		if (Our.suppressBehaviorScriptsAsEditor && (Managers.areaManager.weAreEditorOfCurrentArea || Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, true)))
		{
			return;
		}
		if (!Managers.thingManager)
		{
			return;
		}
		Thing thing = ((!(base.transform.parent != null)) ? null : base.transform.parent.GetComponent<Thing>());
		if (thing == null)
		{
			Log.Debug("ExecuteCommands parent Thing not found");
			return;
		}
		if (listener.setState == -1)
		{
			RelativeStateTarget? setStateRelative = listener.setStateRelative;
			if (setStateRelative == null)
			{
				goto IL_210;
			}
		}
		this.tweenType = listener.tweenType;
		if (originatesFromOtherClient)
		{
			this.stateSwitchOriginatedFromOtherClient = true;
			base.CancelInvoke("ResetStateSwitchOriginatedFromOtherClient");
			base.Invoke("ResetStateSwitchOriginatedFromOtherClient", 3f);
		}
		int num = -1;
		RelativeStateTarget? setStateRelative2 = listener.setStateRelative;
		if (setStateRelative2 != null)
		{
			RelativeStateTarget? setStateRelative3 = listener.setStateRelative;
			if (setStateRelative3 != null)
			{
				RelativeStateTarget value = setStateRelative3.Value;
				if (value != RelativeStateTarget.Current)
				{
					if (value != RelativeStateTarget.Previous)
					{
						if (value == RelativeStateTarget.Next)
						{
							num = this.currentState + 1;
							if (num >= this.states.Count)
							{
								num = 0;
							}
						}
					}
					else
					{
						num = this.currentState - 1;
						if (num < 0)
						{
							num = this.states.Count - 1;
						}
					}
				}
				else
				{
					num = this.currentState;
				}
			}
		}
		else
		{
			num = listener.setState;
		}
		if (this.currentState != num || listener.setStateRelative == RelativeStateTarget.Current)
		{
			this.currentStateTarget = num;
			this.currentStateTargetCurveVia = listener.curveViaState;
			if (this.PersistStatesNeeded())
			{
				this.stateTargetSeconds = listener.setStateSeconds;
			}
			else
			{
				this.stateTargetSeconds = Misc.ClampMax(listener.setStateSeconds, 30f);
			}
			this.stateTargetTime = Time.time + this.stateTargetSeconds;
		}
		this.timeWhenToRevert = -1f;
		IL_210:
		if (listener.rights != null && this.IsPartOfPlacement())
		{
			bool? emittedClimbing = listener.rights.emittedClimbing;
			if (emittedClimbing != null)
			{
				Managers.areaManager.rights.emittedClimbing = listener.rights.emittedClimbing;
			}
			bool? emittedTransporting = listener.rights.emittedTransporting;
			if (emittedTransporting != null)
			{
				Managers.areaManager.rights.emittedTransporting = listener.rights.emittedTransporting;
			}
			bool? movingThroughObstacles = listener.rights.movingThroughObstacles;
			if (movingThroughObstacles != null)
			{
				Managers.areaManager.rights.movingThroughObstacles = listener.rights.movingThroughObstacles;
			}
			bool? visionInObstacles = listener.rights.visionInObstacles;
			if (visionInObstacles != null)
			{
				Managers.areaManager.rights.visionInObstacles = listener.rights.visionInObstacles;
			}
			bool? invisibility = listener.rights.invisibility;
			if (invisibility != null)
			{
				bool? invisibility2 = listener.rights.invisibility;
				bool valueOrDefault = invisibility2.GetValueOrDefault();
				bool? invisibility3 = Managers.areaManager.rights.invisibility;
				if (valueOrDefault != invisibility3.GetValueOrDefault() || ((invisibility2 != null) ^ (invisibility3 != null)))
				{
					Managers.areaManager.rights.invisibility = listener.rights.invisibility;
					if (Managers.areaManager.rights.invisibility == true && !Managers.areaManager.didShowAllowInvisibilityWarning)
					{
						AlertDialog alertDialog = Managers.dialogManager.ShowInfo("This area allows invisibility, so body spheres and name tags will not show", true, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
						if (alertDialog != null)
						{
							Managers.areaManager.didShowAllowInvisibilityWarning = true;
						}
					}
				}
			}
			bool? anyPersonSize = listener.rights.anyPersonSize;
			if (anyPersonSize != null)
			{
				Managers.areaManager.rights.anyPersonSize = listener.rights.anyPersonSize;
			}
			bool? highlighting = listener.rights.highlighting;
			if (highlighting != null)
			{
				Managers.areaManager.rights.highlighting = listener.rights.highlighting;
			}
			bool? amplifiedSpeech = listener.rights.amplifiedSpeech;
			if (amplifiedSpeech != null)
			{
				Managers.areaManager.rights.amplifiedSpeech = listener.rights.amplifiedSpeech;
			}
			bool? anyDestruction = listener.rights.anyDestruction;
			if (anyDestruction != null)
			{
				Managers.areaManager.rights.anyDestruction = listener.rights.anyDestruction;
			}
			bool? webBrowsing = listener.rights.webBrowsing;
			if (webBrowsing != null)
			{
				bool? webBrowsing2 = listener.rights.webBrowsing;
				bool value2 = webBrowsing2.Value;
				bool? webBrowsing3 = Managers.areaManager.rights.webBrowsing;
				if (value2 != webBrowsing3.Value)
				{
					Managers.areaManager.rights.webBrowsing = listener.rights.webBrowsing;
					bool? webBrowsing4 = Managers.areaManager.rights.webBrowsing;
					if (!webBrowsing4.Value)
					{
						Managers.browserManager.DestroyAllBrowsers();
					}
				}
			}
			bool? untargetedAttractThings = listener.rights.untargetedAttractThings;
			if (untargetedAttractThings != null)
			{
				Managers.areaManager.rights.untargetedAttractThings = listener.rights.untargetedAttractThings;
			}
			bool? slowBuildCreation = listener.rights.slowBuildCreation;
			if (slowBuildCreation != null)
			{
				Managers.areaManager.rights.slowBuildCreation = listener.rights.slowBuildCreation;
			}
		}
		if (listener.tells != null)
		{
			foreach (KeyValuePair<TellType, string> keyValuePair in listener.tells)
			{
				if (Managers.behaviorScriptManager.IterateTellCountIfStillUnderLimit())
				{
					this.HandleTell(thing, keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
		if (listener.sounds != null)
		{
			Transform transform = Managers.treeManager.GetTransform("/OurPersonRig/HeadCore");
			if (transform != null)
			{
				foreach (Sound sound in listener.sounds)
				{
					if (sound.surround || thing.hasSurroundSound || Vector3.Distance(transform.position, base.transform.position) <= 40f)
					{
						SoundLibraryManager soundLibraryManager = Managers.soundLibraryManager;
						Vector3 position = base.transform.position;
						Sound sound2 = sound;
						bool hasSurroundSound = thing.hasSurroundSound;
						soundLibraryManager.Play(position, sound2, false, false, hasSurroundSound, -1f);
					}
				}
			}
		}
		if (listener.soundTrackData != null)
		{
			if (this.soundTracks == null || this.soundTracks.Count > 128)
			{
				this.soundTracks = new List<SoundTrack>();
			}
			SoundTrack soundTrack = new SoundTrack();
			soundTrack.SetByString(listener.soundTrackData);
			soundTrack.surroundSound = thing.hasSurroundSound;
			this.soundTracks.Add(soundTrack);
		}
		if (listener.addCrumbles)
		{
			Effects.SpawnCrumbles((!listener.addEffectIsForAllParts) ? base.gameObject : thing.gameObject);
		}
		float? num2 = listener.propelForwardPercent;
		if (num2 != null)
		{
			float? num3 = listener.propelForwardPercent;
			this.propelForwardPercent = num3.Value;
		}
		float? num4 = listener.rotateForwardPercent;
		if (num4 != null)
		{
			float? num5 = listener.rotateForwardPercent;
			this.rotateForwardPercent = num5.Value;
		}
		if (listener.transportToArea != null || listener.transportOntoThing != null)
		{
			this.HandleTransportToAreaOrThingCommand(listener, originatesFromOtherClient);
		}
		if (listener.callMeThisName != null)
		{
			this.states[this.currentState].name = listener.callMeThisName;
		}
		if (listener.startLoopSoundName != null && !this.isInInventoryOrDialog && Managers.soundLibraryManager.loopSoundsStartedCount < 5)
		{
			Managers.soundLibraryManager.loopSoundsStartedCount++;
			string loopSoundPath = Managers.soundLibraryManager.GetLoopSoundPath(listener.startLoopSoundName);
			if (loopSoundPath != null)
			{
				if (this.loopSound == null)
				{
					this.loopSound = base.gameObject.AddComponent<AudioSource>();
				}
				else
				{
					this.loopSound.Stop();
				}
				this.loopSound.clip = Resources.Load(loopSoundPath) as AudioClip;
				if (thing.version >= 9 && thing.hasSurroundSound)
				{
					this.loopSound.spatialBlend = 0f;
				}
				else
				{
					this.loopSound.spatialBlend = listener.loopSpatialBlend;
				}
				this.loopSound.volume = 0.3f * listener.loopVolume;
				this.loopSound.loop = true;
				this.loopSound.Play();
			}
		}
		if (listener.doEndLoopSound && this.loopSound != null)
		{
			this.loopSound.Stop();
			global::UnityEngine.Object.Destroy(this.loopSound);
			Managers.soundLibraryManager.loopSoundsStartedCount--;
		}
		if (listener.emitId != null && (!this.isInInventoryOrDialog || this.isGiftInDialog || this.activeEvenInInventory) && (this.lastEmitTime == -1f || this.lastEmitTime + 0.05f <= Time.time))
		{
			this.lastEmitTime = Time.time;
			if (!thing.omitAutoHapticFeedback)
			{
				this.TriggerShrinkingHapticPulse();
			}
			if (!this.GetIsOfThingBeingEdited() || Managers.areaManager.weAreEditorOfCurrentArea)
			{
				Managers.thingManager.EmitThingFromOrigin(ThingRequestContext.ThingPartEmitThing, base.transform, listener.emitId, listener.emitVelocityPercent, listener.emitIsGravityFree, this.omitAutoSounds || thing.omitAutoSounds);
			}
		}
		if (listener.doHapticPulse)
		{
			this.TriggerShrinkingHapticPulse();
		}
		if (listener.rotateThingSettings != null)
		{
			this.ApplyRotateThingSettings(thing, listener.rotateThingSettings);
		}
		if (listener.creationPartChangeMode != null && Our.mode == EditModes.Thing && !originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient && (this.IsPartOfPlacement() || Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false) || this.isInInventoryOrDialog))
		{
			Managers.creationPartChangeManager.Apply(listener.creationPartChangeMode, listener.creationPartChangeValues, listener.creationPartChangeIsForAll, listener.creationPartChangeIsLocal, listener.creationPartChangeIsRandom, base.transform, this.omitAutoSounds);
		}
		DialogType? showDialog = listener.showDialog;
		if (showDialog != null && !originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient && (this.IsPartOfPlacement() || Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false)))
		{
			DialogType? showDialog2 = listener.showDialog;
			this.ApplyShowCommand(showDialog2.Value, listener.showData);
		}
		if (listener.goToInventoryPage != 0 && !originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient && Our.mode == EditModes.Inventory)
		{
			Managers.inventoryManager.GoToPage(listener.goToInventoryPage - 1);
		}
		if (listener.setLightIntensity != -1f && this.light != null)
		{
			this.light.intensity = listener.setLightIntensity;
		}
		if (listener.setLightRange != -1f && this.light != null)
		{
			this.light.range = listener.setLightRange;
		}
		if (listener.setLightConeSize != -1f && this.light != null && this.materialType == MaterialTypes.SpotLight)
		{
			this.light.spotAngle = listener.setLightConeSize;
		}
		if (listener.doTypeText != string.Empty && !originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient && Our.mode == EditModes.Thing)
		{
			GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
			if (currentNonStartDialog != null)
			{
				string text = this.ReplaceTextPlaceholders(listener.doTypeText);
				if (currentNonStartDialog.name == DialogType.ThingPart.ToString())
				{
					ThingPartDialog component = currentNonStartDialog.GetComponent<ThingPartDialog>();
					component.ExecuteDoTypeTextCommand(text);
				}
				else if (currentNonStartDialog.name == DialogType.Keyboard.ToString())
				{
					KeyboardDialog component2 = currentNonStartDialog.GetComponent<KeyboardDialog>();
					component2.ExecuteDoTypeTextCommand(text);
				}
			}
		}
		Vector3? velocitySetter = listener.velocitySetter;
		if (velocitySetter != null)
		{
			Thing thing2 = thing;
			Vector3? velocitySetter2 = listener.velocitySetter;
			thing2.ApplyVelocitySetterIfNeeded(velocitySetter2.Value);
		}
		Vector3? forceAdder = listener.forceAdder;
		if (forceAdder != null)
		{
			Thing thing3 = thing;
			Vector3? forceAdder2 = listener.forceAdder;
			thing3.ApplyForceAdderIfNeeded(forceAdder2.Value);
		}
		Vector3? velocityMultiplier = listener.velocityMultiplier;
		if (velocityMultiplier != null)
		{
			Thing thing4 = thing;
			Vector3? velocityMultiplier2 = listener.velocityMultiplier;
			thing4.ApplyVelocityMultiplierIfNeeded(velocityMultiplier2.Value);
		}
		if (listener.variableOperations != null && Our.IsMasterClient(true))
		{
			foreach (string text2 in listener.variableOperations)
			{
				this.HandleVariableOperation(text2);
			}
		}
		if (listener.attachThingIdAsHead != null && (this.IsPartOfPlacement() || Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false)) && (listener.attachToMultiplePeople || (!originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient)) && Our.mode != EditModes.Thing && Our.mode != EditModes.Environment && Our.mode != EditModes.Inventory)
		{
			this.OpenApproveBodyDialogIfAppropriate(listener.attachThingIdAsHead, false);
		}
		float? showNameTagsAgainSeconds = listener.showNameTagsAgainSeconds;
		if (showNameTagsAgainSeconds != null && (this.IsPartOfPlacement() || Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false)))
		{
			PersonManager personManager = Managers.personManager;
			float? showNameTagsAgainSeconds2 = listener.showNameTagsAgainSeconds;
			personManager.AddOrTimeExtendNameTagsForAllOthers(showNameTagsAgainSeconds2.Value);
		}
		float? resizeNearby = listener.resizeNearby;
		if (resizeNearby != null && ((this.IsPartOfPlacement() && this.OurPersonIsClose(2f)) || Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false)))
		{
			float? resizeNearby2 = listener.resizeNearby;
			float num6 = resizeNearby2.Value;
			if (!this.IsPartOfPlacement() && !(Managers.areaManager.rights.anyPersonSize == true) && !Managers.areaManager.weAreEditorOfCurrentArea)
			{
				num6 = Mathf.Clamp(num6, 0.01f, 1.5f);
			}
			Managers.personManager.ApplyAndCachePhotonRigScale(num6, thing.omitAutoSounds);
		}
		if (listener.letGo && Our.mode != EditModes.Thing)
		{
			thing.LetGoIfWeAreHoldableInHand();
		}
		bool? streamMyCameraView = listener.streamMyCameraView;
		if (streamMyCameraView != null && Our.mode != EditModes.Thing)
		{
			if (listener.streamMyCameraView == true)
			{
				this.isCamera = true;
				string streamTargetName = listener.streamTargetName;
				if (streamTargetName != null)
				{
					if (streamTargetName == "nearest")
					{
						Managers.cameraManager.StartStreamToVideoScreen(base.gameObject, null);
						goto IL_1005;
					}
					if (streamTargetName == "desktop")
					{
						if (!originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient)
						{
							Managers.cameraManager.StartStreamToDesktop(base.gameObject);
						}
						goto IL_1005;
					}
				}
				Managers.cameraManager.StartStreamToVideoScreen(base.gameObject, listener.streamTargetName);
				IL_1005:;
			}
			else if (!Managers.cameraManager.isStreamingToDesktop || (!originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient))
			{
				Managers.cameraManager.StopAllStreaming();
			}
		}
		if (listener.say != null)
		{
			string text3 = this.ReplaceTextPlaceholders(listener.say);
			Managers.behaviorScriptManager.Speak(this, text3, this.voiceProperties);
		}
		if (listener.setVoiceProperties != null)
		{
			this.voiceProperties = listener.setVoiceProperties;
		}
		float? setCustomSnapAngles = listener.setCustomSnapAngles;
		if (setCustomSnapAngles != null && Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false))
		{
			CreationHelper.customSnapAngles = ((!(listener.setCustomSnapAngles == 0f)) ? listener.setCustomSnapAngles : null);
		}
		FollowerCameraPosition? setFollowerCameraPosition = listener.setFollowerCameraPosition;
		if (setFollowerCameraPosition == null)
		{
			float? setFollowerCameraLerp = listener.setFollowerCameraLerp;
			if (setFollowerCameraLerp == null)
			{
				goto IL_11C5;
			}
		}
		if (Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false))
		{
			GameObject @object = Managers.treeManager.GetObject("/Universe/FollowerCamera");
			FollowerCamera component3 = @object.GetComponent<FollowerCamera>();
			component3.Init();
			FollowerCameraPosition? setFollowerCameraPosition2 = listener.setFollowerCameraPosition;
			if (setFollowerCameraPosition2 != null)
			{
				FollowerCamera followerCamera = component3;
				FollowerCameraPosition? setFollowerCameraPosition3 = listener.setFollowerCameraPosition;
				followerCamera.cameraPosition = setFollowerCameraPosition3.Value;
			}
			float? setFollowerCameraLerp2 = listener.setFollowerCameraLerp;
			if (setFollowerCameraLerp2 != null)
			{
				FollowerCamera followerCamera2 = component3;
				float? setFollowerCameraLerp3 = listener.setFollowerCameraLerp;
				followerCamera2.lerpFraction = setFollowerCameraLerp3.Value;
			}
			component3.UpdateByProperties();
			bool flag = component3.cameraPosition == FollowerCameraPosition.InHeadVr && component3.lerpFraction == 1f;
			@object.SetActive(!flag);
		}
		IL_11C5:
		Vector3? setGravity = listener.setGravity;
		if (setGravity != null && this.IsPartOfPlacement())
		{
			Vector3? setGravity2 = listener.setGravity;
			Vector3 value3 = setGravity2.Value;
			Physics.gravity = value3;
			if (!originatesFromOtherClient)
			{
				Managers.broadcastNetworkManager.UpdatePhotonCustomRoomProperty("gravity", Misc.GetVector3ToSpaceSeparatedString(value3));
			}
		}
		if (listener.resetSettings != null)
		{
			if (this.IsPartOfPlacement())
			{
				if (listener.resetSettings.area)
				{
					Managers.behaviorScriptManager.ResetArea();
				}
				if (listener.resetSettings.allPersonVariablesInArea)
				{
					Managers.behaviorScriptManager.ResetAllPersonVariablesInArea();
				}
				if (listener.resetSettings.position)
				{
					this.GetMyRootThing().RestoreOriginalPosition();
				}
				if (listener.resetSettings.rotation)
				{
					this.GetMyRootThing().RestoreOriginalRotation();
				}
			}
			if (listener.resetSettings.body && (this.IsPartOfPlacement() || Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false)) && !originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient && Our.mode != EditModes.Thing)
			{
				this.OpenApproveBodyDialogIfAppropriate(null, true);
			}
			if (listener.resetSettings.legsToUniversalDefault && Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false))
			{
				Managers.personManager.ourPerson.ResetLegsPositionRotationToUniversalDefault(null);
				Managers.personManager.SaveOurLegAttachmentPointPositions();
			}
			if (listener.resetSettings.legsToBodyDefault && Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false))
			{
				Managers.personManager.ourPerson.ResetLegsPositionRotationToBodyOrUniversalDefault();
				Managers.personManager.SaveOurLegAttachmentPointPositions();
			}
		}
		if (listener.setText != null && this.isText)
		{
			this.SetOriginalText(listener.setText);
		}
		if (listener.turn != null && !this.GetIsOfThingBeingEdited() && this.WeAreAuthorityForTurnCommandsForThis())
		{
			this.ApplyTurnCommandToProperties(listener.turn);
			this.ApplyCurrentInvisibleAndCollidable();
			Managers.personManager.DoInformOfThingPartInvisibleUncollidableChange(this);
		}
		if (listener.turnThing != null && !this.GetIsOfThingBeingEdited() && this.WeAreAuthorityForTurnCommandsForThis())
		{
			thing.ApplyTurnCommandToProperties(listener.turnThing);
			thing.UpdateAllVisibilityAndCollision(false, false);
			Managers.personManager.DoInformOfThingInvisibleUncollidableChange(thing);
		}
		if (listener.turnSubThing != null && !this.GetIsOfThingBeingEdited() && this.WeAreAuthorityForTurnCommandsForThis())
		{
			foreach (IncludedSubThing includedSubThing in this.includedSubThings)
			{
				GameObject assignedObject = includedSubThing.assignedObject;
				if (assignedObject != null && (listener.turnSubThingName == null || assignedObject.name == listener.turnSubThingName))
				{
					Thing component4 = assignedObject.GetComponent<Thing>();
					if (component4 != null)
					{
						component4.ApplyTurnCommandToProperties(listener.turnSubThing);
						component4.UpdateAllVisibilityAndCollision(false, false);
						Managers.personManager.DoInformOfThingInvisibleUncollidableChange(component4);
					}
				}
			}
		}
		if (listener.playVideoId != null)
		{
			float? playVideoVolume = listener.playVideoVolume;
			if (playVideoVolume != null)
			{
				VideoManager videoManager = Managers.videoManager;
				float? playVideoVolume2 = listener.playVideoVolume;
				videoManager.SetVolume(playVideoVolume2.Value);
			}
			VideoManager videoManager2 = Managers.videoManager;
			string playVideoId = listener.playVideoId;
			string text4 = "Video";
			Transform transform2 = base.transform;
			videoManager2.WePlayVideoAtNearestThingPartScreen(playVideoId, text4, null, transform2);
		}
		if (listener.partTrailSettings != null)
		{
			PartTrail partTrail = base.gameObject.GetComponent<PartTrail>();
			if (listener.partTrailSettings.isStart)
			{
				if (partTrail == null)
				{
					partTrail = base.gameObject.AddComponent<PartTrail>();
				}
				partTrail.settings = listener.partTrailSettings;
			}
			else if (partTrail != null)
			{
				global::UnityEngine.Object.Destroy(partTrail);
			}
		}
		if (listener.projectPartSettings != null && !this.GetIsOfThingBeingEdited())
		{
			ProjectPart projectPart = base.gameObject.GetComponent<ProjectPart>();
			if (listener.projectPartSettings.relativeReach > 0f)
			{
				if (projectPart == null)
				{
					projectPart = base.gameObject.AddComponent<ProjectPart>();
				}
				projectPart.settings = listener.projectPartSettings;
				this.pushBackEnabled = false;
			}
			else if (projectPart != null)
			{
				global::UnityEngine.Object.Destroy(projectPart);
				this.pushBackEnabled = true;
			}
		}
		if (listener.partLineSettings != null && !this.GetIsOfThingBeingEdited())
		{
			PartLine partLine = base.gameObject.GetComponent<PartLine>();
			if (listener.partLineSettings.startWidth == 0f || listener.partLineSettings.endWidth == 0f)
			{
				if (partLine != null)
				{
					global::UnityEngine.Object.Destroy(partLine);
				}
			}
			else
			{
				if (partLine == null)
				{
					partLine = base.gameObject.AddComponent<PartLine>();
				}
				partLine.settings = listener.partLineSettings;
			}
		}
		if (listener.browserSettings != null && !this.GetIsOfThingBeingEdited() && Our.IsMasterClient(true) && !this.isInInventoryOrDialog && (this.IsPartOfPlacement() || Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false)))
		{
			bool flag2 = !Our.IsMasterClient(false);
			Managers.browserManager.TryAttachBrowser(this, listener.browserSettings, flag2);
		}
		float? limitAreaVisibilityMeters = listener.limitAreaVisibilityMeters;
		if (limitAreaVisibilityMeters != null && this.IsPartOfPlacement())
		{
			Managers.areaManager.limitVisibilityMeters = ((!(listener.limitAreaVisibilityMeters == -1f)) ? listener.limitAreaVisibilityMeters : null);
			Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
		}
		Vector3? constantRotation = listener.constantRotation;
		if (constantRotation != null && !this.GetIsOfThingBeingEdited())
		{
			Vector3? constantRotation2 = listener.constantRotation;
			Vector3 value4 = constantRotation2.Value;
			PartConstantRotation partConstantRotation = base.gameObject.GetComponent<PartConstantRotation>();
			if (value4 != Vector3.zero)
			{
				if (partConstantRotation == null)
				{
					partConstantRotation = base.gameObject.AddComponent<PartConstantRotation>();
				}
				partConstantRotation.speed = value4;
			}
			else if (partConstantRotation != null)
			{
				global::UnityEngine.Object.Destroy(partConstantRotation);
			}
		}
		if (listener.settings != null && !originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient && Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false) && Managers.dialogManager.GetCurrentNonStartDialogType() != DialogType.ApproveBody)
		{
			this.EnablePersistStatesForWearerOnly();
			foreach (KeyValuePair<Setting, bool> keyValuePair2 in listener.settings)
			{
				Managers.settingManager.SetState(keyValuePair2.Key, keyValuePair2.Value, true);
			}
		}
		if (listener.makePersonMasterClient && !originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient && (this.IsPartOfPlacement() || (Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false) && Managers.areaManager.weAreEditorOfCurrentArea)))
		{
			Managers.personManager.ourPerson.MakeMasterClientManually();
		}
		if (listener.questAction != null && !originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient && this.IsPartOfPlacement())
		{
			Managers.questManager.HandleQuestAction(listener.questAction);
		}
		if (listener.attractThingsSettings != null && !this.GetIsOfThingBeingEdited())
		{
			this.ApplyAttractThingsSettings(listener.attractThingsSettings, false);
		}
		if (listener.desktopModeSettings != null && this.IsPartOfPlacement())
		{
			this.ApplyDesktopModeSettings(listener.desktopModeSettings);
		}
		if (listener.destroyOtherThings != null)
		{
			bool? anyDestruction2 = Managers.areaManager.rights.anyDestruction;
			if (anyDestruction2.Value)
			{
				Managers.behaviorScriptManager.DestroyOtherThingsInRadius(this, listener.destroyOtherThings);
			}
		}
		if (listener.destroyThingWeArePartOf != null && !this.GetIsOfThingBeingEdited())
		{
			Managers.behaviorScriptManager.DestroyThing(this.GetMyRootThing(), listener.destroyThingWeArePartOf);
		}
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x000D56EC File Offset: 0x000D3AEC
	private void HandleTell(Thing thing, TellType tellType, string tellData)
	{
		if (tellData.Contains("["))
		{
			tellData = this.ReplaceTextPlaceholders(tellData);
			tellData = tellData.ToLower().Trim();
		}
		switch (tellType)
		{
		case TellType.Self:
		{
			Thing myRootThing = this.GetMyRootThing();
			if (myRootThing != null)
			{
				if (thing != null && thing.subThingMasterPart != null)
				{
					myRootThing.TriggerEvent(StateListener.EventType.OnTold, tellData + " by " + thing.transform.name, false, null);
				}
				myRootThing.TriggerEvent(StateListener.EventType.OnTold, tellData, false, null);
			}
			this.TriggerEventTowardsPlacedSubThingMaster(StateListener.EventType.OnTold, tellData);
			break;
		}
		case TellType.Nearby:
			Managers.behaviorScriptManager.TriggerTellNearbyEvent(tellData, base.transform.position);
			break;
		case TellType.Any:
			Managers.behaviorScriptManager.TriggerTellAnyEvent(tellData, false);
			break;
		case TellType.Body:
			if (Managers.personManager != null && base.transform.parent != null)
			{
				Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(base.transform.parent.gameObject);
				if (personThisObjectIsOf)
				{
					Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(personThisObjectIsOf, tellData, false);
				}
			}
			break;
		case TellType.FirstOfAny:
			if (base.transform.parent != null)
			{
				Managers.behaviorScriptManager.TriggerTellFirstOfAnyEvent(tellData, base.transform.position, base.transform.parent.gameObject);
			}
			break;
		case TellType.Web:
		{
			tellData = Managers.behaviorScriptManager.RemovePrivacyRelevantFromWebTellData(tellData);
			Browser browser = (Browser)base.transform.parent.GetComponentInChildren(typeof(Browser), true);
			if (browser != null)
			{
				bool flag = Our.IsMasterClient(true);
				browser.CallFunction("AnylandTold", new JSONNode[] { tellData, flag }).Done();
			}
			break;
		}
		case TellType.AnyWeb:
			tellData = Managers.behaviorScriptManager.RemovePrivacyRelevantFromWebTellData(tellData);
			Debug.Log("tellData = " + tellData);
			Managers.behaviorScriptManager.TriggerTellAnyWebEvent(tellData);
			break;
		case TellType.InFront:
			Managers.behaviorScriptManager.TriggerTellInFront(tellData, this, false);
			break;
		case TellType.FirstInFront:
			Managers.behaviorScriptManager.TriggerTellInFront(tellData, this, true);
			break;
		}
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x000D5940 File Offset: 0x000D3D40
	private void ApplyRotateThingSettings(Thing thing, RotateThingSettings settings)
	{
		if ((settings.startTowardsClosestPerson || settings.startTowardsSecondClosestPerson) && Our.mode != EditModes.Thing)
		{
			if (settings.startTowardsClosestPerson)
			{
				thing.StartRotateTowardsClosestPerson();
			}
			else
			{
				thing.StartRotateTowardsSecondClosestPerson();
			}
		}
		if (settings.startTowardsTop && Our.mode != EditModes.Thing)
		{
			thing.StartRotateTowardsTop();
		}
		if (settings.startTowardsClosestEmptyHand && Our.mode != EditModes.Thing)
		{
			thing.StartRotateTowardsClosestEmptyHand();
		}
		if (settings.startTowardsClosestEmptyHandWhileHeld && Our.mode != EditModes.Thing && thing.isHeldAsHoldable)
		{
			thing.StartRotateTowardsClosestEmptyHand();
		}
		if (settings.startTowardsClosestThingOfName != null && Our.mode != EditModes.Thing)
		{
			thing.StartRotateTowardsClosestThingOfName(settings.startTowardsClosestThingOfName, settings);
		}
		if (settings.stopTowardsPerson)
		{
			thing.StopRotateTowardsPerson();
		}
		if (settings.stopTowardsTop)
		{
			thing.StopRotateTowardsTop();
		}
		if (settings.stopTowardsClosestEmptyHand)
		{
			thing.StopRotateTowardsClosestEmptyHand();
		}
		if (settings.stopTowardsClosestThingOfName)
		{
			thing.StopRotateThingTowardsClosestThingOfName();
		}
		if (!this.GetIsOfThingBeingEdited())
		{
			if (settings.startTowardsMainCamera)
			{
				thing.StartRotateTowardsMainCamera();
			}
			if (settings.stopTowardsMainCamera)
			{
				thing.StopRotateTowardsMainCamera();
			}
		}
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x000D5A80 File Offset: 0x000D3E80
	private void ApplyDesktopModeSettings(DesktopModeSettings settings)
	{
		float? runSpeed = settings.runSpeed;
		if (runSpeed != null)
		{
			DesktopManager desktopManager = Managers.desktopManager;
			float? runSpeed2 = settings.runSpeed;
			desktopManager.fastMovementSpeed = runSpeed2.Value;
		}
		float? jumpSpeed = settings.jumpSpeed;
		if (jumpSpeed != null)
		{
			DesktopManager desktopManager2 = Managers.desktopManager;
			float? jumpSpeed2 = settings.jumpSpeed;
			desktopManager2.jumpSpeed = jumpSpeed2.Value;
		}
		float? slidiness = settings.slidiness;
		if (slidiness != null)
		{
			DesktopManager desktopManager3 = Managers.desktopManager;
			float? slidiness2 = settings.slidiness;
			desktopManager3.slidiness = slidiness2.Value;
		}
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x000D5B10 File Offset: 0x000D3F10
	private void ApplyAttractThingsSettings(AttractThingsSettings settings, bool overwriteStrengthOnly = false)
	{
		AttractThings attractThings = base.GetComponent<AttractThings>();
		if (settings.strength != 0f)
		{
			if (attractThings == null)
			{
				attractThings = base.gameObject.AddComponent<AttractThings>();
			}
			if (overwriteStrengthOnly)
			{
				attractThings.settings.strength = settings.strength;
			}
			else
			{
				attractThings.settings = settings;
			}
		}
		else if (attractThings != null)
		{
			global::UnityEngine.Object.Destroy(attractThings);
		}
	}

	// Token: 0x06001794 RID: 6036 RVA: 0x000D5B88 File Offset: 0x000D3F88
	private void ApplyShowCommand(DialogType dialogType, string data)
	{
		Hand hand = Misc.GetAHandOfOurs();
		if (hand != null)
		{
			switch (dialogType)
			{
			case DialogType.Inventory:
				if (Managers.inventoryManager.CheckIfMayOpenAndAlertIfNot(hand))
				{
					HandDot component = hand.handDot.GetComponent<HandDot>();
					component.holdableInHand = Managers.inventoryManager.OpenDialog(hand, true);
					Our.SetMode(EditModes.Inventory, false);
					Managers.soundManager.Play("clone", base.transform, 0.3f, false, false);
				}
				break;
			default:
				if (dialogType != DialogType.CameraControl)
				{
					if (dialogType == DialogType.FindAreas)
					{
						Our.lastAreasSearchText = data;
						GameObject gameObject = Managers.dialogManager.SwitchToNewDialog(DialogType.FindAreas, null, string.Empty);
						break;
					}
					if (dialogType != DialogType.SlideshowControl && dialogType != DialogType.VideoControl)
					{
						if (dialogType == DialogType.Forum)
						{
							if (Universe.features.forums)
							{
								Managers.forumManager.OpenForumByName(hand, data, true);
							}
							break;
						}
						if (dialogType != DialogType.ForumThread)
						{
							break;
						}
						if (Universe.features.forums)
						{
							Managers.forumManager.OpenThreadById(hand, data, true);
						}
						break;
					}
				}
				Managers.dialogManager.SwitchToNewDialog(dialogType, null, string.Empty);
				break;
			case DialogType.Keyboard:
				Managers.dialogManager.GetInput(delegate(string text)
				{
					if (!string.IsNullOrEmpty(text))
					{
						Managers.personManager.DoAddTypedText(text);
					}
					hand.SwitchToNewDialog(DialogType.Start, string.Empty);
				}, string.Empty, string.Empty, 250, string.Empty, true, false, false, false, 1f, false, false, null, false);
				break;
			}
		}
	}

	// Token: 0x06001795 RID: 6037 RVA: 0x000D5D24 File Offset: 0x000D4124
	private bool WeAreAuthorityForTurnCommandsForThis()
	{
		Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(base.gameObject);
		return (!(personThisObjectIsOf != null)) ? Our.IsMasterClient(false) : personThisObjectIsOf.isOurPerson;
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x000D5D60 File Offset: 0x000D4160
	private void ApplyTurnCommandToProperties(string command)
	{
		if (command != null)
		{
			if (!(command == "on"))
			{
				if (!(command == "off"))
				{
					if (!(command == "visible"))
					{
						if (!(command == "invisible"))
						{
							if (!(command == "collidable"))
							{
								if (command == "uncollidable")
								{
									this.uncollidable = true;
								}
							}
							else
							{
								this.uncollidable = false;
							}
						}
						else
						{
							this.invisible = true;
						}
					}
					else
					{
						this.invisible = false;
					}
				}
				else
				{
					this.invisible = true;
					this.uncollidable = true;
				}
			}
			else
			{
				this.invisible = false;
				this.uncollidable = false;
			}
		}
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x000D5E30 File Offset: 0x000D4230
	private void HandleTransportToAreaOrThingCommand(StateListener listener, bool originatesFromOtherClient)
	{
		if (Our.mode == EditModes.Thing || Our.mode == EditModes.Body)
		{
			return;
		}
		Thing parentThing = this.GetParentThing();
		if (parentThing == null)
		{
			return;
		}
		bool flag = this.OurPersonIsClose(2f);
		bool flag2 = listener.transportMultiplePeople;
		bool flag3 = listener.transportNearbyOnly;
		if (parentThing.version <= 1 && parentThing.isSticky && parentThing.isThrownOrEmitted && flag2)
		{
			flag2 = false;
		}
		bool? flag4 = null;
		if (!flag2)
		{
			flag3 = true;
			if (parentThing.isThrownOrEmitted)
			{
				flag4 = new bool?(Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, true) || Managers.thingManager.IsClosestSurfaceNearbyOurPerson(this, 2f));
			}
			else if (this.IsPartOfPlacement())
			{
				flag4 = new bool?(flag && (!originatesFromOtherClient && !this.stateSwitchOriginatedFromOtherClient));
			}
			else
			{
				flag4 = new bool?(Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false));
			}
		}
		if ((flag2 && (flag || !flag3)) || (!flag2 && flag4.Value))
		{
			if (Managers.personManager.ourPerson.isSoftBanned)
			{
				Managers.soundManager.Play("no", base.transform, 1f, false, false);
				Managers.dialogManager.SwitchToNewDialog(DialogType.Softban, null, string.Empty);
			}
			else
			{
				string area = this.ReplaceTextPlaceholders(listener.transportToArea);
				string viaArea = this.ReplaceTextPlaceholders(listener.transportViaArea);
				string ontoThing = this.ReplaceTextPlaceholders(listener.transportOntoThing);
				bool isThisObjectOfOurPerson = Managers.personManager.GetIsThisObjectOfOurPerson(base.gameObject, false);
				Managers.areaManager.rotationAfterTransport = listener.rotationAfterTransport;
				float viaSeconds = listener.transportViaAreaSeconds;
				bool flag5 = this.IsPartOfPlacement();
				if (!string.IsNullOrEmpty(area))
				{
					if (Managers.areaManager.rights.emittedTransporting == true || isThisObjectOfOurPerson || flag5)
					{
						this.HandleAreaTransport(area, viaArea, ontoThing, viaSeconds);
					}
					else
					{
						Hand hand = Misc.GetAHandOfOurs();
						if (hand != null && hand.currentDialogType != DialogType.Confirm)
						{
							Dialog component = hand.GetComponent<Dialog>();
							if (component != null)
							{
								string text = "Go to " + area + "?";
								component.SwitchToConfirmDialog(text, delegate(bool didConfirm)
								{
									if (didConfirm)
									{
										this.HandleAreaTransport(area, viaArea, ontoThing, viaSeconds);
									}
									else
									{
										hand.SwitchToNewDialog(DialogType.Start, string.Empty);
									}
								});
							}
						}
					}
				}
				else if (Managers.areaManager.rights.emittedClimbing == true || isThisObjectOfOurPerson || flag5)
				{
					Managers.areaManager.SendOurPersonToThingInArea(ontoThing, this.omitAutoSounds);
				}
			}
		}
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x000D6188 File Offset: 0x000D4588
	public void HandleAreaTransport(string area, string viaArea, string ontoThing, float transportViaAreaSeconds)
	{
		if (area == "current")
		{
			area = Managers.areaManager.currentAreaUrlName;
			Managers.areaManager.SetPositionAndRotationToTransportToToCurrent();
		}
		else if (area == "previous" && !Managers.personManager.ourPerson.isSoftBanned)
		{
			area = Managers.areaManager.previousAreaUrlName;
			Managers.areaManager.SetPositionAndRotationToTransportToToPrevious();
		}
		if (!string.IsNullOrEmpty(area))
		{
			if (!string.IsNullOrEmpty(viaArea))
			{
				Managers.areaManager.SetAreaToTransportToAfterNextAreaLoad(area, transportViaAreaSeconds);
				area = viaArea;
			}
			Managers.areaManager.TryTransportToAreaByNameOrUrlName(area, ontoThing, this.omitAutoSounds);
		}
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x000D6234 File Offset: 0x000D4634
	private void OpenApproveBodyDialogIfAppropriate(string attachThingIdAsHead = null, bool clearBody = false)
	{
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		DialogType dialogType = DialogType.None;
		bool flag = false;
		if (currentNonStartDialog != null)
		{
			if (currentNonStartDialog.name == DialogType.ApproveBody.ToString())
			{
				flag = true;
			}
			else if (currentNonStartDialog.name == DialogType.OwnProfile.ToString())
			{
				dialogType = DialogType.OwnProfile;
			}
		}
		if (!flag)
		{
			Hand ahandOfOurs = Misc.GetAHandOfOurs();
			GameObject gameObject = ahandOfOurs.SwitchToNewDialog(DialogType.ApproveBody, string.Empty);
			ApproveBodyDialog component = gameObject.GetComponent<ApproveBodyDialog>();
			if (component)
			{
				if (dialogType != DialogType.None)
				{
					component.dialogToGoBackTo = dialogType;
				}
				component.attachThingIdAsHead = attachThingIdAsHead;
				component.clearBody = clearBody;
			}
		}
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x000D62F0 File Offset: 0x000D46F0
	private void HandleVariableOperation(string fullExpression)
	{
		if (!Managers.behaviorScriptManager.RegisterNewVariableCalculationAttempted())
		{
			return;
		}
		fullExpression = fullExpression.Replace("++", "+= 1");
		fullExpression = fullExpression.Replace("--", "-= 1");
		bool flag = !fullExpression.Contains("=");
		if (flag)
		{
			if (fullExpression.StartsWith("not "))
			{
				fullExpression = fullExpression.Substring("not ".Length);
				fullExpression += " = 0";
			}
			else
			{
				fullExpression += " = 1";
			}
		}
		string[] array = new string[] { "+=", "-=", "*=", "/=", "=" };
		string[] array2 = array;
		int i = 0;
		while (i < array2.Length)
		{
			string text = array2[i];
			string[] array3 = Misc.Split(fullExpression, text, StringSplitOptions.RemoveEmptyEntries);
			if (array3.Length == 2)
			{
				string text2 = Managers.behaviorScriptManager.NormalizeVariableName(array3[0]);
				text2 = this.ReplaceTextPlaceholdersForVariableExpression(text2);
				BehaviorScriptVariableScope variableScope = Managers.behaviorScriptManager.GetVariableScope(text2);
				if (variableScope == BehaviorScriptVariableScope.None)
				{
					Log.Debug("Rejected variable name " + text2);
					return;
				}
				string text3 = array3[1].Trim().ToLower();
				Thing component = base.transform.parent.gameObject.GetComponent<Thing>();
				ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
				string specifierId = component.GetSpecifierId(ref thingSpecifierType);
				Person person = null;
				float num = 0f;
				if (variableScope != BehaviorScriptVariableScope.Thing)
				{
					if (variableScope != BehaviorScriptVariableScope.Area)
					{
						if (variableScope == BehaviorScriptVariableScope.Person)
						{
							person = Managers.behaviorScriptManager.GetPersonVariablesRelevantPerson(this);
							if (!(person != null))
							{
								return;
							}
							num = person.GetBehaviorScriptVariable(text2);
						}
					}
					else
					{
						num = Managers.areaManager.GetBehaviorScriptVariable(text2);
					}
				}
				else
				{
					num = component.GetBehaviorScriptVariable(text2);
				}
				text3 = this.ReplaceTextPlaceholdersForVariableExpression(text3);
				text3 = BehaviorScriptManager.ReplaceVariablesWithValues(component, this, text3, null, false);
				MathParser mathParser = new MathParser();
				float num2 = 0f;
				try
				{
					num2 = (float)mathParser.Parse(text3, false);
				}
				catch (Exception ex)
				{
					Log.Debug("Guarded against someone using \"" + text3 + "\"");
					break;
				}
				float num3 = 0f;
				if (text != null)
				{
					if (!(text == "="))
					{
						if (!(text == "+="))
						{
							if (!(text == "-="))
							{
								if (!(text == "*="))
								{
									if (text == "/=")
									{
										if (num2 == 0f)
										{
											Log.Debug("Guarded against someone dividing by zero");
											return;
										}
										num3 = num / num2;
									}
								}
								else
								{
									num3 = num * num2;
								}
							}
							else
							{
								num3 = num - num2;
							}
						}
						else
						{
							num3 = num + num2;
						}
					}
					else
					{
						num3 = num2;
					}
				}
				if (num != num3)
				{
					bool flag2 = Our.IsMasterClient(false);
					if (variableScope != BehaviorScriptVariableScope.Thing)
					{
						if (variableScope != BehaviorScriptVariableScope.Area)
						{
							if (variableScope == BehaviorScriptVariableScope.Person)
							{
								if (person != null)
								{
									person.SetBehaviorScriptVariable(text2, num3);
									if (flag2)
									{
										Managers.personManager.DoInformOfPersonBehaviorScriptVariableChange(person, text2, num3);
									}
									Managers.behaviorScriptManager.TriggerVariableChangeToThings(text2, person);
								}
							}
						}
						else
						{
							Managers.areaManager.SetBehaviorScriptVariable(text2, num3);
							if (flag2)
							{
								Managers.personManager.DoInformOfAreaBehaviorScriptVariableChange(text2, num3);
							}
							Managers.behaviorScriptManager.TriggerVariableChangeToThings(text2, null);
						}
					}
					else
					{
						component.SetBehaviorScriptVariable(text2, num3);
						if (flag2)
						{
							Managers.personManager.DoInformOfThingBehaviorScriptVariableChange(component, text2, num3);
						}
						component.TriggerEvent(StateListener.EventType.OnVariableChange, text2, false, null);
					}
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x000D66DC File Offset: 0x000D4ADC
	private void ResetStateSwitchOriginatedFromOtherClient()
	{
		this.stateSwitchOriginatedFromOtherClient = false;
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x000D66E8 File Offset: 0x000D4AE8
	private void TriggerEventTowardsPlacedSubThingMaster(StateListener.EventType eventType, string data = "")
	{
		bool flag = base.transform.parent != null && base.transform.parent.parent != null && base.transform.parent.parent.parent != null && base.transform.parent.parent.CompareTag("PlacedSubThings");
		if (flag)
		{
			ThingPart[] componentsInChildren = base.transform.parent.parent.parent.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				thingPart.TriggerEvent(eventType, data, false, null);
			}
		}
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x000D67B0 File Offset: 0x000D4BB0
	public void SetOriginalText(string text)
	{
		this.originalText = text;
		if (this.textMesh == null)
		{
			this.textMesh = base.GetComponent<TextMesh>();
			this.UpdateTextAlignmentAndMore(false);
		}
		if (this.textMesh != null)
		{
			this.textMesh.text = this.originalText;
			this.hasTextPlaceholders = text.IndexOf("[") >= 0 && text.IndexOf("]") >= 0;
			this.UpdateTextCollider();
		}
		else
		{
			this.hasTextPlaceholders = false;
		}
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x000D6848 File Offset: 0x000D4C48
	public void UpdateTextAlignmentAndMore(bool alsoUpdateCollider = false)
	{
		if (this.textAlignCenter)
		{
			this.textMesh.alignment = TextAlignment.Center;
			this.textMesh.anchor = TextAnchor.UpperCenter;
		}
		else if (this.textAlignRight)
		{
			this.textMesh.alignment = TextAlignment.Right;
			this.textMesh.anchor = TextAnchor.UpperRight;
		}
		else
		{
			this.textMesh.alignment = TextAlignment.Left;
			this.textMesh.anchor = TextAnchor.UpperLeft;
		}
		this.textMesh.lineSpacing = this.textLineHeight;
		if (alsoUpdateCollider)
		{
			this.UpdateTextCollider();
		}
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x000D68DC File Offset: 0x000D4CDC
	private void UpdateTextPlaceholders()
	{
		if (this.hasTextPlaceholders)
		{
			if (this.textMesh == null)
			{
				this.textMesh = base.GetComponent<TextMesh>();
			}
			if (!this.GetIsOfThingBeingEdited())
			{
				this.textMesh.text = this.ReplaceTextPlaceholders(this.originalText);
			}
		}
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x000D6934 File Offset: 0x000D4D34
	public bool IsPartOfPlacement()
	{
		Thing myRootThing = this.GetMyRootThing();
		return myRootThing != null && myRootThing.IsPlacement() && myRootThing.gameObject != CreationHelper.thingBeingEdited && !this.isInInventoryOrDialog;
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x000D6980 File Offset: 0x000D4D80
	private bool UseTextureAsSkyAtTheMoment()
	{
		bool flag = this.useTextureAsSky;
		Thing parentThing = this.GetParentThing();
		if (flag && parentThing != null)
		{
			flag = CreationHelper.thingBeingEdited == parentThing.gameObject || !string.IsNullOrEmpty(parentThing.placementId);
		}
		return flag;
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x000D69D8 File Offset: 0x000D4DD8
	private bool OurPersonIsClose(float distanceConsideredClose)
	{
		bool flag = false;
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		if (@object != null)
		{
			Vector3 vector = base.transform.position;
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				vector = component.ClosestPointOnBounds(@object.transform.position);
			}
			float num = Vector3.Distance(@object.transform.position, vector);
			flag = num <= distanceConsideredClose;
		}
		return flag;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000D6A54 File Offset: 0x000D4E54
	public void ResetStates()
	{
		this.ForgetForUndo();
		if (this.currentState != 0)
		{
			if (this.currentState < this.states.Count)
			{
				this.states[this.currentState].didTriggerStartEvent = false;
			}
			this.currentState = 0;
			this.timeWhenToRevert = -1f;
		}
		this.states[this.currentState].didTriggerStartEvent = false;
		this.currentStateTarget = -1;
		this.currentStateTargetCurveVia = -1;
		this.stateSwitchOriginatedFromOtherClient = false;
		if (this.states.Count > 1)
		{
			this.SetTransformPropertiesByState(false, false);
		}
		this.soundTracks = null;
		this.stateTargetTime = -1f;
		this.stateTargetSeconds = -1f;
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x000D6B13 File Offset: 0x000D4F13
	public void ResetSoundTracks()
	{
		this.soundTracks = null;
		this.TriggerEvent(StateListener.EventType.OnStarts, string.Empty, false, null);
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x000D6B2C File Offset: 0x000D4F2C
	public void ResetAndStopStateAsCurrentlyEditing()
	{
		this.timeWhenToRevert = -1f;
		this.stateTargetTime = -1f;
		this.stateTargetSeconds = -1f;
		this.currentStateTarget = -1;
		this.currentStateTargetCurveVia = -1;
		this.stateSwitchOriginatedFromOtherClient = false;
		this.states[this.currentState].didTriggerStartEvent = false;
		this.SetTransformPropertiesByState(false, false);
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x000D6B90 File Offset: 0x000D4F90
	public void SetStatePropertiesByTransform(bool ignoreMaterialAndColor = false)
	{
		if (this.currentState > this.states.Count - 1)
		{
			this.currentState = 0;
		}
		this.MemorizeForUndo();
		this.states[this.currentState].position = base.transform.localPosition;
		this.states[this.currentState].rotation = base.transform.localEulerAngles;
		this.states[this.currentState].scale = base.transform.localScale;
		if (!ignoreMaterialAndColor)
		{
			if (this.IsAnyTransparent(this.materialType))
			{
				this.states[this.currentState].color = new Color(this.material.color.r, this.material.color.g, this.material.color.b, this.GetAlphaForAnyTransparent(this.materialType));
			}
			else
			{
				this.states[this.currentState].color = this.material.color;
			}
		}
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x000D6CC4 File Offset: 0x000D50C4
	private bool IsAnyTransparent(MaterialTypes materialType)
	{
		return materialType == MaterialTypes.Transparent || materialType == MaterialTypes.TransparentGlossy || materialType == MaterialTypes.TransparentGlossyMetallic || materialType == MaterialTypes.VeryTransparent || materialType == MaterialTypes.VeryTransparentGlossy || materialType == MaterialTypes.SlightlyTransparent || materialType == MaterialTypes.TransparentTexture || materialType == MaterialTypes.TransparentGlowTexture;
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x000D6D10 File Offset: 0x000D5110
	private float GetAlphaForAnyTransparent(MaterialTypes materialType)
	{
		switch (materialType)
		{
		case MaterialTypes.VeryTransparent:
		case MaterialTypes.VeryTransparentGlossy:
			return 0.2f;
		case MaterialTypes.SlightlyTransparent:
			return 0.82f;
		case MaterialTypes.TransparentTexture:
		case MaterialTypes.TransparentGlowTexture:
			return 1f;
		}
		return 0.5f;
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x000D6D78 File Offset: 0x000D5178
	public void SetTransformPropertiesByState(bool ignoreMaterialAndColor = false, bool forcedTexturesUpdate = false)
	{
		base.transform.localPosition = this.states[this.currentState].position;
		base.transform.localEulerAngles = this.states[this.currentState].rotation;
		base.transform.localScale = this.states[this.currentState].scale;
		if (!ignoreMaterialAndColor)
		{
			this.material.color = this.states[this.currentState].color;
			this.UpdateMaterial();
			this.UpdateTextures(forcedTexturesUpdate);
		}
		this.UpdateParticleSystem();
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x000D6E24 File Offset: 0x000D5224
	public void UpdateTextures(bool forcedUpdate = false)
	{
		bool flag = false;
		bool flag2 = false;
		bool isOfThingBeingEdited = this.GetIsOfThingBeingEdited();
		int num = 0;
		while (num < this.textureTypes.Length && !flag)
		{
			flag = this.textureTypes[num] != this.currentlyAppliedTextureTypes[num];
			num++;
		}
		if (flag || forcedUpdate)
		{
			List<Material> list = new List<Material>();
			for (int i = 0; i < this.textureTypes.Length; i++)
			{
				if (this.textureTypes[i] != TextureType.None || isOfThingBeingEdited)
				{
					if (this.textureTypes[i] != TextureType.None)
					{
						flag2 = true;
					}
					Material sharedOrDistinctTextureMaterial = Managers.thingManager.GetSharedOrDistinctTextureMaterial(this, i, isOfThingBeingEdited);
					list.Add(sharedOrDistinctTextureMaterial);
				}
				this.currentlyAppliedTextureTypes[i] = this.textureTypes[i];
			}
			if (flag2 || isOfThingBeingEdited)
			{
				this.SetRendererIfNeeded();
				Material[] materials = this.renderer.materials;
				Material[] array = new Material[list.Count + 1];
				array[0] = this.renderer.sharedMaterials[0];
				for (int j = 0; j < list.Count; j++)
				{
					array[j + 1] = list[j];
				}
				this.renderer.sharedMaterials = array;
				foreach (ThingPartState thingPartState in this.states)
				{
					for (int k = 0; k < thingPartState.textureProperties.Length; k++)
					{
						if (thingPartState.textureProperties[k] == null)
						{
							thingPartState.textureProperties[k] = new Dictionary<TextureProperty, float>();
							Managers.thingManager.SetTexturePropertiesToDefault(thingPartState.textureProperties[k], this.textureTypes[k]);
						}
					}
				}
				if (this.UseTextureAsSkyAtTheMoment())
				{
					Managers.skyManager.SetThingPart(this);
				}
			}
		}
		if (flag2 && isOfThingBeingEdited)
		{
			for (int l = 0; l < this.textureTypes.Length; l++)
			{
				if (this.textureTypes[l] != TextureType.None)
				{
					this.ApplyTextureColor(l);
					this.ApplyTextureProperties(l);
				}
			}
		}
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x000D706C File Offset: 0x000D546C
	public void SetMaterial(Material sourceMaterial)
	{
		this.MemorizeForUndo();
		this.material.color = sourceMaterial.color;
		this.states[this.currentState].color = this.material.color;
		if (!this.isText || CreationHelper.materialType == MaterialTypes.None || CreationHelper.materialType == MaterialTypes.Glow)
		{
			this.materialType = CreationHelper.materialType;
		}
		this.UpdateMaterial();
		if (base.transform.parent != null)
		{
			Thing component = base.transform.parent.GetComponent<Thing>();
			if (component != null)
			{
				component.SetLightShadows(true, null);
			}
		}
		if (!string.IsNullOrEmpty(this.imageUrl) && this.GetIsOfThingBeingEdited())
		{
			this.ApplyImageTexture();
		}
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x000D7147 File Offset: 0x000D5547
	public void SetMaterialIgnoringColor(MaterialTypes thisMaterialType)
	{
		this.MemorizeForUndo();
		if (!this.isText || thisMaterialType == MaterialTypes.None || thisMaterialType == MaterialTypes.Glow)
		{
			this.materialType = thisMaterialType;
		}
		this.UpdateMaterial();
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x000D7174 File Offset: 0x000D5574
	public void SetColor(Color color)
	{
		this.MemorizeForUndo();
		this.material.color = color;
		this.states[this.currentState].color = this.material.color;
		this.UpdateMaterial();
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x000D71AF File Offset: 0x000D55AF
	public void SetSharedColor(Color color)
	{
		this.renderer.sharedMaterial.color = color;
		this.states[this.currentState].color = this.material.color;
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x000D71E4 File Offset: 0x000D55E4
	private void SwitchToTargetState()
	{
		this.ForgetForUndo();
		ThingPartState thingPartState = this.states[this.currentStateTarget];
		base.transform.localPosition = thingPartState.position;
		base.transform.localEulerAngles = thingPartState.rotation;
		base.transform.localScale = thingPartState.scale;
		if (this.imageUrl == string.Empty || this.thingVersion >= 3)
		{
			this.material.color = thingPartState.color;
		}
		this.states[this.currentState].didTriggerStartEvent = false;
		this.states[this.currentStateTarget].didTriggerStartEvent = false;
		this.tweenType = TweenType.EaseInOut;
		this.soundTracks = null;
		this.currentState = this.currentStateTarget;
		this.currentStateTarget = -1;
		this.currentStateTargetCurveVia = -1;
		this.stateTargetSeconds = -1f;
		this.timeWhenToRevert = -1f;
		if (this.light != null)
		{
			this.light.intensity = Universe.defaultLightIntensity;
		}
		if (this.MyStatesAreBeingEdited())
		{
			GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
			if (currentNonStartDialog != null)
			{
				ThingPartDialog component = currentNonStartDialog.GetComponent<ThingPartDialog>();
				if (component != null)
				{
					component.SwitchToState(this.currentState);
				}
			}
		}
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x000D7338 File Offset: 0x000D5738
	private void UpdateTransformBasedOnTarget()
	{
		if (this.currentStateTarget != -1 && this.currentStateTarget < this.states.Count && this.currentState < this.states.Count)
		{
			float num = this.stateTargetTime - Time.time;
			if (num <= 0f)
			{
				this.SwitchToTargetState();
			}
			else
			{
				float num2 = 1f - num / this.stateTargetSeconds;
				if (this.tweenType == TweenType.Direct)
				{
					num2 = 0f;
				}
				else if (!this.useUnsoftenedAnimations)
				{
					TweenType tweenType = this.tweenType;
					if (tweenType != TweenType.EaseIn)
					{
						if (tweenType != TweenType.EaseOut)
						{
							if (tweenType == TweenType.EaseInOut)
							{
								num2 = Mathfx.EaseInOut(0f, 1f, num2);
							}
						}
						else
						{
							num2 = Mathfx.EaseOut(0f, 1f, num2);
						}
					}
					else
					{
						num2 = Mathfx.EaseIn(0f, 1f, num2);
					}
				}
				ThingPartState thingPartState = this.states[this.currentState];
				ThingPartState thingPartState2 = this.states[this.currentStateTarget];
				this.TransformTowards(thingPartState, thingPartState2, num2);
				this.TransformTowardsTextures(thingPartState, thingPartState2, num2);
				this.TransformTowardsParticles(thingPartState, thingPartState2, num2);
				if (this.currentStateTargetCurveVia != -1 && this.currentStateTargetCurveVia != this.currentState && this.currentStateTargetCurveVia != this.currentStateTarget && this.currentStateTargetCurveVia < this.states.Count)
				{
					ThingPartState baseThingPartStateByTransform = this.GetBaseThingPartStateByTransform();
					float num3 = Mathf.Abs(0.5f - num2);
					float num4 = (0.5f - num3) * 2f;
					if (num3 >= 0f)
					{
						num4 *= 0.5f + num3;
						num4 = Mathfx.EaseOut(0f, 0.5f, num4);
					}
					this.TransformTowards(baseThingPartStateByTransform, this.states[this.currentStateTargetCurveVia], num4);
				}
			}
		}
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x000D7528 File Offset: 0x000D5928
	private ThingPartState GetBaseThingPartStateByTransform()
	{
		ThingPartState thingPartState = new ThingPartState();
		thingPartState.position = base.transform.localPosition;
		thingPartState.rotation = base.transform.localEulerAngles;
		thingPartState.scale = base.transform.localScale;
		if (this.material != null)
		{
			thingPartState.color = this.material.color;
		}
		return thingPartState;
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000D7594 File Offset: 0x000D5994
	private void TransformTowards(ThingPartState start, ThingPartState target, float fractionOfJourney)
	{
		if (base.transform.localPosition != target.position)
		{
			base.transform.localPosition = Vector3.Lerp(start.position, target.position, fractionOfJourney);
		}
		if (base.transform.localEulerAngles != target.rotation)
		{
			Quaternion quaternion = Quaternion.Euler(start.rotation.x, start.rotation.y, start.rotation.z);
			Quaternion quaternion2 = Quaternion.Euler(target.rotation.x, target.rotation.y, target.rotation.z);
			base.transform.localRotation = Quaternion.Lerp(quaternion, quaternion2, fractionOfJourney);
		}
		if (base.transform.localScale != target.scale)
		{
			base.transform.localScale = Vector3.Lerp(start.scale, target.scale, fractionOfJourney);
		}
		if (this.material.color != target.color && (this.imageUrl == string.Empty || this.thingVersion >= 3))
		{
			if (this.IsAnyTransparent(this.materialType))
			{
				target.color = new Color(target.color.r, target.color.g, target.color.b, this.GetAlphaForAnyTransparent(this.materialType));
			}
			this.material.color = Color.Lerp(start.color, target.color, fractionOfJourney);
			if (this.materialType == MaterialTypes.Glow)
			{
				this.material.SetColor("_EmissionColor", this.material.color);
			}
		}
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x000D7758 File Offset: 0x000D5B58
	private void TransformTowardsTextures(ThingPartState start, ThingPartState target, float fractionOfJourney)
	{
		if (this.textureTypes != null)
		{
			this.SetRendererIfNeeded();
			for (int i = 0; i < this.textureTypes.Length; i++)
			{
				if (this.textureTypes[i] != TextureType.None && start.textureProperties != null && start.textureProperties[i] != null)
				{
					Color color = Color.Lerp(start.textureColors[i], target.textureColors[i], fractionOfJourney);
					Dictionary<TextureProperty, float> dictionary = new Dictionary<TextureProperty, float>();
					Dictionary<TextureProperty, float> dictionary2 = Managers.thingManager.CloneTextureProperty(start.textureProperties[i]);
					Dictionary<TextureProperty, float> dictionary3 = Managers.thingManager.CloneTextureProperty(target.textureProperties[i]);
					this.ModulateTheseTextureProperties(dictionary2, this.textureTypes[i]);
					this.ModulateTheseTextureProperties(dictionary3, this.textureTypes[i]);
					IEnumerator enumerator = Enum.GetValues(typeof(TextureProperty)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							TextureProperty textureProperty = (TextureProperty)obj;
							float num = Mathf.Lerp(dictionary2[textureProperty], dictionary3[textureProperty], fractionOfJourney);
							dictionary.Add(textureProperty, num);
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
					this.ApplyTextureColorByThis(color, this.renderer.materials[i + 1]);
					this.ApplyTexturePropertiesToMaterial(dictionary, this.renderer.materials[i + 1]);
				}
			}
		}
	}

	// Token: 0x060017B4 RID: 6068 RVA: 0x000D78DC File Offset: 0x000D5CDC
	private void TransformTowardsParticles(ThingPartState start, ThingPartState target, float fractionOfJourney)
	{
		if (start.particleSystemProperty != null && this.particleSystemChild != null)
		{
			Color color = Color.Lerp(start.particleSystemColor, target.particleSystemColor, fractionOfJourney);
			Dictionary<ParticleSystemProperty, float> dictionary = new Dictionary<ParticleSystemProperty, float>();
			Dictionary<ParticleSystemProperty, float> dictionary2 = Managers.thingManager.CloneParticleSystemProperty(start.particleSystemProperty);
			Dictionary<ParticleSystemProperty, float> dictionary3 = Managers.thingManager.CloneParticleSystemProperty(target.particleSystemProperty);
			this.ModulateParticleSystemProperties(dictionary2);
			this.ModulateParticleSystemProperties(dictionary3);
			IEnumerator enumerator = Enum.GetValues(typeof(ParticleSystemProperty)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					ParticleSystemProperty particleSystemProperty = (ParticleSystemProperty)obj;
					if (start.particleSystemProperty.ContainsKey(particleSystemProperty))
					{
						float num = Mathf.Lerp(dictionary2[particleSystemProperty], dictionary3[particleSystemProperty], fractionOfJourney);
						dictionary.Add(particleSystemProperty, num);
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
			this.ApplyParticleSystemColorByThis(color);
			this.ApplyParticleSystemPropertiesByThis(dictionary);
		}
	}

	// Token: 0x060017B5 RID: 6069 RVA: 0x000D79F4 File Offset: 0x000D5DF4
	private void UpdateLightBasedOnTransform()
	{
		if (this.light != null && (this.lightLastAppliedScale != base.transform.localScale || this.lightLastAppliedColor != this.material.color))
		{
			float shapeToReferenceShapeScaleFactor = Managers.thingManager.GetShapeToReferenceShapeScaleFactor(this.baseType);
			float scaleFactor = this.GetScaleFactor(shapeToReferenceShapeScaleFactor);
			this.light.range = scaleFactor;
			this.light.color = this.material.color;
			this.light.enabled = scaleFactor >= 0.6f;
			this.material.SetColor("_EmissionColor", this.material.color);
			if (this.materialType == MaterialTypes.SpotLight)
			{
				this.light.spotAngle = (base.transform.localScale.x + base.transform.localScale.y) * (100f * shapeToReferenceShapeScaleFactor);
			}
			this.lightLastAppliedScale = base.transform.localScale;
			this.lightLastAppliedColor = this.material.color;
		}
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x000D7B1C File Offset: 0x000D5F1C
	private void UpdateBaseLayerParticlesBasedOnTransform()
	{
		if (this.baseLayerParticles != null && (this.materialType == MaterialTypes.Particles || this.materialType == MaterialTypes.ParticlesBig) && (this.baseLayerParticlesLastAppliedScale != base.transform.localScale || this.baseLayerParticlesLastAppliedColor != this.material.color))
		{
			float scaleFactor = this.GetScaleFactor(0f);
			ParticleSystem.MainModule main = this.baseLayerParticles.main;
			main.startSize = ((this.materialType != MaterialTypes.ParticlesBig) ? 0.02f : 0.2f);
			this.baseLayerParticles.enableEmission = scaleFactor >= 0.6f;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			this.baseLayerParticles.startSpeed = 0.5f * scaleFactor * base.transform.localScale.z;
			this.baseLayerParticles.startSpeed *= this.baseLayerParticles.startSpeed;
			this.baseLayerParticles.startSpeed = Mathf.Clamp(this.baseLayerParticles.startSpeed, 0.01f, 20f);
			this.baseLayerParticles.startColor = this.material.color;
			this.baseLayerParticlesLastAppliedScale = base.transform.localScale;
			this.baseLayerParticlesLastAppliedColor = this.material.color;
		}
	}

	// Token: 0x060017B7 RID: 6071 RVA: 0x000D7C88 File Offset: 0x000D6088
	private float GetScaleFactor(float referenceFactor = 0f)
	{
		Vector3 localScale = base.transform.localScale;
		float num = (localScale.x + localScale.y + localScale.z) * 0.333f;
		if (referenceFactor == 0f)
		{
			referenceFactor = Managers.thingManager.GetShapeToReferenceShapeScaleFactor(this.baseType);
		}
		float num2 = 100f * referenceFactor;
		return Mathf.Clamp(num * num2, 0.25f, 20f);
	}

	// Token: 0x060017B8 RID: 6072 RVA: 0x000D7CF8 File Offset: 0x000D60F8
	public void UpdateMaterial()
	{
		if (this.material == null)
		{
			this.SetRendererIfNeeded();
			this.material = this.renderer.material;
		}
		if (this.isText)
		{
			if (this.currentlyAppliedMaterialType != this.materialType)
			{
				MaterialTypes materialTypes = this.materialType;
				if (materialTypes != MaterialTypes.Glow)
				{
					this.material.shader = Managers.thingManager.shader_textLit;
				}
				else
				{
					this.material.shader = Managers.thingManager.shader_textEmissive;
				}
			}
			this.currentlyAppliedMaterialType = this.materialType;
			return;
		}
		if (this.currentlyAppliedMaterialType != MaterialTypes.Unshaded && this.materialType == MaterialTypes.Unshaded)
		{
			this.material.shader = Managers.thingManager.shader_customUnshaded;
		}
		else if (this.currentlyAppliedMaterialType != MaterialTypes.Inversion && this.materialType == MaterialTypes.Inversion)
		{
			this.material.shader = Managers.thingManager.shader_customInversion;
		}
		else if (this.currentlyAppliedMaterialType != MaterialTypes.Brightness && this.materialType == MaterialTypes.Brightness)
		{
			this.material.shader = Managers.thingManager.shader_customBrightness;
		}
		else if (this.currentlyAppliedMaterialType != MaterialTypes.TransparentGlowTexture && this.materialType == MaterialTypes.TransparentGlowTexture)
		{
			this.material.shader = Managers.thingManager.shader_customTransparentGlow;
		}
		else if ((this.currentlyAppliedMaterialType == MaterialTypes.Unshaded && this.materialType != MaterialTypes.Unshaded) || (this.currentlyAppliedMaterialType == MaterialTypes.Inversion && this.materialType != MaterialTypes.Inversion) || (this.currentlyAppliedMaterialType == MaterialTypes.Brightness && this.materialType != MaterialTypes.Brightness))
		{
			this.material.shader = Managers.thingManager.shader_standard;
		}
		if (this.IsAnyTransparent(this.currentlyAppliedMaterialType) && !this.IsAnyTransparent(this.materialType))
		{
			this.material.SetInt("_SrcBlend", 1);
			this.material.SetInt("_DstBlend", 0);
			this.material.SetInt("_ZWrite", 1);
			this.material.DisableKeyword("_ALPHATEST_ON");
			this.material.DisableKeyword("_ALPHABLEND_ON");
			this.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			Color color = this.material.color;
			this.material.color = new Color(color.r, color.g, color.b, 1f);
			this.material.renderQueue = -1;
		}
		if ((this.currentlyAppliedMaterialType == MaterialTypes.Metallic && this.materialType != MaterialTypes.Metallic) || (this.currentlyAppliedMaterialType == MaterialTypes.VeryMetallic && this.materialType != MaterialTypes.VeryMetallic) || (this.currentlyAppliedMaterialType == MaterialTypes.DarkMetallic && this.materialType != MaterialTypes.DarkMetallic) || (this.currentlyAppliedMaterialType == MaterialTypes.BrightMetallic && this.materialType != MaterialTypes.BrightMetallic) || (this.currentlyAppliedMaterialType == MaterialTypes.Plastic && this.materialType != MaterialTypes.Plastic) || (this.currentlyAppliedMaterialType == MaterialTypes.Unshiny && this.materialType != MaterialTypes.Unshiny) || (this.currentlyAppliedMaterialType == MaterialTypes.TransparentGlossy && this.materialType != MaterialTypes.TransparentGlossy) || (this.currentlyAppliedMaterialType == MaterialTypes.TransparentGlossyMetallic && this.materialType != MaterialTypes.TransparentGlossyMetallic) || (this.currentlyAppliedMaterialType == MaterialTypes.VeryTransparentGlossy && this.materialType != MaterialTypes.VeryTransparentGlossy))
		{
			this.material.SetFloat("_Metallic", 0f);
			this.material.SetFloat("_Glossiness", 0.25f);
		}
		if (this.light != null && this.materialType != MaterialTypes.PointLight && this.materialType != MaterialTypes.SpotLight)
		{
			this.material.SetColor("_EmissionColor", Color.black);
			global::UnityEngine.Object.Destroy(this.light);
			this.light = null;
		}
		if (this.currentlyAppliedMaterialType == MaterialTypes.Glow && this.materialType != MaterialTypes.Glow)
		{
			this.material.SetColor("_EmissionColor", Color.black);
		}
		if (this.baseLayerParticles != null && this.materialType != MaterialTypes.Particles && this.materialType != MaterialTypes.ParticlesBig)
		{
			global::UnityEngine.Object.Destroy(this.baseLayerParticles);
			this.baseLayerParticles = null;
		}
		if (this.currentlyAppliedMaterialType == MaterialTypes.TransparentTexture && this.materialType != MaterialTypes.TransparentTexture)
		{
			this.material.SetOverrideTag("RenderType", "Opaque");
		}
		switch (this.materialType)
		{
		case MaterialTypes.Metallic:
			this.material.SetFloat("_Metallic", 0.75f);
			this.material.SetFloat("_Glossiness", 0.65f);
			break;
		case MaterialTypes.Glow:
			this.material.SetColor("_EmissionColor", this.material.color);
			break;
		case MaterialTypes.PointLight:
		case MaterialTypes.SpotLight:
			if (this.light == null)
			{
				Light component = base.gameObject.GetComponent<Light>();
				if (component != null)
				{
					this.light = component;
				}
				else
				{
					this.light = base.gameObject.AddComponent<Light>();
				}
			}
			this.light.intensity = Universe.defaultLightIntensity;
			this.light.type = ((this.materialType != MaterialTypes.PointLight) ? LightType.Spot : LightType.Point);
			this.light.shadows = LightShadows.None;
			this.material.SetColor("_EmissionColor", this.material.color);
			this.lightLastAppliedScale = Vector3.zero;
			this.UpdateLightBasedOnTransform();
			break;
		case MaterialTypes.Particles:
		case MaterialTypes.ParticlesBig:
			if (!this.isInInventoryOrDialog || this.activeEvenInInventory)
			{
				if (this.baseLayerParticles == null)
				{
					this.baseLayerParticles = base.gameObject.AddComponent<ParticleSystem>();
					if (this.baseLayerParticles == null)
					{
						this.baseLayerParticles = base.gameObject.GetComponent<ParticleSystem>();
					}
					GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("ParticleSystems/Environment/Snow")) as GameObject;
					Renderer component2 = this.baseLayerParticles.gameObject.GetComponent<ParticleSystemRenderer>();
					component2.material = gameObject.GetComponent<Renderer>().material;
					global::UnityEngine.Object.Destroy(gameObject);
					ParticleSystem.ShapeModule shape = this.baseLayerParticles.shape;
					shape.shapeType = ParticleSystemShapeType.Box;
					bool flag = this.baseType == ThingPartBase.Sphere || this.baseType == ThingPartBase.Icosphere || this.baseType == ThingPartBase.LowPolySphere || this.baseType == ThingPartBase.JitterSphere;
					bool flag2 = this.baseType == ThingPartBase.Pyramid || this.baseType == ThingPartBase.Cone || this.baseType == ThingPartBase.Trapeze;
					bool flag3 = this.baseType == ThingPartBase.Cylinder;
					if (flag)
					{
						shape.randomDirection = true;
					}
					else if (flag3)
					{
						shape.angle = 90f;
						shape.shapeType = ParticleSystemShapeType.Cone;
					}
					else if (flag2)
					{
						shape.shapeType = ParticleSystemShapeType.Cone;
					}
					ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = this.baseLayerParticles.sizeOverLifetime;
					sizeOverLifetime.enabled = true;
					AnimationCurve animationCurve = new AnimationCurve();
					animationCurve.AddKey(0.1f, 1f);
					animationCurve.AddKey(1f, 0.1f);
					sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, animationCurve);
					this.baseLayerParticles.maxParticles = 100;
					this.baseLayerParticles.scalingMode = ParticleSystemScalingMode.Shape;
				}
				this.baseLayerParticlesLastAppliedScale = Vector3.zero;
				this.UpdateBaseLayerParticlesBasedOnTransform();
			}
			break;
		case MaterialTypes.Transparent:
		case MaterialTypes.TransparentGlossy:
		case MaterialTypes.TransparentGlossyMetallic:
		case MaterialTypes.VeryTransparent:
		case MaterialTypes.VeryTransparentGlossy:
		case MaterialTypes.SlightlyTransparent:
		{
			this.material.SetInt("_SrcBlend", 1);
			this.material.SetInt("_DstBlend", 10);
			this.material.SetInt("_ZWrite", 0);
			this.material.DisableKeyword("_ALPHATEST_ON");
			this.material.DisableKeyword("_ALPHABLEND_ON");
			this.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			Color color2 = this.material.color;
			this.material.color = new Color(color2.r, color2.g, color2.b, this.GetAlphaForAnyTransparent(this.materialType));
			MaterialTypes materialTypes2 = this.materialType;
			if (materialTypes2 != MaterialTypes.TransparentGlossy && materialTypes2 != MaterialTypes.VeryTransparentGlossy)
			{
				if (materialTypes2 == MaterialTypes.TransparentGlossyMetallic)
				{
					this.material.SetFloat("_Metallic", 0.5f);
					this.material.SetFloat("_Glossiness", 0.75f);
				}
			}
			else
			{
				this.material.SetFloat("_Metallic", 0f);
				this.material.SetFloat("_Glossiness", 0.75f);
			}
			this.material.renderQueue = 3000;
			break;
		}
		case MaterialTypes.Plastic:
			this.material.SetFloat("_Metallic", 0f);
			this.material.SetFloat("_Glossiness", 0.8f);
			break;
		case MaterialTypes.Unshiny:
			this.material.SetFloat("_Metallic", 0f);
			this.material.SetFloat("_Glossiness", 0f);
			break;
		case MaterialTypes.VeryMetallic:
			this.material.SetFloat("_Metallic", 0.95f);
			this.material.SetFloat("_Glossiness", 0.5f);
			break;
		case MaterialTypes.DarkMetallic:
			this.material.SetFloat("_Metallic", 1f);
			this.material.SetFloat("_Glossiness", 0.65f);
			break;
		case MaterialTypes.BrightMetallic:
			this.material.SetFloat("_Metallic", 0.85f);
			this.material.SetFloat("_Glossiness", 0.2f);
			break;
		case MaterialTypes.TransparentTexture:
			this.material.SetOverrideTag("RenderType", "Fade");
			this.material.SetFloat("_SrcBlend", 5f);
			this.material.SetFloat("_DstBlend", 10f);
			this.material.DisableKeyword("_ALPHATEST_ON");
			this.material.EnableKeyword("_ALPHABLEND_ON");
			this.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			this.material.SetFloat("_Metallic", 0f);
			this.material.SetFloat("_Glossiness", 0.25f);
			this.material.renderQueue = 3000;
			break;
		}
		this.currentlyAppliedMaterialType = this.materialType;
		if (this.UseTextureAsSkyAtTheMoment())
		{
			Managers.skyManager.SetThingPart(this);
		}
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x000D87D0 File Offset: 0x000D6BD0
	public void UpdateParticleSystem()
	{
		if (this.particleSystemType != ParticleSystemType.None)
		{
			if (this.particleSystemChild == null || this.currentlyAppliedParticleSystemType != this.particleSystemType)
			{
				this.AddParticleSystemChild();
			}
			if (this.particleSystemChild != null)
			{
				this.ApplyParticleSystemColor();
				this.ApplyParticleSystemProperties();
			}
		}
		else if (this.particleSystemChild != null)
		{
			global::UnityEngine.Object.Destroy(this.particleSystemChild);
			this.particleSystemChild = null;
			foreach (ThingPartState thingPartState in this.states)
			{
				thingPartState.particleSystemProperty = null;
			}
			this.particleSystemComponents = null;
			this.currentlyAppliedParticleSystemType = ParticleSystemType.None;
		}
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x000D88B4 File Offset: 0x000D6CB4
	public void RemoveParticleSystems()
	{
		this.particleSystemComponents = null;
		global::UnityEngine.Object.Destroy(this.particleSystemChild);
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x000D88C8 File Offset: 0x000D6CC8
	private void AddParticleSystemChild()
	{
		this.particleSystemComponents = null;
		global::UnityEngine.Object.Destroy(this.particleSystemChild);
		string text = "ParticleSystems/" + this.particleSystemType.ToString();
		try
		{
			this.particleSystemChild = global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load(text) as GameObject);
		}
		catch (Exception ex)
		{
			Log.Debug("Did not find particle prefab " + this.materialType.ToString());
			return;
		}
		this.particleSystemChild.name = Misc.RemoveCloneFromName(this.particleSystemChild.name);
		this.particleSystemChild.transform.parent = base.transform;
		this.particleSystemChild.transform.localPosition = Vector3.zero;
		this.particleSystemChild.transform.localRotation = Quaternion.identity;
		this.particleSystemChild.transform.localScale = Vector3.one;
		ParticleSystem component = this.particleSystemChild.GetComponent<ParticleSystem>();
		component.maxParticles = 500;
		if (this.looselyCoupledParticles)
		{
			component.simulationSpace = ParticleSystemSimulationSpace.World;
		}
		if (this.particleSystemType != ParticleSystemType.WaterFlow && this.particleSystemType != ParticleSystemType.WaterFlowSoft)
		{
			ParticleSystem.ShapeModule shape = component.shape;
			shape.shapeType = ParticleSystemShapeType.MeshRenderer;
			shape.meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		}
		this.particleSystemComponents = new List<ParticleSystem>();
		this.particleSystemComponents.Add(component);
		Component[] componentsInChildren = this.particleSystemChild.GetComponentsInChildren(typeof(ParticleSystem), true);
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			if (particleSystem != component)
			{
				this.particleSystemComponents.Add(particleSystem);
				if (this.looselyCoupledParticles)
				{
					particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
				}
			}
		}
		this.currentlyAppliedParticleSystemType = this.particleSystemType;
		if (this.states[0].particleSystemProperty == null)
		{
			foreach (ThingPartState thingPartState in this.states)
			{
				thingPartState.particleSystemProperty = new Dictionary<ParticleSystemProperty, float>();
				Managers.thingManager.SetParticleSystemPropertiesToDefault(thingPartState.particleSystemProperty, this.particleSystemType);
			}
		}
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x000D8B3C File Offset: 0x000D6F3C
	public void ApplyParticleSystemProperties()
	{
		Dictionary<ParticleSystemProperty, float> dictionary = Managers.thingManager.CloneParticleSystemProperty(this.states[this.currentState].particleSystemProperty);
		this.ModulateParticleSystemProperties(dictionary);
		this.ApplyParticleSystemPropertiesByThis(dictionary);
		if (dictionary.ContainsKey(ParticleSystemProperty.Shape))
		{
			this.ModulateApplyParticleSystemProperty_Shape(dictionary[ParticleSystemProperty.Shape]);
		}
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x000D8B91 File Offset: 0x000D6F91
	public void ApplyParticleSystemColor()
	{
		this.ApplyParticleSystemColorByThis(this.states[this.currentState].particleSystemColor);
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x000D8BAF File Offset: 0x000D6FAF
	private void ModulateParticleSystemProperties(Dictionary<ParticleSystemProperty, float> particleSystemProperty)
	{
		if (!Managers.thingManager.IsParticleSystemTypeWithOnlyAlphaSetting(this.particleSystemType))
		{
			this.ModulateParticleSystemProperty_Amount(particleSystemProperty);
			this.ModulateParticleSystemProperty_Speed(particleSystemProperty);
			this.ModulateParticleSystemProperty_Size(particleSystemProperty);
			this.ModulateParticleSystemProperty_Gravity(particleSystemProperty);
		}
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x000D8BE4 File Offset: 0x000D6FE4
	private void ApplyParticleSystemPropertiesByThis(Dictionary<ParticleSystemProperty, float> particleSystemProperty)
	{
		bool flag = Managers.thingManager.IsParticleSystemTypeWithOnlyAlphaSetting(this.particleSystemType);
		foreach (ParticleSystem particleSystem in this.particleSystemComponents)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			Color startColor = particleSystem.startColor;
			startColor.a = particleSystemProperty[ParticleSystemProperty.Alpha];
			particleSystem.startColor = startColor;
			if (!flag)
			{
				if (main.startSize.GetType() == typeof(ParticleSystem.MinMaxCurve))
				{
					float constantMax = main.startSize.constantMax;
					float constantMin = main.startSize.constantMin;
					if (constantMin > 0f)
					{
						float num = constantMin / constantMax;
						main.startSize = new ParticleSystem.MinMaxCurve(particleSystemProperty[ParticleSystemProperty.Size] * num, particleSystemProperty[ParticleSystemProperty.Size]);
					}
					else
					{
						main.startSize = particleSystemProperty[ParticleSystemProperty.Size];
					}
				}
				else
				{
					main.startSize = particleSystemProperty[ParticleSystemProperty.Size];
				}
				main.gravityModifier = particleSystemProperty[ParticleSystemProperty.Gravity];
			}
		}
		if (!flag)
		{
			this.particleSystemComponents[0].main.startSpeed = particleSystemProperty[ParticleSystemProperty.Speed];
			this.particleSystemComponents[0].emission.rate = particleSystemProperty[ParticleSystemProperty.Amount];
		}
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x000D8D90 File Offset: 0x000D7190
	private void ApplyParticleSystemColorByThis(Color color)
	{
		foreach (ParticleSystem particleSystem in this.particleSystemComponents)
		{
			particleSystem.startColor = this.SetColorExceptAlpha(particleSystem.startColor, color);
		}
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x000D8DF8 File Offset: 0x000D71F8
	private void ModulateParticleSystemProperty_Amount(Dictionary<ParticleSystemProperty, float> particleSystemProperty)
	{
		float num = particleSystemProperty[ParticleSystemProperty.Amount];
		num *= 100f;
		num = Mathf.Pow(num, 2.25f);
		if (num > 0f)
		{
			num /= 100f;
		}
		particleSystemProperty[ParticleSystemProperty.Amount] = num;
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x000D8E3C File Offset: 0x000D723C
	private void ModulateParticleSystemProperty_Speed(Dictionary<ParticleSystemProperty, float> particleSystemProperty)
	{
		float num = particleSystemProperty[ParticleSystemProperty.Speed];
		num *= 100f;
		float num2 = ((num < 75f) ? 1.65f : 1.8f);
		num = Mathf.Pow(num, num2);
		if (num > 0f)
		{
			num /= 100f;
		}
		particleSystemProperty[ParticleSystemProperty.Speed] = num;
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x000D8E98 File Offset: 0x000D7298
	private void ModulateParticleSystemProperty_Size(Dictionary<ParticleSystemProperty, float> particleSystemProperty)
	{
		float num = particleSystemProperty[ParticleSystemProperty.Size];
		if (num > 0.2f)
		{
			num -= 0.2f;
			num *= 100f;
			float num2 = 1.5f;
			num = Mathf.Pow(num, num2);
			if (num > 0f)
			{
				num /= 100f;
			}
			num += 0.2f;
		}
		num = Mathf.Max(num, 0.01f);
		particleSystemProperty[ParticleSystemProperty.Size] = num;
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x000D8F08 File Offset: 0x000D7308
	private void ModulateParticleSystemProperty_Gravity(Dictionary<ParticleSystemProperty, float> particleSystemProperty)
	{
		float num = particleSystemProperty[ParticleSystemProperty.Gravity];
		if (num <= 0.4f)
		{
			num = Mathf.Abs(num - 0.4f) * 2.5f;
		}
		else if (num >= 0.6f)
		{
			num = -(Mathf.Abs(num - 0.6f) * 2.5f);
		}
		else
		{
			num = 0f;
		}
		num = Mathf.Clamp(num, -1f, 1f);
		particleSystemProperty[ParticleSystemProperty.Gravity] = num;
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x000D8F84 File Offset: 0x000D7384
	private void ModulateApplyParticleSystemProperty_Shape(float value)
	{
		ParticleSystem.ShapeModule shape = this.particleSystemComponents[0].shape;
		if (value <= 0.1f)
		{
			if (shape.shapeType != ParticleSystemShapeType.MeshRenderer)
			{
				shape.shapeType = ParticleSystemShapeType.MeshRenderer;
				shape.meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
			}
			shape.meshShapeType = ParticleSystemMeshShapeType.Vertex;
			shape.scale = Vector3.one;
		}
		else if (value < 0.5f)
		{
			if (shape.shapeType != ParticleSystemShapeType.MeshRenderer)
			{
				shape.shapeType = ParticleSystemShapeType.MeshRenderer;
				shape.meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
			}
			shape.meshShapeType = ParticleSystemMeshShapeType.Triangle;
			shape.scale = Vector3.one;
		}
		else
		{
			shape.shapeType = ParticleSystemShapeType.Box;
			shape.meshRenderer = null;
			float num = Mathf.Pow(value + 0.5f, 15f);
			shape.scale = base.transform.localScale * num;
		}
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x000D9077 File Offset: 0x000D7477
	private void SetRendererIfNeeded()
	{
		if (this.renderer == null)
		{
			this.renderer = base.gameObject.GetComponent<Renderer>();
		}
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x000D909B File Offset: 0x000D749B
	public void ApplyTextureColor(int index)
	{
		this.SetRendererIfNeeded();
		this.ApplyTextureColorByThis(this.states[this.currentState].textureColors[index], this.renderer.materials[index + 1]);
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x000D90D9 File Offset: 0x000D74D9
	public void ApplyTextureColorByThis(Color color, Material thisMaterial)
	{
		this.SetRendererIfNeeded();
		thisMaterial.SetColor("_ShaderColor", color);
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x000D90F0 File Offset: 0x000D74F0
	public void ApplyTextureProperties(int index)
	{
		Dictionary<TextureProperty, float> dictionary = Managers.thingManager.CloneTextureProperty(this.states[this.currentState].textureProperties[index]);
		this.ModulateTheseTextureProperties(dictionary, this.textureTypes[index]);
		this.ApplyTexturePropertiesToMaterial(dictionary, this.renderer.materials[index + 1]);
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x000D9148 File Offset: 0x000D7548
	public void ApplyTexturePropertiesToMaterial(Dictionary<TextureProperty, float> textureProperty, Material thisMaterial)
	{
		if (this.useTextureAsSky)
		{
			thisMaterial.SetFloat("_ShaderScaleX", textureProperty[TextureProperty.ScaleX] * 0.1f);
			thisMaterial.SetFloat("_ShaderScaleY", textureProperty[TextureProperty.ScaleY] * 0.1f);
		}
		else
		{
			thisMaterial.SetFloat("_ShaderScaleX", textureProperty[TextureProperty.ScaleX]);
			thisMaterial.SetFloat("_ShaderScaleY", textureProperty[TextureProperty.ScaleY]);
		}
		thisMaterial.SetFloat("_ShaderStrength", textureProperty[TextureProperty.Strength]);
		thisMaterial.SetFloat("_ShaderRotation", textureProperty[TextureProperty.Rotation]);
		thisMaterial.SetFloat("_ShaderOffsetX", textureProperty[TextureProperty.OffsetX]);
		thisMaterial.SetFloat("_ShaderOffsetY", textureProperty[TextureProperty.OffsetY]);
		thisMaterial.SetFloat("_ShaderGlow", textureProperty[TextureProperty.Glow]);
		thisMaterial.SetFloat("_ShaderParam1", textureProperty[TextureProperty.Param1]);
		thisMaterial.SetFloat("_ShaderParam2", textureProperty[TextureProperty.Param2]);
		thisMaterial.SetFloat("_ShaderParam3", textureProperty[TextureProperty.Param3]);
		thisMaterial.SetFloat("_Glossiness", 0.25f);
		thisMaterial.SetFloat("_MaximumEffectDistance", 1000f);
		thisMaterial.SetFloat("_AngleEffectStrength", 0f);
		thisMaterial.SetFloat("_AngleEffectMinAlpha", 0f);
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x000D928C File Offset: 0x000D768C
	public void ModulateTheseTextureProperties(Dictionary<TextureProperty, float> textureProperty, TextureType textureType)
	{
		bool flag = Managers.thingManager.IsAlgorithmTextureType(textureType);
		this.ModulateTextureProperty_Strength(textureProperty, textureType, flag);
		if (!Managers.thingManager.IsTextureTypeWithOnlyAlphaSetting(textureType))
		{
			this.ModulateTextureProperty_Scale(textureProperty, "ScaleX", TextureProperty.ScaleX, flag);
			this.ModulateTextureProperty_Scale(textureProperty, "ScaleY", TextureProperty.ScaleY, flag);
			this.ModulateTextureProperty_Offset(textureProperty, "OffsetX", TextureProperty.OffsetX, flag);
			this.ModulateTextureProperty_Offset(textureProperty, "OffsetY", TextureProperty.OffsetY, flag);
			this.ModulateTextureProperty_Rotation(textureProperty);
			Managers.thingManager.AdditionallyModulateTexturePropertiesByType(textureProperty, textureType);
		}
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x000D930C File Offset: 0x000D770C
	private void ModulateTextureProperty_Strength(Dictionary<TextureProperty, float> textureProperty, TextureType textureType, bool isAlgorithmTexture)
	{
		if (textureType != TextureType.SideGlow)
		{
			float num = textureProperty[TextureProperty.Strength];
			num = (num - 0.5f) * 2f;
			if (Mathf.Abs(num) <= 0.1f)
			{
				num = 0f;
			}
			else if (num >= 0f)
			{
				float num2 = (num - 0.1f) * 1.1111112f;
				num = Mathf.Lerp(0f, 1f, num2);
			}
			else
			{
				float num3 = Mathf.Abs((num + 0.1f) * 1.1111112f);
				num = -Mathf.Lerp(0f, 1f, num3);
			}
			textureProperty[TextureProperty.Strength] = num;
		}
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x000D93B0 File Offset: 0x000D77B0
	private void ModulateTextureProperty_Scale(Dictionary<TextureProperty, float> textureProperty, string keyString, TextureProperty key, bool isAlgorithmTexture)
	{
		float num = textureProperty[key];
		if (num > 0.5f)
		{
			num -= 0.5f;
			float num2 = ((!isAlgorithmTexture) ? 3f : 2f);
			float num3 = Mathf.Lerp(1f, num2, num * 2f);
			num *= 100f;
			num = Mathf.Pow(num, num3);
			if (num > 0f)
			{
				num /= 100f;
			}
			num += 0.5f;
		}
		if (num < 0.0002f)
		{
			num = 0.0002f;
		}
		textureProperty[key] = num;
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x000D9448 File Offset: 0x000D7848
	private void ModulateTextureProperty_Offset(Dictionary<TextureProperty, float> textureProperty, string keyString, TextureProperty key, bool isAlgorithmTexture)
	{
		float num = textureProperty[key];
		if (isAlgorithmTexture)
		{
			if (num <= 0.4f)
			{
				num = -Mathf.Abs(num - 0.4f) * 10f;
			}
			else if (num >= 0.6f)
			{
				num = Mathf.Abs(num - 0.6f) * 10f;
			}
			else
			{
				num = 0f;
			}
		}
		else
		{
			num *= 5f;
		}
		textureProperty[key] = num;
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x000D94C8 File Offset: 0x000D78C8
	private void ModulateTextureProperty_Rotation(Dictionary<TextureProperty, float> textureProperty)
	{
		float num = textureProperty[TextureProperty.Rotation];
		num *= 360f;
		textureProperty[TextureProperty.Rotation] = num;
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x000D94ED File Offset: 0x000D78ED
	private Color SetColorExceptAlpha(Color originalColor, Color newColor)
	{
		newColor.a = originalColor.a;
		return newColor;
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x000D9500 File Offset: 0x000D7900
	private void OnDestroy()
	{
		if (this.browser != null)
		{
			global::UnityEngine.Object.Destroy(this.browser.gameObject);
		}
		if (Managers.soundLibraryManager != null && this.loopSound != null)
		{
			Managers.soundLibraryManager.loopSoundsStartedCount--;
		}
		if (this.useTextureAsSky)
		{
			Managers.skyManager.RemoveThingPartIfCurrent(this);
		}
		this.RemoveMyReflectionPartsIfNeeded();
		this.RemoveMyAssignedContinuationParts();
		global::UnityEngine.Object.Destroy(this.assignedIncludedSubThings);
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x000D9590 File Offset: 0x000D7990
	public Thing GetParentThing()
	{
		Thing thing = null;
		if (base.transform.parent != null)
		{
			thing = base.transform.parent.GetComponent<Thing>();
		}
		return thing;
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x000D95C7 File Offset: 0x000D79C7
	public bool HasControllableSettings()
	{
		return this.isControllableCollider || this.controllableBodySlidiness != 0f || this.controllableBodyBounciness != 0f || this.joystickToControllablePart != null;
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x000D9604 File Offset: 0x000D7A04
	public void AddTypedText(string text, bool alsoAddIfNotText = false)
	{
		if (this.isText)
		{
			if (this.hasTextPlaceholders)
			{
				this.typedText = Misc.WrapWithNewlines(text, 30, -1);
				this.timeWhenToClearTypedText = Time.time + 15f;
			}
		}
		else if (this.isImagePasteScreen)
		{
			string imgurImageUrl = Misc.GetImgurImageUrl(text);
			if (imgurImageUrl != null)
			{
				base.CancelInvoke("ClearImagePaste");
				this.localScaleBeforeImagePaste = new Vector3?(base.transform.localScale);
				this.imageUrl = imgurImageUrl;
				this.StartLoadImage();
				base.Invoke("ClearImagePaste", 15f);
			}
		}
		else if (alsoAddIfNotText)
		{
			this.typedText = text;
			this.timeWhenToClearTypedText = Time.time + 15f;
		}
		this.TriggerEvent(StateListener.EventType.OnTyped, string.Empty, false, null);
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x000D96D8 File Offset: 0x000D7AD8
	private void ClearImagePaste()
	{
		this.SetRendererIfNeeded();
		this.renderer.material.mainTexture = null;
		this.renderer.material.color = Color.black;
		Transform transform = base.transform;
		Vector3? vector = this.localScaleBeforeImagePaste;
		transform.localScale = vector.Value;
		this.localScaleBeforeImagePaste = null;
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x000D9739 File Offset: 0x000D7B39
	public bool HasVertexTexture()
	{
		return Managers.thingManager.IsVertexTexture(this.textureTypes[0]) || Managers.thingManager.IsVertexTexture(this.textureTypes[1]);
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x000D9768 File Offset: 0x000D7B68
	public void RemoveImageTexture()
	{
		this.imageUrl = string.Empty;
		this.imageTexture = null;
		this.imageWidth = 0;
		this.imageHeight = 0;
		this.imageType = ImageType.Jpeg;
		this.SetRendererIfNeeded();
		this.renderer.material.mainTexture = null;
		this.UpdateMaterial();
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x000D97B9 File Offset: 0x000D7BB9
	public bool HasIncludedSubThings()
	{
		return this.includedSubThings != null;
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x000D97C7 File Offset: 0x000D7BC7
	public bool HasPlacedSubThings()
	{
		return this.assignedPlacedSubThings != null;
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x000D97D8 File Offset: 0x000D7BD8
	public void AddThingAsIncludedSubThing(Thing thingToAdd)
	{
		if (this.includedSubThings == null)
		{
			this.includedSubThings = new List<IncludedSubThing>();
		}
		if (this.includedSubThings.Count >= 1000)
		{
			Managers.soundManager.Play("no", base.transform, 0.2f, false, false);
			return;
		}
		GameObject gameObject = new GameObject();
		gameObject.name = "TempWrapper";
		gameObject.transform.parent = base.transform.parent;
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = base.transform.rotation;
		gameObject.transform.localScale = Vector3.one;
		thingToAdd.transform.parent = gameObject.transform;
		IncludedSubThing includedSubThing = new IncludedSubThing();
		includedSubThing.thingId = thingToAdd.thingId;
		includedSubThing.originalRelativePosition = thingToAdd.transform.localPosition;
		includedSubThing.originalRelativeRotation = thingToAdd.transform.localEulerAngles;
		if (!string.IsNullOrEmpty(thingToAdd.includedSubThingNameOverride))
		{
			includedSubThing.nameOverride = thingToAdd.includedSubThingNameOverride;
		}
		this.includedSubThings.Add(includedSubThing);
		thingToAdd.transform.parent = null;
		global::UnityEngine.Object.Destroy(gameObject);
		this.RemoveMyCreatedIncludedSubThings();
		this.CreateMyIncludedSubThingsIfNeeded();
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x000D991C File Offset: 0x000D7D1C
	public void RemoveThingAsIncludedSubThing(Thing thingToRemove, bool alsoDestroyObject = false)
	{
		if (this.includedSubThings != null)
		{
			for (int i = 0; i < this.includedSubThings.Count; i++)
			{
				if (this.includedSubThings[i].assignedObject == thingToRemove.gameObject)
				{
					this.includedSubThings.RemoveAt(i);
					break;
				}
			}
			if (this.includedSubThings.Count == 0)
			{
				this.includedSubThings = null;
				global::UnityEngine.Object.Destroy(this.assignedIncludedSubThings);
			}
			if (alsoDestroyObject)
			{
				global::UnityEngine.Object.Destroy(thingToRemove.gameObject);
			}
		}
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x000D99B5 File Offset: 0x000D7DB5
	public void RemoveMyCreatedIncludedSubThings()
	{
		if (this.assignedIncludedSubThings != null)
		{
			global::UnityEngine.Object.Destroy(this.assignedIncludedSubThings);
			this.assignedIncludedSubThings = null;
		}
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x000D99DC File Offset: 0x000D7DDC
	private void CreateMyIncludedSubThingsIfNeeded()
	{
		if (this.includedSubThings != null && this.assignedIncludedSubThings == null)
		{
			this.assignedIncludedSubThings = new GameObject();
			this.assignedIncludedSubThings.transform.name = "IncludedSubThings of " + base.transform.name;
			this.assignedIncludedSubThings.transform.tag = "IncludedSubThings";
			this.assignedIncludedSubThings.transform.parent = base.transform.parent;
			this.assignedIncludedSubThings.transform.localPosition = base.transform.localPosition;
			this.assignedIncludedSubThings.transform.localRotation = base.transform.localRotation;
			this.assignedIncludedSubThings.transform.localScale = Vector3.one;
			IncludedSubThingsWrapper includedSubThingsWrapper = this.assignedIncludedSubThings.AddComponent<IncludedSubThingsWrapper>();
			includedSubThingsWrapper.masterThingPart = this;
			string text = string.Empty;
			Thing parentThing = this.GetParentThing();
			if (parentThing != null && parentThing.crossClientSubThingId != null)
			{
				text = parentThing.crossClientSubThingId;
				if (text != string.Empty)
				{
					text += "_";
				}
			}
			if (text == string.Empty)
			{
				text = this.indexWithinThing + 1 + "_";
			}
			int num = 1;
			foreach (IncludedSubThing includedSubThing in this.includedSubThings)
			{
				string text2 = text + num.ToString();
				base.StartCoroutine(this.CreateMyIncludedSubThing(includedSubThing, text2, base.gameObject.layer, parentThing));
				num++;
			}
		}
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x000D9BB4 File Offset: 0x000D7FB4
	private IEnumerator CreateMyIncludedSubThing(IncludedSubThing subThing, string crossClientSubThingId, int layer = -1, Thing inheritSuppressAttributesThing = null)
	{
		ThingManager thingManager = Managers.thingManager;
		ThingRequestContext thingRequestContext = ThingRequestContext.IncludedSubThing;
		string thingId = subThing.thingId;
		Action<GameObject> action = delegate(GameObject thingObject)
		{
			string text = thingObject.name;
			if (subThing.nameOverride != null)
			{
				text = subThing.nameOverride;
			}
			thingObject.name = text;
			thingObject.transform.parent = this.assignedIncludedSubThings.transform;
			thingObject.transform.localPosition = subThing.originalRelativePosition;
			thingObject.transform.localEulerAngles = subThing.originalRelativeRotation;
			thingObject.transform.localScale = Vector3.one;
			if (layer > 0)
			{
				thingObject.layer = layer;
			}
			subThing.assignedObject = thingObject;
			Thing component = thingObject.GetComponent<Thing>();
			if (component != null)
			{
				component.crossClientSubThingId = crossClientSubThingId;
				component.givenName = text;
				if (subThing.nameOverride != null)
				{
					component.includedSubThingNameOverride = subThing.nameOverride;
				}
				component.subThingMasterPart = this.$this;
				component.doSnapAngles = false;
				component.doSoftSnapAngles = false;
				component.doSnapPosition = false;
				if (this.containsTurnCommands)
				{
					component.containsInvisibleOrUncollidable = true;
				}
				subThing.original_isHoldable = component.isHoldable;
				subThing.original_invisible = component.invisible;
				subThing.original_uncollidable = component.uncollidable;
				if (subThing.invert_isHoldable)
				{
					component.isHoldable = !component.isHoldable;
				}
				if (subThing.invert_invisible)
				{
					component.invisible = !component.invisible;
				}
				if (subThing.invert_uncollidable)
				{
					component.uncollidable = !component.uncollidable;
				}
				bool flag = Managers.areaManager != null && Managers.areaManager.weAreEditorOfCurrentArea;
				component.UpdateAllVisibilityAndCollision(flag && Our.seeInvisibleAsEditor, flag && Our.touchUncollidableAsEditor);
				if (this.isInInventoryOrDialog)
				{
					Collider component2 = this.gameObject.GetComponent<Collider>();
					if (component2 != null && !component2.enabled)
					{
						Component[] componentsInChildren = component.GetComponentsInChildren(typeof(Collider), true);
						foreach (Collider collider in componentsInChildren)
						{
							collider.enabled = false;
						}
					}
					component.isInInventoryOrDialog = this.isInInventoryOrDialog;
					component.isInInventory = this.isInInventory;
					Thing myRootThing = this.GetMyRootThing();
					if (myRootThing != null)
					{
						float num = ((!myRootThing.isInInventory) ? 0.035f : 0.75f);
						if (myRootThing.isGiftInDialog)
						{
							num = 0.17f;
						}
						bool flag2 = myRootThing.CompareTag("DialogThingThumb") && !myRootThing.isInInventory;
						bool flag3 = myRootThing.isInInventory && (myRootThing.keepSizeInInventory || component.keepSizeInInventory);
						if (!flag2 && !flag3)
						{
							myRootThing.transform.localScale = Managers.thingManager.GetAppropriateDownScaleForThing(myRootThing.gameObject, num, false);
						}
					}
				}
			}
		};
		int layer2 = layer;
		yield return base.StartCoroutine(thingManager.InstantiateThingViaCache(thingRequestContext, thingId, action, false, false, layer2, inheritSuppressAttributesThing));
		yield break;
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x000D9BEC File Offset: 0x000D7FEC
	private void MoveMyIncludedSubThings()
	{
		if (this.assignedIncludedSubThings != null)
		{
			if (this.subThingsFollowDelayed)
			{
				this.assignedIncludedSubThings.transform.position = Vector3.Lerp(this.assignedIncludedSubThings.transform.position, base.transform.position, 0.05f);
				this.assignedIncludedSubThings.transform.rotation = Quaternion.Lerp(this.assignedIncludedSubThings.transform.rotation, base.transform.rotation, 0.05f);
			}
			else
			{
				this.assignedIncludedSubThings.transform.position = base.transform.position;
				this.assignedIncludedSubThings.transform.rotation = base.transform.rotation;
			}
		}
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x000D9CBC File Offset: 0x000D80BC
	public List<Transform> GetIncludedSubThingTransforms()
	{
		List<Transform> list = new List<Transform>();
		if (this.assignedIncludedSubThings != null)
		{
			IEnumerator enumerator = this.assignedIncludedSubThings.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					list.Add(transform);
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
		return list;
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x000D9D40 File Offset: 0x000D8140
	public Thing GetMyRootThing()
	{
		Thing parentThing = this.GetParentThing();
		return (!(parentThing != null)) ? null : parentThing.GetMyRootThing();
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x000D9D6C File Offset: 0x000D816C
	public void CreateMyReflectionPartsIfNeeded(string nameForStaticMergeParts = null)
	{
		if ((this.hasReflectionPartSideways || this.hasReflectionPartVertical || this.hasReflectionPartDepth) && this.assignedReflectionParts == null)
		{
			this.assignedReflectionParts = new GameObject[2, 2, 2];
			this.assignedReflectionPartsRenderers = new Renderer[2, 2, 2];
			int num = ((!this.hasReflectionPartSideways) ? 0 : 1);
			int num2 = ((!this.hasReflectionPartVertical) ? 0 : 1);
			int num3 = ((!this.hasReflectionPartDepth) ? 0 : 1);
			for (int i = 0; i <= num; i++)
			{
				for (int j = 0; j <= num2; j++)
				{
					for (int k = 0; k <= num3; k++)
					{
						bool flag = i == 1 && this.hasReflectionPartSideways;
						bool flag2 = j == 1 && this.hasReflectionPartVertical;
						bool flag3 = k == 1 && this.hasReflectionPartDepth;
						if (flag || flag2 || flag3)
						{
							GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(base.gameObject, base.transform.position, base.transform.rotation);
							gameObject.transform.parent = base.transform.parent;
							gameObject.transform.localScale = base.transform.localScale;
							gameObject.name = ((!string.IsNullOrEmpty(nameForStaticMergeParts)) ? nameForStaticMergeParts : ("Reflection " + base.gameObject.name));
							gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
							ThingPart component = gameObject.GetComponent<ThingPart>();
							component.baseType = this.baseType;
							component.materialType = this.materialType;
							if (this.GetIsOfThingBeingEdited())
							{
								gameObject.tag = "ReflectionPartDuringEditing";
								if (component != null)
								{
									component.enabled = false;
								}
								Collider component2 = gameObject.GetComponent<Collider>();
								if (component2 != null)
								{
									component2.enabled = false;
								}
							}
							else
							{
								component.SetIsReflectionPart();
							}
							this.assignedReflectionParts[i, j, k] = gameObject;
							this.assignedReflectionPartsRenderers[i, j, k] = gameObject.GetComponent<Renderer>();
						}
					}
				}
			}
		}
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x000D9FC1 File Offset: 0x000D83C1
	public void SetIsReflectionPart()
	{
		this.isReflectionPart = true;
		this.UpdateIsLedByOther();
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x000D9FD0 File Offset: 0x000D83D0
	public void RemoveMyReflectionPartsIfNeeded()
	{
		if (this.assignedReflectionParts != null)
		{
			for (int i = 0; i <= 1; i++)
			{
				for (int j = 0; j <= 1; j++)
				{
					for (int k = 0; k <= 1; k++)
					{
						if (this.assignedReflectionParts[i, j, k] != null)
						{
							global::UnityEngine.Object.Destroy(this.assignedReflectionParts[i, j, k]);
						}
					}
				}
			}
			this.assignedReflectionParts = null;
			this.assignedReflectionPartsRenderers = null;
		}
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x000DA058 File Offset: 0x000D8458
	public void HandleMyReflectionParts()
	{
		if (this.assignedReflectionParts != null)
		{
			Transform transform = null;
			Vector3 localPosition = base.transform.localPosition;
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			if (Our.mode == EditModes.Thing && base.transform.parent != null)
			{
				Hand component = base.transform.parent.GetComponent<Hand>();
				if (component != null)
				{
					transform = base.transform.parent;
					base.transform.parent = CreationHelper.thingBeingEdited.transform;
					Thing parentThing = this.GetParentThing();
					HandDot component2 = component.handDot.GetComponent<HandDot>();
					SnapHelper.SnapAllNeeded(parentThing, base.gameObject, component2.transform, component2.myPickUpPosition, component2.objectPickUpPosition, component2.myPickUpAngles, component2.objectPickUpAngles);
				}
			}
			for (int i = 0; i <= 1; i++)
			{
				for (int j = 0; j <= 1; j++)
				{
					for (int k = 0; k <= 1; k++)
					{
						bool flag = i == 1 && this.hasReflectionPartSideways;
						bool flag2 = j == 1 && this.hasReflectionPartVertical;
						bool flag3 = k == 1 && this.hasReflectionPartDepth;
						if (flag || flag2 || flag3)
						{
							GameObject gameObject = this.assignedReflectionParts[i, j, k];
							if (gameObject != null)
							{
								Transform transform2 = gameObject.transform;
								transform2.parent = base.transform.parent;
								transform2.localPosition = base.transform.localPosition;
								transform2.localRotation = base.transform.localRotation;
								transform2.localScale = base.transform.localScale;
								this.MirrorTransform(transform2, flag, flag2, flag3);
								if (this.renderer != null && Our.mode == EditModes.Thing)
								{
									this.assignedReflectionPartsRenderers[i, j, k].sharedMaterials = this.renderer.sharedMaterials;
								}
							}
						}
					}
				}
			}
			if (transform != null)
			{
				base.transform.parent = transform;
				base.transform.localPosition = localPosition;
				base.transform.localEulerAngles = localEulerAngles;
			}
		}
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x000DA2AC File Offset: 0x000D86AC
	private void MirrorTransform(Transform t, bool sideways, bool vertical, bool depth)
	{
		if (sideways && !vertical && !depth)
		{
			t.localRotation = new Quaternion(t.localRotation.x * -1f, t.localRotation.y, t.localRotation.z, t.localRotation.w * -1f);
			t.localPosition = new Vector3(t.localPosition.x * -1f, t.localPosition.y, t.localPosition.z);
		}
		if (!sideways && vertical && !depth)
		{
			t.localRotation = new Quaternion(t.localRotation.x * -1f, t.localRotation.y, t.localRotation.z, t.localRotation.w * -1f);
			t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y + 180f, t.localEulerAngles.z);
			t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, t.localPosition.z * -1f);
		}
		if (sideways && vertical && !depth)
		{
			t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y + 180f, t.localEulerAngles.z);
			t.localPosition = new Vector3(t.localPosition.x * -1f, t.localPosition.y, t.localPosition.z * -1f);
		}
		if (sideways && !vertical && depth)
		{
			t.localRotation = new Quaternion(t.localRotation.x, t.localRotation.y, t.localRotation.z * -1f, t.localRotation.w * -1f);
			t.localPosition = new Vector3(t.localPosition.x * -1f, t.localPosition.y * -1f, t.localPosition.z);
		}
		if (sideways && vertical && depth)
		{
			t.localRotation = new Quaternion(t.localRotation.x * -1f, t.localRotation.y * -1f, t.localRotation.z * -1f, t.localRotation.w * -1f);
			t.localEulerAngles = new Vector3(t.localEulerAngles.x + 180f, t.localEulerAngles.y, t.localEulerAngles.z);
			t.localPosition = new Vector3(t.localPosition.x * -1f, t.localPosition.y * -1f, t.localPosition.z * -1f);
		}
		if (!sideways && !vertical && depth)
		{
			t.localRotation = new Quaternion(t.localRotation.x * -1f, t.localRotation.y * -1f, t.localRotation.z * -1f, t.localRotation.w * -1f);
			t.localEulerAngles = new Vector3(t.localEulerAngles.x * -1f, t.localEulerAngles.y, t.localEulerAngles.z + 180f);
			t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y * -1f, t.localPosition.z);
		}
		if (!sideways && vertical && depth)
		{
			t.localRotation = new Quaternion(t.localRotation.x, t.localRotation.y, t.localRotation.z * -1f, t.localRotation.w * -1f);
			t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y + 180f, t.localEulerAngles.z + 180f);
			t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y * -1f, t.localPosition.z * -1f);
		}
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x000DA856 File Offset: 0x000D8C56
	public void ClearAssignedReflectionParts()
	{
		this.assignedReflectionParts = null;
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x000DA860 File Offset: 0x000D8C60
	public void CreateMyAutoContinuationPartsIfNeeded(string nameForStaticMergeParts = null)
	{
		if (this.autoContinuation != null && this.autoContinuation.assignedContinuationParts == null)
		{
			if (this.autoContinuation.fromPart == null && !string.IsNullOrEmpty(this.autoContinuation.fromPartGuid))
			{
				Thing parentThing = this.GetParentThing();
				if (parentThing != null)
				{
					ThingPart thingPartByGuid = parentThing.GetThingPartByGuid(this.autoContinuation.fromPartGuid);
					if (thingPartByGuid != null)
					{
						this.autoContinuation.fromPart = thingPartByGuid.gameObject;
					}
				}
			}
			if (this.autoContinuation.fromPart == null)
			{
				this.RemoveMyAssignedContinuationParts();
				this.autoContinuation = null;
				return;
			}
			if (nameForStaticMergeParts != null)
			{
				ThingPart component = this.autoContinuation.fromPart.GetComponent<ThingPart>();
				if (component != null && component.states.Count >= 2)
				{
					nameForStaticMergeParts = null;
					base.gameObject.name = this.baseType.ToString();
				}
			}
			this.autoContinuation.assignedContinuationParts = new GameObject[this.autoContinuation.count];
			this.autoContinuation.assignedContinuationPartsRenderers = new Renderer[this.autoContinuation.count];
			for (int i = 0; i < this.autoContinuation.count; i++)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(base.gameObject, base.transform.position, base.transform.rotation);
				gameObject.transform.parent = base.transform.parent;
				gameObject.transform.localScale = base.transform.localScale;
				gameObject.name = ((!string.IsNullOrEmpty(nameForStaticMergeParts)) ? nameForStaticMergeParts : ("Continuation " + base.gameObject.name));
				gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
				ThingPart component2 = gameObject.GetComponent<ThingPart>();
				component2.baseType = this.baseType;
				component2.materialType = this.materialType;
				if (this.GetIsOfThingBeingEdited())
				{
					gameObject.tag = "ContinuationPartDuringEditing";
					if (component2 != null)
					{
						component2.enabled = false;
					}
					Collider component3 = gameObject.GetComponent<Collider>();
					if (component3 != null)
					{
						component3.enabled = false;
					}
				}
				else
				{
					component2.SetIsContinuationPart();
				}
				this.autoContinuation.assignedContinuationParts[i] = gameObject;
				this.autoContinuation.assignedContinuationPartsRenderers[i] = gameObject.GetComponent<Renderer>();
			}
		}
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000DAAEC File Offset: 0x000D8EEC
	public void RemoveMyAssignedContinuationParts()
	{
		if (this.autoContinuation != null && this.autoContinuation.assignedContinuationParts != null)
		{
			for (int i = 0; i < this.autoContinuation.assignedContinuationParts.Length; i++)
			{
				global::UnityEngine.Object.Destroy(this.autoContinuation.assignedContinuationParts[i]);
			}
			this.autoContinuation.assignedContinuationParts = null;
		}
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x000DAB50 File Offset: 0x000D8F50
	public void HandleMyAutoContinuationParts()
	{
		if (this.autoContinuation != null && this.autoContinuation.assignedContinuationParts != null)
		{
			Transform transform = null;
			Vector3 localPosition = base.transform.localPosition;
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			if (Our.mode == EditModes.Thing && base.transform.parent != null)
			{
				Hand component = base.transform.parent.GetComponent<Hand>();
				if (component != null)
				{
					transform = base.transform.parent;
					base.transform.parent = CreationHelper.thingBeingEdited.transform;
					Thing parentThing = this.GetParentThing();
					HandDot component2 = component.handDot.GetComponent<HandDot>();
					SnapHelper.SnapAllNeeded(parentThing, base.gameObject, component2.transform, component2.myPickUpPosition, component2.objectPickUpPosition, component2.myPickUpAngles, component2.objectPickUpAngles);
				}
			}
			Transform transform2 = this.autoContinuation.fromPart.transform;
			Vector3 vector = base.transform.localPosition - transform2.localPosition;
			Vector3 vector2 = base.transform.localEulerAngles - transform2.localEulerAngles;
			Vector3 vector3 = base.transform.localScale - transform2.localScale;
			Color black = Color.black;
			Color color = Color.black;
			Vector3 localPosition2 = base.transform.localPosition;
			Vector3 vector4 = base.transform.localEulerAngles;
			Vector3 vector5 = base.transform.localScale;
			if (this.autoContinuation.includeColor)
			{
				if (this.renderer == null || this.material == null)
				{
					this.SetRendererIfNeeded();
					this.material = this.renderer.material;
				}
				if (this.autoContinuation.fromPartMaterial == null)
				{
					Renderer component3 = this.autoContinuation.fromPart.GetComponent<Renderer>();
					if (component3 != null)
					{
						this.autoContinuation.fromPartMaterial = component3.material;
					}
					else
					{
						this.autoContinuation.includeColor = false;
					}
				}
				if (this.renderer == null || this.material == null)
				{
					this.autoContinuation.includeColor = false;
				}
				if (this.autoContinuation.includeColor)
				{
					black = new Color(this.material.color.r - this.autoContinuation.fromPartMaterial.color.r, this.material.color.g - this.autoContinuation.fromPartMaterial.color.g, this.material.color.b - this.autoContinuation.fromPartMaterial.color.b, 0f);
					color = this.material.color;
				}
			}
			float num = 1f;
			int num2 = 0;
			int num3 = 0;
			BoolVector3 boolVector = new BoolVector3(false, false, false);
			if (this.autoContinuation.waves >= 1 && this.autoContinuation.count >= 1)
			{
				int num4 = this.autoContinuation.waves;
				if ((float)num4 > (float)this.autoContinuation.count / 2f)
				{
					num4 = (int)Mathf.Floor((float)this.autoContinuation.count / 2f);
				}
				if (num4 >= 1)
				{
					float num5 = (float)this.autoContinuation.count / (float)num4 / 2f - 1f;
					num2 = (int)Mathf.Round(num5);
					num3 = num2 - 1;
					Vector3 vector6 = new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
					boolVector.x = vector6.x > vector6.y && vector6.x > vector6.z;
					boolVector.y = vector6.y > vector6.x && vector6.y > vector6.z;
					boolVector.z = vector6.z > vector6.x && vector6.z > vector6.y;
				}
			}
			global::UnityEngine.Random.InitState(1);
			for (int i = 0; i < this.autoContinuation.assignedContinuationParts.Length; i++)
			{
				Transform transform3 = this.autoContinuation.assignedContinuationParts[i].transform;
				if (this.autoContinuation.waves > 0)
				{
					localPosition2.x += ((!boolVector.x) ? (vector.x * num) : vector.x);
					localPosition2.y += ((!boolVector.y) ? (vector.y * num) : vector.y);
					localPosition2.z += ((!boolVector.z) ? (vector.z * num) : vector.z);
					vector4 += vector2 * num;
					vector5 += vector3 * num;
				}
				else
				{
					localPosition2.x += vector.x;
					localPosition2.y += vector.y;
					localPosition2.z += vector.z;
					vector4 += vector2;
					vector5 += vector3;
				}
				if (vector5.x < 0.0005f)
				{
					vector5.x = 0.0005f;
				}
				if (vector5.y < 0.0005f)
				{
					vector5.y = 0.0005f;
				}
				if (vector5.z < 0.0005f)
				{
					vector5.z = 0.0005f;
				}
				transform3.localPosition = localPosition2;
				transform3.localEulerAngles = vector4;
				transform3.localScale = vector5;
				this.autoContinuation.ApplyRandomization(transform3);
				if (this.renderer != null && Our.mode == EditModes.Thing)
				{
					this.autoContinuation.assignedContinuationPartsRenderers[i].sharedMaterials = this.renderer.sharedMaterials;
				}
				if (this.autoContinuation.waves > 0 && --num3 < 0)
				{
					num *= -1f;
					num3 = num2;
				}
			}
			if (transform != null)
			{
				base.transform.parent = transform;
				base.transform.localPosition = localPosition;
				base.transform.localEulerAngles = localEulerAngles;
			}
			Misc.RandomizeRandomizer();
		}
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000DB222 File Offset: 0x000D9622
	public void SetIsContinuationPart()
	{
		this.isContinuationPart = true;
		this.UpdateIsLedByOther();
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000DB234 File Offset: 0x000D9634
	public void SplitAutoContinuedAndReflectedPartsMaterial()
	{
		if (this.assignedReflectionParts != null)
		{
			for (int i = 0; i <= 1; i++)
			{
				for (int j = 0; j <= 1; j++)
				{
					for (int k = 0; k <= 1; k++)
					{
						if (this.assignedReflectionParts[i, j, k] != null)
						{
							ThingPart component = this.assignedReflectionParts[i, j, k].GetComponent<ThingPart>();
							if (component != null)
							{
								component.CloneTextureFromOtherThingPart(this);
							}
						}
					}
				}
			}
		}
		if (this.autoContinuation != null && this.autoContinuation.assignedContinuationParts != null)
		{
			for (int l = 0; l < this.autoContinuation.assignedContinuationParts.Length; l++)
			{
				ThingPart component2 = this.autoContinuation.assignedContinuationParts[l].GetComponent<ThingPart>();
				if (component2 != null)
				{
					component2.CloneTextureFromOtherThingPart(this);
				}
			}
		}
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x000DB32C File Offset: 0x000D972C
	private void CloneTextureFromOtherThingPart(ThingPart otherThingPart)
	{
		for (int i = 0; i < this.textureTypes.Length; i++)
		{
			this.textureTypes[i] = otherThingPart.textureTypes[i];
			this.states[0].textureColors[i] = otherThingPart.states[0].textureColors[i];
			this.states[0].textureProperties[i] = Managers.thingManager.CloneTextureProperty(otherThingPart.states[0].textureProperties[i]);
		}
		this.UpdateMaterial();
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x000DB3D0 File Offset: 0x000D97D0
	public void SplitAutoContinuedAndReflectedPartsIntoStandalones()
	{
		this.autoContinuation = null;
		this.hasReflectionPartSideways = false;
		this.hasReflectionPartVertical = false;
		this.hasReflectionPartDepth = false;
		this.assignedReflectionParts = null;
		this.assignedReflectionPartsRenderers = null;
		this.isLedByOther = false;
		this.isReflectionPart = false;
		this.isContinuationPart = false;
		base.transform.tag = "ThingPart";
		Collider component = base.GetComponent<Collider>();
		if (component != null)
		{
			component.enabled = true;
		}
		this.renderer = base.GetComponent<Renderer>();
		if (this.renderer != null)
		{
			this.material = this.renderer.material;
		}
		this.SetStatePropertiesByTransform(false);
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x000DB47C File Offset: 0x000D987C
	public bool IsStrictSyncingToAreaNewcomersNeeded(bool optimizeCheckForSpeed)
	{
		return this.currentState < this.states.Count && (this.PersistStatesNeeded() || this.containsBehaviorScriptVariables || this.containsTextCommands || this.containsTurnCommands || this.HasPlacedSubThings() || !string.IsNullOrEmpty(this.states[this.currentState].name) || (!optimizeCheckForSpeed && this.browser != null && !string.IsNullOrEmpty(this.browser.Url) && !Managers.personManager.GetIsThisObjectOfOurPerson(this.browser.gameObject, false)) || base.GetComponent<AttractThings>() != null);
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x000DB550 File Offset: 0x000D9950
	public bool PersistStatesNeeded()
	{
		bool flag = this.parentPersistsPhysics && this.states.Count > 1;
		return this.persistStates || flag;
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x000DB58C File Offset: 0x000D998C
	public string GetSyncingToAreaNewcomersDataString(bool isOfInterval)
	{
		string text = string.Empty;
		if (!isOfInterval && this.browser != null && !string.IsNullOrEmpty(this.browser.Url) && !Managers.personManager.GetIsThisObjectOfOurPerson(this.browser.gameObject, false))
		{
			text = PersonManager.GetSyncingCompressedString(this.browser.Url);
		}
		AttractThings component = base.GetComponent<AttractThings>();
		float num = ((!(component != null)) ? 0f : component.settings.strength);
		object[] array = new object[27];
		array[0] = this.indexWithinThing;
		array[1] = "|";
		array[2] = this.currentState;
		array[3] = "|";
		array[4] = PersonManager.GetSyncingCompressedInt(this.currentStateTarget);
		array[5] = "|";
		array[6] = PersonManager.GetSyncingCompressedInt(this.currentStateTargetCurveVia);
		array[7] = "|";
		array[8] = PersonManager.GetSyncingCompressedFloat(this.timeWhenToRevert);
		array[9] = "|";
		array[10] = PersonManager.GetSyncingCompressedFloat(this.stateTargetSeconds);
		array[11] = "|";
		array[12] = PersonManager.GetSyncingCompressedFloat(this.stateTargetTime);
		array[13] = "|";
		array[14] = this.states[this.currentState].name;
		array[15] = "|";
		array[16] = Misc.Truncate(PersonManager.EncodeSyncingString(this.originalText), 500, true);
		array[17] = "|";
		int num2 = 18;
		int num3 = (int)this.tweenType;
		array[num2] = num3.ToString();
		array[19] = "|";
		array[20] = PersonManager.GetSyncingCompressedBool(this.invisible);
		array[21] = "|";
		array[22] = PersonManager.GetSyncingCompressedBool(this.uncollidable);
		array[23] = "|";
		array[24] = text;
		array[25] = "|";
		array[26] = num;
		return string.Concat(array);
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x000DB77C File Offset: 0x000D9B7C
	public void SetViaSyncingToAreaNewcomersDataString(int currentState, int currentStateTarget, int currentStateTargetCurveVia, float timeWhenToRevert, float stateTargetSeconds, float stateTargetTime, string stateName, string originalText, TweenType tweenType, bool invisible, bool uncollidable, string browserUrl, float attractStrength)
	{
		if (currentState >= this.states.Count || currentStateTarget >= this.states.Count || currentStateTargetCurveVia >= this.states.Count)
		{
			Log.Debug("States data exceeded states count in SetViaSyncingToAreaNewcomersDataString for " + base.transform.parent.name + ", so ignoring");
			return;
		}
		if (this.persistStatesForWearerOnly)
		{
			return;
		}
		if (!this.MyStatesAreBeingEdited())
		{
			this.currentState = currentState;
			this.currentStateTarget = currentStateTarget;
			this.currentStateTargetCurveVia = currentStateTargetCurveVia;
			this.timeWhenToRevert = timeWhenToRevert;
			this.stateTargetSeconds = stateTargetSeconds;
			this.stateTargetTime = stateTargetTime;
			this.states[this.currentState].name = ((!string.IsNullOrEmpty(stateName)) ? stateName : null);
			if (this.originalText != originalText)
			{
				this.SetOriginalText(originalText);
			}
			this.tweenType = tweenType;
			if (invisible != this.invisible || uncollidable != this.uncollidable)
			{
				this.invisible = invisible;
				this.uncollidable = uncollidable;
				this.ApplyCurrentInvisibleAndCollidable();
			}
			this.states[this.currentState].didTriggerStartEvent = true;
			if (this.states.Count > 1)
			{
				this.SetTransformPropertiesByState(false, false);
			}
			if (!string.IsNullOrEmpty(browserUrl) && Managers.browserManager.IsValidUrlProtocol(browserUrl))
			{
				if (this.browser == null)
				{
					BrowserSettings browserSettings = new BrowserSettings();
					browserSettings.url = browserUrl;
					Managers.browserManager.TryAttachBrowser(this, browserSettings, false);
					if (this.browser != null)
					{
						this.browser.lastRemotePersonReceivedUrl = browserUrl;
					}
				}
				else if (this.browser.Url != browserUrl)
				{
					this.browser.lastRemotePersonReceivedUrl = browserUrl;
					this.browser.Url = browserUrl;
				}
			}
			this.ApplyAttractThingsSettings(new AttractThingsSettings
			{
				strength = attractStrength
			}, true);
		}
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x000DB988 File Offset: 0x000D9D88
	public void ApplyCurrentInvisibleAndCollidable()
	{
		this.SetRendererIfNeeded();
		this.renderer.enabled = !this.invisible;
		Collider component = base.GetComponent<Collider>();
		if (component != null)
		{
			component.enabled = !this.uncollidable;
		}
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x000DB9D4 File Offset: 0x000D9DD4
	private float? GetProximityToNextForwardObject()
	{
		float? num = null;
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		foreach (RaycastHit raycastHit in (from h in Physics.RaycastAll(ray, 10000f)
			orderby h.distance
			select h).ToArray<RaycastHit>())
		{
			ThingPart component = raycastHit.transform.GetComponent<ThingPart>();
			if (component != null && component != this)
			{
				num = new float?(raycastHit.distance);
				break;
			}
		}
		return num;
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x000DBAA0 File Offset: 0x000D9EA0
	public void ForceMySubThingsVisibleCollidable()
	{
		if (this.assignedIncludedSubThings != null)
		{
			Component[] componentsInChildren = this.assignedIncludedSubThings.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				Renderer component = thingPart.GetComponent<Renderer>();
				if (component != null)
				{
					component.enabled = true;
				}
				Collider component2 = thingPart.GetComponent<Collider>();
				if (component2 != null)
				{
					component2.enabled = true;
				}
			}
		}
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x000DBB23 File Offset: 0x000D9F23
	private void EnablePersistStatesForWearerOnly()
	{
		this.persistStatesForWearerOnly = true;
		this.persistStates = false;
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x000DBB34 File Offset: 0x000D9F34
	public void DeleteCurrentStateDuringEditing()
	{
		if (this.states.Count >= 2)
		{
			this.states.RemoveAt(this.currentState);
			this.currentState--;
			if (this.currentState < 0)
			{
				this.currentState = 0;
			}
		}
		else
		{
			this.states = new List<ThingPartState>();
			this.states.Add(new ThingPartState());
			this.SetStatePropertiesByTransform(false);
		}
		this.ResetAtCurrentStateDuringEditing();
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x000DBBB4 File Offset: 0x000D9FB4
	public void ResetAtCurrentStateDuringEditing()
	{
		int num = this.currentState;
		this.ResetStates();
		this.currentState = num;
		this.SetTransformPropertiesByState(false, false);
		this.states[this.currentState].didTriggerStartEvent = true;
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x000DBBF4 File Offset: 0x000D9FF4
	public void SetInvisibleToOurPerson(bool isInvisible)
	{
		string text = ((!isInvisible) ? "Default" : "InvisibleToOurPerson");
		base.gameObject.layer = LayerMask.NameToLayer(text);
	}

	// Token: 0x040015C7 RID: 5575
	public int indexWithinThing = -1;

	// Token: 0x040015C8 RID: 5576
	public ThingPartBase baseType = ThingPartBase.Cube;

	// Token: 0x040015C9 RID: 5577
	public string guid;

	// Token: 0x040015CA RID: 5578
	public MaterialTypes materialType;

	// Token: 0x040015CB RID: 5579
	public MaterialTypes materialTypeBeforeOptimization;

	// Token: 0x040015CC RID: 5580
	public List<ThingPartState> states = new List<ThingPartState>();

	// Token: 0x040015CD RID: 5581
	public int currentState;

	// Token: 0x040015CE RID: 5582
	public int currentStateTarget = -1;

	// Token: 0x040015CF RID: 5583
	private int currentStateTargetCurveVia = -1;

	// Token: 0x040015D0 RID: 5584
	public string givenName;

	// Token: 0x040015D1 RID: 5585
	public const float secondsToShowChatText = 15f;

	// Token: 0x040015D2 RID: 5586
	private Renderer renderer;

	// Token: 0x040015D3 RID: 5587
	public Material material;

	// Token: 0x040015D4 RID: 5588
	public bool isGrabbable;

	// Token: 0x040015D5 RID: 5589
	public bool offersScreen;

	// Token: 0x040015D6 RID: 5590
	public bool videoScreenHasSurroundSound;

	// Token: 0x040015D7 RID: 5591
	public bool videoScreenLoops;

	// Token: 0x040015D8 RID: 5592
	public bool videoScreenIsDirectlyOnMesh;

	// Token: 0x040015D9 RID: 5593
	public bool videoScreenFlipsX;

	// Token: 0x040015DA RID: 5594
	public bool isVideoButton;

	// Token: 0x040015DB RID: 5595
	public bool scalesUniformly;

	// Token: 0x040015DC RID: 5596
	public bool textureScalesUniformly;

	// Token: 0x040015DD RID: 5597
	public bool isLiquid;

	// Token: 0x040015DE RID: 5598
	public bool isImagePasteScreen;

	// Token: 0x040015DF RID: 5599
	public bool useTextureAsSky;

	// Token: 0x040015E0 RID: 5600
	public bool stretchSkydomeSeam;

	// Token: 0x040015E1 RID: 5601
	public bool isDedicatedCollider;

	// Token: 0x040015E2 RID: 5602
	private bool didJustThingPortal;

	// Token: 0x040015E3 RID: 5603
	public bool offersSlideshowScreen;

	// Token: 0x040015E4 RID: 5604
	public bool isSlideshowButton;

	// Token: 0x040015E5 RID: 5605
	public bool isCamera;

	// Token: 0x040015E6 RID: 5606
	public bool isCameraButton;

	// Token: 0x040015E7 RID: 5607
	public bool isFishEyeCamera;

	// Token: 0x040015E8 RID: 5608
	public bool useUnsoftenedAnimations;

	// Token: 0x040015E9 RID: 5609
	public bool omitAutoSounds;

	// Token: 0x040015EA RID: 5610
	public bool doSnapTextureAngles;

	// Token: 0x040015EB RID: 5611
	public bool avoidCastShadow;

	// Token: 0x040015EC RID: 5612
	public bool avoidReceiveShadow;

	// Token: 0x040015ED RID: 5613
	public bool looselyCoupledParticles;

	// Token: 0x040015EE RID: 5614
	public bool textAlignCenter;

	// Token: 0x040015EF RID: 5615
	public bool textAlignRight;

	// Token: 0x040015F0 RID: 5616
	public bool isAngleLocker;

	// Token: 0x040015F1 RID: 5617
	public bool isPositionLocker;

	// Token: 0x040015F2 RID: 5618
	public bool isLocked;

	// Token: 0x040015F3 RID: 5619
	public bool allowBlackImageBackgrounds;

	// Token: 0x040015F4 RID: 5620
	public bool subThingsFollowDelayed;

	// Token: 0x040015F5 RID: 5621
	public bool persistStates;

	// Token: 0x040015F6 RID: 5622
	public bool parentPersistsPhysics;

	// Token: 0x040015F7 RID: 5623
	public bool personalExperience;

	// Token: 0x040015F8 RID: 5624
	public bool invisibleToUsWhenAttached;

	// Token: 0x040015F9 RID: 5625
	public bool lightOmitsShadow;

	// Token: 0x040015FA RID: 5626
	public bool showDirectionArrowsWhenEditing;

	// Token: 0x040015FB RID: 5627
	private bool persistStatesForWearerOnly;

	// Token: 0x040015FC RID: 5628
	public bool? convex;

	// Token: 0x040015FD RID: 5629
	public bool invisible;

	// Token: 0x040015FE RID: 5630
	public bool uncollidable;

	// Token: 0x040015FF RID: 5631
	public bool? invisibleStateToReset;

	// Token: 0x04001600 RID: 5632
	public bool? uncollidableStateToReset;

	// Token: 0x04001601 RID: 5633
	public bool isInInventoryOrDialog;

	// Token: 0x04001602 RID: 5634
	public bool isInInventory;

	// Token: 0x04001603 RID: 5635
	public bool isGiftInDialog;

	// Token: 0x04001604 RID: 5636
	public GameObject videoScreen;

	// Token: 0x04001605 RID: 5637
	public GameObject slideshowScreen;

	// Token: 0x04001606 RID: 5638
	public bool activeEvenInInventory;

	// Token: 0x04001607 RID: 5639
	public bool isUnremovableCenter;

	// Token: 0x04001608 RID: 5640
	public bool isText;

	// Token: 0x04001609 RID: 5641
	public bool hasTextPlaceholders;

	// Token: 0x0400160A RID: 5642
	public string originalText = string.Empty;

	// Token: 0x0400160B RID: 5643
	private TextMesh textMesh;

	// Token: 0x0400160C RID: 5644
	private string typedText = string.Empty;

	// Token: 0x0400160D RID: 5645
	private float timeWhenToClearTypedText;

	// Token: 0x0400160E RID: 5646
	public float textLineHeight = 1f;

	// Token: 0x0400160F RID: 5647
	public bool isControllableCollider;

	// Token: 0x04001610 RID: 5648
	public bool isControllableWheel;

	// Token: 0x04001611 RID: 5649
	public float controllableBodySlidiness;

	// Token: 0x04001612 RID: 5650
	public float controllableBodyBounciness;

	// Token: 0x04001613 RID: 5651
	private bool pushBackEnabled = true;

	// Token: 0x04001614 RID: 5652
	public JoystickToControllablePart joystickToControllablePart;

	// Token: 0x04001615 RID: 5653
	public int thingVersion = 9;

	// Token: 0x04001616 RID: 5654
	private float timeWhenToRevert = -1f;

	// Token: 0x04001617 RID: 5655
	public float propelForwardPercent;

	// Token: 0x04001618 RID: 5656
	public float rotateForwardPercent;

	// Token: 0x04001619 RID: 5657
	public string videoIdToPlayAtAreaStart;

	// Token: 0x0400161A RID: 5658
	public string videoIdToPlayWhenPressed;

	// Token: 0x0400161B RID: 5659
	private Light light;

	// Token: 0x0400161C RID: 5660
	private Vector3 lightLastAppliedScale = Vector3.zero;

	// Token: 0x0400161D RID: 5661
	private Color lightLastAppliedColor = Color.black;

	// Token: 0x0400161E RID: 5662
	private TweenType tweenType = TweenType.EaseInOut;

	// Token: 0x0400161F RID: 5663
	private ParticleSystem baseLayerParticles;

	// Token: 0x04001620 RID: 5664
	private Vector3 baseLayerParticlesLastAppliedScale = Vector3.zero;

	// Token: 0x04001621 RID: 5665
	private Color baseLayerParticlesLastAppliedColor = Color.black;

	// Token: 0x04001622 RID: 5666
	public ParticleSystemType particleSystemType;

	// Token: 0x04001623 RID: 5667
	private GameObject particleSystemChild;

	// Token: 0x04001624 RID: 5668
	private List<ParticleSystem> particleSystemComponents;

	// Token: 0x04001625 RID: 5669
	private ParticleSystemType currentlyAppliedParticleSystemType;

	// Token: 0x04001626 RID: 5670
	private ParticleSystem.MinMaxCurve? particleSystemOriginalAmountBeforeChange;

	// Token: 0x04001627 RID: 5671
	public TextureType[] textureTypes = new TextureType[2];

	// Token: 0x04001628 RID: 5672
	public TextureType[] currentlyAppliedTextureTypes = new TextureType[2];

	// Token: 0x04001629 RID: 5673
	private const float scaleFactorAtWhichToEnableComponent = 0.6f;

	// Token: 0x0400162A RID: 5674
	private float stateTargetTime = -1f;

	// Token: 0x0400162B RID: 5675
	private float stateTargetSeconds = -1f;

	// Token: 0x0400162C RID: 5676
	private bool stateSwitchOriginatedFromOtherClient;

	// Token: 0x0400162D RID: 5677
	private MaterialTypes currentlyAppliedMaterialType;

	// Token: 0x0400162E RID: 5678
	private AudioSource loopSound;

	// Token: 0x0400162F RID: 5679
	private float lastEmitTime = -1f;

	// Token: 0x04001630 RID: 5680
	private Vector3 lastPositionForUndo = Vector3.zero;

	// Token: 0x04001631 RID: 5681
	private Vector3 lastRotationForUndo = Vector3.zero;

	// Token: 0x04001632 RID: 5682
	private Vector3 lastScaleForUndo = Vector3.one;

	// Token: 0x04001633 RID: 5683
	private Color lastColorForUndo = Color.clear;

	// Token: 0x04001634 RID: 5684
	private const float logarithmZoom = 100f;

	// Token: 0x04001635 RID: 5685
	private const float maxDistanceToReactToForNearby = 2f;

	// Token: 0x04001636 RID: 5686
	public Dictionary<string, ThingIdPositionRotation> placedSubThingIdsWithOriginalInfo = new Dictionary<string, ThingIdPositionRotation>();

	// Token: 0x04001637 RID: 5687
	private GameObject assignedPlacedSubThings;

	// Token: 0x04001638 RID: 5688
	public string imageUrl = string.Empty;

	// Token: 0x04001639 RID: 5689
	public bool startedAutoAddingImage;

	// Token: 0x0400163A RID: 5690
	private Texture2D imageTexture;

	// Token: 0x0400163B RID: 5691
	private int imageWidth;

	// Token: 0x0400163C RID: 5692
	private int imageHeight;

	// Token: 0x0400163D RID: 5693
	private Vector3? localScaleBeforeImagePaste;

	// Token: 0x0400163E RID: 5694
	public ImageType imageType;

	// Token: 0x0400163F RID: 5695
	public Transform ourPersonRidingBeaconReference;

	// Token: 0x04001640 RID: 5696
	private float ridingBeaconLastUpdatedTime = -1f;

	// Token: 0x04001641 RID: 5697
	private const float nonsmoothRideUpdateFrequencySeconds = 0.5f;

	// Token: 0x04001642 RID: 5698
	public Vector3? originalLocalRotation;

	// Token: 0x04001643 RID: 5699
	public float videoAutoPlayVolume = 1f;

	// Token: 0x04001644 RID: 5700
	public VoiceProperties voiceProperties;

	// Token: 0x04001645 RID: 5701
	private List<SoundTrack> soundTracks;

	// Token: 0x04001646 RID: 5702
	public List<IncludedSubThing> includedSubThings;

	// Token: 0x04001647 RID: 5703
	private GameObject assignedIncludedSubThings;

	// Token: 0x04001648 RID: 5704
	public bool hasReflectionPartSideways;

	// Token: 0x04001649 RID: 5705
	public bool hasReflectionPartVertical;

	// Token: 0x0400164A RID: 5706
	public bool hasReflectionPartDepth;

	// Token: 0x0400164B RID: 5707
	private GameObject[,,] assignedReflectionParts;

	// Token: 0x0400164C RID: 5708
	private Renderer[,,] assignedReflectionPartsRenderers;

	// Token: 0x0400164D RID: 5709
	private bool isReflectionPart;

	// Token: 0x0400164E RID: 5710
	public ThingPartAutoContinuation autoContinuation;

	// Token: 0x0400164F RID: 5711
	private bool isContinuationPart;

	// Token: 0x04001650 RID: 5712
	private bool isLedByOther;

	// Token: 0x04001651 RID: 5713
	private const string imageHighQualityIndicator = "-hiquality-";

	// Token: 0x04001652 RID: 5714
	public bool containsForAnyStateListeners;

	// Token: 0x04001653 RID: 5715
	public bool containsBehaviorScriptVariables;

	// Token: 0x04001654 RID: 5716
	public bool containsTextCommands;

	// Token: 0x04001655 RID: 5717
	public bool containsTurnCommands;

	// Token: 0x04001656 RID: 5718
	public Browser browser;

	// Token: 0x04001657 RID: 5719
	public Dictionary<int, Vector3> changedVertices;

	// Token: 0x04001658 RID: 5720
	public int? smoothingAngle;
}
