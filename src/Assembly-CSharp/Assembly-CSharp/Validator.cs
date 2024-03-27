using System;

// Token: 0x020001DA RID: 474
public static class Validator
{
	// Token: 0x06000ECF RID: 3791 RVA: 0x0008285C File Offset: 0x00080C5C
	public static bool ContainsOnly(string textToCheck, string allowedChars)
	{
		bool flag = true;
		for (int i = 0; i < textToCheck.Length; i++)
		{
			if (allowedChars.IndexOf(textToCheck[i]) == -1)
			{
				flag = false;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x000828A0 File Offset: 0x00080CA0
	public static bool AreaNameIsValid(string name, out string errorString)
	{
		errorString = string.Empty;
		bool flag = false;
		bool flag2 = false;
		name = name.Trim();
		name = Managers.areaManager.GetUrlNameFromName(name, ref flag, ref flag2);
		if (flag)
		{
			errorString = "Please use a longer area name";
		}
		else if (flag2)
		{
			errorString = "Please use a shorter area name";
		}
		return name != null;
	}

	// Token: 0x04000FA1 RID: 4001
	public const string lowerCaseLettersAndNumbers = "abcdefghijklmnopqrstuvwxyz0123456789";

	// Token: 0x04000FA2 RID: 4002
	public const string charSetBasic = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ -_'";

	// Token: 0x04000FA3 RID: 4003
	public const string charSetExtended = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ -_'!?\".,";

	// Token: 0x04000FA4 RID: 4004
	public const string charSetSpecial = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ -_'!?\".,@#$%^&*()~[]{};:<>/=§+★☆☑☛✓→←|é";

	// Token: 0x04000FA5 RID: 4005
	public const string coverImageUrlIdMustContainOnly = "abcdefghijklmnopqrstuvwxyz0123456789-_/.";

	// Token: 0x04000FA6 RID: 4006
	public const string steamImageUrlIdMustContainOnly = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789/";
}
