using System;
using CefSharp;

// Token: 0x02000003 RID: 3
public class DownloadHandler : IDownloadHandler
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600003E RID: 62 RVA: 0x0000305C File Offset: 0x0000125C
	// (remove) Token: 0x0600003F RID: 63 RVA: 0x00003094 File Offset: 0x00001294
	public event EventHandler<DownloadItem> OnBeforeDownloadFired;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000040 RID: 64 RVA: 0x000030CC File Offset: 0x000012CC
	// (remove) Token: 0x06000041 RID: 65 RVA: 0x00003104 File Offset: 0x00001304
	public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

	// Token: 0x06000042 RID: 66 RVA: 0x0000313C File Offset: 0x0000133C
	public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
	{
		EventHandler<DownloadItem> onBeforeDownloadFired = this.OnBeforeDownloadFired;
		if (onBeforeDownloadFired != null)
		{
			onBeforeDownloadFired(this, downloadItem);
		}
		if (!callback.IsDisposed)
		{
			try
			{
				callback.Continue(downloadItem.SuggestedFileName, true);
			}
			finally
			{
				if (callback != null)
				{
					callback.Dispose();
				}
			}
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003194 File Offset: 0x00001394
	public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
	{
		EventHandler<DownloadItem> onDownloadUpdatedFired = this.OnDownloadUpdatedFired;
		if (onDownloadUpdatedFired == null)
		{
			return;
		}
		onDownloadUpdatedFired(this, downloadItem);
	}
}
