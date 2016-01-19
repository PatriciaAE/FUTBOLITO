using UnityEngine;
using System.Collections;

public class ScoreContainer : MonoBehaviour {
	
	public string fbid;
	public string name;
	public int score;

	[SerializeField]
	private UILabel labelName;
	[SerializeField]
	private UILabel labelScore;

	[SerializeField]
	private UISprite photoBorder;
	[SerializeField]
	private UISprite back;

	public bool selected = false;

	[SerializeField]
	private ScoreContainer ownScore;

	[SerializeField]
	private UILabel rankPosition;

	public void Initialize( string fbid, string name, int score ){
		this.fbid = fbid;
		this.name = name;
		this.score = score;
		//labelName.text = name;
		labelScore.text = score.ToString();
	}

	public void Initialize( string fbid, string name, int score ,int rank){
		this.fbid = fbid;
		this.name = name;
		this.score = score;
		rankPosition.text = rank.ToString();
		labelName.text = name;
		labelScore.text = score.ToString();
	}


	public string GetFbId(){
		return this.fbid;
	}
	public void Selected(){
		this.selected = true;
	}

	public void ChangeColor( Color color){
		this.photoBorder.color = color;
		this.back.color = color;
	}

	public UILabel GetNameLabel() {
		return labelName;
	}

	public UILabel GetScoreLabel() {
		return labelScore;
	}
}
