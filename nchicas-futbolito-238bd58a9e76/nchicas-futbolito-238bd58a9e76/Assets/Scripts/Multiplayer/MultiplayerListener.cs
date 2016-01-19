//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System;
//
//
//using com.shephertz.app42.gaming.multiplayer.client;
//using com.shephertz.app42.gaming.multiplayer.client.events;
//using com.shephertz.app42.gaming.multiplayer.client.listener;
//using com.shephertz.app42.gaming.multiplayer.client.command;
//using com.shephertz.app42.gaming.multiplayer.client.message;
//using com.shephertz.app42.gaming.multiplayer.client.transformer;
//
//namespace AssemblyCSharp
//{
//	public class MultiplayerListener :  ConnectionRequestListener, LobbyRequestListener, ZoneRequestListener, RoomRequestListener,ChatRequestListener, UpdateRequestListener, NotifyListener {
//		int state = 0;
//		string debug = "";
//		
//		private void Log(string msg)
//		{
//			debug = msg + "\n" + debug;
//		}
//		
//		public string getDebug()
//		{
//			return debug;
//		}
//
//		public Action<List<string>> OnReturnOnlinePlayers;
//		public Action<string,string> OnPrivateMessageRecibed;
//		public Action<string> OnRoomCreated;
//		public Action<string,string> OnRoomMessageRecibed;
//		public Action OnConnectedDone;
//		public Action OnUserLeftGame;
//		public Action OnConectionLost;
//		//ConnectionRequestListener
//		public void onConnectDone(ConnectEvent eventObj)
//		{
//			if(eventObj.getResult() == 0)
//			{
//				//When player is connected
//				//WarpClient.GetInstance().SubscribeRoom(MultiplayerManager.Instance.RoomId);
//				//WarpClient.GetInstance().GetOnlineUsers();
//				if(this.OnConnectedDone != null) OnConnectedDone();
//			}else if(eventObj.getResult() == 5){
//				if(this.OnConectionLost !=null) OnConectionLost();
//			}
//			Log ("onConnectDone : " + eventObj.getResult());
//		}
//		
//		public void onInitUDPDone(byte res)
//		{
//		}
//		
//		public void onLog(string message){
//			Log (message);
//		}
//		
//		public void onDisconnectDone(ConnectEvent eventObj)
//		{
//			Log("onDisconnectDone : " + eventObj.getResult());
//		}
//		
//		//LobbyRequestListener
//		public void onJoinLobbyDone (LobbyEvent eventObj)
//		{
//			Log ("onJoinLobbyDone : " + eventObj.getResult());
//			if(eventObj.getResult() == 0)
//			{
//				state = 1;
//			}
//		}
//		
//		public void onLeaveLobbyDone (LobbyEvent eventObj)
//		{
//			Log ("onLeaveLobbyDone : " + eventObj.getResult());
//		}
//		
//		public void onSubscribeLobbyDone (LobbyEvent eventObj)
//		{
//			Log ("onSubscribeLobbyDone : " + eventObj.getResult());
//			if(eventObj.getResult() == 0)
//			{
//				WarpClient.GetInstance().JoinLobby();
//			}
//		}
//		
//		public void onUnSubscribeLobbyDone (LobbyEvent eventObj)
//		{
//			Log ("onUnSubscribeLobbyDone : " + eventObj.getResult());
//		}
//		
//		public void onGetLiveLobbyInfoDone (LiveRoomInfoEvent eventObj)
//		{
//			Log ("onGetLiveLobbyInfoDone : " + eventObj.getResult());
//		}
//		
//		//ZoneRequestListener
//		public void onDeleteRoomDone (RoomEvent eventObj)
//		{
//			Log ("onDeleteRoomDone : " + eventObj.getResult());
//		}
//		
//		public void onGetAllRoomsDone (AllRoomsEvent eventObj)
//		{
//			Log ("onGetAllRoomsDone : " + eventObj.getResult());
//		}
//		
//		public void onCreateRoomDone (RoomEvent eventObj)
//		{
//			Log ("onCreateRoomDone : " + eventObj.getResult());
//			if(this.OnRoomCreated!=null){
//				this.OnRoomCreated(eventObj.getData().getId());
//			}
//		}
//		
//		public void onGetOnlineUsersDone (AllUsersEvent eventObj)
//		{
//			Log ("onGetOnlineUsersDone : " + eventObj.getResult());
//			if(this.OnReturnOnlinePlayers != null ){
//				//For check if are users online
//				List<string> users = ( eventObj.getUserNames() == null) ? new List<string>() : eventObj.getUserNames().ToList();
//				this.OnReturnOnlinePlayers(users);
//			}
//		}
//		
//		public void onGetLiveUserInfoDone (LiveUserInfoEvent eventObj)
//		{
//			Log ("onGetLiveUserInfoDone : " + eventObj.getResult());
//		}
//		
//		public void onSetCustomUserDataDone (LiveUserInfoEvent eventObj)
//		{
//			Log ("onSetCustomUserDataDone : " + eventObj.getResult());
//		}
//		
//		public void onGetMatchedRoomsDone(MatchedRoomsEvent eventObj)
//		{
//			if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
//			{
//				Log ("GetMatchedRooms event received with success status");
//				foreach (var roomData in eventObj.getRoomsData())
//				{
//					Log("Room ID:" + roomData.getId());
//				}
//			}
//		}		
//		//RoomRequestListener
//		public void onSubscribeRoomDone (RoomEvent eventObj)
//		{
//			if(eventObj.getResult() == 0)
//			{
//				/*string json = "{\"start\":\""+id+"\"}";
//				WarpClient.GetInstance().SendChat(json);
//				state = 1;*/
//				//WarpClient.GetInstance().JoinRoom(MultiplayerManager.Instance.RoomId);
//			}
//			
//			Log ("onSubscribeRoomDone : " + eventObj.getResult());
//		}
//		
//		public void onUnSubscribeRoomDone (RoomEvent eventObj)
//		{
//			Log ("onUnSubscribeRoomDone : " + eventObj.getResult());
//		}
//		
//		public void onJoinRoomDone (RoomEvent eventObj)
//		{
//			if(eventObj.getResult() == 0)
//			{
//				state = 1;
//			}
//			Log ("onJoinRoomDone : " + eventObj.getResult());
//			//For get the id of the data
//			WarpClient.GetInstance().GetLiveRoomInfo(MultiplayerManager.Instance.RoomId);  
//		}
//		
//		public void onLockPropertiesDone(byte result)
//		{
//			Log ("onLockPropertiesDone : " + result);
//		}
//		
//		public void onUnlockPropertiesDone(byte result)
//		{
//			Log ("onUnlockPropertiesDone : " + result);
//		}
//		
//		public void onLeaveRoomDone (RoomEvent eventObj)
//		{
//			Log ("onLeaveRoomDone : " + eventObj.getResult());
//		}
//		
//		public void onGetLiveRoomInfoDone (LiveRoomInfoEvent eventObj)
//		{
//			Log ("onGetLiveRoomInfoDone : " + eventObj.getResult());
//			Log ( "user length : " +eventObj.getJoinedUsers().Length);
//			if(GameLoader.Instance.AllPlayerIn = eventObj.getJoinedUsers().Length == 2){
//				MultiplayerManager.Instance.SendStart();
//			}
//
//			GameLoader.Instance.SetMultiplayerGame(eventObj.getJoinedUsers().Length);
//		}
//		
//		public void onSetCustomRoomDataDone (LiveRoomInfoEvent eventObj)
//		{
//			Log ("onSetCustomRoomDataDone : " + eventObj.getResult());
//		}
//		
//		public void onUpdatePropertyDone(LiveRoomInfoEvent eventObj)
//		{
//			if (WarpResponseResultCode.SUCCESS == eventObj.getResult())
//			{
//				Log ("UpdateProperty event received with success status");
//			}
//			else
//			{
//				Log ("Update Propert event received with fail status. Status is :" + eventObj.getResult().ToString());
//			}
//		}
//		
//		//ChatRequestListener
//		public void onSendChatDone (byte result)
//		{
//			Log ("onSendChatDone result : " + result);
//			
//		}
//		
//		public void onSendPrivateChatDone(byte result)
//		{
//			Log ("onSendPrivateChatDone : " + result);
//		}
//		
//		//UpdateRequestListener
//		public void onSendUpdateDone (byte result)
//		{
//		}
//		public void onSendPrivateUpdateDone (byte result)
//		{
//			Log ("onSendPrivateUpdateDone : " + result);
//		}
//		//NotifyListener
//		public void onRoomCreated (RoomData eventObj)
//		{
//			Log ("onRoomCreated");
//		}
//		public void onPrivateUpdateReceived (string sender, byte[] update, bool fromUdp)
//		{
//			Log ("onPrivateUpdate");
//		}
//		public void onRoomDestroyed (RoomData eventObj)
//		{
//			Log ("onRoomDestroyed");
//		}
//		
//		public void onUserLeftRoom (RoomData eventObj, string username)
//		{
//			Log ("onUserLeftRoom : " + username);
//			if(MultiplayerManager.Instance.PlayerName != username && this.OnUserLeftGame != null){
//				this.OnUserLeftGame();
//			}
//
//		}
//		
//		public void onUserJoinedRoom (RoomData eventObj, string username)
//		{
//			Log ("onUserJoinedRoom : " + username);
//			if( username != MultiplayerManager.Instance.PlayerName){
//				GameLoader.Instance.AllPlayerIn = true;
//			}
//		}
//		
//		public void onUserLeftLobby (LobbyData eventObj, string username)
//		{
//			Log ("onUserLeftLobby : " + username);
//
//		}
//		
//		public void onUserJoinedLobby (LobbyData eventObj, string username)
//		{
//			Log ("onUserJoinedLobby : " + username);
//		}
//		
//		public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
//		{
//			Log ("onUserChangeRoomProperty : " + sender);
//		}
//		
//		public void onPrivateChatReceived(string sender, string message)
//		{
//			Log ("onPrivateChatReceived : " + sender);
////			Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
//			if(OnPrivateMessageRecibed !=null) OnPrivateMessageRecibed(sender,message);
//		}
//		
//		public void onMoveCompleted(MoveEvent move)
//		{
//			Log ("onMoveCompleted by : " + move.getSender());
//		}
//		
//		public void onChatReceived (ChatEvent eventObj)
//		{
//			Log(eventObj.getSender() + " Recibed " + eventObj.getMessage());
//			if(OnRoomMessageRecibed !=null){
//				Log("Message Recibed");
//				this.OnRoomMessageRecibed(eventObj.getSender(),eventObj.getMessage());
//			}
//		}
//		
//		public void onUpdatePeersReceived (UpdateEvent eventObj)
//		{
//			Log ("onUpdatePeersReceived");
//		}
//		
//		public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, System.Object> properties)
//		{
//			Log("Notification for User Changed Room Propert received");
//			Log(roomData.getId());
//			Log(sender);
//			foreach (KeyValuePair<string, System.Object> entry in properties)
//			{
//				Log("KEY:" + entry.Key);
//				Log("VALUE:" + entry.Value.ToString());
//			}
//		}
//		
//		public void sendMsg(string msg)
//		{
//			if(state == 1)
//			{
//				WarpClient.GetInstance().SendChat(msg);
//			}
//		}
//		
//		public void onUserPaused(string a, bool b, string c)
//		{
//		}
//		
//		public void onUserResumed(string a, bool b, string c)
//		{
//		}
//		
//		public void onGameStarted(string a, string b, string c)
//		{
//		}
//		
//		public void onGameStopped(string a, string b)
//		{
//		}
//	}
//}
