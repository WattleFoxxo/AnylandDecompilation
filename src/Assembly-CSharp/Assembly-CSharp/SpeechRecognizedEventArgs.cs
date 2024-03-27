using System;

// Token: 0x02000252 RID: 594
public class SpeechRecognizedEventArgs : EventArgs
{
	// Token: 0x06001612 RID: 5650 RVA: 0x000C1225 File Offset: 0x000BF625
	public SpeechRecognizedEventArgs(string text)
	{
		this.text = text;
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06001613 RID: 5651 RVA: 0x000C1234 File Offset: 0x000BF634
	public string Text
	{
		get
		{
			return this.text;
		}
	}

	// Token: 0x040013D0 RID: 5072
	private readonly string text;
}
