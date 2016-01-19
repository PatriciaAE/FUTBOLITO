using UnityEngine;
using System.Collections;

public static class GameObjectExtensions {

	public static void SetLayerRecursively(this GameObject gameObject, int layer) {
		gameObject.layer = layer;
		foreach(Transform child in gameObject.transform) {
			child.gameObject.SetLayerRecursively(layer);
		}
	}
}
