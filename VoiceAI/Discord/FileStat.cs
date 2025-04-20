using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200003A RID: 58
	public struct FileStat
	{
		// Token: 0x0400013B RID: 315
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Filename;

		// Token: 0x0400013C RID: 316
		public ulong Size;

		// Token: 0x0400013D RID: 317
		public ulong LastModified;
	}
}
