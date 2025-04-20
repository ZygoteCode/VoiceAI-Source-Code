using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000036 RID: 54
	public struct Activity
	{
		// Token: 0x04000126 RID: 294
		public ActivityType Type;

		// Token: 0x04000127 RID: 295
		public long ApplicationId;

		// Token: 0x04000128 RID: 296
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Name;

		// Token: 0x04000129 RID: 297
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string State;

		// Token: 0x0400012A RID: 298
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Details;

		// Token: 0x0400012B RID: 299
		public ActivityTimestamps Timestamps;

		// Token: 0x0400012C RID: 300
		public ActivityAssets Assets;

		// Token: 0x0400012D RID: 301
		public ActivityParty Party;

		// Token: 0x0400012E RID: 302
		public ActivitySecrets Secrets;

		// Token: 0x0400012F RID: 303
		public bool Instance;
	}
}
