using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200003F RID: 63
	public struct UserAchievement
	{
		// Token: 0x04000149 RID: 329
		public long UserId;

		// Token: 0x0400014A RID: 330
		public long AchievementId;

		// Token: 0x0400014B RID: 331
		public byte PercentComplete;

		// Token: 0x0400014C RID: 332
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string UnlockedAt;
	}
}
