using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x020001DC RID: 476
public class Joystick : MonoBehaviour
{
	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000EDC RID: 3804 RVA: 0x00082A49 File Offset: 0x00080E49
	// (set) Token: 0x06000EDD RID: 3805 RVA: 0x00082A51 File Offset: 0x00080E51
	public bool stickIsHeld { get; private set; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000EDE RID: 3806 RVA: 0x00082A5A File Offset: 0x00080E5A
	// (set) Token: 0x06000EDF RID: 3807 RVA: 0x00082A62 File Offset: 0x00080E62
	public bool handIsInScope { get; private set; }

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00082A6C File Offset: 0x00080E6C
	private void Start()
	{
		this.PrepareBrowserIfNeeded();
		this.dialogHand = base.transform.parent.parent.parent.parent.GetComponent<Hand>();
		if (this.dialogHand != null)
		{
			this.stickHand = this.dialogHand.otherHandScript;
		}
		if (this.dialogHand == null || this.stickHand == null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this.stickHand.side == Side.Left)
		{
			this.stick.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
		}
		this.stickHandSecondaryDot = (HandSecondaryDot)this.stickHand.GetComponentInChildren(typeof(HandSecondaryDot), true);
		this.originalLocalPosition = this.stick.transform.localPosition;
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x00082B60 File Offset: 0x00080F60
	private void Update()
	{
		if (!this.ControlledObjectExists())
		{
			return;
		}
		float num = Time.deltaTime * 2.5f;
		bool stickIsHeld = this.stickIsHeld;
		if (this.stickHand.controller.GetPressDown(CrossDevice.button_grab))
		{
			if (!this.stickIsHeld)
			{
				this.stickIsHeld = this.handIsInScope;
			}
		}
		else if (this.stickHand.controller.GetPressUp(CrossDevice.button_grab))
		{
			this.stickIsHeld = false;
		}
		if (this.stickIsHeld)
		{
			Transform parent = base.transform.parent.parent;
			Vector3 vector = this.stickHandSecondaryDot.transform.position + parent.up * -0.05f;
			this.stick.transform.position = Vector3.MoveTowards(this.stick.transform.position, vector, num);
		}
		else
		{
			this.stick.transform.localPosition = Vector3.MoveTowards(this.stick.transform.localPosition, this.originalLocalPosition, num);
		}
		this.HandleStickSoundAndHaptic(stickIsHeld);
		if (this.controllable != null)
		{
			this.ControlControllableBasedOnStick();
		}
		else if (this.browser != null)
		{
			this.ControlBrowserBasedOnStick();
		}
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x00082CB7 File Offset: 0x000810B7
	private bool ControlledObjectExists()
	{
		return (this.controllable != null && this.controllable.rigidbody != null) || this.browser != null;
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x00082CF0 File Offset: 0x000810F0
	private void HandleStickSoundAndHaptic(bool stickWasHeld)
	{
		if (this.stickIsHeld && !stickWasHeld)
		{
			Managers.soundManager.Play("pickUp", this.stick.transform, 0.25f, true, false);
		}
		else if (!this.stickIsHeld && stickWasHeld)
		{
			Managers.soundManager.Play("putDown", this.stick.transform, 0.25f, false, false);
		}
		if ((this.stickIsHeld && !stickWasHeld) || (!this.stickIsHeld && stickWasHeld))
		{
			this.stickHand.controller.TriggerHapticPulse(Universe.miniBurstPulse, EVRButtonId.k_EButton_Axis0);
			this.dialogHand.controller.TriggerHapticPulse(Universe.miniBurstPulse, EVRButtonId.k_EButton_Axis0);
		}
		if (this.stickIsHeld)
		{
			float num = Vector3.Distance(this.originalLocalPosition, this.stick.transform.localPosition);
			ushort num2 = (ushort)Mathf.Round(num * 5000f);
			if (num2 >= 100)
			{
				this.stickHand.controller.TriggerHapticPulse(num2, EVRButtonId.k_EButton_Axis0);
			}
		}
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x00082E06 File Offset: 0x00081206
	private void OnTriggerEnter(Collider other)
	{
		if (this.ColliderIsStickHandSecondaryDot(other))
		{
			this.handIsInScope = true;
		}
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x00082E1B File Offset: 0x0008121B
	private void OnTriggerExit(Collider other)
	{
		if (this.ColliderIsStickHandSecondaryDot(other))
		{
			this.handIsInScope = false;
		}
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x00082E30 File Offset: 0x00081230
	private bool ColliderIsStickHandSecondaryDot(Collider other)
	{
		return other.name == "HandSecondaryDot" && other.transform.parent.GetComponent<Hand>() == this.stickHand;
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x00082E68 File Offset: 0x00081268
	private float GetNormalizedStickDistance(float currentValue, float min, float max)
	{
		float num = Mathf.Clamp(currentValue, min, max);
		if (num < 0f)
		{
			num = -(Mathf.Abs(num) / Mathf.Abs(min));
		}
		else if (num > 0f)
		{
			num /= max;
		}
		return num;
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x00082EB0 File Offset: 0x000812B0
	private void ControlControllableBasedOnStick()
	{
		if (this.dialogHand.controller.GetPressDown(CrossDevice.button_grabTip))
		{
			this.controllable.TriggerEventAsStateAuthority(StateListener.EventType.OnTriggered, string.Empty);
		}
		Vector3 vector = this.stick.transform.localPosition - this.originalLocalPosition;
		float normalizedStickDistance = this.GetNormalizedStickDistance(vector.z, -0.135f, 0.135f);
		float normalizedStickDistance2 = this.GetNormalizedStickDistance(vector.y, -0.12f, 0.09847f);
		float normalizedStickDistance3 = this.GetNormalizedStickDistance(vector.x, -0.135f, 0.135f);
		bool flag = false;
		string text = null;
		string text2 = null;
		string text3 = null;
		if (vector.z <= -0.0675f)
		{
			text = "backward";
		}
		else if (vector.z >= 0.0675f)
		{
			text = "forward";
		}
		if (vector.y <= -0.06f)
		{
			text2 = "down";
		}
		else if (vector.y >= 0.049235f)
		{
			text2 = "up";
		}
		if (vector.x <= -0.0675f)
		{
			text3 = "left";
		}
		else if (vector.x >= 0.0675f)
		{
			text3 = "right";
		}
		if (text != null || text2 != null || text3 != null)
		{
			flag = true;
		}
		Component[] componentsInChildren = this.controllable.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (flag)
			{
				if (text != null)
				{
					thingPart.TriggerEventAsStateAuthority(StateListener.EventType.OnJoystickControlled, text);
				}
				if (text2 != null)
				{
					thingPart.TriggerEventAsStateAuthority(StateListener.EventType.OnJoystickControlled, text2);
				}
				if (text3 != null)
				{
					thingPart.TriggerEventAsStateAuthority(StateListener.EventType.OnJoystickControlled, text3);
				}
				thingPart.TriggerEventAsStateAuthority(StateListener.EventType.OnJoystickControlled, string.Empty);
			}
			if (thingPart.joystickToControllablePart != null)
			{
				WheelCollider wheelCollider = ((!thingPart.isControllableWheel) ? null : thingPart.GetComponent<WheelCollider>());
				if (thingPart.joystickToControllablePart.rotation != Vector3.zero)
				{
					Vector3? originalLocalRotation = thingPart.originalLocalRotation;
					if (originalLocalRotation == null)
					{
						thingPart.originalLocalRotation = new Vector3?(thingPart.transform.localEulerAngles);
					}
					Vector3 vector2 = new Vector3(thingPart.joystickToControllablePart.rotation.x * normalizedStickDistance2 * 90f, thingPart.joystickToControllablePart.rotation.y * normalizedStickDistance3 * 90f, thingPart.joystickToControllablePart.rotation.z * normalizedStickDistance * 90f);
					if (thingPart.isControllableWheel)
					{
						wheelCollider.steerAngle = vector2.y;
					}
					else
					{
						Transform transform = thingPart.transform;
						Vector3? originalLocalRotation2 = thingPart.originalLocalRotation;
						transform.localEulerAngles = originalLocalRotation2.Value;
						thingPart.transform.Rotate(vector2);
					}
				}
				Vector3 vector3 = Vector3.zero;
				float num = normalizedStickDistance3 * thingPart.joystickToControllablePart.thrust.x * 50f;
				vector3 += thingPart.transform.right * num;
				if (thingPart.isControllableWheel)
				{
					float num2 = normalizedStickDistance * thingPart.joystickToControllablePart.thrust.z * 100f;
					wheelCollider.motorTorque = num2;
				}
				else
				{
					float num3 = normalizedStickDistance * thingPart.joystickToControllablePart.thrust.z * 50f;
					vector3 += thingPart.transform.forward * num3;
				}
				float num4 = normalizedStickDistance2 * thingPart.joystickToControllablePart.thrust.y * 50f;
				vector3 += thingPart.transform.up * num4;
				if (vector3 != Vector3.zero)
				{
					this.controllable.rigidbody.AddForceAtPosition(vector3, thingPart.transform.position, ForceMode.Force);
				}
			}
		}
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x000832C0 File Offset: 0x000816C0
	private void ControlBrowserBasedOnStick()
	{
		Vector3 vector = this.stick.transform.localPosition - this.originalLocalPosition;
		Dictionary<KeyCode, bool> dictionary = new Dictionary<KeyCode, bool>();
		foreach (KeyValuePair<KeyCode, bool> keyValuePair in this.keyDown)
		{
			dictionary[keyValuePair.Key] = keyValuePair.Value;
		}
		this.keyDown[this.keyCodeDown] = vector.z <= -0.025f;
		this.keyDown[this.keyCodeUp] = vector.z >= 0.025f;
		this.keyDown[this.keyCodeLeft] = vector.x <= -0.025f;
		this.keyDown[this.keyCodeRight] = vector.x >= 0.025f;
		this.keyDown[this.keyCodeAction] = this.dialogHand.controller.GetPressDown(CrossDevice.button_grabTip);
		foreach (KeyValuePair<KeyCode, bool> keyValuePair2 in this.keyDown)
		{
			bool value = keyValuePair2.Value;
			if (dictionary.ContainsKey(keyValuePair2.Key) && value != dictionary[keyValuePair2.Key])
			{
				this.browser.PressKey(keyValuePair2.Key, (!value) ? KeyAction.Release : KeyAction.Press);
			}
		}
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x00083490 File Offset: 0x00081890
	private void OnDestroy()
	{
		this.ReleaseBrowserIfNeeded();
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x00083498 File Offset: 0x00081898
	private void PrepareBrowserIfNeeded()
	{
		if (this.browser != null)
		{
			if (this.useArrowKeysForBrowser)
			{
				this.keyCodeUp = KeyCode.UpArrow;
				this.keyCodeDown = KeyCode.DownArrow;
				this.keyCodeLeft = KeyCode.LeftArrow;
				this.keyCodeRight = KeyCode.RightArrow;
			}
			else
			{
				this.keyCodeUp = KeyCode.W;
				this.keyCodeDown = KeyCode.S;
				this.keyCodeLeft = KeyCode.A;
				this.keyCodeRight = KeyCode.D;
			}
			this.browser.forceFocus = true;
		}
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0008351E File Offset: 0x0008191E
	private void ReleaseBrowserIfNeeded()
	{
		if (this.browser != null)
		{
			this.browser.forceFocus = false;
		}
	}

	// Token: 0x04000FA8 RID: 4008
	public GameObject stick;

	// Token: 0x04000FAA RID: 4010
	public Thing controllable;

	// Token: 0x04000FAB RID: 4011
	public Browser browser;

	// Token: 0x04000FAC RID: 4012
	public bool useArrowKeysForBrowser;

	// Token: 0x04000FAD RID: 4013
	private PointerUIBase pointer;

	// Token: 0x04000FAE RID: 4014
	private Hand dialogHand;

	// Token: 0x04000FAF RID: 4015
	private Hand stickHand;

	// Token: 0x04000FB0 RID: 4016
	private HandSecondaryDot stickHandSecondaryDot;

	// Token: 0x04000FB1 RID: 4017
	private Vector3 originalLocalPosition = Vector3.zero;

	// Token: 0x04000FB2 RID: 4018
	private Vector3 stickTargetPosition = Vector3.zero;

	// Token: 0x04000FB4 RID: 4020
	private KeyCode keyCodeUp;

	// Token: 0x04000FB5 RID: 4021
	private KeyCode keyCodeDown;

	// Token: 0x04000FB6 RID: 4022
	private KeyCode keyCodeLeft;

	// Token: 0x04000FB7 RID: 4023
	private KeyCode keyCodeRight;

	// Token: 0x04000FB8 RID: 4024
	private KeyCode keyCodeAction = KeyCode.Space;

	// Token: 0x04000FB9 RID: 4025
	private Dictionary<KeyCode, bool> keyDown = new Dictionary<KeyCode, bool>();
}
