using System;
using System.Diagnostics;

namespace YoutubeLight
{
	// Token: 0x020002DF RID: 735
	public abstract class Downloader
	{
		// Token: 0x06001B30 RID: 6960 RVA: 0x000F575E File Offset: 0x000F3B5E
		protected Downloader(VideoInfo video, string savePath, int? bytesToDownload = null)
		{
			if (video == null)
			{
				throw new ArgumentNullException("video");
			}
			if (savePath == null)
			{
				throw new ArgumentNullException("savePath");
			}
			this.Video = video;
			this.SavePath = savePath;
			this.BytesToDownload = bytesToDownload;
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001B31 RID: 6961 RVA: 0x000F57A0 File Offset: 0x000F3BA0
		// (remove) Token: 0x06001B32 RID: 6962 RVA: 0x000F57D8 File Offset: 0x000F3BD8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler DownloadFinished;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001B33 RID: 6963 RVA: 0x000F5810 File Offset: 0x000F3C10
		// (remove) Token: 0x06001B34 RID: 6964 RVA: 0x000F5848 File Offset: 0x000F3C48
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler DownloadStarted;

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001B35 RID: 6965 RVA: 0x000F587E File Offset: 0x000F3C7E
		// (set) Token: 0x06001B36 RID: 6966 RVA: 0x000F5886 File Offset: 0x000F3C86
		public int? BytesToDownload { get; private set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x000F588F File Offset: 0x000F3C8F
		// (set) Token: 0x06001B38 RID: 6968 RVA: 0x000F5897 File Offset: 0x000F3C97
		public string SavePath { get; private set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x000F58A0 File Offset: 0x000F3CA0
		// (set) Token: 0x06001B3A RID: 6970 RVA: 0x000F58A8 File Offset: 0x000F3CA8
		public VideoInfo Video { get; private set; }

		// Token: 0x06001B3B RID: 6971
		public abstract void Execute();

		// Token: 0x06001B3C RID: 6972 RVA: 0x000F58B1 File Offset: 0x000F3CB1
		protected void OnDownloadFinished(EventArgs e)
		{
			if (this.DownloadFinished != null)
			{
				this.DownloadFinished(this, e);
			}
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x000F58CB File Offset: 0x000F3CCB
		protected void OnDownloadStarted(EventArgs e)
		{
			if (this.DownloadStarted != null)
			{
				this.DownloadStarted(this, e);
			}
		}
	}
}
