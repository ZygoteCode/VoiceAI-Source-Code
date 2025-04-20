using System;

namespace Discord
{
	// Token: 0x0200002F RID: 47
	public struct ImageHandle
	{
		// Token: 0x060001EA RID: 490 RVA: 0x00009FBD File Offset: 0x000081BD
		public static ImageHandle User(long id)
		{
			return ImageHandle.User(id, 128U);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009FCC File Offset: 0x000081CC
		public static ImageHandle User(long id, uint size)
		{
			return new ImageHandle
			{
				Type = ImageType.User,
				Id = id,
				Size = size
			};
		}

		// Token: 0x04000114 RID: 276
		public ImageType Type;

		// Token: 0x04000115 RID: 277
		public long Id;

		// Token: 0x04000116 RID: 278
		public uint Size;
	}
}
