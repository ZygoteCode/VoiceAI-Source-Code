using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000006 RID: 6
internal class AudioQueue
{
	// Token: 0x0600006A RID: 106 RVA: 0x00003BA0 File Offset: 0x00001DA0
	public AudioQueue(int maxFrameCount = 3000)
	{
		this.maxFrames = maxFrameCount;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003BC8 File Offset: 0x00001DC8
	public void Add(byte[] b)
	{
		object obj = this.syncObject;
		lock (obj)
		{
			this.markIndex++;
			this.queue.Add(b);
			this.didAddData = true;
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00003C24 File Offset: 0x00001E24
	public bool DidAddData()
	{
		object obj = this.syncObject;
		bool flag2;
		lock (obj)
		{
			flag2 = this.didAddData;
		}
		return flag2;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00003C68 File Offset: 0x00001E68
	public int GetLength()
	{
		object obj = this.syncObject;
		int count;
		lock (obj)
		{
			count = this.queue.Count;
		}
		return count;
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600006E RID: 110 RVA: 0x00003CB0 File Offset: 0x00001EB0
	public int Count
	{
		get
		{
			return this.queue.Count;
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00003CC0 File Offset: 0x00001EC0
	public void Clear()
	{
		object obj = this.syncObject;
		lock (obj)
		{
			this.markIndex = 0;
			this.didAddData = false;
			this.queue.Clear();
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00003D14 File Offset: 0x00001F14
	public void MarkClear()
	{
		object obj = this.syncObject;
		lock (obj)
		{
			this.markIndex = 0;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000071 RID: 113 RVA: 0x00003D58 File Offset: 0x00001F58
	public int MarkCount
	{
		get
		{
			return this.markIndex;
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00003D60 File Offset: 0x00001F60
	public byte[] Get(int frameCount, bool clearFlag)
	{
		int num = this.maxFrames;
		object obj = this.syncObject;
		byte[] array2;
		lock (obj)
		{
			if (clearFlag)
			{
				this.didAddData = false;
			}
			MemoryStream memoryStream = new MemoryStream();
			for (int i = Math.Max(0, this.queue.Count - frameCount); i < this.queue.Count; i++)
			{
				byte[] array = this.queue[i];
				memoryStream.Write(array, 0, array.Length);
			}
			while (this.queue.Count > num)
			{
				this.queue.RemoveAt(0);
			}
			array2 = memoryStream.ToArray();
		}
		return array2;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00003E20 File Offset: 0x00002020
	public byte[] xGet(int frameCount, int framesToLeave)
	{
		MemoryStream memoryStream = new MemoryStream();
		int num = 0;
		object obj = this.syncObject;
		lock (obj)
		{
			while (this.queue.Count > 0 && frameCount > 0)
			{
				if (num >= this.queue.Count)
				{
					break;
				}
				byte[] array = this.queue[num];
				if (this.queue.Count > framesToLeave)
				{
					this.queue.RemoveAt(0);
				}
				else
				{
					num++;
				}
				frameCount--;
				memoryStream.Write(array, 0, array.Length);
			}
			while (this.queue.Count > framesToLeave)
			{
				this.queue.RemoveAt(0);
			}
		}
		return memoryStream.ToArray();
	}

	// Token: 0x04000014 RID: 20
	private List<byte[]> queue = new List<byte[]>();

	// Token: 0x04000015 RID: 21
	public object syncObject = new object();

	// Token: 0x04000016 RID: 22
	private bool didAddData;

	// Token: 0x04000017 RID: 23
	private int maxFrames;

	// Token: 0x04000018 RID: 24
	private int markIndex;
}
