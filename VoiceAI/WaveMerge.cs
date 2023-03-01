using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x0200000A RID: 10
public class WaveMerge
{
	// Token: 0x060000D8 RID: 216 RVA: 0x0000611C File Offset: 0x0000431C
	public static void MergeWavFiles(string OutputPath, params string[] Files)
	{
		WaveMerge.RiffFile[] array = new WaveMerge.RiffFile[Files.Length];
		int num = 0;
		for (int i = 0; i <= Files.Length - 1; i++)
		{
			array[num] = WaveMerge.ReadRIFF(new BinaryReader(new FileStream(Files[i], FileMode.Open, FileAccess.Read)));
			num++;
		}
		BinaryWriter binaryWriter = new BinaryWriter(new FileStream(OutputPath, FileMode.OpenOrCreate, FileAccess.Write));
		WaveMerge.MergeRiffsToFile(binaryWriter, array);
		binaryWriter.Close();
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00006180 File Offset: 0x00004380
	public static void MergeWavFiles(Stream OutputStream, Stream[] Files, int start, int count)
	{
		int num = 0;
		OutputStream.Position = 0L;
		WaveMerge.RiffFile[] array = new WaveMerge.RiffFile[count];
		for (int i = start; i <= start + count - 1; i++)
		{
			Files[i].Position = 0L;
			array[num] = WaveMerge.ReadRIFF(new BinaryReader(Files[i]));
			num++;
		}
		WaveMerge.MergeRiffsToFile(new BinaryWriter(OutputStream), array);
	}

	// Token: 0x060000DA RID: 218 RVA: 0x000061E0 File Offset: 0x000043E0
	public static void CreateWave(byte[] samples, int sampleRate, int bitsPerSample, byte channels, Stream saveTo, params WaveMerge.RIFF_Chunk[] extraChunks)
	{
		BinaryWriter binaryWriter = new BinaryWriter(saveTo);
		binaryWriter.Write(Encoding.UTF8.GetBytes("RIFF"));
		int num = 0;
		if (extraChunks != null && extraChunks.Length != 0)
		{
			for (int i = 0; i < extraChunks.Length; i++)
			{
				num += extraChunks[i].GetLength();
			}
		}
		binaryWriter.Write(samples.Length + 36 + num);
		binaryWriter.Write(Encoding.UTF8.GetBytes("WAVEfmt "));
		byte[] array = new byte[4];
		array[0] = 16;
		binaryWriter.Write(array);
		binaryWriter.Write(Convert.ToByte(1));
		binaryWriter.Write(Convert.ToByte(0));
		binaryWriter.Write(Convert.ToByte(channels));
		binaryWriter.Write(Convert.ToByte(0));
		binaryWriter.Write(Convert.ToUInt32(sampleRate));
		binaryWriter.Write(Convert.ToUInt32(sampleRate * (bitsPerSample / 8)));
		binaryWriter.Write(Convert.ToByte(bitsPerSample / 8 * (int)channels));
		binaryWriter.Write(Convert.ToByte(0));
		binaryWriter.Write(Convert.ToByte(bitsPerSample));
		binaryWriter.Write(Convert.ToByte(0));
		binaryWriter.Write(Encoding.UTF8.GetBytes("data"));
		binaryWriter.Write(Convert.ToUInt32(samples.Length));
		binaryWriter.Write(samples);
		if (extraChunks != null && extraChunks.Length != 0)
		{
			for (int j = 0; j < extraChunks.Length; j++)
			{
				extraChunks[j].Serialize(binaryWriter);
			}
		}
		binaryWriter.Close();
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00006340 File Offset: 0x00004540
	public static byte[] CreateWaveData(byte[] samples, int sampleRate, int bitsPerSample, byte channels, params WaveMerge.RIFF_Chunk[] extraChunks)
	{
		MemoryStream memoryStream = new MemoryStream();
		WaveMerge.CreateWave(samples, sampleRate, bitsPerSample, channels, memoryStream, extraChunks);
		return memoryStream.ToArray();
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00006368 File Offset: 0x00004568
	public static void CreateWaveFile(string saveTo, byte[] samples, int sampleRate, int bitsPerSample, byte channels, params WaveMerge.RIFF_Chunk[] extraChunks)
	{
		FileStream fileStream = File.OpenWrite(saveTo);
		fileStream.SetLength(0L);
		WaveMerge.CreateWave(samples, sampleRate, bitsPerSample, channels, fileStream, extraChunks);
		fileStream.Close();
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00006398 File Offset: 0x00004598
	public static void MergeWavFiles(string Outputpath, params Stream[] Files)
	{
		int num = 0;
		WaveMerge.RiffFile[] array = new WaveMerge.RiffFile[Files.Length];
		for (int i = 0; i <= Files.Length - 1; i++)
		{
			Files[i].Position = 0L;
			array[num] = WaveMerge.ReadRIFF(new BinaryReader(Files[i]));
			num++;
		}
		BinaryWriter binaryWriter = new BinaryWriter(new FileStream(Outputpath, FileMode.OpenOrCreate, FileAccess.Write));
		WaveMerge.MergeRiffsToFile(binaryWriter, array);
		binaryWriter.Close();
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00006400 File Offset: 0x00004600
	public static void AppendWave(BinaryWriter stream, BinaryReader fromStream, bool firstWave = false)
	{
		WaveMerge.RiffFile riffFile = WaveMerge.ReadRIFF(fromStream);
		if (firstWave)
		{
			stream.Write(Encoding.UTF8.GetBytes("RIFF"));
			stream.Write(Convert.ToUInt32(riffFile.PayLoadLength + 28U));
			stream.Write(Encoding.UTF8.GetBytes("WAVEfmt "));
			byte[] array = new byte[4];
			array[0] = 16;
			stream.Write(array);
			Array.Resize<byte>(ref riffFile.Wave.FormatBytes, 16);
			stream.Write(riffFile.Wave.FormatBytes);
			stream.Write(Encoding.UTF8.GetBytes("data"));
			stream.Write(Convert.ToUInt32(riffFile.PayLoadLength));
		}
		else
		{
			stream.Seek(0, SeekOrigin.End);
		}
		WaveMerge.StreamToStream(fromStream, stream, riffFile.Wave.PayLoadStart, riffFile.Wave.PayLoadLength);
		stream.Seek(4, SeekOrigin.Begin);
		stream.Write(Convert.ToUInt32(stream.BaseStream.Length - 8L));
		stream.Seek(40, SeekOrigin.Begin);
		stream.Write(Convert.ToUInt32(stream.BaseStream.Length - 44L));
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00006528 File Offset: 0x00004728
	public static void AppendSpacer(BinaryWriter stream)
	{
		stream.Seek(0, SeekOrigin.End);
		BinaryReader binaryReader = new BinaryReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".spacer.wav"));
		WaveMerge.RiffFile riffFile = WaveMerge.ReadRIFF(binaryReader);
		WaveMerge.StreamToStream(binaryReader, stream, riffFile.Wave.PayLoadStart, riffFile.Wave.PayLoadLength);
		stream.Seek(4, SeekOrigin.Begin);
		stream.Write(Convert.ToUInt32(stream.BaseStream.Length - 8L));
		stream.Seek(40, SeekOrigin.Begin);
		stream.Write(Convert.ToUInt32(stream.BaseStream.Length - 44L));
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000065D0 File Offset: 0x000047D0
	public static void MergeRiffsToFile(BinaryWriter stream, params WaveMerge.RiffFile[] riffs)
	{
		WaveMerge.RiffFile riffFile = default(WaveMerge.RiffFile);
		long num = 0L;
		foreach (WaveMerge.RiffFile riffFile in riffs)
		{
			num += (long)((ulong)riffFile.Wave.PayLoadLength);
		}
		long num2 = num;
		int num3 = riffs.Length;
		num = num2 - 0L;
		stream.Write(Encoding.UTF8.GetBytes("RIFF"));
		stream.Write(Convert.ToUInt32(num + 36L));
		stream.Write(Encoding.UTF8.GetBytes("WAVEfmt "));
		byte[] array = new byte[4];
		array[0] = 16;
		stream.Write(array);
		Array.Resize<byte>(ref riffs[0].Wave.FormatBytes, 16);
		stream.Write(riffs[0].Wave.FormatBytes);
		stream.Write(Encoding.UTF8.GetBytes("data"));
		stream.Write(Convert.ToUInt32(num));
		for (int j = 0; j < riffs.Length; j++)
		{
			uint num4 = riffs[j].Wave.PayLoadLength;
			if (j < riffs.Length - 1)
			{
				num4 = num4;
			}
			WaveMerge.StreamToStream(riffs[j].Stream, stream, riffs[j].Wave.PayLoadStart, num4);
		}
		for (int j = 0; j < riffs.Length; j++)
		{
			if (riffs[j].Stream.BaseStream.CanRead)
			{
				riffs[j].Stream.Close();
			}
		}
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00006750 File Offset: 0x00004950
	private static void StreamToStream(BinaryReader fromStream, BinaryWriter toStream, long Start, uint Length)
	{
		fromStream.BaseStream.Seek(Start, SeekOrigin.Begin);
		uint num = Length;
		while (num > 0U)
		{
			byte[] array = fromStream.ReadBytes((int)Math.Max(2048U, num));
			num -= (uint)array.Length;
			toStream.Write(array);
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00006794 File Offset: 0x00004994
	public static WaveMerge.RiffFile ReadRIFF(string file)
	{
		FileStream fileStream = File.OpenRead(file);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		WaveMerge.RiffFile riffFile = WaveMerge.ReadRIFF(binaryReader);
		binaryReader.Close();
		fileStream.Close();
		return riffFile;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000067C0 File Offset: 0x000049C0
	public static short[] ReadSamples(string file)
	{
		FileStream fileStream = File.OpenRead(file);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		WaveMerge.RiffFile riffFile = WaveMerge.ReadRIFF(binaryReader);
		short[] array = new short[riffFile.Wave.PayLoadLength / 2U];
		fileStream.Position = riffFile.Wave.PayLoadStart;
		int num = 2000;
		if (riffFile.Wave.PayLoadLength >= 2147483647U)
		{
			Buffer.BlockCopy(binaryReader.ReadBytes(int.MaxValue - num), 0, array, 0, int.MaxValue - num);
			Buffer.BlockCopy(binaryReader.ReadBytes((int)((ulong)riffFile.Wave.PayLoadLength - (ulong)((long)(int.MaxValue - num)))), 0, array, (int.MaxValue - num) / 2, (int)((ulong)riffFile.Wave.PayLoadLength - (ulong)((long)(int.MaxValue - num))));
		}
		else
		{
			Buffer.BlockCopy(binaryReader.ReadBytes((int)riffFile.Wave.PayLoadLength), 0, array, 0, (int)riffFile.Wave.PayLoadLength);
		}
		binaryReader.Close();
		fileStream.Close();
		return array;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000068B0 File Offset: 0x00004AB0
	public static short[] ReadSamplesFromBytes(byte[] file)
	{
		MemoryStream memoryStream = new MemoryStream(file);
		BinaryReader binaryReader = new BinaryReader(memoryStream);
		WaveMerge.RiffFile riffFile = WaveMerge.ReadRIFF(binaryReader);
		short[] array = new short[riffFile.Wave.PayLoadLength / 2U];
		memoryStream.Position = riffFile.Wave.PayLoadStart;
		int num = 2000;
		if (riffFile.Wave.PayLoadLength >= 2147483647U)
		{
			Buffer.BlockCopy(binaryReader.ReadBytes(int.MaxValue - num), 0, array, 0, int.MaxValue - num);
			Buffer.BlockCopy(binaryReader.ReadBytes((int)((ulong)riffFile.Wave.PayLoadLength - (ulong)((long)(int.MaxValue - num)))), 0, array, (int.MaxValue - num) / 2, (int)((ulong)riffFile.Wave.PayLoadLength - (ulong)((long)(int.MaxValue - num))));
		}
		else
		{
			Buffer.BlockCopy(binaryReader.ReadBytes((int)riffFile.Wave.PayLoadLength), 0, array, 0, (int)riffFile.Wave.PayLoadLength);
		}
		binaryReader.Close();
		memoryStream.Close();
		return array;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000069A0 File Offset: 0x00004BA0
	public unsafe static MemoryTributary ReadSamplesAsStream(string file)
	{
		short[] array = WaveMerge.ReadSamples(file);
		MemoryTributary memoryTributary = new MemoryTributary();
		short[] array2;
		if ((array2 = array) == null || array2.Length == 0)
		{
			short* ptr = null;
		}
		else
		{
			short* ptr = &array2[0];
		}
		byte[] array3 = new byte[2147473647];
		uint num = (uint)(array.Length * 2);
		while (num > 0U && memoryTributary.Length != (long)array.Length * 2L)
		{
			long num2 = (long)array.Length * 2L - (long)((ulong)num);
			Buffer.BlockCopy(array, (int)num2, array3, 0, (int)Math.Min((long)array3.Length, (long)((ulong)num)));
			memoryTributary.Write(array3, 0, (int)Math.Min((long)array3.Length, (long)((ulong)num)));
			num -= (uint)array3.Length;
		}
		array2 = null;
		return memoryTributary;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00006A48 File Offset: 0x00004C48
	public static short[][] SplitChannels(short[] samples, int channels)
	{
		short[][] array = new short[channels][];
		for (int i = 0; i < channels; i++)
		{
			array[i] = new short[samples.Length / channels];
			for (int j = 0; j < samples.Length; j += channels)
			{
				array[i][j / channels] = samples[j + i];
			}
		}
		return array;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00006A90 File Offset: 0x00004C90
	public static short[] SamplesToShort(byte[] samples)
	{
		short[] array = new short[samples.Length / 2];
		Buffer.BlockCopy(samples, 0, array, 0, samples.Length);
		return array;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00006AB8 File Offset: 0x00004CB8
	public static double[] SamplesToDouble(short[] samples)
	{
		double[] array = new double[samples.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (double)samples[i] / 32767.0;
		}
		return array;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00006AEE File Offset: 0x00004CEE
	public static float[] SamplesToFloat(byte[] samples)
	{
		return WaveMerge.SamplesToFloat(WaveMerge.SamplesToShort(samples));
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00006AFC File Offset: 0x00004CFC
	public static float[] SamplesToFloat(short[] samples)
	{
		float[] array = new float[samples.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (float)samples[i] / 32767f;
		}
		return array;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00006B2E File Offset: 0x00004D2E
	public static byte[] SamplesToByte(double[] samples)
	{
		return WaveMerge.SamplesToByte(WaveMerge.SamplesToShort(samples));
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00006B3B File Offset: 0x00004D3B
	public static byte[] SamplesToByte(float[] samples)
	{
		return WaveMerge.SamplesToByte(WaveMerge.SamplesToShort(samples));
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00006B48 File Offset: 0x00004D48
	public static byte[] SamplesToByte(short[] samples)
	{
		byte[] array = new byte[samples.Length * 2];
		Buffer.BlockCopy(samples, 0, array, 0, array.Length);
		return array;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00006B70 File Offset: 0x00004D70
	public static short[] SamplesToShort(double[] r)
	{
		short[] array = new short[r.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (short)Math.Max(-32768.0, Math.Min(32767.0, r[i] * 32767.0));
		}
		return array;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00006BC4 File Offset: 0x00004DC4
	public static short[] SamplesToShort(float[] r)
	{
		short[] array = new short[r.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (short)Math.Max(-32768f, Math.Min(32767f, r[i] * 32767f));
		}
		return array;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00006C0C File Offset: 0x00004E0C
	public static WaveMerge.RiffFile ReadRIFF(BinaryReader stream)
	{
		WaveMerge.RiffFile riffFile = default(WaveMerge.RiffFile);
		riffFile.Stream = stream;
		while (stream.BaseStream.Position < stream.BaseStream.Length)
		{
			string @string = Encoding.UTF8.GetString(stream.ReadBytes(4));
			if (!(@string == "RIFF"))
			{
				if (@string == "WAVE")
				{
					riffFile.Wave = WaveMerge.ReadWav(stream, riffFile.PayLoadLength);
				}
			}
			else
			{
				riffFile.PayLoadLength = stream.ReadUInt32();
			}
		}
		if (riffFile.PayLoadLength == 0U)
		{
			riffFile.PayLoadLength = Convert.ToUInt32(stream.BaseStream.Length);
			riffFile.Wave.PayLoadLength = riffFile.PayLoadLength;
		}
		return riffFile;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00006CC8 File Offset: 0x00004EC8
	private static WaveMerge.WaveFile ReadWav(BinaryReader stream, uint length)
	{
		WaveMerge.WaveFile waveFile = default(WaveMerge.WaveFile);
		List<WaveMerge.RIFF_Chunk> list = new List<WaveMerge.RIFF_Chunk>();
		while (stream.BaseStream.Position < stream.BaseStream.Length && stream.BaseStream.Position < (long)((ulong)length))
		{
			string @string = Encoding.UTF8.GetString(stream.ReadBytes(4));
			if (!(@string == "fmt "))
			{
				if (!(@string == "data"))
				{
					list.Add(new WaveMerge.RIFF_Chunk
					{
						label = @string,
						data = WaveMerge.ReadBuffer(stream, stream.ReadInt32())
					});
					continue;
				}
			}
			else
			{
				waveFile.FormatBytes = WaveMerge.ReadBuffer(stream, stream.ReadInt32());
				waveFile.Format = default(WaveMerge.Format_Chunk);
				GCHandle gchandle = GCHandle.Alloc(waveFile.FormatBytes, GCHandleType.Pinned);
				try
				{
					waveFile.Format = (WaveMerge.Format_Chunk)Marshal.PtrToStructure(gchandle.AddrOfPinnedObject(), waveFile.Format.GetType());
					continue;
				}
				finally
				{
					gchandle.Free();
				}
			}
			waveFile.PayLoadLength = stream.ReadUInt32();
			waveFile.PayLoadStart = stream.BaseStream.Position;
			stream.BaseStream.Seek((long)((ulong)waveFile.PayLoadLength), SeekOrigin.Current);
		}
		if (list.Count > 0)
		{
			waveFile.extraChunks = list.ToArray();
		}
		return waveFile;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00006E28 File Offset: 0x00005028
	private static byte[] ReadBuffer(BinaryReader stream, int Amount)
	{
		byte[] array = new byte[Amount];
		stream.Read(array, 0, Amount);
		return array;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00006E47 File Offset: 0x00005047
	private WaveMerge()
	{
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00006E50 File Offset: 0x00005050
	public static byte[] NormalizeVolume(string file)
	{
		FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		WaveMerge.RiffFile riffFile = WaveMerge.ReadRIFF(binaryReader);
		binaryReader.BaseStream.Position = riffFile.Wave.PayLoadStart;
		byte[] array = binaryReader.ReadBytes((int)riffFile.Wave.PayLoadLength);
		fileStream.Close();
		return WaveMerge.NormalizeVolume(array);
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00006EA4 File Offset: 0x000050A4
	public static short[] NormalizeVolume(short[] Samples)
	{
		int num = int.MinValue;
		foreach (short num2 in Samples)
		{
			if (num2 == -32768)
			{
				return Samples;
			}
			num = Math.Max(num, (int)Math.Abs(num2));
		}
		if (num > 0)
		{
			double num3 = (double)(32767 / num);
			if (num3 > 1.0)
			{
				for (int j = 0; j <= Samples.Length - 1; j++)
				{
					Samples[j] = Convert.ToInt16((double)Samples[j] * num3);
				}
			}
		}
		return Samples;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006F24 File Offset: 0x00005124
	private static short search(short val, short[] table)
	{
		short num = 0;
		while ((int)num < table.Length)
		{
			if (val <= table[(int)num])
			{
				return num;
			}
			num += 1;
		}
		return (short)table.Length;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00006F4C File Offset: 0x0000514C
	public static byte pcm2mulaw(short pcm_val)
	{
		pcm_val = (short)(pcm_val >> 2);
		short num;
		if (pcm_val < 0)
		{
			pcm_val = -pcm_val;
			num = 127;
		}
		else
		{
			num = 255;
		}
		if (pcm_val > 8159)
		{
			pcm_val = 8159;
		}
		pcm_val += 33;
		short num2 = WaveMerge.search(pcm_val, WaveMerge.seg_uend);
		byte b;
		if (num2 >= 8)
		{
			b = (byte)(127 ^ num);
		}
		else
		{
			b = (byte)((short)((byte)(((int)num2 << 4) | ((pcm_val >> (int)(num2 + 1)) & 15))) ^ num);
		}
		if (b >= 128)
		{
			return byte.MaxValue - (b - 128);
		}
		return b;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00006FD0 File Offset: 0x000051D0
	public static short[] mulaw2pcm(byte[] u_val)
	{
		short[] array = new short[u_val.Length];
		for (int i = 0; i < u_val.Length; i++)
		{
			array[i] = WaveMerge.mulaw2pcm(u_val[i]);
		}
		return array;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00007000 File Offset: 0x00005200
	public static short mulaw2pcm(byte u_val)
	{
		if (u_val >= 128)
		{
			u_val = byte.MaxValue - (u_val - 128);
		}
		u_val = ~u_val;
		short num = (short)(((int)(u_val & 15) << 3) + 132);
		num = (short)(num << ((u_val & 112) >> 4));
		return ((u_val & 128) != 0) ? (132 - num) : (num - 132);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00007060 File Offset: 0x00005260
	public static float GetNormalizationFactor(float[] samples)
	{
		float num = 3.051851E-05f;
		foreach (float num2 in samples)
		{
			num = Math.Max(num, Math.Abs(num2));
		}
		return 1f / num;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x0000709C File Offset: 0x0000529C
	public static short[] NormalizeVolume95(short[] Samples, float maxV = float.NaN)
	{
		List<int> list = new List<int>();
		foreach (short num in Samples)
		{
			list.Add(Math.Abs((int)num));
		}
		list.Sort();
		int num2 = (int)((float)list.Count * 0.9885f);
		int num3 = list[num2];
		num3 = list[list.Count - 1];
		if (maxV != float.NaN)
		{
			num3 = (int)((short)maxV);
		}
		if (num3 > 0)
		{
			double num4 = (double)(32390.18f / (float)num3);
			if (num4 > 1.0)
			{
				for (int j = 0; j <= Samples.Length - 1; j++)
				{
					Samples[j] = Convert.ToInt16(Math.Max(-32768.0, Math.Min(32767.0, (double)Samples[j] * num4)));
				}
			}
		}
		return Samples;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00007174 File Offset: 0x00005374
	public static byte[] NormalizeVolume(byte[] Samples)
	{
		short[] array = new short[Samples.Length / 2];
		Buffer.BlockCopy(Samples, 0, array, 0, Samples.Length);
		Buffer.BlockCopy(WaveMerge.NormalizeVolume95(array, float.NaN), 0, Samples, 0, Samples.Length);
		return Samples;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000071B0 File Offset: 0x000053B0
	public static void NormalizeVolume(Stream stream, Stream writeTo)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		WaveMerge.RiffFile riffFile = WaveMerge.ReadRIFF(binaryReader);
		binaryReader.BaseStream.Position = riffFile.Wave.PayLoadStart;
		WaveMerge.CreateWave(WaveMerge.NormalizeVolume(binaryReader.ReadBytes((int)riffFile.Wave.PayLoadLength)), riffFile.Wave.Format.SampleRate, (int)riffFile.Wave.Format.BitsPerSample, Convert.ToByte(riffFile.Wave.Format.NumberOfChannels), writeTo, new WaveMerge.RIFF_Chunk[0]);
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00007238 File Offset: 0x00005438
	public static WaveMerge.AUDIO_GRADE GradeAudio(short[] samples, int silenceThreshold = 800)
	{
		if (samples == null || samples.Length == 0)
		{
			return WaveMerge.AUDIO_GRADE.QUIET;
		}
		WaveMerge.AUDIO_GRADE audio_GRADE = WaveMerge.AUDIO_GRADE.GOOD;
		List<float> list = new List<float>();
		list.Add(0f);
		int num = 0;
		double num2 = 0.0;
		double num3 = 0.0;
		foreach (short num4 in samples)
		{
			num2 += (double)num4;
			num3 += (double)Math.Abs(num4);
			if (Math.Abs((int)num4) > silenceThreshold)
			{
				float num5 = (float)Math.Abs((int)num4) / 32768f;
				list.Add(num5);
			}
			else
			{
				num++;
			}
		}
		list.Sort();
		float num6 = list[Math.Min(list.Count - 1, (int)((double)list.Count * 0.95))];
		float num7 = 1f * (float)num / (float)samples.Length;
		if (num2 == 0.0 || num3 / (double)samples.Length < 10.0 || num3 == 0.0)
		{
			audio_GRADE = WaveMerge.AUDIO_GRADE.NO_AUDIO;
		}
		else if ((double)num7 > 0.9 || (double)num6 < 0.1)
		{
			audio_GRADE = WaveMerge.AUDIO_GRADE.VERY_QUIET;
		}
		else if ((double)num7 > 0.9 || (double)num6 < 0.2)
		{
			audio_GRADE = WaveMerge.AUDIO_GRADE.QUIET;
		}
		else if ((double)num7 < 0.25)
		{
			audio_GRADE = WaveMerge.AUDIO_GRADE.NOISY;
		}
		else if ((double)num6 >= 0.2 && (double)num6 <= 0.6)
		{
			audio_GRADE = WaveMerge.AUDIO_GRADE.GOOD;
		}
		else if ((double)num6 > 0.6)
		{
			audio_GRADE = WaveMerge.AUDIO_GRADE.LOUD;
		}
		return audio_GRADE;
	}

	// Token: 0x04000052 RID: 82
	private const int TruncateBytes = 0;

	// Token: 0x04000053 RID: 83
	private static short[] seg_uend = new short[] { 63, 127, 255, 511, 1023, 2047, 4095, 8191 };

	// Token: 0x04000054 RID: 84
	private const short CLIP = 8159;

	// Token: 0x04000055 RID: 85
	private const byte BIAS = 132;

	// Token: 0x0200005F RID: 95
	public struct RiffFile
	{
		// Token: 0x040001D3 RID: 467
		public WaveMerge.WaveFile Wave;

		// Token: 0x040001D4 RID: 468
		public uint PayLoadLength;

		// Token: 0x040001D5 RID: 469
		public BinaryReader Stream;
	}

	// Token: 0x02000060 RID: 96
	public struct Format_Chunk
	{
		// Token: 0x040001D6 RID: 470
		public short CompressionCode;

		// Token: 0x040001D7 RID: 471
		public short NumberOfChannels;

		// Token: 0x040001D8 RID: 472
		public int SampleRate;

		// Token: 0x040001D9 RID: 473
		public int AverageBytesPerSecond;

		// Token: 0x040001DA RID: 474
		public short BlockAlign;

		// Token: 0x040001DB RID: 475
		public short BitsPerSample;
	}

	// Token: 0x02000061 RID: 97
	public struct WaveFile
	{
		// Token: 0x040001DC RID: 476
		public byte[] FormatBytes;

		// Token: 0x040001DD RID: 477
		public WaveMerge.Format_Chunk Format;

		// Token: 0x040001DE RID: 478
		public long PayLoadStart;

		// Token: 0x040001DF RID: 479
		public uint PayLoadLength;

		// Token: 0x040001E0 RID: 480
		public WaveMerge.RIFF_Chunk[] extraChunks;
	}

	// Token: 0x02000062 RID: 98
	public class RIFF_Chunk
	{
		// Token: 0x06000313 RID: 787 RVA: 0x0000E46D File Offset: 0x0000C66D
		public int GetLength()
		{
			if (this.data != null)
			{
				return 8 + this.data.Length;
			}
			return 8;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000E483 File Offset: 0x0000C683
		public RIFF_Chunk()
		{
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000E48B File Offset: 0x0000C68B
		public RIFF_Chunk(string l, byte[] d)
		{
			this.label = l;
			this.data = d;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000E4A4 File Offset: 0x0000C6A4
		public void Serialize(BinaryWriter bw)
		{
			string text = this.label.Substring(0, Math.Min(4, this.label.Length));
			text = text.PadRight(4, ' ');
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			bw.Write(bytes);
			if (this.data != null)
			{
				bw.Write(this.data.Length);
				bw.Write(this.data);
				return;
			}
			bw.Write(0);
		}

		// Token: 0x040001E1 RID: 481
		public string label;

		// Token: 0x040001E2 RID: 482
		public byte[] data;
	}

	// Token: 0x02000063 RID: 99
	public enum AUDIO_GRADE
	{
		// Token: 0x040001E4 RID: 484
		GOOD,
		// Token: 0x040001E5 RID: 485
		NOISY,
		// Token: 0x040001E6 RID: 486
		LOUD,
		// Token: 0x040001E7 RID: 487
		QUIET,
		// Token: 0x040001E8 RID: 488
		NO_AUDIO,
		// Token: 0x040001E9 RID: 489
		VERY_QUIET
	}
}
