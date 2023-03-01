using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using VoiceAIGui.Properties;

// Token: 0x02000009 RID: 9
internal class VoiceChanger
{
	// Token: 0x060000A1 RID: 161
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WA")]
	private static extern IntPtr CreateSession(IntPtr buffer, int bufferLength, long deviceId);

	// Token: 0x060000A2 RID: 162
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WB")]
	private static extern int Inference(IntPtr session, IntPtr audio, int sampleCount, IntPtr buffer, int bufferLength, int d, int sf, ref int ec, int sn, int sms, int debreath);

	// Token: 0x060000A3 RID: 163
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WC")]
	private static extern int HasGpu();

	// Token: 0x060000A4 RID: 164
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WD")]
	private static extern int ValidEmbedding(IntPtr session);

	// Token: 0x060000A5 RID: 165
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WE")]
	private static extern int SetEmbedding(IntPtr session, IntPtr buffer, int size);

	// Token: 0x060000A6 RID: 166
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WF")]
	private static extern int Geet(IntPtr buffer);

	// Token: 0x060000A7 RID: 167
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WG")]
	private static extern int Expires(IntPtr session);

	// Token: 0x060000A8 RID: 168
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WH")]
	private static extern void DestroySession(IntPtr session);

	// Token: 0x060000A9 RID: 169
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WI")]
	private static extern int GetDeviceInfo(ref VoiceChanger.DeviceInfo info);

	// Token: 0x060000AA RID: 170
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WJ")]
	private static extern int Startup(IntPtr buffer, int size);

	// Token: 0x060000AB RID: 171
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WL")]
	private static extern int StartTraining(IntPtr session);

	// Token: 0x060000AC RID: 172
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WM")]
	private static extern int StopTraining(int wait);

	// Token: 0x060000AD RID: 173
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WN")]
	private static extern int StatusTraining();

	// Token: 0x060000AE RID: 174
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WP")]
	private static extern float GetLastVoiceMean(IntPtr session);

	// Token: 0x060000AF RID: 175
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WQ")]
	private static extern int ResetHistory(IntPtr session);

	// Token: 0x060000B0 RID: 176
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WR")]
	private static extern float LockVoiceMean(IntPtr session, float v);

	// Token: 0x060000B1 RID: 177
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WS")]
	private static extern IntPtr ReloadModel(IntPtr session);

	// Token: 0x060000B2 RID: 178
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WT")]
	private static extern IntPtr SetOutputVolume(IntPtr session, float v);

	// Token: 0x060000B3 RID: 179
	[DllImport("VoiceAILib.dll", EntryPoint = "t4WGBnF65WU")]
	private static extern int AudioFx(IntPtr s, int frames);

