using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Enums;
using CefSharp.Structs;
using CefSharp.Web;
using CefSharp.WinForms;
using VoiceAIGui.Properties;

// Token: 0x02000002 RID: 2
public partial class BrowserForm : Form, IDisplayHandler
{
	// Token: 0x06000001 RID: 1
	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	// Token: 0x06000002 RID: 2
	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002048 File Offset: 0x00000248
	// (set) Token: 0x06000004 RID: 4 RVA: 0x00002050 File Offset: 0x00000250
	public object FavIcon
	{
		get
		{
			return this._favIcon;
		}
		set
		{
			this._favIcon = value;
			this.OnPropertyChanged("FavIcon");
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002064 File Offset: 0x00000264
	protected unsafe override void WndProc(ref Message m)
	{
		if (m.Msg == 786)
		{
			WebBrowserExtensions.ExecuteScriptAsync(this.browser, string.Format("hotkeyPressed({0});", m.WParam.ToInt64().ToString()));
		}
		else if (m.Msg == 134)
		{
			m.LParam = new IntPtr(-1);
		}
		else if (m.Msg == 131)
		{
			BrowserForm.NCCALCSIZE_PARAMS* ptr = (BrowserForm.NCCALCSIZE_PARAMS*)(void*)m.LParam;
			BrowserForm.NCCALCSIZE_PARAMS* ptr2 = ptr;
			ptr2->rectProposed.Top = ptr2->rectProposed.Top;
			BrowserForm.NCCALCSIZE_PARAMS* ptr3 = ptr;
			ptr3->rectProposed.Left = ptr3->rectProposed.Left + 1;
			BrowserForm.NCCALCSIZE_PARAMS* ptr4 = ptr;
			ptr4->rectProposed.Bottom = ptr4->rectProposed.Bottom - 1;
			BrowserForm.NCCALCSIZE_PARAMS* ptr5 = ptr;
			ptr5->rectProposed.Right = ptr5->rectProposed.Right - 1;
			return;
		}
		base.WndProc(ref m);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002128 File Offset: 0x00000328
	public BrowserForm()
	{
		this.InitializeComponent();
		this.SetupNotifyIcon();
		BrowserForm.browserform = this;
		BrowserForm.browserHandle = base.Handle;
		this.Text = this.title;
		this.Dock = DockStyle.Top;
		this.pictureBox2.MouseDown += this.PictureBox2_MouseDown;
		Size size = base.Size;
		int num = size.Width;
		size.Width = num + 1;
		num = size.Height;
		size.Height = num + 1;
		base.Size = size;
		this.browser = new ChromiumWebBrowser(new HtmlString("<body bgcolor=black>", false, null), null);
		BrowserSettings browserSettings = new BrowserSettings(false);
		this.browser.BackColor = Color.Black;
		browserSettings.BackgroundColor = Cef.ColorSetARGB(0U, 0U, 0U, 0U);
		this.browser.BrowserSettings = browserSettings;
		EventHandler<LoadingStateChangedEventArgs> h = null;
		h = delegate(object sender, LoadingStateChangedEventArgs e)
		{
			this.browser.LoadingStateChanged -= h;
			this.browser.LoadingStateChanged += this.OnLoadingStateChanged;
			this.browser.InvokeOnUiThreadIfRequired(delegate
			{
				this.browser.Visible = true;
			});
			this.browser.Load("https://voice.ai/v1/beta");
		};
		this.browser.LoadingStateChanged += h;
		IntPtr handle = this.browser.Handle;
		this.pictureBox3.Controls.Add(this.browser);
		this.browser.IsBrowserInitializedChanged += this.OnIsBrowserInitializedChanged;
		this.browser.ConsoleMessage += this.OnBrowserConsoleMessage;
		this.browser.StatusMessage += this.OnBrowserStatusMessage;
		this.browser.TitleChanged += this.OnBrowserTitleChanged;
		this.browser.AddressChanged += this.OnBrowserAddressChanged;
		this.browser.LoadError += this.OnBrowserLoadError;
		this.browser.FrameLoadEnd += this.OnBrowserFrameLoadEnd;
		this.browser.ConsoleMessage += this.Browser_ConsoleMessage;
		this.browser.IsBrowserInitializedChanged += this.IsBrowserInitializedChanged;
		this.browser.DragHandler = new BrowserForm.CustomDragHandler();
		this.browser.MenuHandler = new BrowserForm.CustomMenuHandler();
		this.browser.RequestHandler = new BrowserForm.CustomRequestHandler();
		this.browser.DisplayHandler = this;
		this.browser.DownloadHandler = new DownloadHandler();
		base.FormClosing += this.BrowserForm_FormClosing;
		base.FormClosed += this.BrowserForm_FormClosed;
		this.browser.JavascriptObjectRepository.Register("VCController", new VCController(), true, null);
		this.label1.Text = "voice.ai";
		string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
		string text = (Environment.Is64BitProcess ? "x64" : "x86");
		string.Format("Environment: {0}", text);
		base.ControlBox = false;
		this.Text = "Voice.ai - Voice Changer";
		new VCController();
		base.Icon = Resources.app;
		Size size2 = base.Size;
		size2.Height = Math.Min(size2.Height, Screen.PrimaryScreen.WorkingArea.Height);
		size2.Width = Math.Min(size2.Width, Screen.PrimaryScreen.WorkingArea.Width);
		base.Size = size2;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002488 File Offset: 0x00000688
	private void BrowserForm_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000248C File Offset: 0x0000068C
	private void SetupNotifyIcon()
	{
		this.notify = new NotifyIcon();
		this.notify.Icon = Resources.app;
		this.notify.Visible = true;
		this.notify.Click += this.NotifyIconClicked;
		this.notifyContext = new ContextMenu();
		MenuItem menuItem = new MenuItem("Voice.ai Voice Changer");
		menuItem.Enabled = false;
		this.notifyContext.MenuItems.Add(menuItem);
		MenuItem menuItem2 = new MenuItem("-");
		menuItem2.Enabled = false;
		this.notifyContext.MenuItems.Add(menuItem2);
		MenuItem menuItem3 = new MenuItem("Show");
		menuItem3.Click += this.NotifyShowClicked;
		this.notifyContext.MenuItems.Add(menuItem3);
		MenuItem menuItem4 = new MenuItem("Close");
		menuItem4.Click += this.NotifyCloseClicked;
		this.notifyContext.MenuItems.Add(menuItem4);
		this.notify.ContextMenu = this.notifyContext;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002599 File Offset: 0x00000799
	public void MinimizeToTray()
	{
	}

	// Token: 0x0600000A RID: 10 RVA: 0x0000259B File Offset: 0x0000079B
	private void NotifyIconClicked(object sender, EventArgs e)
	{
		if (e.GetType() == typeof(MouseEventArgs) && ((MouseEventArgs)e).Button == MouseButtons.Left)
		{
			this.ShowForm();
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000025CC File Offset: 0x000007CC
	public void ShowForm()
	{
		base.Show();
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000025D4 File Offset: 0x000007D4
	private void NotifyShowClicked(object sender, EventArgs e)
	{
		this.ShowForm();
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000025DC File Offset: 0x000007DC
	private void NotifyCloseClicked(object sender, EventArgs e)
	{
		this.forceClose = true;
		this.notify.Visible = false;
		base.Close();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000025F7 File Offset: 0x000007F7
	private void PictureBox2_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			BrowserForm.ReleaseCapture();
			BrowserForm.SendMessage(base.Handle, 161, 2, 0);
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002620 File Offset: 0x00000820
	private void BrowserForm_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			int width = SystemInformation.FrameBorderSize.Width;
			int height = SystemInformation.FrameBorderSize.Height;
			this.mouseDragOffset = new Point(-(e.X + width), -(e.Y + height));
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002674 File Offset: 0x00000874
	private void BrowserForm_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			Point mousePosition = Control.MousePosition;
			mousePosition.Offset(this.mouseDragOffset.X, this.mouseDragOffset.Y);
			base.Location = mousePosition;
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000026B8 File Offset: 0x000008B8
	private void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
	{
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000026BA File Offset: 0x000008BA
	private void IsBrowserInitializedChanged(object sender, EventArgs e)
	{
		bool isBrowserInitialized = this.browser.IsBrowserInitialized;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000026C8 File Offset: 0x000008C8
	private void OnBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs args)
	{
		if (args.Frame.IsMain)
		{
			args.Browser.MainFrame.ExecuteJavaScriptAsync("document.body.style.overflow = \"hidden\"", "about:blank", 1);
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000026F4 File Offset: 0x000008F4
	private async void OnBrowserLoadError(object sender, LoadErrorEventArgs e)
	{
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002724 File Offset: 0x00000924
	private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
	{
		ChromiumWebBrowser b = (ChromiumWebBrowser)sender;
		this.InvokeOnUiThreadIfRequired(delegate
		{
			b.Focus();
		});
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002755 File Offset: 0x00000955
	private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
	{
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002757 File Offset: 0x00000957
	private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
	{
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000275C File Offset: 0x0000095C
	private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
	{
		this.SetCanGoBack(args.CanGoBack);
		this.SetCanGoForward(args.CanGoForward);
		this.InvokeOnUiThreadIfRequired(delegate
		{
			this.SetIsLoading(!args.CanReload);
		});
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000027B1 File Offset: 0x000009B1
	private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
	{
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000027B3 File Offset: 0x000009B3
	private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
	{
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000027B5 File Offset: 0x000009B5
	private void SetCanGoBack(bool canGoBack)
	{
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000027B7 File Offset: 0x000009B7
	private void SetCanGoForward(bool canGoForward)
	{
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000027B9 File Offset: 0x000009B9
	private void SetIsLoading(bool isLoading)
	{
		this.HandleToolStripLayout();
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000027C1 File Offset: 0x000009C1
	public void DisplayOutput(string output)
	{
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000027C3 File Offset: 0x000009C3
	private void HandleToolStripLayout(object sender, LayoutEventArgs e)
	{
		this.HandleToolStripLayout();
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000027CB File Offset: 0x000009CB
	private void HandleToolStripLayout()
	{
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000027D0 File Offset: 0x000009D0
	public void downloadStatus(long id, float percentage, int complete, int error)
	{
		base.Invoke(new MethodInvoker(delegate
		{
			WebBrowserExtensions.ExecuteScriptAsync(this.browser, string.Format("downloadStatus({0},{1},{2},{3});", new object[]
			{
				id.ToString(),
				percentage.ToString(),
				complete.ToString(),
				error.ToString()
			}));
		}));
	}

	// Token: 0x06000022 RID: 34 RVA: 0x0000281A File Offset: 0x00000A1A
	private void ExitMenuItemClick(object sender, EventArgs e)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002821 File Offset: 0x00000A21
	private void GoButtonClick(object sender, EventArgs e)
	{
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002823 File Offset: 0x00000A23
	private void BackButtonClick(object sender, EventArgs e)
	{
		WebBrowserExtensions.Back(this.browser);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002830 File Offset: 0x00000A30
	private void ForwardButtonClick(object sender, EventArgs e)
	{
		WebBrowserExtensions.Forward(this.browser);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x0000283D File Offset: 0x00000A3D
	private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
	{
		Keys keyCode = e.KeyCode;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002849 File Offset: 0x00000A49
	public void LoadUrl(string url)
	{
		if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
		{
			this.browser.Load(url);
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002860 File Offset: 0x00000A60
	private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
	{
		WebBrowserExtensions.ShowDevTools(this.browser, null, 0, 0);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002870 File Offset: 0x00000A70
	private void BrowserForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (Utilities.Singleton().GetMinimizeOnClose() && !this.forceClose)
		{
			base.Hide();
			this.MinimizeToTray();
			e.Cancel = true;
			return;
		}
		APIClient.StopRealtime();
		APIClient.StopRecording();
		AudioService.service.KillRecording();
		ThreadProcessor.Dispose();
		APIClient.ReleaseMemory();
		BrowserForm.browserform = null;
		Program.isActive = false;
		DiscordManager.singleton().Dispose();
		PowerRequester.EnableConstantDisplayAndPower(false);
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000028E2 File Offset: 0x00000AE2
	public void RefreshPage()
	{
		WebBrowserExtensions.Reload(this.browser);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000028F0 File Offset: 0x00000AF0
	public static int CompareVersions(string n, string o)
	{
		string[] array = n.Split(new char[] { '.' });
		string[] array2 = o.Split(new char[] { '.' });
		float[] array3 = new float[Math.Max(array2.Length, array.Length)];
		float[] array4 = new float[array3.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array3[i] = float.Parse("0" + array[i].Trim());
		}
		for (int j = 0; j < array2.Length; j++)
		{
			array4[j] = float.Parse("0" + array2[j].Trim());
		}
		for (int k = 0; k < array3.Length; k++)
		{
			if (array3[k] > array4[k])
			{
				return 1;
			}
			if (array3[k] < array4[k])
			{
				return -1;
			}
		}
		return 0;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000029C4 File Offset: 0x00000BC4
	public bool RunUpdate()
	{
		try
		{
			TimeOutWebClient timeOutWebClient = new TimeOutWebClient(60000, false);
			string text = timeOutWebClient.DownloadString("https://voice.ai/version");
			if (text != null && BrowserForm.CompareVersions(text, VoiceChanger.CurrentVersion) == 1)
			{
				string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				string text2 = Path.Combine(Path.GetTempPath(), "VoiceAIUpdate");
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				string text3 = Path.Combine(text2, "updater.exe");
				timeOutWebClient.Dispose();
				timeOutWebClient = new TimeOutWebClient(60000, false);
				timeOutWebClient.DownloadFile("https://voice.ai/update-download?version=" + VoiceChanger.CurrentVersion, text3);
				Process.Start(new ProcessStartInfo(text3)
				{
					Arguments = "/path \"" + directoryName + "\""
				});
				return true;
			}
		}
		catch
		{
		}
		return false;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002AA8 File Offset: 0x00000CA8
	public static string GetProcessorID()
	{
		string text = "";
		foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor").Get())
		{
			text = (string)((ManagementObject)managementBaseObject)["ProcessorId"];
		}
		return text;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002B14 File Offset: 0x00000D14
	private void button1_customCloseClick(object sender, EventArgs e)
	{
		if (Utilities.Singleton().GetMinimizeOnClose())
		{
			base.Hide();
			this.MinimizeToTray();
			new VCController().releaseMemory();
			return;
		}
		this.browser.Dispose();
		Cef.Shutdown();
		base.Close();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002B4F File Offset: 0x00000D4F
	public void OnAddressChanged(IWebBrowser chromiumWebBrowser, AddressChangedEventArgs addressChangedArgs)
	{
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002B51 File Offset: 0x00000D51
	public bool OnAutoResize(IWebBrowser chromiumWebBrowser, IBrowser browser, Size newSize)
	{
		return false;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002B54 File Offset: 0x00000D54
	public bool OnCursorChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
	{
		return false;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002B57 File Offset: 0x00000D57
	public void OnTitleChanged(IWebBrowser chromiumWebBrowser, TitleChangedEventArgs titleChangedArgs)
	{
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002B59 File Offset: 0x00000D59
	public void OnFaviconUrlChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IList<string> urls)
	{
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002B5B File Offset: 0x00000D5B
	public void OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
	{
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002B5D File Offset: 0x00000D5D
	public void OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress)
	{
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002B5F File Offset: 0x00000D5F
	public bool OnTooltipChanged(IWebBrowser chromiumWebBrowser, ref string text)
	{
		return false;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002B62 File Offset: 0x00000D62
	public void OnStatusMessage(IWebBrowser chromiumWebBrowser, StatusMessageEventArgs statusMessageArgs)
	{
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002B64 File Offset: 0x00000D64
	public bool OnConsoleMessage(IWebBrowser chromiumWebBrowser, ConsoleMessageEventArgs consoleMessageArgs)
	{
		return false;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002B67 File Offset: 0x00000D67
	protected void OnPropertyChanged(string propertyName)
	{
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002B6C File Offset: 0x00000D6C
	public void FireEvent(int code, object message)
	{
		string text = message as string;
		if (text != null)
		{
			WebBrowserExtensions.ExecuteScriptAsync(this.browser, string.Format("hostEvent({0},'{1}');", code.ToString(), text.Replace("\r", "").Replace("\n", "\\n").Replace("'", "\\'")));
			return;
		}
		WebBrowserExtensions.ExecuteScriptAsync(this.browser, string.Format("hostEvent({0},{1});", code.ToString(), message));
	}

	// Token: 0x04000001 RID: 1
	public static BrowserForm browserform;

	// Token: 0x04000002 RID: 2
	public static IntPtr browserHandle;

	// Token: 0x04000003 RID: 3
	private const string Build = "Release";

	// Token: 0x04000004 RID: 4
	private readonly string title = "voice.ai Voice Changer";

	// Token: 0x04000005 RID: 5
	public readonly ChromiumWebBrowser browser;

	// Token: 0x04000006 RID: 6
	private Point mouseDragOffset;

	// Token: 0x04000007 RID: 7
	private NotifyIcon notify;

	// Token: 0x04000008 RID: 8
	private ContextMenu notifyContext;

	// Token: 0x04000009 RID: 9
	private bool forceClose;

	// Token: 0x0400000A RID: 10
	private object _favIcon;

	// Token: 0x02000052 RID: 82
	public struct NCCALCSIZE_PARAMS
	{
		// Token: 0x040001A7 RID: 423
		public BrowserForm.RECT rectProposed;

		// Token: 0x040001A8 RID: 424
		public BrowserForm.RECT rectBeforeMove;

		// Token: 0x040001A9 RID: 425
		public BrowserForm.RECT rectClientBeforeMove;

		// Token: 0x040001AA RID: 426
		public IntPtr lpPos;
	}

	// Token: 0x02000053 RID: 83
	public struct RECT
	{
		// Token: 0x040001AB RID: 427
		public int Left;

		// Token: 0x040001AC RID: 428
		public int Top;

		// Token: 0x040001AD RID: 429
		public int Right;

		// Token: 0x040001AE RID: 430
		public int Bottom;
	}

	// Token: 0x02000054 RID: 84
	private class CustomDragHandler : IDragHandler
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		bool IDragHandler.OnDragEnter(IWebBrowser chromiumWebBrowser, IBrowser browser, IDragData dragData, DragOperationsMask mask)
		{
			return false;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000DD6F File Offset: 0x0000BF6F
		void IDragHandler.OnDraggableRegionsChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IList<DraggableRegion> regions)
		{
		}
	}

	// Token: 0x02000055 RID: 85
	private class CustomMenuHandler : IContextMenuHandler
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x0000DD79 File Offset: 0x0000BF79
		public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
		{
			model.Clear();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000DD83 File Offset: 0x0000BF83
		public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
		{
			return false;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000DD86 File Offset: 0x0000BF86
		public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
		{
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000DD88 File Offset: 0x0000BF88
		public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
		{
			return false;
		}
	}

	// Token: 0x02000056 RID: 86
	private class CustomRequestHandler : IRequestHandler
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000DD93 File Offset: 0x0000BF93
		public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
		{
			if (callback != null)
			{
				callback.Dispose();
			}
			return false;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000DDA1 File Offset: 0x0000BFA1
		public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
		{
			return null;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
		public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
		{
			return request != null && request.Url != null && request.Url.StartsWith("file://");
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DDC9 File Offset: 0x0000BFC9
		public bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
		{
			if (callback != null)
			{
				callback.Dispose();
			}
			return false;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DDD7 File Offset: 0x0000BFD7
		public void OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser)
		{
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DDD9 File Offset: 0x0000BFD9
		public bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
		{
			return false;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		public void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
		{
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000DDDE File Offset: 0x0000BFDE
		public bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
		{
			return false;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000DDE1 File Offset: 0x0000BFE1
		public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
		{
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000DDE3 File Offset: 0x0000BFE3
		public void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
		{
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000DDE5 File Offset: 0x0000BFE5
		public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
		{
			return false;
		}
	}
}
