using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200004E RID: 78
	public class StoreManager
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000D1A4 File Offset: 0x0000B3A4
		private StoreManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(StoreManager.FFIMethods));
				}
				return (StoreManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060002BB RID: 699 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
		// (remove) Token: 0x060002BC RID: 700 RVA: 0x0000D20C File Offset: 0x0000B40C
		public event StoreManager.EntitlementCreateHandler OnEntitlementCreate;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060002BD RID: 701 RVA: 0x0000D244 File Offset: 0x0000B444
		// (remove) Token: 0x060002BE RID: 702 RVA: 0x0000D27C File Offset: 0x0000B47C
		public event StoreManager.EntitlementDeleteHandler OnEntitlementDelete;

		// Token: 0x060002BF RID: 703 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		internal StoreManager(IntPtr ptr, IntPtr eventsPtr, ref StoreManager.FFIEvents events)
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

		// Token: 0x060002C0 RID: 704 RVA: 0x0000D303 File Offset: 0x0000B503
		private void InitEvents(IntPtr eventsPtr, ref StoreManager.FFIEvents events)
		{
			events.OnEntitlementCreate = new StoreManager.FFIEvents.EntitlementCreateHandler(StoreManager.OnEntitlementCreateImpl);
			events.OnEntitlementDelete = new StoreManager.FFIEvents.EntitlementDeleteHandler(StoreManager.OnEntitlementDeleteImpl);
			Marshal.StructureToPtr<StoreManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000D338 File Offset: 0x0000B538
		[MonoPInvokeCallback]
		private static void FetchSkusCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.FetchSkusHandler fetchSkusHandler = (StoreManager.FetchSkusHandler)gchandle.Target;
			gchandle.Free();
			fetchSkusHandler(result);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000D368 File Offset: 0x0000B568
		public void FetchSkus(StoreManager.FetchSkusHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.FetchSkus(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new StoreManager.FFIMethods.FetchSkusCallback(StoreManager.FetchSkusCallbackImpl));
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000D3A4 File Offset: 0x0000B5A4
		public int CountSkus()
		{
			int num = 0;
			this.Methods.CountSkus(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		public Sku GetSku(long skuId)
		{
			Sku sku = default(Sku);
			Result result = this.Methods.GetSku(this.MethodsPtr, skuId, ref sku);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return sku;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000D408 File Offset: 0x0000B608
		public Sku GetSkuAt(int index)
		{
			Sku sku = default(Sku);
			Result result = this.Methods.GetSkuAt(this.MethodsPtr, index, ref sku);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return sku;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000D444 File Offset: 0x0000B644
		[MonoPInvokeCallback]
		private static void FetchEntitlementsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.FetchEntitlementsHandler fetchEntitlementsHandler = (StoreManager.FetchEntitlementsHandler)gchandle.Target;
			gchandle.Free();
			fetchEntitlementsHandler(result);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000D474 File Offset: 0x0000B674
		public void FetchEntitlements(StoreManager.FetchEntitlementsHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.FetchEntitlements(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new StoreManager.FFIMethods.FetchEntitlementsCallback(StoreManager.FetchEntitlementsCallbackImpl));
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
		public int CountEntitlements()
		{
			int num = 0;
			this.Methods.CountEntitlements(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000D4D8 File Offset: 0x0000B6D8
		public Entitlement GetEntitlement(long entitlementId)
		{
			Entitlement entitlement = default(Entitlement);
			Result result = this.Methods.GetEntitlement(this.MethodsPtr, entitlementId, ref entitlement);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return entitlement;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000D514 File Offset: 0x0000B714
		public Entitlement GetEntitlementAt(int index)
		{
			Entitlement entitlement = default(Entitlement);
			Result result = this.Methods.GetEntitlementAt(this.MethodsPtr, index, ref entitlement);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return entitlement;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000D550 File Offset: 0x0000B750
		public bool HasSkuEntitlement(long skuId)
		{
			bool flag = false;
			Result result = this.Methods.HasSkuEntitlement(this.MethodsPtr, skuId, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000D584 File Offset: 0x0000B784
		[MonoPInvokeCallback]
		private static void StartPurchaseCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.StartPurchaseHandler startPurchaseHandler = (StoreManager.StartPurchaseHandler)gchandle.Target;
			gchandle.Free();
			startPurchaseHandler(result);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000D5B4 File Offset: 0x0000B7B4
		public void StartPurchase(long skuId, StoreManager.StartPurchaseHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.StartPurchase(this.MethodsPtr, skuId, GCHandle.ToIntPtr(gchandle), new StoreManager.FFIMethods.StartPurchaseCallback(StoreManager.StartPurchaseCallbackImpl));
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		[MonoPInvokeCallback]
		private static void OnEntitlementCreateImpl(IntPtr ptr, ref Entitlement entitlement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.StoreManagerInstance.OnEntitlementCreate != null)
			{
				discord.StoreManagerInstance.OnEntitlementCreate(ref entitlement);
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000D634 File Offset: 0x0000B834
		[MonoPInvokeCallback]
		private static void OnEntitlementDeleteImpl(IntPtr ptr, ref Entitlement entitlement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.StoreManagerInstance.OnEntitlementDelete != null)
			{
				discord.StoreManagerInstance.OnEntitlementDelete(ref entitlement);
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000D674 File Offset: 0x0000B874
		public IEnumerable<Entitlement> GetEntitlements()
		{
			int num = this.CountEntitlements();
			List<Entitlement> list = new List<Entitlement>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetEntitlementAt(i));
			}
			return list;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		public IEnumerable<Sku> GetSkus()
		{
			int num = this.CountSkus();
			List<Sku> list = new List<Sku>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetSkuAt(i));
			}
			return list;
		}

		// Token: 0x0400019C RID: 412
		private IntPtr MethodsPtr;

		// Token: 0x0400019D RID: 413
		private object MethodsStructure;

		// Token: 0x020000BB RID: 187
		internal struct FFIEvents
		{
			// Token: 0x040002AC RID: 684
			internal StoreManager.FFIEvents.EntitlementCreateHandler OnEntitlementCreate;

			// Token: 0x040002AD RID: 685
			internal StoreManager.FFIEvents.EntitlementDeleteHandler OnEntitlementDelete;

			// Token: 0x02000168 RID: 360
			// (Invoke) Token: 0x0600068B RID: 1675
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void EntitlementCreateHandler(IntPtr ptr, ref Entitlement entitlement);

			// Token: 0x02000169 RID: 361
			// (Invoke) Token: 0x0600068F RID: 1679
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void EntitlementDeleteHandler(IntPtr ptr, ref Entitlement entitlement);
		}

		// Token: 0x020000BC RID: 188
		internal struct FFIMethods
		{
			// Token: 0x040002AE RID: 686
			internal StoreManager.FFIMethods.FetchSkusMethod FetchSkus;

			// Token: 0x040002AF RID: 687
			internal StoreManager.FFIMethods.CountSkusMethod CountSkus;

			// Token: 0x040002B0 RID: 688
			internal StoreManager.FFIMethods.GetSkuMethod GetSku;

			// Token: 0x040002B1 RID: 689
			internal StoreManager.FFIMethods.GetSkuAtMethod GetSkuAt;

			// Token: 0x040002B2 RID: 690
			internal StoreManager.FFIMethods.FetchEntitlementsMethod FetchEntitlements;

			// Token: 0x040002B3 RID: 691
			internal StoreManager.FFIMethods.CountEntitlementsMethod CountEntitlements;

			// Token: 0x040002B4 RID: 692
			internal StoreManager.FFIMethods.GetEntitlementMethod GetEntitlement;

			// Token: 0x040002B5 RID: 693
			internal StoreManager.FFIMethods.GetEntitlementAtMethod GetEntitlementAt;

			// Token: 0x040002B6 RID: 694
			internal StoreManager.FFIMethods.HasSkuEntitlementMethod HasSkuEntitlement;

			// Token: 0x040002B7 RID: 695
			internal StoreManager.FFIMethods.StartPurchaseMethod StartPurchase;

			// Token: 0x0200016A RID: 362
			// (Invoke) Token: 0x06000693 RID: 1683
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchSkusCallback(IntPtr ptr, Result result);

			// Token: 0x0200016B RID: 363
			// (Invoke) Token: 0x06000697 RID: 1687
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchSkusMethod(IntPtr methodsPtr, IntPtr callbackData, StoreManager.FFIMethods.FetchSkusCallback callback);

			// Token: 0x0200016C RID: 364
			// (Invoke) Token: 0x0600069B RID: 1691
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountSkusMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x0200016D RID: 365
			// (Invoke) Token: 0x0600069F RID: 1695
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSkuMethod(IntPtr methodsPtr, long skuId, ref Sku sku);

			// Token: 0x0200016E RID: 366
			// (Invoke) Token: 0x060006A3 RID: 1699
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSkuAtMethod(IntPtr methodsPtr, int index, ref Sku sku);

			// Token: 0x0200016F RID: 367
			// (Invoke) Token: 0x060006A7 RID: 1703
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchEntitlementsCallback(IntPtr ptr, Result result);

			// Token: 0x02000170 RID: 368
			// (Invoke) Token: 0x060006AB RID: 1707
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchEntitlementsMethod(IntPtr methodsPtr, IntPtr callbackData, StoreManager.FFIMethods.FetchEntitlementsCallback callback);

			// Token: 0x02000171 RID: 369
			// (Invoke) Token: 0x060006AF RID: 1711
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountEntitlementsMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x02000172 RID: 370
			// (Invoke) Token: 0x060006B3 RID: 1715
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetEntitlementMethod(IntPtr methodsPtr, long entitlementId, ref Entitlement entitlement);

			// Token: 0x02000173 RID: 371
			// (Invoke) Token: 0x060006B7 RID: 1719
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetEntitlementAtMethod(IntPtr methodsPtr, int index, ref Entitlement entitlement);

			// Token: 0x02000174 RID: 372
			// (Invoke) Token: 0x060006BB RID: 1723
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result HasSkuEntitlementMethod(IntPtr methodsPtr, long skuId, ref bool hasEntitlement);

			// Token: 0x02000175 RID: 373
			// (Invoke) Token: 0x060006BF RID: 1727
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void StartPurchaseCallback(IntPtr ptr, Result result);

			// Token: 0x02000176 RID: 374
			// (Invoke) Token: 0x060006C3 RID: 1731
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void StartPurchaseMethod(IntPtr methodsPtr, long skuId, IntPtr callbackData, StoreManager.FFIMethods.StartPurchaseCallback callback);
		}

		// Token: 0x020000BD RID: 189
		// (Invoke) Token: 0x060003F3 RID: 1011
		public delegate void FetchSkusHandler(Result result);

		// Token: 0x020000BE RID: 190
		// (Invoke) Token: 0x060003F7 RID: 1015
		public delegate void FetchEntitlementsHandler(Result result);

		// Token: 0x020000BF RID: 191
		// (Invoke) Token: 0x060003FB RID: 1019
		public delegate void StartPurchaseHandler(Result result);

		// Token: 0x020000C0 RID: 192
		// (Invoke) Token: 0x060003FF RID: 1023
		public delegate void EntitlementCreateHandler(ref Entitlement entitlement);

		// Token: 0x020000C1 RID: 193
		// (Invoke) Token: 0x06000403 RID: 1027
		public delegate void EntitlementDeleteHandler(ref Entitlement entitlement);
	}
}
