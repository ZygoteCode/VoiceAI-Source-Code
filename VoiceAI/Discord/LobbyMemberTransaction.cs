using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000041 RID: 65
	public struct LobbyMemberTransaction
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000A1C1 File Offset: 0x000083C1
		private LobbyMemberTransaction.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyMemberTransaction.FFIMethods));
				}
				return (LobbyMemberTransaction.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A1F4 File Offset: 0x000083F4
		public void SetMetadata(string key, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetMetadata(this.MethodsPtr, key, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A238 File Offset: 0x00008438
		public void DeleteMetadata(string key)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.DeleteMetadata(this.MethodsPtr, key);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x0400014F RID: 335
		internal IntPtr MethodsPtr;

		// Token: 0x04000150 RID: 336
		internal object MethodsStructure;

		// Token: 0x0200007F RID: 127
		internal struct FFIMethods
		{
			// Token: 0x04000223 RID: 547
			internal LobbyMemberTransaction.FFIMethods.SetMetadataMethod SetMetadata;

			// Token: 0x04000224 RID: 548
			internal LobbyMemberTransaction.FFIMethods.DeleteMetadataMethod DeleteMetadata;

			// Token: 0x020000E2 RID: 226
			// (Invoke) Token: 0x06000473 RID: 1139
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x020000E3 RID: 227
			// (Invoke) Token: 0x06000477 RID: 1143
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key);
		}
	}
}
