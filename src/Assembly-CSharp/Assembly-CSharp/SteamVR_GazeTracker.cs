using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000284 RID: 644
public class SteamVR_GazeTracker : MonoBehaviour
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06001861 RID: 6241 RVA: 0x000E07EC File Offset: 0x000DEBEC
	// (remove) Token: 0x06001862 RID: 6242 RVA: 0x000E0824 File Offset: 0x000DEC24
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GazeEventHandler GazeOn;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06001863 RID: 6243 RVA: 0x000E085C File Offset: 0x000DEC5C
	// (remove) Token: 0x06001864 RID: 6244 RVA: 0x000E0894 File Offset: 0x000DEC94
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GazeEventHandler GazeOff;

	// Token: 0x06001865 RID: 6245 RVA: 0x000E08CA File Offset: 0x000DECCA
	private void Start()
	{
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x000E08CC File Offset: 0x000DECCC
	public virtual void OnGazeOn(GazeEventArgs e)
	{
		if (this.GazeOn != null)
		{
			this.GazeOn(this, e);
		}
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x000E08E6 File Offset: 0x000DECE6
	public virtual void OnGazeOff(GazeEventArgs e)
	{
		if (this.GazeOff != null)
		{
			this.GazeOff(this, e);
		}
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x000E0900 File Offset: 0x000DED00
	private void Update()
	{
		if (this.hmdTrackedObject == null)
		{
			SteamVR_TrackedObject[] array = global::UnityEngine.Object.FindObjectsOfType<SteamVR_TrackedObject>();
			foreach (SteamVR_TrackedObject steamVR_TrackedObject in array)
			{
				if (steamVR_TrackedObject.index == SteamVR_TrackedObject.EIndex.Hmd)
				{
					this.hmdTrackedObject = steamVR_TrackedObject.transform;
					break;
				}
			}
		}
		if (this.hmdTrackedObject)
		{
			Ray ray = new Ray(this.hmdTrackedObject.position, this.hmdTrackedObject.forward);
			Plane plane = new Plane(this.hmdTrackedObject.forward, base.transform.position);
			float num = 0f;
			if (plane.Raycast(ray, out num))
			{
				Vector3 vector = this.hmdTrackedObject.position + this.hmdTrackedObject.forward * num;
				float num2 = Vector3.Distance(vector, base.transform.position);
				if (num2 < this.gazeInCutoff && !this.isInGaze)
				{
					this.isInGaze = true;
					GazeEventArgs gazeEventArgs;
					gazeEventArgs.distance = num2;
					this.OnGazeOn(gazeEventArgs);
				}
				else if (num2 >= this.gazeOutCutoff && this.isInGaze)
				{
					this.isInGaze = false;
					GazeEventArgs gazeEventArgs2;
					gazeEventArgs2.distance = num2;
					this.OnGazeOff(gazeEventArgs2);
				}
			}
		}
	}

	// Token: 0x040016CB RID: 5835
	public bool isInGaze;

	// Token: 0x040016CE RID: 5838
	public float gazeInCutoff = 0.15f;

	// Token: 0x040016CF RID: 5839
	public float gazeOutCutoff = 0.4f;

	// Token: 0x040016D0 RID: 5840
	private Transform hmdTrackedObject;
}
