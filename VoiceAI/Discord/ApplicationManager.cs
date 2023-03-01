using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x02000046 RID: 70
	public class ApplicationManager
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000ABE5 File Offset: 0x00008DE5
		private ApplicationManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ApplicationManager.FFIMethods));
				}
				return (ApplicationManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000AC18 File Offset: 0x00008E18
		internal ApplicationManager(IntPtr ptr, IntPtr eventsPtr, ref ApplicationManager.FFIEvents events)
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

		// Token: 0x06000213 RID: 531 RVA: 0x0000AC67 File Offset: 0x00008E67
		private void InitEvents(IntPtr eventsPtr, ref ApplicationManager.FFIEvents events)
		{
			Marshal.StructureToPtr<ApplicationManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000AC78 File Offset: 0x00008E78
		[MonoPInvokeCallback]
		private static void ValidateOrExitCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.ValidateOrExitHandler validateOrExitHandler = (ApplicationManager.ValidateOrExitHandler)gchandle.Target;
			gchandle.Free();
			validateOrExitHandler(result);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		public void ValidateOrExit(ApplicationManager.ValidateOrExitHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ValidateOrExit(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new ApplicationManager.FFIMethods.ValidateOrExitCallback(ApplicationManager.ValidateOrExitCallbackImpl));
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000ACE4 File Offset: 0x00008EE4
		public string GetCurrentLocale()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			this.Methods.GetCurrentLocale(this.MethodsPtr, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000AD1C File Offset: 0x00008F1C
		public string GetCurrentBranch()
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			this.Methods.GetCurrentBranch(this.MethodsPtr, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000AD54 File Offset: 0x00008F54
		[MonoPInvokeCallback]
		private static void GetOAuth2TokenCallbackImpl(IntPtr ptr, Result result, ref OAuth2Token oauth2Token)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.GetOAuth2TokenHandler getOAuth2TokenHandler = (ApplicationManager.GetOAuth2TokenHandler)gchandle.Target;
			gchandle.Free();
			getOAuth2TokenHandler(result, ref oauth2Token);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000AD84 File Offset: 0x00008F84
		public void GetOAuth2Token(ApplicationManager.GetOAuth2TokenHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.GetOAuth2Token(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new ApplicationManager.FFIMethods.GetOAuth2TokenCallback(ApplicationManager.GetOAuth2TokenCallbackImpl));
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000ADC0 File Offset: 0x00008FC0
		[MonoPInvokeCallback]
		private static void GetTicketCallbackImpl(IntPtr ptr, Result result, ref string data)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.GetTicketHandler getTicketHandler = (ApplicationManager.GetTicketHandler)gchandle.Target;
			gchandle.Free();
			getTicketHandler(result, ref data);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public void GetTicket(ApplicationManager.GetTicketHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.GetTicket(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new ApplicationManager.FFIMethods.GetTicketCallback(ApplicationManager.GetTicketCallbackImpl));
		}

		// Token: 0x0400017E RID: 382
		private IntPtr MethodsPtr;

		// Token: 0x0400017F RID: 383
		private object MethodsStructure;

		// Token: 0x02000085 RID: 133
		internal struct FFIEvents
		{
		}

		// Token: 0x02000086 RID: 134
		internal struct FFIMethods
		{
			// Token: 0x04000254 RID: 596
			internal ApplicationManager.FFIMethods.ValidateOrExitMethod ValidateOrExit;

			// Token: 0x04000255 RID: 597
			internal ApplicationManager.FFIMethods.GetCurrentLocaleMethod GetCurrentLocale;

			// Token: 0x04000256 RID: 598
			internal ApplicationManager.FFIMethods.GetCurrentBranchMethod GetCurrentBranch;

			// Token: 0x04000257 RID: 599
			internal ApplicationManager.FFIMethods.GetOAuth2TokenMethod GetOAuth2Token;

			// Token: 0x04000258 RID: 600
			internal ApplicationManager.FFIMethods.GetTicketMethod GetTicket;

			// Token: 0x020000F8 RID: 248
			// (Invoke) Token: 0x060004CB RID: 1227
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ValidateOrExitCallback(IntPtr ptr, Result result);

			// Token: 0x020000F9 RID: 249
			// (Invoke) Token: 0x060004CF RID: 1231
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ValidateOrExitMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.ValidateOrExitCallback callback);

			// Token: 0x020000FA RID: 250
			// (Invoke) Token: 0x060004D3 RID: 1235
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetCurrentLocaleMethod(IntPtr methodsPtr, StringBuilder locale);

			// Token: 0x020000FB RID: 251
			// (Invoke) Token: 0x060004D7 RID: 1239
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetCurrentBranchMethod(IntPtr methodsPtr, StringBuilder branch);

			// Token: 0x020000FC RID: 252
			// (Invoke) Token: 0x060004DB RID: 1243
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetOAuth2TokenCallback(IntPtr ptr, Result result, ref OAuth2Token oauth2Token);

			// Token: 0x020000FD RID: 253
			// (Invoke) Token: 0x060004DF RID: 1247
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetOAuth2TokenMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.GetOAuth2TokenCallback callback);

			// Token: 0x020000FE RID: 254
			// (Invoke) Token: 0x060004E3 RID: 1251
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetTicketCallback(IntPtr ptr, Result result, [MarshalAs(UnmanagedType.LPStr)] ref string data);

			// Token: 0x020000FF RID: 255
			// (Invoke) Token: 0x060004E7 RID: 1255
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetTicketMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.GetTicketCallback callback);
		}

		// Token: 0x02000087 RID: 135
		// (Invoke) Token: 0x0600035B RID: 859
		public delegate void ValidateOrExitHandler(Result result);

		// Token: 0x02000088 RID: 136
		// (Invoke) Token: 0x0600035F RID: 863
		public delegate void GetOAuth2TokenHandler(Result result, ref OAuth2Token oauth2Token);

		// Token: 0x02000089 RID: 137
		// (Invoke) Token: 0x06000363 RID: 867
		public delegate void GetTicketHandler(Result result, ref string data);
	}
}
