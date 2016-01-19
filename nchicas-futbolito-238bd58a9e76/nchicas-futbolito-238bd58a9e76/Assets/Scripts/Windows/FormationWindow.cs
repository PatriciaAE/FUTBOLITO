using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class FormationWindow : Window<FormationWindow> {
	[SerializeField]
	private Color selectedColor;
	[SerializeField]
	private Color idleColor;
	[SerializeField]
	private List<UISprite> backbuton; 
	[SerializeField]
	private List<UILabel> labels;

	private FieldTeam.Formation selectedFormation;
	[SerializeField]
	private FieldTeam previewTeam;


	public static void Show(){
		if(!isRunning){
			_Show("FormationWindow");
			Instance.Initialize();
		}
	}

	protected override void Initialize ()
	{
		if(Game.Instance != null){
		switch(Game.Instance.localPlayerPref.formation){
		case FieldTeam.Formation.F323:
				Formation323();
			break;
		case FieldTeam.Formation.F332:
				Formation332();
			break;
		case FieldTeam.Formation.F422:
				Formation422();
			break;
		case FieldTeam.Formation.F431:
				Formation431();
			break;
			}
		}else{
			this.Formation422();
		}
	}

	public void Formation422(){
		this.SetFormation(FieldTeam.Formation.F422,0);
	}

	public void Formation332(){
		this.SetFormation( FieldTeam.Formation.F332,1);
	}

	public void Formation431(){
		this.SetFormation( FieldTeam.Formation.F431,2);
	}

	public void Formation323(){
		this.SetFormation(FieldTeam.Formation.F323,3);
	}

	private void SetFormation( FieldTeam.Formation formation , int index){
		Game.Instance.localPlayerPref.formation = this.selectedFormation = formation;
		this.previewTeam.ShowFormation(formation);
		this.SetSelected(index);
	}
	private void SetSelected( int index){
		SetNormalColor();
		backbuton [index].gameObject.GetComponent<UIButton> ().enabled = false;
		backbuton[index].color = selectedColor;
	}
	
	private void SetNormalColor(){
		backbuton.ForEach( s=>{
			s.color = idleColor;
			s.gameObject.GetComponent<UIButton>().enabled = true;
		});
	}

	public void Back(){
		Close();
	}

	public void Next(){
		if(!Game.Instance.isMultiplayer){
			var values = Enum.GetValues(typeof(FieldTeam.Formation));
			int random = UnityEngine.Random.Range(0,values.Length);
			Game.Instance.aiRemotePlayerPref.formation = (FieldTeam.Formation)values.GetValue(random);
		}else{
			//MultiplayerManager.Instance.SendPlayerInformation(Game.Instance.localPlayerPref,Game.Instance.remotePlayerPref.fbId);
			//MultiplayerManager.Instance.ConnectToRoom();
		}
		Application.LoadLevelAsync("Game");
	}
}
