using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubeLight;

// Token: 0x020002C8 RID: 712
public class YoutubePlayer : MonoBehaviour
{
	// Token: 0x06001A78 RID: 6776 RVA: 0x000EE0F0 File Offset: 0x000EC4F0
	private void Awake()
	{
		if (PlayerPrefs.HasKey("utube_config"))
		{
			Debug.Log("Load Utube auto updater data from device");
			this.magicResult = new YoutubePlayer.MagicContent();
			this.magicResult.regexForFuncName = PlayerPrefsX.GetStringArray("utube_regex_funcName");
			this.magicResult.regexForHtmlJson = PlayerPrefs.GetString("utube_regex_htmlJson");
			this.magicResult.regexForHtmlPlayerVersion = PlayerPrefs.GetString("utube_regex_htmlPlayerVersion");
		}
		else
		{
			Debug.Log("create auto updater for utube");
			PlayerPrefs.SetInt("utube_config", 1);
			this.magicResult = new YoutubePlayer.MagicContent();
			PlayerPrefsX.SetStringArray("utube_regex_funcName", this.magicResult.defaultFuncName);
			PlayerPrefs.SetString("utube_regex_htmlJson", this.magicResult.defaultHtmlJson);
			PlayerPrefs.SetString("utube_regex_htmlPlayerVersion", this.magicResult.defaultHtmlPlayerVersion);
			this.magicResult.regexForFuncName = PlayerPrefsX.GetStringArray("utube_regex_funcName");
			this.magicResult.regexForHtmlJson = PlayerPrefs.GetString("utube_regex_htmlJson");
			this.magicResult.regexForHtmlPlayerVersion = PlayerPrefs.GetString("utube_regex_htmlPlayerVersion");
		}
		if (!this.playUsingInternalDevicePlayer && !this.loadYoutubeUrlsOnly)
		{
			if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
			{
				this.videoPlayer.skipOnDrop = this._skipOnDrop;
				if (this.audioPlayer != null)
				{
					this.audioPlayer.transform.gameObject.SetActive(false);
				}
			}
			if (this.videoPlayer.renderMode == VideoRenderMode.CameraFarPlane || this.videoPlayer.renderMode == VideoRenderMode.CameraNearPlane)
			{
				this.fullscreenModeEnabled = true;
			}
			else
			{
				this.fullscreenModeEnabled = false;
			}
		}
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x000EE291 File Offset: 0x000EC691
	private void UpdateRegexMethodsFromServer()
	{
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x000EE294 File Offset: 0x000EC694
	private IEnumerator CallServerForUpdate()
	{
		UnityWebRequest request = UnityWebRequest.Get("http://test-youtube-unity.herokuapp.com/api/regexUpdater");
		yield return request.SendWebRequest();
		if (request.isDone)
		{
			this.RegexUpdaterLoaded(JSON.Parse(request.downloadHandler.text));
		}
		yield break;
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x000EE2B0 File Offset: 0x000EC6B0
	private void RegexUpdaterLoaded(JSONNode regexJsonData)
	{
		if (regexJsonData["patterns"] == null)
		{
			PlayerPrefs.SetInt("utube_config", 1);
			this.magicResult = new YoutubePlayer.MagicContent();
			PlayerPrefsX.SetStringArray("utube_regex_funcName", this.magicResult.defaultFuncName);
			PlayerPrefs.SetString("utube_regex_htmlJson", this.magicResult.defaultHtmlJson);
			PlayerPrefs.SetString("utube_regex_htmlPlayerVersion", this.magicResult.defaultHtmlPlayerVersion);
			this.magicResult.regexForFuncName = PlayerPrefsX.GetStringArray("utube_regex_funcName");
			this.magicResult.regexForHtmlJson = PlayerPrefs.GetString("utube_regex_htmlJson");
			this.magicResult.regexForHtmlPlayerVersion = PlayerPrefs.GetString("utube_regex_htmlPlayerVersion");
		}
		else
		{
			JSONArray asArray = regexJsonData["patterns"].AsArray;
			string[] array = new string[asArray.Count];
			for (int i = 0; i < asArray.Count; i++)
			{
				string value = asArray[i].Value;
				array[i] = value;
			}
			this.magicResult = new YoutubePlayer.MagicContent();
			PlayerPrefsX.SetStringArray("utube_regex_funcName", array);
			PlayerPrefs.SetString("utube_regex_htmlJson", regexJsonData["htmlJson"]);
			PlayerPrefs.SetString("utube_regex_htmlPlayerVersion", regexJsonData["htmlPlayerVersion"]);
			this.magicResult.regexForFuncName = PlayerPrefsX.GetStringArray("utube_regex_funcName");
			this.magicResult.regexForHtmlJson = PlayerPrefs.GetString("utube_regex_htmlJson");
			this.magicResult.regexForHtmlPlayerVersion = PlayerPrefs.GetString("utube_regex_htmlPlayerVersion");
			Debug.Log("<color='yellow'>Regex updated from server, if the error continues mail support at kelvinparkour@gmail.com</color>");
		}
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x000EE448 File Offset: 0x000EC848
	public void Start()
	{
		if (this.playUsingInternalDevicePlayer)
		{
			this.loadYoutubeUrlsOnly = true;
		}
		if (!this.loadYoutubeUrlsOnly)
		{
			base.Invoke("VerifyFrames", 2f);
			this.FixCameraEvent();
			this.Skybox3DSettup();
			if (this.videoFormat == YoutubePlayer.VideoFormatType.WEBM)
			{
				this.videoPlayer.skipOnDrop = this._skipOnDrop;
				this.audioPlayer.skipOnDrop = this._skipOnDrop;
			}
			this.audioPlayer.seekCompleted += this.AudioSeeked;
			this.videoPlayer.seekCompleted += this.VideoSeeked;
		}
		this.PrepareVideoPlayerCallbacks();
		if (this.autoPlayOnStart)
		{
			if (this.customPlaylist)
			{
				this.PlayYoutubeVideo(this.youtubeUrls[this.currentUrlIndex]);
			}
			else
			{
				this.PlayYoutubeVideo(this.youtubeUrl);
			}
		}
		if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.lowRes = true;
		}
		else
		{
			this.lowRes = false;
		}
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x000EE548 File Offset: 0x000EC948
	public void CallNextUrl()
	{
		if (!this.customPlaylist)
		{
			return;
		}
		if (this.currentUrlIndex + 1 < this.youtubeUrls.Length)
		{
			this.currentUrlIndex++;
		}
		else
		{
			this.currentUrlIndex = 0;
		}
		this.PlayYoutubeVideo(this.youtubeUrls[this.currentUrlIndex]);
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000EE5A4 File Offset: 0x000EC9A4
	private void TryToLoadThumbnailBeforeOpenVideo(string id)
	{
		string text = id.Replace("https://youtube.com/watch?v=", string.Empty);
		base.StartCoroutine(this.DownloadThumbnail(text));
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000EE5D0 File Offset: 0x000EC9D0
	private IEnumerator DownloadThumbnail(string videoId)
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://img.youtube.com/vi/" + videoId + "/0.jpg");
		yield return request.SendWebRequest();
		Texture2D thumb = DownloadHandlerTexture.GetContent(request);
		this.videoPlayer.targetMaterialRenderer.material.mainTexture = thumb;
		yield break;
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x000EE5F4 File Offset: 0x000EC9F4
	private void Skybox3DSettup()
	{
		if (this.is3DLayoutVideo)
		{
			if (this.layout3d == YoutubePlayer.Layout3D.OverUnder)
			{
				RenderSettings.skybox = (Material)Resources.Load("Materials/PanoramicSkybox3DOverUnder");
			}
			else if (this.layout3d == YoutubePlayer.Layout3D.sideBySide)
			{
				RenderSettings.skybox = (Material)Resources.Load("Materials/PanoramicSkybox3Dside");
			}
		}
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x000EE650 File Offset: 0x000ECA50
	public void ToogleFullsScreenMode()
	{
		this.fullscreenModeEnabled = !this.fullscreenModeEnabled;
		if (!this.fullscreenModeEnabled)
		{
			this.videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
			if (this.videoPlayer.targetCamera == null)
			{
				this.videoPlayer.targetCamera = this.mainCamera;
			}
		}
		else
		{
			this.videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
		}
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x000EE6BC File Offset: 0x000ECABC
	private void FixCameraEvent()
	{
		if (this.mainCamera == null)
		{
			if (Camera.main != null)
			{
				this.mainCamera = Camera.main;
			}
			else
			{
				this.mainCamera = global::UnityEngine.Object.FindObjectOfType<Camera>();
				Debug.Log("Add the main camera to the mainCamera field");
			}
		}
		if (this.videoControllerCanvas != null)
		{
			Canvas component = this.videoControllerCanvas.GetComponent<Canvas>();
			if (component != null)
			{
				component.worldCamera = this.mainCamera;
			}
		}
		if (this.videoPlayer.renderMode == VideoRenderMode.CameraFarPlane || this.videoPlayer.renderMode == VideoRenderMode.CameraNearPlane)
		{
			this.videoPlayer.targetCamera = this.mainCamera;
		}
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x000EE778 File Offset: 0x000ECB78
	private void OnApplicationPause(bool pause)
	{
		if (!this.playUsingInternalDevicePlayer && !this.loadYoutubeUrlsOnly && this.videoPlayer.isPrepared)
		{
			if (this.audioPlayer != null)
			{
				this.audioPlayer.Pause();
			}
			this.videoPlayer.Pause();
		}
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x000EE7D4 File Offset: 0x000ECBD4
	private void OnApplicationFocus(bool focus)
	{
		if (focus && !this.playUsingInternalDevicePlayer && !this.loadYoutubeUrlsOnly && !this.pauseCalled && this.videoPlayer.isPrepared)
		{
			if (this.audioPlayer != null && !this.noAudioAtacched && this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
			{
				this.audioPlayer.Play();
			}
			this.videoPlayer.Play();
		}
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x000EE855 File Offset: 0x000ECC55
	private void OnEnable()
	{
		if (this.autoPlayOnEnable && !this.pauseCalled)
		{
			base.StartCoroutine(this.WaitThingsGetDone());
		}
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x000EE87C File Offset: 0x000ECC7C
	private IEnumerator WaitThingsGetDone()
	{
		yield return new WaitForSeconds(1f);
		if (this.youtubeUrlReady && this.videoPlayer.isPrepared)
		{
			this.Play();
		}
		else if (!this.youtubeUrlReady)
		{
			this.Play(this.youtubeUrl);
		}
		yield break;
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x000EE898 File Offset: 0x000ECC98
	private void VerifyFrames()
	{
		if (!this.playUsingInternalDevicePlayer && this.videoPlayer.isPlaying)
		{
			if (this.lastFrame == this.videoPlayer.frame)
			{
				this.audioPlayer.Pause();
				this.videoPlayer.Pause();
				base.StartCoroutine(this.WaitSync());
			}
			this.lastFrame = this.videoPlayer.frame;
			base.Invoke("VerifyFrames", 2f);
		}
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x000EE91C File Offset: 0x000ECD1C
	private IEnumerator ReleaseNeedUpdate()
	{
		yield return new WaitForSeconds(40f);
		this.canUpdate = true;
		yield break;
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x000EE938 File Offset: 0x000ECD38
	private void FixedUpdate()
	{
		if (!this.loadYoutubeUrlsOnly)
		{
			if (this.videoPlayer.isPlaying && Time.frameCount % (int)(this.videoPlayer.frameRate + 1f) == 0)
			{
				if (this.lastTimePlayed == this.videoPlayer.time)
				{
					this.ShowLoading();
					Debug.Log("Buffering");
				}
				else
				{
					this.HideLoading();
				}
				this.lastTimePlayed = this.videoPlayer.time;
			}
			if (!this.playUsingInternalDevicePlayer)
			{
				if (this.videoPlayer.isPlaying)
				{
					this.HideLoading();
				}
				else if (!this.pauseCalled)
				{
					this.ShowLoading();
				}
			}
		}
		if (!this.loadYoutubeUrlsOnly)
		{
			if (this.showPlayerControls && this.videoPlayer.isPlaying)
			{
				this.totalVideoDuration = (float)Mathf.RoundToInt(this.videoPlayer.frameCount / this.videoPlayer.frameRate);
				if (!this.lowRes)
				{
					this.audioDuration = (float)Mathf.RoundToInt(this.audioPlayer.frameCount / this.audioPlayer.frameRate);
					if (this.audioDuration < this.totalVideoDuration && this.audioPlayer.url != string.Empty)
					{
						this.currentVideoDuration = (float)Mathf.RoundToInt((float)this.audioPlayer.frame / this.audioPlayer.frameRate);
					}
					else
					{
						this.currentVideoDuration = (float)Mathf.RoundToInt((float)this.videoPlayer.frame / this.videoPlayer.frameRate);
					}
				}
				else
				{
					this.currentVideoDuration = (float)Mathf.RoundToInt((float)this.videoPlayer.frame / this.videoPlayer.frameRate);
				}
			}
			if (this.videoPlayer.frameCount > 0UL && this.progress != null)
			{
				this.progress.fillAmount = (float)this.videoPlayer.frame / this.videoPlayer.frameCount;
			}
		}
		if (YoutubePlayer.needUpdate)
		{
			YoutubePlayer.needUpdate = false;
			base.StartCoroutine(this.ReleaseNeedUpdate());
			this.UpdateRegexMethodsFromServer();
		}
		if (this.gettingYoutubeURL)
		{
			this.currentRequestTime += Time.deltaTime;
			if (this.currentRequestTime >= (float)this.maxRequestTime && !this.ignoreTimeout)
			{
				this.gettingYoutubeURL = false;
				if (this.debug)
				{
					Debug.Log("<color=blue>Max time reached, trying again!</color>");
				}
				this.RetryPlayYoutubeVideo();
			}
		}
		if (this.videoAreReadyToPlay)
		{
			this.videoAreReadyToPlay = false;
		}
		this.ErrorCheck();
		if (!this.loadYoutubeUrlsOnly)
		{
			if (this.showPlayerControls)
			{
				if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
				{
					this.lowRes = false;
				}
				else
				{
					this.lowRes = true;
				}
				if (this.currentTimeString != null && this.totalTimeString != null)
				{
					this.currentTimeString.text = this.FormatTime(Mathf.RoundToInt(this.currentVideoDuration));
					if (!this.lowRes)
					{
						if (this.audioDuration < this.totalVideoDuration && this.audioPlayer.url != string.Empty)
						{
							this.totalTimeString.text = this.FormatTime(Mathf.RoundToInt(this.audioDuration));
						}
						else
						{
							this.totalTimeString.text = this.FormatTime(Mathf.RoundToInt(this.totalVideoDuration));
						}
					}
					else
					{
						this.totalTimeString.text = this.FormatTime(Mathf.RoundToInt(this.totalVideoDuration));
					}
				}
			}
			if (!this.showPlayerControls)
			{
				if (this.mainControllerUi != null)
				{
					this.mainControllerUi.SetActive(false);
				}
			}
			else
			{
				this.mainControllerUi.SetActive(true);
			}
		}
		if (this.decryptedUrlForAudio)
		{
			this.decryptedUrlForAudio = false;
			this.DecryptAudioDone(this.decryptedAudioUrlResult);
			this.decryptedUrlForVideo = true;
		}
		if (this.decryptedUrlForVideo)
		{
			this.decryptedUrlForVideo = false;
			this.DecryptVideoDone(this.decryptedVideoUrlResult);
		}
		if (!this.loadYoutubeUrlsOnly && this.videoPlayer.isPrepared && !this.videoPlayer.isPlaying)
		{
			if (this.audioPlayer != null)
			{
				if (this.audioPlayer.isPrepared && !this.videoStarted)
				{
					this.videoStarted = true;
					this.VideoStarted(this.videoPlayer);
				}
			}
			else if (!this.videoStarted)
			{
				this.videoStarted = true;
				this.VideoStarted(this.videoPlayer);
			}
		}
		if (!this.loadYoutubeUrlsOnly)
		{
			if (this.videoPlayer.frame != 0L && !this.videoEnded && (int)this.videoPlayer.frame >= (int)this.videoPlayer.frameCount)
			{
				this.videoEnded = true;
				this.PlaybackDone(this.videoPlayer);
			}
			if (this.videoPlayer.isPrepared)
			{
				if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
				{
					if (this.audioPlayer.isPrepared && !this.startedPlayingWebgl)
					{
						this.startedPlayingWebgl = true;
						this.StartPlayingWebgl();
					}
				}
				else if (!this.startedPlayingWebgl)
				{
					this.startedPlayingWebgl = true;
					this.StartPlayingWebgl();
				}
			}
		}
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000EEEB8 File Offset: 0x000ED2B8
	private void PrepareVideoPlayerCallbacks()
	{
		this.videoPlayer.errorReceived += this.VideoErrorReceived;
		this.videoPlayer.loopPointReached += this.PlaybackDone;
		if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.audioPlayer.errorReceived += this.VideoErrorReceived;
		}
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x000EEF15 File Offset: 0x000ED315
	private void ShowLoading()
	{
		if (this.loadingContent != null)
		{
			this.loadingContent.SetActive(true);
		}
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x000EEF34 File Offset: 0x000ED334
	private void HideLoading()
	{
		if (this.loadingContent != null)
		{
			this.loadingContent.SetActive(false);
		}
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x000EEF53 File Offset: 0x000ED353
	public void Play(string url)
	{
		this.logTest = "Getting URL";
		this.Stop();
		this.startedPlayingWebgl = false;
		this.PlayYoutubeVideo(url);
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x000EEF74 File Offset: 0x000ED374
	private string CheckVideoUrlAndExtractThevideoId(string url)
	{
		if (url.Contains("?t="))
		{
			int num = url.LastIndexOf("?t=");
			string text = url;
			string text2 = text.Remove(0, num);
			text2 = text2.Replace("?t=", string.Empty);
			this.startFromSecond = true;
			this.startFromSecondTime = int.Parse(text2);
			url = url.Remove(num);
		}
		if (!this.TryNormalizeYoutubeUrlLocal(url, out url))
		{
			url = "none";
			this.OnYoutubeError("Not a Youtube Url");
		}
		return url;
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x000EEFF8 File Offset: 0x000ED3F8
	public void OnYoutubeError(string errorType)
	{
		Debug.Log("<color=red>" + errorType + "</color>");
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000EF010 File Offset: 0x000ED410
	private bool TryNormalizeYoutubeUrlLocal(string url, out string normalizedUrl)
	{
		url = url.Trim();
		url = url.Replace("youtu.be/", "youtube.com/watch?v=");
		url = url.Replace("www.youtube", "youtube");
		url = url.Replace("youtube.com/embed/", "youtube.com/watch?v=");
		if (url.Contains("/v/"))
		{
			url = "https://youtube.com" + new Uri(url).AbsolutePath.Replace("/v/", "/watch?v=");
		}
		url = url.Replace("/watch#", "/watch?");
		IDictionary<string, string> dictionary = HTTPHelperYoutube.ParseQueryString(url);
		string text;
		if (!dictionary.TryGetValue("v", out text))
		{
			normalizedUrl = null;
			return false;
		}
		normalizedUrl = "https://youtube.com/watch?v=" + text;
		return true;
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000EF0D0 File Offset: 0x000ED4D0
	private void ResetThings()
	{
		this.gettingYoutubeURL = false;
		this.videoAreReadyToPlay = false;
		this.audioDecryptDone = false;
		this.videoDecryptDone = false;
		this.isRetry = false;
		this.youtubeUrlReady = false;
		if (this.audioPlayer != null)
		{
			this.audioPlayer.seekCompleted += this.AudioSeeked;
		}
		this.videoPlayer.seekCompleted += this.VideoSeeked;
		this.videoPlayer.frameDropped += this.VideoPlayer_frameDropped;
		if (this.audioPlayer != null)
		{
			this.audioPlayer.frameDropped += this.AudioPlayer_frameDropped;
		}
		this.waitAudioSeek = false;
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x000EF18C File Offset: 0x000ED58C
	public void PlayFromDefaultUrl()
	{
		this.Play(this.youtubeUrl);
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000EF19C File Offset: 0x000ED59C
	private void PlayYoutubeVideo(string _videoId)
	{
		if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.lowRes = true;
		}
		else
		{
			this.lowRes = false;
		}
		this.ResetThings();
		_videoId = this.CheckVideoUrlAndExtractThevideoId(_videoId);
		if (_videoId == "none")
		{
			return;
		}
		if (this.showThumbnailBeforeVideoLoad)
		{
			this.TryToLoadThumbnailBeforeOpenVideo(_videoId);
		}
		this.youtubeUrlReady = false;
		this.ShowLoading();
		this.youtubeUrl = _videoId;
		this.isRetry = false;
		this.lastTryVideoId = _videoId;
		this.lastPlayTime = Time.time;
		if (!this.ForceGetWebServer)
		{
			this.currentRequestTime = 0f;
			this.gettingYoutubeURL = true;
			this.GetDownloadUrls(new Action(this.UrlsLoaded), this.youtubeUrl, this);
		}
		else
		{
			base.StartCoroutine(this.WebRequest(this.youtubeUrl));
		}
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x000EF274 File Offset: 0x000ED674
	public void DecryptAudioDone(string url)
	{
		this.audioUrl = url;
		this.audioDecryptDone = true;
		if (this.videoDecryptDone)
		{
			if (string.IsNullOrEmpty(this.decryptedAudioUrlResult))
			{
				this.RetryPlayYoutubeVideo();
			}
			else
			{
				this.videoAreReadyToPlay = true;
				this.OnYoutubeUrlsLoaded();
			}
		}
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x000EF2C4 File Offset: 0x000ED6C4
	public void DecryptVideoDone(string url)
	{
		this.videoUrl = url;
		this.videoDecryptDone = true;
		if (this.audioDecryptDone)
		{
			if (string.IsNullOrEmpty(this.decryptedVideoUrlResult))
			{
				this.RetryPlayYoutubeVideo();
			}
			else
			{
				this.videoAreReadyToPlay = true;
				this.OnYoutubeUrlsLoaded();
			}
		}
		else if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			if (string.IsNullOrEmpty(this.decryptedVideoUrlResult))
			{
				this.RetryPlayYoutubeVideo();
			}
			else
			{
				this.videoAreReadyToPlay = true;
				this.OnYoutubeUrlsLoaded();
			}
		}
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x000EF34A File Offset: 0x000ED74A
	public string GetVideoTitle()
	{
		return this.videoTitle;
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x000EF354 File Offset: 0x000ED754
	private void UrlsLoaded()
	{
		this.gettingYoutubeURL = false;
		List<VideoInfo> list = this.youtubeVideoInfos;
		this.videoDecryptDone = false;
		this.audioDecryptDone = false;
		this.decryptedUrlForVideo = false;
		this.decryptedUrlForAudio = false;
		if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			foreach (VideoInfo videoInfo in list)
			{
				if (videoInfo.FormatCode == 18)
				{
					if (videoInfo.RequiresDecryption)
					{
						this.DecryptDownloadUrl(videoInfo.DownloadUrl, string.Empty, videoInfo.HtmlPlayerVersion, true);
					}
					else
					{
						this.videoUrl = videoInfo.DownloadUrl;
						this.videoAreReadyToPlay = true;
						this.OnYoutubeUrlsLoaded();
					}
					this.videoTitle = videoInfo.Title;
				}
			}
		}
		else
		{
			bool flag = false;
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			list.Reverse();
			foreach (VideoInfo videoInfo2 in list)
			{
				if (videoInfo2.FormatCode == 18)
				{
					if (videoInfo2.RequiresDecryption)
					{
						flag = true;
						text3 = videoInfo2.HtmlPlayerVersion;
						text = videoInfo2.DownloadUrl;
					}
					else
					{
						text = videoInfo2.DownloadUrl;
						this.audioUrl = videoInfo2.DownloadUrl;
					}
					this.videoTitle = videoInfo2.Title;
					break;
				}
			}
			int num = 360;
			switch (this.videoQuality)
			{
			case YoutubePlayer.YoutubeVideoQuality.STANDARD:
				num = 360;
				break;
			case YoutubePlayer.YoutubeVideoQuality.HD:
				num = 720;
				break;
			case YoutubePlayer.YoutubeVideoQuality.FULLHD:
				num = 1080;
				break;
			case YoutubePlayer.YoutubeVideoQuality.UHD1440:
				num = 1440;
				break;
			case YoutubePlayer.YoutubeVideoQuality.UHD2160:
				num = 2160;
				break;
			}
			bool flag2 = false;
			list.Reverse();
			foreach (VideoInfo videoInfo3 in list)
			{
				VideoType videoType = ((this.videoFormat != YoutubePlayer.VideoFormatType.MP4) ? VideoType.WebM : VideoType.Mp4);
				if (videoInfo3.VideoType == videoType && videoInfo3.Resolution == num)
				{
					if (videoInfo3.RequiresDecryption)
					{
						if (this.debug)
						{
							Debug.Log("REQUIRE DECRYPTION!");
						}
						this.logTest = "Decry";
						flag = true;
						text2 = videoInfo3.DownloadUrl;
					}
					else
					{
						Debug.Log(videoInfo3.DownloadUrl);
						text2 = videoInfo3.DownloadUrl;
						this.videoUrl = videoInfo3.DownloadUrl;
						this.videoAreReadyToPlay = true;
						this.OnYoutubeUrlsLoaded();
					}
					flag2 = true;
					break;
				}
			}
			if (!flag2 && num == 1440)
			{
				foreach (VideoInfo videoInfo4 in list)
				{
					if (videoInfo4.FormatCode == 271)
					{
						Debug.Log(string.Concat(new object[] { "FIXING!! ", videoInfo4.Resolution, " | ", videoInfo4.VideoType, " | ", videoInfo4.FormatCode }));
						if (videoInfo4.RequiresDecryption)
						{
							flag = true;
							text2 = videoInfo4.DownloadUrl;
						}
						else
						{
							text2 = videoInfo4.DownloadUrl;
							this.videoUrl = videoInfo4.DownloadUrl;
							this.videoAreReadyToPlay = true;
							this.OnYoutubeUrlsLoaded();
						}
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2 && num == 2160)
			{
				foreach (VideoInfo videoInfo5 in list)
				{
					if (videoInfo5.FormatCode == 313)
					{
						if (this.debug)
						{
							Debug.Log("Found but with unknow format in results, check to see if the video works normal.");
						}
						if (videoInfo5.RequiresDecryption)
						{
							flag = true;
							text2 = videoInfo5.DownloadUrl;
						}
						else
						{
							text2 = videoInfo5.DownloadUrl;
							this.videoUrl = videoInfo5.DownloadUrl;
							this.videoAreReadyToPlay = true;
							this.OnYoutubeUrlsLoaded();
						}
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				if (this.debug)
				{
					Debug.Log("Desired quality not found, playing with low quality, check if the video id: " + this.youtubeUrl + " support that quality!");
				}
				foreach (VideoInfo videoInfo6 in list)
				{
					if (videoInfo6.VideoType == VideoType.Mp4 && videoInfo6.Resolution == 360)
					{
						if (videoInfo6.RequiresDecryption)
						{
							this.videoQuality = YoutubePlayer.YoutubeVideoQuality.STANDARD;
							flag = true;
							text2 = videoInfo6.DownloadUrl;
						}
						else
						{
							text2 = videoInfo6.DownloadUrl;
							this.videoUrl = videoInfo6.DownloadUrl;
							this.videoAreReadyToPlay = true;
							this.OnYoutubeUrlsLoaded();
						}
						break;
					}
				}
			}
			if (flag)
			{
				this.DecryptDownloadUrl(text2, text, text3, false);
			}
		}
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x000EF8F4 File Offset: 0x000EDCF4
	private void StartPlayingWebgl()
	{
		if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
		}
		if (this.playUsingInternalDevicePlayer && Application.isMobilePlatform)
		{
			base.StartCoroutine(this.HandHeldPlayback());
		}
		else
		{
			this.StartPlayback();
		}
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x000EF930 File Offset: 0x000EDD30
	private IEnumerator HandHeldPlayback()
	{
		Debug.Log("This runs in mobile devices only!");
		yield return new WaitForSeconds(1f);
		this.PlaybackDone(this.videoPlayer);
		yield break;
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x000EF94C File Offset: 0x000EDD4C
	private void StartPlayback()
	{
		if (this.objectsToRenderTheVideoImage.Length > 0)
		{
			foreach (GameObject gameObject in this.objectsToRenderTheVideoImage)
			{
				gameObject.GetComponent<Renderer>().material.mainTexture = this.videoPlayer.texture;
			}
		}
		this.videoEnded = false;
		this.OnVideoStarted.Invoke();
		if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
		}
		this.HideLoading();
		this.waitAudioSeek = true;
		if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD && !this.noAudioAtacched)
		{
			this.audioPlayer.Pause();
			this.videoPlayer.Pause();
		}
		if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.audioPlayer.Play();
			this.videoPlayer.Play();
		}
		else
		{
			this.videoPlayer.Play();
		}
		if (this.startFromSecond)
		{
			this.startedFromTime = true;
			if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
			{
				this.videoPlayer.time = (double)this.startFromSecondTime;
			}
			else
			{
				this.audioPlayer.time = (double)this.startFromSecondTime;
			}
		}
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x000EFA70 File Offset: 0x000EDE70
	private void ErrorCheck()
	{
		if (!this.ForceGetWebServer && !this.isRetry && this.lastStartedTime < this.lastErrorTime && this.lastErrorTime > this.lastPlayTime)
		{
			if (this.debug)
			{
				Debug.Log("Error detected!, retry with low quality!");
			}
			this.isRetry = true;
		}
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x000EFAD4 File Offset: 0x000EDED4
	public int GetMaxQualitySupportedByDevice()
	{
		if (Screen.orientation == ScreenOrientation.LandscapeLeft)
		{
			return Screen.currentResolution.height;
		}
		if (Screen.orientation == ScreenOrientation.Portrait)
		{
			return Screen.currentResolution.width;
		}
		return Screen.currentResolution.height;
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x000EFB20 File Offset: 0x000EDF20
	private IEnumerator WebRequest(string videoID)
	{
		UnityWebRequest request = UnityWebRequest.Get("https://lightshaftstream.herokuapp.com/api/info?url=" + videoID + "&format=best&flatten=true");
		yield return request.SendWebRequest();
		this.newRequestResults = new YoutubePlayer.YoutubeResultIds();
		JSONNode requestData = JSON.Parse(request.downloadHandler.text);
		JSONNode videos = requestData["videos"][0]["formats"];
		Debug.Log(request.downloadHandler.text);
		this.newRequestResults.bestFormatWithAudioIncluded = requestData["videos"][0]["url"];
		for (int i = 0; i < videos.Count; i++)
		{
			if (videos[i]["format_id"] == "160")
			{
				this.newRequestResults.lowQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "133")
			{
				this.newRequestResults.lowQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "134")
			{
				this.newRequestResults.standardQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "136")
			{
				this.newRequestResults.hdQuality = this.newRequestResults.bestFormatWithAudioIncluded;
			}
			else if (videos[i]["format_id"] == "137")
			{
				this.newRequestResults.fullHdQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "266")
			{
				this.newRequestResults.ultraHdQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "139")
			{
				this.newRequestResults.audioUrl = videos[i]["url"];
			}
		}
		this.audioUrl = this.newRequestResults.bestFormatWithAudioIncluded;
		this.videoUrl = this.newRequestResults.bestFormatWithAudioIncluded;
		switch (this.videoQuality)
		{
		case YoutubePlayer.YoutubeVideoQuality.STANDARD:
			this.videoUrl = this.newRequestResults.bestFormatWithAudioIncluded;
			break;
		case YoutubePlayer.YoutubeVideoQuality.HD:
			this.videoUrl = this.newRequestResults.hdQuality;
			break;
		case YoutubePlayer.YoutubeVideoQuality.FULLHD:
			this.videoUrl = this.newRequestResults.fullHdQuality;
			break;
		case YoutubePlayer.YoutubeVideoQuality.UHD1440:
			this.videoUrl = this.newRequestResults.fullHdQuality;
			break;
		case YoutubePlayer.YoutubeVideoQuality.UHD2160:
			this.videoUrl = this.newRequestResults.ultraHdQuality;
			break;
		}
		if (this.videoUrl == string.Empty)
		{
			this.videoUrl = this.newRequestResults.bestFormatWithAudioIncluded;
		}
		this.videoAreReadyToPlay = true;
		this.OnYoutubeUrlsLoaded();
		yield break;
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x000EFB44 File Offset: 0x000EDF44
	private string ConvertToWebglUrl(string url)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(url);
		string text = Convert.ToBase64String(bytes);
		if (this.debug)
		{
			Debug.Log(url);
		}
		return "https://youtubewebgl.herokuapp.com/download.php?mime=video/mp4&title=generatedvideo&token=" + text;
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x000EFB84 File Offset: 0x000EDF84
	public void RetryPlayYoutubeVideo()
	{
		this.Stop();
		this.currentRetryTime++;
		this.logTest = "Retry!!";
		if (this.currentRetryTime < this.retryTimeUntilToRequestFromServer)
		{
			if (!this.ForceGetWebServer)
			{
				this.StopIfPlaying();
				if (this.debug)
				{
					Debug.Log("Youtube Retrying...:" + this.lastTryVideoId);
				}
				this.logTest = "retry";
				this.isRetry = true;
				this.ShowLoading();
				this.youtubeUrl = this.lastTryVideoId;
				this.PlayYoutubeVideo(this.youtubeUrl);
			}
		}
		else
		{
			this.currentRetryTime = 0;
			this.StopIfPlaying();
			if (this.debug)
			{
				Debug.Log("Youtube Retrying...:" + this.lastTryVideoId);
			}
			this.logTest = "retry";
			this.isRetry = true;
			this.ShowLoading();
			this.youtubeUrl = this.lastTryVideoId;
			this.PlayYoutubeVideo(this.youtubeUrl);
		}
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x000EFC84 File Offset: 0x000EE084
	private void StopIfPlaying()
	{
		if (!this.loadYoutubeUrlsOnly)
		{
			if (this.debug)
			{
				Debug.Log("Stopping video");
			}
			if (this.videoPlayer.isPlaying)
			{
				this.videoPlayer.Stop();
			}
			if (this.audioPlayer.isPlaying)
			{
				this.audioPlayer.Stop();
			}
		}
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x000EFCE8 File Offset: 0x000EE0E8
	public void UrlReadyToUse()
	{
		Debug.Log("Here you can call your external video player if you want, passing that two variables:");
		if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			Debug.Log("Your video Url: " + this.videoUrl);
			Debug.Log("Your audio video Url: " + this.audioUrl);
		}
		else
		{
			Debug.Log("Yout video Url:" + this.videoUrl);
		}
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x000EFD50 File Offset: 0x000EE150
	public void OnYoutubeUrlsLoaded()
	{
		this.youtubeUrlReady = true;
		if (!this.loadYoutubeUrlsOnly)
		{
			Uri uri = new Uri(this.videoUrl);
			this.videoUrl = this.videoUrl.Replace(uri.Host, "redirector.googlevideo.com");
			if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
			{
				uri = new Uri(this.audioUrl);
				this.audioUrl = this.audioUrl.Replace(uri.Host, "redirector.googlevideo.com");
			}
			if (this.debug)
			{
				Debug.Log("Play!!" + this.videoUrl);
			}
			this.startedPlayingWebgl = false;
			this.videoPlayer.source = VideoSource.Url;
			this.videoPlayer.url = this.videoUrl;
			this.videoPlayer.Prepare();
			if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
			{
				this.audioPlayer.source = VideoSource.Url;
				this.audioPlayer.url = this.audioUrl;
				this.audioPlayer.Prepare();
			}
		}
		else if (this.playUsingInternalDevicePlayer)
		{
			base.StartCoroutine(this.HandHeldPlayback());
		}
		else
		{
			this.UrlReadyToUse();
		}
		this.OnYoutubeUrlAreReady.Invoke(this.videoUrl);
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x000EFE85 File Offset: 0x000EE285
	public void OnYoutubeVideoAreReadyToPlay()
	{
		this.OnVideoReadyToStart.Invoke();
		this.StartPlayingWebgl();
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x000EFE98 File Offset: 0x000EE298
	private IEnumerator PreventFinishToBeCalledTwoTimes()
	{
		yield return new WaitForSeconds(1f);
		this.finishedCalled = false;
		yield break;
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000EFEB4 File Offset: 0x000EE2B4
	public void OnVideoPlayerFinished()
	{
		if (!this.finishedCalled)
		{
			this.finishedCalled = true;
			base.StartCoroutine(this.PreventFinishToBeCalledTwoTimes());
			if (!this.loadYoutubeUrlsOnly)
			{
				if (this.videoPlayer.isPrepared)
				{
					if (this.debug)
					{
						Debug.Log("Finished");
					}
					if (this.videoPlayer.isLooping)
					{
						this.videoPlayer.time = 0.0;
						this.videoPlayer.frame = 0L;
						this.audioPlayer.time = 0.0;
						this.audioPlayer.frame = 0L;
						this.videoPlayer.Play();
						if (!this.noAudioAtacched)
						{
							this.audioPlayer.Play();
						}
					}
					base.CancelInvoke("CheckIfIsSync");
					this.OnVideoFinished.Invoke();
					if (this.customPlaylist && this.autoPlayNextVideo)
					{
						Debug.Log("Calling next video of playlist");
						this.CallNextUrl();
					}
				}
			}
			else if (this.playUsingInternalDevicePlayer)
			{
				base.CancelInvoke("CheckIfIsSync");
				this.OnVideoFinished.Invoke();
			}
		}
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x000EFFE5 File Offset: 0x000EE3E5
	private void PlaybackDone(VideoPlayer vPlayer)
	{
		this.videoStarted = false;
		this.OnVideoPlayerFinished();
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x000EFFF4 File Offset: 0x000EE3F4
	private void VideoStarted(VideoPlayer source)
	{
		if (!this.videoStarted)
		{
			this.lastStartedTime = Time.time;
			this.lastErrorTime = this.lastStartedTime;
			if (this.debug)
			{
				Debug.Log("Youtube Video Started");
			}
		}
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000F002D File Offset: 0x000EE42D
	private void VideoErrorReceived(VideoPlayer source, string message)
	{
		this.lastErrorTime = Time.time;
		this.RetryPlayYoutubeVideo();
		Debug.Log("Youtube VideoErrorReceived! Retry: " + message);
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000F0050 File Offset: 0x000EE450
	public void Play()
	{
		this.pauseCalled = false;
		if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.videoPlayer.Play();
		}
		else
		{
			this.videoPlayer.Play();
			if (!this.noAudioAtacched)
			{
				this.audioPlayer.Play();
			}
		}
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x000F00A0 File Offset: 0x000EE4A0
	public void Pause()
	{
		this.pauseCalled = true;
		if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.videoPlayer.Pause();
		}
		else
		{
			this.videoPlayer.Pause();
			this.audioPlayer.Pause();
		}
		this.OnVideoPaused.Invoke();
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000F00F0 File Offset: 0x000EE4F0
	public void PlayPause()
	{
		if (this.youtubeUrlReady && this.videoPlayer.isPrepared)
		{
			if (!this.pauseCalled)
			{
				this.Pause();
			}
			else
			{
				this.Play();
			}
		}
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x000F012C File Offset: 0x000EE52C
	private void Update()
	{
		if (!this.loadYoutubeUrlsOnly && this.showPlayerControls && this.autoHideControlsTime > 0)
		{
			if (this.UserInteract())
			{
				this.hideScreenTime = 0f;
				if (this.mainControllerUi != null)
				{
					this.mainControllerUi.SetActive(true);
				}
			}
			else
			{
				this.hideScreenTime += Time.deltaTime;
				if (this.hideScreenTime >= (float)this.autoHideControlsTime)
				{
					this.hideScreenTime = (float)this.autoHideControlsTime;
					this.HideControllers();
				}
			}
		}
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000F01CA File Offset: 0x000EE5CA
	public void Seek(float time)
	{
		this.waitAudioSeek = true;
		this.Pause();
		if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.videoPlayer.time = (double)time;
		}
		else
		{
			this.audioPlayer.time = (double)time;
		}
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000F0204 File Offset: 0x000EE604
	private void HideControllers()
	{
		if (this.mainControllerUi != null)
		{
			this.mainControllerUi.SetActive(false);
			this.showingVolume = false;
			this.showingPlaybackSpeed = false;
			this.volumeSlider.gameObject.SetActive(false);
			this.playbackSpeed.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000F0260 File Offset: 0x000EE660
	public void Volume()
	{
		if (this.videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
		{
			this.audioPlayer.SetDirectAudioVolume(0, this.volumeSlider.value);
			this.videoPlayer.SetDirectAudioVolume(0, this.volumeSlider.value);
		}
		else if (this.videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
		{
			this.videoPlayer.GetComponent<AudioSource>().volume = this.volumeSlider.value;
			this.videoPlayer.SetDirectAudioVolume(0, this.volumeSlider.value);
		}
		else
		{
			this.videoPlayer.GetComponent<AudioSource>().volume = this.volumeSlider.value;
			this.videoPlayer.SetDirectAudioVolume(0, this.volumeSlider.value);
		}
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000F032C File Offset: 0x000EE72C
	public void Speed()
	{
		if (this.videoPlayer.canSetPlaybackSpeed)
		{
			if (this.playbackSpeed.value == 0f)
			{
				this.videoPlayer.playbackSpeed = 0.5f;
				this.audioPlayer.playbackSpeed = 0.5f;
			}
			else
			{
				this.videoPlayer.playbackSpeed = this.playbackSpeed.value;
				this.audioPlayer.playbackSpeed = this.playbackSpeed.value;
			}
		}
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000F03B0 File Offset: 0x000EE7B0
	public void VolumeSlider()
	{
		if (this.showingVolume)
		{
			this.showingVolume = false;
			this.volumeSlider.gameObject.SetActive(false);
		}
		else
		{
			this.showingVolume = true;
			this.volumeSlider.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000F0400 File Offset: 0x000EE800
	public void PlaybackSpeedSlider()
	{
		if (this.showingPlaybackSpeed)
		{
			this.showingPlaybackSpeed = false;
			this.playbackSpeed.gameObject.SetActive(false);
		}
		else
		{
			this.showingPlaybackSpeed = true;
			this.playbackSpeed.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000F044D File Offset: 0x000EE84D
	private void VideoPreparedSeek(VideoPlayer p)
	{
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x000F044F File Offset: 0x000EE84F
	private void AudioPreparedSeek(VideoPlayer p)
	{
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000F0454 File Offset: 0x000EE854
	public void Stop()
	{
		if (!this.playUsingInternalDevicePlayer)
		{
			if (this.audioPlayer != null)
			{
				this.audioPlayer.seekCompleted -= this.AudioSeeked;
			}
			this.videoPlayer.seekCompleted -= this.VideoSeeked;
			this.videoPlayer.frameDropped -= this.VideoPlayer_frameDropped;
			if (this.audioPlayer != null)
			{
				this.audioPlayer.frameDropped -= this.AudioPlayer_frameDropped;
			}
			this.videoPlayer.Stop();
			if (!this.lowRes && this.audioPlayer != null)
			{
				this.audioPlayer.Stop();
			}
		}
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x000F051C File Offset: 0x000EE91C
	private void SeekVideoDone(VideoPlayer vp)
	{
		this.videoSeekDone = true;
		this.videoPlayer.seekCompleted -= this.SeekVideoDone;
		if (!this.lowRes)
		{
			if (this.videoSeekDone && this.videoAudioSeekDone)
			{
				this.isSyncing = false;
				base.StartCoroutine(this.SeekFinished());
			}
		}
		else
		{
			this.isSyncing = false;
			this.HideLoading();
		}
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x000F0590 File Offset: 0x000EE990
	private void SeekVideoAudioDone(VideoPlayer vp)
	{
		Debug.Log("NAAN");
		this.videoAudioSeekDone = true;
		this.audioPlayer.seekCompleted -= this.SeekVideoAudioDone;
		if (!this.lowRes && this.videoSeekDone && this.videoAudioSeekDone)
		{
			this.isSyncing = false;
			base.StartCoroutine(this.SeekFinished());
		}
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x000F05FC File Offset: 0x000EE9FC
	private IEnumerator SeekFinished()
	{
		yield return new WaitForEndOfFrame();
		this.HideLoading();
		yield break;
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x000F0618 File Offset: 0x000EEA18
	private string FormatTime(int time)
	{
		int num = time / 3600;
		int num2 = time % 3600 / 60;
		int num3 = time % 3600 % 60;
		if (num == 0 && num2 != 0)
		{
			return num2.ToString("00") + ":" + num3.ToString("00");
		}
		if (num == 0 && num2 == 0)
		{
			return "00:" + num3.ToString("00");
		}
		return string.Concat(new string[]
		{
			num.ToString("00"),
			":",
			num2.ToString("00"),
			":",
			num3.ToString("00")
		});
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000F06E0 File Offset: 0x000EEAE0
	private bool UserInteract()
	{
		if (Application.isMobilePlatform)
		{
			return Input.touches.Length >= 1;
		}
		return Input.GetMouseButtonDown(0) || Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f;
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000F0740 File Offset: 0x000EEB40
	public void DecryptDownloadUrl(string encryptedUrlVideo, string encrytedUrlAudio, string html, bool videoOnly)
	{
		this.EncryptUrlForAudio = encrytedUrlAudio;
		this.EncryptUrlForVideo = encryptedUrlVideo;
		if (videoOnly)
		{
			string text = this.EncryptUrlForVideo;
			text = text.Replace("&sig=", "|");
			text = text.Replace("lsig=", "|");
			text = text.Replace("&ratebypass=yes", string.Empty);
			string[] array = text.Split(new char[] { '|' });
			this.lsigForVideo = array[array.Length - 2];
			this.encryptedSignatureVideo = array[array.Length - 1];
			base.StartCoroutine(this.Downloader(YoutubePlayer.jsUrl, false));
		}
		else
		{
			string text2 = this.EncryptUrlForVideo;
			text2 = text2.Replace("&sig=", "|");
			text2 = text2.Replace("lsig=", "|");
			text2 = text2.Replace("&ratebypass=yes", string.Empty);
			string[] array2 = text2.Split(new char[] { '|' });
			this.lsigForVideo = array2[array2.Length - 2];
			this.encryptedSignatureVideo = array2[array2.Length - 1];
			text2 = this.EncryptUrlForAudio;
			text2 = text2.Replace("&sig=", "|");
			text2 = text2.Replace("lsig=", "|");
			text2 = text2.Replace("&ratebypass=yes", string.Empty);
			string[] array3 = text2.Split(new char[] { '|' });
			this.lsigForAudio = array3[array3.Length - 2];
			this.encryptedSignatureAudio = array3[array3.Length - 1];
			base.StartCoroutine(this.Downloader(YoutubePlayer.jsUrl, true));
		}
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x000F08C4 File Offset: 0x000EECC4
	public void ReadyForExtract(string r, bool audioExtract)
	{
		if (audioExtract)
		{
			this.SetMasterUrlForAudio(r);
			if (SystemInfo.processorCount > 1)
			{
				this.thread1 = new Thread(delegate
				{
					this.DoRegexFunctionsForAudio(r);
				});
				this.thread1.Start();
			}
			else
			{
				this.DoRegexFunctionsForAudio(r);
			}
		}
		else
		{
			this.SetMasterUrlForVideo(r);
			if (SystemInfo.processorCount > 1)
			{
				this.thread1 = new Thread(delegate
				{
					this.DoRegexFunctionsForVideo(r);
				});
				this.thread1.Start();
			}
			else
			{
				this.DoRegexFunctionsForVideo(r);
			}
		}
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x000F0984 File Offset: 0x000EED84
	private IEnumerator Downloader(string uri, bool audio)
	{
		UnityWebRequest request = UnityWebRequest.Get(uri);
		yield return request.SendWebRequest();
		this.ReadyForExtract(request.downloadHandler.text, audio);
		yield break;
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x000F09B0 File Offset: 0x000EEDB0
	private IEnumerator WebGlRequest(string videoID)
	{
		UnityWebRequest request = UnityWebRequest.Get("https://lightshaftstream.herokuapp.com/api/info?url=" + videoID + "&format=best&flatten=true");
		yield return request.SendWebRequest();
		this.startedPlayingWebgl = false;
		this.webGlResults = new YoutubePlayer.YoutubeResultIds();
		JSONNode requestData = JSON.Parse(request.downloadHandler.text);
		JSONNode videos = requestData["videos"][0]["formats"];
		this.webGlResults.bestFormatWithAudioIncluded = requestData["videos"][0]["url"];
		for (int i = 0; i < videos.Count; i++)
		{
			if (videos[i]["format_id"] == "160")
			{
				this.webGlResults.lowQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "133")
			{
				this.webGlResults.lowQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "134")
			{
				this.webGlResults.standardQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "136")
			{
				this.webGlResults.hdQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "137")
			{
				this.webGlResults.fullHdQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "266")
			{
				this.webGlResults.ultraHdQuality = videos[i]["url"];
			}
			else if (videos[i]["format_id"] == "139")
			{
				this.webGlResults.audioUrl = videos[i]["url"];
			}
		}
		this.WebGlGetVideo(this.webGlResults.bestFormatWithAudioIncluded);
		yield break;
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x000F09D4 File Offset: 0x000EEDD4
	public void WebGlGetVideo(string url)
	{
		this.logTest = "Getting Url Player";
		byte[] bytes = Encoding.UTF8.GetBytes(url);
		string text = Convert.ToBase64String(bytes);
		this.videoUrl = "https://youtubewebgl.herokuapp.com/download.php?mime=video/mp4&title=generatedvideo&token=" + text;
		this.videoQuality = YoutubePlayer.YoutubeVideoQuality.STANDARD;
		this.logTest = this.videoUrl + " Done";
		Debug.Log("Play!! " + this.videoUrl);
		this.videoPlayer.source = VideoSource.Url;
		this.videoPlayer.url = this.videoUrl;
		this.videoPlayer.Prepare();
		this.videoPlayer.prepareCompleted += this.WeblPrepared;
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000F0A81 File Offset: 0x000EEE81
	private void WeblPrepared(VideoPlayer source)
	{
		this.startedPlayingWebgl = true;
		base.StartCoroutine(this.WebGLPlay());
		this.logTest = "Playing!!";
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000F0AA4 File Offset: 0x000EEEA4
	private IEnumerator WebGLPlay()
	{
		yield return new WaitForSeconds(2f);
		this.StartPlayingWebgl();
		yield break;
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x000F0ABF File Offset: 0x000EEEBF
	private void OnGUI()
	{
		if (this.debug)
		{
			GUI.Label(new Rect(0f, 0f, 400f, 30f), this.logTest);
		}
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x000F0AF0 File Offset: 0x000EEEF0
	public void SetMasterUrlForAudio(string url)
	{
		this.masterURLForAudio = url;
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x000F0AF9 File Offset: 0x000EEEF9
	public void SetMasterUrlForVideo(string url)
	{
		this.masterURLForVideo = url;
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000F0B04 File Offset: 0x000EEF04
	public void DoRegexFunctionsForVideo(string jsF)
	{
		this.masterURLForVideo = jsF;
		string text = string.Empty;
		this.patternNames = this.magicResult.regexForFuncName;
		foreach (string text2 in this.patternNames)
		{
			string value = Regex.Match(jsF, text2).Groups[1].Value;
			if (!string.IsNullOrEmpty(value))
			{
				text = value;
				break;
			}
		}
		if (text.Contains("$"))
		{
			text = "\\" + text;
		}
		Debug.Log(text);
		string text3 = "(?!h\\.)" + text + "=function\\(\\w+\\)\\{.*?\\}";
		string value2 = Regex.Match(jsF, text3, RegexOptions.Singleline).Value;
		Debug.Log(value2);
		string[] array2 = value2.Split(new char[] { ';' });
		string text4 = string.Empty;
		string text5 = string.Empty;
		string text6 = string.Empty;
		string text7 = string.Empty;
		string text8 = string.Empty;
		foreach (string text9 in array2.Skip(1).Take(array2.Length - 2))
		{
			if (!string.IsNullOrEmpty(text4) && !string.IsNullOrEmpty(text5) && !string.IsNullOrEmpty(text6))
			{
				break;
			}
			text7 = YoutubePlayer.GetFunctionFromLine(text9);
			string text10 = string.Format("{0}:\\bfunction\\b\\(\\w+\\)", text7);
			string text11 = string.Format("{0}:\\bfunction\\b\\([a],b\\).(\\breturn\\b)?.?\\w+\\.", text7);
			string text12 = string.Format("{0}:\\bfunction\\b\\(\\w+\\,\\w\\).\\bvar\\b.\\bc=a\\b", text7);
			if (text7 == "nt")
			{
				string text13 = "encodeURIComponent:\\bfunction\\b\\(\\w+\\)";
				if (!Regex.Match(jsF, text13).Success)
				{
					if (Regex.Matches(jsF, text10).Count > 1)
					{
						text4 = text7;
					}
				}
			}
			else if (Regex.Match(jsF, text10).Success)
			{
				text4 = text7;
			}
			if (Regex.Match(jsF, text11).Success)
			{
				text5 = text7;
			}
			if (Regex.Match(jsF, text12).Success)
			{
				text6 = text7;
			}
		}
		foreach (string text14 in array2.Skip(1).Take(array2.Length - 2))
		{
			text7 = YoutubePlayer.GetFunctionFromLine(text14);
			Match match;
			if ((match = Regex.Match(text14, "\\(\\w+,(?<index>\\d+)\\)")).Success && text7 == text6)
			{
				text8 = text8 + "w" + match.Groups["index"].Value + " ";
			}
			if ((match = Regex.Match(text14, "\\(\\w+,(?<index>\\d+)\\)")).Success && text7 == text5)
			{
				text8 = text8 + "s" + match.Groups["index"].Value + " ";
			}
			if (text7 == text4)
			{
				text8 += "r ";
			}
		}
		text8 = text8.Trim();
		if (string.IsNullOrEmpty(text8))
		{
			Debug.Log("Operation is empty for low qual, trying again.");
			if (this.canUpdate)
			{
				YoutubePlayer.needUpdate = true;
				this.canUpdate = false;
			}
			this.decryptedVideoUrlResult = null;
			return;
		}
		string text15 = MagicHands.DecipherWithOperations(this.encryptedSignatureVideo, text8);
		this.decryptedVideoUrlResult = HTTPHelperYoutube.ReplaceQueryStringParameter(this.EncryptUrlForVideo, YoutubePlayer.SignatureQuery, text15, this.lsigForVideo);
		this.decryptedUrlForVideo = true;
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000F0EEC File Offset: 0x000EF2EC
	private static int GetOpIndex(string op)
	{
		string text = new Regex(".(\\d+)").Match(op).Result("$1");
		return int.Parse(text);
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x000F0F1C File Offset: 0x000EF31C
	private static char[] SpliceFunction(char[] a, int b)
	{
		return a.Splice(b);
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x000F0F28 File Offset: 0x000EF328
	private static char[] SwapFunction(char[] a, int b)
	{
		char c = a[0];
		a[0] = a[b % a.Length];
		a[b % a.Length] = c;
		return a;
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x000F0F4C File Offset: 0x000EF34C
	private static char[] ReverseFunction(char[] a)
	{
		Array.Reverse(a);
		return a;
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x000F0F58 File Offset: 0x000EF358
	public void DoRegexFunctionsForAudio(string jsF)
	{
		this.masterURLForAudio = jsF;
		string text = this.masterURLForAudio;
		this.patternNames = this.magicResult.regexForFuncName;
		string text2 = string.Empty;
		foreach (string text3 in this.patternNames)
		{
			string value = Regex.Match(text, text3).Groups[1].Value;
			if (!string.IsNullOrEmpty(value))
			{
				text2 = value;
				break;
			}
		}
		if (text2.Contains("$"))
		{
			text2 = "\\" + text2;
		}
		string text4 = "(?!h\\.)" + text2 + "=function\\(\\w+\\)\\{.*?join.*\\};";
		string value2 = Regex.Match(text, text4).Value;
		string[] array2 = value2.Split(new char[] { ';' });
		string text5 = string.Empty;
		string text6 = string.Empty;
		string text7 = string.Empty;
		string text8 = string.Empty;
		string text9 = string.Empty;
		foreach (string text10 in array2.Skip(1).Take(array2.Length - 2))
		{
			if (!string.IsNullOrEmpty(text5) && !string.IsNullOrEmpty(text6) && !string.IsNullOrEmpty(text7))
			{
				break;
			}
			text8 = YoutubePlayer.GetFunctionFromLine(text10);
			string text11 = string.Format("{0}:\\bfunction\\b\\(\\w+\\)", text8);
			string text12 = string.Format("{0}:\\bfunction\\b\\([a],b\\).(\\breturn\\b)?.?\\w+\\.", text8);
			string text13 = string.Format("{0}:\\bfunction\\b\\(\\w+\\,\\w\\).\\bvar\\b.\\bc=a\\b", text8);
			if (text8 == "nt")
			{
				string text14 = "encodeURIComponent:\\bfunction\\b\\(\\w+\\)";
				if (!Regex.Match(text, text14).Success)
				{
					if (Regex.Matches(text, text11).Count > 1)
					{
						text5 = text8;
					}
				}
			}
			else if (Regex.Match(text, text11).Success)
			{
				text5 = text8;
			}
			if (Regex.Match(text, text12).Success)
			{
				text6 = text8;
			}
			if (Regex.Match(text, text13).Success)
			{
				text7 = text8;
			}
		}
		foreach (string text15 in array2.Skip(1).Take(array2.Length - 2))
		{
			text8 = YoutubePlayer.GetFunctionFromLine(text15);
			Match match;
			if ((match = Regex.Match(text15, "\\(\\w+,(?<index>\\d+)\\)")).Success && text8 == text7)
			{
				text9 = text9 + "w" + match.Groups["index"].Value + " ";
			}
			if ((match = Regex.Match(text15, "\\(\\w+,(?<index>\\d+)\\)")).Success && text8 == text6)
			{
				text9 = text9 + "s" + match.Groups["index"].Value + " ";
			}
			if (text8 == text5)
			{
				text9 += "r ";
			}
		}
		text9 = text9.Trim();
		if (string.IsNullOrEmpty(text9))
		{
			Debug.Log("Operation is empty, trying again.");
			if (this.canUpdate)
			{
				YoutubePlayer.needUpdate = true;
				this.canUpdate = false;
			}
			this.decryptedAudioUrlResult = null;
			this.decryptedVideoUrlResult = null;
		}
		else
		{
			string text16 = MagicHands.DecipherWithOperations(this.encryptedSignatureAudio, text9);
			string text17 = MagicHands.DecipherWithOperations(this.encryptedSignatureVideo, text9);
			this.decryptedAudioUrlResult = HTTPHelperYoutube.ReplaceQueryStringParameter(this.EncryptUrlForAudio, YoutubePlayer.SignatureQuery, text16, this.lsigForAudio);
			this.decryptedVideoUrlResult = HTTPHelperYoutube.ReplaceQueryStringParameter(this.EncryptUrlForVideo, YoutubePlayer.SignatureQuery, text17, this.lsigForVideo);
		}
		this.decryptedUrlForAudio = true;
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000F136C File Offset: 0x000EF76C
	private void DelayForAudio()
	{
		this.decryptedUrlForVideo = true;
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000F1378 File Offset: 0x000EF778
	private static string GetFunctionFromLine(string currentLine)
	{
		Regex regex = new Regex("\\w+\\.(?<functionID>\\w+)\\(");
		Match match = regex.Match(currentLine);
		return match.Groups["functionID"].Value;
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x000F13B0 File Offset: 0x000EF7B0
	public IEnumerator WebGlRequest(Action<string> callback, string id, string host)
	{
		Debug.Log(host + "getvideo.php?videoid=" + id + "&type=Download");
		UnityWebRequest request = UnityWebRequest.Get(host + "getvideo.php?videoid=" + id + "&type=Download");
		yield return request.SendWebRequest();
		callback(request.downloadHandler.text);
		yield break;
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x000F13DC File Offset: 0x000EF7DC
	public void GetDownloadUrls(Action callback, string videoUrl, YoutubePlayer player)
	{
		if (videoUrl != null)
		{
		}
		if (videoUrl == null)
		{
			throw new ArgumentNullException("videoUrl");
		}
		if (!YoutubePlayer.TryNormalizeYoutubeUrl(videoUrl, out videoUrl))
		{
			throw new ArgumentException("URL is not a valid youtube URL!");
		}
		base.StartCoroutine(this.DownloadYoutubeUrl(videoUrl, callback, player));
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x000F1430 File Offset: 0x000EF830
	private IEnumerator YoutubeURLDownloadFinished(Action callback, YoutubePlayer player)
	{
		string videoId = this.youtubeUrl.Replace("https://youtube.com/watch?v=", string.Empty);
		string player_response = string.Empty;
		if (Regex.IsMatch(this.jsonForHtmlVersion, "[\"\\']status[\"\\']\\s*:\\s*[\"\\']LOGIN_REQUIRED"))
		{
			Debug.Log("MM");
			string url = "https://www.youtube.com/get_video_info?video_id=" + videoId + "&eurl=https://youtube.googleapis.com/v/" + videoId;
			UnityWebRequest request = UnityWebRequest.Get(url);
			request.SetRequestHeader("User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:10.0) Gecko/20100101 Firefox/10.0 (Chrome)");
			yield return request.SendWebRequest();
			if (request.isNetworkError)
			{
				Debug.Log("Youtube UnityWebRequest isNetworkError!");
			}
			else if (request.isHttpError)
			{
				Debug.Log("Youtube UnityWebRequest isHttpError!");
			}
			else if (request.responseCode != 200L)
			{
				Debug.Log("Youtube UnityWebRequest responseCode:" + request.responseCode);
			}
			player_response = UnityWebRequest.UnEscapeURL(HTTPHelperYoutube.ParseQueryString(request.downloadHandler.text)["player_response"]);
		}
		else
		{
			Regex regex = new Regex("ytplayer\\.config\\s*=\\s*(\\{.+?\\});", RegexOptions.Multiline);
			Match match = regex.Match(this.jsonForHtmlVersion);
			if (match.Success)
			{
				string text = match.Result("$1");
				if (!text.Contains("raw_player_response:ytInitialPlayerResponse"))
				{
					player_response = JObject.Parse(text)["args"]["player_response"].ToString();
				}
			}
			regex = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;\\s*(?:var\\s+meta|</script|\\n)", RegexOptions.Multiline);
			match = regex.Match(this.jsonForHtmlVersion);
			if (match.Success)
			{
				player_response = match.Result("$1");
			}
			regex = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;", RegexOptions.Multiline);
			match = regex.Match(this.jsonForHtmlVersion);
			if (match.Success)
			{
				player_response = match.Result("$1");
			}
		}
		JObject json = JObject.Parse(player_response);
		if (this.downloadYoutubeUrlResponse.isValid)
		{
			if (YoutubePlayer.IsVideoUnavailable(this.downloadYoutubeUrlResponse.data))
			{
				throw new VideoNotAvailableException();
			}
			try
			{
				Regex regex2 = new Regex("\"jsUrl\"\\s*:\\s*\"([^\"]+)\"");
				string text2 = regex2.Match(this.jsonForHtmlVersion).Result("$1").Replace("\\/", "/");
				YoutubePlayer.jsUrl = "https://www.youtube.com" + text2;
				if (this.debug)
				{
					Debug.Log(YoutubePlayer.jsUrl);
				}
				string text3 = YoutubePlayer.TryMatchHtmlVersion(text2, this.magicResult.regexForHtmlPlayerVersion);
				if (this.debug)
				{
					Debug.Log(text3);
				}
				this.htmlVersion = text3;
				string text4 = YoutubePlayer.GetVideoTitle(json);
				if (this.debug)
				{
					Debug.Log(text4);
				}
				IEnumerable<YoutubePlayer.ExtractionInfo> enumerable = YoutubePlayer.ExtractDownloadUrls(json);
				List<VideoInfo> list = YoutubePlayer.GetVideoInfos(enumerable, text4).ToList<VideoInfo>();
				if (string.IsNullOrEmpty(this.htmlVersion))
				{
					this.RetryPlayYoutubeVideo();
				}
				this.youtubeVideoInfos = list;
				foreach (VideoInfo videoInfo in this.youtubeVideoInfos)
				{
					videoInfo.HtmlPlayerVersion = text3;
				}
				callback();
			}
			catch (Exception ex)
			{
				if (!this.loadYoutubeUrlsOnly)
				{
					Debug.Log("Resolver Exception!: " + ex.Message);
					Debug.Log(ex.Source + " " + ex.StackTrace);
					Debug.Log(Application.persistentDataPath);
					if (Application.isEditor && this.debug)
					{
						YoutubePlayer.WriteLog("log_download_exception", "jsonForHtml: " + this.jsonForHtmlVersion);
					}
					Debug.Log("retry!");
					if (player != null)
					{
						player.RetryPlayYoutubeVideo();
					}
					else
					{
						Debug.LogError("Connection to Youtube Server Error! Try Again");
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x000F145C File Offset: 0x000EF85C
	public static bool TryNormalizeYoutubeUrl(string url, out string normalizedUrl)
	{
		url = url.Trim();
		url = url.Replace("youtu.be/", "youtube.com/watch?v=");
		url = url.Replace("www.youtube", "youtube");
		url = url.Replace("youtube.com/embed/", "youtube.com/watch?v=");
		if (url.Contains("/v/"))
		{
			url = "https://youtube.com" + new Uri(url).AbsolutePath.Replace("/v/", "/watch?v=");
		}
		url = url.Replace("/watch#", "/watch?");
		IDictionary<string, string> dictionary = HTTPHelperYoutube.ParseQueryString(url);
		string text;
		if (!dictionary.TryGetValue("v", out text))
		{
			normalizedUrl = null;
			return false;
		}
		normalizedUrl = "https://youtube.com/watch?v=" + text;
		return true;
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000F151C File Offset: 0x000EF91C
	private static IEnumerable<YoutubePlayer.ExtractionInfo> ExtractDownloadUrls(JObject json)
	{
		List<string> urls = new List<string>();
		List<string> ciphers = new List<string>();
		if (json["streamingData"]["formats"][0]["cipher"] != null)
		{
			foreach (JToken jtoken in ((IEnumerable<JToken>)json["streamingData"]["formats"]))
			{
				ciphers.Add(jtoken["cipher"].ToString());
			}
			foreach (JToken jtoken2 in ((IEnumerable<JToken>)json["streamingData"]["adaptiveFormats"]))
			{
				ciphers.Add(jtoken2["cipher"].ToString());
			}
		}
		else if (json["streamingData"]["formats"][0]["signatureCipher"] != null)
		{
			foreach (JToken jtoken3 in ((IEnumerable<JToken>)json["streamingData"]["formats"]))
			{
				ciphers.Add(jtoken3["signatureCipher"].ToString());
			}
			foreach (JToken jtoken4 in ((IEnumerable<JToken>)json["streamingData"]["adaptiveFormats"]))
			{
				ciphers.Add(jtoken4["signatureCipher"].ToString());
			}
		}
		else
		{
			foreach (JToken jtoken5 in ((IEnumerable<JToken>)json["streamingData"]["formats"]))
			{
				urls.Add(jtoken5["url"].ToString());
			}
			foreach (JToken jtoken6 in ((IEnumerable<JToken>)json["streamingData"]["adaptiveFormats"]))
			{
				urls.Add(jtoken6["url"].ToString());
			}
		}
		foreach (string s in ciphers)
		{
			IDictionary<string, string> queries = HTTPHelperYoutube.ParseQueryString(s);
			bool requiresDecryption = false;
			if (queries.ContainsKey("sp"))
			{
				YoutubePlayer.SignatureQuery = "sig";
			}
			else
			{
				YoutubePlayer.SignatureQuery = "signatures";
			}
			string url;
			if (queries.ContainsKey("s") || queries.ContainsKey("signature"))
			{
				requiresDecryption = queries.ContainsKey("s");
				string text = ((!queries.ContainsKey("s")) ? queries["signature"] : queries["s"]);
				if (YoutubePlayer.sp != "none")
				{
					url = string.Format("{0}&{1}={2}", queries["url"], YoutubePlayer.SignatureQuery, text);
				}
				else
				{
					url = string.Format("{0}&{1}={2}", queries["url"], YoutubePlayer.SignatureQuery, text);
				}
				string text2 = ((!queries.ContainsKey("fallback_host")) ? string.Empty : ("&fallback_host=" + queries["fallback_host"]));
				url += text2;
			}
			else
			{
				url = queries["url"];
			}
			url = HTTPHelperYoutube.UrlDecode(url);
			url = HTTPHelperYoutube.UrlDecode(url);
			IDictionary<string, string> parameters = HTTPHelperYoutube.ParseQueryString(url);
			if (!parameters.ContainsKey("ratebypass"))
			{
				url += string.Format("&{0}={1}", "ratebypass", "yes");
			}
			yield return new YoutubePlayer.ExtractionInfo
			{
				RequiresDecryption = requiresDecryption,
				Uri = new Uri(url)
			};
		}
		foreach (string s2 in urls)
		{
			string url2 = s2;
			url2 = HTTPHelperYoutube.UrlDecode(url2);
			url2 = HTTPHelperYoutube.UrlDecode(url2);
			IDictionary<string, string> parameters2 = HTTPHelperYoutube.ParseQueryString(url2);
			if (!parameters2.ContainsKey("ratebypass"))
			{
				url2 += string.Format("&{0}={1}", "ratebypass", "yes");
			}
			yield return new YoutubePlayer.ExtractionInfo
			{
				RequiresDecryption = false,
				Uri = new Uri(url2)
			};
		}
		yield break;
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x000F1540 File Offset: 0x000EF940
	private static string GetAdaptiveStreamMap(JObject json)
	{
		JToken jtoken = json["args"]["adaptive_fmts"];
		if (jtoken == null)
		{
			jtoken = json["args"]["url_encoded_fmt_stream_map"];
			if (jtoken == null)
			{
				JObject jobject = JObject.Parse(json["args"]["player_response"].ToString());
				jtoken = jobject["streamingData"]["adaptiveFormats"];
			}
		}
		return jtoken.ToString();
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x000F15C4 File Offset: 0x000EF9C4
	private static string GetHtml5PlayerVersion(JObject json, string regexForHtmlPVersions)
	{
		Regex regex = new Regex(regexForHtmlPVersions);
		string text = json["assets"]["js"].ToString();
		YoutubePlayer.jsUrl = "https://www.youtube.com" + text;
		Match match = regex.Match(text);
		if (match.Success)
		{
			return match.Result("$1");
		}
		regex = new Regex("player_ias(.+?).js");
		match = regex.Match(text);
		if (match.Success)
		{
			return match.Result("$1");
		}
		regex = new Regex("player-(.+?).js");
		return regex.Match(text).Result("$1");
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x000F1668 File Offset: 0x000EFA68
	private static string TryMatchHtmlVersion(string input, string regexForHtmlPVersions)
	{
		Regex regex = new Regex(regexForHtmlPVersions);
		Match match = regex.Match(input);
		if (match.Success)
		{
			return match.Result("$1");
		}
		regex = new Regex("player_ias(.+?).js");
		match = regex.Match(input);
		if (match.Success)
		{
			return match.Result("$1");
		}
		regex = new Regex("player-(.+?).js");
		return regex.Match(input).Result("$1");
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x000F16E4 File Offset: 0x000EFAE4
	private static string GetStreamMap(JObject json)
	{
		JToken jtoken = json["args"]["url_encoded_fmt_stream_map"];
		if (jtoken == null)
		{
			JObject jobject = JObject.Parse(json["args"]["player_response"].ToString());
			jtoken = jobject["streamingData"]["formats"];
		}
		string text = ((jtoken != null) ? jtoken.ToString() : null);
		if (text != null && !text.Contains("been+removed"))
		{
			return text;
		}
		if (text.Contains("been+removed"))
		{
			throw new VideoNotAvailableException("Video is removed or has an age restriction.");
		}
		return null;
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x000F178C File Offset: 0x000EFB8C
	private static IEnumerable<VideoInfo> GetVideoInfos(IEnumerable<YoutubePlayer.ExtractionInfo> extractionInfos, string videoTitle)
	{
		List<VideoInfo> list = new List<VideoInfo>();
		foreach (YoutubePlayer.ExtractionInfo extractionInfo in extractionInfos)
		{
			string text = HTTPHelperYoutube.ParseQueryString(extractionInfo.Uri.Query)["itag"];
			int formatCode = int.Parse(text);
			VideoInfo videoInfo2 = VideoInfo.Defaults.SingleOrDefault((VideoInfo videoInfo) => videoInfo.FormatCode == formatCode);
			if (videoInfo2 != null)
			{
				videoInfo2 = new VideoInfo(videoInfo2)
				{
					DownloadUrl = extractionInfo.Uri.ToString(),
					Title = videoTitle,
					RequiresDecryption = extractionInfo.RequiresDecryption
				};
			}
			else
			{
				videoInfo2 = new VideoInfo(formatCode)
				{
					DownloadUrl = extractionInfo.Uri.ToString()
				};
			}
			list.Add(videoInfo2);
		}
		return list;
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x000F1894 File Offset: 0x000EFC94
	private static string GetVideoTitle(JObject json)
	{
		JToken jtoken = json["videoDetails"]["title"];
		return (jtoken != null) ? jtoken.ToString() : string.Empty;
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x000F18CD File Offset: 0x000EFCCD
	private static bool IsVideoUnavailable(string pageSource)
	{
		return pageSource.Contains("<div id=\"watch-player-unavailable\">");
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x000F18DC File Offset: 0x000EFCDC
	private IEnumerator DownloadUrl(string url, Action<string> callback, VideoInfo videoInfo)
	{
		UnityWebRequest request = UnityWebRequest.Get(url);
		request.SetRequestHeader("User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:10.0) Gecko/20100101 Firefox/10.0 (Chrome)");
		yield return request.SendWebRequest();
		if (request.isNetworkError)
		{
			Debug.Log("Youtube UnityWebRequest isNetworkError!");
		}
		else if (request.isHttpError)
		{
			Debug.Log("Youtube UnityWebRequest isHttpError!");
		}
		else if (request.responseCode != 200L)
		{
			Debug.Log("Youtube UnityWebRequest responseCode:" + request.responseCode);
		}
		yield break;
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x000F18F8 File Offset: 0x000EFCF8
	private IEnumerator DownloadYoutubeUrl(string url, Action callback, YoutubePlayer player)
	{
		this.downloadYoutubeUrlResponse = new YoutubePlayer.DownloadUrlResponse();
		string videoId = url.Replace("https://youtube.com/watch?v=", string.Empty);
		string newUrl = "https://www.youtube.com/watch?v=" + videoId + "&gl=US&hl=en&has_verified=1&bpctr=9999999999";
		UnityWebRequest request = UnityWebRequest.Get(newUrl);
		request.SetRequestHeader("User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:10.0) Gecko/20100101 Firefox/10.0 (Chrome)");
		yield return request.SendWebRequest();
		this.downloadYoutubeUrlResponse.httpCode = request.responseCode;
		if (request.isNetworkError)
		{
			Debug.Log("Youtube UnityWebRequest isNetworkError!");
		}
		else if (request.isHttpError)
		{
			Debug.Log("Youtube UnityWebRequest isHttpError!");
		}
		else if (request.responseCode == 200L)
		{
			if (request.downloadHandler != null && request.downloadHandler.text != null)
			{
				if (request.downloadHandler.isDone)
				{
					this.downloadYoutubeUrlResponse.isValid = true;
					this.jsonForHtmlVersion = request.downloadHandler.text;
					this.downloadYoutubeUrlResponse.data = request.downloadHandler.text;
				}
			}
			else
			{
				Debug.Log("Youtube UnityWebRequest Null response");
			}
		}
		else
		{
			Debug.Log("Youtube UnityWebRequest responseCode:" + request.responseCode);
		}
		base.StartCoroutine(this.YoutubeURLDownloadFinished(callback, player));
		yield break;
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x000F1928 File Offset: 0x000EFD28
	public static void WriteLog(string filename, string c)
	{
		string text = string.Concat(new string[]
		{
			Application.persistentDataPath,
			"/",
			filename,
			"_",
			DateTime.Now.ToString("ddMMyyyyhhmmssffff"),
			".txt"
		});
		Debug.Log("Log written in: " + Application.persistentDataPath);
		File.WriteAllText(text, c);
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x000F1995 File Offset: 0x000EFD95
	private static void ThrowYoutubeParseException(Exception innerException, string videoUrl)
	{
		throw new YoutubeParseException("Could not parse the Youtube page for URL " + videoUrl + "\nThis may be due to a change of the Youtube page structure.\nPlease report this bug at kelvinparkour@gmail.com with a subject message 'Parse Error' ", innerException);
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x000F19B0 File Offset: 0x000EFDB0
	public void TrySkip(PointerEventData eventData)
	{
		Vector2 vector;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.progress.rectTransform, eventData.position, eventData.pressEventCamera, out vector))
		{
			float num = Mathf.InverseLerp(this.progress.rectTransform.rect.xMin, this.progress.rectTransform.rect.xMax, vector.x);
			this.SkipToPercent(num);
		}
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x000F1A24 File Offset: 0x000EFE24
	private void SkipToPercent(float pct)
	{
		float num = this.videoPlayer.frameCount * pct;
		this.videoPlayer.Pause();
		if (this.videoQuality != YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.audioPlayer.Pause();
		}
		this.waitAudioSeek = true;
		if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			this.videoPlayer.frame = (long)num;
		}
		else
		{
			this.audioPlayer.frame = (long)num;
		}
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x000F1A94 File Offset: 0x000EFE94
	private IEnumerator VideoSeekCall()
	{
		yield return new WaitForSeconds(1f);
		this.videoPlayer.time = this.audioPlayer.time;
		yield break;
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x000F1AB0 File Offset: 0x000EFEB0
	private void VideoSeeked(VideoPlayer source)
	{
		if (!this.waitAudioSeek)
		{
			if (this.startedFromTime)
			{
				base.StartCoroutine(this.PlayNowFromTime(2f));
			}
			else
			{
				base.StartCoroutine(this.PlayNow());
			}
		}
		else if (this.startedFromTime)
		{
			base.StartCoroutine(this.PlayNowFromTime(2f));
		}
		else
		{
			base.StartCoroutine(this.PlayNow());
		}
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x000F1B2B File Offset: 0x000EFF2B
	private void AudioSeeked(VideoPlayer source)
	{
		if (!this.waitAudioSeek)
		{
			base.StartCoroutine(this.VideoSeekCall());
		}
		else
		{
			base.StartCoroutine(this.VideoSeekCall());
		}
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000F1B58 File Offset: 0x000EFF58
	private IEnumerator WaitSync()
	{
		yield return new WaitForSeconds(2f);
		this.Play();
		base.Invoke("VerifyFrames", 2f);
		yield break;
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x000F1B74 File Offset: 0x000EFF74
	private IEnumerator PlayNow()
	{
		if (this.videoQuality == YoutubePlayer.YoutubeVideoQuality.STANDARD)
		{
			yield return new WaitForSeconds(0f);
		}
		else
		{
			yield return new WaitForSeconds(1f);
		}
		if (!this.pauseCalled)
		{
			this.Play();
			base.StartCoroutine(this.ReleaseDrop());
		}
		else
		{
			base.StopCoroutine("PlayNow");
		}
		yield break;
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x000F1B8F File Offset: 0x000EFF8F
	private void CheckIfIsSync()
	{
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x000F1B94 File Offset: 0x000EFF94
	private IEnumerator ReleaseDrop()
	{
		yield return new WaitForSeconds(2f);
		yield break;
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x000F1BA8 File Offset: 0x000EFFA8
	private IEnumerator PlayNowFromTime(float time)
	{
		yield return new WaitForSeconds(time);
		this.startedFromTime = false;
		if (!this.pauseCalled)
		{
			this.Play();
		}
		else
		{
			base.StopCoroutine("PlayNowFromTime");
		}
		yield break;
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x000F1BCA File Offset: 0x000EFFCA
	private void AudioPlayer_frameDropped(VideoPlayer source)
	{
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x000F1BCC File Offset: 0x000EFFCC
	private void VideoPlayer_frameDropped(VideoPlayer source)
	{
	}

	// Token: 0x04001868 RID: 6248
	private const string USER_AGENT = "Mozilla/5.0 (X11; Linux x86_64; rv:10.0) Gecko/20100101 Firefox/10.0 (Chrome)";

	// Token: 0x04001869 RID: 6249
	private const bool INTERNALDEBUG = false;

	// Token: 0x0400186A RID: 6250
	[Space]
	[Tooltip("You can put urls that start at a specific time example: 'https://youtu.be/1G1nCxxQMnA?t=67'")]
	public string youtubeUrl;

	// Token: 0x0400186B RID: 6251
	[Space]
	[Space]
	[Tooltip("The desired video quality you want to play. It's in experimental mod, because we need to use 2 video players in qualities 720+, you can expect some desync, but we are working to find a definitive solution to that. Thanks to DASH format.")]
	public YoutubePlayer.YoutubeVideoQuality videoQuality;

	// Token: 0x0400186C RID: 6252
	[Space]
	public bool customPlaylist;

	// Token: 0x0400186D RID: 6253
	[DrawIf("customPlaylist", true, DrawIfAttribute.DisablingType.DontDraw)]
	public bool autoPlayNextVideo;

	// Token: 0x0400186E RID: 6254
	[Header("If is a custom playlist put urls here")]
	public string[] youtubeUrls;

	// Token: 0x0400186F RID: 6255
	private int currentUrlIndex;

	// Token: 0x04001870 RID: 6256
	[Space]
	[Header("Playback Options")]
	[Space]
	[Tooltip("Start playing the video from a desired time")]
	public bool startFromSecond;

	// Token: 0x04001871 RID: 6257
	[DrawIf("startFromSecond", true, DrawIfAttribute.DisablingType.DontDraw)]
	public int startFromSecondTime;

	// Token: 0x04001872 RID: 6258
	[Space]
	[Tooltip("Play the video when the script initialize")]
	public bool autoPlayOnStart = true;

	// Token: 0x04001873 RID: 6259
	[Header("For Mobiles Leave MP4 ")]
	public YoutubePlayer.VideoFormatType videoFormat;

	// Token: 0x04001874 RID: 6260
	[Space]
	[Tooltip("Play or continue when OnEnable is called")]
	public bool autoPlayOnEnable;

	// Token: 0x04001875 RID: 6261
	[Space]
	[Header("Use Device Video player (Standard quality only)")]
	[Tooltip("Play video in mobiles using the mobile device video player not unity internal player")]
	public bool playUsingInternalDevicePlayer;

	// Token: 0x04001876 RID: 6262
	[Space]
	[Header("Only load the url to use in a custom player.")]
	[Space]
	[Tooltip("If you want to use your custom player, you can enable this and set the callback OnYoutubeUrlLoaded and get the public variables audioUrl or videoUrl of that script.")]
	public bool loadYoutubeUrlsOnly;

	// Token: 0x04001877 RID: 6263
	[Space]
	[Header("Render the same video to more objects")]
	[Tooltip("Render the same video player material to a different materials, if you want")]
	public GameObject[] objectsToRenderTheVideoImage;

	// Token: 0x04001878 RID: 6264
	[Space]
	[Header("Option for 3D video Only.")]
	[Tooltip("If the video is a 3D video sidebyside or Over/Under")]
	public bool is3DLayoutVideo;

	// Token: 0x04001879 RID: 6265
	[DrawIf("is3DLayoutVideo", true, DrawIfAttribute.DisablingType.DontDraw)]
	public YoutubePlayer.Layout3D layout3d;

	// Token: 0x0400187A RID: 6266
	[Space]
	[Header("Video Controller Canvas")]
	public GameObject videoControllerCanvas;

	// Token: 0x0400187B RID: 6267
	[Space]
	public Camera mainCamera;

	// Token: 0x0400187C RID: 6268
	[Space]
	[Header("Loading Settings")]
	[Tooltip("This enable and disable related to the loading needs.")]
	public GameObject loadingContent;

	// Token: 0x0400187D RID: 6269
	[Header("Custom user Events To use with video player only")]
	[Tooltip("When the url's are loaded")]
	public UrlLoadEvent OnYoutubeUrlAreReady;

	// Token: 0x0400187E RID: 6270
	[Tooltip("When the videos are ready to play")]
	public UnityEvent OnVideoReadyToStart;

	// Token: 0x0400187F RID: 6271
	[Tooltip("When the video start playing")]
	public UnityEvent OnVideoStarted;

	// Token: 0x04001880 RID: 6272
	[Tooltip("When the video pause")]
	public UnityEvent OnVideoPaused;

	// Token: 0x04001881 RID: 6273
	[Tooltip("When the video finish")]
	public UnityEvent OnVideoFinished;

	// Token: 0x04001882 RID: 6274
	[Space]
	[Header("The unity video players")]
	[Tooltip("The unity video player")]
	public VideoPlayer videoPlayer;

	// Token: 0x04001883 RID: 6275
	[Tooltip("The audio player, (Needed for videos that dont have audio included 720p+)")]
	public VideoPlayer audioPlayer;

	// Token: 0x04001884 RID: 6276
	[Space]
	[Tooltip("Show the output in the console")]
	public bool debug;

	// Token: 0x04001885 RID: 6277
	[Space]
	[Tooltip("Ignore timeout is good for very low connections")]
	public bool ignoreTimeout;

	// Token: 0x04001886 RID: 6278
	[Space]
	[SerializeField]
	[Header("If the video stucks you can try to disable this.")]
	private bool _skipOnDrop = true;

	// Token: 0x04001887 RID: 6279
	[HideInInspector]
	public string videoUrl;

	// Token: 0x04001888 RID: 6280
	[HideInInspector]
	public string audioUrl;

	// Token: 0x04001889 RID: 6281
	[HideInInspector]
	public bool ForceGetWebServer;

	// Token: 0x0400188A RID: 6282
	[Space]
	[Header("Screen Controls")]
	[Tooltip("Show the video controller in screen [slider with progress, video time, play pause, etc...]")]
	public bool showPlayerControls;

	// Token: 0x0400188B RID: 6283
	private int maxRequestTime = 5;

	// Token: 0x0400188C RID: 6284
	private float currentRequestTime;

	// Token: 0x0400188D RID: 6285
	private int retryTimeUntilToRequestFromServer = 1;

	// Token: 0x0400188E RID: 6286
	private int currentRetryTime;

	// Token: 0x0400188F RID: 6287
	private bool gettingYoutubeURL;

	// Token: 0x04001890 RID: 6288
	private bool videoAreReadyToPlay;

	// Token: 0x04001891 RID: 6289
	private float lastPlayTime;

	// Token: 0x04001892 RID: 6290
	private bool audioDecryptDone;

	// Token: 0x04001893 RID: 6291
	private bool videoDecryptDone;

	// Token: 0x04001894 RID: 6292
	private bool isRetry;

	// Token: 0x04001895 RID: 6293
	private string lastTryVideoId;

	// Token: 0x04001896 RID: 6294
	private float lastStartedTime;

	// Token: 0x04001897 RID: 6295
	private bool youtubeUrlReady;

	// Token: 0x04001898 RID: 6296
	private static bool needUpdate;

	// Token: 0x04001899 RID: 6297
	private YoutubePlayer.YoutubeResultIds newRequestResults;

	// Token: 0x0400189A RID: 6298
	private static string jsUrl;

	// Token: 0x0400189B RID: 6299
	private const string serverURI = "https://lightshaftstream.herokuapp.com/api/info?url=";

	// Token: 0x0400189C RID: 6300
	private const string formatURI = "&format=best&flatten=true";

	// Token: 0x0400189D RID: 6301
	private const string VIDEOURIFORWEBGLPLAYER = "https://youtubewebgl.herokuapp.com/download.php?mime=video/mp4&title=generatedvideo&token=";

	// Token: 0x0400189E RID: 6302
	private bool fullscreenModeEnabled;

	// Token: 0x0400189F RID: 6303
	private long lastFrame = -1L;

	// Token: 0x040018A0 RID: 6304
	private double lastTimePlayed = double.PositiveInfinity;

	// Token: 0x040018A1 RID: 6305
	private bool videoEnded;

	// Token: 0x040018A2 RID: 6306
	private bool noAudioAtacched;

	// Token: 0x040018A3 RID: 6307
	private string videoTitle = string.Empty;

	// Token: 0x040018A4 RID: 6308
	private bool startedFromTime;

	// Token: 0x040018A5 RID: 6309
	private bool finishedCalled;

	// Token: 0x040018A6 RID: 6310
	private bool videoStarted;

	// Token: 0x040018A7 RID: 6311
	private float lastErrorTime;

	// Token: 0x040018A8 RID: 6312
	[HideInInspector]
	public bool pauseCalled;

	// Token: 0x040018A9 RID: 6313
	[DrawIf("showPlayerControls", true, DrawIfAttribute.DisablingType.DontDraw)]
	[Tooltip("Hide the controls if use not interact in desired time, 0 equals to not hide")]
	public int autoHideControlsTime;

	// Token: 0x040018AA RID: 6314
	[DrawIf("showPlayerControls", true, DrawIfAttribute.DisablingType.DontDraw)]
	[Tooltip("The main controller ui parent")]
	public GameObject mainControllerUi;

	// Token: 0x040018AB RID: 6315
	[DrawIf("showPlayerControls", true, DrawIfAttribute.DisablingType.DontDraw)]
	[Tooltip("Slider with duration and progress")]
	public Image progress;

	// Token: 0x040018AC RID: 6316
	[DrawIf("showPlayerControls", true, DrawIfAttribute.DisablingType.DontDraw)]
	[Tooltip("Volume slider")]
	public Slider volumeSlider;

	// Token: 0x040018AD RID: 6317
	[DrawIf("showPlayerControls", true, DrawIfAttribute.DisablingType.DontDraw)]
	[Tooltip("Playback speed")]
	public Slider playbackSpeed;

	// Token: 0x040018AE RID: 6318
	[DrawIf("showPlayerControls", true, DrawIfAttribute.DisablingType.DontDraw)]
	[Tooltip("Current Time")]
	public Text currentTimeString;

	// Token: 0x040018AF RID: 6319
	[DrawIf("showPlayerControls", true, DrawIfAttribute.DisablingType.DontDraw)]
	[Tooltip("Total Time")]
	public Text totalTimeString;

	// Token: 0x040018B0 RID: 6320
	private float totalVideoDuration;

	// Token: 0x040018B1 RID: 6321
	private float currentVideoDuration;

	// Token: 0x040018B2 RID: 6322
	private bool videoSeekDone;

	// Token: 0x040018B3 RID: 6323
	private bool videoAudioSeekDone;

	// Token: 0x040018B4 RID: 6324
	private bool lowRes;

	// Token: 0x040018B5 RID: 6325
	private float hideScreenTime;

	// Token: 0x040018B6 RID: 6326
	private float audioDuration;

	// Token: 0x040018B7 RID: 6327
	private bool showingPlaybackSpeed;

	// Token: 0x040018B8 RID: 6328
	private bool showingVolume;

	// Token: 0x040018B9 RID: 6329
	private string lsigForVideo;

	// Token: 0x040018BA RID: 6330
	private string lsigForAudio;

	// Token: 0x040018BB RID: 6331
	private Thread thread1;

	// Token: 0x040018BC RID: 6332
	private YoutubePlayer.YoutubeResultIds webGlResults;

	// Token: 0x040018BD RID: 6333
	private bool startedPlayingWebgl;

	// Token: 0x040018BE RID: 6334
	private string logTest = "/";

	// Token: 0x040018BF RID: 6335
	[HideInInspector]
	public bool isSyncing;

	// Token: 0x040018C0 RID: 6336
	[Header("Experimental")]
	public bool showThumbnailBeforeVideoLoad;

	// Token: 0x040018C1 RID: 6337
	private string thumbnailVideoID;

	// Token: 0x040018C2 RID: 6338
	private const string RateBypassFlag = "ratebypass";

	// Token: 0x040018C3 RID: 6339
	[HideInInspector]
	public static string SignatureQuery = "sig";

	// Token: 0x040018C4 RID: 6340
	[HideInInspector]
	public string encryptedSignatureVideo;

	// Token: 0x040018C5 RID: 6341
	[HideInInspector]
	public string encryptedSignatureAudio;

	// Token: 0x040018C6 RID: 6342
	[HideInInspector]
	private string masterURLForVideo;

	// Token: 0x040018C7 RID: 6343
	[HideInInspector]
	private string masterURLForAudio;

	// Token: 0x040018C8 RID: 6344
	private string[] patternNames = new string[] { string.Empty };

	// Token: 0x040018C9 RID: 6345
	[HideInInspector]
	public bool decryptedUrlForVideo;

	// Token: 0x040018CA RID: 6346
	[HideInInspector]
	public bool decryptedUrlForAudio;

	// Token: 0x040018CB RID: 6347
	[HideInInspector]
	public string decryptedVideoUrlResult = string.Empty;

	// Token: 0x040018CC RID: 6348
	[HideInInspector]
	public string decryptedAudioUrlResult = string.Empty;

	// Token: 0x040018CD RID: 6349
	public List<VideoInfo> youtubeVideoInfos;

	// Token: 0x040018CE RID: 6350
	private string htmlVersion = string.Empty;

	// Token: 0x040018CF RID: 6351
	private static string sp = string.Empty;

	// Token: 0x040018D0 RID: 6352
	[HideInInspector]
	public string EncryptUrlForVideo;

	// Token: 0x040018D1 RID: 6353
	[HideInInspector]
	public string EncryptUrlForAudio;

	// Token: 0x040018D2 RID: 6354
	private YoutubePlayer.DownloadUrlResponse downloadYoutubeUrlResponse;

	// Token: 0x040018D3 RID: 6355
	[HideInInspector]
	public string jsonForHtmlVersion = string.Empty;

	// Token: 0x040018D4 RID: 6356
	private bool waitAudioSeek;

	// Token: 0x040018D5 RID: 6357
	[HideInInspector]
	public bool checkIfSync;

	// Token: 0x040018D6 RID: 6358
	private YoutubePlayer.MagicContent magicResult;

	// Token: 0x040018D7 RID: 6359
	private bool canUpdate = true;

	// Token: 0x020002C9 RID: 713
	public enum YoutubeVideoQuality
	{
		// Token: 0x040018D9 RID: 6361
		STANDARD,
		// Token: 0x040018DA RID: 6362
		HD,
		// Token: 0x040018DB RID: 6363
		FULLHD,
		// Token: 0x040018DC RID: 6364
		UHD1440,
		// Token: 0x040018DD RID: 6365
		UHD2160
	}

	// Token: 0x020002CA RID: 714
	public enum VideoFormatType
	{
		// Token: 0x040018DF RID: 6367
		MP4,
		// Token: 0x040018E0 RID: 6368
		WEBM
	}

	// Token: 0x020002CB RID: 715
	public enum PlayerType
	{
		// Token: 0x040018E2 RID: 6370
		simple,
		// Token: 0x040018E3 RID: 6371
		advanced
	}

	// Token: 0x020002CC RID: 716
	public enum Layout3D
	{
		// Token: 0x040018E5 RID: 6373
		sideBySide,
		// Token: 0x040018E6 RID: 6374
		OverUnder
	}

	// Token: 0x020002CD RID: 717
	public class YoutubeResultIds
	{
		// Token: 0x040018E7 RID: 6375
		public string lowQuality;

		// Token: 0x040018E8 RID: 6376
		public string standardQuality;

		// Token: 0x040018E9 RID: 6377
		public string mediumQuality;

		// Token: 0x040018EA RID: 6378
		public string hdQuality;

		// Token: 0x040018EB RID: 6379
		public string fullHdQuality;

		// Token: 0x040018EC RID: 6380
		public string ultraHdQuality;

		// Token: 0x040018ED RID: 6381
		public string bestFormatWithAudioIncluded;

		// Token: 0x040018EE RID: 6382
		public string audioUrl;
	}

	// Token: 0x020002CE RID: 718
	public class Html5PlayerResult
	{
		// Token: 0x06001AEB RID: 6891 RVA: 0x000F1BEC File Offset: 0x000EFFEC
		public Html5PlayerResult(string _script, string _result, bool _valid)
		{
			this.scriptName = _script;
			this.result = _result;
			this.isValid = _valid;
		}

		// Token: 0x040018EF RID: 6383
		public string scriptName;

		// Token: 0x040018F0 RID: 6384
		public string result;

		// Token: 0x040018F1 RID: 6385
		public bool isValid;
	}

	// Token: 0x020002CF RID: 719
	private class DownloadUrlResponse
	{
		// Token: 0x06001AEC RID: 6892 RVA: 0x000F1C09 File Offset: 0x000F0009
		public DownloadUrlResponse()
		{
			this.data = null;
			this.isValid = false;
			this.httpCode = 0L;
		}

		// Token: 0x040018F2 RID: 6386
		public string data;

		// Token: 0x040018F3 RID: 6387
		public bool isValid;

		// Token: 0x040018F4 RID: 6388
		public long httpCode;
	}

	// Token: 0x020002D0 RID: 720
	private class ExtractionInfo
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x000F1C2F File Offset: 0x000F002F
		// (set) Token: 0x06001AEF RID: 6895 RVA: 0x000F1C37 File Offset: 0x000F0037
		public bool RequiresDecryption { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x000F1C40 File Offset: 0x000F0040
		// (set) Token: 0x06001AF1 RID: 6897 RVA: 0x000F1C48 File Offset: 0x000F0048
		public Uri Uri { get; set; }
	}

	// Token: 0x020002D1 RID: 721
	private class MagicContent
	{
		// Token: 0x040018F7 RID: 6391
		public string[] regexForFuncName;

		// Token: 0x040018F8 RID: 6392
		public string regexForHtmlJson;

		// Token: 0x040018F9 RID: 6393
		public string regexForHtmlPlayerVersion;

		// Token: 0x040018FA RID: 6394
		public string[] defaultFuncName = new string[] { "(?:\\b|[^\\w$])([\\w$]{2})\\s*=\\s*function\\(\\s*a\\s*\\)\\s*{\\s*a\\s*=\\s*a\\.split\\(\\s*\"\"\\s*\\)", "(\\w+)=function\\(\\w+\\){(\\w+)=\\2\\.split\\(\\x22{2}\\);.*?return\\s+\\2\\.join\\(\\x22{2}\\)}", "\\b[cs]\\s*&&\\s*[adf]\\.set\\([^,]+\\s*,\\s*encodeURIComponent\\s*\\(\\s*([\\w$]+)\\(" };

		// Token: 0x040018FB RID: 6395
		public string defaultHtmlJson = "ytplayer\\.config\\s*=\\s*(\\{.+?\\});ytplayer";

		// Token: 0x040018FC RID: 6396
		public string defaultHtmlPlayerVersion = "player_ias-(.+?).js";
	}
}
