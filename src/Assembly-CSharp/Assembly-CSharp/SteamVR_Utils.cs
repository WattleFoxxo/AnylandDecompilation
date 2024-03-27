using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Valve.VR;

// Token: 0x020002B7 RID: 695
public static class SteamVR_Utils
{
	// Token: 0x060019E4 RID: 6628 RVA: 0x000EB6A8 File Offset: 0x000E9AA8
	public static Quaternion Slerp(Quaternion A, Quaternion B, float t)
	{
		float num = Mathf.Clamp(A.x * B.x + A.y * B.y + A.z * B.z + A.w * B.w, -1f, 1f);
		if (num < 0f)
		{
			B = new Quaternion(-B.x, -B.y, -B.z, -B.w);
			num = -num;
		}
		float num4;
		float num5;
		if (1f - num > 0.0001f)
		{
			float num2 = Mathf.Acos(num);
			float num3 = Mathf.Sin(num2);
			num4 = Mathf.Sin((1f - t) * num2) / num3;
			num5 = Mathf.Sin(t * num2) / num3;
		}
		else
		{
			num4 = 1f - t;
			num5 = t;
		}
		return new Quaternion(num4 * A.x + num5 * B.x, num4 * A.y + num5 * B.y, num4 * A.z + num5 * B.z, num4 * A.w + num5 * B.w);
	}

