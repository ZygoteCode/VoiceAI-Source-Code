using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200003D RID: 61
	public struct Sku
	{
		// Token: 0x04000143 RID: 323
		public long Id;

		// Token: 0x04000144 RID: 324
		public SkuType Type;

		// Token: 0x04000145 RID: 325
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Name;

		// Token: 0x04000146 RID: 326
		public SkuPrice Price;
	}
}
