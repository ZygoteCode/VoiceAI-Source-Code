using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000035 RID: 53
	public struct ActivitySecrets
	{
		// Token: 0x04000123 RID: 291
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Match;

		// Token: 0x04000124 RID: 292
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Join;

		// Token: 0x04000125 RID: 293
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Spectate;
	}
}
