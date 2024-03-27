using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020001F2 RID: 498
[RequireComponent(typeof(ErrorManager))]
[RequireComponent(typeof(TreeManager))]
[RequireComponent(typeof(DialogManager))]
[RequireComponent(typeof(OptimizationManager))]
[RequireComponent(typeof(PersonManager))]
[RequireComponent(typeof(ThingManager))]
[RequireComponent(typeof(SteamManager))]
[RequireComponent(typeof(PollManager))]
[RequireComponent(typeof(AchievementManager))]
[RequireComponent(typeof(ServerManager))]
[RequireComponent(typeof(BroadcastNetworkManager))]
[RequireComponent(typeof(FilterManager))]
[RequireComponent(typeof(AreaManager))]
[RequireComponent(typeof(SoundLibraryManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(SoundManager))]
[RequireComponent(typeof(VideoManager))]
[RequireComponent(typeof(BrowserManager))]
[RequireComponent(typeof(CameraManager))]
[RequireComponent(typeof(ForumManager))]
[RequireComponent(typeof(CreationPartChangeManager))]
[RequireComponent(typeof(UniverseManager))]
[RequireComponent(typeof(ExtrasManager))]
[RequireComponent(typeof(DesktopManager))]
[RequireComponent(typeof(BehaviorScriptManager))]
[RequireComponent(typeof(SettingManager))]
[RequireComponent(typeof(MovableByEveryoneManager))]
public class Managers : MonoBehaviour
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06001140 RID: 4416 RVA: 0x0009440E File Offset: 0x0009280E
	// (set) Token: 0x06001141 RID: 4417 RVA: 0x00094415 File Offset: 0x00092815
	public static ErrorManager errorManager { get; private set; }

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06001142 RID: 4418 RVA: 0x0009441D File Offset: 0x0009281D
	// (set) Token: 0x06001143 RID: 4419 RVA: 0x00094424 File Offset: 0x00092824
	public static TreeManager treeManager { get; private set; }

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06001144 RID: 4420 RVA: 0x0009442C File Offset: 0x0009282C
	// (set) Token: 0x06001145 RID: 4421 RVA: 0x00094433 File Offset: 0x00092833
	public static DialogManager dialogManager { get; private set; }

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06001146 RID: 4422 RVA: 0x0009443B File Offset: 0x0009283B
	// (set) Token: 0x06001147 RID: 4423 RVA: 0x00094442 File Offset: 0x00092842
	public static OptimizationManager optimizationManager { get; private set; }

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06001148 RID: 4424 RVA: 0x0009444A File Offset: 0x0009284A
	// (set) Token: 0x06001149 RID: 4425 RVA: 0x00094451 File Offset: 0x00092851
	public static PersonManager personManager { get; private set; }

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x0600114A RID: 4426 RVA: 0x00094459 File Offset: 0x00092859
	// (set) Token: 0x0600114B RID: 4427 RVA: 0x00094460 File Offset: 0x00092860
	public static ThingManager thingManager { get; private set; }

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x0600114C RID: 4428 RVA: 0x00094468 File Offset: 0x00092868
	// (set) Token: 0x0600114D RID: 4429 RVA: 0x0009446F File Offset: 0x0009286F
	public static SteamManager steamManager { get; private set; }

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x0600114E RID: 4430 RVA: 0x00094477 File Offset: 0x00092877
	// (set) Token: 0x0600114F RID: 4431 RVA: 0x0009447E File Offset: 0x0009287E
	public static PollManager pollManager { get; private set; }

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06001150 RID: 4432 RVA: 0x00094486 File Offset: 0x00092886
	// (set) Token: 0x06001151 RID: 4433 RVA: 0x0009448D File Offset: 0x0009288D
	public static AchievementManager achievementManager { get; private set; }

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06001152 RID: 4434 RVA: 0x00094495 File Offset: 0x00092895
	// (set) Token: 0x06001153 RID: 4435 RVA: 0x0009449C File Offset: 0x0009289C
	public static ServerManager serverManager { get; private set; }

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06001154 RID: 4436 RVA: 0x000944A4 File Offset: 0x000928A4
	// (set) Token: 0x06001155 RID: 4437 RVA: 0x000944AB File Offset: 0x000928AB
	public static BroadcastNetworkManager broadcastNetworkManager { get; private set; }

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06001156 RID: 4438 RVA: 0x000944B3 File Offset: 0x000928B3
	// (set) Token: 0x06001157 RID: 4439 RVA: 0x000944BA File Offset: 0x000928BA
	public static SkyManager skyManager { get; private set; }

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06001158 RID: 4440 RVA: 0x000944C2 File Offset: 0x000928C2
	// (set) Token: 0x06001159 RID: 4441 RVA: 0x000944C9 File Offset: 0x000928C9
	public static FilterManager filterManager { get; private set; }

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x0600115A RID: 4442 RVA: 0x000944D1 File Offset: 0x000928D1
	// (set) Token: 0x0600115B RID: 4443 RVA: 0x000944D8 File Offset: 0x000928D8
	public static AreaManager areaManager { get; private set; }

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x0600115C RID: 4444 RVA: 0x000944E0 File Offset: 0x000928E0
	// (set) Token: 0x0600115D RID: 4445 RVA: 0x000944E7 File Offset: 0x000928E7
	public static TemporarilyDestroyedThingsManager temporarilyDestroyedThingsManager { get; private set; }

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x0600115E RID: 4446 RVA: 0x000944EF File Offset: 0x000928EF
	// (set) Token: 0x0600115F RID: 4447 RVA: 0x000944F6 File Offset: 0x000928F6
	public static SoundLibraryManager soundLibraryManager { get; private set; }

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06001160 RID: 4448 RVA: 0x000944FE File Offset: 0x000928FE
	// (set) Token: 0x06001161 RID: 4449 RVA: 0x00094505 File Offset: 0x00092905
	public static InventoryManager inventoryManager { get; private set; }

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06001162 RID: 4450 RVA: 0x0009450D File Offset: 0x0009290D
	// (set) Token: 0x06001163 RID: 4451 RVA: 0x00094514 File Offset: 0x00092914
	public static SoundManager soundManager { get; private set; }

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06001164 RID: 4452 RVA: 0x0009451C File Offset: 0x0009291C
	// (set) Token: 0x06001165 RID: 4453 RVA: 0x00094523 File Offset: 0x00092923
	public static VideoManager videoManager { get; private set; }

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06001166 RID: 4454 RVA: 0x0009452B File Offset: 0x0009292B
	// (set) Token: 0x06001167 RID: 4455 RVA: 0x00094532 File Offset: 0x00092932
	public static BrowserManager browserManager { get; private set; }

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06001168 RID: 4456 RVA: 0x0009453A File Offset: 0x0009293A
	// (set) Token: 0x06001169 RID: 4457 RVA: 0x00094541 File Offset: 0x00092941
	public static CameraManager cameraManager { get; private set; }

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x0600116A RID: 4458 RVA: 0x00094549 File Offset: 0x00092949
	// (set) Token: 0x0600116B RID: 4459 RVA: 0x00094550 File Offset: 0x00092950
	public static ForumManager forumManager { get; private set; }

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x0600116C RID: 4460 RVA: 0x00094558 File Offset: 0x00092958
	// (set) Token: 0x0600116D RID: 4461 RVA: 0x0009455F File Offset: 0x0009295F
	public static CreationPartChangeManager creationPartChangeManager { get; private set; }

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x0600116E RID: 4462 RVA: 0x00094567 File Offset: 0x00092967
	// (set) Token: 0x0600116F RID: 4463 RVA: 0x0009456E File Offset: 0x0009296E
	public static TwitchManager twitchManager { get; private set; }

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06001170 RID: 4464 RVA: 0x00094576 File Offset: 0x00092976
	// (set) Token: 0x06001171 RID: 4465 RVA: 0x0009457D File Offset: 0x0009297D
	public static UniverseManager universeManager { get; private set; }

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06001172 RID: 4466 RVA: 0x00094585 File Offset: 0x00092985
	// (set) Token: 0x06001173 RID: 4467 RVA: 0x0009458C File Offset: 0x0009298C
	public static ExtrasManager extrasManager { get; private set; }

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06001174 RID: 4468 RVA: 0x00094594 File Offset: 0x00092994
	// (set) Token: 0x06001175 RID: 4469 RVA: 0x0009459B File Offset: 0x0009299B
	public static DesktopManager desktopManager { get; private set; }

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06001176 RID: 4470 RVA: 0x000945A3 File Offset: 0x000929A3
	// (set) Token: 0x06001177 RID: 4471 RVA: 0x000945AA File Offset: 0x000929AA
	public static BehaviorScriptManager behaviorScriptManager { get; private set; }

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06001178 RID: 4472 RVA: 0x000945B2 File Offset: 0x000929B2
	// (set) Token: 0x06001179 RID: 4473 RVA: 0x000945B9 File Offset: 0x000929B9
	public static SettingManager settingManager { get; private set; }

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x0600117A RID: 4474 RVA: 0x000945C1 File Offset: 0x000929C1
	// (set) Token: 0x0600117B RID: 4475 RVA: 0x000945C8 File Offset: 0x000929C8
	public static MovableByEveryoneManager movableByEveryoneManager { get; private set; }

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x0600117C RID: 4476 RVA: 0x000945D0 File Offset: 0x000929D0
	// (set) Token: 0x0600117D RID: 4477 RVA: 0x000945D7 File Offset: 0x000929D7
	public static QuestManager questManager { get; private set; }

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600117E RID: 4478 RVA: 0x000945E0 File Offset: 0x000929E0
	// (remove) Token: 0x0600117F RID: 4479 RVA: 0x00094614 File Offset: 0x00092A14
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event EventHandler StartupCompleted;

	// Token: 0x06001180 RID: 4480 RVA: 0x00094648 File Offset: 0x00092A48
	private void Awake()
	{
		Managers.errorManager = base.GetComponent<ErrorManager>();
		Managers.treeManager = base.GetComponent<TreeManager>();
		Managers.dialogManager = base.GetComponent<DialogManager>();
		Managers.optimizationManager = base.GetComponent<OptimizationManager>();
		Managers.personManager = base.GetComponent<PersonManager>();
		Managers.thingManager = base.GetComponent<ThingManager>();
		Managers.steamManager = base.GetComponent<SteamManager>();
		Managers.pollManager = base.GetComponent<PollManager>();
		Managers.achievementManager = base.GetComponent<AchievementManager>();
		Managers.serverManager = base.GetComponent<ServerManager>();
		Managers.broadcastNetworkManager = base.GetComponent<BroadcastNetworkManager>();
		Managers.skyManager = base.GetComponent<SkyManager>();
		Managers.filterManager = base.GetComponent<FilterManager>();
		Managers.areaManager = base.GetComponent<AreaManager>();
		Managers.temporarilyDestroyedThingsManager = base.GetComponent<TemporarilyDestroyedThingsManager>();
		Managers.soundLibraryManager = base.GetComponent<SoundLibraryManager>();
		Managers.inventoryManager = base.GetComponent<InventoryManager>();
		Managers.soundManager = base.GetComponent<SoundManager>();
		Managers.videoManager = base.GetComponent<VideoManager>();
		Managers.browserManager = base.GetComponent<BrowserManager>();
		Managers.cameraManager = base.GetComponent<CameraManager>();
		Managers.forumManager = base.GetComponent<ForumManager>();
		Managers.creationPartChangeManager = base.GetComponent<CreationPartChangeManager>();
		Managers.twitchManager = base.GetComponent<TwitchManager>();
		Managers.universeManager = base.GetComponent<UniverseManager>();
		Managers.extrasManager = base.GetComponent<ExtrasManager>();
		Managers.desktopManager = base.GetComponent<DesktopManager>();
		Managers.behaviorScriptManager = base.GetComponent<BehaviorScriptManager>();
		Managers.settingManager = base.GetComponent<SettingManager>();
		Managers.movableByEveryoneManager = base.GetComponent<MovableByEveryoneManager>();
		Managers.questManager = base.GetComponent<QuestManager>();
		this._startSequence = new List<IGameManager>();
		this._startSequence.Add(Managers.errorManager);
		this._startSequence.Add(Managers.treeManager);
		this._startSequence.Add(Managers.dialogManager);
		this._startSequence.Add(Managers.optimizationManager);
		this._startSequence.Add(Managers.personManager);
		this._startSequence.Add(Managers.thingManager);
		this._startSequence.Add(Managers.steamManager);
		this._startSequence.Add(Managers.pollManager);
		this._startSequence.Add(Managers.achievementManager);
		this._startSequence.Add(Managers.serverManager);
		this._startSequence.Add(Managers.broadcastNetworkManager);
		this._startSequence.Add(Managers.skyManager);
		this._startSequence.Add(Managers.filterManager);
		this._startSequence.Add(Managers.areaManager);
		this._startSequence.Add(Managers.temporarilyDestroyedThingsManager);
		this._startSequence.Add(Managers.soundLibraryManager);
		this._startSequence.Add(Managers.inventoryManager);
		this._startSequence.Add(Managers.soundManager);
		this._startSequence.Add(Managers.videoManager);
		this._startSequence.Add(Managers.browserManager);
		this._startSequence.Add(Managers.cameraManager);
		this._startSequence.Add(Managers.forumManager);
		this._startSequence.Add(Managers.creationPartChangeManager);
		this._startSequence.Add(Managers.twitchManager);
		this._startSequence.Add(Managers.universeManager);
		this._startSequence.Add(Managers.extrasManager);
		this._startSequence.Add(Managers.desktopManager);
		this._startSequence.Add(Managers.behaviorScriptManager);
		this._startSequence.Add(Managers.settingManager);
		this._startSequence.Add(Managers.movableByEveryoneManager);
		this._startSequence.Add(Managers.questManager);
		base.StartCoroutine(this.StartupManagers());
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x000949B4 File Offset: 0x00092DB4
	private IEnumerator StartupManagers()
	{
		Log.Info("MANAGERS : Starting managers", false);
		bool someManagersFailed = false;
		foreach (IGameManager manager in this._startSequence)
		{
			if (manager != null)
			{
				Log.Info("MANAGERS : " + manager.GetType().Name + " starting.", false);
				if (manager.status == ManagerStatus.Started)
				{
					Log.Info("MANAGERS : " + manager.GetType().Name + " already started.  Skipping startup.", false);
				}
				else
				{
					manager.Startup();
					while (manager.status == ManagerStatus.Initializing)
					{
						yield return null;
					}
					ManagerStatus status = manager.status;
					if (status != ManagerStatus.Started)
					{
						if (status != ManagerStatus.Failed)
						{
							Log.Error(string.Concat(new object[]
							{
								"MANAGERS : ",
								manager.GetType().Name,
								" - Uknown manager status : ",
								manager.status
							}));
						}
						else
						{
							Log.Info("MANAGERS : " + manager.GetType().Name + " failed to start - aborting startup", false);
							if (Managers.errorManager != null)
							{
								if (string.IsNullOrEmpty(manager.failMessage))
								{
									Managers.errorManager.ShowCriticalHaltError(manager.GetType().Name + " failed to start.", true, false, false, false, false);
								}
								else
								{
									Managers.errorManager.ShowCriticalHaltError(manager.failMessage, true, false, false, false, false);
								}
							}
							else
							{
								Log.Error("MANAGERS : " + manager.GetType().Name + " failed to start.");
							}
							someManagersFailed = true;
						}
					}
					else
					{
						Log.Info("MANAGERS : " + manager.GetType().Name + " finished startup.", false);
					}
					if (someManagersFailed)
					{
						break;
					}
				}
			}
			else
			{
				Log.Error("MANAGERS : An unknown manager has failed to instantiate");
				someManagersFailed = true;
			}
		}
		if (someManagersFailed)
		{
			Log.Warning("MANAGERS : Some managers failed to start up correctly");
		}
		else
		{
			Log.Info("MANAGERS : All managers started up successfully", false);
			this.OnStartupCompleted(new EventArgs());
		}
		yield break;
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x000949CF File Offset: 0x00092DCF
	protected virtual void OnStartupCompleted(EventArgs e)
	{
		if (Managers.StartupCompleted != null)
		{
			Managers.StartupCompleted(this, e);
		}
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x000949E7 File Offset: 0x00092DE7
	public static void ClearStartupCompletedHandler()
	{
		Managers.StartupCompleted = null;
	}

	// Token: 0x04001128 RID: 4392
	private List<IGameManager> _startSequence;
}
