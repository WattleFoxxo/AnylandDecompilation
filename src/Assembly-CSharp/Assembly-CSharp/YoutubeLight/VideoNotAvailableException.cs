using System;

namespace YoutubeLight
{
	// Token: 0x020002D7 RID: 727
	public class VideoNotAvailableException : Exception
	{
		// Token: 0x06001B01 RID: 6913 RVA: 0x000F48F1 File Offset: 0x000F2CF1
		public VideoNotAvailableException()
		{
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x000F48F9 File Offset: 0x000F2CF9
		public VideoNotAvailableException(string message)
			: base(message)
		{
		}
	}
}
