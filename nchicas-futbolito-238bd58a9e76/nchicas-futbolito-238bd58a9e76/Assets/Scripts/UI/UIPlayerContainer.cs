using UnityEngine;
using System.Collections;

public class UIPlayerContainer : MonoBehaviour {
	[SerializeField]
	private UILabel   name;

	public UITexture photo;

	public void SetData( string name, string userId){
		string[] nameSplited = name.Split(' ');
		this.name.text = (nameSplited.Length > 2 ) ? string.Format("{0} {1}",nameSplited[0],nameSplited[1]) : name;
		FacebookManager.Instance.GetProfilePicture(userId,(texture)=>{
			photo.mainTexture = texture;
		});
	}
}
