using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	public Transform 	itemTransform;
	public UISprite		itemBG;
	public UILabel		itemNameLabel;
	public UILabel		itemMassLabel;
	public UILabel		totalMassLabel;
	public UIButton		quitLabel;
	public GameObject	countDown;

	public float		itemDisplayLifetime 	= 1.0f;
	public float		itemDisplayAppearTime 	= 0.25f;

	private GameObject	currItemMesh;

	private static UIController instance = null;

	void Awake(){
		if (UIController.instance == null) {
			UIController.instance = this;

			instance.itemBG.gameObject.SetActive(false);
			instance.itemNameLabel.text = "";
			instance.itemMassLabel.text = "";
			instance.totalMassLabel.text = "0.000 kg";

			instance.quitLabel.gameObject.SetActive(false);
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}


	public static void UpdateMass(int num){
		instance.totalMassLabel.text = (num / 1000.0f).ToString() + " kg";
	}

	// Updates the visual item
	public static void UpdateItem(Collectable item){
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

	public static void Resume(){
		instance.StartCoroutine ("PlayCountDown");
	}

	IEnumerator PlayCountDown(){
		float timer = 0.0f;

		foreach (UITweener tween in instance.countDown.GetComponentsInChildren<UITweener>()) {
			tween.ResetToBeginning();
			tween.PlayForward();
		}
		
		while (timer < 3.0f) {
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
		}

		Time.timeScale = 1.0f;
	}

	// Visual feedback of collecting an item
	IEnumerator CreateItem (Collectable item){
		float timer = 0.0f;

		// Create Item Mesh
		instance.currItemMesh = GameObject.Instantiate (item.visualPrefab, instance.itemTransform.position, instance.itemTransform.rotation) as GameObject;
		instance.currItemMesh.transform.localScale = Vector3.zero;
		instance.itemNameLabel.text = item.name;
		instance.itemMassLabel.text = (item.mass / 1000.0f).ToString () + " kg";

		// Scale ItemBG Forward
		instance.itemBG.gameObject.SetActive (true);
		instance.itemBG.transform.localScale = Vector3.zero;
		instance.itemBG.GetComponent<TweenScale> ().PlayForward();

		// Lerp toward the target scale
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

		// Lerp back to zero scale
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
		instance.itemMassLabel.text = "";
		GameObject.Destroy (instance.currItemMesh);
	}

	public void GoToStartMenu(){
		Time.timeScale = 1.0f;
		Application.LoadLevel ("start-menu");
	}

	public void Pause(){
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;

		if (Time.timeScale == 1.0f) {
			instance.quitLabel.gameObject.SetActive (false);
			UIController.Resume();
		} else {
			instance.quitLabel.gameObject.SetActive (true);
		}
	}
}