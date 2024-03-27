using System;
using System.Collections.Generic;

namespace DaikonForge.VoIP
{
	// Token: 0x02000022 RID: 34
	public class VoiceControllerCollection<T> where T : VoiceControllerBase
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00005253 File Offset: 0x00003653
		public static void RegisterVoiceController(T controller)
		{
			VoiceControllerCollection<T>.VoiceControllers.Add(controller);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005260 File Offset: 0x00003660
		public static void UnregisterVoiceController(T controller)
		{
			VoiceControllerCollection<T>.VoiceControllers.Remove(controller);
		}

		// Token: 0x04000072 RID: 114
		public static List<T> VoiceControllers = new List<T>();
	}
}
