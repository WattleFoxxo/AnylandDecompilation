using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200026C RID: 620
public class StateListener
{
	// Token: 0x04001475 RID: 5237
	public StateListener.EventType eventType;

	// Token: 0x04001476 RID: 5238
	public bool isForAnyState;

	// Token: 0x04001477 RID: 5239
	public int setState = -1;

	// Token: 0x04001478 RID: 5240
	public RelativeStateTarget? setStateRelative;

	// Token: 0x04001479 RID: 5241
	public float setStateSeconds = 0.01f;

	// Token: 0x0400147A RID: 5242
	public TweenType tweenType = TweenType.EaseInOut;

	// Token: 0x0400147B RID: 5243
	public int curveViaState = -1;

	// Token: 0x0400147C RID: 5244
	public string whenData;

	// Token: 0x0400147D RID: 5245
	public string whenIsData;

	// Token: 0x0400147E RID: 5246
	public string thenData;

	// Token: 0x0400147F RID: 5247
	public string callMeThisName;

	// Token: 0x04001480 RID: 5248
	public Vector3 pushToLocationInArea;

	// Token: 0x04001481 RID: 5249
	public bool doHapticPulse;

	// Token: 0x04001482 RID: 5250
	public ThingDestruction destroyThingWeArePartOf;

	// Token: 0x04001483 RID: 5251
	public OtherThingDestruction destroyOtherThings;

	// Token: 0x04001484 RID: 5252
	public List<Sound> sounds;

	// Token: 0x04001485 RID: 5253
	public string soundTrackData;

	// Token: 0x04001486 RID: 5254
	public string startLoopSoundName;

	// Token: 0x04001487 RID: 5255
	public bool doEndLoopSound;

	// Token: 0x04001488 RID: 5256
	public float loopVolume;

	// Token: 0x04001489 RID: 5257
	public float loopSpatialBlend;

	// Token: 0x0400148A RID: 5258
	public RotateThingSettings rotateThingSettings;

	// Token: 0x0400148B RID: 5259
	public string transportToArea;

	// Token: 0x0400148C RID: 5260
	public string transportOntoThing;

	// Token: 0x0400148D RID: 5261
	public bool transportMultiplePeople;

	// Token: 0x0400148E RID: 5262
	public bool transportNearbyOnly;

	// Token: 0x0400148F RID: 5263
	public float rotationAfterTransport;

	// Token: 0x04001490 RID: 5264
	public string transportViaArea;

	// Token: 0x04001491 RID: 5265
	public float transportViaAreaSeconds;

	// Token: 0x04001492 RID: 5266
	public List<KeyValuePair<TellType, string>> tells;

	// Token: 0x04001493 RID: 5267
	public string emitId;

	// Token: 0x04001494 RID: 5268
	public float emitVelocityPercent;

	// Token: 0x04001495 RID: 5269
	public bool emitIsGravityFree;

	// Token: 0x04001496 RID: 5270
	public float? propelForwardPercent;

	// Token: 0x04001497 RID: 5271
	public float? rotateForwardPercent;

	// Token: 0x04001498 RID: 5272
	public bool addCrumbles;

	// Token: 0x04001499 RID: 5273
	public bool addEffectIsForAllParts;

	// Token: 0x0400149A RID: 5274
	public AreaRights rights;

	// Token: 0x0400149B RID: 5275
	public string creationPartChangeMode;

	// Token: 0x0400149C RID: 5276
	public float[] creationPartChangeValues;

	// Token: 0x0400149D RID: 5277
	public bool creationPartChangeIsForAll;

	// Token: 0x0400149E RID: 5278
	public bool creationPartChangeIsLocal;

	// Token: 0x0400149F RID: 5279
	public bool creationPartChangeIsRandom;

	// Token: 0x040014A0 RID: 5280
	public float setLightIntensity = -1f;

	// Token: 0x040014A1 RID: 5281
	public float setLightRange = -1f;

	// Token: 0x040014A2 RID: 5282
	public float setLightConeSize = -1f;

	// Token: 0x040014A3 RID: 5283
	public string doTypeText = string.Empty;

	// Token: 0x040014A4 RID: 5284
	public bool pauseLerping;

	// Token: 0x040014A5 RID: 5285
	public DialogType? showDialog;

	// Token: 0x040014A6 RID: 5286
	public string showData;

	// Token: 0x040014A7 RID: 5287
	public Vector3? velocityMultiplier;

	// Token: 0x040014A8 RID: 5288
	public Vector3? velocitySetter;

	// Token: 0x040014A9 RID: 5289
	public Vector3? forceAdder;

	// Token: 0x040014AA RID: 5290
	public List<string> variableOperations;

	// Token: 0x040014AB RID: 5291
	public string attachThingIdAsHead;

	// Token: 0x040014AC RID: 5292
	public bool attachToMultiplePeople;

	// Token: 0x040014AD RID: 5293
	public bool letGo;

	// Token: 0x040014AE RID: 5294
	public int goToInventoryPage;

	// Token: 0x040014AF RID: 5295
	public float? resizeNearby;

	// Token: 0x040014B0 RID: 5296
	public bool? streamMyCameraView;

	// Token: 0x040014B1 RID: 5297
	public string streamTargetName;

	// Token: 0x040014B2 RID: 5298
	public float? showNameTagsAgainSeconds;

	// Token: 0x040014B3 RID: 5299
	public string say;

	// Token: 0x040014B4 RID: 5300
	public VoiceProperties setVoiceProperties;

