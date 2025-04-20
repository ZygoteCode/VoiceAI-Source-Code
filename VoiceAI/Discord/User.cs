using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200002D RID: 45
	public struct User
	{
		// Token: 0x0400010C RID: 268
		public long Id;

		// Token: 0x0400010D RID: 269
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Username;

		// Token: 0x0400010E RID: 270
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string Discriminator;

		// Token: 0x0400010F RID: 271
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Avatar;

		// Token: 0x04000110 RID: 272
		public bool Bot;
	}
}
