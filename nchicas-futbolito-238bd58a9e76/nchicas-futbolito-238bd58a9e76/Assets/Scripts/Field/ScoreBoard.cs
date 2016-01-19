using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {
	[SerializeField]
	private UILabel teamAName;
	[SerializeField]
	private UILabel teamBName;
	[SerializeField]
	private UILabel scoreA;
	[SerializeField]
	private UILabel scoreB;
	
	public int ScoreA{
		get{
			return int.Parse(scoreA.text);
		}
	}

	public int ScoreB{
		get{
			return int.Parse(scoreB.text);
		}
	}

	public void UpScoreA(){
		scoreA.text = (int.Parse(scoreA.text) + 1).ToString();
	}

	public void UpScoreB(){
		scoreB.text = (int.Parse(scoreB.text) + 1).ToString();
	}

	public void SetTeamAName( string name){
		teamAName.text = name;
	}
	public void SetTeamBName(string name){
		teamBName.text = name;
	}

	public void Initialize(string teamAName, string teamBName){
		this.teamAName.text = teamAName;
		this.teamBName.text = teamBName;
		Reset();
	}

	public void Reset(){
		scoreA.text  = "0";
		scoreB.text  = "0";
	}

}
