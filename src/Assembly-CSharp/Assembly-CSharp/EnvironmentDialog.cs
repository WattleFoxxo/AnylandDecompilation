using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class EnvironmentDialog : Dialog
{
	// Token: 0x0600093D RID: 2365 RVA: 0x0003928C File Offset: 0x0003768C
	public void Start()
	{
		this.buttonCount = Enum.GetNames(typeof(EnvironmentType)).Length;
		this.maxPages = Mathf.CeilToInt((float)this.buttonCount / 35f);
		GameObject @object = Managers.treeManager.GetObject("/Universe/EnvironmentManager");
		this.environmentManager = @object.GetComponent<EnvironmentManager>();
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddHeadline("Environment", -370, -460, TextColor.Default, TextAlignment.Left, false);
		base.StartCoroutine(this.AddEnvironmentButtons());
		base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00039340 File Offset: 0x00037740
	private IEnumerator AddEnvironmentButtons()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int x = -385;
		int y = -280;
		int i = 0;
		int minI = this.page * 35;
		int maxI = Mathf.Min(minI + 35 - 1, this.buttonCount - 1);
		int buttonsShownCount = 0;
		IEnumerator enumerator = Enum.GetValues(typeof(EnvironmentType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				EnvironmentType environmentType = (EnvironmentType)obj;
				if (i >= minI && i <= maxI)
				{
					string text = environmentType.ToString();
					bool flag = environmentType == this.environmentManager.type;
					base.AddButton("environment", text, null, "ButtonSmall", x, y, Misc.GetNumberlessString(text), flag, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
					x += 130;
					buttonsShownCount++;
					if (buttonsShownCount % 7 == 0)
					{
						x = -385;
						y += 130;
					}
				}
				i++;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0003935C File Offset: 0x0003775C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "environment"))
			{
				if (!(contextName == "previousPage"))
				{
					if (!(contextName == "nextPage"))
					{
						if (contextName == "back")
						{
							base.SwitchTo(DialogType.Area, string.Empty);
						}
					}
					else
					{
						if (++this.page > this.maxPages - 1)
						{
							this.page = 0;
						}
						base.StartCoroutine(this.AddEnvironmentButtons());
					}
				}
				else
				{
					if (--this.page < 0)
					{
						this.page = this.maxPages - 1;
					}
					base.StartCoroutine(this.AddEnvironmentButtons());
				}
			}
			else
			{
				EnvironmentType environmentType = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), contextId);
				this.environmentManager.SetToType(environmentType);
				Managers.personManager.DoSetEnvironmentType(environmentType);
				base.StartCoroutine(this.AddEnvironmentButtons());
			}
		}
	}

	// Token: 0x040006FC RID: 1788
	private EnvironmentManager environmentManager;

	// Token: 0x040006FD RID: 1789
	private const int buttonsPerRow = 7;

	// Token: 0x040006FE RID: 1790
	private const int buttonsPerPage = 35;

	// Token: 0x040006FF RID: 1791
	private int page;

	// Token: 0x04000700 RID: 1792
	private int maxPages = -1;

	// Token: 0x04000701 RID: 1793
	private int buttonCount = -1;
}
