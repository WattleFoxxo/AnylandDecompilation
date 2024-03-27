using System;

namespace Valve.VR
{
	// Token: 0x0200014E RID: 334
	public enum EVRRenderModelError
	{
		// Token: 0x04000338 RID: 824
		None,
		// Token: 0x04000339 RID: 825
		Loading = 100,
		// Token: 0x0400033A RID: 826
		NotSupported = 200,
		// Token: 0x0400033B RID: 827
		InvalidArg = 300,
		// Token: 0x0400033C RID: 828
		InvalidModel,
		// Token: 0x0400033D RID: 829
		NoShapes,
		// Token: 0x0400033E RID: 830
		MultipleShapes,
		// Token: 0x0400033F RID: 831
		TooManyVertices,
		// Token: 0x04000340 RID: 832
		MultipleTextures,
		// Token: 0x04000341 RID: 833
		BufferTooSmall,
		// Token: 0x04000342 RID: 834
		NotEnoughNormals,
		// Token: 0x04000343 RID: 835
		NotEnoughTexCoords,
		// Token: 0x04000344 RID: 836
		InvalidTexture = 400
	}
}
