using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class IncludedSubThingsLines : MonoBehaviour
{
	// Token: 0x06000BDF RID: 3039 RVA: 0x00064AEC File Offset: 0x00062EEC
	private void Start()
	{
		this.thingPart = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
		Shader shader = Shader.Find("Custom/SeeThroughLine");
		this.lineMaterial = new Material(shader);
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x00064B20 File Offset: 0x00062F20
	private void OnPostRender()
	{
		if (this.thingPart == null)
		{
			return;
		}
		List<Transform> includedSubThingTransforms = this.thingPart.GetIncludedSubThingTransforms();
		Vector3 position = this.thingPart.transform.position;
		this.lineMaterial.SetPass(0);
		GL.Begin(1);
		Color color = new Color(1f, 1f, 1f, 0.5f);
		GL.Color(color);
		foreach (Transform transform in includedSubThingTransforms)
		{
			Vector3 position2 = transform.transform.position;
			GL.Vertex3(position.x, position.y, position.z);
			GL.Vertex3(position2.x, position2.y, position2.z);
		}
		GL.End();
	}

	// Token: 0x040008FE RID: 2302
	private Material lineMaterial;

	// Token: 0x040008FF RID: 2303
	private ThingPart thingPart;
}
