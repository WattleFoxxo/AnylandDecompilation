using System;
using System.Reflection;
using UnityEngine.UI;

// Token: 0x020002C2 RID: 706
public static class UISetExtensions
{
	// Token: 0x06001A67 RID: 6759 RVA: 0x000EDE18 File Offset: 0x000EC218
	public static void Set(this Toggle instance, bool value, bool sendCallback = false)
	{
		UISetExtensions.toggleSetMethod.Invoke(instance, new object[] { value, sendCallback });
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x000EDE3E File Offset: 0x000EC23E
	public static void Set(this Slider instance, float value, bool sendCallback = false)
	{
		UISetExtensions.sliderSetMethod.Invoke(instance, new object[] { value, sendCallback });
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x000EDE64 File Offset: 0x000EC264
	public static void Set(this Scrollbar instance, float value, bool sendCallback = false)
	{
		UISetExtensions.scrollbarSetMethod.Invoke(instance, new object[] { value, sendCallback });
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x000EDE8A File Offset: 0x000EC28A
	public static void Set(this Dropdown instance, int value)
	{
		UISetExtensions.dropdownValueField.SetValue(instance, value);
		instance.RefreshShownValue();
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x000EDEA4 File Offset: 0x000EC2A4
	private static MethodInfo FindSetMethod(Type objectType)
	{
		MethodInfo[] methods = objectType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
		for (int i = 0; i < methods.Length; i++)
		{
			if (methods[i].Name == "Set" && methods[i].GetParameters().Length == 2)
			{
				return methods[i];
			}
		}
		return null;
	}

	// Token: 0x04001859 RID: 6233
	private static readonly MethodInfo toggleSetMethod = UISetExtensions.FindSetMethod(typeof(Toggle));

	// Token: 0x0400185A RID: 6234
	private static readonly MethodInfo sliderSetMethod = UISetExtensions.FindSetMethod(typeof(Slider));

	// Token: 0x0400185B RID: 6235
	private static readonly MethodInfo scrollbarSetMethod = UISetExtensions.FindSetMethod(typeof(Scrollbar));

	// Token: 0x0400185C RID: 6236
	private static readonly FieldInfo dropdownValueField = typeof(Dropdown).GetField("m_Value", BindingFlags.Instance | BindingFlags.NonPublic);
}
