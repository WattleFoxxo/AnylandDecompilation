using System;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class SteamVR_Teleporter : MonoBehaviour
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06001877 RID: 6263 RVA: 0x000E0EEC File Offset: 0x000DF2EC
	private Transform reference
	{
		get
		{
			SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
			return (!(steamVR_Camera != null)) ? null : steamVR_Camera.origin;
		}
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x000E0F18 File Offset: 0x000DF318
	private void Start()
	{
		SteamVR_TrackedController steamVR_TrackedController = base.GetComponent<SteamVR_TrackedController>();
		if (steamVR_TrackedController == null)
		{
			steamVR_TrackedController = base.gameObject.AddComponent<SteamVR_TrackedController>();
		}
		steamVR_TrackedController.TriggerClicked += this.DoClick;
		if (this.teleportType == SteamVR_Teleporter.TeleportType.TeleportTypeUseTerrain)
		{
			Transform reference = this.reference;
			if (reference != null)
			{
				reference.position = new Vector3(reference.position.x, Terrain.activeTerrain.SampleHeight(reference.position), reference.position.z);
			}
		}
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x000E0FAC File Offset: 0x000DF3AC
	private void DoClick(object sender, ClickedEventArgs e)
	{
		if (this.teleportOnClick)
		{
			Transform reference = this.reference;
			if (reference == null)
			{
				return;
			}
			float y = reference.position.y;
			Plane plane = new Plane(Vector3.up, -y);
			Ray ray = new Ray(base.transform.position, base.transform.forward);
			bool flag = false;
			float num = 0f;
			if (this.teleportType == SteamVR_Teleporter.TeleportType.TeleportTypeUseTerrain)
			{
				TerrainCollider component = Terrain.activeTerrain.GetComponent<TerrainCollider>();
				RaycastHit raycastHit;
				flag = component.Raycast(ray, out raycastHit, 1000f);
				num = raycastHit.distance;
			}
			else if (this.teleportType == SteamVR_Teleporter.TeleportType.TeleportTypeUseCollider)
			{
				RaycastHit raycastHit2;
				Physics.Raycast(ray, out raycastHit2);
				num = raycastHit2.distance;
			}
			else
			{
				flag = plane.Raycast(ray, out num);
			}
			if (flag)
			{
				Vector3 vector = new Vector3(SteamVR_Render.Top().head.localPosition.x, 0f, SteamVR_Render.Top().head.localPosition.z);
				reference.position = ray.origin + ray.direction * num - new Vector3(reference.GetChild(0).localPosition.x, 0f, reference.GetChild(0).localPosition.z) - vector;
			}
		}
	}

	// Token: 0x040016E0 RID: 5856
	public bool teleportOnClick;

	// Token: 0x040016E1 RID: 5857
	public SteamVR_Teleporter.TeleportType teleportType = SteamVR_Teleporter.TeleportType.TeleportTypeUseZeroY;

	// Token: 0x02000289 RID: 649
	public enum TeleportType
	{
		// Token: 0x040016E3 RID: 5859
		TeleportTypeUseTerrain,
		// Token: 0x040016E4 RID: 5860
		TeleportTypeUseCollider,
		// Token: 0x040016E5 RID: 5861
		TeleportTypeUseZeroY
	}
}
