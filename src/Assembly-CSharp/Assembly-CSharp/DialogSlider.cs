using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class DialogSlider : MonoBehaviour
{
	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00033629 File Offset: 0x00031A29
	// (set) Token: 0x060008C6 RID: 2246 RVA: 0x00033631 File Offset: 0x00031A31
	public float value { get; private set; }

	// Token: 0x060008C7 RID: 2247 RVA: 0x0003363C File Offset: 0x00031A3C
	private void Start()
	{
		this.mover = base.transform.Find("Mover");
		this.label = base.transform.Find("Label");
		this.labelText = this.label.GetComponent<TextMesh>();
		if (!string.IsNullOrEmpty(this.valuePrefix))
		{
			this.valuePrefix = this.valuePrefix.ToUpper();
		}
		if (!string.IsNullOrEmpty(this.valueSuffix))
		{
			this.valueSuffix = this.valueSuffix.ToUpper();
		}
		if (this.textSizeFactor != 1f)
		{
			this.ApplyTextSizeFactor();
		}
		this.SetValue(this.startValue);
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x000336EA File Offset: 0x00031AEA
	private void Update()
	{
		this.UpdateLabelPosition();
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x000336F2 File Offset: 0x00031AF2
	private void ApplyTextSizeFactor()
	{
		this.labelText.transform.localScale *= this.textSizeFactor;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00033718 File Offset: 0x00031B18
	private void UpdateLabelPosition()
	{
		Vector3 localPosition = this.label.localPosition;
		if (this.mover.localPosition.x <= 0f)
		{
			localPosition.x = this.mover.localPosition.x + 0.01f;
			this.labelText.alignment = TextAlignment.Left;
			this.labelText.anchor = TextAnchor.UpperLeft;
		}
		else
		{
			localPosition.x = this.mover.localPosition.x - 0.01f;
			this.labelText.alignment = TextAlignment.Right;
			this.labelText.anchor = TextAnchor.UpperRight;
		}
		this.label.localPosition = localPosition;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x000337CF File Offset: 0x00031BCF
	private void HandleValueClampAndRound()
	{
		if (this.roundValues)
		{
			this.value = Mathf.Round(this.value);
		}
		this.value = Mathf.Clamp(this.value, this.minValue, this.maxValue);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003380C File Offset: 0x00031C0C
	private void UpdateLabelText()
	{
		this.labelText.text = this.valuePrefix + ((!this.showValue) ? string.Empty : this.value.ToString()) + this.valueSuffix;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00033860 File Offset: 0x00031C60
	public void ValueChanged(float valueFactor)
	{
		float value = this.value;
		this.value = this.minValue + valueFactor * this.maxValue;
		this.HandleValueClampAndRound();
		if (this.onValueChange != null && value != this.value)
		{
			this.onValueChange(this.value);
		}
		this.UpdateLabelText();
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x000338C0 File Offset: 0x00031CC0
	public void SetValue(float thisValue)
	{
		this.value = thisValue;
		this.HandleValueClampAndRound();
		float num = 0f;
		if (this.value != 0f)
		{
			num = (this.value - this.minValue) / this.maxValue;
		}
		Vector3 localPosition = this.mover.localPosition;
		localPosition.x = 0.218f * num - 0.109f;
		this.mover.localPosition = localPosition;
		this.UpdateLabelText();
	}

	// Token: 0x0400067D RID: 1661
	public bool showValue = true;

	// Token: 0x0400067E RID: 1662
	public string valuePrefix = string.Empty;

	// Token: 0x0400067F RID: 1663
	public string valueSuffix = string.Empty;

	// Token: 0x04000680 RID: 1664
	public float minValue;

	// Token: 0x04000681 RID: 1665
	public float maxValue = 100f;

	// Token: 0x04000682 RID: 1666
	public bool roundValues;

	// Token: 0x04000683 RID: 1667
	public float startValue;

	// Token: 0x04000684 RID: 1668
	public float textSizeFactor = 1f;

	// Token: 0x04000685 RID: 1669
	public Action<float> onValueChange;

	// Token: 0x04000687 RID: 1671
	private Transform mover;

	// Token: 0x04000688 RID: 1672
	private Transform label;

	// Token: 0x04000689 RID: 1673
	private TextMesh labelText;

	// Token: 0x0400068A RID: 1674
	private const float labelToMoverDistance = 0.01f;

	// Token: 0x0400068B RID: 1675
	private string fullValueSuffix = string.Empty;
}
