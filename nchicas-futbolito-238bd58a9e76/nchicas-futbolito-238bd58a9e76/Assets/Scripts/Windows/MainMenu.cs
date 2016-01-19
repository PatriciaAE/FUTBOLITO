using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System.Linq;
public class MainMenu : MonoBehaviour {
	
	[SerializeField]
	private AudioClip mainTheme;
	[SerializeField]
	private string fantasyFutbolUrl;
	[SerializeField]
	private string policyUrl;
	[SerializeField]
	private string termsOfServiceUrl;
	[SerializeField]
	private UILabel gamesleft;

	public void Start(){
		Game.Instance.status = Game.Status.loby;
		Game.Instance.state = Game.State.none;
		AudioManager.Play(mainTheme,AudioType.Music);
		//StartCoroutine("CheckPoint");
		
	}

	IEnumerator CheckPoint(){
		bool displayed = false;
		while(!displayed){
			if( Game.Instance.localPlayer != null){
				displayed = true;
				gamesleft.text = Game.Instance.localPlayer["remainingMatches"].ToString();
			}
			yield return null;
		}
	}

	public void InviteFriends(){
		SendRequestWindow.Show();
	}

	public void OnPlayPress(){
		

//		if(Game.Instance.localPlayer != null && Game.Instance.localPlayer.ContainsKey("remainingMatches")){
//			int remainingMatches = int.Parse(Game.Instance.localPlayer["remainingMatches"].ToString());
//			if( remainingMatches == 0 ) {
//				SendRequestWindow.Show();
//				return;
//			}
//		}

		Game.Instance.SetGameMode(Game.Mode.SinglePlayer);
		TeamSelectionWindow.Show();

	}

	public void OnLeaderboardPress(){
		
		Application.LoadLevelAsync("Leaderboard");
	}

	public void OnOptionsPress(){
		
		Application.LoadLevelAsync("Rewards");
	}

	public void OnPolicityPress(){
		
		this.OpenURL(this.policyUrl,"Politicas");
	}

	public void OnTermsOfServicePress(){
		
		this.OpenURL(this.termsOfServiceUrl,"Terminos del Servicio");
	}

	public void ShowTutorial(){
		
		InstructionWindow.Show(false);
	}

	public void OnMultiplayerPress(){
		

//		if(Game.Instance.localPlayer != null && Game.Instance.localPlayer.ContainsKey("remainingMatches")){
//			int remainingMatches = int.Parse(Game.Instance.localPlayer["remainingMatches"].ToString());
//			if( remainingMatches == 0 ) {
//				SendRequestWindow.Show();
//				return;
//			}
//		}
		Game.Instance.SetGameMode(Game.Mode.Multiplayer);
		FriendsSelectionWindow.Show();
	}

	public void OnFantasyFutbolPress(){
		
		this.OpenURL(this.fantasyFutbolUrl,"Fantasy Futbol");
	}

	private void OpenURL(string url,string title){
		Application.ExternalEval(string.Format("window.open('{0}','_blank')",url));
	}

	public void UpdateGames () {
		gamesleft.text = Game.Instance.localPlayer["remainingMatches"].ToString();
	}
}
