using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreWindow : Window<ScoreWindow> {

	PlayerInformation localPlayerInfo;
	PlayerInformation remotePlayerInfo;

	[SerializeField]
	private AudioClip alianzaCheer;

	[SerializeField]
	private AudioClip aguilaCheer;

	[SerializeField]
	private AudioClip fasCheer;

	[SerializeField]
	UIPlayerContainer playerAContainer;

	[SerializeField]
	UIPlayerContainer playerBContainer;

	[SerializeField]
	private GameObject AiContainer;

	[SerializeField]
	private UISprite aFlag;

	[SerializeField]
	private UISprite bFlag;

	[SerializeField]
	private UILabel scoreA;

	[SerializeField]
	private UILabel scoreB;

	[SerializeField]
	private UILabel teamNameA;
	
	[SerializeField]
	private UILabel teamNameB;

	[SerializeField]
	private UILabel nameAI;

	[SerializeField]
	private GameObject playAgainButon;

	[SerializeField]
	private GameObject backButton;

	public static void Show(PlayerInformation localPlayerInfo,PlayerInformation remotePlayerInfo){
		if(!isRunning){
			_Show("ScoreWindow");
			Instance.localPlayerInfo = localPlayerInfo;
			Instance.remotePlayerInfo =remotePlayerInfo;
			Instance.Initialize();
		}
	}

	protected override void Initialize ()
	{	
		if (Game.Instance.isMultiplayer) {
			this.playAgainButon.SetActive (false);

			Vector3 backButtonPosition = this.backButton.transform.position;
			backButtonPosition.x = 0;
			this.backButton.transform.position = backButtonPosition;
		}

		int scorePoints = 0;
		HUD hud = HUD.Instance;
		Game game = Game.Instance;
		hud.turnTimer.Pause();
		if(hud.scoreBoard.ScoreA == hud.scoreBoard.ScoreB){
			scorePoints = game.PointsForDraw;
		}

		teamNameA.text = hud.TeamAName.text;
		teamNameB.text = hud.TeamBName.text;

		nameAI.text = this.GetTeamNameAI (teamNameB.text);

		//Check Owner of scores 
		if(GameLoader.Instance.EnterOrder == 1){
			playerAContainer.SetData(this.localPlayerInfo.name, this.localPlayerInfo.fbId);
			aFlag.spriteName = this.localPlayerInfo.flagImageName;
			scoreA.text = hud.scoreBoard.ScoreA.ToString();

			//Check if the game is against other player or AI
			if(Game.Instance.isMultiplayer){
				playerBContainer.SetData(this.remotePlayerInfo.name, this.remotePlayerInfo.fbId);
				this.AiContainer.SetActive(false);
				this.playerBContainer.gameObject.SetActive(true);
			} else {
				bFlag.gameObject.SetActive (true);
			}

			bFlag.spriteName = this.remotePlayerInfo.flagImageName;
			scoreB.text = hud.scoreBoard.ScoreB.ToString();

			if(hud.scoreBoard.ScoreA >hud.scoreBoard.ScoreB){
				scorePoints = game.PointForWinner + (Game.Instance.isMultiplayer ? 5 : 0);
				this.SetCheerAudio (teamNameA.text);
			}
		}else{
			playerBContainer.SetData(this.localPlayerInfo.name, this.localPlayerInfo.fbId);
			bFlag.spriteName = this.localPlayerInfo.flagImageName;
			scoreB.text = HUD.Instance.scoreBoard.ScoreB.ToString();

			//Check if the game is against other player or AI
			if(Game.Instance.isMultiplayer){
				playerAContainer.SetData(this.remotePlayerInfo.name, this.remotePlayerInfo.fbId);
				this.AiContainer.SetActive(false);
				this.playerBContainer.gameObject.SetActive(true);
			} else {
				bFlag.gameObject.SetActive (true);
			}

			aFlag.spriteName = this.remotePlayerInfo.flagImageName;
			scoreA.text = HUD.Instance.scoreBoard.ScoreA.ToString();

			if(hud.scoreBoard.ScoreA < hud.scoreBoard.ScoreB){
				scorePoints = 10 + (Game.Instance.isMultiplayer ? 5 : 0);
				this.SetCheerAudio (teamNameB.text);
			}
		}

		//Save score in Parse
		int parseScore = int.Parse(Game.Instance.localPlayer["score"].ToString());
		parseScore += scorePoints;
		Game.Instance.localPlayer["score"] = parseScore;
		Game.Instance.localPlayer.SaveAsync();

		FacebookManager.Instance.PostScore( parseScore );

		
	}

	private void SetCheerAudio (string teamAbreviation) {
		AudioClip cheerAudio = null;

		switch (teamAbreviation) {
		case "ali":
			cheerAudio = alianzaCheer;
			break;

		case "agu":
			cheerAudio = aguilaCheer;
			break;

		case "fas":
			cheerAudio = fasCheer;
			break;

		default:
			break;
		}

		if (cheerAudio != null) {
			GameLoader.Instance.gameObject.GetComponent<AudioSource>().enabled = false;
			AudioManager.Play (cheerAudio, AudioType.Music);
		}
	}

	private string GetTeamNameAI (string abreviation) {
		string teamName = "";

		switch (abreviation) {
			case "juv":
				teamName = "juventud ind.";
				break;

			case "fas":
				teamName = "fas";
				break;

			case "stc":
				teamName = "santa tecla f.c.";
				break;

			case "ali":
				teamName = "alianza f.c.";
				break;

			case "psq":
				teamName = "pasaquina f.c.";
				break;

			case "agu":
				teamName = "aguila f.c.";
				break;

			case "met":
				teamName = "metapan";
				break;

			case "dra":
				teamName = "c.d. dragon";
				break;

			case "ues":
				teamName = "ues";
				break;

			case "mar":
				teamName = "atl. marte";
				break;

			case "fir":
				teamName = "l.a. firpo";
				break;

			default:
				teamName = "";
				break;
		}

		return teamName;
	}

	public void BackToMenu(){
		Application.LoadLevelAsync("MainMenu");
	}

	public void PlayAgain(){
//		int remainingMatches = int.Parse(Game.Instance.localPlayer["remainingMatches"].ToString());
//		if( remainingMatches == 0 ) {
//			SendRequestWindow.Show();
//			Close();
//			return;
//		}

		Application.LoadLevelAsync("Game");
	}
}
