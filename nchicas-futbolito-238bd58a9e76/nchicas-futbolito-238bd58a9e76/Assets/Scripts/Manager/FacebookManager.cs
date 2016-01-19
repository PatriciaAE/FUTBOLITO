using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.MiniJSON;
using Facebook.Unity;
using Facebook.Unity.Canvas;
using Facebook.Unity.Editor;
using Facebook.Unity.Mobile;
using Facebook.Unity.Mobile.Android;
using Facebook.Unity.Mobile.IOS;

public class FacebookManager : MonoBehaviour{

	private static readonly string APPID = "1517862941786096";

	private static FacebookManager instance;
	[SerializeField]
	private string invitationMessage="";
	[SerializeField]
	private string invitationTitle="";

	private Action<Dictionary<string,object>> challengeCallback;

	public static FacebookManager Instance{
		get{
			return  instance;
		}
	}
	
	private Action loginCallback;

	private List<string> permisions = new List<string>() {"email,publish_actions,user_about_me,user_friends"};

	public void Init(){
		FB.Init(SetInit,OnHideUnity);
	}
	/// <summary>
	/// Sets the init.
	/// </summary>
	private void SetInit(){

	}
	/// <summary>
	/// Raises the hide unity event.
	/// </summary>
	/// <param name="isGameShown">If set to <c>true</c> is game shown.</param>
	public void OnHideUnity(bool isGameShown){

	}

	void Awake(){
		if(instance == null	) instance = this;
	}

	public void LogIn(Action callback){
		this.loginCallback = null;
		this.loginCallback += callback;
		FB.LogInWithReadPermissions(this.permisions,LoginCallback);
	}

	public void LoginCallback(ILoginResult result){
		if(FB.IsLoggedIn){
			this.loginCallback();
		}
	}

	public void PostScore( int score){
		if(FB.IsLoggedIn){
			var query = new Dictionary<string,string>(){
				{"score",score.ToString()}
			};
			FB.API("/me/scores", Facebook.Unity.HttpMethod.POST, delegate(IGraphResult r) { Debug.Log("Result: " + r.RawResult); }, query);
		}
	}

	public void GetScore( Action<int> callback){
		Debug.Log("Getting Score");
		FB.API("me/scores?fields=score",Facebook.Unity.HttpMethod.GET,(r)=>{
			Debug.Log(r.RawResult);
			var responseobject = Json.Deserialize(r.RawResult) as Dictionary<string,object>;
			var dataDictionary = responseobject["data"] as List<object>;
			if(dataDictionary.Count ==1){
				var score = dataDictionary[0] as Dictionary<string,object>;
				Debug.Log("Exist " + score.ContainsKey("score").ToString());
				Debug.Log( score["score"].ToString());
				callback(int.Parse(score["score"].ToString()));
			}
		});
	}

	public void GetProfileData(Action<Dictionary<string,object>> callback){
		if(FB.IsLoggedIn){
			FB.API("/me",Facebook.Unity.HttpMethod.GET,delegate(IGraphResult result) {
				var responseObject = Json.Deserialize(result.RawResult) as Dictionary<string,object>;
				callback(responseObject);
			});
		}
	}
	public void GetLeaderBoard( Action<List<FacebookScore>> callback ){
		if( FB.IsLoggedIn ){
			FB.API( "/" + APPID + "/scores", Facebook.Unity.HttpMethod.GET, ( r ) => {
				var responseObject = Json.Deserialize(r.RawResult) as Dictionary<string,object>;
				var dataDictionary = responseObject["data"] as List<object>;

				List<FacebookScore> scores = new List<FacebookScore>();
				foreach( Dictionary<string,object> data in dataDictionary ) {
					FacebookScore score = new FacebookScore ();
					score.id = ((Dictionary<string,object>)data["user"])["id"] as string;
					score.name = ((Dictionary<string,object>)data["user"])["name"] as string;
					score.score = 10;
					scores.Add( score );
				}

				callback( scores );
			});

		}
		else {
			callback( null );
		}
	}


	public void BragScore(int score){
		/*FB.Feed(                                                                                                                 
		        linkCaption: string.Format("Tengo {0} puntos en el Futbolito Copa Pepsi,\n ¡Soy un goleador! ¿Cuántos tienes tú?",score),               
		        picture: "https://www.dropbox.com/s/kfwxstc7cpxbsb0/post.jpg?dl=1",                                                     
		        linkName: "¡Juega al Futbolito Copa Pepsi!",                                                                 
		        link: "https://www.facebook.com/PepsiElSalvador"       
		        );  */    
	}

