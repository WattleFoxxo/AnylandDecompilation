using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
[Serializable]
public class PhotonTransformViewPositionModel
{
	// Token: 0x04000442 RID: 1090
	public bool SynchronizeEnabled;

	// Token: 0x04000443 RID: 1091
	public bool TeleportEnabled = true;

	// Token: 0x04000444 RID: 1092
	public float TeleportIfDistanceGreaterThan = 3f;

	// Token: 0x04000445 RID: 1093
	public PhotonTransformViewPositionModel.InterpolateOptions InterpolateOption = PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed;

	// Token: 0x04000446 RID: 1094
	public float InterpolateMoveTowardsSpeed = 1f;

	// Token: 0x04000447 RID: 1095
	public float InterpolateLerpSpeed = 1f;

	// Token: 0x04000448 RID: 1096
	public float InterpolateMoveTowardsAcceleration = 2f;

	// Token: 0x04000449 RID: 1097
	public float InterpolateMoveTowardsDeceleration = 2f;

	// Token: 0x0400044A RID: 1098
	public AnimationCurve InterpolateSpeedCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(-1f, 0f, 0f, float.PositiveInfinity),
		new Keyframe(0f, 1f, 0f, 0f),
		new Keyframe(1f, 1f, 0f, 1f),
		new Keyframe(4f, 4f, 1f, 0f)
	});

	// Token: 0x0400044B RID: 1099
	public PhotonTransformViewPositionModel.ExtrapolateOptions ExtrapolateOption;

	// Token: 0x0400044C RID: 1100
	public float ExtrapolateSpeed = 1f;

	// Token: 0x0400044D RID: 1101
	public bool ExtrapolateIncludingRoundTripTime = true;

	// Token: 0x0400044E RID: 1102
	public int ExtrapolateNumberOfStoredPositions = 1;

	// Token: 0x0400044F RID: 1103
	public bool DrawErrorGizmo = true;

	// Token: 0x020000A6 RID: 166
	public enum InterpolateOptions
	{
		// Token: 0x04000451 RID: 1105
		Disabled,
		// Token: 0x04000452 RID: 1106
		FixedSpeed,
		// Token: 0x04000453 RID: 1107
		EstimatedSpeed,
		// Token: 0x04000454 RID: 1108
		SynchronizeValues,
		// Token: 0x04000455 RID: 1109
		Lerp
	}

	// Token: 0x020000A7 RID: 167
	public enum ExtrapolateOptions
	{
		// Token: 0x04000457 RID: 1111
		Disabled,
		// Token: 0x04000458 RID: 1112
		SynchronizeValues,
		// Token: 0x04000459 RID: 1113
		EstimateSpeedAndTurn,
		// Token: 0x0400045A RID: 1114
		FixedSpeed
	}
}
