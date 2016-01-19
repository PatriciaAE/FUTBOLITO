using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using Facebook.Unity;
using Parse;

/// <summary>
/// Game.
/// </summary>
public class Game : Singleton<Game> {

	public enum Status{
		loby,
		playing,
	}

	public enum State{
		none,
		waiting,
		playing,
		paused,
		finish,
	}
	public enum Mode{
		Multiplayer,
		SinglePlayer,
	}
	/// <summary>
	/// The instance.
	/// </summary>
	/*private static Game instance;
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static Game Instance{
		get{
			return instance;
		}
	}*/

	#region private_variables
	/// <summary>
	/// The players.
	/// </summary>
	private List<Player> players = new List<Player>();
	/// <summary>
	/// The game time in seconds.
	/// </summary>
	[SerializeField]
	private float gameTime;
	/// <summary>
	/// The time that each player has in their turn.
	/// </summary>
	[SerializeField]
	private float turnTime;
	/// <summary>
	/// The actual player.
	/// </summary>
	private Player actualPlayer;
	/// <summary>
	/// The game mode.
	/// </summary>
	[SerializeField]
	private Mode gameMode = Mode.Multiplayer;
	/// <summary>
	/// The points for winner.
	/// </summary>
	[SerializeField]
	private int pointsForWinner;
	/// <summary>
	/// The points for draw.
	/// </summary>
	[SerializeField]
	private int pointsForDraw;
	#endregion

	#region public_variables
	public PlayerInformation localPlayerPref    = new PlayerInformation();
	public PlayerInformation remotePlayerPref   = new PlayerInformation();
	public PlayerInformation aiRemotePlayerPref = new PlayerInformation();
	public ParseObject localPlayer;
	public ParseObject invitations;
	public Status status = Status.loby;
	public State state = State.none;
	public bool isConnectionEstablished = false;
	public int connectionEstablishmentTries = 0;
	public int firpoUnlockScore;
	#endregion
	#region properties

	public int PointForWinner{
		get{
			return pointsForWinner;
		}
	}
	public int PointsForDraw{
		get{
			return pointsForDraw;
		}
	}
	public bool isMultiplayer{
		get{
			return this.gameMode == Mode.Multiplayer;
		}
	}
	/// <summary>
	/// Gets the players.
	/// </summary>
	/// <value>The players.</value>
	public List<Player> Players{
		get{
			return players;
		}
	}
	/// <summary>
	/// Gets the game time.
	/// </summary>
	/// <value>The game time.</value>
	public float GameTime{
		get{
			return gameTime;
		}
	}
	/// <summary>
	/// Gets the turn time.
	/// </summary>
	/// <value>The turn time.</value>
	public float TurnTime{
		get{
			return turnTime;
		}
	}
	public Player ActualPlayer{
		get{
			return actualPlayer;
		}
	}

	public Mode GameMode{
		get{
			return gameMode;
		}
	}
	#endregion
	
	// Use this for initialization
	/*void Awake () {
		if(instance == null) 
			instance = this;
			DontDestroyOnLoad( gameObject );
		}
	}*/

	protected override void Start() {
		base.Start();
		LoadingWindow.Show (0, "iniciando juego");//Iniciando el juego \n Recuerda que puedes \n retar a tus amigos");
		//FacebookLoginWindow.Show();
		FB.Init(()=>{ 
			//Ask the user to login
			if(!FB.IsLoggedIn){
				FacebookManager.Instance.LogIn(()=>{
					FacebookLoginWindow.Close();
					LogIn();
					//StartTryToLogIn ();
				});
			}else{
				LogIn();
				//StartTryToLogIn ();
			}
		});

		//Initializing Loom on main thread
		Loom.Current.ToString();
	}
	
	public void LogIn(){
		FacebookManager.Instance.GetProfileData( (dictionary)=>{
			localPlayerPref.fbId = (string)dictionary["id"];
			string name = (string)dictionary["name"];
			string[] nameSplited = name.Split(' ');
			localPlayerPref.name = (nameSplited.Length>2) ? string.Format("{0} {1}",nameSplited[0],nameSplited[2]) : name;
		});
		/*MultiplayerManager.Instance.OnGameConnected(  ()=>{

		});
		MultiplayerManager.Instance.ConnectUser(AccessToken.CurrentAccessToken.UserId);*/



		ParseManager.Instance.GetUserByProperty( "fbid", AccessToken.CurrentAccessToken.UserId, ( u ) => {
			if(u == null) {
				Debug.Log ("u == null");
				FacebookManager.Instance.GetProfileData( ( d ) => {
					d.Add( "fbid", d["id"] );
					d.Remove("id");

					ParseManager.Instance.SaveUser( d, ( us ) => {
						Debug.Log("Save_User");
						if(us != null) {
							isConnectionEstablished = true;
							localPlayer = us;
							localPlayer["score"] = 0;
							localPlayer["remainingMatches"] = 5;
							localPlayer["playedMatches"] = 0;
							localPlayer["availableMatches"] = 5;
							//localPlayer.SaveAsync();
							DetectCountry();
						}else{
							MessageWindow.Show("problema de conexion","vuelve a intentarlo dentro de unos minutos.");
						}
					});
				});
				LoadingWindow.Close();
			}
			else {
				isConnectionEstablished = true;
				Debug.Log("u != null");
				localPlayer = u;
				DetectCountry();
				//CheckRemainingMatches();
				LoadingWindow.Close();
			}
		});

//		ParseManager.Instance.GetUserTiket(AccessToken.CurrentAccessToken.UserId,(parseObjects)=>{
//			if(parseObjects != null && parseObjects.Count() > 0){
//				TicketWindow.Show(parseObjects);
//			}
//		});
	}

