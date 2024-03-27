using System;
using System.Diagnostics;
using UnityEngine;
using Valve.VR;

// Token: 0x0200028E RID: 654
public class SteamVR_TrackedController : MonoBehaviour
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06001886 RID: 6278 RVA: 0x000E1484 File Offset: 0x000DF884
	// (remove) Token: 0x06001887 RID: 6279 RVA: 0x000E14BC File Offset: 0x000DF8BC
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler MenuButtonClicked;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06001888 RID: 6280 RVA: 0x000E14F4 File Offset: 0x000DF8F4
	// (remove) Token: 0x06001889 RID: 6281 RVA: 0x000E152C File Offset: 0x000DF92C
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler MenuButtonUnclicked;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x0600188A RID: 6282 RVA: 0x000E1564 File Offset: 0x000DF964
	// (remove) Token: 0x0600188B RID: 6283 RVA: 0x000E159C File Offset: 0x000DF99C
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler TriggerClicked;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x0600188C RID: 6284 RVA: 0x000E15D4 File Offset: 0x000DF9D4
	// (remove) Token: 0x0600188D RID: 6285 RVA: 0x000E160C File Offset: 0x000DFA0C
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler TriggerUnclicked;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x0600188E RID: 6286 RVA: 0x000E1644 File Offset: 0x000DFA44
	// (remove) Token: 0x0600188F RID: 6287 RVA: 0x000E167C File Offset: 0x000DFA7C
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler SteamClicked;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06001890 RID: 6288 RVA: 0x000E16B4 File Offset: 0x000DFAB4
	// (remove) Token: 0x06001891 RID: 6289 RVA: 0x000E16EC File Offset: 0x000DFAEC
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler PadClicked;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06001892 RID: 6290 RVA: 0x000E1724 File Offset: 0x000DFB24
	// (remove) Token: 0x06001893 RID: 6291 RVA: 0x000E175C File Offset: 0x000DFB5C
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler PadUnclicked;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06001894 RID: 6292 RVA: 0x000E1794 File Offset: 0x000DFB94
	// (remove) Token: 0x06001895 RID: 6293 RVA: 0x000E17CC File Offset: 0x000DFBCC
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler PadTouched;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06001896 RID: 6294 RVA: 0x000E1804 File Offset: 0x000DFC04
	// (remove) Token: 0x06001897 RID: 6295 RVA: 0x000E183C File Offset: 0x000DFC3C
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler PadUntouched;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06001898 RID: 6296 RVA: 0x000E1874 File Offset: 0x000DFC74
	// (remove) Token: 0x06001899 RID: 6297 RVA: 0x000E18AC File Offset: 0x000DFCAC
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler Gripped;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x0600189A RID: 6298 RVA: 0x000E18E4 File Offset: 0x000DFCE4
	// (remove) Token: 0x0600189B RID: 6299 RVA: 0x000E191C File Offset: 0x000DFD1C
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClickedEventHandler Ungripped;

	// Token: 0x0600189C RID: 6300 RVA: 0x000E1954 File Offset: 0x000DFD54
	private void Start()
	{
		if (base.GetComponent<SteamVR_TrackedObject>() == null)
		{
			base.gameObject.AddComponent<SteamVR_TrackedObject>();
		}
		if (this.controllerIndex != 0U)
		{
			base.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)this.controllerIndex;
			if (base.GetComponent<SteamVR_RenderModel>() != null)
			{
				base.GetComponent<SteamVR_RenderModel>().index = (SteamVR_TrackedObject.EIndex)this.controllerIndex;
			}
		}
		else
		{
			this.controllerIndex = (uint)base.GetComponent<SteamVR_TrackedObject>().index;
		}
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x000E19D2 File Offset: 0x000DFDD2
	public void SetDeviceIndex(int index)
	{
		this.controllerIndex = (uint)index;
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x000E19DB File Offset: 0x000DFDDB
	public virtual void OnTriggerClicked(ClickedEventArgs e)
	{
		if (this.TriggerClicked != null)
		{
			this.TriggerClicked(this, e);
		}
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x000E19F5 File Offset: 0x000DFDF5
	public virtual void OnTriggerUnclicked(ClickedEventArgs e)
	{
		if (this.TriggerUnclicked != null)
		{
			this.TriggerUnclicked(this, e);
		}
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x000E1A0F File Offset: 0x000DFE0F
	public virtual void OnMenuClicked(ClickedEventArgs e)
	{
		if (this.MenuButtonClicked != null)
		{
			this.MenuButtonClicked(this, e);
		}
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x000E1A29 File Offset: 0x000DFE29
	public virtual void OnMenuUnclicked(ClickedEventArgs e)
	{
		if (this.MenuButtonUnclicked != null)
		{
			this.MenuButtonUnclicked(this, e);
		}
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000E1A43 File Offset: 0x000DFE43
	public virtual void OnSteamClicked(ClickedEventArgs e)
	{
		if (this.SteamClicked != null)
		{
			this.SteamClicked(this, e);
		}
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000E1A5D File Offset: 0x000DFE5D
	public virtual void OnPadClicked(ClickedEventArgs e)
	{
		if (this.PadClicked != null)
		{
			this.PadClicked(this, e);
		}
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x000E1A77 File Offset: 0x000DFE77
	public virtual void OnPadUnclicked(ClickedEventArgs e)
	{
		if (this.PadUnclicked != null)
		{
			this.PadUnclicked(this, e);
		}
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x000E1A91 File Offset: 0x000DFE91
	public virtual void OnPadTouched(ClickedEventArgs e)
	{
		if (this.PadTouched != null)
		{
			this.PadTouched(this, e);
		}
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x000E1AAB File Offset: 0x000DFEAB
	public virtual void OnPadUntouched(ClickedEventArgs e)
	{
		if (this.PadUntouched != null)
		{
			this.PadUntouched(this, e);
		}
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x000E1AC5 File Offset: 0x000DFEC5
	public virtual void OnGripped(ClickedEventArgs e)
	{
		if (this.Gripped != null)
		{
			this.Gripped(this, e);
		}
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000E1ADF File Offset: 0x000DFEDF
	public virtual void OnUngripped(ClickedEventArgs e)
	{
		if (this.Ungripped != null)
		{
			this.Ungripped(this, e);
		}
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x000E1AFC File Offset: 0x000DFEFC
	private void Update()
	{
		CVRSystem system = OpenVR.System;
		if (system != null && system.GetControllerState(this.controllerIndex, ref this.controllerState))
		{
			ulong num = this.controllerState.ulButtonPressed & 8589934592UL;
			if (num > 0UL && !this.triggerPressed)
			{
				this.triggerPressed = true;
				ClickedEventArgs clickedEventArgs;
				clickedEventArgs.controllerIndex = this.controllerIndex;
				clickedEventArgs.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs.padX = this.controllerState.rAxis0.x;
				clickedEventArgs.padY = this.controllerState.rAxis0.y;
				this.OnTriggerClicked(clickedEventArgs);
			}
			else if (num == 0UL && this.triggerPressed)
			{
				this.triggerPressed = false;
				ClickedEventArgs clickedEventArgs2;
				clickedEventArgs2.controllerIndex = this.controllerIndex;
				clickedEventArgs2.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs2.padX = this.controllerState.rAxis0.x;
				clickedEventArgs2.padY = this.controllerState.rAxis0.y;
				this.OnTriggerUnclicked(clickedEventArgs2);
			}
			ulong num2 = this.controllerState.ulButtonPressed & 4UL;
			if (num2 > 0UL && !this.gripped)
			{
				this.gripped = true;
				ClickedEventArgs clickedEventArgs3;
				clickedEventArgs3.controllerIndex = this.controllerIndex;
				clickedEventArgs3.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs3.padX = this.controllerState.rAxis0.x;
				clickedEventArgs3.padY = this.controllerState.rAxis0.y;
				this.OnGripped(clickedEventArgs3);
			}
			else if (num2 == 0UL && this.gripped)
			{
				this.gripped = false;
				ClickedEventArgs clickedEventArgs4;
				clickedEventArgs4.controllerIndex = this.controllerIndex;
				clickedEventArgs4.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs4.padX = this.controllerState.rAxis0.x;
				clickedEventArgs4.padY = this.controllerState.rAxis0.y;
				this.OnUngripped(clickedEventArgs4);
			}
			ulong num3 = this.controllerState.ulButtonPressed & 4294967296UL;
			if (num3 > 0UL && !this.padPressed)
			{
				this.padPressed = true;
				ClickedEventArgs clickedEventArgs5;
				clickedEventArgs5.controllerIndex = this.controllerIndex;
				clickedEventArgs5.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs5.padX = this.controllerState.rAxis0.x;
				clickedEventArgs5.padY = this.controllerState.rAxis0.y;
				this.OnPadClicked(clickedEventArgs5);
			}
			else if (num3 == 0UL && this.padPressed)
			{
				this.padPressed = false;
				ClickedEventArgs clickedEventArgs6;
				clickedEventArgs6.controllerIndex = this.controllerIndex;
				clickedEventArgs6.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs6.padX = this.controllerState.rAxis0.x;
				clickedEventArgs6.padY = this.controllerState.rAxis0.y;
				this.OnPadUnclicked(clickedEventArgs6);
			}
			ulong num4 = this.controllerState.ulButtonPressed & 2UL;
			if (num4 > 0UL && !this.menuPressed)
			{
				this.menuPressed = true;
				ClickedEventArgs clickedEventArgs7;
				clickedEventArgs7.controllerIndex = this.controllerIndex;
				clickedEventArgs7.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs7.padX = this.controllerState.rAxis0.x;
				clickedEventArgs7.padY = this.controllerState.rAxis0.y;
				this.OnMenuClicked(clickedEventArgs7);
			}
			else if (num4 == 0UL && this.menuPressed)
			{
				this.menuPressed = false;
				ClickedEventArgs clickedEventArgs8;
				clickedEventArgs8.controllerIndex = this.controllerIndex;
				clickedEventArgs8.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs8.padX = this.controllerState.rAxis0.x;
				clickedEventArgs8.padY = this.controllerState.rAxis0.y;
				this.OnMenuUnclicked(clickedEventArgs8);
			}
			num3 = this.controllerState.ulButtonTouched & 4294967296UL;
			if (num3 > 0UL && !this.padTouched)
			{
				this.padTouched = true;
				ClickedEventArgs clickedEventArgs9;
				clickedEventArgs9.controllerIndex = this.controllerIndex;
				clickedEventArgs9.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs9.padX = this.controllerState.rAxis0.x;
				clickedEventArgs9.padY = this.controllerState.rAxis0.y;
				this.OnPadTouched(clickedEventArgs9);
			}
			else if (num3 == 0UL && this.padTouched)
			{
				this.padTouched = false;
				ClickedEventArgs clickedEventArgs10;
				clickedEventArgs10.controllerIndex = this.controllerIndex;
				clickedEventArgs10.flags = (uint)this.controllerState.ulButtonPressed;
				clickedEventArgs10.padX = this.controllerState.rAxis0.x;
				clickedEventArgs10.padY = this.controllerState.rAxis0.y;
				this.OnPadUntouched(clickedEventArgs10);
			}
		}
	}

	// Token: 0x040016F2 RID: 5874
	public uint controllerIndex;

	// Token: 0x040016F3 RID: 5875
	public VRControllerState_t controllerState;

	// Token: 0x040016F4 RID: 5876
	public bool triggerPressed;

	// Token: 0x040016F5 RID: 5877
	public bool steamPressed;

	// Token: 0x040016F6 RID: 5878
	public bool menuPressed;

	// Token: 0x040016F7 RID: 5879
	public bool padPressed;

	// Token: 0x040016F8 RID: 5880
	public bool padTouched;

	// Token: 0x040016F9 RID: 5881
	public bool gripped;
}
