using System;
using UnityEngine;

// Token: 0x0200020B RID: 523
public static class SkyDriverHelper
{
	// Token: 0x060015B3 RID: 5555 RVA: 0x000BF488 File Offset: 0x000BD888
	public static void CalculateTurbidity(ref Vector3[] datas, float T)
	{
		datas[0].x = -0.0193f * T - 0.2592f;
		datas[1].x = -0.0665f * T + 0.0008f;
		datas[2].x = -0.0004f * T + 0.2125f;
		datas[3].x = -0.0641f * T - 0.8989f;
		datas[4].x = -0.0033f * T + 0.0452f;
		datas[0].y = -0.0167f * T - 0.2608f;
		datas[1].y = -0.095f * T + 0.0092f;
		datas[2].y = -0.0079f * T + 0.2102f;
		datas[3].y = -0.0441f * T - 1.6537f;
		datas[4].y = -0.0109f * T + 0.0529f;
		datas[0].z = 0.1787f * T - 1.463f;
		datas[1].z = -0.3554f * T + 0.4275f;
		datas[2].z = -0.0227f * T + 5.3251f;
		datas[3].z = 0.1206f * T - 2.5771f;
		datas[4].z = -0.067f * T + 0.3703f;
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x000BF61C File Offset: 0x000BDA1C
	public static void CalculateZenith(ref Vector3[] datas, float T, float theta, float ZClamp)
	{
		float num = Mathf.Max(theta, ZClamp);
		float num2 = theta * theta;
		float num3 = num2 * theta;
		float num4 = T * T;
		float num5 = (0.44444445f - T / 120f) * (3.1415927f - 2f * num);
		Vector3 vector;
		vector.z = Mathf.Max((4.0453f * T - 4.971f) * Mathf.Tan(num5) - 0.2155f * T + 2.4192f, 0f);
		vector.z *= 0.06f;
		vector.x = (0.00166f * num3 - 0.00375f * num2 + 0.00209f * theta) * num4 + (-0.02903f * num3 + 0.06377f * num2 - 0.03202f * theta + 0.00394f) * T + (0.11693f * num3 - 0.21196f * num2 + 0.06052f * theta + 0.25886f);
		vector.y = (0.00275f * num3 - 0.0061f * num2 + 0.00317f * theta) * num4 + (-0.04214f * num3 + 0.0897f * num2 - 0.04153f * theta + 0.00516f) * T + (0.15346f * num3 - 0.26756f * num2 + 0.0667f * theta + 0.26688f);
		vector.x /= SkyDriverHelper.PerezFunction(datas[0].x, datas[1].x, datas[2].x, datas[3].x, datas[4].x, 0f, theta);
		vector.y /= SkyDriverHelper.PerezFunction(datas[0].y, datas[1].y, datas[2].y, datas[3].y, datas[4].y, 0f, theta);
		vector.z /= SkyDriverHelper.PerezFunction(datas[0].z, datas[1].z, datas[2].z, datas[3].z, datas[4].z, 0f, num);
		datas[5] = vector;
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x000BF87C File Offset: 0x000BDC7C
	public static void CalculateChannels(ref Vector3[] datas)
	{
		datas[7] = new Vector3(3.2404f, -1.5372f, -0.4985f);
		datas[8] = new Vector3(-0.9692f, 1.876f, 0.0415f);
		datas[9] = new Vector3(0.0556f, -0.204f, 1.0573f);
	}

	// Token: 0x060015B6 RID: 5558 RVA: 0x000BF8ED File Offset: 0x000BDCED
	public static void CalculateZenithCoefficient(ref Vector3[] datas)
	{
		datas[6] = Vector3.one;
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x000BF904 File Offset: 0x000BDD04
	public static float PerezFunction(float A, float B, float C, float D, float E, float theta, float gamma)
	{
		float num = Mathf.Cos(gamma);
		return (1f + A * Mathf.Exp(B / Mathf.Cos(theta))) * (1f + C * Mathf.Exp(D * gamma) + E * num * num);
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x000BF948 File Offset: 0x000BDD48
	public static Vector2 GetSunCoordinate(Vector3 dir)
	{
		return new Vector2(Mathf.Acos(dir.z), Mathf.Atan2(dir.x, dir.y));
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x000BF96E File Offset: 0x000BDD6E
	public static Vector3 linearMult(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	// Token: 0x040012CE RID: 4814
	public static readonly Vector3[] defaultDatas = new Vector3[]
	{
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.one * 0.3f,
		Vector3.one,
		Vector3.right,
		Vector3.up,
		Vector3.forward
	};
}
