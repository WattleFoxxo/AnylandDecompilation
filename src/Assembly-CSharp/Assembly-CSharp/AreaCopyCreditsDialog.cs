using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FE RID: 254
public class AreaCopyCreditsDialog : Dialog
{
	// Token: 0x060008E9 RID: 2281 RVA: 0x00034C30 File Offset: 0x00033030
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		bool flag = false;
		if (Our.areaIdOfInterest != null)
		{
			base.AddBackButton();
			flag = true;
		}
		base.AddCloseButton();
		int num = -440;
		if (flag)
		{
			num += 40;
		}
		TextMesh textMesh = base.AddFlexibleSizeHeadline("Area with pastes from...", num, -450, TextColor.Default, 25);
		textMesh.transform.localScale *= 0.8f;
		Managers.areaManager.GetInfo(delegate(AreaInfo areaInfo)
		{
			if (this == null)
			{
				return;
			}
			this.AddCopiedFromAreasButtons(areaInfo.copiedFromAreas, areaInfo.editors[0].id);
		});
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00034CC4 File Offset: 0x000330C4
	private void AddCopiedFromAreasButtons(List<AreaIdNameAndCreatorId> copiedFromAreas, string currentAreaOwnerId)
	{
		int num = 0;
		int num2 = 0;
		foreach (AreaIdNameAndCreatorId areaIdNameAndCreatorId in copiedFromAreas)
		{
			if (areaIdNameAndCreatorId.creatorId != currentAreaOwnerId)
			{
				int num3 = -225 + num * 450;
				int num4 = -300 + num2 * 75;
				string text = Misc.Truncate(areaIdNameAndCreatorId.name, 17, true);
				GameObject gameObject = base.AddButton("goToArea", areaIdNameAndCreatorId.name, text, "ButtonCompactSmallIcon", num3, num4, "teleportTo", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				if (++num >= 2)
				{
					num = 0;
					num2++;
					if (num2 >= 10)
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00034DB8 File Offset: 0x000331B8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "goToArea"))
			{
				if (!(contextName == "back"))
				{
					if (contextName == "close")
					{
						Our.areaIdOfInterest = null;
						base.CloseDialog();
					}
				}
				else
				{
					base.SwitchTo(DialogType.Area, string.Empty);
				}
			}
			else
			{
				Managers.areaManager.TryTransportToAreaByNameOrUrlName(contextId, string.Empty, false);
			}
		}
	}
}
