using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200002E RID: 46
	public struct OAuth2Token
	{
		// Token: 0x04000111 RID: 273
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string AccessToken;

		// Token: 0x04000112 RID: 274
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		public string Scopes;

		// Token: 0x04000113 RID: 275
		public long Expires;
	}
}
