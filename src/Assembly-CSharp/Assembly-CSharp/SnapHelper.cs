using System;
using UnityEngine;

// Token: 0x02000269 RID: 617
public class SnapHelper
{
	// Token: 0x0600169F RID: 5791 RVA: 0x000CB2A0 File Offset: 0x000C96A0
	public static void SnapAllNeeded(Thing thing, GameObject thisObject, Transform handDotTransform, Vector3 myPickUpPosition, Vector3 objectPickUpPosition, Vector3 myPickUpAngles, Vector3 objectPickUpAngles)
	{
		thisObject.transform.localEulerAngles = SnapHelper.SnapAngles(thing, thisObject, handDotTransform, myPickUpPosition, objectPickUpPosition, myPickUpAngles, objectPickUpAngles);
		if (!Our.disableAllThingSnapping)
		{
			if (thing.doLockPosition)
			{
				thisObject.transform.position = objectPickUpPosition;
			}
			else if (thing.doSnapPosition || Our.doSnapThingPosition)
			{
				thisObject.transform.position = SnapHelper.SnapPositionAlongAxis(thisObject, objectPickUpPosition);
			}
		}
		bool flag = thisObject.GetComponent<Thing>() != null;
		bool flag2 = !flag && thisObject.GetComponent<ThingPart>() != null;
		if ((flag && Our.snapThingsToGrid) || (flag2 && thing.snapAllPartsToGrid))
		{
			thisObject.transform.position = SnapHelper.SnapToGrid(thisObject.transform.position, true);
		}
	}

	// Token: 0x060016A0 RID: 5792 RVA: 0x000CB378 File Offset: 0x000C9778
	public static Vector3 SnapAngles(Thing thing, GameObject thisObject, Transform handDotTransform, Vector3 myPickUpPosition, Vector3 objectPickUpPosition, Vector3 myPickUpAngles, Vector3 objectPickUpAngles)
	{
		Vector3 vector = thisObject.transform.localEulerAngles;
		if (SnapHelper.SomeAngleSnapIsOn(thing))
		{
			if (thing.doLockAngles)
			{
				vector = objectPickUpAngles;
			}
			else
			{
				float num = ((!thing.doSoftSnapAngles) ? 90f : 22.5f);
				float? customSnapAngles = CreationHelper.customSnapAngles;
				if (customSnapAngles != null)
				{
					float? customSnapAngles2 = CreationHelper.customSnapAngles;
					num = customSnapAngles2.Value;
				}
				float num2 = 360f + num;
				float num3 = num / 2f;
				for (float num4 = -num2 + num; num4 < num2; num4 += num)
				{
					if (Mathf.Abs(vector.x - num4) <= num3)
					{
						vector.x = num4;
					}
					if (Mathf.Abs(vector.y - num4) <= num3)
					{
						vector.y = num4;
					}
					if (Mathf.Abs(vector.z - num4) <= num3)
					{
						vector.z = num4;
					}
				}
			}
			if (!(myPickUpPosition == Vector3.zero) || !(objectPickUpPosition == Vector3.zero))
			{
				Vector3 vector2 = handDotTransform.position - myPickUpPosition;
				thisObject.transform.position = objectPickUpPosition + vector2;
			}
		}
		return vector;
	}

