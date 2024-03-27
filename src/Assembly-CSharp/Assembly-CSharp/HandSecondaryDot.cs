using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class HandSecondaryDot : MonoBehaviour
{
	// Token: 0x060007A3 RID: 1955 RVA: 0x0002A13E File Offset: 0x0002853E
	private void Start()
	{
		this.handDot = this.handDotObject.GetComponent<HandDot>();
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0002A151 File Offset: 0x00028551
	private void OnTriggerStay(Collider other)
	{
		if (this.handDot != null && !this.HandDotTouchesThingPart())
		{
			this.handDot.HandleOnTriggerStay(other, true);
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0002A17C File Offset: 0x0002857C
	private bool HandDotTouchesThingPart()
	{
		bool flag = false;
		Collider[] array = Physics.OverlapSphere(this.handDot.transform.position, 0.005f);
		foreach (Collider collider in array)
		{
			ThingPart component = collider.GetComponent<ThingPart>();
			if (component != null && !this.handDot.ColliderIsLockedForEditing(component.gameObject) && !this.handDot.ColliderIsArmAttachmentOfSameSide(component.transform))
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0002A210 File Offset: 0x00028610
	private void Update()
	{
		this.HandleHapticFeedbackWhenTouchingOtherHands();
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x0002A218 File Offset: 0x00028618
	private void HandleHapticFeedbackWhenTouchingOtherHands()
	{
		if (Managers.personManager == null || Managers.behaviorScriptManager == null || Managers.personManager.WeAreResized())
		{
			return;
		}
		Transform transform = Managers.personManager.People.transform;
		ushort num = 0;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform2 = (Transform)obj;
				Person component = transform2.GetComponent<Person>();
				if (component != null)
				{
					ushort num2 = this.HandleHapticFeedbackForThisHand(component.leftHandSecondaryDot);
					ushort num3 = this.HandleHapticFeedbackForThisHand(component.rightHandSecondaryDot);
					if (num2 > num)
					{
						num = num2;
					}
					if (num3 > num)
					{
						num = num3;
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
		if (num > 0)
		{
			this.handDot.TriggerHapticPulse(num);
			if (this.lastHighestPulse == 0)
			{
				string text = this.handDot.side.ToString().ToLower();
				Person ourPerson = Managers.personManager.ourPerson;
				string text2 = text + " hand touches hand";
				string text3 = "hand touches hand";
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(ourPerson, text2, true);
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(ourPerson, text3, true);
			}
		}
		this.lastHighestPulse = num;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0002A384 File Offset: 0x00028784
	private ushort HandleHapticFeedbackForThisHand(Transform secondaryDot)
	{
		ushort num = 0;
		if (secondaryDot != null)
		{
			float num2 = Vector3.Distance(base.transform.position, secondaryDot.transform.position);
			if (num2 <= 0.075f)
			{
				float num3 = 1f - num2 * 13.333333f;
				num = (ushort)Mathf.Round(num3 * 1150f);
			}
		}
		return num;
	}

	// Token: 0x0400059C RID: 1436
	public GameObject handDotObject;

	// Token: 0x0400059D RID: 1437
	private HandDot handDot;

	// Token: 0x0400059E RID: 1438
	private ushort lastHighestPulse;
}
