using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class TeamSelectionWindow : Window<TeamSelectionWindow> {
	public Color selectedColor;
	public Color normalColor;
	[SerializeField]
	private List<CountryItem> countryList;
	[SerializeField]
	private bool scale = false;

	[SerializeField]
	private UISprite firpoLogo;
	[SerializeField]
	private CountryItem pasaquina;
	[SerializeField]
	private UILabel pasaquinaName;

	public static void Show(){
		if(!isRunning){
			_Show("TeamSelectionWindow");
			Instance.Initialize();
		}
	}

	protected override void Initialize ()
	{	
		Debug.Log("Initialize");
		CheckFirpo ();
		countryList = FindObjectsOfType<CountryItem>().ToList();
//		if(!scale){
//			countryList.ForEach( ci=>{
//				ci.DestroyScale();
//			});
//		}
		CountryItem juventud = countryList.First( item=> item.abreviation == "juv");
		juventud.SelectItem();

		
	}

	private void CheckFirpo () {
		int playerScore = int.Parse( Game.Instance.localPlayer["score"].ToString());

		if (playerScore >= Game.Instance.firpoUnlockScore) {
			pasaquina.flag.spriteName = this.firpoLogo.spriteName;
			pasaquina.abreviation = "fir";
			pasaquinaName.text = "l.a. firpo";
		}
	}

	public void UnselectAllCountries(){
		countryList.Where( c=> c.enabled == true).ToList().ForEach(c=> c.UnselectItem());
	}

	public void Back(){
		if(Game.Instance.isMultiplayer){
			MessageWindow.OnOkPress += ()=>{
				//MultiplayerManager.Instance.SendQuitMessage(Game.Instance.remotePlayerPref.fbId);
				Game.Instance.status = Game.Status.loby;
				Close();
			};
			MessageWindow.Show("Conexion","Estas seguro de salir?, perderas tu partida",MessageWindow.Option.OkCancel);
		}else{
			Game.Instance.status = Game.Status.loby;
			Close();
		}

	}
	public void Next(){
		if(!Game.Instance.isMultiplayer){
			CountryItem randomCountry = countryList.Where (team=>!team.selected).ElementAt( UnityEngine.Random.Range(0,countryList.Count-2));
			Game.Instance.aiRemotePlayerPref.teamName = randomCountry.abreviation;
			Game.Instance.aiRemotePlayerPref.flagImageName = randomCountry.flag.spriteName;
		}
		FormationWindow.Show();
	}
}
