using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using VoiceAIGui.Properties;

// Token: 0x02000007 RID: 7
internal class AudioService
{
	// Token: 0x06000074 RID: 116 RVA: 0x00003EE8 File Offset: 0x000020E8
	public AudioService()
	{
		AudioService.service = this;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00003F49 File Offset: 0x00002149
	public void EnableRealtime(bool enable)
	{
		this.voiceChangerEnabled = enable;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00003F54 File Offset: 0x00002154
	private bool SetupOutputBuffer()
	{
		object obj = AudioService.bufSync;
		bool flag3;
		lock (obj)
		{
			bool flag2 = false;
			if (AudioService.wasp != null)
			{
				AudioService.bufWave = null;
				AudioService.wasp.Stop();
				AudioService.wasp.Dispose();
			}
			int num = 10;
			List<MMDevice> list = new MMDeviceEnumerator().EnumerateAudioEndPoints(0, 1).ToList<MMDevice>();
			foreach (MMDevice mmdevice in list)
			{
				if (mmdevice.DeviceFriendlyName.ToLower().Contains("voice.ai"))
				{
					flag2 = true;
					AudioService.wasp = new WasapiOut(mmdevice, 0, false, num);
					break;
				}
			}
			if (!flag2)
			{
				foreach (MMDevice mmdevice2 in list)
				{
					if (mmdevice2.DeviceFriendlyName.ToLower().Contains("vb-audio"))
					{
						flag2 = true;
						AudioService.wasp = new WasapiOut(mmdevice2, 0, false, num);
						break;
					}
				}
			}
			if (!flag2)
			{
				flag3 = false;
			}
			else
			{
				AudioService.bufWave = new BufferedWaveProvider(new WaveFormat(22050, 16, 1));
				AudioService.bufWave.BufferDuration = TimeSpan.FromSeconds(100.0);
				AudioService.wasp.Init(AudioService.bufWave);
				AudioService.wasp.Play();
				flag3 = true;
			}
		}
		return flag3;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x0000410C File Offset: 0x0000230C
	public void DestroyOutputBuffer()
	{
		if (!Utilities.Singleton().GetAudioPassthruEnabled())
		{
			object obj = AudioService.bufSync;
			lock (obj)
			{
				AudioService.bufWave = null;
				AudioService.wasp.Stop();
				AudioService.wasp.Dispose();
			}
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0000416C File Offset: 0x0000236C
	public bool StartStreamingTime()
	{
		if (this.isRealtime)
		{
			return false;
		}
		if (this.isStreamingTime)
		{
			return true;
		}
		if (!this.SetupOutputBuffer())
		{
			return false;
		}
		if (AudioService.streamingTimeThread == null)
		{
			this.aq.Clear();
			AudioService.streamingTimeThread = new Thread(new ParameterizedThreadStart(this.StreamingVC));
			AudioService.streamingTimeThread.Priority = ThreadPriority.AboveNormal;
			AudioService.streamingTimeThread.Start();
		}
		if (!this.StartRecording())
		{
			AudioService.streamingTimeThread = null;
			return false;
		}
		this.isStreamingTime = true;
		return true;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000041EC File Offset: 0x000023EC
	public bool StartRecording()
	{
		if (this.currentMmDevice != null)
		{
			this.ms.SetLength(0L);
			this.isRecording = true;
			return true;
		}
		return false;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004210 File Offset: 0x00002410
	public bool StopStreamingTime()
	{
		if (this.wi != null && this.isStreamingTime)
		{
			AudioService.lastLevel = 0f;
			AudioService.maxLevel = 0f;
			AudioService.streamingTimeThread = null;
			this.isStreamingTime = false;
			this.isRecording = false;
			if (AudioService.bufWave != null)
			{
				AudioService.bufWave.ClearBuffer();
			}
			this.DestroyOutputBuffer();
			return true;
		}
		return false;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00004270 File Offset: 0x00002470
	public bool StopRealtime()
	{
		if (this.wi != null && this.isRealtime)
		{
			AudioService.lastLevel = 0f;
			AudioService.maxLevel = 0f;
			AudioService.realtimeThread = null;
			this.isRealtime = false;
			this.isRecording = false;
			this.DestroyOutputBuffer();
			return true;
		}
		return false;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000042C0 File Offset: 0x000024C0
	public Tuple<byte[], float, int, float, int> StopOnboardingRecording()
	{
		if (this.wi != null)
		{
			this.isRecording = false;
			AudioService.lastLevel = 0f;
			AudioService.maxLevel = 0f;
			if (this.isRealtime)
			{
				return null;
			}
			byte[] array = this.ms.ToArray();
			if (this.ms.Length > 0L)
			{
				short[] array2 = WaveMerge.SamplesToShort(array);
				float num = (float)array2.Length / 22050f;
				AudioService.GateMeta gateMeta;
				AudioService.audio_gate(out gateMeta, WaveMerge.SamplesToFloat(array2), null, 410, 2, -35f);
				int num2 = (int)WaveMerge.GradeAudio(array2, 800);
				return new Tuple<byte[], float, int, float, int>(array, num, num2, gateMeta.db, VoiceChanger.singleton(0L).LastErrorCode);
			}
		}
		return null;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004370 File Offset: 0x00002570
	public byte[] StopRecording()
	{
		if (this.wi != null)
		{
			this.isRecording = false;
			AudioService.lastLevel = 0f;
			AudioService.maxLevel = 0f;
			if (this.isRealtime)
			{
				return null;
			}
			byte[] array = this.ms.ToArray();
			if (this.ms.Length > 0L)
			{
				float[] array2 = WaveMerge.SamplesToFloat(WaveMerge.SamplesToShort(array));
				float[] array3 = VoiceChanger.singleton(0L).Synth(array2, 0, 0);
				if (array3 != null)
				{
					return WaveMerge.SamplesToByte(array3);
				}
			}
		}
		return null;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x000043EC File Offset: 0x000025EC
	public void SetStreamBuffer(short[] samples)
	{
		byte[] array = WaveMerge.SamplesToByte(samples);
		if (AudioService.streamingBuffer != null)
		{
			AudioService.streamingBuffer.SetLength(0L);
			AudioService.streamingBuffer.Write(array, 0, array.Length);
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00004422 File Offset: 0x00002622
	public void KillRecording()
	{
		if (this.wi != null)
		{
			this.wi.StopRecording();
			this.wi.Dispose();
		}
		if (AudioService.wasp != null)
		{
			AudioService.wasp.Stop();
			AudioService.wasp.Dispose();
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00004460 File Offset: 0x00002660
	public bool OpenSelectedDevice(string panel = "levels")
	{
		if (this.currentMmDevice != null)
		{
			try
			{
				Process.Start(new ProcessStartInfo(Path.Combine(Environment.SystemDirectory, "control.exe"))
				{
					Arguments = string.Format("mmsys.cpl,,{0},{1}", this.currentMmDevice.ID, panel),
					UseShellExecute = true
				});
				return true;
			}
			catch
			{
			}
			return false;
		}
		return false;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x000044CC File Offset: 0x000026CC
	public void SelectDevice(MMDevice d)
	{
		this.currentMmDevice = d;
		AudioService.lastLevel = 0f;
		AudioService.maxLevel = 0f;
		this.ms.SetLength(0L);
		if (this.wi != null)
		{
			this.wi.StopRecording();
			this.wi.Dispose();
		}
		this.wi = new WaveInEvent();
		int num = -1;
		for (int i = 0; i < WaveIn.DeviceCount; i++)
		{
			WaveInCapabilities capabilities = WaveIn.GetCapabilities(i);
			if (d.FriendlyName.StartsWith(capabilities.ProductName))
			{
				num = i;
				break;
			}
		}
		if (num > -1)
		{
			this.wi.DeviceNumber = num;
			this.wi.WaveFormat = new WaveFormat(22050, 16, 1);
			this.wi.BufferMilliseconds = 60;
			this.wi.NumberOfBuffers = 10;
			this.wi.DataAvailable += this.Wi_DataAvailable;
			this.wi.RecordingStopped += this.Wi_RecordingStopped;
			this.wi.StartRecording();
			if (Utilities.Singleton().GetAudioPassthruEnabled())
			{
				this.SetupOutputBuffer();
			}
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000045EC File Offset: 0x000027EC
	private void Wi_RecordingStopped(object sender, StoppedEventArgs e)
	{
		if (e.Exception != null)
		{
			this.SelectDevice(this.currentMmDevice);
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00004604 File Offset: 0x00002804
	public float GetMicLevel()
	{
		try
		{
			if (this.currentMmDevice != null)
			{
				return this.currentMmDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
			}
		}
		catch
		{
		}
		return -1f;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00004648 File Offset: 0x00002848
	public float SetMicLevel(float v)
	{
		try
		{
			if (this.currentMmDevice != null)
			{
				this.currentMmDevice.AudioEndpointVolume.MasterVolumeLevelScalar = v;
			}
		}
		catch
		{
		}
		return this.GetMicLevel();
	}

	// Token: 0x06000085 RID: 133 RVA: 0x0000468C File Offset: 0x0000288C
	public void SelectSpeaker(WaveOutCapabilities d)
	{
		this.currentSpeaker = d;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00004698 File Offset: 0x00002898
	private void Wi_DataAvailable(object sender, WaveInEventArgs e)
	{
		float num = 0f;
		foreach (short num2 in WaveMerge.SamplesToShort(e.Buffer))
		{
			num = Math.Max(num, Math.Abs((float)num2 / 32767f));
		}
		AudioService.lastLevel = Math.Min(1f, num / 0.8f);
		if (this.isRecording)
		{
			if (!this.isRealtime && !this.isStreamingTime)
			{
				this.ms.Write(e.Buffer, 0, e.BytesRecorded);
				return;
			}
			byte[] array2 = new byte[e.BytesRecorded];
			this.bytesRec = e.BytesRecorded;
			Buffer.BlockCopy(e.Buffer, 0, array2, 0, e.BytesRecorded);
			AudioQueue audioQueue = this.aq;
			lock (audioQueue)
			{
				this.aq.Add(array2);
				this.qq.Add(array2);
				return;
			}
		}
		object obj = AudioService.bufSync;
		lock (obj)
		{
			if (AudioService.bufWave != null && Utilities.Singleton().GetAudioPassthruEnabled())
			{
				this.AddSamples(e.Buffer, 0, e.Buffer.Length, true);
			}
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000047F8 File Offset: 0x000029F8
	private bool AddSamples(byte[] buffer, int offset, int length, bool forcePassthrough)
	{
		if (AudioService.streamingBuffer.DataRemaining > 0)
		{
			int num = Math.Min(length, AudioService.streamingBuffer.DataRemaining);
			byte[] array = new byte[num];
			int num2 = AudioService.streamingBuffer.Read(array, 0, num);
			if (num2 != num)
			{
				Array.Resize<byte>(ref array, num2);
			}
			short[] array2 = WaveMerge.SamplesToShort(buffer);
			short[] array3 = WaveMerge.SamplesToShort(array);
			for (int i = 0; i < num / 2; i++)
			{
				float num3 = (float)((int)array2[i] + 32768);
				float num4 = (float)((int)array3[i] + 32768);
				float num5;
				if (array2[i] < 0 || array3[i] < 0)
				{
					num5 = num3 * num4 / 32768f;
				}
				else
				{
					num5 = 2f * (num3 + num4) - num3 * num4 / 32768f - 65536f;
				}
				if (num5 == 65536f)
				{
					num5 = 65535f;
				}
				num5 -= 32768f;
				array2[i] = (short)Math.Min(Math.Max(-32768f, num5), 32767f);
			}
			buffer = WaveMerge.SamplesToByte(array2);
		}
		AudioService.GateMeta gateMeta = default(AudioService.GateMeta);
		short[] array4 = WaveMerge.SamplesToShort(buffer);
		float num6 = 0f;
		for (int j = 0; j < array4.Length; j++)
		{
			num6 += (float)(array4[j] * array4[j]);
		}
		num6 = (float)Math.Sqrt((double)(num6 / (float)array4.Length));
		gateMeta.db = num6;
		if (!forcePassthrough && gateMeta.db < 1000f && AudioService.prevDb < 1000f)
		{
			AudioService.prevDb = gateMeta.db;
			return false;
		}
		AudioService.prevDb = gateMeta.db;
		AudioService.bufWave.AddSamples(buffer, offset, length);
		if (AudioService.localDebugStream != null)
		{
			AudioService.localDebugStream.Write(buffer, offset, length);
		}
		return true;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x000049B8 File Offset: 0x00002BB8
	private static short[] HanningRight(short[] data)
	{
		float[] array = AudioService.HanningWindow(data.Length * 2);
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = (short)((float)data[i] * array[i + array.Length / 2]);
		}
		return data;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x000049F4 File Offset: 0x00002BF4
	private static short[] xHanningRight(short[] data, int maxMod = -1)
	{
		if (maxMod == -1)
		{
			maxMod = data.Length;
		}
		float[] array = AudioService.HanningWindow(maxMod * 2);
		for (int i = 0; i < maxMod; i++)
		{
			data[i + (data.Length - maxMod)] = (short)((float)data[i + (data.Length - maxMod)] * array[i + maxMod]);
		}
		return data;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00004A3C File Offset: 0x00002C3C
	private static short[] HanningLeft(short[] data, int maxMod = -1)
	{
		if (maxMod == -1)
		{
			maxMod = data.Length;
		}
		float[] array = AudioService.HanningWindow(maxMod * 2);
		for (int i = 0; i < maxMod; i++)
		{
			data[i] = (short)((float)data[i] * array[i]);
		}
		return data;
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00004A74 File Offset: 0x00002C74
	public static float[] HanningWindow(int size)
	{
		float[] array = new float[size];
		for (int i = 0; i < size; i++)
		{
			array[i] = (float)(0.5 * (1.0 - Math.Cos(6.283185307179586 * (double)i / (double)(size - 1))));
		}
		return array;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00004AC4 File Offset: 0x00002CC4
	public static T[][] sliding_window<T>(T[] arr, int size = 1024, int step = 256)
	{
		List<T[]> list = new List<T[]>();
		Array.Resize<T>(ref arr, (int)(Math.Ceiling(1.0 * (double)arr.Length / (double)step) * (double)step));
		for (int i = 0; i < arr.Length - size + 1; i += step)
		{
			T[] array = new T[size];
			Array.Copy(arr, i, array, 0, array.Length);
			list.Add(array);
		}
		return list.ToArray();
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00004B2C File Offset: 0x00002D2C
	public static float[] audio_square(float[] s)
	{
		float[] array = new float[s.Length];
		for (int i = 0; i < s.Length; i++)
		{
			array[i] = s[i] * s[i];
		}
		return array;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00004B5C File Offset: 0x00002D5C
	public static short[] audio_gate(out AudioService.GateMeta gm, float[] samples, float[] target = null, int winsize = 410, int step_div = 2, float min_db = -35f)
	{
		if (target == null)
		{
			target = samples;
		}
		else
		{
			Array.Resize<float>(ref target, samples.Length);
		}
		int num = winsize / step_div;
		int[] array = new int[samples.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = i;
		}
		int[][] array2 = AudioService.sliding_window<int>(array, winsize, num);
		float[][] array3 = AudioService.sliding_window<float>(AudioService.audio_square(samples), winsize, num);
		int num2 = 50;
		int j = 0;
		bool[] array4 = new bool[target.Length];
		gm = default(AudioService.GateMeta);
		gm.db = float.MinValue;
		int num3 = 0;
		while (j < array3.Length)
		{
			float num4 = float.MinValue;
			for (int k = 0; k < array3[j].Length; k++)
			{
				num4 = (float)Math.Max(20.0 * Math.Log10(Math.Sqrt((double)array3[j][k]) + 1E-08), (double)num4);
			}
			gm.db = Math.Max(gm.db, num4);
			if (num4 < min_db)
			{
				for (int l = 0; l < array2[j].Length; l++)
				{
					if (array2[j][l] < target.Length)
					{
						target[array2[j][l]] *= 0f;
						array4[array2[j][l]] = true;
						num3++;
					}
				}
				if (j < array3.Length - step_div)
				{
					float[] array5 = new float[winsize];
					for (int m = 0; m < array5.Length; m++)
					{
						array5[m] = 1f;
					}
					float[] array6 = AudioService.HanningWindow(num2);
					for (int n = 0; n < array6.Length / 2; n++)
					{
						array5[n] = array6[n];
					}
					for (int num5 = 0; num5 < array2[j + step_div].Length; num5++)
					{
						target[array2[j + step_div][num5]] *= array5[num5];
					}
				}
				if (j > step_div - 1)
				{
					float[] array7 = new float[winsize];
					for (int num6 = 0; num6 < array7.Length; num6++)
					{
						array7[num6] = 1f;
					}
					float[] array8 = AudioService.HanningWindow(num2);
					for (int num7 = 0; num7 < array8.Length / 2; num7++)
					{
						array7[array7.Length - array8.Length / 2 + num7] = array8[num7 + array8.Length / 2];
					}
					for (int num8 = 0; num8 < array2[j - step_div].Length; num8++)
					{
						target[array2[j - step_div][num8]] *= array7[num8];
					}
				}
				j += step_div;
			}
			else
			{
				j++;
			}
		}
		gm.dropped = array4;
		gm.allDropped = num3 >= target.Length;
		gm.AllKept = num3 == 0;
		return WaveMerge.SamplesToShort(target);
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00004DFC File Offset: 0x00002FFC
	private static short[] CreateSine(int amount)
	{
		int num = 22050;
		short[] array = new short[amount];
		double num2 = 8191.75;
		double num3 = 1000.0;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (short)(num2 * Math.Sin(6.283185307179586 * (double)i * num3 / (double)num));
		}
		return array;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00004E5C File Offset: 0x0000305C
	private void StreamingVC(object o)
	{
		while (AudioService.streamingTimeThread != null && this.isStreamingTime)
		{
			if (!this.aq.DidAddData())
			{
				Thread.Sleep(1);
			}
			else
			{
				int num = 200;
				int num2 = Math.Min(100, Math.Max(1, AudioService.service.realtimeOffset));
				byte[] array = null;
				byte[] array2 = null;
				if (AudioService.lastHanning == null)
				{
					AudioService.lastHanning = new byte[num];
				}
				DateTime now = DateTime.Now;
				int num3 = 0;
				AudioQueue audioQueue = this.aq;
				lock (audioQueue)
				{
					if (this.aq.MarkCount >= num2 && this.aq.Count >= Math.Max(6, num2 * 2))
					{
						array2 = this.aq.Get(Math.Max(6, num2 * 2), true);
						num3 = this.aq.Get(num2, false).Length;
						this.aq.MarkClear();
					}
				}
				if (array2 != null)
				{
					AudioService.lastOne = null;
					if (AudioService.lastOne == null)
					{
						AudioService.lastOne = new byte[0];
					}
					array = AudioService.lastOne;
					short[] array3 = WaveMerge.SamplesToShort(array2);
					Array.Reverse(array3);
					Array.Resize<short>(ref array3, Math.Min(2205, array3.Length));
					Array.Resize<byte>(ref array, array.Length + array2.Length + array3.Length * 2);
					Array.Copy(array2, 0, array, AudioService.lastOne.Length, array2.Length);
					Array.Copy(WaveMerge.SamplesToByte(array3), 0, array, AudioService.lastOne.Length + array2.Length, array3.Length * 2);
					if (!this.voiceChangerEnabled || !VoiceChanger.singleton(0L).HasValidEmbedding())
					{
						continue;
					}
					float[] array4 = WaveMerge.SamplesToFloat(array);
					byte[] array5 = array;
					array = WaveMerge.SamplesToByte(VoiceChanger.singleton(0L).Synth(array4, array3.Length, AudioService.lastOne.Length / 2));
					byte[] array6 = new byte[num3 + num];
					Array.Copy(array, array.Length - array3.Length * 2 - array6.Length, array6, 0, array6.Length);
					short[] array7 = WaveMerge.SamplesToShort(AudioService.lastHanning);
					short[] array8 = WaveMerge.SamplesToShort(array6);
					array8 = AudioService.HanningLeft(array8, array7.Length);
					for (int i = 0; i < array7.Length; i++)
					{
						short[] array9 = array8;
						int num4 = i;
						array9[num4] += array7[i];
					}
					array6 = WaveMerge.SamplesToByte(array8);
					AudioService.lastHanning = new byte[num];
					Array.Copy(array6, array6.Length - num, AudioService.lastHanning, 0, num);
					byte[] array10 = new byte[array6.Length];
					Array.Copy(array5, array2.Length - array6.Length - num, array10, 0, array10.Length);
					if (Settings.Default.GateOutputValue > -100f)
					{
						AudioService.GateMeta gateMeta;
						array6 = WaveMerge.SamplesToByte(AudioService.audio_gate(out gateMeta, WaveMerge.SamplesToFloat(array10), WaveMerge.SamplesToFloat(array6), 410, 2, Settings.Default.GateOutputValue));
					}
					AudioService.lastHanning = WaveMerge.SamplesToByte(AudioService.HanningRight(WaveMerge.SamplesToShort(AudioService.lastHanning)));
					AudioService.lastOne = array2;
					object obj = AudioService.bufSync;
					lock (obj)
					{
						if (AudioService.bufWave != null && !this.AddSamples(array6, 0, array6.Length - num, false))
						{
							AudioService.lastHanning = null;
							AudioService.lastOne = null;
						}
						continue;
					}
				}
				Thread.Sleep(1);
			}
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x000051A8 File Offset: 0x000033A8
	public int getAudioGrade()
	{
		float num = 0f;
		AudioService.GateMeta gateMeta = default(AudioService.GateMeta);
		WaveMerge.AUDIO_GRADE audio_GRADE = WaveMerge.AUDIO_GRADE.NO_AUDIO;
		if (this.isRealtime || this.isStreamingTime)
		{
			byte[] array = this.qq.Get(this.qq.Count, false);
			audio_GRADE = WaveMerge.GradeAudio(WaveMerge.SamplesToShort(array), 800);
			num = (float)WaveMerge.SamplesToFloat(array).Length / 22050f;
			AudioService.audio_gate(out gateMeta, WaveMerge.SamplesToFloat(array), null, 410, 2, -35f);
		}
		else if (VoiceChanger.lastRecording != null)
		{
			audio_GRADE = WaveMerge.GradeAudio(WaveMerge.SamplesToShort(VoiceChanger.lastRecording), 800);
			num = (float)VoiceChanger.lastRecording.Length / 22050f;
			AudioService.audio_gate(out gateMeta, VoiceChanger.lastRecording, null, 410, 2, -35f);
		}
		if (num != 0f)
		{
			AudioService.LastAudioMeta = new Tuple<float, int, float, int>(num, (int)audio_GRADE, gateMeta.db, VoiceChanger.singleton(0L).LastErrorCode);
		}
		else
		{
			AudioService.LastAudioMeta = new Tuple<float, int, float, int>(0f, 4, -100f, 0);
		}
		return (int)audio_GRADE;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000052B0 File Offset: 0x000034B0
	public static void WriteFile(string name, string path)
	{
		Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("VoiceAI." + name);
		byte[] array = new byte[manifestResourceStream.Length];
		manifestResourceStream.Read(array, 0, array.Length);
		manifestResourceStream.Close();
		File.WriteAllBytes(path, array);
	}

	// Token: 0x04000019 RID: 25
	private const int MAX_CONNECTIONS = 1000;

	// Token: 0x0400001A RID: 26
	private EventLog log;

	// Token: 0x0400001B RID: 27
	private static int currentConnections;

	// Token: 0x0400001C RID: 28
	private TcpListener apiListener;

	// Token: 0x0400001D RID: 29
	private ulong totalNumberOfConnections;

	// Token: 0x0400001E RID: 30
	private bool isRunning = true;

	// Token: 0x0400001F RID: 31
	private Thread serverApiThread;

	// Token: 0x04000020 RID: 32
	public static AudioService service;

	// Token: 0x04000021 RID: 33
	private MemoryStream ms = new MemoryStream();

	// Token: 0x04000022 RID: 34
	private WaveInEvent wi;

	// Token: 0x04000023 RID: 35
	private WaveOutCapabilities currentSpeaker;

	// Token: 0x04000024 RID: 36
	public static float lastLevel;

	// Token: 0x04000025 RID: 37
	public static float maxLevel;

	// Token: 0x04000026 RID: 38
	public bool isRealtime;

	// Token: 0x04000027 RID: 39
	public bool isStreamingTime;

	// Token: 0x04000028 RID: 40
	public int realtimeOffset = 4;

	// Token: 0x04000029 RID: 41
	private bool isRecording;

	// Token: 0x0400002A RID: 42
	private static DualStream ds = new DualStream(132300L);

	// Token: 0x0400002B RID: 43
	private static DualStream streamingBuffer = new DualStream(long.MaxValue);

	// Token: 0x0400002C RID: 44
	private static short[] prevChunk;

	// Token: 0x0400002D RID: 45
	private static BufferedWaveProvider bufWave;

	// Token: 0x0400002E RID: 46
	private static WasapiOut wasp;

	// Token: 0x0400002F RID: 47
	private int bytesRec;

	// Token: 0x04000030 RID: 48
	private AudioQueue aq = new AudioQueue(200);

	// Token: 0x04000031 RID: 49
	private AudioQueue qq = new AudioQueue(80);

	// Token: 0x04000032 RID: 50
	private static DateTime lastSynth = DateTime.Now;

	// Token: 0x04000033 RID: 51
	private static Thread realtimeThread = null;

	// Token: 0x04000034 RID: 52
	private static Thread streamingTimeThread = null;

	// Token: 0x04000035 RID: 53
	private bool voiceChangerEnabled = true;

	// Token: 0x04000036 RID: 54
	private MMDevice currentMmDevice;

	// Token: 0x04000037 RID: 55
	public float AudioBoostLevel = 1f;

	// Token: 0x04000038 RID: 56
	private static MemoryStream localDebugStream = null;

	// Token: 0x04000039 RID: 57
	private static object bufSync = new object();

	// Token: 0x0400003A RID: 58
	private static float prevDb = 0f;

	// Token: 0x0400003B RID: 59
	private static int streamIndex = 0;

	// Token: 0x0400003C RID: 60
	private static byte[] lastOne = null;

	// Token: 0x0400003D RID: 61
	private static byte[] lastHanning = null;

	// Token: 0x0400003E RID: 62
	public static Tuple<float, int, float, int> LastAudioMeta = null;

	// Token: 0x0200005D RID: 93
	public struct GateMeta
	{
		// Token: 0x040001BB RID: 443
		public float db;

		// Token: 0x040001BC RID: 444
		public bool[] dropped;

		// Token: 0x040001BD RID: 445
		public bool allDropped;

		// Token: 0x040001BE RID: 446
		public bool AllKept;
	}
}
