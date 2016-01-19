using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parse;
using Facebook.MiniJSON;

public class ParseManager : Singleton<ParseManager> {

	#region Constants

	private static readonly string PLAYER = "Player";
	private static readonly string REWARD = "Reward";
	private static readonly string TICKET = "Ticket";
	private static readonly string INVITATION = "Invitation";

	private static readonly string parse_id  = "E6pxcItSetvqyMpufdkrcfegM9FF3qXw6GmNwMdl";
	private static readonly string parse_key = "wtzlS5N4yGXw0ubzvAAejMs3r08C4wJSXa6T6iPX";
	private static ParseObject po = new ParseObject("YOLO");
	#endregion
	
	public void SaveUser ( Dictionary<string,object> data, 
	                       Action<ParseObject> callback ) {

		/*ParseObject user = new ParseObject( PLAYER );
		foreach( KeyValuePair<string,object> kvp in data ) {
			user[ kvp.Key ] = kvp.Value;
		}

		Task saveTask = user.SaveAsync();
		saveTask.ContinueWith( t => {
			Loom.QueueOnMainThread( ()=> {
				callback( t.IsFaulted ? null : user );
				//callback( user );
			});
		});*/

		foreach( var kvp in data ) {
			po.Add( kvp.Key, kvp.Value );
		}

		callback(po);
	}

	public void SaveInvitation(Dictionary<string,object> data, Action<ParseObject> callback){
		/*ParseObject invitation = new ParseObject(INVITATION);
		foreach(KeyValuePair<string,object> kvp in data){
			invitation[kvp.Key] = kvp.Value;
		}

		Task saveTask = invitation.SaveAsync();
		saveTask.ContinueWith( t => {
			Loom.QueueOnMainThread( ()=> {
				callback( t.IsFaulted ? null : invitation);
				//callback( user );
			});
		});*/

		callback(po);
	}
	public void GetUserByProperty ( string property, object value, Action<ParseObject> callback ) {

		/*var query = ParseObject.GetQuery( PLAYER )
							   .WhereEqualTo( property, value );
		query.FirstOrDefaultAsync().ContinueWith( t => {
			Loom.QueueOnMainThread( ()=> {
				callback( t.IsFaulted ? null : t.Result );
			});
		});*/


		callback(null);
	}

	public void GetUserInvitation(string fbid,Action<ParseObject> callback ) {
		
		/*var query = ParseObject.GetQuery( INVITATION )
			.WhereEqualTo( "fbid", fbid );
		query.FirstAsync().ContinueWith( t => {
			Loom.QueueOnMainThread( ()=> {
				callback( t.IsFaulted ? null : t.Result );
			});
		});*/
		
		callback(po);
	}

	public void GetTop100UsersByScore ( Dictionary<string,object> predicates, 
	                         		   Action<IEnumerable<ParseObject>> callback ) {

		/*var query = ParseObject.GetQuery( PLAYER );
		foreach( KeyValuePair<string,object> kvp in predicates ) {
			query.WhereEqualTo( kvp.Key, kvp.Value );
		}
		query.WhereEqualTo("country","SV");
		query.OrderByDescending("score");
		query.Limit( 100 );

		query.FindAsync().ContinueWith( t => {
			callback( t.IsFaulted ? null : t.Result );
		});*/
	}




	public void GetUserTiket(string fbId, Action<IEnumerable<ParseObject>> callback){

		/*var query = ParseObject.GetQuery(TICKET)
							   .WhereEqualTo("fbid",fbId)
							   .WhereEqualTo("reedemed",false);

		query.OrderByDescending("registrationDate");

		query.FindAsync().ContinueWith( t => {
			Loom.QueueOnMainThread( ()=> {
				callback( t.IsFaulted ? null : t.Result );
			});
		});*/
	}

	public void GetUsersWithPredicate (List<string> usersId, 
	                                   Action<List<FacebookFriend>> callback) {

		StartCoroutine( GetUser(usersId,callback));

	}


	
	public void GetTop150Players(string country,Action<List<ParsePlayer>> callback){
		Dictionary<string,object> parameters = new Dictionary<string,object>(){
			{"country",country}
		};
		
		Debug.Log("Before Parse");
		ParseCloud.CallFunctionAsync<object>("GetTop150", parameters)
			.ContinueWith(t=>{
				List<ParsePlayer> parsePlayers = new List<ParsePlayer>();
				var content = t.Result as List<object>;
				foreach( ParseObject user in content){
					parsePlayers.Add( new ParsePlayer {
						fbid  = user["fbid"].ToString(),
						name  = user["name"].ToString(),
						score = int.Parse(user["score"].ToString()),
						team  = (user.ContainsKey("team")) ? user["team"].ToString() : "Super Equipo"
					});
				}
				callback(parsePlayers);
			});
	}

	public void GetTop100Players(Action<List<ParsePlayer>> callback){
		Dictionary<string,object> parameters = new Dictionary<string,object>();

		Debug.Log("Before Parse");
		ParseCloud.CallFunctionAsync<object>("GetTop100", parameters)
			.ContinueWith(t=>{
				List<ParsePlayer> parsePlayers = new List<ParsePlayer>();
				var content = t.Result as List<object>;
				foreach( ParseObject user in content){
							parsePlayers.Add( new ParsePlayer {
									fbid  = user["fbid"].ToString(),
									name  = user["name"].ToString(),
									score = int.Parse(user["score"].ToString()),
									team  = (user.ContainsKey("team")) ? user["team"].ToString() : "Super Equipo"
								});
							}
				callback(parsePlayers);
		});
	}

	IEnumerator GetUser(List<string> usersId, Action<List<FacebookFriend>> callback){
		Dictionary<string,string> hparse = new Dictionary<string, string>();
		hparse.Add("X-Parse-Application-Id",parse_id);
		hparse.Add("X-Parse-REST-API-Key",parse_key);
	
		List<string> idList = new List<string>();
		usersId.ForEach( s=>{
			idList.Add("\"" + s + "\"");
		});
		string ids = string.Join(",", idList.ToArray());
		string url = "https://api.parse.com/1/classes/Player?where={\"fbid\":{\"$in\":["+ids+"]}}";
		Debug.Log(url);
		WWW www = new WWW(url,null,hparse);
		yield return www;
		Debug.Log(www.text);
		var response = Json.Deserialize(www.text) as Dictionary<string,object>;
		var result = response["results"] as List<object>;
		List<FacebookFriend> fbFriendList = new List<FacebookFriend>();
		foreach(Dictionary<string,object> user in result){
			fbFriendList.Add( new FacebookFriend{
				id   = user["fbid"].ToString(),
				name = user["first_name"].ToString() + " " + user["last_name"].ToString()
			});
		}
		callback(fbFriendList);
	}

		
	public void GetRewards ( Dictionary<string,object> predicates, 
	                         Action<IEnumerable<ParseObject>> callback ) {
		
		var query = ParseObject.GetQuery( REWARD );
		foreach( KeyValuePair<string,object> kvp in predicates ) {
			query.WhereEqualTo( kvp.Key, kvp.Value );
			Debug.Log( kvp.Key + " " +kvp.Value.ToString() );
		}

		query.FindAsync().ContinueWith( t => {
			callback( t.IsFaulted ? null : t.Result );
		});
	}

	public void DeleteAll ( string parseObject ) {
		var query = ParseObject.GetQuery( parseObject );
		query.FindAsync().ContinueWith( t => {
			t.Result.ToList().ForEach( o => o.DeleteAsync() );
		});
	}
}


public class ParsePlayer{
	public string fbid ;
	public string name ;
	public string team ;
	public int	  score;
}
