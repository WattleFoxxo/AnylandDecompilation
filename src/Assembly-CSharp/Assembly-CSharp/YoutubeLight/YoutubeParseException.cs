using System;

namespace YoutubeLight
{
	// Token: 0x020002D8 RID: 728
	public class YoutubeParseException : Exception
	{
		// Token: 0x06001B03 RID: 6915 RVA: 0x000F4902 File Offset: 0x000F2D02
		public YoutubeParseException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