	public void ChallengeWindow( Action<Dictionary<string,object>> callback ){
		challengeCallback = callback;

			/*FB.AppRequest(
				to: null,
				filters : "",
				excludeIds : null,
				message: this.invitationMessage,
				title: this.invitationTitle,
				callback:appRequestCallback
			);*/                                                                                                                
			
		}                                                                                                                              
	private void appRequestCallback (IAppRequestResult result)                                                                              
	{                                                                                                                              

		if (result != null)                                                                                                        
		{                                                                                                                          
			var responseObject = Json.Deserialize(result.RawResult) as Dictionary<string, object>;  
			Debug.Log(result.RawResult);
			object obj = 0;                                                                                                        
			if (responseObject.TryGetValue ("cancelled", out obj))                                                                 
			{                                                                                                                      
				UnityEngine.Debug.Log("Cancelled");                                                                                  
			}                

			else if (responseObject.TryGetValue ("request", out obj))                                                              
			{           
				MessageWindow.Show("Invitación","Tu invitación fue enviada!");
				challengeCallback( responseObject );
				challengeCallback = null;
			}                                                                                                                      
		}                                                                                                                          
	}    


	public void GetFriends( Action<List<FacebookFriend>> callback){
		if(FB.IsLoggedIn){
			FB.API("/me/friends",Facebook.Unity.HttpMethod.GET,delegate(IGraphResult result) {
				var responseObject = Json.Deserialize(result.RawResult) as Dictionary<string,object>;
				var friendsList = new List<FacebookFriend>();
				var dataDictionary = responseObject["data"] as List<object>;

				foreach(object friend in dataDictionary){
					var friendDictionary = friend as Dictionary<string,object>;
					friendsList.Add( new FacebookFriend{
						name = (string)friendDictionary["name"],
						id   = (string)friendDictionary["id"]
					});
				}
				callback(friendsList);
			});
		}
	}

	public void GetFriendsUsingApp( Action<List<FacebookFriend>> callback){
		if(FB.IsLoggedIn){
			FB.API("/me/friends?fields=installed,id,name",Facebook.Unity.HttpMethod.GET,delegate(IGraphResult result) {
				var responseObject = Json.Deserialize(result.RawResult) as Dictionary<string,object>;
				var friendsList = new List<FacebookFriend>();
				var dataDictionary = responseObject["data"] as List<object>;
				
				foreach(object friend in dataDictionary){
					var friendDictionary = friend as Dictionary<string,object>;
					friendsList.Add( new FacebookFriend{
						name = (string)friendDictionary["name"],
						id   = (string)friendDictionary["id"]
					});
				}
				callback(friendsList);
			});
		}
	}

	public void GetInvitedFriends( Action<List<FacebookFriend>> callback){
		if(FB.IsLoggedIn){
			string fql = "SELECT recipient_uid FROM apprequest WHERE recipient_uid IN (SELECT uid2 FROM friend WHERE uid1 = me()) AND app_id=" + APPID;
			fql = WWW.EscapeURL( fql );

			FB.API("/fql?q=" + fql,Facebook.Unity.HttpMethod.GET,delegate(IGraphResult result) {
				var responseObject = Json.Deserialize(result.RawResult) as Dictionary<string,object>;
				var friendsList = new List<FacebookFriend>();
				var dataDictionary = responseObject["data"] as List<object>;
				
				foreach(object friend in dataDictionary){
					var friendDictionary = friend as Dictionary<string,object>;
					friendsList.Add( new FacebookFriend{
						id   = (string)friendDictionary["recipient_uid"]
					});
				}
				callback(friendsList);
			});
		}
	}

	public void PostAction ( string action, string friendID ) {
		var querySmash = new Dictionary<string, string>();
		querySmash["profile"] = friendID;
		FB.API ("/me/" + FB.AppId + ":" + action, Facebook.Unity.HttpMethod.POST, 
		        delegate(IGraphResult r) { Debug.Log("Result: " + r.RawResult); }, querySmash);
	}

	public void GetProfilePicture(string userId ,Action<Texture2D> imageCallback ){
		StartCoroutine(GetImage(userId,imageCallback));
	}

	IEnumerator GetImage(string userId ,Action<Texture2D> imageCallback ){
		WWW url = new WWW(string.Format("https://graph.facebook.com/{0}/picture?type=large",userId));
		Texture2D texture = new Texture2D(128,128,TextureFormat.DXT1,false);
		yield return url;
		url.LoadImageIntoTexture(texture);
		imageCallback(texture);
	}


}

public class FacebookFriend{
	public string id;
	public string name;
	public FacebookFriend(){}

}

public class FacebookScore {
	public string id;
	public string name;
	public int score;
	public FacebookScore(){}
}