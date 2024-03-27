using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public static class CreationHelper
{
	// Token: 0x060007FA RID: 2042 RVA: 0x0002C9EC File Offset: 0x0002ADEC
	public static int GetTextureIndex()
	{
		int num = -1;
		MaterialTab materialTab = CreationHelper.currentMaterialTab;
		if (materialTab != MaterialTab.texture1)
		{
			if (materialTab == MaterialTab.texture2)
			{
				num = 1;
			}
		}
		else
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x040005D6 RID: 1494
	public static GameObject thingBeingEdited;

	// Token: 0x040005D7 RID: 1495
	public static GameObject thingPartWhoseStatesAreEdited;

	// Token: 0x040005D8 RID: 1496
	public static GameObject thingThatWasClonedFrom;

	// Token: 0x040005D9 RID: 1497
	public static string thingThatWasClonedFromIdIfRelevant = null;

	// Token: 0x040005DA RID: 1498
	public static int currentThingPartStateLinesPage = 0;

	// Token: 0x040005DB RID: 1499
	public static string thingDefaultName = "thing";

	// Token: 0x040005DC RID: 1500
	public static bool replaceInstancesInAreaOneTime = false;

	// Token: 0x040005DD RID: 1501
	public static bool didSeeAlwaysMergePartsConfirm = false;

	// Token: 0x040005DE RID: 1502
	public static TransformClipboard transformClipboard = new TransformClipboard();

	// Token: 0x040005DF RID: 1503
	public static Texture2D referenceImage = null;

	// Token: 0x040005E0 RID: 1504
	public static GameObject referenceObject = null;

	// Token: 0x040005E1 RID: 1505
	public static Transform lastColorPickTransform;

	// Token: 0x040005E2 RID: 1506
	public static Vector3 lastColorPickTransformOriginalPosition;

	// Token: 0x040005E3 RID: 1507
	public static Transform lastExpanderColorPickTransform;

	// Token: 0x040005E4 RID: 1508
	public static Vector3 lastExpanderColorPickTransformOriginalPosition;

	// Token: 0x040005E5 RID: 1509
	public static int shapesTab = 0;

	// Token: 0x040005E6 RID: 1510
	public const int maxShapesTab = 10;

	// Token: 0x040005E7 RID: 1511
	public const float thingMassDefault = 1f;

	// Token: 0x040005E8 RID: 1512
	public const float thingDragDefault = 0f;

	// Token: 0x040005E9 RID: 1513
	public const float thingAngularDragDefault = 0.05f;

	// Token: 0x040005EA RID: 1514
	public static float? customSnapAngles = null;

	// Token: 0x040005EB RID: 1515
	public const int maxThingPartCountPerCreation = 1000;

	// Token: 0x040005EC RID: 1516
	public const int thingJsonMaxLength = 300000;

	// Token: 0x040005ED RID: 1517
	public const int thingDescriptionMaxLength = 225;

	// Token: 0x040005EE RID: 1518
	public const int maxIncludedSubThings = 1000;

	// Token: 0x040005EF RID: 1519
	public const int maxPlacedSubThings = 100;

	// Token: 0x040005F0 RID: 1520
	public const int maxPlacedSubThingsToRecreate = 20;

	// Token: 0x040005F1 RID: 1521
	public static bool alreadyExceededMaxThingPartCountOnCloning = false;

	// Token: 0x040005F2 RID: 1522
	public static bool showDialogShapesTab = false;

	// Token: 0x040005F3 RID: 1523
	public static ThingPartStatesCopy statesCopy = null;

	// Token: 0x040005F4 RID: 1524
	public const int maxThingPartStates = 50;

	// Token: 0x040005F5 RID: 1525
	public const int maxBehaviorScriptLines = 100;

	// Token: 0x040005F6 RID: 1526
	public const int maxBehaviorScriptLineLength = 10000;

	// Token: 0x040005F7 RID: 1527
	public const string thingPartDefaultText = "ABC";

	// Token: 0x040005F8 RID: 1528
	public const int thingPartTextMaxLength = 10000;

	// Token: 0x040005F9 RID: 1529
	public static Dictionary<MaterialTab, Color> currentColor = new Dictionary<MaterialTab, Color>
	{
		{
			MaterialTab.material,
			Color.cyan
		},
		{
			MaterialTab.texture1,
			Color.black
		},
		{
			MaterialTab.texture2,
			Color.black
		},
		{
			MaterialTab.particleSystem,
			Color.white
		}
	};

	// Token: 0x040005FA RID: 1530
	public static MaterialTab currentMaterialTab = MaterialTab.material;

	// Token: 0x040005FB RID: 1531
	public static Dictionary<MaterialTab, Color> currentBaseColor = new Dictionary<MaterialTab, Color>
	{
		{
			MaterialTab.material,
			Color.cyan
		},
		{
			MaterialTab.texture1,
			Color.black
		},
		{
			MaterialTab.texture2,
			Color.black
		},
		{
			MaterialTab.particleSystem,
			Color.white
		}
	};

	// Token: 0x040005FC RID: 1532
	public static TextureProperty[] currentTextureProperty = new TextureProperty[2];

	// Token: 0x040005FD RID: 1533
	public static ParticleSystemProperty currentParticleSystemProperty = ParticleSystemProperty.Amount;

	// Token: 0x040005FE RID: 1534
	public static Color lastHueColor = Color.cyan;

	// Token: 0x040005FF RID: 1535
	public static Material lastMaterial;

	// Token: 0x04000600 RID: 1536
	public static string lastScriptLineEntered = string.Empty;

	// Token: 0x04000601 RID: 1537
	public static bool wasInAreaEditingMode = false;

	// Token: 0x04000602 RID: 1538
	public static MaterialTypes materialType = MaterialTypes.None;

	// Token: 0x04000603 RID: 1539
	public static ParticleSystemType particleSystemType = ParticleSystemType.None;

	// Token: 0x04000604 RID: 1540
	public static TextureType[] textureTypes = new TextureType[2];

	// Token: 0x04000605 RID: 1541
	public const float particleSystemPropertyDefault = 0.25f;

	// Token: 0x04000606 RID: 1542
	public static DialogType dialogBeforeBrushWasPicked = DialogType.None;

	// Token: 0x04000607 RID: 1543
	public static Vector3 thingPartPickupPosition = Vector3.zero;
}
