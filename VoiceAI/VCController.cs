using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AudioConverter;

// Token: 0x02000016 RID: 22
public class VCController
{
	// Token: 0x06000167 RID: 359 RVA: 0x00008D14 File Offset: 0x00006F14
	private static void expiresCheck(object state)
	{
		if (!Program.isActive)
		{
			VCController.expiresTimer.Dispose();
			return;
		}
		new VCController();
		string expires = APIClient.GetExpires();
		if (expires == null)
		{
			return;
		}
		if (int.Parse(expires) < 30)
		{
			VCController.expiresTimer.Dispose();
			if (BrowserForm.browserform != null)
			{
				BrowserForm.browserform.FireEvent(1, VCController.currentVoice);
			}
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00008D73 File Offset: 0x00006F73
	public bool startRecording()
	{
		return APIClient.StartRecording();
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00008D7C File Offset: 0x00006F7C
	private void SaveAudio(byte[] original, byte[] conversion, string ext)
	{
		try
		{
			if (original != null && conversion != null && conversion.Length > 1000 && original.Length > 1000)
			{
				string text = DateTime.Now.ToString("yyyy-MM-dd_HH-m-ss");
				string text2 = "original_" + text + ".wav";
				string text3 = "audio_" + text + "." + ext;
				string text4 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Voice.ai", text2);
				string text5 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Voice.ai", text3);
				Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Voice.ai"));
				File.WriteAllBytes(text4, original);
				File.WriteAllBytes(text5, conversion);
			}
		}
		catch
		{
		}
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00008E40 File Offset: 0x00007040
	public void startOnboardingRecording()
	{
		this.startRecording();
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00008E49 File Offset: 0x00007049
	public string getLastOnboardingMeta()
	{
		return APIClient.GetLastOnboardingMeta();
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00008E50 File Offset: 0x00007050
	public string getLastAudioMeta()
	{
		APIClient.GetAudioGrade();
		return APIClient.GetLastAudioMeta();
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00008E60 File Offset: 0x00007060
	public string stopOnboardingRecording()
	{
		byte[] array = APIClient.StopOnboardingRecording();
		if (array != null)
		{
			return Convert.ToBase64String(array);
		}
		return "";
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00008E84 File Offset: 0x00007084
	public string stopRecording()
	{
		byte[] array = APIClient.StopRecording();
		if (array != null && array.Length == 7 && Encoding.ASCII.GetString(array) == "invalid")
		{
			return "";
		}
		if (array != null)
		{
			if (VCController.currentFormat == VCController.AUDIO_FORMATS.MP3)
			{
				array = MP3Encoder.ToMp3(array, 0, 0, 22050, VCController.currentBitrate);
			}
			this.SaveAudio(this.GetLastAudioRaw(), array, VCController.currentFormat.ToString().ToLower());
			return Convert.ToBase64String(array);
		}
		return "";
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00008F09 File Offset: 0x00007109
	public string getDevices()
	{
		return APIClient.GetDevices();
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00008F10 File Offset: 0x00007110
	private byte[] GetLastAudioRaw()
	{
		return APIClient.GetLastAudio();
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00008F18 File Offset: 0x00007118
	public string getLastAudio()
	{
		byte[] lastAudioRaw = this.GetLastAudioRaw();
		if (lastAudioRaw != null && lastAudioRaw.Length == 7 && Encoding.ASCII.GetString(lastAudioRaw) == "invalid")
		{
			return "";
		}
		if (lastAudioRaw != null)
		{
			return Convert.ToBase64String(lastAudioRaw);
		}
		return "";
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00008F61 File Offset: 0x00007161
	public string getOffset()
	{
		return APIClient.GetOffset();
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00008F68 File Offset: 0x00007168
	public void setDevice(string id)
	{
		APIClient.SetDevice(id);
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00008F71 File Offset: 0x00007171
	public string setOffset(int offset)
	{
		return APIClient.SetOffset(offset);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00008F79 File Offset: 0x00007179
	private void downloadStatus(long id, float percentage, int complete, int error)
	{
		BrowserForm.browserform.downloadStatus(id, percentage, complete, error);
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00008F8C File Offset: 0x0000718C
	public bool loadModel(long id)
	{
		if (!AudioService.service.isRealtime && VoiceChanger.vc != null)
		{
			if (VoiceChanger.currentModelId == id)
			{
				return true;
			}
			string text = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voice.ai", "Models"), string.Format("model_{0}.vai", id.ToString()));
			if (id == 0L || File.Exists(text))
			{
				VoiceChanger.vc.Dispose();
				VoiceChanger.singleton(id);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00009004 File Offset: 0x00007204
	public bool downloadModel(long id, string url)
	{
		if (this.isDownloading || id < 1L || AudioService.service.isRealtime)
		{
			return false;
		}
		this.isDownloading = true;
		string filename = Path.GetFileNameWithoutExtension(url).ToLower();
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voice.ai", "Models");
		Directory.CreateDirectory(text);
		WebClient webClient = new WebClient();
		webClient.DownloadProgressChanged += delegate(object sender, DownloadProgressChangedEventArgs e)
		{
			this.downloadStatus(id, (float)e.ProgressPercentage / 100f, 0, 0);
		};
		string savePathModel = Path.Combine(text, string.Format("model_{0}.vai", id.ToString()));
		webClient.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
		{
			if (!e.Cancelled && e.Error == null && VoiceChanger.sha256(File.ReadAllBytes(savePathModel)).ToLower() == filename)
			{
				this.isDownloading = false;
				this.downloadStatus(id, 1f, 1, 0);
				return;
			}
			this.isDownloading = false;
			this.downloadStatus(id, 1f, 1, 1);
			File.Delete(savePathModel);
		};
		if (File.Exists(savePathModel))
		{
			if (VoiceChanger.sha256(File.ReadAllBytes(savePathModel)).ToLower() == filename)
			{
				this.isDownloading = false;
				this.downloadStatus(id, 1f, 1, 0);
				return true;
			}
			File.Delete(savePathModel);
		}
		webClient.DownloadFileAsync(new Uri(url), savePathModel);
		return true;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00009130 File Offset: 0x00007330
	public bool validateModel(long id, string url)
	{
		string text = Path.GetFileNameWithoutExtension(url).ToLower();
		string text2 = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voice.ai", "Models"), string.Format("model_{0}.vai", id.ToString()));
		if (File.Exists(text2))
		{
			if (VoiceChanger.sha256(File.ReadAllBytes(text2)).ToLower() == text)
			{
				return true;
			}
			this.deleteModel(id);
		}
		return false;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x000091A4 File Offset: 0x000073A4
	public bool deleteModel(long id)
	{
		string text = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voice.ai", "Models"), string.Format("model_{0}.vai", id.ToString()));
		if (File.Exists(text))
		{
			try
			{
				File.Delete(text);
				return true;
			}
			catch
			{
			}
			return false;
		}
		return false;
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00009208 File Offset: 0x00007408
	public string setEmbedding(string url)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return "0";
		}
		new WebClient().Proxy = null;
		string[] array = url.Split(new char[] { ';' });
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(BitConverter.GetBytes(uint.Parse(array[0])), 0, 4);
		byte[] array2 = Convert.FromBase64String(array[1]);
		memoryStream.Write(array2, 0, array2.Length);
		if (APIClient.SetEmbedding(memoryStream.ToArray()))
		{
			if (VCController.expiresTimer != null)
			{
				VCController.expiresTimer.Dispose();
			}
			VCController.expiresTimer = new System.Threading.Timer(new TimerCallback(VCController.expiresCheck), null, 1000, 1000);
			return "1";
		}
		return "0";
	}

	// Token: 0x0600017B RID: 379 RVA: 0x000092B7 File Offset: 0x000074B7
	public void shutDown()
	{
		APIClient.Shutdown();
	}

	// Token: 0x0600017C RID: 380 RVA: 0x000092C0 File Offset: 0x000074C0
	public string convert(string data)
	{
		byte[] array = APIClient.Convert(Convert.FromBase64String(data.Split(new string[] { "base64," }, StringSplitOptions.None)[1]));
		if (array == null)
		{
			return "";
		}
		if (VCController.currentFormat == VCController.AUDIO_FORMATS.MP3)
		{
			array = MP3Encoder.ToMp3(array, 0, 0, 22050, VCController.currentBitrate);
		}
		this.SaveAudio(this.GetLastAudioRaw(), array, VCController.currentFormat.ToString().ToLower());
		return Convert.ToBase64String(array);
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000933B File Offset: 0x0000753B
	public string streamAudio(string data)
	{
		if (APIClient.StreamAudio(Convert.FromBase64String(data.Split(new string[] { "base64," }, StringSplitOptions.None)[1])))
		{
			return "1";
		}
		return "0";
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000936B File Offset: 0x0000756B
	public float SetMicVolume(double v)
	{
		if (v > 1.0)
		{
			v = Math.Min(1.0, v / 100.0);
		}
		return AudioService.service.SetMicLevel((float)v);
	}

	// Token: 0x0600017F RID: 383 RVA: 0x000093A0 File Offset: 0x000075A0
	public float GetMicVolume()
	{
		return AudioService.service.GetMicLevel();
	}

	// Token: 0x06000180 RID: 384 RVA: 0x000093AC File Offset: 0x000075AC
	public void clipboardSetText(string text)
	{
		if (!string.IsNullOrWhiteSpace(text))
		{
			Thread thread = new Thread(delegate
			{
				try
				{
					Clipboard.SetText(text, TextDataFormat.Text);
				}
				catch
				{
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}
	}

	// Token: 0x06000181 RID: 385 RVA: 0x000093F6 File Offset: 0x000075F6
	public string clipboardGetText()
	{
		Thread thread = new Thread(delegate
		{
			try
			{
				VCController.lastClipboard = Clipboard.GetText(TextDataFormat.Text);
			}
			catch
			{
			}
		});
		thread.SetApartmentState(ApartmentState.STA);
		thread.Start();
		thread.Join();
		return VCController.lastClipboard;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00009433 File Offset: 0x00007633
	public string GetLevel()
	{
		return APIClient.Level();
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000943A File Offset: 0x0000763A
	public string HasGpu()
	{
		return APIClient.HasGpu();
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00009444 File Offset: 0x00007644
	public string GetDeviceInfo()
	{
		string deviceInfo = APIClient.GetDeviceInfo();
		if (deviceInfo == null || string.IsNullOrWhiteSpace(deviceInfo))
		{
			return "[]";
		}
		return deviceInfo;
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00009469 File Offset: 0x00007669
	public bool isReady()
	{
		return APIClient.Ready();
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00009470 File Offset: 0x00007670
	public string geet(int voiceId)
	{
		VCController.currentVoice = voiceId;
		string text = APIClient.Geet();
		if (text != null)
		{
			return text;
		}
		return "";
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00009493 File Offset: 0x00007693
	public void startRealtime()
	{
		APIClient.StartRealtime();
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000949B File Offset: 0x0000769B
	public void stopRealtime()
	{
		APIClient.StopRealtime();
	}

	// Token: 0x06000189 RID: 393 RVA: 0x000094A3 File Offset: 0x000076A3
	public void setMinimizeOnClose(bool minimizeOnClose)
	{
		Utilities.Singleton().SetMinimizeOnClose(minimizeOnClose);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x000094B0 File Offset: 0x000076B0
	public bool getMinimizeOnClose()
	{
		return Utilities.Singleton().GetMinimizeOnClose();
	}

	// Token: 0x0600018B RID: 395 RVA: 0x000094BC File Offset: 0x000076BC
	public void setStartOnBoot(bool startOnBoot)
	{
		Utilities.Singleton().SetStartOnBoot(startOnBoot);
	}

	// Token: 0x0600018C RID: 396 RVA: 0x000094C9 File Offset: 0x000076C9
	public bool getStartOnBoot()
	{
		return Utilities.Singleton().GetStartOnBoot();
	}

	// Token: 0x0600018D RID: 397 RVA: 0x000094D8 File Offset: 0x000076D8
	public void launchUrl(string url)
	{
		try
		{
			Process.Start(url);
		}
		catch
		{
		}
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00009504 File Offset: 0x00007704
	public void releaseMemory()
	{
		APIClient.ReleaseMemory();
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000950C File Offset: 0x0000770C
	public string getAudioGrade()
	{
		return APIClient.GetAudioGrade();
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00009513 File Offset: 0x00007713
	public string getTrainingStatus()
	{
		return APIClient.GetTrainingStatus();
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000951A File Offset: 0x0000771A
	public string startTraining()
	{
		return APIClient.StartTraining();
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00009521 File Offset: 0x00007721
	public string stopTraining()
	{
		return APIClient.StopTraining();
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00009528 File Offset: 0x00007728
	public string getLastVoiceMean()
	{
		return APIClient.GetLastVoiceMean().ToString();
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00009542 File Offset: 0x00007742
	public void resetHistory()
	{
		APIClient.ResetHistory();
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00009549 File Offset: 0x00007749
	public float lockVoiceMean(float v)
	{
		return APIClient.LockVoiceMean(v);
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00009554 File Offset: 0x00007754
	public double getMouseIdleTime()
	{
		return IdleTime.CurrentIdleTime().TotalMilliseconds;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000956E File Offset: 0x0000776E
	public float setAudioBoost(float v)
	{
		AudioService.service.AudioBoostLevel = v;
		return v;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000957C File Offset: 0x0000777C
	public string getIdleMeta()
	{
		return APIClient.GetIdleMeta();
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00009583 File Offset: 0x00007783
	public string getGpuDevices()
	{
		return VoiceChanger.vc.GetGpuMap();
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000958F File Offset: 0x0000778F
	public string setGpuDevice(string i)
	{
		return VoiceChanger.SetGpuDevice(i);
	}

	// Token: 0x0600019B RID: 411 RVA: 0x00009597 File Offset: 0x00007797
	public int registerHotKey(string k, bool shift, bool ctrl, bool alt)
	{
		return GlobalHotKey.Register(BrowserForm.browserHandle, alt, ctrl, shift, k);
	}

	// Token: 0x0600019C RID: 412 RVA: 0x000095A8 File Offset: 0x000077A8
	public string unregisterHotKey(int id)
	{
		return GlobalHotKey.Unregister(BrowserForm.browserHandle, id).ToString();
	}

	// Token: 0x0600019D RID: 413 RVA: 0x000095C8 File Offset: 0x000077C8
	public bool openMicrophoneProperties(string panel)
	{
		return AudioService.service.OpenSelectedDevice(panel);
	}

	// Token: 0x0600019E RID: 414 RVA: 0x000095D5 File Offset: 0x000077D5
	public object getSettingValue(string name)
	{
		return Utilities.Singleton().GetSettingValue(name);
	}

	// Token: 0x0600019F RID: 415 RVA: 0x000095E2 File Offset: 0x000077E2
	public object setSettingValue(string name, float value)
	{
		return Utilities.Singleton().SetSettingValue(name, value);
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000095F0 File Offset: 0x000077F0
	public string getVersion()
	{
		return VoiceChanger.CurrentVersion;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x000095F7 File Offset: 0x000077F7
	public bool updateVersion()
	{
		return BrowserForm.browserform.RunUpdate();
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00009604 File Offset: 0x00007804
	public bool setOutputFileType(string format, int bitrate)
	{
		VCController.AUDIO_FORMATS audio_FORMATS = VCController.AUDIO_FORMATS.WAV;
		if (Enum.TryParse<VCController.AUDIO_FORMATS>(format.ToUpper(), out audio_FORMATS))
		{
			VCController.currentBitrate = bitrate;
			VCController.currentFormat = audio_FORMATS;
			return true;
		}
		return false;
	}

	// Token: 0x04000081 RID: 129
	private static System.Threading.Timer expiresTimer;

	// Token: 0x04000082 RID: 130
	private bool isDownloading;

	// Token: 0x04000083 RID: 131
	private static string lastClipboard = "";

	// Token: 0x04000084 RID: 132
	public static int currentVoice;

	// Token: 0x04000085 RID: 133
	private static VCController.AUDIO_FORMATS currentFormat = VCController.AUDIO_FORMATS.WAV;

	// Token: 0x04000086 RID: 134
	private static int currentBitrate = 0;

	// Token: 0x0200006F RID: 111
	private enum AUDIO_FORMATS
	{
		// Token: 0x04000209 RID: 521
		WAV,
		// Token: 0x0400020A RID: 522
		MP3
	}
}
