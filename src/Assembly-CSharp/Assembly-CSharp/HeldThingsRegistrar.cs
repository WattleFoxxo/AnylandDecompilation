using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class HeldThingsRegistrar : MonoBehaviour
{
	// Token: 0x06000D27 RID: 3367 RVA: 0x00075F88 File Offset: 0x00074388
	public void RegisterHold(Hand hand, bool wasManualDesktopAdjustment = false)
	{
		Thing heldThing = hand.GetHeldThing();
		if (this.coroutineSide[hand.side] != null)
		{
			base.StopCoroutine(this.coroutineSide[hand.side]);
		}
		this.coroutineSide[hand.side] = this.WaitAndRegisterHold(hand, heldThing);
		base.StartCoroutine(this.coroutineSide[hand.side]);
		if (CrossDevice.desktopMode && !wasManualDesktopAdjustment)
		{
			base.StartCoroutine(this.TryGetHoldGeometryAndAutoAdjust(hand));
		}
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x00076018 File Offset: 0x00074418
	private IEnumerator WaitAndRegisterHold(Hand hand, Thing thing)
	{
		yield return new WaitForSeconds(3f);
		this.coroutineSide[hand.side] = null;
		if (thing != null && thing == hand.GetHeldThing())
		{
			HoldGeometryData holdGeometryData = new HoldGeometryData(thing.transform.localPosition, thing.transform.localRotation);
			yield return base.StartCoroutine(Managers.serverManager.RegisterHold(thing.thingId, hand.side, holdGeometryData, CrossDevice.desktopMode, delegate(ResponseBase response)
			{
				if (response.error != null)
				{
					Log.Error(response.error);
				}
			}));
			this.UpdateLocalGetQuickEquipList(thing.thingId);
		}
		yield break;
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x00076044 File Offset: 0x00074444
	private IEnumerator TryGetHoldGeometryAndAutoAdjust(Hand hand)
	{
		Thing thingInHand = hand.GetHeldThing();
		if (thingInHand == null)
		{
			Log.Warning("HeldThingsRegistrar.TryGetHoldGeometryAndAutoAdjust called with nothing in hand");
			yield break;
		}
		HoldGeometryData holdGeometryData = null;
		Log.Info("TryGetHoldGeometryAndAutoAdjust - Trying to get data from server", false);
		yield return base.StartCoroutine(this.getHoldGeometryForThing(thingInHand.thingId, hand.side, delegate(string errorString, HoldGeometryData _holdGeometryData)
		{
			holdGeometryData = _holdGeometryData;
		}));
		if (holdGeometryData != null && holdGeometryData.rotation != Quaternion.identity)
		{
			thingInHand.transform.localPosition = holdGeometryData.position;
			thingInHand.transform.localRotation = holdGeometryData.rotation;
			thingInHand.MemorizeOriginalTransform(false);
			Managers.personManager.DoChangeHeldThingPositionRotation(thingInHand, true);
		}
		else
		{
			Log.Info("No hold geometry available.", false);
		}
		yield break;
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x00076068 File Offset: 0x00074468
	private IEnumerator PrimeLocalQuickEquipListCache(Action<string> callback)
	{
		yield return base.StartCoroutine(Managers.serverManager.GetQuickEquipList(delegate(GetQuickEquipList_Response response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			else
			{
				this.QuickEquipList = response.thingIds;
				this.LocalQuickEquipListPrimed = true;
			}
			callback(response.error);
		}));
		yield break;
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0007608C File Offset: 0x0007448C
	public void GetQuickEquipList(Action<string, List<string>> callback)
	{
		base.StartCoroutine(this.getQuickEquipList(delegate(string errorString, List<string> quickEquipList)
		{
			callback(errorString, quickEquipList);
		}));
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x000760C0 File Offset: 0x000744C0
	private IEnumerator getQuickEquipList(Action<string, List<string>> callback)
	{
		if (this.LocalQuickEquipListPrimed)
		{
			Log.Info("GetQuickEquipList - returning local list", false);
			callback(null, this.QuickEquipList);
			yield break;
		}
		Log.Info("GetQuickEquipList - local list not primed, calling server", false);
		string error = null;
		yield return base.StartCoroutine(this.PrimeLocalQuickEquipListCache(delegate(string _error)
		{
			error = _error;
		}));
		Log.Info("GetQuickEquipList - returning primed list", false);
		callback(error, this.QuickEquipList);
		yield break;
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x000760E2 File Offset: 0x000744E2
	private void UpdateLocalGetQuickEquipList(string thingId)
	{
		this.QuickEquipList.Remove(thingId);
		this.QuickEquipList.Insert(0, thingId);
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x00076100 File Offset: 0x00074500
	public void GetHoldGeometryForThing(string thingId, Side handSide, Action<string, HoldGeometryData> callback)
	{
		base.StartCoroutine(this.getHoldGeometryForThing(thingId, handSide, delegate(string errorString, HoldGeometryData holdGeometry)
		{
			callback(errorString, holdGeometry);
		}));
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x00076138 File Offset: 0x00074538
	private IEnumerator getHoldGeometryForThing(string thingId, Side handSide, Action<string, HoldGeometryData> callback)
	{
		string error = null;
		HoldGeometryData holdGeometry = null;
		yield return base.StartCoroutine(Managers.serverManager.GetHoldGeometry(thingId, handSide, delegate(GetHoldGeometry_Response response)
		{
			error = response.error;
			holdGeometry = response.holdGeometry;
		}));
		if (!string.IsNullOrEmpty(error))
		{
			Log.Error(error);
		}
		callback(error, holdGeometry);
		yield break;
	}

	// Token: 0x04000EC8 RID: 3784
	private const int PERSIST_HOLD_DELAY_SECS = 3;

	// Token: 0x04000EC9 RID: 3785
	private const float ROTATION_COMPARISON_TOLERANCE = 1E-08f;

	// Token: 0x04000ECA RID: 3786
	private const float POSITION_COMPARISON_TOLERANCE = 1E-08f;

	// Token: 0x04000ECB RID: 3787
	private List<string> QuickEquipList = new List<string>();

	// Token: 0x04000ECC RID: 3788
	private bool LocalQuickEquipListPrimed;

	// Token: 0x04000ECD RID: 3789
	private Dictionary<Side, IEnumerator> coroutineSide = new Dictionary<Side, IEnumerator>
	{
		{
			Side.Left,
			null
		},
		{
			Side.Right,
			null
		}
	};
}
