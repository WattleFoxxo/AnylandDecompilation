using System;
using UnityEngine;
using Valve.VR;

// Token: 0x020002A0 RID: 672
public class SteamVR_Menu : MonoBehaviour
{
	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06001952 RID: 6482 RVA: 0x000E6BA3 File Offset: 0x000E4FA3
	public RenderTexture texture
	{
		get
		{
			return (!this.overlay) ? null : (this.overlay.texture as RenderTexture);
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06001953 RID: 6483 RVA: 0x000E6BCB File Offset: 0x000E4FCB
	// (set) Token: 0x06001954 RID: 6484 RVA: 0x000E6BD3 File Offset: 0x000E4FD3
	public float scale { get; private set; }

	// Token: 0x06001955 RID: 6485 RVA: 0x000E6BDC File Offset: 0x000E4FDC
	private void Awake()
	{
		this.scaleLimitX = string.Format("{0:N1}", this.scaleLimits.x);
		this.scaleLimitY = string.Format("{0:N1}", this.scaleLimits.y);
		this.scaleRateText = string.Format("{0:N1}", this.scaleRate);
		SteamVR_Overlay instance = SteamVR_Overlay.instance;
		if (instance != null)
		{
			this.uvOffset = instance.uvOffset;
			this.distance = instance.distance;
		}
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x000E6C70 File Offset: 0x000E5070
	private void OnGUI()
	{
		if (this.overlay == null)
		{
			return;
		}
		RenderTexture renderTexture = this.overlay.texture as RenderTexture;
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		if (Event.current.type == EventType.Repaint)
		{
			GL.Clear(false, true, Color.clear);
		}
		Rect rect = new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height);
		if (Screen.width < renderTexture.width)
		{
			rect.width = (float)Screen.width;
			this.overlay.uvOffset.x = -(float)(renderTexture.width - Screen.width) / (float)(2 * renderTexture.width);
		}
		if (Screen.height < renderTexture.height)
		{
			rect.height = (float)Screen.height;
			this.overlay.uvOffset.y = (float)(renderTexture.height - Screen.height) / (float)(2 * renderTexture.height);
		}
		GUILayout.BeginArea(rect);
		if (this.background != null)
		{
			GUI.DrawTexture(new Rect((rect.width - (float)this.background.width) / 2f, (rect.height - (float)this.background.height) / 2f, (float)this.background.width, (float)this.background.height), this.background);
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		if (this.logo != null)
		{
			GUILayout.Space(rect.height / 2f - this.logoHeight);
			GUILayout.Box(this.logo, new GUILayoutOption[0]);
		}
		GUILayout.Space(this.menuOffset);
		bool flag = GUILayout.Button("[Esc] - Close menu", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(string.Format("Scale: {0:N4}", this.scale), new GUILayoutOption[0]);
		float num = GUILayout.HorizontalSlider(this.scale, this.scaleLimits.x, this.scaleLimits.y, new GUILayoutOption[0]);
		if (num != this.scale)
		{
			this.SetScale(num);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(string.Format("Scale limits:", new object[0]), new GUILayoutOption[0]);
		string text = GUILayout.TextField(this.scaleLimitX, new GUILayoutOption[0]);
		if (text != this.scaleLimitX && float.TryParse(text, out this.scaleLimits.x))
		{
			this.scaleLimitX = text;
		}
		string text2 = GUILayout.TextField(this.scaleLimitY, new GUILayoutOption[0]);
		if (text2 != this.scaleLimitY && float.TryParse(text2, out this.scaleLimits.y))
		{
			this.scaleLimitY = text2;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(string.Format("Scale rate:", new object[0]), new GUILayoutOption[0]);
		string text3 = GUILayout.TextField(this.scaleRateText, new GUILayoutOption[0]);
		if (text3 != this.scaleRateText && float.TryParse(text3, out this.scaleRate))
		{
			this.scaleRateText = text3;
		}
		GUILayout.EndHorizontal();
		if (SteamVR.active)
		{
			SteamVR instance = SteamVR.instance;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			float sceneResolutionScale = SteamVR_Camera.sceneResolutionScale;
			int num2 = (int)(instance.sceneWidth * sceneResolutionScale);
			int num3 = (int)(instance.sceneHeight * sceneResolutionScale);
			int num4 = (int)(100f * sceneResolutionScale);
			GUILayout.Label(string.Format("Scene quality: {0}x{1} ({2}%)", num2, num3, num4), new GUILayoutOption[0]);
			int num5 = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)num4, 50f, 200f, new GUILayoutOption[0]));
			if (num5 != num4)
			{
				SteamVR_Camera.sceneResolutionScale = (float)num5 / 100f;
			}
			GUILayout.EndHorizontal();
		}
		this.overlay.highquality = GUILayout.Toggle(this.overlay.highquality, "High quality", new GUILayoutOption[0]);
		if (this.overlay.highquality)
		{
			this.overlay.curved = GUILayout.Toggle(this.overlay.curved, "Curved overlay", new GUILayoutOption[0]);
			this.overlay.antialias = GUILayout.Toggle(this.overlay.antialias, "Overlay RGSS(2x2)", new GUILayoutOption[0]);
		}
		else
		{
			this.overlay.curved = false;
			this.overlay.antialias = false;
		}
		SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
		if (steamVR_Camera != null)
		{
			steamVR_Camera.wireframe = GUILayout.Toggle(steamVR_Camera.wireframe, "Wireframe", new GUILayoutOption[0]);
			SteamVR_Render instance2 = SteamVR_Render.instance;
			if (instance2.trackingSpace == ETrackingUniverseOrigin.TrackingUniverseSeated)
			{
				if (GUILayout.Button("Switch to Standing", new GUILayoutOption[0]))
				{
					instance2.trackingSpace = ETrackingUniverseOrigin.TrackingUniverseStanding;
				}
				if (GUILayout.Button("Center View", new GUILayoutOption[0]))
				{
					CVRSystem system = OpenVR.System;
					if (system != null)
					{
						system.ResetSeatedZeroPose();
					}
				}
			}
			else if (GUILayout.Button("Switch to Seated", new GUILayoutOption[0]))
			{
				instance2.trackingSpace = ETrackingUniverseOrigin.TrackingUniverseSeated;
			}
		}
		if (GUILayout.Button("Exit", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		GUILayout.Space(this.menuOffset);
		string environmentVariable = Environment.GetEnvironmentVariable("VR_OVERRIDE");
		if (environmentVariable != null)
		{
			GUILayout.Label("VR_OVERRIDE=" + environmentVariable, new GUILayoutOption[0]);
		}
		GUILayout.Label("Graphics device: " + SystemInfo.graphicsDeviceVersion, new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		if (this.cursor != null)
		{
			float x = Input.mousePosition.x;
			float num6 = (float)Screen.height - Input.mousePosition.y;
			float num7 = (float)this.cursor.width;
			float num8 = (float)this.cursor.height;
			GUI.DrawTexture(new Rect(x, num6, num7, num8), this.cursor);
		}
		RenderTexture.active = active;
		if (flag)
		{
			this.HideMenu();
		}
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x000E72D8 File Offset: 0x000E56D8
	public void ShowMenu()
	{
		SteamVR_Overlay instance = SteamVR_Overlay.instance;
		if (instance == null)
		{
			return;
		}
		RenderTexture renderTexture = instance.texture as RenderTexture;
		if (renderTexture == null)
		{
			Debug.LogError("Menu requires overlay texture to be a render texture.");
			return;
		}
		this.SaveCursorState();
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		this.overlay = instance;
		this.uvOffset = instance.uvOffset;
		this.distance = instance.distance;
		Camera[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(Camera)) as Camera[];
		foreach (Camera camera in array)
		{
			if (camera.enabled && camera.targetTexture == renderTexture)
			{
				this.overlayCam = camera;
				this.overlayCam.enabled = false;
				break;
			}
		}
		SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
		if (steamVR_Camera != null)
		{
			this.scale = steamVR_Camera.origin.localScale.x;
		}
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000E73E8 File Offset: 0x000E57E8
	public void HideMenu()
	{
		this.RestoreCursorState();
		if (this.overlayCam != null)
		{
			this.overlayCam.enabled = true;
		}
		if (this.overlay != null)
		{
			this.overlay.uvOffset = this.uvOffset;
			this.overlay.distance = this.distance;
			this.overlay = null;
		}
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000E7454 File Offset: 0x000E5854
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
		{
			if (this.overlay == null)
			{
				this.ShowMenu();
			}
			else
			{
				this.HideMenu();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Home))
		{
			this.SetScale(1f);
		}
		else if (Input.GetKey(KeyCode.PageUp))
		{
			this.SetScale(Mathf.Clamp(this.scale + this.scaleRate * Time.deltaTime, this.scaleLimits.x, this.scaleLimits.y));
		}
		else if (Input.GetKey(KeyCode.PageDown))
		{
			this.SetScale(Mathf.Clamp(this.scale - this.scaleRate * Time.deltaTime, this.scaleLimits.x, this.scaleLimits.y));
		}
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x000E7550 File Offset: 0x000E5950
	private void SetScale(float scale)
	{
		this.scale = scale;
		SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
		if (steamVR_Camera != null)
		{
			steamVR_Camera.origin.localScale = new Vector3(scale, scale, scale);
		}
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x000E7589 File Offset: 0x000E5989
	private void SaveCursorState()
	{
		this.savedCursorVisible = Cursor.visible;
		this.savedCursorLockState = Cursor.lockState;
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x000E75A1 File Offset: 0x000E59A1
	private void RestoreCursorState()
	{
		Cursor.visible = this.savedCursorVisible;
		Cursor.lockState = this.savedCursorLockState;
	}

	// Token: 0x04001797 RID: 6039
	public Texture cursor;

	// Token: 0x04001798 RID: 6040
	public Texture background;

	// Token: 0x04001799 RID: 6041
	public Texture logo;

	// Token: 0x0400179A RID: 6042
	public float logoHeight;

	// Token: 0x0400179B RID: 6043
	public float menuOffset;

	// Token: 0x0400179C RID: 6044
	public Vector2 scaleLimits = new Vector2(0.1f, 5f);

	// Token: 0x0400179D RID: 6045
	public float scaleRate = 0.5f;

	// Token: 0x0400179E RID: 6046
	private SteamVR_Overlay overlay;

	// Token: 0x0400179F RID: 6047
	private Camera overlayCam;

	// Token: 0x040017A0 RID: 6048
	private Vector4 uvOffset;

	// Token: 0x040017A1 RID: 6049
	private float distance;

	// Token: 0x040017A3 RID: 6051
	private string scaleLimitX;

	// Token: 0x040017A4 RID: 6052
	private string scaleLimitY;

	// Token: 0x040017A5 RID: 6053
	private string scaleRateText;

	// Token: 0x040017A6 RID: 6054
	private CursorLockMode savedCursorLockState;

	// Token: 0x040017A7 RID: 6055
	private bool savedCursorVisible;
}
