using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000205 RID: 517
public static class Misc
{
	// Token: 0x06001421 RID: 5153 RVA: 0x000B54AC File Offset: 0x000B38AC
	public static Vector3 AngleLerp(Vector3 startAngle, Vector3 endAngle, float t)
	{
		float num = Mathf.LerpAngle(startAngle.x, endAngle.x, t);
		float num2 = Mathf.LerpAngle(startAngle.y, endAngle.y, t);
		float num3 = Mathf.LerpAngle(startAngle.z, endAngle.z, t);
		return new Vector3(num, num2, num3);
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x000B5500 File Offset: 0x000B3900
	public static Vector3? PlayerPrefsGetVector3(string key)
	{
		Vector3? vector = null;
		if (PlayerPrefs.HasKey(key + "_x") && PlayerPrefs.HasKey(key + "_y") && PlayerPrefs.HasKey(key + "_z"))
		{
			vector = new Vector3?(new Vector3(PlayerPrefs.GetFloat(key + "_x"), PlayerPrefs.GetFloat(key + "_y"), PlayerPrefs.GetFloat(key + "_z")));
		}
		return vector;
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x000B5594 File Offset: 0x000B3994
	public static void PlayerPrefsSetVector3(string key, Vector3 vector)
	{
		PlayerPrefs.SetFloat(key + "_x", vector.x);
		PlayerPrefs.SetFloat(key + "_y", vector.y);
		PlayerPrefs.SetFloat(key + "_z", vector.z);
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x000B55E6 File Offset: 0x000B39E6
	public static string AddThousandSeparatorComma(int number)
	{
		return number.ToString("n0");
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x000B55F4 File Offset: 0x000B39F4
	public static int BoolToInt(bool value)
	{
		return (!value) ? 0 : 1;
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x000B5604 File Offset: 0x000B3A04
	public static string ToTitleCase(string s)
	{
		if (s == null || s.Length == 0)
		{
			return s;
		}
		string[] array = s.Split(new char[] { ' ' });
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Length > 1)
			{
				array[i] = char.ToUpper(array[i][0]) + array[i].Substring(1);
				break;
			}
		}
		return string.Join(" ", array);
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x000B568C File Offset: 0x000B3A8C
	public static Vector3 ReduceVector3Digits(Vector3 v, int digits)
	{
		return new Vector3((float)Math.Round((double)v.x, digits), (float)Math.Round((double)v.y, digits), (float)Math.Round((double)v.z, digits));
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x000B56C0 File Offset: 0x000B3AC0
	public static string RemoveFromStart(string s, string startString)
	{
		if (s != null && s.IndexOf(startString) == 0)
		{
			s = s.Substring(startString.Length);
		}
		return s;
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x000B56E3 File Offset: 0x000B3AE3
	public static Side TopographyIdToSide(TopographyId topographyId)
	{
		return (topographyId != TopographyId.Left) ? Side.Right : Side.Left;
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x000B56F4 File Offset: 0x000B3AF4
	public static Vector3 GetRandomVector3(float maxOffset)
	{
		return new Vector3(global::UnityEngine.Random.Range(-maxOffset, maxOffset), global::UnityEngine.Random.Range(-maxOffset, maxOffset), global::UnityEngine.Random.Range(-maxOffset, maxOffset));
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x000B5714 File Offset: 0x000B3B14
	public static void ReloadScene()
	{
		AreaManager.areaToLoadAfterSceneChange = Managers.areaManager.currentAreaName;
		Misc.CleanUpBeforeSceneChange();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x000B5747 File Offset: 0x000B3B47
	public static void CleanUpBeforeSceneChange()
	{
		Managers.broadcastNetworkManager.Disconnect();
		Managers.personManager.ourPerson.ClearComponentNetworkViewIds();
		Managers.steamManager.ShutdownSteam();
		Managers.ClearStartupCompletedHandler();
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x000B5771 File Offset: 0x000B3B71
	public static bool ContainsAny(string haystack, IEnumerable<string> needles)
	{
		return needles.Any(new Func<string, bool>(haystack.Contains));
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x000B5785 File Offset: 0x000B3B85
	public static void RandomizeRandomizer()
	{
		global::UnityEngine.Random.seed = Environment.TickCount;
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x000B5794 File Offset: 0x000B3B94
	public static string SimpleEncrypt(string text)
	{
		SymmetricAlgorithm symmetricAlgorithm = DES.Create();
		ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor(Misc.encryptKey, Misc.encryptIv);
		byte[] bytes = Encoding.Unicode.GetBytes(text);
		byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
		return Convert.ToBase64String(array);
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x000B57D8 File Offset: 0x000B3BD8
	public static string SimpleDecrypt(string text)
	{
		SymmetricAlgorithm symmetricAlgorithm = DES.Create();
		ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateDecryptor(Misc.encryptKey, Misc.encryptIv);
		byte[] array = Convert.FromBase64String(text);
		byte[] array2 = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
		return Encoding.Unicode.GetString(array2);
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x000B581A File Offset: 0x000B3C1A
	public static bool ContainsCaseInsensitive(string s, string sFind)
	{
		return s != null && sFind != null && s.IndexOf(sFind, StringComparison.InvariantCultureIgnoreCase) >= 0;
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x000B583C File Offset: 0x000B3C3C
	public static string GetVector3ToSpaceSeparatedString(Vector3 v)
	{
		return string.Concat(new object[] { v.x, " ", v.y, " ", v.z });
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x000B5894 File Offset: 0x000B3C94
	public static Vector3? GetSpaceSeparatedStringToVector3(string s, float max)
	{
		Vector3? vector = null;
		string[] array = Misc.Split(s, " ", StringSplitOptions.RemoveEmptyEntries);
		float num;
		float num2;
		float num3;
		if (array.Length == 3 && float.TryParse(array[0], out num) && float.TryParse(array[1], out num2) && float.TryParse(array[2], out num3))
		{
			vector = new Vector3?(new Vector3(Mathf.Clamp(num, -max, max), Mathf.Clamp(num2, -max, max), Mathf.Clamp(num3, -max, max)));
		}
		return vector;
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x000B5917 File Offset: 0x000B3D17
	public static float ClampMin(float value, float min)
	{
		if (value < min)
		{
			value = min;
		}
		return value;
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x000B5924 File Offset: 0x000B3D24
	public static float ClampMax(float value, float max)
	{
		if (value > max)
		{
			value = max;
		}
		return value;
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x000B5934 File Offset: 0x000B3D34
	public static string GetCompressedStringFromFloat(float value, int digits = 2, bool keepDot = false)
	{
		string text = value.ToString("F" + digits);
		text = text.TrimStart(new char[] { '0' });
		text = text.TrimEnd(new char[] { '0' });
		if (!keepDot && text.Length >= 2)
		{
			text = text.TrimEnd(new char[] { '.' });
		}
		return text;
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x000B59A4 File Offset: 0x000B3DA4
	public static float GetDecompressedFloatFromString(string s)
	{
		float num = 0f;
		if (s != null)
		{
			if (s[0] == '.')
			{
				s = "0" + s;
			}
			float num2;
			if (float.TryParse(s, out num2))
			{
				num = num2;
			}
		}
		return num;
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x000B59E8 File Offset: 0x000B3DE8
	public static void DestroyMultiple(params GameObject[] gameObjects)
	{
		for (int i = 0; i < gameObjects.Length; i++)
		{
			global::UnityEngine.Object.Destroy(gameObjects[i]);
		}
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x000B5A14 File Offset: 0x000B3E14
	public static void DestroyMultiple(params TextMesh[] textMeshes)
	{
		for (int i = 0; i < textMeshes.Length; i++)
		{
			global::UnityEngine.Object.Destroy(textMeshes[i]);
		}
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x000B5A40 File Offset: 0x000B3E40
	public static void ShuffleArray<T>(T[] arr)
	{
		for (int i = arr.Length - 1; i > 0; i--)
		{
			int num = global::UnityEngine.Random.Range(0, i);
			T t = arr[i];
			arr[i] = arr[num];
			arr[num] = t;
		}
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x000B5A89 File Offset: 0x000B3E89
	public static float AdjustPitchInOctaves(float octaveChange)
	{
		return Mathf.Pow(1.05946f, 12f * octaveChange);
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x000B5A9C File Offset: 0x000B3E9C
	public static string CamelCaseToSpaceSeparated(string str)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(str[0]);
		for (int i = 1; i < str.Length; i++)
		{
			if (char.IsUpper(str[i]))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(str[i]);
		}
		return stringBuilder.ToString().ToLower();
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x000B5B08 File Offset: 0x000B3F08
	public static string GetImgurImageUrl(string s)
	{
		string text = null;
		if (!string.IsNullOrEmpty(s))
		{
			s = s.Replace("https://", "http://");
			if (s.StartsWith("http://i.imgur.com/") && (s.EndsWith(".png") || s.EndsWith(".jpg")))
			{
				text = s;
			}
		}
		return text;
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x000B5B67 File Offset: 0x000B3F67
	public static Side GetOppositeSide(Side side)
	{
		return (side != Side.Left) ? Side.Left : Side.Right;
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x000B5B78 File Offset: 0x000B3F78
	public static void SetAllObjectLayers(GameObject thisObject, string layerName)
	{
		int num = LayerMask.NameToLayer(layerName);
		Component[] componentsInChildren = thisObject.GetComponentsInChildren(typeof(Transform), true);
		foreach (Component component in componentsInChildren)
		{
			component.gameObject.layer = num;
		}
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x000B5BC9 File Offset: 0x000B3FC9
	public static string BoolAsCheckmarkOrCross(bool thisBool)
	{
		return (!thisBool) ? "✖" : "✔";
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x000B5BE0 File Offset: 0x000B3FE0
	public static string BoolAsCheckmarkOrCross(bool? thisBool)
	{
		return Misc.BoolAsCheckmarkOrCross(thisBool == true);
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x000B5BFB File Offset: 0x000B3FFB
	public static string BoolAsYesNo(bool thisBool)
	{
		return (!thisBool) ? "No" : "Yes";
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x000B5C12 File Offset: 0x000B4012
	public static string BoolAsYesNo(bool? thisBool)
	{
		return Misc.BoolAsYesNo(thisBool == true);
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x000B5C2D File Offset: 0x000B402D
	public static int GetHourIn12HourFormat(int hour)
	{
		return (hour != 0 && hour != 12 && hour != 24) ? (hour % 12) : 12;
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x000B5C50 File Offset: 0x000B4050
	public static Vector3 ClampVector3(Vector3 v, float min, float max)
	{
		return new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x000B5C81 File Offset: 0x000B4081
	public static void OpenWindowsExplorerAtPath(string path)
	{
		path = path.Replace("/", "\\");
		Process.Start("explorer.exe", "/select," + path);
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x000B5CAC File Offset: 0x000B40AC
	public static string ReplaceCaseInsensitive(this string str, string oldValue, string newValue)
	{
		string text = str;
		int num;
		for (int i = text.IndexOf(oldValue, StringComparison.InvariantCultureIgnoreCase); i > -1; i = text.IndexOf(oldValue, num, StringComparison.InvariantCultureIgnoreCase))
		{
			text = text.Remove(i, oldValue.Length);
			text = text.Insert(i, newValue);
			num = i + newValue.Length;
		}
		return text;
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x000B5CFD File Offset: 0x000B40FD
	public static void SetLocalPositionX(GameObject thisGameObject, float value)
	{
		Misc.SetLocalPositionX(thisGameObject.transform, value);
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x000B5D0B File Offset: 0x000B410B
	public static void SetLocalPositionY(GameObject thisGameObject, float value)
	{
		Misc.SetLocalPositionY(thisGameObject.transform, value);
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x000B5D19 File Offset: 0x000B4119
	public static void SetLocalPositionZ(GameObject thisGameObject, float value)
	{
		Misc.SetLocalPositionZ(thisGameObject.transform, value);
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x000B5D28 File Offset: 0x000B4128
	public static void SetLocalPositionX(Transform thisTransform, float value)
	{
		Vector3 localPosition = thisTransform.localPosition;
		localPosition.x = value;
		thisTransform.localPosition = localPosition;
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x000B5D4C File Offset: 0x000B414C
	public static void SetLocalPositionY(Transform thisTransform, float value)
	{
		Vector3 localPosition = thisTransform.localPosition;
		localPosition.y = value;
		thisTransform.localPosition = localPosition;
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x000B5D70 File Offset: 0x000B4170
	public static void SetLocalPositionZ(Transform thisTransform, float value)
	{
		Vector3 localPosition = thisTransform.localPosition;
		localPosition.z = value;
		thisTransform.localPosition = localPosition;
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x000B5D93 File Offset: 0x000B4193
	public static string RemoveCloneFromName(string gameObjectName)
	{
		return gameObjectName.Replace("(Clone)", string.Empty);
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x000B5DA5 File Offset: 0x000B41A5
	public static string RemoveCloneFromName(GameObject gameObject)
	{
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
		return gameObject.name;
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x000B5DC8 File Offset: 0x000B41C8
	public static string RemoveCloneFromName(Transform transform)
	{
		transform.name = transform.name.Replace("(Clone)", string.Empty);
		return transform.name;
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x000B5DEB File Offset: 0x000B41EB
	public static bool ColorsAreSame(Color c1, Color c2)
	{
		return c1.r == c2.r && c1.g == c2.g && c1.b == c2.b;
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x000B5E28 File Offset: 0x000B4228
	public static int GetThisCharInStringCount(string text, char c)
	{
		int num = 0;
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i].Equals(c))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x000B5E6C File Offset: 0x000B426C
	public static long GetDirectorySizeInBytes(string path, string extension = "*")
	{
		long num = 0L;
		if (Directory.Exists(path))
		{
			string[] files = Directory.GetFiles(path, "*." + extension);
			foreach (string text in files)
			{
				FileInfo fileInfo = new FileInfo(text);
				num += fileInfo.Length;
			}
		}
		return num;
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x000B5ECB File Offset: 0x000B42CB
	public static void Destroy(GameObject thisObject)
	{
		thisObject.tag = "Untagged";
		thisObject.name = Universe.objectNameIfAlreadyDestroyed;
		global::UnityEngine.Object.Destroy(thisObject);
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x000B5EE9 File Offset: 0x000B42E9
	public static bool IsDestroyed(GameObject thisObject)
	{
		return thisObject == null || thisObject.name == Universe.objectNameIfAlreadyDestroyed;
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x000B5F0C File Offset: 0x000B430C
	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 vector = point - pivot;
		vector = Quaternion.Euler(angles) * vector;
		point = vector + pivot;
		return point;
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x000B5F38 File Offset: 0x000B4338
	public static string ReplaceRepeated(string s, string sToFind, string sToReplace)
	{
		string text = string.Empty;
		while (s != text)
		{
			text = s;
			s = s.Replace(sToFind, sToReplace);
		}
		return s;
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x000B5F69 File Offset: 0x000B4369
	public static bool CtrlIsPressed()
	{
		return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x000B5F87 File Offset: 0x000B4387
	public static bool AltIsPressed()
	{
		return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x000B5FA8 File Offset: 0x000B43A8
	public static Color ColorStringToColor(string colorString)
	{
		Color color = Color.white;
		if (!string.IsNullOrEmpty(colorString))
		{
			string[] array = Misc.Split(colorString, ",", StringSplitOptions.RemoveEmptyEntries);
			int num = 255;
			int num2 = array.Length;
			if (num2 == 4)
			{
				num = int.Parse(array[3]);
			}
			if (num2 >= 3 && num2 <= 4)
			{
				color = new Color32((byte)int.Parse(array[0]), (byte)int.Parse(array[1]), (byte)int.Parse(array[2]), (byte)num);
			}
		}
		return color;
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x000B6024 File Offset: 0x000B4424
	public static string GetHowLongAgoText(string dateTimeString)
	{
		string text = string.Empty;
		DateTime dateTime = Convert.ToDateTime(dateTimeString);
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
		float num = Mathf.Round((float)Math.Abs(timeSpan.TotalSeconds));
		text = num + " " + Misc.GetPluralOrSingular("second", num);
		if (num >= 60f)
		{
			float num2 = Mathf.Floor(num / 60f);
			text = num2 + " " + Misc.GetPluralOrSingular("minute", num2);
			if (num2 >= 60f)
			{
				float num3 = Mathf.Floor(num2 / 60f);
				text = num3 + " " + Misc.GetPluralOrSingular("hour", num3);
				if (num3 >= 24f)
				{
					float num4 = Mathf.Floor(num3 / 24f);
					text = num4 + " " + Misc.GetPluralOrSingular("day", num4);
					if (num4 >= 30.44f)
					{
						float num5 = Mathf.Floor(num4 / 30.44f);
						text = num5 + " " + Misc.GetPluralOrSingular("month", num5);
						if (num5 >= 12f)
						{
							float num6 = Mathf.Floor(num5 / 12f);
							text = num6 + " " + Misc.GetPluralOrSingular("year", num6);
						}
					}
				}
			}
		}
		return text + " ago";
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x000B61B6 File Offset: 0x000B45B6
	public static string GetPluralOrSingular(string word, float amount)
	{
		return (amount != 1f) ? (word + 's') : word;
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x000B61D8 File Offset: 0x000B45D8
	public static string HtmlEncode(string s)
	{
		s = s.Replace("&", "&amp;");
		s = s.Replace("\"", "&quot;");
		s = s.Replace("'", "&#39;");
		s = s.Replace("<", "&lt;");
		s = s.Replace(">", "&gt;");
		return s;
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x000B6240 File Offset: 0x000B4640
	public static Vector3 AddToAllVectorValues(Vector3 vector, float value)
	{
		vector.x += value;
		vector.y += value;
		vector.z += value;
		return vector;
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x000B6270 File Offset: 0x000B4670
	public static string GetHowManyDaysAgo(string dateString)
	{
		DateTime dateTime = DateTime.Parse(dateString);
		double totalDays = DateTime.Now.Subtract(dateTime).TotalDays;
		return Mathf.Round((float)totalDays).ToString();
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x000B62B4 File Offset: 0x000B46B4
	public static string GetDateStringXYearsInFuture(int x)
	{
		DateTime now = DateTime.Now;
		now.AddYears(x);
		return now.ToString();
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x000B62E0 File Offset: 0x000B46E0
	public static string[] Split(string text, string stringToUseForSplitting = " ", StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
	{
		if (text != null && text != string.Empty && text.IndexOf(stringToUseForSplitting) >= 0)
		{
			string[] array = new string[] { stringToUseForSplitting };
			return text.Split(array, options);
		}
		return new string[] { text };
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x000B632E File Offset: 0x000B472E
	public static int CapInt(int numberToCap, int maxToCapTo)
	{
		if (numberToCap > maxToCapTo)
		{
			numberToCap = maxToCapTo;
		}
		return numberToCap;
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x000B633C File Offset: 0x000B473C
	public static void DeleteCookiesFileIfExists()
	{
		string text = Misc.GetAppRootPath() + "cookies.dat";
		if (File.Exists(text))
		{
			File.Delete(text);
		}
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x000B636C File Offset: 0x000B476C
	public static string GetAppRootPath()
	{
		string text = Application.dataPath;
		if (Application.platform == RuntimePlatform.OSXPlayer)
		{
			text += "/../../";
		}
		else
		{
			text += "/../";
		}
		return text;
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x000B63A8 File Offset: 0x000B47A8
	public static DateTime DeserializeDateTime(string stringDateTimeUtc)
	{
		DateTime dateTime = DateTime.Parse(stringDateTimeUtc);
		return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x000B63C5 File Offset: 0x000B47C5
	public static string SerializeDateTime(DateTime dateTimeUtc)
	{
		dateTimeUtc = DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);
		return dateTimeUtc.ToString("yyyy-MM-dd HH:mm:ss");
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x000B63DC File Offset: 0x000B47DC
	public static string WrapWithNewlines(string input, int rowLength, int maxLines = -1)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		Stack<string> stack = new Stack<string>(Misc.ReverseString(input).Split(new char[] { ' ' }));
		int num = 0;
		while (stack.Count > 0)
		{
			string text = stack.Pop();
			if (text.Length > rowLength)
			{
				string text2 = text.Substring(0, rowLength);
				string text3 = text.Substring(rowLength);
				text = text2;
				stack.Push(text3);
			}
			if (stringBuilder2.Length + text.Length > rowLength)
			{
				if (maxLines != -1 && num >= maxLines - 1)
				{
					stringBuilder.Append(stringBuilder2);
					return stringBuilder.ToString() + "..";
				}
				stringBuilder.AppendLine(stringBuilder2.ToString());
				num++;
				stringBuilder2 = new StringBuilder();
			}
			stringBuilder2.Append(text + " ");
		}
		stringBuilder.Append(stringBuilder2);
		return stringBuilder.ToString();
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x000B64D4 File Offset: 0x000B48D4
	private static string ReverseString(string str)
	{
		int num = 0;
		string text = string.Empty;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == ' ')
			{
				text = " " + text;
				num = 0;
			}
			else
			{
				text = text.Insert(num, str[i].ToString());
				num++;
			}
		}
		return text;
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x000B6543 File Offset: 0x000B4943
	public static string GetNumberlessString(string s)
	{
		return Regex.Replace(s, "[\\d-]", string.Empty);
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x000B6558 File Offset: 0x000B4958
	public static string HtmlDecode(string s)
	{
		s = s.Replace("&gt;", ">");
		s = s.Replace("&lt;", "<");
		s = s.Replace("&quot;", "\"");
		s = s.Replace("&#39;", "'");
		s = s.Replace("&amp;", "&");
		return s;
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x000B65C0 File Offset: 0x000B49C0
	public static List<string> GetTextsBetween(string source, string start, string end)
	{
		List<string> list = new List<string>();
		string text = string.Format("{0}({1}){2}", Regex.Escape(start), ".+?", Regex.Escape(end));
		IEnumerator enumerator = Regex.Matches(source, text).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Match match = (Match)obj;
				list.Add(match.Groups[1].Value);
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
		return list;
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x000B665C File Offset: 0x000B4A5C
	public static string GetTextBetween(string source, string start, string end)
	{
		string text = string.Format("{0}({1}){2}", Regex.Escape(start), ".+?", Regex.Escape(end));
		IEnumerator enumerator = Regex.Matches(source, text).GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				Match match = (Match)enumerator.Current;
				return match.Groups[1].Value;
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
		return null;
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x000B66F4 File Offset: 0x000B4AF4
	public static int GetPercentInt(int max, int value)
	{
		return Mathf.RoundToInt((float)value / (float)max * 100f);
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x000B6706 File Offset: 0x000B4B06
	public static Color GetGray(float brightness)
	{
		return new Color(brightness, brightness, brightness);
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x000B6710 File Offset: 0x000B4B10
	public static Vector3 GetUniformVector3(float value)
	{
		return new Vector3(value, value, value);
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x000B671C File Offset: 0x000B4B1C
	public static float GetLargestValueOfVector(Vector3 vector)
	{
		float num = vector.x;
		if (vector.y > num)
		{
			num = vector.y;
		}
		if (vector.z > num)
		{
			num = vector.z;
		}
		return num;
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x000B675C File Offset: 0x000B4B5C
	public static float GetSmallestValueOfVector(Vector3 vector)
	{
		float num = vector.x;
		if (vector.y < num)
		{
			num = vector.y;
		}
		if (vector.z < num)
		{
			num = vector.z;
		}
		return num;
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x000B679C File Offset: 0x000B4B9C
	public static string[] GetStringInParts(this string text, int partLength, int maxParts = -1)
	{
		int charCount = 0;
		string[] array = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = (from w in array
			group w by (charCount += ((charCount % partLength + w.Length + 1 < partLength) ? 0 : (partLength - charCount % partLength)) + w.Length + 1) / partLength into g
			select string.Join(" ", g.ToArray<string>())).ToArray<string>();
		if (maxParts != -1)
		{
			if (array2.Length > maxParts)
			{
				array2[maxParts - 1] = Misc.Truncate(array2[maxParts - 1] + "..", partLength, true);
			}
			Array.Resize<string>(ref array2, maxParts);
		}
		return array2;
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x000B6843 File Offset: 0x000B4C43
	public static Hand GetAHandOfOurs()
	{
		return (!(Managers.personManager != null) || !(Managers.personManager.ourPerson != null)) ? null : Managers.personManager.ourPerson.GetAHand();
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x000B6880 File Offset: 0x000B4C80
	public static string GetRandomId()
	{
		return ObjectId.GenerateNewId().ToString();
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x000B68A0 File Offset: 0x000B4CA0
	public static bool Chance(float percent = 50f)
	{
		return (float)global::UnityEngine.Random.Range(0, 100) <= percent;
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x000B68B4 File Offset: 0x000B4CB4
	public static GameObject[] GetChildrenAsArray(Transform transform)
	{
		GameObject[] array = new GameObject[transform.childCount];
		int num = 0;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform2 = (Transform)obj;
				array[num++] = transform2.gameObject;
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
		return array;
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x000B692C File Offset: 0x000B4D2C
	public static string Truncate(string text, int maxLength = 20, bool addDots = true)
	{
		if (!string.IsNullOrEmpty(text) && text.Length > maxLength)
		{
			if (addDots)
			{
				text = text.Substring(0, maxLength - 2) + "..";
			}
			else
			{
				text = text.Substring(0, maxLength);
			}
		}
		return text;
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x000B697C File Offset: 0x000B4D7C
	public static string TruncateRightAligned(string text, int maxLength = 28, string prefix = "..")
	{
		if (text.Length > maxLength)
		{
			int num = text.Length - maxLength;
			text = prefix + text.Substring(num, text.Length - num);
		}
		return text;
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x000B69B8 File Offset: 0x000B4DB8
	public static string ReplaceAll(string s, string sFind, string sReplace)
	{
		string text = null;
		while (text != s)
		{
			text = s;
			s = s.Replace(sFind, sReplace);
		}
		return s;
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x000B69E8 File Offset: 0x000B4DE8
	public static GameObject FindObject(GameObject parentObject, string name)
	{
		if (parentObject == null)
		{
			GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (GameObject gameObject in rootGameObjects)
			{
				if (gameObject.name == name)
				{
					return gameObject;
				}
			}
		}
		else
		{
			Component[] componentsInChildren = parentObject.GetComponentsInChildren(typeof(Transform), true);
			foreach (Transform transform in componentsInChildren)
			{
				if (transform.name == name)
				{
					return transform.gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x000B6AA0 File Offset: 0x000B4EA0
	public static bool HasChildWithTag(Transform transform, string tag)
	{
		bool flag = false;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform2 = (Transform)obj;
				if (transform2.CompareTag(tag))
				{
					flag = true;
					break;
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
		return flag;
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x000B6B10 File Offset: 0x000B4F10
	public static GameObject GetChildWithTag(Transform transform, string tag)
	{
		GameObject gameObject = null;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform2 = (Transform)obj;
				if (transform2.CompareTag(tag))
				{
					gameObject = transform2.gameObject;
					break;
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
		return gameObject;
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x000B6B84 File Offset: 0x000B4F84
	public static string BoolToYesNo(bool value)
	{
		return (!value) ? "No" : "Yes";
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x000B6B9C File Offset: 0x000B4F9C
	public static bool CommandLineArgsContain(string value)
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		return Array.IndexOf<string>(commandLineArgs, value) > -1;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x000B6BB9 File Offset: 0x000B4FB9
	public static bool ShouldBypassAuth()
	{
		return Misc.CommandLineArgsContain("bypass");
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x000B6BC5 File Offset: 0x000B4FC5
	public static bool ShouldDisableVR()
	{
		return Misc.CommandLineArgsContain("novr");
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x000B6BD4 File Offset: 0x000B4FD4
	public static void AssertAllNotNull(object[] objects, string message)
	{
		for (int i = 0; i < objects.Length; i++)
		{
		}
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x000B6BF5 File Offset: 0x000B4FF5
	public static bool IsValidObjectIdString(string objectIdString)
	{
		return objectIdString != string.Empty && objectIdString.Length == 24;
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x000B6C14 File Offset: 0x000B5014
	public static int getResponseCode(WWW request)
	{
		int num = 0;
		if (request.responseHeaders == null)
		{
			Log.Error("getResponseCode - no response headers.");
		}
		else if (!request.responseHeaders.ContainsKey("STATUS"))
		{
			Log.Error("getResponseCode - response headers has no STATUS.");
		}
		else
		{
			num = Misc.parseResponseCode(request.responseHeaders["STATUS"]);
		}
		return num;
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x000B6C78 File Offset: 0x000B5078
	public static bool isServerDown(int httpResponseCode)
	{
		bool flag = false;
		if (httpResponseCode == 302 || httpResponseCode == 303)
		{
			flag = true;
		}
		if (httpResponseCode >= 500 && httpResponseCode < 600)
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x000B6CB8 File Offset: 0x000B50B8
	public static bool wwwObjectHasStatusHeader(WWW req)
	{
		return req.responseHeaders != null && req.responseHeaders.ContainsKey("STATUS");
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x000B6CD8 File Offset: 0x000B50D8
	public static int parseResponseCode(string statusLine)
	{
		int num = 0;
		string[] array = statusLine.Split(new char[] { ' ' });
		if (array.Length < 3)
		{
			Log.Error("invalid response status: " + statusLine);
		}
		else if (!int.TryParse(array[1], out num))
		{
			Log.Error("invalid response code: " + array[1]);
		}
		return num;
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x000B6D39 File Offset: 0x000B5139
	public static void LogOff()
	{
		Managers.serverManager.CancelAuthentication();
		Managers.thingManager.UnloadPlacements();
		Managers.broadcastNetworkManager.Disconnect();
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x000B6D5C File Offset: 0x000B515C
	public static bool ForceUpdateIsNeeded(string versionMajorToEnforceUpdates_serverString)
	{
		bool flag = false;
		int num;
		if (!string.IsNullOrEmpty(versionMajorToEnforceUpdates_serverString) && int.TryParse(versionMajorToEnforceUpdates_serverString, out num))
		{
			Universe.versionMajorAsToldByServer = versionMajorToEnforceUpdates_serverString;
			flag = num > Universe.versionMajorToEnforceUpdates;
		}
		else
		{
			Log.Error("Server's response.currentAppVersion seems to send wrong data, value is: " + versionMajorToEnforceUpdates_serverString);
		}
		return flag;
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x000B6DA8 File Offset: 0x000B51A8
	public static void ForceUpdate()
	{
		string text = string.Concat(new string[]
		{
			"Please update to the latest version of Anyland and start again (there were some changes requiring this). Thanks! If there's any questions, please check out the Steam forum or email us at we@manyland.com \n\n[Your version: ",
			Universe.GetClientVersionDisplay(),
			" | Server version: ",
			Universe.versionMajorAsToldByServer,
			"]"
		});
		Managers.errorManager.ShowCriticalHaltError(text, false, true, false, false, false);
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x000B6DFC File Offset: 0x000B51FC
	public static string CreateJsonSuperDocument(Dictionary<string, string> jsonNameValuePairs)
	{
		string text = "{";
		bool flag = true;
		foreach (KeyValuePair<string, string> keyValuePair in jsonNameValuePairs)
		{
			if (!flag)
			{
				text += ",";
				flag = false;
			}
			string text2 = text;
			text = string.Concat(new string[] { text2, "\"", keyValuePair.Key, "\":", keyValuePair.Value });
		}
		text += "}";
		return text;
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x000B6EB0 File Offset: 0x000B52B0
	public static void RestartApp()
	{
		string text = Application.dataPath.Replace("_Data", ".exe");
		Process.Start(text);
		Application.Quit();
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x000B6EDE File Offset: 0x000B52DE
	public static void ExitApp()
	{
		Application.Quit();
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x000B6EE5 File Offset: 0x000B52E5
	public static string SerializeStringList(List<string> list)
	{
		return "[" + string.Join(",", list.ToArray()) + "]";
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x000B6F08 File Offset: 0x000B5308
	// Note: this type is marked as 'beforefieldinit'.
	static Misc()
	{
		bool[] array = new bool[2];
		array[0] = true;
		Misc.trueFalse = array;
		Misc.encryptKey = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
		Misc.encryptIv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
	}

	// Token: 0x04001227 RID: 4647
	public const int adminInfoCount = 10;

	// Token: 0x04001228 RID: 4648
	public static string[] info = new string[10];

	// Token: 0x04001229 RID: 4649
	public static bool[] trueFalse;

	// Token: 0x0400122A RID: 4650
	private static byte[] encryptKey;

	// Token: 0x0400122B RID: 4651
	private static byte[] encryptIv;
}
