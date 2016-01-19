using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;

public class FriendsSelectionWindow : Window<FriendsSelectionWindow> {

	[SerializeField]
	private Color selected;
	[SerializeField]
	private Color unselected;
	[SerializeField]
	private FriendContainer selectedFriend;

	[SerializeField]
	private GameObject itemFriend;
	[SerializeField]
	private UIScrollView scrollview;

	private List<FacebookFriend> friends;
	private List<string> onlineUsers;
	[SerializeField]
	private List<FriendContainer> friendsContainer;

	[SerializeField]
	private int spaceCell;
	
	public static void Show(){
		if(!isRunning){
			_Show("FriendSelectionWindow");
			Instance.Initialize();
		}
	}

	protected override void Initialize ()
	{	

		

		this.friends = null;
		this.onlineUsers = null;
		FacebookManager.Instance.GetFriends( (listFriends)=>{
			this.friends = listFriends;
			this.CheckOnlineFriends();
			Debug.Log("Facebook");
		});
		/*MultiplayerManager.Instance.GetOnlineUsers( (listPlayers)=>{
			this.onlineUsers = listPlayers;
			Debug.Log("User count : " + listPlayers.Count);
			this.CheckOnlineFriends();
			Debug.Log("OnLineUsers");
		});*/
		LoadingWindow.Show(0, "buscando jugadores");
	}

	public void CheckOnlineFriends(){
		Debug.Log("CheckOnlineFriends");
		this.friendsContainer = new List<FriendContainer>();
		int totalSpace = 0;

		if(this.friends != null && this.onlineUsers!=null){
			this.friends.ForEach(f=>{
				if(this.onlineUsers.Contains(f.id)){
					InsertUserInfo( f.id,f.name, totalSpace);
					totalSpace += this.spaceCell;
				}
			});
			List<string> onlinePlayers = onlineUsers.Where( u=> !this.friends.Exists(f=>f.id == u) && u != AccessToken.CurrentAccessToken.UserId).ToList();
			if(onlinePlayers.Count > 0){
				ParseManager.Instance.GetUsersWithPredicate(onlinePlayers,(fbFriends)=>{
					if(fbFriends !=null && isOpen){
						fbFriends.ForEach( f=>{
							InsertUserInfo( f.id,f.name, totalSpace);
							totalSpace += this.spaceCell;
						});
					}
					scrollview.UpdatePosition();
					scrollview.UpdateScrollbars();
				});
			}
			LoadingWindow.Close();
			scrollview.UpdatePosition();
			scrollview.UpdateScrollbars();
		}
	}
	
	private void InsertUserInfo(string fbid,string name,int totalSpace){
				GameObject go = Instantiate(this.itemFriend) as GameObject;
				Transform t = go.transform;
				t.parent = scrollview.transform;
				t.localScale = Vector3.one;
				t.localPosition = Vector3.zero + new Vector3(0,totalSpace,0);
				FriendContainer friendCointainer = go.GetComponent<FriendContainer>();
				friendCointainer.Initialize(fbid,name);
				this.friendsContainer.Add(friendCointainer);
	}

	public void Selected ( FriendContainer friendContainer){
		var lastSelected = friendsContainer.Where( c=> c.selected ==true);

		lastSelected.ToList().ForEach( item=>{
			item.ChangeColor(unselected);
			item.selected = false;
		});
		this.selectedFriend = friendContainer;
		friendContainer.ChangeColor(selected);
		friendContainer.selected = true;
	}


	public void Back(){
		Close();
	}

	public void InviteFriend(){
		FacebookManager.Instance.ChallengeWindow( ( r )=>{
			//Update remainingMatches
		});
	}

	public void Next(){
		if(this.selectedFriend != null){
			Game.Instance.remotePlayerPref.name = this.selectedFriend.name;
			Game.Instance.remotePlayerPref.fbId = this.selectedFriend.id;
			//MultiplayerManager.Instance.SendInvitation( this.selectedFriend.id);
			LoadingWindow.Show(40,string.Format("esperando respuesta de {0}",this.selectedFriend.name),()=>{
				LoadingWindow.Close();
				MessageWindow.Show("invitacion",string.Format("no se obtuvo respuesta de {0}",this.selectedFriend.name));
			});
		}
	}
}
