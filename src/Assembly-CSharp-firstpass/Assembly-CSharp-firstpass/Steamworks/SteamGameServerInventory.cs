using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200019E RID: 414
	public static class SteamGameServerInventory
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x000072C2 File Offset: 0x000054C2
		public static EResult GetResultStatus(SteamInventoryResult_t resultHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GetResultStatus(resultHandle);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000072CF File Offset: 0x000054CF
		public static bool GetResultItems(SteamInventoryResult_t resultHandle, SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GetResultItems(resultHandle, pOutItemsArray, ref punOutItemsArraySize);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000072DE File Offset: 0x000054DE
		public static uint GetResultTimestamp(SteamInventoryResult_t resultHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GetResultTimestamp(resultHandle);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000072EB File Offset: 0x000054EB
		public static bool CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_CheckResultSteamID(resultHandle, steamIDExpected);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000072F9 File Offset: 0x000054F9
		public static void DestroyResult(SteamInventoryResult_t resultHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerInventory_DestroyResult(resultHandle);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00007306 File Offset: 0x00005506
		public static bool GetAllItems(out SteamInventoryResult_t pResultHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GetAllItems(out pResultHandle);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00007313 File Offset: 0x00005513
		public static bool GetItemsByID(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GetItemsByID(out pResultHandle, pInstanceIDs, unCountInstanceIDs);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00007322 File Offset: 0x00005522
		public static bool SerializeResult(SteamInventoryResult_t resultHandle, byte[] pOutBuffer, out uint punOutBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_SerializeResult(resultHandle, pOutBuffer, out punOutBufferSize);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00007331 File Offset: 0x00005531
		public static bool DeserializeResult(out SteamInventoryResult_t pOutResultHandle, byte[] pBuffer, uint unBufferSize, bool bRESERVED_MUST_BE_FALSE = false)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_DeserializeResult(out pOutResultHandle, pBuffer, unBufferSize, bRESERVED_MUST_BE_FALSE);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00007341 File Offset: 0x00005541
		public static bool GenerateItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayItemDefs, uint[] punArrayQuantity, uint unArrayLength)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GenerateItems(out pResultHandle, pArrayItemDefs, punArrayQuantity, unArrayLength);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00007351 File Offset: 0x00005551
		public static bool GrantPromoItems(out SteamInventoryResult_t pResultHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GrantPromoItems(out pResultHandle);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0000735E File Offset: 0x0000555E
		public static bool AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_AddPromoItem(out pResultHandle, itemDef);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0000736C File Offset: 0x0000556C
		public static bool AddPromoItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayItemDefs, uint unArrayLength)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_AddPromoItems(out pResultHandle, pArrayItemDefs, unArrayLength);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0000737B File Offset: 0x0000557B
		public static bool ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_ConsumeItem(out pResultHandle, itemConsume, unQuantity);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0000738A File Offset: 0x0000558A
		public static bool ExchangeItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayGenerate, uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, SteamItemInstanceID_t[] pArrayDestroy, uint[] punArrayDestroyQuantity, uint unArrayDestroyLength)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_ExchangeItems(out pResultHandle, pArrayGenerate, punArrayGenerateQuantity, unArrayGenerateLength, pArrayDestroy, punArrayDestroyQuantity, unArrayDestroyLength);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000073A0 File Offset: 0x000055A0
		public static bool TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_TransferItemQuantity(out pResultHandle, itemIdSource, unQuantity, itemIdDest);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000073B0 File Offset: 0x000055B0
		public static void SendItemDropHeartbeat()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerInventory_SendItemDropHeartbeat();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x000073BC File Offset: 0x000055BC
		public static bool TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_TriggerItemDrop(out pResultHandle, dropListDefinition);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000073CA File Offset: 0x000055CA
		public static bool TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, SteamItemInstanceID_t[] pArrayGive, uint[] pArrayGiveQuantity, uint nArrayGiveLength, SteamItemInstanceID_t[] pArrayGet, uint[] pArrayGetQuantity, uint nArrayGetLength)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_TradeItems(out pResultHandle, steamIDTradePartner, pArrayGive, pArrayGiveQuantity, nArrayGiveLength, pArrayGet, pArrayGetQuantity, nArrayGetLength);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000073E2 File Offset: 0x000055E2
		public static bool LoadItemDefinitions()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_LoadItemDefinitions();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000073EE File Offset: 0x000055EE
		public static bool GetItemDefinitionIDs(SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GetItemDefinitionIDs(pItemDefIDs, out punItemDefIDsArraySize);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000073FC File Offset: 0x000055FC
		public static bool GetItemDefinitionProperty(SteamItemDef_t iDefinition, string pchPropertyName, out string pchValueBuffer, ref uint punValueBufferSizeOut)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)punValueBufferSizeOut);
			bool flag2;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPropertyName))
			{
				bool flag = NativeMethods.ISteamGameServerInventory_GetItemDefinitionProperty(iDefinition, utf8StringHandle, intPtr, ref punValueBufferSizeOut);
				pchValueBuffer = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
				Marshal.FreeHGlobal(intPtr);
				flag2 = flag;
			}
			return flag2;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00007468 File Offset: 0x00005668
		public static SteamAPICall_t RequestEligiblePromoItemDefinitionsIDs(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerInventory_RequestEligiblePromoItemDefinitionsIDs(steamID);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0000747A File Offset: 0x0000567A
		public static bool GetEligiblePromoItemDefinitionIDs(CSteamID steamID, SteamItemDef_t[] pItemDefIDs, ref uint punItemDefIDsArraySize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerInventory_GetEligiblePromoItemDefinitionIDs(steamID, pItemDefIDs, ref punItemDefIDsArraySize);
		}
	}
}
