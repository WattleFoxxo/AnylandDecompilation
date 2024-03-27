using System;

// Token: 0x020000AC RID: 172
[Serializable]
public class PhotonTransformViewScaleModel
{
	// Token: 0x04000467 RID: 1127
	public bool SynchronizeEnabled;

	// Token: 0x04000468 RID: 1128
	public PhotonTransformViewScaleModel.InterpolateOptions InterpolateOption;

	// Token: 0x04000469 RID: 1129
	public float InterpolateMoveTowardsSpeed = 1f;

	// Token: 0x0400046A RID: 1130
	public float InterpolateLerpSpeed;

	// Token: 0x020000AD RID: 173
	public enum InterpolateOptions
	{
		// Token: 0x0400046C RID: 1132
		Disabled,
		// Token: 0x0400046D RID: 1133
		MoveTowards,
		// Token: 0x0400046E RID: 1134
		Lerp
	}
}
