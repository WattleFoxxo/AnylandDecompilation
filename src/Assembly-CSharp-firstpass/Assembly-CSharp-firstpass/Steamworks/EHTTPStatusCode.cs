using System;

namespace Steamworks
{
	// Token: 0x020002AD RID: 685
	public enum EHTTPStatusCode
	{
		// Token: 0x04000C09 RID: 3081
		k_EHTTPStatusCodeInvalid,
		// Token: 0x04000C0A RID: 3082
		k_EHTTPStatusCode100Continue = 100,
		// Token: 0x04000C0B RID: 3083
		k_EHTTPStatusCode101SwitchingProtocols,
		// Token: 0x04000C0C RID: 3084
		k_EHTTPStatusCode200OK = 200,
		// Token: 0x04000C0D RID: 3085
		k_EHTTPStatusCode201Created,
		// Token: 0x04000C0E RID: 3086
		k_EHTTPStatusCode202Accepted,
		// Token: 0x04000C0F RID: 3087
		k_EHTTPStatusCode203NonAuthoritative,
		// Token: 0x04000C10 RID: 3088
		k_EHTTPStatusCode204NoContent,
		// Token: 0x04000C11 RID: 3089
		k_EHTTPStatusCode205ResetContent,
		// Token: 0x04000C12 RID: 3090
		k_EHTTPStatusCode206PartialContent,
		// Token: 0x04000C13 RID: 3091
		k_EHTTPStatusCode300MultipleChoices = 300,
		// Token: 0x04000C14 RID: 3092
		k_EHTTPStatusCode301MovedPermanently,
		// Token: 0x04000C15 RID: 3093
		k_EHTTPStatusCode302Found,
		// Token: 0x04000C16 RID: 3094
		k_EHTTPStatusCode303SeeOther,
		// Token: 0x04000C17 RID: 3095
		k_EHTTPStatusCode304NotModified,
		// Token: 0x04000C18 RID: 3096
		k_EHTTPStatusCode305UseProxy,
		// Token: 0x04000C19 RID: 3097
		k_EHTTPStatusCode307TemporaryRedirect = 307,
		// Token: 0x04000C1A RID: 3098
		k_EHTTPStatusCode400BadRequest = 400,
		// Token: 0x04000C1B RID: 3099
		k_EHTTPStatusCode401Unauthorized,
		// Token: 0x04000C1C RID: 3100
		k_EHTTPStatusCode402PaymentRequired,
		// Token: 0x04000C1D RID: 3101
		k_EHTTPStatusCode403Forbidden,
		// Token: 0x04000C1E RID: 3102
		k_EHTTPStatusCode404NotFound,
		// Token: 0x04000C1F RID: 3103
		k_EHTTPStatusCode405MethodNotAllowed,
		// Token: 0x04000C20 RID: 3104
		k_EHTTPStatusCode406NotAcceptable,
		// Token: 0x04000C21 RID: 3105
		k_EHTTPStatusCode407ProxyAuthRequired,
		// Token: 0x04000C22 RID: 3106
		k_EHTTPStatusCode408RequestTimeout,
		// Token: 0x04000C23 RID: 3107
		k_EHTTPStatusCode409Conflict,
		// Token: 0x04000C24 RID: 3108
		k_EHTTPStatusCode410Gone,
		// Token: 0x04000C25 RID: 3109
		k_EHTTPStatusCode411LengthRequired,
		// Token: 0x04000C26 RID: 3110
		k_EHTTPStatusCode412PreconditionFailed,
		// Token: 0x04000C27 RID: 3111
		k_EHTTPStatusCode413RequestEntityTooLarge,
		// Token: 0x04000C28 RID: 3112
		k_EHTTPStatusCode414RequestURITooLong,
		// Token: 0x04000C29 RID: 3113
		k_EHTTPStatusCode415UnsupportedMediaType,
		// Token: 0x04000C2A RID: 3114
		k_EHTTPStatusCode416RequestedRangeNotSatisfiable,
		// Token: 0x04000C2B RID: 3115
		k_EHTTPStatusCode417ExpectationFailed,
		// Token: 0x04000C2C RID: 3116
		k_EHTTPStatusCode4xxUnknown,
		// Token: 0x04000C2D RID: 3117
		k_EHTTPStatusCode429TooManyRequests = 429,
		// Token: 0x04000C2E RID: 3118
		k_EHTTPStatusCode500InternalServerError = 500,
		// Token: 0x04000C2F RID: 3119
		k_EHTTPStatusCode501NotImplemented,
		// Token: 0x04000C30 RID: 3120
		k_EHTTPStatusCode502BadGateway,
		// Token: 0x04000C31 RID: 3121
		k_EHTTPStatusCode503ServiceUnavailable,
		// Token: 0x04000C32 RID: 3122
		k_EHTTPStatusCode504GatewayTimeout,
		// Token: 0x04000C33 RID: 3123
		k_EHTTPStatusCode505HTTPVersionNotSupported,
		// Token: 0x04000C34 RID: 3124
		k_EHTTPStatusCode5xxUnknown = 599
	}
}
