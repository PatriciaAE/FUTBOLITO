using UnityEngine;
using System.Collections;

public class AIPlayer : Player {

	public enum Mode{
		Goku = 1,
		Human = 2,
		NoComment = 3,
	}

	public Mode mode = Mode.Goku;

	[SerializeField]
	private float timeToThink = 3;
	
	public override void TurnEnd ()
	{
		Debug.Log("AIEndTurn");
		Game.Instance.NextTurn();
	}

	public override void TurnStart ()
	{
		if(Game.Instance.state == Game.State.paused) {
			StartCoroutine("WaitForPlay");
		}else{
			Debug.Log("AIStartTurn");
			GhostBallManager.Instance.ShootAll();
			StartCoroutine("ThinkTime");
		}
	}

	IEnumerator ThinkTime(){
		yield return new WaitForSeconds(this.timeToThink);
		if(Game.Instance.state != Game.State.finish) {
			CourtField.Instance.Ball.Shoot( GhostBallManager.Instance.GetBestShoot((int)mode));
		}
	
	}

	IEnumerator WaitForPlay(){
		while(Game.Instance.state == Game.State.paused){
			yield return null;
		}
		TurnStart();
	}
}
