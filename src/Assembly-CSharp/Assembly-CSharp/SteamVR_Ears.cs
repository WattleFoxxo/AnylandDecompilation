using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000298 RID: 664
[RequireComponent(typeof(AudioListener))]
public class SteamVR_Ears : MonoBehaviour
{
	// Token: 0x06001926 RID: 6438 RVA: 0x000E409C File Offset: 0x000E249C
	private void OnNewPosesApplied(params object[] args)
	{
		Transform origin = this.vrcam.origin;
		Quaternion quaternion = ((!(origin != null)) ? Quaternion.identity : origin.rotation);
		base.transform.rotation = quaternion * this.offset;
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x000E40EC File Offset: 0x000E24EC
	private void OnEnable()
	{
		this.usingSpeakers = false;
		CVRSettings settings = OpenVR.Settings;
		if (settings != null)
		{
			EVRSettingsError evrsettingsError = EVRSettingsError.None;
			if (settings.GetBool("steamvr", "usingSpeakers", false, ref evrsettingsError))
			{
				this.usingSpeakers = true;
				float @float = settings.GetFloat("steamvr", "speakersForwardYawOffsetDegrees", 0f, ref evrsettingsError);
				this.offset = Quaternion.Euler(0f, @float, 0f);
			}
		}
		if (this.usingSpeakers)
		{
			SteamVR_Utils.Event.Listen("new_poses_applied", new SteamVR_Utils.Event.Handler(this.OnNewPosesApplied));
		}
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x000E417C File Offset: 0x000E257C
	private void OnDisable()
	{
		if (this.usingSpeakers)
		{
			SteamVR_Utils.Event.Remove("new_poses_applied", new SteamVR_Utils.Event.Handler(this.OnNewPosesApplied));
		}
	}

	// Token: 0x04001741 RID: 5953
	public SteamVR_Camera vrcam;

	// Token: 0x04001742 RID: 5954
	private bool usingSpeakers;

	// Token: 0x04001743 RID: 5955
	private Quaternion offset;
}
