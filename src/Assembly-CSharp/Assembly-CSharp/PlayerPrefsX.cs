using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002BE RID: 702
public class PlayerPrefsX
{
	// Token: 0x06001A20 RID: 6688 RVA: 0x000ECA60 File Offset: 0x000EAE60
	public static bool SetBool(string name, bool value)
	{
		try
		{
			PlayerPrefs.SetInt(name, (!value) ? 0 : 1);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x000ECAA0 File Offset: 0x000EAEA0
	public static bool GetBool(string name)
	{
		return PlayerPrefs.GetInt(name) == 1;
	}

	// Token: 0x06001A22 RID: 6690 RVA: 0x000ECAAB File Offset: 0x000EAEAB
	public static bool GetBool(string name, bool defaultValue)
	{
		return 1 == PlayerPrefs.GetInt(name, (!defaultValue) ? 0 : 1);
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x000ECAC4 File Offset: 0x000EAEC4
	public static long GetLong(string key, long defaultValue)
	{
		int @int;
		int int2;
		PlayerPrefsX.SplitLong(defaultValue, out @int, out int2);
		@int = PlayerPrefs.GetInt(key + "_lowBits", @int);
		int2 = PlayerPrefs.GetInt(key + "_highBits", int2);
		ulong num = (ulong)int2;
		num <<= 32;
		return (long)(num | (ulong)@int);
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x000ECB0C File Offset: 0x000EAF0C
	public static long GetLong(string key)
	{
		int @int = PlayerPrefs.GetInt(key + "_lowBits");
		int int2 = PlayerPrefs.GetInt(key + "_highBits");
		ulong num = (ulong)int2;
		num <<= 32;
		return (long)(num | (ulong)@int);
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x000ECB47 File Offset: 0x000EAF47
	private static void SplitLong(long input, out int lowBits, out int highBits)
	{
		lowBits = (int)((uint)input);
		highBits = (int)((uint)(input >> 32));
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x000ECB54 File Offset: 0x000EAF54
	public static void SetLong(string key, long value)
	{
		int num;
		int num2;
		PlayerPrefsX.SplitLong(value, out num, out num2);
		PlayerPrefs.SetInt(key + "_lowBits", num);
		PlayerPrefs.SetInt(key + "_highBits", num2);
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x000ECB8D File Offset: 0x000EAF8D
	public static bool SetVector2(string key, Vector2 vector)
	{
		return PlayerPrefsX.SetFloatArray(key, new float[] { vector.x, vector.y });
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x000ECBB0 File Offset: 0x000EAFB0
	private static Vector2 GetVector2(string key)
	{
		float[] floatArray = PlayerPrefsX.GetFloatArray(key);
		if (floatArray.Length < 2)
		{
			return Vector2.zero;
		}
		return new Vector2(floatArray[0], floatArray[1]);
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x000ECBDE File Offset: 0x000EAFDE
	public static Vector2 GetVector2(string key, Vector2 defaultValue)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetVector2(key);
		}
		return defaultValue;
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x000ECBF3 File Offset: 0x000EAFF3
	public static bool SetVector3(string key, Vector3 vector)
	{
		return PlayerPrefsX.SetFloatArray(key, new float[] { vector.x, vector.y, vector.z });
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x000ECC20 File Offset: 0x000EB020
	public static Vector3 GetVector3(string key)
	{
		float[] floatArray = PlayerPrefsX.GetFloatArray(key);
		if (floatArray.Length < 3)
		{
			return Vector3.zero;
		}
		return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x000ECC51 File Offset: 0x000EB051
	public static Vector3 GetVector3(string key, Vector3 defaultValue)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetVector3(key);
		}
		return defaultValue;
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x000ECC66 File Offset: 0x000EB066
	public static bool SetQuaternion(string key, Quaternion vector)
	{
		return PlayerPrefsX.SetFloatArray(key, new float[] { vector.x, vector.y, vector.z, vector.w });
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x000ECC9C File Offset: 0x000EB09C
	public static Quaternion GetQuaternion(string key)
	{
		float[] floatArray = PlayerPrefsX.GetFloatArray(key);
		if (floatArray.Length < 4)
		{
			return Quaternion.identity;
		}
		return new Quaternion(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x000ECCD0 File Offset: 0x000EB0D0
	public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetQuaternion(key);
		}
		return defaultValue;
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x000ECCE5 File Offset: 0x000EB0E5
	public static bool SetColor(string key, Color color)
	{
		return PlayerPrefsX.SetFloatArray(key, new float[] { color.r, color.g, color.b, color.a });
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x000ECD1C File Offset: 0x000EB11C
	public static Color GetColor(string key)
	{
		float[] floatArray = PlayerPrefsX.GetFloatArray(key);
		if (floatArray.Length < 4)
		{
			return new Color(0f, 0f, 0f, 0f);
		}
		return new Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x000ECD64 File Offset: 0x000EB164
	public static Color GetColor(string key, Color defaultValue)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetColor(key);
		}
		return defaultValue;
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x000ECD7C File Offset: 0x000EB17C
	public static bool SetBoolArray(string key, bool[] boolArray)
	{
		byte[] array = new byte[(boolArray.Length + 7) / 8 + 5];
		array[0] = Convert.ToByte(PlayerPrefsX.ArrayType.Bool);
		BitArray bitArray = new BitArray(boolArray);
		bitArray.CopyTo(array, 5);
		PlayerPrefsX.Initialize();
		PlayerPrefsX.ConvertInt32ToBytes(boolArray.Length, array);
		return PlayerPrefsX.SaveBytes(key, array);
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x000ECDCC File Offset: 0x000EB1CC
	public static bool[] GetBoolArray(string key)
	{
		if (!PlayerPrefs.HasKey(key))
		{
			return new bool[0];
		}
		byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
		if (array.Length < 5)
		{
			Debug.LogError("Corrupt preference file for " + key);
			return new bool[0];
		}
		if (array[0] != 2)
		{
			Debug.LogError(key + " is not a boolean array");
			return new bool[0];
		}
		PlayerPrefsX.Initialize();
		byte[] array2 = new byte[array.Length - 5];
		Array.Copy(array, 5, array2, 0, array2.Length);
		BitArray bitArray = new BitArray(array2);
		bitArray.Length = PlayerPrefsX.ConvertBytesToInt32(array);
		bool[] array3 = new bool[bitArray.Count];
		bitArray.CopyTo(array3, 0);
		return array3;
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x000ECE7C File Offset: 0x000EB27C
	public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetBoolArray(key);
		}
		bool[] array = new bool[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x000ECEBC File Offset: 0x000EB2BC
	public static bool SetStringArray(string key, string[] stringArray)
	{
		byte[] array = new byte[stringArray.Length + 1];
		array[0] = Convert.ToByte(PlayerPrefsX.ArrayType.String);
		PlayerPrefsX.Initialize();
		for (int i = 0; i < stringArray.Length; i++)
		{
			if (stringArray[i] == null)
			{
				Debug.LogError("Can't save null entries in the string array when setting " + key);
				return false;
			}
			if (stringArray[i].Length > 255)
			{
				Debug.LogError("Strings cannot be longer than 255 characters when setting " + key);
				return false;
			}
			array[PlayerPrefsX.idx++] = (byte)stringArray[i].Length;
		}
		try
		{
			PlayerPrefs.SetString(key, Convert.ToBase64String(array) + "|" + string.Join(string.Empty, stringArray));
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x000ECF94 File Offset: 0x000EB394
	public static string[] GetStringArray(string key)
	{
		if (!PlayerPrefs.HasKey(key))
		{
			return new string[0];
		}
		string @string = PlayerPrefs.GetString(key);
		int num = @string.IndexOf("|"[0]);
		if (num < 4)
		{
			Debug.LogError("Corrupt preference file for " + key);
			return new string[0];
		}
		byte[] array = Convert.FromBase64String(@string.Substring(0, num));
		if (array[0] != 3)
		{
			Debug.LogError(key + " is not a string array");
			return new string[0];
		}
		PlayerPrefsX.Initialize();
		int num2 = array.Length - 1;
		string[] array2 = new string[num2];
		int num3 = num + 1;
		for (int i = 0; i < num2; i++)
		{
			int num4 = (int)array[PlayerPrefsX.idx++];
			if (num3 + num4 > @string.Length)
			{
				Debug.LogError("Corrupt preference file for " + key);
				return new string[0];
			}
			array2[i] = @string.Substring(num3, num4);
			num3 += num4;
		}
		return array2;
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x000ED098 File Offset: 0x000EB498
	public static string[] GetStringArray(string key, string defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetStringArray(key);
		}
		string[] array = new string[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x000ED0D5 File Offset: 0x000EB4D5
	public static bool SetIntArray(string key, int[] intArray)
	{
		return PlayerPrefsX.SetValue<int[]>(key, intArray, PlayerPrefsX.ArrayType.Int32, 1, new Action<int[], byte[], int>(PlayerPrefsX.ConvertFromInt));
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x000ED0FD File Offset: 0x000EB4FD
	public static bool SetFloatArray(string key, float[] floatArray)
	{
		return PlayerPrefsX.SetValue<float[]>(key, floatArray, PlayerPrefsX.ArrayType.Float, 1, new Action<float[], byte[], int>(PlayerPrefsX.ConvertFromFloat));
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000ED125 File Offset: 0x000EB525
	public static bool SetVector2Array(string key, Vector2[] vector2Array)
	{
		return PlayerPrefsX.SetValue<Vector2[]>(key, vector2Array, PlayerPrefsX.ArrayType.Vector2, 2, new Action<Vector2[], byte[], int>(PlayerPrefsX.ConvertFromVector2));
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x000ED14D File Offset: 0x000EB54D
	public static bool SetVector3Array(string key, Vector3[] vector3Array)
	{
		return PlayerPrefsX.SetValue<Vector3[]>(key, vector3Array, PlayerPrefsX.ArrayType.Vector3, 3, new Action<Vector3[], byte[], int>(PlayerPrefsX.ConvertFromVector3));
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x000ED175 File Offset: 0x000EB575
	public static bool SetQuaternionArray(string key, Quaternion[] quaternionArray)
	{
		return PlayerPrefsX.SetValue<Quaternion[]>(key, quaternionArray, PlayerPrefsX.ArrayType.Quaternion, 4, new Action<Quaternion[], byte[], int>(PlayerPrefsX.ConvertFromQuaternion));
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x000ED19D File Offset: 0x000EB59D
	public static bool SetColorArray(string key, Color[] colorArray)
	{
		return PlayerPrefsX.SetValue<Color[]>(key, colorArray, PlayerPrefsX.ArrayType.Color, 4, new Action<Color[], byte[], int>(PlayerPrefsX.ConvertFromColor));
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x000ED1C8 File Offset: 0x000EB5C8
	private static bool SetValue<T>(string key, T array, PlayerPrefsX.ArrayType arrayType, int vectorNumber, Action<T, byte[], int> convert) where T : IList
	{
		byte[] array2 = new byte[4 * array.Count * vectorNumber + 1];
		array2[0] = Convert.ToByte(arrayType);
		PlayerPrefsX.Initialize();
		for (int i = 0; i < array.Count; i++)
		{
			convert(array, array2, i);
		}
		return PlayerPrefsX.SaveBytes(key, array2);
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x000ED230 File Offset: 0x000EB630
	private static void ConvertFromInt(int[] array, byte[] bytes, int i)
	{
		PlayerPrefsX.ConvertInt32ToBytes(array[i], bytes);
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x000ED23B File Offset: 0x000EB63B
	private static void ConvertFromFloat(float[] array, byte[] bytes, int i)
	{
		PlayerPrefsX.ConvertFloatToBytes(array[i], bytes);
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x000ED246 File Offset: 0x000EB646
	private static void ConvertFromVector2(Vector2[] array, byte[] bytes, int i)
	{
		PlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x000ED26C File Offset: 0x000EB66C
	private static void ConvertFromVector3(Vector3[] array, byte[] bytes, int i)
	{
		PlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].z, bytes);
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x000ED2A4 File Offset: 0x000EB6A4
	private static void ConvertFromQuaternion(Quaternion[] array, byte[] bytes, int i)
	{
		PlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].z, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].w, bytes);
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x000ED2FC File Offset: 0x000EB6FC
	private static void ConvertFromColor(Color[] array, byte[] bytes, int i)
	{
		PlayerPrefsX.ConvertFloatToBytes(array[i].r, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].g, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].b, bytes);
		PlayerPrefsX.ConvertFloatToBytes(array[i].a, bytes);
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x000ED354 File Offset: 0x000EB754
	public static int[] GetIntArray(string key)
	{
		List<int> list = new List<int>();
		PlayerPrefsX.GetValue<List<int>>(key, list, PlayerPrefsX.ArrayType.Int32, 1, new Action<List<int>, byte[]>(PlayerPrefsX.ConvertToInt));
		return list.ToArray();
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x000ED394 File Offset: 0x000EB794
	public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetIntArray(key);
		}
		int[] array = new int[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x000ED3D4 File Offset: 0x000EB7D4
	public static float[] GetFloatArray(string key)
	{
		List<float> list = new List<float>();
		PlayerPrefsX.GetValue<List<float>>(key, list, PlayerPrefsX.ArrayType.Float, 1, new Action<List<float>, byte[]>(PlayerPrefsX.ConvertToFloat));
		return list.ToArray();
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x000ED414 File Offset: 0x000EB814
	public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetFloatArray(key);
		}
		float[] array = new float[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x000ED454 File Offset: 0x000EB854
	public static Vector2[] GetVector2Array(string key)
	{
		List<Vector2> list = new List<Vector2>();
		PlayerPrefsX.GetValue<List<Vector2>>(key, list, PlayerPrefsX.ArrayType.Vector2, 2, new Action<List<Vector2>, byte[]>(PlayerPrefsX.ConvertToVector2));
		return list.ToArray();
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x000ED494 File Offset: 0x000EB894
	public static Vector2[] GetVector2Array(string key, Vector2 defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetVector2Array(key);
		}
		Vector2[] array = new Vector2[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x000ED4DC File Offset: 0x000EB8DC
	public static Vector3[] GetVector3Array(string key)
	{
		List<Vector3> list = new List<Vector3>();
		PlayerPrefsX.GetValue<List<Vector3>>(key, list, PlayerPrefsX.ArrayType.Vector3, 3, new Action<List<Vector3>, byte[]>(PlayerPrefsX.ConvertToVector3));
		return list.ToArray();
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x000ED51C File Offset: 0x000EB91C
	public static Vector3[] GetVector3Array(string key, Vector3 defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetVector3Array(key);
		}
		Vector3[] array = new Vector3[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x000ED564 File Offset: 0x000EB964
	public static Quaternion[] GetQuaternionArray(string key)
	{
		List<Quaternion> list = new List<Quaternion>();
		PlayerPrefsX.GetValue<List<Quaternion>>(key, list, PlayerPrefsX.ArrayType.Quaternion, 4, new Action<List<Quaternion>, byte[]>(PlayerPrefsX.ConvertToQuaternion));
		return list.ToArray();
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x000ED5A4 File Offset: 0x000EB9A4
	public static Quaternion[] GetQuaternionArray(string key, Quaternion defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetQuaternionArray(key);
		}
		Quaternion[] array = new Quaternion[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x000ED5EC File Offset: 0x000EB9EC
	public static Color[] GetColorArray(string key)
	{
		List<Color> list = new List<Color>();
		PlayerPrefsX.GetValue<List<Color>>(key, list, PlayerPrefsX.ArrayType.Color, 4, new Action<List<Color>, byte[]>(PlayerPrefsX.ConvertToColor));
		return list.ToArray();
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x000ED62C File Offset: 0x000EBA2C
	public static Color[] GetColorArray(string key, Color defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetColorArray(key);
		}
		Color[] array = new Color[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x000ED674 File Offset: 0x000EBA74
	private static void GetValue<T>(string key, T list, PlayerPrefsX.ArrayType arrayType, int vectorNumber, Action<T, byte[]> convert) where T : IList
	{
		if (PlayerPrefs.HasKey(key))
		{
			byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
			if ((array.Length - 1) % (vectorNumber * 4) != 0)
			{
				Debug.LogError("Corrupt preference file for " + key);
				return;
			}
			if ((PlayerPrefsX.ArrayType)array[0] != arrayType)
			{
				Debug.LogError(key + " is not a " + arrayType.ToString() + " array");
				return;
			}
			PlayerPrefsX.Initialize();
			int num = (array.Length - 1) / (vectorNumber * 4);
			for (int i = 0; i < num; i++)
			{
				convert(list, array);
			}
		}
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x000ED70D File Offset: 0x000EBB0D
	private static void ConvertToInt(List<int> list, byte[] bytes)
	{
		list.Add(PlayerPrefsX.ConvertBytesToInt32(bytes));
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x000ED71B File Offset: 0x000EBB1B
	private static void ConvertToFloat(List<float> list, byte[] bytes)
	{
		list.Add(PlayerPrefsX.ConvertBytesToFloat(bytes));
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x000ED729 File Offset: 0x000EBB29
	private static void ConvertToVector2(List<Vector2> list, byte[] bytes)
	{
		list.Add(new Vector2(PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes)));
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x000ED742 File Offset: 0x000EBB42
	private static void ConvertToVector3(List<Vector3> list, byte[] bytes)
	{
		list.Add(new Vector3(PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes)));
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x000ED761 File Offset: 0x000EBB61
	private static void ConvertToQuaternion(List<Quaternion> list, byte[] bytes)
	{
		list.Add(new Quaternion(PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes)));
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x000ED786 File Offset: 0x000EBB86
	private static void ConvertToColor(List<Color> list, byte[] bytes)
	{
		list.Add(new Color(PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes), PlayerPrefsX.ConvertBytesToFloat(bytes)));
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x000ED7AC File Offset: 0x000EBBAC
	public static void ShowArrayType(string key)
	{
		byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
		if (array.Length > 0)
		{
			PlayerPrefsX.ArrayType arrayType = (PlayerPrefsX.ArrayType)array[0];
			Debug.Log(key + " is a " + arrayType.ToString() + " array");
		}
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x000ED7F4 File Offset: 0x000EBBF4
	private static void Initialize()
	{
		if (BitConverter.IsLittleEndian)
		{
			PlayerPrefsX.endianDiff1 = 0;
			PlayerPrefsX.endianDiff2 = 0;
		}
		else
		{
			PlayerPrefsX.endianDiff1 = 3;
			PlayerPrefsX.endianDiff2 = 1;
		}
		if (PlayerPrefsX.byteBlock == null)
		{
			PlayerPrefsX.byteBlock = new byte[4];
		}
		PlayerPrefsX.idx = 1;
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x000ED844 File Offset: 0x000EBC44
	private static bool SaveBytes(string key, byte[] bytes)
	{
		try
		{
			PlayerPrefs.SetString(key, Convert.ToBase64String(bytes));
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x000ED880 File Offset: 0x000EBC80
	private static void ConvertFloatToBytes(float f, byte[] bytes)
	{
		PlayerPrefsX.byteBlock = BitConverter.GetBytes(f);
		PlayerPrefsX.ConvertTo4Bytes(bytes);
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x000ED893 File Offset: 0x000EBC93
	private static float ConvertBytesToFloat(byte[] bytes)
	{
		PlayerPrefsX.ConvertFrom4Bytes(bytes);
		return BitConverter.ToSingle(PlayerPrefsX.byteBlock, 0);
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x000ED8A6 File Offset: 0x000EBCA6
	private static void ConvertInt32ToBytes(int i, byte[] bytes)
	{
		PlayerPrefsX.byteBlock = BitConverter.GetBytes(i);
		PlayerPrefsX.ConvertTo4Bytes(bytes);
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x000ED8B9 File Offset: 0x000EBCB9
	private static int ConvertBytesToInt32(byte[] bytes)
	{
		PlayerPrefsX.ConvertFrom4Bytes(bytes);
		return BitConverter.ToInt32(PlayerPrefsX.byteBlock, 0);
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x000ED8CC File Offset: 0x000EBCCC
	private static void ConvertTo4Bytes(byte[] bytes)
	{
		bytes[PlayerPrefsX.idx] = PlayerPrefsX.byteBlock[PlayerPrefsX.endianDiff1];
		bytes[PlayerPrefsX.idx + 1] = PlayerPrefsX.byteBlock[1 + PlayerPrefsX.endianDiff2];
		bytes[PlayerPrefsX.idx + 2] = PlayerPrefsX.byteBlock[2 - PlayerPrefsX.endianDiff2];
		bytes[PlayerPrefsX.idx + 3] = PlayerPrefsX.byteBlock[3 - PlayerPrefsX.endianDiff1];
		PlayerPrefsX.idx += 4;
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x000ED93C File Offset: 0x000EBD3C
	private static void ConvertFrom4Bytes(byte[] bytes)
	{
		PlayerPrefsX.byteBlock[PlayerPrefsX.endianDiff1] = bytes[PlayerPrefsX.idx];
		PlayerPrefsX.byteBlock[1 + PlayerPrefsX.endianDiff2] = bytes[PlayerPrefsX.idx + 1];
		PlayerPrefsX.byteBlock[2 - PlayerPrefsX.endianDiff2] = bytes[PlayerPrefsX.idx + 2];
		PlayerPrefsX.byteBlock[3 - PlayerPrefsX.endianDiff1] = bytes[PlayerPrefsX.idx + 3];
		PlayerPrefsX.idx += 4;
	}

	// Token: 0x04001834 RID: 6196
	private static int endianDiff1;

	// Token: 0x04001835 RID: 6197
	private static int endianDiff2;

	// Token: 0x04001836 RID: 6198
	private static int idx;

	// Token: 0x04001837 RID: 6199
	private static byte[] byteBlock;

	// Token: 0x020002BF RID: 703
	private enum ArrayType
	{
		// Token: 0x04001845 RID: 6213
		Float,
		// Token: 0x04001846 RID: 6214
		Int32,
		// Token: 0x04001847 RID: 6215
		Bool,
		// Token: 0x04001848 RID: 6216
		String,
		// Token: 0x04001849 RID: 6217
		Vector2,
		// Token: 0x0400184A RID: 6218
		Vector3,
		// Token: 0x0400184B RID: 6219
		Quaternion,
		// Token: 0x0400184C RID: 6220
		Color
	}
}
