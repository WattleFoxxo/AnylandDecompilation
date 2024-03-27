using System;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class ThingIdPositionRotation
{
	// Token: 0x06000CFE RID: 3326 RVA: 0x0007538A File Offset: 0x0007378A
	public ThingIdPositionRotation(string _thingId, Vector3 _position, Vector3 _rotation)
	{
		this.thingId = _thingId;
		this.position = _position;
		this.rotation = _rotation;
	}

	// Token: 0x04000A6D RID: 2669
	public string thingId;

	// Token: 0x04000A6E RID: 2670
	public Vector3 position;

	// Token: 0x04000A6F RID: 2671
	public Vector3 rotation;
}
