using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using  Parse;

public class RegisterWindow : Window<RegisterWindow> {

	[SerializeField] private UILabel teamLabel;
	[SerializeField] private UILabel nameLabel;
	[SerializeField] private UILabel emailLabel;
	[SerializeField] private UILabel locationLabel;
	[SerializeField] private UILabel errorLabel;

	[SerializeField] GameObject comboContainer;

	public static void Show(){
		if(!isRunning){
			_Show("RegisterWindow");
			Instance.Initialize();
		}
	}
	
	protected override void Initialize () {
		if( Game.Instance.localPlayer.ContainsKey("team") ) {
			teamLabel.text = (string)Game.Instance.localPlayer["team"];
		}
		else {
			teamLabel.text = "";
		}

		if( Game.Instance.localPlayer.ContainsKey("name") ) {
			nameLabel.text = (string)Game.Instance.localPlayer["name"];
		}
		else {
			nameLabel.text = "";
		}

		if( Game.Instance.localPlayer.ContainsKey("email") ) {
			emailLabel.text = (string)Game.Instance.localPlayer["email"];
		}
		else {
			emailLabel.text = "";
		}

		if(Game.Instance.localPlayer.ContainsKey("country")){
			if((Game.Instance.localPlayer["country"].ToString() != "SV")){
				comboContainer.SetActive(false);
			}
		}
		
	}
	
	public void Back(){
		Close();
	}

	public void Save(){
		if( string.IsNullOrEmpty( teamLabel.text ) ||
		    string.IsNullOrEmpty( nameLabel.text ) ||
		    string.IsNullOrEmpty( emailLabel.text ) ||
		    string.IsNullOrEmpty( locationLabel.text )) {
			errorLabel.text = "Por favor verifique sus datos para continuar";
			return;
		}

		Game.Instance.localPlayer["team"] = teamLabel.text;
		Game.Instance.localPlayer["registeredName"] = nameLabel.text;
		Game.Instance.localPlayer["registeredEmail"] = emailLabel.text;
		Game.Instance.localPlayer["registeredLocation"] = locationLabel.text;
		Game.Instance.localPlayer.SaveAsync();
		Close();
	}
	
}
