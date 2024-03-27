using System;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class SpecialEditModePreview : MonoBehaviour
{
	// Token: 0x06000E9C RID: 3740 RVA: 0x00080C9D File Offset: 0x0007F09D
	private void OnEnable()
	{
		if (this.isFollowerCamera)
		{
			this.headCorePreview.enabled = false;
		}
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00080CB6 File Offset: 0x0007F0B6
	private void OnDisable()
	{
		if (this.isFollowerCamera)
		{
			this.headCorePreview.enabled = true;
		}
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x00080CD0 File Offset: 0x0007F0D0
	private void LateUpdate()
	{
		if (Our.mode == EditModes.Area || Our.mode == EditModes.Thing)
		{
			if (this.handDotLeft.currentlyHeldObject != null)
			{
				GameObject gameObject = null;
				if (Our.mode == EditModes.Area)
				{
					gameObject = this.handDotLeft.currentlyHeldObject;
				}
				else if (Our.mode == EditModes.Thing)
				{
					string name = this.handDotLeft.currentlyHeldObject.name;
					if (name != "Brush" && name != "VertexMover" && name != DialogType.Keyboard.ToString())
					{
						gameObject = CreationHelper.thingBeingEdited;
					}
				}
				if (gameObject != null)
				{
					Thing component = gameObject.GetComponent<Thing>();
					if (component != null)
					{
						if (component.doLockPosition)
						{
							this.MemorizeStateLeft();
							this.resetPositionLeft = true;
							this.handDotLeft.currentlyHeldObject.transform.position = this.handDotLeft.objectPickUpPosition;
						}
						else if (SnapHelper.SomeAngleSnapIsOn(component))
						{
							this.MemorizeStateLeft();
							this.resetAnglesLeft = true;
							this.handDotLeft.currentlyHeldObject.transform.parent = null;
							this.handDotLeft.currentlyHeldObject.transform.localEulerAngles = SnapHelper.SnapAngles(component, this.handDotLeft.currentlyHeldObject, this.handDotLeft.transform, this.handDotLeft.myPickUpPosition, this.handDotLeft.objectPickUpPosition, this.handDotLeft.myPickUpAngles, this.handDotLeft.objectPickUpAngles);
							if (component.doSnapPosition || Our.doSnapThingPosition)
							{
								this.resetPositionLeft = true;
								this.handDotLeft.currentlyHeldObject.transform.position = SnapHelper.SnapPositionAlongAxis(this.handDotLeft.currentlyHeldObject, this.handDotLeft.objectPickUpPosition);
							}
						}
						else if (component.smallEditMovements && Our.mode == EditModes.Thing)
						{
							this.MemorizeStateLeft();
							this.handDotLeft.DoSmallEditMovementOnly(this.handDotLeft.currentlyHeldObject);
						}
					}
				}
			}
			if (this.handDotRight.currentlyHeldObject != null)
			{
				GameObject gameObject2 = null;
				if (Our.mode == EditModes.Area)
				{
					gameObject2 = this.handDotRight.currentlyHeldObject;
				}
				else if (Our.mode == EditModes.Thing)
				{
					string name2 = this.handDotRight.currentlyHeldObject.name;
					if (name2 != "Brush" && name2 != "VertexMover" && name2 != DialogType.Keyboard.ToString())
					{
						gameObject2 = CreationHelper.thingBeingEdited;
					}
				}
				if (gameObject2 != null)
				{
					Thing component2 = gameObject2.GetComponent<Thing>();
					if (component2 != null)
					{
						if (component2.doLockPosition)
						{
							this.MemorizeStateRight();
							this.resetPositionRight = true;
							this.handDotRight.currentlyHeldObject.transform.position = this.handDotRight.objectPickUpPosition;
						}
						else if (SnapHelper.SomeAngleSnapIsOn(component2))
						{
							this.MemorizeStateRight();
							this.resetAnglesRight = true;
							this.handDotRight.currentlyHeldObject.transform.parent = null;
							this.handDotRight.currentlyHeldObject.transform.localEulerAngles = SnapHelper.SnapAngles(component2, this.handDotRight.currentlyHeldObject, this.handDotRight.transform, this.handDotRight.myPickUpPosition, this.handDotRight.objectPickUpPosition, this.handDotRight.myPickUpAngles, this.handDotRight.objectPickUpAngles);
							if (component2.doSnapPosition || Our.doSnapThingPosition)
							{
								this.resetPositionRight = true;
								this.handDotRight.currentlyHeldObject.transform.position = SnapHelper.SnapPositionAlongAxis(this.handDotRight.currentlyHeldObject, this.handDotRight.objectPickUpPosition);
							}
						}
						else if (component2.smallEditMovements && Our.mode == EditModes.Thing)
						{
							this.MemorizeStateRight();
							this.handDotRight.DoSmallEditMovementOnly(this.handDotRight.currentlyHeldObject);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x000810FC File Offset: 0x0007F4FC
	private void MemorizeStateLeft()
	{
		this.oldAnglesLeft = this.handDotLeft.currentlyHeldObject.transform.localEulerAngles;
		this.oldParentLeft = this.handDotLeft.currentlyHeldObject.transform.parent;
		this.oldPositionLeft = this.handDotLeft.currentlyHeldObject.transform.position;
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0008115C File Offset: 0x0007F55C
	private void MemorizeStateRight()
	{
		this.oldAnglesRight = this.handDotRight.currentlyHeldObject.transform.localEulerAngles;
		this.oldParentRight = this.handDotRight.currentlyHeldObject.transform.parent;
		this.oldPositionRight = this.handDotRight.currentlyHeldObject.transform.position;
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x000811BC File Offset: 0x0007F5BC
	private void OnPostRender()
	{
		if (this.resetAnglesLeft)
		{
			this.resetAnglesLeft = false;
			this.handDotLeft.currentlyHeldObject.transform.parent = this.oldParentLeft;
			this.handDotLeft.currentlyHeldObject.transform.localEulerAngles = this.oldAnglesLeft;
			if (this.resetPositionLeft)
			{
				this.resetPositionLeft = false;
				this.handDotLeft.currentlyHeldObject.transform.position = this.oldPositionLeft;
			}
		}
		if (this.resetAnglesRight)
		{
			this.resetAnglesRight = false;
			this.handDotRight.currentlyHeldObject.transform.parent = this.oldParentRight;
			this.handDotRight.currentlyHeldObject.transform.localEulerAngles = this.oldAnglesRight;
			if (this.resetPositionRight)
			{
				this.resetPositionRight = false;
				this.handDotRight.currentlyHeldObject.transform.position = this.oldPositionRight;
			}
		}
	}

	// Token: 0x04000F70 RID: 3952
	public HandDot handDotLeft;

	// Token: 0x04000F71 RID: 3953
	public HandDot handDotRight;

	// Token: 0x04000F72 RID: 3954
	public bool isFollowerCamera;

	// Token: 0x04000F73 RID: 3955
	public SpecialEditModePreview headCorePreview;

	// Token: 0x04000F74 RID: 3956
	private Vector3 oldAnglesLeft;

	// Token: 0x04000F75 RID: 3957
	private Vector3 oldAnglesRight;

	// Token: 0x04000F76 RID: 3958
	private Vector3 oldPositionLeft;

	// Token: 0x04000F77 RID: 3959
	private Vector3 oldPositionRight;

	// Token: 0x04000F78 RID: 3960
	private bool resetAnglesLeft;

	// Token: 0x04000F79 RID: 3961
	private bool resetAnglesRight;

	// Token: 0x04000F7A RID: 3962
	private bool resetPositionLeft;

	// Token: 0x04000F7B RID: 3963
	private bool resetPositionRight;

	// Token: 0x04000F7C RID: 3964
	private Transform oldParentLeft;

	// Token: 0x04000F7D RID: 3965
	private Transform oldParentRight;
}
