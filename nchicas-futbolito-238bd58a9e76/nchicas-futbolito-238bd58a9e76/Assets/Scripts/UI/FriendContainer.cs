using UnityEngine;
using System.Collections;

public class FriendContainer : MonoBehaviour {


	public string id;
	public string name;
	[SerializeField]
	private UILabel labelName;
	[SerializeField]
	private UITexture facebookPicture;

	[SerializeField]
	private UISprite photoBorder;
	[SerializeField]
	private UISprite back;

	public bool selected = false;


	public void Initialize(string fbid , string name){
		labelName.text = this.name = name;
		this.id = fbid;
		FacebookManager.Instance.GetProfilePicture(fbid,(texture)=> {
		 	facebookPicture.mainTexture = texture;
		});
	}

	public void Selected(){
		FriendsSelectionWindow.Instance.Selected(this);
	}

	public void ChangeColor( Color color){
		this.photoBorder.color = color;
		this.back.color = color;
	}
}