	// Token: 0x060000B4 RID: 180 RVA: 0x000055DC File Offset: 0x000037DC
	public void SetDUser(Dictionary<string, string> u)
	{
		VoiceChanger.discordUser = u;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x000055E4 File Offset: 0x000037E4
	public void SetOutputVolume(float v)
	{
		VoiceChanger.SetOutputVolume(this.session, v);
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x000055F4 File Offset: 0x000037F4
	private static List<Dictionary<string, string>> GetMicList()
	{
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		try
		{
			new Dictionary<string, Dictionary<string, string>[]>();
			MMDeviceEnumerator mmdeviceEnumerator = new MMDeviceEnumerator();
			List<string> list2 = new List<string>();
			foreach (MMDevice mmdevice in mmdeviceEnumerator.EnumerateAudioEndPoints(1, 15))
			{
				try
				{
					list2.Add(mmdevice.FriendlyName);
				}
				catch
				{
				}
			}
			mmdeviceEnumerator.Dispose();
			for (int i = 0; i < WaveIn.DeviceCount; i++)
			{
				try
				{
					WaveInCapabilities capabilities = WaveIn.GetCapabilities(i);
					if (!capabilities.ProductName.ToLower().Contains("voice.ai") && !capabilities.ProductName.ToLower().Contains("voice ai") && !capabilities.ProductName.ToLower().Contains("voiceai"))
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						string text = capabilities.ProductName;
						foreach (string text2 in list2)
						{
							if (text2.StartsWith(text))
							{
								text = text2;
								break;
							}
						}
						dictionary.Add("name", text);
						dictionary.Add("id", capabilities.ProductGuid.ToString());
						list.Add(dictionary);
					}
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
		return list;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x000057E0 File Offset: 0x000039E0
	public long GetModelId()
	{
		return VoiceChanger.currentModelId;
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060000B8 RID: 184 RVA: 0x000057E8 File Offset: 0x000039E8
	private static string InstallerName
	{
		get
		{
			try
			{
				string text = "c:\\program files\\voice.ai";
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Voice.ai", true);
				if (registryKey != null)
				{
					text = registryKey.GetValue("InstallLocation", text).ToString();
				}
				return File.ReadAllText(Path.Combine(text, "meta"));
			}
			catch
			{
			}
			return "";
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005850 File Offset: 0x00003A50
	public static string CurrentVersion
	{
		get
		{
			try
			{
				string text = "c:\\program files\\voice.ai";
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Voice.ai", true);
				if (registryKey != null)
				{
					text = registryKey.GetValue("InstallLocation", text).ToString();
				}
				return File.ReadAllText(Path.Combine(text, "version"));
			}
			catch
			{
			}
			return Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000058C8 File Offset: 0x00003AC8
	public static string sha256(byte[] data)
	{
		HashAlgorithm hashAlgorithm = new SHA256Managed();
		string text = "";
		foreach (byte b in hashAlgorithm.ComputeHash(data))
		{
			text += b.ToString("x2");
		}
		return text;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00005910 File Offset: 0x00003B10
	public static VoiceChanger Create(long modelId)
	{
		if (!Program.PrimaryProcess)
		{
			return null;
		}
		VoiceChanger voiceChanger;
		try
		{
			voiceChanger = new VoiceChanger(modelId);
		}
		catch (Exception)
		{
			voiceChanger = null;
		}
		return voiceChanger;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00005948 File Offset: 0x00003B48
	public static VoiceChanger singleton(long modelId = 0L)
	{
		object obj;
		if (VoiceChanger.vc == null)
		{
			obj = VoiceChanger.syncObject;
			lock (obj)
			{
				VoiceChanger.vc = VoiceChanger.Create(modelId);
			}
		}
		obj = VoiceChanger.syncObject;
		VoiceChanger voiceChanger;
		lock (obj)
		{
			if (VoiceChanger.vc == null)
			{
				VoiceChanger.vc = VoiceChanger.Create(modelId);
			}
			voiceChanger = VoiceChanger.vc;
		}
		return voiceChanger;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x000059D4 File Offset: 0x00003BD4
	public void Dispose()
	{
		object obj = VoiceChanger.syncObject;
		lock (obj)
		{
			if (this.session != IntPtr.Zero)
			{
				VoiceChanger.DestroySession(this.session);
				this.session = IntPtr.Zero;
				VoiceChanger.vc = null;
			}
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00005A3C File Offset: 0x00003C3C
	public int GetExpires()
	{
		return VoiceChanger.Expires(this.session);
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00005A49 File Offset: 0x00003C49
	public string[] GetVoices()
	{
		return this.voices;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00005A51 File Offset: 0x00003C51
	public bool isVoice(string voice)
	{
		return voice != null && this.voicesSet.Contains(voice.ToLower());
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00005A6C File Offset: 0x00003C6C
	public bool HasValidEmbedding()
	{
		object obj = VoiceChanger.syncObject;
		bool flag2;
		lock (obj)
		{
			flag2 = VoiceChanger.ValidEmbedding(this.session) == 1;
		}
		return flag2;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00005AB8 File Offset: 0x00003CB8
	public bool ReloadModel()
	{
		object obj = VoiceChanger.syncObject;
		bool flag2;
		lock (obj)
		{
			this.session = VoiceChanger.ReloadModel(this.session);
			if (this.session == IntPtr.Zero)
			{
				VoiceChanger.vc = null;
				flag2 = false;
			}
			else
			{
				flag2 = true;
			}
		}
		return flag2;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00005B24 File Offset: 0x00003D24
	private VoiceChanger(long modelId)
	{
		byte[] array = null;
		VoiceChanger.currentModelId = modelId;
		if (array == null)
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voice.ai\\voice.vai");
			if (File.Exists(text))
			{
				array = File.ReadAllBytes(text);
			}
			else
			{
				string text2 = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voice.ai", "Models"), string.Format("model_{0}.vai", modelId.ToString()));
				if (modelId > 0L && File.Exists(text2))
				{
					array = File.ReadAllBytes(text2);
				}
				else
				{
					FileStream fileStream = File.OpenRead(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "0.vai"));
					array = new byte[fileStream.Length];
					fileStream.Read(array, 0, array.Length);
					fileStream.Close();
				}
			}
		}
		GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		if (!VoiceChanger.didStartup)
		{
			if (VoiceChanger.Startup(gchandle.AddrOfPinnedObject(), array.Length) == 0)
			{
				throw new Exception("Invalid startup");
			}
			VoiceChanger.didStartup = true;
		}
		this.session = VoiceChanger.CreateSession(gchandle.AddrOfPinnedObject(), array.Length, VoiceChanger.currentDeviceId);
		gchandle.Free();
		if (this.session.ToInt64() == 0L)
		{
			throw new Exception("Invalid model");
		}
		ThreadPool.QueueUserWorkItem(new WaitCallback(this.intialSynth), null);
		this.Geet();
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00005C75 File Offset: 0x00003E75
	private void intialSynth(object state)
	{
		this.Synth(new float[4096], 0, 0);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00005C8A File Offset: 0x00003E8A
	public string GetData()
	{
		return Convert.ToBase64String(this.data[0]) + "\n" + Convert.ToBase64String(this.data[1]);
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00005CB0 File Offset: 0x00003EB0
	private void Geet()
	{
		object obj = VoiceChanger.syncObject;
		lock (obj)
		{
			byte[] array = new byte[10240];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			int num = VoiceChanger.Geet(gchandle.AddrOfPinnedObject());
			gchandle.Free();
			Array.Resize<byte>(ref array, num);
			uint num2 = BitConverter.ToUInt32(array, 0);
			uint num3 = BitConverter.ToUInt32(array, (int)(num2 + 4U));
			byte[] array2 = new byte[num2];
			byte[] array3 = new byte[num3];
			Array.Copy(array, 4L, array2, 0L, (long)((ulong)num2));
			Array.Copy(array, (long)((ulong)(8U + num2)), array3, 0L, (long)((ulong)num3));
			this.data = new byte[][] { array2, array3 };
		}
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005D78 File Offset: 0x00003F78
	public byte[] GetEmbedding(string voice)
	{
		return this.map[voice];
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005D86 File Offset: 0x00003F86
	public bool CanUseGpu()
	{
		return VoiceChanger.HasGpu() > 0;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005D90 File Offset: 0x00003F90
	public int GpuMode()
	{
		return VoiceChanger.HasGpu();
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005D98 File Offset: 0x00003F98
	public bool SetEmbedding(byte[] embedding)
	{
		object obj = VoiceChanger.syncObject;
		bool flag2;
		lock (obj)
		{
			GCHandle gchandle = GCHandle.Alloc(embedding, GCHandleType.Pinned);
			try
			{
				flag2 = VoiceChanger.SetEmbedding(this.session, gchandle.AddrOfPinnedObject(), embedding.Length) == 1;
			}
			finally
			{
				gchandle.Free();
			}
		}
		return flag2;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005E08 File Offset: 0x00004008
	public string StartTraining()
	{
		return VoiceChanger.StartTraining(this.session).ToString();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005E28 File Offset: 0x00004028
	public string StopTraining()
	{
		return VoiceChanger.StopTraining(1).ToString();
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005E44 File Offset: 0x00004044
	public string GetTrainingStatus()
	{
		return VoiceChanger.StatusTraining().ToString();
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005E60 File Offset: 0x00004060
	public string GetDeviceInfo()
	{
		VoiceChanger.DeviceInfo deviceInfo = default(VoiceChanger.DeviceInfo);
		if (VoiceChanger.GetDeviceInfo(ref deviceInfo) == 1)
		{
			return deviceInfo.ToJson();
		}
		return null;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005E88 File Offset: 0x00004088
	public string GetGpuMap()
	{
		VoiceChanger.DeviceInfo deviceInfo = default(VoiceChanger.DeviceInfo);
		if (VoiceChanger.GetDeviceInfo(ref deviceInfo) == 1)
		{
			return deviceInfo.GetGpuMap();
		}
		return null;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005EB0 File Offset: 0x000040B0
	public static string SetGpuDevice(string i)
	{
		long num = long.Parse(i);
		if (num != VoiceChanger.currentDeviceId)
		{
			VoiceChanger.currentDeviceId = num;
			if (VoiceChanger.vc != null)
			{
				VoiceChanger.vc.Dispose();
			}
		}
		return "[]";
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00005EE8 File Offset: 0x000040E8
	public float GetLastVoiceMean()
	{
		return VoiceChanger.GetLastVoiceMean(this.session);
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005EF5 File Offset: 0x000040F5
	public void ResetHistory()
	{
		VoiceChanger.ResetHistory(this.session);
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00005F03 File Offset: 0x00004103
	public float LockVoiceMean(float v)
	{
		return VoiceChanger.LockVoiceMean(this.session, v);
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005F14 File Offset: 0x00004114
	public float[] AudioFx(float[] samples)
	{
		GCHandle gchandle = GCHandle.Alloc(samples, GCHandleType.Pinned);
		VoiceChanger.AudioFx(gchandle.AddrOfPinnedObject(), samples.Length);
		gchandle.Free();
		return samples;
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005F41 File Offset: 0x00004141
	public int LastErrorCode
	{
		get
		{
			return this.lastEc;
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00005F4C File Offset: 0x0000414C
	public float[] Synth(float[] samples, int d = 0, int sf = 0)
	{
		object obj = VoiceChanger.syncObject;
		float[] array2;
		lock (obj)
		{
			if (VoiceChanger.ValidEmbedding(this.session) == 1 || samples.Length == 4096)
			{
				if (Settings.Default.VolumeBoost)
				{
					float num = WaveMerge.GetNormalizationFactor(samples);
					if (num > 3f)
					{
						num = 3f;
						for (int i = 0; i < samples.Length; i++)
						{
							samples[i] *= num;
						}
					}
				}
				int num2 = samples.Length;
				Array.Resize<float>(ref samples, (int)Math.Ceiling((double)samples.Length / 256.0) * 256);
				GCHandle gchandle = GCHandle.Alloc(samples, GCHandleType.Pinned);
				int num3 = (samples.Length + 220500) * 4;
				IntPtr intPtr = Marshal.AllocHGlobal(num3);
				if (d == 0 && samples.Length != 4096)
				{
					VoiceChanger.lastRecording = samples;
				}
				this.lastEc = 0;
				int num4 = (Settings.Default.ReduceBreath ? 1 : 0);
				int num5 = VoiceChanger.Inference(this.session, gchandle.AddrOfPinnedObject(), samples.Length, intPtr, num3, d, sf, ref this.lastEc, Settings.Default.SnapNote ? 1 : 0, (int)Settings.Default.SemitoneShift, num4);
				float[] array = null;
				if (num5 > 0)
				{
					array = new float[num5];
					Marshal.Copy(intPtr, array, 0, array.Length);
					if (d == 1)
					{
						Array.Resize<float>(ref array, num2);
					}
				}
				gchandle.Free();
				Marshal.FreeHGlobal(intPtr);
				array2 = array;
			}
			else
			{
				array2 = null;
			}
		}
		return array2;
	}

	// Token: 0x04000043 RID: 67
	private const string CHECK_HASH = "195";

	// Token: 0x04000044 RID: 68
	private Dictionary<string, byte[]> map;

	// Token: 0x04000045 RID: 69
	private string[] voices;

	// Token: 0x04000046 RID: 70
	public static long currentModelId = 0L;

	// Token: 0x04000047 RID: 71
	private HashSet<string> voicesSet = new HashSet<string>();

	// Token: 0x04000048 RID: 72
	private IntPtr session;

	// Token: 0x04000049 RID: 73
	private byte[][] data;

	// Token: 0x0400004A RID: 74
	public static float[] lastRecording = null;

	// Token: 0x0400004B RID: 75
	private static object syncObject = new object();

	// Token: 0x0400004C RID: 76
	private static bool didStartup = false;

	// Token: 0x0400004D RID: 77
	public static long currentDeviceId = 0L;

	// Token: 0x0400004E RID: 78
	private static Dictionary<string, string> discordUser = null;

	// Token: 0x0400004F RID: 79
	public static VoiceChanger vc = null;

	// Token: 0x04000050 RID: 80
	private int lastEc;

	// Token: 0x04000051 RID: 81
	private static int fileIndex = 0;

	// Token: 0x0200005E RID: 94
	public struct DeviceInfo
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
		public string ToJson()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("cpuName", Encoding.UTF8.GetString(this.cpuName).Trim(new char[1]));
			dictionary.Add("cpuProcessorCount", this.cpuProcessorCount);
			dictionary.Add("gpuMemory", this.gpuMemory);
			dictionary.Add("cpuMemory", this.cpuMemory);
			dictionary.Add("gpuCount", this.gpuCount);
			dictionary.Add("gpuName", Encoding.UTF8.GetString(this.gpuName).Trim(new char[1]));
			dictionary.Add("gpuCores", this.cudaCores);
			dictionary.Add("macCount", this.macCount);
			List<ulong> list = new List<ulong>();
			for (int i = 0; i < (int)this.macCount; i++)
			{
				dictionary.Add("maca" + i.ToString(), Encoding.ASCII.GetString(this.macInfo, i * 24, 16).Trim(new char[1]));
				dictionary.Add("macb" + i.ToString(), BitConverter.ToUInt64(this.macInfo, 16 + i * 24));
				list.Add(BitConverter.ToUInt64(this.macInfo, 16 + i * 24));
			}
			dictionary.Add("driveCount", this.driveCount);
			dictionary.Add("drives", Encoding.UTF8.GetString(this.drives).Trim(new char[1]));
			dictionary.Add("mbManufacturer", Encoding.UTF8.GetString(this.mbManufacturer).Trim(new char[1]));
			dictionary.Add("mbModel", Encoding.UTF8.GetString(this.mbModel).Trim(new char[1]));
			dictionary.Add("mbSerial", Encoding.UTF8.GetString(this.mbSerial).Trim(new char[1]));
			dictionary.Add("computerName", Encoding.UTF8.GetString(this.computerName).Trim(new char[1]));
			dictionary.Add("osName", Encoding.UTF8.GetString(this.osName).Trim(new char[1]));
			dictionary.Add("version", VoiceChanger.CurrentVersion);
			dictionary.Add("installerName", VoiceChanger.InstallerName);
			dictionary.Add("gpuNameAlt", Encoding.UTF8.GetString(this.altGpuName).Trim(new char[1]));
			if (VoiceChanger.discordUser != null)
			{
				dictionary.Add("discordUser", VoiceChanger.discordUser);
			}
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = Encoding.UTF8.GetBytes(this.cpuMemory.ToString());
			memoryStream.Write(array, 0, array.Length);
			array = Encoding.UTF8.GetBytes(this.gpuName.ToString());
			memoryStream.Write(array, 0, array.Length);
			list.Sort();
			foreach (ulong num in list)
			{
				byte[] bytes = BitConverter.GetBytes(num);
				memoryStream.Write(bytes, 0, bytes.Length);
			}
			string text = VoiceChanger.sha256(memoryStream.ToArray());
			dictionary.Add("uid", text);
			dictionary.Add("micList", VoiceChanger.GetMicList());
			return new JavaScriptSerializer
			{
				MaxJsonLength = int.MaxValue
			}.Serialize(dictionary);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000E390 File Offset: 0x0000C590
		public unsafe string GetGpuMap()
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			byte* ptr = (byte*)(void*)this.gpuMap;
			for (int i = 0; i < (int)this.gpuCount; i++)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				ulong num = (ulong)(*(long*)ptr);
				ptr += 8;
				byte[] array = new byte[256];
				Marshal.Copy(new IntPtr((void*)ptr), array, 0, array.Length);
				string text = Encoding.UTF8.GetString(array, 0, array.Length).Trim(new char[1]);
				ptr += 256;
				string text2 = num.ToString();
				dictionary.Add("id", text2);
				dictionary.Add("name", text);
				list.Add(dictionary);
			}
			new Dictionary<string, object>().Add("devices", list.ToArray());
			return new JavaScriptSerializer
			{
				MaxJsonLength = int.MaxValue
			}.Serialize(list);
		}

		// Token: 0x040001BF RID: 447
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public byte[] cpuName;

		// Token: 0x040001C0 RID: 448
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
		public byte[] gpuUuid;

		// Token: 0x040001C1 RID: 449
		public uint cpuProcessorCount;

		// Token: 0x040001C2 RID: 450
		public ulong gpuMemory;

		// Token: 0x040001C3 RID: 451
		public ulong cpuMemory;

		// Token: 0x040001C4 RID: 452
		public byte gpuCount;

		// Token: 0x040001C5 RID: 453
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] gpuName;

		// Token: 0x040001C6 RID: 454
		public ulong cudaCores;

		// Token: 0x040001C7 RID: 455
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 480)]
		public byte[] macInfo;

		// Token: 0x040001C8 RID: 456
		public byte macCount;

		// Token: 0x040001C9 RID: 457
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		public byte[] drives;

		// Token: 0x040001CA RID: 458
		public byte driveCount;

		// Token: 0x040001CB RID: 459
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public byte[] mbManufacturer;

		// Token: 0x040001CC RID: 460
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public byte[] mbModel;

		// Token: 0x040001CD RID: 461
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public byte[] mbSerial;

		// Token: 0x040001CE RID: 462
		public uint gpuArch;

		// Token: 0x040001CF RID: 463
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public byte[] computerName;

		// Token: 0x040001D0 RID: 464
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
		public byte[] osName;

		// Token: 0x040001D1 RID: 465
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public byte[] altGpuName;

		// Token: 0x040001D2 RID: 466
		private IntPtr gpuMap;
	}
}
