using System;

namespace Steamworks
{
	// Token: 0x0200019A RID: 410
	public static class SteamController
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x000060C0 File Offset: 0x000042C0
		public static bool Init()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_Init();
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000060CC File Offset: 0x000042CC
		public static bool Shutdown()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_Shutdown();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000060D8 File Offset: 0x000042D8
		public static void RunFrame()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_RunFrame();
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000060E4 File Offset: 0x000042E4
		public static int GetConnectedControllers(ControllerHandle_t[] handlesOut)
		{
			InteropHelp.TestIfAvailableClient();
			if (handlesOut.Length != 16)
			{
				throw new ArgumentException("handlesOut must be the same size as Constants.STEAM_CONTROLLER_MAX_COUNT!");
			}
			return NativeMethods.ISteamController_GetConnectedControllers(handlesOut);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00006106 File Offset: 0x00004306
		public static bool ShowBindingPanel(ControllerHandle_t controllerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_ShowBindingPanel(controllerHandle);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00006114 File Offset: 0x00004314
		public static ControllerActionSetHandle_t GetActionSetHandle(string pszActionSetName)
		{
			InteropHelp.TestIfAvailableClient();
			ControllerActionSetHandle_t controllerActionSetHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionSetName))
			{
				controllerActionSetHandle_t = (ControllerActionSetHandle_t)NativeMethods.ISteamController_GetActionSetHandle(utf8StringHandle);
			}
			return controllerActionSetHandle_t;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0000615C File Offset: 0x0000435C
		public static void ActivateActionSet(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_ActivateActionSet(controllerHandle, actionSetHandle);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0000616A File Offset: 0x0000436A
		public static ControllerActionSetHandle_t GetCurrentActionSet(ControllerHandle_t controllerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return (ControllerActionSetHandle_t)NativeMethods.ISteamController_GetCurrentActionSet(controllerHandle);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0000617C File Offset: 0x0000437C
		public static ControllerDigitalActionHandle_t GetDigitalActionHandle(string pszActionName)
		{
			InteropHelp.TestIfAvailableClient();
			ControllerDigitalActionHandle_t controllerDigitalActionHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionName))
			{
				controllerDigitalActionHandle_t = (ControllerDigitalActionHandle_t)NativeMethods.ISteamController_GetDigitalActionHandle(utf8StringHandle);
			}
			return controllerDigitalActionHandle_t;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000061C4 File Offset: 0x000043C4
		public static ControllerDigitalActionData_t GetDigitalActionData(ControllerHandle_t controllerHandle, ControllerDigitalActionHandle_t digitalActionHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetDigitalActionData(controllerHandle, digitalActionHandle);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000061D2 File Offset: 0x000043D2
		public static int GetDigitalActionOrigins(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle, ControllerDigitalActionHandle_t digitalActionHandle, EControllerActionOrigin[] originsOut)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetDigitalActionOrigins(controllerHandle, actionSetHandle, digitalActionHandle, originsOut);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000061E4 File Offset: 0x000043E4
		public static ControllerAnalogActionHandle_t GetAnalogActionHandle(string pszActionName)
		{
			InteropHelp.TestIfAvailableClient();
			ControllerAnalogActionHandle_t controllerAnalogActionHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszActionName))
			{
				controllerAnalogActionHandle_t = (ControllerAnalogActionHandle_t)NativeMethods.ISteamController_GetAnalogActionHandle(utf8StringHandle);
			}
			return controllerAnalogActionHandle_t;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0000622C File Offset: 0x0000442C
		public static ControllerAnalogActionData_t GetAnalogActionData(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t analogActionHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetAnalogActionData(controllerHandle, analogActionHandle);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0000623A File Offset: 0x0000443A
		public static int GetAnalogActionOrigins(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle, ControllerAnalogActionHandle_t analogActionHandle, EControllerActionOrigin[] originsOut)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetAnalogActionOrigins(controllerHandle, actionSetHandle, analogActionHandle, originsOut);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0000624A File Offset: 0x0000444A
		public static void StopAnalogActionMomentum(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t eAction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_StopAnalogActionMomentum(controllerHandle, eAction);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00006258 File Offset: 0x00004458
		public static void TriggerHapticPulse(ControllerHandle_t controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_TriggerHapticPulse(controllerHandle, eTargetPad, usDurationMicroSec);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00006267 File Offset: 0x00004467
		public static void TriggerRepeatedHapticPulse(ControllerHandle_t controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec, ushort usOffMicroSec, ushort unRepeat, uint nFlags)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_TriggerRepeatedHapticPulse(controllerHandle, eTargetPad, usDurationMicroSec, usOffMicroSec, unRepeat, nFlags);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0000627B File Offset: 0x0000447B
		public static void TriggerVibration(ControllerHandle_t controllerHandle, ushort usLeftSpeed, ushort usRightSpeed)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_TriggerVibration(controllerHandle, usLeftSpeed, usRightSpeed);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0000628A File Offset: 0x0000448A
		public static void SetLEDColor(ControllerHandle_t controllerHandle, byte nColorR, byte nColorG, byte nColorB, uint nFlags)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_SetLEDColor(controllerHandle, nColorR, nColorG, nColorB, nFlags);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0000629C File Offset: 0x0000449C
		public static int GetGamepadIndexForController(ControllerHandle_t ulControllerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetGamepadIndexForController(ulControllerHandle);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000062A9 File Offset: 0x000044A9
		public static ControllerHandle_t GetControllerForGamepadIndex(int nIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (ControllerHandle_t)NativeMethods.ISteamController_GetControllerForGamepadIndex(nIndex);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000062BB File Offset: 0x000044BB
		public static ControllerMotionData_t GetMotionData(ControllerHandle_t controllerHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetMotionData(controllerHandle);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000062C8 File Offset: 0x000044C8
		public static bool ShowDigitalActionOrigins(ControllerHandle_t controllerHandle, ControllerDigitalActionHandle_t digitalActionHandle, float flScale, float flXPosition, float flYPosition)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_ShowDigitalActionOrigins(controllerHandle, digitalActionHandle, flScale, flXPosition, flYPosition);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000062DA File Offset: 0x000044DA
		public static bool ShowAnalogActionOrigins(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t analogActionHandle, float flScale, float flXPosition, float flYPosition)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_ShowAnalogActionOrigins(controllerHandle, analogActionHandle, flScale, flXPosition, flYPosition);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000062EC File Offset: 0x000044EC
		public static string GetStringForActionOrigin(EControllerActionOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamController_GetStringForActionOrigin(eOrigin));
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000062FE File Offset: 0x000044FE
		public static string GetGlyphForActionOrigin(EControllerActionOrigin eOrigin)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamController_GetGlyphForActionOrigin(eOrigin));
		}
	}
}
