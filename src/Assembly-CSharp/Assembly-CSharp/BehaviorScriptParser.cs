using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class BehaviorScriptParser
{
	// Token: 0x06001632 RID: 5682 RVA: 0x000C1F0C File Offset: 0x000C030C
	public static StateListener GetStateListenerFromScriptLine(string line, Thing parentThing, ThingPart parentThingPart)
	{
		StateListener stateListener = new StateListener();
		line = BehaviorScriptParser.NormalizeLine(line, parentThing, false);
		line = BehaviorScriptParser.EscapeCommaIfInQuotes(line);
		char[] array = new char[] { ',' };
		string[] array2 = line.Split(array, StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		for (int i = 0; i < array2.Length; i++)
		{
			string text2 = array2[i];
			text2 = text2.Replace("[comma]", ",");
			string text3 = string.Empty;
			stateListener.isForAnyState = text2.Contains("when_any_state");
			if (stateListener.isForAnyState)
			{
				parentThingPart.containsForAnyStateListeners = true;
				text2 = text2.Replace("when_any_state", "when");
			}
			if (i == 0)
			{
				string[] array3 = new string[] { " then " };
				string[] array4 = text2.Split(array3, StringSplitOptions.RemoveEmptyEntries);
				if (array4.Length == 2)
				{
					text = array4[0];
					text3 = array4[1];
					text = text.Replace("when ", string.Empty);
				}
				else if (array4.Length >= 3)
				{
					text = array4[0];
					text3 = string.Join(" then ", array4, 1, array4.Length - 1);
					text = text.Replace("when ", string.Empty);
				}
			}
			else
			{
				text3 = text2;
			}
			if (text != string.Empty && text3 != string.Empty)
			{
				int num = text.IndexOf(" and is ");
				if (num >= 0)
				{
					string[] array5 = Misc.Split(text, " and is ", StringSplitOptions.RemoveEmptyEntries);
					if (array5.Length == 2)
					{
						text = array5[0].Trim();
						stateListener.whenIsData = array5[1].Trim();
					}
				}
				char[] array6 = new char[] { ' ' };
				string[] array7 = text.Split(array6, StringSplitOptions.RemoveEmptyEntries);
				string[] array8 = text3.Split(array6, StringSplitOptions.RemoveEmptyEntries);
				if (array7.Length >= 1 && array8.Length >= 1)
				{
					string text4 = array7[0];
					switch (text4)
					{
					case "starts":
						stateListener.eventType = StateListener.EventType.OnStarts;
						break;
					case "touches":
						stateListener.eventType = StateListener.EventType.OnTouches;
						break;
					case "any_part_touches":
						stateListener.eventType = StateListener.EventType.OnAnyPartTouches;
						break;
					case "touch_ends":
						stateListener.eventType = StateListener.EventType.OnTouchEnds;
						break;
					case "triggered":
						stateListener.eventType = StateListener.EventType.OnTriggered;
						break;
					case "trigger_let_go":
						stateListener.eventType = StateListener.EventType.OnUntriggered;
						break;
					case "neared":
						stateListener.eventType = StateListener.EventType.OnNeared;
						break;
					case "hitting":
						stateListener.eventType = StateListener.EventType.OnHitting;
						break;
					case "any_part_hitting":
						stateListener.eventType = StateListener.EventType.OnAnyPartHitting;
						break;
					case "told":
						stateListener.eventType = StateListener.EventType.OnTold;
						break;
					case "told_by_nearby":
						stateListener.eventType = StateListener.EventType.OnToldByNearby;
						break;
					case "told_by_any":
						stateListener.eventType = StateListener.EventType.OnToldByAny;
						break;
					case "told_by_body":
						stateListener.eventType = StateListener.EventType.OnToldByBody;
						break;
					case "taken":
						stateListener.eventType = StateListener.EventType.OnTaken;
						break;
					case "grabbed":
						stateListener.eventType = StateListener.EventType.OnGrabbed;
						break;
					case "let_go":
						stateListener.eventType = StateListener.EventType.OnLetGo;
						break;
					case "consumed":
						stateListener.eventType = StateListener.EventType.OnConsumed;
						break;
					case "any_part_consumed":
						stateListener.eventType = StateListener.EventType.OnAnyPartConsumed;
						break;
					case "talked_to":
						stateListener.eventType = StateListener.EventType.OnTalkedTo;
						break;
					case "talked_from":
						stateListener.eventType = StateListener.EventType.OnTalkedFrom;
						break;
					case "pointed_at":
						stateListener.eventType = StateListener.EventType.OnPointedAt;
						break;
					case "any_part_pointed_at":
						stateListener.eventType = StateListener.EventType.OnAnyPartPointedAt;
						break;
					case "looked_at":
						stateListener.eventType = StateListener.EventType.OnLookedAt;
						break;
					case "any_part_looked_at":
						stateListener.eventType = StateListener.EventType.OnAnyPartLookedAt;
						break;
					case "turned_around":
						stateListener.eventType = StateListener.EventType.OnTurnedAround;
						break;
					case "shaken":
						stateListener.eventType = StateListener.EventType.OnShaken;
						break;
					case "high_speed":
						stateListener.eventType = StateListener.EventType.OnHighSpeed;
						break;
					case "gets":
						stateListener.eventType = StateListener.EventType.OnGets;
						break;
					case "walked_into":
						stateListener.eventType = StateListener.EventType.OnWalkedInto;
						break;
					case "raised":
						stateListener.eventType = StateListener.EventType.OnRaised;
						break;
					case "lowered":
						stateListener.eventType = StateListener.EventType.OnLowered;
						break;
					case "blown_at":
						stateListener.eventType = StateListener.EventType.OnBlownAt;
						break;
					case "typed":
						stateListener.eventType = StateListener.EventType.OnTyped;
						break;
					case "any_part_blown_at":
						stateListener.eventType = StateListener.EventType.OnAnyPartBlownAt;
						break;
					case "someone_in_vicinity":
						stateListener.eventType = StateListener.EventType.OnSomeoneInVicinity;
						break;
					case "someone_new_in_vicinity":
						stateListener.eventType = StateListener.EventType.OnSomeoneNewInVicinity;
						break;
					case "hears":
						stateListener.eventType = StateListener.EventType.OnHears;
						break;
					case "hears_anywhere":
						stateListener.eventType = StateListener.EventType.OnHearsAnywhere;
						break;
					case "destroyed":
						stateListener.eventType = StateListener.EventType.OnDestroyed;
						break;
					case "controlled":
						stateListener.eventType = StateListener.EventType.OnJoystickControlled;
						break;
					case "is":
						stateListener.eventType = StateListener.EventType.OnVariableChange;
						break;
					case "destroyed_restores":
						stateListener.eventType = StateListener.EventType.OnDestroyedRestored;
						break;
					case "enable_setting":
						stateListener.eventType = StateListener.EventType.OnSettingEnabled;
						break;
					case "disable_setting":
						stateListener.eventType = StateListener.EventType.OnSettingDisabled;
						break;
					}
					if (array7.Length >= 2)
					{
						stateListener.whenData = string.Join(" ", array7.Skip(1).ToArray<string>());
						if (stateListener.eventType == StateListener.EventType.OnToldByBody && stateListener.whenData.IndexOf("dialog ") >= 0)
						{
							stateListener.whenData = BehaviorScriptParser.RemoveSpacesInDialogNames(stateListener.whenData);
						}
					}
					if (stateListener.eventType != StateListener.EventType.None)
					{
						BehaviorScriptParser.AddCommand(stateListener, array8, parentThing, text3);
					}
				}
			}
		}
		return stateListener;
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x000C26E4 File Offset: 0x000C0AE4
	private static void SetListenerStateNumber(StateListener listener, string word)
	{
		if (word != null)
		{
			if (word == "current")
			{
				listener.setStateRelative = new RelativeStateTarget?(RelativeStateTarget.Current);
				return;
			}
			if (word == "previous")
			{
				listener.setStateRelative = new RelativeStateTarget?(RelativeStateTarget.Previous);
				return;
			}
			if (word == "next")
			{
				listener.setStateRelative = new RelativeStateTarget?(RelativeStateTarget.Next);
				return;
			}
		}
		int num;
		if (int.TryParse(word, out num))
		{
			listener.setState = num - 1;
		}
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x000C277C File Offset: 0x000C0B7C
	private static TweenType GetTweenType(string command)
	{
		TweenType tweenType = TweenType.EaseInOut;
		if (command != null)
		{
			if (!(command == "become_untweened"))
			{
				if (!(command == "become_unsoftened"))
				{
					if (!(command == "become_soft_start"))
					{
						if (command == "become_soft_end")
						{
							tweenType = TweenType.EaseOut;
						}
					}
					else
					{
						tweenType = TweenType.EaseIn;
					}
				}
				else
				{
					tweenType = TweenType.Steady;
				}
			}
			else
			{
				tweenType = TweenType.Direct;
			}
		}
		return tweenType;
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x000C27F4 File Offset: 0x000C0BF4
	private static void AddCommand(StateListener listener, string[] words, Thing parentThing, string thenPart)
	{
		string text = string.Empty;
		char[] array = new char[] { ' ' };
		if (words.Length >= 2)
		{
			text = string.Join(" ", words.Skip(1).ToArray<string>());
		}
		string text2 = words[0];
		switch (text2)
		{
		case "become":
		case "become_untweened":
		case "become_unsoftened":
		case "become_soft_start":
		case "become_soft_end":
		{
			listener.tweenType = BehaviorScriptParser.GetTweenType(words[0]);
			float num2 = ((listener.tweenType != TweenType.Direct) ? 0.05f : 0f);
			if (thenPart.Contains(" via "))
			{
				string[] array2 = Misc.Split(thenPart, " via ", StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length == 2)
				{
					words = array2[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					int num3;
					if (int.TryParse(array2[1], out num3) && num3 >= 1 && num3 <= 51)
					{
						listener.curveViaState = num3 - 1;
					}
				}
			}
			if (words.Length == 4 && words[2] == "in")
			{
				BehaviorScriptParser.SetListenerStateNumber(listener, words[1]);
				words[3] = words[3].Replace("s", string.Empty);
				float num4;
				if (float.TryParse(words[3], out num4))
				{
					listener.setStateSeconds = float.Parse(words[3]);
				}
			}
			else if (words.Length == 2)
			{
				BehaviorScriptParser.SetListenerStateNumber(listener, words[1]);
				listener.setStateSeconds = num2;
			}
			listener.setStateSeconds = Misc.ClampMin(listener.setStateSeconds, num2);
			break;
		}
		case "emit":
		case "emit_gravity_free":
			thenPart = BehaviorScriptParser.ReplaceIncludedNamesWithIds(parentThing.includedNameIds, thenPart);
			words = thenPart.Split(array, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length == 4 && words[2] == "with")
			{
				listener.emitId = words[1];
				words[3] = words[3].Replace("%", string.Empty);
				float num5;
				if (float.TryParse(words[3], out num5))
				{
					listener.emitVelocityPercent = Mathf.Clamp(num5, 0f, 100f);
				}
			}
			else if (words.Length == 2)
			{
				listener.emitId = words[1];
				listener.emitVelocityPercent = 100f;
			}
			listener.setStateSeconds = Misc.ClampMin(listener.setStateSeconds, 0.01f);
			listener.emitIsGravityFree = words[0] == "emit_gravity_free";
			break;
		case "propel_forward":
		{
			float num6;
			if (words.Length == 3 && words[1] == "with" && float.TryParse(words[2], out num6))
			{
				listener.propelForwardPercent = new float?(Mathf.Clamp(num6, -100f, 100f));
			}
			else
			{
				listener.propelForwardPercent = new float?(10f);
			}
			break;
		}
		case "rotate_forward":
		{
			float num7;
			if (words.Length == 3 && words[1] == "with" && float.TryParse(words[2], out num7))
			{
				listener.rotateForwardPercent = new float?(Mathf.Clamp(num7, -100f, 100f));
			}
			else
			{
				listener.rotateForwardPercent = new float?(10f);
			}
			break;
		}
		case "tell":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Self, text));
			}
			break;
		case "tell_nearby":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Nearby, text));
			}
			break;
		case "tell_any":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Any, text));
			}
			break;
		case "tell_body":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Body, text));
			}
			break;
		case "tell_first_of_any":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				listener.tells.Add(new KeyValuePair<TellType, string>(TellType.FirstOfAny, text));
			}
			break;
		case "tell_in_front":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				listener.tells.Add(new KeyValuePair<TellType, string>(TellType.InFront, text));
			}
			break;
		case "tell_first_in_front":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				listener.tells.Add(new KeyValuePair<TellType, string>(TellType.FirstInFront, text));
			}
			break;
		case "tell_web":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				if (parentThing.version >= 6)
				{
					listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Web, text));
				}
				else
				{
					listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Self, "web" + text));
					listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Self, "web " + text));
				}
			}
			break;
		case "tell_any_web":
			if (text != string.Empty)
			{
				if (listener.tells == null)
				{
					listener.tells = new List<KeyValuePair<TellType, string>>();
				}
				if (parentThing.version >= 6)
				{
					listener.tells.Add(new KeyValuePair<TellType, string>(TellType.AnyWeb, text));
				}
				else
				{
					listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Any, "web" + text));
					listener.tells.Add(new KeyValuePair<TellType, string>(TellType.Any, "web " + text));
				}
			}
			break;
		case "play":
			if (text != string.Empty)
			{
				if (listener.sounds == null)
				{
					listener.sounds = new List<Sound>();
				}
				listener.sounds.Add(BehaviorScriptParser.GetSound(text));
			}
			break;
		case "send_all_to":
		case "send_nearby_to":
		case "send_one_nearby_to":
			if (text != string.Empty)
			{
				listener.transportMultiplePeople = words[0] != "send_one_nearby_to";
				listener.transportNearbyOnly = words[0] != "send_all_to";
				listener.rotationAfterTransport = BehaviorScriptParser.ExtractDegreesValue(ref text);
				string[] array3 = Misc.Split(text, " onto ", StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length == 2)
				{
					if (array3[0] != string.Empty)
					{
						listener.transportToArea = array3[0];
					}
					if (array3[1] != string.Empty)
					{
						listener.transportOntoThing = array3[1];
						string[] array4 = Misc.Split(listener.transportOntoThing, " via ", StringSplitOptions.RemoveEmptyEntries);
						if (array4.Length == 2)
						{
							listener.transportOntoThing = array4[0];
							if (!string.IsNullOrEmpty(listener.transportToArea))
							{
								listener.transportToArea = listener.transportToArea + " via " + array4[1];
							}
						}
					}
				}
				else
				{
					listener.transportToArea = text;
				}
				if (listener.transportToArea != null)
				{
					string[] array5 = Misc.Split(listener.transportToArea, " via ", StringSplitOptions.RemoveEmptyEntries);
					if (array5.Length == 2 && array5[0] != string.Empty && array5[1] != string.Empty)
					{
						listener.transportToArea = array5[0];
						string[] array6 = Misc.Split(array5[1], " ", StringSplitOptions.RemoveEmptyEntries);
						if (array6.Length >= 2)
						{
							string text3 = array6[0];
							text3 = text3.Replace("s", string.Empty);
							float num8;
							if (float.TryParse(text3, out num8))
							{
								string text4 = string.Join(" ", array6, 1, array6.Length - 1);
								if (text4 != string.Empty)
								{
									listener.transportViaArea = text4;
									listener.transportViaAreaSeconds = Mathf.Clamp(num8, 1f, 150f);
								}
							}
						}
					}
				}
			}
			break;
		case "send_all_onto":
		case "send_nearby_onto":
		case "send_one_nearby_onto":
			if (text != string.Empty)
			{
				listener.rotationAfterTransport = BehaviorScriptParser.ExtractDegreesValue(ref text);
				listener.transportOntoThing = text;
				listener.transportMultiplePeople = words[0] != "send_one_nearby_onto";
				listener.transportNearbyOnly = words[0] != "send_all_onto";
			}
			break;
		case "call_me":
			if (text != string.Empty)
			{
				string[] array7 = new string[] { ";", ",", "|", "\n" };
				foreach (string text5 in array7)
				{
					text = text.Replace(text5, " ");
				}
				listener.callMeThisName = text;
			}
			break;
		case "loop":
			if (text != string.Empty)
			{
				string[] array9 = Misc.Split(text, " with ", StringSplitOptions.RemoveEmptyEntries);
				if (array9.Length == 2)
				{
					listener.startLoopSoundName = array9[0];
					listener.loopVolume = 1f;
					listener.loopSpatialBlend = 1f;
					string[] array10 = Misc.Split(array9[1], " ", StringSplitOptions.RemoveEmptyEntries);
					int j = 0;
					while (j < array10.Length)
					{
						string text6 = array10[j];
						if (text6 == null)
						{
							goto IL_11ED;
						}
						if (!(text6 == "surround"))
						{
							if (!(text6 == "half-surround"))
							{
								goto IL_11ED;
							}
							listener.loopSpatialBlend = 0.5f;
						}
						else
						{
							listener.loopSpatialBlend = 0f;
						}
						IL_1231:
						j++;
						continue;
						IL_11ED:
						text6 = text6.Replace("%", string.Empty);
						float num9;
						if (float.TryParse(text6, out num9))
						{
							listener.loopVolume = Mathf.Clamp(num9 / 100f, 0.001f, 5f);
						}
						goto IL_1231;
					}
				}
				else
				{
					listener.startLoopSoundName = text;
					listener.loopVolume = 1f;
					listener.loopSpatialBlend = 1f;
				}
			}
			break;
		case "end_loop":
			listener.doEndLoopSound = true;
			break;
		case "all_parts_face_someone":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.startTowardsClosestPerson = true;
			break;
		case "all_parts_face_someone_else":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.startTowardsSecondClosestPerson = true;
			break;
		case "all_parts_face_up":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.startTowardsTop = true;
			break;
		case "all_parts_face_empty_hand":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.startTowardsClosestEmptyHand = true;
			break;
		case "all_parts_face_empty_hand_while_held":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.startTowardsClosestEmptyHandWhileHeld = true;
			break;
		case "all_parts_face_nearest":
			if (text != string.Empty)
			{
				BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
				text = BehaviorScriptParser.AdjustRotateThingLockSettings(text, listener.rotateThingSettings);
				listener.rotateThingSettings.startTowardsClosestThingOfName = text;
			}
			break;
		case "all_parts_face_view":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.startTowardsMainCamera = true;
			break;
		case "stop_all_parts_face_someone":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.stopTowardsPerson = true;
			break;
		case "stop_all_parts_face_up":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.stopTowardsTop = true;
			break;
		case "stop_all_parts_face_empty_hand":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.stopTowardsClosestEmptyHand = true;
			break;
		case "stop_all_parts_face_nearest":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.stopTowardsClosestThingOfName = true;
			break;
		case "stop_all_parts_face_view":
			BehaviorScriptParser.AddRotateThingSettingsIfNeeded(listener);
			listener.rotateThingSettings.stopTowardsMainCamera = true;
			break;
		case "destroy_all_parts":
			listener.destroyThingWeArePartOf = BehaviorScriptParser.GetThingDestruction(words);
			break;
		case "destroy_nearby":
			listener.destroyOtherThings = BehaviorScriptParser.GetOtherThingDestruction(words);
			break;
		case "give_haptic_feedback":
			listener.doHapticPulse = true;
			break;
		case "disallow_emitted_climbing":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.emittedClimbing = new bool?(false);
			break;
		case "disallow_emitted_transporting":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.emittedTransporting = new bool?(false);
			break;
		case "disallow_moving_through_obstacles":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.movingThroughObstacles = new bool?(false);
			break;
		case "disallow_vision_in_obstacles":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.visionInObstacles = new bool?(false);
			Debug.Log("listener.rights.visionInObstacles = false");
			break;
		case "disallow_invisibility":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.invisibility = new bool?(false);
			break;
		case "disallow_any_person_size":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.anyPersonSize = new bool?(false);
			break;
		case "disallow_highlighting":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.highlighting = new bool?(false);
			break;
		case "disallow_amplified_speech":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.amplifiedSpeech = new bool?(false);
			break;
		case "disallow_any_destruction":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.anyDestruction = new bool?(false);
			break;
		case "disallow_web_browsing":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.webBrowsing = new bool?(false);
			break;
		case "disallow_untargeted_attract_and_repel":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.untargetedAttractThings = new bool?(false);
			break;
		case "disallow_build_animations":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.slowBuildCreation = new bool?(false);
			break;
		case "allow_emitted_climbing":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.emittedClimbing = new bool?(true);
			break;
		case "allow_emitted_transporting":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.emittedTransporting = new bool?(true);
			break;
		case "allow_moving_through_obstacles":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.movingThroughObstacles = new bool?(true);
			break;
		case "allow_vision_in_obstacles":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.visionInObstacles = new bool?(true);
			break;
		case "allow_invisibility":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.invisibility = new bool?(true);
			break;
		case "allow_any_person_size":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.anyPersonSize = new bool?(true);
			break;
		case "allow_highlighting":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.highlighting = new bool?(true);
			break;
		case "allow_amplified_speech":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.amplifiedSpeech = new bool?(true);
			break;
		case "allow_any_destruction":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.anyDestruction = new bool?(true);
			break;
		case "allow_web_browsing":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.webBrowsing = new bool?(true);
			break;
		case "allow_untargeted_attract_and_repel":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.untargetedAttractThings = new bool?(true);
			break;
		case "allow_build_animations":
			BehaviorScriptParser.InitAreaRightsIfNeeded(listener);
			listener.rights.slowBuildCreation = new bool?(true);
			break;
		case "show_board":
			listener.showDialog = new DialogType?(DialogType.Forum);
			listener.showData = text;
			break;
		case "show_thread":
			listener.showDialog = new DialogType?(DialogType.ForumThread);
			listener.showData = text;
			break;
		case "show_areas":
			listener.showDialog = new DialogType?(DialogType.FindAreas);
			listener.showData = text;
			break;
		case "show_inventory":
			listener.showDialog = new DialogType?(DialogType.Inventory);
			break;
		case "show_chat_keyboard":
			listener.showDialog = new DialogType?(DialogType.Keyboard);
			break;
		case "show_video_controls":
			listener.showDialog = new DialogType?(DialogType.VideoControl);
			break;
		case "show_camera_controls":
			listener.showDialog = new DialogType?(DialogType.CameraControl);
			break;
		case "show_slideshow_controls":
			listener.showDialog = new DialogType?(DialogType.SlideshowControl);
			break;
		case "show_name_tags":
		{
			float num10 = 30f;
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Replace("s", string.Empty);
				int num11;
				if (int.TryParse(text, out num11))
				{
					num10 = Mathf.Clamp((float)num11, 0.1f, 86400f);
				}
			}
			listener.showNameTagsAgainSeconds = new float?(num10);
			break;
		}
		case "do_creation_part":
		case "do_all_creation_parts":
		{
			listener.creationPartChangeIsForAll = words[0] == "do_all_creation_parts";
			if (thenPart.IndexOf(" local ") >= 0)
			{
				thenPart = thenPart.Replace(" local ", " ");
				listener.creationPartChangeIsLocal = true;
			}
			if (thenPart.IndexOf(" random ") >= 0)
			{
				thenPart = thenPart.Replace(" random ", " ");
				listener.creationPartChangeIsRandom = true;
			}
			string[] array11 = thenPart.Split(array, StringSplitOptions.RemoveEmptyEntries);
			if (array11.Length >= 2)
			{
				listener.creationPartChangeMode = array11[1];
				if (array11.Length >= 3)
				{
					listener.creationPartChangeValues = new float[array11.Length - 2];
					for (int k = 0; k < listener.creationPartChangeValues.Length; k++)
					{
						listener.creationPartChangeValues[k] = BehaviorScriptParser.GetCreationPartChangeFloat(array11[k + 2]);
					}
				}
				else
				{
					listener.creationPartChangeValues = new float[0];
				}
			}
			break;
		}
		case "go_to_inventory_page":
		{
			int num12;
			if (words.Length == 2 && int.TryParse(words[1], out num12))
			{
				listener.goToInventoryPage = Mathf.Clamp(num12, 1, 100);
			}
			break;
		}
		case "add_crumbles":
			listener.addCrumbles = true;
			break;
		case "add_crumbles_for_all_parts":
			listener.addCrumbles = true;
			listener.addEffectIsForAllParts = true;
			break;
		case "set_light_intensity":
		{
			float num13;
			if (listener.eventType == StateListener.EventType.OnStarts && float.TryParse(words[1], out num13))
			{
				listener.setLightIntensity = Mathf.Clamp(num13, 0f, 100f) * 0.01f * Universe.maxLightIntensity;
			}
			break;
		}
		case "set_light_range":
		{
			float num14;
			if (listener.eventType == StateListener.EventType.OnStarts && float.TryParse(words[1], out num14))
			{
				listener.setLightRange = Mathf.Clamp(num14, 0f, 10000f);
			}
			break;
		}
		case "set_light_cone_size":
		{
			float num15;
			if (listener.eventType == StateListener.EventType.OnStarts && float.TryParse(words[1], out num15))
			{
				listener.setLightConeSize = Mathf.Clamp(num15, 0f, 100f) * 0.01f * Universe.maxLightConeSize;
			}
			break;
		}
		case "set_run_speed":
			if (words.Length == 2)
			{
				float num16;
				if (words[1] == "default")
				{
					BehaviorScriptParser.InitDesktopModeSettingsIfNeeded(listener);
					listener.desktopModeSettings.runSpeed = new float?(4.5f);
				}
				else if (float.TryParse(words[1], out num16))
				{
					BehaviorScriptParser.InitDesktopModeSettingsIfNeeded(listener);
					listener.desktopModeSettings.runSpeed = new float?(Mathf.Clamp(num16, 2f, 100f));
				}
			}
			break;
		case "set_jump_speed":
			if (words.Length == 2)
			{
				float num17;
				if (words[1] == "default")
				{
					BehaviorScriptParser.InitDesktopModeSettingsIfNeeded(listener);
					listener.desktopModeSettings.jumpSpeed = new float?(5f);
				}
				else if (float.TryParse(words[1], out num17))
				{
					BehaviorScriptParser.InitDesktopModeSettingsIfNeeded(listener);
					listener.desktopModeSettings.jumpSpeed = new float?(Mathf.Clamp(num17, 0f, 100f));
				}
			}
			break;
		case "set_slidiness":
			if (words.Length == 2)
			{
				float num18;
				if (words[1] == "default")
				{
					BehaviorScriptParser.InitDesktopModeSettingsIfNeeded(listener);
					listener.desktopModeSettings.slidiness = new float?(0f);
				}
				else if (float.TryParse(words[1], out num18))
				{
					BehaviorScriptParser.InitDesktopModeSettingsIfNeeded(listener);
					listener.desktopModeSettings.slidiness = new float?(Mathf.Clamp(num18, 0f, 100f));
				}
			}
			break;
		case "set_speed":
		{
			float num20;
			float num21;
			float num22;
			if (words.Length == 2)
			{
				float num19;
				if (float.TryParse(words[1], out num19))
				{
					listener.velocitySetter = new Vector3?(new Vector3(num19, num19, num19));
				}
			}
			else if (words.Length == 4 && float.TryParse(words[1], out num20) && float.TryParse(words[2], out num21) && float.TryParse(words[3], out num22))
			{
				listener.velocitySetter = new Vector3?(new Vector3(num20, num21, num22));
			}
			Vector3? velocitySetter = listener.velocitySetter;
			if (velocitySetter != null)
			{
				Vector3? velocitySetter2 = listener.velocitySetter;
				listener.velocitySetter = new Vector3?(Misc.ClampVector3(velocitySetter2.Value, -1000f, 1000f));
			}
			break;
		}
		case "add_speed":
		{
			float num24;
			float num25;
			float num26;
			if (words.Length == 2)
			{
				float num23;
				if (float.TryParse(words[1], out num23))
				{
					listener.forceAdder = new Vector3?(new Vector3(num23, num23, num23));
				}
			}
			else if (words.Length == 4 && float.TryParse(words[1], out num24) && float.TryParse(words[2], out num25) && float.TryParse(words[3], out num26))
			{
				listener.forceAdder = new Vector3?(new Vector3(num24, num25, num26));
			}
			Vector3? forceAdder = listener.forceAdder;
			if (forceAdder != null)
			{
				Vector3? forceAdder2 = listener.forceAdder;
				listener.forceAdder = new Vector3?(Misc.ClampVector3(forceAdder2.Value, -1000f, 1000f));
			}
			break;
		}
		case "multiply_speed":
		{
			float num28;
			float num29;
			float num30;
			if (words.Length == 2)
			{
				float num27;
				if (float.TryParse(words[1], out num27))
				{
					listener.velocityMultiplier = new Vector3?(new Vector3(num27, num27, num27));
				}
			}
			else if (words.Length == 4 && float.TryParse(words[1], out num28) && float.TryParse(words[2], out num29) && float.TryParse(words[3], out num30))
			{
				listener.velocityMultiplier = new Vector3?(new Vector3(num28, num29, num30));
			}
			Vector3? velocityMultiplier = listener.velocityMultiplier;
			if (velocityMultiplier != null)
			{
				Vector3? velocityMultiplier2 = listener.velocityMultiplier;
				listener.velocityMultiplier = new Vector3?(Misc.ClampVector3(velocityMultiplier2.Value, 0f, 1000f));
			}
			break;
		}
		case "set_camera_position_to":
			switch (text)
			{
			case "default":
				listener.setFollowerCameraPosition = new FollowerCameraPosition?(FollowerCameraPosition.InHeadVr);
				break;
			case "optimized view":
				listener.setFollowerCameraPosition = new FollowerCameraPosition?(FollowerCameraPosition.InHeadDesktopOptimized);
				break;
			case "view from behind me":
				listener.setFollowerCameraPosition = new FollowerCameraPosition?(FollowerCameraPosition.BehindUp);
				break;
			case "view from further behind me":
				listener.setFollowerCameraPosition = new FollowerCameraPosition?(FollowerCameraPosition.FurtherBehindUp);
				break;
			case "bird's eye":
				listener.setFollowerCameraPosition = new FollowerCameraPosition?(FollowerCameraPosition.BirdsEye);
				break;
			case "looking at me":
				listener.setFollowerCameraPosition = new FollowerCameraPosition?(FollowerCameraPosition.LooksAtMe);
				break;
			case "left hand":
				listener.setFollowerCameraPosition = new FollowerCameraPosition?(FollowerCameraPosition.AtLeftHand);
				break;
			case "right hand":
				listener.setFollowerCameraPosition = new FollowerCameraPosition?(FollowerCameraPosition.AtRightHand);
				break;
			}
			break;
		case "set_camera_following_to":
			if (text != null)
			{
				if (!(text == "default"))
				{
					if (!(text == "smoothly"))
					{
						if (!(text == "very smoothly"))
						{
							if (text == "none")
							{
								listener.setFollowerCameraLerp = new float?(0f);
							}
						}
						else
						{
							listener.setFollowerCameraLerp = new float?(0.0075f);
						}
					}
					else
					{
						listener.setFollowerCameraLerp = new float?(0.025f);
					}
				}
				else
				{
					listener.setFollowerCameraLerp = new float?(1f);
				}
			}
			break;
		case "type":
			text = text.Replace("\"", string.Empty);
			if (text != string.Empty)
			{
				listener.doTypeText = text;
			}
			break;
		case "change_head_to":
		case "change_heads_to":
			if (text != string.Empty)
			{
				text = BehaviorScriptParser.ReplaceIncludedNamesWithIdsInData(parentThing.includedNameIds, text).Trim();
				if (!string.IsNullOrEmpty(text))
				{
					listener.attachThingIdAsHead = text;
					listener.attachToMultiplePeople = words[0] == "change_one_head_to";
				}
			}
			break;
		case "attach_head":
			if (text != string.Empty)
			{
				text = BehaviorScriptParser.ReplaceIncludedNamesWithIdsInData(parentThing.includedNameIds, text).Trim();
				if (!string.IsNullOrEmpty(text))
				{
					listener.attachThingIdAsHead = text;
				}
			}
			break;
		case "resize_nearby_to":
		{
			text = text.Replace("%", string.Empty);
			float num31;
			if (float.TryParse(text, out num31) && num31 >= 1f && num31 <= 2500f)
			{
				num31 = Mathf.Clamp(num31, 1f, 2500f);
				if (num31 == 100f || Mathf.Abs(100f - num31) >= 10f)
				{
					listener.resizeNearby = new float?(num31 * 0.01f);
				}
			}
			break;
		}
		case "let_go":
			listener.letGo = true;
			break;
		case "stream_to":
			if (!string.IsNullOrEmpty(text))
			{
				listener.streamMyCameraView = new bool?(true);
				listener.streamTargetName = text;
			}
			break;
		case "stream_stop":
			listener.streamMyCameraView = new bool?(false);
			break;
		case "say":
			listener.say = text.Replace("\"", " ").Trim();
			break;
		case "set_voice":
			listener.setVoiceProperties = BehaviorScriptParser.GetVoicePropertiesFromData(text);
			break;
		case "set_snap_angles_to":
			if (text != string.Empty)
			{
				float num32;
				if (text == "default")
				{
					listener.setCustomSnapAngles = new float?(0f);
				}
				else if (float.TryParse(text, out num32))
				{
					listener.setCustomSnapAngles = new float?(Mathf.Clamp(num32, 0f, 360f));
				}
			}
			break;
		case "play_track":
			if (text != string.Empty)
			{
				listener.soundTrackData = text;
			}
			break;
		case "set_gravity_to":
			if (text != string.Empty)
			{
				if (text == "default")
				{
					listener.setGravity = new Vector3?(new Vector3(0f, -9.81f, 0f));
				}
				else
				{
					Vector3? spaceSeparatedStringToVector = Misc.GetSpaceSeparatedStringToVector3(text, 1000f);
					if (spaceSeparatedStringToVector != null)
					{
						listener.setGravity = spaceSeparatedStringToVector;
					}
				}
			}
			break;
		case "is":
			if (words.Length >= 2)
			{
				string text7 = string.Join(" ", words, 1, words.Length - 1);
				if (text7 != string.Empty)
				{
					if (listener.variableOperations == null)
					{
						listener.variableOperations = new List<string>();
					}
					listener.variableOperations.Add(text7);
				}
			}
			break;
		case "reset_area":
			BehaviorScriptParser.AddResetSettingsIfNeeded(listener);
			listener.resetSettings.area = true;
			break;
		case "reset_persons":
			BehaviorScriptParser.AddResetSettingsIfNeeded(listener);
			listener.resetSettings.allPersonVariablesInArea = true;
			break;
		case "reset_position":
			BehaviorScriptParser.AddResetSettingsIfNeeded(listener);
			listener.resetSettings.position = true;
			break;
		case "reset_rotation":
			BehaviorScriptParser.AddResetSettingsIfNeeded(listener);
			listener.resetSettings.rotation = true;
			break;
		case "reset_body":
			BehaviorScriptParser.AddResetSettingsIfNeeded(listener);
			listener.resetSettings.body = true;
			break;
		case "reset_legs_to_default":
			BehaviorScriptParser.AddResetSettingsIfNeeded(listener);
			listener.resetSettings.legsToUniversalDefault = true;
			break;
		case "reset_legs_to_body_default":
			BehaviorScriptParser.AddResetSettingsIfNeeded(listener);
			listener.resetSettings.legsToBodyDefault = true;
			break;
		case "write":
			listener.setText = text.Replace("\"", " ").Trim();
			if (listener.setText == listener.setText.ToLower())
			{
				listener.setText = listener.setText.ToUpper();
			}
			break;
		case "turn":
			if (words.Length >= 2 && BehaviorScriptParser.turnCommands.Contains(words[1]))
			{
				listener.turn = words[1];
			}
			break;
		case "turn_thing":
			if (words.Length >= 2 && BehaviorScriptParser.turnCommands.Contains(words[1]))
			{
				listener.turnThing = words[1];
			}
			break;
		case "turn_sub_thing":
			if (words.Length >= 2)
			{
				string text8 = words[words.Length - 1];
				if (BehaviorScriptParser.turnCommands.Contains(text8))
				{
					listener.turnSubThing = text8;
					if (words.Length >= 3)
					{
						listener.turnSubThingName = string.Join(" ", words, 1, words.Length - 2);
					}
				}
			}
			break;
		case "trail_start":
		{
			listener.partTrailSettings = new PartTrailSettings();
			listener.partTrailSettings.isStart = true;
			string[] array12 = Misc.Split(thenPart, " with ", StringSplitOptions.RemoveEmptyEntries);
			if (array12.Length == 2)
			{
				string[] array13 = Misc.Split(array12[1], " ", StringSplitOptions.RemoveEmptyEntries);
				int l = 0;
				while (l < array13.Length)
				{
					string text9 = array13[l];
					if (text9 == null)
					{
						goto IL_2417;
					}
					if (!(text9 == "thick-start"))
					{
						if (!(text9 == "thick-end"))
						{
							goto IL_2417;
						}
						listener.partTrailSettings.thickEnd = true;
					}
					else
					{
						listener.partTrailSettings.thickStart = true;
					}
					IL_2459:
					l++;
					continue;
					IL_2417:
					string text10 = text9.Replace("s", string.Empty);
					float num33;
					if (float.TryParse(text10, out num33))
					{
						listener.partTrailSettings.durationSeconds = Mathf.Clamp(num33, 0.01f, 60f);
					}
					goto IL_2459;
				}
			}
			break;
		}
		case "trail_end":
			listener.partTrailSettings = new PartTrailSettings();
			listener.partTrailSettings.isStart = false;
			break;
		case "project":
		{
			listener.projectPartSettings = new ProjectPartSettings();
			string[] array14 = Misc.Split(thenPart, " with ", StringSplitOptions.RemoveEmptyEntries);
			if (array14.Length == 2)
			{
				string[] array15 = Misc.Split(array14[1], " ", StringSplitOptions.RemoveEmptyEntries);
				for (int m = 0; m < array15.Length; m++)
				{
					string text11 = array15[m];
					string text12 = ((m < 1) ? null : array15[m - 1]);
					if (text11 != null)
					{
						if (!(text11 == "reach"))
						{
							if (!(text11 == "default"))
							{
								if (!(text11 == "max"))
								{
									if (!(text11 == "alignment"))
									{
										if (text11 == "counter-alignment")
										{
											listener.projectPartSettings.align = ProjectPartAlignment.AwayFromSurface;
										}
									}
									else
									{
										listener.projectPartSettings.align = ProjectPartAlignment.TowardsSurface;
									}
								}
								else
								{
									string text13 = text12.Replace("%", string.Empty);
									float num34;
									if (float.TryParse(text13, out num34))
									{
										listener.projectPartSettings.maxDistance = Mathf.Clamp(num34, 0.01f, 10000f);
									}
								}
							}
							else
							{
								string text14 = text12.Replace("%", string.Empty);
								float num35;
								if (float.TryParse(text14, out num35))
								{
									listener.projectPartSettings.defaultDistance = Mathf.Clamp(num35, 0f, 1000f);
								}
							}
						}
						else
						{
							string text15 = text12.Replace("%", string.Empty);
							float num36;
							if (float.TryParse(text15, out num36))
							{
								num36 = Mathf.Clamp(num36, 0f, 1000f);
								listener.projectPartSettings.relativeReach = num36 * 0.01f;
							}
						}
					}
				}
			}
			break;
		}
		case "set_area_visibility_to":
			if (text == "default")
			{
				listener.limitAreaVisibilityMeters = new float?(-1f);
			}
			else
			{
				string text16 = text.Replace("m", string.Empty);
				float num37;
				if (float.TryParse(text16, out num37))
				{
					listener.limitAreaVisibilityMeters = new float?(Mathf.Clamp(num37, 2.5f, 10000f));
				}
			}
			break;
		case "set_person_as_authority":
			listener.makePersonMasterClient = true;
			break;
		case "show_line":
		{
			listener.partLineSettings = new PartLineSettings();
			string[] array16 = Misc.Split(thenPart, " with ", StringSplitOptions.RemoveEmptyEntries);
			if (array16.Length == 2)
			{
				string[] array17 = Misc.Split(array16[1], " ", StringSplitOptions.RemoveEmptyEntries);
				for (int n = 0; n < array17.Length; n++)
				{
					string text17 = array17[n];
					string text18 = ((n < 1) ? null : array17[n - 1]);
					if (text17 != null)
					{
						float num40;
						if (!(text17 == "width"))
						{
							float num39;
							if (!(text17 == "start-width"))
							{
								if (text17 == "end-width")
								{
									float num38;
									if (float.TryParse(text18, out num38))
									{
										listener.partLineSettings.endWidth = Mathf.Clamp(num38, 0f, 10f);
									}
								}
							}
							else if (float.TryParse(text18, out num39))
							{
								listener.partLineSettings.startWidth = Mathf.Clamp(num39, 0f, 10f);
							}
						}
						else if (float.TryParse(text18, out num40))
						{
							listener.partLineSettings.startWidth = Mathf.Clamp(num40, 0f, 10f);
							listener.partLineSettings.endWidth = Mathf.Clamp(num40, 0f, 10f);
						}
					}
				}
			}
			break;
		}
		case "show_video":
			if (words.Length >= 2)
			{
				TextLink textLink = new TextLink();
				string text19 = string.Join(" ", words, 1, words.Length - 1);
				if (textLink.TryParseText(text19))
				{
					listener.playVideoId = textLink.content;
					if (text19.Contains(" with "))
					{
						foreach (string text20 in words)
						{
							string text21 = text20.Replace("%", string.Empty);
							float num42;
							if (float.TryParse(text21, out num42))
							{
								listener.playVideoVolume = new float?(Mathf.Clamp(num42 * 0.01f, 0f, 3f));
							}
						}
					}
				}
			}
			break;
		case "show_web":
			if (words.Length >= 2)
			{
				string text22 = words[1];
				if (text22.IndexOf("://") == -1)
				{
					text22 = "http://" + text22;
				}
				if (text22.IndexOf("http://") == 0 || text22.IndexOf("https://") == 0)
				{
					BrowserSettings browserSettings = new BrowserSettings();
					browserSettings.url = text22;
					string[] array19 = Misc.Split(thenPart, " with ", StringSplitOptions.RemoveEmptyEntries);
					if (array19.Length == 2)
					{
						string[] array20 = Misc.Split(array19[1], " ", StringSplitOptions.RemoveEmptyEntries);
						for (int num43 = 0; num43 < array20.Length; num43++)
						{
							string text23 = array20[num43];
							string text24 = ((num43 < 1) ? null : array20[num43 - 1]);
							if (text23 != null)
							{
								if (!(text23 == "zoom"))
								{
									if (!(text23 == "navigation-free"))
									{
										if (!(text23 == "cursor-free"))
										{
											if (!(text23 == "useJoystick"))
											{
												if (text23 == "unsynced")
												{
													browserSettings.syncUrlChangesBetweenPeople = false;
												}
											}
											else
											{
												browserSettings.useJoystick = true;
											}
										}
										else
										{
											browserSettings.allowCursor = false;
										}
									}
									else
									{
										browserSettings.allowUrlNavigation = false;
									}
								}
								else if (text24 != null)
								{
									string text25 = text24.Replace("%", string.Empty);
									float num44;
									if (float.TryParse(text25, out num44))
									{
										browserSettings.zoomPercent = Mathf.Clamp(num44, 1f, 1000f);
									}
								}
							}
						}
					}
					listener.browserSettings = browserSettings;
				}
			}
			break;
		case "set_constant_rotation_to":
			if (words.Length == 4)
			{
				listener.constantRotation = new Vector3?(new Vector3(BehaviorScriptParser.StringToFloat(words[1], 10000f), BehaviorScriptParser.StringToFloat(words[2], 10000f), BehaviorScriptParser.StringToFloat(words[3], 10000f)));
				if (parentThing.version <= 6)
				{
					Vector3? constantRotation = listener.constantRotation;
					listener.constantRotation = ((constantRotation == null) ? null : new Vector3?(constantRotation.GetValueOrDefault() * 10f));
				}
			}
			break;
		case "set_quest_achieve":
		case "set_quest_unachieve":
		case "set_quest_remove":
			if (!string.IsNullOrEmpty(text))
			{
				QuestAction questAction = new QuestAction();
				questAction.questName = text.Trim().ToLower();
				string text26 = words[0];
				if (text26 != null)
				{
					if (!(text26 == "set_quest_achieve"))
					{
						if (!(text26 == "set_quest_unachieve"))
						{
							if (text26 == "set_quest_remove")
							{
								questAction.actionType = QuestActionType.Remove;
							}
						}
						else
						{
							questAction.actionType = QuestActionType.Unachieve;
						}
					}
					else
					{
						questAction.actionType = QuestActionType.Achieve;
					}
				}
				listener.questAction = questAction;
			}
			break;
		case "enable_setting":
		case "disable_setting":
			if (!string.IsNullOrEmpty(text))
			{
				string text27 = Misc.ToTitleCase(text).Replace(" ", string.Empty);
				try
				{
					Setting setting = (Setting)Enum.Parse(typeof(Setting), text27, true);
					if (listener.settings == null)
					{
						listener.settings = new Dictionary<Setting, bool>();
						listener.settings[setting] = words[0] == "enable_setting";
					}
				}
				catch (Exception ex)
				{
				}
			}
			break;
		case "set_attract":
		case "set_repel":
			BehaviorScriptParser.ParseAttractRepel(listener, text, words);
			break;
		}
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x000C543C File Offset: 0x000C383C
	private static void AddRotateThingSettingsIfNeeded(StateListener listener)
	{
		if (listener.rotateThingSettings == null)
		{
			listener.rotateThingSettings = new RotateThingSettings();
		}
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x000C5454 File Offset: 0x000C3854
	private static void AddResetSettingsIfNeeded(StateListener listener)
	{
		if (listener.resetSettings == null)
		{
			listener.resetSettings = new ResetSettings();
		}
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x000C546C File Offset: 0x000C386C
	private static void ParseAttractRepel(StateListener listener, string data, string[] words)
	{
		if (!string.IsNullOrEmpty(data))
		{
			AttractThingsSettings attractThingsSettings = new AttractThingsSettings();
			string[] array = Misc.Split(data, " ", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				float num;
				if (float.TryParse(text, out num))
				{
					if (words[0] == "set_repel")
					{
						num *= -1f;
					}
					num = Mathf.Clamp(num, -1000f, 1000f);
					attractThingsSettings.strength = num;
				}
				else if (text == "forward-only")
				{
					attractThingsSettings.forwardOnly = true;
				}
				else
				{
					attractThingsSettings.thingNameFilter = ((!(text == "*")) ? text : null);
				}
			}
			listener.attractThingsSettings = attractThingsSettings;
		}
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x000C5540 File Offset: 0x000C3940
	private static string AdjustRotateThingLockSettings(string data, RotateThingSettings settings)
	{
		string text = string.Empty;
		BoolVector3 boolVector = new BoolVector3(false, false, false);
		BoolVector3 boolVector2 = new BoolVector3(false, false, false);
		if (data.Contains(text = " lock xy"))
		{
			data = data.Replace(text, " ");
			boolVector.x = true;
			boolVector.y = true;
		}
		if (data.Contains(text = " lock xz"))
		{
			data = data.Replace(text, " ");
			boolVector.x = true;
			boolVector.z = true;
		}
		if (data.Contains(text = " lock yz"))
		{
			data = data.Replace(text, " ");
			boolVector.y = true;
			boolVector.z = true;
		}
		if (data.Contains(text = " lock x"))
		{
			data = data.Replace(text, " ");
			boolVector.x = true;
		}
		if (data.Contains(text = " lock y"))
		{
			data = data.Replace(text, " ");
			boolVector.y = true;
		}
		if (data.Contains(text = " lock z"))
		{
			data = data.Replace(text, " ");
			boolVector.z = true;
		}
		if (data.Contains(text = " lock local xy"))
		{
			data = data.Replace(text, " ");
			boolVector2.x = true;
			boolVector2.y = true;
		}
		if (data.Contains(text = " lock local xz"))
		{
			data = data.Replace(text, " ");
			boolVector2.x = true;
			boolVector2.z = true;
		}
		if (data.Contains(text = " lock local yz"))
		{
			data = data.Replace(text, " ");
			boolVector2.y = true;
			boolVector2.z = true;
		}
		if (data.Contains(text = " lock local x"))
		{
			data = data.Replace(text, " ");
			boolVector2.x = true;
		}
		if (data.Contains(text = " lock local y"))
		{
			data = data.Replace(text, " ");
			boolVector2.y = true;
		}
		if (data.Contains(text = " lock local z"))
		{
			data = data.Replace(text, " ");
			boolVector2.z = true;
		}
		data = data.TrimEnd(new char[0]);
		if (!boolVector.IsAllDefault())
		{
			if (settings.locked == null)
			{
				settings.locked = new BoolVector3(false, false, false);
			}
			if (boolVector.x)
			{
				settings.locked.x = true;
			}
			if (boolVector.y)
			{
				settings.locked.y = true;
			}
			if (boolVector.z)
			{
				settings.locked.z = true;
			}
		}
		if (!boolVector2.IsAllDefault())
		{
			if (settings.lockedLocal == null)
			{
				settings.lockedLocal = new BoolVector3(false, false, false);
			}
			if (boolVector2.x)
			{
				settings.lockedLocal.x = true;
			}
			if (boolVector2.y)
			{
				settings.lockedLocal.y = true;
			}
			if (boolVector2.z)
			{
				settings.lockedLocal.z = true;
			}
		}
		return data;
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x000C5844 File Offset: 0x000C3C44
	private static float StringToFloat(string s, float max = 0f)
	{
		float num = 0f;
		float num2;
		if (float.TryParse(s, out num2))
		{
			num = num2;
			if (max != 0f)
			{
				num = Mathf.Clamp(num, -max, max);
			}
		}
		return num;
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x000C587C File Offset: 0x000C3C7C
	private static ThingDestruction GetThingDestruction(string[] words)
	{
		ThingDestruction thingDestruction = new ThingDestruction();
		if (words.Length >= 3 && words[1] == "with")
		{
			for (int i = 2; i < words.Length; i++)
			{
				string text = words[i];
				switch (text)
				{
				case "burst":
					thingDestruction.burst = true;
					break;
				case "force":
				{
					thingDestruction.burst = true;
					string text2 = words[i - 1];
					float num2;
					if (float.TryParse(text2, out num2))
					{
						thingDestruction.burstVelocity = Mathf.Clamp(num2, 0f, 1000f);
					}
					break;
				}
				case "parts":
				{
					thingDestruction.burst = true;
					string text3 = words[i - 1];
					int num3;
					if (int.TryParse(text3, out num3))
					{
						thingDestruction.maxParts = Mathf.Clamp(num3, 1, 250);
					}
					break;
				}
				case "gravity-free":
					thingDestruction.burst = true;
					thingDestruction.gravity = false;
					break;
				case "bouncy":
					thingDestruction.burst = true;
					thingDestruction.bouncy = true;
					break;
				case "slidy":
					thingDestruction.burst = true;
					thingDestruction.slidy = true;
					break;
				case "uncollidable":
					thingDestruction.burst = true;
					thingDestruction.collides = false;
					break;
				case "self-uncollidable":
					thingDestruction.burst = true;
					thingDestruction.collidesWithSiblings = false;
					break;
				case "disappear":
				{
					string text4 = words[i - 1];
					float num4;
					if (float.TryParse(text4, out num4))
					{
						thingDestruction.burst = true;
						thingDestruction.hidePartsInSeconds = Mathf.Clamp(num4, 0.1f, 60f);
					}
					break;
				}
				case "grow":
				case "shrink":
				{
					string text5 = words[i - 1];
					float num5;
					if (float.TryParse(text5, out num5))
					{
						num5 = Mathf.Clamp(num5, 0.01f, 100f);
						if (text == "shrink")
						{
							num5 *= -1f;
						}
						thingDestruction.burst = true;
						thingDestruction.growth = num5;
					}
					break;
				}
				case "restore":
				{
					string text6 = words[i - 1];
					text6 = text6.Replace("s", string.Empty);
					float num6;
					if (float.TryParse(text6, out num6))
					{
						thingDestruction.restoreInSeconds = new float?(Mathf.Clamp(num6, 0.01f, 86400f));
					}
					break;
				}
				}
			}
		}
		return thingDestruction;
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x000C5B6C File Offset: 0x000C3F6C
	private static OtherThingDestruction GetOtherThingDestruction(string[] words)
	{
		OtherThingDestruction otherThingDestruction = new OtherThingDestruction();
		otherThingDestruction.thingDestruction = BehaviorScriptParser.GetThingDestruction(words);
		if (words.Length >= 3 && words[1] == "with")
		{
			for (int i = 2; i < words.Length; i++)
			{
				string text = words[i];
				if (text != null)
				{
					if (!(text == "radius"))
					{
						if (text == "max-size")
						{
							string text2 = words[i - 1];
							text2 = text2.Replace("m", string.Empty);
							float num;
							if (float.TryParse(text2, out num))
							{
								otherThingDestruction.maxThingSize = Mathf.Clamp(num, 0.001f, 10000f);
							}
						}
					}
					else
					{
						string text3 = words[i - 1];
						text3 = text3.Replace("m", string.Empty);
						float num2;
						if (float.TryParse(text3, out num2))
						{
							otherThingDestruction.radius = Mathf.Clamp(num2, 0.001f, 10000f);
						}
					}
				}
			}
		}
		return otherThingDestruction;
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x000C5C70 File Offset: 0x000C4070
	private static Sound GetSound(string data)
	{
		Sound sound = new Sound();
		string[] array = Misc.Split(data, " with ", StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 2)
		{
			sound.name = array[0];
			string[] array2 = Misc.Split(array[1], " ", StringSplitOptions.RemoveEmptyEntries);
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				if (text == null)
				{
					goto IL_207;
				}
				if (BehaviorScriptParser.<>f__switch$map37 == null)
				{
					BehaviorScriptParser.<>f__switch$map37 = new Dictionary<string, int>(12)
					{
						{ "very-low-pitch", 0 },
						{ "low-pitch", 1 },
						{ "high-pitch", 2 },
						{ "very-high-pitch", 3 },
						{ "varied-pitch", 4 },
						{ "very-varied-pitch", 5 },
						{ "echo", 6 },
						{ "low-pass", 7 },
						{ "high-pass", 8 },
						{ "stretch", 9 },
						{ "reversal", 10 },
						{ "surround", 11 }
					};
				}
				int num;
				if (!BehaviorScriptParser.<>f__switch$map37.TryGetValue(text, out num))
				{
					goto IL_207;
				}
				switch (num)
				{
				case 0:
					sound.pitch = 0.5f;
					break;
				case 1:
					sound.pitch = 0.75f;
					break;
				case 2:
					sound.pitch = (SoundLibraryManager.defaultPitch + SoundLibraryManager.maxPitch) * 0.5f;
					break;
				case 3:
					sound.pitch = SoundLibraryManager.maxPitch;
					break;
				case 4:
					sound.pitchVariance = 0.1f;
					break;
				case 5:
					sound.pitchVariance = 0.3f;
					break;
				case 6:
					sound.echo = true;
					break;
				case 7:
					sound.lowPass = true;
					break;
				case 8:
					sound.highPass = true;
					break;
				case 9:
					sound.stretch = true;
					break;
				case 10:
					sound.reverse = true;
					break;
				case 11:
					sound.surround = true;
					break;
				case 12:
					goto IL_207;
				default:
					goto IL_207;
				}
				IL_447:
				i++;
				continue;
				IL_207:
				string text2 = ((i >= array2.Length - 1) ? null : array2[i + 1]);
				if (text2 == null)
				{
					goto IL_3F3;
				}
				if (BehaviorScriptParser.<>f__switch$map36 == null)
				{
					BehaviorScriptParser.<>f__switch$map36 = new Dictionary<string, int>(8)
					{
						{ "repeat", 0 },
						{ "repeats", 0 },
						{ "octave", 1 },
						{ "octaves", 1 },
						{ "delay", 2 },
						{ "skip", 3 },
						{ "duration", 4 },
						{ "volume", 5 }
					};
				}
				if (!BehaviorScriptParser.<>f__switch$map36.TryGetValue(text2, out num))
				{
					goto IL_3F3;
				}
				switch (num)
				{
				case 0:
				{
					int num2;
					if (int.TryParse(text, out num2))
					{
						sound.repeatCount = Mathf.Clamp(num2, 0, 50);
					}
					break;
				}
				case 1:
				{
					float num3;
					if (float.TryParse(text, out num3))
					{
						sound.pitch = Misc.AdjustPitchInOctaves(num3);
						sound.pitch = Mathf.Clamp(sound.pitch, 1E-05f, 100f);
					}
					break;
				}
				case 2:
				{
					text = text.Replace("s", string.Empty);
					float num4;
					if (float.TryParse(text, out num4))
					{
						sound.secondsDelay = Mathf.Clamp(num4, 0.001f, 30f);
					}
					break;
				}
				case 3:
				{
					text = text.Replace("s", string.Empty);
					float num5;
					if (float.TryParse(text, out num5))
					{
						sound.secondsToSkip = Mathf.Clamp(num5, 0.001f, 30f);
					}
					break;
				}
				case 4:
				{
					text = text.Replace("s", string.Empty);
					float num6;
					if (float.TryParse(text, out num6))
					{
						sound.secondsDuration = Mathf.Clamp(num6, 0.001f, 60f);
					}
					break;
				}
				case 5:
					goto IL_3F3;
				default:
					goto IL_3F3;
				}
				IL_442:
				goto IL_447;
				IL_3F3:
				string text3 = text.Replace("%", string.Empty);
				float num7;
				if (float.TryParse(text3, out num7))
				{
					num7 = Mathf.Clamp(num7 / 100f, 0.001f, 5f);
					sound.volume *= num7;
				}
				goto IL_442;
			}
		}
		else
		{
			sound.name = data;
		}
		return sound;
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x000C60E0 File Offset: 0x000C44E0
	private static VoiceProperties GetVoicePropertiesFromData(string data)
	{
		VoiceProperties voiceProperties = null;
		if (data != string.Empty)
		{
			voiceProperties = new VoiceProperties();
			string[] array = Misc.Split(data, " ", StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string text2 = ((i >= array.Length - 1) ? null : array[i + 1]);
				if (text == "male")
				{
					voiceProperties.gender = VoiceProperties.Gender.Male;
				}
				else if (text == "female")
				{
					voiceProperties.gender = VoiceProperties.Gender.Female;
				}
				else
				{
					if (text2 != null)
					{
						if (text2 == "pitch")
						{
							float num;
							if (float.TryParse(text, out num))
							{
								voiceProperties.pitch = (int)Mathf.Clamp(num, -10f, 10f);
							}
							goto IL_13F;
						}
						if (text2 == "speed")
						{
							float num2;
							if (float.TryParse(text, out num2))
							{
								voiceProperties.speed = (int)Mathf.Clamp(num2, -10f, 10f);
							}
							goto IL_13F;
						}
					}
					string text3 = text.Replace("%", string.Empty);
					float num3;
					if (float.TryParse(text3, out num3))
					{
						voiceProperties.volume = (int)Mathf.Clamp(num3, 0f, 200f);
					}
				}
				IL_13F:;
			}
		}
		return voiceProperties;
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x000C623A File Offset: 0x000C463A
	private static void InitAreaRightsIfNeeded(StateListener listener)
	{
		if (listener.rights == null)
		{
			listener.rights = new AreaRights();
			listener.rights.SetAllToNull();
		}
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x000C625D File Offset: 0x000C465D
	private static void InitDesktopModeSettingsIfNeeded(StateListener listener)
	{
		if (listener.desktopModeSettings == null)
		{
			listener.desktopModeSettings = new DesktopModeSettings();
		}
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x000C6278 File Offset: 0x000C4678
	private static float ExtractDegreesValue(ref string stringValue)
	{
		float num = 0f;
		for (int i = -360; i <= 360; i += 45)
		{
			string text = " at " + i.ToString() + " degrees";
			if (stringValue.IndexOf(text) >= 0)
			{
				stringValue = stringValue.Replace(text, " ");
				stringValue = stringValue.Replace("  ", " ");
				stringValue = stringValue.Trim();
				num = (float)i;
				if (num == 360f)
				{
					num = 0f;
				}
				break;
			}
		}
		return num;
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x000C6318 File Offset: 0x000C4718
	private static float GetCreationPartChangeFloat(string stringValue)
	{
		float num = 0f;
		float num2;
		if (float.TryParse(stringValue, out num2) && num2 >= -1000f && num2 <= 1000f)
		{
			num = num2;
		}
		return num;
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x000C6354 File Offset: 0x000C4754
	private static string ReplaceIncludedNamesWithIds(Dictionary<string, string> includedNameIds, string text)
	{
		char[] array = new char[] { ' ' };
		foreach (KeyValuePair<string, string> keyValuePair in includedNameIds.OrderBy((KeyValuePair<string, string> i) => -i.Key.Length))
		{
			string text2 = text.Replace(keyValuePair.Key, keyValuePair.Value);
			if (text2 != text)
			{
				string[] array2 = text2.Split(array, StringSplitOptions.RemoveEmptyEntries);
				if ((array2.Length == 4 && array2[2] == "with") || array2.Length == 2)
				{
					text = text2;
					break;
				}
			}
		}
		return text;
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x000C642C File Offset: 0x000C482C
	private static string ReplaceIncludedNamesWithIdsInData(Dictionary<string, string> includedNameIds, string data)
	{
		foreach (KeyValuePair<string, string> keyValuePair in includedNameIds.OrderBy((KeyValuePair<string, string> i) => -i.Key.Length))
		{
			data = data.Replace(keyValuePair.Key, keyValuePair.Value);
		}
		return data;
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x000C64B4 File Offset: 0x000C48B4
	public static string NormalizeLine(string s, Thing parentThing, bool forUseInAutoCompletion = false)
	{
		if (!s.Contains("[youtube:") && !s.Contains("show web"))
		{
			s = s.ToLower();
		}
		s = s.Trim();
		s = s.Replace(", then ", ", ");
		s = s.Replace(",then ", ",");
		s = s.Replace("turn all parts", "all parts turn");
		s = s.Replace("when any part touched ", "when any part touches ");
		s = s.Replace("when any state touched ", "when any state touches ");
		s = s.Replace(" send nearby one ", " send one nearby ");
		s = s.Replace(" set visibility to ", " set area visibility to ");
		s = s.Replace("dialog me opened", "dialog own profile opened");
		s = s.Replace("dialog me closed", "dialog own profile closed");
		s = s.Replace("body dialog board", "body dialog forum");
		s = s.Replace("type \"when touched", "type \"when _touched");
		s = s.Replace("when touched ", "when touches ");
		s = s.Replace("type \"when _touched", "type \"when touched");
		s = s.Replace("when hit ", "when hitting ");
		s = s.Replace("when any part hit ", "when any part hitting ");
		s = s.Replace("send nearby to area ", "send nearby to ");
		s = BehaviorScriptParser.GetAllWithUnderlines(s, forUseInAutoCompletion, parentThing.version);
		if (s.Contains(" then say "))
		{
			s = s.Replace(",", "[comma]");
		}
		return s;
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x000C6644 File Offset: 0x000C4A44
	private static string GetAllWithUnderlines(string s, bool forUseInAutoCompletion, int thingVersion)
	{
		if (forUseInAutoCompletion)
		{
			s = BehaviorScriptParser.GetWithUnderlines(s, "do creation part material");
			s = BehaviorScriptParser.GetWithUnderlines(s, "do all creation parts material");
			s = BehaviorScriptParser.GetWithUnderlines(s, "when is");
		}
		for (int i = 0; i < BehaviorScriptParser.stringsToGetWithUnderlines.Length; i++)
		{
			s = BehaviorScriptParser.GetWithUnderlines(s, BehaviorScriptParser.stringsToGetWithUnderlines[i]);
		}
		if (thingVersion >= 8)
		{
			s = BehaviorScriptParser.GetWithUnderlines(s, "tell in front");
			s = BehaviorScriptParser.GetWithUnderlines(s, "tell first in front");
		}
		if (forUseInAutoCompletion)
		{
			s = BehaviorScriptParser.GetWithUnderlines(s, "stop all parts");
			s = BehaviorScriptParser.GetWithUnderlines(s, "all parts");
			s = BehaviorScriptParser.GetWithUnderlines(s, "any part");
			s = BehaviorScriptParser.GetWithUnderlines(s, "set light");
			s = BehaviorScriptParser.GetWithUnderlines(s, "change head");
		}
		return s;
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x000C6710 File Offset: 0x000C4B10
	private static string EscapeCommaIfInQuotes(string s)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		foreach (char c in s)
		{
			if (c == '"')
			{
				flag = !flag;
			}
			if (flag && c == ',')
			{
				stringBuilder.Append("[comma]");
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x000C678C File Offset: 0x000C4B8C
	public static string GetWithUnderlines(string s, string part)
	{
		string text = part.Replace(" ", "_");
		text = text.Replace("-", "_");
		text = text.Replace("'", string.Empty);
		return s.Replace(part, text);
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x000C67D4 File Offset: 0x000C4BD4
	private static string RemoveSpacesInDialogNames(string s)
	{
		IEnumerator enumerator = Enum.GetValues(typeof(DialogType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				DialogType dialogType = (DialogType)obj;
				string text = dialogType.ToString().ToLower();
				string text2 = Misc.CamelCaseToSpaceSeparated(dialogType.ToString());
				if (text != text2)
				{
					s = s.Replace(text2, text);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		return s;
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x000C687C File Offset: 0x000C4C7C
	public static bool IsPlaceholderContent(string s)
	{
		s = s.ToLower();
		bool flag = false;
		foreach (string text in BehaviorScriptParser.textPlaceholdersStartsWith)
		{
			if (s.StartsWith(text))
			{
				flag = true;
				break;
			}
		}
		return flag || BehaviorScriptParser.textPlaceholdersFull.Contains(s);
	}

	// Token: 0x0400140E RID: 5134
	public const float minStateSeconds = 0.05f;

	// Token: 0x0400140F RID: 5135
	public const float maxStateSeconds = 30f;

	// Token: 0x04001410 RID: 5136
	public const float magnitudeConsideredFast = 1.15f;

	// Token: 0x04001411 RID: 5137
	private const float minRelativeSoundVolume = 0.001f;

	// Token: 0x04001412 RID: 5138
	private const float maxRelativeSoundVolume = 5f;

	// Token: 0x04001413 RID: 5139
	private const string commaKey = "[comma]";

	// Token: 0x04001414 RID: 5140
	public const float maxTrailDurationSeconds = 60f;

	// Token: 0x04001415 RID: 5141
	public const string areaVariablePrefix = "area.";

	// Token: 0x04001416 RID: 5142
	public const string personVariablePrefix = "person.";

	// Token: 0x04001417 RID: 5143
	private static string[] stringsToGetWithUnderlines = new string[]
	{
		"any part touches", "any part consumed", "any part hitting", "any part blown at", "any part pointed at", "any part looked at", "play url", "trigger let go", "neared by", "tell nearby",
		"tell any web", "tell any", "tell body", "tell first of any", "tell web", "told by nearby", "told by any", "told by body", "send nearby to", "send nearby onto",
		"send one nearby to", "send one nearby onto", "send all to", "send all onto", "call me", "talked to", "talked from", "pointed at", "looked at", "turned around",
		"high speed", "end loop", "let go", "blown at", "walked into", "someone new in vicinity", "someone in vicinity", "turn thing", "turn sub-thing", "stop all parts face someone",
		"stop all parts face up", "stop all parts face empty hand", "stop all parts face nearest", "stop all parts face view", "all parts face someone else", "all parts face someone", "all parts face up", "all parts face empty hand while held", "all parts face empty hand", "all parts face nearest",
		"all parts face view", "become untweened", "become unsoftened", "become soft start", "become soft end", "become stopped", "emit gravity-free", "destroy all parts", "destroy nearby", "give haptic feedback",
		"propel forward", "rotate forward", "disallow any person size", "disallow emitted climbing", "disallow emitted transporting", "disallow moving through obstacles", "disallow vision in obstacles", "disallow invisibility", "disallow highlighting", "disallow amplified speech",
		"disallow any destruction", "disallow web browsing", "disallow untargeted attract and repel", "disallow build animations", "allow any person size", "allow emitted climbing", "allow emitted transporting", "allow moving through obstacles", "allow vision in obstacles", "allow invisibility",
		"allow highlighting", "allow amplified speech", "allow any destruction", "allow web browsing", "allow untargeted attract and repel", "allow build animations", "show slideshow controls", "show camera controls", "show video controls", "show name tags",
		"show video", "show board", "show thread", "show areas", "show web", "show inventory", "show chat keyboard", "show line", "go to inventory page", "touch ends",
		"set light intensity", "set light range", "set light cone size", "set constant rotation to", "add crumbles for all parts", "set snap angles to", "set run speed", "set jump speed", "set slidiness", "add crumbles",
		"do creation part", "do all creation parts", "material transparent glossy metallic", "material very transparent glossy", "material slightly transparent", "material transparent glossy", "material very transparent", "material bright metallic", "material very metallic", "material dark metallic",
		"material transparent texture", "material transparent", "material metallic", "material default", "material plastic", "material unshiny", "material glow", "fire with alarm", "filling with air", "set speed",
		"add speed", "multiply speed", "change head to", "change heads to", "align to surface", "resize nearby to", "stream to", "stream stop", "set voice", "hears anywhere",
		"play track loop", "play track", "set camera position to", "set camera following to", "set camera", "insert state", "remove state", "when any state", "set gravity to", "reset area",
		"reset persons", "reset position", "reset rotation", "reset body", "reset legs to default", "reset legs to body default", "destroyed restores", "trail start", "trail end", "set area visibility to",
		"enable setting", "disable setting", "set person as authority", "set quest achieve", "set quest unachieve", "set quest remove", "set quest", "set attract", "set repel"
	};

	// Token: 0x04001418 RID: 5144
	private static string[] textPlaceholdersFull = new string[]
	{
		"year", "month", "day", "hour", "hour 12", "minute", "second", "millisecond", "local hour", "local hour 12",
		"month unpadded", "day unpadded", "hour unpadded", "hour 12 unpadded", "local hour 12 unpadded", "closest person", "closest held", "area name", "thing name", "x",
		"y", "z", "people names", "people count", "typed", "area values", "thing values", "person values", "proximity", "url",
		"person", "own person"
	};

	// Token: 0x04001419 RID: 5145
	private static string[] textPlaceholdersStartsWith = new string[] { "area.", "person." };

	// Token: 0x0400141A RID: 5146
	private static string[] turnCommands = new string[] { "on", "off", "visible", "invisible", "collidable", "uncollidable" };
}
