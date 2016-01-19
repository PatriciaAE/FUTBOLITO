using UnityEngine;
using System.Collections;

public  abstract class Player : MonoBehaviour {

	public Player nextPlayer{get;set;}

	public virtual void TurnStart(){

	}
	public virtual void TurnEnd(){

	}
	public virtual void Shooted(){

	}
}
