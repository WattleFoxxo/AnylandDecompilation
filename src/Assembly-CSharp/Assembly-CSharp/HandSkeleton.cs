using System;
using UnityEngine;
using Valve.VR;

// Token: 0x020000E5 RID: 229
public class HandSkeleton : MonoBehaviour
{
	// Token: 0x060007AA RID: 1962 RVA: 0x0002A46E File Offset: 0x0002886E
	private void Awake()
	{
		this.SetUpReferences();
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x0002A478 File Offset: 0x00028878
	private void Update()
	{
		if (Our.dynamicHands && this.hand.controller != null)
		{
			if (this.handModelRenderer.enabled)
			{
				float num = this.Ease(this.hand.controller.GetAxis(EVRButtonId.k_EButton_Axis1)[0]);
				if (this.handDot.holdableInHand != this.holdableInHand)
				{
					this.holdableInHand = this.handDot.holdableInHand;
					this.holdableHasTriggers = this.HoldableHasTriggers(this.holdableInHand);
					this.holdableIsTool = this.HoldableIsTool(this.holdableInHand);
				}
				this.AdjustBasedOnIndexFingerStrength(num);
				float num2 = ((!CrossDevice.hasSeparateTriggerAndGrab) ? (num * 0.1f) : this.Ease(this.hand.controller.GetAxis(EVRButtonId.k_EButton_Axis2)[0]));
				this.AdjustBasedOnHandGripStrength(num2);
			}
			else if (!this.handDotRendererEnabled)
			{
				this.handDotRendererEnabled = true;
				this.handDotRenderer.enabled = true;
			}
		}
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x0002A58C File Offset: 0x0002898C
	private bool HoldableHasTriggers(GameObject holdable)
	{
		bool flag = false;
		if (this.holdableInHand != null)
		{
			Component[] componentsInChildren = holdable.GetComponentsInChildren(typeof(ThingPart), true);
			foreach (ThingPart thingPart in componentsInChildren)
			{
				for (int j = 0; j < thingPart.states.Count; j++)
				{
					foreach (StateListener stateListener in thingPart.states[j].listeners)
					{
						flag = stateListener.eventType == StateListener.EventType.OnTriggered || stateListener.eventType == StateListener.EventType.OnUntriggered;
						if (flag)
						{
							return flag;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x0002A688 File Offset: 0x00028A88
	private bool HoldableIsTool(GameObject holdable)
	{
		string text = string.Empty;
		if (holdable != null)
		{
			text = holdable.name;
		}
		return text == "Brush" || text == "VertexMover";
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x0002A6CC File Offset: 0x00028ACC
	public Transform GetRingBone()
	{
		return this.thumbBones[2];
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0002A6D8 File Offset: 0x00028AD8
	private void AdjustBasedOnIndexFingerStrength(float strength)
	{
		if (this.holdableInHand != null && !this.holdableIsTool && !this.holdableHasTriggers)
		{
			strength = 0.85f + (strength - 0.5f) * 0.1f;
		}
		this.handDotRendererEnabled = strength <= 0.05f && this.holdableInHand == null;
		this.handDotRenderer.enabled = this.handDotRendererEnabled;
		if (this.holdableInHand != null)
		{
			this.AdjustBone(0, this.indexFingerBones, strength, this.indexFingerStartPosition, this.indexFingerStartRotation, new Vector3?(new Vector3(0.04768f, -0.02024f, 0.06786f)), new Vector3(39.1f, 47.472f, 4.304f));
			this.AdjustBone(1, this.indexFingerBones, strength, this.indexFingerStartPosition, this.indexFingerStartRotation, new Vector3?(new Vector3(0f, 0f, 0.11591f)), new Vector3(25.337f, -17.997f, -9.274f));
			this.AdjustBone(2, this.indexFingerBones, strength, this.indexFingerStartPosition, this.indexFingerStartRotation, new Vector3?(new Vector3(0f, 0f, 0.14579f)), new Vector3(47.183f, -56.587f, -22.801f));
			this.AdjustBone(3, this.indexFingerBones, strength, this.indexFingerStartPosition, this.indexFingerStartRotation, new Vector3?(new Vector3(0f, 0f, 0.1552f)), new Vector3(-45.229f, -75.728f, 70.287f));
		}
		else
		{
			this.AdjustBone(0, this.indexFingerBones, strength, this.indexFingerStartPosition, this.indexFingerStartRotation, null, new Vector3(37.588f, 59.823f, 16.907f));
			this.AdjustBone(1, this.indexFingerBones, strength, this.indexFingerStartPosition, this.indexFingerStartRotation, null, new Vector3(18.615f, -8.569f, -5.259f));
			this.AdjustBone(2, this.indexFingerBones, strength, this.indexFingerStartPosition, this.indexFingerStartRotation, null, new Vector3(27.355f, -17.152f, -1.97f));
			this.AdjustBone(0, this.thumbBones, strength, this.thumbStartPosition, this.thumbStartRotation, new Vector3?(new Vector3(-0.10677f, 0.09338f, 0.08044f)), new Vector3(46.469f, -1.428f, 8.126f));
			this.AdjustBone(1, this.thumbBones, strength, this.thumbStartPosition, this.thumbStartRotation, new Vector3?(new Vector3(0.004f, -0.007f, 0.108f)), new Vector3(16.83f, 9.379001f, -14.448f));
			this.AdjustBone(2, this.thumbBones, strength, this.thumbStartPosition, this.thumbStartRotation, new Vector3?(new Vector3(-0.003f, -0.005f, 0.116f)), new Vector3(24.219f, -11.137f, 46.332f));
			this.AdjustBone(3, this.thumbBones, strength, this.thumbStartPosition, this.thumbStartRotation, new Vector3?(new Vector3(0f, 0f, 0.122f)), new Vector3(-45.589f, 75.36201f, -69.916f));
		}
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x0002AA48 File Offset: 0x00028E48
	private void AdjustBasedOnHandGripStrength(float strength)
	{
		if (this.holdableInHand != null)
		{
			if (this.holdableIsTool)
			{
				strength = 0.15f;
			}
			else if (this.holdableHasTriggers)
			{
				if (CrossDevice.hasSeparateTriggerAndGrab)
				{
					strength = 1f - (1f - strength) * 0.15f;
				}
				else
				{
					strength = 1f;
				}
			}
			else if (CrossDevice.hasSeparateTriggerAndGrab)
			{
				strength = 0.5f + (strength - 0.5f) * 0.1f;
			}
			else
			{
				strength = 0.5f;
			}
		}
		this.AdjustBone(0, this.handGripBones, strength, this.handGripStartPosition, this.handGripStartRotation, new Vector3?(new Vector3(0.108f, -0.066f, -0.04f)), new Vector3(64.264f, -22.693f, -50.036f));
		this.AdjustBone(1, this.handGripBones, strength, this.handGripStartPosition, this.handGripStartRotation, new Vector3?(new Vector3(-0.004f, 0.004f, 0.13f)), new Vector3(27.969f, -67.08701f, 51.006f));
		this.AdjustBone(2, this.handGripBones, strength, this.handGripStartPosition, this.handGripStartRotation, new Vector3?(new Vector3(0f, 0f, 0.10234f)), new Vector3(-21.941f, -21.946f, 17.307f));
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0002ABB8 File Offset: 0x00028FB8
	private void HandleDebugInfo()
	{
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0002ABBC File Offset: 0x00028FBC
	private void AdjustBone(int i, Transform[] bone, float strength, Vector3[] positionStart, Vector3[] rotationStart, Vector3? positionEnd, Vector3 rotationEnd)
	{
		if (positionEnd != null)
		{
			bone[i].localPosition = Vector3.Lerp(positionStart[i], positionEnd.Value, strength);
		}
		bone[i].localEulerAngles = Misc.AngleLerp(rotationStart[i], rotationEnd, strength);
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0002AC14 File Offset: 0x00029014
	private void SetUpReferences()
	{
		this.hand = base.transform.parent.parent.parent.GetComponent<Hand>();
		this.handDot = this.hand.transform.GetChild(0).GetComponent<HandDot>();
		this.handDotRenderer = this.handDot.GetComponent<Renderer>();
		this.SetUpBoneReferences(this.indexFingerBones, this.indexFingerStartPosition, this.indexFingerStartRotation, "IndexFinger", false);
		this.SetUpBoneReferences(this.handGripBones, this.handGripStartPosition, this.handGripStartRotation, "HandGrip", false);
		this.SetUpBoneReferences(this.thumbBones, this.thumbStartPosition, this.thumbStartRotation, "Thumb", false);
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x0002ACC8 File Offset: 0x000290C8
	private void SetUpBoneReferences(Transform[] bones, Vector3[] startPosition, Vector3[] startRotation, string boneRootName, bool isWrapper = false)
	{
		if (isWrapper)
		{
			bones[0] = base.transform;
		}
		else
		{
			bones[0] = base.transform.Find(boneRootName);
			for (int i = 1; i < bones.Length; i++)
			{
				bones[i] = bones[i - 1].GetChild(0);
			}
		}
		for (int j = 0; j < bones.Length; j++)
		{
			startPosition[j] = bones[j].localPosition;
			startRotation[j] = bones[j].localEulerAngles;
		}
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x0002AD58 File Offset: 0x00029158
	private float Ease(float strength)
	{
		return Mathfx.EaseInOut(0f, 1f, strength);
	}

	// Token: 0x0400059F RID: 1439
	private Hand hand;

	// Token: 0x040005A0 RID: 1440
	private HandDot handDot;

	// Token: 0x040005A1 RID: 1441
	public Renderer handModelRenderer;

	// Token: 0x040005A2 RID: 1442
	private Renderer handDotRenderer;

	// Token: 0x040005A3 RID: 1443
	private Transform[] indexFingerBones = new Transform[4];

	// Token: 0x040005A4 RID: 1444
	private Vector3[] indexFingerStartPosition = new Vector3[4];

	// Token: 0x040005A5 RID: 1445
	private Vector3[] indexFingerStartRotation = new Vector3[4];

	// Token: 0x040005A6 RID: 1446
	private Transform[] handGripBones = new Transform[4];

	// Token: 0x040005A7 RID: 1447
	private Vector3[] handGripStartPosition = new Vector3[4];

	// Token: 0x040005A8 RID: 1448
	private Vector3[] handGripStartRotation = new Vector3[4];

	// Token: 0x040005A9 RID: 1449
	private Transform[] thumbBones = new Transform[4];

	// Token: 0x040005AA RID: 1450
	private Vector3[] thumbStartPosition = new Vector3[4];

	// Token: 0x040005AB RID: 1451
	private Vector3[] thumbStartRotation = new Vector3[4];

	// Token: 0x040005AC RID: 1452
	private float lastIndexFingerStrength = -1f;

	// Token: 0x040005AD RID: 1453
	private GameObject holdableInHand;

	// Token: 0x040005AE RID: 1454
	private bool holdableHasTriggers;

	// Token: 0x040005AF RID: 1455
	private bool holdableIsTool;

	// Token: 0x040005B0 RID: 1456
	private bool handDotRendererEnabled;
}
