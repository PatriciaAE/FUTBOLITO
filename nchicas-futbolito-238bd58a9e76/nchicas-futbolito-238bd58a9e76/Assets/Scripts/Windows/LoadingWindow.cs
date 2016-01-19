using UnityEngine;
using System.Collections;
using System;

public class LoadingWindow : Window<LoadingWindow> {

	[SerializeField]
	private UISprite laoderLogo;
	[SerializeField]
	private UILabel labelMessage;

	public static void Show(float time = 0 ,string message = "cargando",Action callback = null){
		Debug.Log("Before Show");
		if(!isRunning){
			Debug.Log("Show");
			_Show("LoadingWindow");
			if( time > 0 ) {
				Instance.StartCoroutine(Instance.WaitForAction(time,callback));
			}
			else {
				Instance.CallBack = callback;
			}
			Instance.labelMessage.text = message.ToLower ();
			Instance.Initialize();
		}
	}

	IEnumerator WaitForAction(float time , Action action){
		yield return new WaitForSeconds( time);
		if(action != null) action();
	}

	protected override void BeforeClose ()
	{
		Debug.Log ("Loading Window Close");
	}
}