	// Token: 0x060019E5 RID: 6629 RVA: 0x000EB7D8 File Offset: 0x000E9BD8
	public static Vector3 Lerp(Vector3 A, Vector3 B, float t)
	{
		return new Vector3(SteamVR_Utils.Lerp(A.x, B.x, t), SteamVR_Utils.Lerp(A.y, B.y, t), SteamVR_Utils.Lerp(A.z, B.z, t));
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x000EB826 File Offset: 0x000E9C26
	public static float Lerp(float A, float B, float t)
	{
		return A + (B - A) * t;
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x000EB82F File Offset: 0x000E9C2F
	public static double Lerp(double A, double B, double t)
	{
		return A + (B - A) * t;
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x000EB838 File Offset: 0x000E9C38
	public static float InverseLerp(Vector3 A, Vector3 B, Vector3 result)
	{
		return Vector3.Dot(result - A, B - A);
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x000EB84D File Offset: 0x000E9C4D
	public static float InverseLerp(float A, float B, float result)
	{
		return (result - A) / (B - A);
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x000EB856 File Offset: 0x000E9C56
	public static double InverseLerp(double A, double B, double result)
	{
		return (result - A) / (B - A);
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x000EB85F File Offset: 0x000E9C5F
	public static float Saturate(float A)
	{
		return (A >= 0f) ? ((A <= 1f) ? A : 1f) : 0f;
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x000EB88C File Offset: 0x000E9C8C
	public static Vector2 Saturate(Vector2 A)
	{
		return new Vector2(SteamVR_Utils.Saturate(A.x), SteamVR_Utils.Saturate(A.y));
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x000EB8AB File Offset: 0x000E9CAB
	public static float Abs(float A)
	{
		return (A >= 0f) ? A : (-A);
	}

	// Token: 0x060019EE RID: 6638 RVA: 0x000EB8C0 File Offset: 0x000E9CC0
	public static Vector2 Abs(Vector2 A)
	{
		return new Vector2(SteamVR_Utils.Abs(A.x), SteamVR_Utils.Abs(A.y));
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x000EB8DF File Offset: 0x000E9CDF
	private static float _copysign(float sizeval, float signval)
	{
		return (Mathf.Sign(signval) != 1f) ? (-Mathf.Abs(sizeval)) : Mathf.Abs(sizeval);
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x000EB904 File Offset: 0x000E9D04
	public static Quaternion GetRotation(this Matrix4x4 matrix)
	{
		Quaternion quaternion = default(Quaternion);
		quaternion.w = Mathf.Sqrt(Mathf.Max(0f, 1f + matrix.m00 + matrix.m11 + matrix.m22)) / 2f;
		quaternion.x = Mathf.Sqrt(Mathf.Max(0f, 1f + matrix.m00 - matrix.m11 - matrix.m22)) / 2f;
		quaternion.y = Mathf.Sqrt(Mathf.Max(0f, 1f - matrix.m00 + matrix.m11 - matrix.m22)) / 2f;
		quaternion.z = Mathf.Sqrt(Mathf.Max(0f, 1f - matrix.m00 - matrix.m11 + matrix.m22)) / 2f;
		quaternion.x = SteamVR_Utils._copysign(quaternion.x, matrix.m21 - matrix.m12);
		quaternion.y = SteamVR_Utils._copysign(quaternion.y, matrix.m02 - matrix.m20);
		quaternion.z = SteamVR_Utils._copysign(quaternion.z, matrix.m10 - matrix.m01);
		return quaternion;
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x000EBA64 File Offset: 0x000E9E64
	public static Vector3 GetPosition(this Matrix4x4 matrix)
	{
		float m = matrix.m03;
		float m2 = matrix.m13;
		float m3 = matrix.m23;
		return new Vector3(m, m2, m3);
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x000EBA94 File Offset: 0x000E9E94
	public static Vector3 GetScale(this Matrix4x4 m)
	{
		float num = Mathf.Sqrt(m.m00 * m.m00 + m.m01 * m.m01 + m.m02 * m.m02);
		float num2 = Mathf.Sqrt(m.m10 * m.m10 + m.m11 * m.m11 + m.m12 * m.m12);
		float num3 = Mathf.Sqrt(m.m20 * m.m20 + m.m21 * m.m21 + m.m22 * m.m22);
		return new Vector3(num, num2, num3);
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x000EBB48 File Offset: 0x000E9F48
	public static object CallSystemFn(SteamVR_Utils.SystemFn fn, params object[] args)
	{
		bool flag = !SteamVR.active && !SteamVR.usingNativeSupport;
		if (flag)
		{
			EVRInitError evrinitError = EVRInitError.None;
			OpenVR.Init(ref evrinitError, EVRApplicationType.VRApplication_Other);
		}
		CVRSystem system = OpenVR.System;
		object obj = ((system == null) ? null : fn(system, args));
		if (flag)
		{
			OpenVR.Shutdown();
		}
		return obj;
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x000EBBA4 File Offset: 0x000E9FA4
	public static void TakeStereoScreenshot(uint screenshotHandle, GameObject target, int cellSize, float ipd, ref string previewFilename, ref string VRFilename)
	{
		Texture2D texture2D = new Texture2D(4096, 4096, TextureFormat.ARGB32, false);
		Stopwatch stopwatch = new Stopwatch();
		Camera camera = null;
		stopwatch.Start();
		Camera camera2 = target.GetComponent<Camera>();
		if (camera2 == null)
		{
			if (camera == null)
			{
				camera = new GameObject().AddComponent<Camera>();
			}
			camera2 = camera;
		}
		Texture2D texture2D2 = new Texture2D(2048, 2048, TextureFormat.ARGB32, false);
		RenderTexture renderTexture = new RenderTexture(2048, 2048, 24);
		RenderTexture targetTexture = camera2.targetTexture;
		bool orthographic = camera2.orthographic;
		float fieldOfView = camera2.fieldOfView;
		float aspect = camera2.aspect;
		StereoTargetEyeMask stereoTargetEye = camera2.stereoTargetEye;
		camera2.stereoTargetEye = StereoTargetEyeMask.None;
		camera2.fieldOfView = 60f;
		camera2.orthographic = false;
		camera2.targetTexture = renderTexture;
		camera2.aspect = 1f;
		camera2.Render();
		RenderTexture.active = renderTexture;
		texture2D2.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
		RenderTexture.active = null;
		camera2.targetTexture = null;
		global::UnityEngine.Object.DestroyImmediate(renderTexture);
		SteamVR_SphericalProjection steamVR_SphericalProjection = camera2.gameObject.AddComponent<SteamVR_SphericalProjection>();
		Vector3 localPosition = target.transform.localPosition;
		Quaternion localRotation = target.transform.localRotation;
		Vector3 position = target.transform.position;
		Quaternion quaternion = Quaternion.Euler(0f, target.transform.rotation.eulerAngles.y, 0f);
		Transform transform = camera2.transform;
		int num = 1024 / cellSize;
		float num2 = 90f / (float)num;
		float num3 = num2 / 2f;
		RenderTexture renderTexture2 = new RenderTexture(cellSize, cellSize, 24);
		renderTexture2.wrapMode = TextureWrapMode.Clamp;
		renderTexture2.antiAliasing = 8;
		camera2.fieldOfView = num2;
		camera2.orthographic = false;
		camera2.targetTexture = renderTexture2;
		camera2.aspect = aspect;
		camera2.stereoTargetEye = StereoTargetEyeMask.None;
		for (int i = 0; i < num; i++)
		{
			float num4 = 90f - (float)i * num2 - num3;
			int num5 = 4096 / renderTexture2.width;
			float num6 = 360f / (float)num5;
			float num7 = num6 / 2f;
			int num8 = i * 1024 / num;
			for (int j = 0; j < 2; j++)
			{
				if (j == 1)
				{
					num4 = -num4;
					num8 = 2048 - num8 - cellSize;
				}
				for (int k = 0; k < num5; k++)
				{
					float num9 = -180f + (float)k * num6 + num7;
					int num10 = k * 4096 / num5;
					int num11 = 0;
					float num12 = -ipd / 2f * Mathf.Cos(num4 * 0.017453292f);
					for (int l = 0; l < 2; l++)
					{
						if (l == 1)
						{
							num11 = 2048;
							num12 = -num12;
						}
						Vector3 vector = quaternion * Quaternion.Euler(0f, num9, 0f) * new Vector3(num12, 0f, 0f);
						transform.position = position + vector;
						Quaternion quaternion2 = Quaternion.Euler(num4, num9, 0f);
						transform.rotation = quaternion * quaternion2;
						Vector3 vector2 = quaternion2 * Vector3.forward;
						float num13 = num9 - num6 / 2f;
						float num14 = num13 + num6;
						float num15 = num4 + num2 / 2f;
						float num16 = num15 - num2;
						float num17 = (num13 + num14) / 2f;
						float num18 = ((Mathf.Abs(num15) >= Mathf.Abs(num16)) ? num16 : num15);
						Vector3 vector3 = Quaternion.Euler(num18, num13, 0f) * Vector3.forward;
						Vector3 vector4 = Quaternion.Euler(num18, num14, 0f) * Vector3.forward;
						Vector3 vector5 = Quaternion.Euler(num15, num17, 0f) * Vector3.forward;
						Vector3 vector6 = Quaternion.Euler(num16, num17, 0f) * Vector3.forward;
						Vector3 vector7 = vector3 / Vector3.Dot(vector3, vector2);
						Vector3 vector8 = vector4 / Vector3.Dot(vector4, vector2);
						Vector3 vector9 = vector5 / Vector3.Dot(vector5, vector2);
						Vector3 vector10 = vector6 / Vector3.Dot(vector6, vector2);
						Vector3 vector11 = vector8 - vector7;
						Vector3 vector12 = vector10 - vector9;
						float magnitude = vector11.magnitude;
						float magnitude2 = vector12.magnitude;
						float num19 = 1f / magnitude;
						float num20 = 1f / magnitude2;
						Vector3 vector13 = vector11 * num19;
						Vector3 vector14 = vector12 * num20;
						steamVR_SphericalProjection.Set(vector2, num13, num14, num15, num16, vector13, vector7, num19, vector14, vector9, num20);
						camera2.aspect = magnitude / magnitude2;
						camera2.Render();
						RenderTexture.active = renderTexture2;
						texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture2.width, (float)renderTexture2.height), num10, num8 + num11);
						RenderTexture.active = null;
					}
					float num21 = ((float)i * ((float)num5 * 2f) + (float)k + (float)(j * num5)) / ((float)num * ((float)num5 * 2f));
					OpenVR.Screenshots.UpdateScreenshotProgress(screenshotHandle, num21);
				}
			}
		}
		OpenVR.Screenshots.UpdateScreenshotProgress(screenshotHandle, 1f);
		previewFilename += ".png";
		VRFilename += ".png";
		texture2D2.Apply();
		File.WriteAllBytes(previewFilename, texture2D2.EncodeToPNG());
		texture2D.Apply();
		File.WriteAllBytes(VRFilename, texture2D.EncodeToPNG());
		if (camera2 != camera)
		{
			camera2.targetTexture = targetTexture;
			camera2.orthographic = orthographic;
			camera2.fieldOfView = fieldOfView;
			camera2.aspect = aspect;
			camera2.stereoTargetEye = stereoTargetEye;
			target.transform.localPosition = localPosition;
			target.transform.localRotation = localRotation;
		}
		else
		{
			camera.targetTexture = null;
		}
		global::UnityEngine.Object.DestroyImmediate(renderTexture2);
		global::UnityEngine.Object.DestroyImmediate(steamVR_SphericalProjection);
		stopwatch.Stop();
		global::UnityEngine.Debug.Log(string.Format("Screenshot took {0} seconds.", stopwatch.Elapsed));
		if (camera != null)
		{
			global::UnityEngine.Object.DestroyImmediate(camera.gameObject);
		}
		global::UnityEngine.Object.DestroyImmediate(texture2D2);
		global::UnityEngine.Object.DestroyImmediate(texture2D);
	}

	// Token: 0x020002B8 RID: 696
	public class Event
	{
		// Token: 0x060019F6 RID: 6646 RVA: 0x000EC1FC File Offset: 0x000EA5FC
		public static void Listen(string message, SteamVR_Utils.Event.Handler action)
		{
			SteamVR_Utils.Event.Handler handler = SteamVR_Utils.Event.listeners[message] as SteamVR_Utils.Event.Handler;
			if (handler != null)
			{
				SteamVR_Utils.Event.listeners[message] = (SteamVR_Utils.Event.Handler)Delegate.Combine(handler, action);
			}
			else
			{
				SteamVR_Utils.Event.listeners[message] = action;
			}
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x000EC248 File Offset: 0x000EA648
		public static void Remove(string message, SteamVR_Utils.Event.Handler action)
		{
			SteamVR_Utils.Event.Handler handler = SteamVR_Utils.Event.listeners[message] as SteamVR_Utils.Event.Handler;
			if (handler != null)
			{
				SteamVR_Utils.Event.listeners[message] = (SteamVR_Utils.Event.Handler)Delegate.Remove(handler, action);
			}
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x000EC284 File Offset: 0x000EA684
		public static void Send(string message, params object[] args)
		{
			SteamVR_Utils.Event.Handler handler = SteamVR_Utils.Event.listeners[message] as SteamVR_Utils.Event.Handler;
			if (handler != null)
			{
				handler(args);
			}
		}

		// Token: 0x0400182B RID: 6187
		private static Hashtable listeners = new Hashtable();

		// Token: 0x020002B9 RID: 697
		// (Invoke) Token: 0x060019FB RID: 6651
		public delegate void Handler(params object[] args);
	}

	// Token: 0x020002BA RID: 698
	[Serializable]
	public struct RigidTransform
	{
		// Token: 0x060019FE RID: 6654 RVA: 0x000EC2BB File Offset: 0x000EA6BB
		public RigidTransform(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this.rot = rot;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x000EC2CB File Offset: 0x000EA6CB
		public RigidTransform(Transform t)
		{
			this.pos = t.position;
			this.rot = t.rotation;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x000EC2E8 File Offset: 0x000EA6E8
		public RigidTransform(Transform from, Transform to)
		{
			Quaternion quaternion = Quaternion.Inverse(from.rotation);
			this.rot = quaternion * to.rotation;
			this.pos = quaternion * (to.position - from.position);
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x000EC330 File Offset: 0x000EA730
		public RigidTransform(HmdMatrix34_t pose)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			identity[0, 0] = pose.m0;
			identity[0, 1] = pose.m1;
			identity[0, 2] = -pose.m2;
			identity[0, 3] = pose.m3;
			identity[1, 0] = pose.m4;
			identity[1, 1] = pose.m5;
			identity[1, 2] = -pose.m6;
			identity[1, 3] = pose.m7;
			identity[2, 0] = -pose.m8;
			identity[2, 1] = -pose.m9;
			identity[2, 2] = pose.m10;
			identity[2, 3] = -pose.m11;
			this.pos = identity.GetPosition();
			this.rot = identity.GetRotation();
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x000EC420 File Offset: 0x000EA820
		public RigidTransform(HmdMatrix44_t pose)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			identity[0, 0] = pose.m0;
			identity[0, 1] = pose.m1;
			identity[0, 2] = -pose.m2;
			identity[0, 3] = pose.m3;
			identity[1, 0] = pose.m4;
			identity[1, 1] = pose.m5;
			identity[1, 2] = -pose.m6;
			identity[1, 3] = pose.m7;
			identity[2, 0] = -pose.m8;
			identity[2, 1] = -pose.m9;
			identity[2, 2] = pose.m10;
			identity[2, 3] = -pose.m11;
			identity[3, 0] = pose.m12;
			identity[3, 1] = pose.m13;
			identity[3, 2] = -pose.m14;
			identity[3, 3] = pose.m15;
			this.pos = identity.GetPosition();
			this.rot = identity.GetRotation();
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x000EC551 File Offset: 0x000EA951
		public static SteamVR_Utils.RigidTransform identity
		{
			get
			{
				return new SteamVR_Utils.RigidTransform(Vector3.zero, Quaternion.identity);
			}
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000EC562 File Offset: 0x000EA962
		public static SteamVR_Utils.RigidTransform FromLocal(Transform t)
		{
			return new SteamVR_Utils.RigidTransform(t.localPosition, t.localRotation);
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x000EC578 File Offset: 0x000EA978
		public HmdMatrix44_t ToHmdMatrix44()
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.pos, this.rot, Vector3.one);
			return new HmdMatrix44_t
			{
				m0 = matrix4x[0, 0],
				m1 = matrix4x[0, 1],
				m2 = -matrix4x[0, 2],
				m3 = matrix4x[0, 3],
				m4 = matrix4x[1, 0],
				m5 = matrix4x[1, 1],
				m6 = -matrix4x[1, 2],
				m7 = matrix4x[1, 3],
				m8 = -matrix4x[2, 0],
				m9 = -matrix4x[2, 1],
				m10 = matrix4x[2, 2],
				m11 = -matrix4x[2, 3],
				m12 = matrix4x[3, 0],
				m13 = matrix4x[3, 1],
				m14 = -matrix4x[3, 2],
				m15 = matrix4x[3, 3]
			};
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000EC6AC File Offset: 0x000EAAAC
		public HmdMatrix34_t ToHmdMatrix34()
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.pos, this.rot, Vector3.one);
			return new HmdMatrix34_t
			{
				m0 = matrix4x[0, 0],
				m1 = matrix4x[0, 1],
				m2 = -matrix4x[0, 2],
				m3 = matrix4x[0, 3],
				m4 = matrix4x[1, 0],
				m5 = matrix4x[1, 1],
				m6 = -matrix4x[1, 2],
				m7 = matrix4x[1, 3],
				m8 = -matrix4x[2, 0],
				m9 = -matrix4x[2, 1],
				m10 = matrix4x[2, 2],
				m11 = -matrix4x[2, 3]
			};
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x000EC7A0 File Offset: 0x000EABA0
		public override bool Equals(object o)
		{
			if (o is SteamVR_Utils.RigidTransform)
			{
				SteamVR_Utils.RigidTransform rigidTransform = (SteamVR_Utils.RigidTransform)o;
				return this.pos == rigidTransform.pos && this.rot == rigidTransform.rot;
			}
			return false;
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x000EC7ED File Offset: 0x000EABED
		public override int GetHashCode()
		{
			return this.pos.GetHashCode() ^ this.rot.GetHashCode();
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x000EC812 File Offset: 0x000EAC12
		public static bool operator ==(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b)
		{
			return a.pos == b.pos && a.rot == b.rot;
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x000EC842 File Offset: 0x000EAC42
		public static bool operator !=(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b)
		{
			return a.pos != b.pos || a.rot != b.rot;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x000EC874 File Offset: 0x000EAC74
		public static SteamVR_Utils.RigidTransform operator *(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b)
		{
			return new SteamVR_Utils.RigidTransform
			{
				rot = a.rot * b.rot,
				pos = a.pos + a.rot * b.pos
			};
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000EC8CA File Offset: 0x000EACCA
		public void Inverse()
		{
			this.rot = Quaternion.Inverse(this.rot);
			this.pos = -(this.rot * this.pos);
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000EC8FC File Offset: 0x000EACFC
		public SteamVR_Utils.RigidTransform GetInverse()
		{
			SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(this.pos, this.rot);
			rigidTransform.Inverse();
			return rigidTransform;
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000EC924 File Offset: 0x000EAD24
		public void Multiply(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b)
		{
			this.rot = a.rot * b.rot;
			this.pos = a.pos + a.rot * b.pos;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000EC964 File Offset: 0x000EAD64
		public Vector3 InverseTransformPoint(Vector3 point)
		{
			return Quaternion.Inverse(this.rot) * (point - this.pos);
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x000EC982 File Offset: 0x000EAD82
		public Vector3 TransformPoint(Vector3 point)
		{
			return this.pos + this.rot * point;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000EC99B File Offset: 0x000EAD9B
		public static Vector3 operator *(SteamVR_Utils.RigidTransform t, Vector3 v)
		{
			return t.TransformPoint(v);
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000EC9A5 File Offset: 0x000EADA5
		public static SteamVR_Utils.RigidTransform Interpolate(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b, float t)
		{
			return new SteamVR_Utils.RigidTransform(Vector3.Lerp(a.pos, b.pos, t), Quaternion.Slerp(a.rot, b.rot, t));
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000EC9D4 File Offset: 0x000EADD4
		public void Interpolate(SteamVR_Utils.RigidTransform to, float t)
		{
			this.pos = SteamVR_Utils.Lerp(this.pos, to.pos, t);
			this.rot = SteamVR_Utils.Slerp(this.rot, to.rot, t);
		}

		// Token: 0x0400182C RID: 6188
		public Vector3 pos;

		// Token: 0x0400182D RID: 6189
		public Quaternion rot;
	}

	// Token: 0x020002BB RID: 699
	// (Invoke) Token: 0x06001A15 RID: 6677
	public delegate object SystemFn(CVRSystem system, params object[] args);
}
