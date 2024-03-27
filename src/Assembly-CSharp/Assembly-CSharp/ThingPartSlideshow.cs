using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000279 RID: 633
public class ThingPartSlideshow : MonoBehaviour
{
	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06001802 RID: 6146 RVA: 0x000DC73B File Offset: 0x000DAB3B
	// (set) Token: 0x06001803 RID: 6147 RVA: 0x000DC743 File Offset: 0x000DAB43
	public int urlIndex { get; private set; }

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06001804 RID: 6148 RVA: 0x000DC74C File Offset: 0x000DAB4C
	// (set) Token: 0x06001805 RID: 6149 RVA: 0x000DC754 File Offset: 0x000DAB54
	public string searchText { get; private set; }

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06001806 RID: 6150 RVA: 0x000DC75D File Offset: 0x000DAB5D
	// (set) Token: 0x06001807 RID: 6151 RVA: 0x000DC765 File Offset: 0x000DAB65
	public bool running { get; private set; }

	// Token: 0x06001808 RID: 6152 RVA: 0x000DC76E File Offset: 0x000DAB6E
	private void Start()
	{
		this.screen = this.AddScreenQuad();
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x000DC77C File Offset: 0x000DAB7C
	private void Update()
	{
		if (Our.IsMasterClient(false) && this.running && Time.time >= this.lastShownTime + 3f)
		{
			this.urlIndex++;
			if (this.urlIndex >= this.urls.Count)
			{
				this.urlIndex = 0;
			}
			Managers.personManager.DoWeSwitchedToThisSlideshowImage(this, this.urlIndex);
			this.lastShownTime = Time.time;
		}
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x000DC7FC File Offset: 0x000DABFC
	public void MasterClientSwitchedToThisImage(int urlIndex)
	{
		if (urlIndex < this.urls.Count)
		{
			this.urlIndex = urlIndex;
			this.LoadAndShowTexture();
		}
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x000DC81C File Offset: 0x000DAC1C
	public List<string> GetUrls()
	{
		return this.urls;
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x000DC824 File Offset: 0x000DAC24
	public bool HasUrls()
	{
		return this.urls.Count >= 1;
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x000DC837 File Offset: 0x000DAC37
	public void SetUrls(List<string> urls, string searchText = "", int urlIndex = 0)
	{
		this.urls = urls;
		this.urlIndex = urlIndex;
		this.lastShownTime = -1f;
		this.searchText = searchText;
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x000DC859 File Offset: 0x000DAC59
	public void Play()
	{
		this.running = true;
		this.lastShownTime = Time.time;
		this.LoadAndShowTexture();
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x000DC873 File Offset: 0x000DAC73
	public void Pause()
	{
		this.running = false;
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x000DC87C File Offset: 0x000DAC7C
	private void OnDestroy()
	{
		global::UnityEngine.Object.Destroy(this.screen);
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x000DC88C File Offset: 0x000DAC8C
	private GameObject AddScreenQuad()
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/SlideshowQuad", typeof(GameObject))) as GameObject;
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		bool flag = Misc.GetLargestValueOfVector(base.transform.localScale) >= 5f;
		gameObject.transform.Rotate(90f, 0f, 180f);
		float num = ((!flag) ? 0.0025f : 0.013f);
		gameObject.transform.Translate(Vector3.forward * -(base.transform.localScale.y / 2f + num));
		Vector3 localScale = base.transform.localScale;
		float num2 = localScale.z / localScale.x;
		if (num2 >= 0.75f)
		{
			gameObject.transform.localScale = new Vector3(1f, 0.75f, 1f);
		}
		else
		{
			gameObject.transform.localScale = new Vector3(0.75f, 1f, 1f);
		}
		gameObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
		return gameObject;
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x000DCA03 File Offset: 0x000DAE03
	private void LoadAndShowTexture()
	{
		base.StartCoroutine(this.DoLoadAndShowTexture());
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x000DCA14 File Offset: 0x000DAE14
	private IEnumerator DoLoadAndShowTexture()
	{
		Texture2D texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		string url = "https://tse2.mm.bing.net/th?id=OIP." + this.urls[this.urlIndex];
		using (WWW www = new WWW(url))
		{
			yield return www;
			if (this.screen != null)
			{
				if (string.IsNullOrEmpty(www.error))
				{
					www.LoadImageIntoTexture(texture);
					Material material = this.screen.GetComponent<Renderer>().material;
					this.ResizeScreenProportionally(this.screen.transform, texture);
					material.mainTexture = texture;
				}
				else
				{
					Log.Debug("DoLoadAndShowTexture image load error: " + www.error);
				}
			}
		}
		yield break;
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x000DCA30 File Offset: 0x000DAE30
	private void ResizeScreenProportionally(Transform screen, Texture2D texture)
	{
		Transform parent = screen.transform.parent;
		screen.parent = null;
		float x = parent.localScale.x;
		float z = parent.localScale.z;
		float num = 1f;
		float num2 = 1f;
		float num3 = (float)texture.width;
		float num4 = (float)texture.height;
		if (num3 > num4)
		{
			num = 1f;
			num2 = num4 / num3;
		}
		else if (num4 > num3)
		{
			num = num3 / num4;
			num2 = 1f;
		}
		float num5 = x / num;
		float num6 = z / num2;
		float num7 = Math.Min(num5, num6);
		float num8 = num * num7;
		float num9 = num2 * num7;
		screen.localScale = new Vector3(num8, num9, 1f);
		screen.parent = parent;
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x000DCB04 File Offset: 0x000DAF04
	public void LoadSearchResultsAndTriggerPlay(string searchText)
	{
		this.searchText = searchText;
		base.StartCoroutine(this.GetImageSearchResults(searchText, delegate(List<string> urls)
		{
			if (urls.Count >= 1)
			{
				ThingPart component = this.gameObject.GetComponent<ThingPart>();
				Managers.personManager.DoSlideshowControl_SetUrls(component, urls, searchText);
				Managers.personManager.DoSlideshowControl_Play(component);
			}
		}));
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x000DCB50 File Offset: 0x000DAF50
	public IEnumerator GetImageSearchResults(string searchText, Action<List<string>> callback)
	{
		string url = "https://www.bing.com/images/search?FORM=HDRSC2&safesearch=moderate&q=" + WWW.EscapeURL("+" + searchText);
		WWW www = new WWW(url);
		yield return www;
		callback(this.GetSearchResultImageUrls(www.text));
		yield break;
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x000DCB7C File Offset: 0x000DAF7C
	private List<string> GetSearchResultImageUrls(string html)
	{
		List<string> list = new List<string>();
		List<string> textsBetween = Misc.GetTextsBetween(html, "https://tse2.mm.bing.net/th?id=OIP.", "\"");
		int num = 0;
		while (num < textsBetween.Count && num < 50)
		{
			string text = textsBetween[num];
			if (text.Contains("&"))
			{
				text = Misc.Split(text, "&", StringSplitOptions.RemoveEmptyEntries)[0];
			}
			text = text.Trim();
			list.Add(text);
			num++;
		}
		return list;
	}

	// Token: 0x04001666 RID: 5734
	private const string urlStart = "https://tse2.mm.bing.net/th?id=OIP.";

	// Token: 0x04001667 RID: 5735
	private List<string> urls = new List<string>();

	// Token: 0x04001669 RID: 5737
	private float lastShownTime = -1f;

	// Token: 0x0400166C RID: 5740
	private GameObject screen;

	// Token: 0x0400166D RID: 5741
	private const float frequencySeconds = 3f;
}
