using System;
using UnityEngine;

// Token: 0x0200020A RID: 522
[ExecuteInEditMode]
public class SkyDriver : MonoBehaviour
{
	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06001593 RID: 5523 RVA: 0x000BED93 File Offset: 0x000BD193
	// (set) Token: 0x06001594 RID: 5524 RVA: 0x000BED9B File Offset: 0x000BD19B
	public Material skybox
	{
		get
		{
			return this.m_skybox;
		}
		set
		{
			if (SkyDriver.SetStruct<Material>(ref this.m_skybox, value))
			{
				this.Start();
			}
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06001595 RID: 5525 RVA: 0x000BEDB4 File Offset: 0x000BD1B4
	// (set) Token: 0x06001596 RID: 5526 RVA: 0x000BEDBC File Offset: 0x000BD1BC
	public int customizationMode
	{
		get
		{
			return this.m_customizationMode;
		}
		set
		{
			if (SkyDriver.SetStruct<int>(ref this.m_customizationMode, value))
			{
				this.Compute();
			}
			this.SaveToMaterial(false);
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06001597 RID: 5527 RVA: 0x000BEDDC File Offset: 0x000BD1DC
	// (set) Token: 0x06001598 RID: 5528 RVA: 0x000BEDE4 File Offset: 0x000BD1E4
	public float turbidity
	{
		get
		{
			return this.m_turbidity;
		}
		set
		{
			if (SkyDriver.SetStruct<float>(ref this.m_turbidity, value))
			{
				this.Compute();
				this.SaveToMaterial(false);
			}
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06001599 RID: 5529 RVA: 0x000BEE04 File Offset: 0x000BD204
	// (set) Token: 0x0600159A RID: 5530 RVA: 0x000BEE0C File Offset: 0x000BD20C
	public float sunSize
	{
		get
		{
			return this.m_sunSize;
		}
		set
		{
			if (SkyDriver.SetStruct<float>(ref this.m_sunSize, value))
			{
				this.Compute();
				this.SaveToMaterial(false);
			}
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x0600159B RID: 5531 RVA: 0x000BEE2C File Offset: 0x000BD22C
	// (set) Token: 0x0600159C RID: 5532 RVA: 0x000BEE34 File Offset: 0x000BD234
	public float exposure
	{
		get
		{
			return this.m_exposure;
		}
		set
		{
			if (SkyDriver.SetStruct<float>(ref this.m_exposure, value))
			{
				this.m_skybox.SetVector("tint", this.m_tint * this.m_exposure);
			}
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x0600159D RID: 5533 RVA: 0x000BEE6D File Offset: 0x000BD26D
	// (set) Token: 0x0600159E RID: 5534 RVA: 0x000BEE75 File Offset: 0x000BD275
	public Color tint
	{
		get
		{
			return this.m_tint;
		}
		set
		{
			if (SkyDriver.SetStruct<Color>(ref this.m_tint, value))
			{
				this.m_skybox.SetVector("tint", this.m_tint * this.m_exposure);
			}
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x0600159F RID: 5535 RVA: 0x000BEEAE File Offset: 0x000BD2AE
	// (set) Token: 0x060015A0 RID: 5536 RVA: 0x000BEEB8 File Offset: 0x000BD2B8
	public float nightStrength
	{
		get
		{
			return this.m_nightStrength;
		}
		set
		{
			if (SkyDriver.SetStruct<float>(ref this.m_nightStrength, value))
			{
				this.m_skybox.SetVector("mcol", new Vector4(this.m_nightColor[0], this.m_nightColor[1], this.m_nightColor[2], this.m_nightStrength));
			}
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x060015A1 RID: 5537 RVA: 0x000BEF15 File Offset: 0x000BD315
	// (set) Token: 0x060015A2 RID: 5538 RVA: 0x000BEF20 File Offset: 0x000BD320
	public Color nightColor
	{
		get
		{
			return this.m_nightColor;
		}
		set
		{
			if (SkyDriver.SetStruct<Color>(ref this.m_nightColor, value))
			{
				this.m_skybox.SetVector("mcol", new Vector4(this.m_nightColor[0], this.m_nightColor[1], this.m_nightColor[2], this.m_nightStrength));
			}
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x060015A3 RID: 5539 RVA: 0x000BEF7D File Offset: 0x000BD37D
	// (set) Token: 0x060015A4 RID: 5540 RVA: 0x000BEF85 File Offset: 0x000BD385
	public Vector2 starOffset
	{
		get
		{
			return this.m_starOffset;
		}
		set
		{
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x060015A5 RID: 5541 RVA: 0x000BEF87 File Offset: 0x000BD387
	// (set) Token: 0x060015A6 RID: 5542 RVA: 0x000BEF8F File Offset: 0x000BD38F
	public float starLerp
	{
		get
		{
			return this.m_starLerp;
		}
		set
		{
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x060015A7 RID: 5543 RVA: 0x000BEF91 File Offset: 0x000BD391
	// (set) Token: 0x060015A8 RID: 5544 RVA: 0x000BEF99 File Offset: 0x000BD399
	public float sunLerp
	{
		get
		{
			return this.m_sunLerp;
		}
		set
		{
			if (SkyDriver.SetStruct<float>(ref this.m_sunLerp, value))
			{
				this.Compute();
			}
			this.SaveToMaterial(true);
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x060015A9 RID: 5545 RVA: 0x000BEFB9 File Offset: 0x000BD3B9
	// (set) Token: 0x060015AA RID: 5546 RVA: 0x000BEFC1 File Offset: 0x000BD3C1
	public bool overrideZenith
	{
		get
		{
			return this.m_overrideZenith;
		}
		set
		{
			if (SkyDriver.SetStruct<bool>(ref this.m_overrideZenith, value))
			{
				this.Compute();
				this.SaveToMaterial(true);
			}
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x060015AB RID: 5547 RVA: 0x000BEFE1 File Offset: 0x000BD3E1
	// (set) Token: 0x060015AC RID: 5548 RVA: 0x000BEFE9 File Offset: 0x000BD3E9
	public float zenithClamp
	{
		get
		{
			return this.m_zenithClamp;
		}
		set
		{
			if (SkyDriver.SetStruct<float>(ref this.m_zenithClamp, value))
			{
				this.Compute();
				this.SaveToMaterial(true);
			}
		}
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x000BF009 File Offset: 0x000BD409
	private void Start()
	{
		this.UpdateCoordinate();
		this.Compute();
		this.SaveToMaterial(false);
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x000BF020 File Offset: 0x000BD420
	private void Update()
	{
		if (this.m_skybox != null && base.transform.rotation != this.lastRot)
		{
			this.UpdateCoordinate();
			this.Compute();
			this.SaveToMaterial(true);
		}
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x000BF06C File Offset: 0x000BD46C
	private void Compute()
	{
		Vector2 sunCoordinate = SkyDriverHelper.GetSunCoordinate(this.direction);
		this.theta = sunCoordinate.x;
		this.phi = sunCoordinate.y;
		if (this.datas == null)
		{
			this.datas = SkyDriverHelper.defaultDatas;
		}
		int num = this.m_customizationMode;
		if (num < 4)
		{
			SkyDriverHelper.CalculateTurbidity(ref this.datas, this.turbidity);
		}
		else
		{
			num -= 4;
		}
		if (num < 2)
		{
			SkyDriverHelper.CalculateZenithCoefficient(ref this.datas);
			this.overrideZenith = false;
		}
		else
		{
			num -= 2;
		}
		if (!this.overrideZenith)
		{
			SkyDriverHelper.CalculateZenith(ref this.datas, this.turbidity, this.theta, this.zenithClamp);
		}
		if (num < 1)
		{
			SkyDriverHelper.CalculateChannels(ref this.datas);
		}
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x000BF13C File Offset: 0x000BD53C
	private void SaveToMaterial(bool quick)
	{
		if (this.overrideZenith)
		{
			this.m_skybox.SetVector("zenith", this.datas[6]);
		}
		else
		{
			this.m_skybox.SetVector("zenith", SkyDriverHelper.linearMult(this.datas[5], this.datas[6]));
		}
		this.m_skybox.SetFloat("phi", this.phi);
		this.m_skybox.SetFloat("theta", this.theta);
		this.m_skybox.SetFloat("sunS", (this.theta <= 1.57079f) ? this.m_sunSize : (this.m_sunSize * Mathf.Max(1f - (this.theta / 1.57079f - 1f) / (this.m_sunLerp + 0.0001f), 0f)));
		if (quick)
		{
			return;
		}
		this.m_skybox.SetVector("propA", this.datas[0]);
		this.m_skybox.SetVector("propB", this.datas[1]);
		this.m_skybox.SetVector("propC", this.datas[2]);
		this.m_skybox.SetVector("propD", this.datas[3]);
		this.m_skybox.SetVector("propE", this.datas[4]);
		this.m_skybox.SetVector("difR", this.datas[7]);
		this.m_skybox.SetVector("difG", this.datas[8]);
		this.m_skybox.SetVector("difB", this.datas[9]);
		this.m_skybox.SetVector("tint", this.m_tint * this.m_exposure);
		this.m_skybox.SetVector("mcol", new Vector4(this.m_nightColor[0], this.m_nightColor[1], this.m_nightColor[2], this.m_nightStrength));
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x000BF3E8 File Offset: 0x000BD7E8
	private void UpdateCoordinate()
	{
		this.lastRot = base.transform.rotation;
		this.direction = this.lastRot * Quaternion.Euler(-90f, 0f, 0f) * Vector3.up;
		this.direction = new Vector3(this.direction[0], this.direction[2], this.direction[1]);
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x000BF464 File Offset: 0x000BD864
	public static bool SetStruct<T>(ref T currentValue, T newValue)
	{
		if (currentValue.Equals(newValue))
		{
			return false;
		}
		currentValue = newValue;
		return true;
	}

	// Token: 0x040012BB RID: 4795
	[SerializeField]
	private Material m_skybox;

	// Token: 0x040012BC RID: 4796
	[SerializeField]
	[Range(0f, 7f)]
	private int m_customizationMode;

	// Token: 0x040012BD RID: 4797
	[SerializeField]
	[Range(1.7f, 10f)]
	private float m_turbidity = 2.5f;

	// Token: 0x040012BE RID: 4798
	[SerializeField]
	[Range(0f, 5f)]
	private float m_sunSize = 1f;

	// Token: 0x040012BF RID: 4799
	[SerializeField]
	[Range(0f, 5f)]
	private float m_exposure = 1f;

	// Token: 0x040012C0 RID: 4800
	[SerializeField]
	private Color m_tint = Color.white;

	// Token: 0x040012C1 RID: 4801
	[SerializeField]
	[Range(0f, 2f)]
	private float m_nightStrength = 0.4f;

	// Token: 0x040012C2 RID: 4802
	[SerializeField]
	private Color m_nightColor = Color.blue * 0.05f;

	// Token: 0x040012C3 RID: 4803
	[SerializeField]
	private Vector2 m_starOffset = Vector2.zero;

	// Token: 0x040012C4 RID: 4804
	[SerializeField]
	[Range(0f, 1.57f)]
	private float m_starLerp = 0.2f;

	// Token: 0x040012C5 RID: 4805
	[SerializeField]
	[Range(0f, 1f)]
	private float m_sunLerp = 0.2f;

	// Token: 0x040012C6 RID: 4806
	[SerializeField]
	private bool m_overrideZenith;

	// Token: 0x040012C7 RID: 4807
	[SerializeField]
	[Range(0.14f, 1.57f)]
	private float m_zenithClamp = 0.14f;

	// Token: 0x040012C8 RID: 4808
	private Quaternion lastRot;

	// Token: 0x040012C9 RID: 4809
	private Vector3 direction;

	// Token: 0x040012CA RID: 4810
	private float phi;

	// Token: 0x040012CB RID: 4811
	private float theta;

	// Token: 0x040012CC RID: 4812
	public Vector3[] datas;

	// Token: 0x040012CD RID: 4813
	private const float M_PI_2 = 1.57079f;
}
