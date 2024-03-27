using System;
using System.IO;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000281 RID: 641
public class Universe : MonoBehaviour
{
	// Token: 0x1700028E RID: 654
	// (get) Token: 0x0600184B RID: 6219 RVA: 0x000E0417 File Offset: 0x000DE817
	// (set) Token: 0x0600184C RID: 6220 RVA: 0x000E041F File Offset: 0x000DE81F
	public string browserCachePath { get; private set; }

	// Token: 0x0600184D RID: 6221 RVA: 0x000E0428 File Offset: 0x000DE828
	private void Awake()
	{
		base.Invoke("ForcedFadeInInCaseOfIssues", 10f);
		this.SetAppropriateScreenSize();
		Universe.features.Init();
		if (!Universe.features.scriptsAsEditor)
		{
			Our.suppressBehaviorScriptsAsEditor = true;
		}
		this.InitBrowser();
		this.InitTransmitFromMicrophone();
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x000E0476 File Offset: 0x000DE876
	private void Start()
	{
		CrossDevice.Init();
		Misc.DeleteCookiesFileIfExists();
		base.Invoke("RegisterSessionExceeded15Minutes", 900f);
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x000E0494 File Offset: 0x000DE894
	private void InitTransmitFromMicrophone()
	{
		int num = 1;
		Universe.transmitFromMicrophone = PlayerPrefs.GetInt("transmitFromMicrophone", num) == 1;
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x000E04B6 File Offset: 0x000DE8B6
	private void InitBrowser()
	{
		this.browserCachePath = Application.persistentDataPath + "/cache/browser";
		Directory.CreateDirectory(this.browserCachePath);
		BrowserNative.ProfilePath = this.browserCachePath;
		UserAgent.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36");
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x000E04EE File Offset: 0x000DE8EE
	private void ForcedFadeInInCaseOfIssues()
	{
		if (!(Managers.areaManager != null) || !Managers.areaManager.startedLaunchFadeIn)
		{
			Log.Debug("Doing ForcedFadeIn");
			SteamVR_Fade.Start(Color.clear, 2f, false);
		}
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x000E0529 File Offset: 0x000DE929
	private void RegisterSessionExceeded15Minutes()
	{
		Managers.achievementManager.RegisterAchievement(Achievement.SessionExceeded15Minutes);
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x000E0537 File Offset: 0x000DE937
	private void Update()
	{
		if (CrossDevice.type == global::DeviceType.OculusTouch && ++Universe.counterForOculusTouchBuzzIssue >= 100000)
		{
			Universe.counterForOculusTouchBuzzIssue = 0;
		}
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x000E0561 File Offset: 0x000DE961
	private void SetAppropriateScreenSize()
	{
		this.DeleteScreenRelatedAutoPlayerPrefs();
		Universe.useWindowedApp = PlayerPrefs.GetInt("useWindowedApp", 0) == 1;
		Universe.ApplyUseWindowedApp();
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x000E0584 File Offset: 0x000DE984
	private static void ApplyUseWindowedApp()
	{
		int num = Screen.currentResolution.width;
		int num2 = Screen.currentResolution.height;
		if (Universe.useWindowedApp)
		{
			num = (int)Mathf.Round((float)num * 0.5f);
			num2 = (int)Mathf.Round((float)num2 * 0.5f);
		}
		Screen.SetResolution(num, num2, !Universe.useWindowedApp);
		if (Universe.useWindowedApp)
		{
			if (Screen.fullScreen)
			{
				Screen.fullScreen = false;
			}
		}
		else if (!Screen.fullScreen)
		{
			Screen.fullScreen = true;
		}
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x000E0613 File Offset: 0x000DEA13
	private void DeleteScreenRelatedAutoPlayerPrefs()
	{
		PlayerPrefs.DeleteKey("Screenmanager Is Fullscreen mode");
		PlayerPrefs.DeleteKey("Screenmanager Resolution Height");
		PlayerPrefs.DeleteKey("Screenmanager Resolution Width");
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x000E0633 File Offset: 0x000DEA33
	public static void SetTransmitFromMicrophone(bool state)
	{
		Universe.transmitFromMicrophone = state;
		PlayerPrefs.SetInt("transmitFromMicrophone", (!state) ? 0 : 1);
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x000E0652 File Offset: 0x000DEA52
	public static void SetUseWindowedApp(bool state)
	{
		Universe.useWindowedApp = state;
		PlayerPrefs.SetInt("useWindowedApp", (!state) ? 0 : 1);
		Universe.ApplyUseWindowedApp();
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x000E0676 File Offset: 0x000DEA76
	public static string GetClientVersionDisplay()
	{
		return Universe.versionMajorToEnforceUpdates.ToString() + "." + Universe.versionMinorClientOnly.ToString();
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x000E06A4 File Offset: 0x000DEAA4
	public static string GetServerVersionDisplay()
	{
		string text = "...";
		if (Universe.versionMajorAsToldByServer != string.Empty || Universe.versionMinorServerOnly != string.Empty)
		{
			text = Universe.versionMajorAsToldByServer.ToString() + ".s" + Universe.versionMinorServerOnly;
		}
		return text;
	}

	// Token: 0x040016A4 RID: 5796
	public static int versionMajorToEnforceUpdates = 188;

	// Token: 0x040016A5 RID: 5797
	public static int versionMinorClientOnly = 1;

	// Token: 0x040016A6 RID: 5798
	public static bool showEffects = true;

	// Token: 0x040016A7 RID: 5799
	public static bool checkForVrDevice = false;

	// Token: 0x040016A8 RID: 5800
	public static bool transmitFromMicrophone = true;

	// Token: 0x040016A9 RID: 5801
	public static bool hearEchoOfMyVoice = false;

	// Token: 0x040016AA RID: 5802
	public const bool controllablesAreActive = false;

	// Token: 0x040016AB RID: 5803
	public static ushort veryLowHapticPulse = 50;

	// Token: 0x040016AC RID: 5804
	public static ushort lowHapticPulse = 250;

	// Token: 0x040016AD RID: 5805
	public static ushort mediumHapticPulse = 800;

	// Token: 0x040016AE RID: 5806
	public static ushort highHapticPulse = 1100;

	// Token: 0x040016AF RID: 5807
	public static ushort miniBurstPulse = 1500;

	// Token: 0x040016B0 RID: 5808
	public static float maxLightIntensity = 8f;

	// Token: 0x040016B1 RID: 5809
	public static float defaultLightIntensity = 1f;

	// Token: 0x040016B2 RID: 5810
	public static float maxLightConeSize = 170f;

	// Token: 0x040016B3 RID: 5811
	private static int playerPrefsVersion = 12;

	// Token: 0x040016B4 RID: 5812
	public static string versionMajorAsToldByServer = string.Empty;

	// Token: 0x040016B5 RID: 5813
	public static string versionMinorServerOnly = string.Empty;

	// Token: 0x040016B6 RID: 5814
	public static string objectNameIfAlreadyDestroyed = "(already_destroyed)";

	// Token: 0x040016B7 RID: 5815
	public const string transmitFromMicrophone_key = "transmitFromMicrophone";

	// Token: 0x040016B8 RID: 5816
	public static bool useWindowedApp = false;

	// Token: 0x040016B9 RID: 5817
	public const string useWindowedApp_key = "useWindowedApp";

	// Token: 0x040016BA RID: 5818
	public static int counterForOculusTouchBuzzIssue = 0;

	// Token: 0x040016BB RID: 5819
	public static bool controllerNotOnDuringStartIssueOccurred = false;

	// Token: 0x040016BC RID: 5820
	public const int minScalePercentAsNonEditor = 1;

	// Token: 0x040016BD RID: 5821
	public const int maxScalePercentAsNonEditor = 150;

	// Token: 0x040016BE RID: 5822
	public const int minScalePercentAsEditor = 1;

	// Token: 0x040016BF RID: 5823
	public const int maxScalePercentAsEditor = 2500;

	// Token: 0x040016C0 RID: 5824
	public const float normalCameraNearClipPlane = 0.01f;

	// Token: 0x040016C1 RID: 5825
	public const float normalCameraFarClipPlane = 10000f;

	// Token: 0x040016C2 RID: 5826
	public const int maxTagLength = 50;

	// Token: 0x040016C3 RID: 5827
	public const int maxCustomSearchWords = 200;

	// Token: 0x040016C4 RID: 5828
	public static TogglableFeatures features = new TogglableFeatures();

	// Token: 0x040016C5 RID: 5829
	public const float maxGravity = 1000f;

	// Token: 0x040016C6 RID: 5830
	public const float defaultGravity = -9.81f;

	// Token: 0x040016C8 RID: 5832
	public static bool demoMode = false;

	// Token: 0x040016C9 RID: 5833
	public static bool showPlacementsAtAnyDistance = false;
}
