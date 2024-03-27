using System;

namespace Steamworks
{
	// Token: 0x020002C5 RID: 709
	internal class CallbackIdentities
	{
		// Token: 0x06000C71 RID: 3185 RVA: 0x0000C300 File Offset: 0x0000A500
		public static int GetCallbackIdentity(Type callbackStruct)
		{
			object[] customAttributes = callbackStruct.GetCustomAttributes(typeof(CallbackIdentityAttribute), false);
			int num = 0;
			if (num >= customAttributes.Length)
			{
				throw new Exception("Callback number not found for struct " + callbackStruct);
			}
			CallbackIdentityAttribute callbackIdentityAttribute = (CallbackIdentityAttribute)customAttributes[num];
			return callbackIdentityAttribute.Identity;
		}
	}
}
