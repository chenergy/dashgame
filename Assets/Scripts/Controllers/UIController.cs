using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	public Transform 	itemTransform;
	public UISprite		itemBG;
	public UILabel		itemNameLabel;
	public UILabel		coinNumLabel;
	public float		itemDisplayLifetime 	= 1.0f;
	public float		itemDisplayAppearTime 	= 0.25f;

	private GameObject	currItemMesh;

	private static UIController instance = null;

	void Awake(){
		if (UIController.instance == null) {
			UIController.instance = this;

			instance.itemBG.gameObject.SetActive(false);
			instance.itemNameLabel.text = "";
			instance.coinNumLabel.text = "0";
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	public static void UpdateCoins(int num){
		instance.coinNumLabel.text = num.ToString();
	}

	// Updates the visual item
	public static void UpdateItem(A_CollectableItem item){
		// Create a new item based on the model if item does not already exist
		if (instance.itemNameLabel.text != item.name) {
			if (instance.currItemMesh != null) {
				instance.StopCoroutine ("CreateItem");
				instance.itemBG.gameObject.transform.localScale = Vector3.zero;
				instance.itemBG.gameObject.SetActive (false);
				instance.itemNameLabel.text = "";
				GameObject.Destroy (instance.currItemMesh);
			}

			instance.StartCoroutine ("CreateItem", item);
		}
	}

	// Visual feedback of collecting an item
	IEnumerator CreateItem (A_CollectableItem item){
		float timer = 0.0f;

		// Create Item Mesh
		instance.currItemMesh = GameObject.Instantiate (item.visualPrefab, instance.itemTransform.position, instance.itemTransform.rotation) as GameObject;
		instance.currItemMesh.transform.localScale = Vector3.zero;
		instance.itemNameLabel.text = item.name;

		// Scale ItemBG Forward
		instance.itemBG.gameObject.SetActive (true);
		instance.itemBG.transform.localScale = Vector3.zero;
		instance.itemBG.GetComponent<TweenScale> ().PlayForward();

		Vector3 targetLocalScale = item.visualPrefab.transform.localScale;
		while (timer < instance.itemDisplayAppearTime) {
			instance.currItemMesh.transform.localScale = Vector3.Lerp (instance.currItemMesh.transform.localScale, 
			                                                           targetLocalScale, 
			                                                           (timer / instance.itemDisplayAppearTime));
			yield return new WaitForEndOfFrame ();
			timer += Time.deltaTime;
		}
		instance.currItemMesh.transform.localScale = targetLocalScale;

		// Wait For ItemBG display liftime
		timer = 0.0f;
		while (timer < instance.itemDisplayLifetime) {
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
		}

		// Scale ItemBG Backward
		timer = 0.0f;
		instance.itemBG.GetComponent<TweenScale> ().PlayReverse ();

		while (timer < instance.itemBG.GetComponent<TweenScale> ().duration) {
			instance.currItemMesh.transform.localScale = Vector3.Lerp (instance.currItemMesh.transform.localScale, 
			                                                           Vector3.zero, 
			                                                           (timer / instance.itemBG.GetComponent<TweenScale> ().duration));
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
		}

		instance.itemBG.transform.localScale = Vector3.zero;
		instance.itemBG.gameObject.SetActive (false);
		instance.itemNameLabel.text = "";
		GameObject.Destroy (instance.currItemMesh);
	}
}