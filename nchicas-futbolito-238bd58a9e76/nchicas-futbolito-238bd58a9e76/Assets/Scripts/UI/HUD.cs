using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	public ScoreBoard scoreBoard;

	public GameTimer gameTimer;  
	
	public TurnTimer turnTimer;

	public UILabel TeamAName;
	public UILabel TeamBName;

	public GameObject pauseResumeButton;
	public GameObject pauseButton;
	public GameObject resumeButton;

	[SerializeField]
	private GameObject golAnimation;

	[SerializeField]
	private AudioClip golAudio;

	private static HUD instance;

	public static HUD Instance{
		get{
			return instance;
		}
	}

	void Awake(){
		if(instance == null) {
			instance = this;
		}else{
			DestroyObject(this);
		}

	}

	public void GolAnimation(){
		golAnimation.SetActive(true);
		AudioManager.Play(golAudio,AudioType.SFX);
		gameTimer.Pause();
		Game.Instance.state = Game.State.paused;
		InputManager.Instance.Lock();
		turnTimer.Pause();
		StartCoroutine("StopGoalAnimation");
	}
	
	IEnumerator StopGoalAnimation(){
		yield return new WaitForSeconds(4f);
		Game.Instance.state = Game.State.playing;
		gameTimer.Play();
		turnTimer.Play();
		InputManager.Instance.Unlock();
		golAnimation.SetActive(false);
	}

	public void OnPause () {
		gameTimer.Pause();
		turnTimer.Pause();
		Game.Instance.state = Game.State.paused;
		InputManager.Instance.Lock();
		Time.timeScale = 0;
		pauseButton.SetActive (false);
		resumeButton.SetActive (true);

	}

	public void OnResume () {
		Time.timeScale = 1;
		gameTimer.Play();
		turnTimer.Play();
		Game.Instance.state = Game.State.playing;
		InputManager.Instance.Unlock();
		resumeButton.SetActive (false);
		pauseButton.SetActive (true);
	}
}
