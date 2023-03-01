using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugSplat;
using CefSharp;
using CefSharp.WinForms;

// Token: 0x02000010 RID: 16
public static class Program
{
	// Token: 0x06000135 RID: 309 RVA: 0x00007E54 File Offset: 0x00006054
	private static void MuteDance()
	{
		Mutex mutex;
		if (Mutex.TryOpenExisting("Global\\Voice.ai", out mutex))
		{
			Mutex mutex2 = new Mutex(true, "Global\\Voice.ai2");
			mutex2.WaitOne();
			Thread.Sleep(1000);
			mutex2.ReleaseMutex();
			mutex2.Dispose();
			mutex.Dispose();
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00007E9C File Offset: 0x0000609C
	private static void MuteThread()
	{
		while (Program.isActive)
		{
			Mutex mutex;
			if (Mutex.TryOpenExisting("Global\\Voice.ai2", out mutex))
			{
				mutex.Dispose();
				if (BrowserForm.browserform != null)
				{
					BrowserForm.browserform.Invoke(new MethodInvoker(delegate
					{
						BrowserForm.browserform.ShowForm();
					}));
				}
				Thread.Sleep(2000);
			}
			Thread.Sleep(200);
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00007F0C File Offset: 0x0000610C
	[STAThread]
	public static int Main(string[] args)
	{
		if (args.Length == 1 && args[0] == "testload")
		{
			Program.PrimaryProcess = true;
			if (VoiceChanger.Create(0L) == null)
			{
				Console.WriteLine("Invalid");
			}
			else
			{
				Console.WriteLine("Started Successfully!\n\n------------------------------------------------------");
				Thread.CurrentThread.Join();
			}
			return 0;
		}
		if (args.Length == 2 && args[0] == "discord")
		{
			DiscordManager.singleton().ActivateDiscord(args[1]);
			return 0;
		}
		if (args.Length == 1 && args[0] == "start" && !Utilities.Singleton().GetStartOnBoot())
		{
			return 0;
		}
		ConsoleExtension.Hide();
		if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1 && args.Length == 0)
		{
			Program.MuteDance();
			return 0;
		}
		Mutex mutex = null;
		if (args.Length == 0 || (args.Length != 0 && string.Join(" ", args).ToLower().Contains("open")))
		{
			mutex = new Mutex(true, "Global\\Voice.ai");
			mutex.WaitOne();
			new Thread(new ThreadStart(Program.MuteThread)).Start();
		}
		int num;
		try
		{
			num = Program.RunNormalStartup(args);
		}
		finally
		{
			if (mutex != null)
			{
				mutex.ReleaseMutex();
				mutex.Dispose();
			}
		}
		return num;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00008044 File Offset: 0x00006244
	private static int RunNormalStartup(string[] args)
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		Cef.EnableHighDPISupport();
		int num = SelfHost.Main(args);
		if (num >= 0)
		{
			return num;
		}
		Program.isActive = true;
		Program.PrimaryProcess = true;
		try
		{
			CrashReporter.Init("voice_ai", "Voice.ai", Assembly.GetExecutingAssembly().GetName().Version.ToString());
			AppDomain.CurrentDomain.UnhandledException += CrashReporter.AppDomainUnhandledExceptionHandler;
			Application.ThreadException += CrashReporter.ApplicationThreadException;
			TaskScheduler.UnobservedTaskException += CrashReporter.TaskSchedulerUnobservedTaskExceptionHandler;
		}
		catch
		{
		}
		CefSettings cefSettings = new CefSettings();
		cefSettings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voice.ai\\Cache");
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voice.ai\\Cache");
		cefSettings.CefCommandLineArgs.Add("enable-media-stream");
		cefSettings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
		cefSettings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");
		cefSettings.CefCommandLineArgs.Add("enable-overlay-scrollbar", "0");
		cefSettings.CefCommandLineArgs.Add("enable-smooth-scrolling", "0");
		cefSettings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
		cefSettings.BackgroundColor = Cef.ColorSetARGB(0U, 0U, 0U, 0U);
		cefSettings.LogSeverity = 99;
		cefSettings.BrowserSubprocessPath = Process.GetCurrentProcess().MainModule.FileName;
		Cef.Initialize(cefSettings, true, null);
		new VCController();
		if (AudioService.service == null)
		{
			new AudioService();
		}
		using (Process currentProcess = Process.GetCurrentProcess())
		{
			currentProcess.PriorityClass = ProcessPriorityClass.High;
		}
		IdleTime.AddHook();
		BrowserForm browserForm = new BrowserForm();
		DiscordManager.singleton().ActivateMainDiscord();
		if (Utilities.Singleton().GetStartInTray())
		{
			browserForm.MinimizeToTray();
		}
		Application.Run(browserForm);
		return 0;
	}

	// Token: 0x04000072 RID: 114
	public const string APP_NAME = "voice.ai";

	// Token: 0x04000073 RID: 115
	public static bool isActive = true;

	// Token: 0x04000074 RID: 116
	private const string appMutex = "Global\\Voice.ai";

	// Token: 0x04000075 RID: 117
	public static bool PrimaryProcess = false;
}
