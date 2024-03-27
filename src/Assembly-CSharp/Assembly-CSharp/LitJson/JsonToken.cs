using System;

namespace LitJson
{
	// Token: 0x02000033 RID: 51
	public enum JsonToken
	{
		// Token: 0x040000BF RID: 191
		None,
		// Token: 0x040000C0 RID: 192
		ObjectStart,
		// Token: 0x040000C1 RID: 193
		PropertyName,
		// Token: 0x040000C2 RID: 194
		ObjectEnd,
		// Token: 0x040000C3 RID: 195
		ArrayStart,
		// Token: 0x040000C4 RID: 196
		ArrayEnd,
		// Token: 0x040000C5 RID: 197
		Int,
		// Token: 0x040000C6 RID: 198
		Long,
		// Token: 0x040000C7 RID: 199
		Double,
		// Token: 0x040000C8 RID: 200
		String,
		// Token: 0x040000C9 RID: 201
		Boolean,
		// Token: 0x040000CA RID: 202
		Null
	}
}
