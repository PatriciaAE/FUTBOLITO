using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Parse;

public class SendRequestWindow : Window<SendRequestWindow> {

	[SerializeField] private GameObject itemScore;
	[SerializeField] private UIScrollView scrollview;
	[SerializeField] private UILabel messageLabel;
	[SerializeField] private Color oddColor;
	[SerializeField] private Color evenColor;
	[SerializeField] private int space;

	private List<FacebookFriend> facebookFriends;
	private List<ScoreContainer> scoreContainers;
	
	public static void Show(){
		if(!isRunning){
			_Show("SendRequestWindow");
			Instance.Initialize();
		}
	}

	protected override void Initialize ()
	{
		FacebookManager.Instance.GetFriendsUsingApp( ( f ) => {
			facebookFriends = f;
		} );

		
	}

	void Update () {

		if( facebookFriends != null && scoreContainers == null ) {
			PopulateList();
		}

	}

	void PopulateList () {
		scoreContainers = new List<ScoreContainer>();
		
		for( int i=0; i<facebookFriends.Count(); ++i ) {
			FacebookFriend friend = facebookFriends[i];
			GameObject go = Instantiate(this.itemScore) as GameObject;
			Transform t = go.transform;
			t.parent = scrollview.transform;
			t.localScale = Vector3.one;
			t.localPosition = new Vector3( 0, space * i, 0 );
			
			ScoreContainer scoreContainer = go.GetComponent<ScoreContainer>();
			scoreContainer.Initialize( friend.id, friend.name, 0 , i + 1);
			scoreContainer.ChangeColor( i % 2 == 0 ? oddColor : evenColor );
			scoreContainer.GetScoreLabel().text = "";
			
			scoreContainers.Add( scoreContainer );
		}
	}

	public void Invite(){
		FacebookManager.Instance.ChallengeWindow( ( r ) => {
			var parseIds = Game.Instance.invitations["invitations"].ToString().Split(',').ToList();
			var ids = r["to"] as List<object>;

			ids.ForEach( id=>{
				string fbid = id.ToString();
				if(!parseIds.Exists(fi=> fi == fbid)){
					parseIds.Add(fbid);
				}
			});

			Game.Instance.invitations["invitations"] = string.Join(",",parseIds.ToArray());
			Game.Instance.invitations.SaveAsync();
			Debug.Log(string.Join(",",parseIds.ToArray()));
		});                                                                                                               
	}

	public void Finish () {
		//Game.Instance.CheckRemainingMatches();

		if( Application.loadedLevelName == "MainMenu" ) {
			Close();
		}
		else {
			Application.LoadLevelAsync("MainMenu");
		}
	}

}
