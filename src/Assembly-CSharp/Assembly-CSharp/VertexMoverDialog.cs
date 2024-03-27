using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class VertexMoverDialog : Dialog
{
	// Token: 0x06000CA1 RID: 3233 RVA: 0x00073CB0 File Offset: 0x000720B0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddModelButton("MinimizeButton", "minimize", null, -170, 0, false);
		base.AddCloseButton();
		if (CreationHelper.thingPartWhoseStatesAreEdited != null)
		{
			this.thingPart = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
			this.SetOriginalMeshIsConvex();
			this.vertexMover = Managers.personManager.OurPersonRig.GetComponentInChildren<VertexMover>();
			Managers.thingManager.smoothingAngles.TryGetValue(this.thingPart.baseType, out this.defaultSmoothingAngle);
			this.AddInterface();
			return;
		}
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00073D60 File Offset: 0x00072160
	private void SetOriginalMeshIsConvex()
	{
		int baseType = (int)this.thingPart.baseType;
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingPartBases[baseType]);
		MeshCollider component = gameObject.GetComponent<MeshCollider>();
		this.originalMeshIsConvex = component == null || component.convex;
		global::UnityEngine.Object.Destroy(gameObject);
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x00073DB4 File Offset: 0x000721B4
	private void AddInterface()
	{
		if (this.thingPart.isLocked)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			global::UnityEngine.Object.Destroy(this.secondaryWrapper);
			this.lockButton = base.AddModelButton("Lock", "toggleLock", null, 0, -15, false);
			base.ApplyEmissionColorToShape(this.lockButton, true);
		}
		else
		{
			global::UnityEngine.Object.Destroy(this.lockButton);
			this.AddIconCheckboxes();
			base.StartCoroutine(this.DoAddInterface());
			base.StartCoroutine(this.DoAddBacksideInterface());
		}
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x00073E40 File Offset: 0x00072240
	private IEnumerator DoAddInterface()
	{
		if (this.secondaryWrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.secondaryWrapper);
			yield return false;
		}
		this.secondaryWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.secondaryWrapper);
		this.AddButtons();
		this.AddSlider();
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x00073E5C File Offset: 0x0007225C
	private IEnumerator DoAddBacksideInterface()
	{
		if (this.backsideWrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.backsideWrapper);
			yield return false;
		}
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		MeshFilter meshFilter = this.thingPart.GetComponent<MeshFilter>();
		int meshCount = meshFilter.mesh.vertices.Length;
		bool usePreciseCollision = meshCount <= 500;
		base.AddLabel("Points: " + meshCount, 0, -60, 1f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		string text = "Collision: " + ((!usePreciseCollision) ? "Based on original" : "Precise");
		int num = 0;
		int num2 = 40;
		TextColor textColor = ((!usePreciseCollision) ? TextColor.Gray : TextColor.Green);
		base.AddLabel(text, num, num2, 1f, false, textColor, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		int helpX = 420 * ((this.handSide != Side.Left) ? 1 : (-1));
		base.AddHelpButton("help", helpX, -420, false);
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x00073E78 File Offset: 0x00072278
	private void AddButtons()
	{
		if (Managers.thingManager.GetSubdividableGroup(this.thingPart.baseType) != null)
		{
			base.AddButton("subdivide", null, "Detail...", "ButtonCompactSquareIcon", 235, -77, "SubdividedCube/SquareIcon", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		bool value = this.originalMeshIsConvex;
		bool? convex = this.thingPart.convex;
		if (convex != null)
		{
			bool? convex2 = this.thingPart.convex;
			value = convex2.Value;
		}
		base.AddCheckbox("concave", null, "Hollow collision", 235, 400, !value, 1f, "CheckboxCompact", TextColor.Default, null, ExtraIcon.Concave);
		this.AddResetAllConfirmButton();
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x00073F54 File Offset: 0x00072354
	private void AddResetAllButton(bool isConfirm = false)
	{
		string text = ((!isConfirm) ? "resetAll" : "resetAllConfirm");
		string text2 = ((!isConfirm) ? "Reset all?" : "Reset...");
		TextColor textColor = ((!isConfirm) ? TextColor.Red : TextColor.Gray);
		global::UnityEngine.Object.Destroy(this.resetAllButton);
		string text3 = text;
		string text4 = null;
		string text5 = text2;
		string text6 = "ButtonCompactNoIcon";
		int num = -235;
		int num2 = 400;
		TextColor textColor2 = textColor;
		this.resetAllButton = base.AddButton(text3, text4, text5, text6, num, num2, null, false, 1f, textColor2, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x00073FFF File Offset: 0x000723FF
	private void AddResetAllConfirmButton()
	{
		this.AddResetAllButton(true);
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x00074008 File Offset: 0x00072408
	private void AddIconCheckboxes()
	{
		base.StartCoroutine(this.DoAddIconCheckboxes());
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x00074018 File Offset: 0x00072418
	private IEnumerator DoAddIconCheckboxes()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		base.AddIconCheckbox("snapPosition", null, "Snap" + Environment.NewLine + "position", -400, -280, "SnapVertexPosition", VertexMover.snapPosition, 1f, false);
		base.AddIconCheckbox("showEdges", null, "Show" + Environment.NewLine + "edges", 75, -280, "ShowEdges", VertexMover.showEdges, 1f, false);
		base.AddIconCheckbox("snapToGrid", null, "Snap to" + Environment.NewLine + "grid", -400, -100, "SnapToVertexGrid", VertexMover.snapToGrid, 1f, false);
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x00074034 File Offset: 0x00072434
	private void AddSlider()
	{
		int? smoothingAngle = this.thingPart.smoothingAngle;
		int value;
		if (smoothingAngle != null)
		{
			int? smoothingAngle2 = this.thingPart.smoothingAngle;
			value = smoothingAngle2.Value;
		}
		else
		{
			value = this.defaultSmoothingAngle;
		}
		int num = value;
		float num2 = (float)num;
		this.slider = base.AddSlider("Smoothing: ", string.Empty, 0, 190, 0f, 180f, true, num2, new Action<float>(this.OnSliderChange), true, 0.8f);
		this.FinalizeSlider();
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x000740BC File Offset: 0x000724BC
	private void OnSliderChange(float value)
	{
		this.thingPart.smoothingAngle = new int?(Mathf.RoundToInt(value));
		if (this.thingPart.smoothingAngle == this.defaultSmoothingAngle)
		{
			this.thingPart.smoothingAngle = null;
		}
		this.vertexMover.RecalculateNormals();
		this.FinalizeSlider();
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x00074130 File Offset: 0x00072530
	private void FinalizeSlider()
	{
		DialogSlider dialogSlider = this.slider;
		int? smoothingAngle = this.thingPart.smoothingAngle;
		dialogSlider.valueSuffix = ((smoothingAngle != null) ? string.Empty : " (Default)".ToUpper());
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x00074177 File Offset: 0x00072577
	private new void Update()
	{
		if (this.thingPart == null || this.vertexMover == null)
		{
			base.CloseDialog();
		}
		base.ReactToOnClickInWrapper(this.secondaryWrapper);
		base.Update();
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x000741B4 File Offset: 0x000725B4
	public void SetThingPart(ThingPart thingPart)
	{
		this.thingPart.UpdateTextures(true);
		this.thingPart = thingPart;
		this.SetOriginalMeshIsConvex();
		this.defaultSmoothingAngle = 0;
		Managers.thingManager.smoothingAngles.TryGetValue(this.thingPart.baseType, out this.defaultSmoothingAngle);
		this.AddInterface();
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x00074208 File Offset: 0x00072608
	private void DropVertexMover()
	{
		if (this.vertexMover != null && this.vertexMover.transform.parent != null)
		{
			global::UnityEngine.Object.Destroy(this.vertexMover.transform.parent.gameObject);
		}
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0007425B File Offset: 0x0007265B
	private void OnDestroy()
	{
		if (this.dropVertexMoverOnClosing)
		{
			this.DropVertexMover();
		}
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00074270 File Offset: 0x00072670
	private void UpdateConvexBasedOnValue()
	{
		MeshCollider component = this.thingPart.GetComponent<MeshCollider>();
		if (component != null)
		{
			bool value = this.originalMeshIsConvex;
			bool? convex = this.thingPart.convex;
			if (convex != null)
			{
				bool? convex2 = this.thingPart.convex;
				value = convex2.Value;
			}
			component.convex = value;
		}
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x000742D0 File Offset: 0x000726D0
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "snapPosition":
			VertexMover.snapPosition = state;
			break;
		case "snapToGrid":
			VertexMover.snapToGrid = state;
			break;
		case "separatePoints":
			VertexMover.separatePoints = state;
			break;
		case "showEdges":
			VertexMover.showEdges = state;
			this.vertexMover.UpdateBasedOnShowEdges();
			break;
		case "resetAllConfirm":
			base.CancelInvoke("AddResetAllConfirmButton");
			this.AddResetAllButton(false);
			base.Invoke("AddResetAllConfirmButton", 4f);
			break;
		case "resetAll":
			base.CancelInvoke("AddResetAllConfirmButton");
			this.vertexMover.ResetAll();
			Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
			this.thingPart.convex = null;
			this.UpdateConvexBasedOnValue();
			this.AddInterface();
			break;
		case "toggleLock":
			this.thingPart.isLocked = false;
			global::UnityEngine.Object.Destroy(this.lockButton);
			this.AddInterface();
			break;
		case "subdivide":
			this.dropVertexMoverOnClosing = false;
			base.SwitchTo(DialogType.Subdivide, string.Empty);
			break;
		case "concave":
		{
			bool flag = !state;
			this.thingPart.convex = null;
			if (flag != this.originalMeshIsConvex)
			{
				this.thingPart.convex = new bool?(flag);
			}
			this.UpdateConvexBasedOnValue();
			break;
		}
		case "help":
			Managers.browserManager.OpenGuideBrowser("CBOPUw9-Ecg", null);
			break;
		case "minimize":
			this.dropVertexMoverOnClosing = false;
			base.CloseDialog();
			break;
		case "close":
			base.CloseDialog();
			break;
		case "back":
			base.SwitchTo(DialogType.ThingPart, string.Empty);
			break;
		}
	}

	// Token: 0x0400097C RID: 2428
	private ThingPart thingPart;

	// Token: 0x0400097D RID: 2429
	private bool dropVertexMoverOnClosing = true;

	// Token: 0x0400097E RID: 2430
	private VertexMover vertexMover;

	// Token: 0x0400097F RID: 2431
	private DialogSlider slider;

	// Token: 0x04000980 RID: 2432
	private const float sliderOffset = 0f;

	// Token: 0x04000981 RID: 2433
	private const float sliderFactor = 1.8f;

	// Token: 0x04000982 RID: 2434
	private int defaultSmoothingAngle;

	// Token: 0x04000983 RID: 2435
	private GameObject lockButton;

	// Token: 0x04000984 RID: 2436
	private GameObject secondaryWrapper;

	// Token: 0x04000985 RID: 2437
	private GameObject resetAllButton;

	// Token: 0x04000986 RID: 2438
	private bool originalMeshIsConvex = true;
}
