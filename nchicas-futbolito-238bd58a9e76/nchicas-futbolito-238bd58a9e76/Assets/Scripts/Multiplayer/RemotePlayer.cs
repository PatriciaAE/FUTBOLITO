using UnityEngine;
using System.Collections;

public class RemotePlayer : Player {

	private bool ready = false;
	public override void TurnEnd ()
	{
		ready = true;
		Game.Instance.NextTurn();
	}
	
	public override void TurnStart ()
	{
		StartCoroutine("WaitForShoot");
	}

	IEnumerator WaitForShoot(){
		//MultiplayerManager.Instance.AddLog("Waiting for shoot");
		bool ready = false;
		while(!ready){
			/*if(MultiplayerManager.Instance.ShootRecibed){
				CourtField.Instance.Ball.RemoteShoot( MultiplayerManager.Instance.GetShoot(),MultiplayerManager.Instance.GetPosition());
				ready = true;
			}*/
			yield return null;
		}
	}
}
