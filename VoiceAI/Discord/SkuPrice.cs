using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200003C RID: 60
	public struct SkuPrice
	{
		// Token: 0x04000141 RID: 321
		public uint Amount;

		// Token: 0x04000142 RID: 322
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string Currency;
	}
}
