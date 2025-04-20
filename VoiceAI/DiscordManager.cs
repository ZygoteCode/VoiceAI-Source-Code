using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using Discord;

// Token: 0x0200000C RID: 12
internal class DiscordManager
{
	// Token: 0x06000118 RID: 280 RVA: 0x000077E2 File Offset: 0x000059E2
	private DiscordManager()
	{
	}

	// Token: 0x06000119 RID: 281 RVA: 0x000077EC File Offset: 0x000059EC
	private void DiscordLoop(object state)
	{
		this.process = Process.Start(new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", ""))
		{
			Arguments = "discord " + Process.GetCurrentProcess().Id.ToString(),
			UseShellExecute = false,
			RedirectStandardOutput = true
		});
		this.process.BeginOutputReadLine();
		this.process.OutputDataReceived += this.Process_OutputDataReceived;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00007878 File Offset: 0x00005A78
	private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (e != null && e.Data != null)
		{
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			javaScriptSerializer.MaxJsonLength = int.MaxValue;
			try
			{
				Dictionary<string, string> dictionary = javaScriptSerializer.Deserialize<Dictionary<string, string>>(e.Data);
				if (dictionary != null)
				{
					VoiceChanger.singleton(0L).SetDUser(dictionary);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000078D4 File Offset: 0x00005AD4
	public void ActivateMainDiscord()
	{
		this.discordThread = new Thread(new ParameterizedThreadStart(this.DiscordLoop));
		this.discordThread.Start();
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000078F8 File Offset: 0x00005AF8
	public static DiscordManager singleton()
	{
		if (DiscordManager.dsmc == null)
		{
			DiscordManager.dsmc = new DiscordManager();
		}
		return DiscordManager.dsmc;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00007910 File Offset: 0x00005B10
	public void Dispose()
	{
		try
		{
			if (this.process != null)
			{
				this.process.Kill();
			}
		}
		catch
		{
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00007948 File Offset: 0x00005B48
	public void ActivateDiscord(string pid)
	{
		Process processById = Process.GetProcessById(int.Parse(pid));
		if (processById == null)
		{
			return;
		}
		Program.isActive = true;
		Thread thread = new Thread(new ParameterizedThreadStart(this.RunDiscordThread));
		thread.Start();
		while (processById.Responding && !processById.HasExited && !processById.WaitForExit(1000))
		{
		}
		Program.isActive = false;
		thread.Join();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x000079B0 File Offset: 0x00005BB0
	private void ConnectToDiscord()
	{
		try
		{
			this.discord = new Discord(967103745152413697L, 1UL);
			this.activityManager = this.discord.GetActivityManager();
			this.lobbyManager = this.discord.GetLobbyManager();
			this.userManager = this.discord.GetUserManager();
			this.userManager.OnCurrentUserUpdate += this.UserManager_OnCurrentUserUpdate;
			LobbyTransaction lobbyCreateTransaction = this.lobbyManager.GetLobbyCreateTransaction();
			lobbyCreateTransaction.SetCapacity(6U);
			lobbyCreateTransaction.SetType(LobbyType.Public);
			this.lobbyManager.CreateLobby(lobbyCreateTransaction, new LobbyManager.CreateLobbyHandler(this.LobbyCreated));
		}
		catch (Exception)
		{
			Thread.Sleep(1000);
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00007A70 File Offset: 0x00005C70
	private void UserManager_OnCurrentUserUpdate()
	{
		User currentUser = this.userManager.GetCurrentUser();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("discriminator", currentUser.Discriminator);
		dictionary.Add("id", currentUser.Id.ToString());
		dictionary.Add("username", currentUser.Username);
		dictionary.Add("avatar", currentUser.Avatar);
		dictionary.Add("bot", currentUser.Bot ? "1" : "0");
		Console.WriteLine(new JavaScriptSerializer
		{
			MaxJsonLength = int.MaxValue
		}.Serialize(dictionary));
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00007B13 File Offset: 0x00005D13
	private void LobbyCreated(Result result, ref Lobby lobby)
	{
		if (result != Result.Ok)
		{
			return;
		}
		this.UpdateActivity(this.discord, lobby);
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00007B2C File Offset: 0x00005D2C
	private void RunDiscordThread(object state)
	{
		try
		{
			while (Program.isActive)
			{
				if (this.discord != null)
				{
					try
					{
						this.discord.RunCallbacks();
						this.lobbyManager.FlushNetwork();
					}
					catch (Exception)
					{
						this.discord = null;
					}
					Thread.Sleep(100);
				}
				else
				{
					this.ConnectToDiscord();
					Thread.Sleep(1);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00007BA4 File Offset: 0x00005DA4
	private void UpdateActivity(Discord discord, Lobby lobby)
	{
		try
		{
			this.activityManager = discord.GetActivityManager();
			this.lobbyManager = discord.GetLobbyManager();
			Activity activity = default(Activity);
			activity.Type = ActivityType.Playing;
			activity.State = "Changing Voice";
			activity.Details = "Changing their voice";
			activity.Assets.LargeImage = "voice_ai__icon__03";
			activity.Assets.LargeText = "App Icon";
			activity.Assets.SmallImage = "voice_ai__icon__03";
			activity.Assets.SmallText = "App Icon";
			activity.Instance = true;
			Activity activity2 = activity;
			this.activityManager.UpdateActivity(activity2, delegate(Result result)
			{
			});
		}
		catch
		{
		}
	}

	// Token: 0x0400005A RID: 90
	private LobbyManager lobbyManager;

	// Token: 0x0400005B RID: 91
	private Discord discord;

	// Token: 0x0400005C RID: 92
	private ActivityManager activityManager;

	// Token: 0x0400005D RID: 93
	private UserManager userManager;

	// Token: 0x0400005E RID: 94
	private Process process;

	// Token: 0x0400005F RID: 95
	private Thread discordThread;

	// Token: 0x04000060 RID: 96
	private static DiscordManager dsmc;
}
