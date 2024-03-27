using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class EffectParticle : MonoBehaviour
{
	// Token: 0x06000D40 RID: 3392 RVA: 0x00076CB8 File Offset: 0x000750B8
	public void BeCrumble(Vector3 position, Color color)
	{
		float num = 4f;
		this.destroyMeInTime = Time.time + num;
		base.gameObject.name = "EffectParticle_Crumble";
		base.transform.position = position;
		base.transform.localScale = new Vector3(global::UnityEngine.Random.Range(0.005f, 0.03f), global::UnityEngine.Random.Range(0.005f, 0.03f), global::UnityEngine.Random.Range(0.005f, 0.03f));
		base.transform.localEulerAngles = this.GetRandomEulerAngles();
		this.renderer = base.gameObject.GetComponent<Renderer>();
		this.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		this.renderer.material.color = color;
		this.targetColor = new Color(color.r, color.g, color.b, 0f);
		this.body = base.gameObject.AddComponent<Rigidbody>();
		this.body.useGravity = true;
		float num2 = 0.75f;
		Vector3 vector = new Vector3(global::UnityEngine.Random.Range(-num2, num2), global::UnityEngine.Random.Range(-num2, num2), global::UnityEngine.Random.Range(-num2, num2));
		this.body.AddForce(vector, ForceMode.Impulse);
		this.transitionFraction = 0.005f;
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x00076E00 File Offset: 0x00075200
	public void BeSparkle(Vector3 position)
	{
		float num = 5f;
		this.destroyMeInTime = Time.time + num;
		this.doShrink = true;
		this.doRotate = true;
		base.gameObject.name = "EffectParticle_Sparkle";
		base.transform.position = position;
		base.transform.localEulerAngles = this.GetRandomEulerAngles();
		this.targetRotation = Quaternion.Euler(this.GetRandomEulerAngles());
		this.renderer = base.gameObject.GetComponent<Renderer>();
		this.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		this.renderer.material.color = new Color(1f, 1f, 1f, 0.4f);
		this.transitionFraction = 0.005f;
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x00076ECB File Offset: 0x000752CB
	private Vector3 GetRandomEulerAngles()
	{
		return new Vector3(global::UnityEngine.Random.Range(0.01f, 100f), global::UnityEngine.Random.Range(0.01f, 100f), global::UnityEngine.Random.Range(0.01f, 100f));
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x00076F00 File Offset: 0x00075300
	private void Update()
	{
		if (this.doShrink)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1E-05f, 1E-05f, 1E-05f), Time.deltaTime * this.shrinkFactor);
		}
		if (this.doRotate)
		{
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, this.targetRotation, Time.deltaTime * 5f);
		}
		if (this.renderer != null)
		{
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, this.targetColor, this.transitionFraction);
			if (this.renderer.material.color.a <= 0f || (this.destroyMeInTime != -1f && this.destroyMeInTime <= Time.time))
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x04000EDF RID: 3807
	private float destroyMeInTime = -1f;

	// Token: 0x04000EE0 RID: 3808
	private Renderer renderer;

	// Token: 0x04000EE1 RID: 3809
	private Rigidbody body;

	// Token: 0x04000EE2 RID: 3810
	private Color targetColor;

	// Token: 0x04000EE3 RID: 3811
	private float transitionFraction;

	// Token: 0x04000EE4 RID: 3812
	private bool doShrink;

	// Token: 0x04000EE5 RID: 3813
	private bool doRotate;

	// Token: 0x04000EE6 RID: 3814
	private float shrinkFactor = 1f;

	// Token: 0x04000EE7 RID: 3815
	private Quaternion targetRotation;
}
