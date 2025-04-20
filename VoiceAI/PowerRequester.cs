using System;
using System.Runtime.InteropServices;

// Token: 0x0200000F RID: 15
internal static class PowerRequester
{
	// Token: 0x06000130 RID: 304
	[DllImport("kernel32.dll")]
	private static extern IntPtr PowerCreateRequest(ref PowerRequester.POWER_REQUEST_CONTEXT Context);

	// Token: 0x06000131 RID: 305
	[DllImport("kernel32.dll")]
	private static extern bool PowerSetRequest(IntPtr PowerRequestHandle, PowerRequester.PowerRequestType RequestType);

	// Token: 0x06000132 RID: 306
	[DllImport("kernel32.dll")]
	private static extern bool PowerClearRequest(IntPtr PowerRequestHandle, PowerRequester.PowerRequestType RequestType);

	// Token: 0x06000133 RID: 307
	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	private static extern int CloseHandle(IntPtr hObject);

	// Token: 0x06000134 RID: 308 RVA: 0x00007DD4 File Offset: 0x00005FD4
	public static void EnableConstantDisplayAndPower(bool enable)
	{
		if (enable && !PowerRequester.state)
		{
			PowerRequester._PowerRequestContext.Version = 0U;
			PowerRequester._PowerRequestContext.Flags = 1U;
			PowerRequester._PowerRequestContext.SimpleReasonString = "Voice.ai";
			PowerRequester._PowerRequest = PowerRequester.PowerCreateRequest(ref PowerRequester._PowerRequestContext);
			PowerRequester.PowerSetRequest(PowerRequester._PowerRequest, PowerRequester.PowerRequestType.PowerRequestSystemRequired);
		}
		else if (PowerRequester.state)
		{
			PowerRequester.PowerClearRequest(PowerRequester._PowerRequest, PowerRequester.PowerRequestType.PowerRequestSystemRequired);
			PowerRequester.CloseHandle(PowerRequester._PowerRequest);
		}
		PowerRequester.state = enable;
	}

	// Token: 0x0400006C RID: 108
	private static PowerRequester.POWER_REQUEST_CONTEXT _PowerRequestContext;

	// Token: 0x0400006D RID: 109
	private static IntPtr _PowerRequest;

	// Token: 0x0400006E RID: 110
	public static bool state;

	// Token: 0x0400006F RID: 111
	private const int POWER_REQUEST_CONTEXT_VERSION = 0;

	// Token: 0x04000070 RID: 112
	private const int POWER_REQUEST_CONTEXT_SIMPLE_STRING = 1;

	// Token: 0x04000071 RID: 113
	private const int POWER_REQUEST_CONTEXT_DETAILED_STRING = 2;

	// Token: 0x02000068 RID: 104
	private enum PowerRequestType
	{
		// Token: 0x040001F4 RID: 500
		PowerRequestDisplayRequired,
		// Token: 0x040001F5 RID: 501
		PowerRequestSystemRequired,
		// Token: 0x040001F6 RID: 502
		PowerRequestAwayModeRequired,
		// Token: 0x040001F7 RID: 503
		PowerRequestMaximum
	}

	// Token: 0x02000069 RID: 105
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct POWER_REQUEST_CONTEXT
	{
		// Token: 0x040001F8 RID: 504
		public uint Version;

		// Token: 0x040001F9 RID: 505
		public uint Flags;

		// Token: 0x040001FA RID: 506
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SimpleReasonString;
	}

	// Token: 0x0200006A RID: 106
	private struct PowerRequestContextDetailedInformation
	{
		// Token: 0x040001FB RID: 507
		public IntPtr LocalizedReasonModule;

		// Token: 0x040001FC RID: 508
		public uint LocalizedReasonId;

		// Token: 0x040001FD RID: 509
		public uint ReasonStringCount;

		// Token: 0x040001FE RID: 510
		[MarshalAs(UnmanagedType.LPWStr)]
		public string[] ReasonStrings;
	}

	// Token: 0x0200006B RID: 107
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct POWER_REQUEST_CONTEXT_DETAILED
	{
		// Token: 0x040001FF RID: 511
		public uint Version;

		// Token: 0x04000200 RID: 512
		public uint Flags;

		// Token: 0x04000201 RID: 513
		public PowerRequester.PowerRequestContextDetailedInformation DetailedInformation;
	}
}
