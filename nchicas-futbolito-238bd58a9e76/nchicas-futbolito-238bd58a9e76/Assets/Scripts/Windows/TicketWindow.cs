using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using Parse;

public class TicketWindow : Window<TicketWindow> {

	[SerializeField]
	private GameObject ticketItem;

	[SerializeField]
	private UIScrollView scrollView;
	[SerializeField] 
	private Color oddColor;
	[SerializeField] 
	private Color evenColor;
	[SerializeField]
	private int space;

	private IEnumerable<ParseObject> tickets;

	public static void Show(IEnumerable<ParseObject> tickets){
		if(!isRunning){
			_Show("TicketWindow");
			Instance.tickets = tickets;
			Instance.Initialize();
		}
	}

	protected override void Initialize ()
	{
		this.CreateItems();

		string message = "Los premios podrán ser canjeados después de 3 días hábiles en nuestros restaurantes con un código que será enviado al correo que nos proporcionaste";
		MessageWindow2.Show("Premios", message);
	}

	private void CreateItems(){
	
		int listCount = this.tickets.Count();

		for(int i = 0; i < listCount; i++){
			ParseObject ticket = this.tickets.ElementAt(i);
			GameObject go = Instantiate(this.ticketItem) as GameObject;
			Transform t = go.transform;
			t.parent = scrollView.transform;
			t.localScale = Vector3.one;
			t.localPosition = new Vector3(0,this.space*i,0);

			var rewardTicketItem = go.GetComponent<RewardTicketItem>();
			rewardTicketItem.Initialize(ticket);
			rewardTicketItem.ChangeColor( i % 2 == 0 ? oddColor : evenColor );
		}
	
		scrollView.UpdatePosition();
		scrollView.UpdateScrollbars();
	}

	public void CloseWindow(){
		Close();
	}
}