	public void StartTryToLogIn () {
		StartCoroutine ("TryToLogIn");
	}
	
	IEnumerator TryToLogIn () { 
		while (!isConnectionEstablished) {
			if (connectionEstablishmentTries == 5) {
				MessageWindow.OnOkPress +=()=>{	
					Application.LoadLevelAsync ("MainMenu");
				};
				MessageWindow.Show("conexion","tu conexion se ha perdido. intenta recargar la pagina.");
			}
			connectionEstablishmentTries++;
			Debug.Log ("TryToLogIn");
			LogIn ();
			yield return new WaitForSeconds(3);
		}
		Debug.Log ("TryToLogIn done");
		connectionEstablishmentTries = 0;
	}

	private void RegisterUser () {
//		if( !localPlayer.ContainsKey("team") ) {
//			RegisterWindow.Show();
//		}
	}

	#region Setup
	/// <summary>
	/// Setups the players.
	/// </summary>
	/// <param name="players">Players.</param>
	public void SetupPlayers ( List<Player> players){
		this.players = players;
		//Set Up References for players
		this.players[0].nextPlayer = this.players[1];
		this.players[1].nextPlayer = this.players[0];
		//TempCode
		this.actualPlayer = this.players[0];
	}

	public void SetGameMode(Game.Mode mode){
		this.gameMode = mode;
	}
	#endregion

	#region Game_logic
	/// <summary>
	/// Nexts the turn.
	/// </summary>
	public void NextTurn(){
		//Change the player
		Debug.Log("NextTurn");
		this.actualPlayer = this.actualPlayer.nextPlayer;
		Debug.Log( "iS AI Player ? "  + this.actualPlayer is AIPlayer);
		this.actualPlayer.TurnStart();
	}
	#endregion

	public void DetectCountry () {
		if( !localPlayer.ContainsKey("country") ) {
			StartCoroutine( SaveCountry() );
		}
	}

	IEnumerator SaveCountry () {
//		string url = "http://geoip.smart-ip.net/json?callback=?";
//		
//		WWW request = new WWW( url );
//		yield return request;
//		
//		string json = request.text.TrimStart( "?(".ToCharArray() ).TrimEnd( ");".ToCharArray() );
//		Dictionary<string,object> locationInfo = Json.Deserialize( json ) as Dictionary<string,object>;
//		
//		if( locationInfo == null || localPlayer == null ) {
//			yield break;
//		}

		localPlayer ["country"] = "SV";//locationInfo["countryCode"] as string;

		RegisterUser();

		yield return null;
	}

	private IEnumerable<string> facebookFriends;
	private IEnumerable<string> invitedFriends;
	private List<string> invitedFriendsParse;



	public void UpdateMatches(){

		if( facebookFriends != null && invitedFriends !=null && invitedFriendsParse != null) {

			Debug.Log("All invitedFriends : " + invitedFriends.Count());
			Debug.Log("Facebook Friends : " + facebookFriends.Count());
			Debug.Log("Facebook Friends Parse" + invitedFriendsParse.Count());



			int a = invitedFriends.Intersect( facebookFriends ).Count() * 3;
			Debug.Log("A data :" + a);
			int b = invitedFriends.Except( facebookFriends ).Count() + invitedFriendsParse.Except(facebookFriends).Count();
			Debug.Log("B data : "  + b);
			localPlayer["availableMatches"] = a + b + 5;
			int played = localPlayer.ContainsKey("playedMatches") ? int.Parse( localPlayer["playedMatches"].ToString() ) : 0;
			int available = localPlayer.ContainsKey("availableMatches") ? int.Parse( localPlayer["availableMatches"].ToString() ) : 3;
			
			Debug.Log( "Remaining Matches : " + Mathf.Max( 0, available - played ));
			
			localPlayer["remainingMatches"] = Mathf.Max( 0, available - played );
			
			localPlayer["playedMatches"] = played;
			localPlayer.SaveAsync();
			
			MainMenu menu = FindObjectOfType(typeof(MainMenu)) as MainMenu;
			if( menu ) {
				menu.UpdateGames();
			}
		}
	}

	public void CheckRemainingMatches () {

		FacebookManager.Instance.GetInvitedFriends( ( f ) => {
			invitedFriends = f.Select( ff => ff.id ).Distinct();
			UpdateMatches();
		});
		
		FacebookManager.Instance.GetFriendsUsingApp( ( f ) => {
			facebookFriends = f.Select( ff => ff.id ).Distinct();
			UpdateMatches();
		});
		if(invitations == null){
			ParseManager.Instance.GetUserInvitation(AccessToken.CurrentAccessToken.UserId,(i)=>{
				if(i!= null){
					invitations = i;
					if(i.ContainsKey("invitations")){
						invitedFriendsParse =  i["invitations"].ToString().Split(',').ToList();
					}else{
						invitedFriendsParse = new List<string>();
					}
				}else{
					Dictionary<string,object> invitation = new Dictionary<string, object>();
					invitation.Add("fbid",AccessToken.CurrentAccessToken.UserId);
					invitation.Add("invitations","");
					ParseManager.Instance.SaveInvitation(invitation,(po)=>{
						invitations = po;
					});
					invitedFriendsParse = new List<string>();
				}
				});
		}else{
			invitedFriendsParse =  invitations["invitations"].ToString().Split(',').ToList();
		}
	}
}
