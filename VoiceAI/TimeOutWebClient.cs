using System;
using System.Net;

// Token: 0x02000014 RID: 20
public class TimeOutWebClient : WebClient
{
	// Token: 0x06000148 RID: 328 RVA: 0x0000848C File Offset: 0x0000668C
	public TimeOutWebClient(int TimeOutInMilliSeconds, bool decompress = false)
	{
		this.timeout = TimeOutInMilliSeconds;
		this.decompress = decompress;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x000084A2 File Offset: 0x000066A2
	public TimeOutWebClient()
	{
		this.timeout = 5000;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x000084B5 File Offset: 0x000066B5
	public void UseProxy(bool useProxy)
	{
		this.useProxy = useProxy;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x000084C0 File Offset: 0x000066C0
	protected override WebRequest GetWebRequest(Uri uri)
	{
		HttpWebRequest httpWebRequest = (HttpWebRequest)base.GetWebRequest(uri);
		if (this.decompress)
		{
			httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
		}
		if (!this.useProxy)
		{
			httpWebRequest.Proxy = null;
		}
		httpWebRequest.Timeout = this.timeout;
		return httpWebRequest;
	}

	// Token: 0x0400007D RID: 125
	private int timeout;

	// Token: 0x0400007E RID: 126
	private bool useProxy;

	// Token: 0x0400007F RID: 127
	private bool decompress;
}
