using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000276 RID: 630
public class ThingMovableByEveryone : MonoBehaviour
{
	// Token: 0x06001759 RID: 5977 RVA: 0x000D0E40 File Offset: 0x000CF240
	private void Start()
	{
		this.thing = base.GetComponent<Thing>();
		if (this.thing == null)
		{
			global::UnityEngine.Object.Destroy(this);
		}
		this.containsLocks = !this.thing.lockPhysicsPosition.IsAllDefault() || !this.thing.lockPhysicsRotation.IsAllDefault();
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x000D0EA1 File Offset: 0x000CF2A1
	private void Update()
	{
		if (this.containsLocks && !this.WeDragItInAnEditMode())
		{
			this.ConstrainToPositionAndRotationLocks();
		}
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x000D0EC0 File Offset: 0x000CF2C0
	private bool WeDragItInAnEditMode()
	{
		bool flag = false;
		if (base.transform.parent != Managers.thingManager.placements.transform)
		{
			Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(base.gameObject);
			flag = personThisObjectIsOf != null && personThisObjectIsOf.isOurPerson && Our.mode != EditModes.None;
		}
		return flag;
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x000D0F2C File Offset: 0x000CF32C
	private void ConstrainToPositionAndRotationLocks()
	{
		Vector3 position = base.transform.position;
		Vector3 eulerAngles = base.transform.eulerAngles;
		if (this.thing.lockPhysicsPosition.x)
		{
			position.x = this.thing.originalPlacementPosition.x;
		}
		if (this.thing.lockPhysicsPosition.y)
		{
			position.y = this.thing.originalPlacementPosition.y;
		}
		if (this.thing.lockPhysicsPosition.z)
		{
			position.z = this.thing.originalPlacementPosition.z;
		}
		if (this.thing.lockPhysicsRotation.x)
		{
			eulerAngles.x = this.thing.originalPlacementRotation.x;
		}
		if (this.thing.lockPhysicsRotation.y)
		{
			eulerAngles.y = this.thing.originalPlacementRotation.y;
		}
		if (this.thing.lockPhysicsRotation.z)
		{
			eulerAngles.z = this.thing.originalPlacementRotation.z;
		}
		base.transform.position = position;
		base.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x000D1087 File Offset: 0x000CF487
	public void OnPickUp(bool isOur = false)
	{
		this.thing.TriggerEvent(StateListener.EventType.OnTaken, string.Empty, false, null);
		if (isOur)
		{
			this.AddRigidBodiesToPartsCheckingTouch();
		}
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x000D10AA File Offset: 0x000CF4AA
	public void OnPutDown(bool isOur = false)
	{
		this.thing.TriggerEvent(StateListener.EventType.OnLetGo, string.Empty, false, null);
		if (isOur)
		{
			this.RemoveRigidBodiesFromParts();
		}
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x000D10D0 File Offset: 0x000CF4D0
	private void AddRigidBodiesToPartsCheckingTouch()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				ThingPart component = transform.GetComponent<ThingPart>();
				if (component != null && this.ContainsTouchListener(component))
				{
					Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
					if (rigidbody == null)
					{
						rigidbody = transform.gameObject.AddComponent<Rigidbody>();
					}
					rigidbody.isKinematic = true;
					Collider component2 = component.GetComponent<Collider>();
					if (component2 != null)
					{
						component2.isTrigger = true;
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
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x000D1194 File Offset: 0x000CF594
	private void RemoveRigidBodiesFromParts()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				ThingPart component = transform.GetComponent<ThingPart>();
				if (component != null)
				{
					global::UnityEngine.Object.Destroy(transform.GetComponent<Rigidbody>());
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
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x000D1210 File Offset: 0x000CF610
	private bool ContainsTouchListener(ThingPart thingPart)
	{
		foreach (ThingPartState thingPartState in thingPart.states)
		{
			foreach (StateListener stateListener in thingPartState.listeners)
			{
				if (stateListener.eventType == StateListener.EventType.OnTouches || stateListener.eventType == StateListener.EventType.OnTouchEnds)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x040015C5 RID: 5573
	private Thing thing;

	// Token: 0x040015C6 RID: 5574
	private bool containsLocks;
}
