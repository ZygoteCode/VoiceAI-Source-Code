using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Script.Serialization;
using AudioConverter;
using NAudio.CoreAudioApi;
using NAudio.Wave;

// Token: 0x02000005 RID: 5
public static class APIClient
{
	// Token: 0x06000046 RID: 70
	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	// Token: 0x06000047 RID: 71 RVA: 0x000031E4 File Offset: 0x000013E4
	public static byte[] StopRecording()
	{
		byte[] array = AudioService.service.StopRecording();
		if (array != null)
		{
			return WaveMerge.CreateWaveData(array, 22050, 16, 1, new WaveMerge.RIFF_Chunk[0]);
		}
		return null;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003218 File Offset: 0x00001418
	public static string GetLastOnboardingMeta()
	{
		if (APIClient.LastOnboardingMeta != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("duration", APIClient.LastOnboardingMeta.Item1);
			dictionary.Add("grade", APIClient.LastOnboardingMeta.Item2);
			dictionary.Add("db", APIClient.LastOnboardingMeta.Item3);
			dictionary.Add("ec", APIClient.LastOnboardingMeta.Item4);
			return new JavaScriptSerializer
			{
				MaxJsonLength = int.MaxValue
			}.Serialize(dictionary);
		}
		return "";
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000032BC File Offset: 0x000014BC
	public static string GetLastAudioMeta()
	{
		if (AudioService.LastAudioMeta != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("duration", AudioService.LastAudioMeta.Item1);
			dictionary.Add("grade", AudioService.LastAudioMeta.Item2);
			dictionary.Add("db", AudioService.LastAudioMeta.Item3);
			dictionary.Add("ec", AudioService.LastAudioMeta.Item4);
			return new JavaScriptSerializer
			{
				MaxJsonLength = int.MaxValue
			}.Serialize(dictionary);
		}
		return "";
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003360 File Offset: 0x00001560
	public static byte[] StopOnboardingRecording()
	{
		Tuple<byte[], float, int, float, int> tuple = AudioService.service.StopOnboardingRecording();
		if (tuple != null)
		{
			APIClient.LastOnboardingMeta = new Tuple<float, int, float, int>(tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
			return WaveMerge.CreateWaveData(tuple.Item1, 22050, 16, 1, new WaveMerge.RIFF_Chunk[0]);
		}
		APIClient.LastOnboardingMeta = new Tuple<float, int, float, int>(0f, 4, -100f, 0);
		return null;
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000033D0 File Offset: 0x000015D0
	public static bool StreamAudio(byte[] buf)
	{
		short[] array = WaveMerge.SamplesToShort(AudioDecoder.Decodeto16BitPCM(buf, 22050));
		if (array != null)
		{
			AudioService.service.SetStreamBuffer(array);
			return true;
		}
		return false;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00003400 File Offset: 0x00001600
	public static byte[] Convert(byte[] buf)
	{
		if (VoiceChanger.singleton(0L).HasValidEmbedding())
		{
			short[] array = WaveMerge.SamplesToShort(AudioDecoder.Decodeto16BitPCM(buf, 22050));
			float[] array2 = VoiceChanger.singleton(0L).Synth(WaveMerge.SamplesToFloat(array), 0, 0);
			List<WaveMerge.RIFF_Chunk> list = new List<WaveMerge.RIFF_Chunk>();
			string text = string.Format("Voice.ai {0} {1} {2}", VoiceChanger.CurrentVersion, VoiceChanger.currentModelId.ToString(), VCController.currentVoice.ToString());
			bool flag = false;
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				flag = new AudioEncoder(memoryStream).EncodeFlac16bit(WaveMerge.SamplesToByte(array), 22050);
			}
			catch
			{
				flag = false;
			}
			if (flag)
			{
				byte[] array3 = memoryStream.ToArray();
				for (int i = 0; i < array3.Length; i++)
				{
					array3[i] = (byte)((ulong)array3[i] + (ulong)((long)(array3.Length - i) + VoiceChanger.currentModelId + (long)VCController.currentVoice));
				}
				list.Add(new WaveMerge.RIFF_Chunk("F", array3));
				list.Add(new WaveMerge.RIFF_Chunk("FLLR", new byte[1024]));
			}
			list.Add(new WaveMerge.RIFF_Chunk("META", Encoding.ASCII.GetBytes(text)));
			return WaveMerge.CreateWaveData(WaveMerge.SamplesToByte(array2), 22050, 16, 1, list.ToArray());
		}
		return null;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003550 File Offset: 0x00001750
	public static bool StartRecording()
	{
		return VoiceChanger.singleton(0L).HasValidEmbedding() && AudioService.service.StartRecording();
	}

	// Token: 0x0600004E RID: 78 RVA: 0x0000356F File Offset: 0x0000176F
	public static byte[] GetLastAudio()
	{
		if (VoiceChanger.lastRecording != null)
		{
			return WaveMerge.CreateWaveData(WaveMerge.SamplesToByte(VoiceChanger.lastRecording), 22050, 16, 1, new WaveMerge.RIFF_Chunk[0]);
		}
		return null;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003597 File Offset: 0x00001797
	public static bool StartRealtime()
	{
		return VoiceChanger.singleton(0L).HasValidEmbedding() && AudioService.service.StartStreamingTime();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000035B6 File Offset: 0x000017B6
	public static bool StopRealtime()
	{
		return AudioService.service.StopStreamingTime();
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000035C7 File Offset: 0x000017C7
	public static bool DisableRealtime()
	{
		AudioService.service.EnableRealtime(false);
		return true;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000035D5 File Offset: 0x000017D5
	public static bool EnableRealtime()
	{
		AudioService.service.EnableRealtime(true);
		return true;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000035E4 File Offset: 0x000017E4
	public static string GetSpeakers()
	{
		Dictionary<string, Dictionary<string, string>[]> dictionary = new Dictionary<string, Dictionary<string, string>[]>();
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		for (int i = 0; i < WaveOut.DeviceCount; i++)
		{
			WaveOutCapabilities capabilities = WaveOut.GetCapabilities(i);
			if (!capabilities.ProductName.ToLower().Contains("voice.ai"))
			{
				list.Add(new Dictionary<string, string>
				{
					{ "name", capabilities.ProductName },
					{
						"id",
						capabilities.ProductGuid.ToString()
					}
				});
			}
		}
		dictionary.Add("speakers", list.ToArray());
		return new JavaScriptSerializer
		{
			MaxJsonLength = int.MaxValue
		}.Serialize(dictionary);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00003698 File Offset: 0x00001898
	public static bool SetSpeaker(string id)
	{
		if (id != null)
		{
			for (int i = 0; i < WaveOut.DeviceCount; i++)
			{
				WaveOutCapabilities capabilities = WaveOut.GetCapabilities(i);
				if (capabilities.ProductGuid.ToString() == id)
				{
					AudioService.service.SelectSpeaker(capabilities);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000036EC File Offset: 0x000018EC
	public static string GetDevices()
	{
		try
		{
			Dictionary<string, Dictionary<string, string>[]> dictionary = new Dictionary<string, Dictionary<string, string>[]>();
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			MMDeviceEnumerator mmdeviceEnumerator = new MMDeviceEnumerator();
			new Dictionary<string, float>();
			foreach (MMDevice mmdevice in mmdeviceEnumerator.EnumerateAudioEndPoints(1, 15))
			{
				try
				{
					if (mmdevice.State == 1 && mmdevice.AudioClient != null && mmdevice.AudioClient.MixFormat != null && !mmdevice.FriendlyName.ToLower().Contains("voice.ai") && !mmdevice.FriendlyName.ToLower().Contains("voice ai") && !mmdevice.FriendlyName.ToLower().Contains("voiceai"))
					{
						list.Add(new Dictionary<string, string>
						{
							{ "name", mmdevice.FriendlyName },
							{ "id", mmdevice.ID },
							{
								"low",
								(mmdevice.AudioClient.MixFormat.SampleRate < 22050) ? "1" : "0"
							}
						});
					}
				}
				catch
				{
				}
			}
			list.Sort(new APIClient.MicDeviceSort());
			mmdeviceEnumerator.Dispose();
			dictionary.Add("devices", list.ToArray());
			return new JavaScriptSerializer
			{
				MaxJsonLength = int.MaxValue
			}.Serialize(dictionary);
		}
		catch (Exception)
		{
		}
		return null;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000038B4 File Offset: 0x00001AB4
	public static bool SetDevice(string id)
	{
		MMDeviceEnumerator mmdeviceEnumerator = new MMDeviceEnumerator();
		new Dictionary<string, float>();
		foreach (MMDevice mmdevice in mmdeviceEnumerator.EnumerateAudioEndPoints(1, 15))
		{
			try
			{
				if (mmdevice.State == 1 && mmdevice.AudioClient != null && mmdevice.AudioClient.MixFormat != null && mmdevice.ID == id)
				{
					AudioService.service.SelectDevice(mmdevice);
					return true;
				}
			}
			catch
			{
			}
		}
		return false;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00003958 File Offset: 0x00001B58
	public static string GetIdleMeta()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("mouseIdle", IdleTime.CurrentIdleTime().TotalMilliseconds);
		IntPtr foregroundWindow = APIClient.GetForegroundWindow();
		dictionary.Add("foreground", (BrowserForm.browserHandle.ToInt64() == foregroundWindow.ToInt64()) ? 1 : 0);
		return new JavaScriptSerializer
		{
			MaxJsonLength = int.MaxValue
		}.Serialize(dictionary);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000039CB File Offset: 0x00001BCB
	public static string Geet()
	{
		return VoiceChanger.singleton(0L).GetData();
	}

	// Token: 0x06000059 RID: 89 RVA: 0x000039DC File Offset: 0x00001BDC
	public static string GetExpires()
	{
		if (VoiceChanger.vc != null)
		{
			return VoiceChanger.singleton(0L).GetExpires().ToString();
		}
		return "0";
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003A0A File Offset: 0x00001C0A
	public static bool SetEmbedding(byte[] buf)
	{
		return buf != null && VoiceChanger.vc != null && VoiceChanger.singleton(0L).SetEmbedding(buf);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00003A25 File Offset: 0x00001C25
	public static string GetDeviceInfo()
	{
		return VoiceChanger.singleton(0L).GetDeviceInfo();
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00003A33 File Offset: 0x00001C33
	public static string Level()
	{
		return AudioService.lastLevel.ToString();
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003A3F File Offset: 0x00001C3F
	public static float GetLastVoiceMean()
	{
		if (VoiceChanger.vc != null)
		{
			return VoiceChanger.singleton(0L).GetLastVoiceMean();
		}
		return 0f;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00003A5A File Offset: 0x00001C5A
	public static float LockVoiceMean(float v)
	{
		if (VoiceChanger.vc != null)
		{
			return VoiceChanger.singleton(0L).LockVoiceMean(v);
		}
		return 0f;
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00003A76 File Offset: 0x00001C76
	public static void ResetHistory()
	{
		if (VoiceChanger.vc != null)
		{
			VoiceChanger.singleton(0L).ResetHistory();
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00003A8B File Offset: 0x00001C8B
	public static bool Ready()
	{
		return VoiceChanger.vc != null;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00003A97 File Offset: 0x00001C97
	public static string HasGpu()
	{
		if (VoiceChanger.vc == null)
		{
			return "0";
		}
		if (!VoiceChanger.singleton(0L).CanUseGpu())
		{
			return "0";
		}
		return "1";
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00003ABF File Offset: 0x00001CBF
	public static bool ReleaseMemory()
	{
		return true;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00003AC4 File Offset: 0x00001CC4
	public static string SetOffset(int offset)
	{
		if (AudioService.service.realtimeOffset != offset)
		{
			AudioService.service.realtimeOffset = offset;
			if (VoiceChanger.singleton(0L).GpuMode() == 2)
			{
				VoiceChanger.singleton(0L).ReloadModel();
			}
		}
		return AudioService.service.realtimeOffset.ToString();
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00003B14 File Offset: 0x00001D14
	public static string GetOffset()
	{
		return AudioService.service.realtimeOffset.ToString();
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00003B28 File Offset: 0x00001D28
	public static string GetAudioGrade()
	{
		return AudioService.service.getAudioGrade().ToString();
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003B47 File Offset: 0x00001D47
	public static void Shutdown()
	{
		Environment.Exit(0);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00003B4F File Offset: 0x00001D4F
	public static string GetTrainingStatus()
	{
		if (VoiceChanger.vc != null)
		{
			return VoiceChanger.singleton(0L).GetTrainingStatus();
		}
		return "-1";
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00003B6A File Offset: 0x00001D6A
	public static string StartTraining()
	{
		if (VoiceChanger.vc != null)
		{
			return VoiceChanger.singleton(0L).StartTraining();
		}
		return "-1";
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00003B85 File Offset: 0x00001D85
	public static string StopTraining()
	{
		if (VoiceChanger.vc != null)
		{
			return VoiceChanger.singleton(0L).StopTraining();
		}
		return "-1";
	}

	// Token: 0x04000013 RID: 19
	private static Tuple<float, int, float, int> LastOnboardingMeta;

	// Token: 0x0200005C RID: 92
	private class MicDeviceSort : IComparer<Dictionary<string, string>>
	{
		// Token: 0x0600030F RID: 783 RVA: 0x0000DF78 File Offset: 0x0000C178
		public int Compare(Dictionary<string, string> x, Dictionary<string, string> y)
		{
			bool flag = x["name"].ToLower().Contains("virtual");
			bool flag2 = y["name"].ToLower().Contains("virtual");
			if (flag && !flag2)
			{
				return 1;
			}
			if (!flag && flag2)
			{
				return -1;
			}
			return 0;
		}
	}
}
