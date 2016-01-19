using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FieldTeam : MonoBehaviour {

	[SerializeField]
	List<GameObject> Formation323  = new List<GameObject>();

	[SerializeField]
	List<GameObject> Formation332  = new List<GameObject>();
	
	[SerializeField]
	List<GameObject> Formation431  = new List<GameObject>();

	[SerializeField]
	List<GameObject> Formation422 = new List<GameObject>();

	[SerializeField]
	GameObject playerPrefab;

	[SerializeField]
	private float sizeMultiplier = 1;

	public enum Formation{
		F422,
		F431,
		F323,
		F332
	}

	void Awake(){
	//Hide the objects
		this.HideAll();
	}

	private void HideAll(){
		HideObjects(Formation323);
		HideObjects(Formation332);
		HideObjects(Formation422);
		HideObjects(Formation431);
	}

	public void ShowFormation( Formation formation){
		switch(formation){
		case Formation.F323:
			ShowFormation(this.Formation323);
			break;
		case Formation.F332:
			ShowFormation(this.Formation332);
			break;
		case Formation.F422:
			ShowFormation(this.Formation422);
			break;
		case Formation.F431:
			ShowFormation(this.Formation431);
			break;
		}
	}

	private void HideObjects(List<GameObject> objects){
		objects.ForEach( o=>{
			o.SetActive(false);
		});
	}

	private void ShowFormation(List<GameObject> objects){
		this.HideAll();
		objects.ForEach( o=>{
			o.SetActive(true);
			o.GetComponent<FieldToken>().CreatePlayerToken(playerPrefab,this.sizeMultiplier);		
		});
	}
}
