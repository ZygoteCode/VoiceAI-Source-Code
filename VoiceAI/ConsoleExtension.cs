using System;
using System.Runtime.InteropServices;

// Token: 0x02000011 RID: 17
internal static class ConsoleExtension
{
	// Token: 0x0600013A RID: 314
	[DllImport("kernel32.dll")]
	private static extern IntPtr GetConsoleWindow();

	// Token: 0x0600013B RID: 315
	[DllImport("user32.dll")]
	private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	// Token: 0x0600013C RID: 316 RVA: 0x00008232 File Offset: 0x00006432
	public static void Hide()
	{
		ConsoleExtension.ShowWindow(ConsoleExtension.handle, 0);
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00008240 File Offset: 0x00006440
	public static void Show()
	{
		ConsoleExtension.ShowWindow(ConsoleExtension.handle, 5);
	}

	// Token: 0x04000076 RID: 118
	private const int SW_HIDE = 0;

	// Token: 0x04000077 RID: 119
	private const int SW_SHOW = 5;

	// Token: 0x04000078 RID: 120
	private static readonly IntPtr handle = ConsoleExtension.GetConsoleWindow();
}
