using System;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class DialogSliderMover : MonoBehaviour
{
	// Token: 0x060008D4 RID: 2260 RVA: 0x00033A10 File Offset: 0x00031E10
	private void Start()
	{
		this.slider = base.transform.parent.GetComponent<DialogSlider>();
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00033A28 File Offset: 0x00031E28
	public void PushInDirection(float direction, HandDot handDot)
	{
		Vector3 localPosition = base.transform.localPosition;
		float x = localPosition.x;
		localPosition.x += direction * 0.0021799998f;
		localPosition.x = Mathf.Clamp(localPosition.x, -0.109f, 0.109f);
		base.transform.localPosition = localPosition;
		if (x != base.transform.localPosition.x)
		{
			HandDot component = handDot.otherDot.GetComponent<HandDot>();
			handDot.TriggerHapticPulse(Universe.mediumHapticPulse);
			component.TriggerHapticPulse(Universe.mediumHapticPulse);
			this.CalculateNewValueAndInformSlider();
		}
		else
		{
			handDot.TriggerHapticPulse(Universe.lowHapticPulse);
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00033AD8 File Offset: 0x00031ED8
	private void CalculateNewValueAndInformSlider()
	{
		float num = base.transform.localPosition.x + 0.109f;
		float num2 = 0f;
		if (num > 0f)
		{
			num2 = num / 0.218f;
		}
		this.slider.ValueChanged(num2);
	}

	// Token: 0x0400068E RID: 1678
	private DialogSlider slider;

	// Token: 0x0400068F RID: 1679
	public const float positionMax = 0.109f;

	// Token: 0x04000690 RID: 1680
	private const float speed = 0.0021799998f;
}
