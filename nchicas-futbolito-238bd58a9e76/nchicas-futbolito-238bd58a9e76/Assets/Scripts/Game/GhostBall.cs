using UnityEngine;
using System.Collections;

public class GhostBall : MonoBehaviour {

	public Vector3 force;
	public Ball ball;
	public bool scored = false;
	public float distance = 1000000;

	void Start(){
		//Reference to the original ball;
		ball = CourtField.Instance.Ball;
	}

	public void Shoot(Vector3 force){
		this.transform.localPosition = CourtField.Instance.GetBallPosition();
		//Rest the variable 
		scored = false;
		//Save the force
		this.force = force;
		//Used temp variables for shor the code
		float maxMagnitud = ball.MaxMagnitud;
		float forceMultiplier = ball.ForceMultiplayer;
		//Code for make shoot
		float magnitud  = this.force.magnitude;
		if(magnitud > maxMagnitud) magnitud = maxMagnitud;
		this.GetComponent<Rigidbody2D>().AddForce(forceMultiplier*magnitud*this.force.normalized);
	}
}

