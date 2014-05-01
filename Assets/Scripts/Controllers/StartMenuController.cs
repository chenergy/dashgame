using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
	private enum MenuScene { START_GAME, PURCHASE, OPTIONS, FRIENDS };

	public float 		seconds = 1.0f;
	public Camera 		startMenuCamera;
	public Camera 		startUICamera;
	
	public Transform	createSceneTransform;
	public Transform	currentSceneTransform;
	public Transform	removeSceneTransform;

	public GameObject 	startGamePrefab;
	public GameObject 	purchasePrefab;
	public GameObject 	optionsPrefab;
	public GameObject 	friendsPrefab;

	public GameObject	startGameUI;
	public GameObject	purchaseUI;
	public GameObject	optionsUI;
	public GameObject	friendsUI;

	public AudioClip 	startMenuLoop;

	private bool 		isMoving = false;
	private GameObject 	create;
	private GameObject 	current;
	private MenuScene	currentScene = MenuScene.START_GAME;
	private GameObject[] uiObjects;

	// Use this for initialization
	void Start ()
	{
		this.current = GameObject.Instantiate (this.startGamePrefab, this.currentSceneTransform.position, Quaternion.identity) as GameObject;

		this.uiObjects = new GameObject[]{
			startGameUI, purchaseUI, optionsUI, friendsUI
		};

		AudioController.PlayAudioLoop(this.startMenuLoop);
	}

	public void StartGame(){
		Application.LoadLevel ("test-mapscene");
		//GameController.StartGame ();
	}

	public void GoToStartGame(){
		AudioController.PlayInterfaceSound ();
		this.CreateNewScene (MenuScene.START_GAME, this.startGamePrefab);
	}

	public void GoToPurchase(){
		AudioController.PlayInterfaceSound ();
		this.CreateNewScene (MenuScene.PURCHASE, this.purchasePrefab);
	}

	public void GoToOptions(){
		AudioController.PlayInterfaceSound ();
		this.CreateNewScene (MenuScene.OPTIONS, this.optionsPrefab);
	}

	public void GoToFriends(){
		AudioController.PlayInterfaceSound ();
		this.CreateNewScene (MenuScene.FRIENDS, this.friendsPrefab);
	}

	private void CreateNewScene(MenuScene type, GameObject prefab){
		if (this.currentScene != type && !this.isMoving) {
			// disable all previous objects
			foreach (GameObject gobj in this.uiObjects){
				gobj.SetActive(false);
			}

			// Enable correct object
			GameObject newUIObjects = this.uiObjects[(int)type];
			newUIObjects.SetActive(true);

			foreach (UITweener tween in newUIObjects.GetComponentsInChildren<UITweener>()){
				tween.ResetToBeginning();
				tween.PlayForward();
			}

			this.create = GameObject.Instantiate (prefab, this.createSceneTransform.position, Quaternion.identity) as GameObject;
			this.currentScene = type;
			StartCoroutine ("ShiftScenes");
		}
	}

	IEnumerator ShiftScenes(){
		float t = 0.0f;
		this.isMoving = true;

		while ((this.create.transform.position - this.currentSceneTransform.position).sqrMagnitude > 0.01f) {
			this.create.transform.position = Vector3.Lerp (this.createSceneTransform.position, this.currentSceneTransform.position, Mathf.SmoothStep (0.0f, 1.0f, t));
			this.current.transform.position = Vector3.Lerp (this.currentSceneTransform.position, this.removeSceneTransform.position, Mathf.SmoothStep (0.0f, 1.0f, t));

			yield return new WaitForEndOfFrame ();
			t += Time.deltaTime / this.seconds;
		}

		GameObject.Destroy (this.current);
		this.current = this.create;
		this.create = null;
		this.isMoving = false;
	}
}

