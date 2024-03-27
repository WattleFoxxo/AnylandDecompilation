using System;

// Token: 0x02000071 RID: 113
public class RaiseEventOptions
{
	// Token: 0x06000360 RID: 864 RVA: 0x0000DAE8 File Offset: 0x0000BEE8
	public void Reset()
	{
		this.CachingOption = RaiseEventOptions.Default.CachingOption;
		this.InterestGroup = RaiseEventOptions.Default.InterestGroup;
		this.TargetActors = RaiseEventOptions.Default.TargetActors;
		this.Receivers = RaiseEventOptions.Default.Receivers;
		this.SequenceChannel = RaiseEventOptions.Default.SequenceChannel;
		this.ForwardToWebhook = RaiseEventOptions.Default.ForwardToWebhook;
		this.Encrypt = RaiseEventOptions.Default.Encrypt;
	}

	// Token: 0x040002C8 RID: 712
	public static readonly RaiseEventOptions Default = new RaiseEventOptions();

	// Token: 0x040002C9 RID: 713
	public EventCaching CachingOption;

	// Token: 0x040002CA RID: 714
	public byte InterestGroup;

	// Token: 0x040002CB RID: 715
	public int[] TargetActors;

	// Token: 0x040002CC RID: 716
	public ReceiverGroup Receivers;

	// Token: 0x040002CD RID: 717
	public byte SequenceChannel;

	// Token: 0x040002CE RID: 718
	public bool ForwardToWebhook;

	// Token: 0x040002CF RID: 719
	public bool Encrypt;
}
