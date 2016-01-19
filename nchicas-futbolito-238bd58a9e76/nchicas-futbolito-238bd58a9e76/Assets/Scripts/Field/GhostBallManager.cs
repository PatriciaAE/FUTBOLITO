using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GhostBallManager : MonoBehaviour {
	[SerializeField]
	private List<GameObject> position;
	[SerializeField]
	private GameObject ghostBall;
	[SerializeField]
	float[] forces;
	[SerializeField]
	private List<GhostBall> ghostBalls;
	[SerializeField]
	private List<Vector3> forceVectors;
	[SerializeField]
	private float errorPosibility = 0.10f;

	[SerializeField]
	private int maxPlayedGames;

	private static GhostBallManager instance;

	[SerializeField]
	private int initialForces = 2;

	[SerializeField]
	private GameObject target;

	public static GhostBallManager Instance{
		get {
			return instance;
		}
	}

	void Start(){

		instance = this;
		int playedGames = int.Parse(Game.Instance.localPlayer["playedMatches"].ToString());
		float dificultyLevel =  playedGames/this.maxPlayedGames;

		int count = Mathf.Clamp( (int)((this.forces.Length - initialForces) * dificultyLevel) + initialForces, 0, forces.Length );
		this.forces = this.forces.ToList().GetRange(0, count).ToArray();
		this.errorPosibility = 0.750f - dificultyLevel*0.75f;

		int maxObjects = forces.Length*position.Count*2;
		Rigidbody2D ballRigiddody = CourtField.Instance.Ball.GetComponent<Rigidbody2D>();

		for(int i = 0 ; i < maxObjects ; i++){
			GameObject go = Instantiate( ghostBall) as GameObject;
			GhostBall gball = go.GetComponent<GhostBall>();
			go.transform.parent = transform;
			go.transform.localPosition = CourtField.Instance.GetBallPosition();
			go.transform.localScale = Vector3.one;
			//Add Data of the rigidBody
			go.GetComponent<Rigidbody2D>().mass = ballRigiddody.mass;
			go.GetComponent<Rigidbody2D>().angularDrag = ballRigiddody.angularDrag;
			go.GetComponent<Rigidbody2D>().drag = ballRigiddody.drag;
			ghostBalls.Add(gball);
		}

		Debug.Log( "GhostBalls : " + LayerMask.NameToLayer("GhostBalls"));
		gameObject.SetLayerRecursively( LayerMask.NameToLayer("GhostBalls"));
		CalculateForces();
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.white;
		position.ForEach( go=>{
			Vector3 p = 100*go.transform.localPosition;
			Gizmos.DrawRay(transform.position, -p);
			Gizmos.DrawRay(transform.position, new Vector3( p.x ,-p.y,p.z));
		});
	}

	void CalculateForces(){
		forceVectors = new List<Vector3>();
		for(int i=0 ; i < this.position.Count ; i++){
			for(int j=0; j <this.forces.Length ; j++){
				Vector3 forceVector = -this.position[i].transform.position.normalized *this.forces[j];
				this.forceVectors.Add(forceVector);
				this.forceVectors.Add( new Vector3(-forceVector.x,forceVector.y,forceVector.z));
			}
		}
	}

	public void ShootAll(){
		int index = 0;
		forceVectors.ForEach( v=>{
			ghostBalls[index++].Shoot(v);
		});
	}
	
	public Vector3 GetBestShoot(float needGols){
		var Goal = ghostBalls.Where( gb=> gb.scored == true);
		GhostBall response;
		if(Goal.Count() > needGols){
			response = Goal.ElementAt( UnityEngine.Random.Range(0,Goal.Count()-1));
		}else{
			Vector3 targetPosition = target.transform.localPosition;
			ghostBalls.ForEach( gb=>{
			gb.distance = Vector2.Distance(gb.transform.localPosition,targetPosition);
			});
			var posibleGB = ghostBalls.Where(gh=>gh.scored == false).OrderBy(gh=> gh.distance);
			//int a = (int)(this.errorPosibility*(posibleGB.Count() - 1));
			int index = Mathf.Clamp((int)(this.errorPosibility*posibleGB.Count()), 0, posibleGB.Count() - 1 );
			response = posibleGB.ElementAt( UnityEngine.Random.Range(0,index));
		}
		return response.force;
	}
}
