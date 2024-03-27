using System;

namespace LitJson
{
	// Token: 0x0200003B RID: 59
	internal enum ParserToken
	{
		// Token: 0x04000120 RID: 288
		None = 65536,
		// Token: 0x04000121 RID: 289
		Number,
		// Token: 0x04000122 RID: 290
		True,
		// Token: 0x04000123 RID: 291
		False,
		// Token: 0x04000124 RID: 292
		Null,
		// Token: 0x04000125 RID: 293
		CharSeq,
		// Token: 0x04000126 RID: 294
		Char,
		// Token: 0x04000127 RID: 295
		Text,
		// Token: 0x04000128 RID: 296
		Object,
		// Token: 0x04000129 RID: 297
		ObjectPrime,
		// Token: 0x0400012A RID: 298
		Pair,
		// Token: 0x0400012B RID: 299
		PairRest,
		// Token: 0x0400012C RID: 300
		Array,
		// Token: 0x0400012D RID: 301
		ArrayPrime,
		// Token: 0x0400012E RID: 302
		Value,
		// Token: 0x0400012F RID: 303
		ValueRest,
		// Token: 0x04000130 RID: 304
		String,
		// Token: 0x04000131 RID: 305
		End,
		// Token: 0x04000132 RID: 306
		Epsilon
	}
}
