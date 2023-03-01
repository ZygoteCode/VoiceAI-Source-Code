using System;
using System.Windows.Forms;

// Token: 0x02000004 RID: 4
public static class ControlExtensions
{
	// Token: 0x06000045 RID: 69 RVA: 0x000031B0 File Offset: 0x000013B0
	public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
	{
		if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
		{
			return;
		}
		if (control.InvokeRequired)
		{
			control.BeginInvoke(action);
			return;
		}
		action();
	}
}
