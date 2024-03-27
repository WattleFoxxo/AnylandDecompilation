using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class ExtrasManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060010D5 RID: 4309 RVA: 0x00091C36 File Offset: 0x00090036
	// (set) Token: 0x060010D6 RID: 4310 RVA: 0x00091C3E File Offset: 0x0009003E
	public ManagerStatus status { get; private set; }

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x060010D7 RID: 4311 RVA: 0x00091C47 File Offset: 0x00090047
	// (set) Token: 0x060010D8 RID: 4312 RVA: 0x00091C4F File Offset: 0x0009004F
	public string failMessage { get; private set; }

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060010D9 RID: 4313 RVA: 0x00091C58 File Offset: 0x00090058
	// (set) Token: 0x060010DA RID: 4314 RVA: 0x00091C60 File Offset: 0x00090060
	public bool transactionInProgress { get; private set; }

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x060010DB RID: 4315 RVA: 0x00091C69 File Offset: 0x00090069
	// (set) Token: 0x060010DC RID: 4316 RVA: 0x00091C71 File Offset: 0x00090071
	public TransactionType transactionTypeInProgress { get; private set; }

	// Token: 0x060010DD RID: 4317 RVA: 0x00091C7C File Offset: 0x0009007C
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		if (Managers.steamManager.status == ManagerStatus.Started)
		{
			this.steamCallback_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(this.OnMicroTxnAuthorizationResponse));
		}
		else
		{
			Log.Error("SteamManager was not initialized at ExtrasManager start!");
		}
		this.status = ManagerStatus.Started;
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x00091CD0 File Offset: 0x000900D0
	public void StartEditToolsTrial(Action<ResponseError> callback)
	{
		base.StartCoroutine(Managers.serverManager.StartEditToolsTrial(delegate(StartEditToolsTrial_Response response)
		{
			ResponseError responseError = null;
			if (!string.IsNullOrEmpty(response.error) || !response.ok)
			{
				responseError = new ResponseError(response.error, response.httpResponseCode, response.reasonFailed);
			}
			if (responseError == null)
			{
				Person ourPerson = Managers.personManager.ourPerson;
				ourPerson.hasEditTools = true;
				ourPerson.isInEditToolsTrial = true;
				ourPerson.wasEditToolsTrialEverActivated = true;
				ourPerson.editToolsExpiryDate = response.editToolsExpiryDate;
			}
			callback(responseError);
		}));
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x00091D08 File Offset: 0x00090108
	public void StartEditToolsPurchase(Action<ResponseError> callback)
	{
		base.StartCoroutine(Managers.serverManager.StartEditToolsPurchase(delegate(StartEditToolsPurchase_Response response)
		{
			ResponseError responseError = null;
			if (!string.IsNullOrEmpty(response.error) || !response.ok)
			{
				responseError = new ResponseError(response.error, response.httpResponseCode, response.reasonFailed);
			}
			if (responseError == null)
			{
				this.transactionInProgress = true;
				this.transactionTypeInProgress = TransactionType.EDIT_TOOLS_1YR;
			}
			callback(responseError);
		}));
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x00091D48 File Offset: 0x00090148
	public void SubmitGiftAndStartPurchase(string toUserId, string thingId, Vector3 position, Vector3 rotation, bool isPrivate, Action<ResponseError> callback)
	{
		ExtrasManager.<SubmitGiftAndStartPurchase>c__AnonStorey3 <SubmitGiftAndStartPurchase>c__AnonStorey = new ExtrasManager.<SubmitGiftAndStartPurchase>c__AnonStorey3();
		<SubmitGiftAndStartPurchase>c__AnonStorey.callback = callback;
		<SubmitGiftAndStartPurchase>c__AnonStorey.$this = this;
		Managers.achievementManager.RegisterAchievement(Achievement.ClickedSubmitGift);
		base.StartCoroutine(Managers.serverManager.SubmitGift(toUserId, thingId, position, rotation, isPrivate, delegate(SubmitGift_Response response)
		{
			ResponseError responseError = null;
			if (!string.IsNullOrEmpty(response.error) || !response.ok)
			{
				responseError = new ResponseError(response.error, response.httpResponseCode, string.Empty);
			}
			if (responseError == null)
			{
				string giftId = response.giftId;
				<SubmitGiftAndStartPurchase>c__AnonStorey.$this.StartCoroutine(Managers.serverManager.StartPurchase(TransactionType.GIFT, giftId, delegate(StartPurchase_Response paymentResponse)
				{
					if (!string.IsNullOrEmpty(paymentResponse.error) || !paymentResponse.ok)
					{
						responseError = new ResponseError(paymentResponse.error, paymentResponse.httpResponseCode, string.Empty);
					}
					if (responseError == null)
					{
						<SubmitGiftAndStartPurchase>c__AnonStorey.transactionInProgress = true;
						<SubmitGiftAndStartPurchase>c__AnonStorey.transactionTypeInProgress = TransactionType.GIFT;
					}
					<SubmitGiftAndStartPurchase>c__AnonStorey.callback(responseError);
				}));
			}
			else
			{
				<SubmitGiftAndStartPurchase>c__AnonStorey.callback(responseError);
			}
		}));
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x00091D9C File Offset: 0x0009019C
	private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback)
	{
		bool flag = pCallback.m_bAuthorized != 0;
		string text = pCallback.m_ulOrderID.ToString();
		Log.Info("Got OnMicroTxnAuthorizationResponse (Indicating user responded to overlay)", false);
		Log.Info("Was Authorized? : " + flag.ToString(), false);
		Log.Info("ourSteamTransactionId : " + text.ToString(), false);
		if (flag)
		{
			this.CompletePurchase(text);
		}
		else
		{
			this.CancelPurchase(text);
		}
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x00091E21 File Offset: 0x00090221
	private void CompletePurchase(string ourSteamTransactionId)
	{
		base.StartCoroutine(Managers.serverManager.CompletePurchase(ourSteamTransactionId, delegate(CompletePurchase_Response response)
		{
			if (this.transactionTypeInProgress == TransactionType.EDIT_TOOLS_1YR)
			{
				if (response.error == null)
				{
					if (response.ok)
					{
						this.SetOurPersonParametersToHasEditTools();
						this.ShowPurchaseSuccessDialog("✔  Creation Tools purchased!\n     Thank you for supporting Anyland!", "EditToolsPurchaseSuccess", DialogType.Create);
					}
				}
				else
				{
					Managers.errorManager.ShowCriticalHaltError(response.error, true, false, true, false, false);
				}
			}
			else if (this.transactionTypeInProgress == TransactionType.GIFT)
			{
				if (response.error == null)
				{
					if (response.ok)
					{
						this.ShowPurchaseSuccessDialog("✔  Nift purchased & sent! Thank you\n     for supporting friends & Anyland!", "GiftPurchaseSuccess", DialogType.None);
					}
				}
				else
				{
					Managers.errorManager.ShowCriticalHaltError(response.error, true, false, true, false, false);
				}
			}
			else
			{
				Log.Error("CompletePurchase called for unsuitable transaction type : " + this.transactionTypeInProgress.ToString());
			}
			this.transactionInProgress = false;
		}));
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x00091E44 File Offset: 0x00090244
	private void SetOurPersonParametersToHasEditTools()
	{
		Person ourPerson = Managers.personManager.ourPerson;
		ourPerson.hasEditTools = true;
		ourPerson.isInEditToolsTrial = false;
		ourPerson.editToolsExpiryDate = Misc.GetDateStringXYearsInFuture(1);
		ourPerson.timesEditToolsPurchased++;
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x00091E84 File Offset: 0x00090284
	private void ShowPurchaseSuccessDialog(string message, string icon = null, DialogType dialogToReturnTo = DialogType.None)
	{
		AlertDialog alertDialog = Managers.dialogManager.ShowInfo(message, true, false, 2, DialogType.None, 1f, false, TextColor.Gold, TextAlignment.Left);
		if (alertDialog != null && icon != null)
		{
			Managers.soundManager.Play("purchaseSuccess", alertDialog.transform.parent, 0.2f, true, false);
			string text = "Prefabs/" + icon;
			GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load(text, typeof(GameObject))) as GameObject;
			gameObject.transform.parent = alertDialog.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Misc.GetUniformVector3(0.1f);
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.Translate(new Vector3(0f, 0.015f, 0f));
			gameObject.transform.Rotate(new Vector3(90f, 0f, 0f));
		}
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x00091F87 File Offset: 0x00090387
	private void CancelPurchase(string transactionId)
	{
		base.StartCoroutine(Managers.serverManager.CancelPurchase(transactionId, delegate(CancelPurchase_Response response)
		{
			this.transactionInProgress = false;
			if (response.error == null)
			{
				if (response.ok)
				{
				}
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x00091FA8 File Offset: 0x000903A8
	public void GetReceivedGifts(string userId, Action<List<GiftInfo>> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetReceivedGifts(userId, delegate(GetReceivedGifts_Response response)
		{
			if (response.error == null)
			{
				callback(response.gifts);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x00091FE0 File Offset: 0x000903E0
	public void ToggleGiftPrivacy(string giftId, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.ToggleGiftPrivacy(giftId, delegate(ToggleGiftPrivacy_Response response)
		{
			if (response.error == null)
			{
				callback(response.isPrivate);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x00092018 File Offset: 0x00090418
	public void MarkGiftSeen(string giftId, Action<bool, string> callback)
	{
		base.StartCoroutine(Managers.serverManager.MarkGiftSeen(giftId, delegate(AcknowledgeOperation_Response response)
		{
			if (response.error == null)
			{
				callback(response.ok, response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x040010CF RID: 4303
	protected Callback<MicroTxnAuthorizationResponse_t> steamCallback_MicroTxnAuthorizationResponse;
}
