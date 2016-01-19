using UnityEngine;
using System.Collections;
using System.Linq;

public class CountryItem : MonoBehaviour {
	[SerializeField]
	private UISprite backSprite;

	public bool selected = false;

	public string abreviation;

	public UISprite flag;

	public void SelectItem(){
		TeamSelectionWindow.Instance.UnselectAllCountries();
		backSprite.color = TeamSelectionWindow.Instance.selectedColor;
		backSprite.gameObject.GetComponentsInParent<UIButton> ().ToList ().ForEach ((button) => {
			button.enabled = false;
			if (button.tweenTarget.GetComponent<UISprite>())
				button.tweenTarget.GetComponent<UISprite>().color = button.pressed;
			if (button.tweenTarget.GetComponent<UILabel>())
				button.tweenTarget.GetComponent<UILabel>().color = button.pressed;
		});
		Game.Instance.localPlayerPref.teamName = abreviation;
		Game.Instance.localPlayerPref.flagImageName = flag.spriteName;
		selected = true;
	}

	public void UnselectItem(){
		//backSprite.color = TeamSelectionWindow.Instance.normalColor;
		backSprite.gameObject.GetComponentsInParent<UIButton> ().ToList ().ForEach ((button) => {
			if (button.tweenTarget.GetComponent<UISprite>())
				button.tweenTarget.GetComponent<UISprite>().color = button.defaultColor;
			if (button.tweenTarget.GetComponent<UILabel>())
				button.tweenTarget.GetComponent<UILabel>().color = button.defaultColor;
			button.enabled = true;
		});
		//backSprite.gameObject.GetComponentInParent<UIButton> ().defaultColor = TeamSelectionWindow.Instance.normalColor;
		selected = false;
	}

	public void DestroyScale(){
		Destroy(backSprite.GetComponent<UIButtonScale>());
	}
}
