using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class TransformClipboard
{
	// Token: 0x06000EBE RID: 3774 RVA: 0x00081EB8 File Offset: 0x000802B8
	public void Clear()
	{
		this.position = null;
		this.rotation = null;
		this.scale = null;
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x00081EF4 File Offset: 0x000802F4
	public void SetFromTransform(Transform transform)
	{
		if (this.useLocal)
		{
			this.position = new Vector3?(transform.localPosition);
			this.rotation = new Vector3?(transform.localEulerAngles);
		}
		else
		{
			this.position = new Vector3?(transform.position);
			this.rotation = new Vector3?(transform.eulerAngles);
		}
		this.scale = new Vector3?(transform.localScale);
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x00081F68 File Offset: 0x00080368
	public bool SetFromTransformIfDifferent(Transform transform)
	{
		bool flag2;
		if (this.useLocal)
		{
			Vector3? vector = this.position;
			bool flag;
			if (vector != null)
			{
				Vector3? vector2 = this.rotation;
				if (vector2 != null)
				{
					Vector3? vector3 = this.scale;
					if (vector3 != null)
					{
						Vector3 localPosition = transform.localPosition;
						Vector3? vector4 = this.position;
						if (!(localPosition != vector4.Value))
						{
							Vector3 localEulerAngles = transform.localEulerAngles;
							Vector3? vector5 = this.rotation;
							if (!(localEulerAngles != vector5.Value))
							{
								Vector3 localScale = transform.localScale;
								Vector3? vector6 = this.scale;
								flag = localScale != vector6.Value;
								goto IL_AA;
							}
						}
					}
				}
			}
			flag = true;
			IL_AA:
			flag2 = flag;
		}
		else
		{
			Vector3? vector7 = this.position;
			bool flag3;
			if (vector7 != null)
			{
				Vector3? vector8 = this.rotation;
				if (vector8 != null)
				{
					Vector3? vector9 = this.scale;
					if (vector9 != null)
					{
						Vector3 vector10 = transform.position;
						Vector3? vector11 = this.position;
						if (!(vector10 != vector11.Value))
						{
							Vector3 eulerAngles = transform.eulerAngles;
							Vector3? vector12 = this.rotation;
							if (!(eulerAngles != vector12.Value))
							{
								Vector3 localScale2 = transform.localScale;
								Vector3? vector13 = this.scale;
								flag3 = localScale2 != vector13.Value;
								goto IL_150;
							}
						}
					}
				}
			}
			flag3 = true;
			IL_150:
			flag2 = flag3;
		}
		if (flag2)
		{
			this.SetFromTransform(transform);
		}
		return flag2;
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x000820D4 File Offset: 0x000804D4
	public void ApplyToTransform(Transform transform)
	{
		if (this.useLocal)
		{
			Vector3? vector = this.position;
			if (vector != null)
			{
				Vector3? vector2 = this.position;
				transform.localPosition = vector2.Value;
			}
			Vector3? vector3 = this.rotation;
			if (vector3 != null)
			{
				Vector3? vector4 = this.rotation;
				transform.localEulerAngles = vector4.Value;
			}
		}
		else
		{
			Vector3? vector5 = this.position;
			if (vector5 != null)
			{
				Vector3? vector6 = this.position;
				transform.position = vector6.Value;
			}
			Vector3? vector7 = this.rotation;
			if (vector7 != null)
			{
				Vector3? vector8 = this.rotation;
				transform.eulerAngles = vector8.Value;
			}
		}
		Vector3? vector9 = this.scale;
		if (vector9 != null)
		{
			Vector3? vector10 = this.scale;
			transform.localScale = vector10.Value;
		}
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x000821BC File Offset: 0x000805BC
	public bool ContainsSomething()
	{
		Vector3? vector = this.position;
		if (vector == null)
		{
			Vector3? vector2 = this.rotation;
			if (vector2 == null)
			{
				Vector3? vector3 = this.scale;
				return vector3 != null;
			}
		}
		return true;
	}

	// Token: 0x04000F8D RID: 3981
	public Vector3? position;

	// Token: 0x04000F8E RID: 3982
	public Vector3? rotation;

	// Token: 0x04000F8F RID: 3983
	public Vector3? scale;

	// Token: 0x04000F90 RID: 3984
	public bool useLocal;
}
