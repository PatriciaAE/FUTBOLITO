using UnityEngine;
using System.Collections;

public class InstructionWindow : Window<InstructionWindow> {
	//Used for save when has to register the changes
	private bool check = false;

	public static void Show(bool check = true){
	
		if(ConfigurationManager.Instance.GetInstructionsSeen() && check) return;
		check = check;

		if(!isRunning){
			_Show("InstructionWindow");
			if(HUD.Instance != null){
				HUD hud = HUD.Instance;
				hud.gameTimer.Pause();
				hud.turnTimer.Pause();
			}

			
		}
	}


	public void OnClosePress(){
		if(HUD.Instance != null){
			HUD hud = HUD.Instance;
			hud.gameTimer.Play();
			hud.turnTimer.Play();
		}
		if(this.check) ConfigurationManager.Instance.SetInstructionToSeen();
		Close();
	}
}
