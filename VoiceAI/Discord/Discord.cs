using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000044 RID: 68
	public class Discord : IDisposable
	{
		// Token: 0x060001FC RID: 508
		[DllImport("discord_game_sdk", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
		private static extern Result DiscordCreate(uint version, ref Discord.FFICreateParams createParams, out IntPtr manager);

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000A3D2 File Offset: 0x000085D2
		private Discord.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(Discord.FFIMethods));
				}
				return (Discord.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A404 File Offset: 0x00008604
		public Discord(long clientId, ulong flags)
		{
			Discord.FFICreateParams fficreateParams;
			fficreateParams.ClientId = clientId;
			fficreateParams.Flags = flags;
			this.Events = default(Discord.FFIEvents);
			this.EventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Discord.FFIEvents>(this.Events));
			fficreateParams.Events = this.EventsPtr;
			this.SelfHandle = GCHandle.Alloc(this);
			fficreateParams.EventData = GCHandle.ToIntPtr(this.SelfHandle);
			this.ApplicationEvents = default(ApplicationManager.FFIEvents);
			this.ApplicationEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ApplicationManager.FFIEvents>(this.ApplicationEvents));
			fficreateParams.ApplicationEvents = this.ApplicationEventsPtr;
			fficreateParams.ApplicationVersion = 1U;
			this.UserEvents = default(UserManager.FFIEvents);
			this.UserEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<UserManager.FFIEvents>(this.UserEvents));
			fficreateParams.UserEvents = this.UserEventsPtr;
			fficreateParams.UserVersion = 1U;
			this.ImageEvents = default(ImageManager.FFIEvents);
			this.ImageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ImageManager.FFIEvents>(this.ImageEvents));
			fficreateParams.ImageEvents = this.ImageEventsPtr;
			fficreateParams.ImageVersion = 1U;
			this.ActivityEvents = default(ActivityManager.FFIEvents);
			this.ActivityEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ActivityManager.FFIEvents>(this.ActivityEvents));
			fficreateParams.ActivityEvents = this.ActivityEventsPtr;
			fficreateParams.ActivityVersion = 1U;
			this.RelationshipEvents = default(RelationshipManager.FFIEvents);
			this.RelationshipEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<RelationshipManager.FFIEvents>(this.RelationshipEvents));
			fficreateParams.RelationshipEvents = this.RelationshipEventsPtr;
			fficreateParams.RelationshipVersion = 1U;
			this.LobbyEvents = default(LobbyManager.FFIEvents);
			this.LobbyEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<LobbyManager.FFIEvents>(this.LobbyEvents));
			fficreateParams.LobbyEvents = this.LobbyEventsPtr;
			fficreateParams.LobbyVersion = 1U;
			this.NetworkEvents = default(NetworkManager.FFIEvents);
			this.NetworkEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NetworkManager.FFIEvents>(this.NetworkEvents));
			fficreateParams.NetworkEvents = this.NetworkEventsPtr;
			fficreateParams.NetworkVersion = 1U;
			this.OverlayEvents = default(OverlayManager.FFIEvents);
			this.OverlayEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<OverlayManager.FFIEvents>(this.OverlayEvents));
			fficreateParams.OverlayEvents = this.OverlayEventsPtr;
			fficreateParams.OverlayVersion = 1U;
			this.StorageEvents = default(StorageManager.FFIEvents);
			this.StorageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<StorageManager.FFIEvents>(this.StorageEvents));
			fficreateParams.StorageEvents = this.StorageEventsPtr;
			fficreateParams.StorageVersion = 1U;
			this.StoreEvents = default(StoreManager.FFIEvents);
			this.StoreEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<StoreManager.FFIEvents>(this.StoreEvents));
			fficreateParams.StoreEvents = this.StoreEventsPtr;
			fficreateParams.StoreVersion = 1U;
			this.VoiceEvents = default(VoiceManager.FFIEvents);
			this.VoiceEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<VoiceManager.FFIEvents>(this.VoiceEvents));
			fficreateParams.VoiceEvents = this.VoiceEventsPtr;
			fficreateParams.VoiceVersion = 1U;
			this.AchievementEvents = default(AchievementManager.FFIEvents);
			this.AchievementEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<AchievementManager.FFIEvents>(this.AchievementEvents));
			fficreateParams.AchievementEvents = this.AchievementEventsPtr;
			fficreateParams.AchievementVersion = 1U;
			this.InitEvents(this.EventsPtr, ref this.Events);
			Result result = Discord.DiscordCreate(2U, ref fficreateParams, out this.MethodsPtr);
			if (result != Result.Ok)
			{
				this.Dispose();
				throw new ResultException(result);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A739 File Offset: 0x00008939
		private void InitEvents(IntPtr eventsPtr, ref Discord.FFIEvents events)
		{
			Marshal.StructureToPtr<Discord.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A748 File Offset: 0x00008948
		public void Dispose()
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				this.Methods.Destroy(this.MethodsPtr);
			}
			this.SelfHandle.Free();
			Marshal.FreeHGlobal(this.EventsPtr);
			Marshal.FreeHGlobal(this.ApplicationEventsPtr);
			Marshal.FreeHGlobal(this.UserEventsPtr);
			Marshal.FreeHGlobal(this.ImageEventsPtr);
			Marshal.FreeHGlobal(this.ActivityEventsPtr);
			Marshal.FreeHGlobal(this.RelationshipEventsPtr);
			Marshal.FreeHGlobal(this.LobbyEventsPtr);
			Marshal.FreeHGlobal(this.NetworkEventsPtr);
			Marshal.FreeHGlobal(this.OverlayEventsPtr);
			Marshal.FreeHGlobal(this.StorageEventsPtr);
			Marshal.FreeHGlobal(this.StoreEventsPtr);
			Marshal.FreeHGlobal(this.VoiceEventsPtr);
			Marshal.FreeHGlobal(this.AchievementEventsPtr);
			if (this.setLogHook != null)
			{
				this.setLogHook.Value.Free();
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A838 File Offset: 0x00008A38
		public void RunCallbacks()
		{
			Result result = this.Methods.RunCallbacks(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A868 File Offset: 0x00008A68
		[MonoPInvokeCallback]
		private static void SetLogHookCallbackImpl(IntPtr ptr, LogLevel level, string message)
		{
			((Discord.SetLogHookHandler)GCHandle.FromIntPtr(ptr).Target)(level, message);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A890 File Offset: 0x00008A90
		public void SetLogHook(LogLevel minLevel, Discord.SetLogHookHandler callback)
		{
			if (this.setLogHook != null)
			{
				this.setLogHook.Value.Free();
			}
			this.setLogHook = new GCHandle?(GCHandle.Alloc(callback));
			this.Methods.SetLogHook(this.MethodsPtr, minLevel, GCHandle.ToIntPtr(this.setLogHook.Value), new Discord.FFIMethods.SetLogHookCallback(Discord.SetLogHookCallbackImpl));
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A901 File Offset: 0x00008B01
		public ApplicationManager GetApplicationManager()
		{
			if (this.ApplicationManagerInstance == null)
			{
				this.ApplicationManagerInstance = new ApplicationManager(this.Methods.GetApplicationManager(this.MethodsPtr), this.ApplicationEventsPtr, ref this.ApplicationEvents);
			}
			return this.ApplicationManagerInstance;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A93E File Offset: 0x00008B3E
		public UserManager GetUserManager()
		{
			if (this.UserManagerInstance == null)
			{
				this.UserManagerInstance = new UserManager(this.Methods.GetUserManager(this.MethodsPtr), this.UserEventsPtr, ref this.UserEvents);
			}
			return this.UserManagerInstance;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A97B File Offset: 0x00008B7B
		public ImageManager GetImageManager()
		{
			if (this.ImageManagerInstance == null)
			{
				this.ImageManagerInstance = new ImageManager(this.Methods.GetImageManager(this.MethodsPtr), this.ImageEventsPtr, ref this.ImageEvents);
			}
			return this.ImageManagerInstance;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		public ActivityManager GetActivityManager()
		{
			if (this.ActivityManagerInstance == null)
			{
				this.ActivityManagerInstance = new ActivityManager(this.Methods.GetActivityManager(this.MethodsPtr), this.ActivityEventsPtr, ref this.ActivityEvents);
			}
			return this.ActivityManagerInstance;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000A9F5 File Offset: 0x00008BF5
		public RelationshipManager GetRelationshipManager()
		{
			if (this.RelationshipManagerInstance == null)
			{
				this.RelationshipManagerInstance = new RelationshipManager(this.Methods.GetRelationshipManager(this.MethodsPtr), this.RelationshipEventsPtr, ref this.RelationshipEvents);
			}
			return this.RelationshipManagerInstance;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000AA32 File Offset: 0x00008C32
		public LobbyManager GetLobbyManager()
		{
			if (this.LobbyManagerInstance == null)
			{
				this.LobbyManagerInstance = new LobbyManager(this.Methods.GetLobbyManager(this.MethodsPtr), this.LobbyEventsPtr, ref this.LobbyEvents);
			}
			return this.LobbyManagerInstance;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000AA6F File Offset: 0x00008C6F
		public NetworkManager GetNetworkManager()
		{
			if (this.NetworkManagerInstance == null)
			{
				this.NetworkManagerInstance = new NetworkManager(this.Methods.GetNetworkManager(this.MethodsPtr), this.NetworkEventsPtr, ref this.NetworkEvents);
			}
			return this.NetworkManagerInstance;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000AAAC File Offset: 0x00008CAC
		public OverlayManager GetOverlayManager()
		{
			if (this.OverlayManagerInstance == null)
			{
				this.OverlayManagerInstance = new OverlayManager(this.Methods.GetOverlayManager(this.MethodsPtr), this.OverlayEventsPtr, ref this.OverlayEvents);
			}
			return this.OverlayManagerInstance;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000AAE9 File Offset: 0x00008CE9
		public StorageManager GetStorageManager()
		{
			if (this.StorageManagerInstance == null)
			{
				this.StorageManagerInstance = new StorageManager(this.Methods.GetStorageManager(this.MethodsPtr), this.StorageEventsPtr, ref this.StorageEvents);
			}
			return this.StorageManagerInstance;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000AB26 File Offset: 0x00008D26
		public StoreManager GetStoreManager()
		{
			if (this.StoreManagerInstance == null)
			{
				this.StoreManagerInstance = new StoreManager(this.Methods.GetStoreManager(this.MethodsPtr), this.StoreEventsPtr, ref this.StoreEvents);
			}
			return this.StoreManagerInstance;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000AB63 File Offset: 0x00008D63
		public VoiceManager GetVoiceManager()
		{
			if (this.VoiceManagerInstance == null)
			{
				this.VoiceManagerInstance = new VoiceManager(this.Methods.GetVoiceManager(this.MethodsPtr), this.VoiceEventsPtr, ref this.VoiceEvents);
			}
			return this.VoiceManagerInstance;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		public AchievementManager GetAchievementManager()
		{
			if (this.AchievementManagerInstance == null)
			{
				this.AchievementManagerInstance = new AchievementManager(this.Methods.GetAchievementManager(this.MethodsPtr), this.AchievementEventsPtr, ref this.AchievementEvents);
			}
			return this.AchievementManagerInstance;
		}

		// Token: 0x04000154 RID: 340
		private GCHandle SelfHandle;

		// Token: 0x04000155 RID: 341
		private IntPtr EventsPtr;

		// Token: 0x04000156 RID: 342
		private Discord.FFIEvents Events;

		// Token: 0x04000157 RID: 343
		private IntPtr ApplicationEventsPtr;

		// Token: 0x04000158 RID: 344
		private ApplicationManager.FFIEvents ApplicationEvents;

		// Token: 0x04000159 RID: 345
		internal ApplicationManager ApplicationManagerInstance;

		// Token: 0x0400015A RID: 346
		private IntPtr UserEventsPtr;

		// Token: 0x0400015B RID: 347
		private UserManager.FFIEvents UserEvents;

		// Token: 0x0400015C RID: 348
		internal UserManager UserManagerInstance;

		// Token: 0x0400015D RID: 349
		private IntPtr ImageEventsPtr;

		// Token: 0x0400015E RID: 350
		private ImageManager.FFIEvents ImageEvents;

		// Token: 0x0400015F RID: 351
		internal ImageManager ImageManagerInstance;

		// Token: 0x04000160 RID: 352
		private IntPtr ActivityEventsPtr;

		// Token: 0x04000161 RID: 353
		private ActivityManager.FFIEvents ActivityEvents;

		// Token: 0x04000162 RID: 354
		internal ActivityManager ActivityManagerInstance;

		// Token: 0x04000163 RID: 355
		private IntPtr RelationshipEventsPtr;

		// Token: 0x04000164 RID: 356
		private RelationshipManager.FFIEvents RelationshipEvents;

		// Token: 0x04000165 RID: 357
		internal RelationshipManager RelationshipManagerInstance;

		// Token: 0x04000166 RID: 358
		private IntPtr LobbyEventsPtr;

		// Token: 0x04000167 RID: 359
		private LobbyManager.FFIEvents LobbyEvents;

		// Token: 0x04000168 RID: 360
		internal LobbyManager LobbyManagerInstance;

		// Token: 0x04000169 RID: 361
		private IntPtr NetworkEventsPtr;

		// Token: 0x0400016A RID: 362
		private NetworkManager.FFIEvents NetworkEvents;

		// Token: 0x0400016B RID: 363
		internal NetworkManager NetworkManagerInstance;

		// Token: 0x0400016C RID: 364
		private IntPtr OverlayEventsPtr;

		// Token: 0x0400016D RID: 365
		private OverlayManager.FFIEvents OverlayEvents;

		// Token: 0x0400016E RID: 366
		internal OverlayManager OverlayManagerInstance;

		// Token: 0x0400016F RID: 367
		private IntPtr StorageEventsPtr;

		// Token: 0x04000170 RID: 368
		private StorageManager.FFIEvents StorageEvents;

		// Token: 0x04000171 RID: 369
		internal StorageManager StorageManagerInstance;

		// Token: 0x04000172 RID: 370
		private IntPtr StoreEventsPtr;

		// Token: 0x04000173 RID: 371
		private StoreManager.FFIEvents StoreEvents;

		// Token: 0x04000174 RID: 372
		internal StoreManager StoreManagerInstance;

		// Token: 0x04000175 RID: 373
		private IntPtr VoiceEventsPtr;

		// Token: 0x04000176 RID: 374
		private VoiceManager.FFIEvents VoiceEvents;

		// Token: 0x04000177 RID: 375
		internal VoiceManager VoiceManagerInstance;

		// Token: 0x04000178 RID: 376
		private IntPtr AchievementEventsPtr;

		// Token: 0x04000179 RID: 377
		private AchievementManager.FFIEvents AchievementEvents;

		// Token: 0x0400017A RID: 378
		internal AchievementManager AchievementManagerInstance;

		// Token: 0x0400017B RID: 379
		private IntPtr MethodsPtr;

		// Token: 0x0400017C RID: 380
		private object MethodsStructure;

		// Token: 0x0400017D RID: 381
		private GCHandle? setLogHook;

		// Token: 0x02000081 RID: 129
		internal struct FFIEvents
		{
		}

		// Token: 0x02000082 RID: 130
		internal struct FFIMethods
		{
			// Token: 0x04000229 RID: 553
			internal Discord.FFIMethods.DestroyHandler Destroy;

			// Token: 0x0400022A RID: 554
			internal Discord.FFIMethods.RunCallbacksMethod RunCallbacks;

			// Token: 0x0400022B RID: 555
			internal Discord.FFIMethods.SetLogHookMethod SetLogHook;

			// Token: 0x0400022C RID: 556
			internal Discord.FFIMethods.GetApplicationManagerMethod GetApplicationManager;

			// Token: 0x0400022D RID: 557
			internal Discord.FFIMethods.GetUserManagerMethod GetUserManager;

			// Token: 0x0400022E RID: 558
			internal Discord.FFIMethods.GetImageManagerMethod GetImageManager;

			// Token: 0x0400022F RID: 559
			internal Discord.FFIMethods.GetActivityManagerMethod GetActivityManager;

			// Token: 0x04000230 RID: 560
			internal Discord.FFIMethods.GetRelationshipManagerMethod GetRelationshipManager;

			// Token: 0x04000231 RID: 561
			internal Discord.FFIMethods.GetLobbyManagerMethod GetLobbyManager;

			// Token: 0x04000232 RID: 562
			internal Discord.FFIMethods.GetNetworkManagerMethod GetNetworkManager;

			// Token: 0x04000233 RID: 563
			internal Discord.FFIMethods.GetOverlayManagerMethod GetOverlayManager;

			// Token: 0x04000234 RID: 564
			internal Discord.FFIMethods.GetStorageManagerMethod GetStorageManager;

			// Token: 0x04000235 RID: 565
			internal Discord.FFIMethods.GetStoreManagerMethod GetStoreManager;

			// Token: 0x04000236 RID: 566
			internal Discord.FFIMethods.GetVoiceManagerMethod GetVoiceManager;

			// Token: 0x04000237 RID: 567
			internal Discord.FFIMethods.GetAchievementManagerMethod GetAchievementManager;

			// Token: 0x020000E8 RID: 232
			// (Invoke) Token: 0x0600048B RID: 1163
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DestroyHandler(IntPtr MethodsPtr);

			// Token: 0x020000E9 RID: 233
			// (Invoke) Token: 0x0600048F RID: 1167
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RunCallbacksMethod(IntPtr methodsPtr);

			// Token: 0x020000EA RID: 234
			// (Invoke) Token: 0x06000493 RID: 1171
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookCallback(IntPtr ptr, LogLevel level, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x020000EB RID: 235
			// (Invoke) Token: 0x06000497 RID: 1175
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookMethod(IntPtr methodsPtr, LogLevel minLevel, IntPtr callbackData, Discord.FFIMethods.SetLogHookCallback callback);

			// Token: 0x020000EC RID: 236
			// (Invoke) Token: 0x0600049B RID: 1179
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetApplicationManagerMethod(IntPtr discordPtr);

			// Token: 0x020000ED RID: 237
			// (Invoke) Token: 0x0600049F RID: 1183
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetUserManagerMethod(IntPtr discordPtr);

			// Token: 0x020000EE RID: 238
			// (Invoke) Token: 0x060004A3 RID: 1187
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetImageManagerMethod(IntPtr discordPtr);

			// Token: 0x020000EF RID: 239
			// (Invoke) Token: 0x060004A7 RID: 1191
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetActivityManagerMethod(IntPtr discordPtr);

			// Token: 0x020000F0 RID: 240
			// (Invoke) Token: 0x060004AB RID: 1195
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetRelationshipManagerMethod(IntPtr discordPtr);

			// Token: 0x020000F1 RID: 241
			// (Invoke) Token: 0x060004AF RID: 1199
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetLobbyManagerMethod(IntPtr discordPtr);

			// Token: 0x020000F2 RID: 242
			// (Invoke) Token: 0x060004B3 RID: 1203
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetNetworkManagerMethod(IntPtr discordPtr);

			// Token: 0x020000F3 RID: 243
			// (Invoke) Token: 0x060004B7 RID: 1207
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetOverlayManagerMethod(IntPtr discordPtr);

			// Token: 0x020000F4 RID: 244
			// (Invoke) Token: 0x060004BB RID: 1211
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStorageManagerMethod(IntPtr discordPtr);

			// Token: 0x020000F5 RID: 245
			// (Invoke) Token: 0x060004BF RID: 1215
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStoreManagerMethod(IntPtr discordPtr);

			// Token: 0x020000F6 RID: 246
			// (Invoke) Token: 0x060004C3 RID: 1219
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetVoiceManagerMethod(IntPtr discordPtr);

			// Token: 0x020000F7 RID: 247
			// (Invoke) Token: 0x060004C7 RID: 1223
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetAchievementManagerMethod(IntPtr discordPtr);
		}

		// Token: 0x02000083 RID: 131
		internal struct FFICreateParams
		{
			// Token: 0x04000238 RID: 568
			internal long ClientId;

			// Token: 0x04000239 RID: 569
			internal ulong Flags;

			// Token: 0x0400023A RID: 570
			internal IntPtr Events;

			// Token: 0x0400023B RID: 571
			internal IntPtr EventData;

			// Token: 0x0400023C RID: 572
			internal IntPtr ApplicationEvents;

			// Token: 0x0400023D RID: 573
			internal uint ApplicationVersion;

			// Token: 0x0400023E RID: 574
			internal IntPtr UserEvents;

			// Token: 0x0400023F RID: 575
			internal uint UserVersion;

			// Token: 0x04000240 RID: 576
			internal IntPtr ImageEvents;

			// Token: 0x04000241 RID: 577
			internal uint ImageVersion;

			// Token: 0x04000242 RID: 578
			internal IntPtr ActivityEvents;

			// Token: 0x04000243 RID: 579
			internal uint ActivityVersion;

			// Token: 0x04000244 RID: 580
			internal IntPtr RelationshipEvents;

			// Token: 0x04000245 RID: 581
			internal uint RelationshipVersion;

			// Token: 0x04000246 RID: 582
			internal IntPtr LobbyEvents;

			// Token: 0x04000247 RID: 583
			internal uint LobbyVersion;

			// Token: 0x04000248 RID: 584
			internal IntPtr NetworkEvents;

			// Token: 0x04000249 RID: 585
			internal uint NetworkVersion;

			// Token: 0x0400024A RID: 586
			internal IntPtr OverlayEvents;

			// Token: 0x0400024B RID: 587
			internal uint OverlayVersion;

			// Token: 0x0400024C RID: 588
			internal IntPtr StorageEvents;

			// Token: 0x0400024D RID: 589
			internal uint StorageVersion;

			// Token: 0x0400024E RID: 590
			internal IntPtr StoreEvents;

			// Token: 0x0400024F RID: 591
			internal uint StoreVersion;

			// Token: 0x04000250 RID: 592
			internal IntPtr VoiceEvents;

			// Token: 0x04000251 RID: 593
			internal uint VoiceVersion;

			// Token: 0x04000252 RID: 594
			internal IntPtr AchievementEvents;

			// Token: 0x04000253 RID: 595
			internal uint AchievementVersion;
		}

		// Token: 0x02000084 RID: 132
		// (Invoke) Token: 0x06000357 RID: 855
		public delegate void SetLogHookHandler(LogLevel level, string message);
	}
}
