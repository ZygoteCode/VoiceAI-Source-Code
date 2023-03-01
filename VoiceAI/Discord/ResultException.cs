using System;

namespace Discord
{
	// Token: 0x02000043 RID: 67
	public class ResultException : Exception
	{
		// Token: 0x060001FB RID: 507 RVA: 0x0000A3BD File Offset: 0x000085BD
		public ResultException(Result result)
			: base(result.ToString())
		{
		}

		// Token: 0x04000153 RID: 339
		public readonly Result Result;
	}
}
