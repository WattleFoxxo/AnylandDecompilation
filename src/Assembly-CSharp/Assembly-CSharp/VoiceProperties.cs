using System;

// Token: 0x0200027D RID: 637
public class VoiceProperties
{
	// Token: 0x0400167C RID: 5756
	public VoiceProperties.Gender gender = VoiceProperties.Gender.Female;

	// Token: 0x0400167D RID: 5757
	public int volume = 100;

	// Token: 0x0400167E RID: 5758
	public int pitch;

	// Token: 0x0400167F RID: 5759
	public int speed;

	// Token: 0x0200027E RID: 638
	public enum Gender
	{
		// Token: 0x04001681 RID: 5761
		Male,
		// Token: 0x04001682 RID: 5762
		Female
	}
}
