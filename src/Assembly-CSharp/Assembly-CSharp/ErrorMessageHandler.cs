using System;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class ErrorMessageHandler : MonoBehaviour
{
	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00075A2E File Offset: 0x00073E2E
	// (set) Token: 0x06000D0D RID: 3341 RVA: 0x00075A36 File Offset: 0x00073E36
	public string message { get; private set; }

	// Token: 0x06000D0E RID: 3342 RVA: 0x00075A40 File Offset: 0x00073E40
	private void Awake()
	{
		this.message = ErrorSceneData.Message;
		if (this.message == null || this.message == string.Empty)
		{
			this.message = "Something happened that wasn't meant to happen.";
		}
		bool flag = ErrorSceneData.AutoReturnToUniverse && !ErrorMessageHandler.weAutoReturnedToUniverseBefore;
		string text = string.Empty;
		string text2 = string.Empty;
		if (ErrorSceneData.ShowBoilerplate)
		{
			text = "Oops, an error occurred:\n\n";
			text2 = "\n\n";
			text2 += ((!flag) ? "Please try restart Anyland." : "A reload starts in few seconds.");
			text2 += " If this issue persists, please go to the Anyland Steam forum for help. You can also contact us at we@anyland.com. Sorry & thanks!";
			text2 = Misc.WrapWithNewlines(text2, 50, -1);
			string text3 = text2;
			text2 = string.Concat(new string[]
			{
				text3,
				"\n\n[Anyland version ",
				Universe.GetClientVersionDisplay(),
				" | ",
				Universe.GetServerVersionDisplay(),
				"]"
			});
		}
		this.message = text + Misc.WrapWithNewlines(this.message, 50, -1) + text2;
		if (flag)
		{
			ErrorMessageHandler.weAutoReturnedToUniverseBefore = true;
			float num = ((!ErrorSceneData.SpeedUpReturnToUniverse) ? 8f : 2f);
			base.Invoke("SwitchBackToUniverseScene", num);
		}
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x00075B76 File Offset: 0x00073F76
	private void SwitchBackToUniverseScene()
	{
		Log.Info("Switching back to universe", false);
		Application.LoadLevel("Universe");
	}

	// Token: 0x04000EBD RID: 3773
	private const int lineLength = 50;

	// Token: 0x04000EBE RID: 3774
	public static bool weAutoReturnedToUniverseBefore;
}
