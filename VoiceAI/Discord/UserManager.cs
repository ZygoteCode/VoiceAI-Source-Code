using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000047 RID: 71
	public class UserManager
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000AE2C File Offset: 0x0000902C
		private UserManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(UserManager.FFIMethods));
				}
				return (UserManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600021D RID: 541 RVA: 0x0000AE5C File Offset: 0x0000905C
		// (remove) Token: 0x0600021E RID: 542 RVA: 0x0000AE94 File Offset: 0x00009094
		public event UserManager.CurrentUserUpdateHandler OnCurrentUserUpdate;

		// Token: 0x0600021F RID: 543 RVA: 0x0000AECC File Offset: 0x000090CC
		internal UserManager(IntPtr ptr, IntPtr eventsPtr, ref UserManager.FFIEvents events)
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

		// Token: 0x06000220 RID: 544 RVA: 0x0000AF1B File Offset: 0x0000911B
		private void InitEvents(IntPtr eventsPtr, ref UserManager.FFIEvents events)
		{
			events.OnCurrentUserUpdate = new UserManager.FFIEvents.CurrentUserUpdateHandler(UserManager.OnCurrentUserUpdateImpl);
			Marshal.StructureToPtr<UserManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000AF3C File Offset: 0x0000913C
		public User GetCurrentUser()
		{
			User user = default(User);
			Result result = this.Methods.GetCurrentUser(this.MethodsPtr, ref user);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return user;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000AF78 File Offset: 0x00009178
		[MonoPInvokeCallback]
		private static void GetUserCallbackImpl(IntPtr ptr, Result result, ref User user)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			UserManager.GetUserHandler getUserHandler = (UserManager.GetUserHandler)gchandle.Target;
			gchandle.Free();
			getUserHandler(result, ref user);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000AFA8 File Offset: 0x000091A8
		public void GetUser(long userId, UserManager.GetUserHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.GetUser(this.MethodsPtr, userId, GCHandle.ToIntPtr(gchandle), new UserManager.FFIMethods.GetUserCallback(UserManager.GetUserCallbackImpl));
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000AFE8 File Offset: 0x000091E8
		public PremiumType GetCurrentUserPremiumType()
		{
			PremiumType premiumType = PremiumType.None;
			Result result = this.Methods.GetCurrentUserPremiumType(this.MethodsPtr, ref premiumType);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return premiumType;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B01C File Offset: 0x0000921C
		public bool CurrentUserHasFlag(UserFlag flag)
		{
			bool flag2 = false;
			Result result = this.Methods.CurrentUserHasFlag(this.MethodsPtr, flag, ref flag2);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag2;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B050 File Offset: 0x00009250
		[MonoPInvokeCallback]
		private static void OnCurrentUserUpdateImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.UserManagerInstance.OnCurrentUserUpdate != null)
			{
				discord.UserManagerInstance.OnCurrentUserUpdate();
			}
		}

		// Token: 0x04000180 RID: 384
		private IntPtr MethodsPtr;

		// Token: 0x04000181 RID: 385
		private object MethodsStructure;

		// Token: 0x0200008A RID: 138
		internal struct FFIEvents
		{
			// Token: 0x04000259 RID: 601
			internal UserManager.FFIEvents.CurrentUserUpdateHandler OnCurrentUserUpdate;

			// Token: 0x02000100 RID: 256
			// (Invoke) Token: 0x060004EB RID: 1259
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CurrentUserUpdateHandler(IntPtr ptr);
		}

		// Token: 0x0200008B RID: 139
		internal struct FFIMethods
		{
			// Token: 0x0400025A RID: 602
			internal UserManager.FFIMethods.GetCurrentUserMethod GetCurrentUser;

			// Token: 0x0400025B RID: 603
			internal UserManager.FFIMethods.GetUserMethod GetUser;

			// Token: 0x0400025C RID: 604
			internal UserManager.FFIMethods.GetCurrentUserPremiumTypeMethod GetCurrentUserPremiumType;

			// Token: 0x0400025D RID: 605
			internal UserManager.FFIMethods.CurrentUserHasFlagMethod CurrentUserHasFlag;

			// Token: 0x02000101 RID: 257
			// (Invoke) Token: 0x060004EF RID: 1263
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetCurrentUserMethod(IntPtr methodsPtr, ref User currentUser);

			// Token: 0x02000102 RID: 258
			// (Invoke) Token: 0x060004F3 RID: 1267
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetUserCallback(IntPtr ptr, Result result, ref User user);

			// Token: 0x02000103 RID: 259
			// (Invoke) Token: 0x060004F7 RID: 1271
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetUserMethod(IntPtr methodsPtr, long userId, IntPtr callbackData, UserManager.FFIMethods.GetUserCallback callback);

			// Token: 0x02000104 RID: 260
			// (Invoke) Token: 0x060004FB RID: 1275
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetCurrentUserPremiumTypeMethod(IntPtr methodsPtr, ref PremiumType premiumType);

			// Token: 0x02000105 RID: 261
			// (Invoke) Token: 0x060004FF RID: 1279
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CurrentUserHasFlagMethod(IntPtr methodsPtr, UserFlag flag, ref bool hasFlag);
		}

		// Token: 0x0200008C RID: 140
		// (Invoke) Token: 0x06000367 RID: 871
		public delegate void GetUserHandler(Result result, ref User user);

		// Token: 0x0200008D RID: 141
		// (Invoke) Token: 0x0600036B RID: 875
		public delegate void CurrentUserUpdateHandler();
	}
}
