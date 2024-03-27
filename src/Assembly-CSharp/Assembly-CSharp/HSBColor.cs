using System;
using UnityEngine;

// Token: 0x020001B6 RID: 438
[Serializable]
public struct HSBColor
{
	// Token: 0x06000D6D RID: 3437 RVA: 0x00077EDB File Offset: 0x000762DB
	public HSBColor(float h, float s, float b, float a)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = a;
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x00077EFA File Offset: 0x000762FA
	public HSBColor(float h, float s, float b)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = 1f;
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x00077F1C File Offset: 0x0007631C
	public HSBColor(Color col)
	{
		HSBColor hsbcolor = HSBColor.FromColor(col);
		this.h = hsbcolor.h;
		this.s = hsbcolor.s;
		this.b = hsbcolor.b;
		this.a = hsbcolor.a;
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x00077F64 File Offset: 0x00076364
	public static HSBColor FromColor(Color color)
	{
		HSBColor hsbcolor = new HSBColor(0f, 0f, 0f, color.a);
		float r = color.r;
		float g = color.g;
		float num = color.b;
		float num2 = Mathf.Max(r, Mathf.Max(g, num));
		if (num2 <= 0f)
		{
			return hsbcolor;
		}
		float num3 = Mathf.Min(r, Mathf.Min(g, num));
		float num4 = num2 - num3;
		if (num2 > num3)
		{
			if (g == num2)
			{
				hsbcolor.h = (num - r) / num4 * 60f + 120f;
			}
			else if (num == num2)
			{
				hsbcolor.h = (r - g) / num4 * 60f + 240f;
			}
			else if (num > g)
			{
				hsbcolor.h = (g - num) / num4 * 60f + 360f;
			}
			else
			{
				hsbcolor.h = (g - num) / num4 * 60f;
			}
			if (hsbcolor.h < 0f)
			{
				hsbcolor.h += 360f;
			}
		}
		else
		{
			hsbcolor.h = 0f;
		}
		hsbcolor.h *= 0.0027777778f;
		hsbcolor.s = num4 / num2 * 1f;
		hsbcolor.b = num2;
		return hsbcolor;
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x000780CC File Offset: 0x000764CC
	public static Color ToColor(HSBColor hsbColor)
	{
		float num = hsbColor.b;
		float num2 = hsbColor.b;
		float num3 = hsbColor.b;
		if (hsbColor.s != 0f)
		{
			float num4 = hsbColor.b;
			float num5 = hsbColor.b * hsbColor.s;
			float num6 = hsbColor.b - num5;
			float num7 = hsbColor.h * 360f;
			if (num7 < 60f)
			{
				num = num4;
				num2 = num7 * num5 / 60f + num6;
				num3 = num6;
			}
			else if (num7 < 120f)
			{
				num = -(num7 - 120f) * num5 / 60f + num6;
				num2 = num4;
				num3 = num6;
			}
			else if (num7 < 180f)
			{
				num = num6;
				num2 = num4;
				num3 = (num7 - 120f) * num5 / 60f + num6;
			}
			else if (num7 < 240f)
			{
				num = num6;
				num2 = -(num7 - 240f) * num5 / 60f + num6;
				num3 = num4;
			}
			else if (num7 < 300f)
			{
				num = (num7 - 240f) * num5 / 60f + num6;
				num2 = num6;
				num3 = num4;
			}
			else if (num7 <= 360f)
			{
				num = num4;
				num2 = num6;
				num3 = -(num7 - 360f) * num5 / 60f + num6;
			}
			else
			{
				num = 0f;
				num2 = 0f;
				num3 = 0f;
			}
		}
		return new Color(Mathf.Clamp01(num), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x00078268 File Offset: 0x00076668
	public void ClampToValid()
	{
		this.h = Mathf.Clamp(this.h, 0f, 1f);
		this.s = Mathf.Clamp(this.s, 0f, 1f);
		this.b = Mathf.Clamp(this.b, 0f, 1f);
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x000782C6 File Offset: 0x000766C6
	public Color ToColor()
	{
		return HSBColor.ToColor(this);
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x000782D4 File Offset: 0x000766D4
	public override string ToString()
	{
		return string.Concat(new object[] { "H:", this.h, " S:", this.s, " B:", this.b });
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x00078330 File Offset: 0x00076730
	public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
	{
		float num;
		float num2;
		if (a.b == 0f)
		{
			num = b.h;
			num2 = b.s;
		}
		else if (b.b == 0f)
		{
			num = a.h;
			num2 = a.s;
		}
		else
		{
			if (a.s == 0f)
			{
				num = b.h;
			}
			else if (b.s == 0f)
			{
				num = a.h;
			}
			else
			{
				float num3;
				for (num3 = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t); num3 < 0f; num3 += 360f)
				{
				}
				while (num3 > 360f)
				{
					num3 -= 360f;
				}
				num = num3 / 360f;
			}
			num2 = Mathf.Lerp(a.s, b.s, t);
		}
		return new HSBColor(num, num2, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
	}

	// Token: 0x04000EF9 RID: 3833
	public float h;

	// Token: 0x04000EFA RID: 3834
	public float s;

	// Token: 0x04000EFB RID: 3835
	public float b;

	// Token: 0x04000EFC RID: 3836
	public float a;
}
