using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200004B RID: 75
	public class NetworkManager
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000C6B2 File Offset: 0x0000A8B2
		private NetworkManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(NetworkManager.FFIMethods));
				}
				return (NetworkManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000288 RID: 648 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		// (remove) Token: 0x06000289 RID: 649 RVA: 0x0000C71C File Offset: 0x0000A91C
		public event NetworkManager.MessageHandler OnMessage;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600028A RID: 650 RVA: 0x0000C754 File Offset: 0x0000A954
		// (remove) Token: 0x0600028B RID: 651 RVA: 0x0000C78C File Offset: 0x0000A98C
		public event NetworkManager.RouteUpdateHandler OnRouteUpdate;

		// Token: 0x0600028C RID: 652 RVA: 0x0000C7C4 File Offset: 0x0000A9C4
		internal NetworkManager(IntPtr ptr, IntPtr eventsPtr, ref NetworkManager.FFIEvents events)
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

		// Token: 0x0600028D RID: 653 RVA: 0x0000C813 File Offset: 0x0000AA13
		private void InitEvents(IntPtr eventsPtr, ref NetworkManager.FFIEvents events)
		{
			events.OnMessage = new NetworkManager.FFIEvents.MessageHandler(NetworkManager.OnMessageImpl);
			events.OnRouteUpdate = new NetworkManager.FFIEvents.RouteUpdateHandler(NetworkManager.OnRouteUpdateImpl);
			Marshal.StructureToPtr<NetworkManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000C848 File Offset: 0x0000AA48
		public ulong GetPeerId()
		{
			ulong num = 0UL;
			this.Methods.GetPeerId(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000C874 File Offset: 0x0000AA74
		public void Flush()
		{
			Result result = this.Methods.Flush(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		public void OpenPeer(ulong peerId, string routeData)
		{
			Result result = this.Methods.OpenPeer(this.MethodsPtr, peerId, routeData);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
		public void UpdatePeer(ulong peerId, string routeData)
		{
			Result result = this.Methods.UpdatePeer(this.MethodsPtr, peerId, routeData);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000C904 File Offset: 0x0000AB04
		public void ClosePeer(ulong peerId)
		{
			Result result = this.Methods.ClosePeer(this.MethodsPtr, peerId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000C934 File Offset: 0x0000AB34
		public void OpenChannel(ulong peerId, byte channelId, bool reliable)
		{
			Result result = this.Methods.OpenChannel(this.MethodsPtr, peerId, channelId, reliable);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000C968 File Offset: 0x0000AB68
		public void CloseChannel(ulong peerId, byte channelId)
		{
			Result result = this.Methods.CloseChannel(this.MethodsPtr, peerId, channelId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000C998 File Offset: 0x0000AB98
		public void SendMessage(ulong peerId, byte channelId, byte[] data)
		{
			Result result = this.Methods.SendMessage(this.MethodsPtr, peerId, channelId, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		[MonoPInvokeCallback]
		private static void OnMessageImpl(IntPtr ptr, ulong peerId, byte channelId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.NetworkManagerInstance.OnMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.NetworkManagerInstance.OnMessage(peerId, channelId, array);
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000CA20 File Offset: 0x0000AC20
		[MonoPInvokeCallback]
		private static void OnRouteUpdateImpl(IntPtr ptr, string routeData)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.NetworkManagerInstance.OnRouteUpdate != null)
			{
				discord.NetworkManagerInstance.OnRouteUpdate(routeData);
			}
		}

		// Token: 0x04000193 RID: 403
		private IntPtr MethodsPtr;

		// Token: 0x04000194 RID: 404
		private object MethodsStructure;

		// Token: 0x020000AB RID: 171
		internal struct FFIEvents
		{
			// Token: 0x04000290 RID: 656
			internal NetworkManager.FFIEvents.MessageHandler OnMessage;

			// Token: 0x04000291 RID: 657
			internal NetworkManager.FFIEvents.RouteUpdateHandler OnRouteUpdate;

			// Token: 0x02000145 RID: 325
			// (Invoke) Token: 0x060005FF RID: 1535
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MessageHandler(IntPtr ptr, ulong peerId, byte channelId, IntPtr dataPtr, int dataLen);

			// Token: 0x02000146 RID: 326
			// (Invoke) Token: 0x06000603 RID: 1539
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RouteUpdateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string routeData);
		}

		// Token: 0x020000AC RID: 172
		internal struct FFIMethods
		{
			// Token: 0x04000292 RID: 658
			internal NetworkManager.FFIMethods.GetPeerIdMethod GetPeerId;

			// Token: 0x04000293 RID: 659
			internal NetworkManager.FFIMethods.FlushMethod Flush;

			// Token: 0x04000294 RID: 660
			internal NetworkManager.FFIMethods.OpenPeerMethod OpenPeer;

			// Token: 0x04000295 RID: 661
			internal NetworkManager.FFIMethods.UpdatePeerMethod UpdatePeer;

			// Token: 0x04000296 RID: 662
			internal NetworkManager.FFIMethods.ClosePeerMethod ClosePeer;

			// Token: 0x04000297 RID: 663
			internal NetworkManager.FFIMethods.OpenChannelMethod OpenChannel;

			// Token: 0x04000298 RID: 664
			internal NetworkManager.FFIMethods.CloseChannelMethod CloseChannel;

			// Token: 0x04000299 RID: 665
			internal NetworkManager.FFIMethods.SendMessageMethod SendMessage;

			// Token: 0x02000147 RID: 327
			// (Invoke) Token: 0x06000607 RID: 1543
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetPeerIdMethod(IntPtr methodsPtr, ref ulong peerId);

			// Token: 0x02000148 RID: 328
			// (Invoke) Token: 0x0600060B RID: 1547
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FlushMethod(IntPtr methodsPtr);

			// Token: 0x02000149 RID: 329
			// (Invoke) Token: 0x0600060F RID: 1551
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenPeerMethod(IntPtr methodsPtr, ulong peerId, [MarshalAs(UnmanagedType.LPStr)] string routeData);

			// Token: 0x0200014A RID: 330
			// (Invoke) Token: 0x06000613 RID: 1555
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result UpdatePeerMethod(IntPtr methodsPtr, ulong peerId, [MarshalAs(UnmanagedType.LPStr)] string routeData);

			// Token: 0x0200014B RID: 331
			// (Invoke) Token: 0x06000617 RID: 1559
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ClosePeerMethod(IntPtr methodsPtr, ulong peerId);

			// Token: 0x0200014C RID: 332
			// (Invoke) Token: 0x0600061B RID: 1563
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenChannelMethod(IntPtr methodsPtr, ulong peerId, byte channelId, bool reliable);

			// Token: 0x0200014D RID: 333
			// (Invoke) Token: 0x0600061F RID: 1567
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CloseChannelMethod(IntPtr methodsPtr, ulong peerId, byte channelId);

			// Token: 0x0200014E RID: 334
			// (Invoke) Token: 0x06000623 RID: 1571
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SendMessageMethod(IntPtr methodsPtr, ulong peerId, byte channelId, byte[] data, int dataLen);
		}

		// Token: 0x020000AD RID: 173
		// (Invoke) Token: 0x060003CB RID: 971
		public delegate void MessageHandler(ulong peerId, byte channelId, byte[] data);

		// Token: 0x020000AE RID: 174
		// (Invoke) Token: 0x060003CF RID: 975
		public delegate void RouteUpdateHandler(string routeData);
	}
}
