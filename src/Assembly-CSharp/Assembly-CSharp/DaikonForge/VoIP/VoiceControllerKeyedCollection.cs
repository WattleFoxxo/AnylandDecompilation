using System;
using System.Collections.Generic;

namespace DaikonForge.VoIP
{
	// Token: 0x02000023 RID: 35
	public class VoiceControllerKeyedCollection<K, T> where T : VoiceControllerBase
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00005282 File Offset: 0x00003682
		public static void RegisterVoiceController(K key, T controller)
		{
			VoiceControllerKeyedCollection<K, T>.voiceControllers.Add(key, controller);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005290 File Offset: 0x00003690
		public static void UnregisterVoiceController(K key)
		{
			VoiceControllerKeyedCollection<K, T>.voiceControllers.Remove(key);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000529E File Offset: 0x0000369E
		public static T GetVoiceController(K key)
		{
			if (!VoiceControllerKeyedCollection<K, T>.voiceControllers.ContainsKey(key))
			{
				return (T)((object)null);
			}
			return VoiceControllerKeyedCollection<K, T>.voiceControllers[key];
		}

		// Token: 0x04000073 RID: 115
		protected static Dictionary<K, T> voiceControllers = new Dictionary<K, T>();
	}
}
