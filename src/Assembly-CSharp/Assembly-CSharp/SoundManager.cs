using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001FD RID: 509
public class SoundManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06001341 RID: 4929 RVA: 0x000AD347 File Offset: 0x000AB747
	// (set) Token: 0x06001342 RID: 4930 RVA: 0x000AD34F File Offset: 0x000AB74F
	public ManagerStatus status { get; private set; }

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06001343 RID: 4931 RVA: 0x000AD358 File Offset: 0x000AB758
	// (set) Token: 0x06001344 RID: 4932 RVA: 0x000AD360 File Offset: 0x000AB760
	public string failMessage { get; private set; }

	// Token: 0x06001345 RID: 4933 RVA: 0x000AD369 File Offset: 0x000AB769
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x000AD379 File Offset: 0x000AB779
	public void PlayVideoMusic()
	{
		if (this.videoMusic == null)
		{
			this.videoMusic = base.GetComponent<AudioSource>();
			this.videoMusic.Play();
		}
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x000AD3A4 File Offset: 0x000AB7A4
	private void Update()
	{
		if (this.status != ManagerStatus.Started)
		{
			return;
		}
		this.didPlaySoundThisUpdate = false;
		if (this.clipsPlayedThisUpdate.Count >= 1 && (this.timeOfLastClipsPlayedClearing == -1f || this.timeOfLastClipsPlayedClearing + 0.01f <= Time.time))
		{
			this.timeOfLastClipsPlayedClearing = Time.time;
			this.clipsPlayedThisUpdate = new Dictionary<string, bool>();
		}
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x000AD412 File Offset: 0x000AB812
	public void Play(string name, Transform transform = null, float volume = 1f, bool forcePlay = false, bool forEveryone = false)
	{
		if (transform == null)
		{
			transform = Misc.GetAHandOfOurs().transform;
		}
		this.Play(name, transform.position, volume, forcePlay, forEveryone);
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x000AD440 File Offset: 0x000AB840
	public void Play(string name, Vector3 position, float volume = 1f, bool forcePlay = false, bool forEveryone = false)
	{
		if ((!this.didPlaySoundThisUpdate || name == "success" || name == "no" || forcePlay) && !this.clipsPlayedThisUpdate.ContainsKey(name))
		{
			this.clipsPlayedThisUpdate.Add(name, true);
			this.didPlaySoundThisUpdate = true;
			this.LoadClipIfNeeded(name);
			AudioClip audioClip = null;
			if (this.clips.TryGetValue(name, out audioClip))
			{
				try
				{
					if (audioClip == null)
					{
						Log.Debug("Tried to play null sound clip : " + name);
					}
					else if (audioClip.loadState != AudioDataLoadState.Loaded)
					{
						Log.Debug("Tried to play sound before loaded : " + name);
					}
					else
					{
						AudioSource.PlayClipAtPoint(audioClip, position, volume);
					}
					if (forEveryone)
					{
						Managers.personManager.DoPlaySound(name, position, volume);
					}
				}
				catch (Exception ex)
				{
					Log.Debug("Could not play sound : " + name);
					Log.Error(ex);
				}
			}
		}
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x000AD550 File Offset: 0x000AB950
	private void LoadClipIfNeeded(string name)
	{
		if (!this.clips.ContainsKey(name))
		{
			if (this.validNames.Contains(name))
			{
				this.clips.Add(name, Resources.Load("Sound/" + name) as AudioClip);
			}
			else
			{
				Log.Error("Invalid sound name: " + name);
			}
		}
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x000AD5B8 File Offset: 0x000AB9B8
	private void ConfirmAllSoundFilesExist()
	{
		foreach (string text in this.validNames)
		{
			AudioClip audioClip = Resources.Load("Sound/" + text) as AudioClip;
			if (audioClip != null)
			{
				AudioSource.PlayClipAtPoint(audioClip, Vector3.zero, 0.5f);
			}
			else
			{
				Log.Debug("Couldn't load audio clip: \"Resources/Sound/" + text + "[.wav or .ogg]\"");
			}
		}
	}

	// Token: 0x040011AB RID: 4523
	private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

	// Token: 0x040011AC RID: 4524
	private string[] validNames = new string[]
	{
		"clone", "click", "delete", "paint", "pickColor", "pickUp", "putDown", "snapshot", "success", "tap",
		"teleport", "teleportLaser", "whoosh", "longWhoosh", "bounce", "no", "ping", "water_tap", "bump", "newTwitchMessage",
		"newPersonBorn", "purchaseSuccess", "seeingNewGift", "textCaretMoveWord", "textCaretMoveLetter", "textCaretMoveNone", "shuffle"
	};

	// Token: 0x040011AD RID: 4525
	private bool didPlaySoundThisUpdate;

	// Token: 0x040011AE RID: 4526
	private Dictionary<string, bool> clipsPlayedThisUpdate = new Dictionary<string, bool>();

	// Token: 0x040011AF RID: 4527
	private const float secondsToAvoidSameSoundPlaying = 0.01f;

	// Token: 0x040011B0 RID: 4528
	private float timeOfLastClipsPlayedClearing = -1f;

	// Token: 0x040011B1 RID: 4529
	private AudioSource videoMusic;
}
