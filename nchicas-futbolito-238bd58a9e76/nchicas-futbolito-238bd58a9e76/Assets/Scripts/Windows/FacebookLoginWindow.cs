using UnityEngine;
using System.Collections;

public class FacebookLoginWindow : Window<FacebookLoginWindow> {

	public static void Show(){
		if(!isRunning){
			_Show("FacebookLoginWindow");
		}
	}

	public void ConnectWithFacebook(){

	}
}
