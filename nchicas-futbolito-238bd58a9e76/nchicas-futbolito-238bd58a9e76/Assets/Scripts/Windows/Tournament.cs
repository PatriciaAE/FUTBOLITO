using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System.Linq;
public class Tournament : MonoBehaviour {
	
	[SerializeField]
	private AudioClip mainTheme;

	public void Start(){
		Game.Instance.status = Game.Status.loby;
		Game.Instance.state = Game.State.none;
		AudioManager.Play(mainTheme,AudioType.Music);
		Game.Instance.SetGameMode(Game.Mode.Multiplayer);
	}

	public void ShowTutorial(){
		
		InstructionWindow.Show(false);
	}

	public void Play () {
		FormationWindow.Show();
	}
}
