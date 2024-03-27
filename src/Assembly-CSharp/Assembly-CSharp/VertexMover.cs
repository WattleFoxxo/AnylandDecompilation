using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class VertexMover : MonoBehaviour
{
	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060007FD RID: 2045 RVA: 0x0002CBD5 File Offset: 0x0002AFD5
	// (set) Token: 0x060007FE RID: 2046 RVA: 0x0002CBDD File Offset: 0x0002AFDD
	public HandDot handDot { get; private set; }

	// Token: 0x060007FF RID: 2047 RVA: 0x0002CBE8 File Offset: 0x0002AFE8
	private void Start()
	{
		this.hand = base.transform.parent.parent.GetComponent<Hand>();
		this.handDot = this.hand.handDot.GetComponent<HandDot>();
		this.didLetGoSinceGrabbing = CrossDevice.hasSeparateTriggerAndGrab;
		this.vertexHighlight = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/VertexHighlight"));
		Misc.RemoveCloneFromName(this.vertexHighlight);
		this.SetThingPart(CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>());
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0002CC68 File Offset: 0x0002B068
	public void SetThingPart(ThingPart thingPart)
	{
		if (this.grabbedVertexIndex != -1 && this.mesh != null)
		{
			this.UpdateMeshAfterChanges();
		}
		this.thingPart = thingPart;
		this.thing = this.thingPart.GetMyRootThing();
		Managers.thingManager.smoothingAngles.TryGetValue(thingPart.baseType, out this.defaultSmoothingAngle);
		MeshFilter component = this.thingPart.GetComponent<MeshFilter>();
		if (component != null)
		{
			this.mesh = component.mesh;
			this.mesh.MarkDynamic();
		}
		this.SetVertexHighlightColor();
		this.UpdateBasedOnShowEdges();
		this.ResetGrabData();
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0002CD10 File Offset: 0x0002B110
	private void ResetGrabData()
	{
		this.pressed = false;
		this.closestVertexIndex = -1;
		this.grabbedVertexIndex = -1;
		this.grabStart = Vector3.zero;
		this.grabStartVertex = Vector3.zero;
		this.originalVertexPosition = Vector3.zero;
		this.vertexHighlight.SetActive(false);
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0002CD60 File Offset: 0x0002B160
	private void Update()
	{
		if (this.thingPart == null || this.mesh == null || this.hand == null || Our.mode != EditModes.Thing || CreationHelper.thingPartWhoseStatesAreEdited == null)
		{
			global::UnityEngine.Object.Destroy(base.transform.parent.gameObject);
			return;
		}
		this.AdjustVertexHighlightSizeByDistance();
		this.HandlePress();
		this.FollowTweezerTargets();
		if (this.grabbedVertexIndex == -1 && !this.pressed)
		{
			this.SetClosestVertex();
		}
		if (this.grabbedVertexIndex != -1)
		{
			this.AdjustGrabbedVertexPosition(false);
			this.hand.TriggerHapticPulse(Universe.veryLowHapticPulse);
		}
		this.MonitorVertexSelectionTouches();
		if (this.thingPartMemory.SetFromTransformIfDifferent(this.thingPart.transform))
		{
			this.UpdateSelectedVertexIndicatorPositions();
		}
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0002CE4C File Offset: 0x0002B24C
	private void AdjustVertexHighlightSizeByDistance()
	{
		if (this.vertexHighlight != null)
		{
			Vector3 position = Managers.personManager.ourPerson.Head.transform.position;
			float num = Vector3.Distance(position, this.vertexHighlight.transform.position);
			float num2 = num * 0.005f;
			this.vertexHighlight.transform.localScale = Misc.GetUniformVector3(Mathf.Clamp(num2, 0.01f, 10f));
		}
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0002CEC8 File Offset: 0x0002B2C8
	private void AdjustGrabbedVertexPosition(bool roundIfNeeded = false)
	{
		Vector3 vector = base.transform.position - this.grabStart;
		Vector3 vector2 = this.originalVertexPosition + vector;
		this.vertexHighlight.transform.position = vector2;
		Vector3 vector3 = this.mesh.vertices[this.grabbedVertexIndex];
		Vector3 vector4 = this.thingPart.transform.InverseTransformPoint(vector2);
		if (vector3 != vector4)
		{
			Vector3[] vertices = this.mesh.vertices;
			if (VertexMover.snapPosition)
			{
				vector4 = SnapHelper.SnapPositionAlongAxis(vector4, this.grabStartVertex);
			}
			if (VertexMover.snapToGrid)
			{
				vector4 = this.SnapToGrid(vector4, 0.1f);
			}
			this.vertexHighlight.transform.position = this.thingPart.transform.TransformPoint(vector4);
			if (this.thingPart.changedVertices == null)
			{
				this.thingPart.changedVertices = new Dictionary<int, Vector3>();
			}
			if (roundIfNeeded && Misc.GetLargestValueOfVector(this.thingPart.transform.localScale) <= 10f)
			{
				vector4 = Misc.ReduceVector3Digits(vector4, 3);
			}
			if (VertexMover.separatePoints)
			{
				vertices[this.grabbedVertexIndex] = vector4;
				this.thingPart.changedVertices[this.grabbedVertexIndex] = vector4;
			}
			else
			{
				for (int i = 0; i < vertices.Length; i++)
				{
					if (vertices[i] == vector3 && !this.selectedVertices.ContainsKey(i))
					{
						vertices[i] = vector4;
						this.thingPart.changedVertices[i] = vector4;
					}
				}
			}
			Vector3 vector5 = vector4 - vector3;
			this.UpdateSelectedVerticesIndicatorPositions(vertices, vector5);
			this.mesh.vertices = vertices;
			this.UpdateSelectedVertexIndicatorPositions();
			this.RecalculateNormals();
		}
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0002D0B8 File Offset: 0x0002B4B8
	private void UpdateSelectedVerticesIndicatorPositions(Vector3[] vertices, Vector3 offset)
	{
		foreach (KeyValuePair<int, Transform> keyValuePair in this.selectedVertices)
		{
			int key = keyValuePair.Key;
			Transform value = keyValuePair.Value;
			for (int i = 0; i < vertices.Length; i++)
			{
				if (vertices[i] == vertices[key])
				{
					vertices[i] += offset;
					this.thingPart.changedVertices[i] = vertices[i];
				}
			}
			value.localPosition = vertices[key];
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0002D1A4 File Offset: 0x0002B5A4
	private Vector3 SnapToGrid(Vector3 position, float gridSize)
	{
		return new Vector3(SnapHelper.SnapValueToGrid(position.x, gridSize), SnapHelper.SnapValueToGrid(position.y, gridSize), SnapHelper.SnapValueToGrid(position.z, gridSize));
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0002D1D4 File Offset: 0x0002B5D4
	private void SetClosestVertex()
	{
		float num = 100f;
		int num2 = -1;
		Vector3 vector = Vector3.zero;
		Vector3 position = base.transform.position;
		Transform transform = this.thingPart.transform;
		Vector3[] vertices = this.mesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector2 = transform.TransformPoint(vertices[i]);
			float num3 = Vector3.Distance(position, vector2);
			if (num3 <= num)
			{
				num2 = i;
				vector = vector2;
				num = num3;
			}
		}
		if (num2 != -1)
		{
			if (this.closestVertexIndex != num2)
			{
				this.closestVertexIndex = num2;
				this.hand.TriggerHapticPulse(Universe.lowHapticPulse);
			}
			this.vertexHighlight.SetActive(true);
			this.vertexHighlight.transform.position = vector;
			this.originalVertexPosition = vector;
		}
		else
		{
			this.vertexHighlight.SetActive(false);
		}
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x0002D2C0 File Offset: 0x0002B6C0
	private void MonitorVertexSelectionTouches()
	{
		float num = 0.01f;
		int num2 = -1;
		Vector3 zero = Vector3.zero;
		Vector3 position = this.selecter.position;
		Transform transform = this.thingPart.transform;
		Vector3[] vertices = this.mesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = transform.TransformPoint(vertices[i]);
			float num3 = Vector3.Distance(position, vector);
			if (num3 <= num)
			{
				num2 = i;
				num = num3;
			}
		}
		if (num2 != -1)
		{
			bool flag = this.closestSelectedVertexIndex != num2;
			if (flag)
			{
				this.closestSelectedVertexIndex = num2;
				this.ToggleSelectedVertex(this.closestSelectedVertexIndex);
			}
		}
		else
		{
			this.closestSelectedVertexIndex = -1;
		}
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x0002D388 File Offset: 0x0002B788
	private void ToggleSelectedVertex(int vertexIndex)
	{
		Transform transform;
		if (this.selectedVertices.TryGetValue(vertexIndex, out transform))
		{
			if (transform != null)
			{
				Managers.soundManager.Play("putDown", transform, 0.3f, false, false);
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
			this.selectedVertices.Remove(vertexIndex);
		}
		else if (this.thingPart != null && this.thingPart.transform.transform != null && this.mesh != null)
		{
			Transform transform2 = global::UnityEngine.Object.Instantiate<Transform>(this.selectedVertexPrefab);
			this.selectedVertices.Add(vertexIndex, transform2);
			this.UpdateSelectedVertexIndicatorPosition(vertexIndex, transform2);
			Managers.soundManager.Play("pickUp", transform2, 0.3f, false, false);
			Component[] componentsInChildren = transform2.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.material.color = this.highlightColor;
			}
		}
		this.hand.TriggerHapticPulse(Universe.miniBurstPulse);
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x0002D4AC File Offset: 0x0002B8AC
	private void UpdateSelectedVertexIndicatorPositions()
	{
		foreach (KeyValuePair<int, Transform> keyValuePair in this.selectedVertices)
		{
			this.UpdateSelectedVertexIndicatorPosition(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0002D518 File Offset: 0x0002B918
	private void UpdateSelectedVertexIndicatorPosition(int index, Transform indicatorTransform)
	{
		indicatorTransform.position = this.thingPart.transform.TransformPoint(this.mesh.vertices[index]);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0002D546 File Offset: 0x0002B946
	private void OnTriggerStay(Collider other)
	{
		if (this.handDot != null && !other.gameObject.CompareTag("ThingPart"))
		{
			this.handDot.HandlePressableDialogPartCollision(other);
		}
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0002D57C File Offset: 0x0002B97C
	private void HandlePress()
	{
		if (this.hand.GetPressDown(CrossDevice.button_grabTip))
		{
			if (this.didLetGoSinceGrabbing && !this.thingPart.isLocked)
			{
				this.pressed = true;
				this.grabbedVertexIndex = this.closestVertexIndex;
				this.grabStart = base.transform.position;
				if (this.grabbedVertexIndex != -1)
				{
					this.grabStartVertex = this.mesh.vertices[this.grabbedVertexIndex];
				}
			}
		}
		else if (this.hand.GetPressUp(CrossDevice.button_grabTip))
		{
			this.didLetGoSinceGrabbing = true;
			this.pressed = false;
			if (this.grabbedVertexIndex != -1)
			{
				this.AdjustGrabbedVertexPosition(true);
				this.grabbedVertexIndex = -1;
				this.UpdateMeshAfterChanges();
			}
		}
		if (CrossDevice.hasSeparateTriggerAndGrab && this.hand.GetPressUp(CrossDevice.button_grab))
		{
			if (Managers.dialogManager.GetCurrentNonStartDialogType() == DialogType.VertexMover)
			{
				Managers.dialogManager.SwitchToNewDialog(DialogType.ThingPart, null, string.Empty);
			}
			else
			{
				global::UnityEngine.Object.Destroy(base.transform.parent.gameObject);
			}
		}
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x0002D6B0 File Offset: 0x0002BAB0
	public void UpdateMeshAfterChanges()
	{
		this.RecalculateNormals();
		bool flag = this.mesh.vertices.Length <= 500;
		if (flag)
		{
			JsonToThingConverter.RecreateColliderAndChangeTypeIfNeeded(this.thingPart, this.mesh);
		}
		this.mesh.RecalculateTangents();
		this.CreateInsideMeshFacesIfNeeded();
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x0002D704 File Offset: 0x0002BB04
	public void ResetAll()
	{
		int baseType = (int)this.thingPart.baseType;
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingPartBases[baseType]);
		MeshFilter component = gameObject.GetComponent<MeshFilter>();
		this.mesh.vertices = component.mesh.vertices;
		global::UnityEngine.Object.Destroy(gameObject);
		this.thingPart.changedVertices = null;
		this.thingPart.smoothingAngle = null;
		this.UpdateMeshAfterChanges();
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0002D778 File Offset: 0x0002BB78
	public void RecalculateNormals()
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
		this.mesh.RecalculateNormals((float)num);
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x0002D7CC File Offset: 0x0002BBCC
	private void FollowTweezerTargets()
	{
		this.tweezerAngle = Mathf.MoveTowards(this.tweezerAngle, (!this.pressed) ? 0f : 5.35f, 100f * Time.deltaTime);
		this.tweezers[0].localEulerAngles = new Vector3(-this.tweezerAngle, 0f, 0f);
		this.tweezers[1].localEulerAngles = new Vector3(this.tweezerAngle, 0f, 0f);
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x0002D854 File Offset: 0x0002BC54
	private void SetVertexHighlightColor()
	{
		Renderer component = this.thingPart.GetComponent<Renderer>();
		if (component != null)
		{
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(component.material.color, out num, out num2, out num3);
			Renderer component2 = this.vertexHighlight.GetComponent<Renderer>();
			this.highlightColor = ((num3 > 0.75f) ? new Color(0f, 0f, 0f, 0.7f) : new Color(1f, 1f, 1f, 0.5f));
			component2.material.color = this.highlightColor;
		}
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x0002D8F8 File Offset: 0x0002BCF8
	public void UpdateBasedOnShowEdges()
	{
		if (VertexMover.showEdges)
		{
			Renderer component = this.thingPart.GetComponent<Renderer>();
			Material[] materials = component.materials;
			Material[] array = new Material[materials.Length + 2];
			int i;
			for (i = 0; i < materials.Length; i++)
			{
				array[i] = materials[i];
			}
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(component.material.color, out num, out num2, out num3);
			num3 = ((num3 > 0.5f) ? (num3 - 0.1f) : (num3 + 0.1f));
			num3 = Mathf.Clamp(num3, 0f, 1f);
			Color color = Color.HSVToRGB(num, num2, num3);
			this.wireframeMaterial.SetColor("_ShaderColor", color);
			this.wireframeMaterial.SetColor("_BaseColor", component.material.color);
			this.wireframeMaterial.SetFloat("_ShaderStrength", 0.2f);
			this.wireframeMaterial.SetInt("_WireSmoothness", 20);
			array[i + 1] = this.wireframeMaterial;
			component.materials = array;
			this.appliedWireframeThingPart = this.thingPart;
		}
		else
		{
			this.ClearCurrentAppliedWireframe();
		}
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x0002DA21 File Offset: 0x0002BE21
	private void ClearCurrentAppliedWireframe()
	{
		if (this.appliedWireframeThingPart != null)
		{
			this.appliedWireframeThingPart.UpdateTextures(true);
			this.appliedWireframeThingPart = null;
		}
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0002DA48 File Offset: 0x0002BE48
	public void SwitchToNewMeshForSubdivision(ThingPartBase baseType)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingPartBases[(int)baseType]);
		MeshFilter component = gameObject.GetComponent<MeshFilter>();
		MeshFilter component2 = this.thingPart.GetComponent<MeshFilter>();
		component2.sharedMesh = component.mesh;
		this.mesh = component2.sharedMesh;
		this.mesh.vertices = component.mesh.vertices;
		global::UnityEngine.Object.Destroy(gameObject);
		this.mesh.RecalculateBounds();
		this.mesh.RecalculateNormals();
		this.mesh.RecalculateTangents();
		this.mesh = null;
		this.mesh = this.thingPart.GetComponent<MeshFilter>().mesh;
		this.thingPart.baseType = baseType;
		this.UpdateMeshAfterChanges();
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0002DB00 File Offset: 0x0002BF00
	private void CreateInsideMeshFacesIfNeeded()
	{
		if (VertexMover.separatePoints)
		{
			if (this.insideMesh == null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingPart.gameObject);
				gameObject.transform.parent = this.thingPart.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				global::UnityEngine.Object.Destroy(gameObject.GetComponent<ThingPart>());
				global::UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
				gameObject.tag = "Untagged";
				MeshFilter component = gameObject.GetComponent<MeshFilter>();
				this.insideMesh = component.mesh;
				gameObject.GetComponent<MeshFilter>().mesh = this.insideMesh;
				this.InvertNormals(this.insideMesh);
			}
			else
			{
				this.insideMesh.vertices = this.mesh.vertices;
			}
		}
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x0002DBEC File Offset: 0x0002BFEC
	private void InvertNormals(Mesh mesh)
	{
		Vector3[] normals = mesh.normals;
		for (int i = 0; i < normals.Length; i++)
		{
			normals[i] = -normals[i];
		}
		mesh.normals = normals;
		for (int j = 0; j < mesh.subMeshCount; j++)
		{
			int[] triangles = mesh.GetTriangles(j);
			for (int k = 0; k < triangles.Length; k += 3)
			{
				int num = triangles[k];
				triangles[k] = triangles[k + 1];
				triangles[k + 1] = num;
			}
			mesh.SetTriangles(triangles, j);
		}
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x0002DC90 File Offset: 0x0002C090
	private void ClearSelectedVertices()
	{
		foreach (KeyValuePair<int, Transform> keyValuePair in this.selectedVertices)
		{
			Transform value = keyValuePair.Value;
			if (value != null)
			{
				global::UnityEngine.Object.Destroy(value.gameObject);
			}
		}
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0002DD04 File Offset: 0x0002C104
	private void OnDestroy()
	{
		this.ClearCurrentAppliedWireframe();
		this.ClearSelectedVertices();
		global::UnityEngine.Object.Destroy(this.vertexHighlight);
	}

	// Token: 0x04000608 RID: 1544
	public Material wireframeMaterial;

	// Token: 0x04000609 RID: 1545
	private ThingPart thingPart;

	// Token: 0x0400060A RID: 1546
	private Thing thing;

	// Token: 0x0400060B RID: 1547
	private bool pressed;

	// Token: 0x0400060C RID: 1548
	public Transform[] tweezers;

	// Token: 0x0400060D RID: 1549
	private Hand hand;

	// Token: 0x0400060F RID: 1551
	public Mesh mesh;

	// Token: 0x04000610 RID: 1552
	private bool didLetGoSinceGrabbing;

	// Token: 0x04000611 RID: 1553
	private GameObject vertexHighlight;

	// Token: 0x04000612 RID: 1554
	private int defaultSmoothingAngle;

	// Token: 0x04000613 RID: 1555
	private float tweezerAngle;

	// Token: 0x04000614 RID: 1556
	private ThingPart appliedWireframeThingPart;

	// Token: 0x04000615 RID: 1557
	private int closestVertexIndex = -1;

	// Token: 0x04000616 RID: 1558
	private int grabbedVertexIndex = -1;

	// Token: 0x04000617 RID: 1559
	private Vector3 grabStart = Vector3.zero;

	// Token: 0x04000618 RID: 1560
	private Vector3 originalVertexPosition = Vector3.zero;

	// Token: 0x04000619 RID: 1561
	private Vector3 grabStartVertex = Vector3.zero;

	// Token: 0x0400061A RID: 1562
	public static bool snapPosition;

	// Token: 0x0400061B RID: 1563
	public static bool snapToGrid;

	// Token: 0x0400061C RID: 1564
	public static bool showEdges;

	// Token: 0x0400061D RID: 1565
	public static bool separatePoints;

	// Token: 0x0400061E RID: 1566
	public const int maxVertexCountForPreciseCollisions = 500;

	// Token: 0x0400061F RID: 1567
	public const int positionFloatDigits = 3;

	// Token: 0x04000620 RID: 1568
	private Color highlightColor = Color.white;

	// Token: 0x04000621 RID: 1569
	private Mesh insideMesh;

	// Token: 0x04000622 RID: 1570
	public Transform selecter;

	// Token: 0x04000623 RID: 1571
	private Dictionary<int, Transform> selectedVertices = new Dictionary<int, Transform>();

	// Token: 0x04000624 RID: 1572
	private int closestSelectedVertexIndex = -1;

	// Token: 0x04000625 RID: 1573
	public Transform selectedVertexPrefab;

	// Token: 0x04000626 RID: 1574
	private Vector3 thingPartOldPosition = Vector3.zero;

	// Token: 0x04000627 RID: 1575
	private TransformClipboard thingPartMemory = new TransformClipboard();
}
