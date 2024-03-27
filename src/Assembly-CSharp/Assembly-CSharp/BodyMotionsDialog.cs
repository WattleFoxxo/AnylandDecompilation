using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class BodyMotionsDialog : Dialog
{
	// Token: 0x06000973 RID: 2419 RVA: 0x0003CA70 File Offset: 0x0003AE70
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		base.AddMirror();
		List<ThingPart> myThingParts = this.GetMyThingParts();
		if (myThingParts.Count == 0)
		{
			this.ShowIssue("You don't have body parts or holdables. Please try find some first.");
		}
		else
		{
			this.motions = this.GetTellBodyDataBodyIsListeningFor(myThingParts);
			this.maxPages = Mathf.CeilToInt((float)this.motions.Count / 14f);
			if (this.motions.Count == 0)
			{
				this.ShowIssue("Your current body does not have any motions.");
			}
			else
			{
				base.StartCoroutine(this.AddMotionButtons());
				if (this.maxPages >= 2)
				{
					base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
				}
				base.AddLabel("Motions are added using" + Environment.NewLine + "\"When told by body...\" commands", 0, -30, 0.8f, true, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0003CB70 File Offset: 0x0003AF70
	private IEnumerator AddMotionButtons()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int buttonX = 0;
		int buttonY = 0;
		int minI = this.page * 14;
		int maxI = Mathf.Min(minI + 14 - 1, this.motions.Count - 1);
		int i = 0;
		foreach (string text in this.motions)
		{
			if (i >= minI && i <= maxI)
			{
				int num = -225 + buttonX * 450;
				int num2 = -310 + buttonY * 100;
				string text2 = Misc.Truncate(text, 23, true);
				base.AddButton("doMotion", text, text2, "ButtonCompactNoIcon", num, num2, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				if (++buttonX >= 2)
				{
					buttonX = 0;
					buttonY++;
				}
			}
			i++;
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0003CB8C File Offset: 0x0003AF8C
	private List<string> GetSampleMotions(int multiplier = 1)
	{
		List<string> list = new List<string>();
		for (int i = 0; i <= multiplier; i++)
		{
			list.Add("stare");
			list.Add("wave");
			list.Add("some really long motion name to test long motion names");
		}
		return list;
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x0003CBD4 File Offset: 0x0003AFD4
	private List<ThingPart> GetMyThingParts()
	{
		List<ThingPart> list = new List<ThingPart>();
		Component[] componentsInChildren = Managers.personManager.ourPerson.Rig.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (Component component in componentsInChildren)
		{
			if (component.gameObject.name != "HandModelLeftThingPart" && component.gameObject.name != "HandModelRightThingPart")
			{
				list.Add(component.GetComponent<ThingPart>());
			}
		}
		return list;
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0003CC68 File Offset: 0x0003B068
	private List<string> GetTellBodyDataBodyIsListeningFor(List<ThingPart> thingParts)
	{
		List<string> list = new List<string>();
		foreach (ThingPart thingPart in thingParts)
		{
			foreach (ThingPartState thingPartState in thingPart.states)
			{
				foreach (StateListener stateListener in thingPartState.listeners)
				{
					if (stateListener.eventType == StateListener.EventType.OnToldByBody && !string.IsNullOrEmpty(stateListener.whenData) && list.IndexOf(stateListener.whenData) == -1)
					{
						list.Add(stateListener.whenData);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0003CD8C File Offset: 0x0003B18C
	private void ShowIssue(string issue)
	{
		issue += " Motions can be added by creating a body with \"When told by body...\" commands.";
		base.AddLabel(issue, 0, -200, 0.9f, false, TextColor.Default, false, TextAlignment.Center, 40, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0003CDC6 File Offset: 0x0003B1C6
	private void OnDestroy()
	{
		base.RemoveMirror();
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0003CDD0 File Offset: 0x0003B1D0
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "doMotion"))
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
						base.StartCoroutine(this.AddMotionButtons());
					}
				}
				else
				{
					if (--this.page < 0)
					{
						this.page = this.maxPages - 1;
					}
					base.StartCoroutine(this.AddMotionButtons());
				}
			}
			else
			{
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, contextId, true);
			}
		}
	}

	// Token: 0x04000725 RID: 1829
	private int page;

	// Token: 0x04000726 RID: 1830
	private int maxPages = -1;

	// Token: 0x04000727 RID: 1831
	private const int motionsPerPage = 14;

	// Token: 0x04000728 RID: 1832
	private List<string> motions = new List<string>();
}
