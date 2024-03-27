using System;
using POpusCodec.Enums;

namespace POpusCodec
{
	// Token: 0x02000195 RID: 405
	public class OpusException : Exception
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x00005352 File Offset: 0x00003552
		public OpusException(OpusStatusCode statusCode, string message)
			: base(message)
		{
			this._statusCode = statusCode;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00005362 File Offset: 0x00003562
		public OpusStatusCode StatusCode
		{
			get
			{
				return this._statusCode;
			}
		}

		// Token: 0x04000562 RID: 1378
		private OpusStatusCode _statusCode;
	}
}
