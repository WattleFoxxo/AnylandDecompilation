using System;

namespace FragLabs.Audio.Codecs.Opus
{
	// Token: 0x02000005 RID: 5
	public enum Errors
	{
		// Token: 0x0400000C RID: 12
		OK,
		// Token: 0x0400000D RID: 13
		BadArg = -1,
		// Token: 0x0400000E RID: 14
		BufferToSmall = -2,
		// Token: 0x0400000F RID: 15
		InternalError = -3,
		// Token: 0x04000010 RID: 16
		InvalidPacket = -4,
		// Token: 0x04000011 RID: 17
		Unimplemented = -5,
		// Token: 0x04000012 RID: 18
		InvalidState = -6,
		// Token: 0x04000013 RID: 19
		AllocFail = -7
	}
}
