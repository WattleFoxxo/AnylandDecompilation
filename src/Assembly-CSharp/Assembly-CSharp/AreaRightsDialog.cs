using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class AreaRightsDialog : Dialog
{
	// Token: 0x06000923 RID: 2339 RVA: 0x000379A8 File Offset: 0x00035DA8
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Area Rights", -370, -460, TextColor.Default, TextAlignment.Left, false);
		base.AddGenericHelpButton();
		this.ShowRights();
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x000379FC File Offset: 0x00035DFC
	private void ShowRights()
	{
		AreaRights rights = Managers.areaManager.rights;
		AreaRights areaRights = new AreaRights();
		this.AddRight("Emitted climbing", rights.emittedClimbing, areaRights.emittedClimbing);
		this.AddRight("Emitted transporting", rights.emittedTransporting, areaRights.emittedTransporting);
		this.AddRight("moving through obstacles", rights.movingThroughObstacles, areaRights.movingThroughObstacles);
		this.AddRight("vision in obstacles", rights.visionInObstacles, areaRights.visionInObstacles);
		this.AddRight("Invisibility", rights.invisibility, areaRights.invisibility);
		this.AddRight("Any person size", rights.anyPersonSize, areaRights.anyPersonSize);
		this.AddRight("Highlighting", rights.highlighting, areaRights.highlighting);
		this.AddRight("Amplified speech", rights.amplifiedSpeech, areaRights.amplifiedSpeech);
		this.AddRight("Any Destruction", rights.anyDestruction, areaRights.anyDestruction);
		this.AddRight("Web browsing", rights.webBrowsing, areaRights.webBrowsing);
		this.AddRight("Untargetted attract", rights.untargetedAttractThings, areaRights.untargetedAttractThings);
		this.AddRight("Build animations", rights.slowBuildCreation, areaRights.slowBuildCreation);
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00037B30 File Offset: 0x00035F30
	private void AddRight(string label, bool? currentRight, bool? defaultRight)
	{
		int num = -310 + this.rightsCounter * 60;
		TextColor textColor = ((!(currentRight == true)) ? TextColor.Red : TextColor.Green);
		label = Misc.BoolAsCheckmarkOrCross(currentRight) + "  " + label;
		string text = label;
		int num2 = -410;
		int num3 = num;
		float num4 = 0.9f;
		TextColor textColor2 = textColor;
		base.AddLabel(text, num2, num3, num4, false, textColor2, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		string text2 = "default";
		textColor = TextColor.Gray;
		if (currentRight.GetValueOrDefault() != defaultRight.GetValueOrDefault() || ((currentRight != null) ^ (defaultRight != null)))
		{
			text2 = "changed";
			textColor = TextColor.Default;
		}
		text = text2;
		num3 = 250;
		num2 = num + 4;
		num4 = 0.7f;
		textColor2 = textColor;
		base.AddLabel(text, num3, num2, num4, false, textColor2, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.rightsCounter++;
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00037C2C File Offset: 0x0003602C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "help"))
			{
				if (!(contextName == "back"))
				{
					if (contextName == "close")
					{
						base.CloseDialog();
					}
				}
				else
				{
					base.SwitchTo(DialogType.AreaAttributes, string.Empty);
				}
			}
			else
			{
				string text = "http://anyland.com/scripting/#thenAllow";
				Managers.browserManager.ToggleGuideBrowser(text);
			}
		}
	}

	// Token: 0x040006E3 RID: 1763
	private int rightsCounter;
}
