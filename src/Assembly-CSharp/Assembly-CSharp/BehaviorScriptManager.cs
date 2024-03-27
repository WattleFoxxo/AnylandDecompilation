using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x020001E2 RID: 482
public class BehaviorScriptManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00087CDE File Offset: 0x000860DE
	// (set) Token: 0x06000FBA RID: 4026 RVA: 0x00087CE6 File Offset: 0x000860E6
	public ManagerStatus status { get; private set; }

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000FBB RID: 4027 RVA: 0x00087CEF File Offset: 0x000860EF
	// (set) Token: 0x06000FBC RID: 4028 RVA: 0x00087CF7 File Offset: 0x000860F7
	public string failMessage { get; private set; }

	// Token: 0x06000FBD RID: 4029 RVA: 0x00087D00 File Offset: 0x00086100
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.comparators = new string[]
		{
			"<=", ">=", "==", "<>", "!=", "=<", "=>", "><", "<", ">",
			"="
		};
		this.disallowedVariableNames = new string[] { "false", "true", "is", "when", "then", "and", "or", "if", "not" };
		this.ResetTimeToClearVariableCalculationsLimitHit();
		this.placeholdersReturningStrings = new string[] { "area name", "thing name", "closest held", "people names", "typed", "area values", "thing values", "person values", "person" };
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x00087E32 File Offset: 0x00086232
	private void Update()
	{
		this.HandleVariableCalculationsCapping();
		this.tellCountForThisUpdate = 0;
	}

	// Token: 0x06000FBF RID: 4031 RVA: 0x00087E44 File Offset: 0x00086244
	public bool IterateTellCountIfStillUnderLimit()
	{
		bool flag = this.tellCountForThisUpdate < 250;
		if (flag)
		{
			this.tellCountForThisUpdate++;
		}
		return flag;
	}

	// Token: 0x06000FC0 RID: 4032 RVA: 0x00087E74 File Offset: 0x00086274
	private void HandleVariableCalculationsCapping()
	{
		this.variableCalculationsThisFrame = 0;
		if (Time.time >= this.timeToClearVariableCalculationsLimitHit && this.variableCalculationsLimitHit > 0)
		{
			Managers.soundManager.Play("no", null, 1f, true, false);
			this.variableCalculationsLimitHit = 0;
		}
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x00087EC2 File Offset: 0x000862C2
	private void ResetTimeToClearVariableCalculationsLimitHit()
	{
		this.timeToClearVariableCalculationsLimitHit = Time.time + 2.5f;
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x00087ED8 File Offset: 0x000862D8
	public bool RegisterNewVariableCalculationAttempted()
	{
		bool flag = false;
		if (this.variableCalculationsThisFrame < 50 && this.variableCalculationsLimitHit <= 2)
		{
			this.variableCalculationsThisFrame++;
			if (this.variableCalculationsThisFrame == 50)
			{
				this.variableCalculationsLimitHit++;
			}
			flag = true;
		}
		return flag;
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x00087F2C File Offset: 0x0008632C
	public void TriggerEventsRelatedToPosition()
	{
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
		Vector3 position = Managers.personManager.ourPerson.Torso.transform.position;
		foreach (Thing thing in componentsInChildren)
		{
			float num = Vector3.Distance(thing.transform.position, position);
			if (num <= 7.5f)
			{
				if (num <= 3f)
				{
					if (num <= 2f)
					{
						thing.TriggerEventAsStateAuthority(StateListener.EventType.OnWalkedInto, string.Empty);
					}
					thing.TriggerEventAsStateAuthority(StateListener.EventType.OnNeared, string.Empty);
				}
				if (!thing.triggeredOnSomeoneNewInVicinity)
				{
					thing.triggeredOnSomeoneNewInVicinity = true;
					thing.TriggerEventAsStateAuthority(StateListener.EventType.OnSomeoneNewInVicinity, string.Empty);
				}
				thing.TriggerEventAsStateAuthority(StateListener.EventType.OnSomeoneInVicinity, string.Empty);
			}
		}
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x00088014 File Offset: 0x00086414
	public void TriggerTellNearbyEvent(string data, Vector3 originPosition)
	{
		Component[] allThings = Managers.thingManager.GetAllThings();
		foreach (Thing thing in allThings)
		{
			if (Vector3.Distance(thing.transform.position, originPosition) <= 7.5f)
			{
				thing.TriggerEvent(StateListener.EventType.OnToldByNearby, data, false, null);
			}
		}
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x00088074 File Offset: 0x00086474
	public void TriggerTellFirstOfAnyEvent(string data, Vector3 originPosition, GameObject ownThingObject)
	{
		Component[] array = Managers.thingManager.GetAllThings();
		array = array.OrderBy((Component x) => Vector3.Distance(originPosition, x.transform.position)).ToArray<Component>();
		foreach (Thing thing in array)
		{
			if (thing.gameObject != ownThingObject)
			{
				bool flag = thing.TriggerEvent(StateListener.EventType.OnToldByAny, data, false, null);
				if (flag)
				{
					break;
				}
			}
		}
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x00088100 File Offset: 0x00086500
	public void TriggerTellInFront(string data, ThingPart thingPart, bool firstAndUnblockedPathOnly = false)
	{
		Ray ray = new Ray(thingPart.transform.position, thingPart.transform.forward);
		RaycastHit[] array = (from h in Physics.RaycastAll(ray)
			orderby h.distance
			select h).ToArray<RaycastHit>();
		foreach (RaycastHit raycastHit in array)
		{
			ThingPart component = raycastHit.transform.GetComponent<ThingPart>();
			if (component != null && component != thingPart && component.transform.parent != null)
			{
				Thing component2 = component.transform.parent.GetComponent<Thing>();
				if (component2 && !component2.isPassable)
				{
					component.TriggerEvent(StateListener.EventType.OnToldByAny, data, false, null);
					if (firstAndUnblockedPathOnly)
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x00088204 File Offset: 0x00086604
	public void TriggerTellBodyEventToAttachments(Person person, string data, bool weAreStateAuthority = false)
	{
		foreach (KeyValuePair<AttachmentPointId, GameObject> keyValuePair in person.AttachmentPointsById)
		{
			if (keyValuePair.Value != null)
			{
				Thing[] componentsInChildren = keyValuePair.Value.GetComponentsInChildren<Thing>();
				if (componentsInChildren.Length >= 1)
				{
					componentsInChildren[0].TriggerEvent(StateListener.EventType.OnToldByBody, data, weAreStateAuthority, null);
				}
			}
		}
		this.TriggerTellBodyEventToHandHoldable(person, data, TopographyId.Left, weAreStateAuthority);
		this.TriggerTellBodyEventToHandHoldable(person, data, TopographyId.Right, weAreStateAuthority);
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x000882A8 File Offset: 0x000866A8
	private void TriggerTellBodyEventToHandHoldable(Person person, string data, TopographyId topographyId, bool weAreStateAuthority = false)
	{
		GameObject handByTopographyId = person.GetHandByTopographyId(topographyId);
		GameObject thingInHand = person.GetThingInHand(handByTopographyId, true);
		if (thingInHand != null)
		{
			Thing component = thingInHand.GetComponent<Thing>();
			if (component != null)
			{
				component.TriggerEvent(StateListener.EventType.OnToldByBody, data, weAreStateAuthority, null);
			}
		}
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x000882F4 File Offset: 0x000866F4
	public void TriggerTellAnyEvent(string data, bool weAreStateAuthority = false)
	{
		Component[] allThings = Managers.thingManager.GetAllThings();
		foreach (Thing thing in allThings)
		{
			thing.TriggerEvent(StateListener.EventType.OnToldByAny, data, weAreStateAuthority, null);
		}
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x00088338 File Offset: 0x00086738
	public void TriggerTellAnyWebEvent(string data)
	{
		bool flag = Our.IsMasterClient(true);
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		foreach (GameObject gameObject in rootGameObjects)
		{
			Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Browser), true);
			foreach (Browser browser in componentsInChildren)
			{
				if (browser.gameObject.name != "GuideBrowser")
				{
					browser.CallFunction("AnylandToldByAny", new JSONNode[] { data, flag }).Done();
				}
			}
		}
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x000883FC File Offset: 0x000867FC
	public void TriggerVariableChangeToThings(string data = null, Person relevantPerson = null)
	{
		Component[] allThings = Managers.thingManager.GetAllThings();
		foreach (Thing thing in allThings)
		{
			if (thing.containsBehaviorScriptVariables)
			{
				Thing thing2 = thing;
				StateListener.EventType eventType = StateListener.EventType.OnVariableChange;
				thing2.TriggerEvent(eventType, data, false, relevantPerson);
			}
		}
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x00088458 File Offset: 0x00086858
	private GameObject[] GetAllPlacementThingsAsArray()
	{
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
		GameObject[] array = new GameObject[componentsInChildren.Length];
		int num = 0;
		foreach (Component component in componentsInChildren)
		{
			array[num++] = component.gameObject;
		}
		return array;
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x000884BC File Offset: 0x000868BC
	public void TriggerOnHears(string speech)
	{
		Vector3 position = Managers.personManager.ourPerson.Head.transform.position;
		GameObject @object = Managers.treeManager.GetObject("/Universe/ThrownOrEmittedThings");
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
		Component[] componentsInChildren2 = @object.GetComponentsInChildren(typeof(Thing), true);
		Component[] array = componentsInChildren.Concat(componentsInChildren2).ToArray<Component>();
		foreach (Thing thing in array)
		{
			if (Vector3.Distance(thing.transform.position, position) <= 7.5f)
			{
				thing.TriggerEvent(StateListener.EventType.OnHears, speech, true, null);
			}
			thing.TriggerEvent(StateListener.EventType.OnHearsAnywhere, speech, true, null);
		}
		Component[] componentsInChildren3 = Managers.personManager.ourPerson.GetComponentsInChildren(typeof(Thing), true);
		foreach (Thing thing2 in componentsInChildren3)
		{
			thing2.TriggerEvent(StateListener.EventType.OnHears, speech, true, null);
			thing2.TriggerEvent(StateListener.EventType.OnHearsAnywhere, speech, true, null);
		}
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x000885EC File Offset: 0x000869EC
	public void Speak(ThingPart thingPart, string text, VoiceProperties properties)
	{
		if (this.voice == null)
		{
			try
			{
				this.voice = base.gameObject.AddComponent<WindowsVoice>();
				this.voice.Init();
			}
			catch (Exception ex)
			{
				this.voice = null;
				Log.Debug("Failed to initialize Windows Voice");
			}
		}
		if (this.voice != null)
		{
			bool flag = false;
			Thing component = thingPart.transform.parent.gameObject.GetComponent<Thing>();
			if (component != null)
			{
				flag = component.hasSurroundSound;
			}
			Vector3 position = Managers.personManager.ourPerson.Head.transform.position;
			float num = Vector3.Distance(position, thingPart.transform.position);
			float num2 = 1f;
			if (num > 2f && !flag)
			{
				num2 = 1f - (num - 2f) / 10.5f;
			}
			num2 *= 0.5f;
			if (properties == null)
			{
				properties = new VoiceProperties();
			}
			VoiceProperties voiceProperties = new VoiceProperties();
			voiceProperties.gender = properties.gender;
			voiceProperties.volume = (int)((float)properties.volume * num2);
			voiceProperties.pitch = properties.pitch;
			voiceProperties.speed = properties.speed;
			if (voiceProperties.volume > 0)
			{
				this.voice.Speak(text, voiceProperties);
			}
		}
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x0008875C File Offset: 0x00086B5C
	public void StopSpeech()
	{
		if (this.voice != null)
		{
			global::UnityEngine.Object.Destroy(this.voice);
			this.voice = null;
		}
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x00088784 File Offset: 0x00086B84
	public string ReplaceTextPlaceholders(string s, Thing thing, ThingPart thingPart)
	{
		if (Misc.ContainsCaseInsensitive(s, "value"))
		{
			Person person = null;
			if (Misc.ContainsCaseInsensitive(s, " values]"))
			{
				bool flag = thing.IsPlacement() || Managers.areaManager.weAreEditorOfCurrentArea;
				if (Misc.ContainsCaseInsensitive(s, "[area values]"))
				{
					string text = ((!flag) ? string.Empty : this.GetVariablesString(Managers.areaManager.behaviorScriptVariables));
					s = s.ReplaceCaseInsensitive("[area values]", text);
				}
				if (Misc.ContainsCaseInsensitive(s, "[person values]"))
				{
					person = this.GetPersonVariablesRelevantPerson(thingPart);
					string text2 = ((!flag || !(person != null)) ? string.Empty : this.GetVariablesString(person.behaviorScriptVariables));
					s = s.ReplaceCaseInsensitive("[person values]", text2);
				}
				if (Misc.ContainsCaseInsensitive(s, "[thing values]"))
				{
					s = s.ReplaceCaseInsensitive("[thing values]", this.GetVariablesString(thing.behaviorScriptVariables));
				}
				if (Misc.ContainsCaseInsensitive(s, "[person.") && Misc.ContainsCaseInsensitive(s, " values]"))
				{
					if (flag)
					{
						s = this.ReplacePersonVariableNamesForAll(s);
					}
					Regex regex = new Regex("\\[([^\\]]+) values\\]", RegexOptions.IgnoreCase);
					s = regex.Replace(s, string.Empty);
				}
			}
			if (Misc.ContainsCaseInsensitive(s, " value]"))
			{
				if (Misc.ContainsCaseInsensitive(s, "[area."))
				{
					foreach (KeyValuePair<string, float> keyValuePair in Managers.areaManager.behaviorScriptVariables)
					{
						string text3 = "[" + keyValuePair.Key + " value]";
						s = s.ReplaceCaseInsensitive(text3, keyValuePair.Value.ToString());
					}
				}
				if (Misc.ContainsCaseInsensitive(s, "["))
				{
					foreach (KeyValuePair<string, float> keyValuePair2 in thing.behaviorScriptVariables)
					{
						string text4 = "[" + keyValuePair2.Key + " value]";
						s = s.ReplaceCaseInsensitive(text4, keyValuePair2.Value.ToString());
					}
				}
				if (Misc.ContainsCaseInsensitive(s, "[person."))
				{
					if (person == null)
					{
						person = this.GetPersonVariablesRelevantPerson(thingPart);
					}
					if (person != null)
					{
						foreach (KeyValuePair<string, float> keyValuePair3 in person.behaviorScriptVariables)
						{
							string text5 = "[" + keyValuePair3.Key + " value]";
							s = s.ReplaceCaseInsensitive(text5, keyValuePair3.Value.ToString());
						}
					}
				}
				if (Misc.ContainsCaseInsensitive(s, "[area.") || Misc.ContainsCaseInsensitive(s, "["))
				{
					Regex regex2 = new Regex("\\[([^\\]]+) value\\]", RegexOptions.IgnoreCase);
					s = regex2.Replace(s, "0");
				}
			}
		}
		return s;
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x00088AF0 File Offset: 0x00086EF0
	private string ReplacePersonVariableNamesForAll(string s)
	{
		List<string> allPersonVariableNames = this.GetAllPersonVariableNames();
		foreach (string text in allPersonVariableNames)
		{
			string text2 = "[" + text + " values]";
			if (Misc.ContainsCaseInsensitive(s, text2))
			{
				string allPersonVariableValues = this.GetAllPersonVariableValues(text);
				s = s.ReplaceCaseInsensitive(text2, allPersonVariableValues);
			}
		}
		return s;
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x00088B7C File Offset: 0x00086F7C
	private List<string> GetAllPersonVariableNames()
	{
		List<string> list = new List<string>();
		List<Person> persons = Managers.personManager.GetPersons(false);
		foreach (Person person in persons)
		{
			foreach (KeyValuePair<string, float> keyValuePair in person.behaviorScriptVariables)
			{
				if (!list.Contains(keyValuePair.Key))
				{
					list.Add(keyValuePair.Key);
				}
			}
		}
		list.Sort();
		return list;
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x00088C4C File Offset: 0x0008704C
	private string GetAllPersonVariableValues(string variableName)
	{
		string text = string.Empty;
		List<Person> persons = Managers.personManager.GetPersons(true);
		foreach (Person person in persons)
		{
			float num = 0f;
			foreach (KeyValuePair<string, float> keyValuePair in person.behaviorScriptVariables)
			{
				if (keyValuePair.Key == variableName)
				{
					num = keyValuePair.Value;
					break;
				}
			}
			if (text != string.Empty)
			{
				text += Environment.NewLine;
			}
			text = text + person.screenName.ToUpper() + ": " + num.ToString();
		}
		return text;
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x00088D5C File Offset: 0x0008715C
	public Person GetPersonVariablesRelevantPerson(ThingPart thingPart)
	{
		Person person = Managers.personManager.GetPersonThisObjectIsOf(thingPart.gameObject);
		if (person == null)
		{
			GameObject personHeadClosestToPosition = Managers.personManager.GetPersonHeadClosestToPosition(thingPart.transform.position, null);
			if (personHeadClosestToPosition != null)
			{
				person = Managers.personManager.GetPersonThisObjectIsOf(personHeadClosestToPosition);
			}
		}
		return person;
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x00088DB8 File Offset: 0x000871B8
	private string GetVariablesString(Dictionary<string, float> variables)
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, float> keyValuePair in variables)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				keyValuePair.Key,
				": ",
				keyValuePair.Value.ToString(),
				Environment.NewLine
			});
		}
		text = text.ToUpper();
		return text;
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x00088E5C File Offset: 0x0008725C
	public void ResetAllThingBehaviorScriptVariables()
	{
		Component[] allThings = Managers.thingManager.GetAllThings();
		foreach (Thing thing in allThings)
		{
			thing.behaviorScriptVariables = new Dictionary<string, float>();
		}
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x00088EA0 File Offset: 0x000872A0
	public void ResetArea()
	{
		Managers.temporarilyDestroyedThingsManager.RestoreAll();
		Managers.personManager.ourPerson.lastHandledInformedOthersOfStatesTime = Time.time;
		Managers.areaManager.behaviorScriptVariables = new Dictionary<string, float>();
		Component[] allThings = Managers.thingManager.GetAllThings();
		foreach (Thing thing in allThings)
		{
			thing.behaviorScriptVariables = new Dictionary<string, float>();
			thing.ResetStates();
			if (thing.IsPlacement() && thing.subThingMasterPart == null)
			{
				thing.RestoreOriginalPlacement(true);
			}
		}
		if (Managers.personManager.ourPerson.isMasterClient)
		{
			base.CancelInvoke("InvokableTriggerVariableChangeToThings");
			base.Invoke("InvokableTriggerVariableChangeToThings", 1f);
		}
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x00088F68 File Offset: 0x00087368
	public void ResetAllPersonVariablesInArea()
	{
		List<Person> currentAreaPersons = Managers.personManager.GetCurrentAreaPersons();
		foreach (Person person in currentAreaPersons)
		{
			person.behaviorScriptVariables = new Dictionary<string, float>();
		}
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x00088FD0 File Offset: 0x000873D0
	private void InvokableTriggerVariableChangeToThings()
	{
		this.TriggerVariableChangeToThings(null, null);
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x00088FDA File Offset: 0x000873DA
	public string NormalizeVariableName(string s)
	{
		s = s.Trim();
		s = s.ToLower();
		s = s.Replace(";", string.Empty);
		s = s.Replace("|", string.Empty);
		return s;
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x00089014 File Offset: 0x00087414
	public BehaviorScriptVariableScope GetVariableScope(string s)
	{
		BehaviorScriptVariableScope behaviorScriptVariableScope = BehaviorScriptVariableScope.None;
		bool flag = Validator.ContainsOnly(s, "abcdefghijklmnopqrstuvwxyz0123456789_.") && char.IsLetter(s[0]) && Array.IndexOf<string>(Managers.behaviorScriptManager.disallowedVariableNames, s) == -1;
		if (flag)
		{
			if (s.Contains("."))
			{
				string[] array = Misc.Split(s, ".", StringSplitOptions.None);
				if (array.Length == 2)
				{
					string text = array[0];
					if (text != null)
					{
						if (!(text == "area"))
						{
							if (text == "person")
							{
								behaviorScriptVariableScope = BehaviorScriptVariableScope.Person;
							}
						}
						else
						{
							behaviorScriptVariableScope = BehaviorScriptVariableScope.Area;
						}
					}
				}
			}
			else
			{
				behaviorScriptVariableScope = BehaviorScriptVariableScope.Thing;
			}
		}
		return behaviorScriptVariableScope;
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x000890CC File Offset: 0x000874CC
	public void DestroyThing(Thing rootThing, ThingDestruction destruction)
	{
		if (rootThing != null && rootThing.name != Universe.objectNameIfAlreadyDestroyed)
		{
			bool flag = rootThing.IsPlacement();
			bool flag2 = rootThing.isHeldAsHoldable || rootThing.isThrownOrEmitted;
			if (flag || flag2)
			{
				if (destruction.burst)
				{
					Effects.BreakIntoPieces(rootThing, destruction);
					rootThing.name = Universe.objectNameIfAlreadyDestroyed;
				}
				if (flag)
				{
					Managers.personManager.DoRedundantlyInformAboutThingDestruction(rootThing.placementId, destruction);
					Managers.temporarilyDestroyedThingsManager.AddPlacement(rootThing, destruction.restoreInSeconds);
				}
				else if (flag2)
				{
					Misc.Destroy(rootThing.gameObject);
					if (rootThing.isHeldAsHoldableByOurPerson)
					{
						Managers.personManager.CachePhotonHeldThingsData();
					}
				}
			}
		}
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x00089194 File Offset: 0x00087594
	public void DestroyOtherThingsInRadius(ThingPart originThingPart, OtherThingDestruction otherDestruction)
	{
		Thing myRootThing = originThingPart.GetMyRootThing();
		Collider[] array = Physics.OverlapSphere(originThingPart.transform.position, otherDestruction.radius);
		foreach (Collider collider in array)
		{
			ThingPart component = collider.gameObject.GetComponent<ThingPart>();
			if (component != null)
			{
				Thing myRootThing2 = component.GetMyRootThing();
				if (myRootThing2 != myRootThing && myRootThing2.biggestSize <= otherDestruction.maxThingSize)
				{
					this.DestroyThing(myRootThing2, otherDestruction.thingDestruction);
				}
			}
		}
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x0008922E File Offset: 0x0008762E
	public static void TestReplaceVariablesWithValues(Thing thing, ThingPart thingPart)
	{
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x00089230 File Offset: 0x00087630
	public static string ReplaceVariablesWithValues(Thing thing, ThingPart thingPart, string expression, Person requiredRelevantPerson = null, bool doDebug = false)
	{
		string text = string.Empty;
		expression = BehaviorScriptManager.AddSpacesToScriptVariableExpression(expression);
		Person person = null;
		string[] array = Misc.Split(expression, " ", StringSplitOptions.RemoveEmptyEntries);
		if (doDebug)
		{
			Debug.Log("parts = " + string.Join("  ", array));
		}
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i];
			text2 = text2.Trim();
			if (char.IsLetter(text2[0]))
			{
				string text3 = ((i + 1 >= array.Length) ? null : array[i + 1]);
				bool flag = text3 == "(";
				if (!flag)
				{
					if (Array.IndexOf<string>(Managers.behaviorScriptManager.disallowedVariableNames, text2) == -1)
					{
						bool flag2 = false;
						foreach (KeyValuePair<string, float> keyValuePair in thing.behaviorScriptVariables)
						{
							if (text2 == keyValuePair.Key)
							{
								text2 = keyValuePair.Value.ToString();
								flag2 = true;
								break;
							}
						}
						if (!flag2 && text2.StartsWith("area."))
						{
							foreach (KeyValuePair<string, float> keyValuePair2 in Managers.areaManager.behaviorScriptVariables)
							{
								if (text2 == keyValuePair2.Key)
								{
									text2 = keyValuePair2.Value.ToString();
									flag2 = true;
									break;
								}
							}
						}
						if (!flag2 && text2.StartsWith("person."))
						{
							if (person == null)
							{
								person = Managers.behaviorScriptManager.GetPersonVariablesRelevantPerson(thingPart);
							}
							if (person != null && (requiredRelevantPerson == null || person == requiredRelevantPerson))
							{
								foreach (KeyValuePair<string, float> keyValuePair3 in person.behaviorScriptVariables)
								{
									if (text2 == keyValuePair3.Key)
									{
										text2 = keyValuePair3.Value.ToString();
										flag2 = true;
										break;
									}
								}
							}
						}
						if (!flag2)
						{
							text2 = "0";
						}
					}
				}
			}
			text = text + text2 + " ";
		}
		text = text.Replace(" + -", " -");
		text = text.Replace(" (", "(");
		text = text.Replace("( ", "(");
		text = text.Replace(" )", ")");
		text = text.Replace(") ", ")");
		text = text.Replace("  ", " ");
		text = text.Trim();
		text = BehaviorScriptManager.ModifyTwoParamFunctionSyntax(text, doDebug);
		return text;
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x00089570 File Offset: 0x00087970
	private static string ModifyTwoParamFunctionSyntax(string expression, bool doDebug = false)
	{
		string[] array = Misc.Split(expression, " ", StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i];
			text2 = text2.Trim();
			bool flag = text2.Contains("(");
			if (flag)
			{
				int num = i + 1;
				bool flag2 = num < array.Length && array[num].Contains(")");
				if (flag2)
				{
					string text3 = text2 + " " + array[i + 1];
					text3 = text3.Replace("(", ";");
					text3 = text3.Replace(")", string.Empty);
					text3 = text3.Replace(" ", ";");
					string[] array2 = Misc.Split(text3, ";", StringSplitOptions.RemoveEmptyEntries);
					bool flag3 = array2.Length == 3;
					if (flag3)
					{
						text2 = string.Concat(new string[]
						{
							"(",
							array2[1],
							")",
							array2[0],
							"(",
							array2[2],
							")"
						});
						i++;
					}
				}
			}
			text = text + text2 + " ";
		}
		return text.Trim();
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x000896B0 File Offset: 0x00087AB0
	private static string AddSpacesToScriptVariableExpression(string expression)
	{
		string text = string.Empty;
		bool flag = false;
		for (int i = 0; i < expression.Length; i++)
		{
			string text2 = expression[i].ToString();
			bool flag2 = "abcdefghijklmnopqrstuvwxyz0123456789_.".Contains(text2);
			if (flag != flag2)
			{
				text += " ";
			}
			text += text2;
			flag = flag2;
		}
		text = text.Replace("(", "( ");
		text = text.Replace(")", ") ");
		text = text.Replace("  ", " ");
		return text.Trim();
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x0008975C File Offset: 0x00087B5C
	public string RemovePrivacyRelevantFromWebTellData(string data)
	{
		List<Person> persons = Managers.personManager.GetPersons(false);
		foreach (Person person in persons)
		{
			string text = person.screenName.ToLower();
			if (text.Length >= 3)
			{
				data = data.Replace(person.screenName, "anonymized");
			}
			else
			{
				data = "anonymized";
			}
		}
		if ((Managers.areaManager.isPrivate || Managers.areaManager.isExcluded) && Managers.areaManager.currentAreaName.ToLower() == data)
		{
			data = "anonymized";
		}
		return data;
	}

	// Token: 0x04001018 RID: 4120
	public string[] comparators = new string[0];

	// Token: 0x04001019 RID: 4121
	public string[] disallowedVariableNames = new string[0];

	// Token: 0x0400101A RID: 4122
	public string[] functionNames = new string[0];

	// Token: 0x0400101B RID: 4123
	public string[] functionNamesWithTwoParams = new string[0];

	// Token: 0x0400101C RID: 4124
	public const int maxVariableCalculationsPerFrame = 50;

	// Token: 0x0400101D RID: 4125
	private int variableCalculationsThisFrame;

	// Token: 0x0400101E RID: 4126
	private int variableCalculationsLimitHit;

	// Token: 0x0400101F RID: 4127
	private float timeToClearVariableCalculationsLimitHit = -1f;

	// Token: 0x04001020 RID: 4128
	public const string validVariableNameChars = "abcdefghijklmnopqrstuvwxyz0123456789_.";

	// Token: 0x04001021 RID: 4129
	public string[] placeholdersReturningStrings = new string[0];

	// Token: 0x04001022 RID: 4130
	private static bool didExpressionTest;

	// Token: 0x04001023 RID: 4131
	private WindowsVoice voice;

	// Token: 0x04001024 RID: 4132
	private int tellCountForThisUpdate;

	// Token: 0x04001025 RID: 4133
	private const int maxTellCountPerUpdateToAvoidInfiniteLoops = 250;
}
