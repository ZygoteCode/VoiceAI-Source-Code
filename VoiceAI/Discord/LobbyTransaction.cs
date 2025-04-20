using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000040 RID: 64
	public struct LobbyTransaction
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00009FFA File Offset: 0x000081FA
		private LobbyTransaction.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyTransaction.FFIMethods));
				}
				return (LobbyTransaction.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A02C File Offset: 0x0000822C
		public void SetType(LobbyType type)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetType(this.MethodsPtr, type);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000A070 File Offset: 0x00008270
		public void SetOwner(long ownerId)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetOwner(this.MethodsPtr, ownerId);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A0B4 File Offset: 0x000082B4
		public void SetCapacity(uint capacity)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetCapacity(this.MethodsPtr, capacity);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A0F8 File Offset: 0x000082F8
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

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A13C File Offset: 0x0000833C
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

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A180 File Offset: 0x00008380
		public void SetLocked(bool locked)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetLocked(this.MethodsPtr, locked);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x0400014D RID: 333
		internal IntPtr MethodsPtr;

		// Token: 0x0400014E RID: 334
		internal object MethodsStructure;

		// Token: 0x0200007E RID: 126
		internal struct FFIMethods
		{
			// Token: 0x0400021D RID: 541
			internal LobbyTransaction.FFIMethods.SetTypeMethod SetType;

			// Token: 0x0400021E RID: 542
			internal LobbyTransaction.FFIMethods.SetOwnerMethod SetOwner;

			// Token: 0x0400021F RID: 543
			internal LobbyTransaction.FFIMethods.SetCapacityMethod SetCapacity;

			// Token: 0x04000220 RID: 544
			internal LobbyTransaction.FFIMethods.SetMetadataMethod SetMetadata;

			// Token: 0x04000221 RID: 545
			internal LobbyTransaction.FFIMethods.DeleteMetadataMethod DeleteMetadata;

			// Token: 0x04000222 RID: 546
			internal LobbyTransaction.FFIMethods.SetLockedMethod SetLocked;

			// Token: 0x020000DC RID: 220
			// (Invoke) Token: 0x0600045B RID: 1115
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetTypeMethod(IntPtr methodsPtr, LobbyType type);

			// Token: 0x020000DD RID: 221
			// (Invoke) Token: 0x0600045F RID: 1119
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetOwnerMethod(IntPtr methodsPtr, long ownerId);

			// Token: 0x020000DE RID: 222
			// (Invoke) Token: 0x06000463 RID: 1123
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetCapacityMethod(IntPtr methodsPtr, uint capacity);

			// Token: 0x020000DF RID: 223
			// (Invoke) Token: 0x06000467 RID: 1127
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x020000E0 RID: 224
			// (Invoke) Token: 0x0600046B RID: 1131
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key);

			// Token: 0x020000E1 RID: 225
			// (Invoke) Token: 0x0600046F RID: 1135
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLockedMethod(IntPtr methodsPtr, bool locked);
		}
	}
}
