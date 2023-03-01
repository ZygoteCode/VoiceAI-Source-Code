using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000042 RID: 66
	public struct LobbySearchQuery
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A279 File Offset: 0x00008479
		private LobbySearchQuery.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbySearchQuery.FFIMethods));
				}
				return (LobbySearchQuery.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A2AC File Offset: 0x000084AC
		public void Filter(string key, LobbySearchComparison comparison, LobbySearchCast cast, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Filter(this.MethodsPtr, key, comparison, cast, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A2F4 File Offset: 0x000084F4
		public void Sort(string key, LobbySearchCast cast, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Sort(this.MethodsPtr, key, cast, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000A338 File Offset: 0x00008538
		public void Limit(uint limit)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Limit(this.MethodsPtr, limit);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000A37C File Offset: 0x0000857C
		public void Distance(LobbySearchDistance distance)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Distance(this.MethodsPtr, distance);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x04000151 RID: 337
		internal IntPtr MethodsPtr;

		// Token: 0x04000152 RID: 338
		internal object MethodsStructure;

		// Token: 0x02000080 RID: 128
		internal struct FFIMethods
		{
			// Token: 0x04000225 RID: 549
			internal LobbySearchQuery.FFIMethods.FilterMethod Filter;

			// Token: 0x04000226 RID: 550
			internal LobbySearchQuery.FFIMethods.SortMethod Sort;

			// Token: 0x04000227 RID: 551
			internal LobbySearchQuery.FFIMethods.LimitMethod Limit;

			// Token: 0x04000228 RID: 552
			internal LobbySearchQuery.FFIMethods.DistanceMethod Distance;

			// Token: 0x020000E4 RID: 228
			// (Invoke) Token: 0x0600047B RID: 1147
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FilterMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, LobbySearchComparison comparison, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x020000E5 RID: 229
			// (Invoke) Token: 0x0600047F RID: 1151
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SortMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x020000E6 RID: 230
			// (Invoke) Token: 0x06000483 RID: 1155
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result LimitMethod(IntPtr methodsPtr, uint limit);

			// Token: 0x020000E7 RID: 231
			// (Invoke) Token: 0x06000487 RID: 1159
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DistanceMethod(IntPtr methodsPtr, LobbySearchDistance distance);
		}
	}
}
