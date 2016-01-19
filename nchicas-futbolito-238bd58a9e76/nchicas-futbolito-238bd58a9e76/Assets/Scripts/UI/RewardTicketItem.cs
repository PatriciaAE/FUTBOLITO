using UnityEngine;
using System.Collections;
using Parse;

public class RewardTicketItem : MonoBehaviour {


	public ParseObject ticket;
	[SerializeField]
	private UILabel rewardName;
	[SerializeField]
	private UILabel rewardCode;
	[SerializeField]	
	private UISprite back;

	public void Initialize(ParseObject ticket){
		this.ticket = ticket;
		rewardName.text = (string) ticket["rewardName"];
		rewardCode.text = (string) ticket["code"];
	}
	
	public void ChangeColor( Color color){
		this.back.color = color;
	}

	public void Selected () {
		string sucursal = ticket["branchOffice"].ToString();
		if( sucursal == "todos" || sucursal == "Todos" ) {
			MessageWindow.Show( "Premio", "Con este código puedes canjear tu premio en el  Pollo Campero más cercano. Revisa tu correo para más información.");
		}
		else {
			MessageWindow.Show( "Premio", "Con este código puedes canjear tu premio en Pollo Campero " + sucursal + " después de 3 días hábiles. Revisa tu correo para más información." );
		}
	}
}
