using System;
using Photon;
using UnityEngine;

// Token: 0x020000B9 RID: 185
[RequireComponent(typeof(PhotonView))]
public class MoveByKeys : global::Photon.MonoBehaviour
{
	// Token: 0x06000643 RID: 1603 RVA: 0x0001D2CB File Offset: 0x0001B6CB
	public void Start()
	{
		this.isSprite = base.GetComponent<SpriteRenderer>() != null;
		this.body2d = base.GetComponent<Rigidbody2D>();
		this.body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0001D2F8 File Offset: 0x0001B6F8
	public void FixedUpdate()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		if (Input.GetAxisRaw("Horizontal") < -0.1f || Input.GetAxisRaw("Horizontal") > 0.1f)
		{
			base.transform.position += Vector3.right * (this.Speed * Time.deltaTime) * Input.GetAxisRaw("Horizontal");
		}
		if (this.jumpingTime <= 0f)
		{
			if ((this.body != null || this.body2d != null) && Input.GetKey(KeyCode.Space))
			{
				this.jumpingTime = this.JumpTimeout;
				Vector2 vector = Vector2.up * this.JumpForce;
				if (this.body2d != null)
				{
					this.body2d.AddForce(vector);
				}
				else if (this.body != null)
				{
					this.body.AddForce(vector);
				}
			}
		}
		else
		{
			this.jumpingTime -= Time.deltaTime;
		}
		if (!this.isSprite && (Input.GetAxisRaw("Vertical") < -0.1f || Input.GetAxisRaw("Vertical") > 0.1f))
		{
			base.transform.position += Vector3.forward * (this.Speed * Time.deltaTime) * Input.GetAxisRaw("Vertical");
		}
	}

	// Token: 0x040004AA RID: 1194
	public float Speed = 10f;

	// Token: 0x040004AB RID: 1195
	public float JumpForce = 200f;

	// Token: 0x040004AC RID: 1196
	public float JumpTimeout = 0.5f;

	// Token: 0x040004AD RID: 1197
	private bool isSprite;

	// Token: 0x040004AE RID: 1198
	private float jumpingTime;

	// Token: 0x040004AF RID: 1199
	private Rigidbody body;

	// Token: 0x040004B0 RID: 1200
	private Rigidbody2D body2d;
}
