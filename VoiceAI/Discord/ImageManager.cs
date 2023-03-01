using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000048 RID: 72
	public class ImageManager
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000B08E File Offset: 0x0000928E
		private ImageManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ImageManager.FFIMethods));
				}
				return (ImageManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B0C0 File Offset: 0x000092C0
		internal ImageManager(IntPtr ptr, IntPtr eventsPtr, ref ImageManager.FFIEvents events)
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

		// Token: 0x06000229 RID: 553 RVA: 0x0000B10F File Offset: 0x0000930F
		private void InitEvents(IntPtr eventsPtr, ref ImageManager.FFIEvents events)
		{
			Marshal.StructureToPtr<ImageManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000B120 File Offset: 0x00009320
		[MonoPInvokeCallback]
		private static void FetchCallbackImpl(IntPtr ptr, Result result, ImageHandle handleResult)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ImageManager.FetchHandler fetchHandler = (ImageManager.FetchHandler)gchandle.Target;
			gchandle.Free();
			fetchHandler(result, handleResult);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B150 File Offset: 0x00009350
		public void Fetch(ImageHandle handle, bool refresh, ImageManager.FetchHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.Fetch(this.MethodsPtr, handle, refresh, GCHandle.ToIntPtr(gchandle), new ImageManager.FFIMethods.FetchCallback(ImageManager.FetchCallbackImpl));
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000B190 File Offset: 0x00009390
		public ImageDimensions GetDimensions(ImageHandle handle)
		{
			ImageDimensions imageDimensions = default(ImageDimensions);
			Result result = this.Methods.GetDimensions(this.MethodsPtr, handle, ref imageDimensions);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return imageDimensions;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000B1CC File Offset: 0x000093CC
		public void GetData(ImageHandle handle, byte[] data)
		{
			Result result = this.Methods.GetData(this.MethodsPtr, handle, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000B1FF File Offset: 0x000093FF
		public void Fetch(ImageHandle handle, ImageManager.FetchHandler callback)
		{
			this.Fetch(handle, false, callback);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000B20C File Offset: 0x0000940C
		public byte[] GetData(ImageHandle handle)
		{
			ImageDimensions dimensions = this.GetDimensions(handle);
			byte[] array = new byte[dimensions.Width * dimensions.Height * 4U];
			this.GetData(handle, array);
			return array;
		}

		// Token: 0x04000183 RID: 387
		private IntPtr MethodsPtr;

		// Token: 0x04000184 RID: 388
		private object MethodsStructure;

		// Token: 0x0200008E RID: 142
		internal struct FFIEvents
		{
		}

		// Token: 0x0200008F RID: 143
		internal struct FFIMethods
		{
			// Token: 0x0400025E RID: 606
			internal ImageManager.FFIMethods.FetchMethod Fetch;

			// Token: 0x0400025F RID: 607
			internal ImageManager.FFIMethods.GetDimensionsMethod GetDimensions;

			// Token: 0x04000260 RID: 608
			internal ImageManager.FFIMethods.GetDataMethod GetData;

			// Token: 0x02000106 RID: 262
			// (Invoke) Token: 0x06000503 RID: 1283
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchCallback(IntPtr ptr, Result result, ImageHandle handleResult);

			// Token: 0x02000107 RID: 263
			// (Invoke) Token: 0x06000507 RID: 1287
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchMethod(IntPtr methodsPtr, ImageHandle handle, bool refresh, IntPtr callbackData, ImageManager.FFIMethods.FetchCallback callback);

			// Token: 0x02000108 RID: 264
			// (Invoke) Token: 0x0600050B RID: 1291
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetDimensionsMethod(IntPtr methodsPtr, ImageHandle handle, ref ImageDimensions dimensions);

			// Token: 0x02000109 RID: 265
			// (Invoke) Token: 0x0600050F RID: 1295
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetDataMethod(IntPtr methodsPtr, ImageHandle handle, byte[] data, int dataLen);
		}

		// Token: 0x02000090 RID: 144
		// (Invoke) Token: 0x0600036F RID: 879
		public delegate void FetchHandler(Result result, ImageHandle handleResult);
	}
}
