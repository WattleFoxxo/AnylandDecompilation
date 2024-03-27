using System;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class SubdivideDialog : Dialog
{
	// Token: 0x06000C9A RID: 3226 RVA: 0x000737F4 File Offset: 0x00071BF4
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Detail", -370, -460, TextColor.Default, TextAlignment.Left, false);
		if (CreationHelper.thingPartWhoseStatesAreEdited != null)
		{
			this.thingPart = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
			this.baseGroup = Managers.thingManager.GetSubdividableGroup(this.thingPart.baseType).Value;
		}
		this.vertexMover = Managers.personManager.OurPersonRig.GetComponentInChildren<VertexMover>();
		this.AddButtons();
		base.AddGenericHelpButton();
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x000738A0 File Offset: 0x00071CA0
	private void AddButtons()
	{
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		ThingPartBase thingPartBase = ((this.baseGroup != ThingPartBase.Cube) ? ThingPartBase.Quad : ThingPartBase.Cube);
		string text = ((this.baseGroup != ThingPartBase.Cube) ? "Quad" : "Cube");
		for (int i = 0; i <= 4; i++)
		{
			for (int j = 0; j <= 3; j++)
			{
				int num = i + 2;
				int num2 = j + 2;
				string text2 = num + "x" + num2;
				string text3 = "SubdividedCube/" + text2;
				int num3 = (i - 2) * 160;
				int num4 = -220 + j * 160;
				ThingPartBase thingPartBase2 = thingPartBase;
				if (i != 0 || j != 0)
				{
					thingPartBase2 = (ThingPartBase)Enum.Parse(typeof(ThingPartBase), text + text2, true);
				}
				bool flag = thingPartBase2 == this.thingPart.baseType;
				string text4 = ((this.thingPart.changedVertices == null || this.thingPart.changedVertices.Count < 1) ? "choose" : "confirmChoose");
				GameObject gameObject = base.AddButton(text4, thingPartBase2.ToString(), null, "ButtonSmall", num3, num4, text3, flag, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				DialogPart component = gameObject.GetComponent<DialogPart>();
				component.autoStopHighlight = false;
			}
		}
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00073A40 File Offset: 0x00071E40
	private new void Update()
	{
		if (this.thingPart == null || this.vertexMover == null)
		{
			base.CloseDialog();
		}
		base.Update();
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00073A70 File Offset: 0x00071E70
	private void DropVertexMover()
	{
		if (this.vertexMover != null && this.vertexMover.transform.parent != null)
		{
			global::UnityEngine.Object.Destroy(this.vertexMover.transform.parent.gameObject);
		}
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00073AC3 File Offset: 0x00071EC3
	private void OnDestroy()
	{
		if (this.dropVertexMoverOnClosing)
		{
			this.DropVertexMover();
		}
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x00073AD8 File Offset: 0x00071ED8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "confirmChoose"))
			{
				if (!(contextName == "choose"))
				{
					if (!(contextName == "help"))
					{
						if (!(contextName == "close"))
						{
							if (contextName == "back")
							{
								this.dropVertexMoverOnClosing = false;
								base.SwitchTo(DialogType.VertexMover, string.Empty);
							}
						}
						else
						{
							base.CloseDialog();
						}
					}
					else
					{
						Managers.browserManager.OpenGuideBrowser("CBOPUw9-Ecg", null);
					}
				}
				else
				{
					ThingPartBase thingPartBase = (ThingPartBase)Enum.Parse(typeof(ThingPartBase), contextId, true);
					if (thingPartBase != this.thingPart.baseType)
					{
						this.vertexMover.SwitchToNewMeshForSubdivision(thingPartBase);
						Managers.soundManager.Play("success", this.transform, 0.15f, false, false);
					}
					this.dropVertexMoverOnClosing = false;
					base.SwitchTo(DialogType.VertexMover, string.Empty);
				}
			}
			else
			{
				ThingPartBase thingPartBase2 = (ThingPartBase)Enum.Parse(typeof(ThingPartBase), contextId, true);
				if (thingPartBase2 == this.thingPart.baseType)
				{
					this.dropVertexMoverOnClosing = false;
					base.SwitchTo(DialogType.VertexMover, string.Empty);
				}
				else
				{
					global::UnityEngine.Object.Destroy(this.confirmButton);
					global::UnityEngine.Object.Destroy(this.wrapper);
					base.SetUiWrapper(base.gameObject);
					this.confirmButton = base.AddButton("choose", thingPartBase2.ToString(), "Reset points to this?", "ButtonCompactNoIcon", 0, -10, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
			}
		}
	}

	// Token: 0x04000977 RID: 2423
	private ThingPart thingPart;

	// Token: 0x04000978 RID: 2424
	private bool dropVertexMoverOnClosing = true;

	// Token: 0x04000979 RID: 2425
	private VertexMover vertexMover;

	// Token: 0x0400097A RID: 2426
	private GameObject confirmButton;

	// Token: 0x0400097B RID: 2427
	private ThingPartBase baseGroup = ThingPartBase.Cube;
}
