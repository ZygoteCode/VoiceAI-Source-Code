using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200004C RID: 76
	public class OverlayManager
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000CA5F File Offset: 0x0000AC5F
		private OverlayManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(OverlayManager.FFIMethods));
				}
				return (OverlayManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000299 RID: 665 RVA: 0x0000CA90 File Offset: 0x0000AC90
		// (remove) Token: 0x0600029A RID: 666 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
		public event OverlayManager.ToggleHandler OnToggle;

		// Token: 0x0600029B RID: 667 RVA: 0x0000CB00 File Offset: 0x0000AD00
		internal OverlayManager(IntPtr ptr, IntPtr eventsPtr, ref OverlayManager.FFIEvents events)
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

		// Token: 0x0600029C RID: 668 RVA: 0x0000CB4F File Offset: 0x0000AD4F
		private void InitEvents(IntPtr eventsPtr, ref OverlayManager.FFIEvents events)
		{
			events.OnToggle = new OverlayManager.FFIEvents.ToggleHandler(OverlayManager.OnToggleImpl);
			Marshal.StructureToPtr<OverlayManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000CB70 File Offset: 0x0000AD70
		public bool IsEnabled()
		{
			bool flag = false;
			this.Methods.IsEnabled(this.MethodsPtr, ref flag);
			return flag;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000CB98 File Offset: 0x0000AD98
		public bool IsLocked()
		{
			bool flag = false;
			this.Methods.IsLocked(this.MethodsPtr, ref flag);
			return flag;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000CBC0 File Offset: 0x0000ADC0
		[MonoPInvokeCallback]
		private static void SetLockedCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.SetLockedHandler setLockedHandler = (OverlayManager.SetLockedHandler)gchandle.Target;
			gchandle.Free();
			setLockedHandler(result);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000CBF0 File Offset: 0x0000ADF0
		public void SetLocked(bool locked, OverlayManager.SetLockedHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SetLocked(this.MethodsPtr, locked, GCHandle.ToIntPtr(gchandle), new OverlayManager.FFIMethods.SetLockedCallback(OverlayManager.SetLockedCallbackImpl));
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000CC30 File Offset: 0x0000AE30
		[MonoPInvokeCallback]
		private static void OpenActivityInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenActivityInviteHandler openActivityInviteHandler = (OverlayManager.OpenActivityInviteHandler)gchandle.Target;
			gchandle.Free();
			openActivityInviteHandler(result);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000CC60 File Offset: 0x0000AE60
		public void OpenActivityInvite(ActivityActionType type, OverlayManager.OpenActivityInviteHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.OpenActivityInvite(this.MethodsPtr, type, GCHandle.ToIntPtr(gchandle), new OverlayManager.FFIMethods.OpenActivityInviteCallback(OverlayManager.OpenActivityInviteCallbackImpl));
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000CCA0 File Offset: 0x0000AEA0
		[MonoPInvokeCallback]
		private static void OpenGuildInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenGuildInviteHandler openGuildInviteHandler = (OverlayManager.OpenGuildInviteHandler)gchandle.Target;
			gchandle.Free();
			openGuildInviteHandler(result);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000CCD0 File Offset: 0x0000AED0
		public void OpenGuildInvite(string code, OverlayManager.OpenGuildInviteHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.OpenGuildInvite(this.MethodsPtr, code, GCHandle.ToIntPtr(gchandle), new OverlayManager.FFIMethods.OpenGuildInviteCallback(OverlayManager.OpenGuildInviteCallbackImpl));
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000CD10 File Offset: 0x0000AF10
		[MonoPInvokeCallback]
		private static void OpenVoiceSettingsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenVoiceSettingsHandler openVoiceSettingsHandler = (OverlayManager.OpenVoiceSettingsHandler)gchandle.Target;
			gchandle.Free();
			openVoiceSettingsHandler(result);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000CD40 File Offset: 0x0000AF40
		public void OpenVoiceSettings(OverlayManager.OpenVoiceSettingsHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.OpenVoiceSettings(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new OverlayManager.FFIMethods.OpenVoiceSettingsCallback(OverlayManager.OpenVoiceSettingsCallbackImpl));
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000CD7C File Offset: 0x0000AF7C
		[MonoPInvokeCallback]
		private static void OnToggleImpl(IntPtr ptr, bool locked)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.OverlayManagerInstance.OnToggle != null)
			{
				discord.OverlayManagerInstance.OnToggle(locked);
			}
		}

		// Token: 0x04000197 RID: 407
		private IntPtr MethodsPtr;

		// Token: 0x04000198 RID: 408
		private object MethodsStructure;

		// Token: 0x020000AF RID: 175
		internal struct FFIEvents
		{
			// Token: 0x0400029A RID: 666
			internal OverlayManager.FFIEvents.ToggleHandler OnToggle;

			// Token: 0x0200014F RID: 335
			// (Invoke) Token: 0x06000627 RID: 1575
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ToggleHandler(IntPtr ptr, bool locked);
		}

		// Token: 0x020000B0 RID: 176
		internal struct FFIMethods
		{
			// Token: 0x0400029B RID: 667
			internal OverlayManager.FFIMethods.IsEnabledMethod IsEnabled;

			// Token: 0x0400029C RID: 668
			internal OverlayManager.FFIMethods.IsLockedMethod IsLocked;

			// Token: 0x0400029D RID: 669
			internal OverlayManager.FFIMethods.SetLockedMethod SetLocked;

			// Token: 0x0400029E RID: 670
			internal OverlayManager.FFIMethods.OpenActivityInviteMethod OpenActivityInvite;

			// Token: 0x0400029F RID: 671
			internal OverlayManager.FFIMethods.OpenGuildInviteMethod OpenGuildInvite;

			// Token: 0x040002A0 RID: 672
			internal OverlayManager.FFIMethods.OpenVoiceSettingsMethod OpenVoiceSettings;

			// Token: 0x02000150 RID: 336
			// (Invoke) Token: 0x0600062B RID: 1579
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void IsEnabledMethod(IntPtr methodsPtr, ref bool enabled);

			// Token: 0x02000151 RID: 337
			// (Invoke) Token: 0x0600062F RID: 1583
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void IsLockedMethod(IntPtr methodsPtr, ref bool locked);

			// Token: 0x02000152 RID: 338
			// (Invoke) Token: 0x06000633 RID: 1587
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLockedCallback(IntPtr ptr, Result result);

			// Token: 0x02000153 RID: 339
			// (Invoke) Token: 0x06000637 RID: 1591
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLockedMethod(IntPtr methodsPtr, bool locked, IntPtr callbackData, OverlayManager.FFIMethods.SetLockedCallback callback);

			// Token: 0x02000154 RID: 340
			// (Invoke) Token: 0x0600063B RID: 1595
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenActivityInviteCallback(IntPtr ptr, Result result);

			// Token: 0x02000155 RID: 341
			// (Invoke) Token: 0x0600063F RID: 1599
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenActivityInviteMethod(IntPtr methodsPtr, ActivityActionType type, IntPtr callbackData, OverlayManager.FFIMethods.OpenActivityInviteCallback callback);

			// Token: 0x02000156 RID: 342
			// (Invoke) Token: 0x06000643 RID: 1603
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenGuildInviteCallback(IntPtr ptr, Result result);

			// Token: 0x02000157 RID: 343
			// (Invoke) Token: 0x06000647 RID: 1607
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenGuildInviteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string code, IntPtr callbackData, OverlayManager.FFIMethods.OpenGuildInviteCallback callback);

			// Token: 0x02000158 RID: 344
			// (Invoke) Token: 0x0600064B RID: 1611
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenVoiceSettingsCallback(IntPtr ptr, Result result);

			// Token: 0x02000159 RID: 345
			// (Invoke) Token: 0x0600064F RID: 1615
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenVoiceSettingsMethod(IntPtr methodsPtr, IntPtr callbackData, OverlayManager.FFIMethods.OpenVoiceSettingsCallback callback);
		}

		// Token: 0x020000B1 RID: 177
		// (Invoke) Token: 0x060003D3 RID: 979
		public delegate void SetLockedHandler(Result result);

		// Token: 0x020000B2 RID: 178
		// (Invoke) Token: 0x060003D7 RID: 983
		public delegate void OpenActivityInviteHandler(Result result);

		// Token: 0x020000B3 RID: 179
		// (Invoke) Token: 0x060003DB RID: 987
		public delegate void OpenGuildInviteHandler(Result result);

		// Token: 0x020000B4 RID: 180
		// (Invoke) Token: 0x060003DF RID: 991
		public delegate void OpenVoiceSettingsHandler(Result result);

		// Token: 0x020000B5 RID: 181
		// (Invoke) Token: 0x060003E3 RID: 995
		public delegate void ToggleHandler(bool locked);
	}
}
