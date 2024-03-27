using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class EyeCollisionSphere : MonoBehaviour
{
	// Token: 0x060006FD RID: 1789 RVA: 0x00020528 File Offset: 0x0001E928
	private void Start()
	{
		GameObject gameObject = GameObject.FindWithTag("MainCamera");
		this.camera = gameObject.GetComponent<Camera>();
		this.environmentScript = this.environmentManager.GetComponent<EnvironmentManager>();
		this.rig = base.transform.parent.parent;
		base.Invoke("ReActivate", 1f);
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00020584 File Offset: 0x0001E984
	private bool UseOldApproach()
	{
		return Managers.areaManager && Managers.areaManager.rights.visionInObstacles == false;
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x000205C7 File Offset: 0x0001E9C7
	private void Update()
	{
		if (this.UseOldApproach() && this.isColliding && this.collidingObject == null)
		{
			this.DisableInObjectBehavior(null);
		}
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x000205F8 File Offset: 0x0001E9F8
	private void OnTriggerEnter(Collider other)
	{
		if (this.UseOldApproach())
		{
			if (other.gameObject.tag == "ThingPart" && !this.isColliding && Managers.areaManager && Managers.areaManager.didTeleportMoveInThisArea && Managers.areaManager.rights.movingThroughObstacles == false && !CrossDevice.desktopMode)
			{
				this.EnableInObjectBehavior(other);
			}
		}
		else if (this.active && this.IsAppropriateForBumpingOneAway(other))
		{
			Vector3? lastTeleportHitPoint = Our.lastTeleportHitPoint;
			if (lastTeleportHitPoint != null)
			{
				this.active = false;
				Vector3? lastTeleportHitPoint2 = Our.lastTeleportHitPoint;
				Hand.TeleportToPosition(lastTeleportHitPoint2.Value);
			}
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x000206D4 File Offset: 0x0001EAD4
	private void OnTriggerExit(Collider other)
	{
		if (this.UseOldApproach())
		{
			if (other.gameObject.tag == "ThingPart" && this.isColliding)
			{
				this.DisableInObjectBehavior(other);
			}
		}
		else
		{
			this.active = true;
		}
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00020724 File Offset: 0x0001EB24
	private bool IsAppropriateForBumpingOneAway(Collider other)
	{
		bool flag = false;
		if (Managers.personManager == null || Managers.areaManager == null || other.transform.parent == null)
		{
			return flag;
		}
		Thing component = other.transform.parent.GetComponent<Thing>();
		ThingPart component2 = other.GetComponent<ThingPart>();
		if (component == null || component2 == null)
		{
			return flag;
		}
		bool flag2 = !Managers.areaManager.weAreEditorOfCurrentArea || Our.mode == EditModes.None;
		bool flag3 = Managers.areaManager.rights.movingThroughObstacles == true;
		bool flag4 = other.gameObject.layer == LayerMask.NameToLayer("InvisibleToOurPerson");
		bool flag5 = Managers.personManager.GetPersonThisObjectIsOf(other.gameObject) != null;
		bool flag6 = component.biggestSize <= 1f;
		return flag2 && component.IsPlacement() && Managers.areaManager.didTeleportMoveInThisArea && (!flag3 && !flag5 && !CrossDevice.desktopMode && !flag4 && !flag5 && !component.isSittable && !component.isPassable && !component2.invisible && !component2.isLiquid && !flag6);
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x000208A4 File Offset: 0x0001ECA4
	private void EnableInObjectBehavior(Collider other)
	{
		if (Our.mode == EditModes.Area || Our.mode == EditModes.Thing || other.transform.parent == null)
		{
			return;
		}
		Thing component = other.transform.parent.GetComponent<Thing>();
		ThingPart component2 = other.GetComponent<ThingPart>();
		if (component == null || component2 == null)
		{
			return;
		}
		string tag = other.transform.parent.gameObject.tag;
		bool flag = tag == "CurrentlyHeldLeft" || tag == "CurrentlyHeldRight";
		bool flag2 = tag == "Attachment";
		if ((!flag || Our.mode != EditModes.Body) && !component.isSittable && !component.isPassable && !component2.invisible && !component2.isLiquid && !component2.isInInventoryOrDialog && !flag2 && other.gameObject.layer != LayerMask.NameToLayer("InvisibleToOurPerson"))
		{
			this.useInObjectMovementPreventor = !this.didJustTeleport && !flag && Our.mode != EditModes.Area;
			this.isColliding = true;
			this.collidingObject = other.gameObject;
			this.oldCullingMask = this.camera.cullingMask;
			this.camera.cullingMask = 0;
			this.camera.clearFlags = CameraClearFlags.Color;
			Renderer component3 = other.gameObject.GetComponent<Renderer>();
			float num = 0.15f;
			Color color = new Color(component3.material.color.r * num, component3.material.color.g * num, component3.material.color.b * num);
			this.camera.backgroundColor = color;
			this.environmentScript.AdjustVolumeByFactor(0.5f);
			this.positionBefore = base.transform.position;
		}
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00020AAC File Offset: 0x0001EEAC
	private void DisableInObjectBehavior(Collider other = null)
	{
		this.camera.cullingMask = this.oldCullingMask;
		this.camera.clearFlags = CameraClearFlags.Skybox;
		this.environmentScript.AdjustVolumeByFactor(2f);
		this.isColliding = false;
		this.collidingObject = null;
		if (this.useInObjectMovementPreventor && other != null)
		{
			Vector3 vector = base.transform.position - this.positionBefore;
			float y = this.rig.position.y;
			this.rig.position -= vector;
			Vector3 position = this.rig.position;
			position.y = y;
			this.rig.position = position;
		}
		this.positionBefore = Vector3.zero;
		this.useInObjectMovementPreventor = true;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00020B7F File Offset: 0x0001EF7F
	public void BrieflyDeactivateAsJustTeleported()
	{
		base.CancelInvoke();
		this.didJustTeleport = true;
		base.Invoke("ReActivate", 0.25f);
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00020B9E File Offset: 0x0001EF9E
	private void ReActivate()
	{
		this.didJustTeleport = false;
	}

	// Token: 0x04000512 RID: 1298
	private bool active = true;

	// Token: 0x04000513 RID: 1299
	public GameObject environmentManager;

	// Token: 0x04000514 RID: 1300
	private EnvironmentManager environmentScript;

	// Token: 0x04000515 RID: 1301
	public bool isColliding;

	// Token: 0x04000516 RID: 1302
	private int oldCullingMask;

	// Token: 0x04000517 RID: 1303
	private GameObject collidingObject;

	// Token: 0x04000518 RID: 1304
	private Camera camera;

	// Token: 0x04000519 RID: 1305
	private Vector3 positionBefore = Vector3.zero;

	// Token: 0x0400051A RID: 1306
	private Transform rig;

	// Token: 0x0400051B RID: 1307
	private bool didJustTeleport = true;

	// Token: 0x0400051C RID: 1308
	private bool useInObjectMovementPreventor = true;
}
