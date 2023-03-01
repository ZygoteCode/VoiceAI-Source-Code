using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x0200004A RID: 74
	public class LobbyManager
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000B56B File Offset: 0x0000976B
		private LobbyManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyManager.FFIMethods));
				}
				return (LobbyManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600023F RID: 575 RVA: 0x0000B59C File Offset: 0x0000979C
		// (remove) Token: 0x06000240 RID: 576 RVA: 0x0000B5D4 File Offset: 0x000097D4
		public event LobbyManager.LobbyUpdateHandler OnLobbyUpdate;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000241 RID: 577 RVA: 0x0000B60C File Offset: 0x0000980C
		// (remove) Token: 0x06000242 RID: 578 RVA: 0x0000B644 File Offset: 0x00009844
		public event LobbyManager.LobbyDeleteHandler OnLobbyDelete;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000243 RID: 579 RVA: 0x0000B67C File Offset: 0x0000987C
		// (remove) Token: 0x06000244 RID: 580 RVA: 0x0000B6B4 File Offset: 0x000098B4
		public event LobbyManager.MemberConnectHandler OnMemberConnect;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000245 RID: 581 RVA: 0x0000B6EC File Offset: 0x000098EC
		// (remove) Token: 0x06000246 RID: 582 RVA: 0x0000B724 File Offset: 0x00009924
		public event LobbyManager.MemberUpdateHandler OnMemberUpdate;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000247 RID: 583 RVA: 0x0000B75C File Offset: 0x0000995C
		// (remove) Token: 0x06000248 RID: 584 RVA: 0x0000B794 File Offset: 0x00009994
		public event LobbyManager.MemberDisconnectHandler OnMemberDisconnect;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000249 RID: 585 RVA: 0x0000B7CC File Offset: 0x000099CC
		// (remove) Token: 0x0600024A RID: 586 RVA: 0x0000B804 File Offset: 0x00009A04
		public event LobbyManager.LobbyMessageHandler OnLobbyMessage;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600024B RID: 587 RVA: 0x0000B83C File Offset: 0x00009A3C
		// (remove) Token: 0x0600024C RID: 588 RVA: 0x0000B874 File Offset: 0x00009A74
		public event LobbyManager.SpeakingHandler OnSpeaking;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600024D RID: 589 RVA: 0x0000B8AC File Offset: 0x00009AAC
		// (remove) Token: 0x0600024E RID: 590 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		public event LobbyManager.NetworkMessageHandler OnNetworkMessage;

		// Token: 0x0600024F RID: 591 RVA: 0x0000B91C File Offset: 0x00009B1C
		internal LobbyManager(IntPtr ptr, IntPtr eventsPtr, ref LobbyManager.FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
			this.InitEvents(eventsPtr, ref events);
			this.MethodsPtr = ptr;
			if (this.MethodsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000B96C File Offset: 0x00009B6C
		private void InitEvents(IntPtr eventsPtr, ref LobbyManager.FFIEvents events)
		{
			events.OnLobbyUpdate = new LobbyManager.FFIEvents.LobbyUpdateHandler(LobbyManager.OnLobbyUpdateImpl);
			events.OnLobbyDelete = new LobbyManager.FFIEvents.LobbyDeleteHandler(LobbyManager.OnLobbyDeleteImpl);
			events.OnMemberConnect = new LobbyManager.FFIEvents.MemberConnectHandler(LobbyManager.OnMemberConnectImpl);
			events.OnMemberUpdate = new LobbyManager.FFIEvents.MemberUpdateHandler(LobbyManager.OnMemberUpdateImpl);
			events.OnMemberDisconnect = new LobbyManager.FFIEvents.MemberDisconnectHandler(LobbyManager.OnMemberDisconnectImpl);
			events.OnLobbyMessage = new LobbyManager.FFIEvents.LobbyMessageHandler(LobbyManager.OnLobbyMessageImpl);
			events.OnSpeaking = new LobbyManager.FFIEvents.SpeakingHandler(LobbyManager.OnSpeakingImpl);
			events.OnNetworkMessage = new LobbyManager.FFIEvents.NetworkMessageHandler(LobbyManager.OnNetworkMessageImpl);
			Marshal.StructureToPtr<LobbyManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000BA18 File Offset: 0x00009C18
		public LobbyTransaction GetLobbyCreateTransaction()
		{
			LobbyTransaction lobbyTransaction = default(LobbyTransaction);
			Result result = this.Methods.GetLobbyCreateTransaction(this.MethodsPtr, ref lobbyTransaction.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobbyTransaction;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000BA58 File Offset: 0x00009C58
		public LobbyTransaction GetLobbyUpdateTransaction(long lobbyId)
		{
			LobbyTransaction lobbyTransaction = default(LobbyTransaction);
			Result result = this.Methods.GetLobbyUpdateTransaction(this.MethodsPtr, lobbyId, ref lobbyTransaction.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobbyTransaction;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000BA98 File Offset: 0x00009C98
		public LobbyMemberTransaction GetMemberUpdateTransaction(long lobbyId, long userId)
		{
			LobbyMemberTransaction lobbyMemberTransaction = default(LobbyMemberTransaction);
			Result result = this.Methods.GetMemberUpdateTransaction(this.MethodsPtr, lobbyId, userId, ref lobbyMemberTransaction.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobbyMemberTransaction;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000BAD8 File Offset: 0x00009CD8
		[MonoPInvokeCallback]
		private static void CreateLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.CreateLobbyHandler createLobbyHandler = (LobbyManager.CreateLobbyHandler)gchandle.Target;
			gchandle.Free();
			createLobbyHandler(result, ref lobby);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000BB08 File Offset: 0x00009D08
		public void CreateLobby(LobbyTransaction transaction, LobbyManager.CreateLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.CreateLobby(this.MethodsPtr, transaction.MethodsPtr, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.CreateLobbyCallback(LobbyManager.CreateLobbyCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000BB58 File Offset: 0x00009D58
		[MonoPInvokeCallback]
		private static void UpdateLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.UpdateLobbyHandler updateLobbyHandler = (LobbyManager.UpdateLobbyHandler)gchandle.Target;
			gchandle.Free();
			updateLobbyHandler(result);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000BB88 File Offset: 0x00009D88
		public void UpdateLobby(long lobbyId, LobbyTransaction transaction, LobbyManager.UpdateLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.UpdateLobby(this.MethodsPtr, lobbyId, transaction.MethodsPtr, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.UpdateLobbyCallback(LobbyManager.UpdateLobbyCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000BBD8 File Offset: 0x00009DD8
		[MonoPInvokeCallback]
		private static void DeleteLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DeleteLobbyHandler deleteLobbyHandler = (LobbyManager.DeleteLobbyHandler)gchandle.Target;
			gchandle.Free();
			deleteLobbyHandler(result);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000BC08 File Offset: 0x00009E08
		public void DeleteLobby(long lobbyId, LobbyManager.DeleteLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.DeleteLobby(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.DeleteLobbyCallback(LobbyManager.DeleteLobbyCallbackImpl));
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000BC48 File Offset: 0x00009E48
		[MonoPInvokeCallback]
		private static void ConnectLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectLobbyHandler connectLobbyHandler = (LobbyManager.ConnectLobbyHandler)gchandle.Target;
			gchandle.Free();
			connectLobbyHandler(result, ref lobby);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000BC78 File Offset: 0x00009E78
		public void ConnectLobby(long lobbyId, string secret, LobbyManager.ConnectLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ConnectLobby(this.MethodsPtr, lobbyId, secret, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.ConnectLobbyCallback(LobbyManager.ConnectLobbyCallbackImpl));
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		[MonoPInvokeCallback]
		private static void ConnectLobbyWithActivitySecretCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectLobbyWithActivitySecretHandler connectLobbyWithActivitySecretHandler = (LobbyManager.ConnectLobbyWithActivitySecretHandler)gchandle.Target;
			gchandle.Free();
			connectLobbyWithActivitySecretHandler(result, ref lobby);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000BCE8 File Offset: 0x00009EE8
		public void ConnectLobbyWithActivitySecret(string activitySecret, LobbyManager.ConnectLobbyWithActivitySecretHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ConnectLobbyWithActivitySecret(this.MethodsPtr, activitySecret, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback(LobbyManager.ConnectLobbyWithActivitySecretCallbackImpl));
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000BD28 File Offset: 0x00009F28
		[MonoPInvokeCallback]
		private static void DisconnectLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DisconnectLobbyHandler disconnectLobbyHandler = (LobbyManager.DisconnectLobbyHandler)gchandle.Target;
			gchandle.Free();
			disconnectLobbyHandler(result);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000BD58 File Offset: 0x00009F58
		public void DisconnectLobby(long lobbyId, LobbyManager.DisconnectLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.DisconnectLobby(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.DisconnectLobbyCallback(LobbyManager.DisconnectLobbyCallbackImpl));
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000BD98 File Offset: 0x00009F98
		public Lobby GetLobby(long lobbyId)
		{
			Lobby lobby = default(Lobby);
			Result result = this.Methods.GetLobby(this.MethodsPtr, lobbyId, ref lobby);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobby;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000BDD4 File Offset: 0x00009FD4
		public string GetLobbyActivitySecret(long lobbyId)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			Result result = this.Methods.GetLobbyActivitySecret(this.MethodsPtr, lobbyId, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000BE18 File Offset: 0x0000A018
		public string GetLobbyMetadataValue(long lobbyId, string key)
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetLobbyMetadataValue(this.MethodsPtr, lobbyId, key, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000BE5C File Offset: 0x0000A05C
		public string GetLobbyMetadataKey(long lobbyId, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Result result = this.Methods.GetLobbyMetadataKey(this.MethodsPtr, lobbyId, index, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000BEA0 File Offset: 0x0000A0A0
		public int LobbyMetadataCount(long lobbyId)
		{
			int num = 0;
			Result result = this.Methods.LobbyMetadataCount(this.MethodsPtr, lobbyId, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		public int MemberCount(long lobbyId)
		{
			int num = 0;
			Result result = this.Methods.MemberCount(this.MethodsPtr, lobbyId, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000BF08 File Offset: 0x0000A108
		public long GetMemberUserId(long lobbyId, int index)
		{
			long num = 0L;
			Result result = this.Methods.GetMemberUserId(this.MethodsPtr, lobbyId, index, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000BF40 File Offset: 0x0000A140
		public User GetMemberUser(long lobbyId, long userId)
		{
			User user = default(User);
			Result result = this.Methods.GetMemberUser(this.MethodsPtr, lobbyId, userId, ref user);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return user;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000BF7C File Offset: 0x0000A17C
		public string GetMemberMetadataValue(long lobbyId, long userId, string key)
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetMemberMetadataValue(this.MethodsPtr, lobbyId, userId, key, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000BFC0 File Offset: 0x0000A1C0
		public string GetMemberMetadataKey(long lobbyId, long userId, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Result result = this.Methods.GetMemberMetadataKey(this.MethodsPtr, lobbyId, userId, index, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000C004 File Offset: 0x0000A204
		public int MemberMetadataCount(long lobbyId, long userId)
		{
			int num = 0;
			Result result = this.Methods.MemberMetadataCount(this.MethodsPtr, lobbyId, userId, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000C03C File Offset: 0x0000A23C
		[MonoPInvokeCallback]
		private static void UpdateMemberCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.UpdateMemberHandler updateMemberHandler = (LobbyManager.UpdateMemberHandler)gchandle.Target;
			gchandle.Free();
			updateMemberHandler(result);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000C06C File Offset: 0x0000A26C
		public void UpdateMember(long lobbyId, long userId, LobbyMemberTransaction transaction, LobbyManager.UpdateMemberHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.UpdateMember(this.MethodsPtr, lobbyId, userId, transaction.MethodsPtr, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.UpdateMemberCallback(LobbyManager.UpdateMemberCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000C0C0 File Offset: 0x0000A2C0
		[MonoPInvokeCallback]
		private static void SendLobbyMessageCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.SendLobbyMessageHandler sendLobbyMessageHandler = (LobbyManager.SendLobbyMessageHandler)gchandle.Target;
			gchandle.Free();
			sendLobbyMessageHandler(result);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000C0F0 File Offset: 0x0000A2F0
		public void SendLobbyMessage(long lobbyId, byte[] data, LobbyManager.SendLobbyMessageHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SendLobbyMessage(this.MethodsPtr, lobbyId, data, data.Length, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.SendLobbyMessageCallback(LobbyManager.SendLobbyMessageCallbackImpl));
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000C134 File Offset: 0x0000A334
		public LobbySearchQuery GetSearchQuery()
		{
			LobbySearchQuery lobbySearchQuery = default(LobbySearchQuery);
			Result result = this.Methods.GetSearchQuery(this.MethodsPtr, ref lobbySearchQuery.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobbySearchQuery;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000C174 File Offset: 0x0000A374
		[MonoPInvokeCallback]
		private static void SearchCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.SearchHandler searchHandler = (LobbyManager.SearchHandler)gchandle.Target;
			gchandle.Free();
			searchHandler(result);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
		public void Search(LobbySearchQuery query, LobbyManager.SearchHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.Search(this.MethodsPtr, query.MethodsPtr, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.SearchCallback(LobbyManager.SearchCallbackImpl));
			query.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000C1F4 File Offset: 0x0000A3F4
		public int LobbyCount()
		{
			int num = 0;
			this.Methods.LobbyCount(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000C21C File Offset: 0x0000A41C
		public long GetLobbyId(int index)
		{
			long num = 0L;
			Result result = this.Methods.GetLobbyId(this.MethodsPtr, index, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000C254 File Offset: 0x0000A454
		[MonoPInvokeCallback]
		private static void ConnectVoiceCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectVoiceHandler connectVoiceHandler = (LobbyManager.ConnectVoiceHandler)gchandle.Target;
			gchandle.Free();
			connectVoiceHandler(result);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000C284 File Offset: 0x0000A484
		public void ConnectVoice(long lobbyId, LobbyManager.ConnectVoiceHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ConnectVoice(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.ConnectVoiceCallback(LobbyManager.ConnectVoiceCallbackImpl));
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		[MonoPInvokeCallback]
		private static void DisconnectVoiceCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DisconnectVoiceHandler disconnectVoiceHandler = (LobbyManager.DisconnectVoiceHandler)gchandle.Target;
			gchandle.Free();
			disconnectVoiceHandler(result);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000C2F4 File Offset: 0x0000A4F4
		public void DisconnectVoice(long lobbyId, LobbyManager.DisconnectVoiceHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.DisconnectVoice(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.DisconnectVoiceCallback(LobbyManager.DisconnectVoiceCallbackImpl));
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000C334 File Offset: 0x0000A534
		public void ConnectNetwork(long lobbyId)
		{
			Result result = this.Methods.ConnectNetwork(this.MethodsPtr, lobbyId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000C364 File Offset: 0x0000A564
		public void DisconnectNetwork(long lobbyId)
		{
			Result result = this.Methods.DisconnectNetwork(this.MethodsPtr, lobbyId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000C394 File Offset: 0x0000A594
		public void FlushNetwork()
		{
			Result result = this.Methods.FlushNetwork(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000C3C4 File Offset: 0x0000A5C4
		public void OpenNetworkChannel(long lobbyId, byte channelId, bool reliable)
		{
			Result result = this.Methods.OpenNetworkChannel(this.MethodsPtr, lobbyId, channelId, reliable);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
		public void SendNetworkMessage(long lobbyId, long userId, byte channelId, byte[] data)
		{
			Result result = this.Methods.SendNetworkMessage(this.MethodsPtr, lobbyId, userId, channelId, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000C430 File Offset: 0x0000A630
		[MonoPInvokeCallback]
		private static void OnLobbyUpdateImpl(IntPtr ptr, long lobbyId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyUpdate != null)
			{
				discord.LobbyManagerInstance.OnLobbyUpdate(lobbyId);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000C470 File Offset: 0x0000A670
		[MonoPInvokeCallback]
		private static void OnLobbyDeleteImpl(IntPtr ptr, long lobbyId, uint reason)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyDelete != null)
			{
				discord.LobbyManagerInstance.OnLobbyDelete(lobbyId, reason);
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		[MonoPInvokeCallback]
		private static void OnMemberConnectImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberConnect != null)
			{
				discord.LobbyManagerInstance.OnMemberConnect(lobbyId, userId);
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		[MonoPInvokeCallback]
		private static void OnMemberUpdateImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberUpdate != null)
			{
				discord.LobbyManagerInstance.OnMemberUpdate(lobbyId, userId);
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000C530 File Offset: 0x0000A730
		[MonoPInvokeCallback]
		private static void OnMemberDisconnectImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberDisconnect != null)
			{
				discord.LobbyManagerInstance.OnMemberDisconnect(lobbyId, userId);
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000C570 File Offset: 0x0000A770
		[MonoPInvokeCallback]
		private static void OnLobbyMessageImpl(IntPtr ptr, long lobbyId, long userId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.LobbyManagerInstance.OnLobbyMessage(lobbyId, userId, array);
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000C5C4 File Offset: 0x0000A7C4
		[MonoPInvokeCallback]
		private static void OnSpeakingImpl(IntPtr ptr, long lobbyId, long userId, bool speaking)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnSpeaking != null)
			{
				discord.LobbyManagerInstance.OnSpeaking(lobbyId, userId, speaking);
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000C608 File Offset: 0x0000A808
		[MonoPInvokeCallback]
		private static void OnNetworkMessageImpl(IntPtr ptr, long lobbyId, long userId, byte channelId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnNetworkMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.LobbyManagerInstance.OnNetworkMessage(lobbyId, userId, channelId, array);
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000C660 File Offset: 0x0000A860
		public IEnumerable<User> GetMemberUsers(long lobbyID)
		{
			int num = this.MemberCount(lobbyID);
			List<User> list = new List<User>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetMemberUser(lobbyID, this.GetMemberUserId(lobbyID, i)));
			}
			return list;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000C69D File Offset: 0x0000A89D
		public void SendLobbyMessage(long lobbyID, string data, LobbyManager.SendLobbyMessageHandler handler)
		{
			this.SendLobbyMessage(lobbyID, Encoding.UTF8.GetBytes(data), handler);
		}

		// Token: 0x04000189 RID: 393
		private IntPtr MethodsPtr;

		// Token: 0x0400018A RID: 394
		private object MethodsStructure;

		// Token: 0x02000096 RID: 150
		internal struct FFIEvents
		{
			// Token: 0x04000267 RID: 615
			internal LobbyManager.FFIEvents.LobbyUpdateHandler OnLobbyUpdate;

			// Token: 0x04000268 RID: 616
			internal LobbyManager.FFIEvents.LobbyDeleteHandler OnLobbyDelete;

			// Token: 0x04000269 RID: 617
			internal LobbyManager.FFIEvents.MemberConnectHandler OnMemberConnect;

			// Token: 0x0400026A RID: 618
			internal LobbyManager.FFIEvents.MemberUpdateHandler OnMemberUpdate;

			// Token: 0x0400026B RID: 619
			internal LobbyManager.FFIEvents.MemberDisconnectHandler OnMemberDisconnect;

			// Token: 0x0400026C RID: 620
			internal LobbyManager.FFIEvents.LobbyMessageHandler OnLobbyMessage;

			// Token: 0x0400026D RID: 621
			internal LobbyManager.FFIEvents.SpeakingHandler OnSpeaking;

			// Token: 0x0400026E RID: 622
			internal LobbyManager.FFIEvents.NetworkMessageHandler OnNetworkMessage;

			// Token: 0x02000111 RID: 273
			// (Invoke) Token: 0x0600052F RID: 1327
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyUpdateHandler(IntPtr ptr, long lobbyId);

			// Token: 0x02000112 RID: 274
			// (Invoke) Token: 0x06000533 RID: 1331
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyDeleteHandler(IntPtr ptr, long lobbyId, uint reason);

			// Token: 0x02000113 RID: 275
			// (Invoke) Token: 0x06000537 RID: 1335
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberConnectHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x02000114 RID: 276
			// (Invoke) Token: 0x0600053B RID: 1339
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberUpdateHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x02000115 RID: 277
			// (Invoke) Token: 0x0600053F RID: 1343
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberDisconnectHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x02000116 RID: 278
			// (Invoke) Token: 0x06000543 RID: 1347
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyMessageHandler(IntPtr ptr, long lobbyId, long userId, IntPtr dataPtr, int dataLen);

			// Token: 0x02000117 RID: 279
			// (Invoke) Token: 0x06000547 RID: 1351
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SpeakingHandler(IntPtr ptr, long lobbyId, long userId, bool speaking);

			// Token: 0x02000118 RID: 280
			// (Invoke) Token: 0x0600054B RID: 1355
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void NetworkMessageHandler(IntPtr ptr, long lobbyId, long userId, byte channelId, IntPtr dataPtr, int dataLen);
		}

		// Token: 0x02000097 RID: 151
		internal struct FFIMethods
		{
			// Token: 0x0400026F RID: 623
			internal LobbyManager.FFIMethods.GetLobbyCreateTransactionMethod GetLobbyCreateTransaction;

			// Token: 0x04000270 RID: 624
			internal LobbyManager.FFIMethods.GetLobbyUpdateTransactionMethod GetLobbyUpdateTransaction;

			// Token: 0x04000271 RID: 625
			internal LobbyManager.FFIMethods.GetMemberUpdateTransactionMethod GetMemberUpdateTransaction;

			// Token: 0x04000272 RID: 626
			internal LobbyManager.FFIMethods.CreateLobbyMethod CreateLobby;

			// Token: 0x04000273 RID: 627
			internal LobbyManager.FFIMethods.UpdateLobbyMethod UpdateLobby;

			// Token: 0x04000274 RID: 628
			internal LobbyManager.FFIMethods.DeleteLobbyMethod DeleteLobby;

			// Token: 0x04000275 RID: 629
			internal LobbyManager.FFIMethods.ConnectLobbyMethod ConnectLobby;

			// Token: 0x04000276 RID: 630
			internal LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretMethod ConnectLobbyWithActivitySecret;

			// Token: 0x04000277 RID: 631
			internal LobbyManager.FFIMethods.DisconnectLobbyMethod DisconnectLobby;

			// Token: 0x04000278 RID: 632
			internal LobbyManager.FFIMethods.GetLobbyMethod GetLobby;

			// Token: 0x04000279 RID: 633
			internal LobbyManager.FFIMethods.GetLobbyActivitySecretMethod GetLobbyActivitySecret;

			// Token: 0x0400027A RID: 634
			internal LobbyManager.FFIMethods.GetLobbyMetadataValueMethod GetLobbyMetadataValue;

			// Token: 0x0400027B RID: 635
			internal LobbyManager.FFIMethods.GetLobbyMetadataKeyMethod GetLobbyMetadataKey;

			// Token: 0x0400027C RID: 636
			internal LobbyManager.FFIMethods.LobbyMetadataCountMethod LobbyMetadataCount;

			// Token: 0x0400027D RID: 637
			internal LobbyManager.FFIMethods.MemberCountMethod MemberCount;

			// Token: 0x0400027E RID: 638
			internal LobbyManager.FFIMethods.GetMemberUserIdMethod GetMemberUserId;

			// Token: 0x0400027F RID: 639
			internal LobbyManager.FFIMethods.GetMemberUserMethod GetMemberUser;

			// Token: 0x04000280 RID: 640
			internal LobbyManager.FFIMethods.GetMemberMetadataValueMethod GetMemberMetadataValue;

			// Token: 0x04000281 RID: 641
			internal LobbyManager.FFIMethods.GetMemberMetadataKeyMethod GetMemberMetadataKey;

			// Token: 0x04000282 RID: 642
			internal LobbyManager.FFIMethods.MemberMetadataCountMethod MemberMetadataCount;

			// Token: 0x04000283 RID: 643
			internal LobbyManager.FFIMethods.UpdateMemberMethod UpdateMember;

			// Token: 0x04000284 RID: 644
			internal LobbyManager.FFIMethods.SendLobbyMessageMethod SendLobbyMessage;

			// Token: 0x04000285 RID: 645
			internal LobbyManager.FFIMethods.GetSearchQueryMethod GetSearchQuery;

			// Token: 0x04000286 RID: 646
			internal LobbyManager.FFIMethods.SearchMethod Search;

			// Token: 0x04000287 RID: 647
			internal LobbyManager.FFIMethods.LobbyCountMethod LobbyCount;

			// Token: 0x04000288 RID: 648
			internal LobbyManager.FFIMethods.GetLobbyIdMethod GetLobbyId;

			// Token: 0x04000289 RID: 649
			internal LobbyManager.FFIMethods.ConnectVoiceMethod ConnectVoice;

			// Token: 0x0400028A RID: 650
			internal LobbyManager.FFIMethods.DisconnectVoiceMethod DisconnectVoice;

			// Token: 0x0400028B RID: 651
			internal LobbyManager.FFIMethods.ConnectNetworkMethod ConnectNetwork;

			// Token: 0x0400028C RID: 652
			internal LobbyManager.FFIMethods.DisconnectNetworkMethod DisconnectNetwork;

			// Token: 0x0400028D RID: 653
			internal LobbyManager.FFIMethods.FlushNetworkMethod FlushNetwork;

			// Token: 0x0400028E RID: 654
			internal LobbyManager.FFIMethods.OpenNetworkChannelMethod OpenNetworkChannel;

			// Token: 0x0400028F RID: 655
			internal LobbyManager.FFIMethods.SendNetworkMessageMethod SendNetworkMessage;

			// Token: 0x02000119 RID: 281
			// (Invoke) Token: 0x0600054F RID: 1359
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyCreateTransactionMethod(IntPtr methodsPtr, ref IntPtr transaction);

			// Token: 0x0200011A RID: 282
			// (Invoke) Token: 0x06000553 RID: 1363
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyUpdateTransactionMethod(IntPtr methodsPtr, long lobbyId, ref IntPtr transaction);

			// Token: 0x0200011B RID: 283
			// (Invoke) Token: 0x06000557 RID: 1367
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUpdateTransactionMethod(IntPtr methodsPtr, long lobbyId, long userId, ref IntPtr transaction);

			// Token: 0x0200011C RID: 284
			// (Invoke) Token: 0x0600055B RID: 1371
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CreateLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x0200011D RID: 285
			// (Invoke) Token: 0x0600055F RID: 1375
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CreateLobbyMethod(IntPtr methodsPtr, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.CreateLobbyCallback callback);

			// Token: 0x0200011E RID: 286
			// (Invoke) Token: 0x06000563 RID: 1379
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x0200011F RID: 287
			// (Invoke) Token: 0x06000567 RID: 1383
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.UpdateLobbyCallback callback);

			// Token: 0x02000120 RID: 288
			// (Invoke) Token: 0x0600056B RID: 1387
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DeleteLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x02000121 RID: 289
			// (Invoke) Token: 0x0600056F RID: 1391
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DeleteLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DeleteLobbyCallback callback);

			// Token: 0x02000122 RID: 290
			// (Invoke) Token: 0x06000573 RID: 1395
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x02000123 RID: 291
			// (Invoke) Token: 0x06000577 RID: 1399
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyMethod(IntPtr methodsPtr, long lobbyId, [MarshalAs(UnmanagedType.LPStr)] string secret, IntPtr callbackData, LobbyManager.FFIMethods.ConnectLobbyCallback callback);

			// Token: 0x02000124 RID: 292
			// (Invoke) Token: 0x0600057B RID: 1403
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyWithActivitySecretCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x02000125 RID: 293
			// (Invoke) Token: 0x0600057F RID: 1407
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyWithActivitySecretMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string activitySecret, IntPtr callbackData, LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback callback);

			// Token: 0x02000126 RID: 294
			// (Invoke) Token: 0x06000583 RID: 1411
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x02000127 RID: 295
			// (Invoke) Token: 0x06000587 RID: 1415
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DisconnectLobbyCallback callback);

			// Token: 0x02000128 RID: 296
			// (Invoke) Token: 0x0600058B RID: 1419
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMethod(IntPtr methodsPtr, long lobbyId, ref Lobby lobby);

			// Token: 0x02000129 RID: 297
			// (Invoke) Token: 0x0600058F RID: 1423
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyActivitySecretMethod(IntPtr methodsPtr, long lobbyId, StringBuilder secret);

			// Token: 0x0200012A RID: 298
			// (Invoke) Token: 0x06000593 RID: 1427
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMetadataValueMethod(IntPtr methodsPtr, long lobbyId, [MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder value);

			// Token: 0x0200012B RID: 299
			// (Invoke) Token: 0x06000597 RID: 1431
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMetadataKeyMethod(IntPtr methodsPtr, long lobbyId, int index, StringBuilder key);

			// Token: 0x0200012C RID: 300
			// (Invoke) Token: 0x0600059B RID: 1435
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result LobbyMetadataCountMethod(IntPtr methodsPtr, long lobbyId, ref int count);

			// Token: 0x0200012D RID: 301
			// (Invoke) Token: 0x0600059F RID: 1439
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result MemberCountMethod(IntPtr methodsPtr, long lobbyId, ref int count);

			// Token: 0x0200012E RID: 302
			// (Invoke) Token: 0x060005A3 RID: 1443
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUserIdMethod(IntPtr methodsPtr, long lobbyId, int index, ref long userId);

			// Token: 0x0200012F RID: 303
			// (Invoke) Token: 0x060005A7 RID: 1447
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUserMethod(IntPtr methodsPtr, long lobbyId, long userId, ref User user);

			// Token: 0x02000130 RID: 304
			// (Invoke) Token: 0x060005AB RID: 1451
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberMetadataValueMethod(IntPtr methodsPtr, long lobbyId, long userId, [MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder value);

			// Token: 0x02000131 RID: 305
			// (Invoke) Token: 0x060005AF RID: 1455
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberMetadataKeyMethod(IntPtr methodsPtr, long lobbyId, long userId, int index, StringBuilder key);

			// Token: 0x02000132 RID: 306
			// (Invoke) Token: 0x060005B3 RID: 1459
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result MemberMetadataCountMethod(IntPtr methodsPtr, long lobbyId, long userId, ref int count);

			// Token: 0x02000133 RID: 307
			// (Invoke) Token: 0x060005B7 RID: 1463
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateMemberCallback(IntPtr ptr, Result result);

			// Token: 0x02000134 RID: 308
			// (Invoke) Token: 0x060005BB RID: 1467
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateMemberMethod(IntPtr methodsPtr, long lobbyId, long userId, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.UpdateMemberCallback callback);

			// Token: 0x02000135 RID: 309
			// (Invoke) Token: 0x060005BF RID: 1471
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendLobbyMessageCallback(IntPtr ptr, Result result);

			// Token: 0x02000136 RID: 310
			// (Invoke) Token: 0x060005C3 RID: 1475
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendLobbyMessageMethod(IntPtr methodsPtr, long lobbyId, byte[] data, int dataLen, IntPtr callbackData, LobbyManager.FFIMethods.SendLobbyMessageCallback callback);

			// Token: 0x02000137 RID: 311
			// (Invoke) Token: 0x060005C7 RID: 1479
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSearchQueryMethod(IntPtr methodsPtr, ref IntPtr query);

			// Token: 0x02000138 RID: 312
			// (Invoke) Token: 0x060005CB RID: 1483
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SearchCallback(IntPtr ptr, Result result);

			// Token: 0x02000139 RID: 313
			// (Invoke) Token: 0x060005CF RID: 1487
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SearchMethod(IntPtr methodsPtr, IntPtr query, IntPtr callbackData, LobbyManager.FFIMethods.SearchCallback callback);

			// Token: 0x0200013A RID: 314
			// (Invoke) Token: 0x060005D3 RID: 1491
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyCountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x0200013B RID: 315
			// (Invoke) Token: 0x060005D7 RID: 1495
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyIdMethod(IntPtr methodsPtr, int index, ref long lobbyId);

			// Token: 0x0200013C RID: 316
			// (Invoke) Token: 0x060005DB RID: 1499
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectVoiceCallback(IntPtr ptr, Result result);

			// Token: 0x0200013D RID: 317
			// (Invoke) Token: 0x060005DF RID: 1503
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectVoiceMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.ConnectVoiceCallback callback);

			// Token: 0x0200013E RID: 318
			// (Invoke) Token: 0x060005E3 RID: 1507
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectVoiceCallback(IntPtr ptr, Result result);

			// Token: 0x0200013F RID: 319
			// (Invoke) Token: 0x060005E7 RID: 1511
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectVoiceMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DisconnectVoiceCallback callback);

			// Token: 0x02000140 RID: 320
			// (Invoke) Token: 0x060005EB RID: 1515
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ConnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

			// Token: 0x02000141 RID: 321
			// (Invoke) Token: 0x060005EF RID: 1519
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DisconnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

			// Token: 0x02000142 RID: 322
			// (Invoke) Token: 0x060005F3 RID: 1523
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FlushNetworkMethod(IntPtr methodsPtr);

			// Token: 0x02000143 RID: 323
			// (Invoke) Token: 0x060005F7 RID: 1527
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenNetworkChannelMethod(IntPtr methodsPtr, long lobbyId, byte channelId, bool reliable);

			// Token: 0x02000144 RID: 324
			// (Invoke) Token: 0x060005FB RID: 1531
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SendNetworkMessageMethod(IntPtr methodsPtr, long lobbyId, long userId, byte channelId, byte[] data, int dataLen);
		}

		// Token: 0x02000098 RID: 152
		// (Invoke) Token: 0x0600037F RID: 895
		public delegate void CreateLobbyHandler(Result result, ref Lobby lobby);

		// Token: 0x02000099 RID: 153
		// (Invoke) Token: 0x06000383 RID: 899
		public delegate void UpdateLobbyHandler(Result result);

		// Token: 0x0200009A RID: 154
		// (Invoke) Token: 0x06000387 RID: 903
		public delegate void DeleteLobbyHandler(Result result);

		// Token: 0x0200009B RID: 155
		// (Invoke) Token: 0x0600038B RID: 907
		public delegate void ConnectLobbyHandler(Result result, ref Lobby lobby);

		// Token: 0x0200009C RID: 156
		// (Invoke) Token: 0x0600038F RID: 911
		public delegate void ConnectLobbyWithActivitySecretHandler(Result result, ref Lobby lobby);

		// Token: 0x0200009D RID: 157
		// (Invoke) Token: 0x06000393 RID: 915
		public delegate void DisconnectLobbyHandler(Result result);

		// Token: 0x0200009E RID: 158
		// (Invoke) Token: 0x06000397 RID: 919
		public delegate void UpdateMemberHandler(Result result);

		// Token: 0x0200009F RID: 159
		// (Invoke) Token: 0x0600039B RID: 923
		public delegate void SendLobbyMessageHandler(Result result);

		// Token: 0x020000A0 RID: 160
		// (Invoke) Token: 0x0600039F RID: 927
		public delegate void SearchHandler(Result result);

		// Token: 0x020000A1 RID: 161
		// (Invoke) Token: 0x060003A3 RID: 931
		public delegate void ConnectVoiceHandler(Result result);

		// Token: 0x020000A2 RID: 162
		// (Invoke) Token: 0x060003A7 RID: 935
		public delegate void DisconnectVoiceHandler(Result result);

		// Token: 0x020000A3 RID: 163
		// (Invoke) Token: 0x060003AB RID: 939
		public delegate void LobbyUpdateHandler(long lobbyId);

		// Token: 0x020000A4 RID: 164
		// (Invoke) Token: 0x060003AF RID: 943
		public delegate void LobbyDeleteHandler(long lobbyId, uint reason);

		// Token: 0x020000A5 RID: 165
		// (Invoke) Token: 0x060003B3 RID: 947
		public delegate void MemberConnectHandler(long lobbyId, long userId);

		// Token: 0x020000A6 RID: 166
		// (Invoke) Token: 0x060003B7 RID: 951
		public delegate void MemberUpdateHandler(long lobbyId, long userId);

		// Token: 0x020000A7 RID: 167
		// (Invoke) Token: 0x060003BB RID: 955
		public delegate void MemberDisconnectHandler(long lobbyId, long userId);

		// Token: 0x020000A8 RID: 168
		// (Invoke) Token: 0x060003BF RID: 959
		public delegate void LobbyMessageHandler(long lobbyId, long userId, byte[] data);

		// Token: 0x020000A9 RID: 169
		// (Invoke) Token: 0x060003C3 RID: 963
		public delegate void SpeakingHandler(long lobbyId, long userId, bool speaking);

		// Token: 0x020000AA RID: 170
		// (Invoke) Token: 0x060003C7 RID: 967
		public delegate void NetworkMessageHandler(long lobbyId, long userId, byte channelId, byte[] data);
	}
}
