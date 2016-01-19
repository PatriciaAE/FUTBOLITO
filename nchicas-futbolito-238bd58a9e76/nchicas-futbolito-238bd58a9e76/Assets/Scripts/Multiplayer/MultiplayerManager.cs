//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using AssemblyCSharp;
//using Facebook.MiniJSON;
//
//using com.shephertz.app42.gaming.multiplayer.client;
//using com.shephertz.app42.gaming.multiplayer.client.events;
//using com.shephertz.app42.gaming.multiplayer.client.listener;
//using com.shephertz.app42.gaming.multiplayer.client.command;
//using com.shephertz.app42.gaming.multiplayer.client.message;
//using com.shephertz.app42.gaming.multiplayer.client.transformer;
//
//
//public class PackageInvitation{
//	public PackageInvitation(){}
//	
//	public string packagetype {get;set;}
//	public string from {get;set;}
//}
//
//
//public class MultiplayerManager : MonoBehaviour {
//
//	public enum State {
//		Disconected  = 0,
//		Connecting   = 1,
//		Connected    = 2,
//		Disconecting = 3,
//		recovering   = 4
//	}
//
//	[SerializeField]
//	private string roomId;
//	[SerializeField]
//	private string playerName;
//	[SerializeField]
//	private string apiKey 	 = "";
//	[SerializeField]
//	private string secretKey = "";
//	
//	private Vector3 shootVector;
//	private bool shootRecibed = false;
//	private Vector3 position;
//	public bool ShootRecibed{
//		get{
//			return shootRecibed;
//		}
//	}
//
//
//
//
//	public void AddLog(string log){
//		listener.onLog(log);
//	}
//	private static MultiplayerManager instance;
//
//	private MultiplayerListener listener = new MultiplayerListener();
//	public string RoomId{
//		get{
//			return roomId;
//		}
//	}
//	
//	public static MultiplayerManager Instance{
//		get{
//			return instance;
//		}
//	}
//
//	public string PlayerName{
//		get{
//			return playerName;
//		}
//	}
//
//	public void AddShoot(Vector3 shoot,Vector3 position){
//		this.shootRecibed = true;
//		this.shootVector = shoot;
//		this.position = position;
//	}
//
//	public Vector3 GetShoot(){
//		this.shootRecibed = false;
//		return this.shootVector;
//	}
//	public Vector3 GetPosition(){
//		return this.position;
//	}
//	void Update(){
//		WarpClient.GetInstance().Update();
//	}
//	void Awake(){
//		if(instance == null) instance = this;
//	}
//	void Start(){
//		if(instance ==this) {
//		WarpClient.initialize(this.apiKey,this.secretKey,"50.112.253.86");
//		WarpClient.GetInstance().AddConnectionRequestListener(listener);
//		WarpClient.GetInstance().AddChatRequestListener(listener);
//		WarpClient.GetInstance().AddLobbyRequestListener(listener);
//		WarpClient.GetInstance().AddNotificationListener(listener);
//		WarpClient.GetInstance().AddRoomRequestListener(listener);
//		WarpClient.GetInstance().AddUpdateRequestListener(listener);
//		WarpClient.GetInstance().AddZoneRequestListener(listener);
//
//	
//		this.listener.OnPrivateMessageRecibed += OnPrivateMessageRecibed;
//		this.listener.OnRoomCreated += OnRoomCreated;
//		this.listener.OnRoomMessageRecibed += OnRoomMessageRecibed;
//		this.listener.OnUserLeftGame += OnUserLeftGame;
//		this.listener.OnConectionLost += ()=>{
//			//MessageWindow.Show("conexion","tu conexion se ha perdido, recarga la pagina");
//			//Game.Instance.isConnectionEstablished = false;
//			//Game.Instance.StartTryToLogIn ();
//			StartCoroutine("TryToReconnect");
//		};
//		StartCoroutine("TryToConnect");
//		}
//	}
//
//	public void OnGameConnected( Action  callback){
//		this.listener.OnConnectedDone+=callback;
//	}
//	public void OnUserLeftGame(){
//		if(Game.Instance.state == Game.State.playing){
//			MessageWindow.OnOkPress +=()=>{
//				Application.LoadLevelAsync("MainMenu");
//			};
//			MessageWindow.Show("conexión","tu amigo ha salido del juego");
//		}
//	}
//	public void OnRoomCreated(string roomId){
//		this.roomId = roomId;
//		this.SendRoomId(roomId,Game.Instance.remotePlayerPref.fbId);
//	}
//
//
//	public void OnRoomMessageRecibed(string sender, string json){
//		if(playerName == sender) return;
//		var data = Json.Deserialize(json) as Dictionary<string,object>;
//		Debug.Log((string)data["package"]);
//		switch( (string)data["package"]){
//		case "shoot" :
//			Vector3 position= new Vector3 ( float.Parse( (string)data["px"]),float.Parse((string)data["py"]), float.Parse((string)data["pz"]));
//			Vector3 shoot =new Vector3 ( float.Parse( (string)data["sx"]),float.Parse((string)data["sy"]), float.Parse((string)data["sz"]));
//			this.AddShoot(shoot,position );
//			listener.onLog( " x : " + this.shootVector.x + " y " + + this.shootVector.y +  " y " + + this.shootVector.z);
//			break;
//		case "start" :
//			GameLoader.Instance.AllPlayerIn = true;
//			break;
//		case "pass" :
//			Game.Instance.ActualPlayer.TurnEnd();
//			break;
//		}
//	}
//	
//
//	public void OnPrivateMessageRecibed(string sender, string json){
//		var data = Json.Deserialize(json) as Dictionary<string,object>;
//		
//
//		switch( (string)data["package"]){
//		case "invitation" :
//			/*if( int.Parse(Game.Instance.localPlayer["remainingMatches"].ToString()) == 0 ) {
//				SendBusyMessage((string)data["from"]);
//			}
//			else*/ if(Game.Instance.status == Game.Status.playing || InvitationWindow.isOpen){
//				SendBusyMessage((string)data["from"]);
//				LoadingWindow.Close();
//			}else{
//				InvitationWindow.Show((string)data["from"],(string)data["name"]);
//			}
//			break;
//		case "invitationReject" :
//			LoadingWindow.Close();
//			MessageWindow.Show("invitación","invitación rechazada");
//			break;
//		case "invitationAcept":
//			//Creating the room 
//			PlayerInformation player = Game.Instance.localPlayerPref;
//			WarpClient.GetInstance().CreateRoom(player.fbId,player.fbId,2,null);
//			Game.Instance.status = Game.Status.playing;
//			TeamSelectionWindow.Show();
//			LoadingWindow.Close();
//			FriendsSelectionWindow.Close();
//			break;
//		case "palyerInfo":
//			PlayerInformation remotePlayer = Game.Instance.remotePlayerPref;
//			remotePlayer.flagImageName = (string)data["flagImageName"];
//			remotePlayer.teamName = (string)data["teamName"];
//			remotePlayer.formation = (FieldTeam.Formation ) Enum.Parse(typeof(FieldTeam.Formation),(string)data["formation"]);
//			break;
//		case "roomId":
//			this.roomId = (string)data["roomId"];
//			break;
//		case "alreadyPlaying":
//			LoadingWindow.Close();
//			MessageWindow.Show("invitación","tu amigo ya esta jugando en este momento");
//			break;
//		case "quitMatch":
//			MessageWindow.OnOkPress +=()=>{Application.LoadLevelAsync("MainMenu");};
//			MessageWindow.Show("invitación","tu amigo ha salido de la partida");
//			break;
//		}
//	}
//
//	public void ConnectUser( string userId){
//		this.playerName = userId;
//	
//	}
//
//	public void ConnectToRoom(){
//		WarpClient.GetInstance().JoinRoom(this.roomId);
//		WarpClient.GetInstance().SubscribeRoom(this.roomId);
//	}
//
//	public void SendQuitMessage(string userId){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","quitMatch");
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().sendPrivateChat(userId,json);
//	}
//
//	public void SendBusyMessage(string userId){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","alreadyPlaying");
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().sendPrivateChat(userId,json);
//	}
//
//	public void SendPass(){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","pass");
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().SendChat(json);
//	}
//	public void SendShoot(Vector3 shoot,Vector3 position){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","shoot");
//		dictionary.Add("px",position.x.ToString());
//		dictionary.Add("py",position.y.ToString());
//		dictionary.Add("pz",position.z.ToString());
//
//		dictionary.Add("sx",shoot.x.ToString());
//		dictionary.Add("sy",shoot.y.ToString());
//		dictionary.Add("sz",shoot.z.ToString());
//
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().SendChat(json);
//	}
//
//	
//	public void SendStart(){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","start");
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().SendChat(json);
//	}
//	public void SendRoomId(string roomId,string userId){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","roomId");
//		dictionary.Add("roomId",roomId);
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().sendPrivateChat(userId,json);
//	}
//	public void AceptInvitation(string userId){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","invitationAcept");
//		dictionary.Add("from",userId);
//		dictionary.Add("name",Game.Instance.localPlayerPref.name);
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().sendPrivateChat(userId,json);
//
//	}
//	public void DeclineInvitation(string userId){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","invitationReject");
//		dictionary.Add("from",userId);
//		dictionary.Add("name",Game.Instance.localPlayerPref.name);
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().sendPrivateChat(userId,json);
//	}
//	public void SendInvitation( string userId){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","invitation");
//		dictionary.Add("from",Game.Instance.localPlayerPref.fbId);
//		dictionary.Add("name",Game.Instance.localPlayerPref.name);
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().sendPrivateChat(userId,json);
//	}
//
//	public void SendPlayerInformation( PlayerInformation information, string userId){
//		var dictionary = new Dictionary<string,string>();
//		dictionary.Add("package","palyerInfo");
//		dictionary.Add("flagImageName",information.flagImageName);
//		dictionary.Add("teamName",information.teamName);
//		dictionary.Add("formation",information.formation.ToString());
//		string json = DictionaryToJson(dictionary);
//		Debug.Log(json);
//		WarpClient.GetInstance().sendPrivateChat(userId,json);
//
//	}
//	public void GetOnlineUsers( Action<List<string>> callback){
//		listener.OnReturnOnlinePlayers = null;
//		listener.OnReturnOnlinePlayers+= callback;
//
//		WarpClient.GetInstance().GetOnlineUsers();
//	}
//
//	void OnGUI()
//	{
//		GUI.contentColor = Color.black;
//		//GUI.Label(new Rect(10,10,500,200), listener.getDebug());
//	}
//
//	IEnumerator TryToConnect(){
//		bool connectionDone = false;
//		int i = 0;
//		while(!connectionDone){
//			i++;
//			if(this.playerName != ""/* && Game.Instance.isConnectionEstablished*/){
//				WarpClient.GetInstance().Connect(playerName);
//				connectionDone = true;
//			}
//			yield return null;
//		}
//		Debug.Log (i.ToString ());
//	}
//
//	IEnumerator TryToReconnect(){
//		bool isConnectionDone = false;
//		int connectionEstablishmentTries = 0;
//		//this.playerName = "";
//
//		while(!isConnectionDone){
//			if (connectionEstablishmentTries == 5) {
//				MessageWindow.OnOkPress +=()=>{	
//					Application.LoadLevelAsync ("MainMenu");
//				};
//				MessageWindow.Show("conexion","tu conexion se ha perdido. intenta recargar la pagina.");
//			}
//			connectionEstablishmentTries++;
//
//			Debug.Log ("TryToLogIn");
//			//if(this.playerName != ""/* && Game.Instance.isConnectionEstablished*/){
//			//WarpClient.GetInstance().Connect(playerName);
//			//	connectionDone = true;
//			//}
//			ParseManager.Instance.GetUserByProperty( "fbid", AccessToken.CurrentAccessToken.UserId, ( u ) => {
//				if(u != null) {
//					isConnectionDone = true;
//					StartCoroutine("TryToConnect");
//					Debug.Log ("TryToLogIn done");
//				}
//			});
//
//			yield return new WaitForSeconds(3);
//		}
//	}
//
//	void OnApplicationQuit(){
//		WarpClient.GetInstance().Disconnect();
//	}
//
//	string DictionaryToJson(Dictionary<string, string> dictionary)
//	{	
//		var values = dictionary.Select( d=>string.Format("\"{0}\":\"{1}\"",d.Key,d.Value));
//		return "{ " +string.Join(",",values.ToArray()) + " }";
//	}
//}