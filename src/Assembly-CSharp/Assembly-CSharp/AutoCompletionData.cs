using System;

// Token: 0x0200011A RID: 282
public class AutoCompletionData
{
	// Token: 0x06000A54 RID: 2644 RVA: 0x0004C4FF File Offset: 0x0004A8FF
	public AutoCompletionData(string completion, string help = "", string addToCompletionDisplay = "")
	{
		this.completion = completion;
		this.help = help;
		this.addToCompletionDisplay = addToCompletionDisplay;
	}

	// Token: 0x040007BB RID: 1979
	public string completion = string.Empty;

	// Token: 0x040007BC RID: 1980
	public string help = string.Empty;

	// Token: 0x040007BD RID: 1981
	public string addToCompletionDisplay = string.Empty;
}
