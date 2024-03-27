using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MirzaBeig.ParticleSystems.Demos
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class DemoManager : MonoBehaviour
	{
		// Token: 0x06000272 RID: 626 RVA: 0x0000A757 File Offset: 0x00008B57
		private void Awake()
		{
			this.loopingParticleSystems.init();
			this.oneshotParticleSystems.init();
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000A770 File Offset: 0x00008B70
		private void Start()
		{
			this.cameraPositionStart = this.cameraTranslationTransform.localPosition;
			this.cameraRotationStart = this.cameraRotationTransform.localEulerAngles;
			this.resetCameraTransformTargets();
			DemoManager.ParticleMode particleMode = this.particleMode;
			if (particleMode != DemoManager.ParticleMode.looping)
			{
				if (particleMode != DemoManager.ParticleMode.oneshot)
				{
					MonoBehaviour.print("Unknown case.");
				}
				else
				{
					this.setToInstancedParticleMode(true);
					this.loopingParticleModeToggle.isOn = false;
					this.oneshotParticleModeToggle.isOn = true;
				}
			}
			else
			{
				this.setToPerpetualParticleMode(true);
				this.loopingParticleModeToggle.isOn = true;
				this.oneshotParticleModeToggle.isOn = false;
			}
			this.setLighting(this.lighting);
			this.setAdvancedRendering(this.advancedRendering);
			this.lightingToggle.isOn = this.lighting;
			this.advancedRenderingToggle.isOn = this.advancedRendering;
			this.levelToggles = this.levelTogglesContainer.GetComponentsInChildren<Toggle>(true);
			for (int i = 0; i < this.levels.Length; i++)
			{
				if (i == (int)this.currentLevel)
				{
					this.levels[i].SetActive(true);
					this.levelToggles[i].isOn = true;
				}
				else
				{
					this.levels[i].SetActive(false);
					this.levelToggles[i].isOn = false;
				}
			}
			this.updateCurrentParticleSystemNameText();
			this.timeScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.onTimeScaleSliderValueChanged));
			this.onTimeScaleSliderValueChanged(this.timeScaleSlider.value);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000A8F6 File Offset: 0x00008CF6
		public void onTimeScaleSliderValueChanged(float value)
		{
			Time.timeScale = value;
			this.timeScaleSliderValueText.text = value.ToString("0.00");
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A918 File Offset: 0x00008D18
		public void setToPerpetualParticleMode(bool set)
		{
			if (set)
			{
				this.oneshotParticleSystems.clear();
				this.loopingParticleSystems.gameObject.SetActive(true);
				this.oneshotParticleSystems.gameObject.SetActive(false);
				this.particleSpawnInstructionText.gameObject.SetActive(false);
				this.particleMode = DemoManager.ParticleMode.looping;
				this.updateCurrentParticleSystemNameText();
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A978 File Offset: 0x00008D78
		public void setToInstancedParticleMode(bool set)
		{
			if (set)
			{
				this.loopingParticleSystems.gameObject.SetActive(false);
				this.oneshotParticleSystems.gameObject.SetActive(true);
				this.particleSpawnInstructionText.gameObject.SetActive(true);
				this.particleMode = DemoManager.ParticleMode.oneshot;
				this.updateCurrentParticleSystemNameText();
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A9CC File Offset: 0x00008DCC
		public void setLevel(DemoManager.Level level)
		{
			for (int i = 0; i < this.levels.Length; i++)
			{
				if (i == (int)level)
				{
					this.levels[i].SetActive(true);
				}
				else
				{
					this.levels[i].SetActive(false);
				}
			}
			this.currentLevel = level;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000AA21 File Offset: 0x00008E21
		public void setLevelFromToggle(Toggle toggle)
		{
			if (toggle.isOn)
			{
				this.setLevel((DemoManager.Level)Array.IndexOf<Toggle>(this.levelToggles, toggle));
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000AA40 File Offset: 0x00008E40
		public void setLighting(bool value)
		{
			this.lighting = value;
			this.loopingParticleSystems.setLights(value);
			this.oneshotParticleSystems.setLights(value);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000AA64 File Offset: 0x00008E64
		public void setAdvancedRendering(bool value)
		{
			this.advancedRendering = value;
			this.postEffectsCamera.gameObject.SetActive(value);
			this.UICamera.allowHDR = value;
			this.mainCamera.allowHDR = value;
			if (value)
			{
				QualitySettings.SetQualityLevel(32, true);
				this.UICamera.renderingPath = RenderingPath.UsePlayerSettings;
				this.mainCamera.renderingPath = RenderingPath.UsePlayerSettings;
				this.lightingToggle.isOn = true;
				this.mouse.gameObject.SetActive(true);
			}
			else
			{
				QualitySettings.SetQualityLevel(0, true);
				this.UICamera.renderingPath = RenderingPath.VertexLit;
				this.mainCamera.renderingPath = RenderingPath.VertexLit;
				this.lightingToggle.isOn = false;
				this.mouse.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.mainCameraPostEffects.Length; i++)
			{
				if (this.mainCameraPostEffects[i])
				{
					this.mainCameraPostEffects[i].enabled = value;
				}
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000AB5E File Offset: 0x00008F5E
		public static Vector3 dampVector3(Vector3 from, Vector3 to, float speed, float dt)
		{
			return Vector3.Lerp(from, to, 1f - Mathf.Exp(-speed * dt));
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000AB78 File Offset: 0x00008F78
		private void Update()
		{
			this.input.x = Input.GetAxis("Horizontal");
			this.input.y = Input.GetAxis("Vertical");
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.targetCameraPosition.z = this.targetCameraPosition.z + this.input.y * this.cameraMoveAmount;
			}
			else
			{
				this.targetCameraRotation.y = this.targetCameraRotation.y + this.input.x * this.cameraRotateAmount;
				this.targetCameraRotation.x = this.targetCameraRotation.x + this.input.y * this.cameraRotateAmount;
				this.targetCameraRotation.x = Mathf.Clamp(this.targetCameraRotation.x, this.cameraAngleLimits.x, this.cameraAngleLimits.y);
			}
			this.cameraTranslationTransform.localPosition = Vector3.Lerp(this.cameraTranslationTransform.localPosition, this.targetCameraPosition, Time.deltaTime * this.cameraMoveSpeed);
			this.cameraRotation = DemoManager.dampVector3(this.cameraRotation, this.targetCameraRotation, this.cameraRotationSpeed, Time.deltaTime);
			this.cameraRotationTransform.localEulerAngles = this.cameraRotation;
			this.cameraTranslationTransform.LookAt(this.cameraLookAtPosition);
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				this.next();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				this.previous();
			}
			else if (Input.GetKey(KeyCode.R) && this.particleMode == DemoManager.ParticleMode.oneshot)
			{
				this.oneshotParticleSystems.randomize();
				this.updateCurrentParticleSystemNameText();
				if (Input.GetKey(KeyCode.T))
				{
				}
			}
			if (this.particleMode == DemoManager.ParticleMode.oneshot)
			{
				Vector3 mousePosition = Input.mousePosition;
				if (Input.GetMouseButtonDown(0))
				{
					this.oneshotParticleSystems.instantiateParticlePrefab(mousePosition, this.mouse.distanceFromCamera);
				}
				if (Input.GetMouseButton(1))
				{
					this.oneshotParticleSystems.instantiateParticlePrefab(mousePosition, this.mouse.distanceFromCamera);
				}
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				this.resetCameraTransformTargets();
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000ADB8 File Offset: 0x000091B8
		private void LateUpdate()
		{
			this.particleCountText.text = "PARTICLE COUNT: ";
			if (this.particleMode == DemoManager.ParticleMode.looping)
			{
				Text text = this.particleCountText;
				text.text += this.loopingParticleSystems.getParticleCount().ToString();
			}
			else if (this.particleMode == DemoManager.ParticleMode.oneshot)
			{
				Text text2 = this.particleCountText;
				text2.text += this.oneshotParticleSystems.getParticleCount().ToString();
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000AE4F File Offset: 0x0000924F
		private void resetCameraTransformTargets()
		{
			this.targetCameraPosition = this.cameraPositionStart;
			this.targetCameraRotation = this.cameraRotationStart;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000AE6C File Offset: 0x0000926C
		private void updateCurrentParticleSystemNameText()
		{
			if (this.particleMode == DemoManager.ParticleMode.looping)
			{
				this.currentParticleSystemText.text = this.loopingParticleSystems.getCurrentPrefabName(true);
			}
			else if (this.particleMode == DemoManager.ParticleMode.oneshot)
			{
				this.currentParticleSystemText.text = this.oneshotParticleSystems.getCurrentPrefabName(true);
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000AEC3 File Offset: 0x000092C3
		public void next()
		{
			if (this.particleMode == DemoManager.ParticleMode.looping)
			{
				this.loopingParticleSystems.next();
			}
			else if (this.particleMode == DemoManager.ParticleMode.oneshot)
			{
				this.oneshotParticleSystems.next();
			}
			this.updateCurrentParticleSystemNameText();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000AEFD File Offset: 0x000092FD
		public void previous()
		{
			if (this.particleMode == DemoManager.ParticleMode.looping)
			{
				this.loopingParticleSystems.previous();
			}
			else if (this.particleMode == DemoManager.ParticleMode.oneshot)
			{
				this.oneshotParticleSystems.previous();
			}
			this.updateCurrentParticleSystemNameText();
		}

		// Token: 0x04000133 RID: 307
		public Transform cameraRotationTransform;

		// Token: 0x04000134 RID: 308
		public Transform cameraTranslationTransform;

		// Token: 0x04000135 RID: 309
		public Vector3 cameraLookAtPosition = new Vector3(0f, 3f, 0f);

		// Token: 0x04000136 RID: 310
		public FollowMouse mouse;

		// Token: 0x04000137 RID: 311
		private Vector3 targetCameraPosition;

		// Token: 0x04000138 RID: 312
		private Vector3 targetCameraRotation;

		// Token: 0x04000139 RID: 313
		private Vector3 cameraPositionStart;

		// Token: 0x0400013A RID: 314
		private Vector3 cameraRotationStart;

		// Token: 0x0400013B RID: 315
		private Vector2 input;

		// Token: 0x0400013C RID: 316
		private Vector3 cameraRotation;

		// Token: 0x0400013D RID: 317
		public float cameraMoveAmount = 2f;

		// Token: 0x0400013E RID: 318
		public float cameraRotateAmount = 2f;

		// Token: 0x0400013F RID: 319
		public float cameraMoveSpeed = 12f;

		// Token: 0x04000140 RID: 320
		public float cameraRotationSpeed = 12f;

		// Token: 0x04000141 RID: 321
		public Vector2 cameraAngleLimits = new Vector2(-8f, 60f);

		// Token: 0x04000142 RID: 322
		public GameObject[] levels;

		// Token: 0x04000143 RID: 323
		public DemoManager.Level currentLevel = DemoManager.Level.basic;

		// Token: 0x04000144 RID: 324
		public DemoManager.ParticleMode particleMode;

		// Token: 0x04000145 RID: 325
		public bool lighting = true;

		// Token: 0x04000146 RID: 326
		public bool advancedRendering = true;

		// Token: 0x04000147 RID: 327
		public Toggle frontFacingCameraModeToggle;

		// Token: 0x04000148 RID: 328
		public Toggle interactiveCameraModeToggle;

		// Token: 0x04000149 RID: 329
		public Toggle loopingParticleModeToggle;

		// Token: 0x0400014A RID: 330
		public Toggle oneshotParticleModeToggle;

		// Token: 0x0400014B RID: 331
		public Toggle lightingToggle;

		// Token: 0x0400014C RID: 332
		public Toggle advancedRenderingToggle;

		// Token: 0x0400014D RID: 333
		private Toggle[] levelToggles;

		// Token: 0x0400014E RID: 334
		public ToggleGroup levelTogglesContainer;

		// Token: 0x0400014F RID: 335
		public LoopingParticleSystemsManager loopingParticleSystems;

		// Token: 0x04000150 RID: 336
		public OneshotParticleSystemsManager oneshotParticleSystems;

		// Token: 0x04000151 RID: 337
		public Text particleCountText;

		// Token: 0x04000152 RID: 338
		public Text currentParticleSystemText;

		// Token: 0x04000153 RID: 339
		public Text particleSpawnInstructionText;

		// Token: 0x04000154 RID: 340
		public Slider timeScaleSlider;

		// Token: 0x04000155 RID: 341
		public Text timeScaleSliderValueText;

		// Token: 0x04000156 RID: 342
		public Camera UICamera;

		// Token: 0x04000157 RID: 343
		public Camera mainCamera;

		// Token: 0x04000158 RID: 344
		public Camera postEffectsCamera;

		// Token: 0x04000159 RID: 345
		public MonoBehaviour[] mainCameraPostEffects;

		// Token: 0x0200003E RID: 62
		public enum ParticleMode
		{
			// Token: 0x0400015B RID: 347
			looping,
			// Token: 0x0400015C RID: 348
			oneshot
		}

		// Token: 0x0200003F RID: 63
		public enum Level
		{
			// Token: 0x0400015E RID: 350
			none,
			// Token: 0x0400015F RID: 351
			basic
		}
	}
}
