using System;
using UnityEngine;

// Token: 0x020002AD RID: 685
public abstract class SteamVR_Status : MonoBehaviour
{
	// Token: 0x060019B0 RID: 6576
	protected abstract void SetAlpha(float a);

	// Token: 0x060019B1 RID: 6577 RVA: 0x000EA6D6 File Offset: 0x000E8AD6
	private void OnEnable()
	{
		SteamVR_Utils.Event.Listen(this.message, new SteamVR_Utils.Event.Handler(this.OnEvent));
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x000EA6EF File Offset: 0x000E8AEF
	private void OnDisable()
	{
		SteamVR_Utils.Event.Remove(this.message, new SteamVR_Utils.Event.Handler(this.OnEvent));
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x000EA708 File Offset: 0x000E8B08
	private void OnEvent(params object[] args)
	{
		this.status = (bool)args[0];
		if (this.status)
		{
			if (this.mode == SteamVR_Status.Mode.OnTrue)
			{
				this.timer = this.duration;
			}
		}
		else if (this.mode == SteamVR_Status.Mode.OnFalse)
		{
			this.timer = this.duration;
		}
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x000EA764 File Offset: 0x000E8B64
	private void Update()
	{
		if (this.mode == SteamVR_Status.Mode.OnTrue || this.mode == SteamVR_Status.Mode.OnFalse)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.SetAlpha(0f);
			}
			else
			{
				float num = 1f;
				if (this.timer < this.fade)
				{
					num = this.timer / this.fade;
				}
				if (this.timer > this.duration - this.fade)
				{
					num = Mathf.InverseLerp(this.duration, this.duration - this.fade, this.timer);
				}
				this.SetAlpha(num);
			}
		}
		else
		{
			bool flag = (this.mode == SteamVR_Status.Mode.WhileTrue && this.status) || (this.mode == SteamVR_Status.Mode.WhileFalse && !this.status);
			this.timer = ((!flag) ? Mathf.Max(0f, this.timer - Time.deltaTime) : Mathf.Min(this.fade, this.timer + Time.deltaTime));
			this.SetAlpha(this.timer / this.fade);
		}
	}

	// Token: 0x040017F7 RID: 6135
	public string message;

	// Token: 0x040017F8 RID: 6136
	public float duration;

	// Token: 0x040017F9 RID: 6137
	public float fade;

	// Token: 0x040017FA RID: 6138
	protected float timer;

	// Token: 0x040017FB RID: 6139
	protected bool status;

	// Token: 0x040017FC RID: 6140
	public SteamVR_Status.Mode mode;

	// Token: 0x020002AE RID: 686
	public enum Mode
	{
		// Token: 0x040017FE RID: 6142
		OnTrue,
		// Token: 0x040017FF RID: 6143
		OnFalse,
		// Token: 0x04001800 RID: 6144
		WhileTrue,
		// Token: 0x04001801 RID: 6145
		WhileFalse
	}
}
