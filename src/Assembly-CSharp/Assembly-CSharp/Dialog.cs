using System;
using System.Collections;
using System.Collections.Generic;
using DaikonForge.VoIP;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class Dialog : MonoBehaviour
{
	// Token: 0x17000146 RID: 326
	// (get) Token: 0x0600083D RID: 2109 RVA: 0x0002E438 File Offset: 0x0002C838
	// (set) Token: 0x0600083E RID: 2110 RVA: 0x0002E440 File Offset: 0x0002C840
	public bool isBig { get; private set; }

	// Token: 0x0600083F RID: 2111 RVA: 0x0002E44C File Offset: 0x0002C84C
	public void Init(GameObject sourceGameObject, bool _isStartDialog = false, bool isBig = false, bool isDarker = false)
	{
		this.isBig = isBig;
		this.isStartDialog = _isStartDialog;
		this.dialogPartsObject = Managers.treeManager.GetObject("/DialogParts");
		this.gameObject = sourceGameObject;
		this.transform = this.gameObject.transform;
		this.isDarker = isDarker;
		Transform handTransform = this.GetHandTransform();
		if (handTransform != null)
		{
			this.handSide = ((!(handTransform.name == "HandCoreLeft")) ? Side.Right : Side.Left);
		}
		this.transform.localPosition = Vector3.zero;
		this.transform.rotation = Quaternion.identity;
		this.transform.localRotation = Quaternion.identity;
		this.tabCounter = 0;
		this.InitPosition(1f);
		if (handTransform != null)
		{
			this.hand = handTransform.GetComponent<Hand>();
		}
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x0002E52C File Offset: 0x0002C92C
	private void InitPosition(float scale = 1f)
	{
		Transform handTransform = this.GetHandTransform();
		if (handTransform != null)
		{
			float num = (float)((this.handSide != Side.Left) ? (-1) : 1);
			if (this.isStartDialog)
			{
				if (this.handSide == Side.Left)
				{
					this.transform.localPosition = new Vector3(0.02329636f, 0.02587959f, -0.006536559f);
					this.transform.localEulerAngles = new Vector3(-20.628f, -146.48f, 38.201f);
				}
				else
				{
					this.transform.localPosition = new Vector3(0.02117517f, 0.0303622f, 0.002567765f);
					this.transform.localEulerAngles = new Vector3(-20.005f, -34.995f, -34.106f);
				}
			}
			else
			{
				this.transform.position -= handTransform.forward * 0.14f * scale;
				this.transform.position -= handTransform.up * -0.04f * scale;
				this.transform.position -= handTransform.right * -0.09f * num * scale;
			}
		}
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x0002E688 File Offset: 0x0002CA88
	private Transform GetHandTransform()
	{
		Transform transform = null;
		if (this.isStartDialog)
		{
			if (this.transform.parent != null)
			{
				transform = this.transform.parent.parent.parent.parent.parent.parent.parent;
			}
		}
		else
		{
			transform = this.transform.parent;
		}
		return transform;
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x0002E6F4 File Offset: 0x0002CAF4
	public void Update()
	{
		this.HandleHapticPulse();
		this.ReactToOnClick();
		this.ReactToOnClickInWrapper(this.wrapper);
		this.ReactToOnClickInWrapper(this.backsideWrapper);
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x0002E71A File Offset: 0x0002CB1A
	private void HandleHapticPulse()
	{
		if (this.hapticPulseOn && this.hand != null)
		{
			this.hand.TriggerHapticPulse(900);
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x0002E748 File Offset: 0x0002CB48
	public void DoLongerHapticPulse()
	{
		if (!this.hapticPulseOn)
		{
			this.hapticPulseOn = true;
			base.Invoke("StopHapticPulse", 1f);
		}
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0002E76C File Offset: 0x0002CB6C
	private void StopHapticPulse()
	{
		this.hapticPulseOn = false;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x0002E778 File Offset: 0x0002CB78
	public void ReactToOnClick()
	{
		DialogPartData stateChange = this.GetStateChange(false);
		if (stateChange != null)
		{
			this.OnClick(stateChange.contextName, stateChange.contextId, stateChange.state, stateChange.button);
		}
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x0002E7B4 File Offset: 0x0002CBB4
	protected void ReactToOnClickInWrapper(GameObject wrapper)
	{
		if (wrapper != null)
		{
			this.SetUiWrapper(wrapper);
			DialogPartData stateChange = this.GetStateChange(false);
			if (stateChange != null)
			{
				this.OnClick(stateChange.contextName, stateChange.contextId, stateChange.state, stateChange.button);
			}
			this.SetUiWrapper(this.gameObject);
		}
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x0002E80C File Offset: 0x0002CC0C
	public virtual void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0002E810 File Offset: 0x0002CC10
	protected void UpdateScale()
	{
		float ourScale = Managers.personManager.GetOurScale();
		if (ourScale != 1f)
		{
			this.transform.localScale *= ourScale;
			this.transform.localPosition = Vector3.zero;
			this.InitPosition(ourScale);
			if (!this.gameObject.activeSelf)
			{
				this.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x0002E880 File Offset: 0x0002CC80
	protected GameObject GetUiWrapper()
	{
		return new GameObject
		{
			name = "Wrapper",
			transform = 
			{
				parent = this.transform,
				localPosition = Vector3.zero,
				localRotation = Quaternion.identity
			}
		};
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0002E8D0 File Offset: 0x0002CCD0
	protected void SetUiWrapper(GameObject wrapper)
	{
		this.transform = wrapper.transform;
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0002E8E0 File Offset: 0x0002CCE0
	protected GameObject GetPrefabInstance(string name)
	{
		string text = "DialogParts/Prefabs/" + name;
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load(text) as GameObject, Vector3.zero, Quaternion.identity);
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		return gameObject;
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0002E928 File Offset: 0x0002CD28
	protected GameObject GetAndAttachPrefabInstance(string name)
	{
		string text = "DialogParts/Prefabs/" + name;
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load(text) as GameObject, this.transform);
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		return gameObject;
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0002E96A File Offset: 0x0002CD6A
	protected Transform GetTransform()
	{
		return this.transform;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0002E974 File Offset: 0x0002CD74
	protected Transform Add(string name, int xOnFundament = 0, int yOnFundament = 0, bool isInTreeInsteadOfResources = false)
	{
		GameObject gameObject2;
		if (isInTreeInsteadOfResources)
		{
			GameObject gameObject = Misc.FindObject(this.dialogPartsObject, name);
			gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject, Vector3.zero, Quaternion.identity);
			gameObject2.name = Misc.RemoveCloneFromName(gameObject2.name);
		}
		else
		{
			gameObject2 = this.GetPrefabInstance(name);
		}
		gameObject2.SetActive(true);
		gameObject2.transform.parent = this.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		Vector3 localPosition = gameObject2.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		gameObject2.transform.localPosition = localPosition;
		return gameObject2.transform;
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0002EA48 File Offset: 0x0002CE48
	protected void AddBrush()
	{
		GameObject prefabInstance = this.GetPrefabInstance("Brush");
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		prefabInstance.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
		float num = 530f;
		if (this.handSide == Side.Right)
		{
			num = -540f;
		}
		float num2 = 50f;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += num * this.scaleFactor;
		localPosition.z -= num2 * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0002EB1C File Offset: 0x0002CF1C
	protected GameObject AddFundament()
	{
		this.fundament = this.GetPrefabInstance("Fundament");
		this.fundament.transform.parent = this.transform;
		this.fundament.transform.localPosition = Vector3.zero;
		this.fundament.transform.localRotation = Quaternion.identity;
		if (this.isBig)
		{
			this.fundament.transform.localScale = new Vector3(this.fundament.transform.localScale.x * 2f, this.fundament.transform.localScale.y, this.fundament.transform.localScale.z * 2f);
		}
		if (this.isDarker)
		{
			this.TurnDarker(this.fundament.transform.Find("FundamentMesh"));
		}
		return this.fundament;
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0002EC1C File Offset: 0x0002D01C
	private void TurnDarker(Transform thisTransform)
	{
		Renderer component = thisTransform.GetComponent<Renderer>();
		int num = -35;
		Color32 color = component.material.color;
		component.material.color = new Color32((byte)((int)color.r + num), (byte)((int)color.g + num), (byte)((int)color.b + num), color.a);
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0002EC80 File Offset: 0x0002D080
	public GameObject AddButton(string contextName, string contextId, string text, string buttonName, int xOnFundament = 0, int yOnFundament = 0, string icon = null, bool state = false, float textSizeFactor = 1f, TextColor textColor = TextColor.Default, float iconQuadSize = 1f, float verticalTextOffset = 0f, float horizontalTextOffset = 0f, string buttonColor = "", bool useMixedCase = false, bool autoBacksideAlign = false, TextAlignment align = TextAlignment.Left, bool isOnBackside = false, bool ignoreTextPositioning = false)
	{
		if (autoBacksideAlign && this.handSide == Side.Right)
		{
			xOnFundament *= -1;
		}
		GameObject prefabInstance = this.GetPrefabInstance(buttonName);
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		DialogPart component = prefabInstance.GetComponent<DialogPart>();
		component.contextName = contextName;
		component.contextId = contextId;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		Transform transform = prefabInstance.transform.Find("Text");
		if (transform != null)
		{
			if (text == null)
			{
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
			else
			{
				TextMesh component2 = transform.GetComponent<TextMesh>();
				if (!useMixedCase)
				{
					text = text.ToUpper();
				}
				component2.text = text;
				this.ApplyTextSizeFactor(transform, textSizeFactor, verticalTextOffset, horizontalTextOffset, ignoreTextPositioning);
				if (textColor != TextColor.Default)
				{
					Renderer component3 = component2.GetComponent<Renderer>();
					component3.material.color = this.GetTextColorAsColor(textColor);
				}
				if (align == TextAlignment.Center)
				{
					component2.alignment = align;
					component2.anchor = TextAnchor.UpperCenter;
					Vector3 localPosition2 = component2.transform.localPosition;
					localPosition2.x = 0f;
					component2.transform.localPosition = localPosition2;
				}
				else if (align == TextAlignment.Right)
				{
					component2.alignment = align;
					component2.anchor = TextAnchor.UpperRight;
					Vector3 localPosition3 = component2.transform.localPosition;
					localPosition3.x *= -1f;
					component2.transform.localPosition = localPosition3;
				}
			}
		}
		Transform transform2 = prefabInstance.transform.Find("IconQuad");
		if (icon == null)
		{
			if (transform2 != null)
			{
				global::UnityEngine.Object.Destroy(transform2.gameObject);
			}
		}
		else
		{
			Managers.thingManager.LoadMaterial(transform2.gameObject, "DialogIconMaterials/" + icon);
			if (iconQuadSize != 1f)
			{
				transform2.localScale = new Vector3(transform2.localScale.x * iconQuadSize, transform2.localScale.y * iconQuadSize, transform2.localScale.z * iconQuadSize);
			}
		}
		if (state)
		{
			this.ApplyEmissionColor(prefabInstance.transform.Find("Cube"), true);
		}
		if (buttonColor != string.Empty)
		{
			this.SetButtonColor(prefabInstance, Misc.ColorStringToColor(buttonColor));
		}
		if (isOnBackside)
		{
			prefabInstance.transform.localRotation *= Quaternion.Euler(0f, 0f, 180f);
		}
		return prefabInstance;
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0002EF6C File Offset: 0x0002D36C
	protected void AddCloseButton()
	{
		int num = ((!this.isBig) ? 0 : 470);
		int num2 = ((!this.isBig) ? 0 : (-480));
		this.mainCloseButton = this.AddModelButton("ButtonClosePositioned", "close", null, num, num2, false);
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0002EFC1 File Offset: 0x0002D3C1
	protected void RemoveCloseButton()
	{
		global::UnityEngine.Object.Destroy(this.mainCloseButton);
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0002EFD0 File Offset: 0x0002D3D0
	protected void AddTab(string text)
	{
		if (this.tabName == string.Empty)
		{
			this.tabName = text;
		}
		Vector2 tabPosition = this.GetTabPosition();
		this.AddButton("tabSwitch", text, text, "ButtonCompactNoIcon", (int)tabPosition.x, (int)tabPosition.y, null, this.tabName == text, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		this.tabCounter++;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0002F060 File Offset: 0x0002D460
	protected void AddTabStyledButton(string name, string text)
	{
		Vector2 tabPosition = this.GetTabPosition();
		this.AddButton(name, null, text, "ButtonCompactNoIcon", (int)tabPosition.x, (int)tabPosition.y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		this.tabCounter++;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0002F0C4 File Offset: 0x0002D4C4
	private Vector2 GetTabPosition()
	{
		Vector2 vector;
		vector.y = -920f;
		vector.x = (float)(-640 + this.tabCounter * 450);
		return vector;
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0002F0F8 File Offset: 0x0002D4F8
	protected void AddBackButton()
	{
		int num = ((!this.isBig) ? (-436) : (-930));
		int num2 = ((!this.isBig) ? (-427) : (-920));
		this.AddModelButton("ButtonBack", "back", null, num, num2, false);
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0002F150 File Offset: 0x0002D550
	protected void AddMinimizeButton()
	{
		this.AddModelButton("MinimizeButton", "minimize", null, 0, 0, false);
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x0002F168 File Offset: 0x0002D568
	public GameObject AddModelButton(string name, string contextName, string contextId = null, int xOnFundament = 0, int yOnFundament = 0, bool autoBacksideAlign = false)
	{
		if (autoBacksideAlign && this.handSide == Side.Right)
		{
			xOnFundament *= -1;
		}
		GameObject prefabInstance = this.GetPrefabInstance(name);
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		if (xOnFundament != 0 || yOnFundament != 0)
		{
			Vector3 localPosition = prefabInstance.transform.localPosition;
			localPosition.x += (float)xOnFundament * this.scaleFactor;
			localPosition.z -= (float)yOnFundament * this.scaleFactor;
			prefabInstance.transform.localPosition = localPosition;
		}
		DialogPart component = prefabInstance.GetComponent<DialogPart>();
		component.contextName = contextName;
		component.contextId = contextId;
		return prefabInstance;
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x0002F23C File Offset: 0x0002D63C
	protected void AddSeparator(int x = 0, int y = 0, bool isGray = false)
	{
		string text = "Separator" + ((!isGray) ? string.Empty : "Gray");
		Transform transform = this.Add(text, x + 34, y, false);
		if (this.isBig)
		{
			Vector3 localScale = transform.localScale;
			localScale.x *= 2f;
			transform.localScale = localScale;
			Vector3 localPosition = transform.localPosition;
			localPosition.x += 0.01f;
			transform.localPosition = localPosition;
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0002F2C4 File Offset: 0x0002D6C4
	private TextAlignment MirrorTextAlignment(TextAlignment alignment)
	{
		if (alignment == TextAlignment.Left)
		{
			alignment = TextAlignment.Right;
		}
		else if (alignment == TextAlignment.Right)
		{
			alignment = TextAlignment.Left;
		}
		return alignment;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0002F2E0 File Offset: 0x0002D6E0
	public TextMesh AddSideHeadline(string text)
	{
		TextMesh textMesh = this.AddLabel(text, -452, 0, 1.1f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		textMesh.transform.Rotate(new Vector3(0f, 0f, 90f));
		return textMesh;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0002F32C File Offset: 0x0002D72C
	public TextMesh AddLabel(string text, int xOnFundament = 0, int yOnFundament = 0, float textSizeFactor = 1f, bool isOnBackside = false, TextColor textColor = TextColor.Default, bool allowMixedCase = false, TextAlignment align = TextAlignment.Left, int maxLineLength = -1, float lineSpacing = 1f, bool autoBacksideAlign = false, TextAnchor anchor = TextAnchor.MiddleLeft)
	{
		if (textColor == TextColor.Default)
		{
			textColor = this.customDefaultTextColor;
		}
		if (maxLineLength != -1)
		{
			text = Misc.WrapWithNewlines(text, maxLineLength, -1);
		}
		if (autoBacksideAlign && this.handSide == Side.Right)
		{
			xOnFundament *= -1;
			align = this.MirrorTextAlignment(align);
		}
		GameObject prefabInstance = this.GetPrefabInstance("Label");
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		Transform transform = prefabInstance.transform.Find("Text");
		TextMesh component = transform.GetComponent<TextMesh>();
		if (align == TextAlignment.Center)
		{
			component.alignment = align;
			component.anchor = TextAnchor.UpperCenter;
		}
		else if (align == TextAlignment.Right)
		{
			component.alignment = align;
			component.anchor = TextAnchor.UpperRight;
		}
		if (anchor != TextAnchor.MiddleLeft)
		{
			component.anchor = anchor;
		}
		if (text != null)
		{
			if (!allowMixedCase)
			{
				text = text.ToUpper();
			}
			component.text = text;
		}
		else
		{
			component.text = string.Empty;
		}
		if (textSizeFactor != 1f)
		{
			transform.localScale = new Vector3(transform.localScale.x * textSizeFactor, transform.localScale.y * textSizeFactor, transform.localScale.z * textSizeFactor);
		}
		if (textColor != TextColor.Default)
		{
			Renderer component2 = component.GetComponent<Renderer>();
			component2.material.color = this.GetTextColorAsColor(textColor);
		}
		if (isOnBackside)
		{
			component.transform.localPosition += Vector3.down * 0.03f;
			component.transform.localRotation *= Quaternion.Euler(0f, 180f, 0f);
		}
		if (lineSpacing != 1f)
		{
			component.lineSpacing = lineSpacing;
		}
		return component;
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0002F56C File Offset: 0x0002D96C
	protected Transform AddHeadline(string text, int xOnFundament = -370, int yOnFundament = -460, TextColor textColor = TextColor.Default, TextAlignment align = TextAlignment.Left, bool allowMixedCase = false)
	{
		if (textColor == TextColor.Default)
		{
			textColor = this.customDefaultTextColor;
		}
		if (this.isBig && xOnFundament == -370 && yOnFundament == -460)
		{
			xOnFundament = -840;
			yOnFundament = -950;
		}
		GameObject prefabInstance = this.GetPrefabInstance("Headline");
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		Transform transform = prefabInstance.transform.Find("Text");
		TextMesh component = transform.GetComponent<TextMesh>();
		if (!allowMixedCase)
		{
			text = text.ToUpper();
		}
		component.text = text;
		if (align == TextAlignment.Center)
		{
			component.alignment = align;
			component.anchor = TextAnchor.UpperCenter;
		}
		if (textColor != TextColor.Default)
		{
			Renderer component2 = component.GetComponent<Renderer>();
			component2.material.color = this.GetTextColorAsColor(textColor);
		}
		return transform;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0002F6AC File Offset: 0x0002DAAC
	protected void AddAreaWithPeopleCountLabel(string areaName, int peopleCount, int xOnFundament = 0, int yOnFundament = 0)
	{
		GameObject prefabInstance = this.GetPrefabInstance("AreaWithPeopleCountLabel");
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		Transform transform = prefabInstance.transform.Find("Text");
		TextMesh component = transform.GetComponent<TextMesh>();
		component.text = Misc.Truncate(areaName, 22, true).ToUpper();
		this.AddPeopleCountToButton(prefabInstance, peopleCount);
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0002F778 File Offset: 0x0002DB78
	protected void ApplyEmissionColorToShape(GameObject thisParentObject, bool isActive = true)
	{
		Transform transform = thisParentObject.transform.Find("Shape");
		this.ApplyEmissionColor(transform, isActive);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0002F79E File Offset: 0x0002DB9E
	protected void ApplyEmissionColor(Transform thisTransform, bool isActive = true)
	{
		if (thisTransform != null)
		{
			this.ApplyEmissionColor(thisTransform.gameObject, isActive);
		}
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0002F7BC File Offset: 0x0002DBBC
	protected void ApplyEmissionColor(GameObject thisObject, bool isActive = true)
	{
		Renderer component = thisObject.GetComponent<Renderer>();
		component.material.SetColor("_EmissionColor", (!isActive) ? this.inactiveEmissionColor : this.activeEmissionColor);
		if (this.isDarker)
		{
			this.TurnDarker(thisObject.transform);
		}
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0002F810 File Offset: 0x0002DC10
	protected Color GetTextColorAsColor(TextColor textColor)
	{
		Color32 color = new Color32(0, 0, 0, byte.MaxValue);
		switch (textColor)
		{
		case TextColor.Gray:
			color = new Color32(99, 111, 126, byte.MaxValue);
			break;
		case TextColor.LightGray:
			color = new Color32(139, 151, 166, byte.MaxValue);
			break;
		case TextColor.Green:
			color = new Color32(0, 131, 50, byte.MaxValue);
			break;
		case TextColor.Red:
			color = new Color32(140, 0, 0, byte.MaxValue);
			break;
		case TextColor.Gold:
			color = new Color32(140, 140, 0, byte.MaxValue);
			break;
		case TextColor.Blue:
			color = new Color32(40, 80, 215, byte.MaxValue);
			break;
		case TextColor.White:
			color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			break;
		}
		return color;
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x0002F91C File Offset: 0x0002DD1C
	protected void AddTextFreeButton(string contextName, string contextId, int xOnFundament, int yOnFundament, string icon, bool state = false)
	{
		GameObject prefabInstance = this.GetPrefabInstance("TextFreeButton");
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		DialogPart dialogPart = (DialogPart)prefabInstance.GetComponent(typeof(DialogPart));
		dialogPart.contextName = contextName;
		dialogPart.contextId = contextId;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		Transform transform = prefabInstance.transform.Find("IconQuad");
		Managers.thingManager.LoadMaterial(transform.gameObject, "DialogIconMaterials/" + icon);
		if (state)
		{
			this.ApplyEmissionColor(prefabInstance.transform.Find("Cube"), true);
		}
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0002FA24 File Offset: 0x0002DE24
	protected GameObject AddCheckbox(string contextName = null, string contextId = null, string text = null, int xOnFundament = 0, int yOnFundament = 0, bool state = false, float textSizeFactor = 1f, string prefabName = "Checkbox", TextColor textColor = TextColor.Default, string footnote = null, ExtraIcon extraIcon = ExtraIcon.None)
	{
		GameObject prefabInstance = this.GetPrefabInstance(prefabName);
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		DialogPart component = prefabInstance.GetComponent<DialogPart>();
		component.contextName = contextName;
		component.contextId = contextId;
		component.state = state;
		component.isCheckbox = true;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		Transform transform = prefabInstance.transform.Find("Text");
		TextMesh component2 = transform.GetComponent<TextMesh>();
		component2.text = text.ToUpper();
		if (textColor != TextColor.Default)
		{
			Renderer component3 = component2.GetComponent<Renderer>();
			component3.material.color = this.GetTextColorAsColor(textColor);
		}
		this.ApplyTextSizeFactor(transform, textSizeFactor, 0f, 0f, false);
		Transform transform2 = prefabInstance.transform.Find("Cube");
		string text2 = ((!state) ? "checkboxInactive" : "checkboxActive");
		if (state)
		{
			Transform transform3 = prefabInstance.transform.Find("IconQuad");
			Managers.thingManager.LoadMaterial(transform3.gameObject, "DialogIconMaterials/" + text2);
			this.ApplyEmissionColor(transform2, true);
		}
		else if (this.isDarker)
		{
			this.TurnDarker(transform2);
		}
		Transform transform4 = null;
		if (!string.IsNullOrEmpty(footnote))
		{
			transform4 = prefabInstance.transform.Find("Footnote");
			transform4.gameObject.SetActive(true);
			TextMesh component4 = transform4.GetComponent<TextMesh>();
			component4.text = footnote.ToUpper().Trim();
			this.ApplyTextSizeFactor(component4.transform, textSizeFactor, 0f, 0f, false);
			Renderer component5 = component4.GetComponent<Renderer>();
			component5.material.color = this.GetTextColorAsColor(TextColor.Gray);
		}
		if (extraIcon != ExtraIcon.None)
		{
			Transform transform5 = prefabInstance.transform.Find("ExtraIconQuad");
			string text3 = "DialogIconMaterials/Extra/" + extraIcon.ToString();
			Managers.thingManager.LoadMaterial(transform5.gameObject, text3);
			transform5.gameObject.SetActive(true);
			if (transform4 != null)
			{
				Vector3 localPosition2 = transform4.localPosition;
				localPosition2.x = 0.0628f;
				transform4.localPosition = localPosition2;
			}
		}
		return prefabInstance;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0002FCBC File Offset: 0x0002E0BC
	protected GameObject AddIconCheckbox(string contextName, string contextId, string text, int xOnFundament, int yOnFundament, string icon, bool state = false, float textSizeFactor = 1f, bool autoSideAlign = false)
	{
		string text2 = "IconCheckbox" + ((this.handSide != Side.Right || !autoSideAlign) ? string.Empty : "RightAligned");
		GameObject prefabInstance = this.GetPrefabInstance(text2);
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		DialogPart component = prefabInstance.GetComponent<DialogPart>();
		component.contextName = contextName;
		component.contextId = contextId;
		component.state = state;
		component.isCheckbox = true;
		component.isIconCheckbox = true;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		Transform transform = prefabInstance.transform.Find("IconQuad");
		Managers.thingManager.LoadMaterial(transform.gameObject, "DialogIconMaterials/" + icon);
		Transform transform2 = prefabInstance.transform.Find("Text");
		TextMesh component2 = transform2.GetComponent<TextMesh>();
		component2.text = text.ToUpper();
		if (textSizeFactor != 1f)
		{
			this.ApplyTextSizeFactor(transform2, textSizeFactor, 0f, 0f, false);
		}
		transform2.parent = this.transform;
		Transform transform3 = prefabInstance.transform.Find("Cube");
		if (state)
		{
			this.ApplyEmissionColor(transform3, true);
		}
		return prefabInstance;
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0002FE54 File Offset: 0x0002E254
	protected void AddDimensionIndicator(string contextName, string contextId, string text, int xOnFundament, int yOnFundament, DimensionIndicatorType dimensionIndicatorType, bool isCheckbox = false, bool checkboxState = false)
	{
		switch (dimensionIndicatorType)
		{
		case DimensionIndicatorType.PositionX:
		case DimensionIndicatorType.RotationZ:
			xOnFundament += 320;
			break;
		case DimensionIndicatorType.PositionY:
		case DimensionIndicatorType.RotationY:
			xOnFundament += 160;
			break;
		}
		GameObject prefabInstance = this.GetPrefabInstance("DimensionIndicator");
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		DialogPart component = prefabInstance.GetComponent<DialogPart>();
		component.contextName = contextName;
		component.contextId = contextId;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)xOnFundament * this.scaleFactor;
		localPosition.z -= (float)yOnFundament * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		prefabInstance.GetComponentInChildren<TextMesh>().text = text.ToUpper();
		if (string.IsNullOrEmpty(text))
		{
			Transform transform = prefabInstance.transform.Find("Shape");
			transform.localPosition = new Vector3(0f, transform.localPosition.y, transform.localPosition.z);
		}
		prefabInstance.GetComponentInChildren<DimensionIndicator>().type = dimensionIndicatorType;
		if (isCheckbox)
		{
			DialogPart component2 = prefabInstance.GetComponent<DialogPart>();
			component2.state = checkboxState;
			component2.isCheckbox = true;
			component2.autoStopHighlight = false;
			this.SetButtonHighlight(prefabInstance, component2.state);
		}
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0002FFE4 File Offset: 0x0002E3E4
	protected void ApplyTextSizeFactor(Transform textPart, float textSizeFactor, float verticalTextOffset = 0f, float horizontalTextOffset = 0f, bool ignoreTextPositioning = false)
	{
		if (textSizeFactor != 1f || verticalTextOffset != 0f || horizontalTextOffset != 0f)
		{
			textPart.localScale = new Vector3(textPart.localScale.x * textSizeFactor, textPart.localScale.y * textSizeFactor, textPart.localScale.z * textSizeFactor);
			if (!ignoreTextPositioning && (textSizeFactor <= 0.5f || verticalTextOffset != 0f || horizontalTextOffset != 0f))
			{
				Vector3 localPosition = textPart.localPosition;
				if (verticalTextOffset != 0f)
				{
					localPosition.z += verticalTextOffset;
				}
				else if (textSizeFactor <= 0.25f)
				{
					localPosition.z -= 0.012f;
				}
				else
				{
					localPosition.z -= 0.009f;
				}
				if (horizontalTextOffset != 0f)
				{
					localPosition.x += horizontalTextOffset;
				}
				textPart.localPosition = localPosition;
			}
		}
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x000300F8 File Offset: 0x0002E4F8
	protected void SetCheckboxState(GameObject part, bool state, bool ignoreIcon = true)
	{
		DialogPart component = part.GetComponent<DialogPart>();
		if (!ignoreIcon && !component.isIconCheckbox)
		{
			string text = ((!state) ? "checkboxInactive" : "checkboxActive");
			Transform transform = part.transform.Find("IconQuad");
			Managers.thingManager.LoadMaterial(transform.gameObject, "DialogIconMaterials/" + text);
		}
		this.SetButtonHighlight(part, state);
		component.state = state;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00030170 File Offset: 0x0002E570
	protected void SetButtonHighlight(GameObject part, bool state)
	{
		Transform transform = part.transform.Find("Cube");
		if (transform == null)
		{
			transform = part.transform.Find("Shape");
		}
		Renderer component = transform.GetComponent<Renderer>();
		Material material = component.material;
		material.SetColor("_EmissionColor", (!state) ? this.inactiveEmissionColor : this.activeEmissionColor);
		if (this.isDarker)
		{
			this.TurnDarker(transform.transform);
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x000301F4 File Offset: 0x0002E5F4
	protected void SetButtonColor(GameObject button, Color color)
	{
		Transform transform = button.transform.Find("Cube");
		Renderer component = transform.GetComponent<Renderer>();
		Material material = component.material;
		material.color = color;
		if (this.isDarker)
		{
			this.TurnDarker(transform.transform);
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00030240 File Offset: 0x0002E640
	protected void SetText(GameObject thisObject, string text, float textSizeFactor = 1f, TextColor? textColor = null, bool useMixedCase = false)
	{
		Transform transform = thisObject.transform.Find("Text");
		TextMesh component = transform.GetComponent<TextMesh>();
		if (!useMixedCase)
		{
			text = text.ToUpper();
		}
		component.text = text;
		if (textSizeFactor != 1f)
		{
			transform.localScale = new Vector3(transform.localScale.x * textSizeFactor, transform.localScale.y * textSizeFactor, transform.localScale.z * textSizeFactor);
		}
		if (textColor != null)
		{
			this.SetTextColor(component, textColor.Value);
		}
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x000302DC File Offset: 0x0002E6DC
	public DialogPartData GetStateChange(bool doDebug = false)
	{
		DialogPartData dialogPartData = null;
		if (this.transform != null)
		{
			IEnumerator enumerator = this.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogPart component = transform.GetComponent<DialogPart>();
					if (component != null)
					{
						if (CrossDevice.desktopMode)
						{
							if (component.stateJustChanged && Time.time >= component.timeClicked + 0.1f)
							{
								dialogPartData = this.HandleFullPress(transform, component);
							}
						}
						else if (transform.localPosition.y < component.verticalOffset)
						{
							transform.localPosition += Vector3.up * 0.001f;
							if ((CrossDevice.desktopMode || transform.localPosition.y >= component.verticalOffset) && component.stateJustChanged)
							{
								dialogPartData = this.HandleFullPress(transform, component);
							}
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
		}
		return dialogPartData;
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00030410 File Offset: 0x0002E810
	private DialogPartData HandleFullPress(Transform child, DialogPart dialogPartScript)
	{
		DialogPartData dialogPartData = new DialogPartData();
		dialogPartScript.stateJustChanged = false;
		dialogPartData.contextName = dialogPartScript.contextName;
		dialogPartData.contextId = dialogPartScript.contextId;
		dialogPartData.state = dialogPartScript.state;
		dialogPartData.button = dialogPartScript.gameObject;
		this.TriggerHapticPulseOnParentHand();
		Hand parentHand = this.GetParentHand();
		if (parentHand != null)
		{
			Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "hand touches " + Misc.GetOppositeSide(parentHand.side).ToString().ToLower(), true);
		}
		if (dialogPartScript.autoStopHighlight && !dialogPartScript.isCheckbox)
		{
			IEnumerator enumerator = child.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.CompareTag("PressableDialogPart"))
					{
						dialogPartScript.state = false;
						Renderer component = transform.gameObject.GetComponent<Renderer>();
						Material material = component.material;
						material.SetColor("_EmissionColor", new Color(0f, 0f, 0f));
						break;
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
		}
		return dialogPartData;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x0003056C File Offset: 0x0002E96C
	public void TriggerHapticPulseOnParentHand()
	{
		Transform handTransform = this.GetHandTransform();
		if (handTransform != null && handTransform.parent != null)
		{
			Hand parentHand = this.GetParentHand();
			if (parentHand != null)
			{
				parentHand.TriggerHapticPulse(Universe.miniBurstPulse);
			}
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x000305BC File Offset: 0x0002E9BC
	private Hand GetParentHand()
	{
		Hand hand = null;
		Transform handTransform = this.GetHandTransform();
		if (handTransform != null)
		{
			hand = handTransform.GetComponent<Hand>();
			if (hand == null && handTransform.parent != null)
			{
				hand = handTransform.parent.GetComponent<Hand>();
			}
		}
		return hand;
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0003060F File Offset: 0x0002EA0F
	protected void AddTopCreationsOfPerson(string userId)
	{
		this.AddTopCreationsOfPerson(userId, Vector3.zero);
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00030620 File Offset: 0x0002EA20
	protected void AddTopCreationsOfPerson(string userId, Vector3 creationsOffset)
	{
		Managers.thingManager.GetTopThingIdsCreatedByPerson(userId, delegate(List<string> ids)
		{
			if (this.transform != null)
			{
				Vector3 vector = new Vector3(-0.075f, 0.025f, 0.045f);
				vector += creationsOffset;
				foreach (string text in ids)
				{
					Managers.thingManager.InstantiateThingOnDialogViaCache(ThingRequestContext.AddTopCreationsOfPerson, text, this.transform, vector, 0.035f, false, false, 0f, 0f, 0f, false, false);
					vector.x += 0.05f;
				}
			}
		});
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00030658 File Offset: 0x0002EA58
	protected GameObject AddFlag(string _flagContext, bool isFlagged, string recipientTypeName = "", bool autoBacksideAlign = false, int x = -410, int y = 420)
	{
		this.flagContext = _flagContext;
		if (recipientTypeName == string.Empty)
		{
			recipientTypeName = this.flagContext;
		}
		this.flagButton = this.AddModelButton("FlagButton", this.flagContext, null, x, y, autoBacksideAlign);
		this.flagTextMesh = this.AddLabel(string.Empty, x + 45, y - 20, 0.6f, false, TextColor.Red, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.flagTextMesh.anchor = TextAnchor.MiddleLeft;
		this.UpdateFlag(isFlagged, true, recipientTypeName);
		return this.flagButton;
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x000306EC File Offset: 0x0002EAEC
	protected void UpdateFlag(bool isFlagged, bool isInitialSetup = false, string recipientTypeName = "")
	{
		if (isFlagged)
		{
			string text = string.Empty;
			if (recipientTypeName != null)
			{
				if (!(recipientTypeName == "flagPerson"))
				{
					if (!(recipientTypeName == "flagCreation"))
					{
						if (recipientTypeName == "flagArea")
						{
							text = "area";
						}
					}
					else
					{
						text = "creation";
					}
				}
				else
				{
					text = "person";
				}
			}
			this.flagTextMesh.text = (text + Environment.NewLine + "reported").ToUpper();
			if (!isInitialSetup)
			{
				Managers.soundManager.Play("success", this.transform, 1f, false, false);
			}
		}
		else
		{
			this.flagTextMesh.text = string.Empty;
			if (!isInitialSetup)
			{
				Managers.soundManager.Play("delete", this.transform, 1f, false, false);
			}
		}
		this.ApplyEmissionColorToShape(this.flagButton, isFlagged);
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x000307E8 File Offset: 0x0002EBE8
	protected void AddPersonInfo(Person person, string name, string status, int ageInDays, bool isOwn = false, bool showBackButton = false, bool isOnline = false)
	{
		bool flag = Managers.personManager.WeAreResized();
		int num = -450;
		if (isOwn)
		{
			isOnline = true;
			if (status == null || status == string.Empty)
			{
				status = "my status text";
			}
			this.AddModelButton("EditTextButton", "editMyName", null, -305, -440, false);
			this.AddModelButton("EditTextButton", "editMyStatus", null, -305, -370, false);
			num += 110;
		}
		if (showBackButton)
		{
			this.AddBackButton();
			num += 70;
		}
		this.AddFlexibleSizeHeadline(name, num, -460, (!isOnline) ? TextColor.Default : TextColor.Green, 15);
		string text = Misc.WrapWithNewlines(status, 40, -1);
		this.AddLabel(text, num, -380, 0.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.AddLabel("Days: " + ageInDays, 0, -50, 1.2f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		if (person != null)
		{
			this.AddOrUpdatePersonStats(person);
		}
		if (!flag)
		{
			GameObject gameObject = this.AddModelButton("AreaButton", "findAreasByCreatorId", name, 280, -590, false);
			gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 0.4f, gameObject.transform.localScale.y, gameObject.transform.localScale.z * 0.4f);
		}
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00030980 File Offset: 0x0002ED80
	protected void AddOrUpdatePersonStats(Person person)
	{
		if (this != null)
		{
			int num = 0;
			int bodyThingPartAndStrictSyncCount = person.GetBodyThingPartAndStrictSyncCount(out num);
			string text = string.Empty;
			text = text + "Body parts: " + Misc.AddThousandSeparatorComma(bodyThingPartAndStrictSyncCount) + Environment.NewLine;
			text = text + "Strict-synced: " + Misc.AddThousandSeparatorComma(num) + Environment.NewLine;
			text = text + "Script msgs/s: " + Misc.AddThousandSeparatorComma(person.lastBehaviorScriptMessagesPerSecond) + Environment.NewLine;
			text = text + "Sync authority: " + Misc.BoolAsYesNo(person.isMasterClient);
			if (this.personStatsTextMesh == null)
			{
				this.personStatsTextMesh = this.AddLabel(text, 0, 140, 0.85f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleCenter);
			}
			else
			{
				this.personStatsTextMesh.text = text.ToUpper();
			}
			base.Invoke("AddOrUpdatePersonStats", 1f);
		}
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x00030A68 File Offset: 0x0002EE68
	protected TextMesh AddFlexibleSizeHeadline(string text, int x = -440, int y = -440, TextColor textColor = TextColor.Default, int lengthSplitOff = 25)
	{
		float num = 1.4f;
		if (text == null || text == string.Empty)
		{
			text = "unnamed";
		}
		text = Misc.Truncate(text, 50, true);
		if (text.Length >= lengthSplitOff)
		{
			num = ((text.Length <= 18) ? 1.1f : 0.65f);
		}
		return this.AddLabel(text, x, y, num, false, textColor, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x00030AE4 File Offset: 0x0002EEE4
	protected void ClickedAreaTeleportButton(string areaName)
	{
		Managers.areaManager.ClearAreaToTransportToAfterNextAreaLoad();
		if (areaName == Managers.areaManager.currentAreaName)
		{
			Managers.personManager.ourPerson.ResetPositionAndRotation();
			Managers.thingManager.ResetTriggeredOnSomeoneNewInVicinity();
			Managers.soundManager.Play("success", this.transform, 1f, false, false);
			Managers.behaviorScriptManager.StopSpeech();
			this.hand.FinalizeTeleport();
		}
		else
		{
			Managers.areaManager.TryTransportToAreaByNameOrUrlName(areaName, string.Empty, false);
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00030B74 File Offset: 0x0002EF74
	protected void TurnButtonGreen(GameObject button)
	{
		Transform transform = button.transform.Find("Cube");
		Renderer component = transform.GetComponent<Renderer>();
		component.material.color = new Color(0.7f, 1f, 0.7f);
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00030BB8 File Offset: 0x0002EFB8
	protected GameObject AddCheckboxHelpButton(string contextName, int y)
	{
		return this.AddModelButton("ContextHelpButton", contextName, null, 420, y, false);
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00030BD0 File Offset: 0x0002EFD0
	protected GameObject AddHelpButton(string contextName, int x, int y, bool small = false)
	{
		GameObject gameObject = this.AddModelButton("ContextHelpButton", contextName, null, x, y, false);
		if (small)
		{
			gameObject.transform.localScale = new Vector3(0.8f, 1f, 0.8f);
		}
		return gameObject;
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00030C18 File Offset: 0x0002F018
	protected TextMesh ShowHelpLabel(string text, int maxLength = 50, float fontSizeFactor = 0.7f, TextAlignment align = TextAlignment.Left, int y = -700, bool allowMixedCase = false, bool skipWrapWithNewlines = false, float lineSpacing = 1f, TextColor textColor = TextColor.Default)
	{
		this.lastHelpTextPassed = text;
		if (!skipWrapWithNewlines)
		{
			text = Misc.WrapWithNewlines(text, maxLength, -1);
		}
		bool flag = false;
		if (this.currentHelpText != null)
		{
			flag = this.currentHelpText.text == text;
			global::UnityEngine.Object.Destroy(this.currentHelpText);
			global::UnityEngine.Object.Destroy(this.helpLabelSide);
		}
		if (!flag)
		{
			int num = ((align != TextAlignment.Left) ? 0 : (-430));
			string text2 = text;
			int num2 = num;
			this.currentHelpText = this.AddLabel(text2, num2, y, fontSizeFactor, false, textColor, allowMixedCase, align, -1, 1f, false, TextAnchor.MiddleLeft);
			this.helpLabelSide = this.Add("HelpLabelSide", 0, 0, false).gameObject;
			if (lineSpacing != 1f)
			{
				this.currentHelpText.lineSpacing = lineSpacing;
			}
			if (this.isDarker)
			{
				this.TurnDarker(this.helpLabelSide.transform.Find("Back"));
			}
		}
		return this.currentHelpText;
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00030D23 File Offset: 0x0002F123
	protected void HideHelpLabel()
	{
		if (this.currentHelpText != null)
		{
			global::UnityEngine.Object.Destroy(this.currentHelpText);
			global::UnityEngine.Object.Destroy(this.helpLabelSide);
			this.lastHelpTextPassed = null;
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00030D54 File Offset: 0x0002F154
	protected void ShowCompactHelpLabel(string text)
	{
		this.ShowHelpLabel(text, 50, 0.7f, TextAlignment.Left, -710, false, false, 0.85f, TextColor.Default);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00030D80 File Offset: 0x0002F180
	protected bool ToggleCompactHelpLabel(string text)
	{
		return this.ToggleHelpLabel(text, -710, 0.85f, 50, 0.7f);
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00030DA7 File Offset: 0x0002F1A7
	protected bool HelpLabelShows()
	{
		return this.currentHelpText != null;
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00030DB8 File Offset: 0x0002F1B8
	protected bool ToggleHelpLabel(string text, int y = -700, float lineSpacing = 1f, int maxLength = 50, float fontSizeFactor = 0.7f)
	{
		bool flag = false;
		if (this.HelpLabelShows() && this.lastHelpTextPassed == text)
		{
			this.HideHelpLabel();
		}
		else
		{
			this.ShowHelpLabel(text, maxLength, fontSizeFactor, TextAlignment.Left, y, false, false, lineSpacing, TextColor.Default);
			flag = true;
		}
		return flag;
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00030E0C File Offset: 0x0002F20C
	protected void AddTimeDisplay()
	{
		this.anylandTimeTextMesh = this.AddLabel(string.Empty, 0, -320, 2f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.AddLabel("anyland time", 0, -230, 1f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.userTimeTextMesh = this.AddLabel(string.Empty, 0, 30, 2f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.AddLabel("your time", 0, 120, 1f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.dateTextMesh = this.AddLabel(string.Empty, 0, 230, 1.2f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.UpdateTimeDisplay();
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00030ED8 File Offset: 0x0002F2D8
	protected void AddTimeDisplayCompact()
	{
		TextAlignment textAlignment = TextAlignment.Center;
		int num = -470;
		string text = string.Empty;
		int num2 = 0;
		int num3 = num;
		float num4 = 2f;
		bool flag = true;
		TextAlignment textAlignment2 = textAlignment;
		this.anylandTimeTextMesh = this.AddLabel(text, num2, num3, num4, flag, TextColor.Default, false, textAlignment2, -1, 1f, false, TextAnchor.MiddleLeft);
		text = "Anyland time";
		num3 = 0;
		num2 = num + 90;
		num4 = 1f;
		flag = true;
		textAlignment2 = textAlignment;
		this.AddLabel(text, num3, num2, num4, flag, TextColor.Default, false, textAlignment2, -1, 1f, false, TextAnchor.MiddleLeft);
		this.UpdateTimeDisplay();
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00030F64 File Offset: 0x0002F364
	protected void UpdateTimeDisplay()
	{
		this.showTimeColon = !this.showTimeColon;
		string text = "HH" + ((!this.showTimeColon) ? " " : ":") + "mm";
		DateTime dateTime = DateTime.Now.ToUniversalTime();
		DateTime dateTime2 = DateTime.Now.ToLocalTime();
		if (this.IsNearNewYear(dateTime) || this.IsNearNewYear(dateTime2))
		{
			text = text + ((!this.showTimeColon) ? " " : ":") + "ss";
		}
		if (this.anylandTimeTextMesh != null)
		{
			this.anylandTimeTextMesh.text = dateTime.ToString(text);
		}
		if (this.userTimeTextMesh != null)
		{
			this.userTimeTextMesh.text = dateTime2.ToString(text);
		}
		if (this.dateTextMesh != null)
		{
			this.dateTextMesh.text = dateTime2.ToString("yyyy-MM-dd");
		}
		base.Invoke("UpdateTimeDisplay", 1f);
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00031085 File Offset: 0x0002F485
	private bool IsNearNewYear(DateTime thisTime)
	{
		return thisTime.Month == 12 && thisTime.Day == 31 && thisTime.Hour == 23 && thisTime.Minute >= 30;
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x000310C4 File Offset: 0x0002F4C4
	protected void SetFundamentColor(Color color)
	{
		if (this.fundament != null)
		{
			Transform transform = this.fundament.transform.Find("Cube");
			if (transform != null)
			{
				Renderer component = transform.GetComponent<Renderer>();
				component.material.color = color;
			}
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00031118 File Offset: 0x0002F518
	protected void AddDialogThingFundamentIfNeeded()
	{
		if (!this.didAddFundament)
		{
			string text = null;
			string text2 = null;
			bool flag;
			bool flag2;
			if (Managers.forumManager.currentForumData != null)
			{
				text = Managers.forumManager.currentForumData.dialogThingId;
				text2 = Managers.forumManager.currentForumData.dialogColor;
				flag = true;
				flag2 = true;
				Managers.forumManager.UpdateCurrentThingIdAndColorCache();
			}
			else
			{
				flag = Managers.forumManager.dialogThingIdsByForumId.TryGetValue(Managers.forumManager.currentForumId, out text);
				flag2 = Managers.forumManager.dialogColorsByForumId.TryGetValue(Managers.forumManager.currentForumId, out text2);
			}
			if (flag2 && flag)
			{
				if (string.IsNullOrEmpty(text))
				{
					this.AddFundament();
				}
				else
				{
					Managers.thingManager.InstantiateThingOnDialogViaCache(ThingRequestContext.AddDialogThingFundamentIfNeeded, text, this.transform, Vector3.zero, 1f, false, true, 0f, 0f, 0f, false, false);
					if (!string.IsNullOrEmpty(text2))
					{
						this.customDefaultTextColor = this.GetTextColorForBackground(text2);
					}
				}
				this.didAddFundament = true;
			}
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00031224 File Offset: 0x0002F624
	protected TextColor GetTextColorForBackground(string dialogColorString)
	{
		TextColor textColor = TextColor.Default;
		if (!string.IsNullOrEmpty(dialogColorString))
		{
			Color color = Misc.ColorStringToColor(dialogColorString);
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(color, out num, out num2, out num3);
			if (num3 <= 0.35f)
			{
				textColor = TextColor.White;
			}
		}
		return textColor;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00031260 File Offset: 0x0002F660
	protected void ExecuteTextLink(Hand hand, string linkInner)
	{
		TextLink textLink = new TextLink();
		if (textLink.TryParseLinkInner(linkInner))
		{
			textLink.Execute(hand);
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00031288 File Offset: 0x0002F688
	protected GameObject AddTextLinkButton(TextLink textLink, int x, int y)
	{
		string text = "textLink";
		string linkInner = textLink.linkInner;
		string buttonText = textLink.GetButtonText();
		string text2 = "Button";
		TextColor buttonTextColor = textLink.GetButtonTextColor();
		string iconName = textLink.GetIconName();
		GameObject gameObject = this.AddButton(text, linkInner, buttonText, text2, x, y, iconName, false, 0.9f, buttonTextColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		gameObject.transform.localScale = new Vector3(0.45f, 1f, 0.45f);
		return gameObject;
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0003131C File Offset: 0x0002F71C
	protected GameObject AddVideoCube()
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		gameObject.transform.parent = this.fundament.transform;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = new Vector3(0f, 0.0055f, 0f);
		gameObject.transform.localScale = new Vector3(0.24f, 0.01f, 0.1675f);
		Renderer component = gameObject.GetComponent<Renderer>();
		global::UnityEngine.Object.Destroy(component);
		return gameObject;
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x000313A4 File Offset: 0x0002F7A4
	protected void AddPeopleCountToButton(GameObject button, int peopleCount)
	{
		if (peopleCount >= 1)
		{
			Transform transform = button.transform.Find("PeopleCount");
			transform.gameObject.SetActive(true);
			TextMesh component = transform.GetComponent<TextMesh>();
			component.text = peopleCount.ToString();
			Transform transform2 = button.transform.Find("IconQuadPeopleCount");
			transform2.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0003140C File Offset: 0x0002F80C
	protected void MinifyButton(GameObject button, float x = 1f, float y = 1f, float z = 0.55f, bool alsoMinifyIcon = false)
	{
		Transform transform = button.transform.Find("Cube");
		if (transform == null)
		{
			transform = button.transform.Find("Shape");
		}
		transform.localScale = new Vector3(x, y, z);
		if (alsoMinifyIcon)
		{
			Transform transform2 = button.transform.Find("IconQuad");
			if (transform2 != null)
			{
				transform2.localScale = new Vector3(transform2.localScale.x * x, transform2.localScale.y * z, transform2.localScale.z);
			}
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x000314B8 File Offset: 0x0002F8B8
	protected void MinifyMoreButton(GameObject button)
	{
		Transform transform = button.transform.Find("Cube");
		transform.localScale = new Vector3(0.45f, transform.localScale.y, transform.localScale.z);
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00031504 File Offset: 0x0002F904
	protected void AddDefaultPagingButtons(int x = 80, int y = 400, string nameForPreviousNext = "Page", bool storeAsObjects = false, int offsetX = 0, float size = 0.85f, bool isUpDown = false)
	{
		if (storeAsObjects)
		{
			this.RemoveDefaultPagingButtons();
		}
		GameObject gameObject = this.AddModelButton("ButtonBack", "previous" + nameForPreviousNext, null, -x + offsetX, y, false);
		gameObject.transform.localScale = new Vector3(size, 1f, size);
		if (isUpDown)
		{
			gameObject.transform.Rotate(new Vector3(0f, 90f, 0f));
		}
		if (storeAsObjects)
		{
			this.pageBackwardButton = gameObject;
		}
		GameObject gameObject2 = this.AddModelButton("ButtonForward", "next" + nameForPreviousNext, null, x + offsetX, y, false);
		gameObject2.transform.localScale = new Vector3(size, 1f, size);
		if (isUpDown)
		{
			gameObject2.transform.Rotate(new Vector3(0f, 90f, 0f));
		}
		if (storeAsObjects)
		{
			this.pageForwardButton = gameObject2;
		}
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x000315F5 File Offset: 0x0002F9F5
	protected void RemoveDefaultPagingButtons()
	{
		global::UnityEngine.Object.Destroy(this.pageBackwardButton);
		global::UnityEngine.Object.Destroy(this.pageForwardButton);
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0003160D File Offset: 0x0002FA0D
	protected void SetActiveDefaultPagingButtons(bool isActive)
	{
		if (this.pageBackwardButton != null)
		{
			this.pageBackwardButton.SetActive(isActive);
		}
		if (this.pageForwardButton != null)
		{
			this.pageForwardButton.SetActive(isActive);
		}
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0003164C File Offset: 0x0002FA4C
	protected void ApplyAppropriateLayerForDesktopStream()
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/FollowerCamera");
		if (@object != null && @object.activeSelf)
		{
			FollowerCamera component = @object.GetComponent<FollowerCamera>();
			if (component.weAreInvisible && !component.forceHandsVisible)
			{
				int num = LayerMask.NameToLayer("InvisibleToDesktopCamera");
				Transform[] componentsInChildren = this.gameObject.GetComponentsInChildren<Transform>();
				foreach (Transform transform in componentsInChildren)
				{
					transform.gameObject.layer = num;
				}
			}
		}
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x000316E4 File Offset: 0x0002FAE4
	protected GameObject AddGenericFindButton(string contextName = "find")
	{
		int num = 280;
		int num2 = -410;
		if (this.isBig)
		{
			num = 720;
			num2 = -890;
		}
		GameObject gameObject = this.AddModelButton("Find", contextName, null, num, num2, false);
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 0.85f, gameObject.transform.localScale.y, gameObject.transform.localScale.z * 0.85f);
		return gameObject;
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00031780 File Offset: 0x0002FB80
	protected void HandleBacksideEditButtons()
	{
		if (this.backsideWrapper != null)
		{
			this.SetUiWrapper(this.backsideWrapper);
			DialogPartData stateChange = this.GetStateChange(false);
			if (stateChange != null)
			{
				this.HandlePossibleEditButtonsStateChange(stateChange.contextName, stateChange.contextId, stateChange.state);
			}
			this.SetUiWrapper(this.gameObject);
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x000317DC File Offset: 0x0002FBDC
	protected void AddBacksideEditButtons()
	{
		if (CreationHelper.thingBeingEdited == null)
		{
			return;
		}
		base.StartCoroutine(this.DoAddBacksideEditButtons());
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x000317FC File Offset: 0x0002FBFC
	protected IEnumerator DoAddBacksideEditButtons()
	{
		if (this.backsideWrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.backsideWrapper);
			yield return false;
		}
		Thing thing = CreationHelper.thingBeingEdited.GetComponent<Thing>();
		this.backsideWrapper = this.GetUiWrapper();
		this.SetUiWrapper(this.backsideWrapper);
		int x = -400;
		int y = -383;
		int buttonSpace = 127;
		if (this.handSide == Side.Right)
		{
			x *= -1;
		}
		int i = 0;
		string text = "doSnapAngles";
		string text2 = null;
		string text3 = "Snap angles";
		int num = x;
		int num2 = y;
		int num3;
		i = (num3 = i) + 1;
		this.AddIconCheckbox(text, text2, text3, num, num2 + num3 * buttonSpace, "SnapAngles", thing.doSnapAngles, 1f, true);
		string text4 = "doSoftSnapAngles";
		string text5 = null;
		string text6 = "Soft snap angles";
		int num4 = x;
		int num5 = y;
		i = (num3 = i) + 1;
		this.AddIconCheckbox(text4, text5, text6, num4, num5 + num3 * buttonSpace, "SoftSnapAngles", thing.doSoftSnapAngles, 1f, true);
		string text7 = "doSnapPosition";
		string text8 = null;
		string text9 = "Snap position";
		int num6 = x;
		int num7 = y;
		i = (num3 = i) + 1;
		this.AddIconCheckbox(text7, text8, text9, num6, num7 + num3 * buttonSpace, "SnapPosition", thing.doSnapPosition, 1f, true);
		string text10 = "scaleAllParts";
		string text11 = null;
		string text12 = "Scale all parts";
		int num8 = x;
		int num9 = y;
		i = (num3 = i) + 1;
		this.AddIconCheckbox(text10, text11, text12, num8, num9 + num3 * buttonSpace, "ScaleAllParts", thing.scaleAllParts, 1f, true);
		string text13 = "scaleEachPartUniformly";
		string text14 = null;
		string text15 = "Scale each part uniformly";
		int num10 = x;
		int num11 = y;
		i = (num3 = i) + 1;
		this.AddIconCheckbox(text13, text14, text15, num10, num11 + num3 * buttonSpace, "ScaleEachPartUniformly", thing.scaleEachPartUniformly, 1f, true);
		string text16 = "smallEditMovements";
		string text17 = null;
		string text18 = "Finetune position";
		int num12 = x;
		int num13 = y;
		i = (num3 = i) + 1;
		this.AddIconCheckbox(text16, text17, text18, num12, num13 + num3 * buttonSpace, "SmallEditMovements", thing.smallEditMovements, 1f, true);
		this.AddIconCheckbox("meSettings", null, string.Empty, -x, y + i * buttonSpace, "MeSettings", false, 1f, true);
		x += -12 * ((this.handSide != Side.Right) ? 1 : (-1));
		this.DoAddSymmetryButtons("autoAddReflectionParts", x, y + i * buttonSpace + 12, thing.autoAddReflectionPartsSideways, thing.autoAddReflectionPartsVertical, thing.autoAddReflectionPartsDepth, (this.handSide != Side.Right) ? 1 : (-1));
		this.AddLabel("Symmetry", 0, 375, 0.7f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.RotateBacksideWrapper();
		this.SetUiWrapper(this.gameObject);
		yield break;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x00031818 File Offset: 0x0002FC18
	protected IEnumerator AddSymmetryButtons(string contextNamePrefix, int x, int y, bool sideways, bool vertical, bool depth, int handSideFactor = 1)
	{
		if (this.symmetryButtons != null)
		{
			this.RemoveSymmetryButtons();
			yield return false;
		}
		this.DoAddSymmetryButtons(contextNamePrefix, x, y, sideways, vertical, depth, handSideFactor);
		yield break;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00031868 File Offset: 0x0002FC68
	protected void DoAddSymmetryButtons(string contextNamePrefix, int x, int y, bool sideways, bool vertical, bool depth, int handSideFactor = 1)
	{
		this.symmetryButtons = new List<GameObject>();
		List<string> list = new List<string> { "Sideways", "Vertical", "Depth" };
		int num = 0;
		foreach (string text in list)
		{
			bool flag = false;
			if (text != null)
			{
				if (!(text == "Sideways"))
				{
					if (!(text == "Vertical"))
					{
						if (text == "Depth")
						{
							flag = depth;
						}
					}
					else
					{
						flag = vertical;
					}
				}
				else
				{
					flag = sideways;
				}
			}
			GameObject gameObject = this.AddIconCheckbox(contextNamePrefix + text, null, string.Empty, x + 120 * handSideFactor * num++, y, "Symmetry/" + text, flag, 1f, true);
			this.MinifyButton(gameObject, 0.75f, 1f, 0.75f, true);
			this.symmetryButtons.Add(gameObject);
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x000319A4 File Offset: 0x0002FDA4
	protected void RemoveSymmetryButtons()
	{
		if (this.symmetryButtons != null)
		{
			foreach (GameObject gameObject in this.symmetryButtons)
			{
				global::UnityEngine.Object.Destroy(gameObject.gameObject);
			}
		}
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x00031A10 File Offset: 0x0002FE10
	protected void HandlePossibleEditButtonsStateChange(string contextName, string contextId, bool state)
	{
		if (CreationHelper.thingBeingEdited == null)
		{
			return;
		}
		bool flag = true;
		switch (contextName)
		{
		case "doSnapAngles":
			Managers.settingManager.SetState(Setting.SnapAngles, state, false);
			goto IL_200;
		case "doSoftSnapAngles":
			Managers.settingManager.SetState(Setting.SoftSnapAngles, state, false);
			goto IL_200;
		case "doLockAngles":
			Managers.settingManager.SetState(Setting.LockAngles, state, false);
			goto IL_200;
		case "doSnapPosition":
			Managers.settingManager.SetState(Setting.SnapPosition, state, false);
			goto IL_200;
		case "doLockPosition":
			Managers.settingManager.SetState(Setting.LockPosition, state, false);
			goto IL_200;
		case "smallEditMovements":
			Managers.settingManager.SetState(Setting.FinetunePosition, state, false);
			goto IL_200;
		case "scaleAllParts":
			Managers.settingManager.SetState(Setting.ScaleAllParts, state, false);
			goto IL_200;
		case "scaleEachPartUniformly":
			Managers.settingManager.SetState(Setting.ScaleEachPartUniformly, state, false);
			goto IL_200;
		case "autoAddReflectionPartsSideways":
			Managers.settingManager.SetState(Setting.SymmetrySideways, state, false);
			goto IL_200;
		case "autoAddReflectionPartsVertical":
			Managers.settingManager.SetState(Setting.SymmetryVertical, state, false);
			goto IL_200;
		case "autoAddReflectionPartsDepth":
			Managers.settingManager.SetState(Setting.SymmetryDepth, state, false);
			goto IL_200;
		case "meSettings":
			this.SwitchTo(DialogType.Settings, string.Empty);
			goto IL_200;
		}
		flag = false;
		IL_200:
		if (flag)
		{
			Managers.achievementManager.RegisterAchievement(Achievement.ClickedCreateDialogBacksideButton);
		}
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00031C2F File Offset: 0x0003002F
	public virtual void RecreateInterfaceAfterSettingsChangeIfNeeded()
	{
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00031C34 File Offset: 0x00030034
	protected GameObject AddGiftsButton()
	{
		GameObject gameObject = null;
		if (Our.mode != EditModes.Thing)
		{
			gameObject = this.AddButton("gifts", null, "Nifts", "ButtonSmallCentered", 0, 420, null, false, 1f, TextColor.Gold, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, true);
		}
		return gameObject;
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00031C90 File Offset: 0x00030090
	protected void AddMirror()
	{
		if (this.mirror == null)
		{
			this.mirror = global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Mirror", typeof(GameObject))) as GameObject;
			this.mirror.name = Misc.RemoveCloneFromName(this.mirror.name);
		}
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		Vector3 vector = this.mirror.transform.position;
		vector = @object.transform.position + @object.transform.forward * 2f;
		vector.y = this.gameObject.transform.position.y - 0.1f;
		this.mirror.transform.position = vector;
		this.mirror.transform.rotation = @object.transform.rotation;
		this.mirror.transform.Rotate(Vector3.forward * 90f);
		Vector3 localEulerAngles = this.mirror.transform.localEulerAngles;
		localEulerAngles.x = 0f;
		localEulerAngles.z = 0f;
		this.mirror.transform.localEulerAngles = localEulerAngles;
		this.UpdateMirror();
		if (Managers.personManager.ourPerson.ridingBeacon != null)
		{
			this.mirror.transform.parent = Managers.personManager.ourPerson.Rig.transform;
		}
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00031E24 File Offset: 0x00030224
	protected void UpdateMirror()
	{
		if (this.mirror != null)
		{
			Transform transform = this.mirror.transform.Find("ExtraMirrors");
			transform.gameObject.SetActive(Our.showExtraMirrors);
		}
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00031E68 File Offset: 0x00030268
	protected void RemoveMirror()
	{
		global::UnityEngine.Object.Destroy(this.mirror);
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00031E78 File Offset: 0x00030278
	protected GameObject AddBackground(string materialName, bool isOnBackside = false, bool keepExisting = false)
	{
		GameObject gameObject;
		if (isOnBackside)
		{
			gameObject = this.AddBackgroundBackside(materialName, keepExisting);
		}
		else
		{
			gameObject = this.AddBackgroundFrontside(materialName, keepExisting);
		}
		return gameObject;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x00031EA8 File Offset: 0x000302A8
	private GameObject AddBackgroundFrontside(string materialName, bool keepExisting = false)
	{
		if (!keepExisting)
		{
			global::UnityEngine.Object.Destroy(this.backgroundFrontsideImageObject);
		}
		this.backgroundFrontsideImageObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		Transform transform = this.backgroundFrontsideImageObject.transform;
		transform.parent = this.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = this.transform.localScale * 0.24f;
		transform.Rotate(new Vector3(90f, 0f, 0f));
		transform.Translate(Vector3.forward * -0.0115f);
		Managers.thingManager.LoadMaterial(this.backgroundFrontsideImageObject, "DialogBackgroundMaterials/" + materialName);
		return this.backgroundFrontsideImageObject;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00031F6C File Offset: 0x0003036C
	private GameObject AddBackgroundBackside(string materialName, bool keepExisting = false)
	{
		if (!keepExisting)
		{
			global::UnityEngine.Object.Destroy(this.backgroundBacksideImageObject);
		}
		this.backgroundBacksideImageObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		Transform transform = this.backgroundBacksideImageObject.transform;
		transform.parent = this.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = this.transform.localScale * 0.24f;
		transform.Rotate(new Vector3(90f, 0f, 0f));
		transform.Rotate(new Vector3(0f, 180f, 0f));
		transform.Translate(Vector3.forward * -0.015f);
		Managers.thingManager.LoadMaterial(this.backgroundBacksideImageObject, "DialogBackgroundMaterials/" + materialName);
		return this.backgroundBacksideImageObject;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00032049 File Offset: 0x00030449
	protected void DisableCollisionsBetweenPersonBodyAndCharacterController()
	{
		if (CrossDevice.desktopMode)
		{
			Managers.desktopManager.DisableCollisionsBetweenPersonBodyAndCharacterController();
		}
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0003205F File Offset: 0x0003045F
	protected GameObject SwitchTo(DialogType dialogType, string tabName = "")
	{
		return Managers.dialogManager.SwitchToNewDialog(dialogType, this.hand, tabName);
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00032074 File Offset: 0x00030474
	public void SwitchToConfirmDialog(string text, Action<bool> callback)
	{
		Managers.dialogManager.SwitchToNewDialog(DialogType.Confirm, this.hand, string.Empty);
		ConfirmDialog component = this.hand.currentDialog.GetComponent<ConfirmDialog>();
		component.DoStart(text, callback);
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x000320B4 File Offset: 0x000304B4
	protected void CloseDialog()
	{
		if (this.hand != null)
		{
			this.hand.SwitchToNewDialog(DialogType.Start, string.Empty);
		}
		else
		{
			TopographyId[] array = new TopographyId[]
			{
				TopographyId.Left,
				TopographyId.Right
			};
			foreach (TopographyId topographyId in array)
			{
				GameObject handByTopographyId = Managers.personManager.ourPerson.GetHandByTopographyId(topographyId);
				if (handByTopographyId != null)
				{
					Hand component = handByTopographyId.GetComponent<Hand>();
					if (component != null)
					{
						component.SwitchToNewDialog(DialogType.Start, string.Empty);
					}
				}
			}
		}
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0003215C File Offset: 0x0003055C
	protected void SetTextColor(TextMesh textMesh, TextColor textColor)
	{
		Renderer component = textMesh.GetComponent<Renderer>();
		component.material.color = this.GetTextColorAsColor(textColor);
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00032184 File Offset: 0x00030584
	public void MinifyVolumeButton(GameObject button)
	{
		Transform transform = button.transform.Find("Cube");
		Vector3 localScale = transform.localScale;
		localScale.x = 0.25f;
		transform.localScale = localScale;
		Vector3 localPosition = transform.localPosition;
		localPosition.x -= 0.035f;
		transform.localPosition = localPosition;
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x000321E0 File Offset: 0x000305E0
	public void ScaleModelButtonWidthHeight(GameObject button, float scaleFactor)
	{
		Vector3 localScale = button.transform.localScale;
		localScale.x *= scaleFactor;
		localScale.z *= scaleFactor;
		button.transform.localScale = localScale;
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00032224 File Offset: 0x00030624
	public List<string> AddToRecentlyEnteredInput(List<string> inputs, string input)
	{
		int num = inputs.IndexOf(input);
		if (num >= 0)
		{
			inputs.RemoveAt(num);
		}
		inputs.Add(input);
		if (inputs.Count > 6)
		{
			inputs.RemoveAt(0);
		}
		return inputs;
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00032264 File Offset: 0x00030664
	protected void AddGenericHelpButton()
	{
		GameObject gameObject = this.AddModelButton("HelpButton", "help", null, 100, -640, false);
		gameObject.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x000322AC File Offset: 0x000306AC
	protected void AddSubThingRelatedInterface(ThingPart thingPart)
	{
		this.AddCloseButton();
		this.AddBackButton();
		string text = ((this.dialogType != DialogType.PlacedSubThings) ? string.Empty : "Placed ");
		this.AddHeadline(text + "Sub-Things", -370, -460, TextColor.Default, TextAlignment.Left, false);
		string text2 = "CheckboxCompact";
		string text3 = "invisible";
		string text4 = null;
		string text5 = "Invisible";
		int num = -230;
		int num2 = 410;
		bool flag = thingPart.invisible;
		string text6 = text2;
		this.AddCheckbox(text3, text4, text5, num, num2, flag, 1f, text6, TextColor.Default, "when done", ExtraIcon.None);
		text6 = "subThingsFollowDelayed";
		text5 = null;
		text4 = "Follow smoothly";
		num2 = 230;
		num = 410;
		flag = thingPart.subThingsFollowDelayed;
		text3 = text2;
		this.AddCheckbox(text6, text5, text4, num2, num, flag, 1f, text3, TextColor.Default, null, ExtraIcon.None);
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x00032394 File Offset: 0x00030794
	protected DialogSlider AddSlider(string valuePrefix = "", string valueSuffix = "", int x = 0, int y = 0, float minValue = 0f, float maxValue = 100f, bool roundValues = false, float value = 0f, Action<float> onValueChange = null, bool showValue = true, float textSizeFactor = 1f)
	{
		GameObject prefabInstance = this.GetPrefabInstance("Slider");
		prefabInstance.SetActive(true);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.localPosition = Vector3.zero;
		prefabInstance.transform.localRotation = Quaternion.identity;
		DialogSlider component = prefabInstance.GetComponent<DialogSlider>();
		component.valuePrefix = valuePrefix;
		component.valueSuffix = valueSuffix;
		component.minValue = minValue;
		component.maxValue = maxValue;
		component.roundValues = roundValues;
		component.startValue = value;
		component.onValueChange = onValueChange;
		component.showValue = showValue;
		component.textSizeFactor = textSizeFactor;
		Vector3 localPosition = prefabInstance.transform.localPosition;
		localPosition.x += (float)x * this.scaleFactor;
		localPosition.z -= (float)y * this.scaleFactor;
		prefabInstance.transform.localPosition = localPosition;
		return component;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0003247C File Offset: 0x0003087C
	protected void ShowDesktopHelp()
	{
		GameObject gameObject = this.SwitchTo(DialogType.Video, string.Empty);
		VideoDialog component = gameObject.GetComponent<VideoDialog>();
		component.youTubeVideoId = "YLTmrZE2oNI";
		component.introText = string.Concat(new string[]
		{
			"Welcome to Anyland!",
			Environment.NewLine,
			"You can create with a VR headset",
			Environment.NewLine,
			"and explore via PC + keyboard"
		});
		component.showVersion = true;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x000324EC File Offset: 0x000308EC
	protected void AddMicrophoneIndicators(int y)
	{
		string text = ((!Universe.hearEchoOfMyVoice) ? string.Empty : "Hear your echo");
		this.AddLabel(text, 0, y + 26, 0.7f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.weHeardSomethingFromMicrophone = null;
		this.microphoneIndicatorText = this.AddLabel(string.Empty, 0, y, 1.5f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.UpdateMicrophoneIndicator(null);
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0003256C File Offset: 0x0003096C
	protected void UpdateMicrophoneIndicator(MicrophoneInputDevice microphone = null)
	{
		if (this.microphoneIndicatorText != null)
		{
			bool flag = microphone != null && Universe.transmitFromMicrophone && microphone.didJustMeetAmplitudeThreshold;
			bool? flag2 = this.weHeardSomethingFromMicrophone;
			if (flag2 != null)
			{
				bool flag3 = flag;
				bool? flag4 = this.weHeardSomethingFromMicrophone;
				if (flag3 == flag4.Value)
				{
					return;
				}
			}
			this.weHeardSomethingFromMicrophone = new bool?(flag);
			Renderer component = this.microphoneIndicatorText.GetComponent<Renderer>();
			if (flag)
			{
				this.microphoneIndicatorText.text = "((((                    ))))";
				component.material.color = this.GetTextColorAsColor(TextColor.Green);
			}
			else
			{
				this.microphoneIndicatorText.text = "(((                    )))";
				component.material.color = this.GetTextColorAsColor(TextColor.Gray);
			}
		}
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0003263B File Offset: 0x00030A3B
	protected void RotateBacksideWrapper()
	{
		if (this.backsideWrapper != null)
		{
			this.backsideWrapper.transform.Rotate(new Vector3(0f, 0f, 180f));
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00032674 File Offset: 0x00030A74
	public string GetTeleportToAreaText(string text, int peopleCount)
	{
		text = text.Trim();
		int num = ((peopleCount < 1) ? 20 : 17);
		string[] array = Misc.Split(text, "-", StringSplitOptions.RemoveEmptyEntries);
		bool flag = array.Length >= 3;
		if (flag)
		{
			text = array[0];
		}
		if (text.Length >= num)
		{
			text = Misc.WrapWithNewlines(text, num, 2);
		}
		return text;
	}

	// Token: 0x04000638 RID: 1592
	public DialogType dialogType;

	// Token: 0x04000639 RID: 1593
	protected const int checkboxStep = 115;

	// Token: 0x0400063A RID: 1594
	protected const int checkboxStepSmall = 90;

	// Token: 0x0400063B RID: 1595
	protected const int checkboxSemiStep = 40;

	// Token: 0x0400063C RID: 1596
	protected Hand hand;

	// Token: 0x0400063D RID: 1597
	protected new Transform transform;

	// Token: 0x0400063E RID: 1598
	public string tabName = string.Empty;

	// Token: 0x0400063F RID: 1599
	private bool hapticPulseOn;

	// Token: 0x04000640 RID: 1600
	private GameObject fundament;

	// Token: 0x04000641 RID: 1601
	private const float fundamentPixelReferenceSize = 1024f;

	// Token: 0x04000642 RID: 1602
	private const float fundamentSize = 0.25f;

	// Token: 0x04000643 RID: 1603
	protected float scaleFactor = 0.00024414062f;

	// Token: 0x04000644 RID: 1604
	protected Color activeEmissionColor = new Color(0.75f, 0.75f, 0.75f);

	// Token: 0x04000645 RID: 1605
	protected Color inactiveEmissionColor = new Color(0f, 0f, 0f);

	// Token: 0x04000646 RID: 1606
	protected AreaListSet areaListSetCache;

	// Token: 0x04000647 RID: 1607
	protected GameObject backsideWrapper;

	// Token: 0x04000648 RID: 1608
	protected GameObject wrapper;

	// Token: 0x04000649 RID: 1609
	private GameObject helpLabelSide;

	// Token: 0x0400064A RID: 1610
	private GameObject mainCloseButton;

	// Token: 0x0400064B RID: 1611
	private GameObject mirror;

	// Token: 0x0400064C RID: 1612
	private GameObject backgroundFrontsideImageObject;

	// Token: 0x0400064D RID: 1613
	private GameObject backgroundBacksideImageObject;

	// Token: 0x0400064E RID: 1614
	private new GameObject gameObject;

	// Token: 0x0400064F RID: 1615
	private GameObject dialogPartsObject;

	// Token: 0x04000650 RID: 1616
	private GameObject pageBackwardButton;

	// Token: 0x04000651 RID: 1617
	private GameObject pageForwardButton;

	// Token: 0x04000652 RID: 1618
	protected Side handSide = Side.Left;

	// Token: 0x04000653 RID: 1619
	private TextMesh flagTextMesh;

	// Token: 0x04000654 RID: 1620
	protected bool flagIsBeingToggled;

	// Token: 0x04000655 RID: 1621
	private string flagContext;

	// Token: 0x04000657 RID: 1623
	private int tabCounter;

	// Token: 0x04000658 RID: 1624
	private GameObject flagButton;

	// Token: 0x04000659 RID: 1625
	protected TextMesh currentHelpText;

	// Token: 0x0400065A RID: 1626
	private TextMesh anylandTimeTextMesh;

	// Token: 0x0400065B RID: 1627
	private TextMesh userTimeTextMesh;

	// Token: 0x0400065C RID: 1628
	private TextMesh dateTextMesh;

	// Token: 0x0400065D RID: 1629
	private bool showTimeColon;

	// Token: 0x0400065E RID: 1630
	private bool didAddFundament;

	// Token: 0x0400065F RID: 1631
	private bool isStartDialog;

	// Token: 0x04000660 RID: 1632
	protected TextColor customDefaultTextColor;

	// Token: 0x04000661 RID: 1633
	private string lastHelpTextPassed;

	// Token: 0x04000662 RID: 1634
	protected const int spaceBetweenSliders = 150;

	// Token: 0x04000663 RID: 1635
	protected const int spaceBetweenRelatedSliders = 120;

	// Token: 0x04000664 RID: 1636
	private List<GameObject> symmetryButtons;

	// Token: 0x04000665 RID: 1637
	private bool isDarker;

	// Token: 0x04000666 RID: 1638
	public bool showPlacedSubThingsTab;

	// Token: 0x04000667 RID: 1639
	private TextMesh microphoneIndicatorText;

	// Token: 0x04000668 RID: 1640
	private bool? weHeardSomethingFromMicrophone;

	// Token: 0x04000669 RID: 1641
	private TextMesh personStatsTextMesh;
}
