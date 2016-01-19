using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parse;

public class RewardsWindow : /*Window<RewardsWindow>*/MonoBehaviour {

	[SerializeField] private GameObject itemReward;
	[SerializeField] private UIScrollView scrollview;
	[SerializeField] private Color oddColor;
	[SerializeField] private Color evenColor;
	[SerializeField] private TextAsset rewardsCSV;
	[SerializeField] private int space;

	[SerializeField] private PriceContainer priceContainer;

	private IEnumerable<ParseObject> rewards;
	private List<RewardContainer> rewardContainers;
	private List<RewardEntity> rewardsList;
	/*public static void Show(){
		if(!isRunning){
			_Show("RewardsWindow");
			Instance.Initialize();
		}
	}

	protected override void Initialize () {
		rewards = Parser.ReadStream<Reward>( 
			new StringReader( rewardsCSV.text ), 
		    true 
		);

		PopulateList();
	}*/

	void Start () {
		LoadingWindow.Show(0, "cargando premios");

		//string country = (string)Game.Instance.localPlayer["country"];

		ParseManager.Instance.GetRewards( new Dictionary<string,object> (),
		( r ) => {
			rewards = r.OrderBy( s => int.Parse( s["goal"].ToString() ) ); //.Where( s => int.Parse( s["remaining"].ToString() ) > 0 )					   
		});

		//this.GetClosestPrice ();
		
	}

	public void PopulateList () {
		GetClosestPrice();
		LoadingWindow.Close();

		rewardsList = new List<RewardEntity>();
		rewardContainers = new List<RewardContainer>();

		for( int i=0; i<rewards.Count(); ++i ) {
			ParseObject reward = rewards.ElementAt(i);
			GameObject go = Instantiate(this.itemReward) as GameObject;
			Transform t = go.transform;
			t.parent = scrollview.transform;
			t.localScale = Vector3.one;
			t.localPosition = new Vector3( 0, space * i, 0 );
			rewardsList.Add( new RewardEntity{
				name   =  (string)reward["name"],
				points = int.Parse( reward["goal"].ToString())
			});
			RewardContainer rewardContainer = go.GetComponent<RewardContainer>();
			rewardContainer.Initialize( reward );
			rewardContainer.ChangeColor( i % 2 == 0 ? oddColor : evenColor );
		
			rewardContainers.Add( rewardContainer );

			switch (reward["goal"].ToString ()) {
			case "1":
				rewardContainer.shirtsImage.gameObject.SetActive (true);
				break;

			case "2":
				rewardContainer.rankNumber.text = "2";
				rewardContainer.rankSuperscript.text = "do";
				rewardContainer.cansImage.gameObject.SetActive (true);
				break;

			case "3":
				rewardContainer.rankNumber.text = "3";
				rewardContainer.rankSuperscript.text = "er";
				rewardContainer.cansImage.gameObject.SetActive (true);
				break;
			}

			if (reward["name"].ToString().ToLower() == "firpo") {
				int playerScore = int.Parse( Game.Instance.localPlayer["score"].ToString());

				if (playerScore < Game.Instance.firpoUnlockScore) {
					rewardContainer.labelScore.gameObject.SetActive (true);

					Vector3 rewardContainerPosition = rewardContainer.labelName.transform.localPosition;
					rewardContainerPosition.x += 10;
					rewardContainer.labelName.transform.localPosition = rewardContainerPosition;

					rewardContainer.rankNumber.gameObject.SetActive (false);
					rewardContainer.rankSuperscript.gameObject.SetActive (false);
					rewardContainer.image.transform.parent.gameObject.SetActive (true);
				} else {
					rewardContainer.gameObject.SetActive (false);
				}
			}
		}
		scrollview.UpdatePosition();
		scrollview.UpdateScrollbars();
	}

	private void GetClosestPrice(){
		int score = int.Parse( Game.Instance.localPlayer["score"].ToString());
//		var reward = this.rewardsList.Where(r=> score < r.points).OrderBy(r=>r.points).FirstOrDefault();
//		if(reward == null) return;
		this.priceContainer.Initilize(score,10,"");
	}
	// Dirty hack beacuse of Parse - main thread problem
	void Update () {
		if( rewardContainers == null && rewards != null ) {
			PopulateList();
		}
	}

	public void Back(){
		Application.LoadLevelAsync("MainMenu");
	}

}
	public class RewardEntity{

		public int points;
		public string name;

		public RewardEntity(){}
	}
