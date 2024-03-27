using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class SoundTrack
{
	// Token: 0x060016B4 RID: 5812 RVA: 0x000CC06C File Offset: 0x000CA46C
	public void SetByString(string data)
	{
		data = this.SetAndRemoveWithParams(data);
		string[] array = Misc.Split(data, " ", StringSplitOptions.RemoveEmptyEntries);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Sound sound = null;
		foreach (string text in array)
		{
			if (text.Contains(".") && !text.Contains("c"))
			{
				string text2 = text;
				if (text2.IndexOf("0") != 0)
				{
					text2 = "0" + text2;
				}
				float num;
				if (float.TryParse(text2, out num))
				{
					if (sound == null)
					{
						sound = new Sound();
					}
					Sound sound2 = sound;
					float? num2 = this.quantization;
					float num5;
					if (num2 != null)
					{
						float num3 = num;
						float? num4 = this.quantization;
						num5 = this.Quantize(num3, num4.Value);
					}
					else
					{
						num5 = num;
					}
					sound2.secondsToWaitAfter = num5;
					if (sound.secondsToWaitAfter >= 0f && this.speed >= 0f)
					{
						sound.secondsToWaitAfter /= this.speed;
					}
					this.sounds.Add(sound);
					sound = null;
				}
			}
			else
			{
				if (sound != null && !string.IsNullOrEmpty(sound.name))
				{
					this.sounds.Add(sound);
				}
				sound = new Sound();
				bool flag = text.IndexOf("c") == 0;
				int num6;
				if (flag)
				{
					string text3 = text.Replace("c", string.Empty);
					bool flag2 = text3.Contains("=");
					string text4;
					if (flag2)
					{
						string[] array3 = Misc.Split(text3, "=", StringSplitOptions.RemoveEmptyEntries);
						if (array3.Length == 2)
						{
							dictionary.Add(array3[0], array3[1]);
							sound.SetByStringData(array3[1]);
						}
					}
					else if (dictionary.TryGetValue(text3, out text4))
					{
						sound.SetByStringData(text4);
					}
				}
				else if (int.TryParse(text, out num6))
				{
					sound.name = Managers.soundLibraryManager.GetNameById(num6);
				}
			}
		}
		if (sound != null && !string.IsNullOrEmpty(sound.name))
		{
			this.sounds.Add(sound);
		}
		if (this.sounds.Count == 0)
		{
			this.didFinish = true;
		}
		else
		{
			this.timeWhenToPlayNext = Time.time;
			this.AssembleTellListeners();
		}
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x000CC2BC File Offset: 0x000CA6BC
	public void ContinuePlayback(Vector3 sourcePosition)
	{
		if (this.didFinish)
		{
			return;
		}
		float time = Time.time;
		int num = 0;
		while (time >= this.timeWhenToPlayNext && ++num <= 20)
		{
			Sound sound = this.sounds[this.currentIndex];
			if (sound.name != null)
			{
				if (this.surroundSound || sound.surround || this.IsInVicinity(sourcePosition))
				{
					float num2 = ((!this.surroundSound) ? 0.2f : 0.05f);
					num2 *= this.volume;
					Managers.soundLibraryManager.Play(sourcePosition, sound, false, true, this.surroundSound, num2);
				}
				this.TellListeningThingParts(sound);
			}
			this.timeWhenToPlayNext = time + sound.secondsToWaitAfter;
			if (++this.currentIndex >= this.sounds.Count)
			{
				if (this.loop)
				{
					this.currentIndex = 0;
					this.AssembleTellListeners();
				}
				else
				{
					this.didFinish = true;
				}
				break;
			}
		}
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x000CC3D4 File Offset: 0x000CA7D4
	private void TellListeningThingParts(Sound sound)
	{
		if (sound.pitch == 1f)
		{
			foreach (ThingPart thingPart in this.listeningThingParts)
			{
				if (thingPart != null)
				{
					string text = "track played " + sound.name;
					thingPart.TriggerEvent(StateListener.EventType.OnToldByAny, text, false, null);
				}
			}
		}
	}

	// Token: 0x060016B7 RID: 5815 RVA: 0x000CC464 File Offset: 0x000CA864
	private bool IsInVicinity(Vector3 sourcePosition)
	{
		if (this.ourHead == null)
		{
			this.ourHead = Managers.treeManager.GetTransform("/OurPersonRig/HeadCore");
		}
		return Vector3.Distance(this.ourHead.position, sourcePosition) <= 40f;
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x000CC4B2 File Offset: 0x000CA8B2
	private float Quantize(float value, float quantization)
	{
		if (value > 0f && quantization > 0f)
		{
			value = Mathf.Round(value / quantization) * quantization;
		}
		return value;
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x000CC4D8 File Offset: 0x000CA8D8
	private string SetAndRemoveWithParams(string data)
	{
		if (data.Contains(" with "))
		{
			string[] array = Misc.Split(data, " with ", StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				data = array[0];
				string[] array2 = Misc.Split(array[1], " ", StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					string text2 = ((i >= array2.Length - 1) ? null : array2[i + 1]);
					if (text == "loop")
					{
						this.loop = true;
					}
					else
					{
						if (text2 != null)
						{
							if (text2 == "speed")
							{
								float num;
								if (float.TryParse(text, out num))
								{
									this.speed = Mathf.Clamp(num, 0.01f, 50f);
								}
								goto IL_187;
							}
							if (text2 == "quantization")
							{
								string text3 = text.Replace("s", string.Empty);
								float num2;
								if (float.TryParse(text3, out num2))
								{
									this.quantization = new float?(Mathf.Clamp(num2, 0.001f, 100f));
								}
								goto IL_187;
							}
							if (text2 == "octave" || text2 == "octaves")
							{
								float num3;
								if (float.TryParse(text, out num3))
								{
									this.octavesChange = num3;
								}
								goto IL_187;
							}
							if (!(text2 == "volume"))
							{
							}
						}
						float num4;
						if (float.TryParse(text, out num4))
						{
							this.volume = Mathf.Clamp(num4, 0f, 1000f);
						}
					}
					IL_187:;
				}
			}
		}
		else
		{
			data = data.Replace("with", string.Empty);
		}
		data = data.Trim();
		return data;
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x000CC69C File Offset: 0x000CAA9C
	private void AssembleTellListeners()
	{
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(ThingPart), true);
		Component[] componentsInChildren2 = Managers.personManager.ourPerson.GetComponentsInChildren(typeof(ThingPart), true);
		Component[] componentsInChildren3 = Managers.personManager.People.GetComponentsInChildren(typeof(ThingPart), true);
		Component[] array = componentsInChildren2.Concat(componentsInChildren3).ToArray<Component>();
		Component[] array2 = componentsInChildren.Concat(array).ToArray<Component>();
		this.listeningThingParts = new List<ThingPart>();
		foreach (ThingPart thingPart in array2)
		{
			this.AddListeningThingPartIfNeeded(thingPart);
		}
	}

	// Token: 0x060016BB RID: 5819 RVA: 0x000CC754 File Offset: 0x000CAB54
	private void AddListeningThingPartIfNeeded(ThingPart thingPart)
	{
		foreach (ThingPartState thingPartState in thingPart.states)
		{
			foreach (StateListener stateListener in thingPartState.listeners)
			{
				if (stateListener.eventType == StateListener.EventType.OnToldByAny && stateListener.whenData.Contains("track played"))
				{
					this.listeningThingParts.Add(thingPart);
					return;
				}
			}
		}
	}

	// Token: 0x04001467 RID: 5223
	public bool surroundSound;

	// Token: 0x04001468 RID: 5224
	public const string trackPlayedTellEvent = "track played";

	// Token: 0x04001469 RID: 5225
	private List<Sound> sounds = new List<Sound>();

	// Token: 0x0400146A RID: 5226
	private int currentIndex;

	// Token: 0x0400146B RID: 5227
	private bool didFinish;

	// Token: 0x0400146C RID: 5228
	private float timeWhenToPlayNext = -1f;

	// Token: 0x0400146D RID: 5229
	private bool doSendTells;

	// Token: 0x0400146E RID: 5230
	private Transform ourHead;

	// Token: 0x0400146F RID: 5231
	private List<ThingPart> listeningThingParts;

	// Token: 0x04001470 RID: 5232
	private float volume = 1f;

	// Token: 0x04001471 RID: 5233
	private bool loop;

	// Token: 0x04001472 RID: 5234
	private float speed = 1f;

	// Token: 0x04001473 RID: 5235
	private float? quantization;

	// Token: 0x04001474 RID: 5236
	private float octavesChange;
}
