using System;
using UnityEngine;

// Token: 0x020002BC RID: 700
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class DrawIfAttribute : PropertyAttribute
{
	// Token: 0x06001A18 RID: 6680 RVA: 0x000ECA08 File Offset: 0x000EAE08
	public DrawIfAttribute(string comparedPropertyName, object comparedValue, DrawIfAttribute.DisablingType disablingType = DrawIfAttribute.DisablingType.DontDraw)
	{
		this.comparedPropertyName = comparedPropertyName;
		this.comparedValue = comparedValue;
		this.disablingType = disablingType;
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06001A19 RID: 6681 RVA: 0x000ECA25 File Offset: 0x000EAE25
	// (set) Token: 0x06001A1A RID: 6682 RVA: 0x000ECA2D File Offset: 0x000EAE2D
	public string comparedPropertyName { get; private set; }

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06001A1B RID: 6683 RVA: 0x000ECA36 File Offset: 0x000EAE36
	// (set) Token: 0x06001A1C RID: 6684 RVA: 0x000ECA3E File Offset: 0x000EAE3E
	public object comparedValue { get; private set; }

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06001A1D RID: 6685 RVA: 0x000ECA47 File Offset: 0x000EAE47
	// (set) Token: 0x06001A1E RID: 6686 RVA: 0x000ECA4F File Offset: 0x000EAE4F
	public DrawIfAttribute.DisablingType disablingType { get; private set; }

	// Token: 0x020002BD RID: 701
	public enum DisablingType
	{
		// Token: 0x04001832 RID: 6194
		ReadOnly = 2,
		// Token: 0x04001833 RID: 6195
		DontDraw
	}
}
