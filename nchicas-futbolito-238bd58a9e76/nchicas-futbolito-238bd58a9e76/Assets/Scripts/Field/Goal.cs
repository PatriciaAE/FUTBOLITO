using UnityEngine;
using System.Collections;
using System;
public class Goal : MonoBehaviour {

	public Action onGoalEnter;
	[SerializeField]
	private bool aiScore = false;
	void OnTriggerEnter2D(Collider2D collider){
		switch (collider.gameObject.tag){
		case "Ball":
			if (onGoalEnter!= null) this.onGoalEnter();
			break;
		case "GhostBall":
			collider.gameObject.GetComponent<GhostBall>().scored = aiScore;
			break;
		}
	}
}
