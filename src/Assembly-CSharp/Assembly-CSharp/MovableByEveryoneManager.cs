using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class MovableByEveryoneManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06001185 RID: 4485 RVA: 0x00094DB2 File Offset: 0x000931B2
	// (set) Token: 0x06001186 RID: 4486 RVA: 0x00094DBA File Offset: 0x000931BA
	public ManagerStatus status { get; private set; }

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06001187 RID: 4487 RVA: 0x00094DC3 File Offset: 0x000931C3
	// (set) Token: 0x06001188 RID: 4488 RVA: 0x00094DCB File Offset: 0x000931CB
	public string failMessage { get; private set; }

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06001189 RID: 4489 RVA: 0x00094DD4 File Offset: 0x000931D4
	// (set) Token: 0x0600118A RID: 4490 RVA: 0x00094DDC File Offset: 0x000931DC
	public List<Line> debugLines { get; private set; }

	// Token: 0x0600118B RID: 4491 RVA: 0x00094DE5 File Offset: 0x000931E5
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x00094DF8 File Offset: 0x000931F8
	public void HandleInteraction(HandDot handDot, HandDot otherHandDot)
	{
		if (this.groupItems.Count == 0)
		{
			HandDot handDot2 = ((!handDot.IsDominantSide()) ? otherHandDot : handDot);
			Hand component = handDot2.transform.parent.GetComponent<Hand>();
			this.groupItems = this.GetMovableByEveryoneThingsNearLine(handDot.transform, otherHandDot.transform);
			if (this.groupItems.Count >= 1)
			{
				Transform transform = component.transform;
				foreach (Thing thing in this.groupItems)
				{
					thing.transform.parent = transform;
					ThingMovableByEveryone component2 = thing.GetComponent<ThingMovableByEveryone>();
					if (component2 != null)
					{
						component2.OnPickUp(true);
					}
				}
				Managers.personManager.DoInformOnMovableByEveryoneGroupTransforms(this.groupItems, component.side, MovableByEveryoneGroupInformType.PickUp);
				Managers.soundManager.Play("pickUp", handDot.transform.parent.position, 0.3f, false, false);
			}
		}
		else
		{
			this.HandleShuffle(handDot, otherHandDot);
		}
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x00094F28 File Offset: 0x00093328
	private void HandleShuffle(HandDot handDot, HandDot otherHandDot)
	{
		if (this.groupItems.Count >= 1)
		{
			float? num = this.lastShuffledTime;
			if (num != null)
			{
				float time = Time.time;
				float? num2 = this.lastShuffledTime;
				if (time - num2.Value < 1f)
				{
					return;
				}
			}
			if (this.IsShaken(handDot, otherHandDot))
			{
				TransformClipboard[] array = new TransformClipboard[this.groupItems.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new TransformClipboard();
					array[i].SetFromTransform(this.groupItems[i].transform);
					array[i].scale = null;
				}
				Misc.ShuffleArray<TransformClipboard>(array);
				for (int j = 0; j < array.Length; j++)
				{
					array[j].ApplyToTransform(this.groupItems[j].transform);
				}
				Hand component = this.groupItems[0].transform.parent.GetComponent<Hand>();
				Managers.personManager.DoInformOnMovableByEveryoneGroupTransforms(this.groupItems, component.side, MovableByEveryoneGroupInformType.Shuffle);
				Managers.soundManager.Play("shuffle", handDot.transform.parent.position, 0.3f, false, false);
				this.lastShuffledTime = new float?(Time.time);
			}
		}
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x00095084 File Offset: 0x00093484
	private bool IsShaken(HandDot handDot, HandDot otherHandDot)
	{
		return handDot.controller.velocity.magnitude >= 1f && handDot.controller.angularVelocity.magnitude >= 7f && otherHandDot.controller.velocity.magnitude >= 1f && otherHandDot.controller.angularVelocity.magnitude >= 7f;
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x00095108 File Offset: 0x00093508
	public void ResetGroup(HandDot handDot, HandDot otherHandDot)
	{
		if (this.groupItems.Count >= 1)
		{
			Hand component = this.groupItems[0].transform.parent.GetComponent<Hand>();
			Transform transform = Managers.thingManager.placements.transform;
			foreach (Thing thing in this.groupItems)
			{
				thing.transform.parent = transform;
				ThingMovableByEveryone component2 = thing.GetComponent<ThingMovableByEveryone>();
				if (component2 != null)
				{
					component2.OnPutDown(true);
				}
			}
			Managers.personManager.DoInformOnMovableByEveryoneGroupTransforms(this.groupItems, component.side, MovableByEveryoneGroupInformType.PutDown);
			Managers.soundManager.Play("putDown", handDot.transform.parent.position, 0.35f, false, false);
			this.groupItems = new List<Thing>();
		}
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x0009520C File Offset: 0x0009360C
	public bool HasGroup()
	{
		return this.groupItems.Count >= 1;
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x00095220 File Offset: 0x00093620
	private List<Thing> GetMovableByEveryoneThingsNearLine(Transform dotStart, Transform dotEnd)
	{
		List<Thing> list = new List<Thing>();
		Vector3 vector = Vector3.Lerp(dotStart.position, dotEnd.position, 0.5f);
		IEnumerator enumerator = Managers.thingManager.placements.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (Vector3.Distance(vector, transform.transform.position) <= 0.075f)
				{
					Thing component = transform.GetComponent<Thing>();
					if (component != null && component.movableByEveryone)
					{
						list.Add(component);
					}
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
		for (int i = 0; i <= 8; i++)
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			switch (i)
			{
			case 1:
				zero.z += 0.04f;
				zero2.z -= 0.04f;
				break;
			case 2:
				zero.z -= 0.04f;
				zero2.z += 0.04f;
				break;
			case 3:
				zero.y += 0.04f;
				zero2.y -= 0.04f;
				break;
			case 4:
				zero.y -= 0.04f;
				zero2.y += 0.04f;
				break;
			case 5:
				zero.z += 0.04f;
				zero2.z += 0.04f;
				break;
			case 6:
				zero.z -= 0.04f;
				zero2.z -= 0.04f;
				break;
			case 7:
				zero.y += 0.04f;
				zero2.y += 0.04f;
				break;
			case 8:
				zero.y -= 0.04f;
				zero2.y -= 0.04f;
				break;
			}
			Vector3 position = dotStart.position;
			Vector3 position2 = dotEnd.position;
			dotStart.Translate(zero);
			dotEnd.Translate(zero2);
			Line line = new Line(dotStart.position, dotEnd.position);
			dotStart.position = position;
			dotEnd.position = position2;
			Ray ray = new Ray(line.start, line.direction);
			foreach (RaycastHit raycastHit in Physics.RaycastAll(ray, line.length))
			{
				ThingPart component2 = raycastHit.collider.gameObject.GetComponent<ThingPart>();
				if (component2 != null)
				{
					Thing myRootThing = component2.GetMyRootThing();
					if (myRootThing != null && myRootThing.movableByEveryone && myRootThing.transform.parent == Managers.thingManager.placements.transform && !list.Contains(myRootThing))
					{
						list.Add(myRootThing);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x000955A4 File Offset: 0x000939A4
	public bool MyRootThingIsMovableByEveryonePlacement(GameObject thisObject)
	{
		bool flag = false;
		ThingPart component = thisObject.GetComponent<ThingPart>();
		if (component != null)
		{
			Thing myRootThing = component.GetMyRootThing();
			flag = myRootThing != null && myRootThing.movableByEveryone && myRootThing.IsPlacement();
		}
		return flag;
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x000955F0 File Offset: 0x000939F0
	public bool IsHoldableSubThing(GameObject thisObject)
	{
		bool flag = false;
		if (thisObject.transform.parent != null)
		{
			Thing component = thisObject.transform.parent.GetComponent<Thing>();
			flag = component != null && component.isHoldable;
		}
		return flag;
	}

	// Token: 0x04001131 RID: 4401
	private List<Thing> groupItems = new List<Thing>();

	// Token: 0x04001132 RID: 4402
	private bool addDebugLineVisuals;

	// Token: 0x04001133 RID: 4403
	private float? lastShuffledTime;
}
