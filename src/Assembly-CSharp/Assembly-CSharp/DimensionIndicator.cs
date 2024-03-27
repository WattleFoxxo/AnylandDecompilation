using System;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class DimensionIndicator : MonoBehaviour
{
	// Token: 0x06000D3B RID: 3387 RVA: 0x00076A7A File Offset: 0x00074E7A
	private void Start()
	{
		this.originalPosition = base.transform.localPosition;
		this.originalRotation = base.transform.localEulerAngles;
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x00076AA0 File Offset: 0x00074EA0
	private void Update()
	{
		switch (this.type)
		{
		case DimensionIndicatorType.PositionX:
			this.SetPosition(true, false, false, 1f);
			break;
		case DimensionIndicatorType.PositionY:
			this.SetPosition(false, true, false, 2f);
			break;
		case DimensionIndicatorType.PositionZ:
			this.SetPosition(false, false, true, 1f);
			break;
		case DimensionIndicatorType.RotationX:
			this.SetRotation(true, false, false);
			break;
		case DimensionIndicatorType.RotationY:
			this.SetRotation(false, true, false);
			break;
		case DimensionIndicatorType.RotationZ:
			this.SetRotation(false, false, true);
			break;
		}
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x00076B3C File Offset: 0x00074F3C
	private void SetPosition(bool x, bool y, bool z, float maxFactor = 1f)
	{
		Vector3 vector = this.originalPosition;
		if (x)
		{
			vector.x += 0.0025f * maxFactor * Mathf.Sin(Time.time * 5f);
		}
		else if (y)
		{
			vector.y += 0.0025f * maxFactor * Mathf.Sin(Time.time * 5f);
		}
		else if (z)
		{
			vector.z += 0.0025f * maxFactor * Mathf.Sin(Time.time * 5f);
		}
		base.transform.localPosition = vector;
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x00076BF0 File Offset: 0x00074FF0
	private void SetRotation(bool x, bool y, bool z)
	{
		Vector3 vector = this.originalRotation;
		if (x)
		{
			vector.x += 90f * Mathf.Sin(Time.time * 5f);
		}
		else if (y)
		{
			vector.y += 90f * Mathf.Sin(Time.time * 5f);
		}
		else if (z)
		{
			vector.z += 90f * Mathf.Sin(Time.time * 5f);
		}
		base.transform.localEulerAngles = vector;
	}

	// Token: 0x04000ED7 RID: 3799
	public Transform shape;

	// Token: 0x04000ED8 RID: 3800
	public DimensionIndicatorType type;

	// Token: 0x04000ED9 RID: 3801
	private Vector3 originalPosition;

	// Token: 0x04000EDA RID: 3802
	private const float positionMax = 0.0025f;

	// Token: 0x04000EDB RID: 3803
	private const float positionSpeed = 5f;

	// Token: 0x04000EDC RID: 3804
	private Vector3 originalRotation;

	// Token: 0x04000EDD RID: 3805
	private const float rotationMax = 90f;

	// Token: 0x04000EDE RID: 3806
	private const float rotationSpeed = 5f;
}
