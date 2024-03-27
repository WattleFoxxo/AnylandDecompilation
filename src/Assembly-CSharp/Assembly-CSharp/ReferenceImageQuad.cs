using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class ReferenceImageQuad : MonoBehaviour
{
	// Token: 0x060008BB RID: 2235 RVA: 0x00032E7B File Offset: 0x0003127B
	private void Start()
	{
		this.UpdateTexture();
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00032E84 File Offset: 0x00031284
	private void Update()
	{
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.V) && !this.isLoading)
		{
			string text = GUIUtility.systemCopyBuffer;
			if (text == null)
			{
				return;
			}
			text = this.ConvertLocalPathToFileUrlFormatIfNeeded(text);
			if (text.ToLower().IndexOf(".obj") >= 0 && text.IndexOf(":\\") >= 0)
			{
				this.ParsePathAndLoadObjModel(text);
			}
			else if ((text.IndexOf("http://") == 0 || text.IndexOf("https://") == 0 || text.IndexOf("file://") == 0) && text.IndexOf("https://steamuserimages-a.akamaihd.net/ugc/") != 0)
			{
				base.StartCoroutine(this.StartImageLoad(text));
				Managers.soundManager.Play("putDown", base.transform, 1f, false, false);
			}
		}
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00032F6C File Offset: 0x0003136C
	private void ParsePathAndLoadObjModel(string pathAndMore)
	{
		string text = pathAndMore;
		float num = 1f;
		float num2 = 0f;
		if (pathAndMore.Contains(";"))
		{
			string[] array = Misc.Split(pathAndMore, ";", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text2 in array)
			{
				string text3 = string.Empty;
				string text4 = string.Empty;
				if (text2.Contains("="))
				{
					string[] array3 = Misc.Split(text2, "=", StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length == 2)
					{
						text3 = array3[0];
						text4 = array3[1];
					}
				}
				else
				{
					text3 = "path";
					text4 = text2;
				}
				if (text3 != null)
				{
					if (!(text3 == "path"))
					{
						float num4;
						if (!(text3 == "size"))
						{
							if (text3 == "y")
							{
								float num3;
								if (float.TryParse(text4, out num3))
								{
									num2 = num3;
								}
							}
						}
						else if (float.TryParse(text4, out num4))
						{
							num = num4;
						}
					}
					else
					{
						text = text4;
					}
				}
			}
		}
		this.LoadObjModel(text, num, num2);
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00033098 File Offset: 0x00031498
	private string ConvertLocalPathToFileUrlFormatIfNeeded(string url)
	{
		if (url.IndexOf(":\\") >= 0 && url.IndexOf(".obj") == -1)
		{
			url = url.Replace("\\", "/");
			url = "file:///" + url;
		}
		return url;
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x000330E8 File Offset: 0x000314E8
	private void LoadObjModel(string path, float size = 1f, float y = 0f)
	{
		this.isLoading = true;
		global::UnityEngine.Object.Destroy(CreationHelper.referenceObject);
		GameObject gameObject = OBJLoader.LoadOBJFile(path);
		this.ResetTransform(gameObject.transform);
		IEnumerator enumerator = gameObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				Renderer component = transform.GetComponent<Renderer>();
				if (component != null && component.material != null)
				{
					this.ApplyTransparentMaterial(component.material);
				}
				transform.localScale = Vector3.one;
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
		gameObject.transform.name = "Imported Reference " + gameObject.transform.name;
		gameObject.transform.localScale = Misc.GetUniformVector3(size);
		Vector3 position = gameObject.transform.position;
		position.y = y;
		gameObject.transform.position = position;
		CreationHelper.referenceObject = gameObject;
		string text = "✔ Placed in area center" + Environment.NewLine;
		if (size != 1f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				Environment.NewLine,
				"Size: ",
				size
			});
		}
		if (y != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				Environment.NewLine,
				"Y: ",
				y
			});
		}
		if (size == 1f || y == 0f)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				Environment.NewLine,
				Environment.NewLine,
				"You can append size and y via",
				Environment.NewLine,
				"\"",
				Misc.TruncateRightAligned(path + ";size=0.1;y=1.5", 30, "..."),
				"\""
			});
		}
		Managers.dialogManager.ShowInfo(text, false, false, 1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
		Managers.soundManager.Play("success", base.transform, 0.2f, false, false);
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00033334 File Offset: 0x00031734
	private void ApplyTransparentMaterial(Material thisMaterial)
	{
		thisMaterial.SetInt("_SrcBlend", 1);
		thisMaterial.SetInt("_DstBlend", 10);
		thisMaterial.SetInt("_ZWrite", 0);
		thisMaterial.DisableKeyword("_ALPHATEST_ON");
		thisMaterial.DisableKeyword("_ALPHABLEND_ON");
		thisMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
		thisMaterial.color = new Color(1f, 1f, 1f, 0.5f);
		thisMaterial.renderQueue = 3000;
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x000333B1 File Offset: 0x000317B1
	private void ResetTransform(Transform thisTransform)
	{
		thisTransform.parent = null;
		thisTransform.position = Vector3.zero;
		thisTransform.rotation = Quaternion.identity;
		thisTransform.localScale = Vector3.one;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x000333DC File Offset: 0x000317DC
	private IEnumerator StartImageLoad(string url)
	{
		this.isLoading = true;
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			CreationHelper.referenceImage = www.texture;
			this.isLoading = false;
			this.UpdateTexture();
		}
		else
		{
			Log.Debug("ReferenceImageQuad image load error: " + www.error);
		}
		yield break;
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00033400 File Offset: 0x00031800
	private void UpdateTexture()
	{
		if (CreationHelper.referenceImage != null)
		{
			float num = 0.225f;
			float num2 = num / (float)CreationHelper.referenceImage.width;
			float num3 = num / (float)CreationHelper.referenceImage.height;
			float num4 = Math.Min(num2, num3);
			float num5 = (float)CreationHelper.referenceImage.width * num4;
			float num6 = (float)CreationHelper.referenceImage.height * num4;
			base.transform.localScale = new Vector3(num5, num6, base.transform.localScale.z);
			Renderer component = base.GetComponent<Renderer>();
			component.enabled = true;
			component.material.mainTexture = CreationHelper.referenceImage;
			GameObject gameObject = Misc.FindObject(base.transform.parent.gameObject, "Cube");
			gameObject.GetComponent<Renderer>().enabled = true;
		}
	}

	// Token: 0x0400067C RID: 1660
	private bool isLoading;
}
