using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

// Token: 0x0200029F RID: 671
public class SteamVR_LoadLevel : MonoBehaviour
{
	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06001946 RID: 6470 RVA: 0x000E5C4A File Offset: 0x000E404A
	public static bool loading
	{
		get
		{
			return SteamVR_LoadLevel._active != null;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06001947 RID: 6471 RVA: 0x000E5C57 File Offset: 0x000E4057
	public static float progress
	{
		get
		{
			return (!(SteamVR_LoadLevel._active != null) || SteamVR_LoadLevel._active.async == null) ? 0f : SteamVR_LoadLevel._active.async.progress;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06001948 RID: 6472 RVA: 0x000E5C91 File Offset: 0x000E4091
	public static Texture progressTexture
	{
		get
		{
			return (!(SteamVR_LoadLevel._active != null)) ? null : SteamVR_LoadLevel._active.renderTexture;
		}
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x000E5CB3 File Offset: 0x000E40B3
	private void OnEnable()
	{
		if (this.autoTriggerOnEnable)
		{
			this.Trigger();
		}
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x000E5CC6 File Offset: 0x000E40C6
	public void Trigger()
	{
		if (!SteamVR_LoadLevel.loading && !string.IsNullOrEmpty(this.levelName))
		{
			base.StartCoroutine("LoadLevel");
		}
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x000E5CF0 File Offset: 0x000E40F0
	public static void Begin(string levelName, bool showGrid = false, float fadeOutTime = 0.5f, float r = 0f, float g = 0f, float b = 0f, float a = 1f)
	{
		SteamVR_LoadLevel steamVR_LoadLevel = new GameObject("loader").AddComponent<SteamVR_LoadLevel>();
		steamVR_LoadLevel.levelName = levelName;
		steamVR_LoadLevel.showGrid = showGrid;
		steamVR_LoadLevel.fadeOutTime = fadeOutTime;
		steamVR_LoadLevel.backgroundColor = new Color(r, g, b, a);
		steamVR_LoadLevel.Trigger();
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x000E5D3C File Offset: 0x000E413C
	private void OnGUI()
	{
		if (SteamVR_LoadLevel._active != this)
		{
			return;
		}
		if (this.progressBarEmpty != null && this.progressBarFull != null)
		{
			if (this.progressBarOverlayHandle == 0UL)
			{
				this.progressBarOverlayHandle = this.GetOverlayHandle("progressBar", (!(this.progressBarTransform != null)) ? base.transform : this.progressBarTransform, this.progressBarWidthInMeters);
			}
			if (this.progressBarOverlayHandle != 0UL)
			{
				float num = ((this.async == null) ? 0f : this.async.progress);
				int width = this.progressBarFull.width;
				int height = this.progressBarFull.height;
				if (this.renderTexture == null)
				{
					this.renderTexture = new RenderTexture(width, height, 0);
					this.renderTexture.Create();
				}
				RenderTexture active = RenderTexture.active;
				RenderTexture.active = this.renderTexture;
				if (Event.current.type == EventType.Repaint)
				{
					GL.Clear(false, true, Color.clear);
				}
				GUILayout.BeginArea(new Rect(0f, 0f, (float)width, (float)height));
				GUI.DrawTexture(new Rect(0f, 0f, (float)width, (float)height), this.progressBarEmpty);
				GUI.DrawTextureWithTexCoords(new Rect(0f, 0f, num * (float)width, (float)height), this.progressBarFull, new Rect(0f, 0f, num, 1f));
				GUILayout.EndArea();
				RenderTexture.active = active;
				CVROverlay overlay = OpenVR.Overlay;
				if (overlay != null)
				{
					Texture_t texture_t = default(Texture_t);
					texture_t.handle = this.renderTexture.GetNativeTexturePtr();
					texture_t.eType = SteamVR.instance.graphicsAPI;
					texture_t.eColorSpace = EColorSpace.Auto;
					overlay.SetOverlayTexture(this.progressBarOverlayHandle, ref texture_t);
				}
			}
		}
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x000E5F28 File Offset: 0x000E4328
	private void Update()
	{
		if (SteamVR_LoadLevel._active != this)
		{
			return;
		}
		this.alpha = Mathf.Clamp01(this.alpha + this.fadeRate * Time.deltaTime);
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay != null)
		{
			if (this.loadingScreenOverlayHandle != 0UL)
			{
				overlay.SetOverlayAlpha(this.loadingScreenOverlayHandle, this.alpha);
			}
			if (this.progressBarOverlayHandle != 0UL)
			{
				overlay.SetOverlayAlpha(this.progressBarOverlayHandle, this.alpha);
			}
		}
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x000E5FB0 File Offset: 0x000E43B0
	private IEnumerator LoadLevel()
	{
		if (this.loadingScreen != null && this.loadingScreenDistance > 0f)
		{
			SteamVR_Controller.Device hmd = SteamVR_Controller.Input(0);
			while (!hmd.hasTracking)
			{
				yield return null;
			}
			SteamVR_Utils.RigidTransform tloading = hmd.transform;
			tloading.rot = Quaternion.Euler(0f, tloading.rot.eulerAngles.y, 0f);
			tloading.pos += tloading.rot * new Vector3(0f, 0f, this.loadingScreenDistance);
			Transform t = ((!(this.loadingScreenTransform != null)) ? base.transform : this.loadingScreenTransform);
			t.position = tloading.pos;
			t.rotation = tloading.rot;
		}
		SteamVR_LoadLevel._active = this;
		SteamVR_Utils.Event.Send("loading", new object[] { true });
		if (this.loadingScreenFadeInTime > 0f)
		{
			this.fadeRate = 1f / this.loadingScreenFadeInTime;
		}
		else
		{
			this.alpha = 1f;
		}
		CVROverlay overlay = OpenVR.Overlay;
		if (this.loadingScreen != null && overlay != null)
		{
			this.loadingScreenOverlayHandle = this.GetOverlayHandle("loadingScreen", (!(this.loadingScreenTransform != null)) ? base.transform : this.loadingScreenTransform, this.loadingScreenWidthInMeters);
			if (this.loadingScreenOverlayHandle != 0UL)
			{
				Texture_t texture_t = default(Texture_t);
				texture_t.handle = this.loadingScreen.GetNativeTexturePtr();
				texture_t.eType = SteamVR.instance.graphicsAPI;
				texture_t.eColorSpace = EColorSpace.Auto;
				overlay.SetOverlayTexture(this.loadingScreenOverlayHandle, ref texture_t);
			}
		}
		bool fadedForeground = false;
		SteamVR_Utils.Event.Send("loading_fade_out", new object[] { this.fadeOutTime });
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			if (this.front != null)
			{
				SteamVR_Skybox.SetOverride(this.front, this.back, this.left, this.right, this.top, this.bottom);
				compositor.FadeGrid(this.fadeOutTime, true);
				yield return new WaitForSeconds(this.fadeOutTime);
			}
			else if (this.backgroundColor != Color.clear)
			{
				if (this.showGrid)
				{
					compositor.FadeToColor(0f, this.backgroundColor.r, this.backgroundColor.g, this.backgroundColor.b, this.backgroundColor.a, true);
					compositor.FadeGrid(this.fadeOutTime, true);
					yield return new WaitForSeconds(this.fadeOutTime);
				}
				else
				{
					compositor.FadeToColor(this.fadeOutTime, this.backgroundColor.r, this.backgroundColor.g, this.backgroundColor.b, this.backgroundColor.a, false);
					yield return new WaitForSeconds(this.fadeOutTime + 0.1f);
					compositor.FadeGrid(0f, true);
					fadedForeground = true;
				}
			}
		}
		SteamVR_Render.pauseRendering = true;
		while (this.alpha < 1f)
		{
			yield return null;
		}
		base.transform.parent = null;
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!string.IsNullOrEmpty(this.internalProcessPath))
		{
			global::UnityEngine.Debug.Log("Launching external application...");
			CVRApplications applications = OpenVR.Applications;
			if (applications == null)
			{
				global::UnityEngine.Debug.Log("Failed to get OpenVR.Applications interface!");
			}
			else
			{
				string currentDirectory = Directory.GetCurrentDirectory();
				string text = Path.Combine(currentDirectory, this.internalProcessPath);
				global::UnityEngine.Debug.Log("LaunchingInternalProcess");
				global::UnityEngine.Debug.Log("ExternalAppPath = " + this.internalProcessPath);
				global::UnityEngine.Debug.Log("FullPath = " + text);
				global::UnityEngine.Debug.Log("ExternalAppArgs = " + this.internalProcessArgs);
				global::UnityEngine.Debug.Log("WorkingDirectory = " + currentDirectory);
				EVRApplicationError evrapplicationError = applications.LaunchInternalProcess(text, this.internalProcessArgs, currentDirectory);
				global::UnityEngine.Debug.Log("LaunchInternalProcessError: " + evrapplicationError);
				Process.GetCurrentProcess().Kill();
			}
		}
		else
		{
			LoadSceneMode mode = ((!this.loadAdditive) ? LoadSceneMode.Single : LoadSceneMode.Additive);
			if (this.loadAsync)
			{
				Application.backgroundLoadingPriority = ThreadPriority.Low;
				this.async = SceneManager.LoadSceneAsync(this.levelName, mode);
				while (!this.async.isDone)
				{
					yield return null;
				}
			}
			else
			{
				SceneManager.LoadScene(this.levelName, mode);
			}
		}
		yield return null;
		GC.Collect();
		yield return null;
		Shader.WarmupAllShaders();
		yield return new WaitForSeconds(this.postLoadSettleTime);
		SteamVR_Render.pauseRendering = false;
		if (this.loadingScreenFadeOutTime > 0f)
		{
			this.fadeRate = -1f / this.loadingScreenFadeOutTime;
		}
		else
		{
			this.alpha = 0f;
		}
		SteamVR_Utils.Event.Send("loading_fade_in", new object[] { this.fadeInTime });
		if (compositor != null)
		{
			if (fadedForeground)
			{
				compositor.FadeGrid(0f, false);
				compositor.FadeToColor(this.fadeInTime, 0f, 0f, 0f, 0f, false);
				yield return new WaitForSeconds(this.fadeInTime);
			}
			else
			{
				compositor.FadeGrid(this.fadeInTime, false);
				yield return new WaitForSeconds(this.fadeInTime);
				if (this.front != null)
				{
					SteamVR_Skybox.ClearOverride();
				}
			}
		}
		while (this.alpha > 0f)
		{
			yield return null;
		}
		if (overlay != null)
		{
			if (this.progressBarOverlayHandle != 0UL)
			{
				overlay.HideOverlay(this.progressBarOverlayHandle);
			}
			if (this.loadingScreenOverlayHandle != 0UL)
			{
				overlay.HideOverlay(this.loadingScreenOverlayHandle);
			}
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
		SteamVR_LoadLevel._active = null;
		SteamVR_Utils.Event.Send("loading", new object[] { false });
		yield break;
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x000E5FCC File Offset: 0x000E43CC
	private ulong GetOverlayHandle(string overlayName, Transform transform, float widthInMeters = 1f)
	{
		ulong num = 0UL;
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay == null)
		{
			return num;
		}
		string text = SteamVR_Overlay.key + "." + overlayName;
		EVROverlayError evroverlayError = overlay.FindOverlay(text, ref num);
		if (evroverlayError != EVROverlayError.None)
		{
			evroverlayError = overlay.CreateOverlay(text, overlayName, ref num);
		}
		if (evroverlayError == EVROverlayError.None)
		{
			overlay.ShowOverlay(num);
			overlay.SetOverlayAlpha(num, this.alpha);
			overlay.SetOverlayWidthInMeters(num, widthInMeters);
			if (SteamVR.instance.graphicsAPI == EGraphicsAPIConvention.API_DirectX)
			{
				VRTextureBounds_t vrtextureBounds_t = default(VRTextureBounds_t);
				vrtextureBounds_t.uMin = 0f;
				vrtextureBounds_t.vMin = 1f;
				vrtextureBounds_t.uMax = 1f;
				vrtextureBounds_t.vMax = 0f;
				overlay.SetOverlayTextureBounds(num, ref vrtextureBounds_t);
			}
			SteamVR_Camera steamVR_Camera = ((this.loadingScreenDistance != 0f) ? null : SteamVR_Render.Top());
			if (steamVR_Camera != null && steamVR_Camera.origin != null)
			{
				SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(steamVR_Camera.origin, transform);
				rigidTransform.pos.x = rigidTransform.pos.x / steamVR_Camera.origin.localScale.x;
				rigidTransform.pos.y = rigidTransform.pos.y / steamVR_Camera.origin.localScale.y;
				rigidTransform.pos.z = rigidTransform.pos.z / steamVR_Camera.origin.localScale.z;
				HmdMatrix34_t hmdMatrix34_t = rigidTransform.ToHmdMatrix34();
				overlay.SetOverlayTransformAbsolute(num, SteamVR_Render.instance.trackingSpace, ref hmdMatrix34_t);
			}
			else
			{
				SteamVR_Utils.RigidTransform rigidTransform2 = new SteamVR_Utils.RigidTransform(transform);
				HmdMatrix34_t hmdMatrix34_t2 = rigidTransform2.ToHmdMatrix34();
				overlay.SetOverlayTransformAbsolute(num, SteamVR_Render.instance.trackingSpace, ref hmdMatrix34_t2);
			}
		}
		return num;
	}

	// Token: 0x04001775 RID: 6005
	private static SteamVR_LoadLevel _active;

	// Token: 0x04001776 RID: 6006
	public string levelName;

	// Token: 0x04001777 RID: 6007
	public string internalProcessPath;

	// Token: 0x04001778 RID: 6008
	public string internalProcessArgs;

	// Token: 0x04001779 RID: 6009
	public bool loadAdditive;

	// Token: 0x0400177A RID: 6010
	public bool loadAsync = true;

	// Token: 0x0400177B RID: 6011
	public Texture loadingScreen;

	// Token: 0x0400177C RID: 6012
	public Texture progressBarEmpty;

	// Token: 0x0400177D RID: 6013
	public Texture progressBarFull;

	// Token: 0x0400177E RID: 6014
	public float loadingScreenWidthInMeters = 6f;

	// Token: 0x0400177F RID: 6015
	public float progressBarWidthInMeters = 3f;

	// Token: 0x04001780 RID: 6016
	public float loadingScreenDistance;

	// Token: 0x04001781 RID: 6017
	public Transform loadingScreenTransform;

	// Token: 0x04001782 RID: 6018
	public Transform progressBarTransform;

	// Token: 0x04001783 RID: 6019
	public Texture front;

	// Token: 0x04001784 RID: 6020
	public Texture back;

	// Token: 0x04001785 RID: 6021
	public Texture left;

	// Token: 0x04001786 RID: 6022
	public Texture right;

	// Token: 0x04001787 RID: 6023
	public Texture top;

	// Token: 0x04001788 RID: 6024
	public Texture bottom;

	// Token: 0x04001789 RID: 6025
	public Color backgroundColor = Color.black;

	// Token: 0x0400178A RID: 6026
	public bool showGrid;

	// Token: 0x0400178B RID: 6027
	public float fadeOutTime = 0.5f;

	// Token: 0x0400178C RID: 6028
	public float fadeInTime = 0.5f;

	// Token: 0x0400178D RID: 6029
	public float postLoadSettleTime;

	// Token: 0x0400178E RID: 6030
	public float loadingScreenFadeInTime = 1f;

	// Token: 0x0400178F RID: 6031
	public float loadingScreenFadeOutTime = 0.25f;

	// Token: 0x04001790 RID: 6032
	private float fadeRate = 1f;

	// Token: 0x04001791 RID: 6033
	private float alpha;

	// Token: 0x04001792 RID: 6034
	private AsyncOperation async;

	// Token: 0x04001793 RID: 6035
	private RenderTexture renderTexture;

	// Token: 0x04001794 RID: 6036
	private ulong loadingScreenOverlayHandle;

	// Token: 0x04001795 RID: 6037
	private ulong progressBarOverlayHandle;

	// Token: 0x04001796 RID: 6038
	public bool autoTriggerOnEnable;
}