	// Token: 0x040014B5 RID: 5301
	public float? setCustomSnapAngles;

	// Token: 0x040014B6 RID: 5302
	public FollowerCameraPosition? setFollowerCameraPosition;

	// Token: 0x040014B7 RID: 5303
	public float? setFollowerCameraLerp;

	// Token: 0x040014B8 RID: 5304
	public Vector3? setGravity;

	// Token: 0x040014B9 RID: 5305
	public ResetSettings resetSettings;

	// Token: 0x040014BA RID: 5306
	public string setText;

	// Token: 0x040014BB RID: 5307
	public string turn;

	// Token: 0x040014BC RID: 5308
	public string turnThing;

	// Token: 0x040014BD RID: 5309
	public string turnSubThing;

	// Token: 0x040014BE RID: 5310
	public string turnSubThingName;

	// Token: 0x040014BF RID: 5311
	public string playVideoId;

	// Token: 0x040014C0 RID: 5312
	public float? playVideoVolume;

	// Token: 0x040014C1 RID: 5313
	public BrowserSettings browserSettings;

	// Token: 0x040014C2 RID: 5314
	public ProjectPartSettings projectPartSettings;

	// Token: 0x040014C3 RID: 5315
	public PartLineSettings partLineSettings;

	// Token: 0x040014C4 RID: 5316
	public PartTrailSettings partTrailSettings;

	// Token: 0x040014C5 RID: 5317
	public float? limitAreaVisibilityMeters;

	// Token: 0x040014C6 RID: 5318
	public Vector3? constantRotation;

	// Token: 0x040014C7 RID: 5319
	public Dictionary<Setting, bool> settings;

	// Token: 0x040014C8 RID: 5320
	public bool makePersonMasterClient;

	// Token: 0x040014C9 RID: 5321
	public QuestAction questAction;

	// Token: 0x040014CA RID: 5322
	public AttractThingsSettings attractThingsSettings;

	// Token: 0x040014CB RID: 5323
	public DesktopModeSettings desktopModeSettings;

	// Token: 0x0200026D RID: 621
	public enum EventType
	{
		// Token: 0x040014CD RID: 5325
		None,
		// Token: 0x040014CE RID: 5326
		OnStarts,
		// Token: 0x040014CF RID: 5327
		OnTouches,
		// Token: 0x040014D0 RID: 5328
		OnTouchEnds,
		// Token: 0x040014D1 RID: 5329
		OnTriggered,
		// Token: 0x040014D2 RID: 5330
		OnUntriggered,
		// Token: 0x040014D3 RID: 5331
		OnTold,
		// Token: 0x040014D4 RID: 5332
		OnToldByNearby,
		// Token: 0x040014D5 RID: 5333
		OnToldByAny,
		// Token: 0x040014D6 RID: 5334
		OnToldByBody,
		// Token: 0x040014D7 RID: 5335
		OnTaken,
		// Token: 0x040014D8 RID: 5336
		OnGrabbed,
		// Token: 0x040014D9 RID: 5337
		OnConsumed,
		// Token: 0x040014DA RID: 5338
		OnBlownAt,
		// Token: 0x040014DB RID: 5339
		OnTalkedFrom,
		// Token: 0x040014DC RID: 5340
		OnTalkedTo,
		// Token: 0x040014DD RID: 5341
		OnTurnedAround,
		// Token: 0x040014DE RID: 5342
		OnLetGo,
		// Token: 0x040014DF RID: 5343
		OnHighSpeed,
		// Token: 0x040014E0 RID: 5344
		OnGets,
		// Token: 0x040014E1 RID: 5345
		OnNeared,
		// Token: 0x040014E2 RID: 5346
		OnSomeoneInVicinity,
		// Token: 0x040014E3 RID: 5347
		OnSomeoneNewInVicinity,
		// Token: 0x040014E4 RID: 5348
		OnHitting,
		// Token: 0x040014E5 RID: 5349
		OnShaken,
		// Token: 0x040014E6 RID: 5350
		OnWalkedInto,
		// Token: 0x040014E7 RID: 5351
		OnPointedAt,
		// Token: 0x040014E8 RID: 5352
		OnLookedAt,
		// Token: 0x040014E9 RID: 5353
		OnRaised,
		// Token: 0x040014EA RID: 5354
		OnLowered,
		// Token: 0x040014EB RID: 5355
		OnHears,
		// Token: 0x040014EC RID: 5356
		OnHearsAnywhere,
		// Token: 0x040014ED RID: 5357
		OnJoystickControlled,
		// Token: 0x040014EE RID: 5358
		OnDestroyed,
		// Token: 0x040014EF RID: 5359
		OnDestroyedRestored,
		// Token: 0x040014F0 RID: 5360
		OnTyped,
		// Token: 0x040014F1 RID: 5361
		OnVariableChange,
		// Token: 0x040014F2 RID: 5362
		OnSettingEnabled,
		// Token: 0x040014F3 RID: 5363
		OnSettingDisabled,
		// Token: 0x040014F4 RID: 5364
		OnAnyPartTouches,
		// Token: 0x040014F5 RID: 5365
		OnAnyPartConsumed,
		// Token: 0x040014F6 RID: 5366
		OnAnyPartHitting,
		// Token: 0x040014F7 RID: 5367
		OnAnyPartBlownAt,
		// Token: 0x040014F8 RID: 5368
		OnAnyPartPointedAt,
		// Token: 0x040014F9 RID: 5369
		OnAnyPartLookedAt
	}
}
