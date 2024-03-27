using System;

// Token: 0x020000A9 RID: 169
[Serializable]
public class PhotonTransformViewRotationModel
{
	// Token: 0x0400045D RID: 1117
	public bool SynchronizeEnabled;

	// Token: 0x0400045E RID: 1118
	public PhotonTransformViewRotationModel.InterpolateOptions InterpolateOption = PhotonTransformViewRotationModel.InterpolateOptions.RotateTowards;

	// Token: 0x0400045F RID: 1119
	public float InterpolateRotateTowardsSpeed = 180f;

	// Token: 0x04000460 RID: 1120
	public float InterpolateLerpSpeed = 5f;

	// Token: 0x020000AA RID: 170
	public enum InterpolateOptions
	{
		// Token: 0x04000462 RID: 1122
		Disabled,
		// Token: 0x04000463 RID: 1123
		RotateTowards,
		// Token: 0x04000464 RID: 1124
		Lerp
	}
}
