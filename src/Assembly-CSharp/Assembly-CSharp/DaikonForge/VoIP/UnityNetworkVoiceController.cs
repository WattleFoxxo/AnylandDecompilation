using System;
using UnityEngine;

namespace DaikonForge.VoIP
{
	// Token: 0x02000010 RID: 16
	public class UnityNetworkVoiceController : VoiceControllerBase
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0000314B File Offset: 0x0000154B
		public override bool IsLocal
		{
			get
			{
				return base.GetComponent<NetworkView>().isMine;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003158 File Offset: 0x00001558
		protected override void Awake()
		{
			base.Awake();
			VoiceControllerCollection<UnityNetworkVoiceController>.RegisterVoiceController(this);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003166 File Offset: 0x00001566
		protected override void OnDestroy()
		{
			base.OnDestroy();
			VoiceControllerCollection<UnityNetworkVoiceController>.UnregisterVoiceController(this);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003174 File Offset: 0x00001574
		protected override void OnAudioDataEncoded(VoicePacketWrapper encodedFrame)
		{
			byte[] array = encodedFrame.ObtainHeaders();
			base.GetComponent<NetworkView>().RPC("vc", RPCMode.All, new object[] { array, encodedFrame.RawData });
			encodedFrame.ReleaseHeaders();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000031B8 File Offset: 0x000015B8
		[PunRPC]
		private void vc(byte[] headers, byte[] rawData)
		{
			VoicePacketWrapper voicePacketWrapper = new VoicePacketWrapper(headers, rawData);
			this.ReceiveAudioData(voicePacketWrapper);
		}
	}
}
