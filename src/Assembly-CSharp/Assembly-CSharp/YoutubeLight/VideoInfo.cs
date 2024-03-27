using System;
using System.Collections.Generic;

namespace YoutubeLight
{
	// Token: 0x020002DE RID: 734
	public class VideoInfo
	{
		// Token: 0x06001B0E RID: 6926 RVA: 0x000F4D34 File Offset: 0x000F3134
		internal VideoInfo(int formatCode)
			: this(formatCode, VideoType.Unknown, 0, false, false, AudioType.Unknown, 0, AdaptiveType.None)
		{
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x000F4D50 File Offset: 0x000F3150
		internal VideoInfo(VideoInfo info)
			: this(info.FormatCode, info.VideoType, info.Resolution, info.HDR, info.Is3D, info.AudioType, info.AudioBitrate, info.AdaptiveType)
		{
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000F4D94 File Offset: 0x000F3194
		private VideoInfo(int formatCode, VideoType videoType, int resolution, bool HDR, bool is3D, AudioType audioType, int audioBitrate, AdaptiveType adaptiveType)
		{
			this.FormatCode = formatCode;
			this.VideoType = videoType;
			this.Resolution = resolution;
			this.Is3D = is3D;
			this.AudioType = audioType;
			this.AudioBitrate = audioBitrate;
			this.AdaptiveType = adaptiveType;
			this.HDR = HDR;
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x000F4DE4 File Offset: 0x000F31E4
		// (set) Token: 0x06001B12 RID: 6930 RVA: 0x000F4DEC File Offset: 0x000F31EC
		public AdaptiveType AdaptiveType { get; private set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x000F4DF5 File Offset: 0x000F31F5
		// (set) Token: 0x06001B14 RID: 6932 RVA: 0x000F4DFD File Offset: 0x000F31FD
		public int AudioBitrate { get; private set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x000F4E08 File Offset: 0x000F3208
		public string AudioExtension
		{
			get
			{
				switch (this.AudioType)
				{
				case AudioType.Aac:
					return ".aac";
				case AudioType.Mp3:
					return ".mp3";
				case AudioType.Opus:
					return ".webm";
				case AudioType.Vorbis:
					return ".ogg";
				default:
					return null;
				}
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x000F4E50 File Offset: 0x000F3250
		// (set) Token: 0x06001B17 RID: 6935 RVA: 0x000F4E58 File Offset: 0x000F3258
		public AudioType AudioType { get; private set; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x000F4E61 File Offset: 0x000F3261
		public bool CanExtractAudio
		{
			get
			{
				return this.VideoType == VideoType.Flv;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x000F4E6C File Offset: 0x000F326C
		// (set) Token: 0x06001B1A RID: 6938 RVA: 0x000F4E74 File Offset: 0x000F3274
		public string DownloadUrl { get; internal set; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x000F4E7D File Offset: 0x000F327D
		// (set) Token: 0x06001B1C RID: 6940 RVA: 0x000F4E85 File Offset: 0x000F3285
		public int FormatCode { get; private set; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x000F4E8E File Offset: 0x000F328E
		// (set) Token: 0x06001B1E RID: 6942 RVA: 0x000F4E96 File Offset: 0x000F3296
		public bool Is3D { get; private set; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x000F4E9F File Offset: 0x000F329F
		// (set) Token: 0x06001B20 RID: 6944 RVA: 0x000F4EA7 File Offset: 0x000F32A7
		public bool HDR { get; private set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x000F4EB0 File Offset: 0x000F32B0
		// (set) Token: 0x06001B22 RID: 6946 RVA: 0x000F4EB8 File Offset: 0x000F32B8
		public bool RequiresDecryption { get; internal set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x000F4EC1 File Offset: 0x000F32C1
		// (set) Token: 0x06001B24 RID: 6948 RVA: 0x000F4EC9 File Offset: 0x000F32C9
		public int Resolution { get; private set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x000F4ED2 File Offset: 0x000F32D2
		// (set) Token: 0x06001B26 RID: 6950 RVA: 0x000F4EDA File Offset: 0x000F32DA
		public string Title { get; internal set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x000F4EE4 File Offset: 0x000F32E4
		public string VideoExtension
		{
			get
			{
				switch (this.VideoType)
				{
				case VideoType.Mobile_3gp:
					return ".3gp";
				case VideoType.Flv:
					return ".flv";
				case VideoType.Mp4:
					return ".mp4";
				case VideoType.WebM:
					return ".webm";
				}
				return null;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x000F4F30 File Offset: 0x000F3330
		// (set) Token: 0x06001B29 RID: 6953 RVA: 0x000F4F38 File Offset: 0x000F3338
		public VideoType VideoType { get; private set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x000F4F41 File Offset: 0x000F3341
		// (set) Token: 0x06001B2B RID: 6955 RVA: 0x000F4F49 File Offset: 0x000F3349
		internal string HtmlPlayerVersion { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x000F4F52 File Offset: 0x000F3352
		// (set) Token: 0x06001B2D RID: 6957 RVA: 0x000F4F5A File Offset: 0x000F335A
		internal string HtmlscriptName { get; set; }

		// Token: 0x06001B2E RID: 6958 RVA: 0x000F4F63 File Offset: 0x000F3363
		public override string ToString()
		{
			return string.Format("Full Title: {0}, Type: {1}, Resolution: {2}p", this.Title + this.VideoExtension, this.VideoType, this.Resolution);
		}

		// Token: 0x0400191A RID: 6426
		internal static IEnumerable<VideoInfo> Defaults = new List<VideoInfo>
		{
			new VideoInfo(5, VideoType.Flv, 240, false, false, AudioType.Mp3, 64, AdaptiveType.Audio_Video),
			new VideoInfo(6, VideoType.Flv, 270, false, false, AudioType.Mp3, 64, AdaptiveType.Audio_Video),
			new VideoInfo(17, VideoType.Mobile_3gp, 144, false, false, AudioType.Aac, 24, AdaptiveType.Audio_Video),
			new VideoInfo(18, VideoType.Mp4, 360, false, false, AudioType.Aac, 96, AdaptiveType.Audio_Video),
			new VideoInfo(22, VideoType.Mp4, 720, false, false, AudioType.Aac, 192, AdaptiveType.Audio_Video),
			new VideoInfo(34, VideoType.Flv, 360, false, false, AudioType.Aac, 128, AdaptiveType.Audio_Video),
			new VideoInfo(35, VideoType.Flv, 480, false, false, AudioType.Aac, 128, AdaptiveType.Audio_Video),
			new VideoInfo(36, VideoType.Mobile_3gp, 240, false, false, AudioType.Aac, 38, AdaptiveType.Audio_Video),
			new VideoInfo(37, VideoType.Mp4, 1080, false, false, AudioType.Aac, 192, AdaptiveType.Audio_Video),
			new VideoInfo(38, VideoType.Mp4, 3072, false, false, AudioType.Aac, 192, AdaptiveType.Audio_Video),
			new VideoInfo(43, VideoType.WebM, 360, false, false, AudioType.Vorbis, 128, AdaptiveType.Audio_Video),
			new VideoInfo(44, VideoType.WebM, 480, false, false, AudioType.Vorbis, 128, AdaptiveType.Audio_Video),
			new VideoInfo(45, VideoType.WebM, 720, false, false, AudioType.Vorbis, 192, AdaptiveType.Audio_Video),
			new VideoInfo(46, VideoType.WebM, 1080, false, false, AudioType.Vorbis, 192, AdaptiveType.Audio_Video),
			new VideoInfo(82, VideoType.Mp4, 360, false, true, AudioType.Aac, 96, AdaptiveType.Audio_Video),
			new VideoInfo(83, VideoType.Mp4, 480, false, true, AudioType.Aac, 96, AdaptiveType.Audio_Video),
			new VideoInfo(84, VideoType.Mp4, 720, false, true, AudioType.Aac, 152, AdaptiveType.Audio_Video),
			new VideoInfo(85, VideoType.Mp4, 1080, false, true, AudioType.Aac, 152, AdaptiveType.Audio_Video),
			new VideoInfo(100, VideoType.Hls, 240, false, true, AudioType.Vorbis, 128, AdaptiveType.Audio_Video),
			new VideoInfo(100, VideoType.Hls, 360, false, true, AudioType.Vorbis, 128, AdaptiveType.Audio_Video),
			new VideoInfo(100, VideoType.Hls, 480, false, true, AudioType.Vorbis, 128, AdaptiveType.Audio_Video),
			new VideoInfo(100, VideoType.Hls, 720, false, true, AudioType.Vorbis, 128, AdaptiveType.Audio_Video),
			new VideoInfo(100, VideoType.Hls, 1080, false, true, AudioType.Vorbis, 128, AdaptiveType.Audio_Video),
			new VideoInfo(100, VideoType.WebM, 360, false, true, AudioType.Vorbis, 128, AdaptiveType.Audio_Video),
			new VideoInfo(101, VideoType.WebM, 360, false, true, AudioType.Vorbis, 192, AdaptiveType.Audio_Video),
			new VideoInfo(102, VideoType.WebM, 720, false, true, AudioType.Vorbis, 192, AdaptiveType.Audio_Video),
			new VideoInfo(132, VideoType.Hls, 240, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(133, VideoType.Mp4, 240, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(134, VideoType.Mp4, 360, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(135, VideoType.Mp4, 480, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(136, VideoType.Mp4, 720, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(137, VideoType.Mp4, 1080, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(138, VideoType.Mp4, 2160, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(139, VideoType.Unknown, 0, false, false, AudioType.Aac, 48, AdaptiveType.Audio),
			new VideoInfo(140, VideoType.Unknown, 0, false, false, AudioType.Aac, 128, AdaptiveType.Audio),
			new VideoInfo(141, VideoType.Unknown, 0, false, false, AudioType.Aac, 256, AdaptiveType.Audio),
			new VideoInfo(160, VideoType.Mp4, 144, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(167, VideoType.WebM, 360, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(168, VideoType.WebM, 480, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(169, VideoType.WebM, 720, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(170, VideoType.WebM, 1080, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(171, VideoType.Unknown, 0, false, false, AudioType.Vorbis, 128, AdaptiveType.Audio),
			new VideoInfo(172, VideoType.Unknown, 0, false, false, AudioType.Vorbis, 192, AdaptiveType.Audio),
			new VideoInfo(218, VideoType.WebM, 480, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(219, VideoType.WebM, 144, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(242, VideoType.WebM, 240, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(243, VideoType.WebM, 360, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(244, VideoType.WebM, 480, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(247, VideoType.WebM, 720, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(248, VideoType.WebM, 1080, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(249, VideoType.Unknown, 0, false, false, AudioType.Opus, 50, AdaptiveType.Audio),
			new VideoInfo(250, VideoType.Unknown, 0, false, false, AudioType.Opus, 70, AdaptiveType.Audio),
			new VideoInfo(251, VideoType.Unknown, 0, false, false, AudioType.Opus, 160, AdaptiveType.Audio),
			new VideoInfo(264, VideoType.Mp4, 1440, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(266, VideoType.Mp4, 2160, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(271, VideoType.WebM, 1440, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(272, VideoType.WebM, 4320, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(278, VideoType.WebM, 144, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(298, VideoType.Mp4, 720, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(299, VideoType.Mp4, 1080, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(302, VideoType.WebM, 720, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(303, VideoType.WebM, 1080, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(308, VideoType.WebM, 1440, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(313, VideoType.WebM, 2160, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(315, VideoType.WebM, 2160, false, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(330, VideoType.WebM, 144, true, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(331, VideoType.WebM, 240, true, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(332, VideoType.WebM, 360, true, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(333, VideoType.WebM, 480, true, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(334, VideoType.WebM, 720, true, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(335, VideoType.WebM, 1080, true, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(336, VideoType.WebM, 1440, true, false, AudioType.Unknown, 0, AdaptiveType.Video),
			new VideoInfo(337, VideoType.WebM, 2160, true, false, AudioType.Unknown, 0, AdaptiveType.Video)
		};
	}
}
