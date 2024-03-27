using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class SoundLibraryManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06001329 RID: 4905 RVA: 0x000A69AC File Offset: 0x000A4DAC
	// (set) Token: 0x0600132A RID: 4906 RVA: 0x000A69B4 File Offset: 0x000A4DB4
	public ManagerStatus status { get; private set; }

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x0600132B RID: 4907 RVA: 0x000A69BD File Offset: 0x000A4DBD
	// (set) Token: 0x0600132C RID: 4908 RVA: 0x000A69C5 File Offset: 0x000A4DC5
	public string failMessage { get; private set; }

	// Token: 0x0600132D RID: 4909 RVA: 0x000A69CE File Offset: 0x000A4DCE
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.soundParent = Managers.treeManager.GetTransform("/Universe/TemporarySounds");
		this.AddNormalSounds();
		this.AddLoopSounds();
		this.loopNames.Sort();
		this.status = ManagerStatus.Started;
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x000A6A0C File Offset: 0x000A4E0C
	private void Update()
	{
		if (this.status != ManagerStatus.Started)
		{
			return;
		}
		this.didPlaySoundThisUpdate = false;
		if (this.clipsPlayedThisUpdate.Count >= 1 && (this.timeOfLastClipsPlayedClearing == -1f || this.timeOfLastClipsPlayedClearing + 0.05f <= Time.time))
		{
			this.timeOfLastClipsPlayedClearing = Time.time;
			this.clipsPlayedThisUpdate = new Dictionary<string, bool>();
		}
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x000A6A7C File Offset: 0x000A4E7C
	public void Play(Vector3 position, Sound sound, bool isLoop = false, bool isOfSoundTrack = false, bool forceSurroundSound = false, float volumeModulator = -1f)
	{
		if (!this.Enabled)
		{
			return;
		}
		if (!this.clipsPlayedThisUpdate.ContainsKey(sound.name))
		{
			this.clipsPlayedThisUpdate.Add(sound.name, true);
			this.didPlaySoundThisUpdate = true;
			base.StartCoroutine(this.PlayAsync(position, sound, isLoop, isOfSoundTrack, forceSurroundSound, volumeModulator));
		}
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x000A6ADC File Offset: 0x000A4EDC
	private IEnumerator PlayAsync(Vector3 position, Sound sound, bool isLoop, bool isOfSoundTrack, bool forceSurroundSound, float volumeModulator)
	{
		if (isLoop)
		{
			string loopFilePath = this.GetFilePath(sound.name, isLoop, false);
			if (loopFilePath != null)
			{
				ResourceRequest loopRequest = Resources.LoadAsync(loopFilePath);
				yield return loopRequest;
				AudioClip audioClip = loopRequest.asset as AudioClip;
				AudioSource.PlayClipAtPoint(audioClip, position, sound.volume);
			}
		}
		else
		{
			int index = this.names.FindIndex((string item) => item == sound.name);
			if (index != -1)
			{
				string filePath = this.GetFilePath(this.names[index], isLoop, sound.stretch);
				if (!this.cachedClips.ContainsKey(filePath))
				{
					if (this.cachedClips.Count >= 75)
					{
						this.cachedClips = new Dictionary<string, AudioClip>();
					}
					ResourceRequest request = Resources.LoadAsync(filePath);
					yield return request;
					this.cachedClips.Add(filePath, request.asset as AudioClip);
				}
				AudioClip clip = null;
				if (this.cachedClips.TryGetValue(filePath, out clip))
				{
					this.PlayClipAtPoint(sound.name, clip, position, sound, forceSurroundSound, volumeModulator);
				}
				if (!isOfSoundTrack && this.recordingKeyboard != null)
				{
					this.recordingKeyboard.RecordSound(sound);
				}
			}
		}
		yield break;
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x000A6B24 File Offset: 0x000A4F24
	public bool SoundExists(string name)
	{
		return this.names.FindIndex((string item) => item == name) >= 0;
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x000A6B5C File Offset: 0x000A4F5C
	private void PlayClipAtPoint(string name, AudioClip clip, Vector3 position, Sound sound, bool forceSurroundSound, float volumeModulator)
	{
		GameObject gameObject = new GameObject(sound.name);
		gameObject.transform.position = position;
		gameObject.transform.parent = this.soundParent;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.spatialBlend = 1f;
		audioSource.dopplerLevel = 0f;
		float num = 0.2f;
		if (volumeModulator != -1f)
		{
			num = volumeModulator;
		}
		audioSource.volume = sound.volume * num;
		float num2 = 1f;
		if (sound.pitch != SoundLibraryManager.defaultPitch)
		{
			audioSource.pitch = sound.pitch;
			num2 /= sound.pitch;
		}
		if (sound.pitchVariance != 0f)
		{
			float num3 = audioSource.pitch + global::UnityEngine.Random.Range(-sound.pitchVariance, sound.pitchVariance);
			audioSource.pitch = Mathf.Clamp(num3, 0.5f, SoundLibraryManager.maxPitch);
			if (audioSource.pitch < SoundLibraryManager.defaultPitch)
			{
				num2 = 5f;
			}
		}
		if (sound.echo)
		{
			AudioEchoFilter audioEchoFilter = gameObject.AddComponent<AudioEchoFilter>();
			audioEchoFilter.decayRatio = 0.2f;
			audioEchoFilter.dryMix = 0.5f;
			audioEchoFilter.wetMix = 0.5f;
			num2 = 5f;
		}
		if (sound.lowPass)
		{
			AudioLowPassFilter audioLowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
			audioLowPassFilter.cutoffFrequency = 2000f;
		}
		else if (sound.highPass)
		{
			AudioHighPassFilter audioHighPassFilter = gameObject.AddComponent<AudioHighPassFilter>();
			audioHighPassFilter.cutoffFrequency = 2000f;
		}
		if (sound.reverse)
		{
			audioSource.timeSamples = clip.samples - 1;
			audioSource.pitch *= -1f;
		}
		if (sound.surround || forceSurroundSound)
		{
			audioSource.spatialBlend = 0f;
		}
		int num4 = 1 + sound.repeatCount;
		float num5 = sound.secondsDelay + clip.length * num2 * (float)num4;
		float num6 = ((sound.secondsDuration == 0f) ? num5 : (sound.secondsDelay + sound.secondsDuration));
		if (sound.secondsToSkip != 0f && sound.secondsToSkip < num5)
		{
			try
			{
				audioSource.time = sound.secondsToSkip;
			}
			catch (Exception ex)
			{
				Log.Debug(ex);
			}
		}
		audioSource.loop = num4 > 1;
		if (sound.secondsDelay > 0f)
		{
			audioSource.PlayDelayed(sound.secondsDelay);
		}
		else
		{
			audioSource.Play();
		}
		global::UnityEngine.Object.Destroy(gameObject, num6);
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x000A6E08 File Offset: 0x000A5208
	private void AddInstrumentTones(string name, int minOctave, int maxOctave, int licenseId = -1)
	{
		for (int i = minOctave; i <= maxOctave; i++)
		{
			string text = i.ToString();
			foreach (string text2 in this.musicNotes)
			{
				this.Add(string.Concat(new string[] { name, " ", text2, " ", text }), licenseId, false);
			}
		}
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x000A6E8C File Offset: 0x000A528C
	private void AddEnumeratedTones(string name, int min, int max, int licenseId = -1)
	{
		for (int i = min; i <= max; i++)
		{
			this.Add(name + " " + i, licenseId, false);
		}
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x000A6EC5 File Offset: 0x000A52C5
	public void SortIfNeeded()
	{
		if (!this.didSortNames)
		{
			this.AddRemoveZeroPrefixToCertainNumberNames(true);
			this.names.Sort();
			this.AddRemoveZeroPrefixToCertainNumberNames(false);
			this.didSortNames = true;
		}
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x000A6EF4 File Offset: 0x000A52F4
	private void AddRemoveZeroPrefixToCertainNumberNames(bool doAdd)
	{
		for (int i = 1; i <= 4; i++)
		{
			for (int j = 1; j <= 9; j++)
			{
				string text = string.Concat(new object[] { "guitar", i, " ", j });
				string text2 = string.Concat(new object[] { "guitar", i, " 0", j });
				if (!doAdd)
				{
					string text3 = text;
					text = text2;
					text2 = text3;
				}
				int num = this.names.IndexOf(text);
				if (num >= 0)
				{
					this.names[num] = text2;
				}
			}
		}
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x000A6FB4 File Offset: 0x000A53B4
	private void Add(string name, int licenseId = -1, bool isLoop = false)
	{
		this.idCounter++;
		if (isLoop)
		{
			this.loopNames.Add(name);
		}
		else
		{
			this.names.Add(name);
		}
		this.namesById.Add(this.idCounter, name);
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x000A7004 File Offset: 0x000A5404
	public string GetNameById(int id)
	{
		string text = null;
		string text2;
		if (this.namesById.TryGetValue(id, out text2))
		{
			text = text2;
		}
		return text;
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x000A702C File Offset: 0x000A542C
	public int GetIdByName(string name)
	{
		int num = -1;
		foreach (KeyValuePair<int, string> keyValuePair in this.namesById)
		{
			if (keyValuePair.Value == name)
			{
				num = keyValuePair.Key;
				break;
			}
		}
		return num;
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x000A70A4 File Offset: 0x000A54A4
	public string GetFilePath(string name, bool isLoop = false, bool useStretch = false)
	{
		string text = name.Replace(" ", "_");
		text = text.Replace("-", "_");
		string text2 = "SoundLibrary/";
		if (isLoop)
		{
			text2 += "Loops/";
		}
		else if (useStretch)
		{
			text2 += "StretchedX2/";
		}
		return text2 + text;
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x000A710D File Offset: 0x000A550D
	public string GetLoopSoundPath(string name)
	{
		return (this.loopNames.IndexOf(name) < 0) ? null : this.GetFilePath(name, true, false);
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x000A7130 File Offset: 0x000A5530
	public List<string> GetSearchResults(string search)
	{
		search = search.Trim();
		List<string> list = new List<string>();
		string[] array = Misc.Split(search, " ", StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in this.names)
		{
			bool flag = true;
			foreach (string text2 in array)
			{
				if (!text.Contains(text2))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(text);
			}
		}
		return list;
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x000A71EC File Offset: 0x000A55EC
	private void AddLoopSounds()
	{
		this.Add("fire", 138018, true);
		this.Add("water", 339324, true);
		this.Add("hum", 119838, true);
		this.Add("hum 2", 261858, true);
		this.Add("hum 3", 91986, true);
		this.Add("hum 4", 124626, true);
		this.Add("hum 5", 59, true);
		this.Add("birds", 345852, true);
		this.Add("birds 2", 214869, true);
		this.Add("wind", 335889, true);
		this.Add("wind 2", 92654, true);
		this.Add("bubbles", 133901, true);
		this.Add("glimmer", -1, true);
		this.Add("machine", 267156, true);
		this.Add("machine 2", 204510, true);
		this.Add("machine 3", 207123, true);
		this.Add("machine 4", 58496, true);
		this.Add("machine 5", 76211, true);
		this.Add("machine 6", 174822, true);
		this.Add("machine 7", 325414, true);
		this.Add("alarm", 324394, true);
		this.Add("alarm 2", 205951, true);
		this.Add("alarm 3", 248211, true);
		this.Add("alarm 4", 33751, true);
		this.Add("alarm 5", 270883, true);
		this.Add("alarm 6", 117793, true);
		this.Add("alarm 7", 88445, true);
		this.Add("alarm 8", 56778, true);
		this.Add("alarm 9", 195660, true);
		this.Add("alarm 10", 96973, true);
		this.Add("alarm 11", 17468, true);
		this.Add("chanting", 66583, true);
		this.Add("crying voices", 26724, true);
		this.Add("computer", 4009, true);
		this.Add("air fade", 417314, true);
		this.Add("rain", 275598, true);
		this.Add("bubbles 2", 95244, true);
		this.Add("om", 106561, true);
		this.Add("heartbeat", 418866, true);
		this.Add("scary violins", 244417, true);
		this.Add("unmusic", 349119, true);
		this.Add("retro computer", 266156, true);
		this.Add("f plus quint", 268176, true);
		this.Add("water 2", 346746, true);
		this.Add("music street ambient", 331027, true);
		this.Add("noise", 153494, true);
		this.Add("diesel engine", 26972, true);
		this.Add("mystical aura", 166185, true);
		this.Add("chatting crowd", 111201, true);
		this.Add("mystical aura 2", 120365, true);
		this.Add("chatting crowd 2", 261985, true);
		this.Add("horror music", 166179, true);
		this.Add("orchestra", 420762, true);
		this.Add("future ambient", 143913, true);
		this.Add("wall of sound", 269341, true);
		this.Add("future ambient 2", 424285, true);
		this.Add("future ambient 3", 241071, true);
		this.Add("strange ambient", 345010, true);
		this.Add("harbor seagulls", 11, true);
		this.Add("vacuum suction", 11, true);
		this.Add("freightliner wagons", 11, true);
		this.Add("multiverse interaction", 11, true);
		this.Add("eerie cave", 11, true);
		this.Add("auto interior freeway", 11, true);
		this.Add("helicopter", 11, true);
		this.Add("industrial ambience", 11, true);
		this.Add("cockpit ambience", 11, true);
		this.Add("synth ambience", 11, true);
		this.Add("heavy winds", 11, true);
		this.Add("eerie synth", 11, true);
		this.Add("lake waves", 11, true);
		this.Add("boat slosh", 11, true);
		this.Add("ferry metal", 11, true);
		this.Add("ferry rattle", 11, true);
		this.Add("propeller airplane", 11, true);
		this.Add("amazonia farm", 11, true);
		this.Add("jungle ambient", 11, true);
		this.Add("caves ambient", 11, true);
		this.Add("desert ambient", 11, true);
		this.Add("construction site", 11, true);
		this.Add("city autos", 11, true);
		this.Add("evening cricket", 11, true);
		this.Add("wind distant traffic", 11, true);
		this.Add("countryside birds", 11, true);
		this.Add("cheering crowd", 11, true);
		this.Add("city voices", 11, true);
		this.Add("hall crowd", 11, true);
		this.Add("mall crowd", 11, true);
		this.Add("arcade air hockey", 11, true);
		this.Add("fair crowd", 11, true);
		this.Add("heavy interference", 11, true);
		this.Add("whirring ambience", 11, true);
		this.Add("wavering soundscape", 11, true);
		this.Add("heavy fan", 11, true);
		this.Add("bubbling fan", 11, true);
		this.Add("exhaust buzz", 11, true);
		this.Add("helicopter 2", 11, true);
		this.Add("planet rumble", 11, true);
		this.Add("electro magnetic", 11, true);
		this.Add("magical water", 11, true);
		this.Add("industrial ambience 2", 11, true);
		this.Add("metallic ball rolling", 11, true);
		this.Add("tank drive", 11, true);
		this.Add("park people", 11, true);
		this.Add("park wind", 11, true);
		this.Add("seaplane", 11, true);
		this.Add("seaplane 2", 11, true);
		this.Add("eerie ambience", 11, true);
		this.Add("coastal docks", 11, true);
		this.Add("coastal restaurants", 11, true);
		this.Add("city rain", 11, true);
		this.Add("city rain 2", 11, true);
		this.Add("synth ambience 2", 11, true);
		this.Add("hologram ambience", 11, true);
		this.Add("sea between rocks", 11, true);
		this.Add("sea between rocks 2", 11, true);
		this.Add("pedestrian street", 11, true);
		this.Add("radiation planet", 11, true);
		this.Add("rain on auto", 11, true);
		this.Add("rain on concrete", 11, true);
		this.Add("jet ambience", 11, true);
		this.Add("room tone", 11, true);
		this.Add("computer room", 11, true);
		this.Add("furnace ambient", 11, true);
		this.Add("outside ambience", 11, true);
		this.Add("ultraviolet rays", 11, true);
		this.Add("seawash", 11, true);
		this.Add("seawash heavy", 11, true);
		this.Add("winter city streets", 11, true);
		this.Add("winter pedestrians", 11, true);
		this.Add("singing bowl", 11, true);
		this.Add("singing bowl 2", 11, true);
		this.Add("nature birds insects", 11, true);
		this.Add("heavy winds 2", 11, true);
		this.Add("propeller airplane 2", 11, true);
		this.Add("subway line", 11, true);
		this.Add("mountain sheep", 11, true);
		this.Add("mountain stream", 11, true);
		this.Add("city autos 2", 11, true);
		this.Add("train station", 11, true);
		this.Add("diesel train", 11, true);
		this.Add("transit train", 11, true);
		this.Add("transit train 2", 11, true);
		this.Add("steam train", 11, true);
		this.Add("typewriter ambience", 11, true);
		this.Add("underwater ambience", 11, true);
		this.Add("underwater lava", 11, true);
		this.Add("winter wind", 11, true);
		this.Add("motor drive", 11, true);
		this.Add("motor drive 2", 11, true);
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x000A7AD4 File Offset: 0x000A5ED4
	private void AddNormalSounds()
	{
		this.AddInstrumentTones("tone", 2, 4, -1);
		this.AddInstrumentTones("trumpet", 3, 5, 357478);
		this.AddInstrumentTones("fiddle", 3, 5, 56228);
		this.AddInstrumentTones("bell", 2, 4, -1);
		this.AddInstrumentTones("piano2", 1, 3, -1);
		this.AddEnumeratedTones("guitar1", 1, 54, -1);
		this.AddEnumeratedTones("guitar2", 1, 54, -1);
		this.AddEnumeratedTones("guitar3", 1, 54, -1);
		this.AddEnumeratedTones("guitar4", 1, 54, -1);
		this.Add("bleeps", 219506, false);
		this.Add("alien communication", 174240, false);
		this.Add("double bleep", 124907, false);
		this.Add("bleeps hi", 219506, false);
		this.Add("bleeps lo", 219506, false);
		this.Add("alien communication hi", 174240, false);
		this.Add("alien communication lo", 174240, false);
		this.Add("double bleep lo", 124907, false);
		this.Add("background hum", 194239, false);
		this.Add("hovering flow", 22678, false);
		this.Add("space radio", 41911, false);
		this.Add("eerie space", 58307, false);
		this.Add("odd sci-fi machine", 46484, false);
		this.Add("tense", 235450, false);
		this.Add("mellow zoom", 128349, false);
		this.Add("laser shot", 151022, false);
		this.Add("futuristic warp", 66807, false);
		this.Add("dry laser shot", 130126, false);
		this.Add("dry double laser shot", 130126, false);
		this.Add("drinking", -1, false);
		this.Add("engine starts", -1, false);
		this.Add("glass shatters", -1, false);
		this.Add("colliding", -1, false);
		this.Add("taking in", -1, false);
		this.Add("odd laugh", -1, false);
		this.Add("munching", -1, false);
		this.Add("munching bite", -1, false);
		this.Add("emit", -1, false);
		this.Add("smash", -1, false);
		this.Add("success bling", -1, false);
		this.Add("bounce", -1, false);
		this.Add("water splash", -1, false);
		this.Add("water splash thick", -1, false);
		this.Add("whoosh", -1, false);
		this.Add("bear", -1, false);
		this.Add("bird", -1, false);
		this.Add("cat", -1, false);
		this.Add("chick", -1, false);
		this.Add("dog", -1, false);
		this.Add("elephant", -1, false);
		this.Add("frog", -1, false);
		this.Add("hen", -1, false);
		this.Add("horse", -1, false);
		this.Add("monkey", -1, false);
		this.Add("pig", -1, false);
		this.Add("piglet", -1, false);
		this.Add("pony", -1, false);
		this.Add("rooster", -1, false);
		this.Add("sheep", -1, false);
		this.Add("snake", -1, false);
		this.Add("wildcat", -1, false);
		this.Add("wolf", -1, false);
		this.Add("drum", -1, false);
		this.Add("drum 2", -1, false);
		this.Add("drum 3", -1, false);
		this.Add("piano key", -1, false);
		this.Add("piano key 2", -1, false);
		this.Add("bell", -1, false);
		this.Add("bell hi", -1, false);
		this.Add("bongo", -1, false);
		this.Add("bubbles", -1, false);
		this.Add("bubbles 2", -1, false);
		this.Add("city", -1, false);
		this.Add("dripping", -1, false);
		this.Add("glimmer", -1, false);
		this.Add("hum", -1, false);
		this.Add("lightning", -1, false);
		this.Add("birds", -1, false);
		this.Add("ocean", -1, false);
		this.Add("quake", -1, false);
		this.Add("raindrop", -1, false);
		this.Add("village", -1, false);
		this.Add("wind", -1, false);
		this.Add("notification", 221359, false);
		this.Add("phone vibration", 121346, false);
		this.Add("on the phone", 162019, false);
		this.Add("phone ring", 124959, false);
		this.Add("magic 2", 221683, false);
		this.Add("magic 3", 178318, false);
		this.Add("magic 4", 241727, false);
		this.Add("magic 1", 216089, false);
		this.Add("magic 5", 219571, false);
		this.Add("magic 6", 219570, false);
		this.Add("magic 7", 219566, false);
		this.Add("magic 8", 241809, false);
		this.Add("magic 9", 172876, false);
		this.Add("magic 10", 172876, false);
		this.Add("magic 11", 172876, false);
		this.Add("magic 12", 172876, false);
		this.Add("magic 13", 211624, false);
		this.Add("magic 14", 214455, false);
		this.Add("magic 15", 234546, false);
		this.Add("magic 16", 20134, false);
		this.Add("magic fizzle 1", 204303, false);
		this.Add("magic fizzle 2", 204303, false);
		this.Add("magic fizzle 3", 204303, false);
		this.Add("baby laughter", 47370, false);
		this.Add("can opened", 69165, false);
		this.Add("brewing machine", 60459, false);
		this.Add("washer", 50704, false);
		this.Add("drums playing", 43567, false);
		this.Add("electronic beeping", 3647, false);
		this.Add("filling_with_air", -1, false);
		this.Add("rustling", 79385, false);
		this.Add("future mystery", 58962, false);
		this.Add("clinging", 83678, false);
		this.Add("guitar", 4258, false);
		this.Add("piano playing", 47817, false);
		this.Add("pool table playing", 47245, false);
		this.Add("popcorn", -1, false);
		this.Add("shower water sprinkling", 18616, false);
		this.Add("toilet flushing", 114602, false);
		this.Add("metal clink", 63506, false);
		this.Add("table tennis ball bumping", 39705, false);
		this.Add("vacuum cleaner", 19872, false);
		this.Add("classic video game sound", -1, false);
		this.Add("fire", 17730, false);
		this.Add("soda machine", 84710, false);
		this.Add("inserting coin", 233013, false);
		this.Add("inserting coin 2", 113095, false);
		this.Add("sword hitting", 175949, false);
		this.Add("sword draw", 162560, false);
		this.Add("sword slicing", 109432, false);
		this.Add("sword draw 2", 218479, false);
		this.Add("swiping hit", 209126, false);
		this.Add("big woomph", 86016, false);
		this.Add("energy shot", 49672, false);
		this.Add("air cut", 84616, false);
		this.Add("double swoosh", 19312, false);
		this.Add("knife sharpening", 175520, false);
		this.Add("hitting metal", 91529, false);
		this.Add("notification chime", 202029, false);
		this.Add("microwave bing", 171450, false);
		this.Add("crystal", 153102, false);
		this.Add("doorbell", 81072, false);
		this.Add("knocking on wooden door", 120491, false);
		this.Add("knocking on metal door", 68702, false);
		this.Add("chopping wood", 23700, false);
		this.Add("pick axe", 61307, false);
		this.Add("hammering", 207782, false);
		this.Add("hammer", 207782, false);
		this.Add("shot", 163456, false);
		this.Add("shot and reload", 149177, false);
		this.Add("reload", 149177, false);
		this.Add("empty gun shot", 154934, false);
		this.Add("pool shot", 138402, false);
		this.Add("machine gun shots", 212600, false);
		this.Add("machine gun shot", 205013, false);
		this.Add("firework rocket", 205609, false);
		this.Add("rocket fired", 66233, false);
		this.Add("arrow impact", 205938, false);
		this.Add("arrow shot", 205563, false);
		this.Add("chest opening", 202092, false);
		this.Add("getting coins", 160502, false);
		this.Add("getting coins 2", 156891, false);
		this.Add("holy chorus", 170602, false);
		this.Add("holy mystery", 170568, false);
		this.Add("microphone feedback", 42931, false);
		this.Add("tv static", 154674, false);
		this.Add("button click", 173420, false);
		this.Add("button click 2", 107145, false);
		this.Add("button click 3", 107145, false);
		this.Add("button click 4", 107145, false);
		this.Add("key unlocking door", 119918, false);
		this.Add("key locking door", 118232, false);
		this.Add("key unlocking door 2", 131438, false);
		this.Add("rumbling", 216605, false);
		this.Add("shaking", 235561, false);
		this.Add("oven humming", 219505, false);
		this.Add("typing", 240280, false);
		this.Add("machine starting", 122683, false);
		this.Add("machine stopping", 199759, false);
		this.Add("door screeching and squeaking", 98152, false);
		this.Add("flonsh", 24206, false);
		this.Add("opening", 63113, false);
		this.Add("electronic pickup", 170170, false);
		this.Add("energetic power up", 157419, false);
		this.Add("electronic power up", 220173, false);
		this.Add("power up", 132560, false);
		this.Add("wrong", 210579, false);
		this.Add("wrong 2", 192376, false);
		this.Add("correct", 131660, false);
		this.Add("correct 2", 181859, false);
		this.Add("danger", 130396, false);
		this.Add("danger 2", 130396, false);
		this.Add("danger 3", 198415, false);
		this.Add("action", 231254, false);
		this.Add("soft whoosh", 168180, false);
		this.Add("romantic", 217799, false);
		this.Add("party noise maker", 203246, false);
		this.Add("dinner party", 72848, false);
		this.Add("happy music", 189965, false);
		this.Add("music box", 201060, false);
		this.Add("party music", 167233, false);
		this.Add("fire_with_alarm", 25032, false);
		this.Add("arcades", 99561, false);
		this.Add("beauty salon", 50599, false);
		this.Add("crowd cheering", -1, false);
		this.Add("explosion", 33245, false);
		this.Add("night club", 38938, false);
		this.Add("lever pulled", 105379, false);
		this.Add("machine hum", 65175, false);
		this.Add("pinball machine playing", 34730, false);
		this.Add("plates clanking", 47245, false);
		this.Add("sink water flowing", 23872, false);
		this.Add("voice hey", 131063, false);
		this.Add("voice hey hi", 131063, false);
		this.Add("voice hey lo", 131063, false);
		this.Add("attention seeking whistle", 77752, false);
		this.Add("short whistle", 151092, false);
		this.Add("voice heyah", 95660, false);
		this.Add("voice mmmm", 19541, false);
		this.Add("voice brrr", 86212, false);
		this.Add("voice hi", 238819, false);
		this.Add("voice hello", 238819, false);
		this.Add("voice hello hi", 238819, false);
		this.Add("voice hello higher", 238819, false);
		this.Add("voice hello lo", 238819, false);
		this.Add("voice hi there", 238819, false);
		this.Add("voice hello 2", 238819, false);
		this.Add("voice evil laughter", 237984, false);
		this.Add("voice scream", 208893, false);
		this.Add("voice shock", 149492, false);
		this.Add("voice welcome", 213288, false);
		this.Add("voice giggle", 204496, false);
		this.Add("voice evil laughter 2", 219110, false);
		this.Add("voice crazy laughter", 79769, false);
		this.Add("voice group laugh", 207797, false);
		this.Add("voice evil laughter 3", 126113, false);
		this.Add("voice muhahaha", 167578, false);
		this.Add("voice laugh", 93775, false);
		this.Add("voice muhahaha 2", 120762, false);
		this.Add("crowd laugh", 219450, false);
		this.Add("voice laugh 2", 235102, false);
		this.Add("voice laugh 3", 15291, false);
		this.Add("baby laugh", 110392, false);
		this.Add("voice hmmm", 22090, false);
		this.Add("voice hmmm 2", 22090, false);
		this.Add("voice hmmm 3", 170767, false);
		this.Add("voice ahmm", 170767, false);
		this.Add("wilhelm scream", 13797, false);
		this.Add("voice scream 2", 220619, false);
		this.Add("squelchy squirt", 167073, false);
		this.Add("futuristic background noise", 30304, false);
		this.Add("epic horns", 104675, false);
		this.Add("epic horns 2", 104832, false);
		this.Add("monster growl", 192364, false);
		this.Add("monster roar", 192364, false);
		this.Add("monster hiss", 192364, false);
		this.Add("monster scream", 219945, false);
		this.Add("monster scream 2", 233279, false);
		this.Add("monster scream 3", 92620, false);
		this.Add("scary horror background", 216050, false);
		this.Add("scary horror background 2", 132925, false);
		this.Add("ghost whoosh", 216095, false);
		this.Add("scary horror background 3", 127962, false);
		this.Add("scary horror ghost", 177768, false);
		this.Add("chains dropped", 191511, false);
		this.Add("chains rattling", 191511, false);
		this.Add("breaking stick", 152413, false);
		this.Add("bashing", 152732, false);
		this.Add("finger snapping", 137104, false);
		this.Add("clap", 194464, false);
		this.Add("keycard", 118230, false);
		this.Add("electric buzz hum", 184569, false);
		this.Add("electric spark", 189630, false);
		this.Add("electric shock zap", 136542, false);
		this.Add("sparks", 32682, false);
		this.Add("electric machine started", 175397, false);
		this.Add("electric shock zap 2", 18381, false);
		this.Add("success", 109663, false);
		this.Add("success 2", 109662, false);
		this.Add("success 3", 242501, false);
		this.Add("lose", 159408, false);
		this.Add("no scream", 180350, false);
		this.Add("lose 2", 232444, false);
		this.Add("lose 3", 43697, false);
		this.Add("pouring water", 168868, false);
		this.Add("ba da bum tish joke drums", 221642, false);
		this.Add("rising", 237493, false);
		this.Add("lowering", 237493, false);
		this.Add("voice cough", 151217, false);
		this.Add("sneeze", 185619, false);
		this.Add("aspirin in water", 175057, false);
		this.Add("throat clearing", 151921, false);
		this.Add("car driving away", 128190, false);
		this.Add("race countdown", 165991, false);
		this.Add("airplane passing by", 86040, false);
		this.Add("car passing by", 171447, false);
		this.Add("sports car passing by", 74384, false);
		this.Add("group cheering", 99634, false);
		this.Add("whip crack", 93100, false);
		this.Add("glitchy theme", -1, false);
		this.Add("glitchy moon music", -1, false);
		this.Add("glitchy meditation", -1, false);
		this.Add("glitchy meditation 2", -1, false);
		this.Add("glitchy metalmaker", -1, false);
		this.Add("glitchy rainbow run", -1, false);
		this.Add("glitchy spinning wheel", -1, false);
		this.Add("glitchy musicblock dr1", -1, false);
		this.Add("glitchy musicblock dg2", -1, false);
		this.Add("glitchy musicblock xs3", -1, false);
		this.Add("voice hello 3", 213284, false);
		this.Add("voice hello 4", 173114, false);
		this.Add("voice konichiwa", 235022, false);
		this.Add("voice hello there", 81129, false);
		this.Add("voice hello 5", 173091, false);
		this.Add("echoing taiko drum", 204111, false);
		this.Add("computer voice hello", 42352, false);
		this.Add("voice squeaky hello", 241164, false);
		this.Add("computer sounds", 193714, false);
		this.Add("voice pleased mmm", 169342, false);
		this.Add("female robot voice 1", 169342, false);
		this.Add("female robot voice 2", 169342, false);
		this.Add("female robot voice 3", 169342, false);
		this.Add("female robot voice 4", 169342, false);
		this.Add("female robot voice 5", 169342, false);
		this.Add("female robot voice 6", 169342, false);
		this.Add("female robot voice 7", 169342, false);
		this.Add("female robot voice 8", 169342, false);
		this.Add("female robot voice 9", 169342, false);
		this.Add("female robot voice 10", 169342, false);
		this.Add("slot machine winning coins", 156891, false);
		this.Add("slot machine winning coins 2", 69682, false);
		this.Add("slot machine winning coins 3", 69682, false);
		this.Add("slot machine started", 209578, false);
		this.Add("sonar ping", 70299, false);
		this.Add("sonar ping 2", 28693, false);
		this.Add("roulette wheel spinning", 59194, false);
		this.Add("wheel spinning", 170907, false);
		this.Add("horn", 188039, false);
		this.Add("horn 2", 175946, false);
		this.Add("horn 3", 186645, false);
		this.Add("man laugh", 166150, false);
		this.Add("horn 6", 27881, false);
		this.Add("punch", 89769, false);
		this.Add("punch 2", 118513, false);
		this.Add("punch 3", 244513, false);
		this.Add("punch 4", 96486, false);
		this.Add("arcades 2", 79700, false);
		this.Add("ship horn", 208714, false);
		this.Add("punches", 96486, false);
		this.Add("car horn", 182474, false);
		this.Add("bongo set 1", -1, false);
		this.Add("bongo set 2", -1, false);
		this.Add("bongo set 3", -1, false);
		this.Add("bongo set 4", -1, false);
		this.Add("bongo set 5", -1, false);
		this.Add("bongo set 6", -1, false);
		this.Add("bongo set 7", -1, false);
		this.Add("bongo set 8", -1, false);
		this.Add("drum set 1", -1, false);
		this.Add("drum set 2", -1, false);
		this.Add("drum set 3", -1, false);
		this.Add("drum set 4", -1, false);
		this.Add("drum set 5", -1, false);
		this.Add("drum set 6", -1, false);
		this.Add("drum set 7", -1, false);
		this.Add("drum set 8", -1, false);
		this.Add("drum set 9", -1, false);
		this.Add("drum set 10", -1, false);
		this.Add("drum set 11", -1, false);
		this.Add("drum set 12", -1, false);
		this.Add("drum set 13", -1, false);
		this.Add("drum set 14", -1, false);
		this.Add("drum set 15", -1, false);
		this.Add("drum set 16", -1, false);
		this.Add("drum set 17", -1, false);
		this.Add("drum set 18", -1, false);
		this.Add("drum set 19", -1, false);
		this.Add("drum set 20", -1, false);
		this.Add("drum set 21", -1, false);
		this.Add("drum set 22", -1, false);
		this.Add("creature painful scream", 213842, false);
		this.Add("creature grunt or pain", 103528, false);
		this.Add("male pain grunt", 238346, false);
		this.Add("male pain grunt 2", 166944, false);
		this.Add("male pain grunt 3", 135855, false);
		this.Add("creature pain", 115887, false);
		this.Add("pain scream", 220693, false);
		this.Add("male pain grunt 4", 90164, false);
		this.Add("male pain scream", 162157, false);
		this.Add("female pain grunt", 234039, false);
		this.Add("female pain scream", 203976, false);
		this.Add("female pain scream 2", 42847, false);
		this.Add("female pain grunt 2", 234039, false);
		this.Add("female pain grunt 3", 234039, false);
		this.Add("magic 17", 186675, false);
		this.Add("glitchy loading", -1, false);
		this.Add("glitchy tincture", -1, false);
		this.Add("glitchy quest complete", -1, false);
		this.Add("glitchy imagination upgrade", -1, false);
		this.Add("glitchy cauldron", -1, false);
		this.Add("glitchy level up kazoo", -1, false);
		this.Add("glitchy loaded 2", -1, false);
		this.Add("glitchy new quest alert", -1, false);
		this.Add("glitchy qurazy quoin", -1, false);
		this.Add("glitchy discover new item", -1, false);
		this.Add("glitchy piggy plop", -1, false);
		this.Add("ho ho ho voice", 110641, false);
		this.Add("owl", 187669, false);
		this.Add("evil laugh", 145619, false);
		this.Add("robot wow", 252230, false);
		this.Add("crying", 219433, false);
		this.Add("dragon roar", 185069, false);
		this.Add("wawa", 256142, false);
		this.Add("i am your death", 235842, false);
		this.Add("metal gong", -1, false);
		this.Add("sad trombone", 73581, false);
		this.Add("tada trumpet", 152574, false);
		this.Add("sorry dave", 102537, false);
		this.Add("self destruct sequence", 219593, false);
		this.Add("kiss", 239907, false);
		this.Add("blowing a kiss", 88450, false);
		this.Add("smoochie pop", 74238, false);
		this.Add("clonk", 320456, false);
		this.Add("oven fan starting", 320453, false);
		this.Add("air hammer drill", 320451, false);
		this.Add("wading through water", 320430, false);
		this.Add("eerie cycloid spiral", 320429, false);
		this.Add("eerie cycloid spiral 2", 320429, false);
		this.Add("plunger plop", 320420, false);
		this.Add("sliding wooden door", 320418, false);
		this.Add("jet plane flyby", 320395, false);
		this.Add("sink draining", 320394, false);
		this.Add("eerie sounds", 320371, false);
		this.Add("eerie wobbling", 320371, false);
		this.Add("one minute remaining until launch", 320359, false);
		this.Add("monster hiss 2", 320345, false);
		this.Add("monster roar 2", 320345, false);
		this.Add("noise repeat", 320337, false);
		this.Add("eerie sound rise", 320331, false);
		this.Add("water pool splash", 320330, false);
		this.Add("mechanical clanking", 320329, false);
		this.Add("creek running water", 320289, false);
		this.Add("stack of books falling", 320285, false);
		this.Add("object falling", 320285, false);
		this.Add("female buddhist monks chanting", 320270, false);
		this.Add("i am death voice", 320259, false);
		this.Add("dropping object on glass table", 320246, false);
		this.Add("reggae guitar", 320243, false);
		this.Add("funky guitar", 320242, false);
		this.Add("neo soul guitar", 320237, false);
		this.Add("by a small fire", 320231, false);
		this.Add("paper scroll roll", 320214, false);
		this.Add("dog barking", 320211, false);
		this.Add("mystic exhalation", 320207, false);
		this.Add("aerosol spray can", 320206, false);
		this.Add("electronic music", 320203, false);
		this.Add("electronic music 2", 320203, false);
		this.Add("electronic music 3", 320203, false);
		this.Add("guitar synth", 320197, false);
		this.Add("guitar synth 2", 320197, false);
		this.Add("electronic music 4", 320189, false);
		this.Add("electronic music 5", 320189, false);
		this.Add("electronic sounds", 320186, false);
		this.Add("short plink", 320181, false);
		this.Add("horror whoosh", 320180, false);
		this.Add("pencil writing", 320154, false);
		this.Add("pencil erasing scratch", 320154, false);
		this.Add("paper book page flipping", 320149, false);
		this.Add("blanket cloth movement", 320144, false);
		this.Add("attention seeking whistle 2", 320140, false);
		this.Add("drink sip and swallow", 320139, false);
		this.Add("folk guitar", 320117, false);
		this.Add("motor starting", 320096, false);
		this.Add("dance track", 320089, false);
		this.Add("horror falling gate", 320072, false);
		this.Add("horror falling gate 2", 320072, false);
		this.Add("splosh", 320041, false);
		this.Add("synth sounds", 320037, false);
		this.Add("synth sounds 2", 320037, false);
		this.Add("marching street band", 320009, false);
		this.Add("dot matrix printer", 320008, false);
		this.Add("rising sound", 320005, false);
		this.Add("creature roar", 320004, false);
		this.Add("electric drill", 320002, false);
		this.Add("short creature grunt", 320001, false);
		this.Add("fight voice", 320000, false);
		this.Add("layer air", 319999, false);
		this.Add("death", 319998, false);
		this.Add("turns of a ratchet", 319996, false);
		this.Add("cocktail shaker full of ice", 319995, false);
		this.Add("flag pole being struck", 319986, false);
		this.Add("metro background noise", 319981, false);
		this.Add("electric typewriter return", 319945, false);
		this.Add("iron clang", 319939, false);
		this.Add("sizzling", 319939, false);
		this.Add("funny ghost scare", 199522, false);
		this.Add("strange noise", 175897, false);
		this.Add("laughing baby", 207327, false);
		this.Add("laughing baby 2", 207327, false);
		this.Add("laughing baby 3", 207327, false);
		this.Add("laughing baby 4", 207327, false);
		this.Add("robotic beeps", 255883, false);
		this.Add("funny laugh", 172923, false);
		this.Add("banjo pluck", 157420, false);
		this.Add("funny loop", 157339, false);
		this.Add("funny alien", 316154, false);
		this.Add("evil why hello there voice", 263967, false);
		this.Add("cosmic rays", 93015, false);
		this.Add("bird scream echo", 77997, false);
		this.Add("funny helium voice", 84725, false);
		this.Add("laughter", 109759, false);
		this.Add("cartoony sound 2", 273985, false);
		this.Add("funny scream", 172401, false);
		this.Add("funny synth", 41021, false);
		this.Add("scratch loop", 68315, false);
		this.Add("distorted voice", 252001, false);
		this.Add("that is so funny voice", 277596, false);
		this.Add("ponging", 149058, false);
		this.Add("strange distorted noise", 39437, false);
		this.Add("group applause", 166054, false);
		this.Add("odd synth music", 68212, false);
		this.Add("distant thump", 237558, false);
		this.Add("distant thump 2", 237558, false);
		this.Add("sigh", 242907, false);
		this.Add("coin", 170147, false);
		this.Add("laser", 170168, false);
		this.Add("jump", 170164, false);
		this.Add("hurt", 170148, false);
		this.Add("alarm clock analog", 198841, false);
		this.Add("alarm clock digital", 219244, false);
		this.Add("dice throw", 250714, false);
		this.Add("fireworks", 212783, false);
		this.Add("fireworks 2", 56579, false);
		this.Add("firework rocket 2", 192657, false);
		this.Add("happy birthday", 150966, false);
		this.Add("happy birthday 2", 17643, false);
		this.Add("happy birthday 3", 17643, false);
		this.Add("happy birthday 4", 23270, false);
		this.Add("happy birthday 5", 245781, false);
		this.Add("happy new year", 87048, false);
		this.Add("short metal tick", 160052, false);
		this.Add("ray gun", 330394, false);
		this.Add("bird flap", 146328, false);
		this.Add("win buzz", 220184, false);
		this.Add("store scanner beep", 144418, false);
		this.Add("gun cock", 182229, false);
		this.Add("gun cock 2", 177054, false);
		this.Add("gun shot", 337696, false);
		this.Add("laser gun cannon shot", 194312, false);
		this.Add("silenced gun shot", 163584, false);
		this.Add("gun shot 2", 75347, false);
		this.Add("gun shot 3", 150137, false);
		this.Add("bullet impact", 150839, false);
		this.Add("gun shot 4", 209554, false);
		this.Add("fireworks explosion", 157801, false);
		this.Add("shot 2", 33276, false);
		this.Add("bullet shells", 159006, false);
		this.Add("digital effect", 244356, false);
		this.Add("gui click beep", 264762, false);
		this.Add("gui click beep 2", 264763, false);
		this.Add("sci fi button beep", 196979, false);
		this.Add("switch", 51167, false);
		this.Add("button click 5", 161544, false);
		this.Add("enter digital beep", 141327, false);
		this.Add("short beep", 244450, false);
		this.Add("switch off", 218108, false);
		this.Add("blip beep", 350864, false);
		this.Add("short beep 2", 351210, false);
		this.Add("ping", 215416, false);
		this.Add("power on", 70107, false);
		this.Add("digital effect 2", 83287, false);
		this.Add("digital effect 3", 8680, false);
		this.Add("error", 351500, false);
		this.Add("error 2", 327736, false);
		this.Add("error 3", 344687, false);
		this.Add("error 4", 136756, false);
		this.Add("cue scratch", 160907, false);
		this.Add("error 5", 160907, false);
		this.Add("error voice", 72964, false);
		this.Add("error 6", 139037, false);
		this.Add("splat hit", 316552, false);
		this.Add("splat hit 2", 360942, false);
		this.Add("splat vocal", 47976, false);
		this.Add("splat crunch hit", 176549, false);
		this.Add("balloon pop", 321670, false);
		this.Add("menu click", 205545, false);
		this.Add("red alert", 178032, false);
		this.Add("metal ding", 145805, false);
		this.Add("fireworks rocket", 347163, false);
		this.Add("toy cannon shot", 186950, false);
		this.Add("heavy explosion", 172870, false);
		this.Add("gun shot 5", 204009, false);
		this.Add("firework crack", 142017, false);
		this.Add("digital hit hurt", 344301, false);
		this.Add("digital explosion", 164855, false);
		this.Add("robot start voice", 196889, false);
		this.Add("digital welcome voice", 221148, false);
		this.Add("futuristic motor start", 327566, false);
		this.Add("space engine starts", 274184, false);
		this.Add("start bell ding", 333693, false);
		this.Add("glass ding", 66876, false);
		this.Add("goblet ding sparkle", 66136, false);
		this.Add("slurping drink gulp", 348648, false);
		this.Add("swallow", 213052, false);
		this.Add("bite", 80551, false);
		this.Add("bite 2", 80550, false);
		this.Add("apple bite", 275015, false);
		this.Add("snack bite", 186547, false);
		this.Add("carrot bite", 106392, false);
		this.Add("mouth popping", 245646, false);
		this.Add("pop", 320549, false);
		this.Add("pop 2", 362201, false);
		this.Add("finger snapping 2", 361768, false);
		this.Add("finger snapping 3", 235277, false);
		this.Add("hello voice", 361927, false);
		this.Add("hello dude voice", 154599, false);
		this.Add("bye dude voice", 154550, false);
		this.Add("bye bye voice", 323361, false);
		this.Add("bye voice", 343893, false);
		this.Add("computer access granted voice", 215004, false);
		this.Add("e guitar riff", 232924, false);
		this.Add("shaky losing", 364929, false);
		this.Add("energy effect", 364935, false);
		this.Add("boiling water", 365022, false);
		this.Add("glitched crying", 365017, false);
		this.Add("glitched crying 2", 365017, false);
		this.Add("glitched crying 3", 365017, false);
		this.Add("frying in a pan", 364906, false);
		this.Add("stone bouncing on stone", 364711, false);
		this.Add("glass bottle smashing", 344271, false);
		this.Add("glass smashing", 221528, false);
		this.Add("glass breaks", 84704, false);
		this.Add("water splash 2", 364700, false);
		this.Add("yawn", 364695, false);
		this.Add("unlock deadbolt", 364683, false);
		this.Add("door rattling", 364682, false);
		this.Add("door hammering knocking", 17012, false);
		this.Add("banging closed door knocking", 362633, false);
		this.Add("zombie monster", 163439, false);
		this.Add("zombie monster 2", 315844, false);
		this.Add("zombie monster 3", 193759, false);
		this.Add("zombie monster 4", 144003, false);
		this.Add("breaking kicking in door", 160213, false);
		this.Add("digital effect 4", 198969, false);
		this.Add("energy field", 209857, false);
		this.Add("pencil scribbling on paper", 211247, false);
		this.Add("jaw harp ploing", 95594, false);
		this.Add("bounce 2", 19347, false);
		this.Add("bounce boing springboard", 89542, false);
		this.Add("bounce boing springboard 2", 119793, false);
		this.Add("bounce boing springboard 3", 119794, false);
		this.Add("bounce boing springboard 4", 334033, false);
		this.Add("deodorant aerosol spray", 267709, false);
		this.Add("lightning crash", 238302, false);
		this.Add("lightning crash 2", 213015, false);
		this.Add("lightsword hit 1", 78678, false);
		this.Add("lightsword hit 2", 78678, false);
		this.Add("lightsword hit 3", 78678, false);
		this.Add("lightsword hit 4", 78678, false);
		this.Add("lightsword hit 5", 78678, false);
		this.Add("lightsword on", 78678, false);
		this.Add("lightsword off", 78678, false);
		this.Add("lightsword strike 1", 78678, false);
		this.Add("lightsword strike 2", 78678, false);
		this.Add("lightsword strike 3", 78678, false);
		this.Add("lightsword swing 1", 78678, false);
		this.Add("lightsword swing 2", 78678, false);
		this.Add("lightsword swing 3", 78678, false);
		this.Add("lightsword swing 4", 78678, false);
		this.Add("lightsword swing 5", 78678, false);
		this.Add("lightsword swing 6", 78678, false);
		this.Add("lightsword swing 7", 78678, false);
		this.Add("lightsword swing 8", 78678, false);
		this.Add("machine 1", 194994, false);
		this.Add("machine 2", 194994, false);
		this.Add("machine 3", 194994, false);
		this.Add("machine 4", 194994, false);
		this.Add("machine 5", 194994, false);
		this.Add("machine 6", 194994, false);
		this.Add("machine 7", 194994, false);
		this.Add("machine 8", 194994, false);
		this.Add("machine 9", 194994, false);
		this.Add("machine 10", 194994, false);
		this.Add("machine 11", 194994, false);
		this.Add("machine 12", 368260, false);
		this.Add("machine 13", 368260, false);
		this.Add("machine 14", 368260, false);
		this.Add("machine 15", 368260, false);
		this.Add("machine 16", 368260, false);
		this.Add("machine 17", 368260, false);
		this.Add("machine 18", 368260, false);
		this.Add("machine 19", 170633, false);
		this.Add("machine 20", 20327, false);
		this.Add("machine 21", 342724, false);
		this.Add("clunks", 196146, false);
		this.Add("metallic impact", 94130, false);
		this.Add("clunk 1", 89575, false);
		this.Add("clunk 2", 89575, false);
		this.Add("clunks 2", 89575, false);
		this.Add("clunks 3", 89575, false);
		this.Add("clunks 4", 89575, false);
		this.Add("metal clunk", 57785, false);
		this.Add("metal cling", 260208, false);
		this.Add("machine 22", 260208, false);
		this.Add("machine 23", 179350, false);
		this.Add("steam machine", 179350, false);
		this.Add("machine 24", 333999, false);
		this.Add("machine 25", 333999, false);
		this.Add("machine clanking", 333999, false);
		this.Add("machine scratching", 333999, false);
		this.Add("machine switch button", 333999, false);
		this.Add("metal clanking", 218849, false);
		this.Add("chain on wood", 44096, false);
		this.Add("wood stick clanking", 143297, false);
		this.Add("machine 26", 30313, false);
		this.Add("machine 27", 30313, false);
		this.Add("machine 28", 30313, false);
		this.Add("machine clanking 2", 23991, false);
		this.Add("machine 29", 58496, false);
		this.Add("machine switch button 2", 110548, false);
		this.Add("metallic drop", 60643, false);
		this.Add("machine switch button 3", 60643, false);
		this.Add("machine squeak", 109122, false);
		this.Add("machine 31", 109122, false);
		this.Add("machine 32", 109122, false);
		this.Add("machine squeak 2", 109122, false);
		this.Add("machine 33", 76211, false);
		this.Add("machine 34", 106368, false);
		this.Add("machine 35", 106368, false);
		this.Add("machine 36", 106368, false);
		this.Add("machine switch button 4", 85204, false);
		this.Add("machine switch button 5", 96128, false);
		this.Add("wooden clanking", 170772, false);
		this.Add("umbrella opening", 352588, false);
		this.Add("machine 37", 174822, false);
		this.Add("metal clanking 2", 345080, false);
		this.Add("machine switch button click", 94133, false);
		this.Add("metal switch button clanking", 337375, false);
		this.Add("steam puff", 250703, false);
		this.Add("steam release", 234782, false);
		this.Add("hydraulics", 48664, false);
		this.Add("activating alarm voice", 169206, false);
		this.Add("evil laugh 2", 133674, false);
		this.Add("evil laugh 3", 133674, false);
		this.Add("chanting", 66583, false);
		this.Add("chanting 2", 66583, false);
		this.Add("crying voices", 26724, false);
		this.Add("zipper", 250702, false);
		this.Add("button beep", 154953, false);
		this.Add("mechanical clanking 2", 94127, false);
		this.Add("censor beep", 213795, false);
		this.Add("odd shout", 234347, false);
		this.Add("female fear scream", 319258, false);
		this.Add("female fear scream 2", 319258, false);
		this.Add("female fear scream 3", 319258, false);
		this.Add("female fear scream 4", 319258, false);
		this.Add("pig 2", 257858, false);
		this.Add("pig 3", 257858, false);
		this.Add("pig nose sniffling", 257858, false);
		this.Add("pig 4", 257858, false);
		this.Add("pig 5", 257858, false);
		this.Add("electric shock zap 3", 160421, false);
		this.Add("electric shock zap 4", 160421, false);
		this.Add("computer sounds", 4009, false);
		this.Add("charging laser", 168986, false);
		this.Add("activate laser", 321216, false);
		this.Add("deactivate laser", 321216, false);
		this.Add("plasma shot", 39023, false);
		this.Add("heavy plasma shot", 160880, false);
		this.Add("machine shutdown", 207321, false);
		this.Add("machine shutdown 2", 207321, false);
		this.Add("evil roar", 249686, false);
		this.Add("evil roar 2", 249686, false);
		this.Add("chiptune snare", 341816, false);
		this.Add("chiptune snare 2", 341816, false);
		this.Add("electronic snare drum", 341816, false);
		this.Add("electronic snare drum 2", 341816, false);
		this.Add("short metallic", 376038, false);
		this.Add("noisy perc delay", 353373, false);
		this.Add("metallic tin can", 352677, false);
		this.Add("metallic electron buzz", 250557, false);
		this.Add("percussive", 250536, false);
		this.Add("percussive 2", 250534, false);
		this.Add("bone percussion", 246422, false);
		this.Add("basic snare fill", 245259, false);
		this.Add("perc snare pop", 245258, false);
		this.Add("screech", 233535, false);
		this.Add("metallic robot perc beep", 222062, false);
		this.Add("metallic beep", 222059, false);
		this.Add("percussive 3", 222055, false);
		this.Add("percussive tom click", 221363, false);
		this.Add("harsh metallic noise", 200175, false);
		this.Add("raw metallic", 197468, false);
		this.Add("electro wobble", 133435, false);
		this.Add("electro wobble dubstep", 133434, false);
		this.Add("electro wobble dubstep 2", 133433, false);
		this.Add("electro wobble dubstep 3", 104885, false);
		this.Add("softsynth electro riff", 48456, false);
		this.Add("softsynth electro riff 2", 48456, false);
		this.Add("electro dubstep riff", 107096, false);
		this.Add("electro dubstep riff 2", 102147, false);
		this.Add("electro dubstep riff 3", 102147, false);
		this.Add("comedic boing", 345689, false);
		this.Add("funny scary souls", 199522, false);
		this.Add("funny scary souls 2", 199522, false);
		this.Add("funny scary souls 3", 199522, false);
		this.Add("funny yay voice", 323703, false);
		this.Add("funny voice stretch", 175897, false);
		this.Add("funny voice stretch 2", 175897, false);
		this.Add("funny baby laughter", 207327, false);
		this.Add("funny baby laughter 2", 207327, false);
		this.Add("funny baby laughter 3", 207327, false);
		this.Add("funny baby laughter 4", 207327, false);
		this.Add("funny baby laughter 5", 207327, false);
		this.Add("funny baby laughter 6", 207327, false);
		this.Add("funny baby laughter 7", 207327, false);
		this.Add("funny baby laughter 8", 207327, false);
		this.Add("thats funny man voice", 131416, false);
		this.Add("funny cute sound", 346179, false);
		this.Add("funny cute sound 2", 346179, false);
		this.Add("funny cute sound 3", 346179, false);
		this.Add("funny cute sound 4", 346179, false);
		this.Add("funny cute sound 5", 346179, false);
		this.Add("funny cute sound 6", 346179, false);
		this.Add("funny cute sound 7", 345875, false);
		this.Add("funny cute sound 8", 345875, false);
		this.Add("funny cute sound 9", 345875, false);
		this.Add("funny cute sound 10", 345875, false);
		this.Add("funny cute sound 11", 345875, false);
		this.Add("funny cute sound 12", 345875, false);
		this.Add("crowd laughter", 346039, false);
		this.Add("no voice", 345873, false);
		this.Add("cute magic sound", 368323, false);
		this.Add("cute magic sound 2", 368323, false);
		this.Add("cute magic sound 3", 368323, false);
		this.Add("cute magic sound 4", 368323, false);
		this.Add("cute magic sound 5", 368323, false);
		this.Add("cute magic sound 6", 368323, false);
		this.Add("cute magic sound 7", 368323, false);
		this.Add("cute magic sound 8", 368323, false);
		this.Add("cute magic sound 9", 368323, false);
		this.Add("cute magic sound 10", 368323, false);
		this.Add("cute magic sound 11", 368323, false);
		this.Add("cute magic sound 12", 368323, false);
		this.Add("cute magic sound 13", 368323, false);
		this.Add("cute magic sound 14", 368323, false);
		this.Add("increasing speed audio line", 345543, false);
		this.Add("sneeze 2", 179902, false);
		this.Add("goat", 161194, false);
		this.Add("funny laughter", 343994, false);
		this.Add("upwards banjo pluck", 157420, false);
		this.Add("vocal robotic", 348643, false);
		this.Add("funny metallic hit", 217554, false);
		this.Add("weird wavy sound", 93015, false);
		this.Add("female laughter", 152852, false);
		this.Add("female laughter 2", 152852, false);
		this.Add("female laughter 3", 152852, false);
		this.Add("female laughter 4", 152852, false);
		this.Add("funny singing", 151180, false);
		this.Add("funny singing 2", 151180, false);
		this.Add("funny singing 3", 151180, false);
		this.Add("robot death", 188047, false);
		this.Add("cartoony wheel whirl", 273985, false);
		this.Add("vhs tracking chord", 202711, false);
		this.Add("funny squeaktoy shot", 198867, false);
		this.Add("buzzer", 17468, false);
		this.Add("heavy pages turning", 82427, false);
		this.Add("short alarm ringing", 216311, false);
		this.Add("mouse click", 180968, false);
		this.Add("chime", 62984, false);
		this.Add("fast mechanical tapping", 389868, false);
		this.Add("fast mechanical tapping long", 389868, false);
		this.Add("charge up", 97748, false);
		this.Add("chime 2", 258709, false);
		this.Add("chime 3", 258709, false);
		this.Add("glass tap", 38009, false);
		this.Add("glass tap long", 38007, false);
		this.Add("faucet water", 261458, false);
		this.Add("wobbly mute synth", 205851, false);
		this.Add("door handle", 251575, false);
		this.Add("door handle 2", 251575, false);
		this.Add("door handle 3", 251575, false);
		this.Add("door handle 4", 251575, false);
		this.Add("gun sizzle cooldown", 266102, false);
		this.Add("firework pop", 336008, false);
		this.Add("sharp explosion pop", 336006, false);
		this.Add("sharp explosion pop 2", 336005, false);
		this.Add("fun explosion", 347310, false);
		this.Add("laser fire", 234083, false);
		this.Add("fire starter poof", 186374, false);
		this.Add("pneumatic drilling", 386291, false);
		this.Add("fan", 389750, false);
		this.Add("heavy charge up", 321217, false);
		this.Add("distant explosion", 108640, false);
		this.Add("wild piano", 115262, false);
		this.Add("wild piano 2", 115262, false);
		this.Add("techno beeps", 151548, false);
		this.Add("scary_ki_ki_ki_ma_ma_ma", 396268, false);
		this.Add("violin suspense", 374618, false);
		this.Add("death sound 8 bit", 417486, false);
		this.Add("slow firing heavy weapon", 417148, false);
		this.Add("computer be beep", 417141, false);
		this.Add("soft echo sweep", 417560, false);
		this.Add("short beep 3", 417319, false);
		this.Add("beep wobble", 417318, false);
		this.Add("turkey gobble", 417327, false);
		this.Add("turkey gobble 2", 417327, false);
		this.Add("synth hum", 417309, false);
		this.Add("synth hum 2", 417309, false);
		this.Add("synth hum 3", 417309, false);
		this.Add("small crack and shatter", 417297, false);
		this.Add("sharp crack and shatter", 417296, false);
		this.Add("metal crank cutter", 417224, false);
		this.Add("air pump short", 417217, false);
		this.Add("techno bass wave", 352245, false);
		this.Add("water splash bucket emptying", 417545, false);
		this.Add("laser gun shot", 417601, false);
		this.Add("plastic container bounce", 417232, false);
		this.Add("finger snap with reverb", 417515, false);
		this.Add("glitch shot", 273046, false);
		this.Add("circuit machine", 273035, false);
		this.Add("blaster rifle shot", 330293, false);
		this.Add("blaster rifle shot 2", 330293, false);
		this.Add("space ship landing", 407234, false);
		this.Add("sci fi small vehicle passing", 327413, false);
		this.Add("passing whirl shot", 361645, false);
		this.Add("droid blips", 103525, false);
		this.Add("droid blips 2", 103525, false);
		this.Add("droid blips 3", 103525, false);
		this.Add("droid blips 4", 103525, false);
		this.Add("droid blips 5", 103525, false);
		this.Add("droid blips 6", 103525, false);
		this.Add("droid blips 7", 103525, false);
		this.Add("droid blips 8", 103525, false);
		this.Add("droid blips 9", 103525, false);
		this.Add("droid blips 10", 103525, false);
		this.Add("droid blips 11", 103525, false);
		this.Add("droid blips 12", 103525, false);
		this.Add("droid blips 13", 103525, false);
		this.Add("droid blips 14", 103525, false);
		this.Add("droid blips 15", 103525, false);
		this.Add("droid blips 16", 103525, false);
		this.Add("droid blips 17", 103525, false);
		this.Add("distortion warp", 386992, false);
		this.Add("scary creaking knocking wood", 175205, false);
		this.Add("scary creaking knocking wood 2", 175205, false);
		this.Add("scary creaking knocking wood 3", 175205, false);
		this.Add("scary creaking knocking wood 4", 175205, false);
		this.Add("scary creaking knocking wood 5", 175205, false);
		this.Add("scary creaking knocking wood 6", 175205, false);
		this.Add("long horror shrill scream", 171078, false);
		this.Add("human quack", 242664, false);
		this.Add("rubber duck squeak squeeze", 350917, false);
		this.Add("rubber duck squeak squeeze 2", 350917, false);
		this.Add("rubber duck squeak squeeze 3", 350917, false);
		this.Add("rubber duck squeak squeeze 4", 350917, false);
		this.Add("rubber duck squeak squeeze 5", 350917, false);
		this.Add("rubber duck squeak squeeze 6", 350917, false);
		this.Add("paper rip", 342541, false);
		this.Add("baby voice giggle", 10, false);
		this.Add("baby voice giggle 2", 10, false);
		this.Add("baby voice giggle 3", 10, false);
		this.Add("baby voice giggle 4", 10, false);
		this.Add("baby voice giggle 5", 10, false);
		this.Add("baby voice giggle 6", 10, false);
		this.Add("baby voice giggle 7", 10, false);
		this.Add("baby voice growl", 10, false);
		this.Add("balloon blow up", 10, false);
		this.Add("balloon pop short", 10, false);
		this.Add("balloon rub", 10, false);
		this.Add("bone neck break", 10, false);
		this.Add("bone neck break 2", 10, false);
		this.Add("break snap crack", 10, false);
		this.Add("bricks fall collision", 10, false);
		this.Add("bricks friction", 10, false);
		this.Add("bright appear", 10, false);
		this.Add("brushing asphalt", 10, false);
		this.Add("bullet energy impact", 10, false);
		this.Add("bullet metal impact", 10, false);
		this.Add("button click power socket", 10, false);
		this.Add("button spring press", 10, false);
		this.Add("camera shutter close", 10, false);
		this.Add("cardboard box open", 10, false);
		this.Add("cardboard knife cut", 10, false);
		this.Add("chain rattle", 10, false);
		this.Add("coins rattle", 10, false);
		this.Add("compressed air burst", 10, false);
		this.Add("computer sci fi processing", 10, false);
		this.Add("cow moo", 10, false);
		this.Add("creature squeal", 10, false);
		this.Add("creature squeal 2", 10, false);
		this.Add("creature squeal 3", 10, false);
		this.Add("deep smooth click", 10, false);
		this.Add("digital alarm", 10, false);
		this.Add("digital alarm 2", 10, false);
		this.Add("digital alarm 3", 10, false);
		this.Add("drinking can open", 10, false);
		this.Add("duck quack", 10, false);
		this.Add("elevator movement noise", 10, false);
		this.Add("female hurt short pain", 10, false);
		this.Add("female voice begin", 10, false);
		this.Add("female voice complete", 10, false);
		this.Add("female voice cough", 10, false);
		this.Add("female voice counting 0", 10, false);
		this.Add("female voice counting 1", 10, false);
		this.Add("female voice counting 10", 10, false);
		this.Add("female voice counting 2", 10, false);
		this.Add("female voice counting 3", 10, false);
		this.Add("female voice counting 4", 10, false);
		this.Add("female voice counting 5", 10, false);
		this.Add("female voice counting 6", 10, false);
		this.Add("female voice counting 7", 10, false);
		this.Add("female voice counting 8", 10, false);
		this.Add("female voice counting 9", 10, false);
		this.Add("fireworks rocket launch short", 10, false);
		this.Add("flute whistle down", 10, false);
		this.Add("flute whistle wobble", 10, false);
		this.Add("flyby missile rocket", 10, false);
		this.Add("gatling gun", 10, false);
		this.Add("gatling gun 2", 10, false);
		this.Add("generic impact", 10, false);
		this.Add("glass fragment", 10, false);
		this.Add("glass shattering", 10, false);
		this.Add("horn inside car", 10, false);
		this.Add("ice skating", 10, false);
		this.Add("insect cricket", 10, false);
		this.Add("insect crickets", 10, false);
		this.Add("kid ouch pain", 10, false);
		this.Add("kid voice excellent", 10, false);
		this.Add("kid voice game over", 10, false);
		this.Add("kid voice go", 10, false);
		this.Add("kid voice i win", 10, false);
		this.Add("kid voice lets play", 10, false);
		this.Add("kid voice oh no", 10, false);
		this.Add("kid voice thank you", 10, false);
		this.Add("kid voice too bad", 10, false);
		this.Add("kid voice try again", 10, false);
		this.Add("kid voice whow", 10, false);
		this.Add("kid voice you win", 10, false);
		this.Add("magic spell flame", 10, false);
		this.Add("male hooray voice scream", 10, false);
		this.Add("male hurt long pain", 10, false);
		this.Add("male radio voice counting 0", 10, false);
		this.Add("male radio voice counting 1", 10, false);
		this.Add("male radio voice counting 10", 10, false);
		this.Add("male radio voice counting 2", 10, false);
		this.Add("male radio voice counting 3", 10, false);
		this.Add("male radio voice counting 4", 10, false);
		this.Add("male radio voice counting 5", 10, false);
		this.Add("male radio voice counting 6", 10, false);
		this.Add("male radio voice counting 7", 10, false);
		this.Add("male radio voice counting 8", 10, false);
		this.Add("male radio voice counting 9", 10, false);
		this.Add("male radio voice fire", 10, false);
		this.Add("male radio voice follow me", 10, false);
		this.Add("male radio voice hello", 10, false);
		this.Add("male radio voice no", 10, false);
		this.Add("male radio voice ok", 10, false);
		this.Add("male radio voice orders received", 10, false);
		this.Add("male radio voice success", 10, false);
		this.Add("male radio voice unacceptable", 10, false);
		this.Add("male radio voice yes", 10, false);
		this.Add("male robot voice counting 0", 10, false);
		this.Add("male robot voice counting 1", 10, false);
		this.Add("male robot voice counting 10", 10, false);
		this.Add("male robot voice counting 2", 10, false);
		this.Add("male robot voice counting 3", 10, false);
		this.Add("male robot voice counting 4", 10, false);
		this.Add("male robot voice counting 5", 10, false);
		this.Add("male robot voice counting 6", 10, false);
		this.Add("male robot voice counting 7", 10, false);
		this.Add("male robot voice counting 8", 10, false);
		this.Add("male robot voice counting 9", 10, false);
		this.Add("male voice affirmative", 10, false);
		this.Add("male voice ahh", 10, false);
		this.Add("male voice ahh 2", 10, false);
		this.Add("male voice alright", 10, false);
		this.Add("male voice analyzing", 10, false);
		this.Add("male voice argh", 10, false);
		this.Add("male voice as you wish", 10, false);
		this.Add("male voice backup", 10, false);
		this.Add("male voice counting 0", 10, false);
		this.Add("male voice counting 1", 10, false);
		this.Add("male voice counting 10", 10, false);
		this.Add("male voice counting 2", 10, false);
		this.Add("male voice counting 3", 10, false);
		this.Add("male voice counting 4", 10, false);
		this.Add("male voice counting 5", 10, false);
		this.Add("male voice counting 6", 10, false);
		this.Add("male voice counting 7", 10, false);
		this.Add("male voice counting 8", 10, false);
		this.Add("male voice counting 9", 10, false);
		this.Add("male voice danger", 10, false);
		this.Add("male voice detected", 10, false);
		this.Add("male voice enter password", 10, false);
		this.Add("male voice error", 10, false);
		this.Add("male voice fire", 10, false);
		this.Add("male voice game over", 10, false);
		this.Add("male voice go", 10, false);
		this.Add("male voice hey", 10, false);
		this.Add("male voice highscore", 10, false);
		this.Add("male voice howdy", 10, false);
		this.Add("male voice howdy 2", 10, false);
		this.Add("male voice incoming", 10, false);
		this.Add("male voice let move", 10, false);
		this.Add("male voice mega combo", 10, false);
		this.Add("male voice no", 10, false);
		this.Add("male voice ok", 10, false);
		this.Add("male voice orders received", 10, false);
		this.Add("male voice ouch", 10, false);
		this.Add("male voice pain ugh", 10, false);
		this.Add("male voice ready", 10, false);
		this.Add("male voice welcome", 10, false);
		this.Add("male voice whoops", 10, false);
		this.Add("male voice yes sir", 10, false);
		this.Add("male voice yes strong", 10, false);
		this.Add("male yippee voice scream", 10, false);
		this.Add("martial arts kick whoosh", 10, false);
		this.Add("martial arts shout", 10, false);
		this.Add("martial arts shout 2", 10, false);
		this.Add("martial arts shout 3", 10, false);
		this.Add("martial arts shout 4", 10, false);
		this.Add("martial arts shout 5", 10, false);
		this.Add("medieval click button select", 10, false);
		this.Add("metal bars friction", 10, false);
		this.Add("metal bars friction 2", 10, false);
		this.Add("metal fence shake", 10, false);
		this.Add("metal mechanism", 10, false);
		this.Add("monster breath", 10, false);
		this.Add("monster effort", 10, false);
		this.Add("monster hurt", 10, false);
		this.Add("mouth pop", 10, false);
		this.Add("nailgun tool", 10, false);
		this.Add("pigeon call", 10, false);
		this.Add("plasma cannon zap", 10, false);
		this.Add("plastic box friction", 10, false);
		this.Add("psycho laughter", 10, false);
		this.Add("psycho laughter 2", 10, false);
		this.Add("psycho laughter 3", 10, false);
		this.Add("psycho laughter 4", 10, false);
		this.Add("psycho laughter 5", 10, false);
		this.Add("retro 8bit beep", 10, false);
		this.Add("retro 8bit beep 2", 10, false);
		this.Add("retro 8bit coin collect", 10, false);
		this.Add("retro 8bit coin collect 2", 10, false);
		this.Add("retro 8bit explosion", 10, false);
		this.Add("retro 8bit explosion 2", 10, false);
		this.Add("retro 8bit shot", 10, false);
		this.Add("retro 8bit wobble", 10, false);
		this.Add("retro 8bit zap", 10, false);
		this.Add("robot voice danger", 10, false);
		this.Add("robot voice oh no", 10, false);
		this.Add("robot voice ok", 10, false);
		this.Add("robot voice times up", 10, false);
		this.Add("robot voice welcome", 10, false);
		this.Add("robotic servo", 10, false);
		this.Add("robotic short burst", 10, false);
		this.Add("sci fi charge", 10, false);
		this.Add("sci fi charge 2", 10, false);
		this.Add("shock rifle drop", 10, false);
		this.Add("shock rifle thunder", 10, false);
		this.Add("shock rifle tube", 10, false);
		this.Add("shock rifle zap", 10, false);
		this.Add("short cookie bite", 10, false);
		this.Add("short kid scream", 10, false);
		this.Add("short sniff", 10, false);
		this.Add("small computer beep", 10, false);
		this.Add("soft foof explosion", 10, false);
		this.Add("spade scrape", 10, false);
		this.Add("steam train whistle short", 10, false);
		this.Add("sword blade blunt hit", 10, false);
		this.Add("sword blade chop", 10, false);
		this.Add("table tennis ball hit", 10, false);
		this.Add("tank hatch close", 10, false);
		this.Add("tank turret rotate", 10, false);
		this.Add("thruster burning air", 10, false);
		this.Add("tiger growl", 10, false);
		this.Add("toy horn triple", 10, false);
		this.Add("water drop plop", 10, false);
		this.Add("whoosh air blade", 10, false);
		this.Add("whoosh air blade 2", 10, false);
		this.Add("whoosh air blade 3", 10, false);
		this.Add("whoosh air blade 4", 10, false);
		this.Add("whoosh air blade 5", 10, false);
		this.Add("whoosh air blade 6", 10, false);
		this.Add("whoosh miss", 10, false);
		this.Add("wood creak short", 10, false);
		this.Add("wood creak short 2", 10, false);
		this.Add("wood cupboard close", 10, false);
		this.Add("yehey voice scream", 10, false);
		this.Add("zap bright disappear", 10, false);
		this.Add("alarm distorted", 10, false);
		this.Add("alarm distorted short", 10, false);
		this.Add("alarm pingpong", 10, false);
		this.Add("apple bite 2", 10, false);
		this.Add("apple bite 3", 10, false);
		this.Add("balloon release air", 10, false);
		this.Add("balloon release air 2", 10, false);
		this.Add("balloon squeal", 10, false);
		this.Add("bin crumbled paper", 10, false);
		this.Add("bin crumbled paper 2", 10, false);
		this.Add("book turn page", 10, false);
		this.Add("book turn page 2", 10, false);
		this.Add("book turn page 3", 10, false);
		this.Add("book turn page 4", 10, false);
		this.Add("bright crisp crack", 10, false);
		this.Add("brush glass fragments", 10, false);
		this.Add("cardboard drop pieces", 10, false);
		this.Add("cardboard open box", 10, false);
		this.Add("cartoon boing", 10, false);
		this.Add("dark short thud", 10, false);
		this.Add("deep distant explosion", 10, false);
		this.Add("deep distant explosion 2", 10, false);
		this.Add("drink sip", 10, false);
		this.Add("eat swallow", 10, false);
		this.Add("elevator door open", 10, false);
		this.Add("elevator stop", 10, false);
		this.Add("explosion kickback crackle", 10, false);
		this.Add("fridge door close", 10, false);
		this.Add("fridge door open", 10, false);
		this.Add("gaffer tape pull", 10, false);
		this.Add("gaffer tape pull 2", 10, false);
		this.Add("gaffer tape pull 3", 10, false);
		this.Add("gaffer tape remove", 10, false);
		this.Add("gaffer tape tear", 10, false);
		this.Add("glass tap short", 10, false);
		this.Add("glove compartment click", 10, false);
		this.Add("goose", 10, false);
		this.Add("hammer nail", 10, false);
		this.Add("hollow break", 10, false);
		this.Add("hollow wood creak", 10, false);
		this.Add("horse snort", 10, false);
		this.Add("horse snort 2", 10, false);
		this.Add("horse snort 3", 10, false);
		this.Add("indoor wood door close", 10, false);
		this.Add("indoor wood door open", 10, false);
		this.Add("light button fluorescent", 10, false);
		this.Add("light match", 10, false);
		this.Add("locked metal door open", 10, false);
		this.Add("long sniff", 10, false);
		this.Add("metal bin knock fall", 10, false);
		this.Add("metal door close bang", 10, false);
		this.Add("metal door open cling", 10, false);
		this.Add("metal fence shake subtle", 10, false);
		this.Add("metal hinge squeak", 10, false);
		this.Add("metal hinge squeak 2", 10, false);
		this.Add("metal hinge squeak 3", 10, false);
		this.Add("metal key padlock", 10, false);
		this.Add("metal key padlock 2", 10, false);
		this.Add("metal mechanism 2", 10, false);
		this.Add("metal mechanism 3", 10, false);
		this.Add("metal mechanism 4", 10, false);
		this.Add("metal mechanism 5", 10, false);
		this.Add("metal mechanism 6", 10, false);
		this.Add("metal mechanism 7", 10, false);
		this.Add("metal mechanism 8", 10, false);
		this.Add("microwave door close", 10, false);
		this.Add("microwave door open", 10, false);
		this.Add("multi chew", 10, false);
		this.Add("multi chew 2", 10, false);
		this.Add("paper crumble", 10, false);
		this.Add("paper shake", 10, false);
		this.Add("paper tear fast", 10, false);
		this.Add("pen on paper", 10, false);
		this.Add("pen on paper 2", 10, false);
		this.Add("pen on paper 3", 10, false);
		this.Add("pen on paper 4", 10, false);
		this.Add("pour liquid in glass", 10, false);
		this.Add("quick fabric tear", 10, false);
		this.Add("servo charge down", 10, false);
		this.Add("servo charge up", 10, false);
		this.Add("short cough", 10, false);
		this.Add("short metal sliding", 10, false);
		this.Add("short muffled explosion", 10, false);
		this.Add("short muffled exploson 2", 10, false);
		this.Add("short spray", 10, false);
		this.Add("small whip", 10, false);
		this.Add("smooth thud", 10, false);
		this.Add("solid energy impact", 10, false);
		this.Add("solid energy impact 2", 10, false);
		this.Add("spray can medium", 10, false);
		this.Add("spray can medium 2", 10, false);
		this.Add("spray can shake subtle", 10, false);
		this.Add("squeaky bright toy", 10, false);
		this.Add("squishy break", 10, false);
		this.Add("stab bubbles", 10, false);
		this.Add("stab bubbles 2", 10, false);
		this.Add("stab bubbles 3", 10, false);
		this.Add("stick on fence", 10, false);
		this.Add("subtle tap", 10, false);
		this.Add("success banjo", 10, false);
		this.Add("success guitar", 10, false);
		this.Add("success guitar 2", 10, false);
		this.Add("success piano", 10, false);
		this.Add("success piano 2", 10, false);
		this.Add("success xylophone", 10, false);
		this.Add("success xylophone 2", 10, false);
		this.Add("toaster eject", 10, false);
		this.Add("tree creak", 10, false);
		this.Add("whoosh steam wobble", 10, false);
		this.Add("zipper short", 10, false);
		this.Add("zipper short 2", 10, false);
		this.Add("zipper short 3", 10, false);
		this.Add("behind you voice raspy", 12, false);
		this.Add("behind you voice", 12, false);
		this.Add("come in voice raspy", 12, false);
		this.Add("goodbye voice", 12, false);
		this.Add("hello voice raspy", 12, false);
		this.Add("hello there", 12, false);
		this.Add("help me voice raspy", 12, false);
		this.Add("help me help me begging voice", 12, false);
		this.Add("mad scientist laughter", 12, false);
		this.Add("mad scientist laughter 2", 12, false);
		this.Add("mad scientist laughter 3", 12, false);
		this.Add("mumbling voice raspy", 12, false);
		this.Add("mumbling voice", 12, false);
		this.Add("no voice plain", 12, false);
		this.Add("spooky whispering", 12, false);
		this.Add("spooky whispering 2", 12, false);
		this.Add("spooky whispering 3", 12, false);
		this.Add("tread lightly voice", 12, false);
		this.Add("welcome voice raspy", 12, false);
		this.Add("welcome voice", 12, false);
		this.Add("rat squeak", 13, false);
		this.Add("squirrel run by fast", 13, false);
		this.Add("church bell resonance", 13, false);
		this.Add("chainsaw eletric short", 13, false);
		this.Add("chainsaw eletric long", 13, false);
		this.Add("coffin lid movement", 13, false);
		this.Add("ghostly pass by", 13, false);
		this.Add("ghostly pass by 2", 13, false);
		this.Add("ghostly pass by 3", 13, false);
		this.Add("horror exhale", 13, false);
		this.Add("horror piano dark", 13, false);
		this.Add("monster growl 2", 13, false);
		this.Add("monster growl 3", 13, false);
		this.Add("piano bow broken string horror", 13, false);
		this.Add("heartbeat", 13, false);
		this.Add("static interference noise", 13, false);
		this.Add("radio static", 13, false);
		this.Add("radio static then tuning", 13, false);
		this.Add("sliding door", 13, false);
		this.Add("slamming metal door", 13, false);
		this.Add("knocking fast", 13, false);
		this.Add("squeaky horror door", 13, false);
		this.Add("squeaky horror door 2", 13, false);
		this.Add("squeaky horror door 3", 13, false);
		this.Add("metal lock wobble", 13, false);
		this.Add("bell telephone ring", 13, false);
		this.Add("heavy wind", 341368, false);
		this.Add("horror violin long", 370936, false);
		this.Add("horror waterphone", 315652, false);
		this.Add("locked door handle", 12, false);
		this.Add("raven call", 328846, false);
		this.Add("tenor voice ahh", 395421, false);
		this.Add("throat singing", 12, false);
		this.Add("throat singing deep", 12, false);
		this.Add("thunder rumble", 527664, false);
		this.Add("vicious dog bark", 479637, false);
	}

	// Token: 0x04001191 RID: 4497
	public static float defaultPitch = 1f;

	// Token: 0x04001192 RID: 4498
	public static float minPitch = 0.1f;

	// Token: 0x04001193 RID: 4499
	public static float maxPitch = 3f;

	// Token: 0x04001194 RID: 4500
	public bool Enabled = true;

	// Token: 0x04001195 RID: 4501
	public List<string> names = new List<string>();

	// Token: 0x04001196 RID: 4502
	public List<string> loopNames = new List<string>();

	// Token: 0x04001197 RID: 4503
	public int loopSoundsStartedCount;

	// Token: 0x04001198 RID: 4504
	public const int maxLoopSoundsAtSameTime = 5;

	// Token: 0x04001199 RID: 4505
	public bool didPlaySoundThisUpdate;

	// Token: 0x0400119A RID: 4506
	private Dictionary<string, AudioClip> cachedClips = new Dictionary<string, AudioClip>();

	// Token: 0x0400119B RID: 4507
	private const int maxCachedClips = 75;

	// Token: 0x0400119C RID: 4508
	private Dictionary<string, bool> clipsPlayedThisUpdate = new Dictionary<string, bool>();

	// Token: 0x0400119D RID: 4509
	private const float secondsToAvoidSameSoundPlaying = 0.05f;

	// Token: 0x0400119E RID: 4510
	private float timeOfLastClipsPlayedClearing = -1f;

	// Token: 0x0400119F RID: 4511
	private string[] musicNotes = new string[]
	{
		"c", "c sharp", "d", "d sharp", "e", "f", "f sharp", "g", "g sharp", "a",
		"a sharp", "b"
	};

	// Token: 0x040011A0 RID: 4512
	private bool didSortNames;

	// Token: 0x040011A1 RID: 4513
	private Transform soundParent;

	// Token: 0x040011A2 RID: 4514
	public KeyboardDialog recordingKeyboard;

	// Token: 0x040011A3 RID: 4515
	private int idCounter;

	// Token: 0x040011A4 RID: 4516
	private Dictionary<int, string> namesById = new Dictionary<int, string>();

	// Token: 0x040011A5 RID: 4517
	private const int licenseId_universalSoundFx = 10;

	// Token: 0x040011A6 RID: 4518
	private const int licenseId_soniss = 11;

	// Token: 0x040011A7 RID: 4519
	private const int licenseId_delcoJinx = 12;

	// Token: 0x040011A8 RID: 4520
	private const int licenseId_zapsplat = 13;
}
