using UnityEngine;
using System.Collections;
using System;

public class MessageWindow2 : Window<MessageWindow2> {

	public enum Option{
		Ok,
		OkCancel
	}
	public static Action OnOkPress;
	public static Action OnCancelPress;

	[SerializeField]
	private UILabel title;
	[SerializeField]
	private UILabel message;
	[SerializeField]
	private GameObject okOnly;
	[SerializeField]
	private GameObject okCancel;
	public static void Show(string title,string message,Option option = Option.Ok){
		if(!isRunning){
			_Show("MessageWindow2");
			Instance.title.text = title;
			Instance.message.text = message;
			if(option == Option.Ok){
				Instance.okCancel.SetActive(false);
			}else if(option == Option.OkCancel){
				Instance.okOnly.SetActive(false);
			}
		}
	}

	public void OkPress(){
		if(OnOkPress != null) OnOkPress();
		Close();
	}

	public void CancelPress(){
		if(OnCancelPress !=null) OnCancelPress();
		Close();
	}

	protected override void BeforeClose ()
	{
		OnOkPress = null;
		OnCancelPress = null;
	}
	

}
