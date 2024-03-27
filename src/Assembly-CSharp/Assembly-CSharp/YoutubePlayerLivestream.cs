using System;
using System.Collections;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using YoutubeLight;

// Token: 0x020002D4 RID: 724
public class YoutubePlayerLivestream : MonoBehaviour
{
	// Token: 0x06001AF6 RID: 6902 RVA: 0x000F424F File Offset: 0x000F264F
	private void Start()
	{
		this.GetLivestreamUrl(this._livestreamUrl);
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000F425D File Offset: 0x000F265D
	public void GetLivestreamUrl(string url)
	{
		this.StartProcess(new Action<string>(this.OnLiveUrlLoaded), url);
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x000F4272 File Offset: 0x000F2672
	public void StartProcess(Action<string> callback, string url)
	{
		base.StartCoroutine(this.DownloadYoutubeUrl(url, callback));
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x000F4283 File Offset: 0x000F2683
	private void OnLiveUrlLoaded(string url)
	{
		Debug.Log("You can check how to use double clicking in that log");
		Debug.Log("This is the live url, pass to the player: " + url);
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x000F42A0 File Offset: 0x000F26A0
	private IEnumerator DownloadYoutubeUrl(string url, Action<string> callback)
	{
		this.downloadYoutubeUrlResponse = new YoutubePlayerLivestream.DownloadUrlResponse();
		string videoId = url.Replace("https://youtube.com/watch?v=", string.Empty);
		string newUrl = "https://www.youtube.com/watch?v=" + videoId + "&gl=US&hl=en&has_verified=1&bpctr=9999999999";
		UnityWebRequest request = UnityWebRequest.Get(url);
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
		base.StartCoroutine(this.GetUrlFromJson(callback, videoId, request.downloadHandler.text));
		yield break;
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x000F42CC File Offset: 0x000F26CC
	private IEnumerator GetUrlFromJson(Action<string> callback, string _videoID, string pageSource)
	{
		string player_response = string.Empty;
		if (Regex.IsMatch(pageSource, "[\"\\']status[\"\\']\\s*:\\s*[\"\\']LOGIN_REQUIRED"))
		{
			Debug.Log("MM");
			string url = "https://www.youtube.com/get_video_info?video_id=" + _videoID + "&eurl=https://youtube.googleapis.com/v/" + _videoID;
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
			Match match = regex.Match(pageSource);
			if (match.Success)
			{
				string text = match.Result("$1");
				if (!text.Contains("raw_player_response:ytInitialPlayerResponse"))
				{
					player_response = JObject.Parse(text)["args"]["player_response"].ToString();
				}
			}
			regex = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;\\s*(?:var\\s+meta|</script|\\n)", RegexOptions.Multiline);
			match = regex.Match(pageSource);
			if (match.Success)
			{
				player_response = match.Result("$1");
			}
			regex = new Regex("ytInitialPlayerResponse\\s*=\\s*({.+?})\\s*;", RegexOptions.Multiline);
			match = regex.Match(pageSource);
			if (match.Success)
			{
				player_response = match.Result("$1");
			}
		}
		JObject json = JObject.Parse(player_response);
		bool isLive = json["videoDetails"]["isLive"].Value<bool>();
		if (isLive)
		{
			string text2 = json["streamingData"]["hlsManifestUrl"].ToString();
			callback(text2);
		}
		else
		{
			Debug.Log("This is not a livestream url");
		}
		yield break;
	}

	// Token: 0x040018FD RID: 6397
	public string _livestreamUrl;

	// Token: 0x040018FE RID: 6398
	private YoutubePlayerLivestream.DownloadUrlResponse downloadYoutubeUrlResponse;

	// Token: 0x020002D5 RID: 725
	private class DownloadUrlResponse
	{
		// Token: 0x06001AFC RID: 6908 RVA: 0x000F42F5 File Offset: 0x000F26F5
		public DownloadUrlResponse()
		{
			this.data = null;
			this.isValid = false;
			this.httpCode = 0L;
		}

		// Token: 0x040018FF RID: 6399
		public string data;

		// Token: 0x04001900 RID: 6400
		public bool isValid;

		// Token: 0x04001901 RID: 6401
		public long httpCode;
	}
}
