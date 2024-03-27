using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class VideoDialog : Dialog
{
	// Token: 0x06000B86 RID: 2950 RVA: 0x00060728 File Offset: 0x0005EB28
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		this.fundament = base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		Managers.videoManager.StopVideo(false);
		if (!string.IsNullOrEmpty(this.youTubeVideoId))
		{
			GameObject gameObject = base.AddVideoCube();
			Managers.videoManager.AddVideoScreenMesh(gameObject, this.youTubeVideoId, false, 0, false, false);
		}
		if (!string.IsNullOrEmpty(this.introText))
		{
			base.AddLabel(this.introText, 0, -450, 0.8f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		int num = 420;
		int num2 = 235;
		base.AddButton("copyToClipboard", string.Empty, "Copy link to clipboard", "ButtonCompactNoIcon", -num2, num, null, false, 1f, TextColor.Gray, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddButton("showOnVideoScreen", string.Empty, "Show on screen nearby", "ButtonCompactNoIcon", num2, num, null, false, 1f, TextColor.Gray, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		if (this.showVersion)
		{
			this.ShowVersion();
		}
		else
		{
			base.AddTimeDisplay();
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00060870 File Offset: 0x0005EC70
	private void ShowVersion()
	{
		base.AddLabel("Anyland\nversion " + Universe.GetClientVersionDisplay(), 0, -80, 1f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddLabel("Server version ".ToUpper() + Universe.GetServerVersionDisplay(), 0, 40, 0.6f, true, TextColor.Gray, true, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x000608D8 File Offset: 0x0005ECD8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "copyToClipboard"))
			{
				if (!(contextName == "showOnVideoScreen"))
				{
					if (!(contextName == "back"))
					{
						if (contextName == "close")
						{
							base.CloseDialog();
						}
					}
					else if (!string.IsNullOrEmpty(Managers.forumManager.currentForumThreadId))
					{
						base.SwitchTo(DialogType.ForumThread, string.Empty);
					}
					else
					{
						base.SwitchTo(DialogType.Main, string.Empty);
					}
				}
				else
				{
					bool flag = Managers.videoManager.WePlayVideoAtNearestThingPartScreen(this.youTubeVideoId, "shared video", null, null);
					if (flag)
					{
						base.SwitchTo(DialogType.VideoControl, string.Empty);
					}
					else
					{
						Managers.soundManager.Play("no", this.transform, 1f, false, false);
					}
				}
			}
			else
			{
				GUIUtility.systemCopyBuffer = "https://www.youtube.com/watch?v=" + this.youTubeVideoId;
				Managers.soundManager.Play("pickUp", this.transform, 1f, false, false);
			}
		}
	}

	// Token: 0x040008B9 RID: 2233
	private GameObject playButton;

	// Token: 0x040008BA RID: 2234
	private GameObject screenCube;

	// Token: 0x040008BB RID: 2235
	private bool isPlayingVideo;

	// Token: 0x040008BC RID: 2236
	private GameObject fundament;

	// Token: 0x040008BD RID: 2237
	public string youTubeVideoId;

	// Token: 0x040008BE RID: 2238
	public string introText;

	// Token: 0x040008BF RID: 2239
	public bool showVersion;
}
