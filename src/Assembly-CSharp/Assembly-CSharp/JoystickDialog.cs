using System;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000118 RID: 280
public class JoystickDialog : Dialog
{
	// Token: 0x06000A43 RID: 2627 RVA: 0x00046C28 File Offset: 0x00045028
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		GameObject gameObject = base.AddFundament();
		base.AddCloseButton();
		base.AddBackground("Joystick", false, false);
		if (!this.ControlledObjectExists())
		{
			this.Close();
			return;
		}
		this.AddJoystick(gameObject);
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00046C78 File Offset: 0x00045078
	private void AddJoystick(GameObject fundament)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("DialogParts/Prefabs/Joystick", typeof(GameObject))) as GameObject;
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		gameObject.transform.parent = fundament.transform;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = Vector3.zero;
		this.joystick = (Joystick)gameObject.GetComponentInChildren(typeof(Joystick), true);
		if (this.controllable != null)
		{
			this.joystick.controllable = this.controllable;
		}
		else if (this.browser != null)
		{
			this.joystick.browser = this.browser;
			this.joystick.useArrowKeysForBrowser = this.useArrowKeysForBrowser;
		}
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00046D5C File Offset: 0x0004515C
	private bool ControlledObjectExists()
	{
		return (this.controllable != null && this.controllable.rigidbody != null) || this.browser != null;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x00046D94 File Offset: 0x00045194
	private new void Update()
	{
		if (!this.ControlledObjectExists())
		{
			this.Close();
			return;
		}
		base.Update();
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x00046DAE File Offset: 0x000451AE
	private void Close()
	{
		if (this.controllable != null)
		{
			Managers.personManager.ourPerson.StopControlControllable(this.controllable);
		}
		base.CloseDialog();
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x00046DDC File Offset: 0x000451DC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (contextName == "close")
			{
				if (!this.joystick.stickIsHeld)
				{
					this.Close();
				}
			}
		}
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00046E21 File Offset: 0x00045221
	public bool JoystickHandIsInScope()
	{
		return this.joystick != null && this.joystick.handIsInScope;
	}

	// Token: 0x040007AD RID: 1965
	public Thing controllable;

	// Token: 0x040007AE RID: 1966
	public Browser browser;

	// Token: 0x040007AF RID: 1967
	public bool useArrowKeysForBrowser;

	// Token: 0x040007B0 RID: 1968
	private Joystick joystick;
}
