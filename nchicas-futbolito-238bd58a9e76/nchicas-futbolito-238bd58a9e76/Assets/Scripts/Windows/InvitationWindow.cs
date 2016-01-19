using UnityEngine;
using System.Collections;

public class InvitationWindow : Window<InvitationWindow> {


	private string fbId;
	private string playerName;

	[SerializeField]
	private UITexture fbPicture;
	[SerializeField]
	private UILabel message;

	public static void Show(string fbId,string name){
		if(!isRunning){
			_Show("InvitationWindow");
			Instance.fbId = fbId;
			Instance.playerName = name;
			Instance.message.text = string.Format(name);
			Instance.Initialize();
		}
	}

	protected override void Initialize ()
	{
		

		FacebookManager.Instance.GetProfilePicture(Instance.fbId,(texture)=> {
			Instance.fbPicture.mainTexture = texture;
		});
		//Wait for 35 seconds for an answer
		StartCoroutine(WaitForAnswer(35));
	}

	public void Denied(){
		
		//MultiplayerManager.Instance.DeclineInvitation(fbId);
		Close();
	}

	public void Acept(){
		
		//MultiplayerManager.Instance.AceptInvitation(fbId);
		Game.Instance.SetGameMode(Game.Mode.Multiplayer);
		Game.Instance.remotePlayerPref.fbId = this.fbId;
		Game.Instance.remotePlayerPref.name = this.playerName;
		FriendsSelectionWindow.Close();
		TeamSelectionWindow.Show();
		Close();
	}

	IEnumerator WaitForAnswer(float time){
		yield return new WaitForSeconds(time);
		Close();
	}
}
