using System;

namespace Valve.VR
{
	// Token: 0x0200012E RID: 302
	public enum ETrackedPropertyError
	{
		// Token: 0x0400018F RID: 399
		TrackedProp_Success,
		// Token: 0x04000190 RID: 400
		TrackedProp_WrongDataType,
		// Token: 0x04000191 RID: 401
		TrackedProp_WrongDeviceClass,
		// Token: 0x04000192 RID: 402
		TrackedProp_BufferTooSmall,
		// Token: 0x04000193 RID: 403
		TrackedProp_UnknownProperty,
		// Token: 0x04000194 RID: 404
		TrackedProp_InvalidDevice,
		// Token: 0x04000195 RID: 405
		TrackedProp_CouldNotContactServer,
		// Token: 0x04000196 RID: 406
		TrackedProp_ValueNotProvidedByDevice,
		// Token: 0x04000197 RID: 407
		TrackedProp_StringExceedsMaximumLength,
		// Token: 0x04000198 RID: 408
		TrackedProp_NotYetAvailable
	}
}
