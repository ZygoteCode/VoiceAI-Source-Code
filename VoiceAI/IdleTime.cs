using System;
using System.Runtime.InteropServices;

// Token: 0x0200000E RID: 14
public static class IdleTime
{
	// Token: 0x06000129 RID: 297
	[DllImport("user32.dll", SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(int idHook, IdleTime.HookProc lpfn, IntPtr hMod, uint dwThreadId);

	// Token: 0x0600012A RID: 298
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhk);

	// Token: 0x0600012B RID: 299 RVA: 0x00007D55 File Offset: 0x00005F55
	private static IntPtr SetMouseHook(IdleTime.HookProc proc)
	{
		return IdleTime.SetWindowsHookEx(14, proc, IntPtr.Zero, 0U);
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00007D65 File Offset: 0x00005F65
	private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
	{
		IdleTime.didMove = true;
		IdleTime.lastMovement = DateTime.Now;
		return IntPtr.Zero;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00007D7C File Offset: 0x00005F7C
	public static void AddHook()
	{
		IdleTime._p = new IdleTime.HookProc(IdleTime.HookCallback);
		IdleTime._hook = IdleTime.SetMouseHook(IdleTime._p);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00007D9E File Offset: 0x00005F9E
	public static void Unhook()
	{
		IdleTime.UnhookWindowsHookEx(IdleTime._hook);
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00007DAB File Offset: 0x00005FAB
	public static TimeSpan CurrentIdleTime()
	{
		if (IdleTime.didMove)
		{
			return DateTime.Now - IdleTime.lastMovement;
		}
		return TimeSpan.FromSeconds(0.0);
	}

	// Token: 0x04000067 RID: 103
	private const int WH_MOUSE_LL = 14;

	// Token: 0x04000068 RID: 104
	private static DateTime lastMovement;

	// Token: 0x04000069 RID: 105
	private static IntPtr _hook;

	// Token: 0x0400006A RID: 106
	private static bool didMove;

	// Token: 0x0400006B RID: 107
	private static IdleTime.HookProc _p;

	// Token: 0x02000067 RID: 103
	// (Invoke) Token: 0x0600031F RID: 799
	private delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
}
