using System;

namespace POpusCodec.Enums
{
	// Token: 0x02000190 RID: 400
	public enum OpusStatusCode
	{
		// Token: 0x0400053E RID: 1342
		OK,
		// Token: 0x0400053F RID: 1343
		BadArguments = -1,
		// Token: 0x04000540 RID: 1344
		BufferTooSmall = -2,
		// Token: 0x04000541 RID: 1345
		InternalError = -3,
		// Token: 0x04000542 RID: 1346
		InvalidPacket = -4,
		// Token: 0x04000543 RID: 1347
		Unimplemented = -5,
		// Token: 0x04000544 RID: 1348
		InvalidState = -6,
		// Token: 0x04000545 RID: 1349
		AllocFail = -7
	}
}
