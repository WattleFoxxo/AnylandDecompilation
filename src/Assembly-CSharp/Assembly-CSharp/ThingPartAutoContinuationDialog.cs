using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class ThingPartAutoContinuationDialog : Dialog
{
	// Token: 0x06000C45 RID: 3141 RVA: 0x0006E244 File Offset: 0x0006C644
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddGenericHelpButton();
		base.AddHeadline("Auto-Complete", -370, -460, TextColor.Default, TextAlignment.Left, false);
		if (CreationHelper.thingPartWhoseStatesAreEdited == null)
		{
			return;
		}
		this.thingPart = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
		if (this.thingPart.autoContinuation != null && this.thingPart.autoContinuation.count > 100)
		{
			ThingPartAutoContinuationDialog.higherPartsMaxCount = true;
		}
		this.AddMainInterface();
		this.AddBacksideInterface();
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0006E2EC File Offset: 0x0006C6EC
	private void AddBacksideInterface()
	{
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		base.AddCheckbox("higherPartsMaxCount", null, "More parts", 0, 0, ThingPartAutoContinuationDialog.higherPartsMaxCount, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0006E34C File Offset: 0x0006C74C
	public void AddOrRemoveAutoContinuationFrom(ThingPart otherThingPart)
	{
		if (this.thingPart != otherThingPart)
		{
			this.thingPart.RemoveMyAssignedContinuationParts();
			if (this.thingPart.autoContinuation != null && this.thingPart.autoContinuation.fromPart == otherThingPart.gameObject)
			{
				this.thingPart.autoContinuation = null;
			}
			else if (this.thingPart.baseType != otherThingPart.baseType)
			{
				Managers.soundManager.Play("no", this.transform, 0.15f, false, false);
			}
			else
			{
				this.thingPart.autoContinuation = new ThingPartAutoContinuation();
				this.thingPart.autoContinuation.fromPart = otherThingPart.gameObject;
				this.thingPart.autoContinuation.fromPartGuid = Misc.GetRandomId();
				this.thingPart.autoContinuation.count = 1;
				otherThingPart.guid = this.thingPart.autoContinuation.fromPartGuid;
			}
			this.AddMainInterface();
			Our.lastTransformHandled = this.thingPart.transform;
		}
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x0006E465 File Offset: 0x0006C865
	private void AddMainInterface()
	{
		base.StartCoroutine(this.DoAddMainInterface());
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0006E474 File Offset: 0x0006C874
	private IEnumerator DoAddMainInterface()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		if (this.thingPart.autoContinuation == null || this.thingPart.autoContinuation.fromPart == null)
		{
			string text = "Context-laser origin part" + Environment.NewLine + "from which to complete";
			base.AddLabel(text, 0, -60, 0.85f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		else
		{
			if (this.page == 0)
			{
				int num = -150;
				int num2 = num;
				base.AddSlider("+", " parts", 0, num2, 0f, (float)((!ThingPartAutoContinuationDialog.higherPartsMaxCount) ? 100 : 500), true, (float)this.thingPart.autoContinuation.count, new Action<float>(this.OnCountChange), true, 1f);
				num += 120;
				num += 150;
				num2 = num;
				float num3 = (float)this.thingPart.autoContinuation.waves;
				this.wavesSlider = base.AddSlider(string.Empty, " Waves", 0, num2, 0f, 50f, true, num3, new Action<float>(this.OnWavesChange), true, 1f);
				num += 120;
			}
			else if (this.page == 1)
			{
				int num = -120;
				int num2 = num;
				float num3 = this.thingPart.autoContinuation.positionRandomization.x;
				base.AddSlider("Random X ", string.Empty, 0, num2, 0f, 100f, true, num3, new Action<float>(this.OnPositionRandomizationXChange), false, 1f);
				num += 120;
				num2 = num;
				num3 = this.thingPart.autoContinuation.positionRandomization.y;
				base.AddSlider("Random Y ", string.Empty, 0, num2, 0f, 100f, true, num3, new Action<float>(this.OnPositionRandomizationYChange), false, 1f);
				num += 120;
				num2 = num;
				num3 = this.thingPart.autoContinuation.positionRandomization.z;
				base.AddSlider("Random Z ", string.Empty, 0, num2, 0f, 100f, true, num3, new Action<float>(this.OnPositionRandomizationZChange), false, 1f);
				num += 120;
			}
			else if (this.page == 2)
			{
				int num = -100;
				int num2 = num;
				float num3 = this.thingPart.autoContinuation.rotationRandomization.x;
				base.AddSlider("Random Rotation ", string.Empty, 0, num2, 0f, 100f, true, num3, new Action<float>(this.OnRotationRandomizationChange), false, 1f);
				num += 120;
				num += 60;
				num2 = num;
				num3 = this.thingPart.autoContinuation.scaleRandomization.x;
				base.AddSlider("Random Size ", string.Empty, 0, num2, 0f, 100f, true, num3, new Action<float>(this.OnScaleRandomizationChange), false, 1f);
				num += 120;
			}
			base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x0006E490 File Offset: 0x0006C890
	private void OnCountChange(float value)
	{
		if (this.thingPart.autoContinuation != null)
		{
			this.countToApply = (int)value;
			bool flag = value > 100f;
			if (flag)
			{
				if (!this.applyCountChangeInvokeInProcess)
				{
					this.applyCountChangeInvokeInProcess = true;
					base.CancelInvoke("ApplyCountChange");
					base.Invoke("ApplyCountChange", 0.5f);
				}
			}
			else
			{
				this.ApplyCountChange();
			}
		}
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0006E4FC File Offset: 0x0006C8FC
	private void ApplyCountChange()
	{
		this.applyCountChangeInvokeInProcess = false;
		if (this.thingPart.autoContinuation != null)
		{
			GameObject fromPart = this.thingPart.autoContinuation.fromPart;
			string fromPartGuid = this.thingPart.autoContinuation.fromPartGuid;
			int waves = this.thingPart.autoContinuation.waves;
			Vector3 positionRandomization = this.thingPart.autoContinuation.positionRandomization;
			Vector3 rotationRandomization = this.thingPart.autoContinuation.rotationRandomization;
			Vector3 scaleRandomization = this.thingPart.autoContinuation.scaleRandomization;
			this.thingPart.RemoveMyAssignedContinuationParts();
			this.thingPart.autoContinuation = new ThingPartAutoContinuation();
			this.thingPart.autoContinuation.fromPart = fromPart;
			this.thingPart.autoContinuation.fromPartGuid = fromPartGuid;
			this.thingPart.autoContinuation.count = this.countToApply;
			this.thingPart.autoContinuation.waves = waves;
			this.thingPart.autoContinuation.positionRandomization = positionRandomization;
			this.thingPart.autoContinuation.rotationRandomization = rotationRandomization;
			this.thingPart.autoContinuation.scaleRandomization = scaleRandomization;
		}
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0006E624 File Offset: 0x0006CA24
	private void OnWavesChange(float value)
	{
		this.thingPart.autoContinuation.waves = (int)value;
		string text = " Wave" + ((this.thingPart.autoContinuation.waves != 1) ? "s" : string.Empty);
		text = text.ToUpper();
		this.wavesSlider.valueSuffix = text;
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x0006E686 File Offset: 0x0006CA86
	private void OnPositionRandomizationXChange(float value)
	{
		this.thingPart.autoContinuation.positionRandomization.x = value;
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x0006E69E File Offset: 0x0006CA9E
	private void OnPositionRandomizationYChange(float value)
	{
		this.thingPart.autoContinuation.positionRandomization.y = value;
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x0006E6B6 File Offset: 0x0006CAB6
	private void OnPositionRandomizationZChange(float value)
	{
		this.thingPart.autoContinuation.positionRandomization.z = value;
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x0006E6CE File Offset: 0x0006CACE
	private void OnRotationRandomizationChange(float value)
	{
		this.thingPart.autoContinuation.rotationRandomization = Misc.GetUniformVector3(value);
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x0006E6E6 File Offset: 0x0006CAE6
	private void OnScaleRandomizationChange(float value)
	{
		this.thingPart.autoContinuation.scaleRandomization = Misc.GetUniformVector3(value);
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x0006E700 File Offset: 0x0006CB00
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "help"))
			{
				if (!(contextName == "previousPage"))
				{
					if (!(contextName == "nextPage"))
					{
						if (!(contextName == "higherPartsMaxCount"))
						{
							if (!(contextName == "back"))
							{
								if (contextName == "close")
								{
									this.Close();
								}
							}
							else
							{
								base.SwitchTo(DialogType.ThingPartAttributes, string.Empty);
							}
						}
						else
						{
							ThingPartAutoContinuationDialog.higherPartsMaxCount = state;
							this.AddMainInterface();
						}
					}
					else
					{
						if (++this.page > 2)
						{
							this.page = 0;
						}
						this.AddMainInterface();
					}
				}
				else
				{
					if (--this.page < 0)
					{
						this.page = 2;
					}
					this.AddMainInterface();
				}
			}
			else
			{
				Managers.browserManager.OpenGuideBrowser("E4YKMTAKFPQ", null);
			}
		}
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x0006E80E File Offset: 0x0006CC0E
	private void Close()
	{
		CreationHelper.thingPartWhoseStatesAreEdited = null;
		base.SwitchTo(DialogType.Create, string.Empty);
	}

	// Token: 0x04000948 RID: 2376
	private ThingPart thingPart;

	// Token: 0x04000949 RID: 2377
	private int page;

	// Token: 0x0400094A RID: 2378
	private const int maxPages = 3;

	// Token: 0x0400094B RID: 2379
	private DialogSlider wavesSlider;

	// Token: 0x0400094C RID: 2380
	private const int defaultPartsMax = 100;

	// Token: 0x0400094D RID: 2381
	private const int higherPartsMax = 500;

	// Token: 0x0400094E RID: 2382
	private static bool higherPartsMaxCount;

	// Token: 0x0400094F RID: 2383
	private int countToApply;

	// Token: 0x04000950 RID: 2384
	private bool applyCountChangeInvokeInProcess;
}
