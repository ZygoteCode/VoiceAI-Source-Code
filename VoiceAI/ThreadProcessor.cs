using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x02000013 RID: 19
public class ThreadProcessor
{
	// Token: 0x06000141 RID: 321 RVA: 0x000082EC File Offset: 0x000064EC
	private ThreadProcessor()
	{
		this.thread = new Thread(new ParameterizedThreadStart(this.event_loop));
		this.thread.Name = "ThreadProcessorMainThread";
		this.thread.Start();
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00008344 File Offset: 0x00006544
	private void event_loop(object state)
	{
		while (this.running || this.queue.Count > 0)
		{
			Monitor.Enter(this.queue);
			if (this.queue.Count > 0)
			{
				ThreadProcessor.QueueItem queueItem = this.queue.Dequeue();
				queueItem.result = queueItem.d(queueItem.param1);
				Monitor.Exit(this.queue);
				queueItem.w.Set();
			}
			else
			{
				Monitor.Exit(this.queue);
				Thread.Sleep(1);
			}
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x000083CE File Offset: 0x000065CE
	private void _Dispose()
	{
		this.running = false;
	}

	// Token: 0x06000144 RID: 324 RVA: 0x000083D7 File Offset: 0x000065D7
	public static void Dispose()
	{
		ThreadProcessor.singleton()._Dispose();
	}

	// Token: 0x06000145 RID: 325 RVA: 0x000083E3 File Offset: 0x000065E3
	private static ThreadProcessor singleton()
	{
		if (ThreadProcessor.threadProcessor == null)
		{
			ThreadProcessor.threadProcessor = new ThreadProcessor();
		}
		return ThreadProcessor.threadProcessor;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000083FB File Offset: 0x000065FB
	public static object RunOnThread(ThreadProcessor.ThreadFunc j, object param1 = null)
	{
		return ThreadProcessor.singleton()._RunOnThread(j, param1);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000840C File Offset: 0x0000660C
	public object _RunOnThread(ThreadProcessor.ThreadFunc j, object param1 = null)
	{
		ThreadProcessor.QueueItem queueItem = new ThreadProcessor.QueueItem();
		Queue<ThreadProcessor.QueueItem> queue = this.queue;
		lock (queue)
		{
			queueItem.d = j;
			queueItem.param1 = param1;
			queueItem.result = null;
			queueItem.w = new ManualResetEventSlim(false);
			this.queue.Enqueue(queueItem);
		}
		queueItem.w.Wait();
		return queueItem.result;
	}

	// Token: 0x04000079 RID: 121
	private static ThreadProcessor threadProcessor;

	// Token: 0x0400007A RID: 122
	private Thread thread;

	// Token: 0x0400007B RID: 123
	private bool running = true;

	// Token: 0x0400007C RID: 124
	private Queue<ThreadProcessor.QueueItem> queue = new Queue<ThreadProcessor.QueueItem>();

	// Token: 0x0200006D RID: 109
	private class QueueItem
	{
		// Token: 0x04000204 RID: 516
		public ThreadProcessor.ThreadFunc d;

		// Token: 0x04000205 RID: 517
		public ManualResetEventSlim w;

		// Token: 0x04000206 RID: 518
		public object param1;

		// Token: 0x04000207 RID: 519
		public object result;
	}

	// Token: 0x0200006E RID: 110
	// (Invoke) Token: 0x06000327 RID: 807
	public delegate object ThreadFunc(object o);
}
