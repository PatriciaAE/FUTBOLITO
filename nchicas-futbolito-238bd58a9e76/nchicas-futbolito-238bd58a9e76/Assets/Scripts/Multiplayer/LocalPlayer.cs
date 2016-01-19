using UnityEngine;
using System.Collections;

public class LocalPlayer : Player {

	public override void TurnStart ()
	{
		HUD hud = HUD.Instance;
		hud.turnTimer.Show();
		//Intialize Timer
		hud.turnTimer.Setup(Game.Instance.TurnTime,()=>{
			//if(Game.Instance.isMultiplayer) MultiplayerManager.Instance.SendPass();
			Game.Instance.NextTurn();
		});
		
		hud.turnTimer.Init();
		
		InputManager.Instance.Unlock();
	}

	public override void Shooted ()
	{
		HUD hud = HUD.Instance;
		hud.turnTimer.Pause();
	}

	public override void TurnEnd ()
	{
		Game.Instance.NextTurn();
		HUD hud = HUD.Instance;
		hud.turnTimer.Pause();
		hud.turnTimer.Hide();
	}
}
