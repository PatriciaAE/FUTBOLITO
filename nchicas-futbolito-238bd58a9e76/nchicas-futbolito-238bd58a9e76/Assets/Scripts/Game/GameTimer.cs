using UnityEngine;
using System;
using System.Collections;
using System.Timers;
public class GameTimer : Timer {
	[SerializeField]
	private UILabel label;
	[SerializeField]
	private UISprite progressBar;
	protected override void UpdateUI(){
	//	label.text = this.actualTime.ToString();
		progressBar.fillAmount =  this.actualTime/this.time;
	//Get Time in seconds
		TimeSpan t = TimeSpan.FromSeconds(this.actualTime);
		label.text = string.Format("{0:D2}:{1:D2}",t.Minutes,t.Seconds);
	}
}
