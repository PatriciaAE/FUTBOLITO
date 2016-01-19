using UnityEngine;
using System.Collections;

public class PriceContainer : MonoBehaviour {
	[SerializeField]
	private UILabel labelScore;
	[SerializeField]
	private UILabel LabelleftScore;
	[SerializeField]
	private UILabel labelPrice;

	public void Initilize ( int score, int leftScore, string price){
		labelScore.text = string.Format("{0}", score.ToString());
		LabelleftScore.text = string.Format("te faltan {0} puntos\npara ganar",leftScore);
		labelPrice.text = price;
	}

}
