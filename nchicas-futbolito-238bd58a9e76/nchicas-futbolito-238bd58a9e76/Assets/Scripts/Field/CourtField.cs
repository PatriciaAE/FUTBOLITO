using UnityEngine;
using System.Collections;
using System;

public class CourtField : MonoBehaviour {
	[SerializeField]
	private Goal goalA;
	[SerializeField]
	private Goal goalB;
	[SerializeField]
	private Ball ball;
	[SerializeField]
	private Camera windowCamera;

	public FieldTeam fieldTeamA;
	public FieldTeam fieldTeamB;

	public Camera WindowCamera{
		get{
			return windowCamera;
		}
	}
	public Ball Ball{
		get{
			return ball;
		}
	}

	private Vector3 startBallPosition;

	private static CourtField instance;

	public static CourtField Instance{
		get{
			return instance;
		}
	}

	void Awake(){
		if(instance == null){
			instance = this;
			startBallPosition = ball.transform.localPosition;
		}else{
			Destroy(this);
		}
	}

	public void SetGoalAAction(Action callback){
		goalA.onGoalEnter += callback; 
	}

	public void SetOnStopBall(Action callback){
		ball.onBallStop +=callback;
	}

	public void SetGoalBAction(Action callback){
		goalB.onGoalEnter += callback; 
	}

	public Vector3 GetBallPosition(){
		return ball.transform.localPosition;
	}

	public void ResetBall(){
		ball.transform.localPosition = this.startBallPosition;
		ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
	}


}
