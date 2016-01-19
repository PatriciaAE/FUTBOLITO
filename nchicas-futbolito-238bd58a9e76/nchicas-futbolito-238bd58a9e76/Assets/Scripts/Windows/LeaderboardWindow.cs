using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;

public class LeaderboardWindow : MonoBehaviour {

	[SerializeField] private GameObject itemScore;
	[SerializeField] private UIScrollView scrollview;
	[SerializeField] private Color oddColor;
	[SerializeField] private Color evenColor;
	[SerializeField] private int space;

	private List<ParsePlayer> scores;
	private List<ScoreContainer> scoreContainers;
	[SerializeField]
	private ScoreContainer ownScore;

	void Start () {
		LoadingWindow.Show(0, "cargando goleadores");

		//string country = (string)Game.Instance.localPlayer["country"];

		ParseManager.Instance.GetTop100Players( (fbScore)=>{
			scores = fbScore.OrderByDescending(f=> f.score).ToList();
		});

		ShowOwnScore();
		

//		if( country == "SV" ) {		
//			string message = "Los primeros 150 goleadores con mayor puntaje ganarán un pase doble para la Fiesta Campero y disfrutar de la final del Mundial de Fútbol en pantalla gigante en CIFCO con todo incluido, Banquete de Alitas/Camperitos y bebida gratis, shows de malabarismo de fútbol, y muchas sorpresas más.";
//			MessageWindow2.Show("Top 150", message);
//		}
	}
	
	public void PopulateList () {
		LoadingWindow.Close();
		scoreContainers = new List<ScoreContainer>();
		int i = 0;
		scores.ForEach( pp=>{
			GameObject go = Instantiate(this.itemScore) as GameObject;
			Transform t = go.transform;
			t.parent = scrollview.transform;
			t.localScale = Vector3.one;
			t.localPosition = new Vector3( 35, space * i++, 0);

			//string name = (pp.name.Length > 16) ? pp.name.Substring(0,13) + "..." : pp.name;

			ScoreContainer scoreContainer = go.GetComponent<ScoreContainer>();
			scoreContainer.Initialize( pp.fbid, pp.name.ToLower (), pp.score,i);
			scoreContainer.ChangeColor( i % 2 == 0 ? oddColor : evenColor );
			
			scoreContainers.Add( scoreContainer );

			});
		scrollview.UpdatePosition();
		scrollview.UpdateScrollbars();
	}

	public void PublishFacebook(){
		if(scoreContainers == null) return;
		FacebookManager.Instance.BragScore(int.Parse(Game.Instance.localPlayer["score"].ToString()));
	}

	private void ShowOwnScore(){
		this.ownScore.Initialize(AccessToken.CurrentAccessToken.UserId, Game.Instance.localPlayer["score"].ToString(),0);
		this.ownScore.GetScoreLabel().text = Game.Instance.localPlayer["score"].ToString();
	}
	public void Back(){
		Application.LoadLevelAsync("MainMenu");
	}

	// Dirty hack beacuse of Parse - main thread problem
	void Update () {
		if( scoreContainers == null && scores != null ) {
			PopulateList();
		}
	}

}
