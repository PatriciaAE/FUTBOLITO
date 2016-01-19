using UnityEngine;
using System.Collections;

public class FBImageDisplay : MonoBehaviour {

	[SerializeField]
	private UITexture fbImage;
	[SerializeField]
	private bool wasRended = false;
	private string fbId = "";
	[SerializeField]
	private ScoreContainer scoreContainer;

	void Start(){
		this.fbId = scoreContainer.GetFbId();
	}

	void OnBecameVisible() {
		if(!wasRended){
			if( this.fbId != ""){
				this.wasRended = true;
				FacebookManager.Instance.GetProfilePicture(this.fbId, (texture)=>{
					fbImage.mainTexture = texture;
				});
			}else{
				StartCoroutine("WaitForFbId");
			}
		}
	}

	IEnumerator WaitForFbId(){
		while(!this.wasRended){
			if(this.fbId != ""){
				this.wasRended = true;
				FacebookManager.Instance.GetProfilePicture(this.fbId, (texture)=>{
					fbImage.mainTexture = texture;
				});
			}
			yield return null;
		}
	}
}
