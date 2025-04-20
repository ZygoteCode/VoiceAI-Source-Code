using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200004F RID: 79
	public class VoiceManager
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000D6DC File Offset: 0x0000B8DC
		private VoiceManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(VoiceManager.FFIMethods));
				}
				return (VoiceManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060002D3 RID: 723 RVA: 0x0000D70C File Offset: 0x0000B90C
		// (remove) Token: 0x060002D4 RID: 724 RVA: 0x0000D744 File Offset: 0x0000B944
		public event VoiceManager.SettingsUpdateHandler OnSettingsUpdate;

		// Token: 0x060002D5 RID: 725 RVA: 0x0000D77C File Offset: 0x0000B97C
		internal VoiceManager(IntPtr ptr, IntPtr eventsPtr, ref VoiceManager.FFIEvents events)
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

		// Token: 0x060002D6 RID: 726 RVA: 0x0000D7CB File Offset: 0x0000B9CB
		private void InitEvents(IntPtr eventsPtr, ref VoiceManager.FFIEvents events)
		{
			events.OnSettingsUpdate = new VoiceManager.FFIEvents.SettingsUpdateHandler(VoiceManager.OnSettingsUpdateImpl);
			Marshal.StructureToPtr<VoiceManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D7EC File Offset: 0x0000B9EC
		public InputMode GetInputMode()
		{
			InputMode inputMode = default(InputMode);
			Result result = this.Methods.GetInputMode(this.MethodsPtr, ref inputMode);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return inputMode;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D828 File Offset: 0x0000BA28
		[MonoPInvokeCallback]
		private static void SetInputModeCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			VoiceManager.SetInputModeHandler setInputModeHandler = (VoiceManager.SetInputModeHandler)gchandle.Target;
			gchandle.Free();
			setInputModeHandler(result);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000D858 File Offset: 0x0000BA58
		public void SetInputMode(InputMode inputMode, VoiceManager.SetInputModeHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SetInputMode(this.MethodsPtr, inputMode, GCHandle.ToIntPtr(gchandle), new VoiceManager.FFIMethods.SetInputModeCallback(VoiceManager.SetInputModeCallbackImpl));
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000D898 File Offset: 0x0000BA98
		public bool IsSelfMute()
		{
			bool flag = false;
			Result result = this.Methods.IsSelfMute(this.MethodsPtr, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000D8CC File Offset: 0x0000BACC
		public void SetSelfMute(bool mute)
		{
			Result result = this.Methods.SetSelfMute(this.MethodsPtr, mute);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000D8FC File Offset: 0x0000BAFC
		public bool IsSelfDeaf()
		{
			bool flag = false;
			Result result = this.Methods.IsSelfDeaf(this.MethodsPtr, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000D930 File Offset: 0x0000BB30
		public void SetSelfDeaf(bool deaf)
		{
			Result result = this.Methods.SetSelfDeaf(this.MethodsPtr, deaf);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000D960 File Offset: 0x0000BB60
		public bool IsLocalMute(long userId)
		{
			bool flag = false;
			Result result = this.Methods.IsLocalMute(this.MethodsPtr, userId, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000D994 File Offset: 0x0000BB94
		public void SetLocalMute(long userId, bool mute)
		{
			Result result = this.Methods.SetLocalMute(this.MethodsPtr, userId, mute);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000D9C4 File Offset: 0x0000BBC4
		public byte GetLocalVolume(long userId)
		{
			byte b = 0;
			Result result = this.Methods.GetLocalVolume(this.MethodsPtr, userId, ref b);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return b;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000D9F8 File Offset: 0x0000BBF8
		public void SetLocalVolume(long userId, byte volume)
		{
			Result result = this.Methods.SetLocalVolume(this.MethodsPtr, userId, volume);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000DA28 File Offset: 0x0000BC28
		[MonoPInvokeCallback]
		private static void OnSettingsUpdateImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.VoiceManagerInstance.OnSettingsUpdate != null)
			{
				discord.VoiceManagerInstance.OnSettingsUpdate();
			}
		}

		// Token: 0x040001A0 RID: 416
		private IntPtr MethodsPtr;

		// Token: 0x040001A1 RID: 417
		private object MethodsStructure;

		// Token: 0x020000C2 RID: 194
		internal struct FFIEvents
		{
			// Token: 0x040002B8 RID: 696
			internal VoiceManager.FFIEvents.SettingsUpdateHandler OnSettingsUpdate;

			// Token: 0x02000177 RID: 375
			// (Invoke) Token: 0x060006C7 RID: 1735
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SettingsUpdateHandler(IntPtr ptr);
		}

		// Token: 0x020000C3 RID: 195
		internal struct FFIMethods
		{
			// Token: 0x040002B9 RID: 697
			internal VoiceManager.FFIMethods.GetInputModeMethod GetInputMode;

			// Token: 0x040002BA RID: 698
			internal VoiceManager.FFIMethods.SetInputModeMethod SetInputMode;

			// Token: 0x040002BB RID: 699
			internal VoiceManager.FFIMethods.IsSelfMuteMethod IsSelfMute;

			// Token: 0x040002BC RID: 700
			internal VoiceManager.FFIMethods.SetSelfMuteMethod SetSelfMute;

			// Token: 0x040002BD RID: 701
			internal VoiceManager.FFIMethods.IsSelfDeafMethod IsSelfDeaf;

			// Token: 0x040002BE RID: 702
			internal VoiceManager.FFIMethods.SetSelfDeafMethod SetSelfDeaf;

			// Token: 0x040002BF RID: 703
			internal VoiceManager.FFIMethods.IsLocalMuteMethod IsLocalMute;

			// Token: 0x040002C0 RID: 704
			internal VoiceManager.FFIMethods.SetLocalMuteMethod SetLocalMute;

			// Token: 0x040002C1 RID: 705
			internal VoiceManager.FFIMethods.GetLocalVolumeMethod GetLocalVolume;

			// Token: 0x040002C2 RID: 706
			internal VoiceManager.FFIMethods.SetLocalVolumeMethod SetLocalVolume;

			// Token: 0x02000178 RID: 376
			// (Invoke) Token: 0x060006CB RID: 1739
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetInputModeMethod(IntPtr methodsPtr, ref InputMode inputMode);

			// Token: 0x02000179 RID: 377
			// (Invoke) Token: 0x060006CF RID: 1743
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetInputModeCallback(IntPtr ptr, Result result);

			// Token: 0x0200017A RID: 378
			// (Invoke) Token: 0x060006D3 RID: 1747
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetInputModeMethod(IntPtr methodsPtr, InputMode inputMode, IntPtr callbackData, VoiceManager.FFIMethods.SetInputModeCallback callback);

			// Token: 0x0200017B RID: 379
			// (Invoke) Token: 0x060006D7 RID: 1751
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsSelfMuteMethod(IntPtr methodsPtr, ref bool mute);

			// Token: 0x0200017C RID: 380
			// (Invoke) Token: 0x060006DB RID: 1755
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetSelfMuteMethod(IntPtr methodsPtr, bool mute);

			// Token: 0x0200017D RID: 381
			// (Invoke) Token: 0x060006DF RID: 1759
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsSelfDeafMethod(IntPtr methodsPtr, ref bool deaf);

			// Token: 0x0200017E RID: 382
			// (Invoke) Token: 0x060006E3 RID: 1763
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetSelfDeafMethod(IntPtr methodsPtr, bool deaf);

			// Token: 0x0200017F RID: 383
			// (Invoke) Token: 0x060006E7 RID: 1767
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsLocalMuteMethod(IntPtr methodsPtr, long userId, ref bool mute);

			// Token: 0x02000180 RID: 384
			// (Invoke) Token: 0x060006EB RID: 1771
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLocalMuteMethod(IntPtr methodsPtr, long userId, bool mute);

			// Token: 0x02000181 RID: 385
			// (Invoke) Token: 0x060006EF RID: 1775
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLocalVolumeMethod(IntPtr methodsPtr, long userId, ref byte volume);

			// Token: 0x02000182 RID: 386
			// (Invoke) Token: 0x060006F3 RID: 1779
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLocalVolumeMethod(IntPtr methodsPtr, long userId, byte volume);
		}

		// Token: 0x020000C4 RID: 196
		// (Invoke) Token: 0x06000407 RID: 1031
		public delegate void SetInputModeHandler(Result result);

		// Token: 0x020000C5 RID: 197
		// (Invoke) Token: 0x0600040B RID: 1035
		public delegate void SettingsUpdateHandler();
	}
}
