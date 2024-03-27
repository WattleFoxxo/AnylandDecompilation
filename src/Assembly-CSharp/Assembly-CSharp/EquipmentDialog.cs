using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class EquipmentDialog : Dialog
{
	// Token: 0x060009A3 RID: 2467 RVA: 0x0003EB48 File Offset: 0x0003CF48
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		Managers.thingManager.heldThingsRegistrar.GetQuickEquipList(delegate(string err, List<string> _thingIds)
		{
			if (err != null)
			{
				Log.Debug(err);
				return;
			}
			this.thingIds = _thingIds;
			if (this.thingIds.Count >= 1)
			{
				this.maxPages = Mathf.CeilToInt((float)this.thingIds.Count / 20f);
				if (this.maxPages >= 2)
				{
					base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
				}
				base.StartCoroutine(this.AddThings());
			}
			else
			{
				base.AddLabel("Your previously held items will show here", 0, -75, 0.85f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		});
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x0003EB84 File Offset: 0x0003CF84
	private IEnumerator AddThings()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int x = 0;
		int y = 0;
		int minI = this.page * 20;
		int maxI = Mathf.Min(minI + 20 - 1, this.thingIds.Count - 1);
		int i = 0;
		foreach (string text in this.thingIds)
		{
			if (i >= minI && i <= maxI)
			{
				Vector3 vector = new Vector3((float)(-380 + x * 200) * this.scaleFactor, 0.015f, (float)(300 - y * 200) * this.scaleFactor);
				Managers.thingManager.InstantiateThingOnDialogViaCache(ThingRequestContext.EquipmentDialog, text, this.transform, vector, 0.035f, true, true, 0f, 0f, 0f, false, false);
				if (++x >= 5)
				{
					x = 0;
					y++;
				}
			}
			i++;
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0003EBA0 File Offset: 0x0003CFA0
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "previousPage"))
			{
				if (!(contextName == "nextPage"))
				{
					if (contextName == "close")
					{
						base.CloseDialog();
					}
				}
				else
				{
					if (++this.page > this.maxPages - 1)
					{
						this.page = 0;
					}
					base.StartCoroutine(this.AddThings());
				}
			}
			else
			{
				if (--this.page < 0)
				{
					this.page = this.maxPages - 1;
				}
				base.StartCoroutine(this.AddThings());
			}
		}
	}

	// Token: 0x04000738 RID: 1848
	private const int thingsPerRow = 5;

	// Token: 0x04000739 RID: 1849
	private const int thingsPerPage = 20;

	// Token: 0x0400073A RID: 1850
	private const bool addMoreThingsForTesting = false;

	// Token: 0x0400073B RID: 1851
	private int page;

	// Token: 0x0400073C RID: 1852
	private int maxPages = -1;

	// Token: 0x0400073D RID: 1853
	private List<string> thingIds = new List<string>();
}
