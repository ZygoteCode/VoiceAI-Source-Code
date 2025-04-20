using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200000B RID: 11
public class MemoryTributary : Stream
{
	// Token: 0x06000100 RID: 256 RVA: 0x000073D9 File Offset: 0x000055D9
	public MemoryTributary()
	{
		this.Position = 0L;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00007400 File Offset: 0x00005600
	public MemoryTributary(byte[] source)
	{
		this.Write(source, 0, source.Length);
		this.Position = 0L;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00007432 File Offset: 0x00005632
	public MemoryTributary(int length)
	{
		this.SetLength((long)length);
		this.Position = (long)length;
		byte[] block = this.block;
		this.Position = 0L;
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000103 RID: 259 RVA: 0x00007470 File Offset: 0x00005670
	public override bool CanRead
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000104 RID: 260 RVA: 0x00007473 File Offset: 0x00005673
	public override bool CanSeek
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000105 RID: 261 RVA: 0x00007476 File Offset: 0x00005676
	public override bool CanWrite
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000106 RID: 262 RVA: 0x00007479 File Offset: 0x00005679
	public override long Length
	{
		get
		{
			return this.length;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000107 RID: 263 RVA: 0x00007481 File Offset: 0x00005681
	// (set) Token: 0x06000108 RID: 264 RVA: 0x00007489 File Offset: 0x00005689
	public override long Position { get; set; }

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000109 RID: 265 RVA: 0x00007494 File Offset: 0x00005694
	protected byte[] block
	{
		get
		{
			while ((long)this.blocks.Count <= this.blockId)
			{
				this.blocks.Add(new byte[this.blockSize]);
			}
			return this.blocks[(int)this.blockId];
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600010A RID: 266 RVA: 0x000074E0 File Offset: 0x000056E0
	protected long blockId
	{
		get
		{
			return this.Position / this.blockSize;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600010B RID: 267 RVA: 0x000074EF File Offset: 0x000056EF
	protected long blockOffset
	{
		get
		{
			return this.Position % this.blockSize;
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x000074FE File Offset: 0x000056FE
	public override void Flush()
	{
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00007500 File Offset: 0x00005700
	public override int Read(byte[] buffer, int offset, int count)
	{
		long num = (long)count;
		if (num < 0L)
		{
			throw new ArgumentOutOfRangeException("count", num, "Number of bytes to copy cannot be negative.");
		}
		long num2 = this.length - this.Position;
		if (num > num2)
		{
			num = num2;
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", offset, "Destination offset cannot be negative.");
		}
		int num3 = 0;
		do
		{
			long num4 = Math.Min(num, this.blockSize - this.blockOffset);
			Buffer.BlockCopy(this.block, (int)this.blockOffset, buffer, offset, (int)num4);
			num -= num4;
			offset += (int)num4;
			num3 += (int)num4;
			this.Position += num4;
		}
		while (num > 0L);
		return num3;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x000075C0 File Offset: 0x000057C0
	public override long Seek(long offset, SeekOrigin origin)
	{
		switch (origin)
		{
		case SeekOrigin.Begin:
			this.Position = offset;
			break;
		case SeekOrigin.Current:
			this.Position += offset;
			break;
		case SeekOrigin.End:
			this.Position = this.Length - offset;
			break;
		}
		return this.Position;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0000760E File Offset: 0x0000580E
	public override void SetLength(long value)
	{
		this.length = value;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00007618 File Offset: 0x00005818
	public override void Write(byte[] buffer, int offset, int count)
	{
		long position = this.Position;
		try
		{
			do
			{
				int num = Math.Min(count, (int)(this.blockSize - this.blockOffset));
				this.EnsureCapacity(this.Position + (long)num);
				Buffer.BlockCopy(buffer, offset, this.block, (int)this.blockOffset, num);
				count -= num;
				offset += num;
				this.Position += (long)num;
			}
			while (count > 0);
		}
		catch (Exception ex)
		{
			this.Position = position;
			throw ex;
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x0000769C File Offset: 0x0000589C
	public override int ReadByte()
	{
		if (this.Position >= this.length)
		{
			return -1;
		}
		int num = (int)this.block[(int)(checked((IntPtr)this.blockOffset))];
		long position = this.Position;
		this.Position = position + 1L;
		return num;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x000076D8 File Offset: 0x000058D8
	public override void WriteByte(byte value)
	{
		this.EnsureCapacity(this.Position + 1L);
		this.block[(int)(checked((IntPtr)this.blockOffset))] = value;
		long position = this.Position;
		this.Position = position + 1L;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00007714 File Offset: 0x00005914
	protected void EnsureCapacity(long intended_length)
	{
		if (intended_length > this.length)
		{
			this.length = intended_length;
		}
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00007726 File Offset: 0x00005926
	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00007730 File Offset: 0x00005930
	public byte[] ToArray()
	{
		long position = this.Position;
		this.Position = 0L;
		byte[] array = new byte[this.Length];
		this.Read(array, 0, (int)this.Length);
		this.Position = position;
		return array;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00007774 File Offset: 0x00005974
	public void ReadFrom(Stream source, long length)
	{
		byte[] array = new byte[4096];
		do
		{
			int num = source.Read(array, 0, (int)Math.Min(4096L, length));
			length -= (long)num;
			this.Write(array, 0, num);
		}
		while (length > 0L);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x000077B8 File Offset: 0x000059B8
	public void WriteTo(Stream destination)
	{
		long position = this.Position;
		this.Position = 0L;
		base.CopyTo(destination);
		this.Position = position;
	}

	// Token: 0x04000057 RID: 87
	protected long length;

	// Token: 0x04000058 RID: 88
	protected long blockSize = 65536L;

	// Token: 0x04000059 RID: 89
	protected List<byte[]> blocks = new List<byte[]>();
}
