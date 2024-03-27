using System;

namespace LitJson
{
	// Token: 0x02000028 RID: 40
	public class JsonException : ApplicationException
	{
		// Token: 0x0600016D RID: 365 RVA: 0x0000629A File Offset: 0x0000469A
		public JsonException()
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000062A2 File Offset: 0x000046A2
		internal JsonException(ParserToken token)
			: base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000062BA File Offset: 0x000046BA
		internal JsonException(ParserToken token, Exception inner_exception)
			: base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000062D3 File Offset: 0x000046D3
		internal JsonException(int c)
			: base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000062EC File Offset: 0x000046EC
		internal JsonException(int c, Exception inner_exception)
			: base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006306 File Offset: 0x00004706
		public JsonException(string message)
			: base(message)
		{
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000630F File Offset: 0x0000470F
		public JsonException(string message, Exception inner_exception)
			: base(message, inner_exception)
		{
		}
	}
}
