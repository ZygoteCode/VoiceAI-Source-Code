using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000039 RID: 57
	public struct Lobby
	{
		// Token: 0x04000135 RID: 309
		public long Id;

		// Token: 0x04000136 RID: 310
		public LobbyType Type;

		// Token: 0x04000137 RID: 311
		public long OwnerId;

		// Token: 0x04000138 RID: 312
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Secret;

		// Token: 0x04000139 RID: 313
		public uint Capacity;

		// Token: 0x0400013A RID: 314
		public bool Locked;
	}
}
