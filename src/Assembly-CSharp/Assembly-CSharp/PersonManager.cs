using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x020001F6 RID: 502
public class PersonManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000221 RID: 545
	// (get) Token: 0x060011BA RID: 4538 RVA: 0x00096064 File Offset: 0x00094464
	// (set) Token: 0x060011BB RID: 4539 RVA: 0x0009606C File Offset: 0x0009446C
	public ManagerStatus status { get; private set; }

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x060011BC RID: 4540 RVA: 0x00096075 File Offset: 0x00094475
	// (set) Token: 0x060011BD RID: 4541 RVA: 0x0009607D File Offset: 0x0009447D
	public string failMessage { get; private set; }

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x060011BE RID: 4542 RVA: 0x00096086 File Offset: 0x00094486
	// (set) Token: 0x060011BF RID: 4543 RVA: 0x0009608E File Offset: 0x0009448E
	public GameObject People { get; private set; }

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00096097 File Offset: 0x00094497
	// (set) Token: 0x060011C1 RID: 4545 RVA: 0x0009609F File Offset: 0x0009449F
	public GameObject OurPersonRig { get; private set; }

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x060011C2 RID: 4546 RVA: 0x000960A8 File Offset: 0x000944A8
	// (set) Token: 0x060011C3 RID: 4547 RVA: 0x000960B0 File Offset: 0x000944B0
	public Person ourPerson { get; private set; }

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x060011C4 RID: 4548 RVA: 0x000960B9 File Offset: 0x000944B9
	// (set) Token: 0x060011C5 RID: 4549 RVA: 0x000960C1 File Offset: 0x000944C1
	public List<PersonInfo> recentPersons { get; private set; }

	// Token: 0x060011C6 RID: 4550 RVA: 0x000960CC File Offset: 0x000944CC
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.PeopleByUserId = new Dictionary<string, GameObject>();
		this.recentPersons = new List<PersonInfo>();
		this.People = new GameObject("People");
		this.OurPersonRig = Managers.treeManager.GetObject("/OurPersonRig");
		this.ourPerson = this.OurPersonRig.GetComponent<Person>();
		this.ourPerson.photonPlayer = PhotonNetwork.player;
		this.photonView = base.GetComponent<PhotonView>();
		if (this.photonView == null)
		{
			Log.Error("Could not find PhotonView for person manager");
		}
		this.horatioId = "577390702da36d2d18b870f7";
		this.philippId = "5773b5232da36d2d18b870fb";
		base.InvokeRepeating("DoAudioReachAndVisibilityChecks", 0.5f, 0.5f);
		this.status = ManagerStatus.Started;
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x00096198 File Offset: 0x00094598
	public void InitializeOurPerson(StartInfo startInfo)
	{
		Log.Info("InitializeOurPerson: " + startInfo.screenName, false);
		this.ourPerson.photonPlayer.customProperties.Clear();
		this.ourPerson.photonPlayer.NickName = startInfo.screenName;
		this.ourPerson.userId = startInfo.personId;
		this.ourPerson.statusText = startInfo.statusText;
		this.ourPerson.isFindable = startInfo.isFindable;
		this.ourPerson.homeAreaId = startInfo.homeAreaId;
		this.ourPerson.ageInDays = startInfo.age;
		this.ourPerson.ageInSecs = startInfo.ageSecs;
		this.ourPerson.isHardBanned = startInfo.isHardBanned;
		this.ourPerson.isSoftBanned = startInfo.isSoftBanned;
		this.ourPerson.showFlagWarning = startInfo.showFlagWarning;
		this.ourPerson.flagTags = startInfo.flagTags;
		this.ourPerson.areaCount = startInfo.areaCount;
		this.ourPerson.thingTagCount = startInfo.thingTagCount;
		this.ourPerson.allThingsClonable = startInfo.allThingsClonable;
		this.ourPerson.hasEditTools = startInfo.hasEditTools;
		this.ourPerson.hasEditToolsPermanently = startInfo.hasEditToolsPermanently;
		this.ourPerson.editToolsExpiryDate = startInfo.editToolsExpiryDate;
		this.ourPerson.isInEditToolsTrial = startInfo.isInEditToolsTrial;
		this.ourPerson.wasEditToolsTrialEverActivated = startInfo.wasEditToolsTrialEverActivated;
		this.ourPerson.timesEditToolsPurchased = startInfo.timesEditToolsPurchased;
		SearchWords.InitializeCustomWordsFromString(startInfo.customSearchWords);
		this.attachmentsData = startInfo.attachmentsData;
		this.ourPerson.ConstructAttachments(this.attachmentsData);
		this.CachePhotonAttachmentData();
		Color? handColor = startInfo.handColor;
		if (handColor != null)
		{
			Person ourPerson = this.ourPerson;
			Color? handColor2 = startInfo.handColor;
			ourPerson.SetHandsColor(handColor2.Value);
			Color? handColor3 = startInfo.handColor;
			this.CachePhotonHandColor(handColor3.Value);
		}
		Log.Info(string.Concat(new string[]
		{
			"OurPerson ",
			this.ourPerson.photonPlayer.NickName,
			" [",
			this.ourPerson.userId,
			"] Initialized.  Home area : ",
			this.ourPerson.homeAreaId
		}), false);
		this.INFO_Our_Id = this.ourPerson.userId;
		this.INFO_Our_HomeAreaId = this.ourPerson.homeAreaId;
		if (this.ourPerson.isSoftBanned || this.ourPerson.isHardBanned)
		{
			Universe.transmitFromMicrophone = false;
		}
		this.ourPerson.Initialized = true;
		this.LoadOurLegAttachmentPointPositions();
		this.ourPerson.AlertOthersOfMyBirthIfAppropriate();
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00096454 File Offset: 0x00094854
	public void OnJoinedRoom()
	{
		int[] newComponentViewIds = this.GetNewComponentViewIds();
		this.OurPersonRig.GetComponent<Person>().SetComponentNetworkViewIds(newComponentViewIds);
		this.InstantiateOurPersonOnOtherClients();
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x0009647F File Offset: 0x0009487F
	public void OnLeftRoom()
	{
		this.DeinstantiateAllRemotePeople();
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x00096487 File Offset: 0x00094887
	public void OnPhotonPlayerConnected(PhotonPlayer otherPlayer)
	{
		Log.Info(otherPlayer.NickName + " JOINED. UserId:" + otherPlayer.userId, false);
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x000964A5 File Offset: 0x000948A5
	public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		Log.Info(otherPlayer.NickName + "/" + otherPlayer.userId + " LEFT", false);
		this.DeinstantiateRemotePerson(otherPlayer.userId);
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x000964D4 File Offset: 0x000948D4
	public void InstantiateOurPersonOnOtherClients()
	{
		Log.Info("Instantiating ourPerson on other clients", false);
		int[] componentNetworkViewIds = this.ourPerson.GetComponentNetworkViewIds();
		this.photonView.RPC("InstantiateRemotePerson", PhotonTargets.OthersBuffered, new object[]
		{
			componentNetworkViewIds,
			this.ourPerson.photonPlayer
		});
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x00096524 File Offset: 0x00094924
	[PunRPC]
	private void InstantiateRemotePerson(int[] componentViewIds, PhotonPlayer p)
	{
		if (p.userId == this.ourPerson.userId)
		{
			Managers.errorManager.ShowCriticalHaltError("An attempt was made to add ourselves to remote players list!", true, false, true, false, false);
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.PersonPrefab, Vector3.zero, Quaternion.identity);
		gameObject.transform.SetParent(this.People.transform);
		gameObject.name = p.NickName;
		Person newPerson = gameObject.GetComponent<Person>();
		newPerson.SetComponentNetworkViewIds(componentViewIds);
		gameObject.SetActive(true);
		newPerson.InitializeFromPhotonPlayer(p);
		if (!(Managers.areaManager != null) || !(Managers.areaManager.rights.invisibility == true))
		{
			newPerson.AddNameTagIfNeeded(30f);
		}
		this.PeopleByUserId.Add(newPerson.userId, gameObject);
		this.AddToStartOfRecentPersons(newPerson);
		Managers.achievementManager.RegisterAchievement(Achievement.MetPerson);
		float? secondsSinceJoiningArea = Managers.areaManager.GetSecondsSinceJoiningArea();
		if (secondsSinceJoiningArea != null && secondsSinceJoiningArea >= 10f)
		{
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "someone arrived in area", true);
		}
		string text;
		if (this.receivedUnassignedPersonBehaviorScriptVariables.TryGetValue(newPerson.userId, out text))
		{
			newPerson.SetMyBehaviorScriptVariablesFromString(text);
			this.receivedUnassignedPersonBehaviorScriptVariables.Remove(newPerson.userId);
		}
		Managers.personManager.GetPersonInfoBasic(newPerson.userId, delegate(PersonInfoBasic personInfoBasic)
		{
			newPerson.isEditorHere = new bool?(personInfoBasic.isEditorHere);
		});
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x00096700 File Offset: 0x00094B00
	private void AddToStartOfRecentPersons(Person person)
	{
		for (int i = 0; i < this.recentPersons.Count; i++)
		{
			if (this.recentPersons[i].id == person.userId)
			{
				this.recentPersons.RemoveAt(i);
				break;
			}
		}
		if (this.recentPersons.Count >= 10000)
		{
			this.recentPersons = new List<PersonInfo>();
		}
		PersonInfo personInfo = new PersonInfo();
		personInfo.id = person.userId;
		personInfo.screenName = person.screenName;
		this.recentPersons.Insert(0, personInfo);
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x000967A8 File Offset: 0x00094BA8
	private void DeinstantiateRemotePerson(string userId)
	{
		GameObject gameObject;
		this.PeopleByUserId.TryGetValue(userId, out gameObject);
		if (gameObject != null)
		{
			global::UnityEngine.Object.Destroy(gameObject);
			this.PeopleByUserId.Remove(userId);
		}
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x000967E4 File Offset: 0x00094BE4
	private void DeinstantiateAllRemotePeople()
	{
		if (this.People == null)
		{
			return;
		}
		IEnumerator enumerator = this.People.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				global::UnityEngine.Object.Destroy(transform.gameObject);
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
		this.PeopleByUserId.Clear();
		Log.Info("All remote persons de-instantiated", false);
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x0009687C File Offset: 0x00094C7C
	public int GetOurScalePercent()
	{
		return (int)Mathf.Round(this.OurPersonRig.transform.localScale.x * 100f);
	}

	// Token: 0x060011D2 RID: 4562 RVA: 0x000968B0 File Offset: 0x00094CB0
	public float GetOurScale()
	{
		return (!(this.OurPersonRig != null)) ? 1f : this.OurPersonRig.transform.localScale.x;
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x000968F0 File Offset: 0x00094CF0
	public bool WeAreResized()
	{
		return this.GetOurScale() != 1f;
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x00096904 File Offset: 0x00094D04
	public void AddOrTimeExtendNameTagsForAllOthers(float secondsToDisplayNameTag = 30f)
	{
		Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
		foreach (Person person in componentsInChildren)
		{
			person.AddNameTagIfNeeded(secondsToDisplayNameTag);
			person.UpdateNameTagContent();
		}
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x0009694C File Offset: 0x00094D4C
	public GameObject GetPersonHeadClosestToPosition(Vector3 position, string userIdToIgnore = null)
	{
		GameObject gameObject = null;
		float num = -1f;
		if (this.ourPerson.userId != userIdToIgnore)
		{
			gameObject = this.ourPerson.Head;
			num = Vector3.Distance(position, gameObject.transform.position);
		}
		Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
		foreach (Person person in componentsInChildren)
		{
			if (userIdToIgnore == null || person.userId != userIdToIgnore)
			{
				GameObject head = person.Head;
				if (head != null)
				{
					float num2 = Vector3.Distance(position, head.transform.position);
					if (num == -1f || num2 < num)
					{
						num = num2;
						gameObject = head;
					}
				}
			}
		}
		return gameObject;
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x00096A24 File Offset: 0x00094E24
	public List<Person> GetPersons(bool sortByName = false)
	{
		List<Person> list = new List<Person>();
		list.Add(this.ourPerson);
		Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
		foreach (Person person in componentsInChildren)
		{
			list.Add(person);
		}
		if (sortByName)
		{
			list.Sort((Person a, Person b) => a.screenName.CompareTo(b.screenName));
		}
		return list;
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x00096AA4 File Offset: 0x00094EA4
	public GameObject GetPersonHeadSecondClosestToPosition(Vector3 position)
	{
		GameObject gameObject = this.GetPersonHeadClosestToPosition(position, null);
		if (gameObject != null)
		{
			Person personThisObjectIsOf = this.GetPersonThisObjectIsOf(gameObject);
			if (personThisObjectIsOf != null)
			{
				gameObject = this.GetPersonHeadClosestToPosition(position, personThisObjectIsOf.userId);
			}
		}
		return gameObject;
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x00096AEC File Offset: 0x00094EEC
	public GameObject GetHeldThingClosestToPosition(Vector3 position)
	{
		GameObject gameObject = null;
		GameObject[] array = this.GetEveryonesHands();
		array = array.OrderBy((GameObject x) => Vector3.Distance(position, x.transform.position)).ToArray<GameObject>();
		foreach (GameObject gameObject2 in array)
		{
			string text = ((!(gameObject2.name == "HandCoreLeft")) ? "Right" : "Left");
			GameObject childWithTag = Misc.GetChildWithTag(gameObject2.transform, "CurrentlyHeld" + text);
			if (childWithTag != null)
			{
				gameObject = childWithTag;
				break;
			}
		}
		return gameObject;
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x00096BA0 File Offset: 0x00094FA0
	public GameObject[] GetEveryonesHands()
	{
		int num = 0;
		int currentAreaPersonCount = this.GetCurrentAreaPersonCount();
		GameObject[] array = new GameObject[currentAreaPersonCount * 2];
		array[num++] = Misc.FindObject(this.ourPerson.gameObject, "HandCoreLeft");
		array[num++] = Misc.FindObject(this.ourPerson.gameObject, "HandCoreRight");
		IEnumerator enumerator = this.People.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				array[num++] = Misc.FindObject(transform.gameObject, "HandCoreLeft");
				array[num++] = Misc.FindObject(transform.gameObject, "HandCoreRight");
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
		return array;
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x00096C84 File Offset: 0x00095084
	public Person GetPersonById(string id)
	{
		Person person = null;
		if (this.ourPerson.userId == id)
		{
			person = this.ourPerson;
		}
		else
		{
			Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
			foreach (Person person2 in componentsInChildren)
			{
				if (person2.userId == id)
				{
					person = person2;
					break;
				}
			}
		}
		return person;
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x00096D00 File Offset: 0x00095100
	public string GetCurrentAreaPeopleNames()
	{
		int currentAreaPersonCount = this.GetCurrentAreaPersonCount();
		string[] array = new string[currentAreaPersonCount];
		int num = 0;
		array[num++] = this.ourPerson.screenName;
		Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
		foreach (Person person in componentsInChildren)
		{
			array[num++] = person.screenName;
		}
		Array.Sort<string>(array, StringComparer.InvariantCultureIgnoreCase);
		string text = string.Join(Environment.NewLine, array);
		return text.ToUpper();
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x00096D94 File Offset: 0x00095194
	public List<PersonInfo> GetCurrentAreaPersonInfos(int extraAmountForTesting = 0)
	{
		List<PersonInfo> list = new List<PersonInfo>();
		extraAmountForTesting = 0;
		for (int i = 0; i < extraAmountForTesting + 1; i++)
		{
			list.Add(new PersonInfo
			{
				id = this.ourPerson.userId,
				screenName = this.ourPerson.screenName
			});
		}
		Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
		foreach (Person person in componentsInChildren)
		{
			list.Add(new PersonInfo
			{
				id = person.userId,
				screenName = person.screenName
			});
		}
		list.Sort((PersonInfo a, PersonInfo b) => a.screenName.CompareTo(b.screenName));
		return list;
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x00096E70 File Offset: 0x00095270
	public List<Person> GetCurrentAreaPersons()
	{
		List<Person> list = new List<Person>();
		if (this.ourPerson != null)
		{
			list.Add(this.ourPerson);
		}
		Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
		foreach (Person person in componentsInChildren)
		{
			list.Add(person);
		}
		return list;
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x00096ED8 File Offset: 0x000952D8
	public bool PartMovementIsRelevantEnoughToTransmit(bool doHandleForceSending, Vector3 positionToSend, Quaternion rotationToSend, ref Vector3 lastPositionSent, ref Quaternion lastRotationSent, ref float timeOfLastForceSent)
	{
		float num = Vector3.Distance(lastPositionSent, positionToSend);
		float num2 = Quaternion.Angle(lastRotationSent, rotationToSend);
		bool flag = false;
		if (doHandleForceSending)
		{
			flag = timeOfLastForceSent == -1f || timeOfLastForceSent + 5f <= Time.time;
		}
		bool flag2 = flag || num >= 0.01f || num2 >= 1f;
		if (flag2)
		{
			lastPositionSent = positionToSend;
			lastRotationSent = rotationToSend;
			if (flag)
			{
				timeOfLastForceSent = Time.time;
			}
		}
		return flag2;
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x00096F74 File Offset: 0x00095374
	public string GetScreenNameWithDiscloser(string id, string name)
	{
		string text = name.ToLower();
		if ((text == "horatio" && id != this.horatioId) || (text == "philipp" && id != this.philippId))
		{
			name += "*";
		}
		return name;
	}

	// Token: 0x060011E0 RID: 4576 RVA: 0x00096FD8 File Offset: 0x000953D8
	public bool WeAreAppAdmin()
	{
		return this.ourPerson.userId == this.horatioId || this.ourPerson.userId == this.philippId;
	}

	// Token: 0x060011E1 RID: 4577 RVA: 0x00097010 File Offset: 0x00095410
	public void DoAdditionalSanityClearOfHandObjects(Transform handTransform)
	{
		string text = null;
		IEnumerator enumerator = handTransform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				Thing component = transform.GetComponent<Thing>();
				if (component != null)
				{
					if (!component.movableByEveryone)
					{
						text = "Thing " + transform.name;
						global::UnityEngine.Object.Destroy(transform.gameObject);
					}
				}
				else
				{
					ThingPart component2 = transform.GetComponent<ThingPart>();
					if (component2 != null)
					{
						text = "ThingPart";
						global::UnityEngine.Object.Destroy(transform.gameObject);
					}
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
		if (text != null)
		{
			Log.Debug("Doing sanity clear of unrecognized object " + text + " in hand");
		}
	}

	// Token: 0x060011E2 RID: 4578 RVA: 0x000970F4 File Offset: 0x000954F4
	public void GetPersonInfo(string personId, Action<PersonInfo> callback)
	{
		string currentAreaId = Managers.areaManager.currentAreaId;
		base.StartCoroutine(Managers.serverManager.GetPersonInfo(personId, currentAreaId, delegate(GetPersonInfo_Response response)
		{
			if (response.error == null)
			{
				callback(response.personInfo);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x00097138 File Offset: 0x00095538
	public void GetPersonInfoBasic(string personId, Action<PersonInfoBasic> callback)
	{
		string currentAreaId = Managers.areaManager.currentAreaId;
		base.StartCoroutine(Managers.serverManager.GetPersonInfoBasic(personId, currentAreaId, delegate(GetPersonInfoBasic_Response response)
		{
			if (response.error == null)
			{
				callback(response.personInfoBasic);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x0009717C File Offset: 0x0009557C
	public void GetFriendsByStrength(Action<FriendListInfoCollection, FriendListInfoCollection> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetFriendsByStrength(delegate(GetFriendsByStrength_Response response)
		{
			if (response.error == null)
			{
				callback(response.friends.online, response.friends.offline);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x000971B4 File Offset: 0x000955B4
	public void AddFriend(string personId, Action callback)
	{
		base.StartCoroutine(Managers.serverManager.AddFriend(personId, delegate(ExtendedServerResponse response)
		{
			if (response.error == null)
			{
				callback();
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x000971EC File Offset: 0x000955EC
	public void RemoveFriend(string personId, Action callback)
	{
		base.StartCoroutine(Managers.serverManager.RemoveFriend(personId, delegate(ServerResponse response)
		{
			if (response.error == null)
			{
				callback();
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x00097224 File Offset: 0x00095624
	public void IncreaseFriendshipStrength(string personId, Action callback)
	{
		base.StartCoroutine(Managers.serverManager.IncreaseFriendshipStrength(personId, delegate(ServerResponse response)
		{
			if (response.error == null)
			{
				callback();
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x0009725C File Offset: 0x0009565C
	public void GetPersonFlagStatus(string personId, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetPersonFlagStatus(personId, delegate(FlagStatus_Response response)
		{
			if (response.error == null)
			{
				callback(response.isFlagged);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x00097294 File Offset: 0x00095694
	public void TogglePersonFlag(string personId, string reason, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.TogglePersonFlag(personId, reason, delegate(FlagStatus_Response response)
		{
			if (response.error == null)
			{
				callback(response.isFlagged);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x000972D0 File Offset: 0x000956D0
	public void PingPerson(string personId, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.PingPerson(personId, Managers.areaManager.currentAreaId, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			callback(response.ok);
		}));
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x00097314 File Offset: 0x00095714
	public void RequestWelcome(Action callback)
	{
		base.StartCoroutine(Managers.serverManager.RequestWelcome(delegate(ResponseBase response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			callback();
		}));
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x0009734B File Offset: 0x0009574B
	public int GetCurrentAreaPersonCount()
	{
		return (!(this.People != null)) ? 1 : (this.People.transform.childCount + 1);
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x00097378 File Offset: 0x00095778
	public void SetCustomSearchWords(string words, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetCustomSearchWords(words, delegate(ResponseBase response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			callback(response.error == null);
		}));
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x000973B0 File Offset: 0x000957B0
	private void CachePhotonAttachmentData()
	{
		string text = this.attachmentsData.toJSON();
		this.UpdatePhotonCustomProperty(PhotonCacheKeys.Attachments, text);
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x000973D8 File Offset: 0x000957D8
	public void CachePhotonHeldThingsData()
	{
		string text = this.ourPerson.GetHeldThingsDataSet().toJSON();
		this.UpdatePhotonCustomProperty(PhotonCacheKeys.HeldThings, text);
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x00097404 File Offset: 0x00095804
	private void CachePhotonHandColor(Color color)
	{
		string text = JsonUtility.ToJson(color);
		this.UpdatePhotonCustomProperty(PhotonCacheKeys.HandColour, text);
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x0009742C File Offset: 0x0009582C
	public void CachePhotonRigLocation()
	{
		LocationData locationData = new LocationData(this.ourPerson.transform);
		string text = JsonUtility.ToJson(locationData);
		this.UpdatePhotonCustomProperty(PhotonCacheKeys.RigLocation, text);
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x0009745D File Offset: 0x0009585D
	public void ResetAndCachePhotonRigScale()
	{
		this.ApplyAndCachePhotonRigScale(1f, false);
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x0009746B File Offset: 0x0009586B
	public void CachePhotonIsInvisibleWhereAllowed(bool state)
	{
		this.ourPerson.isInvisibleWhereAllowed = state;
		this.UpdatePhotonCustomProperty(PhotonCacheKeys.IsInvisibleWhereAllowed, state);
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x00097488 File Offset: 0x00095888
	private void CachePhotonRidingBeacon()
	{
		if (this.ourPerson.ridingBeacon != null)
		{
			this.UpdatePhotonCustomProperty(PhotonCacheKeys.RidingBeacon, this.ourPerson.ridingBeaconCache.GetJson());
		}
		else
		{
			this.UpdatePhotonCustomProperty(PhotonCacheKeys.RidingBeacon, string.Empty);
		}
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x000974DC File Offset: 0x000958DC
	public void ApplyAndCachePhotonRigScale(float scale, bool omitSound = false)
	{
		if (this.ourPerson.Head == null)
		{
			return;
		}
		this.DropOurHoldable(TopographyId.Left);
		this.DropOurHoldable(TopographyId.Right);
		this.OurPersonRig.transform.localScale = Misc.GetUniformVector3(scale);
		this.UpdatePhotonCustomProperty(PhotonCacheKeys.RigScale, this.OurPersonRig.transform.localScale.x);
		float num = scale * 100f;
		Camera component = this.ourPerson.Head.GetComponent<Camera>();
		component.nearClipPlane = ((num >= 10f) ? 0.01f : 0.001f);
		Vector3? lastTeleportHitPoint = Our.lastTeleportHitPoint;
		if (lastTeleportHitPoint != null)
		{
			Transform transform = this.ourPerson.Head.transform;
			Vector3 position = transform.position;
			Vector3? lastTeleportHitPoint2 = Our.lastTeleportHitPoint;
			Vector3 value = lastTeleportHitPoint2.Value;
			value.y += scale * 1.75f;
			transform.position = value;
			Vector3 vector = new Vector3(transform.position.x - position.x, transform.position.y - position.y, transform.position.z - position.z);
			this.OurPersonRig.transform.position += vector;
			this.CachePhotonRigLocation();
		}
		if (!omitSound)
		{
			Managers.soundManager.Play("teleport", this.ourPerson.Head.transform, 0.4f, true, false);
		}
	}

	// Token: 0x060011F6 RID: 4598 RVA: 0x0009767B File Offset: 0x00095A7B
	private void DoAudioReachAndVisibilityChecks()
	{
		this.SetOtherPeoplesVisibility();
		this.SetOtherPeoplesAudioDependentOnOurAndTheirScale();
	}

	// Token: 0x060011F7 RID: 4599 RVA: 0x0009768C File Offset: 0x00095A8C
	private void SetOtherPeoplesVisibility()
	{
		if (Managers.areaManager != null)
		{
			bool flag = Managers.areaManager.rights.invisibility == true;
			Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
			foreach (Person person in componentsInChildren)
			{
				string text = LayerMask.LayerToName(person.gameObject.layer);
				if (person.isInvisibleWhereAllowed && flag && text != "InvisibleToOurPerson")
				{
					Misc.SetAllObjectLayers(person.gameObject, "InvisibleToOurPerson");
				}
				else if ((!person.isInvisibleWhereAllowed || !flag) && text == "InvisibleToOurPerson")
				{
					Misc.SetAllObjectLayers(person.gameObject, "Default");
				}
			}
		}
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x0009777C File Offset: 0x00095B7C
	private void SetOtherPeoplesAudioDependentOnOurAndTheirScale()
	{
		float x = this.OurPersonRig.transform.localScale.x;
		Component[] componentsInChildren = this.People.GetComponentsInChildren<AudioSource>();
		foreach (AudioSource audioSource in componentsInChildren)
		{
			if (audioSource.gameObject.name == "HeadCore")
			{
				float num = 1f;
				if (audioSource.transform.parent != null)
				{
					num = audioSource.transform.parent.localScale.x;
				}
				if (num >= 2.5f)
				{
					if (audioSource.rolloffMode != AudioRolloffMode.Logarithmic || audioSource.minDistance == 2f)
					{
						audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
						audioSource.minDistance = 2f * num * 0.5f;
						audioSource.maxDistance = 12f * num * 0.5f;
					}
				}
				else if (x >= 2.5f)
				{
					if (audioSource.rolloffMode != AudioRolloffMode.Logarithmic || audioSource.minDistance != 2f)
					{
						audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
						audioSource.minDistance = 2f;
						audioSource.maxDistance = 12f;
					}
				}
				else if (audioSource.rolloffMode != AudioRolloffMode.Custom)
				{
					audioSource.rolloffMode = AudioRolloffMode.Custom;
					audioSource.minDistance = 2f;
					audioSource.maxDistance = 12f;
				}
			}
		}
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x000978F4 File Offset: 0x00095CF4
	private void DropOurHoldable(TopographyId handTopographyId)
	{
		GameObject handByTopographyId = this.ourPerson.GetHandByTopographyId(handTopographyId);
		if (handByTopographyId != null)
		{
			Hand component = handByTopographyId.GetComponent<Hand>();
			if (component != null && component.handDot != null)
			{
				HandDot component2 = component.handDot.GetComponent<HandDot>();
				if (component2 != null && component2.holdableInHand != null)
				{
					Managers.personManager.DoThrowThing(component2.holdableInHand);
					component2.holdableInHand = null;
					component2.currentlyHeldObject = null;
				}
			}
		}
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x00097985 File Offset: 0x00095D85
	public void CachePhotonTestItem()
	{
		this.UpdatePhotonCustomProperty(PhotonCacheKeys.Test, "x");
	}

	// Token: 0x060011FB RID: 4603 RVA: 0x00097998 File Offset: 0x00095D98
	private void UpdatePhotonCustomProperty(string name, string value)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(name, value);
		this.ourPerson.photonPlayer.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x000979C8 File Offset: 0x00095DC8
	private void UpdatePhotonCustomProperty(string name, float value)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(name, value);
		this.ourPerson.photonPlayer.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x000979FC File Offset: 0x00095DFC
	private void UpdatePhotonCustomProperty(string name, bool value)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(name, value);
		this.ourPerson.photonPlayer.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x00097A30 File Offset: 0x00095E30
	private int[] GetNewComponentViewIds()
	{
		return new int[]
		{
			PhotonNetwork.AllocateViewID(),
			PhotonNetwork.AllocateViewID(),
			PhotonNetwork.AllocateViewID(),
			PhotonNetwork.AllocateViewID(),
			PhotonNetwork.AllocateViewID(),
			PhotonNetwork.AllocateViewID()
		};
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x00097A78 File Offset: 0x00095E78
	public TopographyId GetTopographyIdOfHand(GameObject hand)
	{
		TopographyId topographyId = TopographyId.None;
		if (hand.name == "HandCoreLeft")
		{
			topographyId = TopographyId.Left;
		}
		else if (hand.name == "HandCoreRight")
		{
			topographyId = TopographyId.Right;
		}
		else
		{
			Log.Error("Hand name is " + hand.name + " when we were expecting HandCoreLeft or HandCoreRight");
		}
		return topographyId;
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x00097ADC File Offset: 0x00095EDC
	public Person GetPersonThisObjectIsOf(GameObject anyTypeOfObject)
	{
		if (anyTypeOfObject.transform.parent == null)
		{
			return null;
		}
		Person component = anyTypeOfObject.transform.parent.GetComponent<Person>();
		if (component != null)
		{
			return component;
		}
		return this.GetPersonThisObjectIsOf(anyTypeOfObject.transform.parent.gameObject);
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x00097B38 File Offset: 0x00095F38
	public bool GetIsThisObjectOfOurPerson(GameObject anyTypeOfObject, bool includeStickiesOnUs = false)
	{
		bool flag = false;
		if (anyTypeOfObject != null)
		{
			Person personThisObjectIsOf = this.GetPersonThisObjectIsOf(anyTypeOfObject);
			if (personThisObjectIsOf != null && this.ourPerson != null && personThisObjectIsOf.userId == this.ourPerson.userId)
			{
				if (includeStickiesOnUs)
				{
					flag = true;
				}
				else
				{
					bool flag2 = false;
					if (anyTypeOfObject.CompareTag("ThingPart") && anyTypeOfObject.transform.parent != null)
					{
						Thing component = anyTypeOfObject.transform.parent.GetComponent<Thing>();
						if (component != null)
						{
							Thing myRootThing = component.GetMyRootThing();
							flag2 = myRootThing != null && myRootThing.isThrownOrEmitted;
						}
					}
					flag = !flag2;
				}
			}
		}
		return flag;
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x00097C0C File Offset: 0x0009600C
	public void DoChangeName(string newName)
	{
		string oldName = this.ourPerson.photonPlayer.NickName;
		this.ourPerson.photonPlayer.NickName = newName;
		base.StartCoroutine(Managers.serverManager.UpdatePersonalSetting("screenName", newName, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
				Managers.dialogManager.ShowInfo("oops, this name cannot be used", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				this.ourPerson.photonPlayer.NickName = oldName;
			}
		}));
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x00097C70 File Offset: 0x00096070
	public void DoChangeStatusText(string statusText)
	{
		this.ourPerson.statusText = statusText;
		base.StartCoroutine(Managers.serverManager.UpdatePersonalSetting("statusText", statusText, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x00097CC0 File Offset: 0x000960C0
	public void DoSetIsFindable(bool isFindable)
	{
		this.ourPerson.isFindable = isFindable;
		base.StartCoroutine(Managers.serverManager.UpdatePersonalSetting("isFindable", isFindable.ToString(), delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x00097D1C File Offset: 0x0009611C
	public void DoSetAllThingsClonable(bool allThingsClonable)
	{
		this.ourPerson.allThingsClonable = allThingsClonable;
		base.StartCoroutine(Managers.serverManager.UpdatePersonalSetting("allThingsClonable", allThingsClonable.ToString(), delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x00097D78 File Offset: 0x00096178
	public void DoAttachThing(GameObject attachmentPoint, GameObject thing, bool snapIfMemorized = false)
	{
		AttachmentPointId id = attachmentPoint.GetComponent<AttachmentPoint>().id;
		Thing component = thing.GetComponent<Thing>();
		bool flag = component.replacesHandsWhenAttached && Managers.personManager && Managers.personManager.ourPerson && Managers.personManager.ourPerson.ageInSecs <= 7200;
		if (flag)
		{
			Managers.soundManager.Play("no", null, 0.5f, false, false);
			global::UnityEngine.Object.Destroy(thing);
			return;
		}
		string thingId = component.thingId;
		component.SetInvisibleToOurPerson(id == AttachmentPointId.Head || component.invisibleToUsWhenAttached);
		Component[] componentsInChildren = thing.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.invisibleToUsWhenAttached)
			{
				thingPart.SetInvisibleToOurPerson(true);
			}
		}
		attachmentPoint.GetComponent<AttachmentPoint>().AttachThing(thing);
		if (snapIfMemorized)
		{
			Our.attachmentPointsMemory.SnapAttachmentIfMemorized(id, thing);
		}
		AttachmentData attachmentData = new AttachmentData(thingId, thing.transform.localPosition, thing.transform.localEulerAngles);
		this.attachmentsData.UpdateWithNewData(id, attachmentData);
		this.CachePhotonAttachmentData();
		component.OnAttachedInMeMode();
		this.ourPerson.photonView.RPC("DoAttachThing_Remote", PhotonTargets.Others, new object[]
		{
			id,
			thingId,
			thing.transform.localPosition,
			thing.transform.localRotation
		});
		base.StartCoroutine(Managers.serverManager.UpdateAttachment(id, attachmentData, delegate(ServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
		}));
		this.CachePhotonHeldThingsData();
		Managers.achievementManager.RegisterAchievement(Achievement.AddedBodyAttachment);
		base.Invoke("UpdateAttachmentPointSpheres", 0.05f);
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00097F60 File Offset: 0x00096360
	public void DoRemoveAttachedThing(GameObject attachmentPointObject)
	{
		AttachmentPoint component = attachmentPointObject.GetComponent<AttachmentPoint>();
		if (component != null)
		{
			AttachmentPointId id = component.id;
			this.ourPerson.DoRemoveAttachedThing_LocalOrRemote(id);
			AttachmentData attachmentData = null;
			this.attachmentsData.UpdateWithNewData(id, attachmentData);
			this.CachePhotonAttachmentData();
			this.ourPerson.photonView.RPC("DoRemoveAttachedThing_LocalOrRemote", PhotonTargets.Others, new object[] { id });
			base.StartCoroutine(Managers.serverManager.UpdateAttachment(id, attachmentData, delegate(ServerResponse response)
			{
				if (response.error != null)
				{
					Log.Error(response.error);
				}
			}));
			base.Invoke("UpdateAttachmentPointSpheres", 0.05f);
		}
		else
		{
			Log.Debug("DoRemoveAttachedThing didn't find attachmentPoint " + attachmentPointObject.name);
		}
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x00098027 File Offset: 0x00096427
	private void UpdateAttachmentPointSpheres()
	{
		Our.UpdateAttachmentPointSpheres();
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x00098030 File Offset: 0x00096430
	public void DoHoldThing(GameObject thing, GameObject sourceThing)
	{
		GameObject gameObject = thing.transform.parent.gameObject;
		TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(gameObject);
		Managers.thingManager.MakeDeepThingClone(sourceThing, thing, true, false, false);
		Thing component = thing.GetComponent<Thing>();
		component.inWorldSourceObject = sourceThing;
		component.OnHold(true);
		this.ourPerson.photonView.RPC("DoHoldThing_Remote", PhotonTargets.Others, new object[]
		{
			topographyIdOfHand,
			component.thingId,
			thing.transform.localPosition,
			thing.transform.localRotation,
			Our.mode
		});
		this.CachePhotonHeldThingsData();
		Managers.thingManager.heldThingsRegistrar.RegisterHold(gameObject.GetComponent<Hand>(), false);
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x000980F8 File Offset: 0x000964F8
	public void DoChangeHeldThingPositionRotation(Thing thing, bool wasAutoAdjust = false)
	{
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = thing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoChangeHeldThingPositionRotation_Remote", PhotonTargets.Others, new object[]
		{
			thingSpecifierType,
			specifierId,
			thing.transform.localPosition,
			thing.transform.localRotation
		});
		this.CachePhotonHeldThingsData();
		if (!wasAutoAdjust)
		{
			GameObject gameObject = thing.transform.parent.gameObject;
			Managers.thingManager.heldThingsRegistrar.RegisterHold(gameObject.GetComponent<Hand>(), true);
		}
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x00098194 File Offset: 0x00096594
	public void DoEditHold(GameObject hand, GameObject thing)
	{
		TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(hand);
		Thing component = thing.GetComponent<Thing>();
		component.MemorizePositionAndRotationForUndo();
		string placementId = component.placementId;
		thing.transform.parent = hand.transform;
		component.OnEditHold(true);
		this.ourPerson.photonView.RPC("DoEditHold_Remote", PhotonTargets.Others, new object[]
		{
			placementId,
			topographyIdOfHand,
			component.thingId,
			thing.transform.localPosition,
			thing.transform.localRotation
		});
		this.CachePhotonHeldThingsData();
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x00098234 File Offset: 0x00096634
	public void DoThrowThing(GameObject thing)
	{
		if (thing.transform.parent != null)
		{
			GameObject gameObject = thing.transform.parent.gameObject;
			TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(gameObject);
			Hand component = gameObject.GetComponent<Hand>();
			if (component != null)
			{
				Vector3 vector = Vector3.zero;
				if (component.controller != null)
				{
					vector = component.controller.velocity;
				}
				vector = this.OurPersonRig.transform.rotation * vector;
				Vector3 vector2 = Vector3.zero;
				if (component.controller != null)
				{
					vector2 = component.controller.angularVelocity;
				}
				vector2 = this.OurPersonRig.transform.rotation * vector2;
				Thing component2 = thing.GetComponent<Thing>();
				component2.TriggerEventAsStateAuthority(StateListener.EventType.OnLetGo, string.Empty);
				if (!Misc.IsDestroyed(thing))
				{
					component2.ThrowMe(vector, vector2, null);
					this.ourPerson.photonView.RPC("DoThrowThing_Remote", PhotonTargets.Others, new object[]
					{
						component2.thrownId,
						topographyIdOfHand,
						component2.thingId,
						thing.transform.position,
						thing.transform.rotation,
						vector,
						vector2
					});
				}
			}
			else
			{
				Misc.Destroy(thing);
			}
			this.CachePhotonHeldThingsData();
		}
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x000983A4 File Offset: 0x000967A4
	public void DoPlaceThingFromHand(GameObject hand, GameObject thingOriginal)
	{
		Thing component = thingOriginal.GetComponent<Thing>();
		bool flag = !string.IsNullOrEmpty(component.placementId);
		string text;
		if (flag || !Managers.thingManager.GetPlacementsReachedLimit(out text, null))
		{
			GameObject gameObject;
			Thing thing;
			if (flag)
			{
				Managers.areaManager.UpdateThingPlacement(thingOriginal);
				gameObject = thingOriginal;
				thing = component;
			}
			else
			{
				Managers.areaManager.SaveThingPlacement(thingOriginal);
				gameObject = global::UnityEngine.Object.Instantiate<GameObject>(thingOriginal, thingOriginal.transform.position, thingOriginal.transform.rotation);
				thing = gameObject.GetComponent<Thing>();
				thing.placementId = component.placementId;
				Managers.thingManager.MakeDeepThingClone(thingOriginal, gameObject, true, true, false);
				ThingDialog thingDialogIfAvailable = Managers.dialogManager.GetThingDialogIfAvailable();
				if (thingDialogIfAvailable != null && thingDialogIfAvailable.thingObject == thingOriginal)
				{
					thingDialogIfAvailable.thingObject = gameObject;
					if (thingDialogIfAvailable.thing != null)
					{
						thingDialogIfAvailable.thing = thing;
					}
				}
				Misc.Destroy(thingOriginal);
			}
			TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(hand);
			string placementId = thing.placementId;
			gameObject.transform.parent = Managers.thingManager.placements.transform;
			thing.OnPlacedFromHand(true, flag);
			this.ourPerson.photonView.RPC("DoPlaceThingFromHand_Remote", PhotonTargets.Others, new object[]
			{
				placementId,
				topographyIdOfHand,
				thing.thingId,
				gameObject.transform.position,
				gameObject.transform.rotation,
				gameObject.transform.localScale.x,
				flag
			});
			this.CachePhotonHeldThingsData();
		}
		else
		{
			this.DoClearFromHand(thingOriginal, hand, false);
			Managers.dialogManager.ShowInfo(text, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
		}
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x00098584 File Offset: 0x00096984
	public void DoHoldFromHand(GameObject thing, GameObject newHand)
	{
		if (thing.transform.parent != null)
		{
			GameObject gameObject = thing.transform.parent.gameObject;
			Hand component = gameObject.GetComponent<Hand>();
			HandDot component2 = component.handDot.GetComponent<HandDot>();
			component2.currentlyHeldObject = null;
			component2.holdableInHand = null;
			TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(gameObject);
			thing.transform.parent = newHand.transform;
			Thing component3 = thing.GetComponent<Thing>();
			component3.OnHoldFromHand(true);
			this.ourPerson.photonView.RPC("DoHoldFromHand_Remote", PhotonTargets.Others, new object[]
			{
				topographyIdOfHand,
				component3.thingId,
				thing.transform.localPosition,
				thing.transform.localRotation
			});
			this.CachePhotonHeldThingsData();
			Managers.thingManager.heldThingsRegistrar.RegisterHold(newHand.GetComponent<Hand>(), false);
		}
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x00098674 File Offset: 0x00096A74
	public void DoHoldThrownThing(GameObject thing, string thrownId)
	{
		GameObject gameObject = thing.transform.parent.gameObject;
		TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(gameObject);
		Thing component = thing.GetComponent<Thing>();
		component.OnHoldFromThrownThing(true);
		this.ourPerson.photonView.RPC("DoHoldThrownThing_Remote", PhotonTargets.Others, new object[]
		{
			thrownId,
			topographyIdOfHand,
			component.thingId,
			thing.transform.localPosition,
			thing.transform.localRotation
		});
		this.CachePhotonHeldThingsData();
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x00098708 File Offset: 0x00096B08
	public string DoPlaceJustCreatedThing(GameObject thing, string clonedFromPlacementId, string clonedFromThingId)
	{
		Thing component = thing.GetComponent<Thing>();
		component.UnassignMyPlacedSubThings();
		Managers.thingManager.UpdateShowThingPartDirectionArrows(component, false);
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(thing, thing.transform.position, thing.transform.rotation);
		Thing component2 = gameObject.GetComponent<Thing>();
		bool flag = component.replaceInstancesInArea || CreationHelper.replaceInstancesInAreaOneTime;
		Managers.thingManager.MakeDeepThingClone(thing, gameObject, true, true, false);
		gameObject.transform.parent = thing.transform.parent;
		if (string.IsNullOrEmpty(clonedFromPlacementId))
		{
			if (!flag)
			{
				Managers.areaManager.SaveThingPlacement(gameObject);
			}
		}
		else
		{
			component2.placementId = clonedFromPlacementId;
			if (!flag)
			{
				Managers.areaManager.UpdateThingPlacement(gameObject);
			}
		}
		Misc.Destroy(thing);
		component2.OnPlacedJustCreated(true);
		component2.SetLightShadows(true, null);
		this.ourPerson.photonView.RPC("DoPlaceJustCreatedThing_Remote", PhotonTargets.Others, new object[]
		{
			component2.placementId,
			component2.thingId,
			gameObject.transform.position,
			gameObject.transform.rotation
		});
		if (flag)
		{
			Managers.areaManager.ReplaceAllOccurrencesOfThingInArea(clonedFromThingId, component2, true);
		}
		CreationHelper.replaceInstancesInAreaOneTime = false;
		return component2.placementId;
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x00098858 File Offset: 0x00096C58
	public string DoPlaceRecreatedPlacedSubThing(GameObject thing)
	{
		Thing component = thing.GetComponent<Thing>();
		Managers.areaManager.SaveThingPlacement(thing);
		component.OnPlacedRecreatedPlacedSubThing(true);
		this.ourPerson.photonView.RPC("DoPlaceRecreatedPlacedSubThing_Remote", PhotonTargets.Others, new object[]
		{
			component.placementId,
			component.thingId,
			component.transform.position,
			component.transform.rotation
		});
		return component.placementId;
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x000988D8 File Offset: 0x00096CD8
	public void DoDeletePlacement(GameObject thing, bool avoidDeletionEffects = false)
	{
		if (thing == null)
		{
			return;
		}
		Thing component = thing.GetComponent<Thing>();
		if (component != null && !component.alreadyDeleted && component.name != Universe.objectNameIfAlreadyDestroyed && !string.IsNullOrEmpty(component.placementId))
		{
			Managers.areaManager.DeleteThingPlacement(thing);
			this.ourPerson.photonView.RPC("DoDeletePlacement_Remote", PhotonTargets.Others, new object[] { component.placementId });
			component.DeleteMe(avoidDeletionEffects);
			Managers.areaManager.AssignPlacedSubThings(string.Empty);
			Managers.achievementManager.RegisterAchievement(Achievement.DeletedSomething);
		}
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x00098988 File Offset: 0x00096D88
	public void DoBehaviorScriptLine(GameObject thing, int thingPartIndex, int scriptLine, int stateI, int currentState)
	{
		Thing component = thing.GetComponent<Thing>();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = component.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.IncreaseBehaviorScriptMessagesPerSecond();
		Managers.optimizationManager.IndicateScriptActivityHere(thing.transform.position);
		this.ourPerson.photonView.RPC("DoBehaviorScriptLine_Remote", PhotonTargets.Others, new object[]
		{
			thingSpecifierType,
			specifierId,
			thingPartIndex,
			scriptLine,
			stateI,
			currentState,
			thing.transform.position,
			thing.transform.rotation
		});
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x00098A3C File Offset: 0x00096E3C
	public void DoSetEnvironmentChanger(GameObject changer)
	{
		Renderer component = changer.GetComponent<Renderer>();
		EnvironmentChangerData environmentChangerData = new EnvironmentChangerData(changer.name, changer.transform.eulerAngles, changer.transform.localScale.x, component.material.color);
		Managers.areaManager.PersistEnvironmentChanger(environmentChangerData);
		this.ourPerson.photonView.RPC("DoSetEnvironmentChanger_Remote", PhotonTargets.Others, new object[]
		{
			environmentChangerData.Name,
			environmentChangerData.Rotation,
			environmentChangerData.Scale,
			environmentChangerData.Color.r,
			environmentChangerData.Color.g,
			environmentChangerData.Color.b
		});
		Managers.achievementManager.RegisterAchievement(Achievement.ChangedEnvironmentChanger);
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x00098B16 File Offset: 0x00096F16
	public void DoSetEnvironmentType(EnvironmentType environmentType)
	{
		Managers.areaManager.PersistEnvironmentType(environmentType);
		this.ourPerson.photonView.RPC("DoSetEnvironmentType_Remote", PhotonTargets.Others, new object[] { environmentType });
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x00098B48 File Offset: 0x00096F48
	public void DoSetAreaPrivate(bool isPrivate)
	{
		Managers.areaManager.SetIsPrivate(isPrivate, delegate(bool settingSavedOk)
		{
			if (settingSavedOk)
			{
				if (isPrivate)
				{
					Log.Info("Area now private, and has new room key - sending reload area event to all", false);
					Managers.broadcastNetworkManager.RaiseEvent_ReloadArea();
				}
				else
				{
					this.ourPerson.photonView.RPC("DoSetAreaPrivate_Remote", PhotonTargets.Others, new object[] { isPrivate });
				}
			}
		});
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x00098B88 File Offset: 0x00096F88
	public void DoSetAreaZeroGravity(bool isZeroGravity)
	{
		Managers.areaManager.SetIsZeroGravity(isZeroGravity, delegate(bool settingSavedOk)
		{
			if (settingSavedOk)
			{
				this.ourPerson.photonView.RPC("DoSetAreaZeroGravity_Remote", PhotonTargets.Others, new object[] { isZeroGravity });
			}
		});
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x00098BC8 File Offset: 0x00096FC8
	public void DoSetAreaFloatingDust(bool hasFloatingDust)
	{
		Managers.areaManager.SetHasFloatingDust(hasFloatingDust, delegate(bool settingSavedOk)
		{
			if (settingSavedOk)
			{
				this.ourPerson.photonView.RPC("DoSetAreaFloatingDust_Remote", PhotonTargets.Others, new object[] { hasFloatingDust });
			}
		});
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x00098C08 File Offset: 0x00097008
	public void DoSetAreaCopyable(bool isCopyable)
	{
		Managers.areaManager.SetIsCopyable(isCopyable, delegate(bool settingSavedOk)
		{
			if (settingSavedOk)
			{
				this.ourPerson.photonView.RPC("DoSetAreaCopyable_Remote", PhotonTargets.Others, new object[] { isCopyable });
			}
		});
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x00098C48 File Offset: 0x00097048
	public void DoSetAreaOnlyOwnerSetsLocks(bool onlyOwnerSetsLocks)
	{
		Managers.areaManager.SetOnlyOwnerSetsLocks(onlyOwnerSetsLocks, delegate(bool settingSavedOk)
		{
			if (settingSavedOk)
			{
				this.ourPerson.photonView.RPC("DoSetAreaOnlyOwnerSetsLocks_Remote", PhotonTargets.Others, new object[] { onlyOwnerSetsLocks });
			}
		});
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x00098C88 File Offset: 0x00097088
	public void DoSetAreaIsExcluded(bool isExcluded)
	{
		Managers.areaManager.SetIsExcluded(isExcluded, delegate(bool settingSavedOk)
		{
			if (settingSavedOk)
			{
				this.ourPerson.photonView.RPC("DoSetAreaIsExcluded_Remote", PhotonTargets.Others, new object[] { isExcluded });
			}
		});
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x00098CC8 File Offset: 0x000970C8
	public void DoSetHandsColor(Color color)
	{
		this.ourPerson.SetHandsColor(color);
		base.StartCoroutine(Managers.serverManager.SetHandColor(color, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
		}));
		this.CachePhotonHandColor(color);
		this.ourPerson.photonView.RPC("DoSetHandsColor_Remote", PhotonTargets.Others, new object[] { color.r, color.g, color.b });
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x00098D60 File Offset: 0x00097160
	public GameObject DoAddToHand(GameObject hand, GameObject thing, bool downscaleForInventory = false)
	{
		TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(hand);
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(thing, thing.transform.position, thing.transform.rotation);
		gameObject.transform.parent = null;
		Thing component = gameObject.GetComponent<Thing>();
		if (downscaleForInventory && !component.keepSizeInInventory)
		{
			gameObject.transform.localScale = Managers.thingManager.GetAppropriateDownScaleForThing(gameObject, 0.1f, false);
		}
		else
		{
			gameObject.transform.localScale = Vector3.one;
		}
		gameObject.transform.parent = hand.transform;
		bool flag = thing.transform.parent != null && thing.transform.parent.name == "Placements";
		if (flag)
		{
			gameObject.transform.localPosition = Vector3.zero;
		}
		component.isInInventoryOrDialog = false;
		component.isInInventory = false;
		Managers.thingManager.MakeDeepThingClone(thing, gameObject, false, false, false);
		component.isInInventoryOrDialog = false;
		component.isInInventory = false;
		component.OnAddToHand(true);
		this.ourPerson.photonView.RPC("DoAddToHand_Remote", PhotonTargets.Others, new object[]
		{
			topographyIdOfHand,
			component.thingId,
			gameObject.transform.localPosition,
			gameObject.transform.localRotation,
			Our.mode,
			Our.previousMode
		});
		this.CachePhotonHeldThingsData();
		return gameObject;
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x00098EEC File Offset: 0x000972EC
	public GameObject DoCloneToHand(GameObject hand, GameObject thing, bool forceSizeReset = false)
	{
		TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(hand);
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(thing, thing.transform.position, thing.transform.rotation);
		Managers.thingManager.MakeDeepThingClone(thing, gameObject, false, false, false);
		gameObject.transform.parent = hand.transform;
		if (forceSizeReset)
		{
			gameObject.transform.localScale = Vector3.one;
		}
		Thing component = gameObject.GetComponent<Thing>();
		component.OnCloneToHand(true);
		this.ourPerson.photonView.RPC("DoCloneToHand_Remote", PhotonTargets.Others, new object[]
		{
			topographyIdOfHand,
			component.thingId,
			gameObject.transform.localPosition,
			gameObject.transform.localRotation,
			gameObject.transform.localScale
		});
		this.CachePhotonHeldThingsData();
		return gameObject;
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x00098FD0 File Offset: 0x000973D0
	public void DoClearPlacementIdOfHeldThing(TopographyId topographyId)
	{
		GameObject handByTopographyId = this.ourPerson.GetHandByTopographyId(topographyId);
		GameObject thingInHand = this.ourPerson.GetThingInHand(handByTopographyId, false);
		if (thingInHand != null)
		{
			Thing component = thingInHand.GetComponent<Thing>();
			if (component != null)
			{
				component.placementId = null;
			}
		}
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x00099020 File Offset: 0x00097420
	public void DoClearFromHand(GameObject thing, GameObject hand = null, bool destroyOnlyOnRemote = false)
	{
		if (hand == null && thing != null && thing.transform.parent != null)
		{
			hand = thing.transform.parent.gameObject;
		}
		if (hand != null)
		{
			TopographyId topographyIdOfHand = this.GetTopographyIdOfHand(hand);
			if (!destroyOnlyOnRemote)
			{
				thing.tag = "Untagged";
				global::UnityEngine.Object.Destroy(thing);
				thing = null;
			}
			this.ourPerson.photonView.RPC("DoClearFromHand_Remote", PhotonTargets.Others, new object[] { topographyIdOfHand });
			this.CachePhotonHeldThingsData();
		}
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x000990C8 File Offset: 0x000974C8
	public void DoPlayVideo(GameObject thing, string videoId, string videoTitle)
	{
		Thing component = thing.GetComponent<Thing>();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = component.GetSpecifierId(ref thingSpecifierType);
		string userId = Managers.personManager.ourPerson.userId;
		string screenName = Managers.personManager.ourPerson.screenName;
		Managers.videoManager.PlayVideoAtThing(thing, videoId, videoTitle, userId, screenName, 0, false);
		this.ourPerson.photonView.RPC("DoPlayVideo_Remote", PhotonTargets.Others, new object[] { thingSpecifierType, specifierId, videoId, videoTitle, userId, screenName });
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x00099154 File Offset: 0x00097554
	public void DoSetVideoVolume(float volume)
	{
		Managers.videoManager.SetVolume(volume);
		this.ourPerson.photonView.RPC("DoSetVideoVolume_Remote", PhotonTargets.Others, new object[] { volume });
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x00099186 File Offset: 0x00097586
	public void DoStopVideo()
	{
		this.ourPerson.photonView.RPC("DoStopVideo_Remote", PhotonTargets.Others, new object[0]);
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x000991A4 File Offset: 0x000975A4
	public void DoMovePlacement(GameObject thing, Vector3 localPosition, Vector3 localRotation)
	{
		Thing component = thing.GetComponent<Thing>();
		this.ourPerson.photonView.RPC("DoMovePlacement_Remote", PhotonTargets.Others, new object[] { component.placementId, component.thingId, localPosition, localRotation });
		thing.transform.localPosition = localPosition;
		thing.transform.localEulerAngles = localRotation;
		component.OnMovePlacement(true);
		Managers.areaManager.UpdateThingPlacement(thing);
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x00099224 File Offset: 0x00097624
	public IEnumerator DoPlaceThing(string thingId, Vector3 position, Quaternion rotation)
	{
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.PersonManagerDoPlaceThing, thingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, false, -1, null));
		if (thing != null)
		{
			thing.transform.parent = Managers.thingManager.placements.transform;
			thing.transform.localPosition = position;
			thing.transform.localRotation = rotation;
			Thing component = thing.GetComponent<Thing>();
			Managers.areaManager.SaveThingPlacement(thing);
			component.OnPlaced(true);
			this.ourPerson.photonView.RPC("DoPlaceThing_Remote", PhotonTargets.Others, new object[] { component.placementId, thingId, position, rotation });
		}
		else
		{
			Log.Debug("DoPlaceThing ignored as thing is null. ThingId was " + thingId);
		}
		yield break;
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x00099254 File Offset: 0x00097654
	public void DoFinalizeLoadedAllPlacements()
	{
		Managers.areaManager.AssignPlacedSubThings(string.Empty);
		Component[] componentsInChildren = this.People.GetComponentsInChildren<Person>();
		foreach (Person person in componentsInChildren)
		{
			person.StartNameTagRemovalCountdown(30f);
		}
		Managers.areaManager.ApplyCustomRoomProperties();
		Managers.videoManager.InitializeFromRoomVideoInfo();
		if (this.ourPerson.isMasterClient)
		{
			Managers.behaviorScriptManager.TriggerVariableChangeToThings(null, null);
		}
		else
		{
			string myBehaviorScriptVariablesString = this.ourPerson.GetMyBehaviorScriptVariablesString();
			bool flag = !string.IsNullOrEmpty(Managers.areaManager.areaToTransportToAfterNextAreaLoad);
			this.ourPerson.photonView.RPC("DoFinalizeLoadedAllPlacements_Remote", PhotonTargets.MasterClient, new object[]
			{
				this.ourPerson.userId,
				myBehaviorScriptVariablesString,
				flag
			});
		}
		Managers.areaManager.didRPCFinalizeLoadedAllPlacements = true;
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00099340 File Offset: 0x00097740
	public void DoSetPlacementAttribute(string placementId, PlacementAttribute attribute, bool state)
	{
		this.ourPerson.photonView.RPC("DoSetPlacementAttribute_Remote", PhotonTargets.Others, new object[] { placementId, attribute, state });
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00099374 File Offset: 0x00097774
	public void DoReplaceAllOccurrencesOfThingInArea(string oldThingId, string newThingId)
	{
		this.ourPerson.photonView.RPC("DoReplaceAllOccurrencesOfThingInArea_Remote", PhotonTargets.Others, new object[] { oldThingId, newThingId });
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x0009939C File Offset: 0x0009779C
	public void DoAddRidingBeacon(Thing thingToAddTo, Vector3 beaconPosition)
	{
		this.ourPerson.RemoveRidingBeacon();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = thingToAddTo.GetSpecifierId(ref thingSpecifierType);
		Transform parent = thingToAddTo.transform.parent;
		Managers.personManager.ourPerson.AddRidingBeacon(thingToAddTo.gameObject, beaconPosition, false);
		this.ourPerson.ridingBeaconPositionOffset = this.ourPerson.ridingBeacon.transform.position - this.ourPerson.Rig.transform.position;
		this.ourPerson.ridingBeaconRotationOffset = this.ourPerson.ridingBeacon.transform.eulerAngles - this.ourPerson.Rig.transform.eulerAngles;
		if (thingToAddTo.transform.parent != null && thingToAddTo.transform.parent.parent != null)
		{
			Thing componentInChildren = thingToAddTo.transform.parent.parent.GetComponentInChildren<Thing>();
			if (componentInChildren != null)
			{
				Component[] componentsInChildren = componentInChildren.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
				foreach (ThingPart thingPart in componentsInChildren)
				{
					thingPart.ourPersonRidingBeaconReference = this.ourPerson.ridingBeacon;
				}
			}
		}
		this.ourPerson.photonView.RPC("DoAddRidingBeacon_Remote", PhotonTargets.Others, new object[]
		{
			thingSpecifierType,
			specifierId,
			this.ourPerson.ridingBeacon.transform.localPosition,
			this.ourPerson.ridingBeaconPositionOffset,
			this.ourPerson.ridingBeaconRotationOffset
		});
		this.ourPerson.ridingBeaconCache = new RidingBeaconCache();
		this.ourPerson.ridingBeaconCache.thingAttachedToSpecifierType = thingSpecifierType;
		this.ourPerson.ridingBeaconCache.thingAttachedToSpecifierId = specifierId;
		this.ourPerson.ridingBeaconCache.beaconPosition = this.ourPerson.ridingBeacon.transform.localPosition;
		this.ourPerson.ridingBeaconCache.beaconPositionOffset = this.ourPerson.ridingBeaconPositionOffset;
		this.ourPerson.ridingBeaconCache.beaconRotationOffset = this.ourPerson.ridingBeaconRotationOffset;
		this.CachePhotonRidingBeacon();
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x000995FC File Offset: 0x000979FC
	public void DoRemoveRidingBeacon()
	{
		if (this.ourPerson.ridingBeacon != null)
		{
			this.ourPerson.photonView.RPC("DoRemoveRidingBeacon_Remote", PhotonTargets.Others, new object[0]);
			Managers.personManager.ourPerson.RemoveRidingBeacon();
			this.CachePhotonRidingBeacon();
		}
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x00099650 File Offset: 0x00097A50
	public void DoUpdateRidingBeaconOffset()
	{
		if (this.ourPerson.ridingBeacon != null)
		{
			this.ourPerson.ridingBeaconPositionOffset = this.ourPerson.ridingBeacon.transform.position - this.ourPerson.Rig.transform.position;
			this.ourPerson.ridingBeaconRotationOffset = this.ourPerson.ridingBeacon.transform.eulerAngles - this.ourPerson.Rig.transform.eulerAngles;
			this.ourPerson.photonView.RPC("DoUpdateRidingBeaconOffset_Remote", PhotonTargets.Others, new object[]
			{
				this.ourPerson.Rig.transform.eulerAngles,
				this.ourPerson.ridingBeaconPositionOffset,
				this.ourPerson.ridingBeaconRotationOffset
			});
			this.ourPerson.ridingBeaconCache.beaconPositionOffset = this.ourPerson.ridingBeaconPositionOffset;
			this.ourPerson.ridingBeaconCache.beaconRotationOffset = this.ourPerson.ridingBeaconRotationOffset;
			this.CachePhotonRidingBeacon();
		}
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x00099784 File Offset: 0x00097B84
	public void DoAddTypedText(string text)
	{
		this.ourPerson.AddTypedText(text);
		this.ourPerson.photonView.RPC("DoAddTypedText_Remote", PhotonTargets.Others, new object[] { text });
		if (!string.IsNullOrEmpty(text))
		{
			Managers.behaviorScriptManager.TriggerOnHears(text.ToLower().Trim());
		}
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x000997DD File Offset: 0x00097BDD
	public void DoUpdatePlacementScale(Thing thing, Vector3 scale)
	{
		this.ourPerson.photonView.RPC("DoUpdatePlacementScale_Remote", PhotonTargets.Others, new object[] { thing.placementId, scale.x });
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x00099813 File Offset: 0x00097C13
	public void DoUpdatePlacementShowAt(Thing thing)
	{
		this.ourPerson.photonView.RPC("DoUpdatePlacementShowAt_Remote", PhotonTargets.Others, new object[] { thing.placementId, thing.distanceToShow });
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x00099848 File Offset: 0x00097C48
	public void DoUpdateViaAreaSettingsJson()
	{
		this.ourPerson.photonView.RPC("DoUpdateViaAreaSettingsJson_Remote", PhotonTargets.Others, new object[] { Managers.filterManager.GetJson() });
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x00099874 File Offset: 0x00097C74
	public void DoAddJustCreatedTemporaryThing(GameObject thing)
	{
		Thing component = thing.GetComponent<Thing>();
		Managers.thingManager.UpdateShowThingPartDirectionArrows(component, false);
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(thing, thing.transform.position, thing.transform.rotation);
		Thing component2 = gameObject.GetComponent<Thing>();
		Managers.thingManager.MakeDeepThingClone(thing, gameObject, true, false, false);
		gameObject.transform.parent = thing.transform.parent;
		Misc.Destroy(thing);
		component2.thrownId = Misc.GetRandomId();
		component2.OnAddJustCreatedTemporary(true);
		this.ourPerson.photonView.RPC("DoAddJustCreatedTemporaryThing_Remote", PhotonTargets.Others, new object[]
		{
			component2.thingId,
			gameObject.transform.position,
			gameObject.transform.rotation,
			component2.thrownId
		});
		CreationHelper.replaceInstancesInAreaOneTime = false;
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00099950 File Offset: 0x00097D50
	public void DoInformOfAreaBehaviorScriptVariableChange(string variableName, float variableValue)
	{
		this.ourPerson.IncreaseBehaviorScriptMessagesPerSecond();
		variableName = variableName.Replace("area.", string.Empty);
		this.ourPerson.photonView.RPC("DoInformOfAreaBehaviorScriptVariableChange_Remote", PhotonTargets.Others, new object[] { variableName, variableValue });
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x000999A4 File Offset: 0x00097DA4
	public void DoInformOfThingBehaviorScriptVariableChange(Thing thing, string variableName, float variableValue)
	{
		this.ourPerson.IncreaseBehaviorScriptMessagesPerSecond();
		Managers.optimizationManager.IndicateScriptActivityHere(thing.transform.position);
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = thing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoInformOfThingBehaviorScriptVariableChange_Remote", PhotonTargets.Others, new object[] { thingSpecifierType, specifierId, variableName, variableValue });
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x00099A14 File Offset: 0x00097E14
	public void DoInformOfPersonBehaviorScriptVariableChange(Person person, string variableName, float variableValue)
	{
		this.ourPerson.IncreaseBehaviorScriptMessagesPerSecond();
		variableName = variableName.Replace("person.", string.Empty);
		this.ourPerson.photonView.RPC("DoInformOfPersonBehaviorScriptVariableChange_Remote", PhotonTargets.Others, new object[] { person.userId, variableName, variableValue });
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x00099A70 File Offset: 0x00097E70
	public void DoInformOfBehaviorScriptVariablesAndThingStates(bool isOfInterval = false)
	{
		this.ourPerson.IncreaseBehaviorScriptMessagesPerSecond();
		this.ourPerson.lastHandledInformedOthersOfStatesTime = Time.time;
		string text = "|";
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = string.Empty;
		string text6 = string.Empty;
		string text7 = string.Empty;
		Component[] allThings = Managers.thingManager.GetAllThings();
		float time = Time.time;
		foreach (Thing thing in allThings)
		{
			if (thing.name != Universe.objectNameIfAlreadyDestroyed)
			{
				ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
				string specifierId = thing.GetSpecifierId(ref thingSpecifierType);
				string syncingCompressedSpecifierType = PersonManager.GetSyncingCompressedSpecifierType(thingSpecifierType);
				IEnumerator enumerator = thing.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform = (Transform)obj;
						ThingPart component = transform.GetComponent<ThingPart>();
						if (component != null && component.IsStrictSyncingToAreaNewcomersNeeded(isOfInterval))
						{
							if (text2 != string.Empty)
							{
								text2 += "\n";
							}
							string text8 = string.Concat(new string[]
							{
								syncingCompressedSpecifierType,
								text,
								specifierId,
								text,
								component.GetSyncingToAreaNewcomersDataString(isOfInterval)
							});
							text2 += text8;
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
				foreach (KeyValuePair<string, float> keyValuePair in thing.behaviorScriptVariables)
				{
					if (text5 != string.Empty)
					{
						text5 += "\n";
					}
					string text9 = text5;
					text5 = string.Concat(new object[] { text9, syncingCompressedSpecifierType, text, specifierId, text, keyValuePair.Key, text, keyValuePair.Value });
				}
				if (!isOfInterval && thing.stricterPhysicsSyncing && thing.rigidbody != null && thing.isThrownOrEmitted && !string.IsNullOrEmpty(thing.thrownId))
				{
					if (text3 != string.Empty)
					{
						text3 += "\n";
					}
					string text9 = text3;
					text3 = string.Concat(new string[]
					{
						text9,
						syncingCompressedSpecifierType,
						text,
						specifierId,
						text,
						thing.thingId,
						text,
						PersonManager.GetSyncingCompressedVector3(thing.transform.position),
						text,
						PersonManager.GetSyncingCompressedQuaternion(thing.transform.rotation),
						text,
						PersonManager.GetSyncingCompressedVector3(thing.rigidbody.velocity),
						text,
						PersonManager.GetSyncingCompressedVector3(thing.rigidbody.angularVelocity),
						text,
						PersonManager.GetSyncingCompressedFloat(thing.destroyMeInTime)
					});
				}
				if (thing.persistentAttributeChangingCommandWasApplied)
				{
					if (text7 != string.Empty)
					{
						text7 += "\n";
					}
					string text9 = text7;
					text7 = string.Concat(new string[]
					{
						text9,
						syncingCompressedSpecifierType,
						text,
						specifierId,
						text,
						PersonManager.GetSyncingCompressedBool(thing.invisible),
						text,
						PersonManager.GetSyncingCompressedBool(thing.uncollidable)
					});
				}
			}
		}
		if (Managers.areaManager != null)
		{
			foreach (KeyValuePair<string, float> keyValuePair2 in Managers.areaManager.behaviorScriptVariables)
			{
				if (text4 != string.Empty)
				{
					text4 += "\n";
				}
				string text9 = text4;
				text4 = string.Concat(new object[]
				{
					text9,
					keyValuePair2.Key.Replace("area.", string.Empty),
					text,
					keyValuePair2.Value
				});
			}
		}
		List<Person> currentAreaPersons = this.GetCurrentAreaPersons();
		foreach (Person person in currentAreaPersons)
		{
			if (text6 != string.Empty)
			{
				text6 += "\n";
			}
			text6 = text6 + person.userId + text + person.GetMyBehaviorScriptVariablesString();
		}
		string text10 = (isOfInterval ? string.Empty : this.GetSlideshowUrlsString());
		string text11 = string.Empty;
		if (!isOfInterval)
		{
			text11 = Managers.temporarilyDestroyedThingsManager.GetMasterClientSyncString();
		}
		string text12 = string.Empty;
		if (!isOfInterval)
		{
			text12 = this.GetMovableByEveryoneThingsString();
		}
		this.ourPerson.photonView.RPC("DoInformOfBehaviorScriptVariablesAndThingStates_Remote", PhotonTargets.Others, new object[] { time, text2, text3, text4, text5, text6, text10, text7, text11, text12 });
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x00099FF0 File Offset: 0x000983F0
	public void DoInformOfThingPhysics(Thing thing)
	{
		if (thing.rigidbody == null || string.IsNullOrEmpty(thing.thrownId))
		{
			return;
		}
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = thing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoInformOfThingPhysics_Remote", PhotonTargets.Others, new object[]
		{
			thingSpecifierType,
			specifierId,
			thing.transform.position,
			thing.transform.rotation,
			thing.rigidbody.velocity,
			thing.rigidbody.angularVelocity
		});
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x0009A0A0 File Offset: 0x000984A0
	public void DoSendTestCall()
	{
		PhotonTargets photonTargets = PhotonTargets.Others;
		string text = "Test call to PhotonTargets." + photonTargets.ToString();
		Log.Debug(text);
		this.ourPerson.photonView.RPC("DoSendTestCall_Remote", photonTargets, new object[]
		{
			this.ourPerson.screenName,
			text
		});
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x0009A0FC File Offset: 0x000984FC
	public void DoWeSwitchedToThisSlideshowImage(ThingPartSlideshow slideshow, int urlIndex)
	{
		ThingPart component = slideshow.GetComponent<ThingPart>();
		Thing myRootThing = component.GetMyRootThing();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = myRootThing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoWeSwitchedToThisSlideshowImage_Remote", PhotonTargets.All, new object[] { thingSpecifierType, specifierId, component.indexWithinThing, urlIndex });
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x0009A164 File Offset: 0x00098564
	public void DoSlideshowControl_SetUrls(ThingPart thingPart, List<string> urls, string searchText)
	{
		Thing myRootThing = thingPart.GetMyRootThing();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = myRootThing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoSlideshowControl_SetUrls_Remote", PhotonTargets.All, new object[]
		{
			thingSpecifierType,
			specifierId,
			thingPart.indexWithinThing,
			urls.ToArray(),
			searchText
		});
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x0009A1C8 File Offset: 0x000985C8
	public void DoSlideshowControl_Play(ThingPart thingPart)
	{
		Thing myRootThing = thingPart.GetMyRootThing();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = myRootThing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoSlideshowControl_Play_Remote", PhotonTargets.All, new object[] { thingSpecifierType, specifierId, thingPart.indexWithinThing });
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0009A220 File Offset: 0x00098620
	public void DoSlideshowControl_Pause(ThingPart thingPart)
	{
		Thing myRootThing = thingPart.GetMyRootThing();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = myRootThing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoSlideshowControl_Pause_Remote", PhotonTargets.All, new object[] { thingSpecifierType, specifierId, thingPart.indexWithinThing });
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x0009A278 File Offset: 0x00098678
	public void DoSlideshowControl_Stop(ThingPart thingPart)
	{
		Thing myRootThing = thingPart.GetMyRootThing();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = myRootThing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoSlideshowControl_Stop_Remote", PhotonTargets.All, new object[] { thingSpecifierType, specifierId, thingPart.indexWithinThing });
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x0009A2D0 File Offset: 0x000986D0
	public void DoInformOfThingInvisibleUncollidableChange(Thing thing)
	{
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = thing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoInformOfThingInvisibleUncollidableChange_Remote", PhotonTargets.Others, new object[] { thingSpecifierType, specifierId, thing.invisible, thing.uncollidable });
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x0009A330 File Offset: 0x00098730
	public void DoInformOfThingPartInvisibleUncollidableChange(ThingPart thingPart)
	{
		Thing myRootThing = thingPart.GetMyRootThing();
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		string specifierId = myRootThing.GetSpecifierId(ref thingSpecifierType);
		this.ourPerson.photonView.RPC("DoInformOfThingPartInvisibleUncollidableChange_Remote", PhotonTargets.Others, new object[] { thingSpecifierType, specifierId, thingPart.indexWithinThing, thingPart.invisible, thingPart.uncollidable });
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x0009A3A4 File Offset: 0x000987A4
	public void DoRedundantlyInformAboutThingDestruction(string placementId, ThingDestruction thingDestruction)
	{
		PhotonView photonView = this.ourPerson.photonView;
		string text = "DoRedundantlyInformAboutThingDestruction_Remote";
		PhotonTargets photonTargets = PhotonTargets.Others;
		object[] array = new object[12];
		array[0] = placementId;
		array[1] = thingDestruction.burst;
		array[2] = thingDestruction.burstVelocity;
		array[3] = thingDestruction.maxParts;
		array[4] = thingDestruction.growth;
		array[5] = thingDestruction.bouncy;
		array[6] = thingDestruction.slidy;
		array[7] = thingDestruction.hidePartsInSeconds;
		int num = 8;
		float? restoreInSeconds = thingDestruction.restoreInSeconds;
		array[num] = ((restoreInSeconds == null) ? new float?(0f) : thingDestruction.restoreInSeconds);
		array[9] = thingDestruction.gravity;
		array[10] = thingDestruction.collides;
		array[11] = thingDestruction.collidesWithSiblings;
		photonView.RPC(text, photonTargets, array);
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x0009A494 File Offset: 0x00098894
	public void DoHoldAsMovableByEveryone(Thing thing, Side handSide)
	{
		ThingMovableByEveryone component = thing.GetComponent<ThingMovableByEveryone>();
		component.OnPickUp(true);
		this.ourPerson.photonView.RPC("DoHoldAsMovableByEveryone_Remote", PhotonTargets.Others, new object[]
		{
			thing.placementId,
			handSide,
			thing.transform.localPosition,
			thing.transform.localRotation
		});
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x0009A504 File Offset: 0x00098904
	public void DoPlaceAsMovableByEveryone(Thing thing, Side handSide)
	{
		ThingMovableByEveryone component = thing.GetComponent<ThingMovableByEveryone>();
		component.OnPutDown(true);
		this.ourPerson.photonView.RPC("DoPlaceAsMovableByEveryone_Remote", PhotonTargets.Others, new object[]
		{
			thing.placementId,
			handSide,
			thing.transform.position,
			thing.transform.rotation
		});
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x0009A573 File Offset: 0x00098973
	public void DoPlaySound(string soundName, Vector3 position, float volume)
	{
		this.ourPerson.photonView.RPC("DoPlaySound_Remote", PhotonTargets.Others, new object[] { soundName, position, volume });
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x0009A5A8 File Offset: 0x000989A8
	public void DoInformOnMovableByEveryoneGroupTransforms(List<Thing> things, Side holdingHandSide, MovableByEveryoneGroupInformType informType)
	{
		List<string> list = new List<string>();
		foreach (Thing thing in things)
		{
			string text = string.Concat(new string[]
			{
				thing.placementId,
				"|",
				PersonManager.GetSyncingCompressedVector3(thing.transform.localPosition),
				"|",
				PersonManager.GetSyncingCompressedQuaternion(thing.transform.localRotation)
			});
			list.Add(text);
		}
		this.ourPerson.photonView.RPC("DoInformOnMovableByEveryoneGroupTransforms_Remote", PhotonTargets.Others, new object[]
		{
			list.ToArray(),
			holdingHandSide,
			informType
		});
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0009A688 File Offset: 0x00098A88
	public void DoSetBrowserUrl(Browser browser, string url, bool isSetByUs = false, bool alreadySetLocally = false)
	{
		browser.lastRemotePersonReceivedUrl = null;
		if (!alreadySetLocally)
		{
			browser.Url = url;
			Managers.browserManager.RegisterBrowserFunctions(browser);
		}
		if (isSetByUs)
		{
			browser.AddUrlSetByPersonId(url, this.ourPerson.userId);
		}
		if (!this.GetIsThisObjectOfOurPerson(browser.gameObject, false) && this.GetCurrentAreaPersonCount() >= 2 && browser.syncUrlChangesBetweenPeople)
		{
			ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
			Thing component = browser.transform.parent.GetComponent<Thing>();
			string specifierId = component.GetSpecifierId(ref thingSpecifierType);
			this.ourPerson.photonView.RPC("DoSetBrowserUrl_Remote", PhotonTargets.Others, new object[] { thingSpecifierType, specifierId, url, isSetByUs });
		}
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x0009A748 File Offset: 0x00098B48
	public void DoCloseBrowser(Browser browser)
	{
		bool isThisObjectOfOurPerson = this.GetIsThisObjectOfOurPerson(browser.gameObject, false);
		ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
		Thing component = browser.transform.parent.GetComponent<Thing>();
		string specifierId = component.GetSpecifierId(ref thingSpecifierType);
		global::UnityEngine.Object.Destroy(browser.gameObject);
		browser = null;
		if (!isThisObjectOfOurPerson)
		{
			this.ourPerson.photonView.RPC("DoCloseBrowser_Remote", PhotonTargets.Others, new object[] { thingSpecifierType, specifierId });
		}
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x0009A7BC File Offset: 0x00098BBC
	public void DoReloadBrowserPage(Browser browser)
	{
		string url = browser.Url;
		browser.Reload(true);
		base.StartCoroutine(Managers.browserManager.RegisterBrowserFunctionsDelayed(browser));
		if (!this.GetIsThisObjectOfOurPerson(browser.gameObject, false))
		{
			ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
			Thing component = browser.transform.parent.GetComponent<Thing>();
			string specifierId = component.GetSpecifierId(ref thingSpecifierType);
			this.ourPerson.photonView.RPC("DoReloadBrowserPage_Remote", PhotonTargets.Others, new object[] { thingSpecifierType, specifierId, url });
		}
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x0009A844 File Offset: 0x00098C44
	public void DoAmplifySpeech(bool state, bool skipSyncing = false)
	{
		this.ourPerson.SetAmplifySpeech(state);
		this.UpdatePhotonCustomProperty(PhotonCacheKeys.AmplifySpeech, state);
		if (!skipSyncing)
		{
			this.ourPerson.photonView.RPC("DoSetAmplifySpeech_Remote", PhotonTargets.Others, new object[] { state });
		}
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x0009A894 File Offset: 0x00098C94
	public void DoIndicateIsSpeaking()
	{
		this.ourPerson.photonView.RPC("DoIndicateIsSpeaking_Remote", PhotonTargets.Others, new object[0]);
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x0009A8B4 File Offset: 0x00098CB4
	private string GetMovableByEveryoneThingsString()
	{
		string text = string.Empty;
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
		foreach (Thing thing in componentsInChildren)
		{
			if (thing.name != Universe.objectNameIfAlreadyDestroyed && thing.movableByEveryone && !thing.IsAtOriginalPlacement() && thing.transform.parent == Managers.thingManager.placements.transform)
			{
				if (text != string.Empty)
				{
					text += "\n";
				}
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					thing.placementId,
					"|",
					PersonManager.GetSyncingCompressedVector3(thing.transform.localPosition),
					"|",
					PersonManager.GetSyncingCompressedQuaternion(thing.transform.localRotation)
				});
			}
		}
		return text;
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x0009A9C8 File Offset: 0x00098DC8
	private string GetSlideshowUrlsString()
	{
		string text = string.Empty;
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(ThingPartSlideshow), true);
		foreach (ThingPartSlideshow thingPartSlideshow in componentsInChildren)
		{
			if (thingPartSlideshow.HasUrls())
			{
				List<string> urls = thingPartSlideshow.GetUrls();
				ThingPart component = thingPartSlideshow.GetComponent<ThingPart>();
				if (component != null)
				{
					if (text != string.Empty)
					{
						text += "\n";
					}
					Thing myRootThing = component.GetMyRootThing();
					ThingSpecifierType thingSpecifierType = ThingSpecifierType.None;
					string specifierId = myRootThing.GetSpecifierId(ref thingSpecifierType);
					string syncingCompressedSpecifierType = PersonManager.GetSyncingCompressedSpecifierType(thingSpecifierType);
					string text2 = string.Join(" ", urls.ToArray());
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						syncingCompressedSpecifierType,
						"|",
						specifierId,
						"|",
						component.indexWithinThing.ToString(),
						"|",
						text2,
						"|",
						thingPartSlideshow.urlIndex.ToString(),
						"|",
						thingPartSlideshow.searchText,
						"|",
						PersonManager.GetSyncingCompressedBool(thingPartSlideshow.running)
					});
				}
			}
		}
		return text;
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x0009AB2F File Offset: 0x00098F2F
	public static string GetSyncingCompressedInt(int v)
	{
		return (v != -1) ? v.ToString() : string.Empty;
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x0009AB4F File Offset: 0x00098F4F
	public static string GetSyncingCompressedFloat(float v)
	{
		return (v != -1f) ? v.ToString() : string.Empty;
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x0009AB73 File Offset: 0x00098F73
	public static string GetSyncingCompressedString(string s)
	{
		s = s.Replace("|", "[r1]");
		s = s.Replace("\n", "[r2]");
		return s;
	}

	// Token: 0x0600124D RID: 4685 RVA: 0x0009AB9A File Offset: 0x00098F9A
	public static string GetSyncingCompressedBool(bool v)
	{
		return (!v) ? string.Empty : "1";
	}

	// Token: 0x0600124E RID: 4686 RVA: 0x0009ABB1 File Offset: 0x00098FB1
	public static bool GetSyncingUncompressedBool(string v)
	{
		return v == "1";
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x0009ABC0 File Offset: 0x00098FC0
	public static string GetSyncingCompressedVector3(Vector3 v)
	{
		string text = string.Empty;
		if (v != Vector3.zero)
		{
			text = string.Concat(new string[]
			{
				v.x.ToString(),
				",",
				v.y.ToString(),
				",",
				v.z.ToString()
			});
		}
		return text;
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x0009AC40 File Offset: 0x00099040
	public static Vector3 GetSyncingUncompressedVector3(string s)
	{
		Vector3 zero = Vector3.zero;
		if (s != string.Empty)
		{
			string[] array = s.Split(new char[] { ',' });
			zero = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
		}
		return zero;
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x0009AC98 File Offset: 0x00099098
	public static string GetSyncingCompressedQuaternion(Quaternion v)
	{
		string text = string.Empty;
		if (v != Quaternion.identity)
		{
			text = string.Concat(new string[]
			{
				v.x.ToString(),
				",",
				v.y.ToString(),
				",",
				v.z.ToString(),
				",",
				v.w.ToString()
			});
		}
		return text;
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x0009AD34 File Offset: 0x00099134
	public static Quaternion GetSyncingUncompressedQuaternion(string s)
	{
		Quaternion identity = Quaternion.identity;
		if (s != string.Empty)
		{
			string[] array = s.Split(new char[] { ',' });
			identity = new Quaternion(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
		}
		return identity;
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x0009AD94 File Offset: 0x00099194
	public static string GetSyncingCompressedSpecifierType(ThingSpecifierType specifierType)
	{
		string text;
		if (specifierType == ThingSpecifierType.Placement)
		{
			text = string.Empty;
		}
		else
		{
			int num = (int)specifierType;
			text = num.ToString();
		}
		return text;
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x0009ADC1 File Offset: 0x000991C1
	public static int GetSyncingUncompressedInt(string s)
	{
		return (!string.IsNullOrEmpty(s)) ? int.Parse(s) : (-1);
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x0009ADDA File Offset: 0x000991DA
	public static float GetSyncingUncompressedFloat(string s)
	{
		return (!string.IsNullOrEmpty(s)) ? float.Parse(s) : (-1f);
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x0009ADF7 File Offset: 0x000991F7
	public static string GetSyncingUncompressedString(string s)
	{
		s = s.Replace("[r2]", "\n");
		s = s.Replace("[r1]", "|");
		return s;
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x0009AE1E File Offset: 0x0009921E
	public static ThingSpecifierType GetSyncingUncompressedSpecifierType(string specifierTypeString)
	{
		return (ThingSpecifierType)((!string.IsNullOrEmpty(specifierTypeString)) ? int.Parse(specifierTypeString) : 4);
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x0009AE37 File Offset: 0x00099237
	public static string EncodeSyncingString(string s)
	{
		if (s == null)
		{
			s = string.Empty;
		}
		s = s.Replace("|", "[s_]");
		s = s.Replace("\n", "[i_]");
		return s;
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x0009AE6B File Offset: 0x0009926B
	public static string UnencodeSyncingString(string s)
	{
		s = s.Replace("[i_]", "\n");
		s = s.Replace("[s_]", "|");
		return s;
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x0009AE94 File Offset: 0x00099294
	public void RestoreOurMemorizedAttachments(Dictionary<string, AttachmentData> attachmentIdData)
	{
		Component[] componentsInChildren = this.OurPersonRig.GetComponentsInChildren(typeof(AttachmentPoint), true);
		foreach (Component component in componentsInChildren)
		{
			AttachmentPoint component2 = component.GetComponent<AttachmentPoint>();
			AttachmentData attachmentData = null;
			if (attachmentIdData.TryGetValue(component2.name, out attachmentData))
			{
				base.StartCoroutine(this.RestoreOurMemorizedAttachment(component2, attachmentData));
			}
			else if (component2.attachedThing != null)
			{
				this.DoRemoveAttachedThing(component2.gameObject);
			}
		}
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x0009AF28 File Offset: 0x00099328
	private IEnumerator RestoreOurMemorizedAttachment(AttachmentPoint attachmentPoint, AttachmentData attachmentData)
	{
		if (attachmentPoint.attachedThing != null)
		{
			this.DoRemoveAttachedThing(attachmentPoint.gameObject);
		}
		yield return new WaitForSeconds(0.35f);
		GameObject attachmentThing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.ApproveBodyDialogAttachment, attachmentData.thingId, delegate(GameObject returnThing)
		{
			attachmentThing = returnThing;
		}, false, false, -1, null));
		if (attachmentThing != null)
		{
			attachmentThing.transform.parent = attachmentPoint.transform;
			attachmentThing.transform.localPosition = attachmentData.position;
			attachmentThing.transform.localEulerAngles = attachmentData.rotation;
			this.DoAttachThing(attachmentPoint.gameObject, attachmentThing.gameObject, false);
		}
		yield break;
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x0009AF54 File Offset: 0x00099354
	public void SetOurPersonIsVisibleToDesktopCamera(bool isVisible, bool forceHeadInvisible = false, bool forceHandsVisible = false)
	{
		int num = LayerMask.NameToLayer("Default");
		int num2 = LayerMask.NameToLayer("InvisibleToOurPerson");
		int num3 = LayerMask.NameToLayer("InvisibleToDesktopCamera");
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig");
		Transform[] componentsInChildren = @object.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			bool flag = isVisible;
			if (forceHeadInvisible && flag && this.ThisOrAnyParentNameIs(transform, "HeadCore"))
			{
				flag = false;
			}
			else if (forceHandsVisible && !flag)
			{
				flag = !transform.CompareTag("AttachmentPoint") && this.ThisOrAnyParentTagIs(transform, "HandCore");
			}
			if (transform.gameObject.layer != num2)
			{
				if (flag)
				{
					transform.gameObject.layer = num;
				}
				else
				{
					transform.gameObject.layer = num3;
				}
			}
		}
		GameObject object2 = Managers.treeManager.GetObject("/OurPersonRig/HandCoreLeft");
		GameObject object3 = Managers.treeManager.GetObject("/OurPersonRig/HandCoreRight");
		Hand component = object2.GetComponent<Hand>();
		Hand component2 = object3.GetComponent<Hand>();
		string text = ((!isVisible) ? "InvisibleToDesktopCamera" : "Default");
		int num4 = LayerMask.NameToLayer(text);
		component.SetTargetSphereLayer(num4);
		component2.SetTargetSphereLayer(num4);
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x0009B0B8 File Offset: 0x000994B8
	private bool ThisOrAnyParentTagIs(Transform thisTransform, string tag)
	{
		bool flag = false;
		while (thisTransform != null)
		{
			if (thisTransform.CompareTag(tag))
			{
				flag = true;
				break;
			}
			thisTransform = thisTransform.parent;
		}
		return flag;
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x0009B0F4 File Offset: 0x000994F4
	private bool ThisOrAnyParentNameIs(Transform thisTransform, string thisName)
	{
		bool flag = false;
		while (thisTransform != null)
		{
			if (thisTransform.name == thisName)
			{
				flag = true;
				break;
			}
			thisTransform = thisTransform.parent;
		}
		return flag;
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x0009B138 File Offset: 0x00099538
	public void ShowOurSecondaryDots(bool doShow)
	{
		if (this.ourPerson.leftHandSecondaryDot != null)
		{
			Renderer component = this.ourPerson.leftHandSecondaryDot.GetComponent<Renderer>();
			component.enabled = doShow;
		}
		if (this.ourPerson.rightHandSecondaryDot != null)
		{
			Renderer component2 = this.ourPerson.rightHandSecondaryDot.GetComponent<Renderer>();
			component2.enabled = doShow;
		}
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x0009B1A1 File Offset: 0x000995A1
	public void SaveOurLegAttachmentPointPositions()
	{
		base.CancelInvoke("DoSaveOurLegAttachmentPointPositions");
		base.Invoke("DoSaveOurLegAttachmentPointPositions", 2f);
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x0009B1C0 File Offset: 0x000995C0
	private void DoSaveOurLegAttachmentPointPositions()
	{
		Misc.PlayerPrefsSetVector3("legLeftPosition", this.ourPerson.AttachmentPointLegLeft.transform.localPosition);
		Misc.PlayerPrefsSetVector3("legLeftRotation", this.ourPerson.AttachmentPointLegLeft.transform.localEulerAngles);
		Misc.PlayerPrefsSetVector3("legRightPosition", this.ourPerson.AttachmentPointLegRight.transform.localPosition);
		Misc.PlayerPrefsSetVector3("legRightRotation", this.ourPerson.AttachmentPointLegRight.transform.localEulerAngles);
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x0009B24C File Offset: 0x0009964C
	private void LoadOurLegAttachmentPointPositions()
	{
		Vector3? vector = Misc.PlayerPrefsGetVector3("legLeftPosition");
		Vector3? vector2 = Misc.PlayerPrefsGetVector3("legLeftRotation");
		if (vector != null && vector2 != null)
		{
			this.ourPerson.AttachmentPointLegLeft.transform.localPosition = vector.Value;
			this.ourPerson.AttachmentPointLegLeft.transform.localEulerAngles = vector2.Value;
		}
		Vector3? vector3 = Misc.PlayerPrefsGetVector3("legRightPosition");
		Vector3? vector4 = Misc.PlayerPrefsGetVector3("legRightRotation");
		if (vector3 != null && vector4 != null)
		{
			this.ourPerson.AttachmentPointLegRight.transform.localPosition = vector3.Value;
			this.ourPerson.AttachmentPointLegRight.transform.localEulerAngles = vector4.Value;
		}
	}

	// Token: 0x04001153 RID: 4435
	private Dictionary<string, GameObject> PeopleByUserId;

	// Token: 0x04001154 RID: 4436
	[Tooltip("The person rig should be dragged here.")]
	public GameObject PersonPrefab;

	// Token: 0x04001155 RID: 4437
	private PhotonView photonView;

	// Token: 0x04001156 RID: 4438
	private AttachmentDataSet attachmentsData;

	// Token: 0x04001159 RID: 4441
	public string INFO_Our_Id;

	// Token: 0x0400115A RID: 4442
	public string INFO_Our_HomeAreaId;

	// Token: 0x0400115B RID: 4443
	private int[] OurComponentViewIds;

	// Token: 0x0400115C RID: 4444
	private string horatioId;

	// Token: 0x0400115D RID: 4445
	private string philippId;

	// Token: 0x0400115E RID: 4446
	public const string syncDataSeparator = "|";

	// Token: 0x0400115F RID: 4447
	public const string syncDataItemSeparator = "\n";

	// Token: 0x04001161 RID: 4449
	public Dictionary<string, string> receivedUnassignedPersonBehaviorScriptVariables = new Dictionary<string, string>();
}
