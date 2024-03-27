using System;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public sealed class Mathfx
{
	// Token: 0x06000D88 RID: 3464 RVA: 0x000788CD File Offset: 0x00076CCD
	public static float EaseInOut(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value * value * (3f - 2f * value));
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x000788E7 File Offset: 0x00076CE7
	public static Vector2 EaseInOut(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Mathfx.EaseInOut(start.x, end.x, value), Mathfx.EaseInOut(start.y, end.y, value));
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x00078918 File Offset: 0x00076D18
	public static Vector3 EaseInOut(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Mathfx.EaseInOut(start.x, end.x, value), Mathfx.EaseInOut(start.y, end.y, value), Mathfx.EaseInOut(start.z, end.z, value));
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x00078966 File Offset: 0x00076D66
	public static float EaseOut(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(value * 3.1415927f * 0.5f));
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x00078984 File Offset: 0x00076D84
	public static Vector2 EaseOut(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * 3.1415927f * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * 3.1415927f * 0.5f)));
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x000789E0 File Offset: 0x00076DE0
	public static Vector3 EaseOut(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * 3.1415927f * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * 3.1415927f * 0.5f)), Mathf.Lerp(start.z, end.z, Mathf.Sin(value * 3.1415927f * 0.5f)));
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x00078A61 File Offset: 0x00076E61
	public static float EaseIn(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(value * 3.1415927f * 0.5f));
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x00078A82 File Offset: 0x00076E82
	public static Vector2 EaseIn(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Mathfx.EaseIn(start.x, end.x, value), Mathfx.EaseIn(start.y, end.y, value));
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00078AB4 File Offset: 0x00076EB4
	public static Vector3 EaseIn(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Mathfx.EaseIn(start.x, end.x, value), Mathfx.EaseIn(start.y, end.y, value), Mathfx.EaseIn(start.z, end.z, value));
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00078B04 File Offset: 0x00076F04
	public static float Berp(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x00078B68 File Offset: 0x00076F68
	public static Vector2 Berp(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Mathfx.Berp(start.x, end.x, value), Mathfx.Berp(start.y, end.y, value));
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x00078B98 File Offset: 0x00076F98
	public static Vector3 Berp(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Mathfx.Berp(start.x, end.x, value), Mathfx.Berp(start.y, end.y, value), Mathfx.Berp(start.z, end.z, value));
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x00078BE8 File Offset: 0x00076FE8
	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float num = (x - min) / (max - min);
		float num2 = (x - min) / (max - min);
		return -2f * num * num * num + 3f * num2 * num2;
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x00078C24 File Offset: 0x00077024
	public static Vector2 SmoothStep(Vector2 vec, float min, float max)
	{
		return new Vector2(Mathfx.SmoothStep(vec.x, min, max), Mathfx.SmoothStep(vec.y, min, max));
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x00078C47 File Offset: 0x00077047
	public static Vector3 SmoothStep(Vector3 vec, float min, float max)
	{
		return new Vector3(Mathfx.SmoothStep(vec.x, min, max), Mathfx.SmoothStep(vec.y, min, max), Mathfx.SmoothStep(vec.z, min, max));
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x00078C78 File Offset: 0x00077078
	public static float Lerp(float start, float end, float value)
	{
		return (1f - value) * start + value * end;
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x00078C88 File Offset: 0x00077088
	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = Vector3.Normalize(lineEnd - lineStart);
		float num = Vector3.Dot(point - lineStart, vector);
		return lineStart + num * vector;
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x00078CC0 File Offset: 0x000770C0
	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = lineEnd - lineStart;
		Vector3 vector2 = Vector3.Normalize(vector);
		float num = Vector3.Dot(point - lineStart, vector2);
		return lineStart + Mathf.Clamp(num, 0f, Vector3.Magnitude(vector)) * vector2;
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x00078D07 File Offset: 0x00077107
	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x00078D30 File Offset: 0x00077130
	public static Vector2 Bounce(Vector2 vec)
	{
		return new Vector2(Mathfx.Bounce(vec.x), Mathfx.Bounce(vec.y));
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x00078D4F File Offset: 0x0007714F
	public static Vector3 Bounce(Vector3 vec)
	{
		return new Vector3(Mathfx.Bounce(vec.x), Mathfx.Bounce(vec.y), Mathfx.Bounce(vec.z));
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x00078D7A File Offset: 0x0007717A
	public static bool Approx(float val, float about, float range)
	{
		return Mathf.Abs(val - about) < range;
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x00078D88 File Offset: 0x00077188
	public static bool Approx(Vector3 val, Vector3 about, float range)
	{
		return (val - about).sqrMagnitude < range * range;
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x00078DAC File Offset: 0x000771AC
	public static float Clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num5;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			num5 = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			num5 = start + num4;
		}
		else
		{
			num5 = start + (end - start) * value;
		}
		return num5;
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x00078E24 File Offset: 0x00077224
	public static float LerpWithOvershoot(float a, float b, float t)
	{
		return t * b + (1f - t) * a;
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x00078E34 File Offset: 0x00077234
	public static Vector3 LerpWithOvershoot(Vector3 a, Vector3 b, float t)
	{
		return new Vector3(Mathfx.LerpWithOvershoot(a.x, b.x, t), Mathfx.LerpWithOvershoot(a.y, b.y, t), Mathfx.LerpWithOvershoot(a.z, b.z, t));
	}
}
