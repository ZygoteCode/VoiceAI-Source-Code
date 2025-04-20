using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000049 RID: 73
	public class RelationshipManager
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B23F File Offset: 0x0000943F
		private RelationshipManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(RelationshipManager.FFIMethods));
				}
				return (RelationshipManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000231 RID: 561 RVA: 0x0000B270 File Offset: 0x00009470
		// (remove) Token: 0x06000232 RID: 562 RVA: 0x0000B2A8 File Offset: 0x000094A8
		public event RelationshipManager.RefreshHandler OnRefresh;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000233 RID: 563 RVA: 0x0000B2E0 File Offset: 0x000094E0
		// (remove) Token: 0x06000234 RID: 564 RVA: 0x0000B318 File Offset: 0x00009518
		public event RelationshipManager.RelationshipUpdateHandler OnRelationshipUpdate;

		// Token: 0x06000235 RID: 565 RVA: 0x0000B350 File Offset: 0x00009550
		internal RelationshipManager(IntPtr ptr, IntPtr eventsPtr, ref RelationshipManager.FFIEvents events)
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

		// Token: 0x06000236 RID: 566 RVA: 0x0000B39F File Offset: 0x0000959F
		private void InitEvents(IntPtr eventsPtr, ref RelationshipManager.FFIEvents events)
		{
			events.OnRefresh = new RelationshipManager.FFIEvents.RefreshHandler(RelationshipManager.OnRefreshImpl);
			events.OnRelationshipUpdate = new RelationshipManager.FFIEvents.RelationshipUpdateHandler(RelationshipManager.OnRelationshipUpdateImpl);
			Marshal.StructureToPtr<RelationshipManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000B3D4 File Offset: 0x000095D4
		[MonoPInvokeCallback]
		private static bool FilterCallbackImpl(IntPtr ptr, ref Relationship relationship)
		{
			return ((RelationshipManager.FilterHandler)GCHandle.FromIntPtr(ptr).Target)(ref relationship);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B3FC File Offset: 0x000095FC
		public void Filter(RelationshipManager.FilterHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.Filter(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new RelationshipManager.FFIMethods.FilterCallback(RelationshipManager.FilterCallbackImpl));
			gchandle.Free();
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B440 File Offset: 0x00009640
		public int Count()
		{
			int num = 0;
			Result result = this.Methods.Count(this.MethodsPtr, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B474 File Offset: 0x00009674
		public Relationship Get(long userId)
		{
			Relationship relationship = default(Relationship);
			Result result = this.Methods.Get(this.MethodsPtr, userId, ref relationship);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return relationship;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000B4B0 File Offset: 0x000096B0
		public Relationship GetAt(uint index)
		{
			Relationship relationship = default(Relationship);
			Result result = this.Methods.GetAt(this.MethodsPtr, index, ref relationship);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return relationship;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000B4EC File Offset: 0x000096EC
		[MonoPInvokeCallback]
		private static void OnRefreshImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.RelationshipManagerInstance.OnRefresh != null)
			{
				discord.RelationshipManagerInstance.OnRefresh();
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000B52C File Offset: 0x0000972C
		[MonoPInvokeCallback]
		private static void OnRelationshipUpdateImpl(IntPtr ptr, ref Relationship relationship)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.RelationshipManagerInstance.OnRelationshipUpdate != null)
			{
				discord.RelationshipManagerInstance.OnRelationshipUpdate(ref relationship);
			}
		}

		// Token: 0x04000185 RID: 389
		private IntPtr MethodsPtr;

		// Token: 0x04000186 RID: 390
		private object MethodsStructure;

		// Token: 0x02000091 RID: 145
		internal struct FFIEvents
		{
			// Token: 0x04000261 RID: 609
			internal RelationshipManager.FFIEvents.RefreshHandler OnRefresh;

			// Token: 0x04000262 RID: 610
			internal RelationshipManager.FFIEvents.RelationshipUpdateHandler OnRelationshipUpdate;

			// Token: 0x0200010A RID: 266
			// (Invoke) Token: 0x06000513 RID: 1299
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RefreshHandler(IntPtr ptr);

			// Token: 0x0200010B RID: 267
			// (Invoke) Token: 0x06000517 RID: 1303
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RelationshipUpdateHandler(IntPtr ptr, ref Relationship relationship);
		}

		// Token: 0x02000092 RID: 146
		internal struct FFIMethods
		{
			// Token: 0x04000263 RID: 611
			internal RelationshipManager.FFIMethods.FilterMethod Filter;

			// Token: 0x04000264 RID: 612
			internal RelationshipManager.FFIMethods.CountMethod Count;

			// Token: 0x04000265 RID: 613
			internal RelationshipManager.FFIMethods.GetMethod Get;

			// Token: 0x04000266 RID: 614
			internal RelationshipManager.FFIMethods.GetAtMethod GetAt;

			// Token: 0x0200010C RID: 268
			// (Invoke) Token: 0x0600051B RID: 1307
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate bool FilterCallback(IntPtr ptr, ref Relationship relationship);

			// Token: 0x0200010D RID: 269
			// (Invoke) Token: 0x0600051F RID: 1311
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FilterMethod(IntPtr methodsPtr, IntPtr callbackData, RelationshipManager.FFIMethods.FilterCallback callback);

			// Token: 0x0200010E RID: 270
			// (Invoke) Token: 0x06000523 RID: 1315
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x0200010F RID: 271
			// (Invoke) Token: 0x06000527 RID: 1319
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMethod(IntPtr methodsPtr, long userId, ref Relationship relationship);

			// Token: 0x02000110 RID: 272
			// (Invoke) Token: 0x0600052B RID: 1323
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetAtMethod(IntPtr methodsPtr, uint index, ref Relationship relationship);
		}

		// Token: 0x02000093 RID: 147
		// (Invoke) Token: 0x06000373 RID: 883
		public delegate bool FilterHandler(ref Relationship relationship);

		// Token: 0x02000094 RID: 148
		// (Invoke) Token: 0x06000377 RID: 887
		public delegate void RefreshHandler();

		// Token: 0x02000095 RID: 149
		// (Invoke) Token: 0x0600037B RID: 891
		public delegate void RelationshipUpdateHandler(ref Relationship relationship);
	}
}
