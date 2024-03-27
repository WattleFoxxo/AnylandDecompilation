using System;
using System.IO;
using UnityEngine;

// Token: 0x0200025A RID: 602
public class CameraCalibrator : MonoBehaviour
{
	// Token: 0x0600164F RID: 5711 RVA: 0x000C7101 File Offset: 0x000C5501
	private void Start()
	{
		base.InvokeRepeating("UpdateCamera", 0.01f, 1f);
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x000C7118 File Offset: 0x000C5518
	public void UpdateCamera()
	{
		Camera currentCameraComponent = Managers.cameraManager.currentCameraComponent;
		if (!string.IsNullOrEmpty(this.configPath) && File.Exists(this.configPath) && currentCameraComponent != null)
		{
			try
			{
				string text = File.ReadAllText(this.configPath);
				this.UpdateCameraBasedOnData(currentCameraComponent, text);
			}
			catch (Exception ex)
			{
				Log.Debug("Camera calibrator config path " + this.configPath + " not found, skipping");
			}
		}
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x000C71A8 File Offset: 0x000C55A8
	private void UpdateCameraBasedOnData(Camera camera, string data)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 60f;
		float num8 = 0f;
		float num9 = 0f;
		string[] array = Misc.Split(data, "\n", StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in array)
		{
			string text2 = text.Replace("\r", string.Empty);
			string[] array3 = Misc.Split(text2, "=", StringSplitOptions.RemoveEmptyEntries);
			if (array3.Length == 2)
			{
				string text3 = array3[0];
				string text4 = array3[1];
				switch (text3)
				{
				case "x":
					float.TryParse(text4, out num);
					break;
				case "y":
					float.TryParse(text4, out num2);
					break;
				case "z":
					float.TryParse(text4, out num3);
					break;
				case "rx":
					float.TryParse(text4, out num4);
					break;
				case "ry":
					float.TryParse(text4, out num5);
					break;
				case "rz":
					float.TryParse(text4, out num6);
					break;
				case "fov":
					float.TryParse(text4, out num7);
					break;
				case "near":
					float.TryParse(text4, out num8);
					break;
				case "far":
					float.TryParse(text4, out num9);
					break;
				}
			}
		}
		Transform transform = Managers.personManager.OurPersonRig.transform;
		camera.transform.position = transform.position + new Vector3(num, num2, num3);
		camera.transform.eulerAngles = transform.eulerAngles + new Vector3(num4, num5, num6);
		camera.fieldOfView = num7;
		camera.nearClipPlane = num8;
		camera.farClipPlane = num9;
	}

	// Token: 0x04001423 RID: 5155
	public string configPath = string.Empty;
}
