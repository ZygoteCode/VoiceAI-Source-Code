using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000032 RID: 50
	public struct ActivityAssets
	{
		// Token: 0x0400011B RID: 283
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string LargeImage;

		// Token: 0x0400011C RID: 284
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string LargeText;

		// Token: 0x0400011D RID: 285
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SmallImage;

		// Token: 0x0400011E RID: 286
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SmallText;
	}
}
