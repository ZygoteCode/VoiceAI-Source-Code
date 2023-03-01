using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000019 RID: 25
	public class ActivityManager
	{
		// Token: 0x060001CE RID: 462 RVA: 0x00009983 File Offset: 0x00007B83
		public void RegisterCommand()
		{
			this.RegisterCommand(null);
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000998C File Offset: 0x00007B8C
		private ActivityManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ActivityManager.FFIMethods));
				}
				return (ActivityManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001D0 RID: 464 RVA: 0x000099BC File Offset: 0x00007BBC
		// (remove) Token: 0x060001D1 RID: 465 RVA: 0x000099F4 File Offset: 0x00007BF4
		public event ActivityManager.ActivityJoinHandler OnActivityJoin;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060001D2 RID: 466 RVA: 0x00009A2C File Offset: 0x00007C2C
		// (remove) Token: 0x060001D3 RID: 467 RVA: 0x00009A64 File Offset: 0x00007C64
		public event ActivityManager.ActivitySpectateHandler OnActivitySpectate;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060001D4 RID: 468 RVA: 0x00009A9C File Offset: 0x00007C9C
		// (remove) Token: 0x060001D5 RID: 469 RVA: 0x00009AD4 File Offset: 0x00007CD4
		public event ActivityManager.ActivityJoinRequestHandler OnActivityJoinRequest;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001D6 RID: 470 RVA: 0x00009B0C File Offset: 0x00007D0C
		// (remove) Token: 0x060001D7 RID: 471 RVA: 0x00009B44 File Offset: 0x00007D44
		public event ActivityManager.ActivityInviteHandler OnActivityInvite;

		// Token: 0x060001D8 RID: 472 RVA: 0x00009B7C File Offset: 0x00007D7C
		internal ActivityManager(IntPtr ptr, IntPtr eventsPtr, ref ActivityManager.FFIEvents events)
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

		// Token: 0x060001D9 RID: 473 RVA: 0x00009BCC File Offset: 0x00007DCC
		private void InitEvents(IntPtr eventsPtr, ref ActivityManager.FFIEvents events)
		{
			events.OnActivityJoin = new ActivityManager.FFIEvents.ActivityJoinHandler(ActivityManager.OnActivityJoinImpl);
			events.OnActivitySpectate = new ActivityManager.FFIEvents.ActivitySpectateHandler(ActivityManager.OnActivitySpectateImpl);
			events.OnActivityJoinRequest = new ActivityManager.FFIEvents.ActivityJoinRequestHandler(ActivityManager.OnActivityJoinRequestImpl);
			events.OnActivityInvite = new ActivityManager.FFIEvents.ActivityInviteHandler(ActivityManager.OnActivityInviteImpl);
			Marshal.StructureToPtr<ActivityManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009C30 File Offset: 0x00007E30
		public void RegisterCommand(string command)
		{
			Result result = this.Methods.RegisterCommand(this.MethodsPtr, command);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009C60 File Offset: 0x00007E60
		public void RegisterSteam(uint steamId)
		{
			Result result = this.Methods.RegisterSteam(this.MethodsPtr, steamId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009C90 File Offset: 0x00007E90
		[MonoPInvokeCallback]
		private static void UpdateActivityCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.UpdateActivityHandler updateActivityHandler = (ActivityManager.UpdateActivityHandler)gchandle.Target;
			gchandle.Free();
			updateActivityHandler(result);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009CC0 File Offset: 0x00007EC0
		public void UpdateActivity(Activity activity, ActivityManager.UpdateActivityHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.UpdateActivity(this.MethodsPtr, ref activity, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.UpdateActivityCallback(ActivityManager.UpdateActivityCallbackImpl));
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009D00 File Offset: 0x00007F00
		[MonoPInvokeCallback]
		private static void ClearActivityCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.ClearActivityHandler clearActivityHandler = (ActivityManager.ClearActivityHandler)gchandle.Target;
			gchandle.Free();
			clearActivityHandler(result);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009D30 File Offset: 0x00007F30
		public void ClearActivity(ActivityManager.ClearActivityHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ClearActivity(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.ClearActivityCallback(ActivityManager.ClearActivityCallbackImpl));
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009D6C File Offset: 0x00007F6C
		[MonoPInvokeCallback]
		private static void SendRequestReplyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.SendRequestReplyHandler sendRequestReplyHandler = (ActivityManager.SendRequestReplyHandler)gchandle.Target;
			gchandle.Free();
			sendRequestReplyHandler(result);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009D9C File Offset: 0x00007F9C
		public void SendRequestReply(long userId, ActivityJoinRequestReply reply, ActivityManager.SendRequestReplyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SendRequestReply(this.MethodsPtr, userId, reply, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.SendRequestReplyCallback(ActivityManager.SendRequestReplyCallbackImpl));
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009DDC File Offset: 0x00007FDC
		[MonoPInvokeCallback]
		private static void SendInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.SendInviteHandler sendInviteHandler = (ActivityManager.SendInviteHandler)gchandle.Target;
			gchandle.Free();
			sendInviteHandler(result);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009E0C File Offset: 0x0000800C
		public void SendInvite(long userId, ActivityActionType type, string content, ActivityManager.SendInviteHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SendInvite(this.MethodsPtr, userId, type, content, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.SendInviteCallback(ActivityManager.SendInviteCallbackImpl));
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009E4C File Offset: 0x0000804C
		[MonoPInvokeCallback]
		private static void AcceptInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.AcceptInviteHandler acceptInviteHandler = (ActivityManager.AcceptInviteHandler)gchandle.Target;
			gchandle.Free();
			acceptInviteHandler(result);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009E7C File Offset: 0x0000807C
		public void AcceptInvite(long userId, ActivityManager.AcceptInviteHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.AcceptInvite(this.MethodsPtr, userId, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.AcceptInviteCallback(ActivityManager.AcceptInviteCallbackImpl));
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009EBC File Offset: 0x000080BC
		[MonoPInvokeCallback]
		private static void OnActivityJoinImpl(IntPtr ptr, string secret)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityJoin != null)
			{
				discord.ActivityManagerInstance.OnActivityJoin(secret);
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009EFC File Offset: 0x000080FC
		[MonoPInvokeCallback]
		private static void OnActivitySpectateImpl(IntPtr ptr, string secret)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivitySpectate != null)
			{
				discord.ActivityManagerInstance.OnActivitySpectate(secret);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009F3C File Offset: 0x0000813C
		[MonoPInvokeCallback]
		private static void OnActivityJoinRequestImpl(IntPtr ptr, ref User user)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityJoinRequest != null)
			{
				discord.ActivityManagerInstance.OnActivityJoinRequest(ref user);
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009F7C File Offset: 0x0000817C
		[MonoPInvokeCallback]
		private static void OnActivityInviteImpl(IntPtr ptr, ActivityActionType type, ref User user, ref Activity activity)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityInvite != null)
			{
				discord.ActivityManagerInstance.OnActivityInvite(type, ref user, ref activity);
			}
		}

		// Token: 0x0400008A RID: 138
		private IntPtr MethodsPtr;

		// Token: 0x0400008B RID: 139
		private object MethodsStructure;

		// Token: 0x02000073 RID: 115
		internal struct FFIEvents
		{
			// Token: 0x04000212 RID: 530
			internal ActivityManager.FFIEvents.ActivityJoinHandler OnActivityJoin;

			// Token: 0x04000213 RID: 531
			internal ActivityManager.FFIEvents.ActivitySpectateHandler OnActivitySpectate;

			// Token: 0x04000214 RID: 532
			internal ActivityManager.FFIEvents.ActivityJoinRequestHandler OnActivityJoinRequest;

			// Token: 0x04000215 RID: 533
			internal ActivityManager.FFIEvents.ActivityInviteHandler OnActivityInvite;

			// Token: 0x020000CC RID: 204
			// (Invoke) Token: 0x0600041B RID: 1051
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityJoinHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

			// Token: 0x020000CD RID: 205
			// (Invoke) Token: 0x0600041F RID: 1055
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivitySpectateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

			// Token: 0x020000CE RID: 206
			// (Invoke) Token: 0x06000423 RID: 1059
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityJoinRequestHandler(IntPtr ptr, ref User user);

			// Token: 0x020000CF RID: 207
			// (Invoke) Token: 0x06000427 RID: 1063
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityInviteHandler(IntPtr ptr, ActivityActionType type, ref User user, ref Activity activity);
		}

		// Token: 0x02000074 RID: 116
		internal struct FFIMethods
		{
			// Token: 0x04000216 RID: 534
			internal ActivityManager.FFIMethods.RegisterCommandMethod RegisterCommand;

			// Token: 0x04000217 RID: 535
			internal ActivityManager.FFIMethods.RegisterSteamMethod RegisterSteam;

			// Token: 0x04000218 RID: 536
			internal ActivityManager.FFIMethods.UpdateActivityMethod UpdateActivity;

			// Token: 0x04000219 RID: 537
			internal ActivityManager.FFIMethods.ClearActivityMethod ClearActivity;

			// Token: 0x0400021A RID: 538
			internal ActivityManager.FFIMethods.SendRequestReplyMethod SendRequestReply;

			// Token: 0x0400021B RID: 539
			internal ActivityManager.FFIMethods.SendInviteMethod SendInvite;

			// Token: 0x0400021C RID: 540
			internal ActivityManager.FFIMethods.AcceptInviteMethod AcceptInvite;

			// Token: 0x020000D0 RID: 208
			// (Invoke) Token: 0x0600042B RID: 1067
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RegisterCommandMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string command);

			// Token: 0x020000D1 RID: 209
			// (Invoke) Token: 0x0600042F RID: 1071
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RegisterSteamMethod(IntPtr methodsPtr, uint steamId);

			// Token: 0x020000D2 RID: 210
			// (Invoke) Token: 0x06000433 RID: 1075
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateActivityCallback(IntPtr ptr, Result result);

			// Token: 0x020000D3 RID: 211
			// (Invoke) Token: 0x06000437 RID: 1079
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateActivityMethod(IntPtr methodsPtr, ref Activity activity, IntPtr callbackData, ActivityManager.FFIMethods.UpdateActivityCallback callback);

			// Token: 0x020000D4 RID: 212
			// (Invoke) Token: 0x0600043B RID: 1083
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ClearActivityCallback(IntPtr ptr, Result result);

			// Token: 0x020000D5 RID: 213
			// (Invoke) Token: 0x0600043F RID: 1087
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ClearActivityMethod(IntPtr methodsPtr, IntPtr callbackData, ActivityManager.FFIMethods.ClearActivityCallback callback);

			// Token: 0x020000D6 RID: 214
			// (Invoke) Token: 0x06000443 RID: 1091
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendRequestReplyCallback(IntPtr ptr, Result result);

			// Token: 0x020000D7 RID: 215
			// (Invoke) Token: 0x06000447 RID: 1095
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendRequestReplyMethod(IntPtr methodsPtr, long userId, ActivityJoinRequestReply reply, IntPtr callbackData, ActivityManager.FFIMethods.SendRequestReplyCallback callback);

			// Token: 0x020000D8 RID: 216
			// (Invoke) Token: 0x0600044B RID: 1099
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendInviteCallback(IntPtr ptr, Result result);

			// Token: 0x020000D9 RID: 217
			// (Invoke) Token: 0x0600044F RID: 1103
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendInviteMethod(IntPtr methodsPtr, long userId, ActivityActionType type, [MarshalAs(UnmanagedType.LPStr)] string content, IntPtr callbackData, ActivityManager.FFIMethods.SendInviteCallback callback);

			// Token: 0x020000DA RID: 218
			// (Invoke) Token: 0x06000453 RID: 1107
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void AcceptInviteCallback(IntPtr ptr, Result result);

			// Token: 0x020000DB RID: 219
			// (Invoke) Token: 0x06000457 RID: 1111
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void AcceptInviteMethod(IntPtr methodsPtr, long userId, IntPtr callbackData, ActivityManager.FFIMethods.AcceptInviteCallback callback);
		}

		// Token: 0x02000075 RID: 117
		// (Invoke) Token: 0x06000333 RID: 819
		public delegate void UpdateActivityHandler(Result result);

		// Token: 0x02000076 RID: 118
		// (Invoke) Token: 0x06000337 RID: 823
		public delegate void ClearActivityHandler(Result result);

		// Token: 0x02000077 RID: 119
		// (Invoke) Token: 0x0600033B RID: 827
		public delegate void SendRequestReplyHandler(Result result);

		// Token: 0x02000078 RID: 120
		// (Invoke) Token: 0x0600033F RID: 831
		public delegate void SendInviteHandler(Result result);

		// Token: 0x02000079 RID: 121
		// (Invoke) Token: 0x06000343 RID: 835
		public delegate void AcceptInviteHandler(Result result);

		// Token: 0x0200007A RID: 122
		// (Invoke) Token: 0x06000347 RID: 839
		public delegate void ActivityJoinHandler(string secret);

		// Token: 0x0200007B RID: 123
		// (Invoke) Token: 0x0600034B RID: 843
		public delegate void ActivitySpectateHandler(string secret);

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x0600034F RID: 847
		public delegate void ActivityJoinRequestHandler(ref User user);

		// Token: 0x0200007D RID: 125
		// (Invoke) Token: 0x06000353 RID: 851
		public delegate void ActivityInviteHandler(ActivityActionType type, ref User user, ref Activity activity);
	}
}
