using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x0200004D RID: 77
	public class StorageManager
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000CDBB File Offset: 0x0000AFBB
		private StorageManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(StorageManager.FFIMethods));
				}
				return (StorageManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000CDEC File Offset: 0x0000AFEC
		internal StorageManager(IntPtr ptr, IntPtr eventsPtr, ref StorageManager.FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
			this.InitEvents(eventsPtr, ref events);
			this.MethodsPtr = ptr;
			if (this.MethodsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000CE3B File Offset: 0x0000B03B
		private void InitEvents(IntPtr eventsPtr, ref StorageManager.FFIEvents events)
		{
			Marshal.StructureToPtr<StorageManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000CE4C File Offset: 0x0000B04C
		public uint Read(string name, byte[] data)
		{
			uint num = 0U;
			Result result = this.Methods.Read(this.MethodsPtr, name, data, data.Length, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000CE84 File Offset: 0x0000B084
		[MonoPInvokeCallback]
		private static void ReadAsyncCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.ReadAsyncHandler readAsyncHandler = (StorageManager.ReadAsyncHandler)gchandle.Target;
			gchandle.Free();
			byte[] array = new byte[dataLen];
			Marshal.Copy(dataPtr, array, 0, dataLen);
			readAsyncHandler(result, array);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000CEC4 File Offset: 0x0000B0C4
		public void ReadAsync(string name, StorageManager.ReadAsyncHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ReadAsync(this.MethodsPtr, name, GCHandle.ToIntPtr(gchandle), new StorageManager.FFIMethods.ReadAsyncCallback(StorageManager.ReadAsyncCallbackImpl));
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000CF04 File Offset: 0x0000B104
		[MonoPInvokeCallback]
		private static void ReadAsyncPartialCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.ReadAsyncPartialHandler readAsyncPartialHandler = (StorageManager.ReadAsyncPartialHandler)gchandle.Target;
			gchandle.Free();
			byte[] array = new byte[dataLen];
			Marshal.Copy(dataPtr, array, 0, dataLen);
			readAsyncPartialHandler(result, array);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000CF44 File Offset: 0x0000B144
		public void ReadAsyncPartial(string name, ulong offset, ulong length, StorageManager.ReadAsyncPartialHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ReadAsyncPartial(this.MethodsPtr, name, offset, length, GCHandle.ToIntPtr(gchandle), new StorageManager.FFIMethods.ReadAsyncPartialCallback(StorageManager.ReadAsyncPartialCallbackImpl));
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000CF84 File Offset: 0x0000B184
		public void Write(string name, byte[] data)
		{
			Result result = this.Methods.Write(this.MethodsPtr, name, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000CFB8 File Offset: 0x0000B1B8
		[MonoPInvokeCallback]
		private static void WriteAsyncCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.WriteAsyncHandler writeAsyncHandler = (StorageManager.WriteAsyncHandler)gchandle.Target;
			gchandle.Free();
			writeAsyncHandler(result);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
		public void WriteAsync(string name, byte[] data, StorageManager.WriteAsyncHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.WriteAsync(this.MethodsPtr, name, data, data.Length, GCHandle.ToIntPtr(gchandle), new StorageManager.FFIMethods.WriteAsyncCallback(StorageManager.WriteAsyncCallbackImpl));
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000D02C File Offset: 0x0000B22C
		public void Delete(string name)
		{
			Result result = this.Methods.Delete(this.MethodsPtr, name);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000D05C File Offset: 0x0000B25C
		public bool Exists(string name)
		{
			bool flag = false;
			Result result = this.Methods.Exists(this.MethodsPtr, name, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000D090 File Offset: 0x0000B290
		public int Count()
		{
			int num = 0;
			this.Methods.Count(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		public FileStat Stat(string name)
		{
			FileStat fileStat = default(FileStat);
			Result result = this.Methods.Stat(this.MethodsPtr, name, ref fileStat);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return fileStat;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000D0F4 File Offset: 0x0000B2F4
		public FileStat StatAt(int index)
		{
			FileStat fileStat = default(FileStat);
			Result result = this.Methods.StatAt(this.MethodsPtr, index, ref fileStat);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return fileStat;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000D130 File Offset: 0x0000B330
		public string GetPath()
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetPath(this.MethodsPtr, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000D170 File Offset: 0x0000B370
		public IEnumerable<FileStat> Files()
		{
			int num = this.Count();
			List<FileStat> list = new List<FileStat>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.StatAt(i));
			}
			return list;
		}

		// Token: 0x0400019A RID: 410
		private IntPtr MethodsPtr;

		// Token: 0x0400019B RID: 411
		private object MethodsStructure;

		// Token: 0x020000B6 RID: 182
		internal struct FFIEvents
		{
		}

		// Token: 0x020000B7 RID: 183
		internal struct FFIMethods
		{
			// Token: 0x040002A1 RID: 673
			internal StorageManager.FFIMethods.ReadMethod Read;

			// Token: 0x040002A2 RID: 674
			internal StorageManager.FFIMethods.ReadAsyncMethod ReadAsync;

			// Token: 0x040002A3 RID: 675
			internal StorageManager.FFIMethods.ReadAsyncPartialMethod ReadAsyncPartial;

			// Token: 0x040002A4 RID: 676
			internal StorageManager.FFIMethods.WriteMethod Write;

			// Token: 0x040002A5 RID: 677
			internal StorageManager.FFIMethods.WriteAsyncMethod WriteAsync;

			// Token: 0x040002A6 RID: 678
			internal StorageManager.FFIMethods.DeleteMethod Delete;

			// Token: 0x040002A7 RID: 679
			internal StorageManager.FFIMethods.ExistsMethod Exists;

			// Token: 0x040002A8 RID: 680
			internal StorageManager.FFIMethods.CountMethod Count;

			// Token: 0x040002A9 RID: 681
			internal StorageManager.FFIMethods.StatMethod Stat;

			// Token: 0x040002AA RID: 682
			internal StorageManager.FFIMethods.StatAtMethod StatAt;

			// Token: 0x040002AB RID: 683
			internal StorageManager.FFIMethods.GetPathMethod GetPath;

			// Token: 0x0200015A RID: 346
			// (Invoke) Token: 0x06000653 RID: 1619
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ReadMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen, ref uint read);

			// Token: 0x0200015B RID: 347
			// (Invoke) Token: 0x06000657 RID: 1623
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			// Token: 0x0200015C RID: 348
			// (Invoke) Token: 0x0600065B RID: 1627
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, IntPtr callbackData, StorageManager.FFIMethods.ReadAsyncCallback callback);

			// Token: 0x0200015D RID: 349
			// (Invoke) Token: 0x0600065F RID: 1631
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncPartialCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			// Token: 0x0200015E RID: 350
			// (Invoke) Token: 0x06000663 RID: 1635
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncPartialMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ulong offset, ulong length, IntPtr callbackData, StorageManager.FFIMethods.ReadAsyncPartialCallback callback);

			// Token: 0x0200015F RID: 351
			// (Invoke) Token: 0x06000667 RID: 1639
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result WriteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen);

			// Token: 0x02000160 RID: 352
			// (Invoke) Token: 0x0600066B RID: 1643
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void WriteAsyncCallback(IntPtr ptr, Result result);

			// Token: 0x02000161 RID: 353
			// (Invoke) Token: 0x0600066F RID: 1647
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void WriteAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen, IntPtr callbackData, StorageManager.FFIMethods.WriteAsyncCallback callback);

			// Token: 0x02000162 RID: 354
			// (Invoke) Token: 0x06000673 RID: 1651
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name);

			// Token: 0x02000163 RID: 355
			// (Invoke) Token: 0x06000677 RID: 1655
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ExistsMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ref bool exists);

			// Token: 0x02000164 RID: 356
			// (Invoke) Token: 0x0600067B RID: 1659
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x02000165 RID: 357
			// (Invoke) Token: 0x0600067F RID: 1663
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result StatMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ref FileStat stat);

			// Token: 0x02000166 RID: 358
			// (Invoke) Token: 0x06000683 RID: 1667
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result StatAtMethod(IntPtr methodsPtr, int index, ref FileStat stat);

			// Token: 0x02000167 RID: 359
			// (Invoke) Token: 0x06000687 RID: 1671
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetPathMethod(IntPtr methodsPtr, StringBuilder path);
		}

		// Token: 0x020000B8 RID: 184
		// (Invoke) Token: 0x060003E7 RID: 999
		public delegate void ReadAsyncHandler(Result result, byte[] data);

		// Token: 0x020000B9 RID: 185
		// (Invoke) Token: 0x060003EB RID: 1003
		public delegate void ReadAsyncPartialHandler(Result result, byte[] data);

		// Token: 0x020000BA RID: 186
		// (Invoke) Token: 0x060003EF RID: 1007
		public delegate void WriteAsyncHandler(Result result);
	}
}
