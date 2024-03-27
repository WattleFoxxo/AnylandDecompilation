using System;

// Token: 0x02000275 RID: 629
public class ThingDestruction
{
	// Token: 0x040015BA RID: 5562
	public bool burst;

	// Token: 0x040015BB RID: 5563
	public float burstVelocity;

	// Token: 0x040015BC RID: 5564
	public int maxParts = 40;

	// Token: 0x040015BD RID: 5565
	public float growth;

	// Token: 0x040015BE RID: 5566
	public bool bouncy;

	// Token: 0x040015BF RID: 5567
	public bool slidy;

	// Token: 0x040015C0 RID: 5568
	public float hidePartsInSeconds = 12.5f;

	// Token: 0x040015C1 RID: 5569
	public float? restoreInSeconds;

	// Token: 0x040015C2 RID: 5570
	public bool gravity = true;

	// Token: 0x040015C3 RID: 5571
	public bool collides = true;

	// Token: 0x040015C4 RID: 5572
	public bool collidesWithSiblings = true;
}
