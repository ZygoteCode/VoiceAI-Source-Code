using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000034 RID: 52
	public struct ActivityParty
	{
		// Token: 0x04000121 RID: 289
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Id;

		// Token: 0x04000122 RID: 290
		public PartySize Size;
	}
}
