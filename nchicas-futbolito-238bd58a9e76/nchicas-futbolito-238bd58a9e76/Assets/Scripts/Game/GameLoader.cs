using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour {

	private static GameLoader instance;
	public bool AllPlayerIn = false;
	[SerializeField]
	private GameObject fieldTexture;
	[SerializeField]
	private GameObject fieldItems;

	public AudioClip gamePlayAudio;

	[SerializeField]
	private AudioClip gameEnd;
	public int EnterOrder = 0;
	public static GameLoader Instance{
		get{
			return instance;
		}
	}
	void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
		//Game.Instance.CheckRemainingMatches();

		AudioManager.Play(this.gamePlayAudio,AudioType.Music);
		Game game = Game.Instance;
		game.status = Game.Status.playing;
		game.state = Game.State.waiting;
		//Temp variables for short write
		CourtField courtField = CourtField.Instance;
		HUD hud = HUD.Instance;

		//Intialize Timer
		hud.gameTimer.Setup(Game.Instance.GameTime,()=>{
			if(!game.isMultiplayer){
				ScoreWindow.Show(game.localPlayerPref,game.aiRemotePlayerPref);
			}else{
				FacebookManager.Instance.PostAction("golear",game.remotePlayerPref.fbId);
				ScoreWindow.Show(game.localPlayerPref,game.remotePlayerPref);
			}
			AudioManager.Play(this.gameEnd,AudioType.SFX);
			game.state = Game.State.finish;
			Debug.Log("Game Finish");

			int remainingMatches = int.Parse(Game.Instance.localPlayer["remainingMatches"].ToString());
			int playedMatches = int.Parse(Game.Instance.localPlayer["playedMatches"].ToString());
			Game.Instance.localPlayer["remainingMatches"] = Mathf.Max( 0, remainingMatches - 1  );
			Game.Instance.localPlayer["playedMatches"] = playedMatches + 1;
			Game.Instance.localPlayer.SaveAsync();
		});




		//Set the action when the team B Scores
		courtField.SetGoalAAction( ()=>{
			CommondInGoal();
			hud.scoreBoard.UpScoreB();
		});
		//Set the action when the team A Scores
		courtField.SetGoalBAction( ()=>{
			CommondInGoal();
			hud.scoreBoard.UpScoreA();
		});

		courtField.SetOnStopBall( ()=>{
			Game.Instance.ActualPlayer.TurnEnd();
		});

		List<Player> playerTemp = new List<Player>();
		switch( Game.Instance.GameMode){
		case Game.Mode.Multiplayer:
			hud.pauseResumeButton.SetActive (false);
			LoadingWindow.Show(120,string.Format("esperando a {0}",game.remotePlayerPref.name), ()=>{
				LoadingWindow.Close();
				MessageWindow.OnOkPress += ()=>{
					LoadingWindow.Show(0,"cargando menu");
					Application.LoadLevelAsync("MainMenu");
				};
				MessageWindow.Show("conexion",string.Format("{0} no logro conectarse",game.remotePlayerPref.name),MessageWindow.Option.Ok);
			});
			break;
		case Game.Mode.SinglePlayer:

			this.EnterOrder = 1;

			game.Players.Clear();

			courtField.fieldTeamA.ShowFormation(game.localPlayerPref.formation);
			courtField.fieldTeamB.ShowFormation(game.aiRemotePlayerPref.formation);

			hud.TeamAName.text = game.localPlayerPref.teamName;
			hud.TeamBName.text = game.aiRemotePlayerPref.teamName;
			
			playerTemp.Add( GetComponent<LocalPlayer>());
			playerTemp.Add( GetComponent<AIPlayer>());

			game.SetupPlayers(playerTemp);
			game.ActualPlayer.TurnStart();
			game.state = Game.State.playing;

			hud.gameTimer.Init();
			hud.turnTimer.Init();

			InstructionWindow.Show();
			break;
		}

	}

	public void CommondInGoal(){
		CourtField.Instance.ResetBall();
		HUD.Instance.GolAnimation();
	}

	public void SetMultiplayerGame(int i){
		HUD hud = HUD.Instance;
		CourtField courtField = CourtField.Instance;
		Game game = Game.Instance;
		List<Player> playerTemp = new List<Player>();
		Game.Instance.Players.Clear();
		switch(i){
		case 1:
			this.EnterOrder = 1;
			StartCoroutine("WaitForPlayer2");
			break;
		case 2:
			this.EnterOrder =2;
			game.state = Game.State.playing;
			playerTemp.Add( GetComponent<RemotePlayer>());
			playerTemp.Add( GetComponent<LocalPlayer>());
			Game.Instance.SetupPlayers(playerTemp);

			
			courtField.fieldTeamB.ShowFormation(game.localPlayerPref.formation);
			courtField.fieldTeamA.ShowFormation(game.remotePlayerPref.formation);
			
			hud.TeamBName.text = game.localPlayerPref.teamName;
			hud.TeamAName.text = game.remotePlayerPref.teamName;

			//Field and camera position to match with new field orientation
			CourtField.Instance.WindowCamera.transform.localRotation = Quaternion.Euler (new Vector3(0, 0, 180));//.AngleAxis(180,Vector3.right);
			CourtField.Instance.WindowCamera.transform.localPosition = new Vector3(4,-12,0);
			fieldTexture.transform.localRotation = Quaternion.Euler (new Vector3(0, 0, 180));//AngleAxis(180,Vector3.right);
			fieldTexture.transform.localPosition = new Vector3(4,-12,0);

			Game.Instance.ActualPlayer.TurnStart();
			hud.gameTimer.Init();
			LoadingWindow.Close();
			break;
		}
	}

	IEnumerator WaitForPlayer2(){
		while(this.AllPlayerIn == false){
			yield return null;
		}

		HUD hud = HUD.Instance;
		CourtField courtField = CourtField.Instance;
		Game game = Game.Instance;

		game.state = Game.State.playing;

		List<Player> playerTemp = new List<Player>();

		playerTemp.Add( GetComponent<LocalPlayer>());
		playerTemp.Add( GetComponent<RemotePlayer>());
		
		courtField.fieldTeamA.ShowFormation(game.localPlayerPref.formation);
		courtField.fieldTeamB.ShowFormation(game.remotePlayerPref.formation);
		
		hud.TeamAName.text = game.localPlayerPref.teamName;
		hud.TeamBName.text = game.remotePlayerPref.teamName;
		
		Game.Instance.SetupPlayers(playerTemp);
		
		Game.Instance.ActualPlayer.TurnStart();
		hud.gameTimer.Init();
		LoadingWindow.Close();
	}
}
