using System;

namespace Valve.VR
{
	// Token: 0x02000138 RID: 312
	public enum EVROverlayError
	{
		// Token: 0x0400023A RID: 570
		None,
		// Token: 0x0400023B RID: 571
		UnknownOverlay = 10,
		// Token: 0x0400023C RID: 572
		InvalidHandle,
		// Token: 0x0400023D RID: 573
		PermissionDenied,
		// Token: 0x0400023E RID: 574
		OverlayLimitExceeded,
		// Token: 0x0400023F RID: 575
		WrongVisibilityType,
		// Token: 0x04000240 RID: 576
		KeyTooLong,
		// Token: 0x04000241 RID: 577
		NameTooLong,
		// Token: 0x04000242 RID: 578
		KeyInUse,
		// Token: 0x04000243 RID: 579
		WrongTransformType,
		// Token: 0x04000244 RID: 580
		InvalidTrackedDevice,
		// Token: 0x04000245 RID: 581
		InvalidParameter,
		// Token: 0x04000246 RID: 582
		ThumbnailCantBeDestroyed,
		// Token: 0x04000247 RID: 583
		ArrayTooSmall,
		// Token: 0x04000248 RID: 584
		RequestFailed,
		// Token: 0x04000249 RID: 585
		InvalidTexture,
		// Token: 0x0400024A RID: 586
		UnableToLoadFile,
		// Token: 0x0400024B RID: 587
		VROVerlayError_KeyboardAlreadyInUse,
		// Token: 0x0400024C RID: 588
		NoNeighbor
	}
}
