using UnityEngine;
using System.Collections;
using Parse;

public class RewardContainer : MonoBehaviour {

	public ParseObject reward;
	public UILabel labelName;
	public UILabel labelScore;
	public UILabel rankNumber;
	public UILabel rankSuperscript;
	public UISprite image;
	public UISprite cansImage;
	public UISprite shirtsImage;
	
	[SerializeField]
	private UISprite back;

	public bool selected = false;
//	public string description;
//	public int scoreToWin;
//	public bool isCollectable = false;

	public void Initialize ( ParseObject reward /*string name, string description*/ ){
		this.reward = reward;
		labelName.text = (string)reward["name"];
		labelScore.text = reward["goal"].ToString();
//		this.description = description;
	}

//	public void Initialize (string name, string description, int scoreToWin){
//		labelName.text = name;
//		labelScore.text = "";
//		this.description = description;
//		this.scoreToWin = scoreToWin;
//		this.isCollectable = true;
//	}

	public void Selected(){
		int remainingItems = int.Parse( reward["remaining"].ToString() );
		string message = (string)reward["description"];
		MessageWindow.Show( labelName.text, message );
	}

	public void ChangeColor( Color color){
		this.back.color = color;
	}
}
