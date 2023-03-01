using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000050 RID: 80
	public class AchievementManager
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000DA66 File Offset: 0x0000BC66
		private AchievementManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(AchievementManager.FFIMethods));
				}
				return (AchievementManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060002E4 RID: 740 RVA: 0x0000DA98 File Offset: 0x0000BC98
		// (remove) Token: 0x060002E5 RID: 741 RVA: 0x0000DAD0 File Offset: 0x0000BCD0
		public event AchievementManager.UserAchievementUpdateHandler OnUserAchievementUpdate;

		// Token: 0x060002E6 RID: 742 RVA: 0x0000DB08 File Offset: 0x0000BD08
		internal AchievementManager(IntPtr ptr, IntPtr eventsPtr, ref AchievementManager.FFIEvents events)
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

		// Token: 0x060002E7 RID: 743 RVA: 0x0000DB57 File Offset: 0x0000BD57
		private void InitEvents(IntPtr eventsPtr, ref AchievementManager.FFIEvents events)
		{
			events.OnUserAchievementUpdate = new AchievementManager.FFIEvents.UserAchievementUpdateHandler(AchievementManager.OnUserAchievementUpdateImpl);
			Marshal.StructureToPtr<AchievementManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000DB78 File Offset: 0x0000BD78
		[MonoPInvokeCallback]
		private static void SetUserAchievementCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			AchievementManager.SetUserAchievementHandler setUserAchievementHandler = (AchievementManager.SetUserAchievementHandler)gchandle.Target;
			gchandle.Free();
			setUserAchievementHandler(result);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000DBA8 File Offset: 0x0000BDA8
		public void SetUserAchievement(long achievementId, byte percentComplete, AchievementManager.SetUserAchievementHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SetUserAchievement(this.MethodsPtr, achievementId, percentComplete, GCHandle.ToIntPtr(gchandle), new AchievementManager.FFIMethods.SetUserAchievementCallback(AchievementManager.SetUserAchievementCallbackImpl));
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		[MonoPInvokeCallback]
		private static void FetchUserAchievementsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			AchievementManager.FetchUserAchievementsHandler fetchUserAchievementsHandler = (AchievementManager.FetchUserAchievementsHandler)gchandle.Target;
			gchandle.Free();
			fetchUserAchievementsHandler(result);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000DC18 File Offset: 0x0000BE18
		public void FetchUserAchievements(AchievementManager.FetchUserAchievementsHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.FetchUserAchievements(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new AchievementManager.FFIMethods.FetchUserAchievementsCallback(AchievementManager.FetchUserAchievementsCallbackImpl));
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000DC54 File Offset: 0x0000BE54
		public int CountUserAchievements()
		{
			int num = 0;
			this.Methods.CountUserAchievements(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000DC7C File Offset: 0x0000BE7C
		public UserAchievement GetUserAchievement(long userAchievementId)
		{
			UserAchievement userAchievement = default(UserAchievement);
			Result result = this.Methods.GetUserAchievement(this.MethodsPtr, userAchievementId, ref userAchievement);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return userAchievement;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		public UserAchievement GetUserAchievementAt(int index)
		{
			UserAchievement userAchievement = default(UserAchievement);
			Result result = this.Methods.GetUserAchievementAt(this.MethodsPtr, index, ref userAchievement);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return userAchievement;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000DCF4 File Offset: 0x0000BEF4
		[MonoPInvokeCallback]
		private static void OnUserAchievementUpdateImpl(IntPtr ptr, ref UserAchievement userAchievement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.AchievementManagerInstance.OnUserAchievementUpdate != null)
			{
				discord.AchievementManagerInstance.OnUserAchievementUpdate(ref userAchievement);
			}
		}

		// Token: 0x040001A3 RID: 419
		private IntPtr MethodsPtr;

		// Token: 0x040001A4 RID: 420
		private object MethodsStructure;

		// Token: 0x020000C6 RID: 198
		internal struct FFIEvents
		{
			// Token: 0x040002C3 RID: 707
			internal AchievementManager.FFIEvents.UserAchievementUpdateHandler OnUserAchievementUpdate;

			// Token: 0x02000183 RID: 387
			// (Invoke) Token: 0x060006F7 RID: 1783
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UserAchievementUpdateHandler(IntPtr ptr, ref UserAchievement userAchievement);
		}

		// Token: 0x020000C7 RID: 199
		internal struct FFIMethods
		{
			// Token: 0x040002C4 RID: 708
			internal AchievementManager.FFIMethods.SetUserAchievementMethod SetUserAchievement;

			// Token: 0x040002C5 RID: 709
			internal AchievementManager.FFIMethods.FetchUserAchievementsMethod FetchUserAchievements;

			// Token: 0x040002C6 RID: 710
			internal AchievementManager.FFIMethods.CountUserAchievementsMethod CountUserAchievements;

			// Token: 0x040002C7 RID: 711
			internal AchievementManager.FFIMethods.GetUserAchievementMethod GetUserAchievement;

			// Token: 0x040002C8 RID: 712
			internal AchievementManager.FFIMethods.GetUserAchievementAtMethod GetUserAchievementAt;

			// Token: 0x02000184 RID: 388
			// (Invoke) Token: 0x060006FB RID: 1787
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetUserAchievementCallback(IntPtr ptr, Result result);

			// Token: 0x02000185 RID: 389
			// (Invoke) Token: 0x060006FF RID: 1791
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetUserAchievementMethod(IntPtr methodsPtr, long achievementId, byte percentComplete, IntPtr callbackData, AchievementManager.FFIMethods.SetUserAchievementCallback callback);

			// Token: 0x02000186 RID: 390
			// (Invoke) Token: 0x06000703 RID: 1795
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchUserAchievementsCallback(IntPtr ptr, Result result);

			// Token: 0x02000187 RID: 391
			// (Invoke) Token: 0x06000707 RID: 1799
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchUserAchievementsMethod(IntPtr methodsPtr, IntPtr callbackData, AchievementManager.FFIMethods.FetchUserAchievementsCallback callback);

			// Token: 0x02000188 RID: 392
			// (Invoke) Token: 0x0600070B RID: 1803
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountUserAchievementsMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x02000189 RID: 393
			// (Invoke) Token: 0x0600070F RID: 1807
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetUserAchievementMethod(IntPtr methodsPtr, long userAchievementId, ref UserAchievement userAchievement);

			// Token: 0x0200018A RID: 394
			// (Invoke) Token: 0x06000713 RID: 1811
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetUserAchievementAtMethod(IntPtr methodsPtr, int index, ref UserAchievement userAchievement);
		}

		// Token: 0x020000C8 RID: 200
		// (Invoke) Token: 0x0600040F RID: 1039
		public delegate void SetUserAchievementHandler(Result result);

		// Token: 0x020000C9 RID: 201
		// (Invoke) Token: 0x06000413 RID: 1043
		public delegate void FetchUserAchievementsHandler(Result result);

		// Token: 0x020000CA RID: 202
		// (Invoke) Token: 0x06000417 RID: 1047
		public delegate void UserAchievementUpdateHandler(ref UserAchievement userAchievement);
	}
}
