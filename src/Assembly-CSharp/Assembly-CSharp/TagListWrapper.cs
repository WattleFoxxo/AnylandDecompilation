using System;
using System.Collections.Generic;

// Token: 0x0200016C RID: 364
[Serializable]
public class TagListWrapper
{
	// Token: 0x06000CFC RID: 3324 RVA: 0x00075373 File Offset: 0x00073773
	public TagListWrapper(List<string> _tags)
	{
		this.tags = _tags;
	}

	// Token: 0x04000A6A RID: 2666
	public List<string> tags;
}
