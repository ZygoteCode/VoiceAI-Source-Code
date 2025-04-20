using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200003E RID: 62
	public struct InputMode
	{
		// Token: 0x04000147 RID: 327
		public InputModeType Type;

		// Token: 0x04000148 RID: 328
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Shortcut;
	}
}
