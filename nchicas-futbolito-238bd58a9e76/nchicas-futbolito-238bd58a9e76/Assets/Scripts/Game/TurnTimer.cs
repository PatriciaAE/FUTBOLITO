using UnityEngine;
using System.Collections;
using System.Timers;
using System;

public class TurnTimer : Timer {

	[SerializeField]
	private UILabel label;
	[SerializeField]
	private UISprite progressBar;

	protected override void UpdateUI ()
	{
		//if(Game.Instance.state != Game.State.playing) this.Pause();
		//	label.text = this.actualTime.ToString();
		progressBar.fillAmount =  this.actualTime/this.time;
		//Get Time in seconds
		TimeSpan t = TimeSpan.FromSeconds(this.actualTime);
		label.text = string.Format("{1:D2}",t.Minutes,t.Seconds);
	}
}