	// Token: 0x060016A1 RID: 5793 RVA: 0x000CB4B8 File Offset: 0x000C98B8
	public static bool SomeAngleSnapIsOn(Thing thing)
	{
		bool flag;
		if (!Our.disableAllThingSnapping)
		{
			if (!thing.doSnapAngles && !thing.doSoftSnapAngles && !thing.doLockAngles && !Our.doSnapThingAngles)
			{
				float? customSnapAngles = CreationHelper.customSnapAngles;
				flag = customSnapAngles != null;
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x060016A2 RID: 5794 RVA: 0x000CB510 File Offset: 0x000C9910
	public static Vector3 SnapPositionAlongAxis(GameObject thisObject, Vector3 objectPickUpPosition)
	{
		Vector3 vector = thisObject.transform.position;
		if (objectPickUpPosition != Vector3.zero)
		{
			vector = SnapHelper.SnapPositionAlongAxis(vector, objectPickUpPosition);
		}
		return vector;
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x000CB544 File Offset: 0x000C9944
	public static Vector3 SnapPositionAlongAxis(Vector3 position, Vector3 pickUpPosition)
	{
		Vector3 vector = pickUpPosition - position;
		vector = new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
		if (vector.x >= vector.y && vector.x >= vector.z)
		{
			position.y = pickUpPosition.y;
			position.z = pickUpPosition.z;
		}
		else if (vector.y >= vector.x && vector.y >= vector.z)
		{
			position.x = pickUpPosition.x;
			position.z = pickUpPosition.z;
		}
		else
		{
			position.x = pickUpPosition.x;
			position.y = pickUpPosition.y;
		}
		return position;
	}

	// Token: 0x060016A4 RID: 5796 RVA: 0x000CB630 File Offset: 0x000C9A30
	public static Vector3 SnapPositionAlongAxisInDirection(GameObject thisObject, GameObject referenceObject)
	{
		Vector3 position = thisObject.transform.position;
		Vector3 position2 = referenceObject.transform.position;
		Transform transform = referenceObject.transform;
		Vector3[] array = new Vector3[3];
		Vector3 vector = position - position2;
		int num = 0;
		array[num++] = position2 + transform.right * Vector3.Dot(vector, transform.right);
		array[num++] = position2 + transform.forward * Vector3.Dot(vector, transform.forward);
		array[num++] = position2 + transform.up * Vector3.Dot(vector, transform.up);
		float? num2 = null;
		int num3 = -1;
		for (int i = 0; i < array.Length; i++)
		{
			float num4 = Vector3.Distance(position, array[i]);
			if (num2 == null || num4 < num2)
			{
				num3 = i;
				num2 = new float?(num4);
			}
		}
		return array[num3];
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x000CB784 File Offset: 0x000C9B84
	public static Vector3 SnapToGrid(Vector3 point, bool allowHalfPoints = false)
	{
		float num = Our.gridSize;
		if (allowHalfPoints)
		{
			num *= 0.5f;
		}
		point.x = SnapHelper.SnapValueToGrid(point.x, num);
		point.y = SnapHelper.SnapValueToGrid(point.y, num);
		point.z = SnapHelper.SnapValueToGrid(point.z, num);
		return point;
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x000CB7E2 File Offset: 0x000C9BE2
	public static float SnapValueToGrid(float value, float gridSize)
	{
		if (value != 0f)
		{
			value = Mathf.Round(value / gridSize) * gridSize;
		}
		return value;
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x000CB7FC File Offset: 0x000C9BFC
	public static void SnapToAngleLockerIfNeeded(ThingPart thingPart)
	{
		if (!thingPart.isAngleLocker)
		{
			Component[] componentsInChildren = CreationHelper.thingBeingEdited.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart2 in componentsInChildren)
			{
				if (thingPart2.isAngleLocker)
				{
					thingPart.transform.eulerAngles = thingPart2.transform.eulerAngles;
					break;
				}
			}
		}
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x000CB864 File Offset: 0x000C9C64
	public static void SnapToPositionLockerIfNeeded(ThingPart thingPart)
	{
		if (!thingPart.isPositionLocker)
		{
			Component[] componentsInChildren = CreationHelper.thingBeingEdited.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart2 in componentsInChildren)
			{
				if (thingPart2.isPositionLocker)
				{
					thingPart.transform.position = SnapHelper.SnapPositionAlongAxisInDirection(thingPart.gameObject, thingPart2.gameObject);
					break;
				}
			}
		}
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x000CB8D4 File Offset: 0x000C9CD4
	public static void PositionSnappedInFrontOfUs(Transform thisTransform, Transform reference)
	{
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		Vector3 vector = @object.transform.position + @object.transform.forward * 0.5f;
		vector.y = reference.position.y;
		thisTransform.position = vector;
		thisTransform.rotation = @object.transform.rotation;
		thisTransform.transform.Rotate(Vector3.forward * 90f);
		Vector3 localEulerAngles = thisTransform.localEulerAngles;
		localEulerAngles.x = 0f;
		localEulerAngles.z = 0f;
		for (float num = -360f; num < 450f; num += 90f)
		{
			if (Mathf.Abs(localEulerAngles.x - num) <= 45f)
			{
				localEulerAngles.x = num;
			}
			if (Mathf.Abs(localEulerAngles.y - num) <= 45f)
			{
				localEulerAngles.y = num;
			}
			if (Mathf.Abs(localEulerAngles.z - num) <= 45f)
			{
				localEulerAngles.z = num;
			}
		}
		thisTransform.localEulerAngles = localEulerAngles;
	}
}
