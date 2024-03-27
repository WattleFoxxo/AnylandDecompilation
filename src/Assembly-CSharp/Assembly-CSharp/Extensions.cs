using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200005C RID: 92
public static class Extensions
{
	// Token: 0x06000308 RID: 776 RVA: 0x0000C5BC File Offset: 0x0000A9BC
	public static ParameterInfo[] GetCachedParemeters(this MethodInfo mo)
	{
		ParameterInfo[] parameters;
		if (!Extensions.ParametersOfMethods.TryGetValue(mo, out parameters))
		{
			parameters = mo.GetParameters();
			Extensions.ParametersOfMethods[mo] = parameters;
		}
		return parameters;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0000C5F1 File Offset: 0x0000A9F1
	public static PhotonView[] GetPhotonViewsInChildren(this GameObject go)
	{
		return go.GetComponentsInChildren<PhotonView>(true);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0000C5FA File Offset: 0x0000A9FA
	public static PhotonView GetPhotonView(this GameObject go)
	{
		return go.GetComponent<PhotonView>();
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0000C604 File Offset: 0x0000AA04
	public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagnitudePrecision;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0000C624 File Offset: 0x0000AA24
	public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagnitudePrecision;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0000C643 File Offset: 0x0000AA43
	public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
	{
		return Quaternion.Angle(target, second) < maxAngle;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0000C64F File Offset: 0x0000AA4F
	public static bool AlmostEquals(this float target, float second, float floatDiff)
	{
		return Mathf.Abs(target - second) < floatDiff;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0000C65C File Offset: 0x0000AA5C
	public static void Merge(this IDictionary target, IDictionary addHash)
	{
		if (addHash == null || target.Equals(addHash))
		{
			return;
		}
		IEnumerator enumerator = addHash.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				target[obj] = addHash[obj];
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0000C6D8 File Offset: 0x0000AAD8
	public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
	{
		if (addHash == null || target.Equals(addHash))
		{
			return;
		}
		IEnumerator enumerator = addHash.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (obj is string)
				{
					target[obj] = addHash[obj];
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0000C760 File Offset: 0x0000AB60
	public static string ToStringFull(this IDictionary origin)
	{
		return SupportClass.DictionaryToString(origin, false);
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0000C76C File Offset: 0x0000AB6C
	public static string ToStringFull(this object[] data)
	{
		if (data == null)
		{
			return "null";
		}
		string[] array = new string[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			object obj = data[i];
			array[i] = ((obj == null) ? "null" : obj.ToString());
		}
		return string.Join(", ", array);
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0000C7CC File Offset: 0x0000ABCC
	public static ExitGames.Client.Photon.Hashtable StripToStringKeys(this IDictionary original)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if (original != null)
		{
			IEnumerator enumerator = original.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (obj is string)
					{
						hashtable[obj] = original[obj];
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
		return hashtable;
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0000C84C File Offset: 0x0000AC4C
	public static void StripKeysWithNullValues(this IDictionary original)
	{
		object[] array = new object[original.Count];
		int num = 0;
		IEnumerator enumerator = original.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				array[num++] = obj;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		foreach (object obj2 in array)
		{
			if (original[obj2] == null)
			{
				original.Remove(obj2);
			}
		}
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0000C8F0 File Offset: 0x0000ACF0
	public static bool Contains(this int[] target, int nr)
	{
		if (target == null)
		{
			return false;
		}
		for (int i = 0; i < target.Length; i++)
		{
			if (target[i] == nr)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040001F0 RID: 496
	public static Dictionary<MethodInfo, ParameterInfo[]> ParametersOfMethods = new Dictionary<MethodInfo, ParameterInfo[]>();
}
