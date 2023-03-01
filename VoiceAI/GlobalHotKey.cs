using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// Token: 0x0200000D RID: 13
public static class GlobalHotKey
{
	// Token: 0x06000124 RID: 292
	[DllImport("user32.dll")]
	private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

	// Token: 0x06000125 RID: 293
	[DllImport("user32.dll")]
	private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

	// Token: 0x06000126 RID: 294
	[DllImport("user32.dll")]
	private static extern ushort VkKeyScan(char ch);

	// Token: 0x06000127 RID: 295 RVA: 0x00007C80 File Offset: 0x00005E80
	public static int Register(IntPtr hwnd, bool alt, bool control, bool shift, string key)
	{
		uint flags = 16384U;
		if (alt)
		{
			flags |= 1U;
		}
		if (control)
		{
			flags |= 2U;
		}
		if (shift)
		{
			flags |= 4U;
		}
		if (!control && !alt && !shift)
		{
			return 0;
		}
		int res = 0;
		BrowserForm.browserform.Invoke(new MethodInvoker(delegate
		{
			if (GlobalHotKey.RegisterHotKey(hwnd, ++GlobalHotKey.hotKeyIndex, flags, (uint)GlobalHotKey.VkKeyScan(key.ToLower().ToCharArray()[0])))
			{
				res = GlobalHotKey.hotKeyIndex;
			}
		}));
		return res;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00007D10 File Offset: 0x00005F10
	public static int Unregister(IntPtr hwnd, int id)
	{
		int res = 0;
		BrowserForm.browserform.Invoke(new MethodInvoker(delegate
		{
			if (GlobalHotKey.UnregisterHotKey(hwnd, id))
			{
				res = 1;
			}
		}));
		return res;
	}

	// Token: 0x04000061 RID: 97
	private const int MOD_ALT = 1;

	// Token: 0x04000062 RID: 98
	private const int MOD_CONTROL = 2;

	// Token: 0x04000063 RID: 99
	private const int MOD_SHIFT = 4;

	// Token: 0x04000064 RID: 100
	private const int MOD_NOREPEAT = 16384;

	// Token: 0x04000065 RID: 101
	public const int WM_HOTKEY = 786;

	// Token: 0x04000066 RID: 102
	private static int hotKeyIndex;
}
