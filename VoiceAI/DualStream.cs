using System;
using System.IO;

// Token: 0x02000008 RID: 8
internal class DualStream : Stream
{
	// Token: 0x06000094 RID: 148 RVA: 0x00005370 File Offset: 0x00003570
	public DualStream(long maxBufferLength = 9223372036854775807L)
	{
		this.maxLength = maxBufferLength;
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000095 RID: 149 RVA: 0x00005395 File Offset: 0x00003595
	public override bool CanRead
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000096 RID: 150 RVA: 0x00005398 File Offset: 0x00003598
	public override bool CanSeek
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000097 RID: 151 RVA: 0x0000539B File Offset: 0x0000359B
	public override bool CanWrite
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000098 RID: 152 RVA: 0x0000539E File Offset: 0x0000359E
	public override long Length
	{
		get
		{
			return this.ms.Length;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000099 RID: 153 RVA: 0x000053AB File Offset: 0x000035AB
	// (set) Token: 0x0600009A RID: 154 RVA: 0x000053B3 File Offset: 0x000035B3
	public override long Position
	{
		get
		{
			return this.readOffset;
		}
		set
		{
			this.readOffset = value;
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x000053BC File Offset: 0x000035BC
	public override void Flush()
	{
	}

	// Token: 0x0600009C RID: 156 RVA: 0x000053C0 File Offset: 0x000035C0
	public override int Read(byte[] buffer, int offset, int count)
	{
		object obj = this.syncObject;
		int num2;
		lock (obj)
		{
			this.ms.Position = this.readOffset;
			int num = this.ms.Read(buffer, offset, count);
			this.readOffset += (long)num;
			this.ms.Position = this.ms.Length;
			num2 = num;
		}
		return num2;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00005444 File Offset: 0x00003644
	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0000544C File Offset: 0x0000364C
	public override void SetLength(long value)
	{
		object obj = this.syncObject;
		lock (obj)
		{
			this.ms.SetLength(value);
			this.readOffset = value;
			this.ms.Position = value;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600009F RID: 159 RVA: 0x000054A8 File Offset: 0x000036A8
	public int DataRemaining
	{
		get
		{
			object obj = this.syncObject;
			int num;
			lock (obj)
			{
				num = (int)(this.ms.Length - this.readOffset);
			}
			return num;
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000054F8 File Offset: 0x000036F8
	public override void Write(byte[] buffer, int offset, int count)
	{
		object obj = this.syncObject;
		lock (obj)
		{
			this.ms.Position = this.ms.Length;
			if (this.ms.Length + (long)count > this.maxLength)
			{
				long num = this.ms.Length + (long)count - this.maxLength;
				if (this.readOffset - num >= 0L)
				{
					byte[] array = this.ms.ToArray();
					this.ms.SetLength(0L);
					this.ms.Write(array, (int)num, (int)((long)array.Length - num));
					this.readOffset -= num;
				}
			}
			this.ms.Write(buffer, offset, count);
			this.ms.Position = this.readOffset;
		}
	}

	// Token: 0x0400003F RID: 63
	private MemoryStream ms = new MemoryStream();

	// Token: 0x04000040 RID: 64
	private long readOffset;

	// Token: 0x04000041 RID: 65
	private long maxLength;

	// Token: 0x04000042 RID: 66
	private object syncObject = new object();
}
